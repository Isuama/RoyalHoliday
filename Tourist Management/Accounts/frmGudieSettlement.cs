using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
namespace Tourist_Management.Accounts
{
    public partial class frmGudieSettlement : Form
    {
        private const string msghd = "Guide Settlement";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE        
        string[] GuideNames = new string[10];
        string[] GuideIds = new string[10];
        string grid = "";
        enum TA { gIDN, gDID, gDNM, gEID, gENM, gAMT, gRAM, gBAL, gPID, gPDT, gPBY, gIST, gSDT, gSBY, gNPD };
        enum TP { gIDN, gIDR, gDID, gDNM, gEID, gENM, gAMT, gPID, gNPD };
        public frmGudieSettlement(){InitializeComponent();}
        private void frmGudieSettlement_Load(object sender, EventArgs e)
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
                #region TRANSPORT EXPENSES DETAILS
                grdTExpense.Cols.Count = 9;
                grdTExpense.Cols[(int)TP.gIDN].Width = 50;
                grdTExpense.Cols[(int)TP.gIDR].Width = 0;
                grdTExpense.Cols[(int)TP.gDID].Width = 0;
                grdTExpense.Cols[(int)TP.gDNM].Width = 0;
                grdTExpense.Cols[(int)TP.gEID].Width = 0;
                grdTExpense.Cols[(int)TP.gENM].Width = 320;
                grdTExpense.Cols[(int)TP.gAMT].Width = 112;
                grdTExpense.Cols[(int)TP.gPID].Width = 60;
                grdTExpense.Cols[(int)TP.gNPD].Width = 60;
                grdTExpense.Cols[(int)TP.gIDN].Caption = "ID";
                grdTExpense.Cols[(int)TP.gIDR].Caption = "IsDriver";
                grdTExpense.Cols[(int)TP.gDID].Caption = "Guide ID";
                grdTExpense.Cols[(int)TP.gDNM].Caption = "Name";
                grdTExpense.Cols[(int)TP.gEID].Caption = "Expense ID";
                grdTExpense.Cols[(int)TP.gENM].Caption = "Expense Name";
                grdTExpense.Cols[(int)TP.gAMT].Caption = "Amount";
                grdTExpense.Cols[(int)TP.gPID].Caption = "Is Paid";
                grdTExpense.Cols[(int)TP.gNPD].Caption = "Not Paid";
                grdTExpense.Cols[(int)TP.gAMT].Format = "##.##";
                grdTExpense.Cols[(int)TP.gDNM].ComboList = "...";
                grdTExpense.Cols[(int)TP.gENM].ComboList = "...";
                grdTExpense.Cols[(int)TP.gIDR].DataType = Type.GetType(" System.Boolean");
                grdTExpense.Cols[(int)TP.gPID].DataType = Type.GetType(" System.Boolean");
                grdTExpense.Cols[(int)TP.gNPD].DataType = Type.GetType(" System.Boolean");
                grdTExpense.Rows[1].AllowEditing = true;
                #endregion
                #region TOUR ADVANCE
                grdTAdvance.Cols.Count = 15;
                grdTAdvance.Rows.Count = 100;
                grdTAdvance.Cols[(int)TA.gIDN].Width = 50;
                grdTAdvance.Cols[(int)TA.gDID].Width = 0;
                grdTAdvance.Cols[(int)TA.gDNM].Width = 0;
                grdTAdvance.Cols[(int)TA.gEID].Width = 0;
                grdTAdvance.Cols[(int)TA.gENM].Width = 144;
                grdTAdvance.Cols[(int)TA.gAMT].Width = 70;
                grdTAdvance.Cols[(int)TA.gRAM].Width = 80;
                grdTAdvance.Cols[(int)TA.gBAL].Width = 70;
                grdTAdvance.Cols[(int)TA.gPID].Width = 0;
                grdTAdvance.Cols[(int)TA.gPDT].Width = 0;
                grdTAdvance.Cols[(int)TA.gPBY].Width = 0;
                grdTAdvance.Cols[(int)TA.gIST].Width = 50;
                grdTAdvance.Cols[(int)TA.gSDT].Width = 83;
                grdTAdvance.Cols[(int)TA.gSBY].Width = 0;
                grdTAdvance.Cols[(int)TA.gNPD].Width = 60;
                grdTAdvance.Cols[(int)TA.gIDN].Caption = "ID";
                grdTAdvance.Cols[(int)TA.gDID].Caption = "Guide ID";
                grdTAdvance.Cols[(int)TA.gDNM].Caption = "Guide Name";
                grdTAdvance.Cols[(int)TA.gEID].Caption = "Expense ID";
                grdTAdvance.Cols[(int)TA.gENM].Caption = "Expense Name";
                grdTAdvance.Cols[(int)TA.gAMT].Caption = "Amount";
                grdTAdvance.Cols[(int)TA.gRAM].Caption = "Returned";
                grdTAdvance.Cols[(int)TA.gBAL].Caption = "Balance";
                grdTAdvance.Cols[(int)TA.gPID].Caption = "Paid";
                grdTAdvance.Cols[(int)TA.gPDT].Caption = "Paid Date";
                grdTAdvance.Cols[(int)TA.gPBY].Caption = "Paid By";
                grdTAdvance.Cols[(int)TA.gIST].Caption = "Settled";
                grdTAdvance.Cols[(int)TA.gSDT].Caption = "Settled Date";
                grdTAdvance.Cols[(int)TA.gSBY].Caption = "Settled By";
                grdTAdvance.Cols[(int)TA.gNPD].Caption = "Not Paid";
                grdTAdvance.Cols[(int)TA.gDNM].ComboList = "...";
                grdTAdvance.Cols[(int)TA.gAMT].Format = "##.##";
                grdTAdvance.Cols[(int)TA.gPID].DataType = Type.GetType(" System.Boolean");
                grdTAdvance.Cols[(int)TA.gIST].DataType = Type.GetType(" System.Boolean");
                grdTAdvance.Cols[(int)TA.gNPD].DataType = Type.GetType(" System.Boolean");
                grdTAdvance.Cols[(int)TA.gPDT].DataType = Type.GetType(" System.DateTime");
                grdTAdvance.Cols[(int)TA.gSDT].DataType = Type.GetType(" System.DateTime");
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID FROM trn_GroupAmendment";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.ToString().Trim() == "")
            {
                lblGuideName.Visible = false;
                cmbGuide.Visible = false;
                return;
            }
            else
            {
                lblGuideName.Visible = true;
                cmbGuide.Visible = true;
            }
            Fill_Control();
            Get_Details();
            Generate_Total();
        }
        private void Clear_Contents()
        {
            lblTotPay.Text = "0.00";
            lblDue.Text = "0.00";
            txtGuideName.Text = "";
            txtCompany.Text = "";
            txtNIC.Text = "";
            txtLicense.Text = "";
            txtTelephone.Text = "";
            txtFee.Text = "";
            txtDays.Text = "";
            txtPaidAmt.Text = "";
            chkConfirm.Checked = false;
            chkPaid.Checked = false;
            grdTAdvance.Rows.Count = 1;
            grdTAdvance.Rows.Count = 500;
            grdTExpense.Rows.Count = 1;
            grdTExpense.Rows.Count = 500;
        }
        private void Fill_Control()
        {
            try
            {
                if (txtTourNo.Text.ToString().Trim() == "")  return;
                else
                {
                    SystemCode = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
                    cmbGuide.Enabled = true;
                    btnOk.Enabled = true;
                }
                DataTable[] DTB = new DataTable[2]; DataTable DT;
                 cmbGuide.DataSource =DT=Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DISTINCT GuideID,Name FROM vw_trn_GuideDetails Where TransID=" + txtTourNo.Text.Trim() + " AND ISNULL(IsCancelled,0)<>1"); 
                txtGuideName.Text="";
                if (DT.Rows.Count > 0)   txtGuideName.Text = DT.Rows[0]["Name"] + "".Trim();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Get_Details()
        {
            string ssql;
            DataTable DT;
            int GuideID = 0;
                if (cmbGuide.Items.Count == 0)    return;
                if (cmbGuide.SelectedValue.ToString().Trim() != "")       GuideID = Convert.ToInt32(cmbGuide.SelectedValue.ToString().Trim());
                else     return;
                grdTAdvance.Rows.Count = 1;
                grdTAdvance.Rows.Count = 500;
                grdTExpense.Rows.Count = 1;
                grdTExpense.Rows.Count = 500;
                #region FILL GUIDE DETAILS
                if (cmbGuide.SelectedValue == null)
                    return;
                ssql = "SELECT Name,CompanyName,IdentityNo,LicenseNo,TelHome" +
                       " FROM vwGuideVsEmployee WHERE ID=" + cmbGuide.SelectedValue.ToString().Trim() + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    if (DT.Rows[0]["Name"].ToString() != "")
                        txtGuideName.Text = DT.Rows[0]["Name"].ToString();
                    if (DT.Rows[0]["CompanyName"].ToString() != "")
                        txtCompany.Text = DT.Rows[0]["CompanyName"].ToString();
                    if (DT.Rows[0]["IdentityNo"].ToString() != "")
                        txtNIC.Text = DT.Rows[0]["IdentityNo"].ToString();
                    if (DT.Rows[0]["LicenseNo"].ToString() != "")
                        txtLicense.Text = DT.Rows[0]["LicenseNo"].ToString();
                    if (DT.Rows[0]["TelHome"].ToString() != "")
                        txtTelephone.Text = DT.Rows[0]["TelHome"].ToString();
                }
                #endregion
                #region FILL GUIDE COST DETAILS
                ssql = "SELECT Fee,Days,PaidAmount,IsNull(IsChecked,0)AS IsChecked,PaidDate,IsNull(IsPaid,0)AS IsPaid," +
                       "IsNull(NotPaid,0)AS NotPaid,ISNULL(IsConfirm,0)AS IsConfirm,BankPay,ChkNo" +
                       " FROM vw_trn_GuideDetails WHERE GuideID=" + cmbGuide.SelectedValue.ToString().Trim() + ""+
                       "AND TransID=" + txtTourNo.Text.ToString().Trim() + " AND IsNull(IsCancelled,0)<>1";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    if (DT.Rows[0]["Fee"].ToString() != "")
                        txtFee.Text = DT.Rows[0]["Fee"].ToString();
                    else
                        txtFee.Text = "0.00";
                    if (DT.Rows[0]["Days"].ToString() != "")
                        txtDays.Text = DT.Rows[0]["Days"].ToString();
                    else
                        txtDays.Text = "0.00";
                    if (DT.Rows[0]["PaidAmount"].ToString() != "")
                    {
                        lblTotPay.Text = DT.Rows[0]["PaidAmount"].ToString();
                        txtPaidAmt.Text = DT.Rows[0]["PaidAmount"].ToString();
                    }
                    chkPaid.Checked = Convert.ToBoolean(DT.Rows[0]["IsPaid"]);
                    chkNotPaid.Checked = Convert.ToBoolean(DT.Rows[0]["NotPaid"]);
                    chkConfirm.Checked = Convert.ToBoolean(DT.Rows[0]["IsConfirm"]);
                    if (chkPaid.Checked)
                    {
                        if (DT.Rows[0]["PaidDate"].ToString().Trim() != "")
                            dtpPaidDate.Value = Convert.ToDateTime(DT.Rows[0]["PaidDate"]);
                    }
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
                        chkConfirm.Enabled = true;
                        grpBasics.Enabled = true;
                        grdTAdvance.Enabled = true;
                        grdTExpense.Enabled = true;
                    lblDue.Text = ((Convert.ToDouble(txtFee.Text) * Convert.ToDouble(txtDays.Text)) - Convert.ToDouble(lblTotPay.Text)).ToString().Trim();
                }
                #endregion
                #region Fill Tour Advance
                ssql = "SELECT ID,DriverID,DriverName,ExpenseID,Expense,ISNULL(Amount,0)AS Amount," +
                       "ISNULL(ReturnAmt,0)AS ReturnAmt," +
                       "IsNull(IsChecked,0)AS IsChecked,IsNull(NotPaid,0)AS NotPaid,"+
                       "IsNull(IsPaid,0)AS IsPaid,PaidDate,PaidBy," +
                       "IsNull(IsSettled,0)AS IsSettled,SettledDate,SettledBy, IsDeleted" +
                       " FROM vw_trn_Tour_Advance WHERE TransID=" + SystemCode + " AND ISNULL(IsDriver,0)=0 AND DriverID=" + GuideID + "" +
                       " AND IsDeleted='False' ORDER BY SrNo ";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    int RowNumb = 0;
                    double Amt = 0, Rtn = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdTAdvance[RowNumb + 1, (int)TA.gIDN] = Convert.ToInt32(DT.Rows[RowNumb]["ID"]);
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
                #region TRAVEL EXPENSES
                ssql = "SELECT ID,ISNULL(IsDriver,0)AS IsDriver,DriverID,ExpenseID,Expense,Amount,ISNULL(IsPaid,0)AS IsPaid" +
                       ",ISNULL(NotPaid,0)AS NotPaid, IsDeleted " +
                       "FROM vw_trn_Travel_Expenses WHERE TransID=" + SystemCode + " AND ISNULL(IsDriver,0)=0 AND DriverID=" + GuideID + " AND IsDeleted='False' ORDER BY SrNo";
                DataTable DTTravel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DTTravel.Rows.Count > 0)
                {
                    int RowNumb = 0;
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
                        grdTExpense[RowNumb + 1, (int)TP.gIDN] = Convert.ToInt32(DTTravel.Rows[RowNumb]["ID"]);
                        grdTExpense[RowNumb + 1, (int)TP.gEID] = DTTravel.Rows[RowNumb]["ExpenseID"].ToString();
                        grdTExpense[RowNumb + 1, (int)TP.gENM] = DTTravel.Rows[RowNumb]["Expense"].ToString();
                        grdTExpense[RowNumb + 1, (int)TP.gAMT] = DTTravel.Rows[RowNumb]["Amount"].ToString();
                        grdTExpense[RowNumb + 1, (int)TP.gPID] = Convert.ToBoolean(DTTravel.Rows[RowNumb]["IsPaid"]);
                        grdTExpense[RowNumb + 1, (int)TP.gNPD] = Convert.ToBoolean(DTTravel.Rows[RowNumb]["NotPaid"]);
                        RowNumb++;
                    }
                }
                #endregion
                Generate_Total();
        }
        private void Generate_Total()
        {
            int RowNumb = 1;
            double fee = 0, days = 0, paidamt = 0, DueAmt = 0; 
            double ExpAmt = 0, PaidExpAmt = 0,DueExpAmt = 0;
            if (txtFee.Text.ToString().Trim() != "")
            {
                fee = Convert.ToDouble(txtFee.Text.ToString().Trim());
            }
            if (txtDays.Text.ToString().Trim() != "")
            {
                days = Convert.ToDouble(txtDays.Text.ToString().Trim());
            }
            if (txtPaidAmt.Text.ToString().Trim() != "")
            {
                if(chkPaid.Checked)
                    paidamt = Convert.ToDouble(txtPaidAmt.Text.ToString().Trim());   
            }
            DueAmt = (fee * days) - paidamt;
            #region TOUR ADVANCES
            #endregion
            RowNumb = 1;
            while (grdTExpense[RowNumb, grdTExpense.Cols[(int)TP.gENM].Index] != null)
            {
                if (grdTExpense[RowNumb, (int)TP.gAMT] != null && grdTExpense[RowNumb, (int)TP.gAMT].ToString() != "")
                    ExpAmt = Convert.ToDouble(grdTExpense[RowNumb, (int)TP.gAMT].ToString());
                if (Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gPID]))
                  PaidExpAmt+= ExpAmt;
                else
                   DueExpAmt+= ExpAmt;
                RowNumb++;
            }
            lblTotPay.Text = (paidamt + PaidExpAmt).ToString();
            lblDue.Text = (DueAmt + DueExpAmt).ToString();
        }
        private void btnGetTot_Click(object sender, EventArgs e)
        {
            Generate_Total();
        }
        private void cmbGuide_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_Details();
            Generate_Total();
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                Get_Details();
                Generate_Total();
            }
        }
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
                if (Save_Guide_Expenses(sqlCom) == false)
                    return false;
                if (Save_Tour_Advance(sqlCom) == false)
                    return false;
                if (Save_Travel_Expenses(sqlCom) == false)
                    return false;
                return true;
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
                    sqlCom.Parameters.Add("@IsDriver", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(cmbGuide.SelectedValue.ToString().Trim());
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
                    sqlCom.Parameters.Add("@IsDriver", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(cmbGuide.SelectedValue.ToString().Trim());
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
        private Boolean Save_Guide_Expenses(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Guide_Details_1";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode; 
                sqlCom.Parameters.Add("@GuideID", SqlDbType.Int).Value = cmbGuide.SelectedValue.ToString().Trim();
                if (txtFee.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@Fee", SqlDbType.Decimal).Value = Convert.ToDouble(txtFee.Text.ToString().Trim());
                if (txtDays.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@NoOfDays", SqlDbType.Decimal).Value = Convert.ToDouble(txtDays.Text.ToString().Trim());
                if(txtPaidAmt.Text.ToString().Trim()!="")
                    sqlCom.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = Convert.ToDouble(txtPaidAmt.Text.ToString().Trim());
                sqlCom.Parameters.Add("@IsConfirm", SqlDbType.Int).Value = chkConfirm.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = chkPaid.Checked ? "1" : "0";
                if (chkPaid.Checked)
                {
                    sqlCom.Parameters.Add("@BankPay", SqlDbType.Int).Value = rdbBank.Checked ? "1" : "0";
                    sqlCom.Parameters.Add("@ChkNo", SqlDbType.NVarChar, 100).Value = txtChkNo.Text;
                    sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = dtpPaidDate.Value;
                    sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Classes.clsGlobal.UserID;
                }
                sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = chkNotPaid.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                {
                    RtnVal = false;
                }
                return RtnVal;
        }
        private void chkPaid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPaid.Checked)
            {
                lblPaidDate.Visible = true;
                dtpPaidDate.Visible = true;
            }
            else
            {
                lblPaidDate.Visible = false;
                dtpPaidDate.Visible = false;
            }
        }
        private void txtFee_TextChanged(object sender, EventArgs e)
        {
            Generate_Guide_Total();
        }
        private void txtDays_TextChanged(object sender, EventArgs e)
        {
            Generate_Guide_Total();
        }
        private void Generate_Guide_Total()
        {
                double fee = 0, days = 0;
                if (txtFee.Text.ToString().Trim() != "")
                    fee = Convert.ToDouble(txtFee.Text.ToString().Trim());
                if (txtDays.Text.ToString().Trim() != "")
                    days = Convert.ToDouble(txtDays.Text.ToString().Trim());
                txtPaidAmt.Text=(fee * days).ToString();
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
        private void grdTAdvance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdTAdvance.Rows.Remove(grdTAdvance.Row);
                grdTAdvance.Rows[1].AllowEditing = true;
                grdTAdvance.Rows.Count += 1;
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
        private void lblOriginal_Click(object sender, EventArgs e)
        {
                double tourno;
                if (txtTourNo.Text.ToString().Trim() == "")
                    return;
                else
                    tourno = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
                Transaction.frmGroupAmend frmGA;
                frmGA = new Transaction.frmGroupAmend();
                frmGA.Mode = 1;
                frmGA.SystemCode = tourno;
                frmGA.ShowDialog();
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
                    if (!chkPaid.Checked)
                    {
                        MessageBox.Show("Cannot Confirm With Due on basic payments.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
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
                while (grdTExpense[RowNumb, grdTExpense.Cols[(int)TP.gENM].Index] != null)
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
        private void chkPaid_CheckedChanged_1(object sender, EventArgs e)
        {
            if (!chkPaid.Checked)
                chkConfirm.Checked = false;
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
        private void btnIPreview_Click(object sender, EventArgs e)
        {
            Print_Report();
        }
        private void Print_Report()
        {
            try
            {
                int Gcode = Convert.ToInt32(cmbGuide.SelectedValue);
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                string q = "select * from vw_trn_GuidePayments_ALL where ID=" + SystemCode + " AND GuideID=" + Gcode + 
                            " AND UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                string reptype = "Guide Settlements";
                DataTable DT;
                DataSet DTG = new DataSet();
                ReportDocument rptGS = new ReportDocument();
                DTG = new DataSets.dss_acc_GP();
                rptGS = new Tourist_Management.TransacReports.rpt_GuideSettlement_new();
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(q);
                if (DT.Rows.Count > 0)
                {
                    sConnection.Print_Report("", q, DTG, rptGS, reptype);
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdTAdvance_Leave(object sender, EventArgs e)
        {
            grid = "Advance";
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
        public void Pre_Delete(int uniqueID)
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
        private Boolean DeleteExpence(System.Data.SqlClient.SqlCommand objCom, int uniqueID)
        {
            Boolean RtnVal = true;
                objCom.CommandType = CommandType.StoredProcedure;
                objCom.CommandText = "sp_Delete_Travel_Expenses";
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
        private void grdTExpense_Leave(object sender, EventArgs e)
        {
            grid = "Expence";
        }
        private void rdbBank_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBank.Checked)
            {
                txtChkNo.Enabled = true;
                chkPaid.Checked = true;
            }
            else
                txtChkNo.Enabled = false;
        }
        private void rdbCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCash.Checked)
            {
                chkPaid.Checked = true;
                txtChkNo.Text = "";
            }
        }
        private void chkNotPaid_CheckedChanged(object sender, EventArgs e)
        {
            if (Check_For_confirmation() && chkNotPaid.Checked)
                chkNotPaid.Checked = true;
            else
                chkNotPaid.Checked = false;
        }
    }
}
