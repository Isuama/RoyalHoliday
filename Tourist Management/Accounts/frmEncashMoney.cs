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
    public partial class frmEncashMoney : Form
    {
        private const string msghd = "Encash Foriegn Currency";
        enum EN { ID, CashID, RecNo, Currency, Amount, Balance, ExAmount, ExRate, ExAccountID, ExAccount, ExDate };
        public double CashID = 0;
        public frmEncashMoney(){InitializeComponent();}
        private void frmEncashMoney_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                fill_Data();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                grdCash.Cols.Count = 11;
                grdCash.Rows.Count = 1;
                grdCash.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                grdCash.Cols[(int)EN.ID].Width = 0;
                grdCash.Cols[(int)EN.CashID].Width = 0;
                grdCash.Cols[(int)EN.RecNo].Width = 100;
                grdCash.Cols[(int)EN.Currency].Width = 81;
                grdCash.Cols[(int)EN.Amount].Width = 92;
                grdCash.Cols[(int)EN.Balance].Width = 93;
                grdCash.Cols[(int)EN.ExAccountID].Width = 0;
                grdCash.Cols[(int)EN.ExAccount].Width = 100;
                grdCash.Cols[(int)EN.ExAmount].Width = 92;
                grdCash.Cols[(int)EN.ExRate].Width = 86;
                grdCash.Cols[(int)EN.ExDate].Width = 83;                                
                grdCash.Cols[(int)EN.ID].Caption = "ID";
                grdCash.Cols[(int)EN.CashID].Caption = "Cash ID";
                grdCash.Cols[(int)EN.RecNo].Caption = "Receipt No";
                grdCash.Cols[(int)EN.Currency].Caption = "Currency";
                grdCash.Cols[(int)EN.Amount].Caption = "Amount";
                grdCash.Cols[(int)EN.Balance].Caption = "Balance";
                grdCash.Cols[(int)EN.ExAccountID].Caption = "Enc. Account ID";
                grdCash.Cols[(int)EN.ExAccount].Caption = "Enc. Account ";
                grdCash.Cols[(int)EN.ExAmount].Caption = "Enc. Amount";
                grdCash.Cols[(int)EN.ExRate].Caption = "Exch. Rate";
                grdCash.Cols[(int)EN.ExDate].Caption = "Enc. Date";
                grdCash.Cols[(int)EN.ExDate].DataType = Type.GetType("System.DateTime");
                grdCash.Cols[(int)EN.ExAccount].ComboList = "...";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void fill_Data()
        {
            try
            {
                decimal amt=0, rec=0;
                string SqlQuery = "SELECT ID,CashID,ReceiptNo,Currency,Amount,AccountID," +
                "AccountName,ExAmount,ExRate,ExDate FROM vw_EncashMoney WHERE CashID=" + CashID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                grdCash.Rows.Count = 1;
                grdCash.Rows.Count = DT.Rows.Count+2;
                int rowVal = 0;
                foreach (DataRow dr in DT.Rows)
                {
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.ID] = dr["ID"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.CashID] = dr["CashID"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.RecNo] = dr["ReceiptNo"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.Currency] = dr["Currency"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.Amount] = dr["Amount"];
                    amt = Convert.ToDecimal(dr["Amount"]);
                    rec += Convert.ToDecimal(dr["ExAmount"]);
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.Balance] = (Convert.ToDecimal(dr["Amount"]) - Convert.ToDecimal(dr["ExAmount"])).ToString();
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.ExAccountID] = dr["AccountID"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.ExAccount] = dr["AccountName"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.ExAmount] = dr["ExAmount"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.ExRate] = dr["ExRate"];
                    grdCash.Rows[DT.Rows.IndexOf(dr) + 1][(int)EN.ExDate] = dr["ExDate"];
                    rowVal = DT.Rows.IndexOf(dr)+1;
                }
                rowVal++;
                if (rec > 0 && DT.Rows.Count>0)
                {
                    if(DT.Rows[0]["ID"]+"".Trim()=="")
                        return;
                    grdCash.Rows[rowVal][(int)EN.ID] = 0;
                    grdCash.Rows[rowVal][(int)EN.CashID] = DT.Rows[0]["CashID"];
                    grdCash.Rows[rowVal][(int)EN.RecNo] = DT.Rows[0]["ReceiptNo"];
                    grdCash.Rows[rowVal][(int)EN.Currency] = DT.Rows[0]["Currency"];
                    grdCash.Rows[rowVal][(int)EN.Amount] = DT.Rows[0]["Amount"];
                    grdCash.Rows[rowVal][(int)EN.Balance] = (amt - rec).ToString();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {                
                fill_Data();                
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
                int row=1;
                decimal amt=0,exc=0;
                while (grdCash.Rows[row][(int)EN.ID] + "".Trim() != "")
                {
                    if (grdCash.Rows[row][(int)EN.ExAccountID] + "".Trim() == "")
                    {
                        MessageBox.Show("'Encash Account' cannot be blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (grdCash.Rows[row][(int)EN.ExAmount] + "".Trim() == ""  || Convert.ToDecimal(grdCash.Rows[row][(int)EN.ExAmount])<=0)
                    {
                        MessageBox.Show("'Encash Amount' cannot be blank or zero.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (grdCash.Rows[row][(int)EN.ExRate] + "".Trim() == "" || Convert.ToDecimal(grdCash.Rows[row][(int)EN.ExRate]) <= 0)
                    {
                        MessageBox.Show("'Encash Rate' cannot be blank or zero.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (grdCash.Rows[row][(int)EN.ExDate] + "".Trim() == "")
                    {
                        MessageBox.Show("'Encash Date' cannot be blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    amt = Convert.ToDecimal(grdCash.Rows[row][(int)EN.Amount]);
                    exc += Convert.ToDecimal(grdCash.Rows[row][(int)EN.ExAmount]);
                    row++;
                    if (grdCash.Rows.Count <= row)
                        break;
                }
                if (amt < exc)
                {
                    MessageBox.Show("'Encash Amount' cannot be greater than 'Received Amount'", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                    MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (Save_Encash_Details(sqlCom) == false)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Encash_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_act_EncashMoney";
                int row=1;
                while (grdCash.Rows[row][(int)EN.ID] + "".Trim() != "")
                {
                    sqlCom.Parameters.Clear();
                    RtnVal = false;
                    sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToDecimal(grdCash.Rows[row][(int)EN.ID]);
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@CashID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCash.Rows[row][(int)EN.CashID]);
                    sqlCom.Parameters.Add("@AccountID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCash.Rows[row][(int)EN.ExAccountID]);
                    sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCash.Rows[row][(int)EN.ExAmount]);
                    sqlCom.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCash.Rows[row][(int)EN.ExRate]);
                    sqlCom.Parameters.Add("@Date", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCash.Rows[row][(int)EN.ExDate]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                    {
                        RtnVal = true;
                        grdCash.Rows[row][(int)EN.ID] = Convert.ToDouble(sqlCom.Parameters["@ID"].Value);
                    }
                    row++;
                    if (grdCash.Rows.Count <= row)
                        break;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void grdCash_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTAcc;
                string SqlQuery;
                if (e.Col == grdCash.Cols[(int)EN.ExAccount].Index)
                {
                    SqlQuery = "SELECT ID,AccountType [Name] FROM comAcc_AccountTypes WHERE AccountTypeID=1 AND ID<>1 AND ISNULL(IsActive,0)=1";
                    DTAcc = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTAcc;
                    frm.SubForm = new Accounts.frmChartOfAccount();
                    frm.Width = grdCash.Cols[(int)EN.ExAccount].Width;
                    frm.Height = grdCash.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCash);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdCash[grdCash.Row, (int)EN.ExAccountID] = SelText[0];
                        grdCash[grdCash.Row, (int)EN.ExAccount] = SelText[1];
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdCash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdCash.Rows[grdCash.Row][(int)EN.ExAmount] = "0.00".Trim();
                string val = grdCash.Rows[grdCash.Row][(int)EN.ID] + "".Trim();
                if (val != "" && val != "0")
                {
                    C1.Win.C1FlexGrid.CellStyle deleted = grdCash.Styles.Add("deleted");
                    deleted.BackColor = ColorTranslator.FromHtml("#F78181");
                    grdCash.Rows[grdCash.Row].Style = grdCash.Styles["deleted"];
                }
                else
                {
                    grdCash.Rows.Remove(grdCash.Row);
                    grdCash.Rows[1].AllowEditing = true;
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                C1.Win.C1FlexGrid.CellStyle undoDeleted = grdCash.Styles.Add("undoDeleted");
                undoDeleted.BackColor = Color.Transparent;
                grdCash.Rows[grdCash.Row].Style = grdCash.Styles["undoDeleted"];
            }
        }
    }
}
