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
using System.Data.SqlClient;
namespace Tourist_Management.Accounts
{
    public partial class frmUnsettled : Form
    { 
        private const string msghd = "Payment List";
        DataTable dtHotel = new DataTable();
        bool Loaded=false;
        string InvSql = "", HotelSql = "", DriverSql = "", GuideSql="";
        enum IN { gTID,gTRI,gCLN,gDTE,gAID,gANM,gAMT,gRTE,gRAM,gRRT,gHID,gHNM}; 
        enum HD { gTID, gTRI, gVID, gCNM, gHID, gHNM,gTDL, gCHI, gCHO, gAMT, gCHN, gIPD, gICN, gHBI, gHBN} 
        enum DD { gTID, gTRI, gCNM, gDID, gDNM, gDTA, gDTD, gPKM, gPBT, gEXC, gADV, gEXP, gTOT, gIPD, gHBI, gHBN } 
        enum GD { gTID, gTRI, gCNM, gGID, gGNM, gDTA, gDTD, gFEE, gDYS, gADV, gEXP, gTOT, gIPD, gHBI, gHBN } 
        public frmUnsettled()  { InitializeComponent();  }
        private void frmUnsettled_Load(object sender, EventArgs e)
        {
            Intializer(); 
            tbUnsettled.TabPages.Remove(tbUnsettled.SelectedTab); //chathuri
        }
        private void btnICancel_Click(object sender, EventArgs e)       {   this.Close();    }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Control();
                Loaded = true;
            }
            catch (Exception ex)     {   db.MsgERR(ex);  }
         }
        private void Grd_Initializer()
        {
            try
            {
                db.GridInit(grdInvoice, 3000, false, IN.gTID, 00, "Trans ID", IN.gTRI, 100, "Tour ID", IN.gCLN, 195, "Client Name", IN.gDTE, 106, "Date", IN.gAID, 0, "Agent ID", IN.gANM, 195, "Agent Name", IN.gAMT, 92, "Amount", "##.#", IN.gRTE, 77, "Rate", "##.#", IN.gRAM, 82, "Received", "##.#", IN.gRRT, 90, "Received Rate", "##.#", IN.gHID, 00, "Handled ID", IN.gHNM, 92, "Handled By"); 
                db.GridInit(grdHotel, 3000, false, HD.gTID, 00, "Trans ID", HD.gTRI, 100, "Tour ID", HD.gVID, 90, "Voucher ID", HD.gCNM, 200, "Client Name", HD.gHID, 00, "Hotel ID", HD.gHNM, 200, "Hotel Name", HD.gTDL, 60, "TDL", HD.gCHI, 100, "Check In", HD.gCHO, 100, "Check Out", HD.gAMT, 90, "Cost", "##.##", HD.gCHN, 90, "Cheque No", HD.gIPD, 50, "Paid", Type.GetType("System.Boolean"), HD.gICN, 00, HD.gHBI, 00, "Handled By ID", HD.gHBN, 75, "Handled By"); 
                db.GridInit(grdDriver, 3000, false, DD.gTID, 00, "Trans ID", DD.gTRI, 100, "Tour ID", DD.gCNM, 195, "Client Name", DD.gDID, 00, "Driver ID", DD.gDNM, 147, "Driver Name", DD.gDTA, 00, "Arrival", DD.gDTD, 00, "Departure", DD.gPKM, 79, "Paid For Km", "##.##", DD.gPBT, 67, "Bata", "##.##", DD.gEXC, 85, "Excursion", "##.##", DD.gADV, 78, "Advance", "##.##", DD.gEXP, 76, "Expenses", "##.##", DD.gTOT, 82, "Total", "##.##", DD.gIPD, 40, "Paid", Type.GetType("System.Boolean"), DD.gHBI, 00, "Handled ID", DD.gHBN, 80, "Handled By"); 
                db.GridInit(grdGuide, 3000, false, GD.gTID, 00, "Trans ID", GD.gTRI, 100, "Tour ID", GD.gCNM, 219, "Client Name", GD.gGID, 00, "Guide ID", GD.gGNM, 208, "Guide Name", GD.gDTA, 00, "Arrival", GD.gDTD, 00, "Departure", GD.gFEE, 79, "Fee", "##.##", GD.gDYS, 67, "#Days", "##.##", GD.gADV, 78, "Advance", "##.##", GD.gEXP, 76, "Expenses", "##.##", GD.gTOT, 82, "Total", "##.##", GD.gIPD, 40, "Paid", Type.GetType("System.Boolean"), GD.gHBI, 00, "Handled ID", GD.gHBN, 80, "Handled By"); 
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {
                drpIAgent.Enabled =   drpIHandled.Enabled = false;
                drpIAgent.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name as [AgentName] FROM mst_AgentDetails Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpIHandled.DataSource =    drpHHandled.DataSource =   drpDHandled.DataSource =  drpGHandled.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS [HandledPerson] FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpsHotel.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS HotelName FROM mst_HotelDetails Where IsNull(IsActive,0)=1 ORDER BY HotelName");
                drpDriver.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,DriverName FROM vw_TR_DriverVSVehicle WHERE IsNull(IsActive,0)=1 ORDER BY DriverName");
                drpGuide.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM vwGuideVsEmployee WHERE IsNull(IsActive,0)=1 ORDER BY Name");
                cmbICompany.DataSource = cmbHCompany.DataSource = cmbDCompany.DataSource = cmbGCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkIByAgent_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return;
            if (chkIByAgent.Checked)
            {
                drpIAgent.setSelectedValue(null);
                drpIAgent.Enabled = true;
            }
            else
            {
                drpIAgent.setSelectedValue(null);
                drpIAgent.Enabled = false;
                Fill_Unsettled_Invoice();
            }
        }
        private void chkIByHandled_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded) return; 
            if (chkIByHandled.Checked)
            {
                drpIHandled.setSelectedValue(null);
                drpIHandled.Enabled = true;
            }
            else
            {
                drpIHandled.setSelectedValue(null);
                drpIHandled.Enabled = false;
                Fill_Unsettled_Invoice();
            }
        }
        private void Fill_Unsettled_Invoice()
        {
            try
            {
                if (!Loaded)      return; 
                string format = "yyyy-MM-dd", ssql = "", filter = "";
                DateTime datefrom = dtpIFromDate.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpIToDate.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                string vwName = "";
                if (rdbSettled.Checked)   vwName = "vw_act_SettledInvoices";
                else if (rdbUnSettled.Checked)   vwName = "vw_act_UnsettledInvoices";
                 else      vwName = "vw_act_AllInvoices"; 
                ssql = "SELECT ID,TourID,Guest,DateArrival,DateDeparture,AgentID,AgentName," +
                           "ISNULL(Amount,0)AS Amount,ISNULL(Rate,0)AS Rate,InvoiceNo," +
                           "ISNULL(AgentRecAmt,0)AS AgentRecAmt,ISNULL(AgentRecRate,0)AS AgentRecRate," +
                           "CompID,DisplayName,Telephone,Fax,E_Mail,E_mailTo,UserName,UserGroupID,GroupName,Web,Postal_Addres,Company_Logo," +
                           "HandledByID,HandledBy FROM " + vwName +
                           " WHERE UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                if (chkICmpny.Checked)
                {
                    int compID = Convert.ToInt32(cmbICompany.SelectedValue);
                    filter = " AND CompID=" + compID + "";
                }
                if (chkIByDate.Checked)
                {
                    filter += " AND DateDeparture>='" + DateFrom.Trim() + "' AND DateDeparture<='" + DateTo.Trim() + "'";
                }
                if (chkIByAgent.Checked)
                {
                    if (drpIAgent.SelectedValue != null)
                    {
                        int AgentID = Convert.ToInt32(drpIAgent.SelectedValue);
                        filter += " AND AgentID=" + AgentID + "";
                    }
                }
                if (chkIByHandled.Checked)
                {
                    if (drpIHandled.SelectedValue != null)
                    {
                        int HandledByID = Convert.ToInt32(drpIHandled.SelectedValue);
                        filter += " AND HandledByID=" + HandledByID + "";
                    }
                }
                if (rdbIUninvioced.Checked)
                {
                    filter += " AND ISNULL(Amount,0)=0";
                }
                else if (rdbIUnrated.Checked)
                {
                    filter += " AND ISNULL(Amount,0)>0 AND ISNULL(AgentRecRate,0)=0";
                }
                InvSql = (ssql.Trim() + filter.Trim() + " ORDER BY DateDeparture").Trim();
                if (InvSql == "")
                    return;
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(InvSql);
                int RowNumb = 0;
                DateTime Arrival, Depart;
                string ArrivalFrom, DaprtTo;
                grdInvoice.Rows.Count = 1;
                while (DT.Rows.Count > RowNumb)
                {                   
                    grdInvoice.Rows.Count = DT.Rows.Count + 1;
                    grdInvoice[RowNumb + 1, (int)IN.gTID] = DT.Rows[RowNumb]["ID"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gTRI] = DT.Rows[RowNumb]["TourID"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gCLN] = DT.Rows[RowNumb]["Guest"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gAID] = DT.Rows[RowNumb]["AgentID"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gANM] = DT.Rows[RowNumb]["AgentName"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gAMT] = DT.Rows[RowNumb]["Amount"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gRTE] = DT.Rows[RowNumb]["Rate"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gRAM] = DT.Rows[RowNumb]["AgentRecAmt"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gRRT] = DT.Rows[RowNumb]["AgentRecRate"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gHID] = DT.Rows[RowNumb]["HandledByID"].ToString();
                    grdInvoice[RowNumb + 1, (int)IN.gHNM] = DT.Rows[RowNumb]["HandledBy"].ToString();
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
                    grdInvoice[RowNumb + 1, (int)IN.gDTE] = ArrivalFrom + " / " + DaprtTo;
                    RowNumb++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Unsettled_Hotels()
        {
            try
            {
                if (!Loaded) return;
                string ssql, conPaid = "", filter="",amendNo="";
                if (rdbSettled.Checked)   conPaid = "1"; 
                else if (rdbUnSettled.Checked)  conPaid = "0"; 
                else  conPaid = "1,0"; 
                ssql = "SELECT TransID,TourID,VoucherID,Guest,DateIn,DateOut,HotelID,HotelName,TDLNo,PayID," +
                          "Cost,ModifiedCost,RoomCount,ConRate,GuideCost,GuideRooms AS GuideRoomCount,GuideConRate," +
                          "FOCRooms,Nights,ISNULL(Commission,0)AS Commission,ISNULL(Advance,0)AS Advance," +
                          "NoOfAdult,NoOfChild,NoOfGuide,FOCAdult," +
                          "FOCChild,AdultMealCost,ChildMealCost,GuideMealCost,IsNull(ExtraBed,0)AS Ebed,IsNull(EbedCost,0)AS EbedCost," +
                          "CurCode,GuideCurCode,OtherAmt," +
                          "ISNULL(IsPaid,0)AS IsPaid,ISNULL(ConfirmPaid,0)AS ConfirmPaid,ISNULL(ChkNo, '')AS ChkNo,HandledByID,HandledBy," +
                          "CompID,DisplayName,Telephone,Fax,E_Mail,Web,Postal_Addres,Company_Logo" +
                          " FROM vw_acc_HotelAllPayments" +
                          " WHERE ISNULL(ConfirmPaid,0) IN(" + conPaid + ")";
                if (chkHcmpny.Checked)
                {
                    int compID = Convert.ToInt32(cmbHCompany.SelectedValue);
                    filter += " AND CompID=" + compID + "";
                }
                if (chkHByDate.Checked)
                {
                    filter += " AND DateOut>='" + dtpHFromDate.Value.ToString("yyyy-MM-dd").Trim() +
                              "' AND DateOut<='" + dtpHToDate.Value.ToString("yyyy-MM-dd").Trim() + "'";
                }
                if(chkReserv.Checked)  amendNo="0,1,";
                if(chkMeal.Checked)  amendNo+="2,";
                if(chkComplement.Checked)  amendNo+="99,90,8,7,";
                if(chkCancel.Checked) amendNo+="9,999,";
                if(amendNo+"".Trim()!="")
                {
                    amendNo = amendNo.Substring(0,amendNo.Length-1);
                    filter += " AND ISNULL(AmendNo,0) IN ("+amendNo+")";
                }
                if (chkByHotel.Checked)
                {
                    if (drpsHotel.SelectedList != null)
                    {
                        string HotelID="";
                        foreach (string s in drpsHotel.SelectedList)
                        {
                            if (HotelID.Trim() == "")     HotelID = s.Trim();
                            else  HotelID += ",".Trim() + s.Trim();
                        } 
                        filter += " AND HotelID IN(" + HotelID + ")";
                    }
                }
                if (chkHByHandled.Checked)
                {
                    if (drpHHandled.SelectedValue != null)
                    {
                        int HandledByID = Convert.ToInt32(drpHHandled.SelectedValue);
                        filter += " AND HandledByID=" + HandledByID + "";
                    }
                }
                if (grpByCheque.Visible == true)
                {
                    if(txtChequeNo.Text.Trim() != "")
                    filter += " AND ChkNo LIKE '%" + txtChequeNo.Text.Trim() + "%'";
                }
                if (grpByInv.Visible == true)
                {
                    if (txtInvoiceNo.Text.Trim() != "")
                        filter += " AND BillNoRoom LIKE '%" + txtInvoiceNo.Text.Trim() + "%'";
                }
                HotelSql = (ssql.Trim() + filter.Trim() + " ORDER BY HotelID,DateIn").Trim();
                if (HotelSql == "")  return;
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(HotelSql);
                int RowNumb = 0;
                double TotExp;
                string qry;
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    int CurNo = 1;
                    string CurVoucherNo = "", TempVoucherNo = ""; 
                    grdHotel.Rows.Count = 3000;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (RowNumb == 0)
                            CurVoucherNo = DT.Rows[RowNumb]["VoucherID"] + "";
                        if (CurVoucherNo != TempVoucherNo)
                        { 
                            CurVoucherNo = DT.Rows[RowNumb]["VoucherID"] + "";
                                    grdHotel.Rows[CurNo].AllowEditing = false;
                            grdHotel[CurNo, (int)HD.gTID] = DT.Rows[RowNumb]["TransID"].ToString();
                            grdHotel[CurNo, (int)HD.gTRI] = DT.Rows[RowNumb]["TourID"].ToString();
                            grdHotel[CurNo, (int)HD.gVID] = DT.Rows[RowNumb]["VoucherID"].ToString();
                            grdHotel[CurNo, (int)HD.gCNM] = DT.Rows[RowNumb]["Guest"].ToString();
                            grdHotel[CurNo, (int)HD.gHID] = DT.Rows[RowNumb]["HotelID"].ToString();
                            grdHotel[CurNo, (int)HD.gHNM] = DT.Rows[RowNumb]["HotelName"].ToString();
                            grdHotel[CurNo, (int)HD.gTDL] = DT.Rows[RowNumb]["TDLNo"] + "".Trim();
                            grdHotel[CurNo, (int)HD.gHBI] = DT.Rows[RowNumb]["HandledByID"].ToString();
                            grdHotel[CurNo, (int)HD.gHBN] = DT.Rows[RowNumb]["HandledBy"].ToString();
                            if (DT.Rows[RowNumb]["DateIn"] + "".Trim() != "")
                            { 
                                grdHotel[CurNo, (int)HD.gCHI] = DT.Rows[RowNumb]["DateIn"];
                            }
                            if (DT.Rows[RowNumb]["DateOut"] + "".Trim() != "")
                            { 
                                grdHotel[CurNo, (int)HD.gCHO] = DT.Rows[RowNumb]["DateOut"];
                            }
                            grdHotel[CurNo, (int)HD.gCHN] = DT.Rows[RowNumb]["ChkNo"].ToString();
                            grdHotel[CurNo, (int)HD.gIPD] = Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]);
                            qry = "SELECT dbo.fun_CalculateHotelAmount('" + DT.Rows[RowNumb]["VoucherID"].ToString().Trim() + "')Amt";
                            TotExp = Convert.ToDouble(Classes.clsGlobal.objCon.Fill_Table(qry).Rows[0]["Amt"]);
                            grdHotel[CurNo, (int)HD.gAMT] = TotExp.ToString();
                            CurNo++;
                        }
                        RowNumb++;
                        if (DT.Rows.Count > RowNumb)
                            TempVoucherNo = DT.Rows[RowNumb]["VoucherID"] + "";
                    }                    
                    grdHotel.Rows.Count = CurNo;
                }
                else
                {
                    grdHotel.Rows.Count = 1; 
                }
            }
            catch (Exception ex)  {  db.MsgERR(ex);     }
        }
        private void Fill_Unsettled_Drivers()
        {
            try
            {
                if (!Loaded)  return;
                string format = "yyyy-MM-dd", ssql = "", filter = "";
                DateTime datefrom = dtpDFromDate.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpDToDate.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                string conPaid = ""; 
                if (rdbSettled.Checked)    conPaid = "1";    else if (rdbUnSettled.Checked)  conPaid = "0";  else  conPaid = "1,0";
                ssql = "SELECT DISTINCT TransID as ID,TourID,Guest,DateArrival,DateDeparture,DriverID,DriverName," +
                           "ISNULL(TotalKm,0)AS TotalKm,ISNULL(RatePerKm,0)AS RatePerKm," +
                           "ISNULL(Bata,0)AS Bata,ISNULL(NoOfNights,0)AS NoOfNights," +
                           "ISNULL(ExcurAmt,0)AS ExcurAmt,ISNULL(IsPaid,0)AS IsPaid, ISNULL(NotPaid,0)as NotPaid," +
                           "HandledByID,HandledBy,SrNo FROM vw_acc_UnsettledDrivers" +
                           " WHERE ISNULL(IsConfirm,0) IN(" + conPaid + ") AND ISNULL(IsCancelled,0)<>1";
                if (chkDCmpny.Checked)
                {
                    int compID = Convert.ToInt32(cmbDCompany.SelectedValue);
                    filter += " AND CompID=" + compID + "";
                } 
                if (chkDByDate.Checked)
                {
                    filter += " AND DateDeparture>='" + DateFrom.Trim() + "' AND DateDeparture<='" + DateTo.Trim() + "'";
                } 
                if (chkByDriver.Checked)
                {
                    if (drpDriver.SelectedValue != null)
                    {
                        int DriverID = Convert.ToInt32(drpDriver.SelectedValue);
                        filter += " AND DriverID=" + DriverID + "";
                    }
                }
                if (chkDByHandled.Checked)
                {
                    if (drpDHandled.SelectedValue != null)
                    {
                        int HandledByID = Convert.ToInt32(drpDHandled.SelectedValue);
                        filter += " AND HandledByID=" + HandledByID + "";
                    }
                }
                DriverSql = (ssql.Trim() + filter.Trim() + " ORDER BY SrNo,DateDeparture").Trim();
                if (DriverSql == "")  return;
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(DriverSql);
                int RowNumb = 0;
                double syscode,Km, Bata, Excursion, Expenses, Advance, Total;
                int DID = 0;
                DateTime Arrival, Depart;
                string ArrivalFrom, DaprtTo;
                grdDriver.Rows.Count = 1;
                while (DT.Rows.Count > RowNumb)
                {
                    grdDriver.Rows.Count = DT.Rows.Count + 1;
                    Km = 0; Bata = 0; Excursion = 0; Expenses = 0; Advance = 0; Total = 0;
                    syscode = Convert.ToDouble(DT.Rows[RowNumb]["ID"]);
                    grdDriver[RowNumb + 1, (int)DD.gTID] = syscode;
                    grdDriver[RowNumb + 1, (int)DD.gTRI] = DT.Rows[RowNumb]["TourID"].ToString();
                    grdDriver[RowNumb + 1, (int)DD.gCNM] = DT.Rows[RowNumb]["Guest"].ToString();
                    DID = Convert.ToInt32(DT.Rows[RowNumb]["DriverID"]);
                    grdDriver[RowNumb + 1, (int)DD.gDID] = DID;
                    grdDriver[RowNumb + 1, (int)DD.gDNM] = DT.Rows[RowNumb]["DriverName"].ToString();
                    if (!Convert.ToBoolean(DT.Rows[RowNumb]["NotPaid"]))
                    {
                        Km = Convert.ToDouble(DT.Rows[RowNumb]["TotalKm"]) * Convert.ToDouble(DT.Rows[RowNumb]["RatePerKm"]);
                        Bata = Convert.ToDouble(DT.Rows[RowNumb]["Bata"]) * Convert.ToDouble(DT.Rows[RowNumb]["NoOfNights"]);
                        Excursion = Convert.ToDouble(DT.Rows[RowNumb]["ExcurAmt"]);
                    }
                    grdDriver[RowNumb + 1, (int)DD.gPKM] = Km.ToString();
                    grdDriver[RowNumb + 1, (int)DD.gPBT] = Bata.ToString();
                    grdDriver[RowNumb + 1, (int)DD.gEXC] = Excursion.ToString();
                    Advance = Get_Driver_Advance(syscode,DID);
                    grdDriver[RowNumb + 1, (int)DD.gADV] = Advance.ToString();
                    Expenses = Get_Driver_Expenses(syscode, DID);
                    grdDriver[RowNumb + 1, (int)DD.gEXP] = Expenses.ToString();
                    Total = ((Km + Bata + Excursion + Expenses) - Advance);
                    grdDriver[RowNumb + 1, (int)DD.gTOT] = Total.ToString();
                    grdDriver[RowNumb + 1, (int)DD.gIPD] = Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]);
                    grdDriver[RowNumb + 1, (int)DD.gHBI] = DT.Rows[RowNumb]["HandledByID"].ToString();
                    grdDriver[RowNumb + 1, (int)DD.gHBN] = DT.Rows[RowNumb]["HandledBy"].ToString();
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
                    grdDriver[RowNumb + 1, (int)DD.gDTA] = ArrivalFrom;
                    grdDriver[RowNumb + 1, (int)DD.gDTD] = DaprtTo;
                    RowNumb++;
                }
            }
            catch (Exception ex)    {  db.MsgERR(ex);        }
        }
        private void Fill_Unsettled_Guides()
        {
            try
            {
                if (!Loaded)    return;
                string format = "yyyy-MM-dd";
                string ssql = "";
                string filter = "";
                DateTime datefrom = dtpGFromDate.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpGToDate.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                string conPaid = "";
                if (rdbSettled.Checked) conPaid = "1";
                else if (rdbUnSettled.Checked)    conPaid = "0"; 
                else  conPaid = "1,0"; 
                ssql = "SELECT DISTINCT TransID,TourID,Guest,DateArrival,DateDeparture,GuideID,GuideName," +
                           "ISNULL(Fee,0)AS Fee,ISNULL(Days,0)AS Days," +
                           "ISNULL(IsPaid,0)AS IsPaid, ISNULL(NotPaid,0)as NotPaid," +
                           "HandledByID,HandledBy,SrNo FROM vw_acc_UnsettledGuides" +
                           " WHERE ISNULL(IsConfirm,0) IN(" + conPaid + ") AND ISNULL(IsCancelled,0)<>1";
                if (chkGCmpny.Checked)
                {
                    int compID = Convert.ToInt32(cmbGCompany.SelectedValue);
                    filter += " AND CompID=" + compID + "";
                }
                if (chkGByDate.Checked)
                {
                    filter += " AND DateDeparture>='" + DateFrom.Trim() + "' AND DateDeparture<='" + DateTo.Trim() + "'";
                }
                if (chkByGuide.Checked)
                {
                    if (drpGuide.SelectedValue != null)
                    {
                        int GuideID = Convert.ToInt32(drpGuide.SelectedValue);
                        filter += " AND GuideID=" + GuideID + "";
                    }
                }
                if (chkGByHandled.Checked)
                {
                    if (drpGHandled.SelectedValue != null)
                    {
                        int HandledByID = Convert.ToInt32(drpGHandled.SelectedValue);
                        filter += " AND HandledByID=" + HandledByID + "";
                    }
                }
                GuideSql = (ssql.Trim() + filter.Trim() + " ORDER BY SrNo,DateDeparture").Trim();
                if (GuideSql == "")
                    return;
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(GuideSql);
                int RowNumb = 0;
                double syscode, fee, days, Expenses, Advance, Total;
                int GID = 0;
                DateTime Arrival, Depart;
                string ArrivalFrom, DaprtTo;
                grdGuide.Rows.Count = 1;
                while (DT.Rows.Count > RowNumb)
                {
                    grdGuide.Rows.Count = DT.Rows.Count + 1;
                    fee = 0; days = 0; Expenses = 0; Advance = 0; Total = 0;
                    syscode = Convert.ToDouble(DT.Rows[RowNumb]["TransID"]);
                    grdGuide[RowNumb + 1, (int)GD.gTID] = syscode;
                    grdGuide[RowNumb + 1, (int)GD.gTRI] = DT.Rows[RowNumb]["TourID"].ToString();
                    grdGuide[RowNumb + 1, (int)GD.gCNM] = DT.Rows[RowNumb]["Guest"].ToString();
                    GID = Convert.ToInt32(DT.Rows[RowNumb]["GuideID"]);
                    grdGuide[RowNumb + 1, (int)GD.gGID] = GID;
                    grdGuide[RowNumb + 1, (int)GD.gGNM] = DT.Rows[RowNumb]["GuideName"].ToString();
                    if (!Convert.ToBoolean(DT.Rows[RowNumb]["NotPaid"]))
                    {
                        fee = Convert.ToDouble(DT.Rows[RowNumb]["fee"]);
                        days = Convert.ToDouble(DT.Rows[RowNumb]["days"]);                        
                    }
                    grdGuide[RowNumb + 1, (int)GD.gFEE] = fee.ToString();
                    grdGuide[RowNumb + 1, (int)GD.gDYS] = days.ToString();
                    Advance = Get_Guide_Advance(syscode, GID);
                    grdGuide[RowNumb + 1, (int)GD.gADV] = Advance.ToString();
                    Expenses = Get_Guide_Expenses(syscode, GID);
                    grdGuide[RowNumb + 1, (int)GD.gEXP] = Expenses.ToString();
                    Total = (((fee*days) + Expenses) - Advance);
                    grdGuide[RowNumb + 1, (int)GD.gTOT] = Total.ToString();
                    grdGuide[RowNumb + 1, (int)GD.gIPD] = Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]);
                    grdGuide[RowNumb + 1, (int)GD.gHBI] = DT.Rows[RowNumb]["HandledByID"].ToString();
                    grdGuide[RowNumb + 1, (int)GD.gHBN] = DT.Rows[RowNumb]["HandledBy"].ToString();
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
                    grdGuide[RowNumb + 1, (int)GD.gDTA] = ArrivalFrom;
                    grdGuide[RowNumb + 1, (int)GD.gDTD] = DaprtTo;
                    RowNumb++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private double Get_Driver_Advance(double syscode,int DID)
        {
                string sql = "SELECT SUM(ISNULL(Amount,0))AS Amount,SUM(ISNULL(ReturnAmt,0))AS ReturnAmt FROM trn_TourAdvance"+
                             " WHERE ISNULL(NotPaid,0)=0 AND ISNULL(IsDriver,0)=1 AND TransID=" + syscode + " AND DriverID=" + DID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                double Advance=0,Return=0;
                if (DT.Rows[0]["Amount"]+"".Trim()!="")
                {
                    Advance = Convert.ToDouble(DT.Rows[0]["Amount"]);
                    Return  = Convert.ToDouble(DT.Rows[0]["ReturnAmt"]);
                    return (Advance - Return);
                }
                else
                    return 0;
        }
        private double Get_Guide_Advance(double syscode, int GID)
        {
                string sql = "SELECT SUM(ISNULL(Amount,0))AS Amount,SUM(ISNULL(ReturnAmt,0))AS ReturnAmt FROM trn_TourAdvance" +
                             " WHERE ISNULL(NotPaid,0)=0 AND ISNULL(IsDriver,0)=0 AND TransID=" + syscode + " AND DriverID=" + GID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                double Advance = 0, Return = 0;
                if (DT.Rows[0]["Amount"] + "".Trim() != "")
                {
                    Advance = Convert.ToDouble(DT.Rows[0]["Amount"]);
                    Return = Convert.ToDouble(DT.Rows[0]["ReturnAmt"]);
                    return (Advance - Return);
                }
                else
                    return 0;
        }
        private double Get_Driver_Expenses(double syscode, int DID)
        {
                string sql = "SELECT SUM(ISNULL(Amount,0))AS Amount FROM trn_TravelExpenses " +
                             "WHERE ISNULL(NotPaid,0)=0 AND ISNULL(IsDriver,0)=1 AND TransID=" + syscode + " AND DriverID=" + DID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows[0]["Amount"] + "".Trim() != "")
                    return Convert.ToDouble(DT.Rows[0]["Amount"]);
                else
                    return 0;
        }
        private double Get_Guide_Expenses(double syscode, int GID)
        {
                string sql = "SELECT SUM(ISNULL(Amount,0))AS Amount FROM trn_TravelExpenses " +
                             "WHERE ISNULL(NotPaid,0)=0 AND ISNULL(IsDriver,0)=0 AND TransID=" + syscode + " AND DriverID=" + GID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows[0]["Amount"] + "".Trim() != "")
                    return Convert.ToDouble(DT.Rows[0]["Amount"]);
                else
                    return 0;
        }
        private void dtpIFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return;
            Fill_Unsettled_Invoice();
        } 
        private void dtpIToDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            Fill_Unsettled_Invoice();
        } 
        private void drpIAgent_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)    return; 
            Fill_Unsettled_Invoice();
        } 
        private void drpIHandled_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            Fill_Unsettled_Invoice();
        } 
        private void btnIPreview_Click(object sender, EventArgs e)
        {
            Print_Report("Invoice",InvSql);
        } 
        private void Print_Report(string Type,String sql)
        {
            try
            {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                DataTable DT;
                DataSet DTG = new DataSet();
                ReportDocument ga = new ReportDocument(); 
                if (Type.Trim() == "Invoice")
                {
                    DTG = new DataSets.ds_acc_UnsettledInvoices(); 
                       ga = new Tourist_Management.Reports.rpt_act_UnsettledInvoices(); 
                }
                else if (Type.Trim() == "Hotel")
                {
                    DTG = new DataSets.ds_acc_UnsettledHotels(); 
                    ga = new Tourist_Management.Reports.rpt_act_UnsettledHotels(); 
                    DataTable Dt =  Get_Unsettled_Hotel_DataTable();
                    if (Dt.Rows.Count > 0)   sConnection.Print_Via_Datatable(DTG, Dt, ga, "", new SqlParameter("comp", chkHcmpny.Checked)); 
                    else   MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (Type.Trim() == "Driver")
                {
                    DTG = new DataSets.ds_acc_UnsettledDrivers();
                    ga = new Tourist_Management.Reports.rpt_act_UnsettledDrivers(); 
                    DataTable Dt =  Get_Unsettled_Drivers_DataTable();
                    if (Dt.Rows.Count > 0) sConnection.Print_Via_Datatable(DTG, Dt, ga, "", new SqlParameter("comp", chkDCmpny.Checked)); 
                    else    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; 
                }
                else if (Type.Trim() == "Guide")
                {
                    DTG = new DataSets.ds_acc_UnsettledGuides(); 
                    ga = new Tourist_Management.Reports.rpt_act_UnsettledGuides();  
                    DataTable Dt =  Get_Unsettled_Guides_DataTable();
                    if (Dt.Rows.Count > 0)    sConnection.Print_Via_Datatable(DTG, Dt, ga, "", new SqlParameter("comp", chkGCmpny.Checked)); 
                    else  MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    DT.Columns.Add("Payment_State", typeof(string));
                    foreach (DataRow dr in DT.Rows)  dr["Payment_State"] = Get_Payment_State(); 
                    sConnection.Print_Via_Datatable(DTG, DT, ga, "", new SqlParameter("comp", chkICmpny.Checked)); 
                }
                else
                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private DataTable Get_Unsettled_Hotel_DataTable()
        { 
                int RowNumb = 1;
                DataTable table = new DataTable();
                table.Columns.Add("TransID", typeof(string));
                table.Columns.Add("TourID", typeof(string));
                table.Columns.Add("VoucherID", typeof(string));
                table.Columns.Add("Guest", typeof(string));
                table.Columns.Add("HotelID", typeof(int));
                table.Columns.Add("HotelName", typeof(string));                
                table.Columns.Add("DateIn", typeof(DateTime));
                table.Columns.Add("DateOut", typeof(DateTime));
                table.Columns.Add("Amount", typeof(double));
                table.Columns.Add("IsPaid", typeof(bool));
                table.Columns.Add("HandledBy", typeof(string));
                table.Columns.Add("TDLNo", typeof(string));
                table.Columns.Add("CompID", typeof(int));
                table.Columns.Add("DisplayName", typeof(string));
                table.Columns.Add("Telephone", typeof(string));
                table.Columns.Add("Fax", typeof(string));
                table.Columns.Add("E_Mail", typeof(string));
                table.Columns.Add("Web", typeof(string));
                table.Columns.Add("Physical_Address", typeof(string));
                table.Columns.Add("Company_Logo", typeof(byte[]));
                table.Columns.Add("Payment_State", typeof(string));                
                string transid, tourid,voucherid,guest,hotelname,tdlno,handled;
                DateTime datein,dateout;
                int hotelid;
                double amount;
                bool IsPaid;
                int compID;
                string displayName, Address, tel, fax, email, web , pstate;
                byte[] comLogo;
                while (grdHotel.Rows.Count > RowNumb)
                { 
                    transid = grdHotel[RowNumb, (int)HD.gTID].ToString();
                    tourid = grdHotel[RowNumb, (int)HD.gTRI].ToString();
                    voucherid = grdHotel[RowNumb, (int)HD.gVID].ToString();
                    guest = grdHotel[RowNumb, (int)HD.gCNM].ToString();
                    hotelid = Convert.ToInt32(grdHotel[RowNumb, (int)HD.gHID]);
                    hotelname = grdHotel[RowNumb, (int)HD.gHNM].ToString();
                    tdlno = grdHotel[RowNumb, (int)HD.gTDL] + "".Trim();
                    datein = Convert.ToDateTime(grdHotel[RowNumb, (int)HD.gCHI].ToString());
                    dateout = Convert.ToDateTime(grdHotel[RowNumb, (int)HD.gCHO].ToString());
                    amount = Convert.ToDouble(grdHotel[RowNumb, (int)HD.gAMT]);
                    IsPaid = Convert.ToBoolean(grdHotel[RowNumb, (int)HD.gIPD]);
                    handled = grdHotel[RowNumb, (int)HD.gHBN].ToString();
                    if (RowNumb == 1)
                    {
                        if (chkHcmpny.Checked)
                        {
                            DataTable dt = Classes.clsGlobal.getCompanyDetails(Convert.ToInt32(cmbDCompany.SelectedValue));
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
                            pstate = Get_Payment_State();
                            table.Rows.Add(transid, tourid, voucherid, guest, hotelid, hotelname, datein, dateout, amount, IsPaid, handled,tdlno,compID, displayName, tel, fax, email, web, Address, comLogo, pstate);
                        }
                    }
                    else
                    {
                        table.Rows.Add(transid, tourid, voucherid, guest, hotelid, hotelname, datein, dateout, amount, IsPaid, handled,tdlno);
                    }
                    RowNumb++;
                }
                return table; 
        }
        private DataTable Get_Unsettled_Drivers_DataTable()
        { 
                int RowNumb = 1; 
                DataTable table = new DataTable();  
                table.Columns.Add("TransID", typeof(string));
                table.Columns.Add("TourID", typeof(string));
                table.Columns.Add("Guest", typeof(string));
                table.Columns.Add("DriverID", typeof(int));
                table.Columns.Add("DriverName", typeof(string));
                table.Columns.Add("Arrival", typeof(DateTime));
                table.Columns.Add("Departure", typeof(DateTime));
                table.Columns.Add("PaidForKm", typeof(double));
                table.Columns.Add("Bata", typeof(double));
                table.Columns.Add("Excursion", typeof(double));
                table.Columns.Add("Advance", typeof(double));
                table.Columns.Add("Expenses", typeof(double));
                table.Columns.Add("Total", typeof(double));
                table.Columns.Add("IsPaid", typeof(bool));
                table.Columns.Add("HandledBy", typeof(string));
                table.Columns.Add("CompID", typeof(int));
                table.Columns.Add("DisplayName", typeof(string));
                table.Columns.Add("Telephone", typeof(string));
                table.Columns.Add("Fax", typeof(string));
                table.Columns.Add("E_Mail", typeof(string));
                table.Columns.Add("Web", typeof(string));
                table.Columns.Add("Physical_Address", typeof(string));
                table.Columns.Add("Company_Logo", typeof(byte[]));
                table.Columns.Add("Payment_State", typeof(string));
                string transid, tourid, guest, drivername, handled;
                DateTime arrival, departure;
                int driverid;
                double km,bata,excursion,advance,expenses,total;
                bool IsPaid;
                int compID;
                string displayName, Address, tel, fax, email, web, pstate;
                byte[] comLogo;
                while (grdDriver.Rows.Count > RowNumb)
                {
                    transid = grdDriver[RowNumb, (int)DD.gTID].ToString();
                    tourid = grdDriver[RowNumb, (int)DD.gTRI].ToString();
                    guest = grdDriver[RowNumb, (int)DD.gCNM].ToString();
                    driverid = Convert.ToInt32(grdDriver[RowNumb, (int)DD.gDID]);
                    drivername = grdDriver[RowNumb, (int)DD.gDNM].ToString();
                    arrival = Convert.ToDateTime(grdDriver[RowNumb, (int)DD.gDTA].ToString());
                    departure = Convert.ToDateTime(grdDriver[RowNumb, (int)DD.gDTD].ToString());
                    km = Convert.ToDouble(grdDriver[RowNumb, (int)DD.gPKM]);
                    bata = Convert.ToDouble(grdDriver[RowNumb, (int)DD.gPBT]);
                    excursion = Convert.ToDouble(grdDriver[RowNumb, (int)DD.gEXC]);
                    advance = Convert.ToDouble(grdDriver[RowNumb, (int)DD.gADV]);
                    expenses = Convert.ToDouble(grdDriver[RowNumb, (int)DD.gEXP]);
                    total = Convert.ToDouble(grdDriver[RowNumb, (int)DD.gTOT]);
                    IsPaid = Convert.ToBoolean(grdDriver[RowNumb, (int)DD.gIPD]);
                    handled = grdDriver[RowNumb, (int)DD.gHBN].ToString();
                    if (RowNumb == 1)
                    {
                        if (chkDCmpny.Checked)
                        {
                            DataTable dt = Classes.clsGlobal.getCompanyDetails(Convert.ToInt32(cmbDCompany.SelectedValue));
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
                            pstate = Get_Payment_State();
                            table.Rows.Add(transid, tourid, guest, driverid, drivername, arrival, departure, km, bata, excursion, advance, expenses, total, IsPaid, handled, compID, displayName, tel, fax, email, web, Address, comLogo, pstate);
                        }
                    }
                    else
                    {
                        table.Rows.Add(transid, tourid, guest, driverid, drivername, arrival, departure, km, bata, excursion, advance, expenses, total, IsPaid, handled);
                    }
                    RowNumb++;
                }
                return table; 
        }
        private DataTable Get_Unsettled_Guides_DataTable()
        {
                int RowNumb = 1;
                DataTable table = new DataTable();
                table.Columns.Add("TransID", typeof(string));
                table.Columns.Add("TourID", typeof(string));
                table.Columns.Add("Guest", typeof(string));
                table.Columns.Add("GuideID", typeof(int));
                table.Columns.Add("GuideName", typeof(string));
                table.Columns.Add("Arrival", typeof(DateTime));
                table.Columns.Add("Departure", typeof(DateTime));
                table.Columns.Add("Fee", typeof(double));
                table.Columns.Add("Days", typeof(double));
                table.Columns.Add("Advance", typeof(double));
                table.Columns.Add("Expenses", typeof(double));
                table.Columns.Add("Total", typeof(double));
                table.Columns.Add("IsPaid", typeof(bool));
                table.Columns.Add("HandledBy", typeof(string));
                table.Columns.Add("CompID", typeof(int));
                table.Columns.Add("DisplayName", typeof(string));
                table.Columns.Add("Telephone", typeof(string));
                table.Columns.Add("Fax", typeof(string));
                table.Columns.Add("E_Mail", typeof(string));
                table.Columns.Add("Web", typeof(string));
                table.Columns.Add("Physical_Address", typeof(string));
                table.Columns.Add("Company_Logo", typeof(byte[]));
                table.Columns.Add("Payment_State", typeof(string));
                string transid, tourid, guest, guidename, handled;
                DateTime arrival, departure;
                int guideid,days;
                double fee, advance, expenses, total;
                bool IsPaid;
                int compID;
                string displayName, Address, tel, fax, email, web, pstate;
                byte[] comLogo;
                while (grdGuide.Rows.Count > RowNumb)
                {
                    transid = grdGuide[RowNumb, (int)GD.gTID].ToString();
                    tourid = grdGuide[RowNumb, (int)GD.gTRI].ToString();
                    guest = grdGuide[RowNumb, (int)GD.gCNM].ToString();
                    guideid = Convert.ToInt32(grdGuide[RowNumb, (int)GD.gGID]);
                    guidename = grdGuide[RowNumb, (int)GD.gGNM].ToString();
                    arrival = Convert.ToDateTime(grdGuide[RowNumb, (int)GD.gDTA].ToString());
                    departure = Convert.ToDateTime(grdGuide[RowNumb, (int)GD.gDTD].ToString());
                    fee = Convert.ToDouble(grdGuide[RowNumb, (int)GD.gFEE]);
                    days = Convert.ToInt32(grdGuide[RowNumb, (int)GD.gDYS]);
                    advance = Convert.ToDouble(grdGuide[RowNumb, (int)GD.gADV]);
                    expenses = Convert.ToDouble(grdGuide[RowNumb, (int)GD.gEXP]);
                    total = Convert.ToDouble(grdGuide[RowNumb, (int)GD.gTOT]);
                    IsPaid = Convert.ToBoolean(grdGuide[RowNumb, (int)GD.gIPD]);
                    handled = grdGuide[RowNumb, (int)GD.gHBN].ToString();
                    if (RowNumb == 1)
                    {
                        if (chkGCmpny.Checked)
                        {
                            DataTable dt = Classes.clsGlobal.getCompanyDetails(Convert.ToInt32(cmbDCompany.SelectedValue));
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
                            pstate = Get_Payment_State();
                            table.Rows.Add(transid, tourid, guest, guideid, guidename, arrival, departure, fee, days, advance, expenses, total, IsPaid, handled, compID, displayName, tel, fax, email, web, Address, comLogo, pstate);
                        }
                    }
                    else
                    {
                        table.Rows.Add(transid, tourid, guest, guideid, guidename, arrival, departure, fee, days, advance, expenses, total, IsPaid, handled);
                    }
                    RowNumb++;
                }
                return table;
        }
        private DataTable Get_Company_Details(DataTable DT,int RowNumb,string sql)
        { 
            return DT;
        }
        private string Get_Payment_State()
        {
            if (!Loaded)   return ""; 
                return rdbSettled.Checked?"Settled":"Unsettled"; 
        }
        private void btnHCancel_Click(object sender, EventArgs e){this.Close();}
        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            if(rdbAll.Checked)
            {
                chkByHotel.Checked = false; 
                dtHotel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS HotelName FROM mst_HotelDetails Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpsHotel.DataSource = dtHotel;
            }
        }
        private void rdbCredit_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)    return; 
            if (rdbCredit.Checked)
            {
                chkByHotel.Checked = false;  
                dtHotel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS HotelName FROM mst_HotelDetails Where ISNULL(PayID,1)=2 AND IsNull(IsActive,0)=1 ORDER BY ID");
                drpsHotel.DataSource = dtHotel;
            }
        }
        private void rdbDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            if (rdbDaily.Checked)
            {
                chkByHotel.Checked = false;  
                dtHotel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS HotelName FROM mst_HotelDetails Where ISNULL(PayID,1)=1 AND IsNull(IsActive,0)=1 ORDER BY ID");
                drpsHotel.DataSource = dtHotel;
            }
        }
        private void chkByHotel_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)      return;
