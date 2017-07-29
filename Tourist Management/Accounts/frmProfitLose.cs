using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Threading;
using System.Reflection;
using C1.Win.C1FlexGrid;
using System.Collections;
using System.Data.SqlClient;
namespace Tourist_Management.Accounts
{
    public partial class frmProfitLose : Form
    {
        private const string msghd = "Profit And Loss";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE       
        double extend = 0.00;
        bool IsGain = false;
        enum OI { gIID, gEID, gEXN, gTOT, gRMK };
        enum EX { gEXN, gRCS, gRCT, gCRT, gGRC, gGRO, gGCR, gFRM, gNGT, gCMS, gNOA, gNOC, gNOG, gFOA, gFOC, gAMC, gCMC, gGMC, gEBD, gEBC, gCUR, gGCU, gAMT, gGAM, gTOT, gVAT, gVAMT, gGTOT, gPID, gRMK };
        enum ST { gIDN, gTID, gCNM, gCAN, gCON, gDTE, gNOA, gNOC, gNOP, gPOLU, gPOLR, gAID, gANM, gIAMU, gIAMR, gSTS, gPAMU, gPAMR, gHID, gHNM };
        public delegate void FillMonthlyStatement();
        Thread thread;
        public frmProfitLose()   { InitializeComponent();     tpStatement.Hide();    }
        private void frmProfitLose_Load(object sender, EventArgs e)  {   Intializer();  }
        private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID,DateArrival,DateDeparture,NoOfAdult,NoOfChild FROM trn_GroupAmendment";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            Load_Tour();
        }
        private void Load_Tour()
        { 
                if (txtTourNo.Text.ToString().Trim() != "")   SystemCode = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
                else
                {
                    SystemCode = 0;
                    return;
                }
                Fill_Control();
                Get_Details();
                Generate_Tot_Income();
                btnGenerate_Click(null, null); 
        }
        private void Clear_Contents()
        {
            txtCombineID.Text = "";
            lblCombineName.Text = "";
                txtTourNo.Text = "";
                txtDate.Text = "";
                txtPax.Text = "0";
                txtGuide.Text = "";
                txtGuest.Text = "";
                txtHotelCom.Text = "0.00";
                txtExcurtion.Text = "0.00";
                txtExtras.Text = "0.00";
                txtMaldive.Text = "0.00";
                txtDirectPay.Text = "0.00";
                txtAgentName.Text = "";
                txtInvRate.Text = "0.00";
                drpAgentCurrency.setSelectedValue(null);
                txtAgentCom.Text = "0.00";
                txtBankChargers.Text = "0.00";
                rdbPercentage.Checked =true;
                rdbAmount.Checked = false;
                txtAgentInvNo.Text = "";
                dtpInvDate.Text = "";
                txtInvAmt.Text = "0.00";
                chkRecConf.Checked = false;
                txtReceived.Text = "0.00";
                txtRecRate.Text = "0.00";
                txtDriverName.Text = "";
                txtVehNo.Text = "";
                txtTourAdvance.Text = "0.00";
                chkPaxManually.Checked = false;
                chkIsCompleted.Checked = false;    
                grdOI.Rows.Count = 1;
                grdOI.Rows.Count = 100;
        }
        private void Fill_Control()
        {
            try
            { 
                drpAgent.Enabled = false;
                drpHandled.Enabled = false;
                drpAgentCurrency.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Currency FROM mst_Currency Where IsNull(IsActive,0)=1 ORDER BY ID");
                cmbRType.Items.Clear();
                cmbRType.Items.Add("Profit & Loss");
                cmbRType.Items.Add("Statement");
                cmbRType.SelectedIndex=0;
                drpAgent.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name as [AgentName] FROM mst_AgentDetails Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpHandled.DataSource =  Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS [HandledPerson] FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                cmbCompany.DataSource  = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
                DateTime today = DateTime.Today;
                if ((int)today.Month == 1)
                {
                    dtpFromDate.Value = new DateTime(today.Year, 12, 1);
                    dtpToDate.Value = new DateTime(today.Year, 12, 31);
                }
                else
                {
                    dtpFromDate.Value = new DateTime(today.Year, today.Month - 1, 1);
                    dtpToDate.Value = new DateTime(today.Year, today.Month - 1, DateTime.DaysInMonth(today.Year, today.Month - 1));
                }
            }
            catch (Exception ex)     {  db.MsgERR(ex);  }
        }
        private void Get_Details()
        {
            string ssql;
            DataTable DT;
            int RowNumb = 0,AgentID = 0;
            try
            {
                grdExp.Rows.Count = 1;
                grdExp.Rows.Count = 500;
                if (Classes.clsGlobal.Check_For_TourCompleteness(SystemCode.ToString().Trim()))
                {
                    tcPNL.Enabled = false;
                    chkIsCompleted.Enabled = false;
                    btnOk.Enabled = false;
                    btnPrint.Enabled = true;
                    btnCancel.Enabled = true;
                } 
                int Adult = 0, Child = 0;
                string GudieName = "", temp, temp1 = "";
                double HotelCom = 0.00;
                string date = "";
                ssql = "SELECT ISNULL(IsCancelled,0)AS IsCancelled,DateArrival,DateDeparture,NoOfAdult,NoOfChild,Guest,GuideName,HotelID,HotelCommission,"+
                       "ISNULL(ReceiveConfirm,0)AS Status,ISNULL(SetPaxManually,0)AS SetPaxManually" +
                       " FROM vw_act_Profit_Lose " +
                        "Where ID=" + SystemCode + "";// ORDER BY GuideName,HotelCommission";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                while (DT.Rows.Count > RowNumb)
                {
                    if (RowNumb == 0)
                    {
                        chkPaxManually.Checked = Convert.ToBoolean(DT.Rows[RowNumb]["SetPaxManually"]);
                        chkCancelled.Checked = Convert.ToBoolean(DT.Rows[RowNumb]["IsCancelled"]);
                        if (DT.Rows[RowNumb]["DateArrival"].ToString().Trim() == "" || DT.Rows[RowNumb]["DateDeparture"].ToString().Trim() == "")
                        {
                            MessageBox.Show("Arrival or Departure Dates Cannot be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        date = DT.Rows[RowNumb]["DateArrival"].ToString().Substring(0, 10) + " - " + DT.Rows[RowNumb]["DateDeparture"].ToString().Substring(0, 10);
                        txtDate.Text = date.Trim();
                        if (DT.Rows[RowNumb]["NoOfAdult"].ToString() != "")
                        {
                            txtAdult.Text = Convert.ToInt32(DT.Rows[RowNumb]["NoOfAdult"]).ToString();
                            Adult = Convert.ToInt32(DT.Rows[RowNumb]["NoOfAdult"]);
                        }
                        if (DT.Rows[RowNumb]["NoOfChild"].ToString() != "")
                        {
                            txtChild.Text = Convert.ToInt32(DT.Rows[RowNumb]["NoOfChild"]).ToString();
                            Child = Convert.ToInt32(DT.Rows[RowNumb]["NoOfChild"]);
                        }
                        txtPax.Text = (Adult + Child).ToString();
                        txtGuest.Text = DT.Rows[RowNumb]["Guest"].ToString();
                        string com,sql;
                        sql = "SELECT SUM(Commission)AS SumCom FROM trn_CityItinerary Where TransID=" + SystemCode + "";
                        com = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql).Rows[0]["SumCom"].ToString();
                        if (com != "")
                            HotelCom = Convert.ToDouble(com);
                        chkRecConf.Checked = Convert.ToBoolean(DT.Rows[RowNumb]["Status"]);
                    }
                    if (DT.Rows[RowNumb]["GuideName"].ToString() != "")
                    {
                        temp = DT.Rows[RowNumb]["GuideName"].ToString().Trim();
                        if (temp != temp1)
                        {
                            temp1 = temp;
                            if(DT.Rows.Count==RowNumb)  GudieName = GudieName + temp;    else   GudieName = GudieName + temp + ",";
                        }
                    }
                    RowNumb++;
                }
                txtHotelCom.Text = HotelCom.ToString().Trim(); 
                ssql = "SELECT CombineID FROM dbo.trn_GroupAmendment " + 
                       "WHERE ID=" + SystemCode +" ";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    txtCombineID.Text = DT.Rows[0]["CombineID"].ToString();
                    if (DT.Rows[0]["CombineID"].ToString() != "")
                    {
                        ssql = "SELECT ID,Guest FROM trn_GroupAmendment WHERE ID=" + Convert.ToDouble(txtCombineID.Text) + "";
                        DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                        if (DT.Rows.Count > 0)
                        {
                            lblCombineName.Text = DT.Rows[0]["Guest"].ToString().Trim();
                        }
                    }
                } 
                ssql = " SELECT PaidTo,PaidToName,Rate,CurrencyID,InvoiceNo,CreatedDate,Amount FROM vw_trn_act_PaymentIssued_AGENT Where TransID=" + SystemCode + " ORDER BY InvoiceNo";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                RowNumb = DT.Rows.Count-1;
                if (DT.Rows.Count > 0)
                {
                    if (DT.Rows[RowNumb]["PaidTo"].ToString() != "") AgentID = Convert.ToInt32(DT.Rows[RowNumb]["PaidTo"].ToString());
                    if (DT.Rows[RowNumb]["PaidToName"].ToString() != "") txtAgentName.Text = DT.Rows[RowNumb]["PaidToName"].ToString();
                    if (DT.Rows[RowNumb]["Rate"].ToString() != "") txtInvRate.Text = DT.Rows[RowNumb]["Rate"].ToString();
                    if (DT.Rows[RowNumb]["CurrencyID"].ToString() != "") drpAgentCurrency.setSelectedValue(DT.Rows[RowNumb]["CurrencyID"].ToString());
                    if (DT.Rows[RowNumb]["InvoiceNo"].ToString() != "") txtAgentInvNo.Text = DT.Rows[RowNumb]["InvoiceNo"].ToString();
                    if (DT.Rows[RowNumb]["CreatedDate"].ToString() != "") dtpInvDate.Value = Convert.ToDateTime(DT.Rows[RowNumb]["CreatedDate"]);
                    if (DT.Rows[RowNumb]["Amount"].ToString() != "") txtInvAmt.Text = DT.Rows[RowNumb]["Amount"].ToString();
                } 
                int driverid = 0;
                ssql = "SELECT DriverName,DriverID FROM vw_trn_DriverDetails Where TransID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                RowNumb = 0;
                if (DT.Rows.Count > 0)
                {
                    if (DT.Rows[RowNumb]["DriverName"].ToString() != "") txtDriverName.Text = DT.Rows[RowNumb]["DriverName"].ToString();
                    if (DT.Rows[RowNumb]["DriverID"].ToString() != "") driverid = Convert.ToInt32(DT.Rows[RowNumb]["DriverID"]);
                    string VehNo,sql;
                    sql = "SELECT VehicleNo FROM vw_TR_DriverVSVehicle Where ID=" + driverid + "";
                    VehNo = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql).Rows[0]["VehicleNo"].ToString();
                    if (VehNo != "") txtVehNo.Text = VehNo;
                    string Advance;
                    sql = "SELECT SUM(Amount) AS TotAdvance FROM vw_trn_Tour_Advance Where TransID=" + txtTourNo.Text.Trim() + " AND DriverID=" + driverid + "";
                    Advance = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql).Rows[0]["TotAdvance"].ToString();
                    if (Advance != "") txtTourAdvance.Text = Advance;
                } 
                ssql = "SELECT AgentCom,AgentRecAmt,AgentRecRate,IsNull(AsPercentage,0)AS AsPercentage,BankCharges,Excurtion,Extras,IsCompleted ,ISNULL(MaldivesAmt,0)AS MaldivesAmt,ISNULL(DirectPay,0)AS DirectPay FROM act_Profit_Lose Where TourID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                RowNumb = 0;
                if (DT.Rows.Count > 0)
                {
                    if (DT.Rows[RowNumb]["AgentCom"].ToString() != "") txtAgentCom.Text = DT.Rows[RowNumb]["AgentCom"].ToString();
                    if (DT.Rows[RowNumb]["AgentRecAmt"].ToString() != "") txtReceived.Text = DT.Rows[RowNumb]["AgentRecAmt"].ToString();
                    if (DT.Rows[RowNumb]["AgentRecRate"].ToString() != "") txtRecRate.Text = DT.Rows[RowNumb]["AgentRecRate"].ToString();
                    if (Convert.ToBoolean(DT.Rows[RowNumb]["AsPercentage"]) == true) rdbPercentage.Checked = true; else  rdbAmount.Checked = true;
                    if (DT.Rows[RowNumb]["BankCharges"].ToString() != "") txtBankChargers.Text = DT.Rows[RowNumb]["BankCharges"].ToString();
                    if (DT.Rows[RowNumb]["Excurtion"].ToString() != "") txtExcurtion.Text = DT.Rows[RowNumb]["Excurtion"].ToString();
                    if (DT.Rows[RowNumb]["Extras"].ToString() != "") txtExtras.Text = DT.Rows[RowNumb]["Extras"].ToString();
                    txtMaldive.Text = DT.Rows[RowNumb]["MaldivesAmt"].ToString();
                    txtDirectPay.Text = DT.Rows[RowNumb]["DirectPay"].ToString();
                    if (DT.Rows[RowNumb]["IsCompleted"].ToString() != "") chkIsCompleted.Checked = Convert.ToBoolean(DT.Rows[RowNumb]["IsCompleted"]);
                } 
                ssql = "SELECT ID,IncomeID,IncomeName,Amount,Remarks FROM vw_acc_PNL_OtherIncome WHERE TourID=" + SystemCode + " ORDER BY SrNo";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["ID"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gIID] = DT.Rows[RowNumb]["ID"].ToString();
                        if (DT.Rows[RowNumb]["IncomeID"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gEID] = DT.Rows[RowNumb]["IncomeID"].ToString();
                        if (DT.Rows[RowNumb]["IncomeName"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gEXN] = DT.Rows[RowNumb]["IncomeName"].ToString();
                        if (DT.Rows[RowNumb]["Amount"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gTOT] = DT.Rows[RowNumb]["Amount"].ToString();
                        if (DT.Rows[RowNumb]["Remarks"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gRMK] = DT.Rows[RowNumb]["Remarks"].ToString();
                        RowNumb++;
                    }
                } 
                double TotExp = 0.00;
                int TGirdRow = 1;
                double RoomCost = 0, GuideRoomCost = 0, ContRate = 0, GuideContRate = 0, RoomCount = 0, GuideRoomCount = 0, FOCRooms = 0, NoOfNights = 1, Commission = 0;
                double NoOfAdult = 0, NoOfChild = 0, NoOfGuide = 0, FOCAdult = 0, FOCChild = 0, AdultMealCost = 0, ChildMealCost = 0, GuideMealCost = 0, Ebed = 0, EbedCost = 0;
                double AllPaxTOT = 0, PaxTOT = 0, AllGuideTOT=0, GuideTOT = 0;
                string CUR="",GUIDECUR=""; 
                ssql = "SELECT TransID,DateIn,DateOut,VoucherID,Guest," +
                             "HandleBy,HotelID,HotelName,RoomTypeName,RoomBasisName,Occupancy,Cost,IsNull(ModifiedCost,0)AS ModifiedCost,GuideCost," +
                             "IsNull(ExtraBed,0)AS Ebed,IsNull(EbedCost,0)AS EbedCost," +
                             "IsNull(Commission,0)AS Commission,Advance,IsNull(ConRate,0)AS ConRate,IsNull(GuideConRate,0)AS GuideConRate," +
                             "IsNull(RoomCount,0)AS RoomCount,GuideRooms AS GuideRoomCount,IsNull(FOCRooms,0)AS FOCRooms,IsNull(Nights,1)AS Nights,MealFor," +
                             "IsNull(AdultMealCost,0)AS AdultMealCost,IsNull(ChildMealCost,0)AS ChildMealCost,IsNull(GuideMealCost,0)AS GuideMealCost," +
                             "IsNull(NoOfAdult,0) AS NoOfAdult,IsNull(NoOfChild,0) AS NoOfChild,IsNull(NoOfGuide,0) AS NoOfGuide," +
                             "IsNull(FOCAdult,0) AS FOCAdult,IsNull(FOCChild,0) AS FOCChild," +
                             "ISNULL(IsPaid,0)AS IsPaid,PaidDate,PaidBy,PartiallyPaid,CurCode,GuideCurCode,ISNULL(OtherAmt,0.00)AS OtherAmt,Remarks" +
                             " FROM vw_acc_PnL_HotelExpenses" +
                             " Where TransID=" + SystemCode + " ORDER BY VoucherID";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    C1.Win.C1FlexGrid.CellStyle rsHE = grdExp.Styles.Add("HE");
                    rsHE.BackColor = Color.PowderBlue;
                    grdExp.Rows[TGirdRow].Style = grdExp.Styles["HE"];
                    grdExp[TGirdRow, (int)EX.gEXN] = "HOTEL EXEPENSES";
                    TGirdRow++;
                    string CurVoucherNo = "",TempVoucherNo="";
                    bool GotOnce = false;
                    while (DT.Rows.Count > RowNumb)
                    {                           
                        if(RowNumb==0)  CurVoucherNo = DT.Rows[RowNumb]["VoucherID"] + "";
                        if (CurVoucherNo != TempVoucherNo)
                        {
                            GotOnce = false;
                            CurVoucherNo = DT.Rows[RowNumb]["VoucherID"] + "";
                            RoomCost = 0; GuideRoomCost = 0; ContRate = 0; GuideContRate = 0; RoomCount = 0; GuideRoomCount = 0; FOCRooms = 0; NoOfNights = 1; Commission = 0;
                            NoOfAdult = 0; NoOfChild = 0; NoOfGuide = 0; FOCAdult = 0; FOCChild = 0; AdultMealCost = 0; ChildMealCost = 0; GuideMealCost = 0; Ebed = 0; EbedCost = 0;
                            AllPaxTOT = 0; PaxTOT = 0; AllGuideTOT = 0;  GuideTOT = 0;// GrandTOT = 0;
                            CUR = ""; GUIDECUR = "";
                            if(RowNumb!=0) TGirdRow++;
                        }
                        if (DT.Rows[RowNumb]["HotelName"].ToString() != "") grdExp[TGirdRow, (int)EX.gEXN] = DT.Rows[RowNumb]["HotelName"].ToString();
                        if (DT.Rows[RowNumb]["ModifiedCost"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gRCS] = DT.Rows[RowNumb]["ModifiedCost"].ToString();
                            RoomCost = Convert.ToDouble(DT.Rows[RowNumb]["ModifiedCost"]);
                            if (RoomCost == 0)
                            {
                                grdExp[TGirdRow, (int)EX.gRCS] = DT.Rows[RowNumb]["Cost"].ToString();
                                RoomCost = Convert.ToDouble(DT.Rows[RowNumb]["Cost"]);
                            }
                        }    
                        if (DT.Rows[RowNumb]["RoomCount"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gRCT] = DT.Rows[RowNumb]["RoomCount"].ToString();
                            RoomCount = Convert.ToDouble(DT.Rows[RowNumb]["RoomCount"]);
                        }
                        if (DT.Rows[RowNumb]["ConRate"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gCRT] = DT.Rows[RowNumb]["ConRate"].ToString();
                            ContRate = Convert.ToDouble(DT.Rows[RowNumb]["ConRate"]);
                        }
                        if (DT.Rows[RowNumb]["GuideCost"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gGRC] = DT.Rows[RowNumb]["GuideCost"].ToString();
                            GuideRoomCost = Convert.ToDouble(DT.Rows[RowNumb]["GuideCost"]);
                        }
                        if (DT.Rows[RowNumb]["GuideRoomCount"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gGRO] = DT.Rows[RowNumb]["GuideRoomCount"].ToString();
                            GuideRoomCount = Convert.ToDouble(DT.Rows[RowNumb]["GuideRoomCount"]);
                        }
                        if (DT.Rows[RowNumb]["GuideConRate"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gGCR] = DT.Rows[RowNumb]["GuideConRate"].ToString();
                            GuideContRate = Convert.ToDouble(DT.Rows[RowNumb]["GuideConRate"]);
                        }
                        if (DT.Rows[RowNumb]["FOCRooms"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gFRM] = DT.Rows[RowNumb]["FOCRooms"].ToString();
                            FOCRooms = Convert.ToDouble(DT.Rows[RowNumb]["FOCRooms"]);
                        }
                        if (DT.Rows[RowNumb]["Nights"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gNGT] = DT.Rows[RowNumb]["Nights"].ToString();
                            NoOfNights = Convert.ToDouble(DT.Rows[RowNumb]["Nights"]);
                            if(NoOfNights.ToString().Trim()=="0")  NoOfNights=1;
                        }
                        if (DT.Rows[RowNumb]["Commission"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gCMS] = DT.Rows[RowNumb]["Commission"].ToString();
                            Commission = Convert.ToDouble(DT.Rows[RowNumb]["Commission"]);
                        }
                        if (DT.Rows[RowNumb]["NoOfAdult"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gNOA] = DT.Rows[RowNumb]["NoOfAdult"].ToString();
                            NoOfAdult = Convert.ToDouble(DT.Rows[RowNumb]["NoOfAdult"]);
                        }
                        if (DT.Rows[RowNumb]["NoOfChild"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gNOC] = DT.Rows[RowNumb]["NoOfChild"].ToString();
                            NoOfChild = Convert.ToDouble(DT.Rows[RowNumb]["NoOfChild"]);
                        }
                        if (DT.Rows[RowNumb]["NoOfGuide"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gNOG] = DT.Rows[RowNumb]["NoOfGuide"].ToString();
                            NoOfGuide = Convert.ToDouble(DT.Rows[RowNumb]["NoOfGuide"]);
                        }
                        if (DT.Rows[RowNumb]["FOCAdult"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gFOA] = DT.Rows[RowNumb]["FOCAdult"].ToString();
                            FOCAdult = Convert.ToDouble(DT.Rows[RowNumb]["FOCAdult"]);
                        }
                        if (DT.Rows[RowNumb]["FOCChild"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gFOC] = DT.Rows[RowNumb]["FOCChild"].ToString();
                            FOCChild = Convert.ToDouble(DT.Rows[RowNumb]["FOCChild"]);
                        }
                        if (DT.Rows[RowNumb]["AdultMealCost"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gAMC] = DT.Rows[RowNumb]["AdultMealCost"].ToString();
                            AdultMealCost = Convert.ToDouble(DT.Rows[RowNumb]["AdultMealCost"]);
                        }
                        if (DT.Rows[RowNumb]["ChildMealCost"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gCMC] = DT.Rows[RowNumb]["ChildMealCost"].ToString();
                            ChildMealCost = Convert.ToDouble(DT.Rows[RowNumb]["ChildMealCost"]);
                        }
                        if (DT.Rows[RowNumb]["GuideMealCost"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gGMC] = DT.Rows[RowNumb]["GuideMealCost"].ToString();
                            GuideMealCost = Convert.ToDouble(DT.Rows[RowNumb]["GuideMealCost"]);
                        }
                        if (DT.Rows[RowNumb]["Ebed"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gEBD] = DT.Rows[RowNumb]["Ebed"].ToString();
                            Ebed = Convert.ToDouble(DT.Rows[RowNumb]["Ebed"]);
                        }
                        if (DT.Rows[RowNumb]["EbedCost"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gEBC] = DT.Rows[RowNumb]["EbedCost"].ToString();
                            EbedCost = Convert.ToDouble(DT.Rows[RowNumb]["EbedCost"]);
                        }
                        if (DT.Rows[RowNumb]["CurCode"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gCUR] = DT.Rows[RowNumb]["CurCode"].ToString();
                            CUR = DT.Rows[RowNumb]["CurCode"].ToString().Trim();
                        }
                        if (DT.Rows[RowNumb]["GuideCurCode"].ToString() != "")
                        {
                            grdExp[TGirdRow, (int)EX.gGCU] = DT.Rows[RowNumb]["GuideCurCode"].ToString();
                            GUIDECUR=DT.Rows[RowNumb]["GuideCurCode"].ToString().Trim();
                        }
                        PaxTOT = ((RoomCost * (RoomCount - FOCRooms) * NoOfNights) +
                                 (Ebed * EbedCost * NoOfNights) +
                                 (AdultMealCost * (NoOfAdult - FOCAdult) * NoOfNights) +
                                 (ChildMealCost * (NoOfChild - FOCChild) * NoOfNights));
                        if (DT.Rows[RowNumb]["OtherAmt"].ToString().Trim() != "0.00")
                        {
                            if (GotOnce == false)
                            {
                                AllPaxTOT += Convert.ToDouble(DT.Rows[RowNumb]["OtherAmt"]);
                                GotOnce = true;
                            }
                        }
                        else if (CUR != "LKR")  AllPaxTOT += PaxTOT * ContRate;
                        else  AllPaxTOT += PaxTOT;
                        if (RowNumb == 0) AllPaxTOT = AllPaxTOT - Commission;
                        if (RowNumb != 0 && DT.Rows[RowNumb]["VoucherID"].ToString().Trim() != DT.Rows[RowNumb - 1]["VoucherID"].ToString().Trim()) AllPaxTOT = AllPaxTOT - Commission;
                        grdExp[TGirdRow, (int)EX.gAMT] = AllPaxTOT.ToString();
                        AllGuideTOT = ((GuideRoomCost * NoOfNights) + (GuideMealCost * NoOfGuide * NoOfNights));
                        if (GUIDECUR != "LKR") AllGuideTOT = AllGuideTOT * GuideContRate;
                        grdExp[TGirdRow, (int)EX.gGAM] = GuideTOT.ToString();
                        TotExp = AllPaxTOT + AllGuideTOT;
                        grdExp[TGirdRow, (int)EX.gTOT] = TotExp.ToString();
                        grdExp[TGirdRow, (int)EX.gPID] = Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]);
                        RowNumb++;
                        if(DT.Rows.Count>RowNumb) TempVoucherNo = DT.Rows[RowNumb]["VoucherID"] + "";
                    }
                } 
                double TotKm = 0.00,Rate=0.00,Bata=0.00,Nights=0.00;
                double TotBata=0.00,TotPaid=0.00;
                ssql = " SELECT DriverID,StartMeter,EndMeter,TotalKm,RatePerKm,Bata,NoOfNights, IsNull(IsChecked,0)AS IsChecked,IsNull(IsPaid,0)AS IsPaid FROM trn_BasicTransport Where TransID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    TGirdRow++;
                    C1.Win.C1FlexGrid.CellStyle rsTE = grdExp.Styles.Add("TE");
                    rsTE.BackColor = Color.PowderBlue;
                    grdExp.Rows[TGirdRow].Style = grdExp.Styles["TE"];
                    grdExp[TGirdRow, (int)EX.gEXN] = "TRANSPORT EXEPENSES";
                    TGirdRow++;
                    int count = 0;
                    while (DT.Rows.Count > count)
                    {
                        TotKm =  Rate =  Bata =  Nights = 0.00;
                        if (DT.Rows[count]["TotalKm"].ToString() != "") TotKm = Convert.ToDouble(DT.Rows[count]["TotalKm"].ToString());
                        if (DT.Rows[count]["RatePerKm"].ToString() != "") Rate = Convert.ToDouble(DT.Rows[count]["RatePerKm"].ToString());
                        if (DT.Rows[count]["Bata"].ToString() != "") Bata = Convert.ToDouble(DT.Rows[count]["Bata"].ToString());
                        if (DT.Rows[count]["NoOfNights"].ToString() != "") Nights = Convert.ToDouble(DT.Rows[count]["NoOfNights"].ToString());
                        TotPaid += TotKm * Rate;
                        count++;
                        if (Bata != 0) TotBata += Bata * Nights; 
                    }
                    grdExp[TGirdRow, (int)EX.gEXN] = "Transport";
                    grdExp[TGirdRow, (int)EX.gTOT] = TotPaid;
                    TotExp += TotPaid;
                    grdExp[TGirdRow, (int)EX.gPID] = Convert.ToBoolean(DT.Rows[0]["IsPaid"]);
                    TGirdRow++;
                    if (TotBata != 0)
                    {
                        grdExp[TGirdRow, (int)EX.gEXN] = "Bata";
                        grdExp[TGirdRow, (int)EX.gTOT] = TotBata;
                        TotExp += TotBata;
                        grdExp[TGirdRow, (int)EX.gPID] = Convert.ToBoolean(DT.Rows[0]["IsPaid"]);
                        TGirdRow++;
                    }
                } 
                ssql = "SELECT ISNULL(IsDriver,0)AS IsDriver,DriverID,ExpenseID,Expense,Amount, IsNull(IsPaid,0)AS IsPaid FROM vw_trn_Travel_Expenses WHERE TransID=" + txtTourNo.Text.Trim() + " ORDER BY SrNo";
                DataTable DTTravel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DTTravel.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DTTravel.Rows.Count > RowNumb)
                    {
                                if (DTTravel.Rows[RowNumb]["ExpenseID"].ToString().Trim() == "1001" || DTTravel.Rows[RowNumb]["ExpenseID"].ToString().Trim() == "1011")
                                {
                                    RowNumb++;
                                    continue;
                                }
                        grdExp[TGirdRow, (int)EX.gEXN] = DTTravel.Rows[RowNumb]["Expense"].ToString();
                        grdExp[TGirdRow, (int)EX.gTOT] = DTTravel.Rows[RowNumb]["Amount"].ToString();
                        grdExp[TGirdRow, (int)EX.gPID] = Convert.ToBoolean(DTTravel.Rows[RowNumb]["IsPaid"]);
                        TotExp += Convert.ToDouble(DTTravel.Rows[RowNumb]["Amount"].ToString());
                        TGirdRow++;
                        RowNumb++;
                    }
                } 
                ssql = " SELECT Name,IsNull(PaidAmount,0)AS PaidAmount,IsNull(IsPaid,0)AS IsPaid FROM vw_trn_GuideDetails Where TransID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                RowNumb = 0;
                while(DT.Rows.Count>RowNumb)
                {
                    TGirdRow++;
                    if (RowNumb == 0)
                    {
                        C1.Win.C1FlexGrid.CellStyle rsGE = grdExp.Styles.Add("GE");
                        rsGE.BackColor = Color.PowderBlue;
                        grdExp.Rows[TGirdRow].Style = grdExp.Styles["TE"];
                        grdExp[TGirdRow, (int)EX.gEXN] = "GUIDE EXEPENSES";
                        TGirdRow++;
                    }
                    grdExp[TGirdRow, (int)EX.gEXN] = DT.Rows[RowNumb]["Name"].ToString();
                    grdExp[TGirdRow, (int)EX.gTOT] = DT.Rows[RowNumb]["PaidAmount"].ToString();
                    TotExp += Convert.ToDouble(DT.Rows[RowNumb]["PaidAmount"].ToString());
                    grdExp[TGirdRow, (int)EX.gPID] = Convert.ToBoolean(DT.Rows[0]["IsPaid"]);
                    RowNumb++;
                } 
                ssql = " SELECT ExpenseName,Isnull(TotAmount,0)as TotAmount FROM trn_OtherExpenses Where TransID=" + SystemCode + " ORDER BY SrNo";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                RowNumb = 0;
                while (DT.Rows.Count > RowNumb)
                {
                    TGirdRow++;
                    if (RowNumb == 0)
                    {
                        C1.Win.C1FlexGrid.CellStyle rsOTE = grdExp.Styles.Add("OTE");
                        rsOTE.BackColor = Color.PowderBlue;
                        grdExp.Rows[TGirdRow].Style = grdExp.Styles["OTE"];
                        grdExp[TGirdRow, (int)EX.gEXN] = "OTHER EXEPENSES";
                        TGirdRow++;
                    }
                    grdExp[TGirdRow, (int)EX.gEXN] = DT.Rows[RowNumb]["ExpenseName"].ToString();
                    grdExp[TGirdRow, (int)EX.gTOT] = DT.Rows[RowNumb]["TotAmount"].ToString();
                    if (DT.Rows[0]["TotAmount"].ToString().Trim() != "")   TotExp += Convert.ToDouble(DT.Rows[RowNumb]["TotAmount"].ToString());
                    RowNumb++;
                } 
                double BankCh = 0.00;
                TGirdRow = TGirdRow+3;
                C1.Win.C1FlexGrid.CellStyle rsTT = grdExp.Styles.Add("TT");
                rsTT.BackColor = Color.Aqua;
                grdExp.Rows[TGirdRow].Style = grdExp.Styles["TT"];
                grdExp[TGirdRow, (int)EX.gEXN] = "TOTAL EXEPENSES";
                grdExp[TGirdRow, (int)EX.gTOT] = TotExp.ToString();
                lblTotExp.Text = TotExp.ToString();// +" LKR";
                if (txtBankChargers.Text.ToString().Trim() != "" && txtReceived.Text.ToString().Trim()!= "")
                {
                    if (rdbPercentage.Checked)   BankCh = Convert.ToDouble(txtReceived.Text.ToString().Trim()) - ((Convert.ToDouble(txtBankChargers.Text.ToString().Trim())) / 100);
                     else if (rdbAmount.Checked)   BankCh = Convert.ToDouble(txtReceived.Text.ToString().Trim()) - Convert.ToDouble(txtBankChargers.Text.ToString().Trim());
                     lblTotExp.Text = (TotExp + BankCh).ToString();// +" LKR";
                }
                TGirdRow++;
                grdExp.Rows.Count = TGirdRow; 
                ssql ="SELECT SUM(Gain)AS Gain,SUM(Lose)AS Lose FROM vw_act_GainLose WHERE AgentID="+AgentID+"";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                RowNumb = 0;
                if (DT.Rows.Count > 0)
                {
                    lblExtGain.Text = DT.Rows[RowNumb]["Gain"].ToString();
                    lblExtLose.Text = DT.Rows[RowNumb]["Lose"].ToString();
                } 
                btnGenerate_Click(null, null);
            }
            catch (Exception ex)        {  db.MsgERR(ex);    }
        } 
        public void Fill_Monthly_Statement()
        {
            try
            {
                lblInvalid.Invoke(new Action(() => lblInvalid.Visible = false));
                string format = "yyyy-MM-dd";
                string ssql = "", filter = "";
                DateTime datefrom = dtpFromDate.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpToDate.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                string NewArr = "2012-11-01";
                string compID = "";
                ssql = "SELECT ID,TourID,Guest,ISNULL(NoOfAdult,0)AS NoOfAdult,ISNULL(NoOfChild,0)AS NoOfChild," +
                       "DateArrival,DateDeparture,AgentID,AgentName,ISNULL(IsCancelled,0)as IsCancelled," +
                       "ISNULL(AgentRecAmt,0)AS AgentRecAmt,ISNULL(AgentRecRate,0)AS AgentRecRate," +
                       "ISNULL(AgentCom,0)AS AgentCom,ISNULL(BankCharges,0)AS BankCharges," +
                       "ISNULL(TourAdvance,0)AS TourAdvance,ISNULL(Excurtion,0)AS Excurtion,ISNULL(Extras,0)AS Extras," +
                       "HandledByID,HandledBy,ReceiveConfirm AS Status,Country," +
                       "ISNULL(MaldivesAmt,0)AS MaldivesAmt,ISNULL(DirectPay,0)AS DirectPay,ISNULL(IsCompleted,0)AS IsCompleted " +
                       "FROM vw_acc_PNL_Monthly_Statement " +
                       "WHERE DateDeparture>='" + DateFrom.Trim() + "' AND DateDeparture<='" + DateTo.Trim() + "' "+
                       "AND DateArrival>='" + NewArr.Trim() + "'"; // AND CompID=" + compID.Trim() + "";
                if (chkCmpny.Checked)
                {
                    cmbCompany.Invoke(new Action(() => compID = cmbCompany.SelectedValue.ToString()));
                    filter = "AND CompID=" + compID.Trim() + "";
                }
                if (chkAllAgent.Checked && !chkAllHandled.Checked)
                {
                    if (drpAgent.SelectedValue != null)
                    {
                        string AgentID ="";
                        drpAgent.Invoke(new Action(() => AgentID = drpAgent.SelectedValue.Trim()));
                        filter += " AND AgentID=" + AgentID + "";
                    }
                }
                else if (chkAllHandled.Checked && !chkAllAgent.Checked)
                {
                    if (drpHandled.SelectedValue != null)
                    {
                        string HandledID="";
                        drpHandled.Invoke(new Action(() => HandledID = drpHandled.SelectedValue.Trim()));
                        filter += " AND HandledByID=" + HandledID + "";
                    }
                }
                else if (chkAllHandled.Checked && chkAllAgent.Checked)
                {
                    if (drpHandled.SelectedValue != null || drpAgent.SelectedValue != null)
                    {
                        string AgentID = "";
                        drpAgent.Invoke(new Action(() => AgentID = drpAgent.SelectedValue.Trim()));
                        string HandledID = "";
                        drpHandled.Invoke(new Action(() => HandledID = drpHandled.SelectedValue.Trim()));
                        filter += " AND AgentID=" + AgentID + " AND HandledByID=" + HandledID + "";
                    }
                }
                if(rdbAll.Checked)    {   }
                else if(rdbClosed.Checked)  filter += " AND Isnull(IsCompleted,0)<>0 ";   
                else if (rdbUnclosed.Checked)      filter += " AND Isnull(IsCompleted,0)=0 ";  
                ssql = (ssql + filter + " ORDER BY DateDeparture").Trim();
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                int RowNumb = 0;
                DateTime Arrival, Depart;
                string ArrivalFrom, DaprtTo;
                int NoOfPax = 0;
                double PaidAmt = 0, TourAdvance = 0, BankCharg = 0, Excursion = 0, Extras = 0, MaldivesAmt = 0, DirectPay = 0, InvoiceAmt = 0, AgentCom = 0;
                double InvAmtU = 0, PaidAmtU = 0;
                double AgentRecRate = 0;
                double TotIncome = 0;
                double TotPax = 0;
                double TotProfit = 0, TotProfitU = 0;
                double TotInvoice = 0, TotInvoiceU = 0;
                double TotPaid = 0, TotPaidU = 0;
                double PorfitOrLoss = 0;
                grdStatement.Rows.Count = 1;
                grdStatement.Rows.Count = 5000;
                C1.Win.C1FlexGrid.CellStyle LOSS = grdStatement.Styles.Add("LOSS");
                LOSS.BackColor = Color.LightPink;
                C1.Win.C1FlexGrid.CellStyle CANCELL = grdStatement.Styles.Add("CANCELL");
                CANCELL.BackColor = Color.RosyBrown;
                C1.Win.C1FlexGrid.CellStyle TOT = grdStatement.Styles.Add("TOT");
                TOT.BackColor = Color.Aqua;
                grdStatement.Rows.Count = DT.Rows.Count + 3;
                while (DT.Rows.Count > RowNumb)
                {                   
                    double id = 0;
                    id = Convert.ToDouble(DT.Rows[RowNumb]["ID"]);
                    grdStatement[RowNumb + 1, (int)ST.gIDN] = DT.Rows[RowNumb]["ID"].ToString();
                    grdStatement[RowNumb + 1, (int)ST.gTID] = DT.Rows[RowNumb]["TourID"].ToString();
                    grdStatement[RowNumb + 1, (int)ST.gCNM] = DT.Rows[RowNumb]["Guest"].ToString();
                    grdStatement[RowNumb + 1, (int)ST.gCON] = DT.Rows[RowNumb]["Country"].ToString();
                    ArrivalFrom = "";
                    if (DT.Rows[RowNumb]["DateArrival"].ToString().Trim() != "")
                    {
                        Arrival = Convert.ToDateTime(DT.Rows[RowNumb]["DateArrival"].ToString());
                        ArrivalFrom = String.Format("{0:MMM dd}", Arrival);
                    }
                    DaprtTo = "";
                    if (DT.Rows[RowNumb]["DateDeparture"].ToString().Trim() != "")
                    {
                        Depart = Convert.ToDateTime(DT.Rows[RowNumb]["DateDeparture"].ToString());
                        DaprtTo = String.Format("{0:MMM dd}", Depart);
                    }
                    grdStatement[RowNumb + 1, (int)ST.gDTE] = ArrivalFrom + " / " + DaprtTo;
                    grdStatement[RowNumb + 1, (int)ST.gNOA] = DT.Rows[RowNumb]["NoOfAdult"].ToString();
                    grdStatement[RowNumb + 1, (int)ST.gNOC] = DT.Rows[RowNumb]["NoOfChild"].ToString();
                    NoOfPax = Convert.ToInt32(DT.Rows[RowNumb]["NoOfAdult"]) + Convert.ToInt32(DT.Rows[RowNumb]["NoOfChild"]);
                    grdStatement[RowNumb + 1, (int)ST.gNOP] = NoOfPax;
                    TotPax += NoOfPax;
                    grdStatement[RowNumb + 1, (int)ST.gAID] = DT.Rows[RowNumb]["AgentID"].ToString();
                    grdStatement[RowNumb + 1, (int)ST.gANM] = DT.Rows[RowNumb]["AgentName"].ToString();
                    InvoiceAmt = Get_Invoice_Amount(DT.Rows[RowNumb]["ID"].ToString().Trim());
                    AgentRecRate = Convert.ToDouble(DT.Rows[RowNumb]["AgentRecRate"]);
                    if (AgentRecRate <= 0)  InvAmtU = 0;  else    InvAmtU = InvoiceAmt / AgentRecRate;
                    grdStatement[RowNumb + 1, (int)ST.gIAMU] = InvAmtU;
                    grdStatement[RowNumb + 1, (int)ST.gIAMR] = InvoiceAmt;
                    TotInvoiceU += InvAmtU;
                    TotInvoice += InvoiceAmt;
                    grdStatement[RowNumb + 1, (int)ST.gSTS] = Convert.ToBoolean(DT.Rows[RowNumb]["IsCompleted"]) ? "Closed" : "Unclosed";
                    PaidAmtU = InvAmtU;
                    PaidAmt = InvoiceAmt;// *Convert.ToDouble(DT.Rows[RowNumb]["AgentRecRate"]);
                    grdStatement[RowNumb + 1, (int)ST.gPAMU] = PaidAmtU;// DT.Rows[RowNumb]["AgentRecAmt"].ToString().Trim();
                    grdStatement[RowNumb + 1, (int)ST.gPAMR] = PaidAmt;
                    TotPaidU += PaidAmtU;
                    TotPaid += PaidAmt;
                    TourAdvance = Convert.ToDouble(DT.Rows[RowNumb]["TourAdvance"]);
                    BankCharg = Convert.ToDouble(DT.Rows[RowNumb]["BankCharges"]);
                    Excursion = Convert.ToDouble(DT.Rows[RowNumb]["Excurtion"]);
                    Extras = Convert.ToDouble(DT.Rows[RowNumb]["Extras"]);
                    MaldivesAmt = Convert.ToDouble(DT.Rows[RowNumb]["MaldivesAmt"]);
                    DirectPay = Convert.ToDouble(DT.Rows[RowNumb]["DirectPay"]);
                    AgentCom = Convert.ToDouble(DT.Rows[RowNumb]["AgentCom"]);
                    string qry = "SELECT dbo.fun_getTotIncome(" + id.ToString().Trim() + ")Income";
                    TotIncome = Convert.ToDouble(Classes.clsGlobal.objCon.Fill_Table(qry).Rows[0]["Income"]);
                    PorfitOrLoss = Check_Profit_Loss(id.ToString().Trim(), TotIncome);
                    if (AgentRecRate != 0)
                    {
                        grdStatement[RowNumb + 1, (int)ST.gPOLU] = PorfitOrLoss / AgentRecRate;
                        TotProfitU = TotProfitU + (PorfitOrLoss / AgentRecRate);
                    }
                    grdStatement[RowNumb + 1, (int)ST.gPOLR] = PorfitOrLoss;
                    TotProfit += PorfitOrLoss;
                    if (grdStatement[RowNumb + 1, (int)ST.gPOLR] + "".ToString().Trim() != "")
                    {
                        if (Convert.ToDouble(grdStatement[RowNumb + 1, (int)ST.gPOLR]) < 0)
                            grdStatement.Rows[RowNumb + 1].Style = LOSS;
                    }
                    if (Convert.ToBoolean(DT.Rows[RowNumb]["IsCancelled"]))
                    {
                        grdStatement.Rows[RowNumb + 1].Style = CANCELL;
                        grdStatement[RowNumb + 1, (int)ST.gCAN] = 1;
                    }
                    grdStatement[RowNumb + 1, (int)ST.gHID] = DT.Rows[RowNumb]["HandledByID"].ToString();
                    grdStatement[RowNumb + 1, (int)ST.gHNM] = DT.Rows[RowNumb]["HandledBy"].ToString();
                    RowNumb++;
                }
                grdStatement[RowNumb + 2, (int)ST.gNOP] = TotPax.ToString();
                grdStatement[RowNumb + 2, (int)ST.gPOLU] = TotProfitU;
                grdStatement[RowNumb + 2, (int)ST.gPOLR] = TotProfit.ToString();
                grdStatement[RowNumb + 2, (int)ST.gIAMU] = TotInvoiceU;
                grdStatement[RowNumb + 2, (int)ST.gIAMR] = TotInvoice.ToString();
                grdStatement[RowNumb + 2, (int)ST.gPAMU] = TotPaidU;
                grdStatement[RowNumb + 2, (int)ST.gPAMR] = TotPaid.ToString();
                grdStatement.Rows[RowNumb + 2].Style = TOT;
                grdStatement.Rows.Count = RowNumb + 3;
            }            
            catch (System.InvalidOperationException)   { Fill_Monthly_Statement();  }
            catch (Exception ex)  {   db.MsgERR(ex);    }
        }
        private double Get_Invoice_Amount(string TourID)
        { 
                string ssql = "SELECT dbo.fun_getTotIncome(" + TourID.Trim() + ")Amount";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                double Amt=0; //Rate,Tot=0;
                if (DT.Rows.Count > 0)
                {
                    Amt = Convert.ToDouble(DT.Rows[0]["Amount"]);
                }
                return Amt; 
        }
        private double Check_Profit_Loss(string TourID, double Income)
        { 
                double Exp = 0.00;
                string qry = "SELECT dbo.fun_getTotExpenses(" + TourID.ToString().Trim() + ")Expense";
                Exp = Convert.ToDouble(Classes.clsGlobal.objCon.Fill_Table(qry).Rows[0]["Expense"]);
                return Income - Exp; 
        }
        private void Intializer()
        {
            try
            {
                chkPrint.Checked = true;
                Grd_Initializer();
                Fill_Control();
            }
            catch (Exception ex)   {  db.MsgERR(ex);   }
        }
        private void Grd_Initializer()
        {
            try
            {  
                db.GridInit(grdOI, 100, OI.gIID, 0, "Income ID", OI.gEID, 0, "OI ID", OI.gEXN, 250, "Income Name", true, OI.gTOT, 100, "Total", "##.#", OI.gRMK, 300, "Remarks"); 
                grdOI.Rows[1].AllowEditing = true; 
                db.GridInit(grdExp, 200, EX.gEXN, 632, "Expense Name", EX.gRCS, 0, "Room Cost", EX.gRCT, 0, "Room Count", EX.gCRT, 0, "ContRate", EX.gGRC, 0, "Guide Room Cost", EX.gGRO, 0, "Guide Room Count", EX.gGCR, 0, "Guide Cont Rate", EX.gFRM, 0, "FOC Rooms", EX.gNGT, 0, "Nights", EX.gCMS, 0, "Commission", EX.gNOA, 0, "#Adult", EX.gNOC, 0, "#Child", EX.gNOG, 0, "#Guide", EX.gFOA, 0, "FOC Adult", EX.gFOC, 0, "FOC Child", EX.gAMC, 0, "Adult Meal", EX.gCMC, 0, "Child Meal", EX.gGMC, 0, "Guide Meal", EX.gEBD, 0, "Ebed", EX.gEBC, 0, "Ebed Cost", EX.gCUR, 0, "CUR", EX.gGCU, 0, "GUIDE CUR", EX.gAMT, 0, "Amount", EX.gGAM, 0, "Guide Amt", EX.gTOT, 120, "Total", "##.#", EX.gVAT, 50, "VAT %", "##.#", EX.gVAMT, 100, "VAT Amt", "##.#", EX.gGTOT, 120, "Gross Total", "##.#", EX.gPID, 80, "Paid", Type.GetType(" System.Boolean"), EX.gRMK, 0, "Remarks"); 
                grdExp.Rows[0].AllowEditing = true;
                grdExp.Cols[(int)EX.gVAT].AllowEditing = true; 
                db.GridInit(grdStatement, 5000, ST.gIDN, 0, "ID", ST.gTID, 80, "Tour ID", ST.gCNM, 214, "Client Name", ST.gAID, 0, "Agent ID", ST.gANM, 150, "Agent Name", ST.gCAN, 0, "IsCancelled", Type.GetType("System.Boolean"), ST.gCON, 100, "Country", ST.gDTE, 110, "Date", ST.gNOA, 0, "#Adults", ST.gNOC, 0, "#Childs", ST.gNOP, 50, "Pax", ST.gPOLU, 120, "Profit / Loss ($)", "##.##", ST.gPOLR, 120, "Profit / Loss (LKR)", "##.##", ST.gIAMU, 120, "Invoice Amount ($)", "##.##", ST.gIAMR, 120, "Invoice Amount(LKR)", "##.##", ST.gSTS, 60, "Status", ST.gPAMU, 120, "Paid Amount ($)", "##.##", ST.gPAMR, 120, "Paid Amount(LKR)", "##.##", ST.gHID, 0, "Hanlded By ID", ST.gHNM, 100, "Hanlded By"); 
                grdStatement.Rows[1].AllowEditing = false;
             }
            catch (Exception ex)            { db.MsgERR(ex);            }
        }
        private void Generate_Tot_Income()
        {
            try
            {  
                int RowNumb;
                double TotIncome = 0.00; 
               string ssql = "SELECT ID,IncomeID,IncomeName,Amount,Remarks FROM vw_acc_PNL_OtherIncome WHERE TourID=" + SystemCode + " ORDER BY SrNo ";
              DataTable  DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["ID"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gIID] = DT.Rows[RowNumb]["ID"].ToString();
                        if (DT.Rows[RowNumb]["IncomeID"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gEID] = DT.Rows[RowNumb]["IncomeID"].ToString();
                        if (DT.Rows[RowNumb]["IncomeName"].ToString() != "") grdOI[RowNumb + 1, (int)OI.gEXN] = DT.Rows[RowNumb]["IncomeName"].ToString();
                        if (DT.Rows[RowNumb]["Amount"].ToString() != "")
                        {
                            grdOI[RowNumb + 1, (int)OI.gTOT] = DT.Rows[RowNumb]["Amount"].ToString();
                            TotIncome += Convert.ToDouble(DT.Rows[RowNumb]["Amount"].ToString());
                        }
                        if (DT.Rows[RowNumb]["Remarks"].ToString() != "")  grdOI[RowNumb + 1, (int)OI.gRMK] = DT.Rows[RowNumb]["Remarks"].ToString();
                        RowNumb++;
                    }
                } 
                if (txtReceived.Text.ToString().Trim() != "") TotIncome += Convert.ToDouble(txtReceived.Text.ToString().Trim());
                if (txtAgentCom.Text.ToString().Trim() != "") TotIncome -= Convert.ToDouble(txtAgentCom.Text.ToString().Trim()); 
                if (txtExcurtion.Text.ToString().Trim() != "") TotIncome += Convert.ToDouble(txtExcurtion.Text.ToString().Trim());
                if (txtExtras.Text.ToString().Trim() != "") TotIncome += Convert.ToDouble(txtExtras.Text.ToString().Trim()); 
                lblTotIncome.Text = TotIncome.ToString();
            }
            catch (Exception ex)      {  db.MsgERR(ex);      }
        }
        private void rdbPercentage_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbPercentage.Checked)
            {
                txtBankChargers.Text = "";
                lblBCname.Text = "Percentage";
                lblBCmark.Text = "%";
            }
            else
            {
                txtBankChargers.Text = "";
                lblBCname.Text = "Amount";
                lblBCmark.Text = "LKR";
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)  return; 
            if (Save_Procedure()  == true)  if (chkPrint.Checked) Print_Report(); 
            btnGenerate_Click(null, null);
        }
        private void btnCancel_Click(object sender, EventArgs e)  {  this.Close();   } 
        private Boolean Save_Procedure()
        { 
                    System.Data.SqlClient.SqlCommand     objCom = new System.Data.SqlClient.SqlCommand();
                System.Data.SqlClient.SqlConnection   objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                    System.Data.SqlClient.SqlTransaction   objTrn = objCon.BeginTransaction();
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
        private Boolean Save_Tabs(System.Data.SqlClient.SqlCommand sqlCom)   {      return Save_Tour_Income(sqlCom) && Save_Tour_Expenses(sqlCom) && Save_Tour_Other_Income(sqlCom)  && Save_Extended_Gain_loss(sqlCom)  ;       }
        private Boolean Save_Tour_Income(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = true; 
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_act_TourIncome";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                if (txtAgentCom.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@AgentCom", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAgentCom.Text.ToString().Trim());
                if (txtReceived.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@AgentRecAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(txtReceived.Text.ToString().Trim());
                if (txtRecRate.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@AgentRecRate", SqlDbType.Decimal).Value = Convert.ToDecimal(txtRecRate.Text.ToString().Trim());
                if (txtInvRate.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@AgentInvRate", SqlDbType.Decimal).Value = Convert.ToDecimal(txtInvRate.Text.ToString().Trim());
                if (txtBankChargers.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@BankCharges", SqlDbType.Decimal).Value = Convert.ToDecimal(txtBankChargers.Text.ToString().Trim());
                sqlCom.Parameters.Add("@AsPercentage", SqlDbType.Int).Value = rdbPercentage.Checked ? "1" : "0";
                if (txtExcurtion.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@Excurtion", SqlDbType.Decimal).Value = Convert.ToDecimal(txtExcurtion.Text.ToString().Trim());
                if (txtExtras.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@Extras", SqlDbType.Decimal).Value = Convert.ToDecimal(txtExtras.Text.ToString().Trim());
                if (txtMaldive.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@MaldivesAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(txtMaldive.Text.ToString().Trim());
                if (txtDirectPay.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@DirectPay", SqlDbType.Decimal).Value = Convert.ToDecimal(txtDirectPay.Text.ToString().Trim());
                if (chkRecConf.Checked)
                {
                    sqlCom.Parameters.Add("@ReceiveConfirm", SqlDbType.Int).Value = 1;
                    sqlCom.Parameters.Add("@ReceiveDate", SqlDbType.DateTime).Value = dtpRecDate.Value;
                    sqlCom.Parameters.Add("@RecConfirmBy", SqlDbType.Int).Value = Classes.clsGlobal.UserID;
                }
                else
                    sqlCom.Parameters.Add("@ReceiveConfirm", SqlDbType.Int).Value = 0;
                sqlCom.Parameters.Add("@SetPaxManually", SqlDbType.Int).Value = chkPaxManually.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@IsCompleted", SqlDbType.Int).Value = chkIsCompleted.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = chkCancelled.Checked ? "1" : "0";
                if (txtCombineID.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@CombineID", SqlDbType.Decimal).Value = Convert.ToDecimal(txtCombineID.Text.ToString().Trim());
                if (txtAdult.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@NoOfAdult", SqlDbType.Int).Value = Convert.ToInt32(txtAdult.Text.ToString());
                if (txtChild.Text.ToString().Trim() != "") sqlCom.Parameters.Add("@NoOfChild", SqlDbType.Int).Value = Convert.ToInt32(txtChild.Text.ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)    RtnVal = false; 
                return RtnVal; 
        }
        private Boolean Save_Tour_Expenses(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true; 
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_PNL_Expenses";
                RowNumb = 1;
                while (grdExp.Rows.Count-1>RowNumb)
                {
                    sqlCom.Parameters.Clear();
                    if (grdExp[RowNumb, (int)EX.gTOT] == null || grdExp[RowNumb, (int)EX.gTOT].ToString() == "")
                    {
                        RowNumb++;
                        continue;
                    }
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdExp[RowNumb, (int)EX.gEXN] != null && grdExp[RowNumb, (int)EX.gEXN].ToString() != "") sqlCom.Parameters.Add("@Expense", SqlDbType.NVarChar, 200).Value = grdExp[RowNumb, (int)EX.gEXN].ToString();
                    if (grdExp[RowNumb, (int)EX.gTOT] != null && grdExp[RowNumb, (int)EX.gTOT].ToString() != "") sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDouble(grdExp[RowNumb, (int)EX.gTOT].ToString());
                    else
                        sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = 0;
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdExp[RowNumb, (int)EX.gPID]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)   RtnVal = false; 
                    RowNumb++;
                }
                return RtnVal; 
        }
        private Boolean Save_Tour_Other_Income(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true; 
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Tour_OtherIncome";
                RowNumb = 1;
                while (grdOI[RowNumb, grdOI.Cols[(int)OI.gEXN].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    int id;
                    if (grdOI[RowNumb, (int)OI.gIID] != null)
                    {
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(grdOI[RowNumb, (int)OI.gIID]);
                        id = Convert.ToInt32(grdOI[RowNumb, (int)OI.gIID]);
                    }
                    else
                    {
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                        id = 0;
                    }
                    sqlCom.Parameters.Add("@TourID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdOI[RowNumb, (int)OI.gEXN] != null && grdOI[RowNumb, (int)OI.gEXN].ToString() != "")  sqlCom.Parameters.Add("@IncomeID", SqlDbType.Int).Value = Convert.ToInt32(grdOI[RowNumb, (int)OI.gEID].ToString());
                    else    continue;
                    if (grdOI[RowNumb, (int)OI.gTOT] != null && grdOI[RowNumb, (int)OI.gTOT].ToString() != "") sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdOI[RowNumb, (int)OI.gTOT].ToString());
                    if (grdOI[RowNumb, (int)OI.gRMK] != null && grdOI[RowNumb, (int)OI.gRMK].ToString() != "") sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 200).Value = grdOI[RowNumb, (int)OI.gRMK].ToString();
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)    RtnVal = false; 
                    RowNumb++;
                }
                return RtnVal; 
        }
        private Boolean Save_Extended_Gain_loss(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = true; 
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Extended_Gain_Lose";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                if (IsGain)   sqlCom.Parameters.Add("@Gain", SqlDbType.Decimal).Value = extend;
                else  sqlCom.Parameters.Add("@Lose", SqlDbType.Decimal).Value = extend;
                 sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                 sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                 sqlCom.ExecuteNonQuery();
                 if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)   RtnVal = false; 
                return RtnVal;
        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            double totIN = 0.00, totEX = 0.00,tot=0.00;
            Generate_Tot_Income();
            string toti= lblTotIncome.Text.ToString().Trim();//.Substring(0, lblTotIncome.Text.ToString().Trim().Length - 4);
            if (toti != "")
            {
                totIN = Convert.ToDouble(toti) * (Convert.ToDouble(txtRecRate.Text.ToString().Trim()));
                lblTotIncome.Text = totIN.ToString();
            }
            string totie = lblTotExp.Text.ToString().Trim();//.Substring(0, lblTotExp.Text.ToString().Trim().Length - 4);
            if (toti != "") totEX = Convert.ToDouble(totie); 
            tot = totIN - totEX;
            lblTotPay.Text = tot.ToString();// +" LKR";
            double ProfPercen = 0.00;
            double IOE = 0.00;
            if (totIN >= totEX)
            {
                if (lblTotPay.Text.ToString().Trim() == "")                    return;
                IOE = Convert.ToDouble(lblTotPay.Text.ToString().Trim());
                ProfPercen =Math.Round((IOE / totIN) * 100,2);
                lblNmProfPerPax.Text = "Profit Per Pax (LKR)";
                lblNmProfPerPax.ForeColor = Color.Blue;
                lblProfPax.ForeColor = Color.Blue;
                lblDot1.ForeColor = Color.Blue;
                lblNmProfit.Text = "Profit %";
                lblNmProfit.ForeColor = Color.Blue;
                lblProfit.ForeColor = Color.Blue;
                lblDot2.ForeColor = Color.Blue;
            }
            else
            {
                if (lblTotPay.Text.ToString().Trim() == "")                    return;
                IOE = Convert.ToDouble(lblTotPay.Text.ToString().Trim());
                ProfPercen =Math.Round(Math.Abs((IOE / totIN) * 100),2);
                lblNmProfPerPax.Text = "Loss Per Pax (LKR)";
                lblNmProfPerPax.ForeColor = Color.Red;
                lblProfPax.ForeColor = Color.Red;
                lblDot1.ForeColor = Color.Red;
                lblNmProfit.Text = "loss %";
                lblNmProfit.ForeColor = Color.Red;
                lblProfit.ForeColor = Color.Red;
                lblDot2.ForeColor = Color.Red;
            }
            lblProfit.Text = ProfPercen.ToString();// +" %";
            if (txtPax.Text.ToString().Trim() != "" && lblTotPay.Text.ToString().Trim() != "")
            {
                double nopax = Convert.ToDouble(txtPax.Text.ToString().Trim());
                double totp = Convert.ToDouble(lblTotPay.Text.ToString().Trim());
                lblProfPax.Text =Math.Abs(Math.Round((totp / nopax),2)).ToString();
            } 
            if (txtInvRate.Text.ToString().Trim() != "" && txtRecRate.Text.ToString().Trim() != "" && txtInvAmt.Text.ToString().Trim() != "" && txtReceived.Text.ToString().Trim() != "")
            {
                double InvAmt = Convert.ToDouble(txtInvAmt.Text.ToString().Trim());
                double RecAmt = Convert.ToDouble(txtReceived.Text.ToString().Trim());
                double Commsn = Convert.ToDouble(txtAgentCom.Text.ToString().Trim());
                double InvRate=Convert.ToDouble(txtInvRate.Text.ToString().Trim());
                double RecRate=Convert.ToDouble(txtRecRate.Text.ToString().Trim());
                if (InvRate > RecRate)
                {
                    extend = ((InvAmt * InvRate) - (InvAmt * RecRate)+Commsn);
                    IsGain = false;
                    lblCurExtend.Text = "This Transaction is Extended Loss of "+extend+" LKR.";
                    lblCurExtend.ForeColor = Color.Red;
                }
                else if (InvRate < RecRate)
                {
                    extend = ((InvAmt * RecRate) - (InvAmt * InvRate) - Commsn);
                    IsGain = true;
                    lblCurExtend.Text = "This Transaction is Extended Gain of " + extend + " LKR.";
                    lblCurExtend.ForeColor = Color.Green;
                }
                else
                {
                    IsGain = false;
                    lblCurExtend.Text = "This Transaction is Not an Extended Gain/Loss";
                    lblCurExtend.ForeColor = Color.Black;
                }
            }
            else  lblCurExtend.Text = "This Transaction is Extended ... of 0.00 LKR."; 
            Fill_Report_Combo(); 
        }
        private void Fill_Report_Combo()
        {
            try
            {
                if (txtInvAmt.Text.ToString().Trim() == "" || txtReceived.Text.ToString().Trim() == "")       return;
                cmbRType.Items.Clear();
                cmbRType.Items.Add("Profit & Loss");
                cmbRType.Items.Add("Statement");
                cmbRType.SelectedIndex = 0;
                double InvAmt=Convert.ToDouble(txtInvAmt.Text.ToString().Trim());
                double RecAmt = Convert.ToDouble(txtReceived.Text.ToString().Trim());
                if(txtAgentCom.Text!="")   RecAmt+=Convert.ToDouble(txtAgentCom.Text);
                if (RecAmt > InvAmt) cmbRType.Items.Add("Credit Note"); 
                else if (RecAmt < InvAmt)   cmbRType.Items.Add("Debit Note"); 
            }
            catch (Exception ex)     { db.MsgERR(ex);   }
        }
        private void btnPrint_Click(object sender, EventArgs e)        {            Print_Report();        }
        private void Print_Report()
        {
            try
            {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                if (txtTourNo.Text.ToString().Trim() == "" && cmbRType.SelectedItem.ToString() != "Statement")                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (cmbRType.Text.ToString().Trim() == "Profit & Loss")                    db.showReport(new Tourist_Management.Reports.ProfitNloss(), Convert.ToDecimal(txtTourNo.Text), Convert.ToDecimal(txtTourNo.Text), Convert.ToInt32(Classes.clsGlobal.UserID.ToString()));
                else if (cmbRType.Text.ToString().Trim() == "Statement")
                {
                    DataSet DTG = new DataSets.ds_acc_MonthlyStatement();
                    ReportDocument ga = new Tourist_Management.Reports.PNL_MonthlyStatement();
                    SqlParameter[] par = new SqlParameter[] { new SqlParameter("comp", chkCmpny.Checked) };
                    DataTable Dt = Get_MonthlyStatement_DataTable();
                    if (Dt.Rows.Count > 0) sConnection.Print_Via_Datatable(DTG, Dt, ga, ""); else MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (cmbRType.Text.ToString().Trim() == "Credit Note")                    db.showReport(new Tourist_Management.Reports.CreditNote(), Convert.ToDecimal(txtTourNo.Text), Convert.ToInt32(Classes.clsGlobal.UserID.ToString()));
                else if (cmbRType.Text.ToString().Trim() == "Debit Note")                    db.showReport(new Tourist_Management.Reports.DebitNote(), Convert.ToDecimal(txtTourNo.Text), Classes.clsGlobal.UserID);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private DataTable Get_MonthlyStatement_DataTable()
        { 
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                int RowNumb = 1;
                DataTable table = new DataTable();
                table.Columns.Add("ID", typeof(string));
                table.Columns.Add("TransID", typeof(string));
                table.Columns.Add("Guest", typeof(string));
                table.Columns.Add("IsCancelled", typeof(bool));
                table.Columns.Add("Country", typeof(string));
                table.Columns.Add("Date", typeof(string));
                table.Columns.Add("Pax", typeof(int));
                table.Columns.Add("Agent", typeof(string));
                table.Columns.Add("InvoiceAmt", typeof(double));
                table.Columns.Add("InvoiceAmtU", typeof(double));
                table.Columns.Add("Status", typeof(bool));
                table.Columns.Add("ReceivedAmt", typeof(double));
                table.Columns.Add("ReceivedAmtU", typeof(double));
                table.Columns.Add("Profit", typeof(double));
                table.Columns.Add("ProfitU", typeof(double));
                table.Columns.Add("HandledBy", typeof(string));
                table.Columns.Add("CompID", typeof(int));
                table.Columns.Add("DisplayName", typeof(string));
                table.Columns.Add("Telephone", typeof(string));
                table.Columns.Add("Fax", typeof(string));
                table.Columns.Add("E_Mail", typeof(string));
                table.Columns.Add("Web", typeof(string));
                table.Columns.Add("Physical_Address", typeof(string));
                table.Columns.Add("Company_Logo", typeof(byte[]));
                table.Columns.Add("DateFrom", typeof(DateTime));
                table.Columns.Add("DateTo", typeof(DateTime));
                string id, transid, guest, country, date, agent, handled;
                int pax;
                double invamt, invamtU, recamt, recamtU, profit, profitU;
                bool status,IsCancelled;
                int compID=0;
                string displayName="", Address="", tel="", fax="", email="", web="" ;
                byte[] comLogo=null;
                DateTime dateFrom, dateTo;
                while (grdStatement[RowNumb, (int)ST.gTID] != null)
                {
                    id = grdStatement[RowNumb, (int)ST.gIDN].ToString();
                    transid = grdStatement[RowNumb, (int)ST.gTID].ToString();
                    guest = grdStatement[RowNumb, (int)ST.gCNM].ToString();
                    IsCancelled = Convert.ToBoolean(grdStatement[RowNumb, (int)ST.gCAN]);
                    country = grdStatement[RowNumb, (int)ST.gCON].ToString();
                    date = grdStatement[RowNumb, (int)ST.gDTE].ToString();
                    agent = grdStatement[RowNumb, (int)ST.gANM].ToString();
                    handled = grdStatement[RowNumb, (int)ST.gHNM].ToString();
                    pax = Convert.ToInt32(grdStatement[RowNumb, (int)ST.gNOP]);
                    if (grdStatement[RowNumb, (int)ST.gSTS] == null)
                        status = false;
                    else
                    { 
                            status = (grdStatement[RowNumb, (int)ST.gSTS].ToString().Trim() == "Closed");
                    }
                    invamt = Convert.ToDouble(grdStatement[RowNumb, (int)ST.gIAMR].ToString());
                    invamtU = Convert.ToDouble(grdStatement[RowNumb, (int)ST.gIAMU].ToString());
                    recamt = Convert.ToDouble(grdStatement[RowNumb, (int)ST.gPAMR].ToString());
                    recamtU = Convert.ToDouble(grdStatement[RowNumb, (int)ST.gPAMU].ToString());
                    if (grdStatement[RowNumb, (int)ST.gPOLR] != null)
                    {
                        profit = Convert.ToDouble(grdStatement[RowNumb, (int)ST.gPOLR]);
                        profitU = Convert.ToDouble(grdStatement[RowNumb, (int)ST.gPOLU]);
                    }
                    else
                    {
                        profit = 0.00;
                        profitU = 0.00;
                    }
                    if (RowNumb == 1) 
                    {
                        if (chkCmpny.Checked)
                        {
                            DataTable dt = Classes.clsGlobal.getCompanyDetails(Convert.ToInt32(cmbCompany.SelectedValue));
                            DataTable user = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("select UserGroupID from vw_CurrentUserDetails where UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()));
                            displayName = dt.Rows[0]["DisplayName"].ToString().Trim();
                            compID = Convert.ToInt32(dt.Rows[0]["ID"]);
                            Address = dt.Rows[0]["Physical_Address"].ToString().Trim();
                            tel = dt.Rows[0]["Telephone"].ToString().Trim();
                            fax = dt.Rows[0]["Fax"].ToString().Trim();
                            if (Convert.ToInt32(user.Rows[0]["UserGroupID"]) == 1008)
                                email = dt.Rows[0]["E_MailTo"].ToString().Trim();
                            else
                                email = dt.Rows[0]["E_Mail"].ToString().Trim();
                            web = dt.Rows[0]["Web"].ToString().Trim();
                            comLogo = (byte[])dt.Rows[0]["Company_Logo"];
                        }
                            dateFrom = dtpFromDate.Value;
                            dateTo = dtpToDate.Value;
                            table.Rows.Add(id, transid, guest, IsCancelled, country, date, pax, agent, invamt, invamtU, status, recamt, recamtU, profit, profitU, handled, compID, displayName, tel, fax, email, web, Address, comLogo, dateFrom, dateTo);
                    }
                    else
                    {
                        table.Rows.Add(id, transid, guest, IsCancelled, country, date, pax, agent, invamt, invamtU, status, recamt, recamtU, profit, profitU, handled);
                    }
                    RowNumb++;
                }
                return table; 
        }
        private void chkRemarks_CheckedChanged(object sender, EventArgs e)  {  grdExp.Cols[(int)EX.gRMK].Width =chkRemarks.Checked?400: 0;    }
        private void chkAdvance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdvance.Checked)
            {
                grdExp.Cols[(int)EX.gRCS].Width = 80;
                grdExp.Cols[(int)EX.gRCT].Width = 80;
                grdExp.Cols[(int)EX.gCRT].Width = 80;
                grdExp.Cols[(int)EX.gGRC].Width = 80;
                grdExp.Cols[(int)EX.gGRO].Width = 80;
                grdExp.Cols[(int)EX.gGCR].Width = 110;
                grdExp.Cols[(int)EX.gFRM].Width = 80;
                grdExp.Cols[(int)EX.gNGT].Width = 80;
                grdExp.Cols[(int)EX.gCMS].Width = 80;
                grdExp.Cols[(int)EX.gNOA].Width = 80;
                grdExp.Cols[(int)EX.gNOC].Width = 80;
                grdExp.Cols[(int)EX.gNOG].Width = 80;
                grdExp.Cols[(int)EX.gFOA].Width = 80;
                grdExp.Cols[(int)EX.gFOC].Width = 80;
                grdExp.Cols[(int)EX.gAMC].Width = 80;
                grdExp.Cols[(int)EX.gCMC].Width = 80;
                grdExp.Cols[(int)EX.gGMC].Width = 80;
                grdExp.Cols[(int)EX.gEBD].Width = 80;
                grdExp.Cols[(int)EX.gEBC].Width = 80;
                grdExp.Cols[(int)EX.gCUR].Width = 80;
                grdExp.Cols[(int)EX.gGCU].Width = 80;
                grdExp.Cols[(int)EX.gAMT].Width = 80;
                grdExp.Cols[(int)EX.gGAM].Width = 80;
            }
            else
            {
                grdExp.Cols[(int)EX.gRCS].Width = 0;
                grdExp.Cols[(int)EX.gRCT].Width = 0;
                grdExp.Cols[(int)EX.gCRT].Width = 0;
                grdExp.Cols[(int)EX.gGRC].Width = 0;
                grdExp.Cols[(int)EX.gGRO].Width = 0;
                grdExp.Cols[(int)EX.gGCR].Width = 0;
                grdExp.Cols[(int)EX.gFRM].Width = 0;
                grdExp.Cols[(int)EX.gNGT].Width = 0;
                grdExp.Cols[(int)EX.gCMS].Width = 0;
                grdExp.Cols[(int)EX.gNOA].Width = 0;
                grdExp.Cols[(int)EX.gNOC].Width = 0;
                grdExp.Cols[(int)EX.gNOG].Width = 0;
                grdExp.Cols[(int)EX.gFOA].Width = 0;
                grdExp.Cols[(int)EX.gFOC].Width = 0;
                grdExp.Cols[(int)EX.gAMC].Width = 0;
                grdExp.Cols[(int)EX.gCMC].Width = 0;
                grdExp.Cols[(int)EX.gGMC].Width = 0;
                grdExp.Cols[(int)EX.gEBD].Width = 0;
                grdExp.Cols[(int)EX.gEBC].Width = 0;
                grdExp.Cols[(int)EX.gCUR].Width = 0;
                grdExp.Cols[(int)EX.gGCU].Width = 0;
                grdExp.Cols[(int)EX.gAMT].Width = 0;
                grdExp.Cols[(int)EX.gGAM].Width = 0;
            }
        } 
        private void chkAllAgent_CheckedChanged(object sender, EventArgs e)
        {
                drpAgent.setSelectedValue(null); 
            drpAgent.Enabled = chkAllAgent.Checked;
        }
        private void chkAllHandled_CheckedChanged(object sender, EventArgs e)
        {
                drpHandled.setSelectedValue(null); 
            drpHandled.Enabled = chkAllHandled.Checked;
        } 
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            if ((grdStatement[grdStatement.Row, (int)ST.gIDN] + "").ToString().Trim() != "")
            {
                txtTourNo.Text = grdStatement[grdStatement.Row, (int)ST.gIDN].ToString().Trim();
                Load_Tour();
                this.tcPNL.SelectedTab = tpIncome;
            }
            else    MessageBox.Show("Please Select a Correct Row", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);  
        }
        private void tcPNL_Click(object sender, EventArgs e)
        {
            if (tcPNL.SelectedTab.Name.Trim() == "tpStatement")
            {
                if(cmbRType.Items.Count>=2)  cmbRType.SelectedIndex = 1;
            }
            else if (tcPNL.SelectedTab.Name.Trim() == "tpIncome")
            {
                if (cmbRType.Items.Count >= 1)    cmbRType.SelectedIndex = 0;
            }
        } 
        private void chkRecConf_CheckedChanged(object sender, EventArgs e)     {  lblRecDate.Visible = dtpRecDate.Visible = chkRecConf.Checked;   }
        private void grdOI_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText; 
                if (e.Col == grdOI.Cols[(int)OI.gEXN].Index)
                {
                  DataTable  DTOtherIncome = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,IncomeName FROM mst_OtherIncome WHERE IsNull(IsActive,0)=1 ORDER BY IncomeName");
                    Other.frmSearchGrd frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTOtherIncome;
                    frm.SubForm = new Accounts.frmOtherIncome();
                    frm.Width = grdOI.Cols[(int)OI.gEXN].Width;
                    frm.Height = grdOI.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdOI);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        if (DTOtherIncome.Rows[0]["ID"].ToString() != "") grdOI[grdOI.Row, (int)OI.gEID] = SelText[0].ToString();
                        if (DTOtherIncome.Rows[0]["IncomeName"].ToString() != "") grdOI[grdOI.Row, (int)OI.gEXN] = SelText[1].ToString();
                    }
                } 
        } 
        private void btnShow_Click(object sender, EventArgs e)
        {
            thread = new Thread(displayWait);
            thread.IsBackground = true;
            GC.Collect();
            thread.Start();
            pbLoad.Visible = true;
            grdStatement.Visible = false;
            btnShow.Enabled = false;
        }
        public void displayWait()
        {
            try
            {
                FillMonthlyStatement f = Fill_Monthly_Statement;
                f();                
                thread.Abort();
            }
            catch (System.Threading.ThreadAbortException)
            {
                Invoke(new Action(() =>
                {
                    pbLoad.Visible = false;
                    grdStatement.Visible = true;
                    btnShow.Enabled = true;
                    lblInvalid.Visible = Check_For_Invalid_Bookings();
                    GC.Collect();
                }));
            }
            catch (Exception ex)   {   db.MsgERR(ex);  }
        }
        public Boolean Check_For_Invalid_Bookings()
        {
            try
            {
                C1.Win.C1FlexGrid.CellStyle INV = grdStatement.Styles.Add("INV");
                INV.BackColor = ColorTranslator.FromHtml("#F7FE2E");
                int RowNumb = 0;
                bool rtnVal = false;
                while (grdStatement[RowNumb + 1, (int)ST.gTID] + "".Trim() != "")
                { 
                    if (grdStatement[RowNumb + 1, (int)ST.gNOP] + "".Trim() == "" || Convert.ToInt32(grdStatement[RowNumb + 1, (int)ST.gNOP]) <= 0)
                    {
                        grdStatement.Rows[RowNumb + 1].Style = INV;
                        rtnVal=true;
                    } 
                    if (grdStatement[RowNumb + 1, (int)ST.gIAMU] + "".Trim() == "" && grdStatement[RowNumb + 1, (int)ST.gIAMR] + "".Trim() == "")
                    {
                        grdStatement.Rows[RowNumb + 1].Style = INV;
                        rtnVal = true;
                    }
                    if (grdStatement[RowNumb + 1, (int)ST.gIAMU] + "".Trim() != "")
                    {
                        if (Convert.ToDecimal(grdStatement[RowNumb + 1, (int)ST.gIAMU]) <= 0)
                        {
                            grdStatement.Rows[RowNumb + 1].Style = INV;
                            rtnVal = true;
                        }
                    }
                    if (grdStatement[RowNumb + 1, (int)ST.gIAMR] + "".Trim() != "")
                    {
                        if (Convert.ToDecimal(grdStatement[RowNumb + 1, (int)ST.gIAMR]) <= 0)
                        {
                            grdStatement.Rows[RowNumb + 1].Style = INV;
                            rtnVal = true;
                        }
                    } 
                    if (grdStatement[RowNumb + 1, (int)ST.gPOLU] + "".Trim() == "" && grdStatement[RowNumb + 1, (int)ST.gPOLR] + "".Trim() == "")
                    {
                        grdStatement.Rows[RowNumb + 1].Style = INV;
                        rtnVal = true;
                    }
                    if (grdStatement[RowNumb + 1, (int)ST.gPOLU] + "".Trim() != "")
                    {
                        if (Convert.ToDecimal(grdStatement[RowNumb + 1, (int)ST.gPOLU]) == 0)
                        {
                            grdStatement.Rows[RowNumb + 1].Style = INV;
                            rtnVal = true;
                        }
                    }
                    if (grdStatement[RowNumb + 1, (int)ST.gPOLR] + "".Trim() != "")
                    {
                        if (Convert.ToDecimal(grdStatement[RowNumb + 1, (int)ST.gPOLR]) == 0)
                        {
                            grdStatement.Rows[RowNumb + 1].Style = INV;
                            rtnVal = true;
                        }
                    } 
                    RowNumb++;
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void btnCombine_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTourNo.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select a Tour First", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string sql;
                sql = "SELECT InvoiceNo FROM act_PaymentIssued WHERE TransID=" + txtTourNo.Text.Trim() + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    if (DT.Rows[0]["InvoiceNo"].ToString().Trim() != "")
                    {
                        MessageBox.Show("Invoiced Booking Cannot Be Combined", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                sql = "SELECT ID,TourID,Guest,AgentID,DateArrival,DateDeparture,NoOfAdult,NoOfChild FROM trn_GroupAmendment WHERE ID!=" + txtTourNo.Text.Trim() + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
                txtCombineID.Text = finder.Load_search(DT);
                if (txtCombineID.Text.Trim() != "")
                {
                    sql = "SELECT ID,Guest FROM trn_GroupAmendment WHERE ID=" + txtCombineID.Text.Trim() + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                    if (DT.Rows.Count > 0)  lblCombineName.Text = DT.Rows[0]["Guest"].ToString().Trim(); 
                }
            }
            catch (Exception ex) { db.MsgERR(ex);       }
        }
        private void chkCmpny_CheckedChanged(object sender, EventArgs e)    {   cmbCompany.Enabled = chkCmpny.Checked;   }
        ErrorProvider errorProvider1 = new ErrorProvider();
        private void txtAdult_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!Tourist_Management.Classes.clsGlobal.IsNumeric(txtAdult.Text.Trim()) && txtAdult.Text.Length > 0) // Check if valid value is entered
            {
                if (txtAdult.Text.Length > 0)
                {
                    txtAdult.Text = txtAdult.Text.Remove(txtAdult.Text.Length - 1); // Remove Invalid characters
                    txtAdult.SelectionStart = txtAdult.Text.Length; // Set Cursor Start 
                    errorProvider1.SetError(txtAdult, "Please Enter Valid Amount"); // Show Error Message
                }
            }
            else  Calculate_Pax(); 
        }
        private void txtChild_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (!Tourist_Management.Classes.clsGlobal.IsNumeric(txtChild.Text.Trim()) && txtChild.Text.Length > 0) // Check if valid value is entered
            {
                if (txtChild.Text.Length > 0)
                {
                    txtChild.Text = txtChild.Text.Remove(txtChild.Text.Length - 1); // Remove Invalid characters
                    txtChild.SelectionStart = txtChild.Text.Length; // Set Cursor Start 
                    errorProvider1.SetError(txtChild, "Please Enter Valid Amount"); // Show Error Message
                }
            }
            else  Calculate_Pax();
        }
        private void Calculate_Pax()
        {
            int adult = 0, child = 0;
            if(txtAdult.Text != "")  adult = Convert.ToInt32(txtAdult.Text);
            if (txtChild.Text != "")   child = Convert.ToInt32(txtChild.Text);
            txtPax.Text = (adult + child).ToString(); ;
        }
        private void btnRemCombine_Click(object sender, EventArgs e)
        {
            txtCombineID.Text = null;
            lblCombineName.Text = null;
        }
        private List<string> l = new List<string>(); // Keep the list of IDs
        private void chkIsCompleted_CheckedChanged(object sender, EventArgs e)
        {
            if (txtTourNo.Text != "" && chkIsCompleted.Checked) // Check whether Tour No is empty and chkIsCompleted Checked
            {                
                if (Check_Driver_IsConfirm(Convert.ToInt32(txtTourNo.Text.Trim())) == false)
                {
                    string s = Get_Details("dbo.vw_trn_DriverDetails", "DriverID", "DriverName"); // Get The Details of not Confirmed Payments of Drivers
                    MessageBox.Show("Please Confirm Payments of the following Driver(s) :\n" + s, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chkIsCompleted.Checked = false;
                }
                if (Check_Hotel_IsConfirm(Convert.ToInt32(txtTourNo.Text.Trim())) == false)
                {
                    string s = Get_Details("dbo.vw_HotelDetails", "ID", "Name"); // Get The Details of not Confirmed Payments of Hotels
                    MessageBox.Show("Please Confirm Payments of the following Hotel(s) :\n" + s,"Info",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    chkIsCompleted.Checked = false;
                }
                if (Check_Guide_IsConfirm(Convert.ToInt32(txtTourNo.Text.Trim())) == false)
                {
                    string s = Get_Details("dbo.vw_trn_GuideDetails", "GuideID", "Name"); // Get The Details of not Confirmed Payments of Guides
                    MessageBox.Show("Please Confirm Payments of the following Guide(s) :\n" + s, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    chkIsCompleted.Checked = false;
                }
            }
        }
        private Boolean Check_Driver_IsConfirm(int TourID)
        {
                int RowNumb = 0;
                bool rtnVal = true;
                l.Clear(); // Clear the List
                string  sql = "SELECT DriverID,NotPaid,IsConfirm FROM dbo.trn_BasicTransport WHERE TransID=" + TourID + " AND ISNULL(DriverID,0)<>0 AND ISNULL(IsCancelled,0)<>1";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                while (DT.Rows.Count > RowNumb)
                {
                    if (DT.Rows[RowNumb]["IsConfirm"].ToString() != "")
                    {
                        if (!Convert.ToBoolean(DT.Rows[RowNumb]["IsConfirm"]))
                        {
                            l.Add(DT.Rows[RowNumb]["DriverID"].ToString()); // If not confirmed add to the list
                            rtnVal = false; // Set return value to false
                        }
                    }
                    else
                    {
                        if (DT.Rows[RowNumb]["NotPaid"].ToString() == "")
                        {
                            l.Add(DT.Rows[RowNumb]["DriverID"].ToString()); // If not confirmed add to the list
                            rtnVal = false; // Set return value to false
                        } 
                        else if (!Convert.ToBoolean(DT.Rows[RowNumb]["NotPaid"]))
                        {
                            l.Add(DT.Rows[RowNumb]["DriverID"].ToString()); // If not confirmed add to the list
                            rtnVal = false; // Set return value to false
                        }
                    }
                    RowNumb++;
                }
                return rtnVal;
        }
        private bool Check_Hotel_IsConfirm(int TourID)
        {
                int RowNumb = 0;
                bool rtnVal = true;
                l.Clear(); // Clear the List
                string   sql = "SELECT HotelID,AmendNo,ISNULL(DirectPay,0)AS DirectPay,VoucherID FROM dbo.trn_CityItinerary WHERE TransID=" + TourID + " AND ISNULL(HotelID,0)<>0";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                Double amt = 0.00;
                while (DT.Rows.Count > RowNumb)
                {
                    string qry = "SELECT dbo.fun_CalculateHotelAmount('" + DT.Rows[RowNumb]["VoucherID"].ToString() + "')AS Amount";
                    amt = Convert.ToDouble(Classes.clsGlobal.objCon.Fill_Table(qry).Rows[0]["Amount"]);
                    if(amt <= 0)
                    {
                        if (!Convert.ToBoolean(DT.Rows[RowNumb]["DirectPay"]))
                        {
                            if (DT.Rows[RowNumb]["AmendNo"].ToString() == "")
                            {
                                l.Add(DT.Rows[RowNumb]["HotelID"].ToString()); // If amend no not set add to the list
                                rtnVal = false; // Set return value to false
                            }
                            else if (Convert.ToInt32(DT.Rows[RowNumb]["AmendNo"]) == 0 || Convert.ToInt32(DT.Rows[RowNumb]["AmendNo"]) == 1 || Convert.ToInt32(DT.Rows[RowNumb]["AmendNo"]) == 2)
                            {
                                l.Add(DT.Rows[RowNumb]["HotelID"].ToString()); // If above voucher add to the list
                                rtnVal = false; // Set return value to false
                            }
                        }
                    }
                    RowNumb++;
                }
                return rtnVal;
        }
        private bool Check_Guide_IsConfirm(int TourID)
        {
                int RowNumb = 0;
                bool rtnVal = true;
                l.Clear(); // Clear the List
                string sql = "SELECT GuideID,NotPaid,IsConfirm FROM dbo.trn_GuideDetails WHERE TransID=" + TourID + " AND ISNULL(GuideID,0)<>0 AND ISNULL(IsCancelled,0)<>1";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                while (DT.Rows.Count > RowNumb)
                {
                    if (DT.Rows[RowNumb]["IsConfirm"].ToString() != "")
                    {
                        if (!Convert.ToBoolean(DT.Rows[RowNumb]["IsConfirm"]))
                        {
                            l.Add(DT.Rows[RowNumb]["GuideID"].ToString()); // If not confirmed add to the list
                            rtnVal = false; // Set return value to false
                        }
                    }
                    else
                    {
                        if (DT.Rows[RowNumb]["NotPaid"].ToString() == "")
                        {
                            l.Add(DT.Rows[RowNumb]["GuideID"].ToString()); // If not confirmed add to the list
                            rtnVal = false; // Set return value to false
                        } 
                        else if (!Convert.ToBoolean(DT.Rows[RowNumb]["NotPaid"]))
                        {
                            l.Add(DT.Rows[RowNumb]["GuideID"].ToString()); // If not confirmed add to the list
                            rtnVal = false; // Set return value to false
                        }
                    }
                    RowNumb++;
                }
                return rtnVal;
        }
        private string Get_Details(string view, string filter, string get)
        {
            string details = ""; 
            foreach (string s in l) // Retreive details for every ParentID in the list
            {
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT " + get + " FROM " + view + " WHERE " + filter + "=" + s + "");
                if(DT.Rows.Count >0 && DT.Rows[0][get]+"".Trim()!="") details += "\t" +DT.Rows[0][get].ToString() + "\n";
            }
            return details;
        }
    }
}
