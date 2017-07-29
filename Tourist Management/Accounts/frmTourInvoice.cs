using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
namespace Tourist_Management.Accounts
{
    public partial class frmTourInvoice : Form
    {
        int InsMode = 0;
        decimal Syscode = 0;
        int CompanyID = 0;
        private const string msghd = "Invoice";
        enum INV { ID, InvoiceID, Description, Amount, IsDeleted };
        public frmTourInvoice() { InitializeComponent(); }
        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
        private void rdbAmend_CheckedChanged(object sender, EventArgs e) { }
        private void btnPrint_Click(object sender, EventArgs e) { Print_Invoice(); }
        private void frmTourInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                grdPayments.Cols.Count = 5;
                grdPayments.Rows.Count = 50;
                grdPayments.Cols[(int)INV.ID].Width = 00;
                grdPayments.Cols[(int)INV.InvoiceID].Width = 00;
                grdPayments.Cols[(int)INV.Description].Width = 478;
                grdPayments.Cols[(int)INV.Amount].Width = 120;
                grdPayments.Cols[(int)INV.IsDeleted].Width = 00;
                grdPayments.Cols[(int)INV.ID].Caption = "ID";
                grdPayments.Cols[(int)INV.InvoiceID].Caption = "Invoice ID";
                grdPayments.Cols[(int)INV.Description].Caption = "Description";
                grdPayments.Cols[(int)INV.Amount].Caption = "Amount";
                grdPayments.Cols[(int)INV.IsDeleted].Caption = "Is Deleted";
                grdPayments.Cols[(int)INV.IsDeleted].DataType = Type.GetType("System.Boolean");
                grdPayments.Cols[(int)INV.Amount].Format = "####.##";
                grdPayments.Rows[1].AllowEditing = true;
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void Fill_Data()
        {
            try
            {
                string qry, val;
                DataTable DT; 
                qry="SELECT MAX(InvoiceNo)InvoiceNo FROM act_PaymentIssued WHERE TransID="+txtTourNo.Text+"";
                val= Classes.clsConnection.getSingle_Value_Using_Reader(qry);
                qry = "SELECT InvoiceID,TransID,Guest,InvoiceNo,PaidTo,AgentContSrNo,ComSrNo,Description,Date,CurrencyID,Amount,VAT,Rate, ISNULL(IsAmend,0)IsAmend,ISNULL(IsCancelled,0)IsCancelled FROM vw_trn_act_PaymentIssued_AGENT WHERE InvoiceNo='"+val.Trim()+"'";
                DT = Classes.clsGlobal.objCon.Fill_Table(qry);
                if(DT.Rows.Count==0 || DT.Rows[0]["TransID"]+"".Trim()=="")                    return;
                InsMode = 1;
                Syscode = Convert.ToDecimal(DT.Rows[0]["InvoiceID"]);
                txtTourNo.Text = DT.Rows[0]["TransID"]+"".Trim();
                lblBookingName.Text = DT.Rows[0]["Guest"] + "".Trim();
                lblInvoiceNo.Text = DT.Rows[0]["InvoiceNo"] + "".Trim();
                drpPaidTo.setSelectedValue(DT.Rows[0]["PaidTo"]+"".ToString().Trim());
                drpPaidTo_Selected_TextChanged(null, null);
                cmbAgentCont.SelectedValue = Convert.ToInt32(DT.Rows[0]["AgentContSrNo"]);
                drpBankBranch.setSelectedValue(DT.Rows[0]["ComSrNo"] + "".ToString().Trim());
                txtRemarks.Text = DT.Rows[0]["Description"] + "".Trim();
                dtpDate.Value = Convert.ToDateTime(DT.Rows[0]["Date"]);
                drpCurrnecy.setSelectedValue(DT.Rows[0]["CurrencyID"] + "".Trim());
                txtRate.Text = DT.Rows[0]["Rate"] + "".Trim();
                txtAmount.Text = DT.Rows[0]["Amount"] + "".Trim();
                txtVat.Text = DT.Rows[0]["VAT"] + "".Trim();
                rdbNormal.Checked = true;
                if(Convert.ToBoolean(DT.Rows[0]["IsAmend"]))                    rdbAmend.Checked = true;
                if (Convert.ToBoolean(DT.Rows[0]["IsCancelled"]))                    rdbCancel.Checked = true;
                grdPayments.Rows.Count = 1;
                grdPayments.Rows.Count = 50;
                qry = "SELECT ID,InvoiceID,Description,Amount FROM act_InvoiceDetails WHERE InvoiceID="+Syscode+" AND ISNULL(IsDeleted,0)<>1";
                DT = Classes.clsGlobal.objCon.Fill_Table(qry);
                if (DT.Rows.Count == 0 || DT.Rows[0]["ID"] + "".Trim() == "")
                    if (txtAmount.Text == "" || Convert.ToDecimal(txtAmount.Text) <= 0)
                        return;
                    else
                    {
                        grdPayments[1, (int)INV.InvoiceID] = Syscode;
                        grdPayments[1, (int)INV.Description] = "Tour Cost";
                        grdPayments[1, (int)INV.Amount] = txtAmount.Text;
                    }
                foreach (DataRow dr in DT.Rows)
                {
                    grdPayments[DT.Rows.IndexOf(dr) + 1, (int)INV.ID] = dr["ID"];
                    grdPayments[DT.Rows.IndexOf(dr) + 1, (int)INV.InvoiceID] = dr["InvoiceID"];
                    grdPayments[DT.Rows.IndexOf(dr) + 1, (int)INV.Description] = dr["Description"];
                    grdPayments[DT.Rows.IndexOf(dr) + 1, (int)INV.Amount] = dr["Amount"];
                } 
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
                private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql  = "SELECT ID,TourID,Guest,AgentID FROM trn_GroupAmendment";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.ToString().Trim() == "")                return;
            Fill_Control();
            Set_InvoiceNo();  
            Fill_Data();                      
        }
        private void Clear_Contents()
        {
            CompanyID = 0;
            lblAccountNo.Text = "";
            if (drpBankBranch.SelectedValue.ToString() != "")                drpBankBranch.setSelectedValue(null);
            if (drpPaidTo.SelectedValue.ToString() != "")                drpPaidTo.setSelectedValue("0");
                        dtpDate.Value = Classes.clsGlobal.CurDate(); 
            if (drpCurrnecy.SelectedValue.ToString() != "")                drpCurrnecy.setSelectedValue(null);
            txtRate.Text = "";
            txtAmount.Text = ""; 
            txtRemarks.Text = "";
        }
        private void Fill_Control()
        {
            try
            {
                if (txtTourNo.Text.ToString().Trim() == "")
                    return;
                DataTable[] DTB;
                DTB = new DataTable[4];
                dtpDate.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM vw_trn_act_PaymentParties Where TransID=" + txtTourNo.Text.Trim() + "");
                drpPaidTo.DataSource = DTB[0];
                DTB[1] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Code AS Currency FROM mst_Currency Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpCurrnecy.DataSource = DTB[1];
                string val = Classes.clsConnection.getSingle_Value_Using_Reader("SELECT CompID FROM trn_GroupAmendment WHERE ID=" + txtTourNo.Text.Trim() + "");
                DTB[2] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT SrNo,Identifier FROM vw_Company_Bank_Details WHERE CompanyID=" + val.Trim() + " ORDER BY Identifier");
                drpBankBranch.DataSource = DTB[2];
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void Set_InvoiceNo()
        {
            try
            {
                int InvID, UniqueNo;
                string compCode,val, InvNo;
                CompanyID = Convert.ToInt32(Classes.clsConnection.getSingle_Value_Using_Reader("SELECT CompID FROM trn_GroupAmendment WHERE ID="+txtTourNo.Text+""));
                compCode = Classes.clsConnection.getSingle_Value_Using_Reader( "SELECT CompanyCode FROM mst_CompanyGenaral WHERE ID=" + CompanyID + "").Trim();
                val = Classes.clsConnection.getSingle_Value_Using_Reader("SELECT ISNULL(InvoiceID,0)InvoiceID FROM dbo.act_PaymentIssued WHERE TransID=" + txtTourNo.Text + "");
                InvID = Convert.ToInt32(val.Trim()=="" ? "0" : val);
                val = Classes.clsConnection.getSingle_Value_Using_Reader( "SELECT ISNULL(MAX(UniqueID),0)UniqueID FROM dbo.act_PaymentIssued WHERE CompID=" + CompanyID + " GROUP BY CompID");
                UniqueNo = Convert.ToInt32(val.Trim() == "" ? "0" : val);
                if (UniqueNo == 0)                    UniqueNo = 1001;                else                    UniqueNo += 1;
                InvNo = (compCode + "/" + UniqueNo).Trim(); // + amendPart
                lblInvoiceNo.Text = InvNo.Trim();
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void drpBankBranch_Selected_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpBankBranch.SelectedValue != null)
                {
                    string identifier = drpBankBranch.SelectedText.ToString().Trim();
                    lblAccountNo.Text = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccountNo FROM vw_Company_Bank_Details WHERE SrNo=" + drpBankBranch.SelectedValue.ToString().Trim() + " AND Identifier LIKE '%" + identifier + "%'").Rows[0]["AccountNo"].ToString();
                }
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void grdPayments_Click(object sender, EventArgs e)        {            calculate_Total();        } 
        private void calculate_Total()
        {
            try
            {
                int row = 1;
                decimal tot=0;
                while (grdPayments[row,(int)INV.Description] + "".Trim() != "")
                {
                    if (Convert.ToBoolean(grdPayments[row, (int)INV.IsDeleted]))
                    {
                        row++;
                        continue;
                    }
                    if (grdPayments[row,(int)INV.Description] + "".Trim() != "")
                    {
                        if (grdPayments[row,(int)INV.Amount] + "".Trim() != "")
                        {
                            if (!Classes.clsGlobal.IsNumeric(grdPayments[row,(int)INV.Amount].ToString()))
                            {
                                MessageBox.Show("Please enter valid values for amount.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                tot = 0;
                                break;
                            }
                            tot += Convert.ToDecimal(grdPayments[row,(int)INV.Amount]);
                        }
                    }
                    row++;
                    if (grdPayments[row,(int)INV.Description] + "".Trim() == "")
                        break;
                }
                txtAmount.Text = tot.ToString();
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)               return;           
            calculate_Total();
            if (Validate_Data() && Save_Data() && Save_Details())
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (chkPrint.Checked)                              Print_Invoice();
                Fill_Data(); 
            }
        }
        private Boolean Validate_Data()
        {
                string M = "";
            try
            {
                if (drpPaidTo.SelectedValue.ToString() == "") M = "'INVOICE TO' Cannot Be Blank";
                if (cmbAgentCont.Text == "") M = "'Agent contact person' Cannot Be Blank";
                if (drpCurrnecy.SelectedValue.ToString() == "") M = "'CURRENCY' Cannot Be Blank";
                if (drpBankBranch.SelectedValue.ToString() == "") M = "'BANK BRANCH' Cannot Be Blank";
                if (txtAmount.Text.Trim() == "") M = "'AMOUNT' Cannot Be Blank";
                if (txtRate.Text.Trim() == "") M = "Please Enter Rate";
                if (M == "") return true;
                return false;
            }
            catch (Exception ex) { M = ex.Message; }
            MessageBox.Show(M, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
            try
            {
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_AgentInvoice";
                RtnVal = false;
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Syscode;
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = InsMode;
                sqlCom.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = lblInvoiceNo.Text;
                sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = txtTourNo.Text;
                sqlCom.Parameters.Add("@CompID", SqlDbType.Int).Value = CompanyID;
                sqlCom.Parameters.Add("@PaidTo", SqlDbType.Int).Value = drpPaidTo.SelectedValue.ToString().Trim();
                sqlCom.Parameters.Add("@AgentCont", SqlDbType.Int).Value = Convert.ToInt32(cmbAgentCont.SelectedValue);
                string SrNo = drpBankBranch.SelectedValue.ToString().Trim();
                string sql = "SELECT BranchID FROM vw_Company_Bank_Details WHERE CompanyID=" + CompanyID + " AND SrNo= " + SrNo + "";
                string val = Classes.clsConnection.getSingle_Value_Using_Reader(sql);
                val = val.Trim()=="" ? "0" : val;
                sqlCom.Parameters.Add("@BranchID", SqlDbType.Int).Value = Convert.ToInt32(val);
                sqlCom.Parameters.Add("@ComSrNo", SqlDbType.Int).Value = Convert.ToInt32(SrNo);
                sqlCom.Parameters.Add("@Date", SqlDbType.DateTime).Value = dtpDate.Value;
                sqlCom.Parameters.Add("@Currency", SqlDbType.Int).Value = Convert.ToInt32(drpCurrnecy.SelectedValue);
                sqlCom.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Convert.ToDecimal(txtRate.Text);
                sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAmount.Text);
                if (txtVat.Text.Trim() != "")
                    sqlCom.Parameters.Add("@VAT", SqlDbType.Decimal).Value = Convert.ToDecimal(txtVat.Text);
                else
                    sqlCom.Parameters.Add("@VAT", SqlDbType.Decimal).Value = 0;
                if(txtPaidAmt.Text.Trim()!="")
                    sqlCom.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtPaidAmt.Text);
                sqlCom.Parameters.Add("@IsAmend", SqlDbType.Int).Value = rdbAmend.Checked ? 1 : 0;                                
                sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = rdbCancel.Checked ? 1 : 0;
                sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = txtRemarks.Text;
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID);
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if(Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    InsMode = 1;                    
                    Syscode = Convert.ToDecimal(sqlCom.Parameters["@ID"].Value);
                    lblInvoiceNo.Text = Classes.clsConnection.getSingle_Value_Using_Reader("SELECT InvoiceID FROM act_PaymentIssued WHERE InvoiceID=" + Syscode + "");
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
        private Boolean Save_Details()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
            try
            {
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_AgentInvoiceDetails";
                int row = 1;
                while (grdPayments[row, (int)INV.Description] + "".Trim() != "")                 
                {
                    RtnVal = false;
                    sqlCom.Parameters.Clear();
                    if (grdPayments[row, (int)INV.ID] + "".Trim() == "")
                        sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                    else
                        sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToDouble(grdPayments[row, (int)INV.ID]);
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@InvoiceID", SqlDbType.BigInt).Value = Syscode;
                    grdPayments[row, (int)INV.InvoiceID] = Syscode;
                    sqlCom.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value = grdPayments[row, (int)INV.Description];
                    sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPayments[row, (int)INV.Amount]);
                    sqlCom.Parameters.Add("@IsDeleted", SqlDbType.Int).Value = Convert.ToBoolean(grdPayments[row, (int)INV.IsDeleted]) ? "1" : "0";
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                    {                        
                        grdPayments[row, (int)INV.ID] = Convert.ToDecimal(sqlCom.Parameters["@ID"].Value);                     
                        RtnVal = true;
                    }
                    row++;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void drpPaidTo_Click_Open(object sender, EventArgs e)
        {
            Transaction.frmGroupAmend frmGA;
            frmGA = new Transaction.frmGroupAmend();
            frmGA.Mode = 1;
            frmGA.SystemCode = Convert.ToDouble(txtTourNo.Text.Trim());
            frmGA.ShowDialog();
            Fill_Control();
        }
        private void drpPaidTo_Selected_TextChanged(object sender, EventArgs e)
        {
                int AgentID = 0;
                if (drpPaidTo.SelectedValue.ToString() == "")
                {
                    cmbAgentCont.Enabled = false;
                    btnAddCont.Enabled = false;
                    return;
                }
                else
                {
                    cmbAgentCont.Enabled = true;
                    btnAddCont.Enabled = true;
                }
                AgentID = Convert.ToInt32(drpPaidTo.SelectedValue.ToString());
                cmbAgentCont.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT SrNo,ContactName FROM mst_AgentContactsDetails Where AgentID=" + AgentID + " ORDER BY SrNo");
        }
        private void btnAddCont_Click(object sender, EventArgs e)
        { 
                drpPaidTo_Selected_TextChanged(null, null);
                int agentid;
                if (drpPaidTo.SelectedValue.ToString() != "")
                    agentid = Convert.ToInt32(drpPaidTo.SelectedValue.ToString());
                else
                {
                    agentid = 0;
                    MessageBox.Show("Agent Cannot Be Found", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Master.frmAgent frmAG;
                frmAG = new Master.frmAgent();
                frmAG.Mode = 1;
                frmAG.SystemCode = agentid;
                frmAG.StartPosition = FormStartPosition.CenterScreen;
                frmAG.ShowDialog(); 
        }
        private void drpBankBranch_Selected_TextChanged_1(object sender, EventArgs e)
        { 
                if (drpBankBranch.SelectedValue != null)
                {
                    string identifier = drpBankBranch.SelectedText.ToString().Trim();
                    lblAccountNo.Text = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccountNo FROM vw_Company_Bank_Details WHERE SrNo=" + drpBankBranch.SelectedValue.ToString().Trim() + " AND Identifier LIKE '%" + identifier + "%'").Rows[0]["AccountNo"].ToString();
                } 
        }
        private void Print_Invoice() {
            db.showReportExport(new Reports.TourInvoice(), "SELECT i.* ,ga.Guest FROM vInvoice i LEFT JOIN  vw_trn_GroupAmend ga ON i.TransID =ga.ID WHERE i.transID='" + txtTourNo.Text.Trim() + "'", "INVOICE", txtTourNo.Text.Trim(), Classes.clsGlobal.UserID, chkDetail.Checked); 
        
        }   
        private void drpCurrnecy_Selected_TextChanged(object sender, EventArgs e)
        {
            if (drpCurrnecy.SelectedValue + "".Trim() == "")                return;
            txtVat.Text = "0.00";
            if ( txtVat.Enabled = Convert.ToInt32(drpCurrnecy.SelectedValue) == 1)  txtRate.Text = "1";           
        }
        private void grdPayments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Do you really want to delete this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)                                   return;
                string val = grdPayments.Rows[grdPayments.Row][(int)INV.ID] + "".Trim();
                if (val != "" && val != "0")
                {
                    grdPayments.Rows[grdPayments.Row][(int)INV.IsDeleted] = true;
                    C1.Win.C1FlexGrid.CellStyle deleted = grdPayments.Styles.Add("deleted");
                    deleted.BackColor = ColorTranslator.FromHtml("#F78181");
                    grdPayments.Rows[grdPayments.Row].Style = grdPayments.Styles["deleted"];
                }
                else
                {
                    grdPayments.Rows.Remove(grdPayments.Row);
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                grdPayments.Rows[grdPayments.Row][(int)INV.IsDeleted] = false;
                C1.Win.C1FlexGrid.CellStyle undoDeleted = grdPayments.Styles.Add("undoDeleted");
                undoDeleted.BackColor = Color.Transparent;
                grdPayments.Rows[grdPayments.Row].Style = grdPayments.Styles["undoDeleted"];
            }
        }
    }
}
