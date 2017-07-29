using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Account_Reports
{
    public partial class frmDebtorAgents : Form
    {
        private const string msghd = "Debtor Agents";
   public     int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
   public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        string  ssql = ""; 
        int prevRow = 0;
        enum DG { gID, gTID, gGuest, gDate, gPax, gAgName, gHD, gInvoice, gRAmnt, gBalance, gSkipRecAmt, gInvoiceNo, gIsSupplementary, gRemark };
        public frmDebtorAgents(){InitializeComponent();}
        private void frmDebtorAgents_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Control();
                dtpFromDate.Value = Convert.ToDateTime("2014-04-01");
                dtpToDate.Value = Classes.clsGlobal.CurDate();
                dtpUnIFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                lblUnIFrom.Enabled = false;
                dtpUnIFrom.Enabled = false;
                Fill_Details();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                #region DEBTOR DETAILS
                grdDAgent.Cols.Count = 14;
                grdDAgent.Rows.Count = 500;
                grdDAgent.Cols[(int)DG.gID].Width = 0;
                grdDAgent.Cols[(int)DG.gTID].Width = 99;
                grdDAgent.Cols[(int)DG.gGuest].Width = 150;              
                grdDAgent.Cols[(int)DG.gDate].Width = 150;
                grdDAgent.Cols[(int)DG.gPax].Width = 32;
                grdDAgent.Cols[(int)DG.gAgName].Width = 149;
                grdDAgent.Cols[(int)DG.gHD].Width = 73;
                grdDAgent.Cols[(int)DG.gInvoice].Width = 86;
                grdDAgent.Cols[(int)DG.gRAmnt].Width = 88;
                grdDAgent.Cols[(int)DG.gBalance].Width = 82;
                grdDAgent.Cols[(int)DG.gSkipRecAmt].Width = 00;
                grdDAgent.Cols[(int)DG.gInvoiceNo].Width = 0;
                grdDAgent.Cols[(int)DG.gIsSupplementary].Width = 0;
                grdDAgent.Cols[(int)DG.gRemark].Width = 200;
                grdDAgent.Cols[(int)DG.gID].Caption = "ID";
                grdDAgent.Cols[(int)DG.gTID].Caption = "Tour Id";
                grdDAgent.Cols[(int)DG.gGuest].Caption = "Guest";               
                grdDAgent.Cols[(int)DG.gDate].Caption = "Date";
                grdDAgent.Cols[(int)DG.gPax].Caption = "PAX";
                grdDAgent.Cols[(int)DG.gAgName].Caption = "Agent";
                grdDAgent.Cols[(int)DG.gHD].Caption = "Handled By";
                grdDAgent.Cols[(int)DG.gInvoice].Caption = "Invoice";
                grdDAgent.Cols[(int)DG.gRAmnt].Caption = "Paid Amount";
                grdDAgent.Cols[(int)DG.gBalance].Caption = "Balance";
                grdDAgent.Cols[(int)DG.gSkipRecAmt].Caption = "Skip";
                grdDAgent.Cols[(int)DG.gInvoiceNo].Caption = "Invoice No";
                grdDAgent.Cols[(int)DG.gIsSupplementary].Caption = "Supplementary";
                grdDAgent.Cols[(int)DG.gRemark].Caption = "Remark";
                grdDAgent.Cols[(int)DG.gSkipRecAmt].DataType = Type.GetType("System.Boolean");
                grdDAgent.Cols[(int)DG.gIsSupplementary].DataType = Type.GetType("System.Boolean");
                grdDAgent.Rows[1].AllowEditing = false;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
         private void Fill_Control()
        {
            try
            {
                DataTable[] DTB;
                DTB = new DataTable[2];
                drpAgent.Enabled = false;
                drpHandled.Enabled = false;
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name as [AgentName] FROM mst_AgentDetails Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpAgent.DataSource = DTB[0];
                DTB[1] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS [HandledPerson] FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpHandled.DataSource = DTB[1];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_DebterReport();
        }
        private void Print_DebterReport()
        {
            Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DT.Rows.Count > 0)
            {
                DataSets.ds_acc_newDebtor DTP = new DataSets.ds_acc_newDebtor();
                Tourist_Management.Reports.newDebtorReport pia = new Tourist_Management.Reports.newDebtorReport();
                pia.SetDataSource(DTP);
                sConnection.Print_Report(SystemCode.ToString(), ssql, DTP, pia, "DEBTER");
            }
            else
                MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Fill_Details()
        {
            try
            {
                string ADate,DDate;
                int RowNumb,noOfA,noOfC;
                double Amt=0, RecAmt=0, balance=0;
                ssql = "SELECT da.ID,da.TourID, Company_Logo,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address,Guest,AgentName, DateArrival, DateDeparture," +
                       "HDName,IsNull(NoOfAdult,0) AS NoOfAdult,IsNull(NoOfChild,0) AS NoOfChild,"+
                       "ISNULL(Amount,0)Amount,ISNULL(AgentRecAmt,0)AgentRecAmt,AmountRate," +
                       "AgentRecRate,AgentName,AgentID, MarketingDep,SrNo,ISNULL(SkipRecAmt,0)SkipRecAmt,InvoiceNo," +
                       "ISNULL(IsSupplementary,0)IsSupplementary,DebtorRemarks,TourID,InvoiceNo,ISNULL(Currency,'Un-Invoice')Currency,IsSUpplementary" +
                       " FROM vw_acc_Debtor_Agents da "+
                       " WHERE (ISNULL(AgentRecAmt,0)<ISNULL(Amount,0) OR ISNULL(AgentRecAmt,0)=0)" +
                       "  AND DateArrival>='" + dtpFromDate.Value.ToString("yyyy-MM-dd").Trim() + "' AND DateArrival<='" + dtpToDate.Value.ToString("yyyy-MM-dd").Trim() + "'";
                if (chkAllAgent.Checked && !chkAllHandled.Checked)
                {
                    if (drpAgent.SelectedValue != null)
                    {
                        int AgentID = Convert.ToInt32(drpAgent.SelectedValue);
                        ssql += " AND AgentID=" + AgentID + "";
                    }
                }
                else if (chkAllHandled.Checked && !chkAllAgent.Checked)
                {
                    if (drpHandled.SelectedValue != null)
                    {
                        int HandledID = Convert.ToInt32(drpHandled.SelectedValue);
                        ssql +=" AND MarketingDep=" +HandledID + " " ;
                    }
                }
                else if (chkAllHandled.Checked && chkAllAgent.Checked)
                {
                    if (drpHandled.SelectedValue != null || drpAgent.SelectedValue != null)
                    {
                        int HandledID = Convert.ToInt32(drpHandled.SelectedValue);
                        int AgentID = Convert.ToInt32(drpAgent.SelectedValue);
                        ssql += " AND AgentID=" + AgentID + " AND MarketingDep=" + HandledID + " ";
                    }
                }
                if (rdbInvoice.Checked)
                    ssql += " AND IsSupplementary=0";
                if (rdbSupple.Checked)
                    ssql += " AND IsSupplementary=1";
                if (rdb_A_Invoiced.Checked) ssql += " AND ISNULL(InvoiceNo,'') != ''";
                if (rdb_A_All.Checked) ssql += " AND (ISNULL(InvoiceNo,'') != '' OR (ISNULL(InvoiceNo,'') = '' AND DateArrival >='" + dtpUnIFrom.Value.ToString("yyyy-MM-dd").Trim() + "'))";
                if (ssql == "")
                    return;
                ssql += " ORDER BY da.AgentName";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                grdDAgent.Rows.Count = 1;
                grdDAgent.Rows.Count = 5000;
                double TotBal = 0, TotAmt = 0, TotRecAmt = 0;
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        ADate = DT.Rows[RowNumb]["DateArrival"].ToString().Trim().Substring(0, 10);
                        DDate = DT.Rows[RowNumb]["DateDeparture"].ToString().Trim().Substring(0, 10);
                        Amt = Convert.ToDouble(DT.Rows[RowNumb]["Amount"].ToString());
                        RecAmt = Convert.ToDouble(DT.Rows[RowNumb]["AgentRecAmt"].ToString());
                        noOfA=Convert.ToInt32(DT.Rows[RowNumb]["NoOfAdult"].ToString());
                        noOfC=Convert.ToInt32(DT.Rows[RowNumb]["NoOfChild"].ToString());
                        balance = Amt - RecAmt;
                        TotBal += balance;
                        TotAmt += Amt;
                        TotRecAmt += RecAmt;
                        grdDAgent[RowNumb + 1, (int)DG.gDate] = ADate + " - " + DDate;
                        grdDAgent[RowNumb + 1, (int)DG.gID] = DT.Rows[RowNumb]["ID"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gTID] = DT.Rows[RowNumb]["TourID"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gGuest] = DT.Rows[RowNumb]["Guest"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gPax] = noOfA+noOfC;
                        grdDAgent[RowNumb + 1, (int)DG.gAgName] = DT.Rows[RowNumb]["AgentName"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gInvoice] = DT.Rows[RowNumb]["Amount"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gRAmnt] = DT.Rows[RowNumb]["AgentRecAmt"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gBalance] = balance.ToString("0.00");
                        grdDAgent[RowNumb + 1, (int)DG.gHD] = DT.Rows[RowNumb]["HDName"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gSkipRecAmt] = Convert.ToBoolean(DT.Rows[RowNumb]["SkipRecAmt"]);
                        grdDAgent[RowNumb + 1, (int)DG.gIsSupplementary] = Convert.ToBoolean(DT.Rows[RowNumb]["IsSupplementary"]);
                        grdDAgent[RowNumb + 1, (int)DG.gInvoiceNo] = DT.Rows[RowNumb]["InvoiceNo"].ToString();
                        grdDAgent[RowNumb + 1, (int)DG.gRemark] = DT.Rows[RowNumb]["DebtorRemarks"] + "".Trim();
                        RowNumb++;
                    }
                    C1.Win.C1FlexGrid.CellStyle TOT = grdDAgent.Styles.Add("TOT");
                    TOT.BackColor = Color.BurlyWood;
                    grdDAgent[RowNumb+1, (int)DG.gBalance] = TotBal.ToString("0.00");
                    grdDAgent[RowNumb + 1, (int)DG.gInvoice]=TotAmt;
                    grdDAgent[RowNumb + 1, (int)DG.gRAmnt] = TotRecAmt.ToString("0.00");
                    grdDAgent[RowNumb + 1, (int)DG.gHD] = "TOTAL";
                    grdDAgent.Rows[RowNumb + 1].Style = grdDAgent.Styles["TOT"];
                    grdDAgent.Rows.Count = RowNumb + 2;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void drpMarketingDep_Selected_TextChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
        }
        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
        }
        private void chkAllAgent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllAgent.Checked)
            {
                drpAgent.setSelectedValue(null);
                drpAgent.Enabled = true;
            }
            else
            {
                drpAgent.setSelectedValue(null);
                drpAgent.Enabled = false;
            }
            grdDAgent.Rows.Count = 1;
        }
        private void chkAllHandled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllHandled.Checked)
            {
                drpHandled.setSelectedValue(null);
                drpHandled.Enabled = true;
            }
            else
            {
                drpHandled.setSelectedValue(null);
                drpHandled.Enabled = false;
            }
            grdDAgent.Rows.Count = 1;
        }
        private void drpAgent_Selected_TextChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
        }
        private void drpHandled_Selected_TextChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
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
                btnFilter_Click(null, null);
            }
        }
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Data() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
        private Boolean Validate_Data()
        {
                return true;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
            try
            {
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_SkipRecAmt";
                int row = 1;
                while (grdDAgent.Rows[row][(int)DG.gTID] + "".Trim() != "")
                {
                        sqlCom.Parameters.Clear();
                        sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal,18).Value = Convert.ToDecimal(grdDAgent.Rows[row][(int)DG.gID]);
                        sqlCom.Parameters.Add("@IsSupplementary", SqlDbType.Int).Value = Convert.ToInt32(grdDAgent.Rows[row][(int)DG.gIsSupplementary]);
                        sqlCom.Parameters.Add("@InvoiceNo", SqlDbType.NVarChar,50).Value = grdDAgent.Rows[row][(int)DG.gInvoiceNo].ToString().Trim();
                        sqlCom.Parameters.Add("@SkipRecAmt", SqlDbType.Int).Value = Convert.ToBoolean(grdDAgent.Rows[row][(int)DG.gSkipRecAmt]);
                        sqlCom.Parameters.Add("@Remark", SqlDbType.NVarChar, 200).Value = grdDAgent.Rows[row][(int)DG.gRemark] + "".Trim();
                        sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                        if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                        {
                            RtnVal = true;
                        }
                        else
                        {
                            return false;
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
        private void btnFilter_Click(object sender, EventArgs e)
        {
            Fill_Details();
        }
        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
        }
        private void rdbInvoice_CheckedChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
        }
        private void rdbSupple_CheckedChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
        }
        private void grdDAgent_AfterSelChange(object sender, C1.Win.C1FlexGrid.RangeEventArgs e)
        {
            if (grdDAgent.Rows.Count == 1)    return; 
            if (prevRow != 0)
            { 
                C1.Win.C1FlexGrid.CellStyle DEF = grdDAgent.Styles.Add("DEF");
                DEF.BackColor = Color.Transparent;// ColorTranslator.FromHtml("#FBFBEF");
                grdDAgent.Rows[prevRow].Style = DEF;
            }
            C1.Win.C1FlexGrid.CellStyle SEL = grdDAgent.Styles.Add("SEL");
            SEL.BackColor = ColorTranslator.FromHtml("#BCF5A9");
            grdDAgent.Rows[grdDAgent.Row].Style = SEL;
            prevRow = grdDAgent.Row;
        }
        private void btnDirect_Click(object sender, EventArgs e)
        {
            Accounts.frmDirectBooking fdb = new Tourist_Management.Accounts.frmDirectBooking();
            fdb.ShowDialog();
        }
        private void rdb_A_Invoiced_CheckedChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
            lblUnIFrom.Enabled = false;
            dtpUnIFrom.Enabled = false;
        }
        private void rdb_A_All_CheckedChanged(object sender, EventArgs e)
        {
            grdDAgent.Rows.Count = 1;
            lblUnIFrom.Enabled = true;
            dtpUnIFrom.Enabled = true;
        }
    }
}
