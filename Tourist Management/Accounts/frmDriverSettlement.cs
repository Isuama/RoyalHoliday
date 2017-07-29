using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
namespace Tourist_Management.Accounts
{
    public partial class frmDriverSettlement : Form
    {
        private const string msghd = "Driver Settlement";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE        
        string[] DriverNames=new string[10];
        string[] DriverIds = new string[10]; 
        byte[] imageData = null;  
        int DrvSrNo=0;
        string grid = "";
        enum TA { gIDN, gDID, gDNM, gEID, gENM, gAMT, gRAM, gBAL, gPID, gPDT, gPBY, gIST, gSDT, gSBY, gNPD };
        enum TR { gTR, gTN, gDT, gTM, gFI, gFR, gTI, gTO, gVI, gVN, gDI, gDN, gDS, gCH, gPID, gNPD };
        enum TP { gIDN, gIDR, gDID, gDNM, gEID, gENM, gAMT, gPID, gNPD };
        public frmDriverSettlement(){InitializeComponent();}
        private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID FROM trn_GroupAmendment";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.Trim() == "")
                return;
            SystemCode=Convert.ToDouble(txtTourNo.Text.ToString().Trim());
            Fill_Control();
            if (cmbDriver.Items.Count == 0)
                return;
            Get_Details();
            Generate_Total();
            Load_Driver_Photo();
        }
        private void Load_Driver_Photo()
        {
                DataTable DT;
                string ssql;
                ssql = " SELECT EmpPhoto FROM vwDriverVsEmployee " +
                        "Where ID=" + cmbDriver.SelectedValue.ToString().Trim() + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows[0]["EmpPhoto"] != DBNull.Value)
                {
                    byte[] Photo = (byte[])DT.Rows[0]["EmpPhoto"];
                    imageData = Photo;
                    MemoryStream ms = new MemoryStream(Photo);
                    pbCompLogo.Image = Image.FromStream(ms, false, false);
                }
                else
                    pbCompLogo.Image = global::Tourist_Management.Properties.Resources.noimage;
        }
        private void Clear_Contents()
        {
            lblTotPay.Text = "0.00";
            lblDue.Text = "0.00";
            txtStartMeter.Text = "";
            txtEndMeter.Text = "";
            txtTotKm.Text = "";
            txtRateKm.Text = "";
            txtPaidForKm.Text = "";
            txtBata.Text = "";
            txtNights.Text = "";
            txtPaidForBata.Text = "";
            chkExcursion.Checked = false;
            txtExcursion.Text = "Excursion";
            txtExcurDesc.Text = "";
            txtRemarks.Text = "";
            chkIsPaid.Checked = false;
            dtpPaidDate.Value = Classes.clsGlobal.CurDate();
            grdTAdvance.Rows.Count = 1;
            grdTAdvance.Rows.Count = 500;
            grdTExpense.Rows.Count = 1;
            grdTExpense.Rows.Count = 500;
            grdTR.Rows.Count = 1;
            grdTR.Rows.Count = 500;
        }
        private void Fill_Control()
        {
            try
            {
                if (txtTourNo.Text.ToString().Trim() == "")  return;
                else
                {
                    SystemCode = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
                    cmbDriver.Enabled = true;
                    btnOk.Enabled = true;
                    tcDriverSett.Enabled = true;
                }
                cmbDriver.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DISTINCT DriverID,Name FROM vw_trn_BasicTransport Where TransID=" + txtTourNo.Text.Trim() + " AND ISNULL(IsCancelled,0)<>1"); 
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void frmDriverSettlement_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            Grd_Initializer();
        }
        private void Grd_Initializer()
        {
            try
            {
                db.GridInit(grdTExpense, true, TP.gIDN, 0, "ID", TP.gIDR, 0, "IsDriver", Type.GetType(" System.Boolean"), TP.gDID, 0, "Driver/Guide ID", TP.gDNM, 0, "Name", true, TP.gEID, 0, "Expense ID", TP.gENM, 367, "Expense Name", true, TP.gAMT, 127, "Amount", "##.##", TP.gPID, 67, "Is Paid", Type.GetType(" System.Boolean"), TP.gNPD, 67, "Not Paid", Type.GetType(" System.Boolean"));
                db.GridInit(grdTAdvance, 100, TA.gIDN, 0, "ID", TA.gDID, 0, "Driver ID", TA.gDNM, 0, "Driver Name", true, TA.gEID, 0, "Expense ID", TA.gENM, 193, "Advance Name", TA.gAMT, 107, "Advance Amount", "##.##", TA.gRAM, 70, "Returned", TA.gBAL, 70, "Balance", TA.gPID, 0, "Paid", Type.GetType(" System.Boolean"), TA.gPDT, 0, "Paid Date", Type.GetType(" System.DateTime"), TA.gPBY, 0, "Paid By", TA.gIST, 50, "Settled", Type.GetType(" System.Boolean"), TA.gSDT, 83, "Settled Date", Type.GetType(" System.DateTime"), TA.gSBY, 0, "Settled By", TA.gNPD, 55, "Not Paid", Type.GetType(" System.Boolean"));
                db.GridInit(grdTR, 500, TR.gTR, 0, "Tans Type ID", TR.gTN, 0, "Tans Type", true, TR.gDT, 100, "Date", Type.GetType(" System.DateTime"), TR.gTM, 0, "Time", TR.gFI, 0, "From ID", TR.gFR, 153, "From", true, TR.gTI, 0, "To ID", TR.gTO, 153, "To", true, TR.gVI, 0, "Vehicle ID", TR.gVN, 0, "Vehicle", TR.gDI, 0, "Driver ID", TR.gDN, 0, "Driver", true, TR.gDS, 100, "Distance", TR.gCH, 122, "Cost", "##.#", TR.gPID, 00, "Paid", Type.GetType(" System.Boolean"), TR.gNPD, 00, "Not Paid", Type.GetType(" System.Boolean")); 
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void cmbDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear_Contents();
            Get_Details();
            Load_Driver_Photo();
            Generate_Total();
        }
        private void Get_Details()
        {
            string ssql;
            int RowNumb;
            DataTable DT;
            int DriverID=0;
            if (cmbDriver.SelectedValue.ToString().Trim() != "")
                DriverID = Convert.ToInt32(cmbDriver.SelectedValue.ToString().Trim());
            else
                return;
            #region Fill Tour Advance       
            ssql = "SELECT ID,DriverID,DriverName,ExpenseID,Expense,ISNULL(Amount,0)AS Amount,"+
                   "ISNULL(ReturnAmt,0)AS ReturnAmt," +
                   "IsNull(IsChecked,0)AS IsChecked,"+
                   "IsNull(IsPaid,0)AS IsPaid,PaidDate,PaidBy,"+
                   "IsNull(IsSettled,0)AS IsSettled,SettledDate,SettledBy,"+
                   "IsNull(NotPaid,0)AS NotPaid, IsDeleted" +
                   " FROM vw_trn_Tour_Advance_Driver WHERE TransID=" + SystemCode + " AND IsDriver=1 AND DriverID=" + DriverID + " AND IsDeleted='False' ORDER BY SrNo ";
            DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DT.Rows.Count > 0)
            {
                RowNumb = 0;
                double Amt=0, Rtn=0;
                while (DT.Rows.Count > RowNumb)
                {
                    grdTAdvance[RowNumb + 1, (int)TA.gIDN] = DT.Rows[RowNumb]["ID"].ToString();
                    if (DT.Rows[RowNumb]["DriverID"].ToString() != "")
                        grdTAdvance[RowNumb + 1, (int)TA.gDID] = DT.Rows[RowNumb]["DriverID"].ToString();
                    if (DT.Rows[RowNumb]["DriverName"].ToString() != "")
                        grdTAdvance[RowNumb + 1, (int)TA.gDNM] = DT.Rows[RowNumb]["DriverName"].ToString();
                    if (DT.Rows[RowNumb]["ExpenseID"].ToString() != "")
                        grdTAdvance[RowNumb + 1, (int)TA.gEID] = DT.Rows[RowNumb]["ExpenseID"].ToString();
                    if (DT.Rows[RowNumb]["Expense"].ToString() != "")
                        grdTAdvance[RowNumb + 1, (int)TA.gENM] = DT.Rows[RowNumb]["Expense"].ToString();
                    if (DT.Rows[RowNumb]["Amount"].ToString() != "")
                    {
                        grdTAdvance[RowNumb + 1, (int)TA.gAMT] = DT.Rows[RowNumb]["Amount"].ToString();
                        Amt = Convert.ToDouble(DT.Rows[RowNumb]["Amount"]);
                    }
                    if (DT.Rows[RowNumb]["ReturnAmt"].ToString() != "")
                    {
                        grdTAdvance[RowNumb + 1, (int)TA.gRAM] = DT.Rows[RowNumb]["ReturnAmt"].ToString();
                        Rtn = Convert.ToDouble(DT.Rows[RowNumb]["ReturnAmt"]);
                    }
                    grdTAdvance[RowNumb + 1, (int)TA.gBAL] = (Amt - Rtn).ToString();
                    if (Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]))
                    {
                        grdTAdvance[RowNumb + 1, (int)TA.gPID] = 1;
                        if (DT.Rows[RowNumb]["PaidDate"] + "".Trim() != "")
                            grdTAdvance[RowNumb + 1, (int)TA.gPDT] = Convert.ToDateTime(DT.Rows[RowNumb]["PaidDate"]);
                        if (DT.Rows[RowNumb]["PaidBy"] + "".Trim() != "")
                            grdTAdvance[RowNumb + 1, (int)TA.gPBY] = DT.Rows[RowNumb]["PaidBy"].ToString();
                    }
                    if (Convert.ToBoolean(DT.Rows[RowNumb]["IsSettled"]))
                    {
                        grdTAdvance[RowNumb + 1, (int)TA.gIST] = 1;
                        if (DT.Rows[RowNumb]["SettledDate"] + "".Trim() != "")
                            grdTAdvance[RowNumb + 1, (int)TA.gSDT] = Convert.ToDateTime(DT.Rows[RowNumb]["SettledDate"]);
                        if (DT.Rows[RowNumb]["SettledBy"] + "".Trim() != "")
                            grdTAdvance[RowNumb + 1, (int)TA.gSBY] = DT.Rows[RowNumb]["SettledBy"].ToString();
                    }
                    grdTAdvance[RowNumb + 1, (int)TA.gNPD] = Convert.ToBoolean(DT.Rows[RowNumb]["NotPaid"]);
                    RowNumb++;
                }
            }
            #endregion
            #region Fill Transport Details
            ssql = "SELECT TransTypeID,TransTypeName,[Date],[Time],FromID,CityFrom,ToID,CityTo," +
                    "VehicleNo,DriverID,DriverName,Distance,Cost," +
                    "IsNull(IsChecked,0)AS IsChecked,IsNull(IsPaid,0)AS IsPaid,IsNull(NotPaid,0)AS NotPaid" +
                   " FROM vw_trn_Transport WHERE TransID=" + SystemCode + " AND DriverID=" + DriverID + " ORDER BY SrNo ";
            DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DT.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DT.Rows.Count > RowNumb)
                {
                    if (DT.Rows[RowNumb]["TransTypeID"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gTR] = DT.Rows[RowNumb]["TransTypeID"].ToString();
                    if (DT.Rows[RowNumb]["TransTypeName"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gTN] = DT.Rows[RowNumb]["TransTypeName"].ToString();
                    if (DT.Rows[RowNumb]["Date"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gDT] = DT.Rows[RowNumb]["Date"].ToString();
                    if (DT.Rows[RowNumb]["Time"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gTM] = DT.Rows[RowNumb]["Time"].ToString();
                    if (DT.Rows[RowNumb]["FromID"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gFI] = DT.Rows[RowNumb]["FromID"].ToString();
                    if (DT.Rows[RowNumb]["CityFrom"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gFR] = DT.Rows[RowNumb]["CityFrom"].ToString();
                    if (DT.Rows[RowNumb]["ToID"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gTI] = DT.Rows[RowNumb]["ToID"].ToString();
                    if (DT.Rows[RowNumb]["CityTo"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gTO] = DT.Rows[RowNumb]["CityTo"].ToString();
                    if (DT.Rows[RowNumb]["VehicleNo"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gVN] = DT.Rows[RowNumb]["VehicleNo"].ToString();
                    if (DT.Rows[RowNumb]["DriverID"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gDI] = DT.Rows[RowNumb]["DriverID"].ToString();
                    if (DT.Rows[RowNumb]["DriverName"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gDN] = DT.Rows[RowNumb]["DriverName"].ToString();
                    if (DT.Rows[RowNumb]["Distance"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gDS] = DT.Rows[RowNumb]["Distance"].ToString();
                    if (DT.Rows[RowNumb]["Cost"].ToString() != "")
                        grdTR[RowNumb + 1, (int)TR.gCH] = DT.Rows[RowNumb]["Cost"].ToString();
                    grdTR[RowNumb + 1, (int)TR.gPID] = Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]);
                    grdTR[RowNumb + 1, (int)TR.gNPD] = Convert.ToBoolean(DT.Rows[RowNumb]["NotPaid"]);
                    RowNumb++;
                }
            }
            #endregion
            #region TRAVEL EXPENSES
            ssql = "SELECT ID,ISNULL(IsDriver,0)AS IsDriver,DriverID,ExpenseID,Expense,Amount,"+
                   "ISNULL(IsPaid,0)AS IsPaid,IsNull(NotPaid,0)AS NotPaid, IsDeleted " +
                   "FROM vw_trn_Travel_Expenses WHERE TransID=" + SystemCode + " AND"+
                   " IsDriver=1 AND DriverID=" + DriverID + " AND IsDeleted='False' ORDER BY SrNo";
            DataTable DTTravel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DTTravel.Rows.Count > 0)
            {
                RowNumb = 0;
                int driverid = 0;
                string driverName = "";
                while (DTTravel.Rows.Count > RowNumb)
                {
                    driverid = Convert.ToInt32(DTTravel.Rows[RowNumb]["DriverID"]);
                    grdTExpense[RowNumb + 1, (int)TP.gDID] = driverid;
                    if (Convert.ToBoolean(DTTravel.Rows[RowNumb]["IsDriver"]))
                    {
                        grdTExpense[RowNumb + 1, (int)TP.gIDR] = true;
                        driverName = Classes.clsGlobal.Get_Driver_Name(true, driverid);
                    }
                    else
                    {
                        grdTExpense[RowNumb + 1, (int)TP.gIDR] = false;
                        driverName = Classes.clsGlobal.Get_Driver_Name(false, driverid);
                    }
                    grdTExpense[RowNumb + 1, (int)TP.gDNM] = driverName;
                    grdTExpense[RowNumb + 1, (int)TP.gIDN] = DTTravel.Rows[RowNumb]["ID"].ToString();
                    grdTExpense[RowNumb + 1, (int)TP.gEID] = DTTravel.Rows[RowNumb]["ExpenseID"].ToString();
                    grdTExpense[RowNumb + 1, (int)TP.gENM] = DTTravel.Rows[RowNumb]["Expense"].ToString();
                    grdTExpense[RowNumb + 1, (int)TP.gAMT] = DTTravel.Rows[RowNumb]["Amount"].ToString();
                    grdTExpense[RowNumb + 1, (int)TP.gPID] = Convert.ToBoolean(DTTravel.Rows[RowNumb]["IsPaid"]);
                    grdTExpense[RowNumb + 1, (int)TP.gNPD] = Convert.ToBoolean(DTTravel.Rows[RowNumb]["NotPaid"]);
                    RowNumb++;
                }
            }
            #endregion
            #region Fill Basic Transport Details
            ssql = " SELECT DriverID,Isnull(Excursion,0) as Excursion,ExcurDesc,Isnull(ExcurAmt,0)as ExcurAmt," +
                   "StartMeter,EndMeter,TotalKm,RatePerKm,PaidForKm,Bata,NoOfNights,PaidForBata," +
                   "IsNull(IsChecked,0)AS IsChecked,IsNull(AskConfirm,0)AS AskConfirm,"+
                   "PaidDate,ISNULL(IsPaid,0)AS IsPaid,ISNULL(IsConfirm,0)AS IsConfirm,Remarks,ISNULL(SrNo,0)AS SrNo," +
                   "ArrivalDate,ArrivalTime,ArrivalFlight,DeprtDate,DepartTime,DepartFlight,ISNULL(IsCancelled,0)AS IsCancelled" +
                   ",IsNull(NotPaid,0)AS NotPaid,ISNULL(BankPay,0)BankPay,ChkNo FROM trn_BasicTransport " +
                    "Where TransID=" + SystemCode + " AND DriverID=" + DriverID + " AND ISNULL(IsCancelled,0)<>1";
            DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DT.Rows.Count != 0)
            {
                if (Convert.ToBoolean(DT.Rows[0]["BankPay"]))
                {
                    rdbBank.Checked = true;
                    txtChkNo.Text = DT.Rows[0]["ChkNo"] + "".Trim();
                }
                else
                {
                    if (Convert.ToBoolean(DT.Rows[0]["IsPaid"]))
                    {
                        rdbCash.Checked = true;
                        txtChkNo.Text = "";
                    }
                }
                if (Convert.ToBoolean(DT.Rows[0]["Excursion"]))
                {
                    txtExcursion.Enabled = true;
                    chkExcursion.Checked = true;
                    txtExcurDesc.Text = DT.Rows[0]["ExcurDesc"].ToString();
                    txtExcursion.Text = DT.Rows[0]["ExcurAmt"].ToString();
                }
                else
                {
                    chkExcursion.Checked = false;
                    txtExcursion.Enabled = false;
                    txtExcurDesc.Text = "";
                    txtExcursion.Text = "Excursion";
                }
                if (DT.Rows[0]["Remarks"].ToString() != "")
                    txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                if (DT.Rows[0]["StartMeter"].ToString() != "")
                    txtStartMeter.Text = DT.Rows[0]["StartMeter"].ToString();
                if (DT.Rows[0]["EndMeter"].ToString() != "")
                    txtEndMeter.Text = DT.Rows[0]["EndMeter"].ToString();
                if (DT.Rows[0]["TotalKm"].ToString() != "")
                    txtTotKm.Text = DT.Rows[0]["TotalKm"].ToString();
                if (DT.Rows[0]["RatePerKm"].ToString() != "")
                    txtRateKm.Text = DT.Rows[0]["RatePerKm"].ToString();
                if (DT.Rows[0]["PaidForKm"].ToString() != "")
                    txtPaidForKm.Text = DT.Rows[0]["PaidForKm"].ToString();
                if (DT.Rows[0]["Bata"].ToString() != "")
                    txtBata.Text = DT.Rows[0]["Bata"].ToString();
                if (DT.Rows[0]["PaidForBata"].ToString() != "")
                    txtPaidForBata.Text = DT.Rows[0]["PaidForBata"].ToString();
                if (DT.Rows[0]["NoOfNights"].ToString() != "")
                    txtNights.Text = DT.Rows[0]["NoOfNights"].ToString();
                    chkConfirm.Enabled = true;
                    grpBasics.Enabled = true;
                    grdTR.Enabled = true;
                    grdTAdvance.Enabled = true;
                    grdTExpense.Enabled = true;
                chkIsPaid.Checked = Convert.ToBoolean(DT.Rows[0]["IsPaid"]);
                chkNotPaid.Checked = Convert.ToBoolean(DT.Rows[0]["NotPaid"]);
                chkConfirm.Checked = Convert.ToBoolean(DT.Rows[0]["IsConfirm"]);
                if (DT.Rows[0]["PaidDate"].ToString() != "")
                    dtpPaidDate.Value = Convert.ToDateTime(DT.Rows[0]["PaidDate"]);
                DrvSrNo = Convert.ToInt32(DT.Rows[0]["SrNo"]);
            }
            #endregion
        }
        private void grdTAdvance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdTAdvance.Rows.Remove(grdTAdvance.Row);
                grdTAdvance.Rows[1].AllowEditing = true;
            }
        }
        private void grdTAdvance_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTDriver;
            #region DRIVER DETAILS________________________________
            if (e.Col == grdTAdvance.Cols[(int)TA.gDNM].Index)
            {
                DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,DriverName FROM vw_TR_DriverVSVehicle WHERE IsNull(IsActive,0)=1 ORDER BY DriverName");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTDriver;
                frm.Width = grdTAdvance.Cols[(int)TA.gDNM].Width;
                frm.Height = grdTAdvance.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTAdvance);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    int DriverID = Convert.ToInt16(SelText[0]);
                    DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,DriverCode,DriverName,OwnerName,LicenseNo,Tel1 FROM vw_TR_DriverVSVehicle WHERE ID=" + DriverID + " AND IsNull(IsActive,0)=1 ORDER BY DriverName");
                    if (DTDriver.Rows[0]["ID"].ToString() != "")
                        grdTAdvance[grdTAdvance.Row, (int)TA.gDID] = DTDriver.Rows[0]["ID"].ToString();
                    if (DTDriver.Rows[0]["DriverName"].ToString() != "")
                        grdTAdvance[grdTAdvance.Row, (int)TA.gDNM] = DTDriver.Rows[0]["DriverName"].ToString();
                }
            }
            #endregion
        }
        private void grdTR_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                #region COMMENTED
#endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdTR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdTR.Rows.Remove(grdTR.Row);
                grdTR.Rows[1].AllowEditing = true;
            }
        }
        private void grdTExpense_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdTExpense.Rows.Remove(grdTExpense.Row);
                grdTExpense.Rows[1].AllowEditing = true;
                grdTExpense.Rows.Count += 1;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                SystemCode = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
                if (cmbDriver.Items.Count == 0)
                    return;
                Get_Details();
                Generate_Total();
                Load_Driver_Photo();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private Boolean Save_Pro()
        {
                if (Validate_Data() == false)
                {
                    return false;
                }
                if (Save_Procedure() == false)
                {
                    return false;
                }
                return true;
        }
        private Boolean Validate_Data()
        {
                if (!chkNotPaid.Checked)
                {
                    if (!rdbBank.Checked && !rdbCash.Checked)
                    {
                        MessageBox.Show("Please select a pay method", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                if (chkExcursion.Checked)
                {
                    if (txtExcursion.Text.Trim() == "")
                    {
                        MessageBox.Show("Excursion Amount Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else
                    {
                        if (!Classes.clsGlobal.IsNumeric(txtExcursion.Text.Trim()))
                        {
                            MessageBox.Show("Please Enter Valid Excursion Amount.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        decimal chkAmt=0.00m;
                        chkAmt = Convert.ToDecimal(txtExcursion.Text.Trim());
                        if (chkAmt == 0.00m)
                        {
                            MessageBox.Show("Excursion Amount Cannot Be Zero", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                }
                return true;
        }
        private Boolean Save_Procedure()
        {
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if (Save_Tabs(objCom) == true)
                {
                    objTrn.Commit();
                    MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    objCon.Close();
                    return true;
                }
                else
                {
                    objTrn.Rollback();
                    MessageBox.Show("Data Not Saved Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                objCon.Close();
                return false;
        }
        private Boolean Save_Tabs(System.Data.SqlClient.SqlCommand sqlCom)
        {
                if (Save_Basic_Transport(sqlCom) == false)
                    return false;
                if (Save_Tour_Advance(sqlCom) == false)
                    return false;
                if (Save_Travel_Expenses(sqlCom) == false)
                    return false;
                return true;
        }
        private Boolean Save_Basic_Transport(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_BasicTransport_1";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(cmbDriver.SelectedValue.ToString().Trim());
                    if (chkExcursion.Checked)
                    {
                        sqlCom.Parameters.Add("@Excursion", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@ExcurDesc", SqlDbType.NVarChar, 200).Value = txtExcurDesc.Text.Trim();
                        sqlCom.Parameters.Add("@ExcurAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(txtExcursion.Text.Trim());
                    }
                if (txtStartMeter.Text.ToString() != "")
                    sqlCom.Parameters.Add("@StartMeter", SqlDbType.NVarChar, 100).Value = txtStartMeter.Text.Trim();
                if (txtEndMeter.Text.ToString() != "")
                    sqlCom.Parameters.Add("@EndMeter", SqlDbType.NVarChar, 100).Value = txtEndMeter.Text.Trim();
                if (txtTotKm.Text.ToString() != "")
                    sqlCom.Parameters.Add("@TotalKm", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotKm.Text.Trim());
                if (txtPaidForKm.Text.ToString() != "")
                    sqlCom.Parameters.Add("@PaidForKm", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidForKm.Text.Trim());
                if (txtRateKm.Text.ToString() != "")
                    sqlCom.Parameters.Add("@RatePerKm", SqlDbType.Decimal).Value = Convert.ToDecimal(txtRateKm.Text.Trim());
                if (txtBata.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Bata", SqlDbType.Decimal).Value = Convert.ToDecimal(txtBata.Text.Trim());
                if (txtNights.Text.ToString() != "")
                    sqlCom.Parameters.Add("@NoOfNights", SqlDbType.Decimal).Value = Convert.ToDecimal(txtNights.Text.Trim());
                if (txtPaidForBata.Text.ToString() != "")
                    sqlCom.Parameters.Add("@PaidForBata", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidForBata.Text.Trim());
                sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = chkIsPaid.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = chkNotPaid.Checked ? "1" : "0";
                if (chkIsPaid.Checked)
                {
                    sqlCom.Parameters.Add("@BankPay", SqlDbType.Int).Value = rdbBank.Checked ? "1" : "0";
                    sqlCom.Parameters.Add("@ChkNo", SqlDbType.NVarChar, 100).Value = txtChkNo.Text;
                    sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = dtpPaidDate.Value;
                    sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Classes.clsGlobal.UserID;
                }
                sqlCom.Parameters.Add("@IsConfirm", SqlDbType.Int).Value = chkConfirm.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 500).Value = txtRemarks.Text.ToString().Trim();
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = DrvSrNo;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                {
                    RtnVal = false;
                }
                return RtnVal;
        }
        private Boolean Save_Travel_Expenses(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Travel_Expenses_1";
                RowNumb = 1;
                while (grdTExpense[RowNumb, grdTExpense.Cols[(int)TP.gENM].Index] != null)
                {
                    if (grdTExpense[RowNumb, (int)TP.gENM].ToString().Trim() == "")
                        return true;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdTExpense[RowNumb, (int)TP.gIDN] + "".Trim() != "")
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = Convert.ToInt32(grdTExpense[RowNumb, (int)TP.gIDN]);
                    else
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters.Add("@IsDriver", SqlDbType.Int).Value = 1;
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(cmbDriver.SelectedValue.ToString().Trim());
                    if (grdTExpense[RowNumb, (int)TP.gEID] != null && grdTExpense[RowNumb, (int)TP.gEID].ToString() != "")
                        sqlCom.Parameters.Add("@ExpenseID", SqlDbType.NVarChar).Value = grdTExpense[RowNumb, (int)TP.gEID].ToString();
                    if (grdTExpense[RowNumb, (int)TP.gAMT] != null && grdTExpense[RowNumb, (int)TP.gAMT].ToString() != "")
                        sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTExpense[RowNumb, (int)TP.gAMT].ToString());
                    sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gPID]);
                    sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gNPD]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                    }
                    RowNumb++;
                }
                return RtnVal;
        }
        private Boolean Save_Tour_Advance(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Tour_Advance_1";
                RowNumb = 1;
                while (grdTAdvance[RowNumb, grdTAdvance.Cols[(int)TA.gENM].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdTAdvance[RowNumb, (int)TA.gIDN] + "".Trim() != "")
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gIDN]);
                    else
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters.Add("@IsDriver", SqlDbType.Int).Value = 1;
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(cmbDriver.SelectedValue.ToString().Trim());
                    if (grdTAdvance[RowNumb, (int)TA.gENM] != null && grdTAdvance[RowNumb, (int)TA.gENM].ToString() != "")
                        sqlCom.Parameters.Add("@Expense", SqlDbType.VarChar, 100).Value = grdTAdvance[RowNumb, (int)TA.gENM].ToString();
                    if (grdTAdvance[RowNumb, (int)TA.gAMT] != null && grdTAdvance[RowNumb, (int)TA.gAMT].ToString() != "")
                        sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTAdvance[RowNumb, (int)TA.gAMT].ToString());
                    if (grdTAdvance[RowNumb, (int)TA.gRAM] != null && grdTAdvance[RowNumb, (int)TA.gRAM].ToString() != "")
                        sqlCom.Parameters.Add("@ReturnAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTAdvance[RowNumb, (int)TA.gRAM].ToString());
                    if (Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gPID]))
                    {
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gPBY]);
                        if (grdTAdvance[RowNumb, (int)TA.gPDT] + "".Trim() != "")
                            sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdTAdvance[RowNumb, (int)TA.gPDT]);
                        else
                            sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = Classes.clsGlobal.CurDate();
                    }
                    if (Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gIST]))
                    {
                        sqlCom.Parameters.Add("@IsSettled", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@SettledBy", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gSBY]);
                        if (grdTAdvance[RowNumb, (int)TA.gSDT] + "".Trim() != "")
                            sqlCom.Parameters.Add("@SettledDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdTAdvance[RowNumb, (int)TA.gSDT]);
                        else
                            sqlCom.Parameters.Add("@SettledDate", SqlDbType.DateTime).Value = Classes.clsGlobal.CurDate();
                    }
                    sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gNPD]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                    }
                    RowNumb++;
                }
                return RtnVal;
        }
        private Boolean Save_Transport_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] == null))
                {
                    return true;
                }
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Transport_Details_1";
                while (grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTR].Index] != null))
                        sqlCom.Parameters.Add("@TransTypeID", SqlDbType.Int).Value = Int32.Parse(grdTR[RowNumb, (int)TR.gTR].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gDT].Index] != null))
                        sqlCom.Parameters.Add("@Date", SqlDbType.DateTime).Value = Convert.ToDateTime(grdTR[RowNumb, (int)TR.gDT].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTM].Index] != null))
                        sqlCom.Parameters.Add("@Time", SqlDbType.VarChar, 10).Value = grdTR[RowNumb, (int)TR.gTM].ToString();
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gFI].Index] != null))
                        sqlCom.Parameters.Add("@FromID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gFI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTI].Index] != null))
                        sqlCom.Parameters.Add("@ToID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gTI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gVI].Index] != null))
                        sqlCom.Parameters.Add("@VehicleID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gVI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gDI].Index] != null))
                        sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gDI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gDS].Index] != null))
                        sqlCom.Parameters.Add("@Distance", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTR[RowNumb, (int)TR.gDS].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gCH].Index] != null))
                        sqlCom.Parameters.Add("@Cost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTR[RowNumb, (int)TR.gCH].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    RowNumb++;
                }
                return true;
        }
        private void btnGetTot_Click(object sender, EventArgs e)
        {
            Generate_Total();
        }
        private void Generate_Total()
        {
            try
            {
                int RowNumb;
                double bata = 0.00, travel = 0.00,  expenses = 0.00;
                double bata_d = 0.00, travel_d = 0.00,   expenses_d = 0.00;
                #region BATA AND KM PAYBLE/DUE AMOUNT
                if (chkIsPaid.Checked)
                {
                    if (txtBata.Text.Trim() != "" && txtNights.Text.Trim() != "")
                        bata = Convert.ToDouble(txtBata.Text.Trim()) * Convert.ToDouble(txtNights.Text.Trim());
                    if (txtTotKm.Text.Trim() != "" && txtRateKm.Text.Trim() != "")
                        travel = Convert.ToDouble(txtTotKm.Text.Trim()) * Convert.ToDouble(txtRateKm.Text.Trim());
                }
                else
                {
                    if (txtBata.Text.Trim() != "" && txtNights.Text.Trim() != "")
                        bata_d = Convert.ToDouble(txtBata.Text.Trim()) * Convert.ToDouble(txtNights.Text.Trim());
                    if (txtTotKm.Text.Trim() != "" && txtRateKm.Text.Trim() != "")
                        travel_d = Convert.ToDouble(txtTotKm.Text.Trim()) * Convert.ToDouble(txtRateKm.Text.Trim());
                }
                #endregion
                # region TOUR ADVANCE
                #endregion
                #region TRANSPORT EXPENSES
                RowNumb = 1;
                while (grdTExpense[RowNumb, grdTExpense.Cols[(int)TP.gENM].Index] != null)
                {
                    if (grdTExpense[RowNumb, (int)TP.gAMT] != null && grdTExpense[RowNumb, (int)TP.gAMT].ToString() != "")
                    {
                        if (Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gPID]))
                            expenses += Convert.ToDouble(grdTExpense[RowNumb, (int)TP.gAMT].ToString());
                        else
                            expenses_d += Convert.ToDouble(grdTExpense[RowNumb, (int)TP.gAMT].ToString());
                    }
                    RowNumb++;
                }
                #endregion
                double totpay = bata + travel + expenses;
                lblTotPay.Text = totpay.ToString().Trim() + " LKR";
                double totpay_d = bata_d + travel_d + expenses_d;
                lblDue.Text = totpay_d.ToString().Trim() + " LKR";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkPaid_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void chkIsPaid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsPaid.Checked)
            {
                lblPaidDate.Visible = true;
                dtpPaidDate.Visible = true;
            }
            else
            {
                chkConfirm.Checked = false;
                lblPaidDate.Visible = false;
                dtpPaidDate.Visible = false;
            }
        }
        private void Calculate_Km_Bata_Total()
        {
            try
            {
                double totkm=0,rate=0,bata=0,nights=0,sm=0,em=0;
                if (txtStartMeter.Text.ToString().Trim() != "")
                    sm = Convert.ToDouble(txtStartMeter.Text.ToString().Trim());
                if (txtEndMeter.Text.ToString().Trim() != "")
                    em = Convert.ToDouble(txtEndMeter.Text.ToString().Trim());
                if (txtTotKm.Text.ToString().Trim() != "")
                    totkm = Convert.ToDouble(txtTotKm.Text.ToString().Trim());
                if (txtRateKm.Text.ToString().Trim() != "")
                    rate = Convert.ToDouble(txtRateKm.Text.ToString().Trim());
                if (txtBata.Text.ToString().Trim() != "")
                    bata = Convert.ToDouble(txtBata.Text.ToString().Trim());
                if (txtNights.Text.ToString().Trim() != "")
                    nights = Convert.ToDouble(txtNights.Text.ToString().Trim());
                txtPaidForKm.Text = (totkm * rate).ToString();
                txtPaidForBata.Text = (bata * nights).ToString();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void txtTotKm_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtRateKm_TextChanged(object sender, EventArgs e)
        {
            Calculate_Km_Bata_Total();
        }
        private void txtBata_TextChanged(object sender, EventArgs e)
        {
            Calculate_Km_Bata_Total();
        }
        private void txtNights_TextChanged(object sender, EventArgs e)
        {
            Calculate_Km_Bata_Total();
        }
        private void grdTExpense_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTExpense;
                #region EXPENSE NAME
                if (e.Col == grdTExpense.Cols[(int)TP.gENM].Index)
                {
                    DTExpense = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM mst_TransportExpenses WHERE IsNull(IsActive,0)=1 ORDER BY Name");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTExpense;
                    frm.SubForm = new Transaction.frmExpenses();
                    frm.Width = grdTExpense.Cols[(int)TP.gENM].Width;
                    frm.Height = grdTExpense.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTExpense);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        if (DTExpense.Rows[0]["ID"].ToString() != "")
                            grdTExpense[grdTExpense.Row, (int)TP.gEID] = SelText[0].ToString();
                        if (DTExpense.Rows[0]["Name"].ToString() != "")
                            grdTExpense[grdTExpense.Row, (int)TP.gENM] = SelText[1].ToString();
                    }
                }
                #endregion
        }
        private void grdTAdvance_LeaveCell(object sender, EventArgs e)
        {
                double amt = 0, ret = 0;
                if (grdTAdvance.Rows.Count <= 1)
                    return;
                if (grdTAdvance[1, (int)TA.gAMT] == null || grdTAdvance[1, (int)TA.gAMT].ToString() == "")
                    return;
                if (grdTAdvance[grdTAdvance.Row, (int)TA.gAMT] != null && grdTAdvance[grdTAdvance.Row, (int)TA.gAMT].ToString() != "")
                    amt = Convert.ToDouble(grdTAdvance[grdTAdvance.Row, (int)TA.gAMT].ToString());
                if (grdTAdvance[grdTAdvance.Row, (int)TA.gRAM] != null && grdTAdvance[grdTAdvance.Row, (int)TA.gRAM].ToString() != "")
                    ret = Convert.ToDouble(grdTAdvance[grdTAdvance.Row, (int)TA.gRAM].ToString());
                grdTAdvance[grdTAdvance.Row, (int)TA.gBAL] = (amt - ret).ToString();
        }
        private void lblOriginal_Click(object sender, EventArgs e)
        {
                double tourno;
                if (txtTourNo.Text.ToString().Trim() == "")
                    return;
                else
                    tourno=Convert.ToDouble(txtTourNo.Text.ToString().Trim());
                Transaction.frmGroupAmend frmGA;
                frmGA = new Transaction.frmGroupAmend();
                frmGA.Mode = 1;
                frmGA.SystemCode = tourno;
                frmGA.ShowDialog();
        }
        private void txtExcursion_Enter(object sender, EventArgs e)
        {
            if (txtExcursion.Text.ToString().Trim() == "Excursion")
            {
                txtExcursion.Text = "";
                txtExcursion.TextAlign = HorizontalAlignment.Right;
            }
        }
        private void txtExcursion_Leave(object sender, EventArgs e)
        {
            if (txtExcursion.Text.ToString().Trim() == "")
            {
                txtExcursion.Text = "Excursion";
                txtExcursion.TextAlign = HorizontalAlignment.Center;
            }
            else
            {
                txtExcursion.TextAlign = HorizontalAlignment.Right;
            }
        }
        private void chkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            if (chkConfirm.Checked)
            {
                if (!Check_For_confirmation())
                {
                    chkConfirm.Checked = false;
                }
            }
        }
        private bool Check_For_confirmation()
        {
                int RowNumb;
                #region CHECK FOR PAYMENTS OF BASICS
                if (!chkNotPaid.Checked)
                {
                    if (!chkIsPaid.Checked)
                    {
                        MessageBox.Show("Cannot Confirm With Due on basic payments.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        chkNotPaid.Checked = false;
                    }
                }
                #endregion
                #region CHECK FOR PAYMENTS OF ADVANCES
                RowNumb = 1;
                while (grdTAdvance[RowNumb, grdTAdvance.Cols[(int)TA.gENM].Index] != null)
                {
                    if ((Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gNPD])))
                    {
                        RowNumb++;
                        continue;
                    }
                    if (!Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gIST]))
                    {
                        MessageBox.Show("Cannot Confirm With Due on Advance Payments.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else
                    {
                        grdTAdvance[RowNumb, (int)TA.gNPD] = 0;
                    }
                    RowNumb++;                    
                }
                #endregion
                #region CHECK FOR PAYMENTS OF EXPENSES
                RowNumb = 1;
                while(grdTExpense[RowNumb, grdTExpense.Cols[(int)TP.gENM].Index] != null)
                {
                    if ((Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gNPD])))
                    {
                        RowNumb++;
                        continue;
                    }
                    if (!Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gPID]))
                    {
                        MessageBox.Show("Cannot Confirm With Due on Expenses.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else
                    {
                        grdTExpense[RowNumb, (int)TP.gNPD] = 0;
                    }
                    RowNumb++;
                }
                #endregion
                return true;
        }
        private void grdTR_Click(object sender, EventArgs e)
        {
        }
        private void grdTAdvance_Click(object sender, EventArgs e)
        {
                if (grdTAdvance[grdTAdvance.Row, (int)TA.gENM] + "".Trim() == "")
                    return;
                else if (!Convert.ToBoolean(grdTAdvance[grdTAdvance.Row, (int)TA.gIST]))
                    chkConfirm.Checked = false;
                btnDelete.Enabled = true;
        }
        private void grdTExpense_Click(object sender, EventArgs e)
        {
                if (grdTExpense[grdTExpense.Row, (int)TP.gENM] + "".Trim() == "")
                    return;
                else if (!Convert.ToBoolean(grdTExpense[grdTExpense.Row, (int)TP.gPID]))
                    chkConfirm.Checked = false;
                btnDelete.Enabled = true;
        }
        private void grdTAdvance_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (Convert.ToBoolean(grdTAdvance[grdTAdvance.Row, (int)TA.gIST]))
            {
                grdTAdvance[grdTAdvance.Row, (int)TA.gSBY] = Convert.ToInt32(Classes.clsGlobal.UserID);
                if(grdTAdvance[grdTAdvance.Row, (int)TA.gSDT]+"".Trim()=="")
                    grdTAdvance[grdTAdvance.Row, (int)TA.gSDT] = Convert.ToDateTime(Classes.clsGlobal.CurDate());
            }
            else
            {
                grdTAdvance[grdTAdvance.Row, (int)TA.gSBY] = null;
                grdTAdvance[grdTAdvance.Row, (int)TA.gSDT] = null;
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (cmbRepType.Text.ToString().Trim() == "" || cmbRepType.SelectedItem.ToString().Trim() == "" )
            {
                MessageBox.Show("Please select a report type.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Print_Transaction_Report(SystemCode);
            }
        }  
        public void Print_Transaction_Report(double tourid)
        {
            try
            {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                string sql = "";
                DataTable DT; 
                DataSet DTG = new DataSet();
                ReportDocument ga = new ReportDocument();
                /*fsql = "SELECT Company_Logo,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address," +
                             "ParentID,TourID,Guest,GuestMobile,AAirportID,AFlightNo,AFlightTime,DAirportID,DFlightNo,DFlightTime," +
                             "DateArrival,DateDeparture,NoOfAdult,NoOfChild,HandledBy," +
                             "DriverID,DriverName,VehicleNo," +//IsNull(IsEmp,0) AS IsEmp,
                             "DriverLicenseNo,DriverTel,StartMeter,EndMeter,TotalKm,RatePerKm,Bata,NoOfNights,IsPaid,Name,Amount,EIsPaid," +
                             "Advance,ISNULL(AdvanceAmount,0)AdvanceAmount,"+
                            "ISNULL(ReturnAmt,0)ReturnAmt,ISNULL(AIsPaid,0)AIsPaid,ExcurDesc,ExcurAmt" +
                             " FROM vw_acc_DriverPayments" +
                             " WHERE ParentID=" + tourid + " AND DriverID=" + cmbDriver.SelectedValue + " Order By DriverID "; */
                sql = "SELECT Company_Logo,DisplayName,Telephone,Mobile,Fax,E_mail,E_mailTo,Web,Physical_Address," +
                             "ID,TourID,Guest,ArrivalFlight,ArrivalTime,DepartFlight,DepartTime," +
                             "ArrivalDate,DeprtDate,NoOfAdult,NoOfChild,HandledBy," +
                             "DriverID,DriverName,VehicleNo,UserName,UserGroupID,GroupName," +//IsNull(IsEmp,0) AS IsEmp,
                             "Tel1,Tel2,StartMeter,EndMeter,TotalKm,RatePerKm,Bata,NoOfNights,IsPaid,ExpenseName,Amount,ISNULL(IsSettled,0) AS IsSettled," +
                             "IsConfirm,TypeID,TypeName," +
                            "ISNULL(ReturnAmt,0)ReturnAmt,ExcurDesc,ExcurAmt,UniqueID" +
                             " FROM vw_trn_DriverPayments_ALL" +
                             " WHERE ID=" + tourid + " AND DriverID=" + cmbDriver.SelectedValue + " AND UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "Order By DriverID ";
                DTG = new DataSets.ds_DriverSettlement1();
                if (cmbRepType .SelectedItem.ToString().Trim() == "Detail Report")
                    ga = new Tourist_Management.TransacReports.TR_DriverSettlement1();
                else if (cmbRepType .SelectedItem.ToString().Trim() == "Cash Payment")
                    ga = new Tourist_Management.TransacReports.rpt_CashPaymentVoucher_1();
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    sConnection.Print_Report(tourid.ToString(), sql, DTG, ga, "");
                }
                else
                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void txtEndMeter_TextChanged(object sender, EventArgs e)
        {
            Calculate_Km_Bata_Total();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int uniqueID = 0;
            if (grid == "Advance")
            {
                if (grdTAdvance[grdTAdvance.Row, (int)TA.gIDN] == null || grdTAdvance[grdTAdvance.Row, (int)TA.gIDN].ToString().Trim() == "" || grdTAdvance[grdTAdvance.Row, (int)TA.gIDN].ToString().Trim() == null)
                {
                    grdTAdvance.Rows.Remove(grdTAdvance.Row);
                }
                else
                {
                    if (MessageBox.Show("Do You Want To Delete This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return;
                    }
                    uniqueID = Convert.ToInt32(grdTAdvance[grdTAdvance.Row, (int)TA.gIDN]);
                    Color_Delete_Row(grdTAdvance.Row, grid);
                    Pre_Delete(uniqueID);
                }
            }
            else if (grid == "Expence")
            {
                if (grdTExpense[grdTExpense.Row, (int)TP.gIDN] == null || grdTExpense[grdTExpense.Row, (int)TP.gIDN].ToString().Trim() == "" || grdTExpense[grdTExpense.Row, (int)TP.gIDN].ToString().Trim() == null)
                {
                    grdTExpense.Rows.Remove(grdTExpense.Row);
                }
                else
                {
                    if (MessageBox.Show("Do You Want To Delete This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return;
                    }
                    uniqueID = Convert.ToInt32(grdTExpense[grdTExpense.Row, (int)TP.gIDN]);
                    Color_Delete_Row(grdTExpense.Row, grid);
                    Pre_Delete(uniqueID);
                }
            }
            grid = "";
        }
        private void Color_Delete_Row(int row, string grid)
        {
            try
            {
                C1.Win.C1FlexGrid.CellStyle dltExp = grdTExpense.Styles.Add("Delete");
                dltExp.BackColor = Color.PaleVioletRed;
                C1.Win.C1FlexGrid.CellStyle dltAdv = grdTAdvance.Styles.Add("Delete");
                dltAdv.BackColor = Color.PaleVioletRed;
                if (grid == "Expence")
                {
                    grdTExpense.Rows[row].Style = grdTExpense.Styles["Delete"];
                }
                else if (grid == "Advance")
                {
                    grdTAdvance.Rows[row].Style = grdTAdvance.Styles["Delete"];
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Pre_Delete(int uniqueID)
        {
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
            try
            {
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if (grid == "Expence")
                {
                    if (DeleteExpence(objCom, uniqueID))
                    {
                        objTrn.Commit();
                        MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        objCon.Close();
                    }
                    else
                    {
                        objTrn.Rollback();
                        MessageBox.Show("Data Not Deleted Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (grid == "Advance")
                {
                    if (DeleteAdvance(objCom, uniqueID))
                    {
                        objTrn.Commit();
                        MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        objCon.Close();
                    }
                    else
                    {
                        objTrn.Rollback();
                        MessageBox.Show("Data Not Deleted Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                objCon.Close();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean DeleteExpence(System.Data.SqlClient.SqlCommand objCom, int uniqueID)
        {
            Boolean RtnVal = true;
                objCom.CommandType = CommandType.StoredProcedure;
                objCom.CommandText = "sp_Delete_Travel_Expenses";
                objCom.Parameters.Clear();
                objCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = uniqueID ;
                objCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                objCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                objCom.ExecuteNonQuery();
                if ((int)objCom.Parameters["@RtnValue"].Value != 1)
                {
                    RtnVal = false;
                }
                return RtnVal;
        }
        private Boolean DeleteAdvance(System.Data.SqlClient.SqlCommand objCom, int uniqueID)
        {
            Boolean RtnVal = true;
                objCom.CommandType = CommandType.StoredProcedure;
                objCom.CommandText = "sp_Delete_Tour_Advance";
                objCom.Parameters.Clear();
                objCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = uniqueID;
                objCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                objCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                objCom.ExecuteNonQuery();
                if ((int)objCom.Parameters["@RtnValue"].Value != 1)
                {
                    RtnVal = false;
                }
                return RtnVal;
        }
        private void grdTAdvance_Leave(object sender, EventArgs e)
        {
            grid = "Advance";
        }
        private void grdTExpense_Leave(object sender, EventArgs e)
        {
            grid = "Expence";
        }
        private void rdbBank_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBank.Checked)
            {
                txtChkNo.Enabled = true;
                chkIsPaid.Checked = true;
            }
            else
                txtChkNo.Enabled = false;
        }
        private void rdbCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCash.Checked)
            {
                txtChkNo.Text = "";
                chkIsPaid.Checked = true;
            }
        }
        private void chkNotPaid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNotPaid.Checked)
            {
                if (Check_For_confirmation())
                {
                    chkNotPaid.Checked = true;
                }
                else
                {
                    chkNotPaid.Checked = false;
                }
            }
        }   
    }
}
