using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.User_Controls
{
    public partial class ucHotelNavigation : UserControl
    {
        private const string msghd = "Hotel Details";
        bool IsLoaded;
        bool IsProcess = false;
        int ProcessRowNo=0;
        int HID = 0;
        int InsMode=0;
        bool update;
        double TransID = 0;
        int singles = 0, doubles = 0, triples = 0, twin=0;
        int Adult = 0, Child = 0;
        string VoucherNumber = "";
        int TabNo = 1;
        bool notenbale = true,canl=false;
        string[] Occ = new string[5];
        int[] Rmm = new int[5];
        enum HG { gVNO, gBNO,gCHI,gCHO, gRTI, gRTN, gRBI, gRBN, gCID, gCON, gMID, gMAX, gNOR, gNGR, gEBD, gVAT, gTAX, gSCH, gTPR, gPRI, gTOT, gSEL, gLUP, gFOC, gMRC, gGRC, gMEC };
        enum MS { gMID, gMTM, gNOA, gNOC, gNOG, gAMC, gCMC, gGMC, gTOT };
        Boolean bLoad = false;
        DateTime InDate, OutDate;
        public ucHotelNavigation(){InitializeComponent();}
        public Boolean Cancelled
        {
            get
            {
                return canl;
            }
            set
            {
                canl = value;
            }
        }
        public Boolean NotEnable
        {
            get
            {
                return notenbale;
            }
            set
            {
                notenbale = value;
            }
        }
        public int Mode
        {
            get
            {
                return InsMode;
            }
            set
            {
                InsMode = value;
            }
        }
        public bool IsUpdate
        {
            get { return update; }
            set { update = value; }
        }
        public double TransactionID
        {
            get
            {
                return TransID;
            }
            set
            {
                TransID = value;
            }
        }
        public int HotelID
        {
            get
            {
                return HID;
            }
            set
            {
                HID = value;
            }
        }
        public int SingleRooms
        {
            get
            {
                return singles;
            }
            set
            {
                singles = value;
            }
        }
        public int DoubleRooms
        {
            get
            {
                return doubles;
            }
            set
            {
                doubles = value;
            }
        }
        public int TripleRooms
        {
            get
            {
                return triples;
            }
            set
            {
                triples = value;
            }
        }
        public int TwinRooms
        {
            get
            {
                return twin;
            }
            set
            {
                twin = value;
            }
        }
        public int NoOfAdult
        {
            get
            {
                return Adult;
            }
            set
            {
                Adult = value;
            }
        }
        public int NoOfChild
        {
            get
            {
                return Child;
            }
            set
            {
                Child = value;
            }
        }
        public DateTime CheckIn
        {
            get
            {
                return InDate;
            }
            set
            {
                InDate = value;
            }
        }
        public DateTime CheckOut
        {
            get
            {
                return OutDate;
            }
            set
            {
                OutDate = value;
            }
        }
        public string VoucherNo
        {
            get
            {
                return VoucherNumber;
            }
            set
            {
                VoucherNumber = value;
            }
        }
        public int TabNumber
        {
            get
            {
                return TabNo;
            }
            set
            {
                TabNo = value;
            }
        }
        public void Intializer()
        {
            try
            {
                DataTable dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT UserGroupID FROM vw_CurrentUserDetails Where UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()));
                if (notenbale)
                {
                    if (dt.Rows[0]["UserGroupID"].ToString() == "1001")//----- Check if the user is an admin(user grp 1001). admin can modify settled records
                    {
                        grdHotel.Enabled = true;
                        lblIsPaid.Visible = true;
                    }
                    else
                    {
                        grdHotel.Enabled = false;
                        lblIsPaid.Visible = true;
                    }
                }
                else
                {
                    grdHotel.Enabled = true;
                    lblIsPaid.Visible = false;
                }
                Grd_Initializer();                
                Hotel_Season();
                set_Ocuupancies();                
                if (update) //IF NOT AN EXISITING RECORD FILL WITH DEFAULT 
                {
                    Fill_Control();
                    Fill_Grid();
                }
                else
                {
                    Fill_Info();
                }
                IsLoaded = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {
                DataTable[]       DTB = new DataTable[1];
                cmbMarket.DataSource = DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DISTINCT MarketID,MarketName FROM vwHotelReferance WHERE HotelID=" + HID + " ORDER BY MarketName"); 
                 cmbMarket.Enabled =  (DTB[0].Rows.Count > 0 && DTB[0].Rows[0][0] + "".Trim() != "");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdHotel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdHotel.Rows.Remove(grdHotel.Row);
                grdHotel.Rows[1].AllowEditing = true;
            }
        }
        private void ucHotelNavigation_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        public void Grd_Initializer()
        {
            try
            {
                #region ROOM DETAILS
                grdHotel.Cols.Count = 27;
                grdHotel.Rows.Count = 800;
                grdHotel.Cols[(int)HG.gVNO].Width = 0;
                grdHotel.Cols[(int)HG.gBNO].Width = 0;
                grdHotel.Cols[(int)HG.gCHI].Width = 76;
                grdHotel.Cols[(int)HG.gCHO].Width = 76;
                grdHotel.Cols[(int)HG.gRTI].Width = 0;
                grdHotel.Cols[(int)HG.gRTN].Width = 110;
                grdHotel.Cols[(int)HG.gRBI].Width = 0;
                grdHotel.Cols[(int)HG.gRBN].Width = 108;
                grdHotel.Cols[(int)HG.gCID].Width = 0;
                grdHotel.Cols[(int)HG.gCON].Width = 0;
                grdHotel.Cols[(int)HG.gMID].Width = 0;
                grdHotel.Cols[(int)HG.gMAX].Width = 90;
                grdHotel.Cols[(int)HG.gEBD].Width = 0;
                grdHotel.Cols[(int)HG.gVAT].Width = 0;
                grdHotel.Cols[(int)HG.gTAX].Width = 49;
                grdHotel.Cols[(int)HG.gSCH].Width = 0;
                grdHotel.Cols[(int)HG.gTPR].Width = 0;
                grdHotel.Cols[(int)HG.gPRI].Width = 0;
                grdHotel.Cols[(int)HG.gNOR].Width = 57;
                grdHotel.Cols[(int)HG.gNGR].Width = 93;
                grdHotel.Cols[(int)HG.gTOT].Width = 00;
                grdHotel.Cols[(int)HG.gSEL].Width = 0;
                grdHotel.Cols[(int)HG.gLUP].Width = 0;
                grdHotel.Cols[(int)HG.gFOC].Width = 0;
                grdHotel.Cols[(int)HG.gMRC].Width = 80;
                grdHotel.Cols[(int)HG.gGRC].Width = 80;
                grdHotel.Cols[(int)HG.gMEC].Width = 0;
                grdHotel.Cols[(int)HG.gVNO].Caption = "Voucher No";
                grdHotel.Cols[(int)HG.gBNO].Caption = "Bill No";
                grdHotel.Cols[(int)HG.gCHI].Caption = "IN";
                grdHotel.Cols[(int)HG.gCHO].Caption = "OUT";
                grdHotel.Cols[(int)HG.gRTI].Caption = "Room Type ID";
                grdHotel.Cols[(int)HG.gRTN].Caption = "Room Type";
                grdHotel.Cols[(int)HG.gRBI].Caption = "Basis Type ID";
                grdHotel.Cols[(int)HG.gRBN].Caption = "Room Basis";
                grdHotel.Cols[(int)HG.gCID].Caption = "Condition ID";
                grdHotel.Cols[(int)HG.gCON].Caption = "Condition";
                grdHotel.Cols[(int)HG.gMID].Caption = "Occupancy ID";
                grdHotel.Cols[(int)HG.gMAX].Caption = "Occupancy";
                grdHotel.Cols[(int)HG.gEBD].Caption = "No Of Extra Bed";
                grdHotel.Cols[(int)HG.gVAT].Caption = "VAT %";
                grdHotel.Cols[(int)HG.gTAX].Caption = "Tax %";
                grdHotel.Cols[(int)HG.gSCH].Caption = "Service Charge %";
                grdHotel.Cols[(int)HG.gTPR].Caption = "Price With Tax";
                grdHotel.Cols[(int)HG.gPRI].Caption = "Price Without Tax";
                grdHotel.Cols[(int)HG.gNOR].Caption = "#Rooms";
                grdHotel.Cols[(int)HG.gNGR].Caption = "#Guide Rooms";
                grdHotel.Cols[(int)HG.gTOT].Caption = "Total";
                grdHotel.Cols[(int)HG.gSEL].Caption = "Choose";
                grdHotel.Cols[(int)HG.gLUP].Caption = "Last Used Price";
                grdHotel.Cols[(int)HG.gFOC].Caption = "No Of FOC Rooms";
                grdHotel.Cols[(int)HG.gMRC].Caption = "Guest Cost";// "Modified Room Cost";
                grdHotel.Cols[(int)HG.gGRC].Caption = "Guide Cost";// "Modified Guide Room Cost";
                grdHotel.Cols[(int)HG.gMEC].Caption = "Modified Ebed Cost";
                grdHotel.Cols[(int)HG.gVAT].Format = "##.##";
                grdHotel.Cols[(int)HG.gTAX].Format = "##.##";
                grdHotel.Cols[(int)HG.gSCH].Format = "##.##";
                grdHotel.Cols[(int)HG.gTPR].Format = "##.##";
                grdHotel.Cols[(int)HG.gPRI].Format = "##.##";
                grdHotel.Cols[(int)HG.gTOT].Format = "##.##";
                grdHotel.Cols[(int)HG.gNOR].Format = "##.##";
                grdHotel.Cols[(int)HG.gEBD].Format = "##.##";
                grdHotel.Cols[(int)HG.gLUP].Format = "##.##";
                grdHotel.Cols[(int)HG.gRTN].ComboList = "...";
                grdHotel.Cols[(int)HG.gRBN].ComboList = "...";
                grdHotel.Cols[(int)HG.gCON].ComboList = "...";
                grdHotel.Cols[(int)HG.gMAX].ComboList = "...";
                grdHotel.Cols[(int)HG.gCHI].DataType = Type.GetType("System.DateTime");
                grdHotel.Cols[(int)HG.gCHO].DataType = Type.GetType("System.DateTime");
                grdHotel.Cols[(int)HG.gSEL].DataType = Type.GetType("System.Boolean"); 
                grdHotel.Rows[1].AllowEditing = true;
                #endregion
                #region MEAL SUPPLEMENTS 
                grdMealSup.Cols.Count = 9;
                grdMealSup.Rows.Count = 4;
                grdMealSup.Cols[(int)MS.gMID].Width = 0;
                grdMealSup.Cols[(int)MS.gMTM].Width = 205;
                grdMealSup.Cols[(int)MS.gNOA].Width = 0;
                grdMealSup.Cols[(int)MS.gNOC].Width = 0;
                grdMealSup.Cols[(int)MS.gNOG].Width = 0;
                grdMealSup.Cols[(int)MS.gAMC].Width = 170;
                grdMealSup.Cols[(int)MS.gCMC].Width = 170;
                grdMealSup.Cols[(int)MS.gGMC].Width = 170;
                grdMealSup.Cols[(int)MS.gTOT].Width = 121;
                grdMealSup.Cols[(int)MS.gMID].Caption = "Meal ID";
                grdMealSup.Cols[(int)MS.gMTM].Caption = "Meal Name";
                grdMealSup.Cols[(int)MS.gNOA].Caption = "#Adult";
                grdMealSup.Cols[(int)MS.gNOC].Caption = "#Child";
                grdMealSup.Cols[(int)MS.gNOG].Caption = "#Guide";
                grdMealSup.Cols[(int)MS.gAMC].Caption = "Adult Cost";
                grdMealSup.Cols[(int)MS.gCMC].Caption = "Child Cost";
                grdMealSup.Cols[(int)MS.gGMC].Caption = "Guide Cost";
                grdMealSup.Cols[(int)MS.gTOT].Caption = "Total";
                grdMealSup.Cols[(int)MS.gNOA].Format = "##";
                grdMealSup.Cols[(int)MS.gNOC].Format = "##";
                grdMealSup.Cols[(int)MS.gNOG].Format = "##";
                grdMealSup.Cols[(int)MS.gAMC].Format = "##.##";
                grdMealSup.Cols[(int)MS.gCMC].Format = "##.##";
                grdMealSup.Cols[(int)MS.gGMC].Format = "##.##";
                grdMealSup.Cols[(int)MS.gTOT].Format = "##.##";
                grdMealSup.Cols[(int)MS.gMTM].ComboList = "...";
                grdMealSup.Rows[1].AllowEditing = true;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdHotel_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTRoom, DTBasis, DTOcc, DTNor, DTEbd, DTOthers ;
                string SqlQuery;
                #region LOAD ROOM TYPES DETAILS AS DROP DOWN LIST_____________________________
                DTRoom = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DISTINCT RoomTypeID,RoomTypeName FROM vwHotelReferance Where HotelID="+HID+" AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTRoom;
                if (e.Col == grdHotel.Cols[(int)HG.gRTN].Index)
                {
                    frm.SubForm = new Master.frmRoomTypes();
                    frm.Width = grdHotel.Cols[(int)HG.gRTN].Width;
                    frm.Height = grdHotel.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdHotel);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdHotel[grdHotel.Row, (int)HG.gRTI] = SelText[0];
                        grdHotel[grdHotel.Row, (int)HG.gRTN] = SelText[1];
                        grdHotel[grdHotel.Row, (int)HG.gRBI] = null;
                        grdHotel[grdHotel.Row, (int)HG.gRBN] = null;
                    }
                }
                #endregion
                #region LOAD BASIS TYPES DETAILS AS DROP DOWN LIST
                if (e.Col == grdHotel.Cols[(int)HG.gRBN].Index)
                {
                    if (grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gRTI].Index] != null)
                    {
                        int RoomTypeID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRTI].ToString());
                        SqlQuery = "SELECT DISTINCT BasisID,BasisTypeName FROM vwHotelReferance Where HotelID=" + HID + " AND RoomTypeID=" + RoomTypeID + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1";
                        DTBasis = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTBasis;
                        frm.SubForm = new Master.frmBasisTypes();
                        frm.Width = grdHotel.Cols[(int)HG.gRBN].Width;
                        frm.Height = grdHotel.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdHotel);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdHotel[grdHotel.Row, (int)HG.gRBI] = SelText[0];
                            grdHotel[grdHotel.Row, (int)HG.gRBN] = SelText[1];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Hotel Room Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                #endregion
                #region Commented
                #endregion
                #region LOAD HOTEL OCCUPANCY DETAILS AS DROP DOWN LIST
                if (e.Col == grdHotel.Cols[(int)HG.gMAX].Index)
                {
                    if (grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gRBI].Index] != null)
                    {
                        int RoomTypeID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRTI].ToString());
                        int BasisID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRBI].ToString());
                        SqlQuery = "SELECT DISTINCT OccupancyID,Occupancy FROM vwHotelReferance Where HotelID=" + HID + " AND RoomTypeID=" + RoomTypeID + " AND BasisID=" + BasisID + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1";
                        DTOcc = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTOcc;
                        frm.SubForm = new Master.frmHotelOccupancy();
                        frm.Width = grdHotel.Cols[(int)HG.gMID].Width;
                        frm.Height = grdHotel.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdHotel);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdHotel[grdHotel.Row, (int)HG.gMID] = SelText[0];
                            grdHotel[grdHotel.Row, (int)HG.gMAX] = SelText[1];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Hotel Basis Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                #endregion
                #region LOAD HOTEL NO OF ROOMS
                if (e.Col == grdHotel.Cols[(int)HG.gNOR].Index)
                {
                    if (grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gMID].Index] != null)
                    {
                        int RoomTypeID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRTI].ToString());
                        int BasisID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRBI].ToString());
                        int OccpID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gMID].ToString());
                        SqlQuery = "SELECT NoOfRooms,NoOfRooms FROM vwHotelReferance Where HotelID=" + HID + " AND RoomTypeID=" + RoomTypeID + " AND BasisID=" + BasisID + " AND OccupancyID="+OccpID+" AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1";
                        DTNor = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTNor;
                        frm.SubForm = new Master.frmBasisTypes();
                        frm.Width = grdHotel.Cols[(int)HG.gNOR].Width;
                        frm.Height = grdHotel.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdHotel);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdHotel[grdHotel.Row, (int)HG.gNOR] = SelText[0];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Hotel Occupancy", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                #endregion
                #region LOAD EXTRA BED
                if (e.Col == grdHotel.Cols[(int)HG.gEBD].Index)
                {
                    if (grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gNOR].Index] != null)
                    {
                        int RoomTypeID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRTI].ToString());
                        int BasisID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRBI].ToString());
                        int OccpID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gMID].ToString());
                        int Nor = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gNOR].ToString());
                        SqlQuery = "SELECT ExtraBed,CAST(Isnull(ExtraBed,0) AS INT)AS ExtraBed FROM vwHotelReferance Where HotelID=" + HID + " AND RoomTypeID=" + RoomTypeID + " AND BasisID=" + BasisID + " AND OccupancyID=" + OccpID + " AND NoOfRooms="+Nor+" AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1";
                        DTEbd = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTEbd;
                        frm.SubForm = new Master.frmBasisTypes();
                        frm.Width = grdHotel.Cols[(int)HG.gEBD].Width;
                        frm.Height = grdHotel.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdHotel);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdHotel[grdHotel.Row, (int)HG.gEBD] = SelText[1];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select No Of Rooms", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                #endregion
                #region LOAD OTHER DETAILS UNLESS BAISCS ARE BLANK
                if (grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gRTI].Index] != null && grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gRBI].Index] != null && grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gMID].Index] != null && grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gNOR].Index] != null && grdHotel[grdHotel.Row, grdHotel.Cols[(int)HG.gEBD].Index] != null)
                {
                    int RoomTypeID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRTI].ToString());
                    int BasisID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gRBI].ToString());
                    int OccpID = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gMID].ToString());
                    int Nor = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gNOR].ToString());
                    int Ebd = Convert.ToInt32(grdHotel[grdHotel.Row, (int)HG.gEBD].ToString());
                    SqlQuery = "SELECT Isnull(Vat,0)AS Vat,Isnull(Tax,0)AS Tax,Isnull(ServCharge,0)AS Serv,Isnull(Price,0)AS Cost FROM vwHotelReferance Where HotelID=" + HID + " AND RoomTypeID=" + RoomTypeID + " AND BasisID=" + BasisID + " AND OccupancyID=" + OccpID + " AND NoOfRooms=" + Nor + " AND ExtraBed=" + Ebd + " AND Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1";
                    DTOthers = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    if (DTOthers.Rows.Count > 0)
                    {
                        grdHotel[grdHotel.Row, (int)HG.gVAT] = DTOthers.Rows[0]["Vat"] + "".Trim();
                        grdHotel[grdHotel.Row, (int)HG.gTAX] = DTOthers.Rows[0]["Tax"] + "".Trim();
                        grdHotel[grdHotel.Row, (int)HG.gSCH] = DTOthers.Rows[0]["Serv"] + "".Trim();
                        grdHotel[grdHotel.Row, (int)HG.gPRI] = DTOthers.Rows[0]["Cost"] + "".Trim();
                    }
                }
                #endregion
                    grdMealSup.Rows.Count = 1;
                    grdMealSup.Rows.Count = 4;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace+ex.Message, msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void drpHotel_Selected_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string ssql;
                int RowNumb;
                DataTable DT;
                if (IsProcess)
                    grdHotel.Rows.Remove(ProcessRowNo);
                RowNumb = 1;
                while (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gRTI].Index] != null)
                {
                    grdHotel.Rows.Remove(RowNumb);
                } 
                grdHotel.Rows[1].AllowEditing = true;
                if (drpHotel.SelectedValue.ToString() == "")
                    return;
                ssql = "SELECT HotelID,RoomTypeID,RoomTypeName,BasisID,BasisTypeName,ConditionID,Condition," +
                       "OccupancyID,Occupancy,Vat,Tax,ServCharge,NoOfRooms" +
                       " FROM vwHotelReferance WHERE HotelID=" + drpHotel.SelectedValue.Trim() + " ORDER BY SrNo ";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdHotel[RowNumb + 1, (int)HG.gRTI] = Convert.ToInt16(DT.Rows[RowNumb]["RoomTypeID"].ToString());
                        grdHotel[RowNumb + 1, (int)HG.gRTN] = DT.Rows[RowNumb]["RoomTypeName"].ToString();
                        if (DT.Rows[RowNumb]["BasisID"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gRBI] = Convert.ToInt16(DT.Rows[RowNumb]["BasisID"].ToString());
                        if (DT.Rows[RowNumb]["BasisTypeName"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gRBN] = DT.Rows[RowNumb]["BasisTypeName"].ToString();
                        if (DT.Rows[RowNumb]["ConditionID"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gCID] = Convert.ToInt32(DT.Rows[RowNumb]["ConditionID"].ToString());
                        if (DT.Rows[RowNumb]["Condition"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gCON] = DT.Rows[RowNumb]["Condition"].ToString();
                        if (DT.Rows[RowNumb]["OccupancyID"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gMID] = DT.Rows[RowNumb]["OccupancyID"].ToString();
                        if (DT.Rows[RowNumb]["Occupancy"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gMAX] = DT.Rows[RowNumb]["Occupancy"].ToString();
                        if (DT.Rows[RowNumb]["Vat"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gVAT] = Convert.ToDecimal(DT.Rows[RowNumb]["Vat"].ToString());
                        if (DT.Rows[RowNumb]["Tax"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gTAX] = Convert.ToDecimal(DT.Rows[RowNumb]["Tax"].ToString());
                        if (DT.Rows[RowNumb]["ServCharge"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gSCH] = Convert.ToDecimal(DT.Rows[RowNumb]["ServCharge"].ToString());
                        if (DT.Rows[RowNumb]["NoOfRooms"].ToString() != "")
                            grdHotel[RowNumb + 1, (int)HG.gNOR] = Convert.ToInt16(DT.Rows[RowNumb]["NoOfRooms"].ToString());
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnAEGenerate_Click(object sender, EventArgs e)
        {
            if(Validate_Hotel_Expenses()==false)
                return ;
            Generate_Hotel_Expenses();
            Generate_Meal_Supplement_Expenses();
        }
        private Boolean Validate_Hotel_Expenses()
        {   
                int RowNumb = 1;
                while (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gRTI].Index] != null)
                {
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gVAT].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdHotel[RowNumb, (int)HG.gVAT].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Vat", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gTAX].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdHotel[RowNumb, (int)HG.gTAX].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Tax", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gSCH].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdHotel[RowNumb, (int)HG.gSCH].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Service Chargers", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gPRI].Index] != null)
                    {
                        if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdHotel[RowNumb, (int)HG.gPRI].ToString()) == false)
                        {
                            MessageBox.Show("Please Enter Valid Values For Amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                    RowNumb++;
                }
                return true; 
        }
        private void Generate_Hotel_Expenses()
        {
            try
            {
                double Amt = 0.00, Amt1 = 0.00;
                double TotAmt = 0.00, TotTax = 0.00, TotVat = 0.00, TotServ = 0.00, NetAmt = 0.00;
                double vat = 0.00, tax = 0.00, serv = 0.00;
                double vat1 = 0.00, tax1 = 0.00, serv1 = 0.00;
                int RowNumb = 1, Count=1;
                int NOR=0;
                if(IsProcess)
                    grdHotel.Rows.Remove(ProcessRowNo);
                if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gRTI].Index] == null)
                {
                    MessageBox.Show("No Records Found To Be Processed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                int MaxVal = 0;
                while (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gRTI].Index] != null)
                {
                        MaxVal++;
                    RowNumb++; 
                }
                pbHE.Maximum = MaxVal;
                RowNumb = 1;
                while (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gRTI].Index] != null)
                {
                    C1.Win.C1FlexGrid.CellStyle rs1 = grdHotel.Styles.Add("RowColor");
                    rs1.BackColor = Color.White;
                    grdHotel.Rows[RowNumb].Style = grdHotel.Styles["RowColor"];
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gTOT].Index] == null)
                    {
                        grdHotel[RowNumb, (int)HG.gTOT] = "0.00";
                    }
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gVAT].Index] == null)
                    {
                        vat = 0.00;
                    }
                    else
                    {
                        vat = Convert.ToDouble(grdHotel[RowNumb, (int)HG.gVAT].ToString());
                    }
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gTAX].Index] == null)
                    {
                        tax = 0.00;
                    }
                    else
                    {
                        tax = Convert.ToDouble(grdHotel[RowNumb, (int)HG.gTAX].ToString());
                    }
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gSCH].Index] == null)
                    {
                        serv = 0.00;
                    }
                    else
                    {
                        serv = Convert.ToDouble(grdHotel[RowNumb, (int)HG.gSCH].ToString());
                    }
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gPRI].Index] == null)
                    {
                        Amt1 = 0.00;
                    }
                    else
                    {
                        Amt1 = Convert.ToDouble(grdHotel[RowNumb, (int)HG.gPRI].ToString());
                    }
                    int NOR1 = 0;
                    if (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gNOR].Index] == null)
                    {
                        NOR1 = 0;
                    }
                    else
                    {
                        NOR1 = Convert.ToInt32(grdHotel[RowNumb, (int)HG.gNOR].ToString());
                    }
                    NOR = NOR + NOR1;
                    vat1 = (vat / 100) * Amt1;
                    tax1 = (tax / 100) * Amt1;
                    serv1 = (serv / 100) * Amt1;
                    Amt = Convert.ToDouble(grdHotel[RowNumb, (int)HG.gPRI].ToString()) + (vat1 + tax1 + serv1);
                    grdHotel[RowNumb, (int)HG.gTOT] = Amt.ToString();
                    TotVat = TotVat + vat;
                    TotTax = TotTax + tax;
                    TotServ = TotServ + serv;
                    TotAmt = TotAmt + Amt1;
                    NetAmt = NetAmt + Amt;
                    pbHE.Value = Count;
                    Count++;
                    RowNumb++;
                }
                grdHotel[RowNumb + 4, (int)HG.gRTN] = "TOTAL COST";
                grdHotel[RowNumb + 4, (int)HG.gVAT] = TotVat.ToString();
                grdHotel[RowNumb + 4, (int)HG.gTAX] = TotTax.ToString();
                grdHotel[RowNumb + 4, (int)HG.gSCH] = TotServ.ToString();
                grdHotel[RowNumb + 4, (int)HG.gNOR] = NOR.ToString();
                grdHotel[RowNumb + 4, (int)HG.gPRI] = TotAmt.ToString();
                grdHotel[RowNumb + 4, (int)HG.gTOT] = NetAmt.ToString();
                C1.Win.C1FlexGrid.CellStyle rs2 = grdHotel.Styles.Add("TotalColor");
                rs2.BackColor = Color.PowderBlue;
                grdHotel.Rows[RowNumb + 4].Style = grdHotel.Styles["TotalColor"];
                IsProcess = true;
                ProcessRowNo = RowNumb + 4;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Generate_Meal_Supplement_Expenses()
        {
            try
            {
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Grid()
        {
            try
            {               
                int[] RowsToBeRemoved = new int[6];
                grdHotel.Rows.Count = 1;
                grdHotel.Rows.Count = 800;
                #region LOAD HOTEL DETAILS
                int count = 0;
                while (Occ[count] != null)
                {
                    int MarketID=0;
                    if(cmbMarket.SelectedValue+"".Trim()!="")
                        MarketID= Convert.ToInt16(cmbMarket.SelectedValue.ToString().Trim());
                    string sql;
                    DateTime dateNow_D;
                    dateNow_D = Classes.clsGlobal.CurDate();
                    string dateNow = String.Format("{0:yyyy-MM-dd}",dateNow_D);
                    sql = "SELECT Name FROM dbo.stt_HotelRates WHERE IsActive=1";
                    string Col = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql).Rows[0]["Name"].ToString().Trim();
                    DataTable DT1;
                    switch (Col)
                    {
                        case "Market Wise":
                            DT1 = Classes.clsGlobal.rates_Market_Wise(HID, MarketID, Convert.ToInt32(Occ[count]), 0, 0);
                            break;
                        case "Season Wise":
                            DT1 = Classes.clsGlobal.rates_Season_Wise(HID, dateNow, 0, 0, Convert.ToInt32(Occ[count]));
                            break;
                        case "Both":
                            DT1 = Classes.clsGlobal.rates_MarketAndSeason_Wise(HID, MarketID, dateNow, 0, 0, Convert.ToInt32(Occ[count]));
                            break;
                        default:
                            DT1 = Classes.clsGlobal.rates_Just_Hotel(HID, 0, 0, Convert.ToInt32(Occ[count]));
                            break;
                    }
                    grdHotel[count + 1, (int)HG.gCHI] = InDate;
                    grdHotel[count + 1, (int)HG.gCHO] = OutDate;
                    int SeasonID = 0;
                    if (DT1.Rows.Count > 0)
                    {
                        #region SEASONS
                        if (DT1.Rows[0]["SeasonID"] + "" != "")
                            SeasonID = Convert.ToInt32(DT1.Rows[0]["SeasonID"]);
                        if (DT1.Rows[0]["Season"] + "" != "")
                            lblSeasonName.Text = DT1.Rows[0]["Season"].ToString().Trim();
                        if (DT1.Rows[0]["SeasonFrom"] + "" != "")
                            lblFromDate.Text = String.Format("{0:yyyy-MM-dd}",Convert.ToDateTime(DT1.Rows[0]["SeasonFrom"]));
                        if (DT1.Rows[0]["SeasonTo"] + "" != "")
                           lblToDate.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(DT1.Rows[0]["SeasonTo"]));
                        #endregion
                        #region ROOM DETAILS
                        grdHotel[count + 1, (int)HG.gVNO] = VoucherNo.ToString();
                        grdHotel[count + 1, (int)HG.gRTI] = Convert.ToInt16(DT1.Rows[0]["RoomTypeID"].ToString());
                        grdHotel[count + 1, (int)HG.gRTN] = DT1.Rows[0]["RoomTypeName"].ToString();
                        if (DT1.Rows[0]["BasisID"].ToString() != "")
                            grdHotel[count + 1, (int)HG.gRBI] = Convert.ToInt16(DT1.Rows[0]["BasisID"].ToString());
                        if (DT1.Rows[0]["BasisTypeName"].ToString() != "")
                            grdHotel[count + 1, (int)HG.gRBN] = DT1.Rows[0]["BasisTypeName"].ToString();
                        if (DT1.Rows[0]["OccupancyID"].ToString() != "")
                            grdHotel[count + 1, (int)HG.gMID] = DT1.Rows[0]["OccupancyID"].ToString();
                        if (DT1.Rows[0]["Occupancy"].ToString() != "")
                            grdHotel[count + 1, (int)HG.gMAX] = DT1.Rows[0]["Occupancy"].ToString();
                        grdHotel[count + 1, (int)HG.gNOR] = Rmm[count].ToString();
                        #endregion
                        #region COST DETAILS
                        if (DT1.Rows[0]["Tax"].ToString() != "")//Price With Tax
                            grdHotel[count + 1, (int)HG.gTAX] = DT1.Rows[0]["Tax"].ToString();
                        if (DT1.Rows[0]["PriceWithoutTax"].ToString() != "")//Price With Tax
                            grdHotel[count + 1, (int)HG.gMRC] = DT1.Rows[0]["PriceWithoutTax"].ToString();
                        if (DT1.Rows[0]["GuideRoomCost"].ToString() != "")//Price With Tax
                            grdHotel[count + 1, (int)HG.gGRC] = DT1.Rows[0]["GuideRoomCost"].ToString();
                        #endregion
                    }
                    #region COMMENT
                    #endregion
                    count++;
                }
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Info()
        {
            try
            {
                int[] RowsToBeRemoved = new int[6];
                int row = 1;
                while (grdHotel[row, (int)HG.gRTI] + "".Trim() != "")
                {
                    grdHotel[row, (int)HG.gCHI] = InDate;
                    grdHotel[row, (int)HG.gCHO] = OutDate;
                    row++;
                }                
                #region LOAD HOTEL DETAILS
                int MarketID = 0;
                if (cmbMarket.SelectedValue + "".Trim() != "")
                   MarketID = Convert.ToInt16(cmbMarket.SelectedValue.ToString().Trim());
                string sql;
                DateTime dateNow_D;
                dateNow_D = Classes.clsGlobal.CurDate();
                string dateNow = String.Format("{0:yyyy-MM-dd}", dateNow_D);
                sql = "SELECT Name FROM dbo.stt_HotelRates WHERE IsActive=1";
                string Col = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql).Rows[0]["Name"].ToString().Trim();
                DataTable DT1;
                switch (Col)
                {
                    case "Market Wise":
                        DT1 = Classes.clsGlobal.rates_Market_Wise(HID, MarketID,Convert.ToInt32(Occ[0]), 0, 0);
                        break;
                    case "Season Wise":
                        DT1 = Classes.clsGlobal.rates_Season_Wise(HID, dateNow,0,0,Convert.ToInt32(Occ[0]));
                        break;
                    case "Both":
                        DT1 = Classes.clsGlobal.rates_MarketAndSeason_Wise(HID, MarketID,dateNow,0,0,Convert.ToInt32(Occ[0]));
                        break;
                    default :
                        DT1 = Classes.clsGlobal.rates_Just_Hotel(HID, 0, 0, Convert.ToInt32(Occ[0]));
                        break;
                }
                    int SeasonID = 0;
                    if (DT1.Rows.Count > 0)
                    {
                        #region SEASONS
                        if (DT1.Rows[0]["SeasonID"] + "" != "")
                            SeasonID = Convert.ToInt32(DT1.Rows[0]["SeasonID"]);
                        if (DT1.Rows[0]["Season"] + "" != "")
                            lblSeasonName.Text = DT1.Rows[0]["Season"].ToString().Trim();
                        if (DT1.Rows[0]["SeasonFrom"] + "" != "")
                            lblFromDate.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(DT1.Rows[0]["SeasonFrom"]));
                        if (DT1.Rows[0]["SeasonTo"] + "" != "")
                            lblToDate.Text = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(DT1.Rows[0]["SeasonTo"]));
                        #endregion                      
                    }                                      
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void set_Ocuupancies()
        {
            try
            {                
                int i = 0;
                if (singles > 0)
                {
                    Occ[i] = "1001";
                    Rmm[i] = singles;
                    i++;
                }
                if (doubles > 0)
                {
                    Occ[i] = "1003";
                    Rmm[i] = doubles;
                    i++;
                }
                if (triples > 0)
                {
                    Occ[i] = "1002";
                    Rmm[i] = triples;
                    i++;
                }
                if (twin > 0)
                {
                    Occ[i] = "1005";
                    Rmm[i] = twin;
                    i++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdMealSup_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable   DTMealMode;
            string SqlQuery;
            #region COMMENTED
            #endregion
            if (e.Col == grdMealSup.Cols[(int)MS.gMTM].Index)
            {                
                SqlQuery = "SELECT ID,Name FROM mst_MealTime Where ISNULL(IsActive,0)<>0";
                DTMealMode = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                if (DTMealMode.Rows.Count == 0)
                    return;
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTMealMode;
                frm.Width = grdMealSup.Cols[(int)MS.gMTM].Width;
                frm.Height = grdMealSup.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdMealSup);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdMealSup[grdMealSup.Row, (int)MS.gMID] = SelText[0];
                    grdMealSup[grdMealSup.Row, (int)MS.gMTM] = SelText[1];
                }
            }
            grdHotel.Rows.Count = 1;
            grdHotel.Rows.Count = 800;
        }
        private void grdMealSup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdMealSup.Rows.Remove(grdMealSup.Row);
                grdMealSup.Rows[1].AllowEditing = true;
            }
        }
        private void grdMealSup_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)
            {
                return;
            }
            if (grdMealSup.Rows.Count < 3)
            {
                return;
            }
            if (grdMealSup[grdMealSup.Row - 1, 0] == null)
            {
                grdMealSup.Rows[grdMealSup.Row].AllowEditing = false;
            }
            else
            {
                grdMealSup.Rows[grdMealSup.Row].AllowEditing = true;
            }
        }
        private void Hotel_Season()
        {
            try
            {
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void cmbMarket_SelectedValueChanged(object sender, EventArgs e)
        {
            if(IsLoaded)
                Fill_Grid();
        }
        private void gbHotelDetails_Enter(object sender, EventArgs e)
        {
        }
        private void bntCopyLastRow_Click(object sender, EventArgs e)
        { 
                int RowNumb = 1;
                while (grdHotel[RowNumb, grdHotel.Cols[(int)HG.gRTI].Index] != null)  RowNumb++; 
                if (RowNumb == 1 || RowNumb == grdHotel.Rows.Count)   return;
                grdHotel[RowNumb, (int)HG.gVNO] = grdHotel[RowNumb - 1, (int)HG.gVNO];
                grdHotel[RowNumb, (int)HG.gBNO] = grdHotel[RowNumb - 1, (int)HG.gBNO];
                grdHotel[RowNumb, (int)HG.gCHI] = grdHotel[RowNumb - 1, (int)HG.gCHI];
                grdHotel[RowNumb, (int)HG.gCHO] = grdHotel[RowNumb - 1, (int)HG.gCHO];
                grdHotel[RowNumb, (int)HG.gRTI] = grdHotel[RowNumb - 1, (int)HG.gRTI];
                grdHotel[RowNumb, (int)HG.gRTN] = grdHotel[RowNumb - 1, (int)HG.gRTN];
                grdHotel[RowNumb, (int)HG.gRBI] = grdHotel[RowNumb - 1, (int)HG.gRBI];
                grdHotel[RowNumb, (int)HG.gRBN] = grdHotel[RowNumb - 1, (int)HG.gRBN];
                grdHotel[RowNumb, (int)HG.gCID] = grdHotel[RowNumb - 1, (int)HG.gCID];
                grdHotel[RowNumb, (int)HG.gCON] = grdHotel[RowNumb - 1, (int)HG.gCON];
                grdHotel[RowNumb, (int)HG.gMID] = grdHotel[RowNumb - 1, (int)HG.gMID];
                grdHotel[RowNumb, (int)HG.gMAX] = grdHotel[RowNumb - 1, (int)HG.gMAX];
                grdHotel[RowNumb, (int)HG.gEBD] = grdHotel[RowNumb - 1, (int)HG.gEBD];
                grdHotel[RowNumb, (int)HG.gNOR] = grdHotel[RowNumb - 1, (int)HG.gNOR];
                grdHotel[RowNumb, (int)HG.gNGR] = grdHotel[RowNumb - 1, (int)HG.gNGR];
                grdHotel[RowNumb, (int)HG.gSEL] = grdHotel[RowNumb - 1, (int)HG.gSEL];
                grdHotel[RowNumb, (int)HG.gFOC] = grdHotel[RowNumb - 1, (int)HG.gFOC]; 
        }
        private void btnChangeDates_Click(object sender, EventArgs e)
        { 
                int row = 1;
                while (grdHotel[row, (int)HG.gRTI] + "".Trim() != "")
                {
                    grdHotel[row, (int)HG.gCHI] = InDate;
                    grdHotel[row, (int)HG.gCHO] = OutDate;
                    row++;
                } 
        }
    }
}
