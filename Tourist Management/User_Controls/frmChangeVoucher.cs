using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.User_Controls
{
    public partial class frmChangeVoucher : Form
    {
        private const string msghd = "Tourist Group Amendment";
        enum SC { gUID, gHID, gCDT, gHNM, gMID, gSEL, gCPL };
        DataTable DTSel;
        public frmChangeVoucher(){InitializeComponent();}
        public DataTable DTHOTELS
        {
            get
            {
                return DTSel;
            }
            set
            {
                DTSel = value;
            }
        }
        private void frmChangeVoucher_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Data();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                #region SELECTION GRID
                grdSelection.Cols.Count = 7;
                grdSelection.Cols[(int)SC.gUID].Width = 0;
                grdSelection.Cols[(int)SC.gHID].Width = 0;
                grdSelection.Cols[(int)SC.gCDT].Width = 100;
                grdSelection.Cols[(int)SC.gHNM].Width = 298;
                grdSelection.Cols[(int)SC.gMID].Width = 0;
                grdSelection.Cols[(int)SC.gSEL].Width = 70;
                grdSelection.Cols[(int)SC.gCPL].Width = 100;
                grdSelection.Rows[0].Height = 35;
                grdSelection.Cols[(int)SC.gUID].Caption = "Unique ID";
                grdSelection.Cols[(int)SC.gHID].Caption = "Hotel ID";
                grdSelection.Cols[(int)SC.gCDT].Caption = "Checking Date";
                grdSelection.Cols[(int)SC.gHNM].Caption = "Hotel Name";
                grdSelection.Cols[(int)SC.gMID].Caption = "Meal For";
                grdSelection.Cols[(int)SC.gSEL].Caption = "Select";
                grdSelection.Cols[(int)SC.gCPL].Caption = "Cancellation \nPolicy";
                grdSelection.Cols[(int)SC.gSEL].DataType = Type.GetType("System.Boolean");
                grdSelection.Cols[(int)SC.gCPL].ComboList = "...";
                grdSelection.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                grdSelection.Cols[(int)SC.gCPL].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                grdSelection.Rows[1].AllowEditing = true;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public void Fill_Data()
        {
            try
            {
                #region FILL SELECTION DETAILS
                if (DTSel.Rows.Count > 0)
                {
                    int RowNumb = 0;
                    while (DTSel.Rows.Count > RowNumb)
                    {
                        grdSelection[RowNumb + 1, (int)SC.gUID] = DTSel.Rows[RowNumb][0].ToString();
                        grdSelection[RowNumb + 1, (int)SC.gHID] = DTSel.Rows[RowNumb][1].ToString();
                        grdSelection[RowNumb + 1, (int)SC.gCDT] = Convert.ToDateTime(DTSel.Rows[RowNumb][2].ToString());
                        grdSelection[RowNumb + 1, (int)SC.gHNM] = DTSel.Rows[RowNumb][3].ToString();
                        grdSelection[RowNumb + 1, (int)SC.gMID] = DTSel.Rows[RowNumb][4].ToString();
                        RowNumb++;
                    }
                    grdSelection.Rows.Count = RowNumb+1;
                }
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public DataTable Read_Selected_List()
        {
                DataTable DT=new DataTable();
                DT.Columns.Add("UniqueID", typeof(int));
                DT.Columns.Add("HotelID", typeof(int));
                DT.Columns.Add("CheckIn", typeof(DateTime));
                DT.Columns.Add("HotelName", typeof(string));
                DT.Columns.Add("MealFor", typeof(string));
                DT.Columns.Add("Select", typeof(bool));
                int RowNumb = 1;
                int HotelID = 0, UniqueID=0;
                DateTime ChkIn;
                bool HasChecked = false;
                string Mfor = "";
                if ((grdSelection[RowNumb, grdSelection.Cols[(int)SC.gHID].Index] == null) || (grdSelection[RowNumb, (int)SC.gHID].ToString() == ""))
                {
                    return DT;
                }
                while (grdSelection.Rows.Count>RowNumb)
                {
                        UniqueID = Convert.ToInt32(grdSelection[RowNumb, (int)SC.gUID]);
                        HotelID = Convert.ToInt32(grdSelection[RowNumb, (int)SC.gHID]);
                        ChkIn = Convert.ToDateTime(grdSelection[RowNumb, (int)SC.gCDT]);
                        HasChecked = Convert.ToBoolean(grdSelection[RowNumb, (int)SC.gSEL]);
                        Mfor = grdSelection[RowNumb, (int)SC.gMID].ToString().Trim();
                        DT.Rows.Add(UniqueID, HotelID, ChkIn, grdSelection[RowNumb, (int)SC.gHNM].ToString(), Mfor, HasChecked);
                    RowNumb++;
                }
                return DT;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            Classes.clsGlobal.SelectedHotels = Read_Selected_List() ;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void grdSelection_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if(e.Col == grdSelection.Cols[(int)SC.gCPL].Index)
            {
                Master.frmHotel frmHotel = new Master.frmHotel();
                frmHotel.Mode = 1;
                frmHotel.SystemCode = Convert.ToInt32(grdSelection[e.Row, (int)SC.gHID].ToString());
                frmHotel.StartPosition = FormStartPosition.CenterParent;
                frmHotel.Set_Selected_Tab("tpCancelation");
                frmHotel.ShowDialog();
            }
        }
    }
}
