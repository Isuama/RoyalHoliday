using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Transaction
{
    public partial class frmChangeRooms : Form
    {
        private const string msghd = "Change Room Types";
        enum CA { gRTI, gRTN, gRBI, gRBN, gMID, gMAX, gNOR };
        public frmChangeRooms(){InitializeComponent();}
        private void frmChangeRooms_Load(object sender, EventArgs e)
        {
        }
        private void Grd_Initializer()
        {
            try
            {
                #region ALL IN ONE DETAILS
                grdAll.Cols.Count = 7;
                grdAll.Rows.Count = 10;
                grdAll.Cols[(int)CA.gRTI].Width = 0;
                grdAll.Cols[(int)CA.gRTN].Width = 152;
                grdAll.Cols[(int)CA.gRBI].Width = 0;
                grdAll.Cols[(int)CA.gRBN].Width = 90;
                grdAll.Cols[(int)CA.gMID].Width = 0;
                grdAll.Cols[(int)CA.gMAX].Width = 100;
                grdAll.Cols[(int)CA.gNOR].Width = 90;
                grdAll.Cols[(int)CA.gRTI].Caption = "Room Type ID";
                grdAll.Cols[(int)CA.gRTN].Caption = "Room Type";
                grdAll.Cols[(int)CA.gRBI].Caption = "Basis Type ID";
                grdAll.Cols[(int)CA.gRBN].Caption = "Basis";
                grdAll.Cols[(int)CA.gMID].Caption = "Occupancy ID";
                grdAll.Cols[(int)CA.gMAX].Caption = "Occupancy";
                grdAll.Cols[(int)CA.gNOR].Caption = "No Of Rooms";
                grdAll.Cols[(int)CA.gNOR].Format = "##.##";
                grdAll.Cols[(int)CA.gRTN].ComboList = "...";
                grdAll.Cols[(int)CA.gRBN].ComboList = "...";
                grdAll.Cols[(int)CA.gMAX].ComboList = "...";
                grdAll.Rows[1].AllowEditing = true;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdAll_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTRoom, DTBasis, DTOcc;
                string SqlQuery;
                #region LOAD ROOM TYPES DETAILS AS DROP DOWN LIST_____________________________
                DTRoom = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_RoomTypes Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTRoom;
                if (e.Col == grdAll.Cols[(int)CA.gRTN].Index)
                {
                    frm.SubForm = new Master.frmRoomTypes();
                    frm.Width = grdAll.Cols[(int)CA.gRTN].Width;
                    frm.Height = grdAll.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdAll);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdAll[grdAll.Row, (int)CA.gRTI] = SelText[0];
                        grdAll[grdAll.Row, (int)CA.gRTN] = SelText[1];
                    }
                }
                #endregion
                #region LOAD BASIS TYPES DETAILS AS DROP DOWN LIST
                if (e.Col == grdAll.Cols[(int)CA.gRBN].Index)
                {
                    if (grdAll[grdAll.Row, grdAll.Cols[(int)CA.gRTI].Index] != null)
                    {
                        SqlQuery = "SELECT ID,Code AS Name FROM mst_BasisTypes Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1";
                        DTBasis = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTBasis;
                        frm.SubForm = new Master.frmBasisTypes();
                        frm.Width = grdAll.Cols[(int)CA.gRBN].Width;
                        frm.Height = grdAll.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdAll);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdAll[grdAll.Row, (int)CA.gRBI] = SelText[0];
                            grdAll[grdAll.Row, (int)CA.gRBN] = SelText[1];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Hotel Room Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                #endregion
                #region LOAD HOTEL OCCUPANCY DETAILS AS DROP DOWN LIST
                if (e.Col == grdAll.Cols[(int)CA.gMAX].Index)
                {
                    if (grdAll[grdAll.Row, grdAll.Cols[(int)CA.gRBI].Index] != null)
                    {
                        SqlQuery = "SELECT ID,Name FROM mst_HotelOccupnacy Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1";
                        DTOcc = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        frm = new Tourist_Management.Other.frmSearchGrd();
                        frm.DataSource = DTOcc;
                        frm.SubForm = new Master.frmHotelOccupancy();
                        frm.Width = grdAll.Cols[(int)CA.gMID].Width;
                        frm.Height = grdAll.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdAll);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdAll[grdAll.Row, (int)CA.gMID] = SelText[0];
                            grdAll[grdAll.Row, (int)CA.gMAX] = SelText[1];
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select Hotel Basis Type", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
