using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Reports
{
    public partial class frmCheckCancellation : Form
    {
        enum DR {gID,gTURID,gGUST,gAGTID,gAGTNM,gDPDTE,gARDTE,gDTEIN,gDTEOUT,gHTLID,gHTLNM,gCNFBY,gCOFCOD,gTYPE,gBASIS,gOCUPN,gRMNUM,gHNDLID,gHNDBY,gDTEDUE  };
        String ssql;
        String ssql1;
        String ssql2;
        int RowNumb;
        int RowNumbAry;
        List<int> dateRange = new List<int>();
        List<String> colorDetails = new List<String>();
        C1.Win.C1FlexGrid.CellStyle Aqua;
        C1.Win.C1FlexGrid.CellStyle Blue;
        C1.Win.C1FlexGrid.CellStyle Brown;
        C1.Win.C1FlexGrid.CellStyle Cyan;
        C1.Win.C1FlexGrid.CellStyle Green;
        C1.Win.C1FlexGrid.CellStyle Gray;
        C1.Win.C1FlexGrid.CellStyle Indigo;
        C1.Win.C1FlexGrid.CellStyle Red;
        C1.Win.C1FlexGrid.CellStyle Yellow;
        C1.Win.C1FlexGrid.CellStyle Purple;
        private const string msghd = "Tour Cancel Details";
        public frmCheckCancellation(){InitializeComponent();}
        private void frmCheckCanselation_Load(object sender, EventArgs e)
        {
            Initializer();
            filldata();
        }
        public void Initializer()
        {
            gridInitializer();
            colorInitializer();
        }
        public  void colorInitializer()
        {
            Aqua = grdCancel.Styles.Add("Aqua");
            Blue = grdCancel.Styles.Add("Blue");
            Brown = grdCancel.Styles.Add("Brown");
            Cyan = grdCancel.Styles.Add("Cyan");
            Green = grdCancel.Styles.Add("Green");
            Gray = grdCancel.Styles.Add("Gray");
            Indigo = grdCancel.Styles.Add("Indigo");
            Red = grdCancel.Styles.Add("Red");
            Yellow = grdCancel.Styles.Add("Yellow");
            Purple = grdCancel.Styles.Add("Purple");
            Aqua.BackColor = Color.Aqua;
            Blue.BackColor = Color.Blue;
            Brown.BackColor = Color.Brown;
            Cyan.BackColor = Color.Cyan;
            Green.BackColor = Color.Green;
            Gray.BackColor = Color.Gray;
            Indigo.BackColor = Color.Indigo;
            Red.BackColor = Color.Red;
            Yellow.BackColor = Color.Yellow;
            Purple.BackColor = Color.Purple;
        }
        public void filldata()
        {
            ssql1 = "Select [from],[to],Colour from vw_CancelByDay ORDER BY [to] ";
            DataTable DTARY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql1);
            ssql2 = "SELECT [From],[To] ,[Colour] FROM [TouristManagement].[dbo].[vw_CancelByDate]";
            DataTable DAYRY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql2);
        if (DTARY.Rows.Count > 0)
        {    RowNumbAry = 0;
             while (DTARY.Rows.Count > RowNumbAry)
             {
                 dateRange.Add(Int32.Parse(DTARY.Rows[RowNumbAry]["to"].ToString()) + Int32.Parse(DTARY.Rows[RowNumbAry]["from"].ToString()));
                 colorDetails.Add(DTARY.Rows[RowNumbAry]["Colour"].ToString());
                 RowNumbAry++;
             }
        }
            ssql = "SELECT  [ID],TourID , Guest,  AgentID ,AgentName, DateArrival, DateDeparture ,DateIn, Dateout," +
                   "HotelID, HotelName,  ConfirmBy , ConfirmationCode,  RoomTypeName,  RoomBasisName , Occupancy,"+
                   "NoOfRooms,  HandledByID,  HandledBy " +
                   "FROM [TouristManagement].[dbo].[vw_trn_rpt_TourSummary] where DateIn is not null and DateIn between (SELECT CURRENT_TIMESTAMP) AND DATEADD(day,100,(SELECT CURRENT_TIMESTAMP)) ORDER BY DateIn";
            #region Fill Grid
            DataTable DTCBDY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DTCBDY.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DTCBDY.Rows.Count > RowNumb)
                {
                    grdCancel[RowNumb + 1, (int)DR.gID] = DTCBDY.Rows[RowNumb]    ["ID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTURID] = DTCBDY.Rows[RowNumb] ["TourID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gGUST] = DTCBDY.Rows[RowNumb]  ["Guest"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTID] = DTCBDY.Rows[RowNumb] ["AgentID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTNM] = DTCBDY.Rows[RowNumb] ["AgentName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gARDTE] = DTCBDY.Rows[RowNumb] ["DateArrival"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDPDTE] = DTCBDY.Rows[RowNumb] ["DateDeparture"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEIN] = DTCBDY.Rows[RowNumb] ["DateIn"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEOUT] = DTCBDY.Rows[RowNumb ]["Dateout"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLID] = DTCBDY.Rows[RowNumb] ["HotelID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLNM] = DTCBDY.Rows[RowNumb] ["HotelName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCNFBY] = DTCBDY.Rows[RowNumb] ["ConfirmBy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCOFCOD] = DTCBDY.Rows[RowNumb]["ConfirmationCode"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTYPE] = DTCBDY.Rows[RowNumb]  ["RoomTypeName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gBASIS] = DTCBDY.Rows[RowNumb] ["RoomBasisName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gOCUPN] = DTCBDY.Rows[RowNumb] ["Occupancy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gRMNUM] = DTCBDY.Rows[RowNumb] ["NoOfRooms"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDLID] = DTCBDY.Rows[RowNumb]["HandledByID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDBY] = DTCBDY.Rows[RowNumb] ["HandledBy"].ToString();
                    RowNumbAry = 0;
                    while (DTARY.Rows.Count > RowNumbAry)
                    {
                        if (Int32.Parse(DTCBDY.Rows[RowNumb]["Duration"].ToString()) <= dateRange[RowNumbAry])
                        {
                            grdCancel.Rows[RowNumb + 1].Style = grdCancel.Styles[colorDetails[RowNumbAry]];
                            break;
                        }
                        RowNumbAry++;
                    }
                    RowNumbAry = 0;
                    while (DAYRY.Rows.Count > RowNumbAry)
                    {
                        if (DateTime.Parse(DAYRY.Rows[RowNumbAry]["From"].ToString()) <= DateTime.Parse(DTCBDY.Rows[RowNumb]["DateIn"].ToString()) && DateTime.Parse(DTCBDY.Rows[RowNumb]["DateIn"].ToString()) <= DateTime.Parse(DAYRY.Rows[RowNumbAry]["To"].ToString()))
                        {
                            grdCancel.Rows[RowNumb + 1].Style = grdCancel.Styles[DAYRY.Rows[RowNumbAry]["Colour"].ToString()];
                            break;
                        }
                        RowNumbAry++;
                    }
                    RowNumb++;
                }
            }
            #endregion
        }
        public void gridInitializer()
        {
            #region Grid Cancel Initialize
            grdCancel.Cols.Count =20;
            grdCancel.Rows.Count = 5000;
            grdCancel.Cols[(int)DR.gID].Width = 0;
            grdCancel.Cols[(int)DR.gTURID].Width = 80;
            grdCancel.Cols[(int)DR.gGUST].Width = 80;
            grdCancel.Cols[(int)DR.gAGTID].Width = 0;
            grdCancel.Cols[(int)DR.gAGTNM].Width = 80;
            grdCancel.Cols[(int)DR.gARDTE].Width = 0;
            grdCancel.Cols[(int)DR.gDPDTE].Width = 0;
            grdCancel.Cols[(int)DR.gDTEIN].Width = 80;
            grdCancel.Cols[(int)DR.gDTEOUT].Width =0;
            grdCancel.Cols[(int)DR.gHTLID].Width = 0;
            grdCancel.Cols[(int)DR.gHTLNM].Width = 80;
            grdCancel.Cols[(int)DR.gCNFBY].Width = 80;
            grdCancel.Cols[(int)DR.gCOFCOD].Width = 80;
            grdCancel.Cols[(int)DR.gTYPE].Width = 80;
            grdCancel.Cols[(int)DR.gBASIS].Width = 80;
            grdCancel.Cols[(int)DR.gOCUPN].Width = 80;
            grdCancel.Cols[(int)DR.gRMNUM].Width = 80;
            grdCancel.Cols[(int)DR.gHNDLID].Width =0;
            grdCancel.Cols[(int)DR.gHNDBY].Width = 80;
            grdCancel.Cols[(int)DR.gDTEDUE].Width = 80;
            grdCancel.Cols[(int)DR.gID].Caption     = "ID";
            grdCancel.Cols[(int)DR.gTURID].Caption  = "Tour ID";
            grdCancel.Cols[(int)DR.gGUST].Caption   = "Guest";
            grdCancel.Cols[(int)DR.gAGTID].Caption  = "Agent No";
            grdCancel.Cols[(int)DR.gAGTNM].Caption  = "Agent";
            grdCancel.Cols[(int)DR.gARDTE].Caption  = "Date Arrival";
            grdCancel.Cols[(int)DR.gDPDTE].Caption  = "Date Departure";
            grdCancel.Cols[(int)DR.gDTEIN].Caption  = "Date In";
            grdCancel.Cols[(int)DR.gDTEOUT].Caption = "Date Out";
            grdCancel.Cols[(int)DR.gHTLID].Caption  = "Hotel ID";
            grdCancel.Cols[(int)DR.gHTLNM].Caption  = "Hotel Name";
            grdCancel.Cols[(int)DR.gCNFBY].Caption  = "Confirm By";
            grdCancel.Cols[(int)DR.gCOFCOD].Caption = "Confirm Code";
            grdCancel.Cols[(int)DR.gTYPE].Caption   = "Type";
            grdCancel.Cols[(int)DR.gBASIS].Caption  = "Basis";
            grdCancel.Cols[(int)DR.gOCUPN].Caption  = "Occupancy";
            grdCancel.Cols[(int)DR.gRMNUM].Caption  = "Number of Rooms";
            grdCancel.Cols[(int)DR.gHNDLID].Caption = "Handled by ID";
            grdCancel.Cols[(int)DR.gHNDBY].Caption  = "Handeled by";
            grdCancel.Cols[(int)DR.gDTEDUE].Caption = "Duration";
            #endregion
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            grdCancel.Cols[(int)DR.gCNFBY].Width = 80;
            grdCancel.Cols[(int)DR.gCOFCOD].Width = 80;
            grdCancel.Rows.Count = 1;
            grdCancel.Rows.Count = 5000;
            ssql = "SELECT  [ID],TourID , Guest,  AgentID ,AgentName, DateArrival, DateDeparture ,DateIn, Dateout," +
                   "HotelID, HotelName,  ConfirmBy , ConfirmationCode,  RoomTypeName,  RoomBasisName , Occupancy," +
                   "NoOfRooms,  HandledByID,  HandledBy ,Duration " +
                   "FROM [TouristManagement].[dbo].[vw_trn_rpt_TourSummary] where DateIn is not null and DateIn between"+
                   " (SELECT CURRENT_TIMESTAMP) AND DATEADD(day,20,(SELECT CURRENT_TIMESTAMP)) ORDER BY DateIn";
            #region Fill Grid with all values
            DataTable DTCBDY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DTCBDY.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DTCBDY.Rows.Count > RowNumb)
                {
                    grdCancel[RowNumb + 1, (int)DR.gID] = DTCBDY.Rows[RowNumb]["ID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTURID] = DTCBDY.Rows[RowNumb]["TourID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gGUST] = DTCBDY.Rows[RowNumb]["Guest"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTID] = DTCBDY.Rows[RowNumb]["AgentID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTNM] = DTCBDY.Rows[RowNumb]["AgentName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gARDTE] = DTCBDY.Rows[RowNumb]["DateArrival"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDPDTE] = DTCBDY.Rows[RowNumb]["DateDeparture"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEIN] = DTCBDY.Rows[RowNumb]["DateIn"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEOUT] = DTCBDY.Rows[RowNumb]["Dateout"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLID] = DTCBDY.Rows[RowNumb]["HotelID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLNM] = DTCBDY.Rows[RowNumb]["HotelName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCNFBY] = DTCBDY.Rows[RowNumb]["ConfirmBy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCOFCOD] = DTCBDY.Rows[RowNumb]["ConfirmationCode"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTYPE] = DTCBDY.Rows[RowNumb]["RoomTypeName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gBASIS] = DTCBDY.Rows[RowNumb]["RoomBasisName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gOCUPN] = DTCBDY.Rows[RowNumb]["Occupancy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gRMNUM] = DTCBDY.Rows[RowNumb]["NoOfRooms"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDLID] = DTCBDY.Rows[RowNumb]["HandledByID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDBY] = DTCBDY.Rows[RowNumb]["HandledBy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEDUE] = DTCBDY.Rows[RowNumb]["Duration"].ToString();
                    RowNumb++;
                }
            }
            #endregion
        }
        private void rdbConf_CheckedChanged(object sender, EventArgs e)
        {
            grdCancel.Cols[(int)DR.gCNFBY].Width = 80;
            grdCancel.Cols[(int)DR.gCOFCOD].Width = 80;
            grdCancel.Rows.Count = 1;
            grdCancel.Rows.Count = 5000;
            ssql = "SELECT TOP 1000  [ID],TourID , Guest,  AgentID ,AgentName, DateArrival, DateDeparture ,DateIn, Dateout," +
                   "HotelID, HotelName,  ConfirmBy ,  ConfirmationCode,  RoomTypeName,  RoomBasisName , Occupancy," +
                   "NoOfRooms,  HandledByID,  HandledBy, [Duration]" +
                   "FROM [TouristManagement].[dbo].[vw_trn_rpt_TourSummary] where ConfirmationCode IS NOT NULL AND ConfirmationCode IS NOT NULL "
                   +"and DateIn between (SELECT CURRENT_TIMESTAMP) AND DATEADD(day,20,(SELECT CURRENT_TIMESTAMP)) ORDER BY DateIn ";
            #region Fill Grid with confirm values
            DataTable DTCBDY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DTCBDY.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DTCBDY.Rows.Count > RowNumb)
                {
                    grdCancel[RowNumb + 1, (int)DR.gID] = DTCBDY.Rows[RowNumb]["ID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTURID] = DTCBDY.Rows[RowNumb]["TourID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gGUST] = DTCBDY.Rows[RowNumb]["Guest"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTID] = DTCBDY.Rows[RowNumb]["AgentID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTNM] = DTCBDY.Rows[RowNumb]["AgentName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gARDTE] = DTCBDY.Rows[RowNumb]["DateArrival"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDPDTE] = DTCBDY.Rows[RowNumb]["DateDeparture"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEIN] = DTCBDY.Rows[RowNumb]["DateIn"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEOUT] = DTCBDY.Rows[RowNumb]["Dateout"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLID] = DTCBDY.Rows[RowNumb]["HotelID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLNM] = DTCBDY.Rows[RowNumb]["HotelName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCNFBY] = DTCBDY.Rows[RowNumb]["ConfirmBy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCOFCOD] = DTCBDY.Rows[RowNumb]["ConfirmationCode"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTYPE] = DTCBDY.Rows[RowNumb]["RoomTypeName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gBASIS] = DTCBDY.Rows[RowNumb]["RoomBasisName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gOCUPN] = DTCBDY.Rows[RowNumb]["Occupancy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gRMNUM] = DTCBDY.Rows[RowNumb]["NoOfRooms"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDLID] = DTCBDY.Rows[RowNumb]["HandledByID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDBY] = DTCBDY.Rows[RowNumb]["HandledBy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEDUE] = DTCBDY.Rows[RowNumb]["Duration"].ToString();
                    RowNumb++;
                }
            }
            #endregion
        }
        private void rdbNonConfr_CheckedChanged(object sender, EventArgs e)
        {
            grdCancel.Cols[(int)DR.gCNFBY].Width = 0;
            grdCancel.Cols[(int)DR.gCOFCOD].Width = 0;
            grdCancel.Rows.Count = 1;
            grdCancel.Rows.Count = 5000;
            ssql = "SELECT TOP 1000  [ID],TourID , Guest,  AgentID ,AgentName, DateArrival, DateDeparture ,DateIn, Dateout," +
                   "HotelID, HotelName,  ConfirmBy , ConfirmationCode,  RoomTypeName,  RoomBasisName , Occupancy," +
                   "NoOfRooms,  HandledByID,  HandledBy,[Duration]  " +
                   "FROM [TouristManagement].[dbo].[vw_trn_rpt_TourSummary] where ConfirmationCode IS NULL and DateIn IS NOT NULL"
                   +" and DateIn between (SELECT CURRENT_TIMESTAMP) AND DATEADD(day,20,(SELECT CURRENT_TIMESTAMP)) ORDER BY DateIn";
            #region Fill Grid with non-confirm values
            DataTable DTCBDY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DTCBDY.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DTCBDY.Rows.Count > RowNumb)
                {
                    grdCancel[RowNumb + 1, (int)DR.gID] = DTCBDY.Rows[RowNumb]["ID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTURID] = DTCBDY.Rows[RowNumb]["TourID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gGUST] = DTCBDY.Rows[RowNumb]["Guest"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTID] = DTCBDY.Rows[RowNumb]["AgentID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gAGTNM] = DTCBDY.Rows[RowNumb]["AgentName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gARDTE] = DTCBDY.Rows[RowNumb]["DateArrival"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDPDTE] = DTCBDY.Rows[RowNumb]["DateDeparture"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEIN] = DTCBDY.Rows[RowNumb]["DateIn"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEOUT] = DTCBDY.Rows[RowNumb]["Dateout"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLID] = DTCBDY.Rows[RowNumb]["HotelID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHTLNM] = DTCBDY.Rows[RowNumb]["HotelName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCNFBY] = DTCBDY.Rows[RowNumb]["ConfirmBy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gCOFCOD] = DTCBDY.Rows[RowNumb]["ConfirmationCode"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gTYPE] = DTCBDY.Rows[RowNumb]["RoomTypeName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gBASIS] = DTCBDY.Rows[RowNumb]["RoomBasisName"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gOCUPN] = DTCBDY.Rows[RowNumb]["Occupancy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gRMNUM] = DTCBDY.Rows[RowNumb]["NoOfRooms"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDLID] = DTCBDY.Rows[RowNumb]["HandledByID"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gHNDBY] = DTCBDY.Rows[RowNumb]["HandledBy"].ToString();
                    grdCancel[RowNumb + 1, (int)DR.gDTEDUE] = DTCBDY.Rows[RowNumb]["Duration"].ToString();
                    RowNumb++;
                }
            }
            #endregion
        }
    }
}