drpsHotel.Enabled = chkByHotel.Checked; 
        }
        private void chkHByHandled_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;
                drpHHandled.setSelectedValue(null);
                drpHHandled.Enabled = chkHByHandled.Checked; 
        }
        private void drpHotel_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
        }
        private void drpHHandled_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
        }
        private void dtpHFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
        }
        private void dtpHToDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
        }
        private void btnHPreview_Click(object sender, EventArgs e)
        {
            Print_Report("Hotel", HotelSql);
        }
        private void btnDCancel_Click(object sender, EventArgs e){this.Close();}
        private void chkByDriver_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            clear_Gird(grdDriver);
                drpDriver.setSelectedValue(null);
                drpDriver.Enabled = chkByDriver.Checked; 
        }
        private void chkDByHandled_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            clear_Gird(grdDriver);
                drpDHandled.setSelectedValue(null); 
            drpDHandled.Enabled = chkDByHandled.Checked; 
        }
        private void dtpDFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            clear_Gird(grdDriver); 
        }
        private void dtpDToDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;
            clear_Gird(grdDriver); 
        }
        private void drpDHandled_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;
            clear_Gird(grdDriver); 
        }
        private void drpDriver_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)    return;
            clear_Gird(grdDriver); 
        }
        private void btnDPreview_Click(object sender, EventArgs e)
        {
            Print_Report("Driver", DriverSql);
        }
        private void button5_Click(object sender, EventArgs e){this.Close();}
        private void chkByGuide_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            clear_Gird(grdGuide); 
                drpGuide.setSelectedValue(null); 
            drpGuide.Enabled = chkByGuide.Checked; 
        }
        private void chkGByHandled_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)     return; 
            clear_Gird(grdGuide);
                drpGHandled.setSelectedValue(null); 
            drpGHandled.Enabled = chkGByHandled.Checked; 
        }
        private void dtpGFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;
            clear_Gird(grdGuide); 
        }
        private void dtpGToDate_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return;
            clear_Gird(grdGuide); 
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Print_Report("Guide", GuideSql);
        }
        private void drpGHandled_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;
            clear_Gird(grdGuide); 
        }
        private void drpGuide_Selected_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;
            clear_Gird(grdGuide); 
        } 
        private void rdbIAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            Fill_Unsettled_Invoice();
        }
        private void rdbIUninvioced_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            Fill_Unsettled_Invoice();
        }
        private void rdbIUnrated_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)     return; 
            Fill_Unsettled_Invoice();
        }
        private void cmbICompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            Fill_Unsettled_Invoice();
        }
        private void cmbHCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
        }
        private void cmbDCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Loaded)    return;
            clear_Gird(grdDriver); 
        }
        private void cmbGCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;
            clear_Gird(grdGuide); 
        }
        private void rdbSettled_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            grpByCheque.Visible = true;
            txtChequeNo.Text = "";
            grdHotel.Rows.RemoveRange(1, grdHotel.Rows.Count-1); 
            clear_Gird(grdInvoice);
            clear_Gird(grdHotel);
            clear_Gird(grdDriver);
            clear_Gird(grdGuide); 
        }
        private void rdbUnSettled_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            clear_Gird(grdInvoice);
            clear_Gird(grdHotel);
            clear_Gird(grdDriver);
            clear_Gird(grdGuide); 
            grpByCheque.Visible = false;
            grdHotel.Rows.RemoveRange(1, grdHotel.Rows.Count-1); 
        }
        private void txtChequeNo_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            grdHotel.Rows.RemoveRange(1, grdHotel.Rows.Count - 1); 
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            grdHotel.Rows.RemoveRange(1, grdHotel.Rows.Count - 1); 
        }
        private void chkByDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;  
            dtpIFromDate.Enabled = dtpIToDate.Enabled = chkIByDate.Checked;
                Fill_Unsettled_Invoice();
        }
        private void chkHByDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return;  
            dtpHFromDate.Enabled = dtpHToDate.Enabled = chkHByDate.Checked; 
        }
        private void chkDByDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            clear_Gird(grdDriver);  
            dtpDFromDate.Enabled = dtpDToDate.Enabled = chkDByDate.Checked; 
        }
        private void chkGByDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return; 
            clear_Gird(grdGuide);  
            dtpGFromDate.Enabled = dtpGToDate.Enabled = chkGByDate.Checked; 
        }
        private void rdbAl_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)    return; 
            clear_Gird(grdInvoice);
            clear_Gird(grdHotel);
            clear_Gird(grdDriver);
            clear_Gird(grdGuide); 
            grpByCheque.Visible = false; 
        }
        private void chkICmpny_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded) return;
            cmbICompany.Enabled = chkICmpny.Checked; 
                Fill_Unsettled_Invoice();
        }
        private void chkHcmpny_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)   return;
            cmbHCompany.Enabled =chkHcmpny.Checked; 
        }
        private void chkDCmpny_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            clear_Gird(grdDriver);
cmbDCompany.Enabled = chkDCmpny.Checked; 
        }
        private void chkGCmpny_CheckedChanged(object sender, EventArgs e)
        {
            if (!Loaded)  return; 
            clear_Gird(grdGuide);
            cmbGCompany.Enabled = chkGCmpny.Checked; 
        }
        private void btnFilter_Click(object sender, EventArgs e)        {            Fill_Unsettled_Hotels();        }
        private void btnDriverFilter_Click(object sender, EventArgs e)        {            Fill_Unsettled_Drivers();        }
        private void clear_Gird(C1.Win.C1FlexGrid.C1FlexGrid grd)
        {
            try
            {
                grd.Rows.Count = 1;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnFilterGuides_Click(object sender, EventArgs e)
        {Fill_Unsettled_Guides(); }
    }
}
