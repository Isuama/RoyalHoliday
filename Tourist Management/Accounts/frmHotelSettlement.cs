using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Accounts
{
    public partial class frmHotelSettlement : Form
    {
        private const string msghd = "Manage Balance Hotel Payments";
        enum HTL { SettledID,TourID,VoucherID,BookingName,Balance,Amount };
       public int Hotel_ID;
        public frmHotelSettlement(){InitializeComponent();}
        private void frmHotelSettlement_Load(object sender, EventArgs e)
        {
            Initializer();
        }
        private void Initializer()
        {
            Grid_Initializer();
            Fill_Grid();
            lblHotelName.Text = Classes.clsGlobal.objCon.Fill_Table("SELECT Name FROM mst_HotelDetails WHERE ID=" + Hotel_ID + "").Rows[0]["Name"] + "".Trim();
        }
        private void Grid_Initializer()
        {
            try
            {
                grdManageHotel.Cols.Count = 6;
                grdManageHotel.Rows.Count = 100;
                grdManageHotel.Cols[(int)HTL.SettledID].Width = 50;
                grdManageHotel.Cols[(int)HTL.TourID].Width = 100;
                grdManageHotel.Cols[(int)HTL.VoucherID].Width = 100;
                grdManageHotel.Cols[(int)HTL.BookingName].Width = 200;
                grdManageHotel.Cols[(int)HTL.Balance].Width = 100;
                grdManageHotel.Cols[(int)HTL.Amount].Width = 100;
                grdManageHotel.Cols[(int)HTL.SettledID].Caption = "Settled ID";
                grdManageHotel.Cols[(int)HTL.TourID].Caption = "Tour ID";
                grdManageHotel.Cols[(int)HTL.VoucherID].Caption = "Voucher ID";
                grdManageHotel.Cols[(int)HTL.BookingName].Caption = "Guest";
                grdManageHotel.Cols[(int)HTL.Balance].Caption = "Balance";
                grdManageHotel.Cols[(int)HTL.Amount].Caption = "Amount";
                grdManageHotel.Cols[(int)HTL.Balance].Format = "##.##";
                grdManageHotel.Cols[(int)HTL.Amount].Format = "##.##";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Grid()
        {
            try
            {
                grdManageHotel.Rows.Count = 1;
                string sql;
                sql = "SELECT SettledID,TourID,HotelID,VoucherID,BookingName,Balance" +
                      " FROM vw_acc_BalanceHotelPayments WHERE HotelID=" + Hotel_ID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    grdManageHotel.Rows.Count = DT.Rows.Count + 1;
                    int RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["SettledID"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.SettledID] = DT.Rows[RowNumb]["SettledID"];
                        if (DT.Rows[RowNumb]["TourID"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.TourID] = DT.Rows[RowNumb]["TourID"];
                        if (DT.Rows[RowNumb]["VoucherID"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.VoucherID] = DT.Rows[RowNumb]["VoucherID"];
                        if (DT.Rows[RowNumb]["BookingName"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.BookingName] = DT.Rows[RowNumb]["BookingName"];
                        if (DT.Rows[RowNumb]["Balance"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.Balance] = DT.Rows[RowNumb]["Balance"];
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private Boolean Save_Pro()
        {
            try
            {
                if (Validate_Data() == false)
                {
                    return false;
                }
                if (Save_Procedure() == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Validate_Data()
        {
            try
            {                
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
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
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Tabs(System.Data.SqlClient.SqlCommand sqlCom)
        {
            try
            {
                if (Save_Payments(sqlCom) == false)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Payments(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            try
            {
                if ((grdManageHotel[RowNumb, grdManageHotel.Cols[(int)HTL.VoucherID].Index] == null))
                {
                    return false;
                }
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_ManagePrePayment";
                while (RowNumb < grdManageHotel.Rows.Count && grdManageHotel[RowNumb, grdManageHotel.Cols[(int)HTL.SettledID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    if (grdManageHotel[RowNumb, (int)HTL.Amount] + "".Trim() == "" || Convert.ToDecimal(grdManageHotel[RowNumb, (int)HTL.Amount]) <= 0)
                    {
                        RowNumb++;
                        continue;
                    }
                    sqlCom.Parameters.Add("@PayID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdManageHotel[RowNumb, (int)HTL.SettledID]);
                    sqlCom.Parameters.Add("@SettledVoucherID", SqlDbType.NVarChar, 50).Value = grdManageHotel[RowNumb, (int)HTL.VoucherID].ToString().Trim();
                    sqlCom.Parameters.Add("@SettledAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdManageHotel[RowNumb, (int)HTL.Amount]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
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
    }
}
