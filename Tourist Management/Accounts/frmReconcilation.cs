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
    public partial class frmReconcilation : Form
    {
        private const string msghd = "Reconcilation";
        int earlyRow=0 ;
        C1.Win.C1FlexGrid.CellStyle searched;
        C1.Win.C1FlexGrid.CellStyle transparent;
        enum REC { ID, VoucherID, ChkNo, ChkDate, PayAccountID, PayAccount, PayableToID, PayableTo, Amount, Realize, RealizeDate };
        public frmReconcilation(){InitializeComponent();}
        private void frmReconcilation_Load(object sender, EventArgs e)
        {
            Initializer();
        }
        private void Initializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Control();
                Fill_Details();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                #region DEBTOR DETAILS
                grdRecon.Cols.Count = 11;
                grdRecon.Cols[(int)REC.ID].Width = 0;
                grdRecon.Cols[(int)REC.VoucherID].Width = 106;
                grdRecon.Cols[(int)REC.ChkNo].Width = 84;
                grdRecon.Cols[(int)REC.ChkDate].Width = 86;
                grdRecon.Cols[(int)REC.PayAccountID].Width = 0;
                grdRecon.Cols[(int)REC.PayAccount].Width = 0;
                grdRecon.Cols[(int)REC.PayableToID].Width = 0;
                grdRecon.Cols[(int)REC.PayableTo].Width = 241;
                grdRecon.Cols[(int)REC.Amount].Width = 107;
                grdRecon.Cols[(int)REC.Realize].Width = 52;
                grdRecon.Cols[(int)REC.RealizeDate].Width = 100;
                grdRecon.Cols[(int)REC.RealizeDate].Caption = "ID";
                grdRecon.Cols[(int)REC.VoucherID].Caption = "Voucher ID";
                grdRecon.Cols[(int)REC.ChkNo].Caption = "Cheque No";
                grdRecon.Cols[(int)REC.ChkDate].Caption = "Cheque Date";
                grdRecon.Cols[(int)REC.PayAccountID].Caption = "Pay Account ID";
                grdRecon.Cols[(int)REC.PayAccount].Caption = "Pay Account";
                grdRecon.Cols[(int)REC.PayableToID].Caption = "Payable To ID";
                grdRecon.Cols[(int)REC.PayableTo].Caption = "Payable To";
                grdRecon.Cols[(int)REC.Amount].Caption = "Amount";
                grdRecon.Cols[(int)REC.Realize].Caption = "Realize";
                grdRecon.Cols[(int)REC.RealizeDate].Caption = "Realize Date";
                grdRecon.Cols[(int)REC.Realize].DataType = Type.GetType("System.Boolean");
                grdRecon.Rows[1].AllowEditing = false;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {
                cmbBank.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,AccountType FROM comAcc_AccountTypes Where IsNull(IsActive,0)=1 AND AccountTypeID=1 AND ID NOT IN(1,18) ORDER BY AccountType");
                searched = grdRecon.Styles.Add("searched");
                searched.BackColor = ColorTranslator.FromHtml("#E1F5A9");
                transparent = grdRecon.Styles.Add("transparent");
                transparent.BackColor = Color.Transparent;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            try
            {
                string qry;
                qry = "SELECT CPY.ID,CPY.VoucherID,CPY.ChkNo,CPY.ChkDate,CPY.PayAccount PayAccountID," +
                    "ACC.AccountType PayAccount,CPY.PayableTo PayableToID,APD.Name PayableTo," +
                    "CPD.Credit Amount,ISNULL(CPY.Realize,0)Realize,CPY.RealizeDate" +
                    " FROM dbo.act_CashPayment CPY INNER JOIN" +
                    " vw_ALL_PERSON_DETAILS APD ON CPY.PayableTo=APD.ID AND" +
                    " ISNULL(CPY.IsCancelled,0)<>1 AND ISNULL(CPY.PayAccount,18)<>18 AND" +
                    " CPY.Type IN ('CHQ','IOU') INNER JOIN" +
                    " dbo.comAcc_AccountTypes ACC ON CPY.PayAccount=ACC.ID INNER JOIN" +
                    " dbo.act_CashPayment_Detail CPD ON CPY.ID=CPD.CashPayID" +
                    " AND CPD.AccountNameID=CPY.PayAccount";
                qry += " AND CPY.PayAccount=" + cmbBank.SelectedValue + "";
                qry += " ORDER BY ChkDate";
                DataTable DT = Classes.clsGlobal.objCon.Fill_Table(qry);
                grdRecon.Rows.Count = 1000;
                if (DT.Rows.Count <= 0)
                {
                    grdRecon.Rows.Count = 1;
                    return;
                }
                foreach (DataRow dr in DT.Rows)
                {
                    grdRecon[DT.Rows.IndexOf(dr)+1, (int)REC.ID] = dr["ID"];
                    grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.VoucherID] = dr["VoucherID"];
                    grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.ChkNo] = dr["ChkNo"];
                    if(dr["ChkDate"]+"".Trim()!="")
                        grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.ChkDate] = Convert.ToDateTime(dr["ChkDate"]).ToString("yyyy-MMM-dd");
                    grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.PayAccountID] = dr["PayAccountID"];
                    grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.PayAccount] = dr["PayAccount"];
                    grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.PayableToID] = dr["PayableToID"];
                    grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.PayableTo] = dr["PayableTo"];
                    grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.Amount] = dr["Amount"];
                    if (Convert.ToBoolean(dr["Realize"]))
                    {
                        grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.Realize] = dr["Realize"];
                        grdRecon[DT.Rows.IndexOf(dr) + 1, (int)REC.RealizeDate] = Convert.ToDateTime(dr["RealizeDate"]).ToString("yyyy-MMM-dd");
                    }
                }
                grdRecon.Rows.Count = DT.Rows.Count+2;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void txtChkNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int row = 1;
                string chkNo;
                bool found = false;
                if (txtChkNo.Text.Trim() == "")
                {
                    if (earlyRow != 0)
                        grdRecon.Rows[earlyRow].Style = grdRecon.Styles["transparent"];
                    return;
                }
                while (row < grdRecon.Rows.Count - 1)
                {
                    chkNo = grdRecon[row, (int)REC.ChkNo].ToString().Trim();
                    if (chkNo.Contains(txtChkNo.Text.Trim()))
                    {
                        if (earlyRow != 0)
                            grdRecon.Rows[earlyRow].Style = grdRecon.Styles["transparent"];
                        grdRecon.Select(row, (int)REC.ID);
                        grdRecon.Rows[row].Style = grdRecon.Styles["searched"];
                        found = true;
                        earlyRow = row;
                        break;
                    }
                    row++;
                }
                if (!found && earlyRow != 0)
                    grdRecon.Rows[earlyRow].Style = grdRecon.Styles["transparent"];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtChkNo.Text = "";
        }
        private void cmbBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Details();
        }
        private void dtpYearMonth_ValueChanged(object sender, EventArgs e)
        {
            Fill_Details();
        }
    }
}
