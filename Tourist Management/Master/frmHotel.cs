using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Master
{
    public partial class frmHotel : Form
    {
        private const string msghd = "Hotel Details"; 
        public string SqlQry = "SELECT ID,Code,Name,City,Star,Isnull(IsActive,0)AS IsActive From vw_HotelDetails Where Isnull([Status],0)<>7 Order By Code";
        public int Mode = 0, SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE 
        int RowNumb, BankID = 0;
        byte[] imageData = null, imageData1 = null;  //TO KEEP HOTEL LOGO AS A BINARY DATA
        Boolean bLoad = false;
        enum AV { gMKI, gMKN, gHSI, gHSN, gSFD, gSTD, gRTI, gRTN, gRBI, gRBN, gCID, gCON, gMID, gMAX, gNOR, gEBD, gVAT, gTAX, gSCH, gTPR, gGPR, gPRI };
        enum GD { gRBI, gRBN, gCID, gCNM, gTPR, gPRI };
        enum CS { gSNM, gSDF, gSDT, gRBI, gRBN, gMID, gMNM, gTPR, gPRI };
        enum MS { gMCI, gMCN, gRBI, gRBN, gMID, gMNM, gTPR, gPRI };
        enum BD { gCOD, gNME, gBRC, gBRN, gACN, gANO, gALM };
        enum PH { gNUM, gDES, gBRW, gIMG };
        enum CD { gTID, gTNM, gFID, gFRM, gAMT, gPID, gPER };
        enum CP { gRBI, gRBN, gAFI, gAFN, gATI, gATN, gPID, gPER };
        enum DY { gDFR, gDTO, gPID, gPER };
        enum DT { gDFR, gDTO, gPID, gNOD, gPER };
        enum FT { gRTI, gRTN, gFTS };
        public void Set_Selected_Tab(string tpName) { this.tcHotelDetails.SelectedTab = tcHotelDetails.TabPages[tpName]; }
        public frmHotel() { InitializeComponent(); }
        private void frmHotel_Load(object sender, EventArgs e)
        {
            bLoad = true; 
            Intializer();
            tcHotelDetails_Click(null, null);
            bLoad = false;
        }
        private void Intializer()
        {
            try
            {
                Fill_Control();
                Grd_Initializer();
                if (Mode != 0) Fill_Data();
                else
                {
                    Generate_Hotel_Code();
                    cmbDefRoom.SelectedValue = 1005;
                    cmbDefBasis.SelectedValue = 1001;
                    drpCurrency.setSelectedValue("2");
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Generate_Hotel_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_HotelDetails";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            txtCode.Text = "HTL" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
        }
        private void Grd_Initializer()
        {
            try
            {
                db.GridInit(grdAval, 500,true, AV.gMKI, 0, "Market ID", AV.gMKN, 115, "Market Name", true, AV.gHSI, 0, "Season ID", AV.gHSN, 129, "Season", true, AV.gSFD, 90, "Date From", Type.GetType("System.DateTime"), AV.gSTD, 90, "Date To", Type.GetType("System.DateTime"), AV.gRTI, 0, "Room Type ID", AV.gRTN, 116, "Room Type", true, AV.gRBI, 0, "Basis Type ID", AV.gRBN, 116, "Basis", true, AV.gCID, 0, "Condition ID", AV.gCON, 0, "Condition", true, AV.gMID, 0, "Occupancy ID", AV.gMAX, 90, "Occupancy", true, AV.gEBD, 0, "Extra Bed", Type.GetType("System.Boolean"), AV.gVAT, 0, "VAT %", "##.##", AV.gTAX, 60, "Tax %", "##.##", AV.gSCH, 0, "Service Charge %", "##.##", AV.gTPR, 0, "Price With Tax", "##.##", AV.gGPR, 100, "Guide Room Cost", "##.##", AV.gPRI, 100, "Guest Room Cost", "##.##", AV.gNOR, 0, "No Of Rooms");
                db.GridInit(grdGuide, 100, true, GD.gRBI, 0, "Basis Type ID", GD.gRBN, 272, "Basis Type Name", true, GD.gCID, 0, "Currency ID", GD.gCNM, 128, "Currency", true, GD.gTPR, 100, "Price With Tax", "##.##", GD.gPRI, 100, "Price Without Tax", "##.##");
                db.GridInit(grdComSup, 100, true, CS.gSNM, 130, "Name", CS.gSDF, 110, "Date From", Type.GetType("System.DateTime"), CS.gSDT, 110, "Date To", Type.GetType("System.DateTime"), CS.gMID, 0, "Meal Mode ID", CS.gMNM, 90, "Meal Mode", true, CS.gRBI, 0, "Basis ID", CS.gRBN, 110, "Basis", true, CS.gTPR, 99, "Price With Tax", "##.##", CS.gPRI, 100, "Price Without Tax", "##.##");
                db.GridInit(grdMealSup, 100, true, MS.gMCI, 0, "Category ID", MS.gMCN, 145, "Category Name", true, MS.gMID, 0, "Meal Mode ID", MS.gMNM, 114, "Meal Mode", true, MS.gRBI, 0, "Basis ID", MS.gRBN, 120, "Basis", true, MS.gTPR, 115, "Price With Tax", "##.##", MS.gPRI, 115, "Price Without Tax", "##.##");
                db.GridInit(grdCheck, 100, true, CD.gTID, 0, "Type ID", CD.gTNM, 106, "Type", true, CD.gFID, 0, "From ID", CD.gFRM, 50, "From", true, CD.gAMT, 60, "Amount", CD.gPID, 0, "Percentage ID", CD.gPER, 62, "Percentage", true);
                db.GridInit(grdChild, 100, true, CP.gRBI, 0, "Room Basis Type ID", CP.gRBN, 100, "Basis Type", true, CP.gAFI, 0, "Age From ID", CP.gAFN, 65, "Age From", true, CP.gATI, 0, "Age To ID", CP.gATN, 58, "Age To", true, CP.gPID, 0, "Percentage ID", CP.gPER, 70, "Percentage", true);
                db.GridInit(grdBNK, 200, true, BD.gCOD, "Bank Code", 0, BD.gNME, "Bank", 123, true, BD.gBRC, "Branch ID", 0, BD.gBRN, "Branch", 120, true, BD.gACN, "Account Name", 187, BD.gANO, "AccountNo", 180, BD.gALM, "AccountLimit", 0, Type.GetType("System.Double"), "0.00");
                db.GridInit(grdCDay, 100, true, DY.gDFR, 90, "Days From", "##", DY.gDTO, 90, "Days To", "##.##", DY.gPID, 0, "Percentage ID", DY.gPER, 101, "Percentage", true);
                db.GridInit(grdCDate, 100, true, DT.gDFR, 65, "Date From", Type.GetType("System.DateTime"), DT.gDTO, 65, "Date To", Type.GetType("System.DateTime"), DT.gPID, 0, "Percentage ID", DT.gNOD, 65, "No Of Days", DT.gPER, 86, "Percentage", true);
                db.GridInit(grdPhoto, 100, true, PH.gNUM, 120, "Serial No", PH.gDES, 420, "Description", PH.gBRW, 70, "Browse", true, PH.gIMG, 0, "Image/Binary Data");
                db.GridInit(grdFeatures, 100, true, FT.gRTI, 0, "Room Type ID", FT.gRTN, 200, "Room Type", true, FT.gFTS, 399, "Description");
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Fill_Data()
        {
            try
            {
                string ssql = " SELECT ID,Code,Name,CheckInID,CheckOutID,DefRoomTypeID,DefBasisID,PayID,Star,EBedCost,CurrencyID,Image,CityID,TotRoom,AccTypeID,ContName,Address,Tel1,Tel2,PaxFoc,GuideFOC,Email,Fax,Web,Remarks,CancellationNote,VatNo,Isnull(IsActive,0) AS IsActive FROM mst_HotelDetails Where ID=" + SystemCode + "";
                DataTable DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT1.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    Mode = 1;
                    #region HOTEL BASIC DETAILS
                    SystemCode = (int)DT1.Rows[0]["ID"];
                    txtCompany.Text = DT1.Rows[0]["Name"].ToString();
                    if (DT1.Rows[0]["CheckInID"].ToString() != "") cmbCheckin.SelectedValue = (DT1.Rows[0]["CheckInID"].ToString());
                    if (DT1.Rows[0]["CheckOutID"].ToString() != "") cmbCheckout.SelectedValue = (DT1.Rows[0]["CheckOutID"].ToString());
                    if (DT1.Rows[0]["DefRoomTypeID"].ToString() != "") cmbDefRoom.SelectedValue = (DT1.Rows[0]["DefRoomTypeID"].ToString());
                    if (DT1.Rows[0]["DefBasisID"].ToString() != "") cmbDefBasis.SelectedValue = (DT1.Rows[0]["DefBasisID"].ToString());
                    if (DT1.Rows[0]["PayID"].ToString() != "") cmbPAY.SelectedValue = (DT1.Rows[0]["PayID"].ToString());
                    txtCode.Text = DT1.Rows[0]["Code"].ToString();
                    srcStar.SelectedStar = Convert.ToInt16(DT1.Rows[0]["Star"].ToString());
                    txtEbedCost.Text = DT1.Rows[0]["EBedCost"].ToString();
                    if (DT1.Rows[0]["Image"] != DBNull.Value)
                    {
                        byte[] Photo = (byte[])DT1.Rows[0]["Image"];
                        imageData1 = Photo;
                        MemoryStream ms = new MemoryStream(Photo);
                        pbImage.Image = Image.FromStream(ms, false, false);
                        lblImage.Visible = false;
                    }
                    else lblImage.Visible = true;
                    if (DT1.Rows[0]["CurrencyID"].ToString() != "") drpCurrency.setSelectedValue(DT1.Rows[0]["CurrencyID"].ToString());
                    if (DT1.Rows[0]["CityID"].ToString() != "") drpCity.setSelectedValue(DT1.Rows[0]["CityID"].ToString());
                    txtTotRoom.Text = DT1.Rows[0]["TotRoom"].ToString();
                    if (DT1.Rows[0]["AccTypeID"].ToString() != "") cmbAccType.SelectedValue = (DT1.Rows[0]["AccTypeID"].ToString());
                    txtContName.Text = DT1.Rows[0]["ContName"].ToString();
                    txtAddress.Text = DT1.Rows[0]["Address"].ToString();
                    txtTel1.Text = DT1.Rows[0]["Tel1"].ToString();
                    txtTel2.Text = DT1.Rows[0]["Tel2"].ToString();
                    if (DT1.Rows[0]["PaxFoc"].ToString() != "") nupPaxFOC.Value = Convert.ToInt32(DT1.Rows[0]["PaxFoc"].ToString());
                    if (DT1.Rows[0]["GuideFOC"].ToString() != "") nudGuideFOC.Value = Convert.ToInt16(DT1.Rows[0]["GuideFOC"].ToString());
                    txtEmail.Text = DT1.Rows[0]["Email"].ToString();
                    txtFax.Text = DT1.Rows[0]["Fax"].ToString();
                    txtWeb.Text = DT1.Rows[0]["Web"].ToString();
                    txtRemarks.Text = DT1.Rows[0]["Remarks"].ToString();
                    txtCancellationNote.Text = DT1.Rows[0]["CancellationNote"].ToString();
                    txtVATNo.Text = DT1.Rows[0]["VATNo"].ToString();
                    chkActive.Checked = (Convert.ToBoolean(DT1.Rows[0]["IsActive"].ToString()));
                    ssql = "SELECT HotelID,MarketID,MarketName,SeasonID,SeasonFrom,SeasonTo,Season,RoomTypeID,RoomTypeName,BasisID,BasisTypeName," +
                           "OccupancyID,Occupancy,ExtraBed,Vat,Tax,ServCharge,NoOfRooms,PriceWithTax," +
                           "GuideRoomCost,PriceWithoutTax" +
                           " FROM vwHotelReferance WHERE HotelID=" + SystemCode + " ORDER BY SrNo ";
                    DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT1.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT1.Rows.Count > RowNumb)
                        {
                            if (DT1.Rows[RowNumb]["MarketID"] + "".Trim() != "") grdAval[RowNumb + 1, (int)AV.gMKI] = Convert.ToInt32(DT1.Rows[RowNumb]["MarketID"]);
                            if (DT1.Rows[RowNumb]["MarketName"] + "".Trim() != "") grdAval[RowNumb + 1, (int)AV.gMKN] = DT1.Rows[RowNumb]["MarketName"].ToString();
                            grdAval[RowNumb + 1, (int)AV.gHSI] = Convert.ToInt16(DT1.Rows[RowNumb]["SeasonID"].ToString());
                            grdAval[RowNumb + 1, (int)AV.gHSN] = DT1.Rows[RowNumb]["Season"].ToString();
                            if (DT1.Rows[RowNumb]["SeasonFrom"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gSFD] = Convert.ToDateTime(DT1.Rows[RowNumb]["SeasonFrom"]);
                            if (DT1.Rows[RowNumb]["SeasonTo"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gSTD] = Convert.ToDateTime(DT1.Rows[RowNumb]["SeasonTo"]);
                            grdAval[RowNumb + 1, (int)AV.gRTI] = Convert.ToInt16(DT1.Rows[RowNumb]["RoomTypeID"].ToString());
                            grdAval[RowNumb + 1, (int)AV.gRTN] = DT1.Rows[RowNumb]["RoomTypeName"].ToString();
                            if (DT1.Rows[RowNumb]["BasisID"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gRBI] = Convert.ToInt16(DT1.Rows[RowNumb]["BasisID"].ToString());
                            if (DT1.Rows[RowNumb]["BasisTypeName"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gRBN] = DT1.Rows[RowNumb]["BasisTypeName"].ToString();
                            if (DT1.Rows[RowNumb]["OccupancyID"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gMID] = DT1.Rows[RowNumb]["OccupancyID"].ToString();
                            if (DT1.Rows[RowNumb]["Occupancy"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gMAX] = DT1.Rows[RowNumb]["Occupancy"].ToString();
                            if (DT1.Rows[RowNumb]["ExtraBed"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gEBD] = Convert.ToBoolean(DT1.Rows[RowNumb]["ExtraBed"].ToString());
                            if (DT1.Rows[RowNumb]["Vat"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gVAT] = Convert.ToDecimal(DT1.Rows[RowNumb]["Vat"].ToString());
                            if (DT1.Rows[RowNumb]["Tax"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gTAX] = Convert.ToDecimal(DT1.Rows[RowNumb]["Tax"].ToString());
                            if (DT1.Rows[RowNumb]["ServCharge"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gSCH] = Convert.ToDecimal(DT1.Rows[RowNumb]["ServCharge"].ToString());
                            if (DT1.Rows[RowNumb]["NoOfRooms"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gNOR] = Convert.ToInt16(DT1.Rows[RowNumb]["NoOfRooms"].ToString());
                            if (DT1.Rows[RowNumb]["PriceWithTax"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gTPR] = Convert.ToDecimal(DT1.Rows[RowNumb]["PriceWithTax"].ToString());
                            if (DT1.Rows[RowNumb]["GuideRoomCost"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gGPR] = Convert.ToDecimal(DT1.Rows[RowNumb]["GuideRoomCost"].ToString());
                            if (DT1.Rows[RowNumb]["PriceWithoutTax"].ToString() != "") grdAval[RowNumb + 1, (int)AV.gPRI] = Convert.ToDecimal(DT1.Rows[RowNumb]["PriceWithoutTax"].ToString());
                            RowNumb++;
                        }
                    }
                    DataTable DTGudie = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT BasisID,BasisName,CurrencyID,Currency,CostWithTax,Cost FROM vw_Hotel_Guide_Rates WHERE HotelID=" + SystemCode + " ORDER BY SrNo");
                     db.GridLoad(grdGuide, DTGudie, GD.gRBI, "BasisID", GD.gRBN, "BasisName", GD.gCID, "CurrencyID", GD.gCNM, "Currency", GD.gTPR, "CostWithTax", GD.gPRI, "Cost"); 
                    DataTable DTComSup = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Name,DateFrom,DateTo,BasisID,Basis,MealModeID,MealMode,CostWithTax,Cost FROM vw_Hotel_ComSupplements WHERE HotelID=" + SystemCode + " ORDER BY SrNo");
                    db.GridLoad(grdComSup, DTComSup, CS.gSNM, "Name", CS.gSDF, "DateFrom", CS.gSDT, "DateFrom", CS.gRBI, "BasisID", CS.gRBN, "Basis", CS.gMID, "MealModeID", CS.gMNM, "MealMode", CS.gTPR, "CostWithTax", CS.gPRI, "Cost"); 
                    DataTable DTMealSup = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CategoryID,Category,BasisID,Basis,MealModeID,MealMode,CostWithTax,Cost FROM vw_Hotel_MealSupplements WHERE HotelID=" + SystemCode + " ORDER BY SrNo");
                    db.GridLoad(grdMealSup, DTMealSup, MS.gMCI, "CategoryID", MS.gMCN, "Category", MS.gRBI, "BasisID", MS.gRBN, "Basis", MS.gMID, "MealModeID", MS.gMNM, "MealMode", MS.gTPR, "CostWithTax", MS.gPRI, "Cost"); 
                    DataTable DTCheck = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT TypeID,Type,FromID,[From],Amount,PercentageID,Percentage FROM vw_Hotel_CheckDetails WHERE HotelID=" + SystemCode + " ORDER BY SrNo");
                    db.GridLoad(grdCheck, DTCheck, CD.gTID, "TypeID", CD.gTNM, "Type", CD.gFID, "FromID", CD.gFRM, "From", CD.gAMT, "Amount", CD.gPID, "PercentageID", CD.gPER, "Percentage"); 
                    DataTable DTChild = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT BasisID,Basis,AgeFromID,AgeFrom,AgeToID,AgeTo,PercentageID,Percentage FROM vw_Hotel_ChildPolicy WHERE HotelID=" + SystemCode + " ORDER BY SrNo"); 
                    db.GridLoad(grdChild, DTChild, CP.gRBI, "BasisID", CP.gRBN, "Basis", CP.gAFI, "AgeFromID", CP.gAFN, "AgeFrom", CP.gATI, "AgeToID", CP.gATN, "AgeTo", CP.gPID, "PercentageID", CP.gPER, "Percentage"); 
                    DataTable DTCBDY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [From],[To],PercentageID,Percentage FROM vw_Hotel_CancelByDays WHERE HotelID=" + SystemCode + " ORDER BY SrNo");
                    db.GridLoad(grdCDay, DTCBDY, DY.gDFR, "From", DY.gDTO, "To", DY.gPID, "PercentageID", DY.gPER, "Percentage"); 
                    DataTable DTCBDT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [From],[To],NoOfDays,PercentageID,Percentage FROM vw_Hotel_CancelByDate WHERE HotelID=" + SystemCode + " ORDER BY SrNo"); 
                    db.GridLoad(grdCDate, DTCBDT, DT.gDFR, "From", DT.gDTO, "To", DT.gNOD, "NoOfDays", DT.gPID, "PercentageID", DT.gPER, "Percentage"); 
                    DataTable DTBank = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT BankID,BankName,BranchID,BranchName,Account,AccountNo,Limit FROM vw_Hotel_Bank_Details WHERE HotelID=" + SystemCode + " ORDER BY SrNo");
                    db.GridLoad(grdBNK, DTBank, BD.gCOD, "BankID", BD.gNME, "BankName", BD.gBRC, "BranchID", BD.gBRN, "BranchName", BD.gACN, "Account", BD.gANO, "AccountNo", BD.gALM, "Limit"); 
                    ssql = "SELECT ContName,Tel1,Tel2,Tel3,TDLNo,Fax,Web,Email FROM dbo.mst_HotelAccountDep WHERE HotelID=" + SystemCode + "";
                    DataTable DTAcc = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DTAcc.Rows.Count > 0)
                    {
                        txt_acc_ContName.Text = DTAcc.Rows[0]["ContName"].ToString();
                        txt_acc_Tel1.Text = DTAcc.Rows[0]["Tel1"].ToString();
                        txt_acc_Tel2.Text = DTAcc.Rows[0]["Tel2"].ToString();
                        txt_acc_Tel3.Text = DTAcc.Rows[0]["Tel3"].ToString();
                        txt_acc_TDL.Text = DTAcc.Rows[0]["TDLNo"].ToString();
                        txt_acc_Fax.Text = DTAcc.Rows[0]["Fax"].ToString();
                        txt_acc_Web.Text = DTAcc.Rows[0]["Web"].ToString();
                        txt_acc_Email.Text = DTAcc.Rows[0]["Email"].ToString();
                    } 
                    ssql = "SELECT HotelID,SerialNo,Description,Image FROM mst_HotelPhotos WHERE HotelID=" + SystemCode + " ORDER BY SrNo ";
                    DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT1.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT1.Rows.Count > RowNumb)
                        {
                            grdPhoto[RowNumb + 1, (int)PH.gNUM] = DT1.Rows[RowNumb]["SerialNo"].ToString();
                            grdPhoto[RowNumb + 1, (int)PH.gDES] = DT1.Rows[RowNumb]["Description"].ToString();
                            byte[] Photo = (byte[])DT1.Rows[RowNumb]["Image"];
                            imageData = Photo;
                            grdPhoto[RowNumb + 1, (int)PH.gIMG] = imageData;
                            RowNumb++;
                        }
                        update_tree_view();
                    } 
                    DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT HotelID,RoomTypeID,RoomType,Description FROM vw_HotelRoomFeatures WHERE HotelID=" + SystemCode + " ORDER BY SrNo ");
                    db.GridLoad(grdFeatures, DT1, FT.gRTI, "RoomTypeID", FT.gRTN, "RoomType", FT.gFTS, "Description"); 
                    #endregion
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            { 
                drpCity.DataSource= Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,City FROM mst_City Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpCurrency.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Currency FROM mst_Currency Where IsNull(IsActive,0)=1 ORDER BY ID");
                cmbCheckin.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Time FROM mst_Time Where IsNull(IsActive,0)=1 ORDER BY ID"); 
                cmbCheckout.DataSource  = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Time FROM mst_Time Where IsNull(IsActive,0)=1 ORDER BY ID"); 
                cmbDefRoom.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_RoomTypes Where IsNull(IsActive,0)=1 ORDER BY ID");
                cmbDefBasis.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_BasisTypes Where IsNull(IsActive,0)=1 ORDER BY ID"); 
                cmbAccType.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_AccomTypes Where IsNull(IsActive,0)=1 ORDER BY ID");
                cmbPAY.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Method FROM mst_HotelPayMethods Where IsNull(IsActive,0)=1 ORDER BY ID"); 
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Hotel_Details()
        {
            if (txtCode.Text.Trim() == "") return ERR("Please Enter Hotel-Code", txtCode);
            if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Code from dbo.mst_HotelDetails WHERE Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0) return ERR("Hotel Code is Already Exist.");
            if (txtCompany.Text.Trim() == "") return ERR("Hotel Name Cannot Be Blank.", txtCompany);
            if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Name from dbo.mst_HotelDetails WHERE Name='" + txtCompany.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0) return ERR("Hotel Name is Already Exist.");
            if (drpCity.SelectedValue.ToString() == "") return ERR("Hotel Location Cannot Be Blank.");
            return true;
        }
        private Boolean Validate_RoomRates_Details()
        {
            RowNumb = 1;
            if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gRTI].Index] == null) || (grdAval[RowNumb, (int)AV.gRTI].ToString() == "")) return true;
            do
            {
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gRTN].Index] == null) || (grdAval[RowNumb, (int)AV.gRTN].ToString() == "")) return ERR("Please Select Room Type ");
                else if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gSFD].Index] == null)) return ERR("'Season From' Date Cannot Be Blank ");
                else if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gSTD].Index] == null)) return ERR("'Season To' Date Cannot Be Blank ");
                RowNumb++;
            } while ((grdAval[RowNumb, grdAval.Cols[(int)AV.gRTI].Index] != null));
            return true;
        }
        private Boolean Validate_Guide_Details()
        {
            RowNumb = 1;
            if ((grdGuide[RowNumb, grdGuide.Cols[(int)GD.gRBI].Index] == null)) return true;
            do
            {
                if ((grdGuide[RowNumb, grdGuide.Cols[(int)GD.gCID].Index] == null) || (grdGuide[RowNumb, (int)GD.gCID].ToString() == "")) return ERR("Please Select Currency In Guide Rates");
                else if ((grdGuide[RowNumb, grdGuide.Cols[(int)GD.gPRI].Index] == null) && (grdGuide[RowNumb, grdGuide.Cols[(int)GD.gTPR].Index] == null)) return ERR("Please Select Guide Rates Cost.");
                RowNumb++;
            } while ((grdGuide[RowNumb, grdGuide.Cols[(int)GD.gRBI].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_Com_Supplements()
        {
            RowNumb = 1;
            if ((grdComSup[RowNumb, grdComSup.Cols[(int)CS.gSNM].Index] == null)) return true;
            do
            {
                if (grdComSup[RowNumb, grdComSup.Cols[(int)CS.gSDF].Index] == null) return ERR("Please Select Compulsory Supplements Date From.");
                else if (grdComSup[RowNumb, grdComSup.Cols[(int)CS.gSDT].Index] == null) return ERR("Please Select Compulsory Supplements Date To.");
                else if (grdComSup[RowNumb, grdComSup.Cols[(int)CS.gRBI].Index] == null) return ERR("Please Select Basis Type In Compulsory Supplements.");
                else if ((grdComSup[RowNumb, grdComSup.Cols[(int)CS.gPRI].Index] == null) && (grdComSup[RowNumb, grdComSup.Cols[(int)CS.gTPR].Index] == null)) return ERR("Please Select Compulsory Supplements Cost.");
                RowNumb++;
            } while ((grdComSup[RowNumb, grdComSup.Cols[(int)CS.gSNM].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_Meal_Supplements()
        {
            RowNumb = 1;
            if ((grdMealSup[RowNumb, grdMealSup.Cols[(int)MS.gMCI].Index] == null)) return true;
            do
            {
                if (grdMealSup[RowNumb, grdMealSup.Cols[(int)MS.gRBI].Index] == null) return ERR("Please Select Basis In Meal Supplement");
                if ((grdMealSup[RowNumb, grdMealSup.Cols[(int)MS.gPRI].Index] == null) && (grdMealSup[RowNumb, grdMealSup.Cols[(int)MS.gTPR].Index] == null)) return ERR("Please Select Meal Supplements Cost.");
                RowNumb++;
            } while ((grdMealSup[RowNumb, grdMealSup.Cols[(int)MS.gMCI].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_Check_Details()
        {
            RowNumb = 1;
            if ((grdCheck[RowNumb, grdCheck.Cols[(int)CD.gTID].Index] == null)) return true;
            do
            {
                if (grdCheck[RowNumb, grdCheck.Cols[(int)CD.gFID].Index] == null) return ERR("Please Select 'From Value' In Check Details.");
                else if (grdCheck[RowNumb, grdCheck.Cols[(int)CD.gPID].Index] == null && grdCheck[RowNumb, grdCheck.Cols[(int)CD.gAMT].Index] == null) return ERR("Please Select 'Percentage' Or Enter Amount In Check Details.");
                RowNumb++;
            } while ((grdCheck[RowNumb, grdComSup.Cols[(int)CD.gTID].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_ChildPolicy()
        {
            RowNumb = 1;
            if ((grdChild[RowNumb, grdChild.Cols[(int)CP.gRBI].Index] == null)) return true;
            do
            {
                if (grdChild[RowNumb, grdChild.Cols[(int)CP.gAFI].Index] == null) return ERR("Please Select 'Age From' In Child Policy.");
                else if (grdChild[RowNumb, grdChild.Cols[(int)CP.gATI].Index] == null) return ERR("Please Select 'Age To' In Child Policy.");
                else if (grdChild[RowNumb, grdChild.Cols[(int)CP.gPID].Index] == null) return ERR("Please Select 'Percentage' In Child Policy.");
                RowNumb++;
            } while ((grdChild[RowNumb, grdChild.Cols[(int)CP.gRBI].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_CancelByDay()
        {
            RowNumb = 1;
            if ((grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDFR].Index] == null)) return true;
            do
            {
                if (grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDTO].Index] == null || grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDTO].Index].ToString() == "") return ERR("Please Select 'Days To' In Cancel By Day Range.");
                if (grdCDay[RowNumb, grdCDay.Cols[(int)DY.gPID].Index] == null) return ERR("Please Select 'Percentage' In Cancel By Day Range.");
                RowNumb++;
            } while ((grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDFR].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_CancelByDate()
        {
            RowNumb = 1;
            if ((grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDFR].Index] == null)) return true;
            do
            {
                if (grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDTO].Index] == null) return ERR("Please Select 'Date To' In Cancel By Date Range.");
                if (grdCDate[RowNumb, grdCDate.Cols[(int)DT.gPID].Index] == null) return ERR("Please Select 'Percentage' In Cancel By Date Range.");
                RowNumb++;
            } while ((grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDFR].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_Photos()
        {
            RowNumb = 1;
            if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == "")) return true;
            do
            {
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == "")) return ERR("Please Enter Serial No ");
                else if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gDES].Index] == null) || (grdPhoto[RowNumb, (int)PH.gDES].ToString() == "")) return ERR("Image Description Cannot Be Blank ");
                else if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gIMG].Index] == null) || (grdPhoto[RowNumb, (int)PH.gIMG].ToString() == "")) return ERR("Image Cannot Be Blank.Please Browse For Image");
                RowNumb++;
            } while ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null));
            return true;
        }
        private Boolean Validate_Hotel_RoomFeatures()
        {
            RowNumb = 1;
            if ((grdFeatures[RowNumb, grdFeatures.Cols[(int)FT.gRTI].Index] == null) || (grdFeatures[RowNumb, (int)FT.gRTI].ToString() == "")) return true;
            do
            {
                if ((grdFeatures[RowNumb, grdFeatures.Cols[(int)FT.gFTS].Index] == null) || (grdFeatures[RowNumb, (int)FT.gFTS].ToString() == ""))
                {
                    ERR("Please Enter Room Features Description.");
                    this.tcHotelDetails.SelectedTab = tpViewPhotos;
                    tcHotelDetails_Click(null, null);
                    return false;
                }
                RowNumb++;
            } while ((grdFeatures[RowNumb, grdFeatures.Cols[(int)FT.gRTI].Index] != null));
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
                if (Save_Hotel_Details(objCom) == true && Save_RoomRates_details(objCom) == true && Save_Guide_Rates(objCom) == true && Save_Compusory_Supplements(objCom) == true && Save_Meal_Supplements(objCom) == true && Save_Check_Details(objCom) == true && Save_Child_Policy(objCom) == true && Save_CancelByDays(objCom) == true && Save_CancelByDate(objCom) == true && Save_BankDtls(objCom) == true && Save_AccDep(objCom) == true && Save_Hotel_Photos(objCom) == true && Save_Hotel_RoomFeatures(objCom) == true)
                {
                    objTrn.Commit();
                    MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    objCon.Close();
                    return true;
                }
                else
                {
                    objTrn.Rollback();
                    MessageBox.Show("Error Occured,Rollbacked", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                objCon.Close();
                return false;
        }
        private Boolean Save_Hotel_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Hotel_Details";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtCompany.Text.Trim();
                sqlCom.Parameters.Add("@CheckInID", SqlDbType.Int).Value = Convert.ToInt16(cmbCheckin.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@CheckOutID", SqlDbType.Int).Value = Convert.ToInt16(cmbCheckout.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@DefRoomTypeID", SqlDbType.Int).Value = Convert.ToInt16(cmbDefRoom.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@DefBasisID", SqlDbType.Int).Value = Convert.ToInt16(cmbDefBasis.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@PayID", SqlDbType.Int).Value = Convert.ToInt16(cmbPAY.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@Star", SqlDbType.Int).Value = srcStar.SelectedStar;
                if (imageData1 == null) sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = null;
                else sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = imageData1;
                if (txtEbedCost.Text.ToString() != "") sqlCom.Parameters.Add("@EBedCost", SqlDbType.Decimal).Value = txtEbedCost.Text.Trim();
                if (drpCurrency.SelectedValue.ToString() != "") sqlCom.Parameters.Add("@CurrencyID", SqlDbType.Int).Value = Convert.ToInt16(drpCurrency.SelectedValue.ToString());
                if (drpCity.SelectedValue.ToString() != "") sqlCom.Parameters.Add("@Location", SqlDbType.Int).Value = Convert.ToInt16(drpCity.SelectedValue.ToString());
                if (txtTotRoom.Text.ToString() != "") sqlCom.Parameters.Add("@TotRoom", SqlDbType.Int).Value = Convert.ToInt16(txtTotRoom.Text.Trim());
                sqlCom.Parameters.Add("@AccTypeID", SqlDbType.Int).Value = Convert.ToInt16(cmbAccType.SelectedValue.ToString().Trim());
                if (txtContName.Text.ToString() != "") sqlCom.Parameters.Add("@ContName", SqlDbType.VarChar, 50).Value = txtContName.Text.Trim();
                if (txtAddress.Text.ToString() != "") sqlCom.Parameters.Add("@Address", SqlDbType.VarChar, 250).Value = txtAddress.Text.Trim();
                if (txtTel1.Text.ToString() != "") sqlCom.Parameters.Add("@Tel1", SqlDbType.VarChar, 50).Value = txtTel1.Text.Trim();
                if (txtTel2.Text.ToString() != "") sqlCom.Parameters.Add("@Tel2", SqlDbType.VarChar, 50).Value = txtTel2.Text.Trim();
                sqlCom.Parameters.Add("@PaxFoc", SqlDbType.Int).Value = nupPaxFOC.Value;
                sqlCom.Parameters.Add("@GuideFOC", SqlDbType.Int).Value = nudGuideFOC.Value;
                if (txtEmail.Text.ToString() != "") sqlCom.Parameters.Add("@Email", SqlDbType.VarChar, 1000).Value = txtEmail.Text.Trim();
                if (txtFax.Text.ToString() != "") sqlCom.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = txtFax.Text.Trim();
                if (txtWeb.Text.ToString() != "") sqlCom.Parameters.Add("@Web", SqlDbType.VarChar, 50).Value = txtWeb.Text.Trim();
                if (txtRemarks.Text.ToString() != "") sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = txtRemarks.Text.Trim();
                if (txtCancellationNote.Text.ToString() != "") sqlCom.Parameters.Add("@CancellationNote", SqlDbType.VarChar, 2500).Value = txtCancellationNote.Text.Trim();
                if (txtVATNo.Text.ToString() != "") sqlCom.Parameters.Add("@VATNo", SqlDbType.NVarChar, 50).Value = txtVATNo.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
        }
        private Boolean Save_BankDtls(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelBankDetails";
            RowNumb = 1;
            while (grdBNK[RowNumb, grdBNK.Cols[(int)BD.gCOD].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdBNK[RowNumb, (int)BD.gBRC] != null && grdBNK[RowNumb, (int)BD.gBRC].ToString() != "") sqlCom.Parameters.Add("@BranchID", SqlDbType.Int).Value = Int32.Parse(grdBNK[RowNumb, (int)BD.gBRC].ToString());
                if (grdBNK[RowNumb, (int)BD.gACN] != null && grdBNK[RowNumb, (int)BD.gACN].ToString() != "") sqlCom.Parameters.Add("@Account", SqlDbType.VarChar, 50).Value = grdBNK[RowNumb, (int)BD.gACN].ToString();
                if (grdBNK[RowNumb, (int)BD.gANO] != null && grdBNK[RowNumb, (int)BD.gANO].ToString() != "") sqlCom.Parameters.Add("@AccountNo", SqlDbType.VarChar, 50).Value = grdBNK[RowNumb, (int)BD.gANO].ToString();
                if (grdBNK[RowNumb, (int)BD.gALM] != null && grdBNK[RowNumb, (int)BD.gALM].ToString() != "") sqlCom.Parameters.Add("@Limit", SqlDbType.Decimal).Value = Convert.ToDouble(grdBNK[RowNumb, (int)BD.gALM].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_Guide_Rates(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelGuideRates";
            RowNumb = 1;
            while (grdGuide[RowNumb, grdGuide.Cols[(int)BD.gCOD].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdGuide[RowNumb, (int)GD.gRBI] != null && grdGuide[RowNumb, (int)GD.gRBI].ToString() != "") sqlCom.Parameters.Add("@BasisID", SqlDbType.Int).Value = Int32.Parse(grdGuide[RowNumb, (int)GD.gRBI].ToString());
                if (grdGuide[RowNumb, (int)GD.gCID] != null && grdGuide[RowNumb, (int)GD.gCID].ToString() != "") sqlCom.Parameters.Add("@CurrencyID", SqlDbType.Int).Value = Int32.Parse(grdGuide[RowNumb, (int)GD.gCID].ToString());
                if (grdGuide[RowNumb, (int)GD.gTPR] != null && grdGuide[RowNumb, (int)GD.gTPR].ToString() != "") sqlCom.Parameters.Add("@CostWithTax", SqlDbType.Decimal).Value = Convert.ToDecimal(grdGuide[RowNumb, (int)GD.gTPR].ToString());
                if (grdGuide[RowNumb, (int)GD.gPRI] != null && grdGuide[RowNumb, (int)GD.gPRI].ToString() != "") sqlCom.Parameters.Add("@Cost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdGuide[RowNumb, (int)GD.gPRI].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_Compusory_Supplements(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelCompulsorySup";
            RowNumb = 1;
            while (grdComSup[RowNumb, grdComSup.Cols[(int)CS.gSNM].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdComSup[RowNumb, (int)CS.gSNM] != null && grdComSup[RowNumb, (int)CS.gSNM].ToString() != "") sqlCom.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = grdComSup[RowNumb, (int)CS.gSNM].ToString();
                if (grdComSup[RowNumb, (int)CS.gSDF] != null && grdComSup[RowNumb, (int)CS.gSDF].ToString() != "") sqlCom.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(grdComSup[RowNumb, (int)CS.gSDF].ToString());
                if (grdComSup[RowNumb, (int)CS.gSDT] != null && grdComSup[RowNumb, (int)CS.gSDT].ToString() != "") sqlCom.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(grdComSup[RowNumb, (int)CS.gSDT].ToString());
                if (grdComSup[RowNumb, (int)CS.gRBI] != null && grdComSup[RowNumb, (int)CS.gRBI].ToString() != "") sqlCom.Parameters.Add("@BasisID", SqlDbType.Int).Value = Int32.Parse(grdComSup[RowNumb, (int)CS.gRBI].ToString());
                if (grdComSup[RowNumb, (int)CS.gMID] != null && grdComSup[RowNumb, (int)CS.gMID].ToString() != "") sqlCom.Parameters.Add("@MealModeID", SqlDbType.Int).Value = Int32.Parse(grdComSup[RowNumb, (int)CS.gMID].ToString());
                if (grdComSup[RowNumb, (int)CS.gTPR] != null && grdComSup[RowNumb, (int)CS.gTPR].ToString() != "") sqlCom.Parameters.Add("@CostWithTax", SqlDbType.Decimal).Value = Convert.ToDecimal(grdComSup[RowNumb, (int)CS.gTPR].ToString());
                if (grdComSup[RowNumb, (int)CS.gPRI] != null && grdComSup[RowNumb, (int)CS.gPRI].ToString() != "") sqlCom.Parameters.Add("@Cost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdComSup[RowNumb, (int)CS.gPRI].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_Meal_Supplements(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelMealSup";
            RowNumb = 1;
            while (grdMealSup[RowNumb, grdMealSup.Cols[(int)MS.gMCI].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdMealSup[RowNumb, (int)MS.gMCI] != null && grdMealSup[RowNumb, (int)MS.gMCI].ToString() != "") sqlCom.Parameters.Add("@CategoryID", SqlDbType.Int).Value = Int32.Parse(grdMealSup[RowNumb, (int)MS.gMCI].ToString());
                if (grdMealSup[RowNumb, (int)MS.gRBI] != null && grdMealSup[RowNumb, (int)MS.gRBI].ToString() != "") sqlCom.Parameters.Add("@BasisID", SqlDbType.Int).Value = Int32.Parse(grdMealSup[RowNumb, (int)MS.gRBI].ToString());
                if (grdMealSup[RowNumb, (int)MS.gMID] != null && grdMealSup[RowNumb, (int)MS.gMID].ToString() != "") sqlCom.Parameters.Add("@MealModeID", SqlDbType.Int).Value = Int32.Parse(grdMealSup[RowNumb, (int)MS.gMID].ToString());
                if (grdMealSup[RowNumb, (int)MS.gTPR] != null && grdMealSup[RowNumb, (int)MS.gTPR].ToString() != "") sqlCom.Parameters.Add("@CostWithTax", SqlDbType.Decimal).Value = Convert.ToDecimal(grdMealSup[RowNumb, (int)MS.gTPR].ToString());
                if (grdMealSup[RowNumb, (int)MS.gPRI] != null && grdMealSup[RowNumb, (int)MS.gPRI].ToString() != "") sqlCom.Parameters.Add("@Cost", SqlDbType.Decimal).Value = Convert.ToDecimal(grdMealSup[RowNumb, (int)MS.gPRI].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_Check_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelCheckDetails";
            RowNumb = 1;
            while (grdCheck[RowNumb, grdCheck.Cols[(int)CD.gTID].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdCheck[RowNumb, (int)CD.gTID] != null && grdCheck[RowNumb, (int)CD.gTID].ToString() != "") sqlCom.Parameters.Add("@TypeID", SqlDbType.Int).Value = Int32.Parse(grdCheck[RowNumb, (int)CD.gTID].ToString());
                if (grdCheck[RowNumb, (int)CD.gFID] != null && grdCheck[RowNumb, (int)CD.gFID].ToString() != "") sqlCom.Parameters.Add("@FromID", SqlDbType.Int).Value = Int32.Parse(grdCheck[RowNumb, (int)CD.gFID].ToString());
                if (grdCheck[RowNumb, (int)CD.gAMT] != null && grdCheck[RowNumb, (int)CD.gAMT].ToString() != "") sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCheck[RowNumb, (int)CD.gAMT].ToString());
                if (grdCheck[RowNumb, (int)CD.gPID] != null && grdCheck[RowNumb, (int)CD.gPID].ToString() != "") sqlCom.Parameters.Add("@PercentageID", SqlDbType.Int).Value = Int32.Parse(grdCheck[RowNumb, (int)CD.gPID].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_Child_Policy(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelChildPolicy";
            RowNumb = 1;
            while (grdChild[RowNumb, grdChild.Cols[(int)CP.gRBI].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdChild[RowNumb, (int)CP.gRBI] != null && grdChild[RowNumb, (int)CP.gRBI].ToString() != "") sqlCom.Parameters.Add("@BasisID", SqlDbType.Int).Value = Int32.Parse(grdChild[RowNumb, (int)CP.gRBI].ToString());
                if (grdChild[RowNumb, (int)CP.gAFI] != null && grdChild[RowNumb, (int)CP.gAFI].ToString() != "") sqlCom.Parameters.Add("@AgeFromID", SqlDbType.Int).Value = Int32.Parse(grdChild[RowNumb, (int)CP.gAFI].ToString());
                if (grdChild[RowNumb, (int)CP.gATI] != null && grdChild[RowNumb, (int)CP.gATI].ToString() != "") sqlCom.Parameters.Add("@AgeToID", SqlDbType.Int).Value = Int32.Parse(grdChild[RowNumb, (int)CP.gATI].ToString());
                if (grdChild[RowNumb, (int)CP.gPID] != null && grdChild[RowNumb, (int)CP.gPID].ToString() != "") sqlCom.Parameters.Add("@PercentageID", SqlDbType.Int).Value = Int32.Parse(grdChild[RowNumb, (int)CP.gPID].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_CancelByDays(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelCancelByDays";
            RowNumb = 1;
            while (grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDFR].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdCDay[RowNumb, (int)DY.gDFR] != null && grdCDay[RowNumb, (int)DY.gDFR].ToString() != "") sqlCom.Parameters.Add("@From", SqlDbType.Int).Value = Int32.Parse(grdCDay[RowNumb, (int)DY.gDFR].ToString());
                if (grdCDay[RowNumb, (int)DY.gDTO] != null && grdCDay[RowNumb, (int)DY.gDTO].ToString() != "") sqlCom.Parameters.Add("@To", SqlDbType.Int).Value = Int32.Parse(grdCDay[RowNumb, (int)DY.gDTO].ToString());
                if (grdCDay[RowNumb, (int)DY.gPID] != null && grdCDay[RowNumb, (int)DY.gPID].ToString() != "") sqlCom.Parameters.Add("@PercentageID", SqlDbType.Int).Value = Int32.Parse(grdCDay[RowNumb, (int)DY.gPID].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_CancelByDate(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_HotelCancelByDate";
            RowNumb = 1;
            while (grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDFR].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                if (grdCDate[RowNumb, (int)DT.gDFR] != null && grdCDate[RowNumb, (int)DT.gDFR].ToString() != "") sqlCom.Parameters.Add("@From", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCDate[RowNumb, (int)DT.gDFR].ToString());
                if (grdCDate[RowNumb, (int)DT.gDTO] != null && grdCDate[RowNumb, (int)DT.gDTO].ToString() != "") sqlCom.Parameters.Add("@To", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCDate[RowNumb, (int)DT.gDTO].ToString());
                if (grdCDate[RowNumb, (int)DT.gNOD] != null && grdCDate[RowNumb, (int)DT.gNOD].ToString() != "") sqlCom.Parameters.Add("@NoOfDays", SqlDbType.Int).Value = Convert.ToInt32(grdCDate[RowNumb, (int)DT.gNOD].ToString());
                if (grdCDate[RowNumb, (int)DT.gPID] != null && grdCDate[RowNumb, (int)DT.gPID].ToString() != "") sqlCom.Parameters.Add("@PercentageID", SqlDbType.Int).Value = Int32.Parse(grdCDate[RowNumb, (int)DT.gPID].ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) RtnVal = false;
                RowNumb++;
            }
            return RtnVal;
        }
        private Boolean Save_RoomRates_details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gRTI].Index] == null) || (grdAval[RowNumb, (int)AV.gRTI].ToString() == "")) return true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_Hotel_Referance";
            while (grdAval[RowNumb, grdAval.Cols[(int)AV.gRTI].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@HotelID", SqlDbType.Int).Value = SystemCode;
                if (grdAval[RowNumb, (int)AV.gMKI] + "".Trim() != "") sqlCom.Parameters.Add("@MarketID", SqlDbType.Int).Value = Int32.Parse(grdAval[RowNumb, (int)AV.gMKI].ToString());
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gHSI].Index] != null) && (grdAval[RowNumb, (int)AV.gHSI].ToString() != "")) sqlCom.Parameters.Add("@SeasonID", SqlDbType.Int).Value = Int32.Parse(grdAval[RowNumb, (int)AV.gHSI].ToString());
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gSFD].Index] != null)) sqlCom.Parameters.Add("@SeasonFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(grdAval[RowNumb, (int)AV.gSFD]);
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gSTD].Index] != null)) sqlCom.Parameters.Add("@SeasonTo", SqlDbType.DateTime).Value = Convert.ToDateTime(grdAval[RowNumb, (int)AV.gSTD]);
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gRTI].Index] != null) && (grdAval[RowNumb, (int)AV.gRTI].ToString() != "")) sqlCom.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value = Int32.Parse(grdAval[RowNumb, (int)AV.gRTI].ToString());
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gRBI].Index] != null) && (grdAval[RowNumb, (int)AV.gRBI].ToString() != "")) sqlCom.Parameters.Add("@BasisID", SqlDbType.Int).Value = Int32.Parse(grdAval[RowNumb, (int)AV.gRBI].ToString());
                if (grdAval[RowNumb, grdAval.Cols[(int)AV.gCID].Index] != null) sqlCom.Parameters.Add("@ConditionID", SqlDbType.Int).Value = Convert.ToInt32(grdAval[RowNumb, (int)AV.gCID].ToString());
                if (grdAval[RowNumb, grdAval.Cols[(int)AV.gMID].Index] != null) sqlCom.Parameters.Add("@OccupancyID", SqlDbType.Int).Value = Convert.ToInt32(grdAval[RowNumb, (int)AV.gMID].ToString());
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gTAX].Index] != null) && (grdAval[RowNumb, (int)AV.gTAX].ToString() != "")) sqlCom.Parameters.Add("@Tax", SqlDbType.Decimal, 18).Value = grdAval[RowNumb, (int)AV.gTAX].ToString();
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gGPR].Index] != null) && (grdAval[RowNumb, (int)AV.gGPR].ToString() != "")) sqlCom.Parameters.Add("@GuideRoomCost", SqlDbType.Decimal, 18).Value = grdAval[RowNumb, (int)AV.gGPR].ToString();
                if ((grdAval[RowNumb, grdAval.Cols[(int)AV.gPRI].Index] != null) && (grdAval[RowNumb, (int)AV.gPRI].ToString() != "")) sqlCom.Parameters.Add("@GuestRoomCost", SqlDbType.Decimal, 18).Value = grdAval[RowNumb, (int)AV.gPRI].ToString();
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                RowNumb++;
            }
            return true;
        }
        private Boolean Save_Hotel_Photos(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == "")) return true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_Hotel_Photos";
            while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@HotelID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@SerialNo", SqlDbType.VarChar, 20).Value = grdPhoto[RowNumb, (int)PH.gNUM].ToString();
                sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = grdPhoto[RowNumb, (int)PH.gDES].ToString();
                sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = grdPhoto[RowNumb, (int)PH.gIMG];
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                RowNumb++;
            }
            return true;
        }
        private Boolean Save_Hotel_RoomFeatures(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            if ((grdFeatures[RowNumb, grdFeatures.Cols[(int)FT.gRTI].Index] == null) || (grdFeatures[RowNumb, (int)FT.gRTI].ToString() == "")) return true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_Hotel_RoomFeatures";
            while (grdFeatures[RowNumb, grdFeatures.Cols[(int)FT.gRTI].Index] != null)
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@RoomTypeID", SqlDbType.Int).Value = grdFeatures[RowNumb, (int)FT.gRTI].ToString();
                sqlCom.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = grdFeatures[RowNumb, (int)FT.gFTS].ToString();
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
                RowNumb++;
            }
            return true;
        }
        private Boolean Save_AccDep(System.Data.SqlClient.SqlCommand sqlCom)
        {
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_Hotel_AccDep";
            sqlCom.Parameters.Clear();
            sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
            sqlCom.Parameters.Add("@ContName", SqlDbType.NVarChar, 100).Value = txt_acc_ContName.Text.ToString().Trim();
            sqlCom.Parameters.Add("@Tel1", SqlDbType.NVarChar, 100).Value = txt_acc_Tel1.Text.ToString().Trim();
            sqlCom.Parameters.Add("@Tel2", SqlDbType.NVarChar, 100).Value = txt_acc_Tel2.Text.ToString().Trim();
            sqlCom.Parameters.Add("@Tel3", SqlDbType.NVarChar, 100).Value = txt_acc_Tel3.Text.ToString().Trim();
            sqlCom.Parameters.Add("@TDLNo", SqlDbType.NVarChar, 100).Value = txt_acc_TDL.Text.ToString().Trim();
            sqlCom.Parameters.Add("@Fax", SqlDbType.NVarChar, 100).Value = txt_acc_Fax.Text.ToString().Trim();
            sqlCom.Parameters.Add("@Web", SqlDbType.NVarChar, 100).Value = txt_acc_Web.Text.ToString().Trim();
            sqlCom.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = txt_acc_Email.Text.ToString().Trim();
            sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
            sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
            sqlCom.ExecuteNonQuery();
            if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) return false;
            return true;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;
            if ((Validate_Hotel_Details() && Validate_RoomRates_Details() && Validate_Guide_Details() && Validate_Hotel_Com_Supplements() && Validate_Hotel_Meal_Supplements() && Validate_Hotel_Check_Details() && Validate_Hotel_ChildPolicy() && Validate_Hotel_CancelByDay() && Validate_Hotel_CancelByDate() && Validate_Hotel_Photos() && Validate_Hotel_RoomFeatures() && Save_Procedure()) == true)
            {
                Fill_Data();
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
        private void grdAval_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdAval.Cols[(int)AV.gMKN].Index) Search(grdAval, AV.gMKI, AV.gMKN, "SELECT ID,MarketName [Market Name] FROM mst_HotelMarket Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmMarket());
            if (e.Col == grdAval.Cols[(int)AV.gHSN].Index) if (Search(grdAval, AV.gHSI, AV.gHSN, "SELECT ID,Name AS Season FROM mst_HotelSeasons Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmHotelSeason()))
                {
                    int SeasonID = Convert.ToInt32(grdAval[grdAval.Row, (int)AV.gHSI]);
                    DataTable DTSeason = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [From],[To] FROM mst_HotelSeasons Where ID=" + SeasonID + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                    if (DTSeason.Rows[0]["From"].ToString() != "")
                    {
                        grdAval[grdAval.Row, (int)AV.gSFD] = Convert.ToDateTime(DTSeason.Rows[0]["From"]);
                        grdAval[grdAval.Row, (int)AV.gSTD] = Convert.ToDateTime(DTSeason.Rows[0]["To"]);
                    }
                }
            if (e.Col == grdAval.Cols[(int)AV.gRTN].Index) if (Search(grdAval, AV.gRTI, AV.gRTN, "SELECT ID,Name FROM mst_RoomTypes Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmRoomTypes()))
                {
                    grdAval[grdAval.Row, (int)AV.gRBI] = null;
                    grdAval[grdAval.Row, (int)AV.gRBN] = null;
                }
            if (e.Col == grdAval.Cols[(int)AV.gRBN].Index) if (grdAval[grdAval.Row, grdAval.Cols[(int)AV.gRTI].Index] != null) Search(grdAval, AV.gRBI, AV.gRBN, "SELECT ID,Name FROM mst_BasisTypes Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmBasisTypes());
            if (e.Col == grdAval.Cols[(int)AV.gCON].Index) if (grdAval[grdAval.Row, grdAval.Cols[(int)AV.gRTI].Index] != null) Search(grdAval, AV.gCID, AV.gCON, "SELECT ID,Name FROM mst_HotelConditions Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmHotelCondition());
            if (e.Col == grdAval.Cols[(int)AV.gMAX].Index) if (grdAval[grdAval.Row, grdAval.Cols[(int)AV.gRTI].Index] != null) Search(grdAval, AV.gMID, AV.gMAX, "SELECT ID,Name FROM mst_HotelOccupnacy Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmHotelOccupancy());
        }
        private void grdPhoto_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdPhoto.Cols[(int)PH.gBRW].Index)
            {
                OpenFileDialog fdLogo = new OpenFileDialog();
                fdLogo.Title = "Choose a Photo";
                fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (DialogResult.OK == fdLogo.ShowDialog())
                {
                    string imageLocation = fdLogo.FileName;
                    imageData = null;
                    FileInfo fileInfo = new FileInfo(imageLocation);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);
                    grdPhoto[grdPhoto.Row, (int)PH.gIMG] = imageData;
                    update_tree_view();
                }
            }
        }
        private void grdBNK_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        { 
            if (grdBNK[grdBNK.Row, grdBNK.Cols[(int)BD.gCOD].Index] != null)      BankID = Convert.ToInt32(grdBNK[grdBNK.Row, (int)BD.gCOD].ToString());
            if (e.Col == grdBNK.Cols[(int)BD.gNME].Index)if( Search(grdBNK,BD.gCOD,BD.gNME,"SELECT ID,BankName FROM mst_BankMaster Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmBank()))
            { 
                    grdBNK[grdBNK.Row, (int)BD.gBRC] =    grdBNK[grdBNK.Row, (int)BD.gBRN] = null; 
            }
            if (e.Col == grdBNK.Cols[(int)BD.gBRN].Index)  if (grdBNK[grdBNK.Row, grdBNK.Cols[(int)BD.gCOD].Index] != null) Search(grdBNK,BD.gBRC,BD.gBRN, "SELECT ID,BranchName FROM mst_BankBranchMaster Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1 AND BankID= " + BankID + "", new Master.frmBankBranch());
       }
        public void update_tree_view()
        {
            try
            {
                tvPhotoDesc.Nodes.Clear();
                RowNumb = 1;
                string TreeName = "";
                if (txtCode.Text.ToString() != "")
                    TreeName = txtCode.Text.Trim();
                if (RowNumb == 1)
                {
                    pbMultiPhotos.Image = null;
                    byte[] Photo = (byte[])grdPhoto[1, (int)PH.gIMG];
                    imageData = Photo;
                    MemoryStream ms = new MemoryStream(Photo);
                    pbMultiPhotos.Image = Image.FromStream(ms, false, false);
                }
                TreeNode trNode = new TreeNode(TreeName);
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return;
                }
                while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
                {
                    if (grdPhoto[RowNumb, (int)PH.gDES] != null && grdPhoto[RowNumb, (int)PH.gDES].ToString() != "")
                    {
                        TreeNode trn = new TreeNode(grdPhoto[RowNumb, (int)PH.gDES].ToString());
                        trn.Name = RowNumb.ToString();
                        trNode.Nodes.Add(trn);
                    }
                    RowNumb++;
                }
                tvPhotoDesc.Nodes.Add(trNode);
                trNode.Expand();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public void update_features_tree_view()
        {
            try
            {
                tvFeatures.Nodes.Clear();
                RowNumb = 1;
                if (RowNumb == 1)
                {
                    rtbFeatures.Text = null;
                    rtbFeatures.Text = grdFeatures[1, (int)FT.gFTS].ToString();
                }
                TreeNode trNode = new TreeNode("Room Types");
                if ((grdFeatures[RowNumb, grdFeatures.Cols[(int)FT.gRTI].Index] == null) || (grdFeatures[RowNumb, (int)FT.gRTI].ToString() == ""))
                {
                    return;
                }
                while (grdFeatures[RowNumb, grdFeatures.Cols[(int)FT.gRTI].Index] != null)
                {
                    if (grdFeatures[RowNumb, (int)FT.gRTN] != null && grdFeatures[RowNumb, (int)FT.gRTN].ToString() != "")
                    {
                        TreeNode trn = new TreeNode(grdFeatures[RowNumb, (int)FT.gRTN].ToString());
                        trn.Name = RowNumb.ToString();
                        trNode.Nodes.Add(trn);
                    }
                    RowNumb++;
                }
                tvFeatures.Nodes.Add(trNode);
                trNode.Expand();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void tvPhotoDesc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name.ToString() != "")
            {
                pbMultiPhotos.Image = null;
                int NodeID = Convert.ToInt16(e.Node.Name.ToString());
                byte[] Photo = (byte[])grdPhoto[NodeID, (int)PH.gIMG];
                imageData = Photo;
                MemoryStream ms = new MemoryStream(Photo);
                pbMultiPhotos.Image = Image.FromStream(ms, false, false);
            }
        }
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    chkActive.Checked = true;
                else return;
            }
        }
        private void drpCity_Click_Open(object sender, EventArgs e)
        {
            Form frm = Classes.clsForms.rtnForm("frmCity", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Driver Photo";
            fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (DialogResult.OK == fdLogo.ShowDialog())
            {
                string imageLocation = fdLogo.FileName;
                pbImage.ImageLocation = imageLocation;
                lblImage.Visible = false;
                imageData1 = null;
                FileInfo fileInfo = new FileInfo(imageLocation);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData1 = br.ReadBytes((int)imageFileLength);
            }
        }
        private void btnClearLogo_Click(object sender, EventArgs e)
        {
            pbImage.Image = null;
            lblImage.Visible = true;
            imageData = null;
        }
        private void grdGuide_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdGuide.Cols[(int)GD.gRBN].Index) Search(grdGuide, GD.gRBI, GD.gRBN, "SELECT ID,Name FROM mst_BasisTypes Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmBasisTypes());
            if (e.Col == grdGuide.Cols[(int)GD.gCNM].Index) Search(grdGuide, GD.gCID, GD.gCNM, "SELECT ID,Currency FROM mst_Currency Where IsNull(IsActive,0)=1");
        }
        private void grdComSup_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdComSup.Cols[(int)CS.gMNM].Index) Search(grdComSup, CS.gMID, CS.gMNM, "SELECT ID,Name FROM mst_MealMode Where IsNull(IsActive,0)=1");
            if (e.Col == grdComSup.Cols[(int)CS.gRBN].Index) Search(grdComSup, CS.gRBI, CS.gRBN, "SELECT ID,Name FROM mst_BasisTypes Where IsNull(IsActive,0)=1");
        }
        private void grdMealSup_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdMealSup.Cols[(int)MS.gMCN].Index) Search(grdMealSup, MS.gMCI, MS.gMCN, "SELECT ID,Category FROM mst_HotelCategory Where IsNull(IsActive,0)=1");
            if (e.Col == grdMealSup.Cols[(int)MS.gMNM].Index) Search(grdMealSup, MS.gMID, MS.gMNM, "SELECT ID,Name FROM mst_MealMode Where IsNull(IsActive,0)=1");
            if (e.Col == grdMealSup.Cols[(int)MS.gRBN].Index) Search(grdMealSup, MS.gRBI, MS.gRBN, "SELECT ID,Name FROM mst_BasisTypes Where IsNull(IsActive,0)=1");
        }
        private void grdCheck_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdCheck.Cols[(int)CD.gTNM].Index) Search(grdCheck, CD.gTID, CD.gTNM, "SELECT ID,Name FROM mst_CheckDetails Where IsNull(IsActive,0)=1");
            else if (e.Col == grdCheck.Cols[(int)CD.gFRM].Index) Search(grdCheck, CD.gFID, CD.gFRM, "SELECT ID,Time FROM mst_Time Where IsNull(IsActive,0)=1");
            else if (e.Col == grdCheck.Cols[(int)CD.gPER].Index) Search(grdCheck, CD.gPID, CD.gPER, "SELECT ID,Percentage FROM mst_Percentage Where IsNull(IsActive,0)=1");
        }
        private void grdChild_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdChild.Cols[(int)CP.gRBN].Index) Search(grdChild, CP.gRBI, CP.gRBN, "SELECT ID,Name FROM mst_BasisTypes Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1", new Master.frmBasisTypes());
            else if (e.Col == grdChild.Cols[(int)CP.gAFN].Index) Search(grdChild, CP.gAFI, CP.gAFN, "SELECT ID,Age FROM mst_AgeDetails Where IsNull(IsActive,0)=1");
            else if (e.Col == grdChild.Cols[(int)CP.gATN].Index) Search(grdChild, CP.gATI, CP.gATN, "SELECT ID,Age FROM mst_AgeDetails Where IsNull(IsActive,0)=1");
            else if (e.Col == grdChild.Cols[(int)CP.gPER].Index) Search(grdChild, CP.gPID, CP.gPER, "SELECT ID,Percentage FROM mst_Percentage Where IsNull(IsActive,0)=1");
        }
        private void grdCDay_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdCDay.Cols[(int)DY.gPER].Index) Search(grdCDay, DY.gPID, DY.gPER, "SELECT ID,Percentage FROM mst_Percentage Where IsNull(IsActive,0)=1");
        }
        private void grdCDate_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdCDate.Cols[(int)DT.gPER].Index) Search(grdCDate, DT.gPID, DT.gPER, "SELECT ID,Percentage FROM mst_Percentage Where IsNull(IsActive,0)=1");
        }
        private void grdFeatures_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdFeatures.Cols[(int)FT.gRTN].Index) { if (Search(grdFeatures, FT.gRTI, FT.gRTN, "SELECT ID,Name FROM mst_RoomTypes Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1"))    grdFeatures[grdFeatures.Row, (int)FT.gFTS] = null; }
        }
        private void tcHotelDetails_Click(object sender, EventArgs e)
        {
            if (tcHotelDetails.SelectedTab.Name == "tpRoomRates")
            {
                this.Width = 1052;
                tcHotelDetails.Width = 1050;
                bntCopyRoomRates.Visible = true;
                btnDefaults.Visible = true;
                chkActive.Location = new Point(757, 460);
                btnOk.Location = new Point(838, 456);
                btnCancel.Location = new Point(920, 456);
            }
            else
            {
                this.Width = 660;
                tcHotelDetails.Width = 643;
                bntCopyRoomRates.Visible = false;
                btnDefaults.Visible = false;
                chkActive.Location = new Point(407, 460);
                btnOk.Location = new Point(478, 456);
                btnCancel.Location = new Point(560, 456);
            }
        }
        private void tvFeatures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name.ToString() == "") return;
            int NodeID = Convert.ToInt16(e.Node.Name.ToString());
            string Descrip = grdFeatures[NodeID, (int)FT.gFTS].ToString();
            rtbFeatures.Text = null;
            rtbFeatures.Text = Descrip;
        }
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            try
            {
                SqlQry = "SELECT ISNULL(MarketID,0) AS MarketID,MarketName,ISNULL(SeasonID,0) AS SeasonID,[From]AS Frm,[To] AS Too,Season,RoomTypeID,RoomTypeName,BasisID,BasisTypeName,OccupancyID,Occupancy FROM vw_def_RoomRates WHERE IsNull(IsActive,0)=1 ORDER BY SrNo ";
                DataTable DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                if (DT1.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT1.Rows.Count > RowNumb)
                    {
                        grdAval[RowNumb + 1, (int)AV.gHSI] = Convert.ToInt16(DT1.Rows[RowNumb]["MarketID"].ToString());
                        grdAval[RowNumb + 1, (int)AV.gHSN] = DT1.Rows[RowNumb]["MarketName"].ToString();
                        grdAval[RowNumb + 1, (int)AV.gHSI] = Convert.ToInt16(DT1.Rows[RowNumb]["SeasonID"].ToString());
                        grdAval[RowNumb + 1, (int)AV.gHSN] = DT1.Rows[RowNumb]["Season"].ToString();
                        grdAval[RowNumb + 1, (int)AV.gSFD] = Convert.ToDateTime(DT1.Rows[RowNumb]["Frm"]);
                        grdAval[RowNumb + 1, (int)AV.gSTD] = Convert.ToDateTime(DT1.Rows[RowNumb]["Too"]);
                        grdAval[RowNumb + 1, (int)AV.gRTI] = Convert.ToInt16(DT1.Rows[RowNumb]["RoomTypeID"].ToString());
                        grdAval[RowNumb + 1, (int)AV.gRBI] = Convert.ToInt16(DT1.Rows[RowNumb]["BasisID"].ToString());
                        grdAval[RowNumb + 1, (int)AV.gRTN] = DT1.Rows[RowNumb]["RoomTypeName"].ToString();
                        grdAval[RowNumb + 1, (int)AV.gRBN] = DT1.Rows[RowNumb]["BasisTypeName"].ToString();
                        grdAval[RowNumb + 1, (int)AV.gMID] = DT1.Rows[RowNumb]["OccupancyID"].ToString();
                        grdAval[RowNumb + 1, (int)AV.gMAX] = DT1.Rows[RowNumb]["Occupancy"].ToString();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void txtCompany_TextChanged(object sender, EventArgs e)
        {
            string s = txtCompany.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s, errorProvider1, txtCompany);
        }
        private void txtCompany_Leave(object sender, EventArgs e) { errorProvider1.Clear(); }
        private void RowColChange(object sender, EventArgs e)
        {
            C1.Win.C1FlexGrid.C1FlexGrid fg = (C1.Win.C1FlexGrid.C1FlexGrid)sender;
            if (bLoad == true) return;
            fg.Rows[1].AllowEditing = true;
            if (fg.Rows.Count < 3) return;
            fg.Rows[fg.Row].AllowEditing = (fg[fg.Row - 1, fg == grdAval ? 2 : 0] != null);
        }
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete) return;
            C1.Win.C1FlexGrid.C1FlexGrid fg = (C1.Win.C1FlexGrid.C1FlexGrid)sender;
            fg.Rows.Remove(fg.Row);
            fg.Rows[1].AllowEditing = true;
        }
        private void bntCopyRoomRates_Click(object sender, EventArgs e) { CopyLastRow(grdAval, AV.gMKI, AV.gMKN, AV.gHSI, AV.gHSN, AV.gSFD, AV.gSTD, AV.gRTI, AV.gRTN, AV.gRBI, AV.gRBN, AV.gMID, AV.gMAX); }
        private void bntCopyGuide_Click(object sender, EventArgs e) { CopyLastRow(grdGuide, GD.gRBI, GD.gRBN, GD.gCID, GD.gCNM); }
        private void btnCopyComSup_Click(object sender, EventArgs e) { CopyLastRow(grdComSup, CS.gSNM, CS.gSDF, CS.gMID, CS.gMNM, CS.gRBI, CS.gRBN); }
        private void btnCopyMealSup_Click(object sender, EventArgs e) { CopyLastRow(grdMealSup, MS.gMCI, MS.gMCN, MS.gMID, MS.gMNM, MS.gRBI, MS.gRBN); }
        private void bntCopyCheck_Click(object sender, EventArgs e) { CopyLastRow(grdCheck, CD.gTID, CD.gTNM, CD.gFID, CD.gFRM, CD.gPID, CD.gPER); }
        private void btnCopyChild_Click(object sender, EventArgs e) { CopyLastRow(grdChild, CP.gRBI, CP.gRBN, CP.gAFI, CP.gAFN, CP.gATI, CP.gATN, CP.gPID, CP.gPER); }
        private void btnCopyDay_Click(object sender, EventArgs e) { CopyLastRow(grdCDay, DY.gDFR, DY.gDTO, DY.gPID, DY.gPER); }
        private void btnCopyDate_Click(object sender, EventArgs e) { CopyLastRow(grdCDate, DT.gDFR, DT.gDTO, DT.gPID, DT.gPER); }
        private void CopyLastRow(C1.Win.C1FlexGrid.C1FlexGrid fg, params object[] ar)
        {
            RowNumb = 1;
            while (fg[RowNumb, fg.Cols[0].Index] != null) RowNumb++;
            if (RowNumb == 1 || RowNumb == fg.Rows.Count) return;
            for (int i = 0; i < fg.Cols.Count; i++) fg[RowNumb, i] = fg[RowNumb - 1, i];
        }
        private bool ERR(string msg, Control c = null)
        {
            MessageBox.Show(msg, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (c != null) c.Select();
            return false;
        }
        private bool Search(C1.Win.C1FlexGrid.C1FlexGrid fg, object c1, object c2, string SqlQuery, Form sub = null)
        {
            Other.frmSearchGrd frm = new Tourist_Management.Other.frmSearchGrd();
            try { frm.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery); }
            catch (Exception) { frm.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery); }
            frm.SubForm = sub;
            frm.Width = fg.Cols[(int)c2].Width;
            frm.Height = fg.Height;
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(fg);
            string[] SelText = frm.Open_Search();
            if (SelText != null)
            {
                fg[fg.Row, (int)c1] = SelText[0];
                fg[fg.Row, (int)c2] = SelText[1];
            }
            return SelText != null;
        }
    }
}