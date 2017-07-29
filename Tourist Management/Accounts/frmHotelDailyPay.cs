using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
namespace Tourist_Management.Accounts
{
    public partial class frmHotelDailyPay : Form
    {
        private const string msghd = "Hotel Daily Due Payments";
        string SqlQuery;
        byte[] imageData1 = null;  //TO KEEP HOTEL LOGO AS A BINARY DATA
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        bool FirstLoad = false; //INITIALIZER MODE.......DATE NOT CHANGE....SO NO NEED TO FILTER IN 'DATE VALUE CHANGE'
        enum SP { gTID, gHID, gVID, gHPS, gCNM, gHNM, gDTE, gADL, gCHD, gGUD, gCID, gCUR, gGCI, gGCR, gNON, gFCA, gFCC, gCRT, gGCN, gTOT, gADV, gCOM, gDUE, gPID, gCNF, gPPD, gPRI, gOAM, gRMK, gANO };
        enum RP { gTID, gHID, gVID, gCNM, gHNM, gRTI, gRBI, gROI, gRTY ,gBSS, gOCC, gNOR, gGRM, gFRM, gEBD, gEBC, gRTE, gGRT, gANO };
        enum MP { gTID, gHID, gVID, gCNM, gHNM, gMNM, gAMC, gCMC, gGMC, gANO };
        public frmHotelDailyPay(){InitializeComponent();}
        private void frmHotelDailyPay_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Control();
                fill_Bank_Details();
                Clear_Paymethod();
                dtpFromDate.Value = Classes.clsGlobal.CurDate();// Convert.ToDateTime(dataTable.ToString());
                dtpFromDate.Value = dtpFromDate.Value.AddDays(1);
                dtpToDate.Value = dtpFromDate.Value;
                Filter_Values();
                FirstLoad = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        { 
            cmbCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            Load_Company_Logo();
        }
        private void Load_Company_Logo()
        {
            DataTable DTB;
            DTB = new DataTable();
            DTB = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Company_Logo FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 and ID = "+ Convert.ToInt32(cmbCompany.SelectedValue) +" ORDER BY ID");
            byte[] Photo = (byte[])DTB.Rows[0]["Company_Logo"];
            imageData1 = Photo;
            MemoryStream ms = new MemoryStream(Photo);
            pbImage.Image = Image.FromStream(ms, false, false);
            lblCompLogo.Visible = false;
        }
        private void fill_Bank_Details()
        {
            try
            {
                int CompanyID;
                if (cmbCompany.SelectedValue + "".Trim() != "")
                    CompanyID = Convert.ToInt32(cmbCompany.SelectedValue);
                else
                    return;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Clear_Paymethod()
        {
            try
            {
                rdbBank.Checked = false;
                rdbCash.Checked = false;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                #region SUMMARY PAYMENT
                grdDuePayDaily.Cols.Count = 30;
                grdRoomPay.Cols.Count = 19;
                grdMealPay.Cols.Count = 10;
                grdDuePayDaily.Rows.Count = 5000;
                grdRoomPay.Rows.Count = 5000;
                grdMealPay.Rows.Count = 5000;
                grdDuePayDaily.Cols[(int)SP.gTID].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gHID].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gVID].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gHPS].Width = 66;
                grdDuePayDaily.Cols[(int)SP.gCNM].Width = 129;
                grdDuePayDaily.Cols[(int)SP.gHNM].Width = 131;
                grdDuePayDaily.Cols[(int)SP.gDTE].Width = 75;
                grdDuePayDaily.Cols[(int)SP.gADL].Width = 40;
                grdDuePayDaily.Cols[(int)SP.gCHD].Width = 40;
                grdDuePayDaily.Cols[(int)SP.gGUD].Width = 40;
                grdDuePayDaily.Cols[(int)SP.gFCA].Width = 50;
                grdDuePayDaily.Cols[(int)SP.gFCC].Width = 50;
                grdDuePayDaily.Cols[(int)SP.gCID].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gCUR].Width = 43;
                grdDuePayDaily.Cols[(int)SP.gGCI].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gGCR].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gCRT].Width = 74;
                grdDuePayDaily.Cols[(int)SP.gGCN].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gADV].Width = 75;
                grdDuePayDaily.Cols[(int)SP.gCOM].Width = 75;
                grdDuePayDaily.Cols[(int)SP.gTOT].Width = 75;
                grdDuePayDaily.Cols[(int)SP.gPID].Width = 44;
                grdDuePayDaily.Cols[(int)SP.gCNF].Width = 50;
                grdDuePayDaily.Cols[(int)SP.gPPD].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gNON].Width = 58;
                grdDuePayDaily.Cols[(int)SP.gDUE].Width = 74;
                grdDuePayDaily.Cols[(int)SP.gPRI].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gOAM].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gRMK].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gANO].Width = 0;
                grdRoomPay.Cols[(int)RP.gTID].Width = 0;
                grdRoomPay.Cols[(int)RP.gHID].Width = 0;
                grdRoomPay.Cols[(int)RP.gVID].Width = 0;
                grdRoomPay.Cols[(int)RP.gCNM].Width = 133;
                grdRoomPay.Cols[(int)RP.gHNM].Width = 0;
                grdRoomPay.Cols[(int)RP.gRTI].Width = 0;
                grdRoomPay.Cols[(int)RP.gRBI].Width = 0;
                grdRoomPay.Cols[(int)RP.gROI].Width = 0;
                grdRoomPay.Cols[(int)RP.gRTY].Width = 84;
                grdRoomPay.Cols[(int)RP.gBSS].Width = 60;
                grdRoomPay.Cols[(int)RP.gOCC].Width = 60;
                grdRoomPay.Cols[(int)RP.gNOR].Width = 70;
                grdRoomPay.Cols[(int)RP.gGRM].Width = 0;
                grdRoomPay.Cols[(int)RP.gFRM].Width = 70;
                grdRoomPay.Cols[(int)RP.gEBD].Width = 48;
                grdRoomPay.Cols[(int)RP.gEBC].Width = 69;
                grdRoomPay.Cols[(int)RP.gRTE].Width = 75;
                grdRoomPay.Cols[(int)RP.gGRT].Width = 0;
                grdRoomPay.Cols[(int)RP.gANO].Width = 0;
                grdMealPay.Cols[(int)MP.gTID].Width = 0;
                grdMealPay.Cols[(int)MP.gHID].Width = 0;
                grdMealPay.Cols[(int)MP.gVID].Width = 0;
                grdMealPay.Cols[(int)MP.gCNM].Width = 140;
                grdMealPay.Cols[(int)MP.gHNM].Width = 0;
                grdMealPay.Cols[(int)MP.gMNM].Width = 100;
                grdMealPay.Cols[(int)MP.gAMC].Width = 75;
                grdMealPay.Cols[(int)MP.gCMC].Width = 75;
                grdMealPay.Cols[(int)MP.gGMC].Width = 0;
                grdMealPay.Cols[(int)MP.gANO].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gTID].Caption = "Tour ID";
                grdDuePayDaily.Cols[(int)SP.gHID].Caption = "Hotel ID";
                grdDuePayDaily.Cols[(int)SP.gVID].Caption = "Voucher ID";
                grdDuePayDaily.Cols[(int)SP.gHPS].Caption = "Handle By";
                grdDuePayDaily.Cols[(int)SP.gCNM].Caption = "Client";
                grdDuePayDaily.Cols[(int)SP.gHNM].Caption = "Hotel Name";
                grdDuePayDaily.Cols[(int)SP.gDTE].Caption = "Date";
                grdDuePayDaily.Cols[(int)SP.gADL].Caption = "# Ad";
                grdDuePayDaily.Cols[(int)SP.gCHD].Caption = "# Ch";
                grdDuePayDaily.Cols[(int)SP.gGUD].Caption = "# Gd";
                grdDuePayDaily.Cols[(int)SP.gFCA].Caption = "Ad.FOC";
                grdDuePayDaily.Cols[(int)SP.gFCC].Caption = "Ch.FOC";
                grdDuePayDaily.Cols[(int)SP.gCID].Caption = "CUR ID";
                grdDuePayDaily.Cols[(int)SP.gCUR].Caption = "CUR";
                grdDuePayDaily.Cols[(int)SP.gGCI].Caption = "Guide CUR ID";
                grdDuePayDaily.Cols[(int)SP.gGCR].Caption = "G.CUR";
                grdDuePayDaily.Cols[(int)SP.gNON].Caption = "# Nights";
                grdDuePayDaily.Cols[(int)SP.gCRT].Caption = "Conv. Rate";
                grdDuePayDaily.Cols[(int)SP.gGCN].Caption = "G.Conv. Rate";
                grdDuePayDaily.Cols[(int)SP.gADV].Caption = "Advance";
                grdDuePayDaily.Cols[(int)SP.gCOM].Caption = "Commission";
                grdDuePayDaily.Cols[(int)SP.gTOT].Caption = "Total";
                grdDuePayDaily.Cols[(int)SP.gDUE].Caption = "Due";
                grdDuePayDaily.Cols[(int)SP.gPID].Caption = "Paid";
                grdDuePayDaily.Cols[(int)SP.gCNF].Caption = "Confirm";
                grdDuePayDaily.Cols[(int)SP.gPPD].Caption = "Partially";
                grdDuePayDaily.Cols[(int)SP.gPRI].Caption = "Pre Payment";
                grdDuePayDaily.Cols[(int)SP.gOAM].Caption = "Other Amount";
                grdDuePayDaily.Cols[(int)SP.gRMK].Caption = "Reamrks For Other Payments";
                grdDuePayDaily.Cols[(int)SP.gANO].Caption = "AmendNo";
                grdRoomPay.Cols[(int)RP.gTID].Caption = "Tour ID";
                grdRoomPay.Cols[(int)RP.gHID].Caption = "Hotel ID";
                grdRoomPay.Cols[(int)RP.gVID].Caption = "Voucher ID";
                grdRoomPay.Cols[(int)RP.gCNM].Caption = "Client";
                grdRoomPay.Cols[(int)RP.gHNM].Caption = "Hotel Name";
                grdRoomPay.Cols[(int)RP.gRTI].Caption = "Room Type ID";
                grdRoomPay.Cols[(int)RP.gRBI].Caption = "Basis ID";
                grdRoomPay.Cols[(int)RP.gROI].Caption = "Occupancy ID";
                grdRoomPay.Cols[(int)RP.gRTY].Caption = "Type";
                grdRoomPay.Cols[(int)RP.gBSS].Caption = "Basis";
                grdRoomPay.Cols[(int)RP.gOCC].Caption = "Occup";
                grdRoomPay.Cols[(int)RP.gNOR].Caption = "# Rooms";
                grdRoomPay.Cols[(int)RP.gGRM].Caption = "# G.Rooms";
                grdRoomPay.Cols[(int)RP.gFRM].Caption = "FOC Rm";
                grdRoomPay.Cols[(int)RP.gEBD].Caption = "Ebed";
                grdRoomPay.Cols[(int)RP.gEBC].Caption = "EB.Cost";
                grdRoomPay.Cols[(int)RP.gRTE].Caption = "Room Rate";
                grdRoomPay.Cols[(int)RP.gGRT].Caption = "Guide Rate";
                grdRoomPay.Cols[(int)RP.gANO].Caption = "AmendNo";
                grdMealPay.Cols[(int)MP.gTID].Caption = "Tour ID";
                grdMealPay.Cols[(int)MP.gHID].Caption = "Hotel ID";
                grdMealPay.Cols[(int)MP.gVID].Caption = "Voucher ID";
                grdMealPay.Cols[(int)MP.gCNM].Caption = "Client";
                grdMealPay.Cols[(int)MP.gHNM].Caption = "Hotel Name";
                grdMealPay.Cols[(int)MP.gMNM].Caption = "Meal";
                grdMealPay.Cols[(int)MP.gAMC].Caption = "Meal(Ad)";
                grdMealPay.Cols[(int)MP.gCMC].Caption = "Meal(ch)";
                grdMealPay.Cols[(int)MP.gGMC].Caption = "Guide Rate";
                grdMealPay.Cols[(int)MP.gANO].Caption = "AmendNo";
                grdDuePayDaily.Cols[(int)SP.gHPS].ComboList = "...";
                grdDuePayDaily.Cols[(int)SP.gCNM].ComboList = "...";
                grdDuePayDaily.Cols[(int)SP.gCUR].ComboList = "...";
                grdDuePayDaily.Cols[(int)SP.gGCR].ComboList = "...";
                grdDuePayDaily.Cols[(int)SP.gHNM].ComboList = "...";
                grdDuePayDaily.Cols[(int)SP.gPRI].ComboList = "...";
                grdRoomPay.Cols[(int)RP.gRTE].Format = "##.##";
                grdDuePayDaily.Cols[(int)SP.gCRT].Format = "##.##";
                grdDuePayDaily.Cols[(int)SP.gCOM].Format = "##.##";
                grdDuePayDaily.Cols[(int)SP.gTOT].Format = "##.##";
                grdDuePayDaily.Cols[(int)SP.gDUE].Format = "##.##";
                grdRoomPay.Cols[(int)SP.gFCA].Format = "##.##";
                grdRoomPay.Cols[(int)SP.gFCC].Format = "##.##";
                grdRoomPay.Cols[(int)RP.gFRM].Format = "##.##";
                grdDuePayDaily.Cols[(int)SP.gPID].DataType = Type.GetType("System.Boolean");
                grdDuePayDaily.Cols[(int)SP.gCNF].DataType = Type.GetType("System.Boolean");
                grdDuePayDaily.Cols[(int)SP.gPPD].DataType = Type.GetType("System.Boolean");
                grdDuePayDaily.Rows[1].AllowEditing = true;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Filter_Values()
        {
            try
            {
                DataTable DT;
                string ssql;
                #region DAILY PAYMENT DETAILS
                string format = "yyyy-MM-dd";
                DateTime datefrom = dtpFromDate.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpToDate.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                DateTime Today = dtpPaidDate.Value;
                string today = Today.ToString(format).Substring(0, 10);
                chkAllPaid.Checked = false; 
                #region CREATE SQL QUERY
                ssql = "SELECT TransID,DateIn,DateOut,VoucherID,UniqueID,AmendNo,Guest," +
                                 "HandleBy,HotelID,HotelName,RoomTypeID,RoomBasisID,OccupancyID,RoomTypeName,RoomBasisName,Occupancy," +
                                 "IsNull(FOCAdult,0)AS FOCAdult,IsNull(FOCChild,0)AS FOCChild," +
                                 "IsNull(CurID,2)AS CurID,CurCode," +
                                 "GuideCurID,GuideCurCode," +
                                 "IsNull(ModifiedCost,0)AS ModifiedCost,GuideCost," +
                                 "IsNull(ExtraBed,0)AS Ebed,IsNull(EbedCost,0)AS EbedCost,Advance," +
                                 "IsNull(Commission,0)AS Commission,IsNull(ConRate,0)AS ConRate,IsNull(GuideConRate,0)AS GuideConRate," +
                                 "IsNull(RoomCount,0)AS RoomCount,GuideRooms,IsNull(FOCRooms,0)AS FOCRooms,IsNull(Nights,1)AS Nights," +
                                 "IsNull(MealFor,'')MealFor," +
                                 "IsNull(AdultMealCost,0)AS AdultMealCost,IsNull(ChildMealCost,0)AS ChildMealCost,IsNull(GuideMealCost,0)AS GuideMealCost," +
                                 "IsNull(NoOfAdult,0) AS NoOfAdult,IsNull(NoOfChild,0) AS NoOfChild,NoOfGuide,ISNULL(IsPaid,0) IsPaid,PaidDate,PaidBy," +
                                 "IsNull(ConfirmPaid,0)AS ConfirmPaid,PaidConfirmBy,ISNULL(PartiallyPaid,0) PartiallyPaid,OtherAmt,Remarks," +
                                 "InitiatedCost,InitiatedEbedCost" +
                                 " FROM vw_acc_HotelDailyPayments WHERE ConfirmPaid<>1";
                if (chkCmpny.Checked)
                {
                    ssql += " and CompID = " + Convert.ToInt32(cmbCompany.SelectedValue) + "";
                }
                if (chkTodayPay.Checked)
                {
                    ssql += " AND PaidDate='" + today.Trim() + "'";
                }
                else
                {
                    ssql += " AND Dateout>='" + DateFrom.Trim() + "' AND Dateout<='" + DateTo.Trim() + "'";
                }
                ssql += " ORDER BY Guest";
                #endregion
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                int count = 1;
                grdRoomPay.Rows.Count = 1;
                grdDuePayDaily.Rows.Count = 1;
                grdMealPay.Rows.Count = 1;
                    C1.Win.C1FlexGrid.CellStyle HOT = grdDuePayDaily.Styles.Add("HOT");
                    HOT.BackColor = ColorTranslator.FromHtml("#FBFBEF");
                    C1.Win.C1FlexGrid.CellStyle TOT = grdDuePayDaily.Styles.Add("TOT");
                    TOT.BackColor = Color.LightSteelBlue;
                    #region SUMMARY
                    var Bookings = from val in DT.AsEnumerable()
                                   group val by val["UniqueID"] into reserve
                                   orderby reserve.Max(id => id["Guest"])
                                   select new
                                   {
                                       ID = reserve.Key,
                                       TransID = reserve.Max(id => id["TransID"]),
                                       HotelID = reserve.Max(id => id["HotelID"]),
                                       VoucherID = reserve.Max(id => id["VoucherID"]),
                                       HandleBy = reserve.Max(id => id["HandleBy"]),
                                       Guest = reserve.Max(id => id["Guest"]),
                                       HotelName = reserve.Max(id => id["HotelName"]),
                                       Dateout = reserve.Max(id => id["Dateout"]),
                                       NoOfAdult = reserve.Max(id => id["NoOfAdult"]),
                                       NoOfChild = reserve.Max(id => id["NoOfChild"]),
                                       NoOfGuide = reserve.Max(id => id["NoOfGuide"]),
                                       FOCAdult = reserve.Max(id => id["FOCAdult"]),
                                       FOCChild = reserve.Max(id => id["FOCChild"]),
                                       Advance = reserve.Max(id => id["Advance"]),
                                       CurID = reserve.Max(id => id["CurID"]),
                                       CurCode = reserve.Max(id => id["CurCode"]),
                                       GuideCurID = reserve.Max(id => id["GuideCurID"]),
                                       GuideCurCode = reserve.Max(id => id["GuideCurCode"]),
                                       AmendNo = reserve.Max(id => id["AmendNo"]),
                                       Nights = reserve.Max(id => id["Nights"]),
                                       ConRate = reserve.Max(id => id["ConRate"]),
                                       GuideConRate = reserve.Max(id => id["GuideConRate"]),
                                       Commission = reserve.Max(id => id["Commission"]),
                                       PartiallyPaid = reserve.Max(id => id["PartiallyPaid"]),
                                       IsPaid = reserve.Max(id => id["IsPaid"]),
                                       OtherAmt = reserve.Max(id => id["OtherAmt"]),
                                       Remarks = reserve.Max(id => id["Remarks"])
                                   };    
                    foreach (var items in Bookings)
                    {
                        grdDuePayDaily.Rows.Count = Bookings.Count() + 2;
                        grdDuePayDaily[count, (int)SP.gTID] = items.TransID.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gHID] = items.HotelID.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gVID] = items.VoucherID.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gHPS] = items.HandleBy.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gCNM] = items.Guest.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gHNM] = items.HotelName.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gDTE] = items.Dateout.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gADL] = items.NoOfAdult.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gCHD] = items.NoOfChild.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gGUD] = items.NoOfGuide.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gFCA] = items.FOCAdult.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gFCC] = items.FOCChild.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gADV] = items.Advance.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gCID] = items.CurID.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gCUR] = items.CurCode.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gGCI] = items.GuideCurID.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gGCR] = items.GuideCurCode.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gANO] = items.AmendNo.ToString().Trim();
                        if (items.Nights.ToString().Trim() == "0")
                        {
                            grdDuePayDaily[count, (int)SP.gNON] = "1";
                        }
                        else
                        {
                            grdDuePayDaily[count, (int)SP.gNON] = items.Nights.ToString().Trim();
                        }
                        grdDuePayDaily[count, (int)SP.gCRT] = items.ConRate.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gGCN] = items.GuideConRate.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gCOM] = items.Commission.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gPPD] = Convert.ToBoolean(items.PartiallyPaid);
                        grdDuePayDaily[count, (int)SP.gPID] = Convert.ToBoolean(items.IsPaid);
                        grdDuePayDaily[count, (int)SP.gOAM] = items.OtherAmt.ToString().Trim();
                        grdDuePayDaily[count, (int)SP.gRMK] = items.Remarks.ToString().Trim();
                        decimal arr = Classes.clsGlobal.check_For_Prepayments(Convert.ToInt32(items.HotelID));
                            string sql = "SELECT ISNULL(SettledAmount,0)Val FROM act_ChangeHotelPayments_ALL WHERE SettledVoucherID='" + items.VoucherID.ToString().Trim() + "'";
                            DataTable dt = Classes.clsGlobal.objCon.Fill_Table(sql);
                            if (dt.Rows.Count > 0 && dt.Rows[0]["Val"] + "".Trim() != "")
                            {
                                grdDuePayDaily[count, (int)SP.gPRI] = Convert.ToDecimal(dt.Rows[0]["Val"]).ToString();
                                grdDuePayDaily.Rows[count].Style = grdDuePayDaily.Styles["PAD"];
                            }
                            else
                            {
                                grdDuePayDaily.Rows[count].Style = grdDuePayDaily.Styles["HOT"];
                            }
                        count++;
                    }
                    if (grdDuePayDaily.Rows.Count > 1)
                        grdDuePayDaily.Rows[count].Style = grdDuePayDaily.Styles["TOT"];
                    #endregion
                    #region RATES
                    var Rates = from val in DT.AsEnumerable()
                                where val.Field<string>("MealFor").Trim() == "".Trim()
                                group val by new
                                {
                                    UniqueID = val["UniqueID"],
                                    RoomTypeID = val["RoomTypeID"],
                                    RoomBasisID = val["RoomBasisID"],
                                    OccupancyID = val["OccupancyID"]
                                } into reserve
                                orderby reserve.Max(id => id["Guest"])
                                select new
                                {
                                    ID = reserve.Key,
                                    TransID = reserve.Max(id => id["TransID"]),
                                    HotelID = reserve.Max(id => id["HotelID"]),
                                    VoucherID = reserve.Max(id => id["VoucherID"]),
                                    Guest = reserve.Max(id => id["Guest"]),
                                    HotelName = reserve.Max(id => id["HotelName"]),
                                    RoomTypeID = reserve.Max(id => id["RoomTypeID"]),
                                    RoomBasisID = reserve.Max(id => id["RoomBasisID"]),
                                    OccupancyID = reserve.Max(id => id["OccupancyID"]),
                                    RoomTypeName = reserve.Max(id => id["RoomTypeName"]),
                                    RoomBasisName = reserve.Max(id => id["RoomBasisName"]),
                                    Occupancy = reserve.Max(id => id["Occupancy"]),
                                    RoomCount = reserve.Max(id => id["RoomCount"]),
                                    GuideRooms = reserve.Max(id => id["GuideRooms"]),
                                    FOCRooms = reserve.Max(id => id["FOCRooms"]),
                                    EBed = reserve.Max(id => id["EBed"]),
                                    EbedCost = reserve.Max(id => id["EbedCost"]),
                                    ModifiedCost = reserve.Max(id => id["ModifiedCost"]),
                                    GuideCost = reserve.Max(id => id["GuideCost"]),
                                    AmendNo = reserve.Max(id => id["AmendNo"])
                                };
                    count=1;
                    foreach (var items in Rates)
                    {
                        grdRoomPay.Rows.Count = Rates.Count() + 2;
                        grdRoomPay[count, (int)RP.gTID] = items.TransID.ToString().Trim();
                        grdRoomPay[count, (int)RP.gHID] = items.HotelID.ToString().Trim();
                        grdRoomPay[count, (int)RP.gVID] = items.VoucherID.ToString().Trim();
                        grdRoomPay[count, (int)RP.gCNM] = items.Guest.ToString().Trim();
                        grdRoomPay[count, (int)RP.gHNM] = items.HotelName.ToString().Trim();
                        grdRoomPay[count, (int)RP.gRTI] = items.RoomTypeID.ToString().Trim();
                        grdRoomPay[count, (int)RP.gRBI] = items.RoomBasisID.ToString().Trim();
                        grdRoomPay[count, (int)RP.gROI] = items.OccupancyID.ToString().Trim();
                        grdRoomPay[count, (int)RP.gRTY] = items.RoomTypeName.ToString().Trim();
                        grdRoomPay[count, (int)RP.gBSS] = items.RoomBasisName.ToString().Trim();
                        grdRoomPay[count, (int)RP.gOCC] = items.Occupancy.ToString().Trim();
                        grdRoomPay[count, (int)RP.gNOR] = items.RoomCount.ToString().Trim();
                        grdRoomPay[count, (int)RP.gGRM] = items.GuideRooms.ToString().Trim();
                        grdRoomPay[count, (int)RP.gFRM] = items.FOCRooms.ToString().Trim();
                        grdRoomPay[count, (int)RP.gEBD] = items.EBed.ToString().Trim();
                        if (Convert.ToDouble(items.EbedCost) == 0.00)
                        {
                            grdRoomPay[count, (int)RP.gEBC] = items.EbedCost.ToString().Trim();
                        }
                        else
                        {
                            grdRoomPay[count, (int)RP.gEBC] = items.EbedCost.ToString().Trim();
                        }
                        if (Convert.ToDouble(items.ModifiedCost) == 0.00)
                        {
                            grdRoomPay[count, (int)RP.gRTE] = items.ModifiedCost.ToString().Trim();
                        }
                        else
                        {
                            grdRoomPay[count, (int)RP.gRTE] = items.ModifiedCost.ToString().Trim();
                        }
                        grdRoomPay[count, (int)RP.gGRT] = items.GuideCost.ToString().Trim();
                        grdRoomPay[count, (int)RP.gANO] = items.AmendNo.ToString().Trim();
                        grdRoomPay.Rows[count].Style = grdDuePayDaily.Styles["HOT"];
                        count++;                        
                    }
                    if (grdRoomPay.Rows.Count > 1)
                        grdRoomPay.Rows[count].Style = grdDuePayDaily.Styles["TOT"];
                    else
                        grdRoomPay.Rows.Count = 1;
                    #endregion
                    #region MEAL
                    var Meal = from val in DT.AsEnumerable()
                               where val.Field<string>("MealFor").Trim() != "".Trim()
                               group val by new
                               {
                                 UniqueID = val["UniqueID"],
                                 MealFor = val["MealFor"]                                    
                               } into reserve
                               orderby reserve.Max(id => id["Guest"])
                                select new
                                {
                                    ID = reserve.Key,
                                    TransID = reserve.Max(id => id["TransID"]),
                                    HotelID = reserve.Max(id => id["HotelID"]),
                                    VoucherID = reserve.Max(id => id["VoucherID"]),
                                    Guest = reserve.Max(id => id["Guest"]),
                                    HotelName = reserve.Max(id => id["HotelName"]),
                                    MealFor = reserve.Max(id => id["MealFor"]),
                                    AdultMealCost = reserve.Max(id => id["AdultMealCost"]),
                                    ChildMealCost = reserve.Max(id => id["ChildMealCost"]),
                                    GuideMealCost = reserve.Max(id => id["GuideMealCost"]),
                                    AmendNo = reserve.Max(id => id["AmendNo"])
                                };
                    count = 1;
                    foreach (var items in Meal)
                    {
                        grdMealPay.Rows.Count = Meal.Count() + 2;
                        grdMealPay[count, (int)MP.gTID] = items.TransID.ToString().Trim();
                        grdMealPay[count, (int)MP.gHID] = items.HotelID.ToString().Trim();
                        grdMealPay[count, (int)MP.gVID] = items.VoucherID.ToString().Trim();
                        grdMealPay[count, (int)MP.gCNM] = items.Guest.ToString().Trim();
                        grdMealPay[count, (int)MP.gHNM] = items.HotelName.ToString().Trim();
                        grdMealPay[count, (int)MP.gMNM] = items.MealFor.ToString().Trim();
                        grdMealPay[count, (int)MP.gAMC] = items.AdultMealCost.ToString().Trim();
                        grdMealPay[count, (int)MP.gCMC] = items.ChildMealCost.ToString().Trim();
                        grdMealPay[count, (int)MP.gGMC] = items.GuideMealCost.ToString().Trim();
                        grdMealPay[count, (int)MP.gANO] = items.AmendNo.ToString().Trim();
                        grdMealPay.Rows[count].Style = grdDuePayDaily.Styles["HOT"];
                        count++;                        
                    }
                    if (grdMealPay.Rows.Count > 1)
                        grdMealPay.Rows[count].Style = grdDuePayDaily.Styles["TOT"];
                    else
                        grdMealPay.Rows.Count = 1;
                    #endregion
                #region OLD COMMENTED
                        #region COLOUR ROOM RATES GRID
                    #endregion
                        #region COLOUR SUMMARY GRID
                        #endregion
                        #region COLOUR MEAL GRID
                        #endregion
                        #region FILL SUMMARY GRID
                    #endregion
                        #region FILL MEAL DETAILS
                        #endregion
                        #region FILL ROOM RATES
                        #endregion
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace , msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dtpFilterDate_ValueChanged(object sender, EventArgs e)
        {
            grdDuePayDaily.Rows.Count = 1;
            grdRoomPay.Rows.Count = 1;
            grdMealPay.Rows.Count = 1;
            grdDuePayDaily.Rows.Count = 5000;
            grdRoomPay.Rows.Count = 5000;
            grdMealPay.Rows.Count = 5000;
            chkAllPaid.Checked = false;
            if (!FirstLoad)
                return;
            Filter_Values();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                btnGetTot_Click(null, null);
                if (chkPrint.Checked)
                {
                    Print_Details();
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Data() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
        private Boolean Validate_Data()
        {
                if (!rdbBank.Checked && !rdbCash.Checked)
                {
                    MessageBox.Show("Please select a pay method", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
               return true;
        }
        private Boolean Save_Data()
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
                if(grdDuePayDaily.Rows.Count>1)
                    if (Save_Basic_Details(sqlCom) == false) return false;
                if (grdRoomPay.Rows.Count > 1)
                    if (Save_Room_Details(sqlCom) == false) return false;
                if (grdMealPay.Rows.Count > 1)
                    if (Save_Meal_Details(sqlCom) == false) return false;
                return true;
        }
        private Boolean Save_Basic_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Hotel_Daily_Payments";
                RowNumb = 1;
                while (grdDuePayDaily[RowNumb, grdDuePayDaily.Cols[(int)SP.gVID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gTID].ToString());
                    sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 20).Value = grdDuePayDaily[RowNumb, (int)SP.gVID].ToString();
                    if (rdbBank.Checked && Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPID]))                    
                    {
                        sqlCom.Parameters.Add("@BankPay", SqlDbType.Int).Value = 1;
                    } 
                    if (grdDuePayDaily[RowNumb, (int)SP.gFCA] != null && grdDuePayDaily[RowNumb, (int)SP.gFCA].ToString() != "")
                        sqlCom.Parameters.Add("@FOCAdult", SqlDbType.Float).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gFCA].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gFCC] != null && grdDuePayDaily[RowNumb, (int)SP.gFCC].ToString() != "")
                        sqlCom.Parameters.Add("@FOCChild", SqlDbType.Float).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gFCC].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gADV] != null && grdDuePayDaily[RowNumb, (int)SP.gADV].ToString() != "")
                        sqlCom.Parameters.Add("@Advance", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gADV].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gCOM] != null && grdDuePayDaily[RowNumb, (int)SP.gCOM].ToString() != "")
                        sqlCom.Parameters.Add("@Commission", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gCOM].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gCRT] != null && grdDuePayDaily[RowNumb, (int)SP.gCRT].ToString() != "")
                        sqlCom.Parameters.Add("@ConRate", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gCRT].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gGCN] != null && grdDuePayDaily[RowNumb, (int)SP.gGCN].ToString() != "")
                        sqlCom.Parameters.Add("@GuideConRate", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gGCN].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gCID] != null && grdDuePayDaily[RowNumb, (int)SP.gCID].ToString() != "")
                        sqlCom.Parameters.Add("@PaidCurID", SqlDbType.Int).Value = Convert.ToInt32(grdDuePayDaily[RowNumb, (int)SP.gCID].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gGCI] != null && grdDuePayDaily[RowNumb, (int)SP.gGCI].ToString() != "")
                        sqlCom.Parameters.Add("@GuideCurID", SqlDbType.Int).Value = Convert.ToInt32(grdDuePayDaily[RowNumb, (int)SP.gGCI].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gOAM] != null && grdDuePayDaily[RowNumb, (int)SP.gOAM].ToString() != "")
                        sqlCom.Parameters.Add("@OtherAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(grdDuePayDaily[RowNumb, (int)SP.gOAM].ToString());
                    if (grdDuePayDaily[RowNumb, (int)SP.gRMK] != null && grdDuePayDaily[RowNumb, (int)SP.gRMK].ToString() != "")
                        sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar,1000).Value = grdDuePayDaily[RowNumb, (int)SP.gRMK].ToString();
                    if (Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPID]))
                        sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = dtpPaidDate.Value;
                    if (Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPID]))
                    {
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPID]) == true ? "1" : "0";
                        sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                    }
                    if (Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gCNF]))
                    {
                        sqlCom.Parameters.Add("@ConfirmPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gCNF]) == true ? "1" : "0";
                        sqlCom.Parameters.Add("@PaidConfirmBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                        sqlCom.Parameters.Add("@ConfirmDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dtpPaidDate.Value);
                    }
                    sqlCom.Parameters.Add("@PartiallyPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPPD]) == true ? "1" : "0";
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                        return false;
                    }
                    RowNumb++;
                }
                return RtnVal;
        }
        private Boolean Save_Room_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Hotel_Daily_Payments_R";
                RowNumb = 1;
                while (grdRoomPay[RowNumb, grdRoomPay.Cols[(int)RP.gVID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 20).Value = grdRoomPay[RowNumb, (int)RP.gVID].ToString();
                    if (grdRoomPay[RowNumb, (int)RP.gRTI] != null && grdRoomPay[RowNumb, (int)RP.gRTI].ToString() != "")
                        sqlCom.Parameters.Add("@RoomType", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gRTI].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gRBI] != null && grdRoomPay[RowNumb, (int)RP.gRBI].ToString() != "")
                        sqlCom.Parameters.Add("@BasisID", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gRBI].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gROI] != null && grdRoomPay[RowNumb, (int)RP.gROI].ToString() != "")
                        sqlCom.Parameters.Add("@OccID", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gROI].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gNOR] != null && grdRoomPay[RowNumb, (int)RP.gNOR].ToString() != "")
                        sqlCom.Parameters.Add("@NoOfRooms", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gNOR].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gGRM] != null && grdRoomPay[RowNumb, (int)RP.gGRM].ToString() != "")
                        sqlCom.Parameters.Add("@GuideRooms", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gGRM].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gFRM] != null && grdRoomPay[RowNumb, (int)RP.gFRM].ToString() != "")
                        sqlCom.Parameters.Add("@FOCRooms", SqlDbType.Float).Value = Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gFRM].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gRTE] != null && grdRoomPay[RowNumb, (int)RP.gRTE].ToString() != "")
                        sqlCom.Parameters.Add("@ModifiedCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdRoomPay[RowNumb, (int)RP.gRTE].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gGRT] != null && grdRoomPay[RowNumb, (int)RP.gGRT].ToString() != "")
                        sqlCom.Parameters.Add("@GuideCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdRoomPay[RowNumb, (int)RP.gGRT].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gEBD] != null && grdRoomPay[RowNumb, (int)RP.gEBD].ToString() != "")
                        sqlCom.Parameters.Add("@Ebed", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gEBD].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gEBC] != null && grdRoomPay[RowNumb, (int)RP.gEBC].ToString() != "")
                        sqlCom.Parameters.Add("@EbedCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdRoomPay[RowNumb, (int)RP.gEBC].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                        return false;
                    }
                    RowNumb++;
                }
                return RtnVal;
        }
        private Boolean Save_Meal_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Hotel_Daily_Payments_M";
                RowNumb = 1;
                while (grdMealPay[RowNumb, grdMealPay.Cols[(int)MP.gVID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 20).Value = grdMealPay[RowNumb, (int)MP.gVID].ToString();
                    if (grdMealPay[RowNumb, (int)MP.gAMC] != null && grdMealPay[RowNumb, (int)MP.gAMC].ToString() != "")
                        sqlCom.Parameters.Add("@AdultMealCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdMealPay[RowNumb, (int)MP.gAMC].ToString());
                    if (grdMealPay[RowNumb, (int)MP.gCMC] != null && grdMealPay[RowNumb, (int)MP.gCMC].ToString() != "")
                        sqlCom.Parameters.Add("@ChildMealCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdMealPay[RowNumb, (int)MP.gCMC].ToString());
                    if (grdMealPay[RowNumb, (int)MP.gGMC] != null && grdMealPay[RowNumb, (int)MP.gGMC].ToString() != "")
                        sqlCom.Parameters.Add("@GuideMealCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdMealPay[RowNumb, (int)MP.gGMC].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                        return false;
                    }
                    RowNumb++;
                }
                return RtnVal;
        }
        private void grdDuePayDaily_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DT;
                #region PRE-PAYMENTS
                if (e.Col == grdDuePayDaily.Cols[(int)SP.gPRI].Index)
                {
                    int hotelid = 0;
                    if (grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID] != null && grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID].ToString() != "")
                        hotelid = Convert.ToInt32(grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID]);
                    if (hotelid == 0)
                        return;
                    Accounts.frmHotelSettlement frmHS;
                    frmHS = new Accounts.frmHotelSettlement();
                    frmHS.Hotel_ID = hotelid;
                    frmHS.ShowDialog();
                    decimal arr = Classes.clsGlobal.check_For_Prepayments(hotelid);
                        string sql = "SELECT ISNULL(SettledAmount,0)Val FROM act_ChangeHotelPayments_ALL WHERE SettledVoucherID='" + grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gVID].ToString().Trim() + "'";
                        DataTable dt = Classes.clsGlobal.objCon.Fill_Table(sql);
                        if (dt.Rows.Count > 0 && dt.Rows[0]["Val"] + "".Trim() != "")
                            grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gPRI] = Convert.ToDecimal(dt.Rows[0]["Val"]).ToString();
                }
                #endregion
                #region CURRENCY
                if (e.Col == grdDuePayDaily.Cols[(int)SP.gCUR].Index)
                {                    
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Code FROM mst_Currency Where IsNull(IsActive,0)=1");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DT;
                    frm.Width = grdDuePayDaily.Cols[(int)SP.gCUR].Width;
                    frm.Height = grdDuePayDaily.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdDuePayDaily);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gCID] = SelText[0];
                        grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gCUR] = SelText[1];
                    }
                }
                #endregion
                #region GUIDE CURRENCY
                if (e.Col == grdDuePayDaily.Cols[(int)SP.gGCR].Index)
                {
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Code FROM mst_Currency Where IsNull(IsActive,0)=1");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DT;
                    frm.Width = grdDuePayDaily.Cols[(int)SP.gGCR].Width;
                    frm.Height = grdDuePayDaily.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdDuePayDaily);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gGCI] = SelText[0];
                        grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gGCR] = SelText[1];
                    }
                }
                #endregion
                if (e.Col == grdDuePayDaily.Cols[(int)SP.gHNM].Index)
                {
                    int hotelid = 0;
                    if (grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID] != null && grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID].ToString() != "")
                        hotelid = Convert.ToInt32(grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID]);
                    if (hotelid == 0)
                        return;
                    Master.frmHotel frmHT;
                    frmHT = new Master.frmHotel();
                    frmHT.Mode = 1;
                    frmHT.SystemCode = Convert.ToInt32(hotelid);
                    frmHT.ShowDialog();
                }
                if (e.Col == grdDuePayDaily.Cols[(int)SP.gCNM].Index)
                {
                    Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                    decimal tourid=0;
                    string voucherid="";
                    this.Cursor = Cursors.WaitCursor;
                    if ((grdDuePayDaily[grdDuePayDaily.Row, grdDuePayDaily.Cols[(int)SP.gTID].Index] != null))
                    {
                        if (grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gTID].ToString() != "")
                        {
                            tourid = Convert.ToDecimal(grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gTID].ToString());
                        }
                    }
                    if ((grdDuePayDaily[grdDuePayDaily.Row, grdDuePayDaily.Cols[(int)SP.gVID].Index] != null))
                    {
                        if (grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gVID].ToString() != "")
                        {
                            voucherid = grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gVID].ToString();
                        }
                    }
                    string sql = "SELECT TransID,VoucherNo,TourID,AmendNo,AmendTime,IsNull(MealFor,'')as MealFor,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address,TransID," +
                              "Guest,NoOfAdult,NoOfChild,NoOfGuide,Company_Logo,UniqueHotelID,HotelName," +
                              "CheckIn,CheckOut,RoomTypeName,RoomBasisName,Occupancy,NoOfRooms,NoOfGuideRooms,DateIn AS DateArrival,DateOut AS DateDeparture," +
                              "NoOfFOC,NoOfApart," +
                              "AmendmentTo,BillingIns,OtherIns,Notice,Reference," +
                              "Arrangement,DepName,DepContact,HotelSrNo," +
                              "CreatedBy,CreatedDate,ModifiedBy,LastModifiedDate," +
                              "Rname1,Rno1,Rname2,Rno2,Aname1,Ano1,Aname2,Ano2,Tname1,Tno1,Tname2,Tno2," +
                              "CreatedMobileNo,ModifiedMobileNo" +
                              " FROM vw_rpt_trn_Booking" +
                              " where transid=" + tourid + " and voucherno='" + voucherid.Trim() + "' order by hotelsrno";
                    db.showReport(new Tourist_Management.TransacReports.GroupAmend(),sql);
                    this.Cursor = Cursors.Default;
                }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Details();
            Print_Bank_Details();
        }
        private void Print_Details()
        {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                string reptype = "HotelDailyPay";
                decimal tourid = 0; 
                string format = "yyyy-MM-dd";
                DateTime datefrom = dtpFromDate.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpToDate.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                DateTime Today = dtpPaidDate.Value;
                string today = Today.ToString(format).Substring(0, 10).Trim();
                string sql = "";
                ReportDocument ga = new ReportDocument();
                sql = "SELECT TransID,TourID,DateIn,DateOut,VoucherID,Guest," +
                                 "HandleBy,HotelID,HotelName,RoomTypeName,RoomBasisName,Occupancy,IsNull(ModifiedCost,0)AS ModifiedCost,GuideCost," +
                                 "IsNull(ExtraBed,0)AS Ebed,IsNull(EbedCost,0)AS EbedCost," +
                                 "IsNull(Commission,0)AS Commission,Advance,IsNull(ConRate,0)AS ConRate,IsNull(GuideConRate,0)AS GuideConRate," +
                                 "IsNull(RoomCount,0)AS RoomCount,GuideRooms AS GuideRoomCount,IsNull(FOCRooms,0)AS FOCRooms,IsNull(Nights,1)AS Nights,MealFor," +
                                 "IsNull(AdultMealCost,0)AS AdultMealCost,IsNull(ChildMealCost,0)AS ChildMealCost,IsNull(GuideMealCost,0)AS GuideMealCost," +
                                 "IsNull(NoOfAdult,0) AS NoOfAdult,IsNull(NoOfChild,0) AS NoOfChild,IsNull(NoOfGuide,0) AS NoOfGuide," +
                                 "IsNull(FOCAdult,0) AS FOCAdult,IsNull(FOCChild,0) AS FOCChild," +
                                 "PaidDate,PaidBy,PartiallyPaid,CurCode,GuideCurCode,OtherAmt,Remarks, DisplayName, Physical_Address, Telephone, Fax, Web, E_Mail,E_mailTo,UserName,UserGroupID,GroupName, Company_Logo," +
                                 "Aname1,Ano1,MDname,MDno,AADname,AADno" +
                                 " FROM vw_acc_HotelDailyPayments" +
                                 " WHERE PaidDate='" + today.Trim() + "' AND UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "";
                if (chkCmpny.Checked)
                {
                    sql += " and CompID = " + Convert.ToInt32(cmbCompany.SelectedValue) + "";
                }
                sql += " ORDER BY HandleBy,DateIn";
                DataSets.ds_acc_DailyDueHotel DTG = new DataSets.ds_acc_DailyDueHotel();
                ga = new Tourist_Management.Reports.HotelDailyPayments(); 
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    sConnection.Print_Report(tourid.ToString(), sql, DTG, ga, reptype, new SqlParameter("comp", chkCmpny.Checked));
                }
                else
                   MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Print_Bank_Details()
        {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                string reptype = "HotelBankDetails";
                decimal tourid = 0;
                string HotelString="";
                int RowNumb=1;
                while (grdDuePayDaily[RowNumb, grdDuePayDaily.Cols[(int)SP.gVID].Index] != null)
                {
                    if (!Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPID]))
                    {
                        RowNumb++;
                        continue;
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gHID] != null && grdDuePayDaily[RowNumb, (int)SP.gHID].ToString() != "")
                        HotelString+=grdDuePayDaily[RowNumb, (int)SP.gHID].ToString().Trim()+",";
                    RowNumb++;
                }
                if (HotelString.Length == 0)
                    return;
                HotelString = HotelString.Substring(0, (HotelString.Length-1));
                HotelString = HotelString.Replace(",", " OR ID=");
                string sql = "select HotelName,BankName,BranchName,Account,AccountNo, DisplayName, Physical_Address, Telephone, Fax, Web, E_Mail, Company_Logo from vw_HDPBank where ID=" + HotelString + " and CompID = " + Convert.ToInt32(cmbCompany.SelectedValue) ;
                DataSets.ds_rpt_HotelBanks DTG = new DataSets.ds_rpt_HotelBanks();
                Tourist_Management.Reports.HotelBankDetails ga = new Tourist_Management.Reports.HotelBankDetails();
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    sConnection.Print_Report(tourid.ToString(), sql, DTG, ga, reptype);
                }
                else
                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnGetTot_Click(object sender, EventArgs e)
        {
                int RowNumb = 1;
                string qry;
                string CurVoucherNo = "" ;
                pbPaySum.Maximum = grdDuePayDaily.Rows.Count;
                pbPaySum.Minimum = 0;
                double tot, com, adv ;
                while (grdDuePayDaily.Rows.Count > RowNumb)
                { 
                    tot = 0; com = 0; adv = 0; 
                    pbPaySum.Value = RowNumb + 1;
                    if (grdDuePayDaily[RowNumb, (int)SP.gVID] + "".Trim() == "")
                        break;
                    CurVoucherNo = grdDuePayDaily[RowNumb, (int)SP.gVID].ToString();                                       
                    qry = "SELECT dbo.fun_CalculateHotelAmount('" + CurVoucherNo.Trim() + "')Amt";
                    tot = Convert.ToDouble(Classes.clsGlobal.objCon.Fill_Table(qry).Rows[0]["Amt"]);
                    com = Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gCOM]);
                    adv = Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gADV]);
                    grdDuePayDaily[RowNumb, (int)SP.gTOT] = (tot+com).ToString().Trim();
                    grdDuePayDaily[RowNumb, (int)SP.gDUE] = (tot-adv).ToString().Trim();
                    #region COMMENT
                    #region COMMENT
                    #endregion
                    #endregion
                    RowNumb++;
                }
                #region SET ALL TOTALS
                #region SET SUMMARY PAYMENTS
                double Adult = 0, Child = 0, Gudie = 0, AdFOC = 0, ChFOC = 0, Nights = 0, TotAdvance = 0, TotCommission = 0, OthAmt = 0, Total = 0, Due=0;
                RowNumb = 1;
                while (grdDuePayDaily.Rows.Count - 1 > RowNumb)
                {
                    if (grdDuePayDaily[RowNumb, (int)SP.gADL] != null && grdDuePayDaily[RowNumb, (int)SP.gADL] + "".Trim() != "")
                    {
                        Adult += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gADL]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gCHD] != null && grdDuePayDaily[RowNumb, (int)SP.gCHD] + "".Trim() != "")
                    {
                        Child += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gCHD]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gGUD] != null && grdDuePayDaily[RowNumb, (int)SP.gGUD] + "".Trim() != "")
                    {
                        Gudie += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gGUD]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gFCA] != null && grdDuePayDaily[RowNumb, (int)SP.gFCA] + "".Trim() != "")
                    {
                        AdFOC += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gFCA]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gFCC] != null && grdDuePayDaily[RowNumb, (int)SP.gFCC] + "".Trim() != "")
                    {
                        ChFOC += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gFCC]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gNON] != null && grdDuePayDaily[RowNumb, (int)SP.gNON] + "".Trim() != "")
                    {
                        Nights += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gNON]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gADV] != null && grdDuePayDaily[RowNumb, (int)SP.gADV] + "".Trim() != "")
                    {
                        TotAdvance += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gADV]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gCOM] != null && grdDuePayDaily[RowNumb, (int)SP.gCOM] + "".Trim() != "")
                    {
                        TotCommission += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gCOM]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gOAM] != null && grdDuePayDaily[RowNumb, (int)SP.gOAM] + "".Trim() != "")
                    {
                        OthAmt += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gOAM]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gTOT] != null && grdDuePayDaily[RowNumb, (int)SP.gTOT] + "".Trim() != "")
                    {
                        Total += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gTOT]);
                    }
                    if (grdDuePayDaily[RowNumb, (int)SP.gDUE] != null && grdDuePayDaily[RowNumb, (int)SP.gDUE] + "".Trim() != "")
                    {
                        Due += Convert.ToDouble(grdDuePayDaily[RowNumb, (int)SP.gDUE]);
                    }
                    RowNumb++;
                }
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gADL] = Adult;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gCHD] = Child;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gGUD] = Gudie;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gFCA] = AdFOC;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gFCC] = ChFOC;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gNON] = Nights;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gADV] = TotAdvance;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gCOM] = TotCommission;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gTOT] = Total;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gOAM] = OthAmt;
                grdDuePayDaily[grdDuePayDaily.Rows.Count - 1, (int)SP.gDUE] = Due;
                #endregion
                #region SET ROOM PAYMENTS
                RowNumb = 1;
                double Rooms = 0, GRooms = 0, FOC = 0, EB = 0, EBC = 0, RRate = 0, GRate = 0;
                while (grdRoomPay.Rows.Count - 1 > RowNumb)
                {
                    if (grdRoomPay[RowNumb, (int)RP.gNOR] != null && grdRoomPay[RowNumb, (int)RP.gNOR] + "".Trim() != "")
                    {
                        Rooms += Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gNOR]);
                    }
                    if (grdRoomPay[RowNumb, (int)RP.gGRM] != null && grdRoomPay[RowNumb, (int)RP.gGRM] + "".Trim() != "")
                    {
                        GRooms += Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gGRM]);
                    }
                    if (grdRoomPay[RowNumb, (int)RP.gFRM] != null && grdRoomPay[RowNumb, (int)RP.gFRM] + "".Trim() != "")
                    {
                        FOC += Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gFRM]);
                    }
                    if (grdRoomPay[RowNumb, (int)RP.gEBD] != null && grdRoomPay[RowNumb, (int)RP.gEBD] + "".Trim() != "")
                    {
                        EB += Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gEBD]);
                    }
                    if (grdRoomPay[RowNumb, (int)RP.gEBC] != null && grdRoomPay[RowNumb, (int)RP.gEBC] + "".Trim() != "")
                    {
                        EBC += Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gEBC]);
                    }
                    if (grdRoomPay[RowNumb, (int)RP.gRTE] != null && grdRoomPay[RowNumb, (int)RP.gRTE] + "".Trim() != "")
                    {
                        RRate += Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gRTE]);
                    }
                    if (grdRoomPay[RowNumb, (int)RP.gGRT] != null && grdRoomPay[RowNumb, (int)RP.gGRT] + "".Trim() != "")
                    {
                        GRate += Convert.ToDouble(grdRoomPay[RowNumb, (int)RP.gGRT]);
                    }
                    RowNumb++;
                }
                grdRoomPay[grdRoomPay.Rows.Count - 1, (int)RP.gNOR] = Rooms;
                grdRoomPay[grdRoomPay.Rows.Count - 1, (int)RP.gGRM] = GRooms;
                grdRoomPay[grdRoomPay.Rows.Count - 1, (int)RP.gFRM] = FOC;
                grdRoomPay[grdRoomPay.Rows.Count - 1, (int)RP.gEBD] = EB;
                grdRoomPay[grdRoomPay.Rows.Count - 1, (int)RP.gEBC] = EBC;
                grdRoomPay[grdRoomPay.Rows.Count - 1, (int)RP.gRTE] = RRate;
                grdRoomPay[grdRoomPay.Rows.Count - 1, (int)RP.gGRT] = GRate;
                #endregion
                #region SET MEAL PAYMENTS
                RowNumb = 1;
                double AdMeal = 0, ChMeal = 0, GudMeal = 0;
                while (grdMealPay.Rows.Count - 1 > RowNumb)
                {
                    if (grdMealPay[RowNumb, (int)MP.gAMC] != null && grdMealPay[RowNumb, (int)MP.gAMC] + "".Trim() != "")
                    {
                        AdMeal += Convert.ToDouble(grdMealPay[RowNumb, (int)MP.gAMC]);
                    }
                    if (grdMealPay[RowNumb, (int)MP.gCMC] != null && grdMealPay[RowNumb, (int)MP.gCMC] + "".Trim() != "")
                    {
                        ChMeal += Convert.ToDouble(grdMealPay[RowNumb, (int)MP.gCMC]);
                    }
                    if (grdMealPay[RowNumb, (int)MP.gGMC] != null && grdMealPay[RowNumb, (int)MP.gGMC] + "".Trim() != "")
                    {
                        GudMeal += Convert.ToDouble(grdMealPay[RowNumb, (int)MP.gGMC]);
                    }
                    RowNumb++;
                }
                grdMealPay[grdMealPay.Rows.Count - 1, (int)MP.gAMC] = AdMeal;
                grdMealPay[grdMealPay.Rows.Count - 1, (int)MP.gCMC] = ChMeal;
                grdMealPay[grdMealPay.Rows.Count - 1, (int)MP.gGMC] = GudMeal;
                #endregion
                #endregion
        }
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
                if (!FirstLoad)
                    return;
                if (dtpFromDate.Value > dtpToDate.Value)
                {
                    MessageBox.Show("'FROM DATE'Cannot Be Greater Then 'TO DATE'", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dtpToDate.Value = dtpFromDate.Value;
                }
                else
                    dtpFilterDate_ValueChanged(null, null);
        }
        private void chkTodayPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodayPay.Checked)
                gbDateRange.Enabled = false;
            else
                gbDateRange.Enabled = true;
            grdDuePayDaily.Rows.Count = 1;
            grdDuePayDaily.Rows.Count = 5000;
            grdRoomPay.Rows.Count = 1;
            grdRoomPay.Rows.Count = 5000;
            grdMealPay.Rows.Count = 1;
            grdMealPay.Rows.Count = 5000;
            Filter_Values();
        }
        private void chkAllPaid_CheckedChanged(object sender, EventArgs e)
        {
                bool IsChecked=false;
                if (chkAllPaid.Checked)
                    IsChecked = true;
                int RowNumb = 1;
                while (grdDuePayDaily.Rows.Count > RowNumb)
                {
                    if (grdDuePayDaily[RowNumb, (int)SP.gVID] == null || grdDuePayDaily[RowNumb, (int)SP.gVID].ToString() == "")
                    {
                        RowNumb++;
                        continue;
                    }
                    grdDuePayDaily[RowNumb, (int)SP.gPID] = IsChecked;
                    RowNumb++;
                } 
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
                bool IsChecked = false;
                if (checkBox1.Checked)
                    IsChecked = true;
                int RowNumb = 1;
                while (grdDuePayDaily.Rows.Count > RowNumb)
                {
                    if (grdDuePayDaily[RowNumb, (int)SP.gVID] == null || grdDuePayDaily[RowNumb, (int)SP.gVID].ToString() == "")
                    {
                        RowNumb++;
                        continue;
                    }
                    if(Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPID]))
                        grdDuePayDaily[RowNumb, (int)SP.gPPD] = IsChecked;
                    RowNumb++;
                }
        }
        private void btnDefRate_Click(object sender, EventArgs e)
        {
                int RowNumb = 1;
                string defConvRate = txtConRate.Text;
                while(grdDuePayDaily.Rows.Count > RowNumb)
                {
                    if (grdDuePayDaily[RowNumb, (int)SP.gVID] == null || grdDuePayDaily[RowNumb, (int)SP.gVID].ToString() == "")
                    {
                        RowNumb++;
                        continue;
                    }
                    if (Convert.ToBoolean(grdDuePayDaily[RowNumb, (int)SP.gPID]))
                        grdDuePayDaily[RowNumb, (int)SP.gCRT] = defConvRate;
                    RowNumb++;
                }
        }
        private void Colour_Complementary()
        {
                int RowNumb = 1;
                string AmendNo="";
                C1.Win.C1FlexGrid.CellStyle COM = grdDuePayDaily.Styles.Add("COM");
                COM.BackColor = Color.OrangeRed;
                #region COLOUR SUMMARY GRID
                while (grdDuePayDaily[RowNumb, grdDuePayDaily.Cols[(int)SP.gVID].Index] != null)
                {
                    AmendNo = grdDuePayDaily[RowNumb, (int)SP.gANO].ToString().Trim();
                    if (AmendNo == "99" || AmendNo == "90")
                    {
                        grdDuePayDaily.Rows[RowNumb].Style = grdDuePayDaily.Styles["COM"];
                    }
                    RowNumb++;
                }
                #endregion
                #region MEAL
                RowNumb = 1;
                while (grdMealPay[RowNumb, grdMealPay.Cols[(int)MP.gVID].Index] != null)
                {
                    AmendNo = grdMealPay[RowNumb, (int)MP.gANO].ToString().Trim();
                    if (AmendNo == "99" || AmendNo == "90")
                    {
                        grdMealPay.Rows[RowNumb].Style = grdDuePayDaily.Styles["COM"];
                    }
                    RowNumb++;
                }
                #endregion
                #region ROOM DETAILS
                RowNumb = 1;
                while (grdRoomPay[RowNumb, grdRoomPay.Cols[(int)RP.gVID].Index] != null)
                {
                    AmendNo = grdRoomPay[RowNumb, (int)RP.gANO].ToString().Trim();
                    if (AmendNo == "99" || AmendNo == "90")
                    {
                        grdRoomPay.Rows[RowNumb].Style = grdDuePayDaily.Styles["COM"];
                    }
                    RowNumb++;
                }
                #endregion
        }
        private void chkGuide_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGuide.Checked)
            {
                grdDuePayDaily.Cols[(int)SP.gGCR].Width = 45;
                grdRoomPay.Cols[(int)RP.gGRT].Width = 75;
                grdMealPay.Cols[(int)MP.gGMC].Width = 70;
                grdDuePayDaily.Cols[(int)SP.gGCN].Width = 85;
                grdRoomPay.Cols[(int)RP.gGRM].Width = 66;
            }
            else
            {
                grdDuePayDaily.Cols[(int)SP.gGCR].Width = 0;
                grdRoomPay.Cols[(int)RP.gGRT].Width = 0;
                grdMealPay.Cols[(int)MP.gGMC].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gGCN].Width = 0;
                grdRoomPay.Cols[(int)RP.gGRM].Width = 0;
            }
        }
        private void chkOther_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOther.Checked)
            {
                grdDuePayDaily.Cols[(int)SP.gOAM].Width = 100;
                grdDuePayDaily.Cols[(int)SP.gRMK].Width = 600;
            }
            else
            {
                grdDuePayDaily.Cols[(int)SP.gOAM].Width = 0;
                grdDuePayDaily.Cols[(int)SP.gRMK].Width = 0;
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Update_Hotel_Rates();
        }
        private void Update_Hotel_Rates()
        {
            System.Data.SqlClient.SqlCommand sqlCom=new System.Data.SqlClient.SqlCommand();
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spUpdate_Hotel_Rates";
                RowNumb = 1;
                while (grdRoomPay[RowNumb, grdRoomPay.Cols[(int)RP.gVID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    if (grdRoomPay[RowNumb, (int)RP.gRTE].ToString().Trim() == "0.00")
                    {
                        RowNumb++;
                        continue;
                    }
                    if (grdRoomPay[RowNumb, (int)RP.gHID] != null && grdRoomPay[RowNumb, (int)RP.gHID].ToString() != "")
                        sqlCom.Parameters.Add("@HotelID", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gHID].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gRTI] != null && grdRoomPay[RowNumb, (int)RP.gRTI].ToString() != "")
                        sqlCom.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gRTI].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gRBI] != null && grdRoomPay[RowNumb, (int)RP.gRBI].ToString() != "")
                        sqlCom.Parameters.Add("@BasisID", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gRBI].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gROI] != null && grdRoomPay[RowNumb, (int)RP.gROI].ToString() != "")
                        sqlCom.Parameters.Add("@OccID", SqlDbType.Int).Value = Convert.ToInt32(grdRoomPay[RowNumb, (int)RP.gROI].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gRTE] != null && grdRoomPay[RowNumb, (int)RP.gRTE].ToString() != "")
                        sqlCom.Parameters.Add("@ModifiedCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdRoomPay[RowNumb, (int)RP.gRTE].ToString());
                    if (grdRoomPay[RowNumb, (int)RP.gEBC] != null && grdRoomPay[RowNumb, (int)RP.gEBC].ToString() != "")
                        sqlCom.Parameters.Add("@EbedCost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdRoomPay[RowNumb, (int)RP.gEBC].ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom);
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                    }
                    RowNumb++;
                }
                if (!RtnVal)
                {
                    MessageBox.Show("Records Update Failed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Records Has Been Updated Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }
        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdDuePayDaily.Rows.Count = 1;  // clear existing data in the grid
            grdRoomPay.Rows.Count = 1;
            grdMealPay.Rows.Count = 1;
            grdDuePayDaily.Rows.Count = 5000;
            grdRoomPay.Rows.Count = 5000;
            grdMealPay.Rows.Count = 5000;
            chkAllPaid.Checked = false;
            if (!FirstLoad)    return;
            Load_Company_Logo();
            Filter_Values();
            fill_Bank_Details();
            Clear_Paymethod();
        }
        private void label36_Click(object sender, EventArgs e)
        {
        }
        private void btnEmail_Click(object sender, EventArgs e)
        {
            Send_Email();
        }
        private void Send_Email()
        {
            try
            {
                SqlQuery = "Select Email_To From mst_TransReportSettings WHERE UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                string email_to = "", email_cc = "";
                if (DT.Rows.Count > 0)
                {
                    if (DBNull.Value != DT.Rows[0]["Email_To"] && DT.Rows[0]["Email_To"].ToString() != "")
                    {
                        email_to = DT.Rows[0]["Email_To"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Please Save Email To at Voucher Settings !",msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please Save Email To  at Voucher Settings !");
                    return;
                }
                email_cc = Get_CC_Emails();
                if (!System.IO.Directory.Exists("C:\\Temp\\HotelPayments"))
                {
                    MessageBox.Show("Click the preview button before send the mail", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!System.IO.File.Exists("C:\\Temp\\HotelPayments\\HotelDailyPayments.pdf"))
                { }
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.Display(false);
                string Signature = "";
                Signature = ReadSignature();
                oMsg.HTMLBody = "" + Signature;
                oMsg.CC = email_cc;
                String sDisplayName = "MyAttachment";
                int iPosition =1;
                if (oMsg.Body != null)
                    iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                Outlook.Attachment oAttach;
                ReportDocument oReport = new ReportDocument();
                string path = "C:\\Temp\\HotelPayments\\HotelDailyPayments.pdf";
                string lFileName = path;
                oAttach = oMsg.Attachments.Add(@path, iAttachType, iPosition, sDisplayName);
                oMsg.Subject = "Hotel Daily Payment";
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(email_to);
                oRecip.Resolve();
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private string ReadSignature()
        {
                string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
                string signature = string.Empty;
                DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
                if (diInfo.Exists)
                {
                    FileInfo[] fisignature = diInfo.GetFiles("*.htm");
                    if (fisignature.Length > 0)
                    {
                        StreamReader sr = new StreamReader(fisignature[0].FullName, Encoding.Default);
                        signature = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(signature))
                        {
                            string filename = fisignature[0].Name.Replace(fisignature[0].Extension, string.Empty);
                            signature = signature.Replace(filename + "_files/", appDataDir + "/" + filename + "_files/");
                        }
                    }
                }
                return signature;
        }
        private string Get_CC_Emails()
        {
                string CC = "", ssql="";
                int RowNumb = 0;
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
                return CC;
        }
        private void grdDuePayDaily_AfterSelChange(object sender, C1.Win.C1FlexGrid.RangeEventArgs e)
        {
            try
            {
                if (!FirstLoad || grdDuePayDaily.Rows.Count==1)
                    return;
                if (grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gVID] + "".Trim() == "")
                    return;
                string voucherID = grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gVID] + "".ToString().Trim();
                int HotelID = Convert.ToInt32(grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID]);
                Classes.clsGlobal.selected_Row_Colour_Change(grdDuePayDaily, voucherID, (int)SP.gVID);
                Classes.clsGlobal.selected_Row_Colour_Change(grdMealPay, voucherID, (int)MP.gVID);
                Classes.clsGlobal.selected_Row_Colour_Change(grdRoomPay, voucherID, (int)RP.gVID);
                if (grdDuePayDaily[grdDuePayDaily.Row, (int)RP.gHID] + "".Trim() != "")
                {
                    int hotelID = Convert.ToInt32(grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID]);
                    string prepd = Classes.clsGlobal.Prepayments_Status(voucherID.ToString().Trim(), hotelID);
                    lblPrePaid.Text = prepd.Trim();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdRoomPay_AfterSelChange(object sender, C1.Win.C1FlexGrid.RangeEventArgs e)
        {
            try
            {
                if (!FirstLoad || grdRoomPay.Rows.Count == 1)
                    return;
                if (grdRoomPay[1, (int)RP.gVID] + "".Trim() == "")
                    return;
                if (grdRoomPay[grdRoomPay.Row, (int)RP.gVID] + "".Trim() == "")
                    return;
                string voucherID = grdRoomPay[grdRoomPay.Row, (int)RP.gVID] + "".ToString().Trim();
                int HotelID = Convert.ToInt32(grdDuePayDaily[grdDuePayDaily.Row, (int)SP.gHID]);
                Classes.clsGlobal.selected_Row_Colour_Change(grdDuePayDaily, voucherID, (int)SP.gVID);
                Classes.clsGlobal.selected_Row_Colour_Change(grdRoomPay, voucherID, (int)RP.gVID);
                Classes.clsGlobal.selected_Row_Colour_Change(grdMealPay, voucherID, (int)MP.gVID);
                if (grdRoomPay[grdRoomPay.Row, (int)RP.gHID] + "".Trim() != "")
                {
                    int hotelID = Convert.ToInt32(grdRoomPay[grdRoomPay.Row, (int)RP.gHID]);
                    string prepd = Classes.clsGlobal.Prepayments_Status(voucherID.ToString().Trim(), hotelID);
                    lblPrePaid.Text = prepd.Trim();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdMealPay_AfterSelChange(object sender, C1.Win.C1FlexGrid.RangeEventArgs e)
        {
            try
            {
                if (!FirstLoad || grdMealPay.Rows.Count == 1)
                    return;
                if (grdMealPay[1, (int)SP.gVID] + "".Trim() == "")
                    return;
                if (grdMealPay[grdMealPay.Row, (int)SP.gVID] + "".Trim() == "")
                    return;
                string voucherID = grdMealPay[grdMealPay.Row, (int)SP.gVID] + "".ToString().Trim();
                Classes.clsGlobal.selected_Row_Colour_Change(grdMealPay, voucherID, (int)MP.gVID);
                Classes.clsGlobal.selected_Row_Colour_Change(grdRoomPay, voucherID, (int)RP.gVID);
                Classes.clsGlobal.selected_Row_Colour_Change(grdDuePayDaily, voucherID, (int)SP.gVID);
                if (grdMealPay[grdMealPay.Row, (int)MP.gHID] + "".Trim() != "")
                {
                    int hotelID = Convert.ToInt32(grdMealPay[grdMealPay.Row, (int)MP.gHID]);
                    string prepd = Classes.clsGlobal.Prepayments_Status(voucherID.ToString().Trim(), hotelID);
                    lblPrePaid.Text = prepd.Trim();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkEbed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEbed.Checked)
            {
                grdRoomPay.Cols[(int)RP.gEBC].Width = 60;
                grdRoomPay.Cols[(int)RP.gEBD].Width = 40;
            }
            else
            {
                grdRoomPay.Cols[(int)RP.gEBC].Width = 0;
                grdRoomPay.Cols[(int)RP.gEBD].Width = 0;
            }
        }
        private void chkCmpny_CheckedChanged(object sender, EventArgs e)
        {
            Clear_Paymethod();
            if (chkCmpny.Checked)
            {
                cmbCompany.Enabled = true;
                Filter_Values();
                rdbBank.Visible = true;                
            }
            else
            {
                cmbCompany.Enabled = false;
                Filter_Values();
                rdbBank.Visible = false;
                Clear_Paymethod();
            }
        }
        private void chkPrePay_CheckedChanged(object sender, EventArgs e)
        {
            if(chkPrePay.Checked)
                grdDuePayDaily.Cols[(int)SP.gPRI].Width = 100;
            else
                grdDuePayDaily.Cols[(int)SP.gPRI].Width = 0;
        }
        private void rdbBank_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBank.Checked)
            {
                fill_Bank_Details();
            }
        }
    }
}
