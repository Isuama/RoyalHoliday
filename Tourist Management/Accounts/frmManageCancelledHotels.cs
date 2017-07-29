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
    public partial class frmManageCancelledHotels : Form
    {
        private const string msghd = "Manage Paid Hotel";
        enum HTL { ID, ItenaryID, VoucherID, DateIn, Dateout, HotelName, TotPaid, ActualPayable, IsPaid, ConfirmPaid, Remarks };
        public frmManageCancelledHotels(){InitializeComponent();}
        private void frmManageCancelledHotels_Load(object sender, EventArgs e)
        {
           Initializer();
        }
        private void Initializer()
        {
          Grid_Initializer();  
        }
        private void Grid_Initializer()
        {
            try
            {                
                grdManageHotel.Cols.Count = 11;
                grdManageHotel.Rows.Count = 20;
                grdManageHotel.Cols[(int)HTL.ID].Width = 0;/////
                grdManageHotel.Cols[(int)HTL.ItenaryID].Width = 0;/////
                grdManageHotel.Cols[(int)HTL.VoucherID].Width = 80;
                grdManageHotel.Cols[(int)HTL.DateIn].Width = 75;
                grdManageHotel.Cols[(int)HTL.Dateout].Width = 75;
                grdManageHotel.Cols[(int)HTL.HotelName].Width = 200;              
                grdManageHotel.Cols[(int)HTL.TotPaid].Width = 70;
                grdManageHotel.Cols[(int)HTL.ActualPayable].Width = 70;
                grdManageHotel.Cols[(int)HTL.IsPaid].Width = 50;
                grdManageHotel.Cols[(int)HTL.ConfirmPaid].Width = 50;
                grdManageHotel.Cols[(int)HTL.Remarks].Width = 260;//wadipura thiyanna
                grdManageHotel.Cols[(int)HTL.ID].Caption = "Set Off ID";
                grdManageHotel.Cols[(int)HTL.ItenaryID].Caption = "Itenary ID";
                grdManageHotel.Cols[(int)HTL.VoucherID].Caption = "Voucher ID";
                grdManageHotel.Cols[(int)HTL.DateIn].Caption = "Date In";
                grdManageHotel.Cols[(int)HTL.Dateout].Caption = "Date Out";
                grdManageHotel.Cols[(int)HTL.HotelName].Caption = "Hotel Name";
                grdManageHotel.Cols[(int)HTL.TotPaid].Caption = "Total Paid";
                grdManageHotel.Cols[(int)HTL.ActualPayable].Caption = "Actual Payable";
                grdManageHotel.Cols[(int)HTL.IsPaid].Caption = "Is Paid";
                grdManageHotel.Cols[(int)HTL.ConfirmPaid].Caption = "Confirm Paid";
                grdManageHotel.Cols[(int)HTL.Remarks].Caption = "Remarks";
                grdManageHotel.Cols[(int)HTL.IsPaid].DataType = Type.GetType(" System.Boolean");
                grdManageHotel.Cols[(int)HTL.ConfirmPaid].DataType = Type.GetType(" System.Boolean");
                grdManageHotel.Cols[(int)HTL.TotPaid].Format = "##.##";
                grdManageHotel.Cols[(int)HTL.ActualPayable].Format = "##.##";                                             
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Grid(string TourID)
        {
            try
            {
                grdManageHotel.Rows.Count = 1;
                decimal val;                        
                string sql,ssql;
                sql = "SELECT ItenaryID,VoucherID,DateIn,Dateout,HotelName,TotPaid,ActualPayable," +
                      "ISNULL(IsPaid,0)IsPaid,ISNULL(ConfirmPaid,0)ConfirmPaid,Remarks" +
                      " FROM vw_ManagePaidHotels WHERE TransID="+TourID+"";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    grdManageHotel.Rows.Count = DT.Rows.Count+1;
                    int RowNumb=0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["ItenaryID"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.ItenaryID] = DT.Rows[RowNumb]["ItenaryID"].ToString().Trim();
                        if (DT.Rows[RowNumb]["VoucherID"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.VoucherID] = DT.Rows[RowNumb]["VoucherID"].ToString().Trim();
                        if (DT.Rows[RowNumb]["DateIn"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.DateIn] = DT.Rows[RowNumb]["DateIn"].ToString().Trim();
                        if (DT.Rows[RowNumb]["Dateout"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.Dateout] = DT.Rows[RowNumb]["Dateout"].ToString().Trim();
                        if (DT.Rows[RowNumb]["HotelName"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.HotelName] = DT.Rows[RowNumb]["HotelName"].ToString().Trim();
                        ssql = "SELECT dbo.fun_CalculateHotelAmount('" + DT.Rows[RowNumb]["VoucherID"].ToString().Trim() + "')";
                        val = Convert.ToDecimal(Classes.clsGlobal.objCon.Fill_Table(ssql).Rows[0][0]);
                        grdManageHotel[RowNumb + 1, (int)HTL.TotPaid] = val.ToString().Trim();                            
                        if (DT.Rows[RowNumb]["ActualPayable"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.ActualPayable] = DT.Rows[RowNumb]["ActualPayable"].ToString().Trim();
                        grdManageHotel[RowNumb + 1, (int)HTL.IsPaid] = Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]);
                        grdManageHotel[RowNumb + 1, (int)HTL.ConfirmPaid] = Convert.ToBoolean(DT.Rows[RowNumb]["ConfirmPaid"]);
                        if (DT.Rows[RowNumb]["Remarks"] + "".Trim() != "")
                            grdManageHotel[RowNumb + 1, (int)HTL.Remarks] = DT.Rows[RowNumb]["Remarks"].ToString().Trim();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnTour_Click(object sender, EventArgs e)
        {
            try
            {
                string sql;
                sql = "SELECT ID,TourID,Guest,AgentID FROM trn_GroupAmendment";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
                txtTourNo.Text = finder.Load_search(DT);
                if (txtTourNo.Text.ToString().Trim() != "")
                {
                    Fill_Grid(txtTourNo.Text.Trim());
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                int RowNumb=1;
                bool changeOne=false;
                while (grdManageHotel.Rows.Count > RowNumb)
                {
                    if (grdManageHotel[RowNumb, (int)HTL.ActualPayable] + "".Trim() != "")
                    {
                        if (!Classes.clsGlobal.IsNumeric(grdManageHotel[RowNumb, (int)HTL.ActualPayable].ToString().Trim()))
                        {
                            MessageBox.Show("Please Enter Valid Paid Value.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        changeOne = true;
                    }
                    RowNumb++;
                }
                if (!changeOne)
                {
                    MessageBox.Show("No Records Found To Be Changed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_ManagePaidHotels";
                while ( RowNumb < grdManageHotel.Rows.Count && grdManageHotel[RowNumb, grdManageHotel.Cols[(int)HTL.VoucherID].Index] != null )
                {
                    sqlCom.Parameters.Clear();
                    if (grdManageHotel[RowNumb, (int)HTL.ActualPayable] + "".Trim() == "" || Convert.ToDecimal(grdManageHotel[RowNumb, (int)HTL.ActualPayable]) <= 0)
                    {
                        RowNumb++;
                        continue;
                    }
                    if (grdManageHotel[RowNumb, (int)HTL.ItenaryID].ToString().Trim() != "")
                        sqlCom.Parameters.Add("@ItenaryID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdManageHotel[RowNumb, (int)HTL.ItenaryID]);
                    if (grdManageHotel[RowNumb, (int)HTL.VoucherID].ToString().Trim() != "")
                        sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 50).Value = grdManageHotel[RowNumb, (int)HTL.VoucherID].ToString().Trim();
                    if (grdManageHotel[RowNumb, (int)HTL.ActualPayable]+"".Trim() != "")
                        sqlCom.Parameters.Add("@ActualPayable", SqlDbType.Decimal).Value = Convert.ToDecimal(grdManageHotel[RowNumb, (int)HTL.ActualPayable].ToString().Trim());
                    if (grdManageHotel[RowNumb, (int)HTL.TotPaid] + "".Trim() != "")
                        sqlCom.Parameters.Add("@TotPaid", SqlDbType.Decimal).Value = Convert.ToDecimal(grdManageHotel[RowNumb, (int)HTL.TotPaid].ToString().Trim());
                    if (grdManageHotel[RowNumb, (int)HTL.Remarks] + "".Trim() != "")
                        sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 500).Value = grdManageHotel[RowNumb, (int)HTL.Remarks].ToString().Trim();
                    sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdManageHotel[RowNumb, (int)HTL.IsPaid])? "1" : "0";
                    sqlCom.Parameters.Add("@ConfirmPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdManageHotel[RowNumb, (int)HTL.ConfirmPaid]) ? "1" : "0";
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
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
    }
}
