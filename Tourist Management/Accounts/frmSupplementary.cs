using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
namespace Tourist_Management.Accounts
{   
    public partial class frmSupplementary : Form
    {
        private const string msghd = "Supplimentary Details";
        public string SqlQry = "SELECT DISTINCT TransID,TourID,Guest,CurrencyDescription,Rate,Amount From vw_Supplementary_Invoice Where Isnull([Status],0)<>7 AND ISNULL(TransID,0)<>0 AND Type='SUP' Order By TourID";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        int RetrieveNo = 0, AmendNo = 0, CompanyID=0, AgtID=0;
        string AgentName = "", ClientName = "" ;
        bool DidChange = false; 
        bool IsRetrive = false;
        enum IG { gTID, gINO, gPID, gPNM, gCLNM, gONM, gINVT, gACI, gACN, gBID, gCSN, gICH, gDTE, gCNO, gCHD, gCID, gCNM, gRAT, gAMT, gPAM, gDES, gCBY, gCDT, gLMB, gLMD, gAMDT, gCNCL };
        public frmSupplementary(){InitializeComponent();}
        private void frmSupplementary_Load(object sender, EventArgs e)
        {
            Initializer();
        }
        private void Initializer()
        {
            try
            {
                Grid_Initializer();
                if (Mode != 0)
                {
                    Fill_Data();
                    btnPrint.Enabled = true;
                    btnEmail.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Fill_Control()
        {
            try 
            {
                if (txtTourNo.Text.ToString().Trim() == "")
                    return;
                DataTable[] DTB;
                DTB = new DataTable[3];
                dtpDate.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Code AS Currency FROM mst_Currency Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpCurrnecy.DataSource = DTB[0];
                DTB[1] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT SrNo,Identifier FROM vw_Company_Bank_Details WHERE CompanyID=" + CompanyID + " ORDER BY Identifier");
                drpBankBranch.DataSource = DTB[1];
                DTB[2] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM vw_trn_act_PaymentParties Where TransID=" + txtTourNo.Text.Trim() + "");
                drpInvoiceTo.DataSource = DTB[2];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Grid_Initializer()
        {
            try
            {
                db.GridInit(grdPayments, false, IG.gTID, 0, "Tour ID", IG.gINO, 100, "Invoice No", IG.gPID, 0, "Paid ID", IG.gPNM, 240, "Invoice To", IG.gINVT, 0, "Invoice To Cat", IG.gONM, 0, "Other Name", IG.gBID, 0, "Company Branch", IG.gCSN, 0, "Company Branch Sort Number", IG.gICH, 0, "Is Cash", Type.GetType("System.Boolean"), IG.gDTE, 0, "Date", IG.gCNO, 0, "Cheque No", IG.gCHD, 0, "Cheque Date", IG.gCID, 0, "Currency ID", IG.gCNM, 90, "Currency", IG.gACI, 0, "Contact ID", IG.gACN, 106, "Contact Person", IG.gRAT, 0, "Rate", IG.gAMT, 84, "Amount", IG.gPAM, 84, "Paid", IG.gDES, 0, "Description", IG.gCBY, 0, "Created By", IG.gCDT, 0, "Created Date", IG.gLMB, 0, "Last Modified By", IG.gLMD, 0, "Last Modified Date", IG.gAMDT, 0, "Amendment Time", IG.gCNCL, 0, "IsCancel", Type.GetType("System.Boolean"), IG.gCLNM, "Client Name"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Clear_Contents()
        {
            CompanyID = 0;
            lblAccountNo.Text = "";
            if (drpBankBranch.SelectedValue.ToString() != "")  drpBankBranch.setSelectedValue(null);
            if (cmbInvoiceTo.SelectedValue+"".Trim() != "")  cmbInvoiceTo.SelectedItem = null;
            dtpDate.Value = Classes.clsGlobal.CurDate();
            if (drpCurrnecy.SelectedValue.ToString() != "")       drpCurrnecy.setSelectedValue(null);
            txtRate.Text =      txtAmount.Text =      txtPaidAmt.Text =  txtRemarks.Text = "";
        }
        private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID FROM vw_TourBasics";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.ToString().Trim() == "")
                return;
            SystemCode = Convert.ToInt32(txtTourNo.Text.ToString().Trim()); //Convert.ToDecimal(txtTourNo.Text.ToString().Trim());
            Fill_Data();
            Increase_Invoice_No();
            Fill_Control();
        }
        private void Fill_Data()
        {
            try
            {
                CompanyID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CompID FROM trn_GroupAmendment WHERE ID=" + SystemCode + "").Rows[0][0].ToString());
                int RowNumb;
                SqlQry = "SELECT DISTINCT InvoiceNo,TransID,InvoiceNo,IsCash,[Date],ChkNo,ChkDate,CurrencyID,Currency," +
                       "AgentID, AgentName,AgentSrNo,AgentCont,AGContactName,ClientName,OtherName,Guest,InvoiceTo,BranchID,"+
                       "ComSrNo,Rate,Amount,PaidAmount,IsAmend,AmendTime,[Description],CreatedBy,CreatedDate,SrNo" +
                       " FROM vw_Supplementary_Invoice WHERE TransID=" + SystemCode + " AND Type='SUP' ";
                DataTable DTPay = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                if (DTPay.Rows.Count > 0)
                {
                    RowNumb = 0;
                    Mode = 1;
                    while (DTPay.Rows.Count > RowNumb)
                    {
                        grdPayments[RowNumb + 1, (int)IG.gTID] = DTPay.Rows[RowNumb]["TransID"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gINO] = DTPay.Rows[RowNumb]["InvoiceNo"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gDTE] = DTPay.Rows[RowNumb]["Date"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCNO] = DTPay.Rows[RowNumb]["ChkNo"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCDT] = DTPay.Rows[RowNumb]["ChkDate"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCID] = DTPay.Rows[RowNumb]["CurrencyID"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCNM] = DTPay.Rows[RowNumb]["Currency"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gPID] = DTPay.Rows[RowNumb]["AgentID"].ToString();
                        AgtID = Convert.ToInt32(DTPay.Rows[RowNumb]["AgentID"]);
                        grdPayments[RowNumb + 1, (int)IG.gPNM] = DTPay.Rows[RowNumb]["AgentName"].ToString();
                        AgentName = DTPay.Rows[RowNumb]["AgentName"].ToString();    //AGContactName
                        grdPayments[RowNumb + 1, (int)IG.gACI] = DTPay.Rows[RowNumb]["AgentSrNo"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gACN] = DTPay.Rows[RowNumb]["AGContactName"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gBID] = DTPay.Rows[RowNumb]["BranchID"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCSN] = DTPay.Rows[RowNumb]["ComSrNo"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gRAT] = DTPay.Rows[RowNumb]["Rate"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gAMT] = DTPay.Rows[RowNumb]["Amount"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gPAM] = DTPay.Rows[RowNumb]["PaidAmount"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gDES] = DTPay.Rows[RowNumb]["Description"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCBY] = DTPay.Rows[RowNumb]["CreatedBy"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCDT] = DTPay.Rows[RowNumb]["CreatedDate"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gAMDT] = DTPay.Rows[RowNumb]["AmendTime"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gCLNM] = DTPay.Rows[RowNumb]["Guest"].ToString();
                        ClientName = DTPay.Rows[RowNumb]["Guest"].ToString();       // Client Name
                        grdPayments[RowNumb + 1, (int)IG.gONM] = DTPay.Rows[RowNumb]["OtherName"].ToString();
                        grdPayments[RowNumb + 1, (int)IG.gINVT] = DTPay.Rows[RowNumb]["InvoiceTo"].ToString();
                        RowNumb++;
                    }
                    btnTour.Enabled = false;
                    txtTourNo.Text = DTPay.Rows[0]["TransID"].ToString();
                    Increase_Invoice_No();
                    Fill_Control();
                    cmbAgentCont.SelectedValue = DTPay.Rows[0]["AgentSrNo"].ToString().Trim();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Increase_Invoice_No()
        {
            int RowNumb = 1;
            while (grdPayments[RowNumb, grdPayments.Cols[(int)IG.gTID].Index] != null)
            {
                RowNumb++;
            }
            if (txtTourNo.Text.ToString().Trim() != "")
            {
                if (rdbAmend.Checked)
                {
                    if (AmendNo + "".Trim() == "")
                        return;
                    else
                    {
                        if (AmendNo == 0)
                            AmendNo++;
                        txtInvoiceNo.Text = (txtTourNo.Text + "/S" + RetrieveNo + "/" + (char)((AmendNo - 1) + 65)).Trim();
                        AmendNo++;
                    }
                }
                else if (rdbCancel.Checked)
                    txtInvoiceNo.Text = txtTourNo.Text.Trim() + "/S" + RetrieveNo;
                else
                    txtInvoiceNo.Text = txtTourNo.Text.Trim() + "/S" + RowNumb;
            }
        }
        private void btnAddCont_Click(object sender, EventArgs e)
        {
            try
            {   
                int agentid;
                if (drpInvoiceTo.SelectedValue.ToString() != "")
                    agentid = Convert.ToInt32(drpInvoiceTo.SelectedValue.ToString());
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
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Add_Data()
        {
                int RowNumb = 1;
                if (Validate_Data() == false)
                {
                    return false;
                }
                if (IsRetrive)
                    RowNumb = RetrieveNo;
                else
                {
                    while (grdPayments[RowNumb, grdPayments.Cols[(int)IG.gTID].Index] != null)
                    {
                        RowNumb++;
                    }
                }
                grdPayments[RowNumb, (int)IG.gTID] = txtTourNo.Text.Trim();
                grdPayments[RowNumb, (int)IG.gINO] = txtInvoiceNo.Text.Trim();
                grdPayments[RowNumb, (int)IG.gINVT] = cmbInvoiceTo.SelectedItem.ToString().Trim();      // Invoice to (Agent / Client / other)
                if (cmbInvoiceTo.SelectedItem.ToString().Trim() == "Client")
                {
                    grdPayments[RowNumb, (int)IG.gCLNM] = lblAgentClientName.Text.Trim();    // Client Name
                }
                if (cmbInvoiceTo.SelectedItem.ToString().Trim() == "Agent")
                {
                    grdPayments[RowNumb, (int)IG.gPID] = drpInvoiceTo.SelectedValue.ToString().Trim();      //Agent Company ParentID
                    grdPayments[RowNumb, (int)IG.gPNM] = drpInvoiceTo.SelectedText.ToString().Trim();
                    grdPayments[RowNumb, (int)IG.gACI] = cmbAgentCont.SelectedValue.ToString().Trim();      //Agent Contact ParentID
                    grdPayments[RowNumb, (int)IG.gACN] = cmbAgentCont.SelectedText.ToString().Trim();       //Agent Contact Name
                }
                if (cmbInvoiceTo.SelectedItem.ToString().Trim() == "Other")
                {
                    grdPayments[RowNumb, (int)IG.gONM] = txtOtherName.Text.Trim(); // other Name
                }
                grdPayments[RowNumb, (int)IG.gDTE] = dtpDate.Value;
                if (drpBankBranch.SelectedValue != null)
                {
                    string SrNo = drpBankBranch.SelectedValue.ToString().Trim();
                    string sql = "SELECT BranchID FROM vw_Company_Bank_Details WHERE SrNo= " + SrNo + "";
                    grdPayments[RowNumb, (int)IG.gBID] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql).Rows[0]["BranchID"].ToString();
                    grdPayments[RowNumb, (int)IG.gCSN] = SrNo;
                }
                grdPayments[RowNumb, (int)IG.gCID] = drpCurrnecy.SelectedValue.ToString().Trim();
                grdPayments[RowNumb, (int)IG.gCNM] = drpCurrnecy.SelectedText.ToString().Trim();
                grdPayments[RowNumb, (int)IG.gRAT] = txtRate.Text.Trim();
                grdPayments[RowNumb, (int)IG.gAMT] = txtAmount.Text.Trim();
                grdPayments[RowNumb, (int)IG.gAMDT] = AmendNo; //-------------------
                if (rdbCancel.Checked)
                    grdPayments[RowNumb, (int)IG.gCNCL] = true;
                if (rdbAmend.Checked)
                    grdPayments[RowNumb, (int)IG.gPAM] = txtPaidAmt.Text.Trim();
                grdPayments[RowNumb, (int)IG.gDES] = txtRemarks.Text.Trim();
                grdPayments[RowNumb, (int)IG.gCBY] = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                grdPayments[RowNumb, (int)IG.gCDT] = Classes.clsGlobal.CurDate();
                return true;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Add_Data())
            {
                btnOk.Enabled = true;
                btnPrint.Enabled = true;
                btnEmail.Enabled = true;
                btnRetrieve.Enabled = true;
                btnAddCont.Enabled = false;
                cmbAgentCont.Enabled = false;
                IsRetrive = false;
                RetrieveNo = 0;
                rdbAmend.Checked = false;
                rdbAmend.Visible = false;
                Clear_Contents();
                Increase_Invoice_No();
                DidChange = true;
            }
        }
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPayments[grdPayments.Row, grdPayments.Cols[(int)IG.gTID].Index] == null)
                {
                    MessageBox.Show("No Values Found To Retrieve.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                RetrieveNo = grdPayments.Row;
                IsRetrive = true;
                txtRate.Enabled = true;
                rdbAmend.Visible = true;
                rdbCancel.Visible = true;
                if (rdbAmend.Checked)
                    Increase_Invoice_No();
                #region RETRIEVE VALUES FORM SELECTED ROW
                txtTourNo.Text = grdPayments[RetrieveNo, (int)IG.gTID].ToString();
                txtInvoiceNo.Text = grdPayments[RetrieveNo, (int)IG.gINO].ToString();
                dtpDate.Value = Convert.ToDateTime(grdPayments[RetrieveNo, (int)IG.gDTE].ToString());
                drpCurrnecy.setSelectedValue(grdPayments[RetrieveNo, (int)IG.gCID].ToString());
                cmbInvoiceTo.SelectedItem = grdPayments[RetrieveNo, (int)IG.gINVT].ToString();
                if (grdPayments[RetrieveNo, (int)IG.gINVT].ToString().Trim () == "Client")
                {
                    if (grdPayments[RetrieveNo, (int)IG.gCLNM].ToString().Trim() != "")
                    {
                        lblClientLbl.Visible = true;
                        lblAgentLbl.Visible = false;
                        lblOtherName.Visible = false;
                        txtOtherName.Visible = false;
                        lblAgentClientName.Text = grdPayments[RetrieveNo, (int)IG.gCLNM].ToString().Trim();
                        cmbAgentCont.SelectedValue = grdPayments[RetrieveNo, (int)IG.gACI].ToString().Trim();
                        drpInvoiceTo.Visible = false;
                        lblAgentClientName.Visible = true;
                    }
                }
                else if (grdPayments[RetrieveNo, (int)IG.gINVT].ToString().Trim() == "Agent")
                {
                    if (grdPayments[RetrieveNo, (int)IG.gACI].ToString() != "")
                    {
                        lblAgentLbl.Visible = true;
                        lblClientLbl.Visible = false;
                        lblOtherName.Visible = false;
                        txtOtherName.Visible = false;
                        drpInvoiceTo.Visible = true;
                        drpInvoiceTo.setSelectedValue(grdPayments[RetrieveNo, (int)IG.gPNM].ToString().Trim());
                        lblAgentClientName.Visible = false; //prev -> true
                        cmbAgentCont.SelectedValue = grdPayments[RetrieveNo, (int)IG.gACI].ToString().Trim();
                    }
                }
                else if (grdPayments[RetrieveNo, (int)IG.gINVT].ToString().Trim() == "Other")
                {
                    if (grdPayments[RetrieveNo, (int)IG.gONM].ToString() != "")
                    {
                        lblAgentLbl.Visible = false;
                        lblClientLbl.Visible = false;
                        drpInvoiceTo.Visible = false;
                        lblAgentClientName.Visible = false;
                        lblOtherName.Visible = true;
                        txtOtherName.Visible = true;
                        txtOtherName.Text = grdPayments[RetrieveNo, (int)IG.gONM].ToString();
                    }
                }
                if (grdPayments[RetrieveNo, (int)IG.gACI].ToString() != "")
                {
                    cmbAgentCont.SelectedText = grdPayments[RetrieveNo, (int)IG.gACI].ToString().Trim(); 
                }
                drpBankBranch.setSelectedValue(grdPayments[RetrieveNo, (int)IG.gCSN].ToString());
                txtRate.Text = grdPayments[RetrieveNo, (int)IG.gRAT] + "".ToString();
                txtAmount.Text = grdPayments[RetrieveNo, (int)IG.gAMT] + "".ToString();
                txtPaidAmt.Text = grdPayments[RetrieveNo, (int)IG.gPAM] + "".ToString();
                txtRemarks.Text = grdPayments[RetrieveNo, (int)IG.gDES] + "".ToString();
                AmendNo = Convert.ToInt32(grdPayments[RetrieveNo, (int)IG.gAMDT]);
                if (txtPaidAmt.Text.Trim() != "")
                {
                    rdbAmend.Checked = true;
                    txtPaidAmt.Enabled = true;
                }
                #endregion
                #region CLEAR EXISTING ROW VALUES
                grdPayments[RetrieveNo, (int)IG.gTID] = "";
                grdPayments[RetrieveNo, (int)IG.gINO] = "";
                grdPayments[RetrieveNo, (int)IG.gPID] = "";
                grdPayments[RetrieveNo, (int)IG.gPNM] = "";
                grdPayments[RetrieveNo, (int)IG.gACI] = "";
                grdPayments[RetrieveNo, (int)IG.gACN] = "";
                grdPayments[RetrieveNo, (int)IG.gDTE] = "";
                grdPayments[RetrieveNo, (int)IG.gBID] = "";
                grdPayments[RetrieveNo, (int)IG.gCSN] = "";
                grdPayments[RetrieveNo, (int)IG.gCID] = "";
                grdPayments[RetrieveNo, (int)IG.gCNM] = "";
                grdPayments[RetrieveNo, (int)IG.gRAT] = "";
                grdPayments[RetrieveNo, (int)IG.gAMT] = "";
                grdPayments[RetrieveNo, (int)IG.gPAM] = "";
                grdPayments[RetrieveNo, (int)IG.gDES] = "";
                grdPayments[RetrieveNo, (int)IG.gCBY] = "";
                grdPayments[RetrieveNo, (int)IG.gCDT] = "";
                grdPayments[RetrieveNo, (int)IG.gAMDT] = "";
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void cmbInvoiceTo_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (cmbInvoiceTo.SelectedItem.ToString() == "Client")
                {
                    lblClientLbl.Visible =   lblAgentClientName.Visible = true;
                    lblAgentClientName.Text = ClientName;
                    cmbAgentCont.Enabled =  btnAddCont.Enabled =    lblAgentLbl.Visible =       lblOtherName.Visible =  txtOtherName.Visible = false;
                    string  sql="SELECT Guest FROM trn_GroupAmendment WHERE id="+txtTourNo.Text.Trim()+"";
                    lblAgentClientName.Text = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql).Rows[0]["Guest"].ToString().Trim();
                    lblAgentClientName.Visible = true;
                    drpInvoiceTo.Visible = false;
                }
                else if (cmbInvoiceTo.SelectedItem.ToString() == "Agent")
                {
                    lblAgentLbl.Visible =     drpInvoiceTo.Enabled =  cmbAgentCont.Enabled =       btnAddCont.Enabled = true;
                    lblClientLbl.Visible = lblOtherName.Visible =   txtOtherName.Visible = false;
                    cmbAgentCont.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT SrNo,ContactName FROM mst_AgentContactsDetails Where AgentID=" + AgtID + " ORDER BY SrNo");
                    lblAgentClientName.Visible = false;
                    drpInvoiceTo.Visible = true;
                }
                else if (cmbInvoiceTo.SelectedItem.ToString() == "Other")
                {
                    lblClientLbl.Visible = lblAgentClientName.Visible =  lblAgentLbl.Visible =   drpInvoiceTo.Enabled =   drpInvoiceTo.Visible = cmbAgentCont.Enabled =  btnAddCont.Enabled = false;
                    lblOtherName.Visible =   txtOtherName.Visible = true;
                }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!DidChange)
                this.Close();
            else if (MessageBox.Show("Any Unsaved Data Will Be Lost. Close Anyway ?", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes){this.Close();}
        }
        private void Set_Description()
        {
            if (rdbNormal.Checked)
                return;
            string curval = txtAmount.Text.Trim();
            string paid = txtPaidAmt.Text.Trim();
            string currency = drpCurrnecy.SelectedText.ToString().Trim();
            double bal = 0.00;
            if (curval != "" & paid != "")
                bal = (Convert.ToDouble(curval) - Convert.ToDouble(paid));
            txtRemarks.Text = "*** Due to an oversight we have forwarded " + currency + " " + paid + " for ... statement" +
                              " which you have already settled.The total invoice amount is " + currency + " " + curval + "" +
                              " and balance " + currency + " " + bal + " we included in ... statement.";
        }
        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            Set_Description();
        }
        private void txtPaidAmt_TextChanged(object sender, EventArgs e)
        {
            Set_Description();
        }
        private Boolean Validate_Data()
        {
                if (txtInvoiceNo.Text.Trim() == "")
                {
                    MessageBox.Show("'INVOICE NUMBER' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (cmbInvoiceTo.SelectedItem == null || cmbInvoiceTo.SelectedItem.ToString().Trim() == "")
                {
                    MessageBox.Show("'INVOICE TO' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if ( cmbInvoiceTo.SelectedItem.ToString() == "Agent")
                {
                    if (cmbAgentCont.Text == "")
                    {
                        MessageBox.Show("'AGENT CONTACT PERSON' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (cmbInvoiceTo.SelectedItem.ToString() == "Other")
                {
                    if (txtOtherName .Text.Trim() == "")
                    {
                        MessageBox.Show("'OTHER NAME' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (drpCurrnecy.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("'CURRENCY' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (drpBankBranch.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("'BANK BRANCH' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtAmount.Text.Trim() == "")
                {
                    MessageBox.Show("'AMOUNT' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                {
                    if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtAmount.Text.ToString().Trim()) == false)
                    {
                        MessageBox.Show("Please Enter Valid Values For 'AMOUNT'", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (rdbAmend.Checked)
                {
                    if (txtPaidAmt.Text.Trim() == "")
                    {
                        MessageBox.Show("'PAID AMOUNT' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                return true;
        }
        private void rdbAmend_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAmend.Checked)
            {
                Increase_Invoice_No();
                txtPaidAmt.Enabled = true;
            }
            else
            {
                txtRemarks.Text = "";
                txtPaidAmt.Text = "";
                txtPaidAmt.Enabled = false;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (chkPrint.Checked)
                {
                    Print_Invoice();
                }
                this.Close();
            }
        }
        private Boolean Save_Pro()
        {
                if (Save_Data() == false)
                {
                    return false;
                }
                if (Mode != 1)
                {
                    if (Save_BankRef() == false)
                    {
                        return false;
                    }
                }
                return true;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
                int RowNumb = 1;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "sp_Save_Supplementary_Details";
                SystemCode = Convert.ToInt32(grdPayments[RowNumb, (int)IG.gTID].ToString());
                while (grdPayments[RowNumb, grdPayments.Cols[(int)IG.gTID].Index] != null)
                {
                    DidChange = true;
                    RtnVal = false;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@Flag", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPayments[RowNumb, (int)IG.gTID].ToString());
                    sqlCom.Parameters.Add("@InvoiceNo", SqlDbType.VarChar, 50).Value = grdPayments[RowNumb, (int)IG.gINO].ToString();
                    sqlCom.Parameters.Add("@InvoiceTo", SqlDbType.NVarChar, 100).Value = grdPayments[RowNumb, (int)IG.gINVT].ToString();
                    if ( cmbInvoiceTo.SelectedItem.ToString().Trim() == "Client")     //
                    {
                        sqlCom.Parameters.Add("@IsClient", SqlDbType.Int).Value = 1;
                        if (grdPayments[RowNumb, (int)IG.gCLNM].ToString() != "")
                        {
                            sqlCom.Parameters.Add("@ClientName", SqlDbType.NVarChar, 100).Value = grdPayments[RowNumb, (int)IG.gCLNM].ToString();
                        }
                    }
                    if (cmbInvoiceTo.SelectedItem.ToString().Trim() == "Agent")        //
                    {
                        if (grdPayments[RowNumb, (int)IG.gACI].ToString() != "")
                        {
                            sqlCom.Parameters.Add("@AgentCont", SqlDbType.Int).Value = grdPayments[RowNumb, (int)IG.gACI].ToString();
                        }
                    }
                    if (cmbInvoiceTo.SelectedItem.ToString().Trim() == "Other")        //
                    {
                        if (grdPayments[RowNumb, (int)IG.gONM].ToString() != "")
                        {
                            sqlCom.Parameters.Add("@OtherName", SqlDbType.NVarChar, 100).Value = grdPayments[RowNumb, (int)IG.gONM].ToString();
                        }
                    }
                    if (grdPayments[RowNumb, (int)IG.gBID].ToString() != "")
                        sqlCom.Parameters.Add("@BranchID", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)IG.gBID].ToString());
                    if (grdPayments[RowNumb, (int)IG.gCSN].ToString() != "")
                        sqlCom.Parameters.Add("@ComSrNo", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)IG.gCSN].ToString());
                    if (grdPayments[RowNumb, (int)IG.gDTE].ToString() != "")
                        sqlCom.Parameters.Add("@Date", SqlDbType.DateTime).Value = Convert.ToDateTime(grdPayments[RowNumb, (int)IG.gDTE].ToString());
                    sqlCom.Parameters.Add("@Currency", SqlDbType.Int).Value = grdPayments[RowNumb, (int)IG.gCID].ToString();
                    if (grdPayments[RowNumb, (int)IG.gRAT].ToString() != "")
                        sqlCom.Parameters.Add("@Rate", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPayments[RowNumb, (int)IG.gRAT].ToString());
                    else
                        sqlCom.Parameters.Add("@Rate", SqlDbType.Decimal).Value = 1;
                    sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPayments[RowNumb, (int)IG.gAMT].ToString());
                    if (grdPayments[RowNumb, (int)IG.gPAM] != null)
                    {
                        if (grdPayments[RowNumb, (int)IG.gPAM].ToString() != "")
                        {
                            sqlCom.Parameters.Add("@IsAmend", SqlDbType.Int).Value = 1;
                            sqlCom.Parameters.Add("@AmendTime", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)IG.gAMDT].ToString());  //AmendNo;
                            sqlCom.Parameters.Add("@PaidAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPayments[RowNumb, (int)IG.gPAM].ToString());
                        }
                    }
                    if (Convert.ToBoolean(grdPayments[RowNumb, (int)IG.gCNCL]))
                        sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = 1;
                    else
                        sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = 0;
                    if (grdPayments[RowNumb, (int)IG.gDES].ToString() != "")
                        sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = grdPayments[RowNumb, (int)IG.gDES].ToString();
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@Type", SqlDbType.VarChar, 10).Value = "SUP";
                    sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)IG.gCBY].ToString());
                    sqlCom.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdPayments[RowNumb, (int)IG.gCDT].ToString());
                    RowNumb++;
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                    {
                        RtnVal = true;
                        DidChange = false;
                    }
                }
                return RtnVal;
        }
        private Boolean Save_BankRef()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false; 
                int RowNumb = 1;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_BankRef";
                while (grdPayments[RowNumb, grdPayments.Cols[(int)IG.gTID].Index] != null)
                {
                    DidChange = true;
                    RtnVal = false;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPayments[RowNumb, (int)IG.gTID].ToString());
                    if (grdPayments[RowNumb, (int)IG.gBID].ToString() != "")
                        sqlCom.Parameters.Add("@BranchID", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)IG.gBID].ToString());
                    if (grdPayments[RowNumb, (int)IG.gCSN].ToString() != "")
                        sqlCom.Parameters.Add("@ComSrNo", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)IG.gCSN].ToString());
                    sqlCom.Parameters.Add("@IsSupplementary", SqlDbType.Int).Value = 1;
                    RowNumb++;
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                    {
                        RtnVal = true;
                        DidChange = false;
                    }
                }
                return RtnVal;
        }
        private void drpBankBranch_Selected_TextChanged(object sender, EventArgs e)
        {
                if (drpBankBranch.SelectedValue != null)
                {
                    string identifier = drpBankBranch.SelectedText.ToString().Trim();
                    lblAccountNo.Text = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccountNo FROM vw_Company_Bank_Details WHERE SrNo=" + drpBankBranch.SelectedValue.ToString().Trim() + " AND Identifier LIKE '%" + identifier + "%'").Rows[0]["AccountNo"].ToString();
                }
        }
        private void btnEmailConts_Click(object sender, EventArgs e)
        {
            Email.frmFilterEmail feml = new Tourist_Management.Email.frmFilterEmail();
            feml.ShowDialog();
            txtTo.Text = "";
            txtTo.Text = feml.SelectedContacts;
        }
        private void btnUpdateEmail_Click(object sender, EventArgs e)
        { 
                if (MessageBox.Show("Do You Want To Update Agent Email Address", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
                if (drpInvoiceTo.SelectedValue.ToString().Trim() == "")
                {
                    MessageBox.Show("Please Select a Agent", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtTo.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Please Enter an Email Address", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                System.Data.SqlClient.SqlCommand sqlCom;
                Boolean RtnVal = false;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "sp_Update_Agent_Email";
                sqlCom.Parameters.Add("@AgentID", SqlDbType.Int).Value = Convert.ToInt32(drpInvoiceTo.SelectedValue.ToString().Trim()); //AgtID;
                sqlCom.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = txtTo.Text.Trim();
                sqlCom.Parameters.Add("@LastModifiedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                if (RtnVal)
                    MessageBox.Show("Successfully Updated", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Update Failed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnEmail_Click(object sender, EventArgs e)
        {
            Send_Email();
        }
        private void Send_Email()
        { 
                if (Validate_Email_Options() == false)
                    return;
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.Display(false);
                string Signature = "";
                Signature = ReadSignature();
                oMsg.HTMLBody = rtbBody.Text + Signature;
                oMsg.CC = txtCC.Text;
                String sDisplayName = "MyAttachment";
                int iPosition;
                if (rtbBody.Text.ToString().Trim() != "")
                    iPosition = (int)oMsg.Body.Length + 1;
                else
                    iPosition = 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                Outlook.Attachment oAttach;
                if (chkExSum.Checked)
                {
                    ReportDocument oReport = new ReportDocument();
                    string path = Classes.clsGlobal.InvoicePath.ToString();
                    string lFileName = path;
                    oAttach = oMsg.Attachments.Add(@path, iAttachType, iPosition, sDisplayName);
                }
                oMsg.Subject = txtSubject.Text;
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                if (txtTo.Text.ToString().Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(txtTo.Text.ToString().Trim());
                    oRecip.Resolve();
                    oRecip = null;
                }
                oRecips = null;
                oMsg = null;
                oApp = null;
        }
        private Boolean Validate_Email_Options()
        {
                return true;
        }
        private string ReadSignature()
        {
                string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
                string signature = string.Empty;
                DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
                if (diInfo.Exists)
                {
                    FileInfo[] fisignature = diInfo.GetFiles("*.htm");
                    if (fisignature.Length > 0)
                    {
                        StreamReader sr = new StreamReader(fisignature[0].FullName, Encoding.Default);
                        signature = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(signature))
                        {
                            string filename = fisignature[0].Name.Replace(fisignature[0].Extension, string.Empty);
                            signature = signature.Replace(filename + "_files/", appDataDir + "/" + filename + "_files/");
                        }
                    }
                }
                return signature;
        }
        private void drpInvoiceTo_Click_Open(object sender, EventArgs e)
        {
            Transaction.frmGroupAmend frmGA;
            frmGA = new Transaction.frmGroupAmend();
            frmGA.Mode = 1;
            frmGA.SystemCode = Convert.ToDouble(txtTourNo.Text.Trim());
            frmGA.ShowDialog();
            Fill_Control();
        }
        private void drpInvoiceTo_Selected_TextChanged(object sender, EventArgs e)
        {
            AgtID = Convert.ToInt32(drpInvoiceTo.SelectedValue.ToString().Trim());
            cmbAgentCont.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table( "SELECT SrNo,ContactName FROM mst_AgentContactsDetails Where AgentID=" + AgtID + " ORDER BY SrNo");
            lblAgentClientName.Visible = false;
            drpInvoiceTo.Visible = true;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Invoice();
        }
        private void Print_Invoice()
        {
            db.showReport(new Tourist_Management.Reports.rpt_acc_SupplementaryInvoice(), SystemCode, Classes.clsGlobal.UserID);
        }
    }
}
