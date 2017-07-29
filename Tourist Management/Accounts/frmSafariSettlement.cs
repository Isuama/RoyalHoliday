using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
namespace Tourist_Management.Accounts
{
    public partial class frmSafariSettlement : Form
    {
        private const string msghd = "Saffri Payments";
        enum SP { ID, TourID, Guest, Company, VoucherID, FromD, ToD, Child, Adult, Amount, ChequeNo, IsPaid,IsConfirm, HandledBy };
        bool isFormLoad;
        string sql;
        public frmSafariSettlement(){InitializeComponent();}
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void frmSafariSettlement_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Fill_Control();
                Grd_Initializer();
                isFormLoad = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {
                drpSafariCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Company FROM mst_SafariCompanies Where IsNull(IsActive,0)=1 ORDER BY ID");
                cmbCompany.DataSource= Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                db.GridInit(grdSafariPay, 2, true, SP.ID, 0, "ID", SP.TourID, 112, "Tour ID", SP.VoucherID, 80, "Voucher ID", true, SP.Guest, 185, "Guest", SP.Company, 185, "Company", SP.FromD, 69, "Date From", SP.ToD, 66, "Date To", SP.Adult, 58, "#Adults", SP.Child, 77, "#Children", SP.Amount, 100, "Amount", "##.##", SP.ChequeNo, 100, "Cheque No", SP.IsPaid, 59, "Is Paid", Type.GetType("System.Boolean"), SP.IsConfirm, 59, "Confirm", Type.GetType("System.Boolean"), SP.HandledBy, 100, "Handled By"); 
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Filter_Values()
        {
            try
            {
                if (!isFormLoad)
                    return;
                DateTime dateFrom = dtpFromDate.Value;
                DateTime dateTo = dtpToDate.Value;
                bool IsPaid = Convert.ToBoolean(chkTodayPay.Checked);
                DateTime datePaid = dtpPaidDate.Value;
                sql = "SELECT Company_Logo,DisplayName,Telephone,Mobile,Fax,E_Mail,E_mailTo,UserName,UserGroupID,GroupName,Web,Physical_Address," +
                      "ID,TourID,Guest,SafariCompany,VoucherID,FromDate,ToDate,ISNULL(Amount,0.00)AS Amount," +
                      "ISNULL(ChequeNo,'')AS ChequeNo,ISNULL(Children,0)AS Children,ISNULL(Adult,0)AS Adult," +
                      "VehicleType,ISNULL(NoOfVehicles,0)AS NoOfVehicles,HandledBy,SafariCompany," +
                      "ISNULL(IsPaid,0)AS IsPaid,PaidBy,PaidDate,ISNULL(IsConfirm,0)AS IsConfirm," +
                      "Aname1,Ano1,MDname,AADname,AADno FROM vw_acc_SafriPayments" +
                      " WHERE ISNULL(IsConfirm,0)<>1 AND UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                if (chkCmpny.Checked)
                {
                    int compID = Convert.ToInt32(cmbCompany.SelectedValue);
                    sql += " AND CompID=" + compID + "";
                }
                int safariCompID;
                grdSafariPay.Cols[(int)SP.Company].Width = 185;
                if (drpSafariCompany.SelectedValue + "".Trim() != "")
                {
                    safariCompID = Convert.ToInt32(drpSafariCompany.SelectedValue);
                    sql += " AND SafariCompanyID=" + safariCompID + "";
                    grdSafariPay.Cols[(int)SP.Company].Width = 0;
                }
                if (chkTodayPay.Checked)
                {
                    sql += " AND PaidDate='" + dtpPaidDate.Value.ToString("yyyy/MM/dd") + "'";
                }
                else
                {
                     sql+= " AND FromDate >='" + dtpFromDate.Value.ToString("yyyy/MM/dd") + "'" +
                      " AND ToDate <='" + dtpToDate.Value.ToString("yyyy/MM/dd") + "'";
                }
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                grdSafariPay.Rows.Count = 1;
                btnOk.Enabled = false;
                btnPrint.Enabled = false;
                if (DT.Rows.Count > 0)
                {
                    grdSafariPay.Rows.Count = DT.Rows.Count + 2;
                    btnOk.Enabled = true;
                    btnPrint.Enabled = true;
                    int RowNumb = 0;                    
                    decimal totAmt = 0.00m;
                    int totCh = 0, totAd = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdSafariPay[RowNumb + 1, (int)SP.ID] = DT.Rows[RowNumb]["ID"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.TourID] = DT.Rows[RowNumb]["TourID"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.Guest] = DT.Rows[RowNumb]["Guest"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.Company] = DT.Rows[RowNumb]["SafariCompany"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.VoucherID] = DT.Rows[RowNumb]["VoucherID"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.FromD] = DT.Rows[RowNumb]["FromDate"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.ToD] = DT.Rows[RowNumb]["ToDate"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.Adult] = Convert.ToInt32(DT.Rows[RowNumb]["Adult"]).ToString();
                        totAd+=Convert.ToInt32(DT.Rows[RowNumb]["Adult"]);
                        grdSafariPay[RowNumb + 1, (int)SP.Child] = Convert.ToInt32(DT.Rows[RowNumb]["Children"]).ToString();
                        totCh += Convert.ToInt32(DT.Rows[RowNumb]["Children"]);
                        grdSafariPay[RowNumb + 1, (int)SP.Amount] = Convert.ToDecimal(DT.Rows[RowNumb]["Amount"]).ToString();
                        totAmt += Convert.ToDecimal(DT.Rows[RowNumb]["Amount"]);
                        grdSafariPay[RowNumb + 1, (int)SP.ChequeNo] = DT.Rows[RowNumb]["ChequeNo"].ToString();
                        grdSafariPay[RowNumb + 1, (int)SP.IsPaid] = Convert.ToBoolean(DT.Rows[RowNumb]["IsPaid"]);
                        grdSafariPay[RowNumb + 1, (int)SP.HandledBy] = DT.Rows[RowNumb]["HandledBy"].ToString();
                        RowNumb++;
                    }
                    grdSafariPay[RowNumb + 1, (int)SP.Adult] = totAd.ToString();
                    grdSafariPay[RowNumb + 1, (int)SP.Child] = totCh.ToString();
                    grdSafariPay[RowNumb + 1, (int)SP.Amount] = totAmt.ToString();
                    C1.Win.C1FlexGrid.CellStyle TOT = grdSafariPay.Styles.Add("TOT");                    
                    TOT.BackColor = Color.LightSteelBlue;
                    grdSafariPay.Rows[RowNumb + 1].Style = TOT;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void cmbCompany_SelectedValueChanged(object sender, EventArgs e)
        {
            Filter_Values();
        }
        private void drpSafariCompany_Selected_TextChanged(object sender, EventArgs e)
        {
            Filter_Values();
        }
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            Filter_Values();
        }
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            Filter_Values();
        }
        private void dtpPaidDate_ValueChanged(object sender, EventArgs e)
        {
            Filter_Values();
        }
        private void chkTodayPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodayPay.Checked)
            {
                dtpFromDate.Enabled = false;
                dtpToDate.Enabled = false;
            }
            else
            {
                dtpFromDate.Enabled = true;
                dtpToDate.Enabled = true;
            }
        }
        private void chkAllPaid_CheckedChanged(object sender, EventArgs e)
        {
            Change_IsPaid_Check_Status(chkAllPaid.Checked);
        }        
        private void Change_IsPaid_Check_Status(bool isPaid)
        {
            try
            {
                int RowNumb=1;
                while (grdSafariPay[RowNumb, (int)SP.ID] + "".Trim() != "")
                {
                    grdSafariPay[RowNumb, (int)SP.IsPaid] = isPaid;
                    RowNumb++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnDefChkNo_Click(object sender, EventArgs e)
        {
            try
            {
                int RowNumb = 1;
                string chkno = txtChkNo.Text.Trim();
                while (grdSafariPay[RowNumb, (int)SP.ID] + "".Trim() != "")
                {
                    grdSafariPay[RowNumb, (int)SP.ChequeNo] = chkno;
                    RowNumb++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Data() == true)
            {
                MessageBox.Show("Transaction Successfully Saved", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (chkPrint.Checked)
                {
                    Print_Details();
                }                
            }
            else
                MessageBox.Show("Transaction Failed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            try
            {
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Update_Safari_Payments";
                int RowNumb=1;
                if (!rdbBank.Checked && !rdbCash.Checked)
                {
                    MessageBox.Show("Please select a pay method", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                while (grdSafariPay[RowNumb, grdSafariPay.Cols[(int)SP.ID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar,50).Value = grdSafariPay[RowNumb, (int)SP.VoucherID].ToString().Trim();
                    if (grdSafariPay[RowNumb, (int)SP.ChequeNo]+"".Trim()!="")
                    sqlCom.Parameters.Add("@ChequeNo", SqlDbType.NVarChar, 50).Value = grdSafariPay[RowNumb, (int)SP.ChequeNo].ToString().Trim();
                    sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdSafariPay[RowNumb, (int)SP.Amount]);
                    if (Convert.ToBoolean(grdSafariPay[RowNumb, (int)SP.IsPaid]))
                    {
                        sqlCom.Parameters.Add("@IsPaid", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@IsConfirm", SqlDbType.Int).Value = Convert.ToBoolean(grdSafariPay[RowNumb, (int)SP.IsConfirm]);
                        sqlCom.Parameters.Add("@PaidBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                        sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = dtpPaidDate.Value;
                    }
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == false)
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
        private void chkConfirmAll_CheckedChanged(object sender, EventArgs e)
        {
            Change_IsConfirm_Check_Status(chkConfirmAll.Checked);
        }
        private void Change_IsConfirm_Check_Status(bool isConfirm)
        {
            try
            {
                int RowNumb = 1;
                while (grdSafariPay[RowNumb, (int)SP.ID] + "".Trim() != "")
                {
                    grdSafariPay[RowNumb, (int)SP.IsConfirm] = isConfirm;
                    RowNumb++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Details();
        }
        private void Print_Details()
        {
            try
            {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                DataTable DT;
                DataSet DTG = new DataSet();
                ReportDocument ga = new ReportDocument();
                string reptype = "";
                sql += " AND ISNULL(IsPaid,0)<>0 ";
                DTG = new DataSets.ds_acc_SafariPayments();
                    ga = new Tourist_Management.Reports.SafariPayments(); 
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    sConnection.Print_Report("", sql, DTG, ga, reptype, new SqlParameter("comp", chkCmpny.Checked));
                }
                else
                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkCmpny_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCmpny.Checked)
            {
                cmbCompany.Enabled = true;
                Filter_Values();
            }
            else
            {
                cmbCompany.Enabled = false;
                Filter_Values();
            }
        }
    }
}
