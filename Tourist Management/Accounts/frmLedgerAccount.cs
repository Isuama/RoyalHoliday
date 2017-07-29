using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace Tourist_Management.Accounts
{
    public partial class frmLedgerAccount : Form
    {
        private const string msghd = "Ledger Accounts";
        int earlyRow = 0;
        enum ACC { gID, gName, gSelect };
        DataTable DTBL = new DataTable();
        string SubAccount;
        public frmLedgerAccount() { InitializeComponent(); }
        private void frmLedgerAccount_Load(object sender, EventArgs e)
        {
            try
            {
                db.GridInit(grdAccount, 1, ACC.gID, 00, "Account ID", ACC.gName, 827, "AccountType", ACC.gSelect, 136, "Select", Type.GetType(" System.Boolean"));
                Fill_Control();
                Fill_Grid();
                SubAccount = "";
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Fill_Grid()
        {
            try
            {
                string sql = "SELECT ID,AccountType FROM comAcc_AccountTypes WHERE ISNULL(IsActive,0)<>0 AND ID<>AccountTypeID";
                if (rdbSummary.Checked && cmbAccountType.SelectedValue + "".Trim() != "")
                    sql += " AND AccountTypeID=" + cmbAccountType.SelectedValue + "";
                DTBL = Classes.clsGlobal.objCon.Fill_Table(sql);
                grdAccount.Rows.Count = 1;
                int row = 1;
                foreach (DataRow dr in DTBL.Rows)
                {
                    grdAccount.Rows.Add();
                    grdAccount[row, (int)ACC.gID] = dr["ID"];
                    grdAccount[row, (int)ACC.gName] = dr["AccountType"];
                    grdAccount[row, (int)ACC.gSelect] = false;
                    row++;
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Fill_Control()
        {
            try
            {
                ucFilterByCompany1.Intializer();
                ucFilterByOther1.Query = "SELECT ID, Name FROM vw_ALL_PERSON_DETAILS";
                ucFilterByOther1.Intializer(); 
                cmbAccountType.DataSource  = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,AccountType FROM comAcc_AccountTypes Where IsNull(IsActive,0)=1 AND ISNULL(COD,'x')<>'x' ORDER BY ID");
                for (int x = 0; x < grdAccount.Cols.Count - 1; x++) cmbFld.Items.Add(grdAccount[0, x].ToString());
                db.LoadSearch(cmbOp);
                cmbFld.SelectedIndex = 1;
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }

        private void chkCheck_CheckedChanged(object sender, EventArgs e)
        {
            chkCheck.Text = chkCheck.Checked ? "Check None" : "Check All";
            check_uncheck(chkCheck.Checked);
        }
        public void check_uncheck(bool status)
        {
            try
            {
                int row = 1;
                while (grdAccount.Rows.Count > 1 && grdAccount[row, (int)ACC.gID] + "".Trim() != "")
                {
                    grdAccount[row, (int)ACC.gSelect] = status;
                    row++;
                    if (grdAccount.Rows.Count == row) break;
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void btnICancel_Click(object sender, EventArgs e) { this.Close(); }
        private void txtAccountName_TextChanged(object sender, EventArgs e) { Apply_Filter(); }
        public void search_By_ID(int ID)
        {
            try
            {
                int row = 1, AccID;
                bool found = false;
                C1.Win.C1FlexGrid.CellStyle transparent;
                transparent = grdAccount.Styles.Add("transparent");
                transparent.BackColor = Color.Transparent;
                C1.Win.C1FlexGrid.CellStyle searched;
                searched = grdAccount.Styles.Add("searched");
                searched.BackColor = ColorTranslator.FromHtml("#E1F5A9");
                if (ID + "".Trim() == "")
                {
                    if (earlyRow != 0) grdAccount.Rows[earlyRow].Style = grdAccount.Styles["transparent"];
                    return;
                }
                while (row < grdAccount.Rows.Count - 1)
                {
                    AccID = Convert.ToInt32(grdAccount[row, (int)ACC.gID]);
                    if (ID == AccID)
                    {
                        if (earlyRow != 0) grdAccount.Rows[earlyRow].Style = grdAccount.Styles["transparent"];
                        grdAccount.Select(row, (int)ACC.gName);
                        grdAccount.Rows[row].Style = grdAccount.Styles["searched"];
                        found = true;
                        earlyRow = row;
                        break;
                    }
                    row++;
                }
                if (!found && earlyRow != 0)
                    grdAccount.Rows[earlyRow].Style = grdAccount.Styles["transparent"];
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtAccountName.Text = "";
            if (earlyRow != 0) grdAccount.Rows[earlyRow].Style = grdAccount.Styles["transparent"];
        }
        private void btnIPreview_Click(object sender, EventArgs e) { Print_Payment(get_Query()); }
        private string get_Query()
        {
            try
            {
                string dfrom, dto, sql, part = "";
                if (ucFilterByDate1.chkIByDate.Checked)
                {
                    dfrom = ucFilterByDate1.dtpIFromDate.Value.ToString("yyyy-MM-dd").Trim();
                    dto = ucFilterByDate1.dtpIToDate.Value.ToString("yyyy-MM-dd").Trim();
                    part += " AND PaidDate>='" + dfrom + "' AND PaidDate<='" + dto + "'";
                }
                if (ucFilterByCompany1.chkICmpny.Checked) part += " AND CompID=" + ucFilterByCompany1.cmbICompany.SelectedValue + "";
                if (ucFilterByOther1.chkIByOther.Checked)
                {
                    if (ucFilterByOther1.drpOther.SelectedValue + "".Trim() != "") part += " AND PayableToID=" + ucFilterByOther1.drpOther.SelectedValue + "";
                }
                string val = selected_Accounts().Trim();
                if (val != "") part += " AND AccountNameID IN(" + val + ")";

                if (rdbDetail.Checked)
                {
                    sql = "SELECT DisplayName Company,VoucherID,PayableToID,ChkNo,PayableTo,PaidDate,AccountNameID,AccountName,Memo,Debit,Credit,AccountTypeID,ISNULL(OpenBal,0)OpenBal,OpenBalDate FROM vw_Acc_LedgerAccount WHERE 1=1";
                    sql += part + " ORDER BY PaidDate";
                }
                else
                {
                    sql = " SELECT * FROM( SELECT AccountNameID,AccountType,YEAR(PaidDate)[Year], LEFT(DATENAME(MONTH,PaidDate),3)[Month],ISNULL(Debit,0)Debit FROM vw_Account_Summary where 1=1 " +
                    part + ")AS S1 PIVOT ( SUM(Debit) FOR [Month] IN (jan, feb, mar, apr,may, jun, jul, aug, sep, oct, nov, [dec]) )AS P1";
                }
                return sql;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return "";
            }
        }

        private void bPrintTotals_Click(object sender, EventArgs e)
        {
            rdbDetail.Checked = true;
            string sql = "select AccountNameID,AccountName,AccountTypeID,dbo.V(sum(Debit)) Debit,dbo.V(Sum(Credit)) Credit,OpenBalDate, dbo.V(OpenBal) OpenBal FROM (" + get_Query().Replace("ORDER BY PaidDate", "") + ") x GROUP BY AccountNameID,AccountName,AccountTypeID,OpenBalDate, OpenBal";
            db.showReport(new Tourist_Management.Reports.rpt_LedgerTotal(), sql);
        }
        private void Print_Payment(string sql)
        {
            Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql); 
            if (DT.Rows.Count > 0)
            {
                if (rdbDetail.Checked)
                {
                    DataSets.acc_LedgerAccount DTP = new Tourist_Management.DataSets.acc_LedgerAccount();
                    string SubAccName = (SubAccount.Trim() != "") ? SubAccount.Trim() : "";
                    Classes.clsConnection.Fill_DataAdapter(sql).Fill(DTP.Tables[0]);
                    if (SubAccount.Trim() != "") DTP.Tables[0].Rows[0]["AccountName"] = SubAccount;
                    db.showReport(new Tourist_Management.Reports.rpt_LedgerAccount(), DTP.Tables[0]);
                }
                else
                {
                    DataSets.acc_LedgerAccount_Summary DTP = new Tourist_Management.DataSets.acc_LedgerAccount_Summary();
                    Classes.clsConnection.Fill_DataAdapter(sql).Fill(DTP.Tables["dtLedgerSum"]);
                    db.showReport(new Tourist_Management.Reports.rpt_LedgerSummary(), DTP);
                }
            }
            else
                MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private string selected_Accounts()
        {
            try
            {
                int row = 1;
                string sel = "";
                SubAccount = "";
                while (grdAccount.Rows.Count > 1 && grdAccount[row, (int)ACC.gID] + "".Trim() != "")
                {
                    if (Convert.ToBoolean(grdAccount[row, (int)ACC.gSelect]))
                    {
                        if (grdAccount[row, (int)ACC.gID] + "".Trim() != "")
                        {
                            string id = grdAccount[row, (int)ACC.gID] + "".Trim();
                            string qry = "SELECT ID,AccountType FROM comAcc_AccountTypes WHERE ISNULL(IsActive,0)=1 AND ISNULL(Status,0)<>7 AND ISNULL(SubAccTypeID,0)=" + id + "";
                            DataTable dt = Classes.clsGlobal.objCon.Fill_Table(qry);
                            if (dt.Rows.Count > 0 && dt.Rows[0]["ID"] + "".Trim() != "") SubAccount = grdAccount[row, (int)ACC.gName] + "".Trim();
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (sel.Trim() != "") sel += ",".Trim() + dr["ID"] + "".Trim();
                                else sel += dr["ID"] + "".Trim();
                            }
                            if (sel.Trim() != "") sel += ",".Trim() + grdAccount[row, (int)ACC.gID] + "".Trim();
                            else sel += grdAccount[row, (int)ACC.gID] + "".Trim();
                        }
                    }
                    row++;
                    if (grdAccount.Rows.Count == row) break;
                }
                return sel;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return "";
            }
        }
        private void rdbSummary_CheckedChanged(object sender, EventArgs e)
        {
            cmbAccountType.Enabled = rdbSummary.Checked; Fill_Grid();
        }
        private void cmbAccountType_SelectedIndexChanged(object sender, EventArgs e) { Fill_Grid(); }
        private void Apply_Filter()
        {
            DataView DV = new DataView(DTBL);
            if (txtAccountName.Text.Trim() == "") return;
            switch (cmbOp.Text.Trim())
            {
                case "Contains":
                case "Begins with":
                case "Ends with":
                    DV.RowFilter = cmbFld.Text.Trim() + " " + cmbOp.SelectedValue.ToString().Trim().Replace("##", txtAccountName.Text.Trim()).ToString();
                    break;
                default:
                    DV.RowFilter = cmbFld.Text.Trim() + " " + cmbOp.SelectedValue.ToString().Trim() + "'" + txtAccountName.Text.Trim() + "'";
                    break;
            }
            DataTable dt = DV.ToTable();
            if (dt.Rows.Count > 0 && dt.Rows[0]["ID"] + "".Trim() != "")
                search_By_ID(Convert.ToInt32(dt.Rows[0]["ID"]));
        }
    }
}
