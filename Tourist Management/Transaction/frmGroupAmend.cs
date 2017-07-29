using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net.Mail;
using System.Net;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using C1.Win.C1FlexGrid;
namespace Tourist_Management.Transaction
{
    public partial class frmGroupAmend : Form
    {
        private const string msghd = "Tourist Group Amendment";
        public int Mode = 0;
        public double SystemCode = 0;
        double TotalAmount = 0;
        bool IsPreview = false, AllowChange = true, isLoad = false, bLoad = false, HasHotel = false, HasSightSeeing = false, IsLock = false, IsCancelled = false, IsRateChecked;
        int RowNumb, NoOfChild = 0, NoOfAdult = 0, NoOfGuide = 0, DefDriverID = 0, DefGuideID = 0, TRE_TotRowNo = 0;
        int[] HotelID, SightID;
        string ssql = "", HotelsCannotBeEdited = "";
        string[] HotelName, HotelVoucher, Sightseeing;
        DateTime DateArrival, DateDeparture;
        DateTime[] Checkin, Checkout;
        List<int> slGuide = new List<int>();
        TabControl tcTransHotel = new TabControl(), tcTransSightseeing = new TabControl(), tcTransOther = new TabControl(), tcTransEmail = new TabControl();
        DataTable DTHot;
        enum DR { gIDN, gDID, gDCD, gDCM, gDNM, gDGL, gTEL, gIPD };
        enum GD { gIDN, gGID, gDID, gGCD, gGNM, gFEE, gNOD, gGLC, gTEL, gIPD, gPDT, gPBY, gICN, gNPD, gCNF };
        enum MG { gDID, gDNM, gGID, gGNM, gADT, gATM, gAFL, gDDT, gDTM, gDFL, gIDC, gIGC, gIPD, gIPG };
        public enum CI { gIDN, gCID, gCTY, gDTI, gNOD, gDTO, gHID, gHNM, gGNM, gCNC, gCON, gCAN, gNOA, gNOC, gNOG, gFOC, gNAP, gVNO, gBNO, gCNO, gANO, gATM, gMID, gSCI, gSCN, gCST, gCBY, gCDT, gMBY, gMDT, gOAMT, gRMK, gAMC, gCMC, gGMC, gADV, gCMS, gCNR, gTOT, gFAD, gFCD, gPCI, gGCI, gGCR, gIPD, gPBY, gPDT, gCNF, gCNB, gCND, gDPY };
        enum AG { gAFI, gAFR, gATI, gATO, gCNT };
        enum SD { gCID, gCTY, gSID, gSNM, gTSL };
        enum HG { gVNO, gBNO, gCHI, gCHO, gRTI, gRTN, gRBI, gRBN, gCID, gCON, gMID, gMAX, gNOR, gNGR, gEBD, gVAT, gTAX, gSCH, gTPR, gPRI, gTOT, gSEL, gLUP, gFOC, gMRC, gGRC, gMEC };
        enum TA { gIDN, gIDR, gDID, gDNM, gEID, gENM, gAMT, gIPD, gPDT, gPBY, gIST, gSDT, gSBY, gNPD };
        enum TR { gTR, gTN, gVO, gHT, gDT, gTM, gFI, gFR, gTI, gTO, gVI, gVN, gDI, gDN, gGI, gGN, gDS, gCH };
        enum TI { gIID, gIDR, gName, gINM, gNOI, gREC, gRTN };
        enum SM { gSNO, gMNO, gCOM };
        enum SC { gSNO, gAMT, gSDT };
        enum TP { gIDN, gIDR, gDID, gDNM, gEID, gENM, gUNT, gAMT, gIPD, gNPD };
        enum SE { gSSI, gSCI, gSCN, gSPC, gSPN, gNOA, gNOC, gSAC, gSCC, gNAC, gNCC, gTOT, gSEL };
        enum OE { gEXN, gVAT, gTAX, gSCH, gPRC, gTOT, gPID, gPDT, gIBP, gCNO, gRMK };
        enum TE { gEXN, gVAT, gTAX, gSCH, gPRC, gTOT, gRMK };
        enum DB { gDID, gDNM, gADT, gATM, gAFL, gDDT, gDTM, gDFL, gIDC, gEXC, gEXD, gEXA, gSMT, gEMT, gTKM, gRKM, gBAT, gNON, gIPD, gNPD, gCNF, gPDT, gPBY, gRMK };
        enum MS { gMID, gMTM, gNOA, gNOC, gNOG, gAMC, gCMC, gGMC, gTOT };
        public Dictionary<string, User_Controls.ucHotelNavigation> dicHotels = new Dictionary<string, User_Controls.ucHotelNavigation>();
        public Dictionary<string, User_Controls.ucSSNavigation> dicSight = new Dictionary<string, User_Controls.ucSSNavigation>();
        public Dictionary<string, User_Controls.ucTransOther> dicOthers = new Dictionary<string, User_Controls.ucTransOther>();
        public Dictionary<string, User_Controls.ucTransEmail> dicEmail = new Dictionary<string, User_Controls.ucTransEmail>();
        public string SqlQry
        {
            get
            {
                DataTable DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ManagerID FROM mst_UserMaster Where ID=" + Classes.clsGlobal.UserID + "");
                if (DT.Rows[0]["ManagerID"].ToString() != "") return "SELECT ID,TourID,Guest,CONVERT(varchar(12), DateArrival, 107)AS [Arrival Date],AFlightTime AS[Arrival Flight Time],HandledBy AS Coordinator From vw_TourBasics_list Where MarketingDep=" + Convert.ToInt32(DT.Rows[0]["ManagerID"].ToString()) + " AND Isnull([Status],0)<>7 Order By ID Desc";
                else return "SELECT ID,TourID,Guest,CONVERT(varchar(12), DateArrival, 107)AS [Arrival Date],AFlightTime AS[Arrival Flight Time],HandledBy AS Coordinator From vw_TourBasics_list Where Isnull([Status],0)<>7 Order By ID Desc";
            }
        }
        public frmGroupAmend() { InitializeComponent(); }
        private void frmTouristAmend_Load(object sender, EventArgs e)
        {
            btnuc.Visible = false;
            dtpFrom.Visible = false;
            dtpTo.Visible = false;
            Intializer();
            cmbReportType.Visible = true;
        }
        private void Intializer()
        {
            try
            {
                Enable_Disable_Save(true);
                chkPrint.Checked = true;
                Grd_Initializer();
                Fill_Control();
                if (Mode != 0)
                {
                    Fill_Data();
                    Fill_Hotel_Name_In_Increase_Amend();
                    cmbCompany.Enabled = false;
                    drpMarketingDep.Enabled = false;
                }
                else
                {
                    Fill_Grids();
                    drpArivalAirport.setSelectedValue("1002");
                    drpDepartAirport.setSelectedValue("1002");
                }
                isLoad = true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        public void Get_Tour_Number()
        {
            decimal numb = 101001;
            string country = "", ID = "", company = "", HPC = "", CPC = "";
            if (Mode != 1)
            {
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select MAX(ID) AS ID FROM dbo.trn_GroupAmendment");
                if (DT.Rows[0]["ID"].ToString() != "")
                    numb = Convert.ToDecimal(DT.Rows[0]["ID"].ToString()) + 1;
                else if (DT.Rows.Count == 0)
                    numb = 1001;
            }
            else
                numb = Convert.ToDecimal(SystemCode);
            DataTable dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Field, Code, SrNo FROM trn_TourID ORDER BY SrNo");
            int row = 0;
            if (drpCountry.SelectedValue.ToString() != "") country = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code FROM dbo.mst_Country WHERE ID =" + drpCountry.SelectedValue).Rows[0]["Code"].ToString().ToUpper();
            if (drpMarketingDep.SelectedValue.ToString().Trim() != "") HPC = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code FROM mst_MarketingDep WHERE ID=" + drpMarketingDep.SelectedValue.ToString().Trim()).Rows[0]["Code"].ToString().ToUpper();
            if (chkCompany.Checked == true) company = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select CompanyCode FROM mst_CompanyGenaral WHERE ID=" + cmbCompany.SelectedValue + "").Rows[0]["CompanyCode"].ToString().ToUpper().Trim();
            while (dt.Rows.Count > row)
            {
                string field = dt.Rows[row]["Field"].ToString().Trim();
                if (field == "Company Code") if (company == null || company == "") ID += "/" + dt.Rows[row]["Code"].ToString().Trim(); else ID += "/" + company;
                if (field == "Country Code") if (country == null || country == "") ID += "/" + dt.Rows[row]["Code"].ToString().Trim(); else ID += "/" + country;
                if (field == "Handled Person Code")
                {
                    string d = "Select ID FROM vw_CurrentUserDetails WHERE ID = " + drpMarketingDep.SelectedValue + "";
                    HPC = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(d).Rows[0]["UserID"].ToString().ToUpper();
                    if (HPC == null || HPC == "") ID += "/" + dt.Rows[row]["Code"].ToString().Trim(); else ID += "/" + HPC;
                }
                if (field == "Created Person Code")
                {
                    string d = "Select UserID FROM vw_CurrentUserDetails WHERE UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "";
                    CPC = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(d).Rows[0]["UserID"].ToString().ToUpper();
                    if (CPC == null || CPC == "") ID += "/" + dt.Rows[row]["Code"].ToString().Trim(); else ID += "/" + CPC;
                }
                if (field == "ID") if (numb == 0 || numb.ToString() == "") ID += "/" + dt.Rows[row]["Code"].ToString().Trim(); else ID += "/" + numb;
                row++;
            }
            ID = ID.Substring(1).Trim();
            txtTourID.Text = ID.Trim();
        }
        private void Get_VoucherNo()
        {
            try
            {
                int RowNumb = 1;
                decimal numb = 101001;
                if (Mode != 1)
                {
                    numb = Convert.ToDecimal(txtTourID.Text.Trim().Substring(7, 6));
                    while (grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        grdCI[RowNumb, (int)CI.gANO] = 0;
                        grdCI[RowNumb, (int)CI.gVNO] = numb + "/" + RowNumb;
                        HotelVoucher[RowNumb - 1] = (numb + "/" + RowNumb).Trim();
                        grdCI[RowNumb, (int)CI.gCBY] = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                        grdCI[RowNumb, (int)CI.gCDT] = DateTime.Now;
                        RowNumb++;
                    }
                }
                else
                {
                    while (grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCBY].Index] == null))
                        {
                            grdCI[RowNumb, (int)CI.gCBY] = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                            grdCI[RowNumb, (int)CI.gCDT] = DateTime.Now;
                        }
                        RowNumb++;
                    }
                }
                if (chkManage.Checked == false)
                    return;
                int chkHotelID, curHotelID, newAmendNo;
                string[] UniqueID = new string[DTHot.Rows.Count];
                string[] SelHot = new string[DTHot.Rows.Count];
                string[] SChkIn = new string[DTHot.Rows.Count];
                string[] MealFor = new string[DTHot.Rows.Count];
                string SID;
                RowNumb = 1;
                int CurNo;
                bool HasChecked = false;
                if (DTHot.Rows.Count > 0)
                {
                    RowNumb = 0;
                    CurNo = 0;
                    while (DTHot.Rows.Count > RowNumb)
                    {
                        if (DTHot.Rows[RowNumb]["Select"].ToString() == "")
                        {
                            RowNumb++;
                            continue;
                        }
                        if (Convert.ToBoolean(DTHot.Rows[RowNumb]["Select"]))
                        {
                            UniqueID[CurNo] = DTHot.Rows[RowNumb]["UniqueID"].ToString();
                            SelHot[CurNo] = DTHot.Rows[RowNumb]["HotelID"].ToString();
                            SChkIn[CurNo] = DTHot.Rows[RowNumb]["CheckIn"].ToString().Substring(0, 10).Trim();
                            MealFor[CurNo] = DTHot.Rows[RowNumb]["MealFor"].ToString();
                            CurNo++;
                            HasChecked = true;
                        }
                        RowNumb++;
                    }
                }
                if (HasChecked == false)
                {
                    MessageBox.Show("Please Select Hotels To Make Changes.\nSaved Without Changes", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (UniqueID[0] == null) return;
                for (int i = 0; i < UniqueID.Length; i++)
                {
                    if (UniqueID[i] == null)
                        continue;
                    SID = UniqueID[i];
                    chkHotelID = Convert.ToInt32(UniqueID[i].ToString().Trim());
                    RowNumb = 1;
                    string nm, nm1;
                    while (grdCI[RowNumb, grdCI.Cols[(int)CI.gIDN].Index] != null)
                    {
                        nm = "OTH" + (RowNumb);
                        User_Controls.ucTransOther u = dicOthers[nm];
                        nm1 = ("HTL" + RowNumb).ToString().Trim();
                        curHotelID = Int32.Parse(grdCI[RowNumb, (int)CI.gIDN].ToString());
                        if (chkComplementary.Checked == true && rdbAmend.Checked == true && chkHotelID == curHotelID && dicHotels[nm1].grdMealSup[1, (int)MS.gMTM] + "".Trim() != "")//&& SChkIn[i] == RChkIn && MealFor[i] == CurMealFor)
                        {
                            u.rtbAmendNo.Text = grdCI[RowNumb, (int)CI.gVNO].ToString();
                            newAmendNo = Int32.Parse(grdCI[RowNumb, (int)CI.gATM].ToString()) + 1; ;
                            grdCI[RowNumb, (int)CI.gANO] = 7;
                            grdCI[RowNumb, (int)CI.gATM] = newAmendNo;
                            grdCI[RowNumb, (int)CI.gVNO] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            grdCI[RowNumb, (int)CI.gCAN] = false;
                            HotelVoucher[RowNumb - 1] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            break;
                        }
                        else if (chkComplementary.Checked == true && rdbAmend.Checked == true && chkHotelID == curHotelID)//&& SChkIn[i] == RChkIn && MealFor[i] == CurMealFor)
                        {
                            u.rtbAmendNo.Text = grdCI[RowNumb, (int)CI.gVNO].ToString();
                            newAmendNo = Int32.Parse(grdCI[RowNumb, (int)CI.gATM].ToString()) + 1; ;
                            grdCI[RowNumb, (int)CI.gANO] = 8;
                            grdCI[RowNumb, (int)CI.gATM] = newAmendNo;
                            grdCI[RowNumb, (int)CI.gVNO] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            grdCI[RowNumb, (int)CI.gCAN] = false;
                            HotelVoucher[RowNumb - 1] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            break;
                        }
                        if (rdbAmend.Checked == true && chkHotelID == curHotelID && dicHotels[nm1].grdMealSup[1, (int)MS.gMTM] + "".Trim() != "")//&& SChkIn[i] == RChkIn && MealFor[i] == CurMealFor)
                        {
                            u.rtbAmendNo.Text = grdCI[RowNumb, (int)CI.gVNO].ToString();
                            newAmendNo = Int32.Parse(grdCI[RowNumb, (int)CI.gATM].ToString()) + 1; ;
                            grdCI[RowNumb, (int)CI.gANO] = 2;
                            grdCI[RowNumb, (int)CI.gATM] = newAmendNo;
                            grdCI[RowNumb, (int)CI.gVNO] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            grdCI[RowNumb, (int)CI.gCAN] = false;
                            if (grdCI[RowNumb, (int)CI.gMID] != null)
                                grdCI[RowNumb, (int)CI.gMID] = grdCI[RowNumb, (int)CI.gMID].ToString();
                            HotelVoucher[RowNumb - 1] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            break;
                        }
                        else if (rdbAmend.Checked == true && chkHotelID == curHotelID)//&& SChkIn[i] == RChkIn && MealFor[i] == CurMealFor)
                        {
                            u.rtbAmendNo.Text = grdCI[RowNumb, (int)CI.gVNO].ToString();
                            newAmendNo = Int32.Parse(grdCI[RowNumb, (int)CI.gATM].ToString()) + 1; ;
                            grdCI[RowNumb, (int)CI.gANO] = 1;
                            grdCI[RowNumb, (int)CI.gATM] = newAmendNo;
                            grdCI[RowNumb, (int)CI.gVNO] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            grdCI[RowNumb, (int)CI.gCAN] = false;
                            if (grdCI[RowNumb, (int)CI.gMID] != null)
                                grdCI[RowNumb, (int)CI.gMID] = grdCI[RowNumb, (int)CI.gMID].ToString();
                            HotelVoucher[RowNumb - 1] = (SystemCode + "/" + RowNumb + "/" + (char)((newAmendNo - 1) + 65)).Trim();
                            break;
                        }
                        if (chkComplementary.Checked == true && chkHotelID == curHotelID && dicHotels[nm1].grdMealSup[1, (int)MS.gMTM] + "".Trim() != "")//&& SChkIn[i] == RChkIn && chkMeal.Checked == true && MealFor[i] == CurMealFor)
                        {
                            grdCI[RowNumb, (int)CI.gANO] = 90;
                            grdCI[RowNumb, (int)CI.gATM] = 0;
                            grdCI[RowNumb, (int)CI.gVNO] = SystemCode + "/" + RowNumb;
                            grdCI[RowNumb, (int)CI.gCAN] = false;
                            grdCI[RowNumb, (int)CI.gMID] = cmbMealTime.SelectedValue.ToString().Trim();
                            HotelVoucher[RowNumb - 1] = SystemCode + "/" + RowNumb;
                            break;
                        }
                        else if (chkComplementary.Checked == true && chkHotelID == curHotelID)//&& SChkIn[i] == RChkIn && chkMeal.Checked == false && MealFor[i] == CurMealFor)
                        {
                            grdCI[RowNumb, (int)CI.gANO] = 99;
                            grdCI[RowNumb, (int)CI.gATM] = 0;
                            grdCI[RowNumb, (int)CI.gVNO] = SystemCode + "/" + RowNumb;
                            grdCI[RowNumb, (int)CI.gCAN] = false;
                            grdCI[RowNumb, (int)CI.gMID] = null;
                            HotelVoucher[RowNumb - 1] = SystemCode + "/" + RowNumb;
                            break;
                        }
                        if (rdbCancel.Checked == true && chkHotelID == curHotelID && dicHotels[nm1].grdMealSup[1, (int)MS.gMTM] + "".Trim() != "")//&& SChkIn[i] == RChkIn && chkMeal.Checked == true && MealFor[i] == CurMealFor)
                        {
                            grdCI[RowNumb, (int)CI.gANO] = 999;
                            grdCI[RowNumb, (int)CI.gCAN] = true;
                            grdCI[RowNumb, (int)CI.gMID] = dicHotels[nm1].grdMealSup[1, (int)MS.gMID].ToString().Trim();
                            break;
                        }
                        else if (rdbCancel.Checked == true && chkHotelID == curHotelID)//&& SChkIn[i] == RChkIn && MealFor[i] == CurMealFor)
                        {
                            grdCI[RowNumb, (int)CI.gANO] = 9;
                            grdCI[RowNumb, (int)CI.gCAN] = true;
                            grdCI[RowNumb, (int)CI.gMID] = null;
                            break;
                        }
                        if (chkHotelID == curHotelID && dicHotels[nm1].grdMealSup[1, (int)MS.gMTM] + "".Trim() != "")//&& SChkIn[i] == RChkIn && MealFor[i] == CurMealFor)
                        {
                            grdCI[RowNumb, (int)CI.gANO] = 2;
                            grdCI[RowNumb, (int)CI.gMID] = dicHotels[nm1].grdMealSup[1, (int)MS.gMID] + "".Trim();
                            grdCI[RowNumb, (int)CI.gCAN] = false;
                        }
                        RowNumb++;
                    }
                    grdCI[RowNumb, (int)CI.gMBY] = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                    grdCI[RowNumb, (int)CI.gMDT] = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void Grd_Initializer()
        {
            try
            {
                db.GridInit(grdDriver, DR.gIDN, "ID", 0, DR.gDID, "Driver ID", 0, DR.gDCD, "Code", 0, DR.gDCM, "Company", 0, DR.gDNM, "Name", 151, true, DR.gDGL, "License No", 0, DR.gTEL, "Telephone", 129, DR.gIPD, "Is Paid", 0, Type.GetType(" System.Boolean"));
                db.GridInit(grdDBasic, false, DB.gDID, 0, "Driver ID", DB.gDNM, 100, "Driver Name", true, DB.gADT, 0, "Arr Date", Type.GetType(" System.DateTime"), DB.gATM, 0, "Arr Time", DB.gAFL, 0, "Arr Flight", DB.gDDT, 0, "Dep Date", Type.GetType(" System.DateTime"), DB.gDTM, 0, "Dep Time", DB.gDFL, 0, "Dep Flight", DB.gIDC, 0, "Cancel Driver", Type.GetType(" System.Boolean"), DB.gEXC, 70, "Excursion", Type.GetType(" System.Boolean"), DB.gEXD, 300, "Excursion Desc", DB.gEXA, 100, "Excur Amt", DB.gSMT, 0, "Start Meter", DB.gEMT, 0, "End Meter", DB.gTKM, 0, "Tot KM", DB.gRKM, 0, "Rate(KM)", DB.gBAT, 0, "Bata", DB.gNON, 0, "Nights", DB.gIPD, 0, "Is Paid", Type.GetType(" System.Boolean"), DB.gNPD, 0, "Not Paid", Type.GetType(" System.Boolean"), DB.gCNF, 0, "Is Confirm", Type.GetType(" System.Boolean"), DB.gPDT, 0, "Paid Date", Type.GetType(" System.DateTime"), DB.gPBY, 100, "Paid By", DB.gRMK, 300, "Remarks");
                db.GridInit(grdGudie, 500, false, GD.gIDN, 0, "ID", GD.gGID, 0, "Guide ID", GD.gDID, 0, "Match Driver ID", GD.gGCD, 0, "Code", GD.gGNM, 131, "Name", true, GD.gFEE, 0, "Fee", GD.gNOD, 0, "Days", GD.gGLC, 0, "License No", GD.gTEL, 103, "Telephone", GD.gICN, 0, "Is Cancelled", Type.GetType(" System.Boolean"), GD.gIPD, 0, "Is Paid", Type.GetType(" System.Boolean"), GD.gPDT, 0, "Paid Date", Type.GetType(" System.DateTime"), GD.gPBY, 0, "Paid By", GD.gNPD, 0, "Not Paid", Type.GetType(" System.Boolean"), GD.gCNF, 0, "Is Confirm", Type.GetType(" System.Boolean"));
                db.GridInit(grdMatch, MG.gDID, 0, "Driver ID", MG.gDNM, 126, "Driver Name", true, MG.gGID, 0, "Guide ID", MG.gGNM, 131, "Guide Name", true, MG.gADT, 68, "Arr Date", Type.GetType(" System.DateTime"), MG.gATM, 72, "Arr Time", MG.gAFL, 69, "Arr Flight", MG.gDDT, 72, "Dep Date", Type.GetType(" System.DateTime"), MG.gDTM, 68, "Dep Time", MG.gDFL, 68, "Dep Flight", MG.gIDC, 88, "Cancel Driver", Type.GetType(" System.Boolean"), MG.gIGC, 89, "Cancel Guide", Type.GetType(" System.Boolean"), MG.gIPD, 00, "Is Driver Paid", Type.GetType(" System.Boolean"), MG.gIPG, 00, "Is Guide Paid");
                db.GridInit(grdCI, true, CI.gIDN, 0, "ID", CI.gCID, 0, "City ID", CI.gCTY, 140, "City Name", true, CI.gDTI, 90, "Date In", Type.GetType(" System.DateTime"), CI.gNOD, 85, "No Of Nights", CI.gDTO, 90, "Date Out", Type.GetType(" System.DateTime"), CI.gHID, 0, "Hotel ID", CI.gHNM, 231, "Hotel Name", true, CI.gGNM, 0, "Guest Name", CI.gCNC, 110, "Confirm Code", CI.gCON, 110, "Confirm By", CI.gCAN, 0, "Cancel Tour", Type.GetType(" System.Boolean"), CI.gNOA, 0, "No Of Adult", "##", CI.gNOC, 0, "No Of Child", "##", CI.gNOG, 0, "No Of Guide", "##", CI.gFOC, 0, "No Of FOC", "##", CI.gNAP, 0, "No Of Apartments", CI.gVNO, 0, "VoucherNo", CI.gBNO, 0, "BillNo", CI.gCNO, 0, "ChkNo", CI.gANO, 0, "AmedmentNo", CI.gATM, 0, "Amedment Time", "##", CI.gMID, 0, "Meal Time ID", "#", CI.gSCI, 0, "Sightseeing Category ID", CI.gSCN, 0, "Sightseeing Categoray", true, CI.gCST, 0, "Hotel Cost", "#####.##", CI.gCBY, 0, "Created By", CI.gCDT, 0, "Created Date", Type.GetType(" System.DateTime"), CI.gMBY, 0, "Modified By", CI.gMDT, 0, "Modified Date", Type.GetType(" System.DateTime"), CI.gOAMT, 0, CI.gRMK, 0, CI.gAMC, 0, "AdultMealCost", CI.gCMC, 0, "ChildMealCost", CI.gGMC, 0, "GuideMealCost", CI.gADV, 0, "Advance", CI.gCMS, 0, "Commission", CI.gCNR, 0, "ConRate", CI.gTOT, 0, "Total", CI.gFAD, 0, "FOCAdult", CI.gFCD, 0, "FOCChild", CI.gPCI, 0, "PaidCurID", CI.gGCI, 0, "GuideCurID", CI.gGCR, 0, "GuideConRate", CI.gIPD, 0, "IsPaid", Type.GetType(" System.Boolean"), CI.gPBY, 0, "PaidBy", CI.gPDT, 0, "PaidDate", CI.gCNF, 0, "ConfirmPaid", CI.gCNB, 0, "PaidConfirmBy", CI.gCND, 0, "ConfirmDate", CI.gDPY, 0, "Direct Pay", Type.GetType(" System.Boolean"));
                db.GridInit(grdShopping, true, SD.gCID, 0, "City ID", SD.gCTY, 200, "City Name", true, SD.gSID, 0, "Shop ID", SD.gSNM, 507, "Shop Name", true, SD.gTSL, 150, "Total Sales", "##.##");
                db.GridInit(grdAge, AG.gAFI, "Age From ID", 0, AG.gAFR, "Age From", 80, true, AG.gATI, "Age To ID", 0, AG.gATO, "Age To", 80, true, AG.gCNT, "Count", 129, "###");
                db.GridInit(grdItems, true, TI.gIID, 60, "Item ID", TI.gIDR, 0, "Is Driver", Type.GetType(" System.Boolean"), TI.gName, 60, "Name", true, TI.gINM, 109, "Item", true, TI.gNOI, 105, "No Of Items", "###", TI.gREC, 00, "Received", "###", TI.gRTN, 90, "Return", "###");
                db.GridInit(grdSim, true, SM.gSNO, 205, "Sim Card No", SM.gMNO, 150, "Mobile No", SM.gCOM, 100, "Is Complete", Type.GetType(" System.Boolean"));
                db.GridInit(grdScratch, true, SC.gSNO, 254, "Scratch Card Serial No", SC.gAMT, 100, "Amount", SC.gSDT, 100, "Date", Type.GetType(" System.DateTime"));
                db.GridInit(grdTExpense, true, TP.gIDN, 0, "ID", TP.gIDR, 60, "IsDriver", Type.GetType(" System.Boolean"), TP.gDID, 0, "Driver/Guide ID", TP.gDNM, 100, "Name", true, TP.gEID, 0, "Expense ID", TP.gENM, 100, "Expense Name", true, TP.gUNT, 100, "Units", "##.##", TP.gAMT, 100, "Amount", "##.##", TP.gIPD, 0, "Is Paid", Type.GetType(" System.Boolean"), TP.gNPD, 0, "Not Paid", Type.GetType(" System.Boolean"));
                db.GridInit(grdTAdvance, 100, TA.gIDN, 0, "ID", TA.gIDR, 60, "Is Driver", Type.GetType(" System.Boolean"), TA.gDID, 0, "Guide/Driver ID", TA.gDNM, 266, "Guide/Driver Name", true, TA.gEID, 0, "Expense ID", TA.gENM, 378, "Expense Name", TA.gAMT, 111, "Expense Amount", "##.##", TA.gIPD, 45, "IsPaid", Type.GetType(" System.Boolean"), TA.gPDT, 0, "Paid Date", Type.GetType(" System.DateTime"), TA.gPBY, 00, "Paid By", TA.gIST, 0, "Settled", Type.GetType(" System.Boolean"), TA.gSDT, 0, "Settled Date", Type.GetType(" System.DateTime"), TA.gSBY, 0, "Settled By", TA.gNPD, 00, "NotPaid", Type.GetType(" System.Boolean"));
                db.GridInit(grdTR, 500, TR.gTR, 0, "Tans Type ID", TR.gTN, 120, "Tans Type", true, TR.gVO, 0, "Voucher ID", TR.gHT, 0, "Hotel Name", true, TR.gDT, 70, "Date", Type.GetType(" System.DateTime"), TR.gTM, 0, "Time", TR.gFI, 0, "From ID", TR.gFR, 100, "From", true, TR.gTI, 0, "To ID", TR.gTO, 100, "To", true, TR.gVI, 0, "Vehicle ID", TR.gVN, 100, "Vehicle", TR.gDI, 0, "Driver ID", TR.gDN, 130, "Driver", true, TR.gGI, 0, "Guide ID", TR.gGN, 130, "Guide", true, TR.gDS, 70, "Distance", TR.gCH, 100, "Cost", "##.#");
                db.GridInit(grdTE, 500, false, TE.gEXN, 301, "Expense Name", TE.gVAT, 60, "VAT %", "##.#", TE.gTAX, 60, "Tax %", "##.#", TE.gSCH, 120, "Service Charges %", "##.#", TE.gPRC, 140, "Amount Without Tax", "##.#", TE.gTOT, 176, "Total", "##.#", TE.gRMK, 0, "Remarks");
                db.GridInit(grdOE, OE.gEXN, "Expense Name", 180, OE.gVAT, "VAT %", 49, "##.#", OE.gTAX, "Tax %", 47, "##.#", OE.gSCH, "Service Charges %", 00, "##.#", OE.gPRC, "Amount", 85, "##.#", OE.gTOT, "Total", 84, "##.#", OE.gPID, "Paid", 40, Type.GetType(" System.Boolean"), OE.gPDT, "Paid Date", 67, Type.GetType(" System.DateTime"), OE.gIBP, "Bank", 40, Type.GetType(" System.Boolean"), OE.gCNO, "Chk No", 78, OE.gRMK, "Remarks", 190);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                MessageBox.Show(ex.Message, msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Fill_Control()
        {
            try
            { 
             dtpDeparture.Value =    dtpArrival.Value =         Tourist_Management.Classes.clsGlobal.CurDate(); 
                drpCountry.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Country FROM mst_Country Where IsNull(IsActive,0)=1 ORDER BY ID");
                if (drpCountry.SelectedValue.ToString() != "") drpAgent.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_AgentDetails Where IsNull(IsActive,0)=1 AND CountryID=" + Convert.ToInt32(drpCountry.SelectedValue.ToString()) + " ORDER BY Name");
                else drpAgent.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_AgentDetails Where IsNull(IsActive,0)=1 ORDER BY Name");
               drpArivalAirport.DataSource = drpDepartAirport.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_Airport Where IsNull(IsActive,0)=1 ORDER BY Name");
                cmbReportType.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,ReportType FROM mst_ReportTypes Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpMarketingDep.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                 cmbMealTime.DataSource =   Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_MealTime Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void Fill_Grids()
        {
            try
            {
                NoOfAdult = Convert.ToInt32(nudAdult.Value);
                NoOfChild = Convert.ToInt32(nudChild.Value);
                grdAge[1, (int)AG.gAFI] = 1;
                grdAge[1, (int)AG.gAFR] = 0;
                grdAge[1, (int)AG.gATI] = 3;
                grdAge[1, (int)AG.gATO] = 2;
                grdAge[1, (int)AG.gCNT] = "0";
                grdAge[2, (int)AG.gAFI] = 4;
                grdAge[2, (int)AG.gAFR] = 3;
                grdAge[2, (int)AG.gATI] = 13;
                grdAge[2, (int)AG.gATO] = 12;
                grdAge[2, (int)AG.gCNT] = NoOfChild.ToString(); ;
                grdAge[3, (int)AG.gAFI] = 14;
                grdAge[3, (int)AG.gAFR] = 13;
                grdAge[3, (int)AG.gATI] = 15;
                grdAge[3, (int)AG.gATO] = 120;
                grdAge[3, (int)AG.gCNT] = NoOfAdult.ToString();
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;
            Generate_Other_Expenses();
            if (Save_Pro() == true)
            {
                chkManage.Checked = false;
                Mode = 1;
                Fill_Data();
                if (chkPrint.Checked)
                {
                    int reportid = Convert.ToInt32(cmbReportType.SelectedValue.ToString().Trim());
                    Print_Transaction_Report(SystemCode, reportid);
                    Generate_Email_Options();
                    Fill_Hotel_Name_In_Increase_Amend();
                    IsPreview = true;//MARK AS PREVIEWED ONE TIME AT LEAST
                }
            }
        }
        private void Generate_Email_Options()
        {
            try
            {
                string CCMails = Get_CC_Emails();
                Set_Hotel_EmailAddresses();
                RowNumb = 1;
                string nm;
                while (grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    nm = "EML" + (RowNumb).ToString().Trim();
                    User_Controls.ucTransEmail u = dicEmail[nm];
                    if (Classes.clsGlobal.FileName[RowNumb - 1] == null) return;
                    if (Classes.clsGlobal.FileName[RowNumb - 1].ToString().Trim() != "") u.txtSubject.Text = Classes.clsGlobal.FileName[RowNumb - 1].Substring(0, Classes.clsGlobal.FileName[RowNumb - 1].Length - 4);
                    if (Classes.clsGlobal.FileName[RowNumb - 1].ToString().Trim() != "") u.txtCC.Text = CCMails;
                    RowNumb++;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private string Get_CC_Emails()
        {
            try
            {
                string CC = "";
                ssql = "SELECT Email FROM mst_CCEmails WHERE IsNull(IsActive,0)=1 AND UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + " ORDER BY SrNo";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        CC += DT.Rows[RowNumb]["Email"].ToString() + ";";
                        RowNumb++;
                    }
                }
                ssql = "SELECT Email FROM mst_MarketingDep WHERE ID=" + drpMarketingDep.SelectedValue.ToString() + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        CC += DT.Rows[RowNumb]["Email"].ToString();
                        RowNumb++;
                    }
                }
                return CC;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void Set_Hotel_EmailAddresses()
        {
            try
            {
                RowNumb = 0;
                DataTable DT;
                int AllRows = 1, HotelRow = 0;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if (grdCI[AllRows, grdCI.Cols[(int)CI.gHID].Index] != null) HotelRow++;
                    AllRows++;
                }
                string nm;
                while (RowNumb <= (HotelRow - 1))
                {
                    ssql = "SELECT Email FROM mst_HotelDetails WHERE ID=" + Convert.ToInt32(HotelID[RowNumb].ToString().Trim()) + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    nm = "EML" + (RowNumb + 1);
                    User_Controls.ucTransEmail u = dicEmail[nm];
                    if (DT.Rows.Count > 0)    u.txtTo.Text = DT.Rows[0]["Email"].ToString().Trim(); 
                    string body;
                    if (Int32.Parse(grdCI[RowNumb + 1, (int)CI.gANO].ToString()) == 9)   body = "Dear,\t\t<br/><br/>\nPlease find the attached cancellation voucher for the above reservation!\t\t <br/><br/>\nThanks & regards,\t\t<br/><br/>\n";
                    else if (Int32.Parse(grdCI[RowNumb + 1, (int)CI.gANO].ToString()) == 1)  body = "Dear,\t\t<br/><br/>Please find the attached amended voucher for the above reservation!\t\t <br/><br/>\nI would be thankful if you could send the confirmation at your earliest.\t\t<br/><br/>\nLook forward to hearing from you,\t\t<br/><br/>\nThanks & regards,\t\t<br/><br/>\n";
                      else body = "Dear,\t\t<br/><br/>\nPlease find the attached voucher for the above reservation!\t\t <br/><br/>\nI would be thankful if you could send the confirmation at your earliest.\t\t<br/><br/>\nLook forward to hearing from you,\t\t<br/><br/>\nThanks & regards,\t\t<br/><br/>\n";
                     body = "<p style=\"font-family: \"Calibri(Body)\">\n\n" + body + "\n</p>";
                    u.rtbBody.Text = body;
                    RowNumb++;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        public void Print_Transaction_Report(double tourid, int reportid)
        {
            try
            {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                string sql = "";
                int ReportValue;
                DataTable DT;
                DataSet DTG = new DataSet();
                ReportDocument ga = new ReportDocument();
                string reptype = "";
                ReportValue = reportid;
                bool prePrinted = false;
                switch (ReportValue)
                {
                    case 1: db.showReportExport(new Tourist_Management.TransacReports.GroupAmend(), "SELECT TransID,VoucherNo,TourID,AmendNo,AmendTime,IsNull(MealFor,'')as MealFor,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address,TransID,Guest,NoOfAdult,NoOfChild,NoOfGuide,Company_Logo,UniqueHotelID,HotelName,CheckIn,CheckOut,RoomTypeName,RoomBasisName,Occupancy,NoOfRooms,NoOfGuideRooms,DateIn,DateArrival,DateOut,DateDeparture,NoOfFOC,NoOfApart,AmendmentTo,BillingIns,OtherIns,Notice,Reference,Arrangement,DepName,DepContact,HotelSrNo,CreatedBy,CreatedDate,ModifiedBy,LastModifiedDate,Rname1,Rno1,Rname2,Rno2,Aname1,Ano1,Aname2,Ano2,Tname1,Tno1,Tname2,Tno2, CreatedMobileNo,ModifiedMobileNo FROM vw_rpt_trn_Booking WHERE TransID=" + Convert.ToInt32(tourid) + " ORDER BY HotelSrNo", "RESERVATION"); return;
                    case 2: db.showReport2(new Tourist_Management.TransacReports.ShoppingVoucher(), "SELECT Name AS ShopName,RegNo,TotSales,ContPerson,Tel1  FROM vw_trn_Shopping WHERE TransID=" + tourid + ""); return;
                    case 5: db.showReport(new Tourist_Management.TransacReports.Transportation(), "SELECT Company_Logo,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address,ID,TourID,Guest,GuestMobile,AAirportID,AFlightNo,AFlightTime,DAirportID,DFlightNo,DFlightTime,DateArrival,DateDeparture,NoOfAdult,NoOfChild,DepName,DepContact,Date,Time,CityFrom,CityTo,HotelDateIn,HotelDateOut,HotelName,HotelCity,AmendNo,GuideName,GuideLicenseNo,GuideTel,DriverID,DriverName,IsNull(IsEmp,0) AS IsEmp,Type,VehicleNo,DriverLicenseNo,DriverTel,StartMeter,EndMeter,TotalKm,RatePerKm,Bata,NoOfNights,DriverSrNo,VoucherID,Remarks,ExcurDesc,Isnull(ExcurAmt,0)as ExcurAmt,ConfirmationCode,ConfirmBy,Tname1,Tno1,ExpenseID,Expense,Units,Amount,ShowInTR FROM vw_Transportation WHERE ID=" + tourid + " AND AmendNo!=9 ORDER BY HotelDateIn"); return;
                    case 6: db.showReport(new Tourist_Management.TransacReports.Transportation(), "SELECT Company_Logo,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address,ID,TourID,Guest,GuestMobile,AAirportID,AFlightNo,AFlightTime,DAirportID,DFlightNo,DFlightTime,DateArrival,DateDeparture,NoOfAdult,NoOfChild,DepName,DepContact,Date,Time,CityFrom,CityTo,HotelDateIn,HotelDateOut,HotelName,HotelCity,AmendNo,GuideName,GuideLicenseNo,GuideTel,DriverID,DriverName,IsNull(IsEmp,0) AS IsEmp,Type,VehicleNo,DriverLicenseNo,DriverTel,StartMeter,EndMeter,TotalKm,RatePerKm,Bata,NoOfNights,DriverSrNo,VoucherID,Remarks,ExcurDesc,Isnull(ExcurAmt,0)as ExcurAmt,ConfirmationCode,ConfirmBy,Tname1,Tno1,null ExpenseID,null Expense,null Units,null Amount, cast(0 as bit) ShowInTR  FROM vw_Transportation_ONLY WHERE ID=" + tourid + " ORDER BY DriverID"); return;
                    case 4: db.showReport2(new Tourist_Management.TransacReports.ExpenseSheet(), "SELECT TransID, Guest, HandledBy, NoOfChild, NoOfAdult, DateArrival, DateDeparture, Person, Company_Logo,DisplayName, Telephone, Mobile, Fax, E_mail, Web, Physical_Address, AdvanceName, Amount, ReturnAmt,IsSettled, TourID, Country, AgentName, PersonName, PersonID, ExpenceID, ExpenceName,ExpenseSrNo,HandledBy,Aname1,Ano1,Tname1,Tno1,MDname,UniqueID FROM vw_trn_Advances_ALL DTAdvance WHERE TransID = " + tourid + ""); return;
                    case 7:
                        DataSet DTS = new DataSets.ds_trn_TourSummary_New();
                        prePrinted = true; 
                        SqlDataAdapter DA = new SqlDataAdapter();
                        (DA = Classes.clsConnection.Fill_DataAdapter("SELECT TransID ID,VoucherNo,TourID,AmendNo,AmendTime,IsNull(MealFor,'')as MealFor,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address,TransID,Guest,NoOfAdult,NoOfChild,NoOfGuide,Company_Logo,UniqueHotelID,HotelName, ConfirmationCode,ConfirmBy, RoomTypeName,RoomBasisName,Occupancy,NoOfRooms,NoOfGuideRooms,DateIn AS DateArrival,DateOut AS DateDeparture, DateArrival Arrival,DateDeparture Departure, NoOfFOC,NoOfApart, AmendmentTo,BillingIns,OtherIns,Notice,Reference, Arrangement,DepName,DepContact,HotelSrNo,AgentName,Country FROM vw_rpt_trn_Booking  WHERE TransID=" + tourid + " ORDER BY DateIn")).Fill(DTS.Tables["dtBasics"]);
                        (DA = Classes.clsConnection.Fill_DataAdapter("SELECT ID,DriverID,DriverName,VehicleNo,LicenseNo DriverLicenseNo,Tel1 DriverTel FROM vw_trn_DriverDetails WHERE TransID=" + tourid + " AND ISNULL(IsCancelled,0)<>1")).Fill(DTS.Tables["dtDriver"]);
                        (DA = Classes.clsConnection.Fill_DataAdapter("SELECT ID,GuideID,Name GuideName,LicenseNo,Tel1 GuideTel FROM vw_trn_GuideDetails WHERE TransID=" + tourid + " AND ISNULL(IsCancelled,0)<>1")).Fill(DTS.Tables["dtGuide"]);
                        ga = new Tourist_Management.TransacReports.rpt_TourSummary_new();
                        sConnection.Print_Report_New(DTS, ga, SystemCode.ToString().Trim());
                        break;
                }
                if (!prePrinted)
                {
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                    if (DT.Rows.Count > 0)
                    {
                        sConnection.Print_Report(tourid.ToString(), sql, DTG, ga, reptype);
                    }
                    else
                        MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ConstraintException)
            {
                MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
        private void Fill_Data()
        {
            try
            {
                DataTable DT;
                string ssql;
                C1.Win.C1FlexGrid.CellStyle WARN = grdCI.Styles.Add("PAID");
                WARN.BackColor = Color.GreenYellow;
                C1.Win.C1FlexGrid.CellStyle CANCELLED = grdCI.Styles.Add("CANCELLED");
                CANCELLED.BackColor = Color.OrangeRed;
                C1.Win.C1FlexGrid.CellStyle NON = grdCI.Styles.Add("NON");
                NON.BackColor = Color.Empty;
                ssql = "SELECT AgeFromID,AgeFrom,AgeToID,AgeTo,Total FROM vw_trn_AgeDetails WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                DataTable DTAge = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DTAge.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DTAge.Rows.Count > RowNumb)
                    {
                        grdAge[RowNumb + 1, (int)AG.gAFI] = DTAge.Rows[RowNumb]["AgeFromID"].ToString();
                        grdAge[RowNumb + 1, (int)AG.gAFR] = DTAge.Rows[RowNumb]["AgeFrom"].ToString();
                        grdAge[RowNumb + 1, (int)AG.gATI] = DTAge.Rows[RowNumb]["AgeToID"].ToString();
                        grdAge[RowNumb + 1, (int)AG.gATO] = DTAge.Rows[RowNumb]["AgeTo"].ToString();
                        grdAge[RowNumb + 1, (int)AG.gCNT] = DTAge.Rows[RowNumb]["Total"].ToString();
                        RowNumb++;
                    }
                }
                ssql = "SELECT ID,GuideID,ISNULL(MatchingDriverID,0)MatchingDriverID, Code,Name,Fee,[Days] AS Days,LicenseNo,Tel1,ISNULL(IsCancelled,0)AS IsCancelled, ISNULL(IsConfirm,0)AS IsConfirm,ISNULL(NotPaid,0)AS NotPaid, ISNULL(IsPaid,0)AS IsPaid,PaidDate,PaidBy FROM vw_trn_GuideDetails WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                DataTable DTGuide = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DTGuide.Rows.Count > 0)
                {
                    RowNumb = 0;
                    int gID;
                    slGuide.Clear();
                    while (DTGuide.Rows.Count > RowNumb)
                    {
                        gID = Convert.ToInt32(DTGuide.Rows[RowNumb]["GuideID"]);
                        slGuide.Add(gID);
                        grdGudie[RowNumb + 1, (int)GD.gIDN] = DTGuide.Rows[RowNumb]["ID"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gGID] = DTGuide.Rows[RowNumb]["GuideID"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gDID] = DTGuide.Rows[RowNumb]["MatchingDriverID"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gGCD] = DTGuide.Rows[RowNumb]["Code"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gGNM] = DTGuide.Rows[RowNumb]["Name"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gFEE] = DTGuide.Rows[RowNumb]["Fee"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gNOD] = DTGuide.Rows[RowNumb]["Days"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gGLC] = DTGuide.Rows[RowNumb]["LicenseNo"].ToString();
                        grdGudie[RowNumb + 1, (int)GD.gTEL] = DTGuide.Rows[RowNumb]["Tel1"].ToString();
                        if (Convert.ToBoolean(DTGuide.Rows[RowNumb]["IsPaid"]))
                        {
                            grdGudie[RowNumb + 1, (int)GD.gIPD] = 1;
                            if (DTGuide.Rows[RowNumb]["PaidDate"] + "".Trim() != "")
                                grdGudie[RowNumb + 1, (int)GD.gPDT] = Convert.ToDateTime(DTGuide.Rows[RowNumb]["PaidDate"]);
                            if (DTGuide.Rows[RowNumb]["PaidBy"] + "".Trim() != "")
                                grdGudie[RowNumb + 1, (int)GD.gPBY] = Convert.ToInt32(DTGuide.Rows[RowNumb]["PaidBy"]);
                        }
                        grdGudie[RowNumb + 1, (int)GD.gCNF] = Convert.ToBoolean(DTGuide.Rows[RowNumb]["IsConfirm"]);
                        grdGudie[RowNumb + 1, (int)GD.gNPD] = Convert.ToBoolean(DTGuide.Rows[RowNumb]["NotPaid"]);
                        if (Convert.ToBoolean(DTGuide.Rows[RowNumb]["IsCancelled"]))
                        {
                            grdMatch[RowNumb + 1, (int)MG.gIGC] = 1;
                            grdGudie[RowNumb + 1, (int)GD.gICN] = 1;
                        }
                        else
                        {
                            grdMatch[RowNumb + 1, (int)MG.gIGC] = 0;
                            grdGudie[RowNumb + 1, (int)GD.gICN] = 0;
                        }
                        RowNumb++;
                    }
                    NoOfGuide = RowNumb;
                }
                ssql = "SELECT ID,DriverID,DriverCode,DriverName,LicenseNo,Tel1 FROM vw_trn_DriverDetails WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                DataTable DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DTDriver.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DTDriver.Rows.Count > RowNumb)
                    {
                        grdDriver[RowNumb + 1, (int)DR.gIDN] = DTDriver.Rows[RowNumb]["ID"].ToString();
                        grdDriver[RowNumb + 1, (int)DR.gDID] = DTDriver.Rows[RowNumb]["DriverID"].ToString();
                        grdDriver[RowNumb + 1, (int)DR.gDCD] = DTDriver.Rows[RowNumb]["DriverCode"].ToString();
                        grdDriver[RowNumb + 1, (int)DR.gDNM] = DTDriver.Rows[RowNumb]["DriverName"].ToString();
                        grdDriver[RowNumb + 1, (int)DR.gDGL] = DTDriver.Rows[RowNumb]["LicenseNo"].ToString();
                        grdDriver[RowNumb + 1, (int)DR.gTEL] = DTDriver.Rows[RowNumb]["Tel1"].ToString();
                        RowNumb++;
                    }
                }
                ssql = "SELECT DriverID,DriverName,GuideID,GuideName FROM vw_trn_Match_Driver_Guide WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                DataTable DTMatch = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DTMatch.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DTMatch.Rows.Count > RowNumb)
                    {
                        grdMatch[RowNumb + 1, (int)MG.gDID] = DTMatch.Rows[RowNumb]["DriverID"].ToString();
                        grdMatch[RowNumb + 1, (int)MG.gDNM] = DTMatch.Rows[RowNumb]["DriverName"].ToString();
                        grdMatch[RowNumb + 1, (int)MG.gGID] = DTMatch.Rows[RowNumb]["GuideID"].ToString();
                        grdMatch[RowNumb + 1, (int)MG.gGNM] = DTMatch.Rows[RowNumb]["GuideName"].ToString();
                        if (grdMatch[RowNumb + 1, (int)MG.gDID] + "".Trim() != "")
                        {
                            if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ISNULL(IsPaid,0)AS IsPaid FROM trn_BasicTransport WHERE TransID=" + SystemCode + " AND DriverID=" + DTMatch.Rows[RowNumb]["DriverID"].ToString().Trim() + "").Rows.Count > 0)
                            {
                                if (Convert.ToBoolean(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ISNULL(IsPaid,0)AS IsPaid FROM trn_BasicTransport WHERE TransID=" + SystemCode + " AND DriverID=" + DTMatch.Rows[RowNumb]["DriverID"].ToString().Trim() + "").Rows[0]["IsPaid"]))
                                {
                                    grdMatch[RowNumb + 1, (int)MG.gIPD] = 1;
                                    grdMatch.Rows[RowNumb + 1].Style = grdCI.Styles["PAID"];
                                }
                            }
                        }
                        RowNumb++;
                    }
                }
                else
                {
                    RowNumb = 0;
                    while (DTDriver.Rows.Count > RowNumb)
                    {
                        grdMatch[RowNumb + 1, (int)MG.gDID] = DTDriver.Rows[RowNumb]["DriverID"].ToString();
                        grdMatch[RowNumb + 1, (int)MG.gDNM] = DTDriver.Rows[RowNumb]["DriverName"].ToString();
                        RowNumb++;
                    }
                    RowNumb = 0;
                    while (DTGuide.Rows.Count > RowNumb)
                    {
                        grdMatch[RowNumb + 1, (int)MG.gGID] = DTGuide.Rows[RowNumb]["GuideID"].ToString();
                        grdMatch[RowNumb + 1, (int)MG.gGNM] = DTGuide.Rows[RowNumb]["Name"].ToString();
                        RowNumb++;
                    }
                }
                ssql = " SELECT ID,CompID,TourID,Guest,GuestMobile,AgentID,AAirportID,AFlightNo,AFlightTime,DAirportID,DFlightNo,DFlightTime, CountryID,DateArrival,DateDeparture,NoOfAdult,NoOfChild,NoOfSingle,NoOfDouble,NoOfTriple,NoOfTwin, Total,MarketingDep,Remarks,IsNull(IsLock,0)AS IsLock,IsNull(IsCancelled,0)AS IsCancelled, IsNull(TransportOnly,0)AS TransportOnly, IsNull(CompanyTransport,0)AS CompanyTransport FROM trn_GroupAmendment Where ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    if (Convert.ToBoolean(DT.Rows[0]["IsLock"])) IsLock = true;
                    if (Convert.ToBoolean(DT.Rows[0]["IsCancelled"]))  lblCancelled.Visible = chkCanclTour.Checked = true;
                    else
                    {
                        if (Classes.clsGlobal.Check_For_TourCompleteness(SystemCode.ToString().Trim()))
                        {
                            lblCancelled.Text = "TOUR IS COMPLETED";
                            lblCancelled.ForeColor = Color.Green;
                            lblCancelled.Visible = true;
                            foreach (TabPage tp in tcGroupAmend.TabPages) tp.Enabled = false;
                            btnOk.Enabled = false;
                        }
                        else lblCancelled.Visible =  chkCanclTour.Checked = false;
                    }
                    Mode = 1;
                    SystemCode = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    txtTourID.Text = DT.Rows[0]["TourID"].ToString();
                    txtGuest.Text = DT.Rows[0]["Guest"].ToString();
                    txtGuestMobile.Text = DT.Rows[0]["GuestMobile"].ToString();
                    if (DT.Rows[0]["AgentID"].ToString() != "")
                    {
                        drpAgent.setSelectedValue(DT.Rows[0]["AgentID"].ToString());
                        drpAgent.Enabled = false;
                    }
                    chkCompany.Checked = true;
                    cmbCompany.SelectedValue = Convert.ToInt32(DT.Rows[0]["CompID"].ToString());
                    if (DT.Rows[0]["AAirportID"].ToString() != "") drpArivalAirport.setSelectedValue(DT.Rows[0]["AAirportID"].ToString());
                    if (DT.Rows[0]["AFlightNo"].ToString() != "") txtArivalFlightNo.Text = DT.Rows[0]["AFlightNo"].ToString();
                    if (DT.Rows[0]["AFlightTime"].ToString() != "") mtbArrivalTime.Text = DT.Rows[0]["AFlightTime"].ToString();
                    if (DT.Rows[0]["DAirportID"].ToString() != "") drpDepartAirport.setSelectedValue(DT.Rows[0]["DAirportID"].ToString());
                    if (DT.Rows[0]["DFlightNo"].ToString() != "") txtDepartFlightNo.Text = DT.Rows[0]["DFlightNo"].ToString();
                    if (DT.Rows[0]["DFlightTime"].ToString() != "") mtbDepartureTime.Text = DT.Rows[0]["DFlightTime"].ToString();
                    if (DT.Rows[0]["CountryID"].ToString() != "")
                    {
                        drpCountry.setSelectedValue(DT.Rows[0]["CountryID"].ToString());
                        drpCountry.Enabled = false;
                    }
                    if (DT.Rows[0]["NoOfSingle"].ToString() != "") nudSingle.Value = Convert.ToInt32(DT.Rows[0]["NoOfSingle"].ToString());
                    if (DT.Rows[0]["NoOfDouble"].ToString() != "") nudDouble.Value = Convert.ToInt32(DT.Rows[0]["NoOfDouble"].ToString());
                    if (DT.Rows[0]["NoOfTriple"].ToString() != "") nudTriple.Value = Convert.ToInt32(DT.Rows[0]["NoOfTriple"].ToString());
                    if (DT.Rows[0]["NoOfTwin"].ToString() != "") nudTwin.Value = Convert.ToInt32(DT.Rows[0]["NoOfTwin"].ToString());
                    if (DT.Rows[0]["CompanyTransport"].ToString() != "" && Convert.ToBoolean(DT.Rows[0]["CompanyTransport"]) == true)  chkCompanyTr.Checked = true; 
                    if (DT.Rows[0]["TransportOnly"].ToString() != "" && Convert.ToBoolean(DT.Rows[0]["TransportOnly"]) == true)  chkTrOnly.Checked = true; 
                    Check_For_Saarc_Country();
                    if (DT.Rows[0]["DateArrival"].ToString() != "")
                    {
                        chkArrival.Checked = true;
                        dtpArrival.Value = Convert.ToDateTime(DT.Rows[0]["DateArrival"].ToString());
                        DateArrival = Convert.ToDateTime(DT.Rows[0]["DateArrival"].ToString());
                    }
                    if (DT.Rows[0]["DateDeparture"].ToString() != "")
                    {
                        chkDeparture.Checked = true;
                        dtpDeparture.Value = Convert.ToDateTime(DT.Rows[0]["DateDeparture"].ToString());
                        DateDeparture = Convert.ToDateTime(DT.Rows[0]["DateDeparture"].ToString());
                    }
                    NoOfAdult = 0; NoOfChild = 0;
                    if (DT.Rows[0]["NoOfAdult"] + "" != "") NoOfAdult = Convert.ToInt32(DT.Rows[0]["NoOfAdult"].ToString());
                    if (DT.Rows[0]["NoOfChild"] + "" != "") NoOfChild = Convert.ToInt32(DT.Rows[0]["NoOfChild"].ToString());
                    nudAdult.Value = NoOfAdult;
                    nudChild.Value = NoOfChild;
                    if (DT.Rows[0]["MarketingDep"].ToString() != "") drpMarketingDep.setSelectedValue(DT.Rows[0]["MarketingDep"].ToString());
                    txtRemarks.Text = DT.Rows[0]["Remarks"].ToString(); 
                    string nm;
                    ssql = "SELECT ID,CityID,DateIn,Dateout,City,HotelID,HotelName,GuestName,ConfirmationCode,ConfirmBy,Single,[Double] AS Dble,Triple,Twin, NoOfAdult,NoOfChild,NoOfGuide,NoOfFOC,NoOfApart,VoucherID AS VoucherNo,ISNULL(BillNo,'')AS BillNo, ISNULL(ChkNo,'')AS ChkNo,SrNo,AmendNo,AmendTime,MealTime,MealTimeName,CatID,CatName, BillingIns,OtherIns,Notice,AmendmentTo,Reference,Arrangement,Cost, CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate,OtherAmt, AdultMealCost,ChildMealCost,GuideMealCost,Advance,Commission,ConRate,Total,FOCAdult,FOCChild, PaidCurID,GuideCurID,GuideConRate,IsPaid,PaidBy,PaidDate,ConfirmPaid,PaidConfirmBy,ConfirmDate,ISNULL(DirectPay,0)AS DirectPay FROM vw_trn_CityItinerary WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    HotelsCannotBeEdited = "";
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            if (DT.Rows[RowNumb]["IsPaid"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gIPD] = DT.Rows[RowNumb]["IsPaid"].ToString();
                            if (DT.Rows[RowNumb]["ID"] + "".Trim() != "") grdCI[RowNumb + 1, (int)CI.gIDN] = Convert.ToInt32(DT.Rows[RowNumb]["ID"].ToString());
                            if (DT.Rows[RowNumb]["CityID"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCID] = Convert.ToInt32(DT.Rows[RowNumb]["CityID"].ToString());
                            if (DT.Rows[RowNumb]["City"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCTY] = DT.Rows[RowNumb]["City"].ToString();
                            if (DT.Rows[RowNumb]["DateIn"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gDTI] = Convert.ToDateTime(DT.Rows[RowNumb]["DateIn"].ToString());
                            if (DT.Rows[RowNumb]["Dateout"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gDTO] = Convert.ToDateTime(DT.Rows[RowNumb]["Dateout"].ToString());
                            if (DT.Rows[RowNumb]["DateIn"].ToString() != "" && DT.Rows[RowNumb]["Dateout"].ToString() != "")
                            {
                                TimeSpan tspan = Convert.ToDateTime(DT.Rows[RowNumb]["Dateout"].ToString()) - Convert.ToDateTime(DT.Rows[RowNumb]["DateIn"].ToString());
                                grdCI[RowNumb + 1, (int)CI.gNOD] = Convert.ToInt32(tspan.TotalDays.ToString());
                            }
                            if (DT.Rows[RowNumb]["HotelID"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gHID] = Convert.ToInt32(DT.Rows[RowNumb]["HotelID"].ToString());
                            if (DT.Rows[RowNumb]["HotelName"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gHNM] = DT.Rows[RowNumb]["HotelName"].ToString();
                            if (DT.Rows[RowNumb]["GuestName"] + "".Trim() != "") grdCI[RowNumb + 1, (int)CI.gGNM] = DT.Rows[RowNumb]["GuestName"].ToString();
                            else grdCI[RowNumb + 1, (int)CI.gGNM] = txtGuest.Text;
                            if (DT.Rows[RowNumb]["ConfirmationCode"] + "".Trim() != "") grdCI[RowNumb + 1, (int)CI.gCNC] = DT.Rows[RowNumb]["ConfirmationCode"].ToString();
                            if (DT.Rows[RowNumb]["ConfirmBy"] + "".Trim() != "") grdCI[RowNumb + 1, (int)CI.gCON] = DT.Rows[RowNumb]["ConfirmBy"].ToString();
                            grdCI[RowNumb + 1, (int)CI.gDPY] = Convert.ToBoolean(DT.Rows[RowNumb]["DirectPay"]);
                            if (DT.Rows[RowNumb]["NoOfAdult"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gNOA] = DT.Rows[RowNumb]["NoOfAdult"].ToString();
                            if (DT.Rows[RowNumb]["NoOfChild"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gNOC] = DT.Rows[RowNumb]["NoOfChild"].ToString();
                            if (DT.Rows[RowNumb]["NoOfGuide"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gNOG] = DT.Rows[RowNumb]["NoOfGuide"].ToString();
                            if (DT.Rows[RowNumb]["NoOfFOC"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gFOC] = DT.Rows[RowNumb]["NoOfFOC"].ToString();
                            if (DT.Rows[RowNumb]["NoOfApart"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gNAP] = DT.Rows[RowNumb]["NoOfApart"].ToString();
                            if (DT.Rows[RowNumb]["VoucherNo"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gVNO] = DT.Rows[RowNumb]["VoucherNo"].ToString();
                            if (DT.Rows[RowNumb]["BillNo"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gBNO] = DT.Rows[RowNumb]["BillNo"].ToString();
                            if (DT.Rows[RowNumb]["ChkNo"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCNO] = DT.Rows[RowNumb]["ChkNo"].ToString();
                            if (DT.Rows[RowNumb]["AmendNo"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gANO] = DT.Rows[RowNumb]["AmendNo"].ToString();
                            if (DT.Rows[RowNumb]["AmendTime"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gATM] = DT.Rows[RowNumb]["AmendTime"].ToString();
                            if (Convert.ToInt32(DT.Rows[RowNumb]["AmendNo"]) == 9) grdCI[RowNumb + 1, (int)CI.gCAN] = true;
                            if (DT.Rows[RowNumb]["CatID"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gSCI] = Convert.ToInt32(DT.Rows[RowNumb]["CatID"].ToString());
                            if (DT.Rows[RowNumb]["CatName"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gSCN] = DT.Rows[RowNumb]["CatName"].ToString();
                            if (DT.Rows[RowNumb]["Cost"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCST] = DT.Rows[RowNumb]["Cost"].ToString();
                            if (DT.Rows[RowNumb]["CreatedBy"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCBY] = DT.Rows[RowNumb]["CreatedBy"].ToString();
                            if (DT.Rows[RowNumb]["CreatedDate"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCDT] = Convert.ToDateTime(DT.Rows[RowNumb]["CreatedDate"].ToString());
                            if (DT.Rows[RowNumb]["LastModifiedBy"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gMBY] = DT.Rows[RowNumb]["LastModifiedBy"].ToString();
                            if (DT.Rows[RowNumb]["LastModifiedDate"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gMDT] = Convert.ToDateTime(DT.Rows[RowNumb]["LastModifiedDate"].ToString());
                            Create_Hotel_Sightseeing_Grids();
                            nm = "HTL" + (RowNumb + 1);
                            User_Controls.ucHotelNavigation u = dicHotels[nm];
                            u.Grd_Initializer();
                            if (DT.Rows[RowNumb]["MealTime"] + "".Trim() != "")
                            {
                                grdCI[RowNumb + 1, (int)CI.gMID] = DT.Rows[RowNumb]["MealTime"].ToString();
                                u.grdMealSup[1, (int)MS.gMID] = DT.Rows[RowNumb]["MealTime"].ToString();
                            }
                            if (DT.Rows[RowNumb]["MealTimeName"] + "".Trim() != "")
                            {
                                u.grdMealSup[1, (int)MS.gMTM] = DT.Rows[RowNumb]["MealTimeName"].ToString();
                            }
                            if (DT.Rows[RowNumb]["AdultMealCost"] + "".Trim() != "")
                            {
                                grdCI[RowNumb + 1, (int)CI.gAMC] = DT.Rows[RowNumb]["AdultMealCost"].ToString();
                                u.grdMealSup[1, (int)MS.gAMC] = DT.Rows[RowNumb]["AdultMealCost"].ToString();
                            }
                            if (DT.Rows[RowNumb]["ChildMealCost"] + "".Trim() != "")
                            {
                                grdCI[RowNumb + 1, (int)CI.gCMC] = DT.Rows[RowNumb]["ChildMealCost"].ToString();
                                u.grdMealSup[1, (int)MS.gCMC] = DT.Rows[RowNumb]["ChildMealCost"].ToString();
                            }
                            if (DT.Rows[RowNumb]["GuideMealCost"] + "".Trim() != "")
                            {
                                grdCI[RowNumb + 1, (int)CI.gCMC] = DT.Rows[RowNumb]["GuideMealCost"].ToString();
                                u.grdMealSup[1, (int)MS.gGMC] = DT.Rows[RowNumb]["GuideMealCost"].ToString();
                            }
                            if (DT.Rows[RowNumb]["Advance"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gADV] = DT.Rows[RowNumb]["Advance"].ToString();
                            if (DT.Rows[RowNumb]["Commission"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCMS] = DT.Rows[RowNumb]["Commission"].ToString();
                            if (DT.Rows[RowNumb]["ConRate"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCNR] = DT.Rows[RowNumb]["ConRate"].ToString();
                            if (DT.Rows[RowNumb]["Total"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gTOT] = DT.Rows[RowNumb]["Total"].ToString();
                            if (DT.Rows[RowNumb]["FOCAdult"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gFAD] = DT.Rows[RowNumb]["FOCAdult"].ToString();
                            if (DT.Rows[RowNumb]["FOCChild"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gFCD] = DT.Rows[RowNumb]["FOCChild"].ToString();
                            if (DT.Rows[RowNumb]["PaidCurID"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gPCI] = DT.Rows[RowNumb]["PaidCurID"].ToString();
                            if (DT.Rows[RowNumb]["GuideCurID"] + "".ToString().Trim() != "") grdCI[RowNumb + 1, (int)CI.gGCI] = DT.Rows[RowNumb]["GuideCurID"].ToString();
                            if (DT.Rows[RowNumb]["GuideConRate"] + "".ToString().Trim() != "") grdCI[RowNumb + 1, (int)CI.gGCR] = DT.Rows[RowNumb]["GuideConRate"].ToString();
                            if (DT.Rows[RowNumb]["OtherAmt"] + "".ToString().Trim() != "") grdCI[RowNumb + 1, (int)CI.gOAMT] = DT.Rows[RowNumb]["OtherAmt"].ToString();
                            if (DT.Rows[RowNumb]["PaidBy"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gPBY] = DT.Rows[RowNumb]["PaidBy"].ToString();
                            if (DT.Rows[RowNumb]["PaidDate"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gPDT] = Convert.ToDateTime(DT.Rows[RowNumb]["PaidDate"].ToString());
                            if (DT.Rows[RowNumb]["ConfirmPaid"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCNF] = DT.Rows[RowNumb]["ConfirmPaid"].ToString();
                            if (DT.Rows[RowNumb]["PaidConfirmBy"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCBY] = DT.Rows[RowNumb]["PaidConfirmBy"].ToString();
                            if (DT.Rows[RowNumb]["ConfirmDate"].ToString() != "") grdCI[RowNumb + 1, (int)CI.gCND] = Convert.ToDateTime(DT.Rows[RowNumb]["ConfirmDate"].ToString());
                            if (DT.Rows[RowNumb]["AmendNo"].ToString().Trim() == "9" || DT.Rows[RowNumb]["AmendNo"].ToString().Trim() == "999")    grdCI.Rows[RowNumb + 1].Style = grdCI.Styles["CANCELLED"];
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]))
                            {
                                grdCI.Rows[RowNumb + 1].Style = grdCI.Styles["PAID"];
                                HotelsCannotBeEdited += DT.Rows[RowNumb]["HotelName"].ToString() + "( " + DT.Rows[RowNumb]["DateIn"].ToString().Substring(0, 10).Trim() + " )" + "\n";
                            }
                            RowNumb++;
                        }
                        if (IsLock)
                        {
                            DataTable dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT UserGroupID FROM vw_CurrentUserDetails Where UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()));
                            if (HotelsCannotBeEdited.Length > 0)
                                if (dt.Rows[0]["UserGroupID"].ToString() != "1001")//----- Check if the user is an admin(user grp 1001). admin can modify settled records
                                    MessageBox.Show("Please Notice That Below Hotels Cannot Be Modified.\n====================================\n" + HotelsCannotBeEdited.Substring(0, HotelsCannotBeEdited.Length - 1) + "", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        } 
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            nm = "OTH" + (RowNumb + 1);
                            User_Controls.ucTransOther u = dicOthers[nm];
                            u.rtbBillingIns.Text = DT.Rows[RowNumb]["BillingIns"] + "";
                            u.rtbOtherInstructions.Text = DT.Rows[RowNumb]["OtherIns"] + "";
                            u.rtbNotice.Text = DT.Rows[RowNumb]["Notice"] + "";
                            u.rtbAmendNo.Text = DT.Rows[RowNumb]["AmendmentTo"] + "";
                            u.rtbReferance.Text = DT.Rows[RowNumb]["Reference"] + "";
                            u.rtbArrangement.Text = DT.Rows[RowNumb]["Arrangement"] + "";
                            RowNumb++;
                        }
                        Generate_Transport_Expenses();
                    }
                    ssql = "SELECT CityID,City,ShopID,Name,TotSales  FROM vw_trn_Shopping WHERE TransID=" + SystemCode + " ORDER BY SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            if (DT.Rows[RowNumb]["CityID"].ToString() != "") grdShopping[RowNumb + 1, (int)SD.gCID] = Convert.ToInt32(DT.Rows[RowNumb]["CityID"].ToString());
                            if (DT.Rows[RowNumb]["City"].ToString() != "") grdShopping[RowNumb + 1, (int)SD.gCTY] = DT.Rows[RowNumb]["City"].ToString();
                            if (DT.Rows[RowNumb]["ShopID"].ToString() != "") grdShopping[RowNumb + 1, (int)SD.gSID] = Convert.ToInt32(DT.Rows[RowNumb]["ShopID"].ToString());
                            if (DT.Rows[RowNumb]["Name"].ToString() != "") grdShopping[RowNumb + 1, (int)SD.gSNM] = DT.Rows[RowNumb]["Name"].ToString();
                            if (DT.Rows[RowNumb]["TotSales"].ToString() != "") grdShopping[RowNumb + 1, (int)SD.gTSL] = Convert.ToDecimal(DT.Rows[RowNumb]["TotSales"].ToString());
                            RowNumb++;
                        }
                    }
                    Create_Hotel_Sightseeing_Grids();
                    string Ssql;
                    int CurNo, TabNo, AmdNo;
                    string VID, eraly_ChkIn, eraly_ChkOut;
                    Ssql = "SELECT VoucherID,TransID,HotelID,BillNo,CheckIn,CheckOut,RoomTypeID,RoomTypeName,RoomBasisID,RoomBasisName, OccupancyID,Occupancy,NoOfRooms,NoOfGuideRooms,ExtraBed,Vat,Tax,ServCharge,CostWithoutTax,Cost,SrNo,TabNo, FOCRooms,EbedModiCost,ModifiedCost,GuideCost  FROM vw_trn_HotelExpenses WHERE TransID=" + SystemCode + " ORDER BY TabNo,SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(Ssql);
                    Create_Hotel_Sightseeing_Grids();
                    if (DT.Rows.Count > 0)
                    {
                        if (DT.Rows[0][0].ToString() != null && DT.Rows[0][0].ToString() != "")
                        {
                            CurNo = 0;
                            RowNumb = 0;
                            TabNo = Convert.ToInt16(DT.Rows[RowNumb]["TabNo"].ToString());
                            while (DT.Rows.Count > CurNo)
                            {
                                nm = "HTL" + (TabNo);
                                User_Controls.ucHotelNavigation u = dicHotels[nm];
                                VID = DT.Rows[CurNo]["VoucherID"] + "".Trim();
                                AmdNo = Convert.ToInt32(Classes.clsGlobal.objCon.Fill_Table("SELECT AmendNo FROM trn_CityItinerary WHERE VoucherID='" + VID.Trim() + "'").Rows[0]["AmendNo"]);
                                if (AmdNo == 9 || AmdNo == 999) u.lblCancelled.Visible = true;
                                else u.lblCancelled.Visible = false;
                                if (DT.Rows[CurNo]["CheckIn"] + "".Trim() != "")
                                    u.grdHotel[RowNumb + 1, (int)HG.gCHI] = Convert.ToDateTime(DT.Rows[CurNo]["CheckIn"]);
                                else
                                {
                                    eraly_ChkIn = Classes.clsGlobal.objCon.Fill_Table("SELECT DateIn FROM trn_CityItinerary WHERE VoucherID='" + VID.Trim() + "'").Rows[0]["DateIn"] + "".Trim();
                                    if (eraly_ChkIn.Trim() != "") u.grdHotel[RowNumb + 1, (int)HG.gCHI] = Convert.ToDateTime(eraly_ChkIn);
                                }
                                if (DT.Rows[CurNo]["CheckOut"] + "".Trim() != "")
                                    u.grdHotel[RowNumb + 1, (int)HG.gCHO] = Convert.ToDateTime(DT.Rows[CurNo]["CheckOut"]);
                                else
                                {
                                    eraly_ChkOut = Classes.clsGlobal.objCon.Fill_Table("SELECT DateOut FROM trn_CityItinerary WHERE VoucherID='" + VID.Trim() + "'").Rows[0]["DateOut"] + "".Trim();
                                    if (eraly_ChkOut.Trim() != "") u.grdHotel[RowNumb + 1, (int)HG.gCHO] = Convert.ToDateTime(eraly_ChkOut);
                                }
                                if (DT.Rows[CurNo]["BillNo"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gBNO] = DT.Rows[CurNo]["BillNo"].ToString();
                                if (DT.Rows[CurNo]["RoomTypeID"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gRTI] = DT.Rows[CurNo]["RoomTypeID"].ToString();
                                if (DT.Rows[CurNo]["RoomTypeName"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gRTN] = DT.Rows[CurNo]["RoomTypeName"].ToString();
                                if (DT.Rows[CurNo]["RoomBasisID"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gRBI] = DT.Rows[CurNo]["RoomBasisID"].ToString();
                                if (DT.Rows[CurNo]["RoomBasisName"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gRBN] = DT.Rows[CurNo]["RoomBasisName"].ToString();
                                if (DT.Rows[CurNo]["OccupancyID"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gMID] = DT.Rows[CurNo]["OccupancyID"].ToString();
                                if (DT.Rows[CurNo]["Occupancy"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gMAX] = DT.Rows[CurNo]["Occupancy"].ToString();
                                if (DT.Rows[CurNo]["NoOfRooms"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gNOR] = DT.Rows[CurNo]["NoOfRooms"].ToString();
                                if (DT.Rows[CurNo]["NoOfGuideRooms"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gNGR] = DT.Rows[CurNo]["NoOfGuideRooms"].ToString();
                                if (DT.Rows[CurNo]["ExtraBed"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gEBD] = DT.Rows[CurNo]["ExtraBed"].ToString();
                                if (DT.Rows[CurNo]["Vat"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gVAT] = DT.Rows[CurNo]["Vat"].ToString();
                                if (DT.Rows[CurNo]["Tax"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gTAX] = DT.Rows[CurNo]["Tax"].ToString();
                                if (DT.Rows[CurNo]["ServCharge"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gSCH] = DT.Rows[CurNo]["ServCharge"].ToString();
                                if (DT.Rows[CurNo]["CostWithoutTax"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gPRI] = DT.Rows[CurNo]["CostWithoutTax"].ToString();
                                if (DT.Rows[CurNo]["Cost"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gTPR] = DT.Rows[CurNo]["Cost"].ToString();
                                if (DT.Rows[RowNumb]["FOCRooms"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gFOC] = DT.Rows[CurNo]["FOCRooms"].ToString();
                                if (DT.Rows[RowNumb]["EbedModiCost"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gMEC] = DT.Rows[CurNo]["EbedModiCost"].ToString();
                                if (DT.Rows[RowNumb]["ModifiedCost"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gMRC] = DT.Rows[CurNo]["ModifiedCost"].ToString();
                                if (DT.Rows[RowNumb]["GuideCost"].ToString() != "") u.grdHotel[RowNumb + 1, (int)HG.gGRC] = DT.Rows[CurNo]["GuideCost"].ToString();
                                CurNo++;
                                if (DT.Rows.Count > CurNo)
                                {
                                    if (TabNo == Convert.ToInt16(DT.Rows[CurNo]["TabNo"].ToString())) RowNumb++;
                                    else
                                    {
                                        TabNo = Convert.ToInt16(DT.Rows[CurNo]["TabNo"].ToString());
                                        RowNumb = 0;
                                    }
                                }
                            }
                        }
                    }
                    ssql = "SELECT ID,ISNULL(IsDriver,0)AS IsDriver,DriverID,DriverName,ExpenseID,Expense,Amount, ISNULL(IsPaid,0) AS IsPaid,PaidDate,PaidBy, IsNull(IsSettled,0)AS IsSettled,SettledDate,SettledBy, ISNULL(NotPaid,0) AS NotPaid FROM vw_trn_Tour_Advance WHERE TransID=" + SystemCode + " ORDER BY SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    grdTAdvance.Rows.Count = 1;
                    grdTAdvance.Rows.Count = 500;
                    RowNumb = 0;
                    if (DT.Rows.Count > 0)
                    {
                        while (DT.Rows.Count > RowNumb)
                        {
                            grdTAdvance[RowNumb + 1, (int)TA.gIDN] = Convert.ToInt32(DT.Rows[RowNumb]["ID"]);
                            if (DT.Rows[RowNumb]["IsDriver"].ToString() != "") grdTAdvance[RowNumb + 1, (int)TA.gIDR] = DT.Rows[RowNumb]["IsDriver"].ToString();
                            if (DT.Rows[RowNumb]["DriverID"].ToString() != "") grdTAdvance[RowNumb + 1, (int)TA.gDID] = DT.Rows[RowNumb]["DriverID"].ToString();
                            if (DT.Rows[RowNumb]["DriverName"].ToString() != "") grdTAdvance[RowNumb + 1, (int)TA.gDNM] = DT.Rows[RowNumb]["DriverName"].ToString();
                            if (DT.Rows[RowNumb]["ExpenseID"].ToString() != "") grdTAdvance[RowNumb + 1, (int)TA.gEID] = DT.Rows[RowNumb]["ExpenseID"].ToString();
                            if (DT.Rows[RowNumb]["Expense"].ToString() != "") grdTAdvance[RowNumb + 1, (int)TA.gENM] = DT.Rows[RowNumb]["Expense"].ToString();
                            if (DT.Rows[RowNumb]["Amount"].ToString() != "") grdTAdvance[RowNumb + 1, (int)TA.gAMT] = DT.Rows[RowNumb]["Amount"].ToString();
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]))
                            {
                                grdTAdvance[RowNumb + 1, (int)TA.gIPD] = 1;
                                if (DT.Rows[RowNumb]["PaidDate"] + "".Trim() != "") grdTAdvance[RowNumb + 1, (int)TA.gPDT] = Convert.ToDateTime(DT.Rows[RowNumb]["PaidDate"]);
                                if (DT.Rows[RowNumb]["PaidBy"] + "".Trim() != "") grdTAdvance[RowNumb + 1, (int)TA.gPBY] = DT.Rows[RowNumb]["PaidBy"].ToString();
                            }
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsSettled"]))
                            {
                                grdTAdvance[RowNumb + 1, (int)TA.gIST] = 1;
                                if (DT.Rows[RowNumb]["SettledDate"] + "".Trim() != "") grdTAdvance[RowNumb + 1, (int)TA.gSDT] = Convert.ToDateTime(DT.Rows[RowNumb]["SettledDate"]);
                                if (DT.Rows[RowNumb]["SettledBy"] + "".Trim() != "") grdTAdvance[RowNumb + 1, (int)TA.gSBY] = DT.Rows[RowNumb]["SettledBy"].ToString();
                            }
                            grdTAdvance[RowNumb + 1, (int)TA.gNPD] = DT.Rows[RowNumb]["NotPaid"].ToString();
                            RowNumb++;
                        }
                    }
                    ssql = "SELECT ID,ISNULL(IsDriver,0)AS IsDriver,DriverID,DriverName,ExpenseID,Expense,Amount,ISNULL(IsPaid,0) AS IsPaid,PaidDate,PaidBy,IsNull(IsSettled,0)AS IsSettled,SettledDate,SettledBy,ISNULL(NotPaid,0) AS NotPaid FROM vw_trn_Tour_Advance_Driver WHERE TransID=" + SystemCode + " ORDER BY SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        int currow = RowNumb;//ASSIGNED EARLIER ROW NUMBER......GUIDE ALSO MAY HAD THE ADVANCES
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            grdTAdvance[currow + 1, (int)TA.gIDN] = Convert.ToInt32(DT.Rows[RowNumb]["ID"]);
                            if (DT.Rows[RowNumb]["IsDriver"].ToString() != "") grdTAdvance[currow + 1, (int)TA.gIDR] = DT.Rows[RowNumb]["IsDriver"].ToString();
                            if (DT.Rows[RowNumb]["DriverID"].ToString() != "") grdTAdvance[currow + 1, (int)TA.gDID] = DT.Rows[RowNumb]["DriverID"].ToString();
                            if (DT.Rows[RowNumb]["DriverName"].ToString() != "") grdTAdvance[currow + 1, (int)TA.gDNM] = DT.Rows[RowNumb]["DriverName"].ToString();
                            if (DT.Rows[RowNumb]["ExpenseID"].ToString() != "") grdTAdvance[currow + 1, (int)TA.gEID] = DT.Rows[RowNumb]["ExpenseID"].ToString();
                            if (DT.Rows[RowNumb]["Expense"].ToString() != "") grdTAdvance[currow + 1, (int)TA.gENM] = DT.Rows[RowNumb]["Expense"].ToString();
                            if (DT.Rows[RowNumb]["Amount"].ToString() != "") grdTAdvance[currow + 1, (int)TA.gAMT] = DT.Rows[RowNumb]["Amount"].ToString();
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]))
                            {
                                grdTAdvance[currow + 1, (int)TA.gIPD] = 1;
                                if (DT.Rows[RowNumb]["PaidDate"] + "".Trim() != "") grdTAdvance[currow + 1, (int)TA.gPDT] = Convert.ToDateTime(DT.Rows[RowNumb]["PaidDate"]);
                                if (DT.Rows[RowNumb]["PaidBy"] + "".Trim() != "") grdTAdvance[currow + 1, (int)TA.gPBY] = DT.Rows[RowNumb]["PaidBy"].ToString();
                            }
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsSettled"]))
                            {
                                grdTAdvance[currow + 1, (int)TA.gIST] = 1;
                                if (DT.Rows[RowNumb]["SettledDate"] + "".Trim() != "") grdTAdvance[currow + 1, (int)TA.gSDT] = Convert.ToDateTime(DT.Rows[RowNumb]["SettledDate"]);
                                if (DT.Rows[RowNumb]["SettledBy"] + "".Trim() != "") grdTAdvance[currow + 1, (int)TA.gSBY] = DT.Rows[RowNumb]["SettledBy"].ToString();
                            }
                            grdTAdvance[currow + 1, (int)TA.gNPD] = DT.Rows[RowNumb]["NotPaid"].ToString();
                            currow++;
                            RowNumb++;
                        }
                    }
                    ssql = "SELECT TransTypeID,TransTypeName,IsNull(VoucherID,'')AS VoucherID,[Date],[Time],FromID,CityFrom,ToID,CityTo,VehicleNo,DriverID,DriverName,GuideID,GuideName,Distance,Cost FROM vw_trn_Transport WHERE TransID=" + SystemCode + " ORDER BY SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            if (DT.Rows[RowNumb]["TransTypeID"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gTR] = DT.Rows[RowNumb]["TransTypeID"].ToString();
                            if (DT.Rows[RowNumb]["TransTypeName"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gTN] = DT.Rows[RowNumb]["TransTypeName"].ToString();
                            if (DT.Rows[RowNumb]["VoucherID"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gVO] = DT.Rows[RowNumb]["VoucherID"].ToString();
                            if (DT.Rows[RowNumb]["Date"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gDT] = DT.Rows[RowNumb]["Date"].ToString();
                            if (DT.Rows[RowNumb]["Time"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gTM] = DT.Rows[RowNumb]["Time"].ToString();
                            if (DT.Rows[RowNumb]["FromID"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gFI] = DT.Rows[RowNumb]["FromID"].ToString();
                            if (DT.Rows[RowNumb]["CityFrom"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gFR] = DT.Rows[RowNumb]["CityFrom"].ToString();
                            if (DT.Rows[RowNumb]["ToID"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gTI] = DT.Rows[RowNumb]["ToID"].ToString();
                            if (DT.Rows[RowNumb]["CityTo"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gTO] = DT.Rows[RowNumb]["CityTo"].ToString();
                            if (DT.Rows[RowNumb]["VehicleNo"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gVN] = DT.Rows[RowNumb]["VehicleNo"].ToString();
                            if (DT.Rows[RowNumb]["DriverID"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gDI] = DT.Rows[RowNumb]["DriverID"].ToString();
                            if (DT.Rows[RowNumb]["DriverName"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gDN] = DT.Rows[RowNumb]["DriverName"].ToString();
                            if (DT.Rows[RowNumb]["GuideID"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gGI] = DT.Rows[RowNumb]["DriverID"].ToString();
                            if (DT.Rows[RowNumb]["GuideName"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gGN] = DT.Rows[RowNumb]["GuideName"].ToString();
                            if (DT.Rows[RowNumb]["Distance"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gDS] = DT.Rows[RowNumb]["Distance"].ToString();
                            if (DT.Rows[RowNumb]["Cost"].ToString() != "") grdTR[RowNumb + 1, (int)TR.gCH] = DT.Rows[RowNumb]["Cost"].ToString();
                            RowNumb++;
                        }
                        Generate_Transport_Expenses();
                    }
                    ssql = "SELECT TransID,TransItemID,TransItem,NoOfItems,Received,[Return] FROM vw_trn_TransportItems WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                    DataTable DTTransItems = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DTTransItems.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DTTransItems.Rows.Count > RowNumb)
                        {
                            grdItems[RowNumb + 1, (int)TI.gIID] = DTTransItems.Rows[RowNumb]["TransItemID"].ToString();
                            grdItems[RowNumb + 1, (int)TI.gINM] = DTTransItems.Rows[RowNumb]["TransItem"].ToString();
                            grdItems[RowNumb + 1, (int)TI.gNOI] = DTTransItems.Rows[RowNumb]["NoOfItems"].ToString();
                            grdItems[RowNumb + 1, (int)TI.gREC] = DTTransItems.Rows[RowNumb]["Received"].ToString();
                            grdItems[RowNumb + 1, (int)TI.gRTN] = DTTransItems.Rows[RowNumb]["Return"].ToString();
                            RowNumb++;
                        }
                    }
                    ssql = " SELECT DriverID,Name,Isnull(Excursion,0) as Excursion,ExcurDesc,Isnull(ExcurAmt,0)as ExcurAmt, StartMeter,EndMeter,TotalKm,RatePerKm,Bata,NoOfNights, ISNULL(NotPaid,0)AS NotPaid,ISNULL(IsConfirm,0)as IsConfirm, ArrivalDate,ArrivalTime,ArrivalFlight,DeprtDate,DepartTime,DepartFlight,ISNULL(IsCancelled,0)AS IsCancelled, Isnull(IsPaid,0)AS IsPaid,PaidDate,PaidBy,Remarks FROM vw_trn_BasicTransport Where TransID=" + SystemCode + " ORDER BY SrNo";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            grdDBasic[RowNumb + 1, (int)DB.gDID] = DT.Rows[RowNumb]["DriverID"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gDNM] = DT.Rows[RowNumb]["Name"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gADT] = DT.Rows[RowNumb]["ArrivalDate"];
                            grdDBasic[RowNumb + 1, (int)DB.gATM] = DT.Rows[RowNumb]["ArrivalTime"];
                            grdDBasic[RowNumb + 1, (int)DB.gAFL] = DT.Rows[RowNumb]["ArrivalFlight"];
                            grdDBasic[RowNumb + 1, (int)DB.gDDT] = DT.Rows[RowNumb]["DeprtDate"];
                            grdDBasic[RowNumb + 1, (int)DB.gDTM] = DT.Rows[RowNumb]["DepartTime"];
                            grdDBasic[RowNumb + 1, (int)DB.gDFL] = DT.Rows[RowNumb]["DepartFlight"];
                            grdMatch[RowNumb + 1, (int)MG.gADT] = DT.Rows[RowNumb]["ArrivalDate"];
                            grdMatch[RowNumb + 1, (int)MG.gATM] = DT.Rows[RowNumb]["ArrivalTime"];
                            grdMatch[RowNumb + 1, (int)MG.gAFL] = DT.Rows[RowNumb]["ArrivalFlight"];
                            grdMatch[RowNumb + 1, (int)MG.gDDT] = DT.Rows[RowNumb]["DeprtDate"];
                            grdMatch[RowNumb + 1, (int)MG.gDTM] = DT.Rows[RowNumb]["DepartTime"];
                            grdMatch[RowNumb + 1, (int)MG.gDFL] = DT.Rows[RowNumb]["DepartFlight"];
                            grdDBasic[RowNumb + 1, (int)DB.gEXC] = DT.Rows[RowNumb]["Excursion"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gEXD] = DT.Rows[RowNumb]["ExcurDesc"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gEXA] = DT.Rows[RowNumb]["ExcurAmt"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gSMT] = DT.Rows[RowNumb]["StartMeter"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gEMT] = DT.Rows[RowNumb]["EndMeter"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gTKM] = DT.Rows[RowNumb]["TotalKm"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gRKM] = DT.Rows[RowNumb]["RatePerKm"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gBAT] = DT.Rows[RowNumb]["Bata"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gNON] = DT.Rows[RowNumb]["NoOfNights"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gRMK] = DT.Rows[RowNumb]["Remarks"].ToString();
                            grdDBasic[RowNumb + 1, (int)DB.gNPD] = Convert.ToBoolean(DT.Rows[RowNumb]["NotPaid"]);
                            grdDBasic[RowNumb + 1, (int)DB.gCNF] = Convert.ToBoolean(DT.Rows[RowNumb]["IsConfirm"]);
                            grdDBasic[RowNumb + 1, (int)DB.gIDC] = grdMatch[RowNumb + 1, (int)MG.gIDC] = Convert.ToBoolean(DT.Rows[RowNumb]["IsCancelled"]);
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]))
                            {
                                grdDBasic[RowNumb + 1, (int)DB.gIPD] = true;
                                if (DT.Rows[RowNumb]["PaidDate"] + "".Trim() != "") grdDBasic[RowNumb + 1, (int)DB.gPDT] = Convert.ToDateTime(DT.Rows[RowNumb]["PaidDate"]);
                                if (DT.Rows[RowNumb]["PaidBy"] + "".Trim() != "") grdDBasic[RowNumb + 1, (int)DB.gPBY] = Convert.ToInt32(DT.Rows[RowNumb]["PaidBy"]);
                                grdDBasic.Rows[RowNumb + 1].Style = grdCI.Styles["PAID"];
                            }
                            Colour_Driver_Guide(RowNumb + 1);
                            RowNumb++;
                        }
                    }
                    ssql = "SELECT SimNo,MobileNo FROM trn_SimDetails WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                    DataTable DTSim = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DTSim.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DTSim.Rows.Count > RowNumb)
                        {
                            grdSim[RowNumb + 1, (int)SM.gSNO] = DTSim.Rows[RowNumb]["SimNo"].ToString();
                            grdSim[RowNumb + 1, (int)SM.gMNO] = DTSim.Rows[RowNumb]["MobileNo"].ToString();
                            RowNumb++;
                        }
                    }
                    ssql = "SELECT CardNo,Amount,date  FROM trn_ScratchCardsDetails WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                    DataTable DTScartch = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DTScartch.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DTScartch.Rows.Count > RowNumb)
                        {
                            grdScratch[RowNumb + 1, (int)SC.gSNO] = DTScartch.Rows[RowNumb]["CardNo"].ToString();
                            grdScratch[RowNumb + 1, (int)SC.gAMT] = DTScartch.Rows[RowNumb]["Amount"].ToString();
                            grdScratch[RowNumb + 1, (int)SC.gSDT] = Convert.ToDateTime(DTScartch.Rows[RowNumb]["date"].ToString());
                            RowNumb++;
                        }
                    }
                    ssql = "SELECT ID,ISNULL(IsDriver,0)AS IsDriver,DriverID,ExpenseID,Expense, ISNULL(Units,0)Units, Amount,ISNULL(IsPaid,0)AS IsPaid ,ISNULL(NotPaid,0)AS NotPaid FROM vw_trn_Travel_Expenses WHERE TransID=" + SystemCode + " ORDER BY SrNo";
                    DataTable DTTravel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DTTravel.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        int driverid = 0;
                        string driverName = "";
                        while (DTTravel.Rows.Count > RowNumb)
                        {
                            if (DTTravel.Rows[RowNumb]["DriverID"].ToString() == "")
                            {
                                RowNumb++;
                                continue;
                            }
                            grdTExpense[RowNumb + 1, (int)TP.gIDN] = Convert.ToInt32(DTTravel.Rows[RowNumb]["ID"]);
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
                            grdTExpense[RowNumb + 1, (int)TP.gEID] = DTTravel.Rows[RowNumb]["ExpenseID"].ToString();
                            grdTExpense[RowNumb + 1, (int)TP.gENM] = DTTravel.Rows[RowNumb]["Expense"].ToString();
                            grdTExpense[RowNumb + 1, (int)TP.gUNT] = DTTravel.Rows[RowNumb]["Units"];
                            grdTExpense[RowNumb + 1, (int)TP.gAMT] = DTTravel.Rows[RowNumb]["Amount"].ToString();
                            grdTExpense[RowNumb + 1, (int)TP.gNPD] = Convert.ToBoolean(DTTravel.Rows[RowNumb]["NotPaid"]);
                            if (Convert.ToBoolean(DTTravel.Rows[RowNumb]["IsPaid"]))
                            {
                                grdTExpense[RowNumb + 1, (int)TP.gIPD] = 1;
                                grdTExpense.Rows[RowNumb + 1].Style = grdCI.Styles["PAID"];
                            }
                            RowNumb++;
                        }
                    }
                    ssql = "SELECT ExpenseName,Vat,Tax,ServChr,Amount,TotAmount,Remarks," +
                           "IsRepeatVat,IsRepeatTax,IsRepeatServ,ISNULL(IsPaid,0)IsPaid,PaidDate," +
                           "ISNULL(IsBankPayment,0)AS IsBankPayment,ChkNo " +
                           " FROM trn_OtherExpenses WHERE TransID=" + SystemCode + " ORDER BY SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            if (DT.Rows[RowNumb]["ExpenseName"].ToString() != "") grdOE[RowNumb + 1, (int)OE.gEXN] = DT.Rows[RowNumb]["ExpenseName"].ToString();
                            if (DT.Rows[RowNumb]["Vat"].ToString() != "") grdOE[RowNumb + 1, (int)OE.gVAT] = DT.Rows[RowNumb]["Vat"].ToString();
                            if (DT.Rows[RowNumb]["Tax"].ToString() != "") grdOE[RowNumb + 1, (int)OE.gTAX] = DT.Rows[RowNumb]["Tax"].ToString();
                            if (DT.Rows[RowNumb]["ServChr"].ToString() != "") grdOE[RowNumb + 1, (int)OE.gSCH] = DT.Rows[RowNumb]["ServChr"].ToString();
                            if (DT.Rows[RowNumb]["Amount"].ToString() != "") grdOE[RowNumb + 1, (int)OE.gPRC] = DT.Rows[RowNumb]["Amount"].ToString();
                            if (DT.Rows[RowNumb]["TotAmount"].ToString() != "") grdOE[RowNumb + 1, (int)OE.gTOT] = DT.Rows[RowNumb]["TotAmount"].ToString();
                            if (DT.Rows[RowNumb]["Remarks"].ToString() != "") grdOE[RowNumb + 1, (int)OE.gRMK] = DT.Rows[RowNumb]["Remarks"].ToString();
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]))
                            {
                                grdOE[RowNumb + 1, (int)OE.gPID] = 1;
                                if (DT.Rows[RowNumb]["PaidDate"] + "".Trim() != "") grdOE[RowNumb + 1, (int)OE.gPDT] = Convert.ToDateTime(DT.Rows[RowNumb]["PaidDate"]);
                            }
                            if (Convert.ToBoolean(DT.Rows[RowNumb]["IsBankPayment"]))
                            {
                                grdOE[RowNumb + 1, (int)OE.gIBP] = 1;
                                grdOE[RowNumb + 1, (int)OE.gCNO] = DT.Rows[RowNumb]["ChkNo"].ToString();
                            }
                            chkOEVat.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsRepeatVat"].ToString());
                            chkOETax.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsRepeatTax"].ToString());
                            chkOEServ.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsRepeatServ"].ToString());
                            RowNumb++;
                        }
                        Generate_Other_Expenses();
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private Boolean Save_Pro()
        {
            try
            {
                if (!IsRateChecked) Create_Hotel_Sightseeing_Grids();
                return Validate_Data() && Save_Procedure();
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Validate_Data()
        {
            try
            {
                return Validate_Basic_Details() && Validate_Age_Details() && Validate_Shopping_Details() && Validate_Hotel_Expenses() && Validate_Travel_Expenses();
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Validate_Travel_Expenses()
        {
            try
            {
                decimal unitCost;
                string expID;
                int RowNumb = 1;
                while (grdTExpense[RowNumb, (int)TP.gEID] + "".Trim() != "")
                {
                    if (grdTExpense[RowNumb, (int)TP.gUNT] + "".Trim() != "")
                    {
                        if (!Classes.clsGlobal.IsNumeric(grdTExpense[RowNumb, (int)TP.gUNT] + "".Trim()))
                        {
                            MessageBox.Show("Please Enter Valid Unit Amount.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        if (grdTExpense[RowNumb, (int)TP.gEID] + "".Trim() != "")
                        {
                            expID = grdTExpense[RowNumb, (int)TP.gEID].ToString();
                            unitCost = Classes.clsGlobal.ExpenseUnitCost(expID);
                            grdTExpense[RowNumb, (int)TP.gAMT] = Convert.ToDecimal(grdTExpense[RowNumb, (int)TP.gUNT]) * unitCost;
                        }
                    }
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Validate_Hotel_Expenses()
        {
            try
            {
                int AllRows = 1;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    AllRows++;
                }
                for (int i = 0; i < AllRows - 1; i++)
                {
                    RowNumb = 1;
                    string nm;
                    nm = "HTL" + (i + 1).ToString().Trim();
                    User_Controls.ucHotelNavigation u = dicHotels[nm];
                    while (u.grdHotel[RowNumb, u.grdHotel.Cols[(int)HG.gRTI].Index] != null)
                    {
                        if (u.grdHotel[RowNumb, (int)HG.gCHI] + "".Trim() == "" || u.grdHotel[RowNumb, (int)HG.gCHO] + "".Trim() == "")
                        {
                            MessageBox.Show("Check In and Out Dates Cannot Be Blank.( " + HotelName[i] + " )", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        RowNumb++;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Validate_Basic_Details()
        {
            try
            {
                if (!chkCompany.Checked)
                {
                    MessageBox.Show("Please Select a Company", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmbCompany.Select();
                    this.tcGroupAmend.SelectedTab = tpBasic;
                    return false;
                }
                if (txtGuest.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Guest Name cannot be blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtGuest.Select();
                    return false;
                }
                if (drpAgent.SelectedValue + "".Trim() == "")
                {
                    MessageBox.Show("Agent is required.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tcGroupAmend.SelectedTab = tpBasic;
                    drpAgent.Focus();
                    return false;
                }
                if (drpMarketingDep.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Marketting Dep name cannot be blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tcGroupAmend.SelectedTab = tpOthers;
                    drpMarketingDep.Focus();
                    return false;
                }
                if (nudAdult.Value.ToString().Trim() == "0" && nudChild.Value.ToString().Trim() == "0")
                {
                    if (chkTrOnly.Checked == false && chkCompanyTr.Checked == false)
                    {
                        MessageBox.Show("Total Pax cannot be Zero.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        nudAdult.Select();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Validate_Age_Details()
        {
            try
            {
                RowNumb = 1;
                if ((grdAge[RowNumb, grdAge.Cols[(int)AG.gAFI].Index] == null))
                {
                    MessageBox.Show("Please Enter Age Details", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                do
                {
                    if (grdAge[RowNumb, grdAge.Cols[(int)AG.gAFI].Index] == null)
                    {
                        MessageBox.Show("Please Select 'Age From' In Age Details.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if (grdAge[RowNumb, grdAge.Cols[(int)AG.gATI].Index] == null)
                    {
                        MessageBox.Show("Please Select 'Age To' In Age Details.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if (grdAge[RowNumb, grdAge.Cols[(int)AG.gCNT].Index] == null)
                    {
                        MessageBox.Show("Please Select 'Total Count' In Child Policy.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                } while ((grdAge[RowNumb, grdAge.Cols[(int)AG.gAFI].Index] != null));
                RowNumb = 1;
                NoOfAdult = 0;
                NoOfChild = 0;
                int AgeFrom, AgeTo, Count;
                while ((grdAge[RowNumb, grdAge.Cols[(int)AG.gAFI].Index] != null))
                {
                    AgeFrom = Convert.ToInt32(grdAge[RowNumb, (int)AG.gAFR].ToString());
                    AgeTo = Convert.ToInt32(grdAge[RowNumb, (int)AG.gATO].ToString());
                    Count = Convert.ToInt32(grdAge[RowNumb, (int)AG.gCNT].ToString());
                    if (AgeFrom == 13 && AgeTo == 120)
                        NoOfAdult += Count;
                    else
                        NoOfChild += Count;
                    RowNumb++;
                }
                nudAdult.Value = NoOfAdult;
                nudChild.Value = NoOfChild;
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Validate_Shopping_Details()
        {
            try
            {
                RowNumb = 1;
                while ((grdShopping[RowNumb, grdShopping.Cols[(int)SD.gCID].Index] != null))
                {
                    if (grdShopping[RowNumb, grdShopping.Cols[(int)SD.gSID].Index] == null)
                    {
                        MessageBox.Show("Please Select 'Shop Name' In Shopping Details.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_Procedure()
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
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_Tabs(System.Data.SqlClient.SqlCommand sqlCom)
        {
            try
            {
                setVoucherType();
                return Save_Basic_Details(sqlCom) && Save_Age_Details(sqlCom) && Save_Guide_Details(sqlCom) && Save_Driver_Details(sqlCom) && Save_Match_Details(sqlCom) && Save_CityItinerary_Details(sqlCom) && Shopping_Expenses(sqlCom) && Save_Hotel_Expenses(sqlCom) && Save_Sightseeing_Expenses(sqlCom) && Save_Tour_Advance(sqlCom) && Save_Transport_Details(sqlCom) && Save_Transport_Items(sqlCom) && Save_Basic_Transport(sqlCom) && Save_Sim_Details(sqlCom) && Save_ScratchCards_Details(sqlCom) && Save_Travel_Expenses(sqlCom) && Save_OtherExpenses_Details(sqlCom);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_Basic_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_BasicDetails";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if (AllowChange)
                    sqlCom.Parameters.Add("@AllowChange", SqlDbType.Int).Value = 1;
                else
                    sqlCom.Parameters.Add("@AllowChange", SqlDbType.Int).Value = 0;
                if (IsCancelled)
                    sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = "1";
                else
                    sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = "0";
                sqlCom.Parameters.Add("@Guest", SqlDbType.VarChar, 50).Value = txtGuest.Text.Trim();
                sqlCom.Parameters.Add("@GuestMobile", SqlDbType.NVarChar, 100).Value = txtGuestMobile.Text.Trim();
                sqlCom.Parameters.Add("@TourID", SqlDbType.VarChar, 50).Value = txtTourID.Text.Trim();
                sqlCom.Parameters["@TourID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@CompID", SqlDbType.Int).Value = Convert.ToInt32(cmbCompany.SelectedValue.ToString());
                if (drpAgent.SelectedValue.ToString() != "" && drpAgent.SelectedValue != null) sqlCom.Parameters.Add("@AgentID", SqlDbType.Int).Value = drpAgent.SelectedValue.Trim();
                if (drpArivalAirport.SelectedValue.ToString() != "" && drpArivalAirport.SelectedValue != null) sqlCom.Parameters.Add("@AAirportID", SqlDbType.Int).Value = drpArivalAirport.SelectedValue.Trim();
                if (txtArivalFlightNo.Text.ToString() != "") sqlCom.Parameters.Add("@AFlightNo", SqlDbType.VarChar, 50).Value = txtArivalFlightNo.Text.Trim();
                if (mtbArrivalTime.Text.ToString() != "") sqlCom.Parameters.Add("@AFlightTime", SqlDbType.NVarChar, 50).Value = mtbArrivalTime.Text.Trim();
                if (drpDepartAirport.SelectedValue.ToString() != "") sqlCom.Parameters.Add("@DAirportID", SqlDbType.Int).Value = drpDepartAirport.SelectedValue.Trim();
                if (txtArivalFlightNo.Text.ToString() != "") sqlCom.Parameters.Add("@DFlightNo", SqlDbType.VarChar, 50).Value = txtDepartFlightNo.Text.Trim();
                if (mtbDepartureTime.Text.ToString() != "") sqlCom.Parameters.Add("@DFlightTime", SqlDbType.NVarChar, 50).Value = mtbDepartureTime.Text.Trim();
                int count = 1;
                NoOfGuide = 0;
                while (grdGudie[count, grdShopping.Cols[(int)GD.gGID].Index] != null)
                {
                    NoOfGuide++;
                    count++;
                }
                sqlCom.Parameters.Add("@NoOfSingle", SqlDbType.Int).Value = nudSingle.Value;
                sqlCom.Parameters.Add("@NoOfDouble", SqlDbType.Int).Value = nudDouble.Value;
                sqlCom.Parameters.Add("@NoOfTriple", SqlDbType.Int).Value = nudTriple.Value;
                sqlCom.Parameters.Add("@NoOfTwin", SqlDbType.Int).Value = nudTwin.Value;
                if (drpCountry.SelectedValue.ToString() != "") sqlCom.Parameters.Add("@CountryID", SqlDbType.Int).Value = drpCountry.SelectedValue.Trim();
                if (chkArrival.Checked) sqlCom.Parameters.Add("@DateArrival", SqlDbType.DateTime).Value = dtpArrival.Value.Date;
                if (chkDeparture.Checked) sqlCom.Parameters.Add("@DateDeparture", SqlDbType.DateTime).Value = dtpDeparture.Value.Date;
                sqlCom.Parameters.Add("@NoOfAdult", SqlDbType.Int).Value = NoOfAdult;
                sqlCom.Parameters.Add("@NoOfChild", SqlDbType.Int).Value = NoOfChild;
                sqlCom.Parameters.Add("@NoOfGuide", SqlDbType.Int).Value = NoOfGuide;
                sqlCom.Parameters.Add("@Total", SqlDbType.Decimal).Value = TotalAmount;
                if (drpMarketingDep.SelectedValue.ToString() != "") sqlCom.Parameters.Add("@MarketingDep", SqlDbType.Int).Value = drpMarketingDep.SelectedValue.Trim();
                if (txtRemarks.Text.ToString() != "") sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = txtRemarks.Text.Trim();
                sqlCom.Parameters.Add("@NoTransport", SqlDbType.Int).Value = chkNoTransport.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@TransportOnly", SqlDbType.Int).Value = chkTrOnly.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@CompanyTransport", SqlDbType.Int).Value = chkCompanyTr.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = Convert.ToDouble(sqlCom.Parameters["@ID"].Value);
                    txtTourID.Text = sqlCom.Parameters["@TourID"].Value.ToString();
                    Get_VoucherNo();
                    RtnVal = true;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_Age_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_AgeDetails";
                RowNumb = 1;
                while (grdAge[RowNumb, grdAge.Cols[(int)AG.gAFI].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdAge[RowNumb, (int)AG.gAFI] != null && grdAge[RowNumb, (int)AG.gAFI].ToString() != "") sqlCom.Parameters.Add("@AgeFromID", SqlDbType.Int).Value = Int32.Parse(grdAge[RowNumb, (int)AG.gAFI].ToString());
                    if (grdAge[RowNumb, (int)AG.gATI] != null && grdAge[RowNumb, (int)AG.gATI].ToString() != "") sqlCom.Parameters.Add("@AgeToID", SqlDbType.Int).Value = Int32.Parse(grdAge[RowNumb, (int)AG.gATI].ToString());
                    if (grdAge[RowNumb, (int)AG.gCNT] != null && grdAge[RowNumb, (int)AG.gCNT].ToString() != "") sqlCom.Parameters.Add("@Total", SqlDbType.Int).Value = Int32.Parse(grdAge[RowNumb, (int)AG.gCNT].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Transport_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            try
            {
                if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] == null)) return true;
                if (Validate_Transport_Expenses() == false) return false;
                Generate_Transport_Expenses();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Transport_Details";
                while (grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTR].Index] != null)) sqlCom.Parameters.Add("@TransTypeID", SqlDbType.Int).Value = Int32.Parse(grdTR[RowNumb, (int)TR.gTR].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gVO].Index] != null)) sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 50).Value = grdTR[RowNumb, (int)TR.gVO].ToString().Trim();
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gDT].Index] != null)) sqlCom.Parameters.Add("@Date", SqlDbType.DateTime).Value = Convert.ToDateTime(grdTR[RowNumb, (int)TR.gDT].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTM].Index] != null)) sqlCom.Parameters.Add("@Time", SqlDbType.VarChar, 10).Value = grdTR[RowNumb, (int)TR.gTM].ToString();
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gFI].Index] != null)) sqlCom.Parameters.Add("@FromID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gFI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTI].Index] != null)) sqlCom.Parameters.Add("@ToID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gTI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gVI].Index] != null)) sqlCom.Parameters.Add("@VehicleID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gVI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gDI].Index] != null)) sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gDI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gGI].Index] != null)) sqlCom.Parameters.Add("@GuideID", SqlDbType.Int).Value = Convert.ToInt32(grdTR[RowNumb, (int)TR.gGI].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gDS].Index] != null)) sqlCom.Parameters.Add("@Distance", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTR[RowNumb, (int)TR.gDS].ToString());
                    if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gCH].Index] != null)) sqlCom.Parameters.Add("@Cost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTR[RowNumb, (int)TR.gCH].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_CityItinerary_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            try
            {
                if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] == null)) return true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_CityItinerary_Details";
                Get_Hotel_Cost();
                string nm;
                double CurIDN;
                while (grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    nm = "OTH" + (RowNumb);
                    User_Controls.ucTransOther u = dicOthers[nm];
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    CurIDN = Convert.ToDouble(grdCI[RowNumb, (int)CI.gIDN]);
                    if (CurIDN < 1000000)
                    {
                        sqlCom.Parameters.Add("@Flag", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gIDN].ToString());
                    }
                    else sqlCom.Parameters.Add("@Flag", SqlDbType.Int).Value = 0; 
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)) sqlCom.Parameters.Add("@CityID", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gCID].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gDTI].Index] != null)) sqlCom.Parameters.Add("@DateIn", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gDTI].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gDTO].Index] != null)) sqlCom.Parameters.Add("@DateOut", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gDTO].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gHID].Index] != null)) sqlCom.Parameters.Add("@HotelID", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gHID].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gGNM].Index] != null)) sqlCom.Parameters.Add("@GuestName", SqlDbType.NVarChar, 100).Value = grdCI[RowNumb, (int)CI.gGNM].ToString();
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCNC].Index] != null)) sqlCom.Parameters.Add("@ConfirmationCode", SqlDbType.NVarChar, 200).Value = grdCI[RowNumb, (int)CI.gCNC].ToString();
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCON].Index] != null)) sqlCom.Parameters.Add("@ConfirmBy", SqlDbType.NVarChar, 200).Value = grdCI[RowNumb, (int)CI.gCON].ToString();
                    sqlCom.Parameters.Add("@DirectPay", SqlDbType.Bit).Value = Convert.ToBoolean(grdCI[RowNumb, (int)CI.gDPY]) ? 1 : 0;
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gNOA].Index] != null)) sqlCom.Parameters.Add("@NoOfAdult", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gNOA].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gNOC].Index] != null)) sqlCom.Parameters.Add("@NoOfChild", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gNOC].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gNOG].Index] != null)) sqlCom.Parameters.Add("@NoOfGuide", SqlDbType.Int).Value = grdCI[RowNumb, (int)CI.gNOG].ToString();
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gFOC].Index] != null)) sqlCom.Parameters.Add("@NoOfFOC", SqlDbType.Int).Value = grdCI[RowNumb, (int)CI.gFOC].ToString();
                    else sqlCom.Parameters.Add("@NoOfFOC", SqlDbType.Int).Value = 0;
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gNAP].Index] != null)) sqlCom.Parameters.Add("@NoOfApr", SqlDbType.Int).Value = grdCI[RowNumb, (int)CI.gNAP].ToString();
                    else sqlCom.Parameters.Add("@NoOfApr", SqlDbType.Int).Value = 0;
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gVNO].Index] != null)) sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 100).Value = HotelVoucher[RowNumb - 1].ToString();
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gBNO].Index] != null)) sqlCom.Parameters.Add("@BillNo", SqlDbType.NVarChar, 50).Value = grdCI[RowNumb, (int)CI.gBNO].ToString();
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCNO].Index] != null)) sqlCom.Parameters.Add("@ChkNo", SqlDbType.NVarChar, 50).Value = grdCI[RowNumb, (int)CI.gCNO].ToString();
                    /*GET RESERVATION/AMENDMENT/CANCELATION VOUCHER                    0=RESERVATION VOUCHER                    1=AMENDMENT VOUCHER                    2=MEAL VOUCHER                    99=COMPLEMETARY VOUCHER                    9=CANCELATION VOUCHER                    90=COMPLEMETARY MEAL VOUCHER                    999=CANCELATION MEAL VOUCHER                    8=COMPLEMETARY AMENDMENT VOUCHER                    7=COMPLEMETARY AMENDMENT MEAL VOUCHER                      */
                    nm = "HTL" + (RowNumb).ToString().Trim();
                    User_Controls.ucHotelNavigation u1 = dicHotels[nm];
                    if (Convert.ToInt32(grdCI[RowNumb, (int)CI.gANO]) == 0 && u1.grdMealSup[1, (int)MS.gMTM] + "".Trim() != "")//....THIS IS A MEAL VOUCHER
                        sqlCom.Parameters.Add("@AmendNo", SqlDbType.Int).Value = 2;
                    else if (grdCI[RowNumb, (int)CI.gANO] + "".Trim() == "")
                        sqlCom.Parameters.Add("@AmendNo", SqlDbType.Int).Value = 0;
                    else
                        sqlCom.Parameters.Add("@AmendNo", SqlDbType.Int).Value = Convert.ToInt32(grdCI[RowNumb, (int)CI.gANO]);
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gATM].Index] != null))
                        sqlCom.Parameters.Add("@AmendTime", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gATM].ToString());
                    if (u1.grdHotel[1, (int)MS.gMID] + "".Trim() != null) sqlCom.Parameters.Add("@MealTime", SqlDbType.Int).Value = Convert.ToInt32(u1.grdMealSup[1, (int)MS.gMID]);
                    if (u1.grdHotel[1, (int)MS.gAMC] + "".Trim() != null) sqlCom.Parameters.Add("@AdultMealCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u1.grdMealSup[1, (int)MS.gAMC]);
                    if (u1.grdHotel[1, (int)MS.gCMC] + "".Trim() != null) sqlCom.Parameters.Add("@ChildMealCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u1.grdMealSup[1, (int)MS.gCMC]);
                    if (u1.grdHotel[1, (int)MS.gGMC] + "".Trim() != null) sqlCom.Parameters.Add("@GuideMealCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u1.grdMealSup[1, (int)MS.gGMC]);
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gSCI].Index] != null)) sqlCom.Parameters.Add("@CatID", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gSCI].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCST].Index] != null)) sqlCom.Parameters.Add("@Cost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gCST].ToString());
                    sqlCom.Parameters.Add("@BillingIns", SqlDbType.NVarChar, 500).Value = u.rtbBillingIns.Text.ToString();
                    sqlCom.Parameters.Add("@OtherIns", SqlDbType.NVarChar, 500).Value = u.rtbOtherInstructions.Text.ToString();
                    sqlCom.Parameters.Add("@Notice", SqlDbType.NVarChar, 500).Value = u.rtbNotice.Text.ToString();
                    sqlCom.Parameters.Add("@AmendmentTo", SqlDbType.NVarChar, 50).Value = u.rtbAmendNo.Text.ToString();
                    sqlCom.Parameters.Add("@Reference", SqlDbType.NVarChar, 500).Value = u.rtbReferance.Text.ToString();
                    sqlCom.Parameters.Add("@Arrangement", SqlDbType.NVarChar, 500).Value = u.rtbArrangement.Text.ToString();
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCBY].Index] != null)) sqlCom.Parameters.Add("@CreatedBY", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gCBY].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gCDT].Index] != null)) sqlCom.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gCDT].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gMBY].Index] != null)) sqlCom.Parameters.Add("@ModifiedBY", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gMBY].ToString());
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gMDT].Index] != null)) sqlCom.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gMDT].ToString());
                    if (grdCI[RowNumb, (int)CI.gOAMT] != null && grdCI[RowNumb, (int)CI.gOAMT].ToString() != "") sqlCom.Parameters.Add("@OtherAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gOAMT].ToString());
                    if (grdCI[RowNumb, (int)CI.gRMK] != null && grdCI[RowNumb, (int)CI.gRMK].ToString() != "") sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 1000).Value = grdCI[RowNumb, (int)CI.gRMK].ToString();
                    if (grdCI[RowNumb, (int)CI.gFAD] != null && grdCI[RowNumb, (int)CI.gFAD].ToString() != "") sqlCom.Parameters.Add("@FOCAdult", SqlDbType.Float).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gFAD].ToString());
                    if (grdCI[RowNumb, (int)CI.gFCD] != null && grdCI[RowNumb, (int)CI.gFCD].ToString() != "") sqlCom.Parameters.Add("@FOCChild", SqlDbType.Float).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gFCD].ToString());
                    if (grdCI[RowNumb, (int)CI.gADV] != null && grdCI[RowNumb, (int)CI.gADV].ToString() != "") sqlCom.Parameters.Add("@Advance", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gADV].ToString());
                    if (grdCI[RowNumb, (int)CI.gCMS] != null && grdCI[RowNumb, (int)CI.gCMS].ToString() != "") sqlCom.Parameters.Add("@Commission", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gCMS].ToString());
                    if (grdCI[RowNumb, (int)CI.gCNR] != null && grdCI[RowNumb, (int)CI.gCNR].ToString() != "") sqlCom.Parameters.Add("@ConRate", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gCNR].ToString());
                    if (grdCI[RowNumb, (int)CI.gPCI] != null && grdCI[RowNumb, (int)CI.gPCI].ToString() != "") sqlCom.Parameters.Add("@PaidCurID", SqlDbType.Int).Value = Convert.ToInt32(grdCI[RowNumb, (int)CI.gPCI].ToString());
                    if (grdCI[RowNumb, (int)CI.gGCI] != null && grdCI[RowNumb, (int)CI.gGCI].ToString() != "") sqlCom.Parameters.Add("@GuideCurID", SqlDbType.Int).Value = Convert.ToInt32(grdCI[RowNumb, (int)CI.gGCI].ToString());
                    if (grdCI[RowNumb, (int)CI.gGCR] != null && grdCI[RowNumb, (int)CI.gGCR].ToString().Trim() != "") sqlCom.Parameters.Add("@GuideConRate", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gGCR].ToString());
                    sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdCI[RowNumb, (int)CI.gIPD]);
                    if (grdCI[RowNumb, (int)CI.gPBY] != null && grdCI[RowNumb, (int)CI.gPBY].ToString().Trim() != "") sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Convert.ToInt32(grdCI[RowNumb, (int)CI.gPBY].ToString());
                    if (Convert.ToBoolean(grdCI[RowNumb, (int)CI.gCNF]))
                    {
                        sqlCom.Parameters.Add("@ConfirmPaid", SqlDbType.Int).Value = 1;
                        if (grdCI[RowNumb, (int)CI.gCNB] != null && grdCI[RowNumb, (int)CI.gCNB].ToString().Trim() != "") sqlCom.Parameters.Add("@PaidConfirmBy", SqlDbType.Int).Value = Convert.ToInt32(grdCI[RowNumb, (int)CI.gCNB].ToString());
                        if (grdCI[RowNumb, (int)CI.gCND] != null && grdCI[RowNumb, (int)CI.gCND].ToString().Trim() != "") sqlCom.Parameters.Add("@ConfirmDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gCND].ToString());
                    }
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Shopping_Expenses(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            try
            {
                if ((grdShopping[RowNumb, grdShopping.Cols[(int)SD.gCID].Index] == null)) return true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Shopping_Details";
                while (grdShopping[RowNumb, grdShopping.Cols[(int)SD.gCID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if ((grdShopping[RowNumb, grdShopping.Cols[(int)SD.gCID].Index] != null)) sqlCom.Parameters.Add("@CityID", SqlDbType.Int).Value = Int32.Parse(grdShopping[RowNumb, (int)SD.gCID].ToString());
                    if ((grdShopping[RowNumb, grdShopping.Cols[(int)SD.gSID].Index] != null)) sqlCom.Parameters.Add("@ShopID", SqlDbType.Int).Value = Int32.Parse(grdShopping[RowNumb, (int)SD.gSID].ToString());
                    if ((grdShopping[RowNumb, grdShopping.Cols[(int)SD.gTSL].Index] != null)) sqlCom.Parameters.Add("@TotSales", SqlDbType.Decimal).Value = Convert.ToDecimal(grdShopping[RowNumb, (int)SD.gTSL].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_Hotel_Expenses(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Hotel_Expenses";
                int AllRows = 1;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    AllRows++;
                }
                for (int i = 0; i < AllRows - 1; i++)
                {
                    RowNumb = 1;
                    string nm;
                    nm = "HTL" + (i + 1).ToString().Trim();
                    User_Controls.ucHotelNavigation u = dicHotels[nm];
                    while (u.grdHotel[RowNumb, u.grdHotel.Cols[(int)HG.gRTI].Index] != null)
                    {
                        sqlCom.Parameters.Clear();
                        sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                        sqlCom.Parameters.Add("@HotelID", SqlDbType.Int).Value = Convert.ToInt32(HotelID[i].ToString());
                        if (HotelVoucher[i] != null)
                            sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 50).Value = HotelVoucher[i].ToString();//FlexArray[i][RowNumb, (int)HG.gVNO].ToString();
                        sqlCom.Parameters.Add("@CheckIn", SqlDbType.DateTime).Value = Convert.ToDateTime(u.grdHotel[RowNumb, (int)HG.gCHI]);
                        sqlCom.Parameters.Add("@CheckOut", SqlDbType.DateTime).Value = Convert.ToDateTime(u.grdHotel[RowNumb, (int)HG.gCHO]);
                        if (u.grdHotel[RowNumb, (int)HG.gBNO] + "".Trim() != "") sqlCom.Parameters.Add("@BillNo", SqlDbType.NVarChar, 50).Value = u.grdHotel[RowNumb, (int)HG.gBNO].ToString();
                        if (u.grdHotel[RowNumb, (int)HG.gRTI] + "".Trim() != "") sqlCom.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value = Convert.ToInt32(u.grdHotel[RowNumb, (int)HG.gRTI].ToString());
                        if (u.grdHotel[RowNumb, (int)HG.gRBI] + "".Trim() != "") sqlCom.Parameters.Add("@RoomBasisID", SqlDbType.Int).Value = Convert.ToInt32(u.grdHotel[RowNumb, (int)HG.gRBI].ToString());
                        if (u.grdHotel[RowNumb, (int)HG.gCID] + "".Trim() != "") sqlCom.Parameters.Add("@ConditionID", SqlDbType.Int).Value = Convert.ToInt32(u.grdHotel[RowNumb, (int)HG.gCID].ToString());
                        if (u.grdHotel[RowNumb, (int)HG.gMID] + "".Trim() != "") sqlCom.Parameters.Add("@OccupancyID", SqlDbType.Int).Value = Convert.ToInt32(u.grdHotel[RowNumb, (int)HG.gMID].ToString());
                        if (u.grdHotel[RowNumb, (int)HG.gNOR] + "".Trim() != "") sqlCom.Parameters.Add("@NoOfRooms", SqlDbType.Int).Value = Convert.ToInt32(u.grdHotel[RowNumb, (int)HG.gNOR].ToString());
                        else sqlCom.Parameters.Add("@NoOfRooms", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gNGR] + "".Trim() != "") sqlCom.Parameters.Add("@GuideRooms", SqlDbType.Int).Value = Convert.ToInt32(u.grdHotel[RowNumb, (int)HG.gNGR].ToString());
                        else sqlCom.Parameters.Add("@GuideRooms", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gEBD] + "".Trim() != "") sqlCom.Parameters.Add("@ExtraBed", SqlDbType.Int).Value = Convert.ToInt32(u.grdHotel[RowNumb, (int)HG.gEBD].ToString());
                        else sqlCom.Parameters.Add("@ExtraBed", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gVAT] + "".Trim() != "") sqlCom.Parameters.Add("@Vat", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gVAT].ToString());
                        else sqlCom.Parameters.Add("@Vat", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gTAX] + "".Trim() != "") sqlCom.Parameters.Add("@Tax", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gTAX].ToString());
                        else sqlCom.Parameters.Add("@Tax", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gSCH] + "".Trim() != "") sqlCom.Parameters.Add("@ServCharge", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gSCH].ToString());
                        else sqlCom.Parameters.Add("@ServCharge", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gPRI] + "".Trim() != "") sqlCom.Parameters.Add("@CostWithoutTax", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gPRI].ToString());
                        else sqlCom.Parameters.Add("@CostWithoutTax", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gTPR] + "".Trim() != "") sqlCom.Parameters.Add("@Cost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gTPR].ToString());
                        else sqlCom.Parameters.Add("@Cost", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gFOC] + "".Trim() != "") sqlCom.Parameters.Add("@FOCRooms", SqlDbType.Float).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gFOC].ToString());
                        else sqlCom.Parameters.Add("@FOCRooms", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gMRC] + "".Trim() != "") sqlCom.Parameters.Add("@ModifiedCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gMRC].ToString());
                        else sqlCom.Parameters.Add("@ModifiedCost", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gGRC] + "".Trim() != "") sqlCom.Parameters.Add("@GuideCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gGRC].ToString());
                        else sqlCom.Parameters.Add("@GuideCost", SqlDbType.Int).Value = 0;
                        if (u.grdHotel[RowNumb, (int)HG.gMEC] + "".Trim() != "") sqlCom.Parameters.Add("@EbedCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdHotel[RowNumb, (int)HG.gMEC].ToString());
                        else sqlCom.Parameters.Add("@EbedCost", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                        sqlCom.Parameters.Add("@TabNo", SqlDbType.Int).Value = i + 1;
                        sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                        sqlCom.ExecuteNonQuery();
                        if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                        RowNumb++;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_Sightseeing_Expenses(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Sightseeing_Expenses";
                int AllRows = 1;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gSCI].Index] != null)
                {
                    AllRows++;
                }
                string nm;
                for (int i = 0; i < AllRows - 1; i++)
                {
                    RowNumb = 1;
                    nm = "SGH" + (i + 1);
                    User_Controls.ucSSNavigation u = dicSight[nm];
                    while (u.grdSE[RowNumb, u.grdSE.Cols[0].Index] != null)
                    {
                        sqlCom.Parameters.Clear();
                        sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gSSI].Index] != null) sqlCom.Parameters.Add("@SightseeingID", SqlDbType.Int).Value = Convert.ToInt32(u.grdSE[RowNumb, (int)SE.gSSI].ToString());
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gSCI].Index] != null) sqlCom.Parameters.Add("@SightCatID", SqlDbType.Int).Value = Convert.ToInt32(u.grdSE[RowNumb, (int)SE.gSCI].ToString());
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gNOA].Index] != null) sqlCom.Parameters.Add("@NoOfAdult", SqlDbType.Int).Value = Convert.ToInt32(u.grdSE[RowNumb, (int)SE.gNOA].ToString());
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gNOC].Index] != null) sqlCom.Parameters.Add("@NoOfChild", SqlDbType.Int).Value = Convert.ToInt32(u.grdSE[RowNumb, (int)SE.gNOC].ToString());
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gSAC].Index] != null) sqlCom.Parameters.Add("@SAdultCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdSE[RowNumb, (int)SE.gSAC].ToString());
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gSCC].Index] != null) sqlCom.Parameters.Add("@SChildCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdSE[RowNumb, (int)SE.gSCC].ToString());
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gNAC].Index] != null) sqlCom.Parameters.Add("@NAdultCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdSE[RowNumb, (int)SE.gNAC].ToString());
                        if (u.grdSE[RowNumb, u.grdSE.Cols[(int)SE.gNCC].Index] != null) sqlCom.Parameters.Add("@NChildCost", SqlDbType.Decimal).Value = Convert.ToDecimal(u.grdSE[RowNumb, (int)SE.gNCC].ToString());
                        if (Convert.ToBoolean(u.grdSE[RowNumb, (int)SE.gSEL])) sqlCom.Parameters.Add("@Choose", SqlDbType.Int).Value = 1;
                        else sqlCom.Parameters.Add("@Choose", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                        sqlCom.Parameters.Add("@TabNo", SqlDbType.Int).Value = i + 1;
                        sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                        sqlCom.ExecuteNonQuery();
                        if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                        RowNumb++;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_Guide_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Guide_Details";
                RowNumb = 1;
                while (grdGudie[RowNumb, grdGudie.Cols[(int)GD.gGID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdGudie[RowNumb, (int)GD.gIDN] + "".Trim() != "") sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = Int32.Parse(grdGudie[RowNumb, (int)GD.gIDN].ToString());
                    else sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = 0;
                    if (grdGudie[RowNumb, (int)GD.gGID] != null && grdGudie[RowNumb, (int)GD.gGID].ToString() != "") sqlCom.Parameters.Add("@GuideID", SqlDbType.Int).Value = Int32.Parse(grdGudie[RowNumb, (int)GD.gGID].ToString());
                    if (grdGudie[RowNumb, (int)GD.gDID] + "".Trim() != "") sqlCom.Parameters.Add("@MatchingDriverID", SqlDbType.Int).Value = Int32.Parse(grdGudie[RowNumb, (int)GD.gDID].ToString());
                    if (grdGudie[RowNumb, (int)GD.gFEE] != null && grdGudie[RowNumb, (int)GD.gFEE].ToString() != "") sqlCom.Parameters.Add("@Fee", SqlDbType.Decimal).Value = Convert.ToDecimal(grdGudie[RowNumb, (int)GD.gFEE].ToString());
                    if (grdGudie[RowNumb, (int)GD.gNOD] != null && grdGudie[RowNumb, (int)GD.gNOD].ToString() != "") sqlCom.Parameters.Add("@Days", SqlDbType.Int).Value = Int32.Parse(grdGudie[RowNumb, (int)GD.gNOD].ToString());
                    sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = Convert.ToBoolean(grdGudie[RowNumb, (int)GD.gICN]);
                    if (Convert.ToBoolean(grdGudie[RowNumb, (int)GD.gIPD]))
                    {
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = 1;
                        if (grdGudie[RowNumb, (int)GD.gPDT] + "".Trim() != "") sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdGudie[RowNumb, (int)GD.gPDT]);
                        if (grdGudie[RowNumb, (int)GD.gPBY] + "".Trim() != "") sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Convert.ToInt32(grdGudie[RowNumb, (int)GD.gPBY]);
                    }
                    sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdGudie[RowNumb, (int)GD.gNPD]);
                    sqlCom.Parameters.Add("@IsConfirm", SqlDbType.Int).Value = Convert.ToBoolean(grdGudie[RowNumb, (int)GD.gCNF]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)  RtnVal = false; 
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Driver_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Driver_Details";
                RowNumb = 1;
                while (grdDriver[RowNumb, grdDriver.Cols[(int)DR.gDID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    if (grdDriver[RowNumb, (int)DR.gIDN] + "".Trim() != "")
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = Int32.Parse(grdDriver[RowNumb, (int)DR.gIDN].ToString());
                    else
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdDriver[RowNumb, (int)DR.gDID] != null && grdDriver[RowNumb, (int)DR.gDID].ToString() != "")
                        sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Int32.Parse(grdDriver[RowNumb, (int)DR.gDID].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)  RtnVal = false; 
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Match_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Match_Details";
                RowNumb = 1;
                while (grdMatch.Rows.Count > RowNumb)
                {
                    if (grdMatch[RowNumb, grdMatch.Cols[(int)MG.gDID].Index] == null && grdMatch[RowNumb, grdMatch.Cols[(int)MG.gGID].Index] == null)
                        return RtnVal;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdMatch[RowNumb, (int)MG.gDID] != null && grdMatch[RowNumb, (int)MG.gDID].ToString() != "") sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Int32.Parse(grdMatch[RowNumb, (int)MG.gDID].ToString());
                    if (grdMatch[RowNumb, (int)MG.gGID] != null && grdMatch[RowNumb, (int)MG.gGID].ToString() != "") sqlCom.Parameters.Add("@GuideID", SqlDbType.Int).Value = Int32.Parse(grdMatch[RowNumb, (int)MG.gGID].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Transport_Items(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_TransportItems";
                RowNumb = 1;
                while (grdItems[RowNumb, grdItems.Cols[(int)TI.gIID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdItems[RowNumb, (int)TI.gIID] != null && grdItems[RowNumb, (int)TI.gIID].ToString() != "") sqlCom.Parameters.Add("@ItemNo", SqlDbType.Int).Value = Int32.Parse(grdItems[RowNumb, (int)TI.gIID].ToString());
                    if (grdItems[RowNumb, (int)TI.gNOI] != null && grdItems[RowNumb, (int)TI.gNOI].ToString() != "") sqlCom.Parameters.Add("@NoOfItems", SqlDbType.Int).Value = Int32.Parse(grdItems[RowNumb, (int)TI.gNOI].ToString());
                    if (grdItems[RowNumb, (int)TI.gREC] != null && grdItems[RowNumb, (int)TI.gREC].ToString() != "") sqlCom.Parameters.Add("@Received", SqlDbType.Int).Value = Int32.Parse(grdItems[RowNumb, (int)TI.gREC].ToString());
                    if (grdItems[RowNumb, (int)TI.gRTN] != null && grdItems[RowNumb, (int)TI.gRTN].ToString() != "") sqlCom.Parameters.Add("@Return", SqlDbType.Int).Value = Int32.Parse(grdItems[RowNumb, (int)TI.gRTN].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Basic_Transport(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_BasicTransport";
                RowNumb = 1;
                while (grdDBasic[RowNumb, grdDBasic.Cols[(int)DB.gDID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdDBasic[RowNumb, (int)DB.gDID] != null && grdDBasic[RowNumb, (int)DB.gDID].ToString() != "") sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Int32.Parse(grdDBasic[RowNumb, (int)DB.gDID].ToString());
                    if (grdMatch[RowNumb, (int)MG.gADT] != null && grdMatch[RowNumb, (int)MG.gADT].ToString() != "") sqlCom.Parameters.Add("@ArrivalDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdMatch[RowNumb, (int)MG.gADT]);
                    if (grdMatch[RowNumb, (int)MG.gATM] != null && grdMatch[RowNumb, (int)MG.gATM].ToString() != "") sqlCom.Parameters.Add("@ArrivalTime", SqlDbType.NVarChar, 50).Value = grdMatch[RowNumb, (int)MG.gATM].ToString();
                    if (grdMatch[RowNumb, (int)MG.gAFL] != null && grdMatch[RowNumb, (int)MG.gAFL].ToString() != "") sqlCom.Parameters.Add("@ArrivalFlight", SqlDbType.NVarChar, 50).Value = grdMatch[RowNumb, (int)MG.gAFL].ToString();
                    if (grdMatch[RowNumb, (int)MG.gDDT] != null && grdMatch[RowNumb, (int)MG.gDDT].ToString() != "") sqlCom.Parameters.Add("@DeprtDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdMatch[RowNumb, (int)MG.gDDT]);
                    if (grdMatch[RowNumb, (int)MG.gDTM] != null && grdMatch[RowNumb, (int)MG.gDTM].ToString() != "") sqlCom.Parameters.Add("@DepartTime", SqlDbType.NVarChar, 50).Value = grdMatch[RowNumb, (int)MG.gDTM].ToString();
                    if (grdMatch[RowNumb, (int)MG.gDFL] != null && grdMatch[RowNumb, (int)MG.gDFL].ToString() != "") sqlCom.Parameters.Add("@DepartFlight", SqlDbType.NVarChar, 50).Value = grdMatch[RowNumb, (int)MG.gDFL].ToString();
                    if (Convert.ToBoolean(grdDBasic[RowNumb, (int)DB.gEXC]))
                    {
                        sqlCom.Parameters.Add("@Excursion", SqlDbType.Int).Value = 1;
                        if (grdDBasic[RowNumb, (int)DB.gEXD] != null && grdDBasic[RowNumb, (int)DB.gEXD].ToString() != "") sqlCom.Parameters.Add("@ExcurDesc", SqlDbType.NVarChar, 200).Value = grdDBasic[RowNumb, (int)DB.gEXD].ToString();
                        if (grdDBasic[RowNumb, (int)DB.gEXA] != null && grdDBasic[RowNumb, (int)DB.gEXA].ToString() != "") sqlCom.Parameters.Add("@ExcurAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDBasic[RowNumb, (int)DB.gEXA]);
                    }
                    if (grdDBasic[RowNumb, (int)DB.gSMT] != null && grdDBasic[RowNumb, (int)DB.gSMT].ToString() != "") sqlCom.Parameters.Add("@StartMeter", SqlDbType.NVarChar, 100).Value = grdDBasic[RowNumb, (int)DB.gSMT].ToString();
                    if (grdDBasic[RowNumb, (int)DB.gEMT] != null && grdDBasic[RowNumb, (int)DB.gEMT].ToString() != "") sqlCom.Parameters.Add("@EndMeter", SqlDbType.NVarChar, 100).Value = grdDBasic[RowNumb, (int)DB.gEMT].ToString();
                    if (grdDBasic[RowNumb, (int)DB.gTKM] != null && grdDBasic[RowNumb, (int)DB.gTKM].ToString() != "") sqlCom.Parameters.Add("@TotalKm", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDBasic[RowNumb, (int)DB.gTKM].ToString());
                    if (grdDBasic[RowNumb, (int)DB.gRKM] != null && grdDBasic[RowNumb, (int)DB.gRKM].ToString() != "") sqlCom.Parameters.Add("@RatePerKm", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDBasic[RowNumb, (int)DB.gRKM].ToString());
                    if (grdDBasic[RowNumb, (int)DB.gBAT] != null && grdDBasic[RowNumb, (int)DB.gBAT].ToString() != "") sqlCom.Parameters.Add("@Bata", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDBasic[RowNumb, (int)DB.gBAT].ToString());
                    if (grdDBasic[RowNumb, (int)DB.gNON] != null && grdDBasic[RowNumb, (int)DB.gNON].ToString() != "") sqlCom.Parameters.Add("@NoOfNights", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDBasic[RowNumb, (int)DB.gNON].ToString());
                    if (Convert.ToBoolean(grdDBasic[RowNumb, (int)DB.gIPD]))
                    {
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = 1;
                        if (grdDBasic[RowNumb, (int)DB.gPDT] + "".Trim() != "") sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdDBasic[RowNumb, (int)DB.gPDT].ToString());
                        sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Convert.ToInt32(grdDBasic[RowNumb, (int)DB.gPBY]);
                    }
                    sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdDBasic[RowNumb, (int)DB.gNPD]);
                    sqlCom.Parameters.Add("@IsConfirm", SqlDbType.Int).Value = Convert.ToBoolean(grdDBasic[RowNumb, (int)DB.gCNF]);
                    sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = Convert.ToBoolean(grdDBasic[RowNumb, (int)DB.gIDC]);
                    if (grdDBasic[RowNumb, (int)DB.gRMK] != null && grdDBasic[RowNumb, (int)DB.gRMK].ToString() != "") sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 500).Value = grdDBasic[RowNumb, (int)DB.gRMK].ToString();
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                    if (grdDBasic.Rows.Count == RowNumb) return RtnVal;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Sim_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_SimDetails";
                RowNumb = 1;
                while (grdSim[RowNumb, grdSim.Cols[(int)SM.gSNO].Index] != null)
                {
                    if (grdSim[RowNumb, (int)SM.gSNO].ToString().Trim() == "")
                        return true;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdSim[RowNumb, (int)SM.gSNO] != null && grdSim[RowNumb, (int)SM.gSNO].ToString() != "")
                        sqlCom.Parameters.Add("@SimNo", SqlDbType.NVarChar).Value = grdSim[RowNumb, (int)SM.gSNO].ToString();
                    if (grdSim[RowNumb, (int)SM.gMNO] != null && grdSim[RowNumb, (int)SM.gMNO].ToString() != "")
                        sqlCom.Parameters.Add("@MobileNo", SqlDbType.NVarChar).Value = grdSim[RowNumb, (int)SM.gMNO].ToString();
                    sqlCom.Parameters.Add("@IsComplete", SqlDbType.Int).Value = Convert.ToBoolean(grdSim[RowNumb, (int)SM.gCOM]) ? "1" : "0";
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_ScratchCards_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_ScratchCardsDetails";
                RowNumb = 1;
                while (grdScratch[RowNumb, grdScratch.Cols[(int)SC.gSNO].Index] != null)
                {
                    if (grdScratch[RowNumb, (int)SC.gSNO].ToString().Trim() == "")
                        return true;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdScratch[RowNumb, (int)SC.gSNO] != null && grdScratch[RowNumb, (int)SC.gSNO].ToString() != "") sqlCom.Parameters.Add("@CardNo", SqlDbType.NVarChar).Value = grdScratch[RowNumb, (int)SC.gSNO].ToString();
                    if (grdScratch[RowNumb, (int)SC.gAMT] != null && grdScratch[RowNumb, (int)SC.gAMT].ToString() != "") sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdScratch[RowNumb, (int)SC.gAMT].ToString());
                    if (grdScratch[RowNumb, (int)SC.gSDT] != null && grdScratch[RowNumb, (int)SC.gSDT].ToString() != "") sqlCom.Parameters.Add("@Date", SqlDbType.DateTime).Value = Convert.ToDateTime(grdScratch[RowNumb, (int)SC.gSDT].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Travel_Expenses(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Travel_Expenses";
                RowNumb = 1;
                while (grdTExpense[RowNumb, grdTExpense.Cols[(int)TP.gEID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdTExpense[RowNumb, (int)TP.gIDN] + "".Trim() != "")
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = Convert.ToInt32(grdTExpense[RowNumb, (int)TP.gIDN]);
                    else
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = 0;
                    if (Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gIDR])) sqlCom.Parameters.Add("@IsDriver", SqlDbType.Int).Value = 1;
                    if (grdTExpense[RowNumb, (int)TP.gDID] != null && grdTExpense[RowNumb, (int)TP.gDID].ToString() != "") sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(grdTExpense[RowNumb, (int)TP.gDID]);
                    if (grdTExpense[RowNumb, (int)TP.gEID] != null && grdTExpense[RowNumb, (int)TP.gEID].ToString() != "") sqlCom.Parameters.Add("@ExpenseID", SqlDbType.NVarChar).Value = grdTExpense[RowNumb, (int)TP.gEID].ToString();
                    if (grdTExpense[RowNumb, (int)TP.gUNT] + "".Trim() != "") sqlCom.Parameters.Add("@Units", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTExpense[RowNumb, (int)TP.gUNT]);
                    if (grdTExpense[RowNumb, (int)TP.gAMT] != null && grdTExpense[RowNumb, (int)TP.gAMT].ToString() != "") sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTExpense[RowNumb, (int)TP.gAMT].ToString());
                    sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gIPD]);
                    sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdTExpense[RowNumb, (int)TP.gNPD]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private Boolean Save_Tour_Advance(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_Tour_Advance";
                RowNumb = 1;
                while (grdTAdvance[RowNumb, grdTAdvance.Cols[(int)TA.gDID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdTAdvance[RowNumb, (int)TA.gIDN] + "".Trim() != "")
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gIDN]);
                    else
                        sqlCom.Parameters.Add("@UniqueID", SqlDbType.Int).Value = 0;
                    if (grdTAdvance[RowNumb, (int)TA.gIDR] != null && grdTAdvance[RowNumb, (int)TA.gIDR].ToString() != "") sqlCom.Parameters.Add("@IsDriver", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gIDR]);
                    if (grdTAdvance[RowNumb, (int)TA.gDID] != null && grdTAdvance[RowNumb, (int)TA.gDID].ToString() != "") sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gDID].ToString());
                    if (grdTAdvance[RowNumb, (int)TA.gEID] != null && grdTAdvance[RowNumb, (int)TA.gEID].ToString() != "") sqlCom.Parameters.Add("@ExpenseID", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gEID].ToString());
                    if (grdTAdvance[RowNumb, (int)TA.gENM] != null && grdTAdvance[RowNumb, (int)TA.gENM].ToString() != "") sqlCom.Parameters.Add("@Expense", SqlDbType.VarChar, 100).Value = grdTAdvance[RowNumb, (int)TA.gENM].ToString();
                    if (grdTAdvance[RowNumb, (int)TA.gAMT] != null && grdTAdvance[RowNumb, (int)TA.gAMT].ToString() != "") sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdTAdvance[RowNumb, (int)TA.gAMT].ToString());
                    if (Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gIPD]))
                    {
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = 1;
                        if (grdTAdvance[RowNumb, (int)TA.gPDT] + "".Trim() != "") sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdTAdvance[RowNumb, (int)TA.gPDT]);
                        sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                    }
                    if (Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gIST]))
                    {
                        sqlCom.Parameters.Add("@IsSettled", SqlDbType.Int).Value = 1;
                        if (grdTAdvance[RowNumb, (int)TA.gSBY] + "".Trim() != "") sqlCom.Parameters.Add("@SettledBy", SqlDbType.Int).Value = Convert.ToInt32(grdTAdvance[RowNumb, (int)TA.gSBY]);
                        if (grdTAdvance[RowNumb, (int)TA.gSDT] + "".Trim() != "") sqlCom.Parameters.Add("@SettledDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdTAdvance[RowNumb, (int)TA.gSDT]);
                    }
                    sqlCom.Parameters.Add("@NotPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdTAdvance[RowNumb, (int)TA.gNPD]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private void grdTR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdTR.Rows.Remove(grdTR.Row);
                grdTR.Rows[1].AllowEditing = true;
            }
        }
        private void grdTR_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTTans, DTCityFrom, DTCityTo, DTVoucherID, DTDriver, DTGuide, DTDistance, DTAirpotCity;
                bool IsAirport = false;
                if (e.Col == grdTR.Cols[(int)TR.gTN].Index)
                {
                    DTTans = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  ID,Name FROM trn_TransTypes WHERE IsNull(IsActive,0)=1 ORDER BY SrNo");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTTans;
                    frm.SubForm = new Master.frmCityItinerary();
                    frm.Width = grdTR.Cols[(int)TR.gTN].Width;
                    frm.Height = grdTR.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTR);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdTR[grdTR.Row, (int)TR.gTR] = SelText[0];
                        grdTR[grdTR.Row, (int)TR.gTN] = SelText[1];
                        DataTable DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select IsCalPickup,IsCalDrop From trn_TransTypes Where ID='" + SelText[0] + "' ");
                        if (Convert.ToBoolean(DT.Rows[0]["IsCalPickup"]))
                        {
                            grdTR[grdTR.Row, (int)TR.gDT] = Convert.ToDateTime(dtpArrival.Value);
                            IsAirport = true;
                            DTAirpotCity = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  CityID,City FROM vw_Airport WHERE IsNull(IsDefault,0)=1 AND IsNull(IsActive,0)=1");
                            if (DTAirpotCity.Rows.Count > 0)
                            {
                                grdTR[grdTR.Row, (int)TR.gFI] = DTAirpotCity.Rows[0]["CityID"].ToString();
                                grdTR[grdTR.Row, (int)TR.gFR] = DTAirpotCity.Rows[0]["City"].ToString();
                            }
                        }
                        else if (Convert.ToBoolean(DT.Rows[0]["IsCalDrop"]))
                        {
                            grdTR[grdTR.Row, (int)TR.gDT] = Convert.ToDateTime(dtpDeparture.Value);
                            IsAirport = true;
                            DTAirpotCity = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  CityID,City FROM vw_Airport WHERE IsNull(IsDefault,0)=1 AND IsNull(IsActive,0)=1");
                            if (DTAirpotCity.Rows.Count > 0)
                            {
                                grdTR[grdTR.Row, (int)TR.gTI] = DTAirpotCity.Rows[0]["CityID"].ToString();
                                grdTR[grdTR.Row, (int)TR.gTO] = DTAirpotCity.Rows[0]["City"].ToString();
                            }
                        }
                        DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,DriverName,VehicleNo FROM vw_TR_DriverVSVehicle WHERE ID=" + DefDriverID + "");
                        if (DTDriver.Rows.Count > 0)
                        {
                            grdTR[grdTR.Row, (int)TR.gDI] = DTDriver.Rows[0]["ID"].ToString();
                            grdTR[grdTR.Row, (int)TR.gDN] = DTDriver.Rows[0]["DriverName"].ToString();
                            grdTR[grdTR.Row, (int)TR.gVN] = DTDriver.Rows[0]["VehicleNo"].ToString();
                        }
                        if (grdTR.Row != 1)//not apply this for 1st row
                        {
                            if (grdTR[grdTR.Row - 1, grdTR.Cols[(int)TR.gTI].Index] != null)
                            {
                                grdTR[grdTR.Row, (int)TR.gFI] = Convert.ToInt32(grdTR[grdTR.Row - 1, (int)TR.gTI].ToString());
                                grdTR[grdTR.Row, (int)TR.gFR] = grdTR[grdTR.Row - 1, (int)TR.gTO].ToString();
                            }
                            if (grdTR[grdTR.Row - 1, grdTR.Cols[(int)TR.gVI].Index] != null)
                            {
                                grdTR[grdTR.Row, (int)TR.gVI] = Convert.ToInt32(grdTR[1, (int)TR.gVI].ToString());
                                grdTR[grdTR.Row, (int)TR.gVN] = grdTR[1, (int)TR.gVN].ToString();
                            }
                            if (grdTR[grdTR.Row - 1, grdTR.Cols[(int)TR.gDI].Index] != null)
                            {
                                grdTR[grdTR.Row, (int)TR.gDI] = Convert.ToInt32(grdTR[1, (int)TR.gDI].ToString());
                                grdTR[grdTR.Row, (int)TR.gDN] = grdTR[1, (int)TR.gDN].ToString();
                            }
                        }
                    }
                }
                if (e.Col == grdTR.Cols[(int)TR.gHT].Index)
                {
                    if (grdTR[grdTR.Row, grdTR.Cols[(int)TR.gTN].Index] != null && grdTR[grdTR.Row, (int)TR.gTN].ToString() != "")
                    {
                        DTVoucherID = Make_Hotetl_VoucherID();
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTVoucherID;
                        frm.Width = grdTR.Cols[(int)TR.gVO].Width;
                        frm.Height = grdTR.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTR);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdTR[grdTR.Row, (int)TR.gVO] = SelText[0];
                            grdTR[grdTR.Row, (int)TR.gHT] = SelText[1];
                        }
                    }
                    else
                        MessageBox.Show("Please Select a Trans Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (e.Col == grdTR.Cols[(int)TR.gFR].Index)
                {
                    if (grdTR[grdTR.Row, grdTR.Cols[(int)TR.gTN].Index] != null && grdTR[grdTR.Row, (int)TR.gTN].ToString() != "")
                    {
                        if (IsAirport) DTCityFrom = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  DISTINCT CityID,City FROM vw_Airport WHERE Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1 ORDER BY City");
                        else DTCityFrom = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  DISTINCT CityFromID,CityFromName FROM vw_CityItinerary WHERE Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1 ORDER BY CityFromName");
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTCityFrom;
                        frm.SubForm = new Master.frmCityItinerary();
                        frm.Width = grdTR.Cols[(int)TR.gFR].Width;
                        frm.Height = grdTR.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTR);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdTR[grdTR.Row, (int)TR.gFI] = SelText[0];
                            grdTR[grdTR.Row, (int)TR.gFR] = SelText[1];
                        }
                    }
                    else
                        MessageBox.Show("Please Select a Trans Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (e.Col == grdTR.Cols[(int)TR.gTO].Index)
                {
                    if (grdTR[grdTR.Row, grdTR.Cols[(int)TR.gFI].Index] != null && grdTR[grdTR.Row, (int)TR.gFI].ToString() != "")
                    {
                        if (IsAirport) DTCityTo = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  DISTINCT CityID,City FROM vw_Airport WHERE Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1 ORDER BY City");
                        else DTCityTo = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT CityToID,CityToName FROM vw_CityItinerary WHERE CityFromID=" + Convert.ToInt32(grdTR[grdTR.Row, (int)TR.gFI].ToString()) + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTCityTo;
                        frm.SubForm = new Master.frmCityItinerary();
                        frm.Width = grdTR.Cols[(int)TR.gFR].Width;
                        frm.Height = grdTR.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTR);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdTR[grdTR.Row, (int)TR.gTI] = SelText[0];
                            grdTR[grdTR.Row, (int)TR.gTO] = SelText[1];
                            DTDistance = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT DistanceKm FROM mst_CityItinerary WHERE CityFromID=" + Convert.ToInt32(grdTR[grdTR.Row, (int)TR.gFI].ToString()) + " AND CityToID=" + SelText[0] + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                            if (DTDistance.Rows.Count > 0) grdTR[grdTR.Row, (int)TR.gDS] = Convert.ToDouble(DTDistance.Rows[0]["DistanceKm"].ToString());
                        }
                    }
                    else MessageBox.Show("Please Select City From Name", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (e.Col == grdTR.Cols[(int)TR.gDN].Index)
                {
                    if (grdTR[grdTR.Row, grdTR.Cols[(int)TR.gTN].Index] != null && grdTR[grdTR.Row, (int)TR.gTN].ToString() != "")
                    {
                        DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM vw_TR_Driver Where IsNull(IsActive,0)=1 ORDER BY Name");
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTDriver;
                        frm.SubForm = new Master.frmDriver();
                        frm.Width = grdTR.Cols[(int)TR.gFR].Width;
                        frm.Height = grdTR.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTR);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdTR[grdTR.Row, (int)TR.gDI] = SelText[0];
                            grdTR[grdTR.Row, (int)TR.gDN] = SelText[1];
                            DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,DriverName,VehicleNo FROM vw_TR_DriverVSVehicle WHERE ID=" + SelText[0] + "");
                            if (DTDriver.Rows.Count > 0)
                            {
                                if (DTDriver.Rows[0]["VehicleNo"].ToString() != "") grdTR[grdTR.Row, (int)TR.gVN] = DTDriver.Rows[0]["VehicleNo"].ToString(); else grdTR[grdTR.Row, (int)TR.gVN] = "";
                            }
                        }
                    }
                    else
                        MessageBox.Show("Please Select a Trans Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (e.Col == grdTR.Cols[(int)TR.gGN].Index)
                {
                    if (grdTR[grdTR.Row, grdTR.Cols[(int)TR.gTN].Index] != null && grdTR[grdTR.Row, (int)TR.gTN].ToString() != "")
                    {
                        DTGuide = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,GuideName FROM vw_ALL_GUIDE_DETAILS Where IsNull(IsActive,0)=1 ORDER BY GuideName");
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTGuide;
                        frm.SubForm = new Master.frmDriver();
                        frm.Width = grdTR.Cols[(int)TR.gGN].Width;
                        frm.Height = grdTR.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTR);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdTR[grdTR.Row, (int)TR.gGI] = SelText[0];
                            grdTR[grdTR.Row, (int)TR.gGN] = SelText[1];
                        }
                    }
                    else
                        MessageBox.Show("Please Select a Trans Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void grdTR_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true) return;
            grdTR.Rows[1].AllowEditing = true;
            if (grdTR.Rows.Count < 3) return;
            grdTR.Rows[grdTR.Row].AllowEditing = grdTR[grdTR.Row - 1, 0] != null;
        }
        private void btnTREGenerate_Click(object sender, EventArgs e)   {  Generate_Transport_Expenses();   }
        private void Generate_Transport_Expenses()
        {
            try
            {
                if (Validate_Transport_Expenses() == false)
                    return;
                double ChPerKm = 0.00, Dist = 0.00, Amt = 0.00;
                double TotAmt = 0.00, TotDst = 0.00;
                string Sql;
                RowNumb = 1;
                if ((grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] == null) || (grdTR[RowNumb, (int)TR.gTN].ToString() == "")) return;
                int MaxVal = 0;
                while (grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] != null) RowNumb++;
                MaxVal = RowNumb - 1;
                pbTRE.Maximum = MaxVal;
                if (grdTR[TRE_TotRowNo, grdTR.Cols[(int)TR.gFI].Index] == null) grdTR.Rows.Remove(TRE_TotRowNo);
                RowNumb = 1;
                while (grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] != null)
                {
                    C1.Win.C1FlexGrid.CellStyle rs1 = grdTR.Styles.Add("RowColor");
                    rs1.BackColor = Color.White;
                    grdTR.Rows[RowNumb].Style = grdTR.Styles["RowColor"];
                    if (grdTR[RowNumb, grdTR.Cols[(int)TR.gDS].Index] != null)
                        Dist = Convert.ToDouble(grdTR[RowNumb, (int)TR.gDS].ToString());
                    Sql = "SELECT ChargersPerKm from dbo.mst_TRVehicle  WHERE DriverID=" + Convert.ToInt32(grdTR[RowNumb, (int)TR.gDI].ToString()) + " ";
                    if ((Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(Sql).Rows[0]["ChargersPerKm"]).ToString() != "")
                        ChPerKm = Convert.ToDouble(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(Sql).Rows[0]["ChargersPerKm"]);
                    else return;
                    Amt = (Dist * ChPerKm);
                    grdTR[RowNumb, (int)TR.gCH] = Amt.ToString();
                    TotDst = TotDst + Dist;
                    TotAmt = TotAmt + Amt;
                    pbTRE.Value = RowNumb;
                    RowNumb++;
                }
                grdTR[RowNumb + 6, (int)TR.gTN] = "TOTAL COST";
                grdTR[RowNumb + 6, (int)TR.gDS] = TotDst.ToString();
                grdTR[RowNumb + 6, (int)TR.gCH] = TotAmt.ToString();
                TRE_TotRowNo = RowNumb + 6;
                C1.Win.C1FlexGrid.CellStyle rs2 = grdTR.Styles.Add("TotalColor");
                rs2.BackColor = Color.PowderBlue;
                grdTR.Rows[RowNumb + 6].Style = grdTR.Styles["TotalColor"];
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private Boolean Validate_Transport_Expenses()
        {
            try
            {
                RowNumb = 1;
                while (grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] != null)
                {
                    if (grdTR[RowNumb, grdTR.Cols[(int)TR.gTM].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdTR[RowNumb, (int)TR.gTM].ToString()) == true)
                        {
                            string[] TimeArray = grdTR[RowNumb, (int)TR.gTM].ToString().Split('.');
                            if (Tourist_Management.Classes.clsGlobal.IsNumeric(TimeArray[0].ToString()) == false || Tourist_Management.Classes.clsGlobal.IsNumeric(TimeArray[1].ToString()) == false)
                            {
                                MessageBox.Show("Please Enter Correct Time", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                            int hour = Convert.ToInt32(TimeArray[0].ToString());
                            int minits = Convert.ToInt32(TimeArray[1].ToString());
                            if (hour > 12 || hour < 0 || minits < 0 || minits > 59)
                            {
                                MessageBox.Show("Please Enter Correct Time", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Enter Correct Time", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    if (grdTR[RowNumb, grdTR.Cols[(int)TR.gDI].Index] == null)
                    {
                        MessageBox.Show("Please Select a Driver", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (grdTR[RowNumb, grdTR.Cols[(int)TR.gTI].Index] == null)
                    {
                        MessageBox.Show("Please Select City TO Name", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (grdTR[RowNumb, grdTR.Cols[(int)TR.gDS].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdTR[RowNumb, (int)TR.gDS].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Distance", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void tcGroupAmend_Click(object sender, EventArgs e)
        {
            if (tcGroupAmend.SelectedTab.Name == "tpEmailOptions")
            {
                if (IsPreview == false)
                {
                    MessageBox.Show("Please Preview Before Email", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.tcGroupAmend.SelectedTab = tpBasic;
                    return;
                }
            }
            if (tcGroupAmend.SelectedTab.Name == "tpBasic")
            {
                this.Width = 913;
                this.Height = 520;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            }
            else
            {
                this.Width = 1134;
                this.Height = 520;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            }
            if (tcGroupAmend.SelectedTab.Name == "tpHotels")
            {
                Create_Hotel_Sightseeing_Grids();
                IsRateChecked = true;
            }
            else if (tcGroupAmend.SelectedTab.Name == "tpTravel")
            {
                if (Mode != 1)  Create_Tour_Advance();
            }
            else
            {
                if (drpCountry.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("Please Select a Country", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tcGroupAmend.SelectedTab = tpBasic;
                    return;
                }
                txtDisTourID.Text = txtTourID.Text;
                txtROguest.Text = txtGuest.Text;
                txtROnoa.Text = NoOfAdult.ToString();
                txtROnoc.Text = NoOfChild.ToString();
                txtDisVoucherNo.Text = "";
                RowNumb = 1;
                string VoucherNo = "", HotelName = "";
                while (grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gVNO].Index] != null))
                    {
                        VoucherNo = grdCI[RowNumb, (int)CI.gVNO].ToString();
                    }
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gHNM].Index] != null))
                    {
                        HotelName = grdCI[RowNumb, (int)CI.gHNM].ToString();
                    }
                    txtDisVoucherNo.Text += HotelName + Environment.NewLine + VoucherNo;
                    txtDisVoucherNo.Text += Environment.NewLine + "_______________________" + Environment.NewLine + Environment.NewLine;
                    RowNumb++;
                }
            }  
        } 
        private void Create_Tour_Advance()
        {
            int RowNumb;
            try
            {
                int row = 1;
                int AllRows = 1;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gSCI].Index] != null)
                {
                    AllRows++;
                }
                for (int i = 0; i < AllRows - 1; i++)
                {
                    RowNumb = 1;
                    string nm = "SGH" + (i + 1);
                    User_Controls.ucSSNavigation u = dicSight[nm];
                    while (u.grdSE[RowNumb, u.grdSE.Cols[0].Index] != null)
                    {
                        if (Convert.ToBoolean(u.grdSE[RowNumb, (int)SE.gSEL]))
                        {
                            if (grdDriver[1, (int)DR.gDID] != null && grdDriver[1, (int)DR.gDID].ToString() != "")
                            {
                                grdTAdvance[row, (int)TA.gDID] = grdDriver[1, (int)DR.gDID].ToString();
                                grdTAdvance[row, (int)TA.gDNM] = grdDriver[1, (int)DR.gDNM].ToString();
                            }
                            else
                                return;
                            grdTAdvance[row, (int)TA.gEID] = u.grdSE[RowNumb, (int)SE.gSSI].ToString();
                            grdTAdvance[row, (int)TA.gENM] = u.grdSE[RowNumb, (int)SE.gSPN].ToString();
                            if (u.grdSE[RowNumb, (int)SE.gTOT] != null)
                                grdTAdvance[row, (int)TA.gAMT] = u.grdSE[RowNumb, (int)SE.gTOT].ToString();
                            row++;
                        }
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void Add_Remove_Tabs_Hotel(int NoOf, TabControl tbc, TabPage tbpName, string ControllName, string TabPageName, string TabPageText)
        {
            try
            {
                tbc.Location = new System.Drawing.Point(3, 3);
                tbc.Name = ControllName;
                tbc.Size = new System.Drawing.Size(880, 410);
                if (tbc.TabPages.Count < NoOf)//WHEN INCREASING NO OF TABS
                {
                    for (int i = 1; i <= NoOf; i++)
                    {
                        if (tbc.TabPages.Count < i)
                        {
                            TabPage tb = new TabPage();
                            tb.Name = TabPageName + i;
                            tb.Text = HotelName[i - 1].ToString();//+" Hotels";
                            tb.Padding = new System.Windows.Forms.Padding(3);
                            tb.Size = new System.Drawing.Size(865, 409);
                            tb.UseVisualStyleBackColor = true;
                            Tourist_Management.User_Controls.ucHotelNavigation ucHotelNav = new Tourist_Management.User_Controls.ucHotelNavigation();
                            ucHotelNav.Name = TabPageName + i;
                            ucHotelNav.HotelID = HotelID[i - 1];
                            ucHotelNav.NotEnable = Convert.ToBoolean(grdCI[i, (int)CI.gIPD]);
                            ucHotelNav.Mode = Mode;
                            ucHotelNav.VoucherNo = grdCI[i, (int)CI.gVNO].ToString();
                            decimal idn = 0m;
                            idn = Convert.ToDecimal(grdCI[i, (int)CI.gIDN]);
                            if (idn > 1000000)
                                ucHotelNav.IsUpdate = true;
                            else
                                ucHotelNav.IsUpdate = false;
                            ucHotelNav.TransactionID = SystemCode;
                            ucHotelNav.TabNumber = i;
                            ucHotelNav.NoOfAdult = NoOfAdult;
                            ucHotelNav.NoOfChild = NoOfChild;
                            ucHotelNav.SingleRooms = Convert.ToInt16(nudSingle.Value);
                            ucHotelNav.DoubleRooms = Convert.ToInt16(nudDouble.Value);
                            ucHotelNav.TripleRooms = Convert.ToInt16(nudTriple.Value);
                            ucHotelNav.TwinRooms = Convert.ToInt16(nudTwin.Value);
                            ucHotelNav.CheckIn = Checkin[i - 1];
                            ucHotelNav.CheckOut = Checkout[i - 1];
                            ucHotelNav.VoucherNo = HotelVoucher[i - 1];
                            ucHotelNav.Dock = DockStyle.Fill;
                            tb.Controls.Add(ucHotelNav);
                            tbc.Controls.Add(tb);
                            ucHotelNav.Grd_Initializer();
                            dicHotels.Add("HTL" + i, ucHotelNav);
                            ucHotelNav.Intializer();
                        }
                    }
                }
                else//WHEN DECREASIGN NO OF TABS
                {
                    TabPage tp;
                    string currentCity;
                    bool Having1AtLeat = false;
                    for (int i = tbc.TabPages.Count; i > NoOf; i--)
                    {
                        foreach (TabPage ttp in tbc.TabPages)
                        {
                            int currow = 1;
                            while (grdCI[currow, grdCI.Cols[(int)CI.gCID].Index] != null)
                            {
                                Having1AtLeat = true;
                                currentCity = grdCI[currow, (int)CI.gCTY].ToString();
                                if (ttp.Text != currentCity)
                                {
                                    tp = ttp;
                                    tbc.TabPages.Remove(tp);
                                    break;
                                }
                                currow++;
                            }
                            if (Having1AtLeat == false)
                            {
                                tp = ttp;
                                tbc.TabPages.Remove(tp);
                                break;
                            }
                        }
                    }
                }
                tbpName.Controls.Add(tbc);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void OnHotelClick(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(sender.ToString());
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void Add_Remove_Other_Details(int NoOf, TabControl tbc, TabPage tbpName, string ControllName, string TabPageName, string TabPageText)
        {
            try
            {
                tbc.Location = new System.Drawing.Point(3, 3);
                tbc.Name = ControllName;
                tbc.Size = new System.Drawing.Size(870, 409);
                if (tbc.TabPages.Count < NoOf)//WHEN INCREASING NO OF TABS
                {
                    for (int i = 1; i <= NoOf; i++)
                    {
                        if (tbc.TabPages.Count < i)
                        {
                            TabPage tb = new TabPage();
                            tb.Name = TabPageName + i;
                            tb.Text = HotelName[i - 1].ToString();//+" Hotels";
                            tb.Padding = new System.Windows.Forms.Padding(3);
                            tb.Size = new System.Drawing.Size(870, 409);
                            tb.UseVisualStyleBackColor = true;
                            Tourist_Management.User_Controls.ucTransOther ucTransOth = new Tourist_Management.User_Controls.ucTransOther();
                            ucTransOth.Name = TabPageName + i;
                            ucTransOth.NotEnable = Convert.ToBoolean(grdCI[i, (int)CI.gIPD]);
                            ucTransOth.Dock = DockStyle.Fill;
                            dicOthers.Add("OTH" + i, ucTransOth);
                            tb.Controls.Add(ucTransOth);
                            tbc.Controls.Add(tb);
                        }
                    }
                }
                else//WHEN DECREASIGN NO OF TABS
                {
                    TabPage tp;
                    string currentCity;
                    bool Having1AtLeast = false;
                    for (int i = tbc.TabPages.Count; i > NoOf; i--)
                    {
                        foreach (TabPage ttp in tbc.TabPages)
                        {
                            int currow = 1;
                            while (grdCI[currow, grdCI.Cols[(int)CI.gCID].Index] != null)
                            {
                                Having1AtLeast = true;
                                currentCity = grdCI[currow, (int)CI.gCTY].ToString();
                                if (ttp.Text != currentCity)
                                {
                                    tp = ttp;
                                    tbc.TabPages.Remove(tp);
                                    break;
                                }
                                currow++;
                            }
                            if (Having1AtLeast == false)
                            {
                                tp = ttp;
                                tbc.TabPages.Remove(tp);
                                break;
                            }
                        }
                    }
                }
                tbpName.Controls.Add(tbc);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void Add_Remove_Tabs_Sightseeing(int NoOf, TabControl tbcS, TabPage tbpNameS, string ControllNameS, string TabPageNameS, string TabPageTextS, int insmode)
        {
            try
            {
                tbcS.Location = new System.Drawing.Point(3, 3);
                tbcS.Name = ControllNameS;
                tbcS.Size = new System.Drawing.Size(880, 410);
                if (tbcS.TabPages.Count < NoOf)//WHEN INCREASING NO OF TABS
                {
                    for (int i = 1; i <= NoOf; i++)
                    {
                        if (tbcS.TabPages.Count < i)
                        {
                            TabPage tbS = new TabPage();
                            tbS.Name = TabPageNameS + i;
                            tbS.Text = Sightseeing[i - 1].ToString();
                            tbS.Padding = new System.Windows.Forms.Padding(3);
                            tbS.UseVisualStyleBackColor = true;
                            Tourist_Management.User_Controls.ucSSNavigation ucSSNav = new Tourist_Management.User_Controls.ucSSNavigation();
                            ucSSNav.Size = new System.Drawing.Size(870, 398);
                            ucSSNav.Name = TabPageNameS + i;
                            ucSSNav.SightCatID = SightID[i - 1];
                            ucSSNav.Adult = NoOfAdult;
                            ucSSNav.Child = NoOfChild;
                            ucSSNav.Saarc = Convert.ToInt32(chkSaarc.Checked ? "1" : "0");
                            ucSSNav.Mode = insmode;
                            ucSSNav.Dock = DockStyle.Fill;
                            dicSight.Add("SGH" + i, ucSSNav);
                            tbS.Controls.Add(ucSSNav);
                            tbcS.Controls.Add(tbS);
                        }
                    }
                }
                else//WHEN DECREASIGN NO OF TABS
                {
                    TabPage tp;
                    string currentSight;
                    bool Having1AtLeat = false;
                    for (int i = tbcS.TabPages.Count; i > NoOf; i--)
                    {
                        foreach (TabPage ttp in tbcS.TabPages)
                        {
                            int currow = 1;
                            while (grdCI[currow, grdCI.Cols[(int)CI.gSCI].Index] != null)
                            {
                                Having1AtLeat = true;
                                currentSight = grdCI[currow, (int)CI.gSCN].ToString();
                                if (ttp.Text != currentSight)
                                {
                                    tp = ttp;
                                    tbcS.TabPages.Remove(tp);
                                    break;
                                }
                                currow++;
                            }
                            if (Having1AtLeat == false)
                            {
                                tp = ttp;
                                tbcS.TabPages.Remove(tp);
                                break;
                            }
                        }
                    }
                }
                tbpNameS.Controls.Add(tbcS);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void grdOE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdOE.Rows.Remove(grdOE.Row);
                grdOE.Rows[1].AllowEditing = true;
            }
        }
        private void Add_Remove_Email_Details(int NoOf, TabControl tbc, TabPage tbpName, string ControllName, string TabPageName, string TabPageText)
        {
            try
            {
                tbc.Location = new System.Drawing.Point(3, 3);
                tbc.Name = ControllName;
                tbc.Size = new System.Drawing.Size(869, 409);
                if (tbc.TabPages.Count < NoOf)//WHEN INCREASING NO OF TABS
                {
                    for (int i = 1; i <= NoOf; i++)
                    {
                        if (tbc.TabPages.Count < i)
                        {
                            TabPage tb = new TabPage();
                            tb.Name = TabPageName + i;
                            tb.Text = HotelName[i - 1].ToString();//+" Hotels";
                            tb.Padding = new System.Windows.Forms.Padding(3);
                            tb.Size = new System.Drawing.Size(869, 409);
                            tb.UseVisualStyleBackColor = true;
                            Tourist_Management.User_Controls.ucTransEmail ucTransEmail = new Tourist_Management.User_Controls.ucTransEmail();
                            ucTransEmail.Name = TabPageName + i;
                            ucTransEmail.VoucherNo = grdCI[i, (int)CI.gVNO].ToString();
                            ucTransEmail.Dock = DockStyle.Fill;
                            dicEmail.Add("EML" + i, ucTransEmail);
                            tb.Controls.Add(ucTransEmail);
                            tbc.Controls.Add(tb);
                        }
                    }
                }
                else//WHEN DECREASIGN NO OF TABS
                {
                    TabPage tp;
                    string currentCity;
                    bool Having1AtLeast = false;
                    for (int i = tbc.TabPages.Count; i > NoOf; i--)
                    {
                        foreach (TabPage ttp in tbc.TabPages)
                        {
                            int currow = 1;
                            while (grdCI[currow, grdCI.Cols[(int)CI.gCID].Index] != null)
                            {
                                Having1AtLeast = true;
                                currentCity = grdCI[currow, (int)CI.gCTY].ToString();
                                if (ttp.Text != currentCity)
                                {
                                    tp = ttp;
                                    tbc.TabPages.Remove(tp);
                                    break;
                                }
                                currow++;
                            }
                            if (Having1AtLeast == false)
                            {
                                tp = ttp;
                                tbc.TabPages.Remove(tp);
                                break;
                            }
                        }
                    }
                }
                tbpName.Controls.Add(tbc);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void grdOE_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true) return;
            grdOE.Rows[1].AllowEditing = true;
            if (grdOE.Rows.Count < 3) return;
            grdOE.Rows[grdOE.Row].AllowEditing = grdOE[grdOE.Row - 1, 0] != null;
        }
        private void btnOEGenerate_Click(object sender, EventArgs e)   {  Generate_Other_Expenses();  }
        private void Generate_Other_Expenses()
        {
            try
            {
                if (Validate_Other_Expenses() == false) return;
                double Amt = 0.00, Amt1 = 0.00;
                double TotAmt = 0.00, TotTax = 0.00, TotVat = 0.00, TotServ = 0.00, NetAmt = 0.00;
                double vat = 0.00, tax = 0.00, serv = 0.00;
                double vat1 = 0.00, tax1 = 0.00, serv1 = 0.00;
                RowNumb = 1;
                if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] == null) || (grdOE[RowNumb, (int)OE.gEXN].ToString() == "")) return;
                if (chkOETax.Checked)tax = ((grdOE[1, grdOE.Cols[(int)OE.gTAX].Index] != null))? (Convert.ToDouble(grdOE[1, (int)OE.gTAX].ToString())):0.00; 
                if (chkOEVat.Checked) vat =((grdOE[1, grdOE.Cols[(int)OE.gVAT].Index] != null))? (Convert.ToDouble(grdOE[1, (int)OE.gVAT].ToString())):0.00;   
                if (chkOEServ.Checked)serv = ((grdOE[1, grdOE.Cols[(int)OE.gSCH].Index] != null)) ? (Convert.ToDouble(grdOE[1, (int)OE.gSCH].ToString())) : 0.00;
                int MaxVal = 0;
                while (grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] != null)
                { RowNumb++; }
                MaxVal = RowNumb - 1;
                pbOE.Maximum = MaxVal;
                grdOE.Rows.Remove(MaxVal + 5);
                RowNumb = 1;
                while (grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] != null)
                {
                    C1.Win.C1FlexGrid.CellStyle rs1 = grdOE.Styles.Add("RowColor");
                    rs1.BackColor = Color.White;
                    grdOE.Rows[RowNumb].Style = grdOE.Styles["RowColor"];
                    if (grdOE[RowNumb, grdOE.Cols[(int)OE.gPRC].Index] == null && grdOE[RowNumb, grdOE.Cols[(int)OE.gPRC].Index] == null)  grdOE[RowNumb, (int)OE.gPRC] = "0.00";
                     if (chkOEVat.Checked == false)  vat = (grdOE[RowNumb, grdOE.Cols[(int)OE.gVAT].Index] == null)?0.00:Convert.ToDouble(grdOE[RowNumb, (int)OE.gVAT].ToString()); 
                     if (chkOETax.Checked == false) tax = (grdOE[RowNumb, grdOE.Cols[(int)OE.gTAX].Index] == null)?0.00: Convert.ToDouble(grdOE[RowNumb, (int)OE.gTAX].ToString()); 
                      if (chkOEServ.Checked == false) serv = (grdOE[RowNumb, grdOE.Cols[(int)OE.gSCH].Index] == null)?0.00:Convert.ToDouble(grdOE[RowNumb, (int)OE.gSCH].ToString()); 
                     grdOE[RowNumb, (int)OE.gTAX] = tax.ToString();
                    grdOE[RowNumb, (int)OE.gVAT] = vat.ToString();
                    grdOE[RowNumb, (int)OE.gSCH] = serv.ToString();
                    Amt1 = Convert.ToDouble(grdOE[RowNumb, (int)OE.gPRC].ToString());
                    vat1 = ((Convert.ToDouble(grdOE[RowNumb, (int)OE.gVAT].ToString())) / 100) * Amt1;
                    tax1 = ((Convert.ToDouble(grdOE[RowNumb, (int)OE.gTAX].ToString())) / 100) * Amt1;
                    serv1 = ((Convert.ToDouble(grdOE[RowNumb, (int)OE.gSCH].ToString())) / 100) * Amt1;
                    Amt = Convert.ToDouble(grdOE[RowNumb, (int)OE.gPRC].ToString()) + (vat1 + tax1 + serv1);
                    grdOE[RowNumb, (int)OE.gTOT] = Amt.ToString();
                    TotVat = TotVat + vat;
                    TotTax = TotTax + tax;
                    TotServ = TotServ + serv;
                    TotAmt = TotAmt + Amt1;
                    NetAmt = NetAmt + Amt;
                    pbOE.Value = RowNumb;
                    RowNumb++;
                }
                grdOE[RowNumb + 4, (int)OE.gEXN] = "TOTAL COST";
                grdOE[RowNumb + 4, (int)OE.gVAT] = TotVat.ToString();
                grdOE[RowNumb + 4, (int)OE.gTAX] = TotTax.ToString();
                grdOE[RowNumb + 4, (int)OE.gSCH] = TotServ.ToString();
                grdOE[RowNumb + 4, (int)OE.gPRC] = TotAmt.ToString();
                grdOE[RowNumb + 4, (int)OE.gTOT] = NetAmt.ToString();
                C1.Win.C1FlexGrid.CellStyle rs2 = grdOE.Styles.Add("TotalColor");
                rs2.BackColor = Color.PowderBlue;
                grdOE.Rows[RowNumb + 4].Style = grdOE.Styles["TotalColor"];
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private Boolean Validate_Other_Expenses()
        {
            try
            {
                RowNumb = 1;
                while (grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] != null)
                {
                    if (grdOE[RowNumb, grdOE.Cols[(int)OE.gVAT].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdOE[RowNumb, (int)OE.gVAT].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Vat", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    if (grdOE[RowNumb, grdOE.Cols[(int)OE.gTAX].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdOE[RowNumb, (int)OE.gTAX].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Tax", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    if (grdOE[RowNumb, grdOE.Cols[(int)OE.gSCH].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdOE[RowNumb, (int)OE.gSCH].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Service Chargers", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    if (grdOE[RowNumb, grdOE.Cols[(int)OE.gPRC].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdOE[RowNumb, (int)OE.gPRC].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Save_OtherExpenses_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            try
            {
                if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] == null))  return true; 
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_trn_OtherExpenses_Details";
                while (grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@IsRepeatVat", SqlDbType.Int).Value = chkOEVat.Checked == true ? "1" : "0";
                    sqlCom.Parameters.Add("@IsRepeatTax", SqlDbType.Int).Value = chkOETax.Checked == true ? "1" : "0";
                    sqlCom.Parameters.Add("@IsRepeatServ", SqlDbType.Int).Value = chkOEServ.Checked == true ? "1" : "0";
                    sqlCom.Parameters.Add("@ExpenseName", SqlDbType.VarChar, 250).Value = grdOE[RowNumb, (int)OE.gEXN].ToString();
                    if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gVAT].Index] != null)) sqlCom.Parameters.Add("@Vat", SqlDbType.Decimal).Value = Convert.ToDecimal(grdOE[RowNumb, (int)OE.gVAT].ToString());
                    if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gTAX].Index] != null)) sqlCom.Parameters.Add("@Tax", SqlDbType.Decimal).Value = Convert.ToDecimal(grdOE[RowNumb, (int)OE.gTAX].ToString());
                    if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gSCH].Index] != null)) sqlCom.Parameters.Add("@ServChr", SqlDbType.Decimal).Value = Convert.ToDecimal(grdOE[RowNumb, (int)OE.gSCH].ToString());
                    if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gPRC].Index] != null)) sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdOE[RowNumb, (int)OE.gPRC].ToString());
                    if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gTOT].Index] != null)) sqlCom.Parameters.Add("@TotAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdOE[RowNumb, (int)OE.gTOT].ToString());
                    if ((grdOE[RowNumb, grdOE.Cols[(int)OE.gRMK].Index] != null)) sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = grdOE[RowNumb, (int)OE.gRMK].ToString();
                    if (Convert.ToBoolean(grdOE[RowNumb, (int)OE.gPID]))
                    {
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdOE[RowNumb, (int)OE.gPDT]);
                    }
                    if (Convert.ToBoolean(grdOE[RowNumb, (int)OE.gIBP]))
                    {
                        sqlCom.Parameters.Add("@IsBankPayment", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@ChkNo", SqlDbType.NVarChar, 20).Value = grdOE[RowNumb, (int)OE.gCNO].ToString();
                    }
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                    RowNumb++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void grdCI_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToBoolean(grdCI[grdCI.Row, (int)CI.gIPD]))
            {
                grdCI.Rows[grdCI.Row].AllowEditing = false;
                return;
            }
            if (e.KeyCode == Keys.Delete && Convert.ToBoolean(grdCI[grdCI.Row, (int)CI.gIPD])) MessageBox.Show("This Cannot Be Deleted.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); 
            }
        private void grdCI_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)   return; 
            grdCI.Rows[1].AllowEditing = true;
            if (grdCI.Rows.Count < 3)  return;  
            grdCI.Rows[grdCI.Row].AllowEditing = grdCI[grdCI.Row - 1, 0] != null;
        }
        private void grdCI_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTCity, DTSight, DTHotel;
            if (e.Col == grdCI.Cols[(int)CI.gCTY].Index)
            {
                DTCity = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,City FROM mst_City WHERE Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTCity;
                frm.SubForm = new Master.frmCity();
                frm.Width = grdCI.Cols[(int)CI.gCTY].Width;
                frm.Height = grdCI.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCI);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdCI[grdCI.Row, (int)CI.gCID] = SelText[0];
                    grdCI[grdCI.Row, (int)CI.gCTY] = SelText[1];
                }
                if (grdCI.Row == 1)
                {
                    if (chkArrival.Checked && chkDeparture.Checked)
                    {
                        grdCI[grdCI.Row, (int)CI.gDTI] = Convert.ToDateTime(dtpArrival.Value);
                        grdCI[grdCI.Row, (int)CI.gNOD] = 0;
                        DateTime DArr = Convert.ToDateTime(grdCI[grdCI.Row, (int)CI.gDTI].ToString());
                        grdCI[grdCI.Row, (int)CI.gDTO] = DArr.AddDays(0);
                    }
                    else
                    {
                        MessageBox.Show("Please Set Date Arrival & Departure from Basic Details", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grdCI.Rows.Remove(grdCI.Row);
                    }
                }
                if (grdCI.Row != 1)//not apply this for 1st row
                {
                    if (grdCI[grdCI.Row - 1, grdCI.Cols[(int)CI.gDTI].Index] != null)
                    {
                        grdCI[grdCI.Row, (int)CI.gDTI] = Convert.ToDateTime(grdCI[grdCI.Row - 1, (int)CI.gDTO].ToString());
                        grdCI[grdCI.Row, (int)CI.gNOD] = 0;
                        DateTime DArr = Convert.ToDateTime(grdCI[grdCI.Row, (int)CI.gDTI].ToString());
                        grdCI[grdCI.Row, (int)CI.gDTO] = DArr.AddDays(0);
                    }
                }
                grdCI[grdCI.Row, (int)CI.gGNM] = txtGuest.Text;
                decimal numb = 0;
                if (Mode != 1)
                {
                    string id = txtTourID.Text.ToString().Trim().Substring(7, 6);
                    numb = Convert.ToDecimal(id);//NEW TOUR ParentID
                    grdCI[grdCI.Row, (int)CI.gATM] = 0;//AMEND TIME
                    grdCI[grdCI.Row, (int)CI.gVNO] = numb + "/" + grdCI.Row;
                }
                else
                {
                    numb = Convert.ToDecimal(SystemCode);//EXISTING TOUR ParentID
                    grdCI[grdCI.Row, (int)CI.gATM] = 0;
                    grdCI[grdCI.Row, (int)CI.gVNO] = numb + "/" + grdCI.Row;
                }
                grdCI[grdCI.Row, (int)CI.gNOA] = NoOfAdult.ToString();
                grdCI[grdCI.Row, (int)CI.gNOC] = NoOfChild.ToString();
                grdCI[grdCI.Row, (int)CI.gIDN] = (grdCI.Row + 1000000);
            }
            else if (e.Col == grdCI.Cols[(int)CI.gHNM].Index)
            {
                if (grdCI[grdCI.Row, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if (grdCI[grdCI.Row, grdCI.Cols[(int)CI.gDTI].Index] != null && grdCI[grdCI.Row, grdCI.Cols[(int)CI.gDTO].Index] != null)
                    {
                        DTHotel = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_HotelDetails WHERE CityID=" + grdCI[grdCI.Row, (int)CI.gCID].ToString() + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTHotel;
                        frm.SubForm = new Master.frmHotel();
                        frm.Width = grdCI.Cols[(int)CI.gHNM].Width;
                        frm.Height = grdCI.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCI);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdCI[grdCI.Row, (int)CI.gHID] = SelText[0];
                            grdCI[grdCI.Row, (int)CI.gHNM] = SelText[1];
                        }
                    }
                    else
                        MessageBox.Show("Date In And Out Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    MessageBox.Show("Please Select a City", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (e.Col == grdCI.Cols[(int)CI.gSCN].Index)
            {
                if (grdCI[grdCI.Row, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    DTSight = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_SightSeeingCat WHERE Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTSight;
                    frm.SubForm = new Master.frmCity();
                    frm.Width = grdCI.Cols[(int)CI.gCTY].Width;
                    frm.Height = grdCI.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCI);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdCI[grdCI.Row, (int)CI.gSCI] = SelText[0];
                        grdCI[grdCI.Row, (int)CI.gSCN] = SelText[1];
                    }
                }
                else
                    MessageBox.Show("Please Select a City", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        } 
        private void grdCI_Leave(object sender, EventArgs e)  { Create_Hotel_Sightseeing_Grids(); }
        private void Create_Hotel_Sightseeing_Grids()
        {
            try
            { 
                int AllRows = 1;
                int CountRows = 0;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if (grdCI[AllRows, grdCI.Cols[(int)CI.gHID].Index] != null)   CountRows++;
                    AllRows++;
                }
                HotelName = new string[CountRows];
                HotelID = new int[CountRows];
                Checkin = new DateTime[CountRows];
                Checkout = new DateTime[CountRows];
                HotelVoucher = new string[CountRows];
                int currow = 1;
                int i = 0;
                while (grdCI[currow, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if (grdCI[currow, grdCI.Cols[(int)CI.gHID].Index] != null)
                    {
                        HotelName[i] = grdCI[currow, (int)CI.gHNM].ToString();
                        HotelID[i] = Convert.ToInt32(grdCI[currow, (int)CI.gHID].ToString());
                        Checkin[i] = Convert.ToDateTime(grdCI[currow, (int)CI.gDTI].ToString());
                        Checkout[i] = Convert.ToDateTime(grdCI[currow, (int)CI.gDTO].ToString());
                        HotelVoucher[i] = grdCI[currow, (int)CI.gVNO].ToString();
                        i++;
                    }
                    currow++;
                } 
                    lblMSG_HOTEL.Visible = CountRows == 0; 
                Add_Remove_Tabs_Hotel(Convert.ToInt16(CountRows), tcTransHotel, tpHotels, "tcHotel", "tbHotels", "Hotel Record");
                Add_Remove_Other_Details(Convert.ToInt16(CountRows), tcTransOther, tpOthers, "tcOth", "tbOth", "Voucher Other Details");
                Add_Remove_Email_Details(Convert.ToInt16(CountRows), tcTransEmail, tpEmailOptions, "tcEml", "tbEml", "Email Details"); 
                AllRows = 1;
                CountRows = 0;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if (grdCI[AllRows, grdCI.Cols[(int)CI.gSCI].Index] != null)  CountRows++;
                    AllRows++;
                }
                Sightseeing = new string[CountRows];
                SightID = new int[CountRows];
                currow = 1;
                i = 0;
                while (grdCI[currow, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if (grdCI[currow, grdCI.Cols[(int)CI.gSCI].Index] != null)
                    {
                        Sightseeing[i] = grdCI[currow, (int)CI.gSCN].ToString();
                        SightID[i] = Convert.ToInt32(grdCI[currow, (int)CI.gSCI].ToString());
                        i++;
                    }
                    currow++;
                } 
                    lblMSG_SS.Visible = CountRows == 0; 
                Add_Remove_Tabs_Sightseeing(Convert.ToInt16(CountRows), tcTransSightseeing, tpSighseeing, "tcSightseeing", "tbSightseeing", "Sightseeing Details", Mode);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void drpCountry_Selected_TextChanged(object sender, EventArgs e)
        {
            if (drpCountry.SelectedText != "")  Check_For_Saarc_Country(); 
            Get_Tour_Number();
            Fill_Control();
        }
        private void nudNoOfDays_ValueChanged(object sender, EventArgs e)
        {
            dtpDeparture.Value = dtpArrival.Value.AddDays(Convert.ToInt32(nudNoOfDays.Value));
        }
        private void dtpDeparture_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtpArrival.Value == dtpDeparture.Value)
                {
                    DateDeparture = dtpDeparture.Value;
                    return;
                }
                if (dtpArrival.Value < dtpDeparture.Value)
                {
                    TimeSpan tspan = dtpDeparture.Value - dtpArrival.Value;
                    nudNoOfDays.Value =  (int)(tspan.TotalDays );
                }
                else
                {
                    MessageBox.Show("Arrival Date Cannot Be Greater Than The Departure Date.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dtpDeparture.Value = dtpArrival.Value;
                    nudNoOfDays.Value = 0;
                    return;
                }
                DateDeparture = dtpDeparture.Value;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void drpAgent_Selected_TextChanged(object sender, EventArgs e)
        {
            if (Mode != 0) return;
            if (drpAgent.SelectedValue + "".Trim() == "")   return;
            int countryid = 0;
            string cid = "";
            cid = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CountryID from dbo.mst_AgentDetails WHERE ID=" + drpAgent.SelectedValue.Trim() + "").Rows[0]["CountryID"] + "".Trim();
            if (cid.Trim() == "")    return;
            countryid = Convert.ToInt32(cid);
            drpCountry.setSelectedValue(countryid.ToString().Trim());
            Check_For_Saarc_Country();
            Get_Tour_Number();
        }
        private void Check_For_Saarc_Country()
        {
            if (drpCountry.SelectedValue == null)  return;
            chkSaarc.Checked = (Convert.ToBoolean(Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT IsSaarc from dbo.mst_Country WHERE ID=" + drpCountry.SelectedValue.Trim() + "").Rows[0]["IsSaarc"]));
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            int reportid = Convert.ToInt32(cmbReportType.SelectedValue.ToString().Trim());
            Print_Transaction_Report(SystemCode, reportid);
            Generate_Email_Options();
            IsPreview = true;//MARK AS PREVIEWED ONE TIME AT LEAST
        }
        private void btnTEGenerate_Click(object sender, EventArgs e)  {  Generate_Total_Expenditures();  }
        private void Generate_Total_Expenditures()
        {
            try
            {
                double TotVat = 0.00, TotTax = 0.00, TotServ = 0.00, Tot = 0.00, GrandTot = 0.00;
                int AllRows = 1;
                int RowCount = 0;
                pbTE.Maximum = Count_Total_For_ProgressBar();
                pbTE.Value = 0;
                int TGirdRow = 1;
                if (HasHotel)
                {
                    C1.Win.C1FlexGrid.CellStyle rsHE = grdTE.Styles.Add("HE");
                    rsHE.BackColor = Color.PapayaWhip;
                    grdTE.Rows[TGirdRow].Style = grdTE.Styles["HE"];
                    grdTE[TGirdRow, (int)TE.gEXN] = "HOTEL EXEPENSES";
                    TGirdRow++;
                    AllRows = 1;
                    while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        if (grdCI[AllRows, grdCI.Cols[(int)CI.gHID].Index] != null)
                            RowCount++;
                        AllRows++;
                    }
                    string nm;
                    for (int i = 0; i < RowCount; i++)
                    {
                        RowNumb = 1;
                        nm = "HTL" + (i + 1);
                        User_Controls.ucHotelNavigation u = dicHotels[nm];
                        grdTE[TGirdRow, (int)TE.gEXN] = HotelName[i].ToString();
                        C1.Win.C1FlexGrid.CellStyle HNM = grdTE.Styles.Add("HNM");
                        HNM.BackColor = Color.PowderBlue;
                        grdTE.Rows[TGirdRow].Style = grdTE.Styles["HNM"];
                        TGirdRow++;
                        while (u.grdHotel[RowNumb, u.grdHotel.Cols[0].Index] != null)
                        {
                            pbTE.Value++;
                            grdTE[TGirdRow, (int)TE.gEXN] = u.grdHotel[RowNumb, (int)HG.gRTN].ToString();
                            if (u.grdHotel[RowNumb, (int)HG.gVAT] != null)
                            {
                                grdTE[TGirdRow, (int)TE.gVAT] = u.grdHotel[RowNumb, (int)HG.gVAT].ToString();
                                TotVat += Convert.ToDouble(u.grdHotel[RowNumb, (int)HG.gVAT].ToString());
                            }
                            if (u.grdHotel[RowNumb, (int)HG.gTAX] != null)
                            {
                                grdTE[TGirdRow, (int)TE.gTAX] = u.grdHotel[RowNumb, (int)HG.gTAX].ToString();
                                TotTax += Convert.ToDouble(u.grdHotel[RowNumb, (int)HG.gTAX].ToString());
                            }
                            if (u.grdHotel[RowNumb, (int)HG.gSCH] != null)
                            {
                                grdTE[TGirdRow, (int)TE.gSCH] = u.grdHotel[RowNumb, (int)HG.gSCH].ToString();
                                TotServ += Convert.ToDouble(u.grdHotel[RowNumb, (int)HG.gSCH].ToString());
                            }
                            if (u.grdHotel[RowNumb, (int)HG.gPRI] != null)
                            {
                                grdTE[TGirdRow, (int)TE.gPRC] = u.grdHotel[RowNumb, (int)HG.gPRI].ToString();
                                Tot += Convert.ToDouble(u.grdHotel[RowNumb, (int)HG.gPRI].ToString());
                            }
                            if (u.grdHotel[RowNumb, (int)HG.gTOT] != null)
                            {
                                grdTE[TGirdRow, (int)TE.gTOT] = u.grdHotel[RowNumb, (int)HG.gTOT].ToString();
                                GrandTot += Convert.ToDouble(u.grdHotel[RowNumb, (int)HG.gTOT].ToString());
                            }
                            TGirdRow++;
                            RowNumb++;
                        }
                        TGirdRow += 1;
                    }
                }
                if (HasSightSeeing)
                {
                    grdTE.Rows[TGirdRow].Style = grdTE.Styles["HE"];
                    grdTE[TGirdRow, (int)TE.gEXN] = "SIGHSEEING EXEPENSES";
                    TGirdRow++;
                    AllRows = 1;
                    while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        AllRows++;
                    }
                    string nm;
                    for (int i = 0; i < AllRows - 1; i++)
                    {
                        RowNumb = 1;
                        nm = "SGH" + (i + 1);
                        User_Controls.ucSSNavigation u = dicSight[nm];
                        grdTE[TGirdRow, (int)TE.gEXN] = Sightseeing[i].ToString();
                        grdTE.Rows[TGirdRow].Style = grdTE.Styles["HNM"];
                        TGirdRow++;
                        while (u.grdSE[RowNumb, u.grdSE.Cols[0].Index] != null)
                        {
                            pbTE.Value++;
                            grdTE[TGirdRow, (int)TE.gEXN] = u.grdSE[RowNumb, (int)SE.gSPN].ToString();
                            grdTE[TGirdRow, (int)TE.gTOT] = u.grdSE[RowNumb, (int)SE.gTOT].ToString();
                            GrandTot += Convert.ToDouble(u.grdSE[RowNumb, (int)SE.gTOT].ToString());
                            TGirdRow++;
                            RowNumb++;
                        }
                        TGirdRow += 1;
                    }
                }
                grdTE.Rows[TGirdRow].Style = grdTE.Styles["HE"];
                grdTE[TGirdRow, (int)TE.gEXN] = "TRAVELLING EXEPENSES";
                TGirdRow++;
                RowNumb = 1;
                while (grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] != null)
                {
                    pbTE.Value++;
                    grdTE[TGirdRow, (int)TE.gEXN] = grdTR[RowNumb, (int)TR.gFR].ToString() + " TO " + grdTR[RowNumb, (int)TR.gTO].ToString();
                    grdTE[TGirdRow, (int)TE.gTOT] = grdTR[RowNumb, (int)TR.gCH].ToString();
                    GrandTot += Convert.ToDouble(grdTR[RowNumb, (int)TR.gCH].ToString());
                    TGirdRow++;
                    RowNumb++;
                }
                TGirdRow += 1;
                grdTE.Rows[TGirdRow].Style = grdTE.Styles["HE"];
                grdTE[TGirdRow, (int)TE.gEXN] = "OTHER EXEPENSES";
                TGirdRow++;
                RowNumb = 1;
                while (grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] != null)
                {
                    pbTE.Value++;
                    grdTE[TGirdRow, (int)TE.gEXN] = grdOE[RowNumb, (int)OE.gEXN].ToString();
                    grdTE[TGirdRow, (int)TE.gVAT] = grdOE[RowNumb, (int)OE.gVAT].ToString();
                    grdTE[TGirdRow, (int)TE.gTAX] = grdOE[RowNumb, (int)OE.gTAX].ToString();
                    grdTE[TGirdRow, (int)TE.gSCH] = grdOE[RowNumb, (int)OE.gSCH].ToString();
                    grdTE[TGirdRow, (int)TE.gPRC] = grdOE[RowNumb, (int)OE.gPRC].ToString();
                    grdTE[TGirdRow, (int)TE.gTOT] = grdOE[RowNumb, (int)OE.gTOT].ToString();
                    TotTax += Convert.ToDouble(grdOE[RowNumb, (int)OE.gTAX].ToString());
                    TotVat += Convert.ToDouble(grdOE[RowNumb, (int)OE.gVAT].ToString());
                    TotServ += Convert.ToDouble(grdOE[RowNumb, (int)OE.gSCH].ToString());
                    Tot += Convert.ToDouble(grdOE[RowNumb, (int)OE.gPRC].ToString());
                    GrandTot += Convert.ToDouble(grdOE[RowNumb, (int)OE.gTOT].ToString());
                    TGirdRow++;
                    RowNumb++;
                }
                TGirdRow += 1;
                C1.Win.C1FlexGrid.CellStyle TOT = grdTE.Styles.Add("TOT");
                TOT.BackColor = Color.GreenYellow;
                grdTE.Rows[TGirdRow].Style = grdTE.Styles["TOT"];
                grdTE[TGirdRow, (int)TE.gEXN] = "GRAND TOTAL";
                grdTE[TGirdRow, (int)TE.gVAT] = TotVat;
                grdTE[TGirdRow, (int)TE.gTAX] = TotTax;
                grdTE[TGirdRow, (int)TE.gSCH] = TotServ;
                grdTE[TGirdRow, (int)TE.gPRC] = Tot;
                grdTE[TGirdRow, (int)TE.gTOT] = GrandTot;
                grdTE.Rows.Count = TGirdRow + 1;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private int Count_Total_For_ProgressBar()
        {
            try
            {
                int AllRows = 1;
                int RowCount = 0;
                int MaxCount = 0;
                if (HasHotel)
                {
                    while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        if (grdCI[AllRows, grdCI.Cols[(int)CI.gHID].Index] != null)    RowCount++;
                        AllRows++;
                    }
                    string nm;
                    for (int i = 0; i < RowCount; i++)
                    {
                        RowNumb = 1;
                        nm = "HTL" + (i + 1);
                        User_Controls.ucHotelNavigation u = dicHotels[nm];
                        while (u.grdHotel[RowNumb, u.grdHotel.Cols[0].Index] != null)
                        {
                            MaxCount++;
                            RowNumb++;
                        }
                    }
                }
                if (HasSightSeeing)
                {
                    while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        if (grdCI[AllRows, grdCI.Cols[(int)CI.gSCI].Index] != null) RowCount++;
                        AllRows++;
                    }
                    string nm;
                    for (int i = 0; i < RowCount; i++)
                    {
                        RowNumb = 1;
                        nm = "SGH" + (i + 1);
                        User_Controls.ucSSNavigation u = dicSight[nm];
                        while (u.grdSE[RowNumb, u.grdSE.Cols[0].Index] != null)   {  MaxCount++;     RowNumb++;  }
                    }
                }
                RowNumb = 1;
                while (grdTR[RowNumb, grdTR.Cols[(int)TR.gTN].Index] != null)
                {
                    MaxCount++;
                    RowNumb++;
                }
                RowNumb = 1;
                while (grdOE[RowNumb, grdOE.Cols[(int)OE.gEXN].Index] != null)
                {
                    MaxCount++;
                    RowNumb++;
                }
                return MaxCount;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private Boolean Validate_Email_Options()        {                return true;         }
        private void chkArrival_CheckedChanged(object sender, EventArgs e)        {            dtpArrival.Visible = nudNoOfDays.Enabled = chkArrival.Checked;         }
        private void chkDeparture_CheckedChanged(object sender, EventArgs e)        {                 dtpDeparture.Visible = chkDeparture.Checked;        }
        private void grdAge_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTAgeFrom, DTAgeTo;
            string SqlQuery;
            if (e.Col == grdAge.Cols[(int)AG.gAFR].Index)
            {
                SqlQuery = "SELECT ID,Age FROM mst_AgeDetails Where IsNull(IsActive,0)=1";
                DTAgeFrom = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTAgeFrom;
                frm.Width = grdAge.Cols[(int)AG.gAFR].Width;
                frm.Height = grdAge.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdAge);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdAge[grdAge.Row, (int)AG.gAFI] = SelText[0];
                    grdAge[grdAge.Row, (int)AG.gAFR] = SelText[1];
                }
            }
            else if (e.Col == grdAge.Cols[(int)AG.gATO].Index)
            {
                SqlQuery = "SELECT ID,Age FROM mst_AgeDetails Where IsNull(IsActive,0)=1";
                DTAgeTo = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTAgeTo;
                frm.Width = grdAge.Cols[(int)AG.gATO].Width;
                frm.Height = grdAge.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdAge);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdAge[grdAge.Row, (int)AG.gATI] = SelText[0];
                    grdAge[grdAge.Row, (int)AG.gATO] = SelText[1];
                }
            }
        }
        private void grdAge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdAge.Rows.Remove(grdAge.Row);
                grdAge.Rows[1].AllowEditing = true;
            }
        }
        private void grdAge_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)    return; 
            grdAge.Rows[1].AllowEditing = true;
            if (grdAge.Rows.Count < 3)  return; 
            grdAge.Rows[grdAge.Row].AllowEditing = grdAge[grdAge.Row - 1, 0] != null;
        }
        private void grdShopping_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdShopping.Rows.Remove(grdShopping.Row);
                grdShopping.Rows[1].AllowEditing = true;
                grdShopping.Rows.Count += 1;
            }
        }
        private void grdShopping_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)   return; 
            grdShopping.Rows[1].AllowEditing = true;
            if (grdShopping.Rows.Count < 3)  return;  
            grdShopping.Rows[grdShopping.Row].AllowEditing = grdShopping[grdShopping.Row - 1, 0] != null;
        }
        private void grdShopping_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTCity, DTShop;
            if (e.Col == grdShopping.Cols[(int)SD.gCTY].Index)
            {
                DTCity = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,City FROM mst_City WHERE Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTCity;
                frm.SubForm = new Master.frmCity();
                frm.Width = grdShopping.Cols[(int)SD.gCTY].Width;
                frm.Height = grdShopping.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdShopping);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdShopping[grdShopping.Row, (int)SD.gCID] = SelText[0];
                    grdShopping[grdShopping.Row, (int)SD.gCTY] = SelText[1];
                }
            }
            else if (e.Col == grdShopping.Cols[(int)SD.gSNM].Index)
            {
                if (grdShopping[grdShopping.Row, (int)SD.gCID] == null)
                {
                    MessageBox.Show("Please Select the City.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                DTShop = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_ShopDetails WHERE Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTShop;
                frm.SubForm = new Master.frmShop();
                frm.Width = grdShopping.Cols[(int)SD.gSNM].Width;
                frm.Height = grdShopping.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdShopping);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdShopping[grdShopping.Row, (int)SD.gSID] = SelText[0];
                    grdShopping[grdShopping.Row, (int)SD.gSNM] = SelText[1];
                }
            }
        }
        private void grdItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdItems.Rows.Remove(grdItems.Row);
                grdItems.Rows[1].AllowEditing = true;
                grdItems.Rows.Count += 1;
            }
        }
        private void grdItems_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true) return; 
            grdItems.Rows[1].AllowEditing = true;
            if (grdItems.Rows.Count < 3)  return;  
            grdItems.Rows[grdItems.Row].AllowEditing = grdItems[grdItems.Row - 1, 0]!= null;
        }
        private void grdItems_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTItems, DTDriver;
            string sql;
            if (e.Col == grdItems.Cols[(int)TI.gINM].Index)
            {
                DTItems = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_TransportItems WHERE IsNull(IsActive,0)=1");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTItems;
                frm.Width = grdItems.Cols[(int)SD.gCTY].Width;
                frm.Height = grdItems.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdItems);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdItems[grdItems.Row, (int)TI.gIID] = SelText[0];
                    grdItems[grdItems.Row, (int)TI.gINM] = SelText[1];
                }
            }
            string list;
            if (e.Col == grdTAdvance.Cols[(int)TA.gDNM].Index)
            {
                if (Convert.ToBoolean(grdTAdvance[grdTAdvance.Row, (int)TA.gIDR]))
                {
                    list = getCurrent_Driver_Guide_List(true);
                    if (list.Trim() != "")  sql = "SELECT  ID,ISNULL(DriverName,'')AS DriverName FROM vw_TR_DriverVSVehicle WHERE DriverName<>'' AND IsNull(IsActive,0)=1 AND ID IN(" + list.Trim() + ") ORDER BY DriverName";
                    else sql = "SELECT  ID,ISNULL(DriverName,'')AS DriverName FROM vw_TR_DriverVSVehicle WHERE DriverName<>'' AND IsNull(IsActive,0)=1 ORDER BY DriverName";
                   }
                else
                {
                    list = getCurrent_Driver_Guide_List(false);
                    if (list.Trim() != "")  sql = "SELECT  ID,ISNULL(Name,'')AS Name FROM vwGuideVsEmployee WHERE Name<>'' AND IsNull(IsActive,0)=1 AND ID IN(" + list.Trim() + ") ORDER BY Name";
                     else  sql = "SELECT  ID,ISNULL(Name,'')AS Name FROM vwGuideVsEmployee WHERE Name<>'' AND IsNull(IsActive,0)=1 ORDER BY Name";
                     }
                DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTDriver;
                frm.Width = grdTAdvance.Cols[(int)TA.gDNM].Width;
                frm.Height = grdTAdvance.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTAdvance);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    int ID = Convert.ToInt16(SelText[0]);
                    if (Convert.ToBoolean(grdTAdvance[grdTAdvance.Row, (int)TA.gIDR]))
                        DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,DriverCode,DriverName as Name,OwnerName,LicenseNo,Tel1 FROM vw_TR_DriverVSVehicle WHERE ID=" + ID + " AND IsNull(IsActive,0)=1 ORDER BY DriverName");
                    else
                        DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM vwGuideVsEmployee WHERE ID=" + ID + " AND IsNull(IsActive,0)=1 ORDER BY Name");
                    if (DTDriver.Rows[0]["ID"].ToString() != "")  grdTAdvance[grdTAdvance.Row, (int)TA.gDID] = DTDriver.Rows[0]["ID"].ToString();
                    if (DTDriver.Rows[0]["Name"].ToString() != "")  grdTAdvance[grdTAdvance.Row, (int)TA.gDNM] = DTDriver.Rows[0]["Name"].ToString();
                    grdTAdvance[grdTAdvance.Row, (int)TA.gENM] = "Advance";
                    grdTAdvance[grdTAdvance.Row, (int)TA.gIPD] = 1;
                    grdTAdvance[grdTAdvance.Row, (int)TA.gPDT] = Classes.clsGlobal.CurDate();
                }
            }
        }
        private void grdSim_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdSim.Rows.Remove(grdSim.Row);
                grdSim.Rows[1].AllowEditing = true;
                grdSim.Rows.Count += 1;
            }
        }
        private void grdSim_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)  return; 
            grdSim.Rows[1].AllowEditing = true;
            if (grdSim.Rows.Count < 3) return;  
                grdSim.Rows[grdSim.Row].AllowEditing = (grdSim[grdSim.Row - 1, 0] != null);
        }
        private void grdScratch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdScratch.Rows.Remove(grdScratch.Row);
                grdScratch.Rows[1].AllowEditing = true;
                grdScratch.Rows.Count += 1;
            }
        }
        private void grdScratch_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true) return; 
            grdScratch.Rows[1].AllowEditing = true;
            if (grdScratch.Rows.Count < 3) return;
            grdScratch.Rows[grdScratch.Row].AllowEditing = (grdScratch[grdScratch.Row - 1, 0] != null);
        }
        private void grdTExpense_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToBoolean(grdTExpense[grdTExpense.Row, (int)TP.gIPD]))
            {
                grdTExpense.Rows[grdTExpense.Row].AllowEditing = false;
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                grdTExpense.Rows.Remove(grdTExpense.Row);
                grdTExpense.Rows[1].AllowEditing = true;
                grdTExpense.Rows.Count += 1;
            }
        }
        private void grdTExpense_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true) return; 
            grdTExpense.Rows[1].AllowEditing = true;
            if (grdTExpense.Rows.Count < 3) return; 
            grdTExpense.Rows[grdTExpense.Row].AllowEditing = grdTExpense[grdTExpense.Row - 1, 0] != null;
        }
        private void grdGudie_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToBoolean(grdGudie[grdGudie.Row, (int)GD.gIPD]))
            {
                grdGudie.Rows[grdGudie.Row].AllowEditing = false;
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                grdGudie.Rows.Remove(grdGudie.Row);
                grdGudie.Rows[1].AllowEditing = true;
            }
        }
        private void grdGudie_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)   return; 
            grdGudie.Rows[1].AllowEditing = true;
            if (grdGudie.Rows.Count < 3)  {  return;   } 
            grdGudie.Rows[grdGudie.Row].AllowEditing = grdGudie[grdGudie.Row - 1, 0] != null;
        }
        private void grdDriver_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToBoolean(grdDriver[grdDriver.Row, (int)DR.gIPD]))
            {
                grdDriver.Rows[grdDriver.Row].AllowEditing = false;
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                grdDriver.Rows.Remove(grdDriver.Row);
                grdDriver.Rows[1].AllowEditing = true;
            }
        }
        private void grdDriver_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)  return; 
            grdDriver.Rows[1].AllowEditing = true;
            if (grdDriver.Rows.Count < 3) return; 
            grdDriver.Rows[grdDriver.Row].AllowEditing = (grdDriver[grdDriver.Row - 1, 0] != null);
        }
        private void grdGudie_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTGuide;
                if (e.Col == grdGudie.Cols[(int)GD.gGNM].Index)
                {
                    DTGuide = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM vwGuideVsEmployee WHERE IsNull(IsActive,0)=1 ORDER BY Name");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTGuide;
                    frm.SubForm = new Master.frmGuide();
                    frm.Width = grdGudie.Cols[(int)GD.gGNM].Width;
                    frm.Height = grdGudie.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdGudie);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        int GuideID = Convert.ToInt16(SelText[0]);
                        Sort_Out_Guide_Details();
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private void Sort_Out_Guide_Details()
        {
            try
            {
                int Mrownumb = 1;
                string sql;
                DataTable DTGuide;
                int GuideID, count = 1;
                while (grdMatch[Mrownumb, grdMatch.Cols[(int)MG.gDID].Index] != null || grdMatch[Mrownumb, grdMatch.Cols[(int)MG.gGID].Index] != null)
                {
                    if (grdMatch[Mrownumb, grdMatch.Cols[(int)MG.gGID].Index] != null && grdMatch[Mrownumb, (int)MG.gGNM].ToString().Trim() != "")
                    {
                        GuideID = Convert.ToInt32(grdMatch[Mrownumb, (int)MG.gGID]);
                        sql = "SELECT ID,Code,GuideName,Fee,LicenseNo,Tel2 TelHome FROM vw_ALL_GUIDE_DETAILS WHERE ID=" + GuideID + " AND IsNull(IsActive,0)=1 ORDER BY GuideName";
                        DTGuide = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                        if (DTGuide.Rows.Count == 0 || DTGuide.Rows[0]["ID"] + "".Trim() == "")       {   RowNumb++;    continue;     }
                        if (DTGuide.Rows[0]["ID"].ToString() != "") grdGudie[count, (int)GD.gGID] = DTGuide.Rows[0]["ID"].ToString();
                        if (DTGuide.Rows[0]["GuideName"].ToString() != "") grdGudie[count, (int)GD.gGNM] = DTGuide.Rows[0]["GuideName"].ToString();
                        if (DTGuide.Rows[0]["TelHome"].ToString() != "") grdGudie[count, (int)GD.gTEL] = DTGuide.Rows[0]["TelHome"].ToString();
                        if (grdMatch[RowNumb, (int)MG.gDID] + "".Trim() != "") grdGudie[count, (int)GD.gDID] = Convert.ToInt32(grdMatch[RowNumb, (int)MG.gDID]);
                        if (DTGuide.Rows[0]["Code"].ToString() != "") grdGudie[count, (int)GD.gGCD] = DTGuide.Rows[0]["Code"].ToString();
                        if (DTGuide.Rows[0]["Fee"].ToString() != "") grdGudie[count, (int)GD.gFEE] = DTGuide.Rows[0]["Fee"].ToString();
                        grdGudie[count, (int)GD.gNOD] = nudNoOfDays.Value.ToString();
                        if (DTGuide.Rows[0]["LicenseNo"].ToString() != "") grdGudie[count, (int)GD.gGLC] = DTGuide.Rows[0]["LicenseNo"].ToString();
                        Mrownumb++;
                        count++;
                    }
                    else   Mrownumb++; 
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private void grdDriver_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTDriver;
            try
            {
                if (e.Col == grdDriver.Cols[(int)DR.gDNM].Index)
                {
                    DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,DriverName FROM vw_TR_DriverVSVehicle WHERE IsNull(IsActive,0)=1 ORDER BY DriverName");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTDriver;
                    frm.SubForm = new Master.frmTRDetails();
                    frm.Width = grdDriver.Cols[(int)DR.gDNM].Width;
                    frm.Height = grdDriver.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdDriver);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        int DriverID = Convert.ToInt16(SelText[0]);
                        Sort_Out_Driver_Details(DriverID);
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void Add_Default_Expences(int DriverID)//, int grdMatchRow
        {
            try
            {
                int rwnumb = 0, ExRwNumb = 0;
                while (grdTExpense[ExRwNumb, grdCI.Cols[(int)TP.gIDN].Index] != null)    ExRwNumb++; 
                DataTable DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  DriverID ID,DriverCode,DriverName,OwnerName,LicenseNo,Tel1 FROM vw_ALL_DRIVER_VEHICLE_DETAILS WHERE DriverID=" + DriverID + " AND IsNull(IsActive,0)=1 ORDER BY DriverName");
                DataTable DTDefEX = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_TransportExpenses WHERE IsNull(IsDefault,0)=1 AND IsNull(IsActive,0)=1");
                while (DTDefEX.Rows.Count > rwnumb)
                {
                    grdTExpense[ExRwNumb, (int)TP.gIDR] = true;//DTDriver.Rows[0]["ParentID"].ToString();
                    grdTExpense[ExRwNumb, (int)TP.gDID] = DTDriver.Rows[0]["ID"].ToString();
                    grdTExpense[ExRwNumb, (int)TP.gDNM] = DTDriver.Rows[0]["DriverName"].ToString();
                    grdTExpense[ExRwNumb, (int)TP.gEID] = DTDefEX.Rows[rwnumb]["ID"].ToString();
                    grdTExpense[ExRwNumb, (int)TP.gENM] = DTDefEX.Rows[rwnumb]["Name"].ToString();//grdMatch.Row + 
                    grdTExpense[ExRwNumb, (int)TP.gAMT] = 0.00;
                    grdTExpense.AllowEditing = true;
                    rwnumb++;
                    ExRwNumb++;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void Sort_Out_Driver_Details(int DriverID)
        {
            try
            {
                int Drownumb = 1, Mrownumb = 1;
                while (grdMatch[Mrownumb, grdMatch.Cols[(int)MG.gDID].Index] != null || grdMatch[Mrownumb, grdMatch.Cols[(int)MG.gGID].Index] != null)
                {
                    if (grdMatch[Mrownumb, grdMatch.Cols[(int)MG.gDID].Index] != null && grdMatch[Mrownumb, (int)MG.gDNM].ToString().Trim() != "")
                    {
                        int did = Convert.ToInt32(grdMatch[Mrownumb, (int)MG.gDID]);
                        DataTable DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  DriverID ID,DriverCode,DriverName,OwnerName,LicenseNo,Tel1 FROM vw_ALL_DRIVER_VEHICLE_DETAILS WHERE DriverID=" + did + " AND IsNull(IsActive,0)=1 ORDER BY DriverName");
                        string ssql;
                        DataTable dt;
                        ssql = "SELECT ISNULL(IsPaid,0)AS IsPaid FROM trn_BasicTransport WHERE TransID=" + SystemCode + " AND DriverID=" + DTDriver.Rows[0]["ID"].ToString().Trim() + "";
                        dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                        if ((dt.Rows.Count > 0))
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["IsPaid"]))
                            {
                                Mrownumb++;
                                Drownumb++;
                                continue;
                            }
                        }
                        if (DTDriver.Rows[0]["ID"] + "".Trim() != "") grdDriver[Drownumb, (int)DR.gDID] = DTDriver.Rows[0]["ID"].ToString();
                        if (DTDriver.Rows[0]["DriverCode"] + "".Trim() != "") grdDriver[Drownumb, (int)DR.gDCD] = DTDriver.Rows[0]["DriverCode"].ToString();
                        if (DTDriver.Rows[0]["DriverName"] + "".Trim() != "") grdDriver[Drownumb, (int)DR.gDNM] = DTDriver.Rows[0]["DriverName"].ToString();
                        if (DTDriver.Rows[0]["OwnerName"] + "".Trim() != "") grdDriver[Drownumb, (int)DR.gDCM] = DTDriver.Rows[0]["OwnerName"].ToString();
                        if (DTDriver.Rows[0]["LicenseNo"] + "".Trim() != "") grdDriver[Drownumb, (int)DR.gDGL] = DTDriver.Rows[0]["LicenseNo"].ToString();
                        if (DTDriver.Rows[0]["Tel1"] + "".Trim() != "") grdDriver[Drownumb, (int)DR.gTEL] = DTDriver.Rows[0]["Tel1"].ToString();
                        Drownumb++;
                        Mrownumb++;
                    }
                    else
                    {
                        Mrownumb++;
                    }
                }
                btnAsItenarary_Click(null, null);
                btnGetall_Click(null, null);
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private void grdGudie_Leave(object sender, EventArgs e)
        {
            if ((grdGudie[1, grdGudie.Cols[(int)GD.gGID].Index] != null))  DefGuideID = Convert.ToInt16(grdGudie[1, (int)GD.gGID].ToString()); 
            int count = 1;
            NoOfGuide = 0;
            while (grdGudie[count, grdShopping.Cols[(int)GD.gGID].Index] != null)
            {
                NoOfGuide++;
                count++;
            }
        }
        private void grdDriver_Leave(object sender, EventArgs e)    { if ((grdDriver[1, grdDriver.Cols[(int)DR.gDID].Index] != null)) DefDriverID = Convert.ToInt16(grdDriver[1, (int)DR.gDID].ToString());   }
        private void nudAdult_Leave(object sender, EventArgs e)   {   Fill_Grids();        }
        private void nudChild_Leave(object sender, EventArgs e)  {  Fill_Grids();  }
        private void rdbAmend_CheckedChanged(object sender, EventArgs e)  {  btnSelHot.Enabled = rdbAmend.Checked;     }
        private void Fill_Hotel_Name_In_Increase_Amend()
        {
            try
            {
                RowNumb = 1;
                int HOID = 0, UID = 0;
                string HOName = "";
                string MealFor = "";
                DateTime ChkIN;
                if (DTHot != null)    DTHot.Clear();
                DTHot = new DataTable();
                DTHot.Columns.Add("UniqueID", typeof(int));
                DTHot.Columns.Add("HotelID", typeof(int));
                DTHot.Columns.Add("CheckIn", typeof(DateTime));
                DTHot.Columns.Add("HotelName", typeof(string));
                DTHot.Columns.Add("MealFor", typeof(string));
                DTHot.Columns.Add("Select", typeof(bool));
                while (grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    if (!Convert.ToBoolean(grdCI[RowNumb, (int)CI.gIPD]))
                    {
                        UID = Int32.Parse(grdCI[RowNumb, (int)CI.gIDN].ToString());
                        HOID = Int32.Parse(grdCI[RowNumb, (int)CI.gHID].ToString());
                        HOName = grdCI[RowNumb, (int)CI.gHNM].ToString();
                        ChkIN = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gDTI].ToString());
                        if (grdCI[RowNumb, (int)CI.gMID] == null)  MealFor = "";    else    MealFor = grdCI[RowNumb, (int)CI.gMID].ToString();
                        DTHot.Rows.Add(UID, HOID, ChkIN, HOName, MealFor);
                    }
                    RowNumb++;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void chkNoOfPax_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAdvance.Checked)
            {
                grdCI.Cols[(int)CI.gGNM].Width = 150;
                grdCI.Cols[(int)CI.gNOA].Width = 70;
                grdCI.Cols[(int)CI.gNOC].Width = 70;
                grdCI.Cols[(int)CI.gNOG].Width = 70;
                grdCI.Cols[(int)CI.gFOC].Width = 70;
                grdCI.Cols[(int)CI.gDPY].Width = 70;
            }
            else 
                grdCI.Cols[(int)CI.gGNM].Width =    grdCI.Cols[(int)CI.gCAN].Width =   grdCI.Cols[(int)CI.gNOA].Width =   grdCI.Cols[(int)CI.gNOC].Width =    grdCI.Cols[(int)CI.gNOG].Width =    grdCI.Cols[(int)CI.gFOC].Width =  grdCI.Cols[(int)CI.gNAP].Width =   grdCI.Cols[(int)CI.gDPY].Width = 0;
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
            string sql;
            string list;
            if (e.Col == grdTAdvance.Cols[(int)TA.gDNM].Index)
            {
                if (Convert.ToBoolean(grdTAdvance[grdTAdvance.Row, (int)TA.gIDR]))
                {
                    list = getCurrent_Driver_Guide_List(true);
                    if (list.Trim() != "")  sql = "SELECT  ID,ISNULL(DriverName,'')AS DriverName FROM vw_TR_DriverVSVehicle  WHERE DriverName<>'' AND IsNull(IsActive,0)=1 AND ID IN(" + list.Trim() + ") ORDER BY DriverName";
                     else sql = "SELECT  ID,ISNULL(DriverName,'')AS DriverName FROM vw_TR_DriverVSVehicle WHERE DriverName<>'' AND IsNull(IsActive,0)=1 ORDER BY DriverName";
                       }
                else
                {
                    list = getCurrent_Driver_Guide_List(false);
                    if (list.Trim() != "")  sql = "SELECT  ID,ISNULL(Name,'')AS Name FROM vwGuideVsEmployee WHERE Name<>'' AND IsNull(IsActive,0)=1 AND ID IN(" + list.Trim() + ") ORDER BY Name";
                   else  sql = "SELECT  ID,ISNULL(Name,'')AS Name FROM vwGuideVsEmployee WHERE Name<>'' AND IsNull(IsActive,0)=1 ORDER BY Name";
                     }
                DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTDriver;
                frm.Width = grdTAdvance.Cols[(int)TA.gDNM].Width;
                frm.Height = grdTAdvance.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTAdvance);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    int ID = Convert.ToInt16(SelText[0]);
                    if (Convert.ToBoolean(grdTAdvance[grdTAdvance.Row, (int)TA.gIDR])) DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,DriverCode,DriverName as Name,OwnerName,LicenseNo,Tel1 FROM vw_TR_DriverVSVehicle WHERE ID=" + ID + " AND IsNull(IsActive,0)=1 ORDER BY DriverName");
                    else DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM vwGuideVsEmployee WHERE ID=" + ID + " AND IsNull(IsActive,0)=1 ORDER BY Name");
                    if (DTDriver.Rows[0]["ID"].ToString() != "") grdTAdvance[grdTAdvance.Row, (int)TA.gDID] = DTDriver.Rows[0]["ID"].ToString();
                    if (DTDriver.Rows[0]["Name"].ToString() != "") grdTAdvance[grdTAdvance.Row, (int)TA.gDNM] = DTDriver.Rows[0]["Name"].ToString();
                    grdTAdvance[grdTAdvance.Row, (int)TA.gENM] = "Advance";
                    grdTAdvance[grdTAdvance.Row, (int)TA.gIPD] = 1;
                    grdTAdvance[grdTAdvance.Row, (int)TA.gPDT] = Classes.clsGlobal.CurDate();
                }
            }
        }
        private void btnGenTA_Click(object sender, EventArgs e)
        {
            grdTAdvance.Rows.Count = 1;
            grdTAdvance.Rows.Count = 100;
            Create_Tour_Advance();
        }
        private void chkComplementary_CheckedChanged(object sender, EventArgs e)
        {
            if (chkComplementary.Checked)
            {
                rdbCancel.Checked = false;
            }
        }
        private void chkMeal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMeal.Checked)
            {
                btnSelHot.Enabled = true;//drpHotel.Enabled = true;
                lblMealMode.Visible = true;
                cmbMealTime.Visible = true;
            }
            else
            {
                lblMealMode.Visible = false;
                cmbMealTime.Visible = false;
            }
        }
        private void chkManage_CheckedChanged(object sender, EventArgs e)
        {
            if (Mode != 1)
            {
                rdbAmend.Enabled = false;
                rdbCancel.Enabled = false;
            }
            if (chkManage.Checked)
            {
                if (tcGroupAmend.SelectedTab.Name == "tpOthers")
                {
                    Fill_Hotel_Name_In_Increase_Amend();
                }
                btnSelHot.Enabled = true;//drpHotel.Enabled = true;
                rdbAmend.Checked = false;
                rdbCancel.Checked = false;
                chkComplementary.Checked = false;
                chkMeal.Checked = false;
                gbVType.Enabled = true;
            }
            else
            {
                rdbAmend.Checked = false;
                rdbCancel.Checked = false;
                chkComplementary.Checked = false;
                chkMeal.Checked = false;
                gbVType.Enabled = false;
            }
        }
        private void rdbCancel_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCancel.Checked)
            {
                btnSelHot.Enabled = true;//drpHotel.Enabled = true;
                chkComplementary.Checked = false;
            }
        }
        private void btnAsItenarary_Click(object sender, EventArgs e)
        {
            int Rownumb = 1;
            DataTable DT, DTAirpotCity, DTDriver, DTGuide, DTDistance;
            int cityID = 0, Hotid = 0;
            string cityName = "";
            try
            {
                grdTR.Rows.Count = 2;
                grdTR.Rows.Count = 500;
                int Count = 0;
                int CountTour = 1;
                while (grdMatch[Count + 1, grdCI.Cols[(int)MG.gDID].Index] != null)//enum MG { gDID, gDNM, gGID, gGNM };
                {
                    if (grdMatch[Count + 1, (int)MG.gDID] + "".Trim() != "")
                    {
                        DefDriverID = Convert.ToInt16(grdMatch[Count + 1, (int)MG.gDID].ToString());
                    }
                    else
                        break;
                    DefGuideID = 0;
                    if (grdMatch[Count + 1, (int)MG.gGID] + "".Trim() != "")
                    {
                        DefGuideID = Convert.ToInt16(grdMatch[Count + 1, (int)MG.gGID].ToString());
                    }
                    DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,DriverName,VehicleNo FROM vw_TR_DriverVSVehicle WHERE ID=" + DefDriverID + "");
                    DTGuide = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,GuideName FROM vw_ALL_GUIDE_DETAILS WHERE ID=" + DefGuideID + "");
                    Rownumb = 1;
                    while (grdCI[Rownumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        if (DTDriver.Rows.Count > 0)
                        {
                            grdTR[CountTour, (int)TR.gDI] = DTDriver.Rows[0]["ID"].ToString();
                            grdTR[CountTour, (int)TR.gDN] = DTDriver.Rows[0]["DriverName"].ToString();
                            grdTR[CountTour, (int)TR.gVN] = DTDriver.Rows[0]["VehicleNo"].ToString();
                        }
                        if (DTGuide.Rows.Count > 0)
                        {
                            grdTR[CountTour, (int)TR.gGI] = DTGuide.Rows[0]["ID"].ToString();
                            grdTR[CountTour, (int)TR.gGN] = DTGuide.Rows[0]["GuideName"].ToString();
                        }
                        if (Rownumb == 1)
                        {
                            DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  ID,Name FROM trn_TransTypes WHERE ID=1");
                            if (DT.Rows.Count > 0)
                            {
                                grdTR[CountTour, (int)TR.gTR] = DT.Rows[0]["ID"].ToString();
                                grdTR[CountTour, (int)TR.gTN] = DT.Rows[0]["Name"].ToString();
                            }
                        }
                        else
                        {
                            DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  ID,Name FROM trn_TransTypes WHERE ID=5");
                            grdTR[CountTour, (int)TR.gTR] = DT.Rows[0]["ID"].ToString();
                            grdTR[CountTour, (int)TR.gTN] = DT.Rows[0]["Name"].ToString();
                        }
                        grdTR[CountTour, (int)TR.gDT] = Convert.ToDateTime(grdCI[Rownumb, (int)CI.gDTI].ToString());
                        if (Rownumb == 1)
                        {
                            DTAirpotCity = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT  CityID,City FROM vw_Airport WHERE IsNull(IsDefault,0)=1 AND IsNull(IsActive,0)=1");
                            if (DTAirpotCity.Rows.Count > 0)
                            {
                                grdTR[CountTour, (int)TR.gFI] = DTAirpotCity.Rows[0]["CityID"].ToString();
                                grdTR[CountTour, (int)TR.gFR] = DTAirpotCity.Rows[0]["City"].ToString();
                            }
                        }
                        else
                        {
                            grdTR[CountTour, (int)TR.gFI] = grdCI[Rownumb - 1, (int)CI.gCID].ToString();
                            grdTR[CountTour, (int)TR.gFR] = grdCI[Rownumb - 1, (int)CI.gCTY].ToString();
                        }
                        Hotid = Convert.ToInt32(grdCI[Rownumb, (int)CI.gHID].ToString());
                        cityID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CityID from mst_HotelDetails WHERE ID=" + Hotid + "").Rows[0]["CityID"]);
                        cityName = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT City from mst_City WHERE ID=" + cityID + "").Rows[0]["City"].ToString();
                        grdTR[CountTour, (int)TR.gTI] = cityID;
                        grdTR[CountTour, (int)TR.gTO] = cityName;
                        DTDistance = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT DistanceKm FROM mst_CityItinerary WHERE CityFromID=" + Convert.ToInt32(grdTR[Rownumb, (int)TR.gFI].ToString()) + " AND CityToID=" + Convert.ToInt32(grdTR[Rownumb, (int)TR.gTI].ToString()) + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                        if (DTDistance.Rows.Count > 0)
                        {
                            if ((DTDistance.Rows[0]["DistanceKm"] + "").ToString().Trim() != "")
                                grdTR[CountTour, (int)TR.gDS] = Convert.ToDouble(DTDistance.Rows[0]["DistanceKm"].ToString());
                        }
                        CountTour++;
                        Rownumb++;
                    }
                    Count++;
                }
                Count = 1;
                CountTour = 1;
                while (grdMatch[Count, grdMatch.Cols[(int)MG.gDID].Index] != null)
                {
                    Rownumb = 1;
                    while (grdCI[Rownumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                    {
                        string vid = "", htl = "";
                        if ((grdCI[Rownumb, grdCI.Cols[(int)CI.gHNM].Index] != null))
                        {
                            htl = grdCI[Rownumb, (int)CI.gHNM].ToString().Trim();
                        }
                        if ((grdCI[Rownumb, grdCI.Cols[(int)CI.gVNO].Index] != null))
                        {
                            vid = grdCI[Rownumb, (int)CI.gVNO].ToString().Trim();
                        }
                        grdTR[CountTour, (int)TR.gVO] = vid;
                        grdTR[CountTour, (int)TR.gHT] = htl;
                        CountTour++;
                        Rownumb++;
                    }
                    Count++;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void btnGetall_Click(object sender, EventArgs e)
        {
            try
            {
                int Rownumb = 0;
                int DID = 0;
                while (grdDriver[Rownumb + 1, (int)DR.gDID] + "".Trim() != "")
                {
                    DID = Convert.ToInt32(grdDriver[Rownumb + 1, (int)DR.gDID]);
                    if ((Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ISNULL(IsPaid,0)AS IsPaid FROM trn_BasicTransport WHERE TransID=" + SystemCode + " AND DriverID=" + DID + "").Rows.Count > 0))
                    {
                        Rownumb++;
                        continue;
                    }
                    grdDBasic[Rownumb + 1, (int)DB.gDID] = Int32.Parse(grdDriver[Rownumb + 1, (int)DR.gDID].ToString());
                    grdDBasic[Rownumb + 1, (int)DB.gDNM] = grdDriver[Rownumb + 1, (int)DR.gDNM].ToString();
                    grdDBasic[Rownumb + 1, (int)DB.gSMT] = "";
                    grdDBasic[Rownumb + 1, (int)DB.gEMT] = "";
                    grdDBasic[Rownumb + 1, (int)DB.gTKM] = "";
                    grdDBasic[Rownumb + 1, (int)DB.gBAT] = "500.00";
                    grdDBasic[Rownumb + 1, (int)DB.gNON] = "";
                    grdDBasic[Rownumb + 1, (int)DB.gEXC] = true;
                    Rownumb++;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void Get_Hotel_Cost()
        {
            int RowNumb;
            double hotelcost = 0.00;
            try
            {
                int AllRows = 1;
                while (grdCI[AllRows, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    AllRows++;
                }
                string nm;
                for (int i = 0; i < AllRows - 1; i++)
                {
                    RowNumb = 1;
                    hotelcost = 0.00;
                    nm = "HTL" + (i + 1);
                    while (dicHotels[nm].grdHotel[RowNumb, (int)(int)HG.gVNO] + "".Trim() != "")
                    {
                        if (dicHotels[nm].grdHotel[RowNumb, dicHotels[nm].grdHotel.Cols[(int)HG.gTPR].Index] != null)
                            hotelcost += Convert.ToDouble(dicHotels[nm].grdHotel[RowNumb, (int)HG.gTPR].ToString());
                        RowNumb++;
                    }
                    grdCI[i + 1, (int)CI.gCST] = hotelcost.ToString();
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void btnSelHot_Click(object sender, EventArgs e)
        {
            Fill_Hotel_Name_In_Increase_Amend();
            User_Controls.frmChangeVoucher fcv = new Tourist_Management.User_Controls.frmChangeVoucher();
            fcv.DTHOTELS = DTHot;
            fcv.ShowDialog();
            DTHot = Classes.clsGlobal.SelectedHotels;
        }
        private void grdDBasic_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToBoolean(grdDBasic[grdDBasic.Row, (int)DB.gIPD]))
            {
                grdDBasic.Rows[grdDBasic.Row].AllowEditing = false;
                return;
            }
            if (e.KeyCode == Keys.Delete)
            {
                grdDBasic.Rows.Remove(grdDBasic.Row);
                grdDBasic.Rows[1].AllowEditing = true;
            }
        }
        private void grdDBasic_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true) return;
            if (grdDBasic.Rows.Count != 1) grdDBasic.Rows[1].AllowEditing = true;
            if (grdDBasic.Rows.Count < 3) return;
            grdDBasic.Rows[grdDBasic.Row].AllowEditing = grdDBasic[grdDBasic.Row - 1, 0] != null;
        }
        private void grdDBasic_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTDriver, DTRate;
                DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS DriverName FROM vw_TR_Driver Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1 ORDER BY Name");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTDriver;
                if (e.Col == grdDBasic.Cols[(int)DB.gDNM].Index)
                {
                    frm.SubForm = new Master.frmHotelSeason();
                    frm.Width = grdDBasic.Cols[(int)DB.gDNM].Width;
                    frm.Height = grdDBasic.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdDBasic);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdDBasic[grdDBasic.Row, (int)DB.gDID] = SelText[0];
                        grdDBasic[grdDBasic.Row, (int)DB.gDNM] = SelText[1];
                        int DriverID = Convert.ToInt32(SelText[0]);
                        DTRate = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ChargersPerKm FROM vw_TR_DriverVSVehicle Where ID=" + DriverID + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                        if (DTRate.Rows[0]["ChargersPerKm"].ToString() != "")
                        {
                            grdDBasic[grdDBasic.Row, (int)DB.gRKM] = DTRate.Rows[0]["ChargersPerKm"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void drpAgent_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmAgent", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            int countryid;
            if (drpCountry.SelectedValue.ToString() != "")
                countryid = Convert.ToInt32(drpCountry.SelectedValue.ToString());
            else
                countryid = 0;
            Master.frmAgent.CountryID = countryid;
            frm.ShowDialog();
            Fill_Control();
            Master.frmAgent.CountryID = 0;
            return;
        }
        private void btnuc_Click(object sender, EventArgs e)
        {
        }
        private void grdCI_Click(object sender, EventArgs e)
        {
            DataTable dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT UserGroupID FROM vw_CurrentUserDetails Where UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()));
            if (Convert.ToBoolean(grdCI[grdCI.Row, (int)CI.gIPD]))
            {
                if (dt.Rows[0]["UserGroupID"].ToString() != "1001")//----- Check if the user is an admin(user grp 1001). admins can make changes to paid records
                {
                    MessageBox.Show("This Cannot Be Modified.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    grdCI.Col = 0;
                }
            }
        }
        private void chkTourDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTourDetails.Checked)
            {
                grdTR.Cols[(int)TR.gVO].Width = 50;
                grdTR.Cols[(int)TR.gHT].Width = 100;
            }
            else
            {
                grdTR.Cols[(int)TR.gVO].Width = 0;
                grdTR.Cols[(int)TR.gHT].Width = 0;
            }
        }
        private DataTable Make_Hotetl_VoucherID()
        {
            try
            {
                int RowNumb = 1;
                DataTable DT = new DataTable();
                string vid, htl;
                DT.Columns.Add("HotelName", typeof(string));
                DT.Columns.Add("VoucherID", typeof(string));
                while (grdCI[RowNumb, grdCI.Cols[(int)CI.gCID].Index] != null)
                {
                    vid = "";
                    htl = "";
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gHNM].Index] != null)) htl = grdCI[RowNumb, (int)CI.gHNM].ToString().Trim();
                    if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gVNO].Index] != null)) vid = grdCI[RowNumb, (int)CI.gVNO].ToString().Trim();
                    DT.Rows.Add(vid, htl);
                    RowNumb++;
                }
                return DT;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private void grdTExpense_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTDriver, DTExpense;
            string sql = "", list = "";
            try
            {
                if (e.Col == grdTExpense.Cols[(int)TP.gDNM].Index)
                {
                    if (Convert.ToBoolean(grdTExpense[grdTExpense.Row, (int)TP.gIDR]))
                    {
                        list = getCurrent_Driver_Guide_List(true);
                        if (list.Trim() != "") sql = "SELECT  ID,ISNULL(DriverName,'')AS DriverName FROM vw_TR_DriverVSVehicle WHERE DriverName<>'' AND IsNull(IsActive,0)=1 AND ID IN(" + list.Trim() + ") ORDER BY DriverName";
                        else sql = "SELECT  ID,ISNULL(DriverName,'')AS DriverName FROM vw_TR_DriverVSVehicle WHERE DriverName<>'' AND IsNull(IsActive,0)=1 ORDER BY DriverName";
                    }
                    else
                    {
                        list = getCurrent_Driver_Guide_List(false);
                        if (list.Trim() != "") sql = "SELECT  ID,ISNULL(Name,'')AS Name FROM vwGuideVsEmployee WHERE Name<>'' AND IsNull(IsActive,0)=1 AND ID IN(" + list.Trim() + ") ORDER BY Name";
                        else sql = "SELECT  ID,ISNULL(Name,'')AS Name FROM vwGuideVsEmployee WHERE Name<>'' AND IsNull(IsActive,0)=1 ORDER BY Name";
                    }
                    DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTDriver;
                    frm.Width = grdTExpense.Cols[(int)TP.gDNM].Width;
                    frm.Height = grdTExpense.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdTExpense);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        int ID = Convert.ToInt16(SelText[0]);
                        if (Convert.ToBoolean(grdTExpense[grdTExpense.Row, (int)TP.gIDR])) DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,DriverCode,DriverName as Name,OwnerName,LicenseNo,Tel1 FROM vw_TR_DriverVSVehicle WHERE ID=" + ID + " AND IsNull(IsActive,0)=1 ORDER BY DriverName");
                        else DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM vwGuideVsEmployee WHERE ID=" + ID + " AND IsNull(IsActive,0)=1 ORDER BY Name");
                        if (DTDriver.Rows[0]["ID"].ToString() != "") grdTExpense[grdTExpense.Row, (int)TP.gDID] = DTDriver.Rows[0]["ID"].ToString();
                        if (DTDriver.Rows[0]["Name"].ToString() != "") grdTExpense[grdTExpense.Row, (int)TP.gDNM] = DTDriver.Rows[0]["Name"].ToString();
                    }
                }
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
                        if (DTExpense.Rows[0]["ID"].ToString() != "") grdTExpense[grdTExpense.Row, (int)TP.gEID] = SelText[0].ToString();
                        if (DTExpense.Rows[0]["Name"].ToString() != "") grdTExpense[grdTExpense.Row, (int)TP.gENM] = SelText[1].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private void grdMatch_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTGuide, DTDriver;
            try
            {
                if (e.Col == grdMatch.Cols[(int)MG.gDNM].Index)
                {
                    DTDriver = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(filterSelectedDrivers());
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTDriver;
                    frm.SubForm = new Settings.frmFilterDrvGuide();
                    frm.Width = grdMatch.Cols[(int)MG.gDNM].Width;
                    frm.Height = grdMatch.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdMatch);
                    Classes.clsGlobal.filterOutsideLoad = true;
                    Classes.clsGlobal.filterDrivers = true;
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        int DriverID = Convert.ToInt16(SelText[0]);
                        if (SelText[0].ToString().Trim() != "") grdMatch[grdMatch.Row, (int)MG.gDID] = SelText[0].ToString().Trim();
                        if (SelText[1].ToString().Trim() != "") grdMatch[grdMatch.Row, (int)MG.gDNM] = SelText[1].ToString().Trim();
                        if (chkArrival.Checked && chkDeparture.Checked)
                        {
                            grdMatch[grdMatch.Row, (int)MG.gADT] = Convert.ToDateTime(dtpArrival.Value);
                            grdMatch[grdMatch.Row, (int)MG.gDDT] = Convert.ToDateTime(dtpDeparture.Value);
                            Sort_Out_Driver_Details(DriverID);//----------- add new driver to driver grid
                        }
                        else
                        {
                            MessageBox.Show("Please Set Date Arrival & Departure !", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            grdMatch.Rows.Remove(grdMatch.Row);
                        }
                    }
                }
                if (e.Col == grdMatch.Cols[(int)MG.gGNM].Index)
                {
                    DTGuide = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(filterSelectedGuides());
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTGuide;
                    frm.SubForm = new Settings.frmFilterDrvGuide();
                    frm.Width = grdMatch.Cols[(int)MG.gGNM].Width;
                    frm.Height = grdMatch.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdMatch);
                    Classes.clsGlobal.filterOutsideLoad = true;
                    Classes.clsGlobal.filterDrivers = false;
                    SelText = frm.Open_Search();
                    if (Classes.clsGlobal.selectedPersonID != 0)
                    {
                        string[] list = new string[2];
                        list[0] = Classes.clsGlobal.selectedPersonID.ToString();
                        list[1] = Classes.clsGlobal.SelectedPersonName.ToString();
                        SelText = list;
                        Classes.clsGlobal.selectedPersonID = 0;
                        Classes.clsGlobal.SelectedPersonName = "";
                        Classes.clsGlobal.filterOutsideLoad = false;
                        Classes.clsGlobal.filterDrivers = false;
                    }
                    if (SelText != null)
                    {
                        int GuideID = Convert.ToInt16(SelText[0]);
                        if (SelText[0].ToString().Trim() != "") grdMatch[grdMatch.Row, (int)MG.gGID] = SelText[0].ToString().Trim();
                        if (SelText[1].ToString().Trim() != "") grdMatch[grdMatch.Row, (int)MG.gGNM] = SelText[1].ToString().Trim();
                        Sort_Out_Guide_Details();
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private DateTime Get_Driver_Last_TourDate(int DriverID)
        {
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
            DateTime date = new DateTime();
            try
            {
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                objCom.CommandType = CommandType.StoredProcedure;
                objCom.CommandText = "fun_DriverLastTravelDates";
                objCom.Parameters.Clear();
                objCom.Parameters.Add("@Flag", SqlDbType.Int).Value = 1;
                objCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = DriverID;
                objCom.Parameters.Add("@RtnValue", SqlDbType.Int);
                objCom.Parameters["@RtnValue"].Direction = ParameterDirection.ReturnValue;
                date = Convert.ToDateTime(objCom.ExecuteScalar());
                return date;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                throw (ex);
            }
        }
        private void grdMatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToBoolean(grdMatch[grdMatch.Row, (int)MG.gIPD]))
            {
                grdMatch.Rows[grdMatch.Row].AllowEditing = false;
                return;
            }
        }
        private void grdDBasic_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(grdDBasic[grdDBasic.Row, (int)DB.gIPD]))
            {
                MessageBox.Show("This Cannot Be Modified.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                grdDBasic.Col = 0;
            }
        }
        private void grdDriver_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(grdDriver[grdDriver.Row, (int)DR.gIPD]))
            {
                MessageBox.Show("This Cannot Be Modified.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                grdDriver.Col = 4;
            }
        }
        private void grdMatch_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(grdMatch[grdMatch.Row, (int)MG.gIPD]))
            {
                MessageBox.Show("This Cannot Be Modified.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                grdMatch[grdMatch.Row, (int)MG.gIDC] = 0;
                grdMatch.Col = 5;
            }
            else
            {
                Colour_Driver_Guide(grdMatch.Row);
            }
        }
        private string filterSelectedDrivers()
        {
            try
            {
                RowNumb = 1;
                string qry = "SELECT  DriverID,DriverName FROM vw_ALL_DRIVER_VEHICLE_DETAILS WHERE ISNULL(DriverName,'')<>'' AND IsNull(IsActive,0)=1";
                while (grdMatch[RowNumb, (int)MG.gDID] + "".Trim() != "")
                {
                    if (RowNumb == grdMatch.Row)
                    {
                        RowNumb++;
                        continue;
                    }
                    qry += " AND DriverID<>" + grdMatch[RowNumb, (int)MG.gDID].ToString().Trim();
                    RowNumb++;
                }
                qry += " ORDER BY DriverName";
                return qry;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private string filterSelectedGuides()
        {
            try
            {
                RowNumb = 1;
                string    qry = "SELECT  ID,GuideName AS Name FROM vw_ALL_GUIDE_DETAILS WHERE IsNull(IsActive,0)=1";
                while (grdMatch[RowNumb, (int)MG.gDID] + "".Trim() != "" || grdMatch[RowNumb, (int)MG.gGID] + "".Trim() != "")
                {
                    if (grdMatch[RowNumb, (int)MG.gGID] + "".Trim() != "")
                        qry += " AND ID<>" + grdMatch[RowNumb, (int)MG.gGID].ToString().Trim();
                    RowNumb++;
                }
                qry += "  ORDER BY GuideName";
                return qry;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private void Colour_Driver_Guide(int ROW)
        {
            try
            {
                C1.Win.C1FlexGrid.CellStyle CANCELLED = grdMatch.Styles.Add("CANCELLED");
                CANCELLED.BackColor = Color.OrangeRed;
                C1.Win.C1FlexGrid.CellStyle NON = grdMatch.Styles.Add("NON");
                NON.BackColor = Color.Transparent;
                C1.Win.C1FlexGrid.CellRange rg1, rg2, rg3, rg4 = new C1.Win.C1FlexGrid.CellRange();
                if (!Convert.ToBoolean(grdGudie[ROW, (int)GD.gIPD]))
                {
                    if (Convert.ToBoolean(grdMatch[ROW, (int)MG.gIGC]))
                    {
                        rg1 = grdMatch.GetCellRange(ROW, (int)MG.gGNM);
                        rg2 = grdMatch.GetCellRange(ROW, (int)MG.gIGC);
                        rg1.Style = grdMatch.Styles["CANCELLED"];
                        rg2.Style = grdMatch.Styles["CANCELLED"];
                        grdGudie[ROW, (int)GD.gICN] = 1;
                        grdGudie.Rows[ROW].Style = grdMatch.Styles["CANCELLED"];
                    }
                    else if (!Convert.ToBoolean(grdMatch[ROW, (int)MG.gIGC]))
                    {
                        rg1 = grdMatch.GetCellRange(ROW, (int)MG.gGNM);
                        rg2 = grdMatch.GetCellRange(ROW, (int)MG.gIGC);
                        rg1.Style = grdMatch.Styles["NON"];
                        rg2.Style = grdMatch.Styles["NON"];
                        grdGudie[ROW, (int)GD.gICN] = 0;
                        grdGudie.Rows[ROW].Style = grdMatch.Styles["NON"];
                    }
                }
                if (!Convert.ToBoolean(grdMatch[ROW, (int)MG.gIPD]))
                {
                    if (Convert.ToBoolean(grdMatch[ROW, (int)MG.gIDC]))
                    {
                        rg3 = grdMatch.GetCellRange(ROW, (int)MG.gDNM);
                        rg4 = grdMatch.GetCellRange(ROW, (int)MG.gADT, ROW, (int)MG.gIDC);
                        rg3.Style = grdMatch.Styles["CANCELLED"];
                        rg4.Style = grdMatch.Styles["CANCELLED"];
                        grdDBasic[ROW, (int)DB.gIDC] = 1;
                        grdDBasic.Rows[ROW].Style = grdMatch.Styles["CANCELLED"];
                        grdDriver.Rows[ROW].Style = grdMatch.Styles["CANCELLED"];
                    }
                    else if (!Convert.ToBoolean(grdMatch[ROW, (int)MG.gIDC]))
                    {
                        rg3 = grdMatch.GetCellRange(ROW, (int)MG.gDNM);
                        rg4 = grdMatch.GetCellRange(ROW, (int)MG.gADT, ROW, (int)MG.gIDC);
                        rg3.Style = grdMatch.Styles["NON"];
                        rg4.Style = grdMatch.Styles["NON"];
                        grdDBasic[ROW, (int)DB.gIDC] = 0;
                        grdDBasic.Rows[ROW].Style = grdMatch.Styles["NON"];
                        grdDriver.Rows[ROW].Style = grdMatch.Styles["NON"];
                    }
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void grdGudie_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(grdGudie[grdGudie.Row, (int)GD.gIPD]))
            {
                MessageBox.Show("This Cannot Be Modified.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                grdGudie.Col = 4;
            }
        }
        private void grdTExpense_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(grdTExpense[grdTExpense.Row, (int)TP.gIPD]))
            {
                MessageBox.Show("This Cannot Be Modified.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                grdTExpense.Col = 1;
            }
        }
        private void grdMatch_RowColChange(object sender, EventArgs e)
        {
            try
            {
                if (bLoad == true) return;
                grdMatch.Rows[1].AllowEditing = true;
                if (grdMatch.Rows.Count < 3) return;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void grdMatch_LeaveEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            int ROW = 1;
            int DI = Convert.ToInt32(grdMatch[RowNumb, (int)MG.gDID]);
            string q = "select ISNULL(dbo.fun_DriverLastTravelDates(1," + DI + "),0)TravelDate";
            DataTable dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(q);
            DateTime lastdeparture = Convert.ToDateTime(dt.Rows[0]["TravelDate"]);
            if (lastdeparture > Convert.ToDateTime(grdMatch[grdMatch.Row, (int)MG.gADT]))//--- check with driver's previous tour dates 
            {
                MessageBox.Show("Invalid date because his previous tour hasn't been completed yet", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                grdMatch[grdMatch.Row, (int)MG.gADT] = null;
                return;
            }
            while (grdMatch[ROW, grdDBasic.Cols[(int)MG.gDID].Index] != null)
            {
                grdDBasic[ROW, (int)DB.gADT] = grdMatch[ROW, (int)MG.gADT];
                grdDBasic[ROW, (int)DB.gATM] = grdMatch[ROW, (int)MG.gATM];
                grdDBasic[ROW, (int)DB.gAFL] = grdMatch[ROW, (int)MG.gAFL];
                grdDBasic[ROW, (int)DB.gDDT] = grdMatch[ROW, (int)MG.gDDT];
                grdDBasic[ROW, (int)DB.gDTM] = grdMatch[ROW, (int)MG.gDTM];
                grdDBasic[ROW, (int)DB.gDFL] = grdMatch[ROW, (int)MG.gDFL];
                grdDBasic[ROW, (int)DB.gIDC] = grdMatch[ROW, (int)MG.gIDC];
                ROW++;
            }
        }
        private void dtpArrival_ValueChanged(object sender, EventArgs e) { DateArrival = dtpArrival.Value; }
        private void chkNoTransport_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkNoTransport.Checked)
                {
                    int RowNumb = 1;
                    while (grdMatch[RowNumb, grdMatch.Cols[(int)MG.gDID].Index] != null || grdMatch[RowNumb, grdMatch.Cols[(int)MG.gGID].Index] != null)
                    {
                        if (grdMatch[RowNumb, grdMatch.Cols[(int)MG.gDID].Index] != null) grdMatch[RowNumb, (int)MG.gIDC] = true;
                        if (grdMatch[RowNumb, grdMatch.Cols[(int)MG.gGID].Index] != null) grdMatch[RowNumb, (int)MG.gIGC] = true;
                        Colour_Driver_Guide(RowNumb);
                        RowNumb++;
                    }
                    grdMatch.Enabled = false;
                    grdMatch.BackColor = Color.Pink;
                    grdMatch.BackgroundImage = null;
                }
                else
                {
                    int RowNumb = 1;
                    while (grdMatch[RowNumb, grdMatch.Cols[(int)MG.gDID].Index] != null || grdMatch[RowNumb, grdMatch.Cols[(int)MG.gGID].Index] != null)
                    {
                        Colour_Driver_Guide(RowNumb);
                        RowNumb++;
                    }
                    grdMatch.Enabled = true;
                    grdMatch.BackColor = Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Mode != 0) return;
                if (drpCountry.SelectedValue + "".Trim() == "") return;
                int countryid = 0;
                countryid = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID from mst_Country WHERE ID=" + drpCountry.SelectedValue.Trim() + "").Rows[0]["ID"].ToString());
                Check_For_Saarc_Country();
                Get_Tour_Number();
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                throw (ex);
            }
        }
        private string getCurrent_Driver_Guide_List(bool IsDriver)
        {
            try
            {
                string list = "";
                RowNumb = 1;
                if (IsDriver)
                {
                    while (grdMatch.Rows.Count > RowNumb)
                    {
                        if (grdMatch[RowNumb, (int)MG.gDID] + "".Trim() == "" && grdMatch[RowNumb, (int)MG.gGID] + "".Trim() == "") break;
                        if (grdMatch[RowNumb, (int)MG.gDID] + "".Trim() != "") list += grdMatch[RowNumb, (int)MG.gDID].ToString() + ",".Trim();
                        RowNumb++;
                    }
                }
                else
                {
                    while (grdMatch.Rows.Count > RowNumb)
                    {
                        if (grdMatch[RowNumb, (int)MG.gDID] + "".Trim() == "" && grdMatch[RowNumb, (int)MG.gGID] + "".Trim() == "") break;
                        if (grdMatch[RowNumb, (int)MG.gGID] + "".Trim() != "") list += grdMatch[RowNumb, (int)MG.gGID].ToString() + ",".Trim();
                        RowNumb++;
                    }
                }
                if (list.Length > 0) list = list.Substring(0, list.Length - 1);
                return list;
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
                return "";
            }
        }
        private void txtGuest_Leave(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (Mode != 0)
            {
                DialogResult dr = MessageBox.Show("Do You want to apply the changes to the Hotel Details ?", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    RowNumb = 1;
                    while (grdCI[RowNumb, (int)CI.gGNM] != null)
                    {
                        grdCI[RowNumb, (int)CI.gGNM] = txtGuest.Text.Trim();
                        RowNumb = RowNumb + 1;
                    }
                }
            }
        }
        private void drpCountry_Click_Open(object sender, EventArgs e) { Fill_Control(); }
        private void setVoucherType() { }
        private void txtGuest_TextChanged(object sender, EventArgs e)
        {
            string s = txtGuest.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s, errorProvider1, txtGuest);
        }
        private void grdCI_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)        {            if (isLoad) validate_Hotel_Date(e);        }
        private void validate_Hotel_Date(C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                DateTime dtArrival = dtpArrival.Value,dtDeparture = dtpDeparture.Value,selectedArrival, selectedDeparture;
                int days;
                if (e.Col == (int)CI.gDTI)
                {
                    selectedArrival = Convert.ToDateTime(grdCI[grdCI.Row, (int)CI.gDTI]);
                    grdCI[grdCI.Row, (int)CI.gNOD] = 0;
                    grdCI[grdCI.Row, (int)CI.gDTO] = grdCI[grdCI.Row, (int)CI.gDTI];
                    if (selectedArrival > dtDeparture || selectedArrival < dtArrival)
                    {
                        MessageBox.Show("Selected 'Date In' doesn't match with the\n Tour Arrival And Departure.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdCI[grdCI.Row, (int)CI.gDTI] = null;
                        grdCI[grdCI.Row, (int)CI.gNOD] = null;
                        grdCI[grdCI.Row, (int)CI.gDTO] = null;
                        return;
                    }
                }
                else if (e.Col == (int)CI.gNOD)
                {
                    if (Classes.clsGlobal.IsNumeric(grdCI[grdCI.Row, (int)CI.gNOD].ToString() + "".Trim()))
                    {
                        if (grdCI[grdCI.Row, (int)CI.gDTI] + "".Trim() != "")
                        {
                            days = Convert.ToInt32(grdCI[grdCI.Row, (int)CI.gNOD]);
                            selectedArrival = Convert.ToDateTime(grdCI[grdCI.Row, (int)CI.gDTI]);
                            selectedDeparture = selectedArrival.AddDays(days);
                            if (selectedDeparture > dtDeparture || selectedDeparture < dtArrival)
                            {
                                MessageBox.Show("Adding " + days + " days will exceed the 'Departure Date'.\nAdding Failed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                grdCI[grdCI.Row, (int)CI.gNOD] = 0;
                                grdCI[grdCI.Row, (int)CI.gDTO] = grdCI[grdCI.Row, (int)CI.gDTI];
                                return;
                            }
                            grdCI[grdCI.Row, (int)CI.gDTO] = selectedDeparture;
                        }
                        else grdCI[grdCI.Row, (int)CI.gNOD] = null;
                        DateTime DArr;
                        int NOD;
                        RowNumb = grdCI.Row + 1;
                        bool failed = false, show = false;
                        while (grdCI[RowNumb, (int)CI.gNOD] + "".Trim() != "")
                        {
                            if (RowNumb == grdCI.Row + 1 && MessageBox.Show("Do you wish to change the other dates as well ??.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) break;
                            grdCI[RowNumb, (int)CI.gDTI] = grdCI[RowNumb - 1, (int)CI.gDTO];
                            DArr = Convert.ToDateTime(grdCI[RowNumb - 1, (int)CI.gDTO].ToString());
                            NOD = Convert.ToInt32(grdCI[RowNumb, (int)CI.gNOD].ToString());
                            if (failed || DArr.AddDays(NOD) > dtDeparture || DArr.AddDays(NOD) < dtArrival)
                            {
                                if (!show) MessageBox.Show("Adding Failed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                failed = true;
                                show = true;
                                grdCI[RowNumb, (int)CI.gNOD] = 0;
                                grdCI[RowNumb, (int)CI.gDTI] = grdCI[RowNumb - 1, (int)CI.gDTO];
                                grdCI[RowNumb, (int)CI.gDTO] = grdCI[RowNumb, (int)CI.gDTI];
                            }
                            else grdCI[RowNumb, (int)CI.gDTO] = (NOD != 0) ? DArr.AddDays(NOD) : grdCI[RowNumb, (int)CI.gDTI];
                            RowNumb = RowNumb + 1;
                        }
                    }
                    else
                        MessageBox.Show("Please enter a valid number to\n Number of nights.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (e.Col == (int)CI.gDTO)
                {
                    selectedArrival = Convert.ToDateTime(grdCI[grdCI.Row, (int)CI.gDTI]);
                    selectedDeparture = Convert.ToDateTime(grdCI[grdCI.Row, (int)CI.gDTO]);
                    if (selectedDeparture < selectedArrival)
                    {
                        MessageBox.Show("'Date Out' Cannot be less than the 'Date In'", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdCI[grdCI.Row, (int)CI.gNOD] = 0;
                        grdCI[grdCI.Row, (int)CI.gDTO] = grdCI[grdCI.Row, (int)CI.gDTI];
                        return;
                    }
                    if (selectedDeparture > dtDeparture || selectedDeparture < dtArrival)
                    {
                        MessageBox.Show("Selected 'Date In' doesn't match with the\n Tour Arrival And Departure.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdCI[grdCI.Row, (int)CI.gNOD] = 0;
                        grdCI[grdCI.Row, (int)CI.gDTO] = grdCI[grdCI.Row, (int)CI.gDTI];
                        return;
                    }
                    TimeSpan ts = selectedDeparture - selectedArrival;
                    grdCI[grdCI.Row, (int)CI.gNOD] = ts.TotalDays;
                }
            }
            catch (Exception ex)
            {
                Enable_Disable_Save(false);
                db.MsgERR(ex);
            }
        }
        private void chkCanclTour_CheckedChanged(object sender, EventArgs e)
        {
            IsCancelled = true;
            Cancel_DriverGuide();
            Cancel_CityItinary();
        }
        private void Cancel_DriverGuide()
        {
            int row = 1;
            try
            {
                while (grdMatch[row, grdMatch.Cols[(int)MG.gDID].Index] != null || grdMatch[row, grdMatch.Cols[(int)MG.gGID].Index] != null)
                {
                    grdMatch[row, (int)MG.gIDC] = true;
                    grdMatch[row, (int)MG.gIGC] = true;
                    row++;
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Cancel_CityItinary()
        {
            int row = 1;
            try
            {
                while (grdCI[row, grdCI.Cols[(int)CI.gIDN].Index] != null)
                {
                    grdCI[row, (int)CI.gCAN] = true;
                    grdCI[row, (int)CI.gANO] = 9;
                    row++;
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void chkTrOnly_CheckedChanged(object sender, EventArgs e)
        {
            int row = 1, drivers = 0;
            if (chkTrOnly.Checked == true)
            {
                while (grdMatch[row, grdMatch.Cols[(int)MG.gDID].Index] != null)
                {
                    if (Convert.ToBoolean(grdMatch[row, (int)MG.gIDC]) == false) drivers++;
                    row++;
                }
                if (drivers == 0) MessageBox.Show("At least ONE Driver must be selected before selecting this option.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else Cancel_CityItinary();
            }
        }
        private void chkCompany_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCompany.Checked == true)                           cmbCompany.DataSource  =   Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            else if (chkCompany.Checked == false)
            {
                cmbCompany.DataSource = null;
                cmbCompany.SelectedText = "";
            }
            cmbCompany.Enabled = (chkCompany.Checked == true);
        }
        private void grdOE_CellChanged(object sender, RowColEventArgs e)
        {
            if (e.Col == (int)OE.gCNO && e.Row != 0 && Convert.ToBoolean(grdOE[e.Row, (int)OE.gIBP]) == false) grdOE[e.Row, (int)OE.gCNO] = "";
            if (e.Col == (int)OE.gIBP && e.Row != 0 && Convert.ToBoolean(grdOE[e.Row, (int)OE.gIBP]) == false) grdOE[e.Row, (int)OE.gCNO] = "";
            if (e.Row != 0 && grdOE[e.Row, (int)OE.gEXN] + "".Trim() != "" && grdOE[e.Row, (int)OE.gPDT] + "".Trim() == "") grdOE[e.Row, (int)OE.gPDT] = Classes.clsGlobal.CurDate().ToString();
        }
        private void grdTAdvance_AfterEdit(object sender, RowColEventArgs e)
        {
            if (e.Col == (int)TA.gIDR)    grdTAdvance[grdTAdvance.Row, (int)TA.gDID] =    grdTAdvance[grdTAdvance.Row, (int)TA.gEID] =   grdTAdvance[grdTAdvance.Row, (int)TA.gDNM] =     grdTAdvance[grdTAdvance.Row, (int)TA.gENM] =   grdTAdvance[grdTAdvance.Row, (int)TA.gAMT] = "";
                 }
        private void drpMarketingDep_Selected_TextChanged(object sender, EventArgs e) { Get_Tour_Number(); }
        private void Enable_Disable_Save(bool status) { btnOk.Enabled = status; }

        private void frmGroupAmend_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12) btnOk.Enabled = true;
        }

        private void grdAge_AfterEdit(object sender, RowColEventArgs e)
        {

            try
            {  
                nudChild.Value = int.Parse("0" + grdAge[2, (int)AG.gCNT]);
                nudAdult.Value=int.Parse("0"+    grdAge[3, (int)AG.gCNT] ); 
            }
            catch (Exception ex)
            { 
            }
        }
    }
}
