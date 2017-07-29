using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Tourist_Management.Accounts
{
    public partial class frmTDL : Form
    {
        private const string msghd = "TDL Hotel";
        enum GRD { ID, TourID, VoucherID, Date, TDLNo, HotelID, HotelName, Amount };
        Boolean Loaded = false;
        DataTable DT; 
        public frmTDL()        {            InitializeComponent();        }
        private void frmTDL_Load(object sender, EventArgs e)
        {
            try
            {
                Grd_Initializer();
                Fill_Control();
                Loaded = true;
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Grd_Initializer()
        {
            try
            {
                grdViewer.Cols.Count = 8;
                grdViewer.Rows.Count = 2;
                grdViewer.Cols[(int)GRD.ID].Width = 100;
                grdViewer.Cols[(int)GRD.TourID].Width = 100;
                grdViewer.Cols[(int)GRD.VoucherID].Width = 100;
                grdViewer.Cols[(int)GRD.TDLNo].Width = 100;
                grdViewer.Cols[(int)GRD.HotelID].Width = 100;
                grdViewer.Cols[(int)GRD.HotelName].Width = 100;
                grdViewer.Cols[(int)GRD.Date].Width = 100;
                grdViewer.Cols[(int)GRD.Amount].Width = 100;               
                grdViewer.Cols[(int)GRD.ID].Caption = "ID";
                grdViewer.Cols[(int)GRD.TourID].Caption = "Tour ID";
                grdViewer.Cols[(int)GRD.VoucherID].Caption = "Voucher ID";
                grdViewer.Cols[(int)GRD.TDLNo].Caption = "TDL No";
                grdViewer.Cols[(int)GRD.HotelID].Caption = "Hotel ID";
                grdViewer.Cols[(int)GRD.HotelName].Caption = "Hotel Name";
                grdViewer.Cols[(int)GRD.Date].Caption = "Date";
                grdViewer.Cols[(int)GRD.Amount].Caption = "Amount";
                grdViewer.Cols[(int)GRD.Date].DataType = Type.GetType("System.DateTime");
                grdViewer.Cols[(int)GRD.Amount].Format = "##.##";
            }
            catch (Exception ex) {  db.MsgERR(ex);   }
        }
        private void Fill_Control()
        {
            try
            {
                ucFilterByCompany1.cmbICompany.DataSource= Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
                 drpsHotel.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS HotelName FROM mst_HotelDetails Where IsNull(IsActive,0)=1 ORDER BY HotelName");//ISNULL(PayID,1)=2 AND 
             
            }
            catch (Exception ex) {  db.MsgERR(ex); }
        }
        private void btnICancel_Click(object sender, EventArgs e)        {            this.Close();  }
        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Loaded)    return;
                string ssql, conPaid = "", filter = "";
                conPaid = "1,0";
                DT = new DataTable();
                ssql = "SELECT UniqueID,TourID,VoucherID,DateOut,HotelID,HotelName,TDLNo,ISNULL(IsPaid,0)AS IsPaid,ISNULL(ConfirmPaid,0)AS ConfirmPaid FROM vw_acc_HotelAllPayments WHERE ISNULL(ConfirmPaid,0) IN(" + conPaid + ")";
                if (ucFilterByCompany1.chkICmpny.Checked)
                {
                    int compID = Convert.ToInt32(ucFilterByCompany1.cmbICompany.SelectedValue);
                    filter += " AND CompID=" + compID + "";
                }
                if (ucFilterByDate1.chkIByDate.Checked)
                {
                    filter += " AND DateOut>='" + ucFilterByDate1.dtpIFromDate.Value.ToString("yyyy-MM-dd").Trim() +
                              "' AND DateOut<='" + ucFilterByDate1.dtpIToDate.Value.ToString("yyyy-MM-dd").Trim() + "'";
                }
                if (chkByHotel.Checked)
                {
                    if (drpsHotel.SelectedList != null)
                    {
                        string HotelID = "";
                        foreach (string s in drpsHotel.SelectedList) 
                            if (HotelID.Trim() == "")    HotelID = s.Trim(); else   HotelID += ",".Trim() + s.Trim(); 
                        filter += " AND HotelID IN(" + HotelID + ")";
                    }
                }
                string HotelSql = (ssql.Trim() + filter.Trim() + " ORDER BY DateOut,UniqueID").Trim();
                if (HotelSql == "")    return;
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(HotelSql);
                int RowNumb = 0;
                double TotExp;
                string qry;
                RowNumb = 1;
                grdViewer.Rows.Count = 3000;
                foreach (DataRow dr in DT.Rows)
                {
                    grdViewer.Rows[RowNumb].AllowEditing = false;
                    if (dr["UniqueID"] + "".Trim() == "") continue;
                    grdViewer[RowNumb, (int)GRD.ID] = dr["UniqueID"].ToString();
                    grdViewer[RowNumb, (int)GRD.TourID] = dr["TourID"].ToString();
                    grdViewer[RowNumb, (int)GRD.TDLNo] = dr["TDLNo"].ToString();
                    grdViewer[RowNumb, (int)GRD.VoucherID] = dr["VoucherID"].ToString();
                    grdViewer[RowNumb, (int)GRD.HotelID] = dr["HotelID"].ToString();
                    grdViewer[RowNumb, (int)GRD.HotelName] = dr["HotelName"].ToString();
                    if (dr["DateOut"] + "".Trim() != "")   grdViewer[RowNumb, (int)GRD.Date] = dr["DateOut"].ToString();
                    qry = "SELECT dbo.fun_CalculateHotelAmount('" + dr["VoucherID"].ToString().Trim() + "')Amt";
                    TotExp = Convert.ToDouble(Classes.clsGlobal.objCon.Fill_Table(qry).Rows[0]["Amt"]);
                    grdViewer[RowNumb, (int)GRD.Amount] = TotExp.ToString();
                    RowNumb++;
                }
                  grdViewer.Rows.Count = RowNumb; 
            }
            catch (Exception ex)            {                db.MsgERR(ex);    }
        } 
    }
}
