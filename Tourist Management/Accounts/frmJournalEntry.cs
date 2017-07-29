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
    public partial class frmJournalEntry : Form
    {
        private const string msghd = "Journal Entry";
        string RefNo;
        Boolean bLoad = false;
        int InsMode=0;
        double Syscode = 0; 
        enum GRD { ID, accountTypeID, accountType, accountID, accountName, Memo, credit, debit, IsDeleted };
        public frmJournalEntry(){InitializeComponent();}
        private void btnClose_Click(object sender, EventArgs e){this.Close();}
        private void frmJournalEntry_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        public void Intializer()
        {
            try
            {
                RefNo = "";
                lblCredit.Text = "";
                lblDebit.Text = "";
                Fill_Control();
                Grd_Initializer();
                Set_Voucher_No();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            { 
                cmbCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
                dtpPaidDate.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                drpPayableTo.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT MAX(ID),Name FROM vw_ALL_PERSON_DETAILS WHERE Name<>'' GROUP BY Name ORDER BY Name");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                grdPay.Cols.Count = 9;
                grdPay.Rows.Count = 100;
                grdPay.Cols[(int)GRD.ID].Width = 00;
                grdPay.Cols[(int)GRD.ID].Caption = "ID";
                grdPay.Cols[(int)GRD.accountTypeID].Width = 00;
                grdPay.Cols[(int)GRD.accountTypeID].Caption = "Account Type ID";
                grdPay.Cols[(int)GRD.accountType].Width = 121;
                grdPay.Cols[(int)GRD.accountType].Caption = "Account Type";
                grdPay.Cols[(int)GRD.accountID].Width = 00;
                grdPay.Cols[(int)GRD.accountID].Caption = "Account Name ID";
                grdPay.Cols[(int)GRD.accountName].Width = 117;
                grdPay.Cols[(int)GRD.accountName].Caption = "Account Name";
                grdPay.Cols[(int)GRD.Memo].Width = 170;
                grdPay.Cols[(int)GRD.Memo].Caption = "Memo";
                grdPay.Cols[(int)GRD.credit].Width = 85;
                grdPay.Cols[(int)GRD.credit].Caption = "Credit";
                grdPay.Cols[(int)GRD.debit].Width = 85;
                grdPay.Cols[(int)GRD.debit].Caption = "Debit";
                grdPay.Cols[(int)GRD.IsDeleted].Width = 0;
                grdPay.Cols[(int)GRD.IsDeleted].Caption = "Is Deleted";
                grdPay.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                grdPay.Cols[(int)GRD.credit].Format = "##.##";
                grdPay.Cols[(int)GRD.debit].Format = "##.##";
                grdPay.Cols[(int)GRD.IsDeleted].DataType = Type.GetType(" System.Boolean");
                grdPay.Cols[(int)GRD.accountType].ComboList = "...";
                grdPay.Cols[(int)GRD.accountName].ComboList = "...";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public void Set_Voucher_No()
        {
            try
            {
                int compID = Convert.ToInt32(cmbCompany.SelectedValue);
                string code = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CompanyCode FROM mst_CompanyGenaral WHERE ID=" + compID + "").Rows[0]["CompanyCode"].ToString().Trim();
                string SqlQuery = "SELECT MAX(UniqueID)ID FROM act_CashPayment WHERE CompID=" + compID + " AND Type='JEN' GROUP BY [Type]";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                if (DT.Rows.Count > 0 && DT.Rows[0]["ID"] + "".Trim() != "")
                {
                    RefNo = (Convert.ToDouble(DT.Rows[0]["ID"]) + 1).ToString().Trim();
                }
                else
                {
                    RefNo = "1001".Trim();
                }
                RefNo = (code + "/" + "JEN" + "/" + RefNo).Trim();
                txtRefNo.Text = RefNo.Trim();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean calculate_Total()
        {
            try
            {
                int RowNumb = 1;
                decimal totCredit = 0.00m, totDebit = 0.00m;
                while (grdPay[RowNumb, (int)GRD.accountTypeID] + "".Trim() != "")
                {
                    if (grdPay[RowNumb, (int)GRD.credit] + "".Trim() != "")
                        totCredit += Convert.ToDecimal(grdPay[RowNumb, (int)GRD.credit]);
                    if (grdPay[RowNumb, (int)GRD.debit] + "".Trim() != "")
                        totDebit += Convert.ToDecimal(grdPay[RowNumb, (int)GRD.debit]);
                    RowNumb++;
                }                
                lblCredit.Text = totCredit.ToString().Trim();
                lblDebit.Text = totDebit.ToString().Trim();
                lblDifCredit.Visible = false;
                lblDifDebit.Visible = false;
                if (totCredit == totDebit)
                {
                    lblCredit.ForeColor = Color.Green;
                    lblDebit.ForeColor = Color.Green;
                    return true;
                }
                else
                {
                    lblCredit.ForeColor = Color.Red;
                    lblDebit.ForeColor = Color.Red;
                    if (totDebit > totCredit)
                    {
                        lblDifDebit.Visible = true;
                        lblDifDebit.Text = (totDebit - totCredit).ToString().Trim();
                    }
                    else
                    {
                        lblDifCredit.Visible = true;
                        lblDifCredit.Text = (totCredit - totDebit).ToString().Trim();
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void btnTour_Click(object sender, EventArgs e)
        {          
                string  sql = "SELECT ID,TourID,Guest,AgentID FROM vw_TourBasics WHERE ISNULL(IsCancelled,0)<>1";// AND CompID='"+cmbCompany.SelectedValue.ToString().Trim()+"'";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
                txtTourNo.Text = finder.Load_search(DT);
                if (txtTourNo.Text.Trim() == "")
                {
                    lblGuest.Text = "";
                    return;
                }
                if (txtTourNo.Text.Trim() == "")
                    return;
                sql = "SELECT ID,TourID,Guest,AgentID FROM vw_TourBasics WHERE ID=" + txtTourNo.Text.Trim() + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count == 0 && DT.Rows[0]["ID"] + "".Trim() == "")
                    return;
                lblGuest.Text = DT.Rows[0]["Guest"] + "".Trim();
                cmbCompany.Enabled = true;
                string compID = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CompID FROM trn_GroupAmendment WHERE ID=" + txtTourNo.Text.Trim() + "").Rows[0]["CompID"] + "".Trim();
                if (compID != "")
                {
                    cmbCompany.SelectedValue = compID.Trim();
                    cmbCompany.Enabled = false;
                }
        }
        private void grdPay_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                if (e.Col == (int)GRD.credit)
                {
                    if (grdPay[grdPay.Row, (int)GRD.credit] + "".Trim() != "" && !Classes.clsGlobal.IsNumeric(grdPay[grdPay.Row, (int)GRD.credit].ToString().Trim()))
                    {
                        MessageBox.Show("Please enter a valid credit value.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdPay[grdPay.Row, (int)GRD.credit] = "0.00".Trim();
                        return;
                    }   
                     if (grdPay[grdPay.Row, (int)GRD.credit] + "".Trim() != "")
                         grdPay[grdPay.Row, (int)GRD.debit] = null;
                }
                if (e.Col == (int)GRD.debit)
                {
                    if (grdPay[grdPay.Row, (int)GRD.debit] + "".Trim() != "" && !Classes.clsGlobal.IsNumeric(grdPay[grdPay.Row, (int)GRD.debit].ToString().Trim()))
                    {
                        MessageBox.Show("Please enter a valid debit value.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdPay[grdPay.Row, (int)GRD.debit] = "0.00".Trim();
                        return;
                    }
                    if (grdPay[grdPay.Row, (int)GRD.debit] + "".Trim() != "")
                        grdPay[grdPay.Row, (int)GRD.credit] = null;
                }
                if (calculate_Total()) { }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdPay_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTAccType, DTAcc;
                string SqlQuery;
                if (e.Col == grdPay.Cols[(int)GRD.accountType].Index)
                {
                    SqlQuery = "SELECT ID,AccountType [Type] FROM comAcc_AccountTypes Where ID<=16";
                    DTAccType = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTAccType;
                    frm.SubForm = new Accounts.frmChartOfAccount();
                    frm.Width = grdPay.Cols[(int)GRD.accountType].Width;
                    frm.Height = grdPay.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdPay);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdPay[grdPay.Row, (int)GRD.accountTypeID] = SelText[0];
                        grdPay[grdPay.Row, (int)GRD.accountType] = SelText[1];
                        if (grdPay[grdPay.Row, (int)GRD.ID] + "".Trim() == "")
                            grdPay[grdPay.Row, (int)GRD.ID] = 0;
                    }
                    grdPay[grdPay.Row, (int)GRD.accountID] = "";
                    grdPay[grdPay.Row, (int)GRD.accountName] = "";
                    grdPay[grdPay.Row, (int)GRD.Memo] = "";
                }
                else if (e.Col == grdPay.Cols[(int)GRD.accountName].Index)
                {
                    int accountType;
                    SqlQuery = "SELECT ID,AccountType [Name] FROM comAcc_AccountTypes";
                    if (grdPay[grdPay.Row, (int)GRD.accountTypeID] + "".Trim() != "")
                    {
                        accountType = Convert.ToInt32(grdPay[grdPay.Row, (int)GRD.accountTypeID]);
                        SqlQuery += " Where AccountTypeID=" + accountType + "";
                    }
                    DTAcc = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTAcc;
                    frm.SubForm = new Accounts.frmChartOfAccount();
                    frm.Width = grdPay.Cols[(int)GRD.accountName].Width;
                    frm.Height = grdPay.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdPay);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdPay[grdPay.Row, (int)GRD.accountID] = SelText[0];
                        grdPay[grdPay.Row, (int)GRD.accountName] = SelText[1];
                        grdPay[grdPay.Row, (int)GRD.Memo] = SelText[1];
                        int accTypeID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccountTypeID FROM comAcc_AccountTypes WHERE ID=" + SelText[0] + "").Rows[0][0]);
                        SqlQuery = "SELECT ID,AccountType FROM comAcc_AccountTypes WHERE AccountTypeID=" + accTypeID + "";
                        DTAccType = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        grdPay[grdPay.Row, (int)GRD.accountTypeID] = DTAccType.Rows[0]["ID"];
                        grdPay[grdPay.Row, (int)GRD.accountType] = DTAccType.Rows[0]["AccountType"];
                        if (grdPay[grdPay.Row, (int)GRD.ID] + "".Trim() == "")
                            grdPay[grdPay.Row, (int)GRD.ID] = 0;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdPay_Click(object sender, EventArgs e)
        {
            if (calculate_Total()) { }
        }
        private void grdPay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Do you really want to delete this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
                grdPay.Rows[grdPay.Row][(int)GRD.credit] = "0.00".Trim();
                grdPay.Rows[grdPay.Row][(int)GRD.debit] = "0.00".Trim();
                string val = grdPay.Rows[grdPay.Row][(int)GRD.ID] + "".Trim();
                if (val != "" && val != "0")
                {
                    grdPay.Rows[grdPay.Row][(int)GRD.IsDeleted] = true;
                    C1.Win.C1FlexGrid.CellStyle deleted = grdPay.Styles.Add("deleted");
                    deleted.BackColor = ColorTranslator.FromHtml("#F78181");
                    grdPay.Rows[grdPay.Row].Style = grdPay.Styles["deleted"];
                }
                else
                {
                    grdPay.Rows.Remove(grdPay.Row);
                    grdPay.Rows[1].AllowEditing = true;
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                grdPay.Rows[grdPay.Row][(int)GRD.IsDeleted] = false;
                C1.Win.C1FlexGrid.CellStyle undoDeleted = grdPay.Styles.Add("undoDeleted");
                undoDeleted.BackColor = Color.Transparent;
                grdPay.Rows[grdPay.Row].Style = grdPay.Styles["undoDeleted"];
            }
        }
        private void grdPay_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true || grdPay.Rows.Count == 1)
            {
                return;
            }
            grdPay.Rows[1].AllowEditing = true;
            if (grdPay.Row != 1 && grdPay[grdPay.Row - 1, 0] == null)
            {
                grdPay.Rows[grdPay.Row].AllowEditing = false;
            }
            else
            {
                grdPay.Rows[grdPay.Row].AllowEditing = true;
            }
        }
        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Set_Voucher_No();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                InsMode = 1;
                Fill_Data();
            }
        }
        private void Fill_Data()
        {
            try
            {
                #region BASICS
                string SqlQuery = "SELECT ID,TransID,CompID,PayableTo,PaidDate,RefNo," +
                                  "Description,ChkNo,ChkDate,Payee,ISNULL(IsCancelled,0)IsCancelled," +
                                  "ISNULL(IsConfirm,0)IsConfirm,ISNULL(IsCompleted,0)IsCompleted" +
                                  " FROM act_CashPayment Where ID=" + Syscode + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                btnOk.Enabled = true;
                cmbCompany.Enabled = false;
                rdbCancel.Checked = false;
                if (DT.Rows[0]["TransID"] + "".Trim() != "")
                {
                    btnTour.Enabled = false;
                }
                if (Convert.ToBoolean(DT.Rows[0]["IsCancelled"]))
                {
                    rdbCancel.Checked = true;
                    lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "THIS HAS BEEN CANCELLED";
                    btnOk.Enabled = false;
                }
                cmbCompany.SelectedValue = Convert.ToInt32(DT.Rows[0]["CompID"]);
                RefNo = DT.Rows[0]["RefNo"]+"".Trim();
                txtRefNo.Text = RefNo;
                drpPayableTo.setSelectedValue(DT.Rows[0]["PayableTo"].ToString().Trim());
                dtpPaidDate.Value = Convert.ToDateTime(DT.Rows[0]["PaidDate"]);
                txtDescription.Text = DT.Rows[0]["Description"]+"".Trim();
                #endregion
                #region DETAIL
                SqlQuery = "SELECT ID,CashPayID,AccountTypeID,AccountType,AccountNameID,AccountName," +
                           "IouID,VoucherID,Memo,Debit,Credit"+
                           " FROM vw_act_Payment_Details Where CashPayID=" + Syscode + "" +
                           " AND ISNULL(IsDeleted,0)<>1 ORDER BY SrNo";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                int RowNumb = 0;
                if (DT.Rows.Count > 0 && DT.Rows[0][0] + "".Trim() != "")
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdPay.Rows[RowNumb + 1][(int)GRD.ID] = DT.Rows[RowNumb]["ID"];
                        grdPay.Rows[RowNumb + 1][(int)GRD.accountTypeID] = DT.Rows[RowNumb]["AccountTypeID"];
                        grdPay.Rows[RowNumb + 1][(int)GRD.accountType] = DT.Rows[RowNumb]["AccountType"];
                        grdPay.Rows[RowNumb + 1][(int)GRD.accountID] = DT.Rows[RowNumb]["AccountNameID"];
                        grdPay.Rows[RowNumb + 1][(int)GRD.accountName] = DT.Rows[RowNumb]["AccountName"];
                        if (DT.Rows[RowNumb]["Debit"] + "".Trim() != "")
                            grdPay.Rows[RowNumb + 1][(int)GRD.debit] = DT.Rows[RowNumb]["Debit"];
                        if (DT.Rows[RowNumb]["Credit"] + "".Trim() != "")
                            grdPay.Rows[RowNumb + 1][(int)GRD.credit] = DT.Rows[RowNumb]["Credit"];
                        grdPay.Rows[RowNumb + 1][(int)GRD.Memo] = DT.Rows[RowNumb]["Memo"];                        
                        RowNumb++;
                    }
                #endregion
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                btnOk.Enabled = false;
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
                int RowNumb = 1;
                string accType;
                if (!calculate_Total())
                {
                    MessageBox.Show("Unbalance in Credit And Debit Amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                while (grdPay[RowNumb, (int)GRD.accountTypeID] + "".Trim() != "")
                {
                    if (grdPay[RowNumb, (int)GRD.accountID] + "".Trim() == "")
                    {
                        accType = grdPay[RowNumb, (int)GRD.accountID].ToString().Trim();
                        MessageBox.Show("Please select an account for " + accType + "", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdPay.Select(RowNumb, (int)GRD.accountName);
                        return false;
                    }
                    if (grdPay[RowNumb, (int)GRD.credit] + "".Trim() == "" && grdPay[RowNumb, (int)GRD.debit] + "".Trim() == "")
                    {
                        MessageBox.Show("Please enter CREDIT or DEBIT amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdPay.Select(RowNumb, (int)GRD.credit);
                        return false;
                    }
                    else
                    {
                        if (grdPay[RowNumb, (int)GRD.credit] + "".Trim() != "")
                            if (!Classes.clsGlobal.IsNumeric(grdPay[RowNumb, (int)GRD.credit] + "".Trim()))
                            {
                                MessageBox.Show("Please enter valid credit amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                grdPay.Select(RowNumb, (int)GRD.credit);
                                return false;
                            }
                        if (grdPay[RowNumb, (int)GRD.debit] + "".Trim() != "")
                            if (!Classes.clsGlobal.IsNumeric(grdPay[RowNumb, (int)GRD.debit] + "".Trim()))
                            {
                                MessageBox.Show("Please enter valid debit amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                grdPay.Select(RowNumb, (int)GRD.debit);
                                return false;
                            }
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
                if (Save_Basic_Details(sqlCom) == false)
                    return false;
                if (Save_Pay_Details(sqlCom) == false)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Basic_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;            
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_act_JournalEntry";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Syscode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@Type", SqlDbType.NChar, 10).Value = "JEN";
                sqlCom.Parameters.Add("@CompID", SqlDbType.Int).Value = Convert.ToInt32(cmbCompany.SelectedValue.ToString());
                if (txtTourNo.Text.Trim() != "")
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTourNo.Text);
                if(drpPayableTo.SelectedValue+"".Trim()!="")
                    sqlCom.Parameters.Add("@PayableTo", SqlDbType.Decimal).Value = Convert.ToDecimal(drpPayableTo.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = dtpPaidDate.Value;
                sqlCom.Parameters.Add("@RefNo", SqlDbType.NVarChar, 50).Value = txtRefNo.Text;
                sqlCom.Parameters["@RefNo"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@Description", SqlDbType.NVarChar, 200).Value = txtDescription.Text;
                sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = rdbCancel.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = InsMode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    Syscode = Convert.ToDouble(sqlCom.Parameters["@ID"].Value);
                    RefNo = sqlCom.Parameters["@RefNo"].Value.ToString();
                    txtRefNo.Text = RefNo.Trim();
                    RtnVal = true;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Pay_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_act_JournalEntry_Detail";
                RowNumb = 1;
                while (grdPay[RowNumb, (int)GRD.accountTypeID] + "".Trim() != "")
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.ID]);
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@CashPayID", SqlDbType.Decimal).Value = Syscode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@AccountNameID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.accountID]);
                    sqlCom.Parameters.Add("@Memo", SqlDbType.NVarChar, 100).Value = grdPay[RowNumb, (int)GRD.Memo] + "".Trim();
                    if (grdPay[RowNumb, (int)GRD.credit] + "".Trim() != "")
                        sqlCom.Parameters.Add("@Credit", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.credit]);
                    if (grdPay[RowNumb, (int)GRD.debit] + "".Trim() != "")
                        sqlCom.Parameters.Add("@Debit", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.debit]);
                    sqlCom.Parameters.Add("@IsDeleted", SqlDbType.Int).Value = Convert.ToBoolean(grdPay[RowNumb, (int)GRD.IsDeleted]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                    }
                    RowNumb++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
    }
}
