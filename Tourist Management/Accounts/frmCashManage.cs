using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
namespace Tourist_Management.Accounts
{
    public partial class frmCashManage : Form
    {
        private const string msghd = "Main Cash Management";
        enum MC { gID, gDate, gPOR, gRefNo, gDebit, gCredit, gBalance, gIssued, gIssuedDate };
        bool formLoaded = false;
        public frmCashManage()        {            InitializeComponent();        }
        private void btnCancel_Click(object sender, EventArgs e)        {            this.Close();        }
        private void frmCashManage_Load(object sender, EventArgs e)        {            Intializer();        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Control();
                formLoaded = true;
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private DataTable get_DataTable(bool ForIssued,string dateFilter)
        {
            DataTable DT=new DataTable();
            try
            {
                string sql = "SELECT ID,Date,Issued,IssuedDate,TransID,CompID,CompName,VoucherID,PayableTo,ISNULL(Credit,0)Credit,ISNULL(Debit,0)Debit, Aname1,AADname FROM vw_MainCashManage CROSS JOIN mst_OtherSettings OST WHERE 1=1";
                sql += dateFilter;
                if (ucFilterByOther1.chkIByOther.Checked && ucFilterByOther1.drpOther.SelectedValue + "".Trim() != "")
                    sql += " AND CurrencyID='" + ucFilterByOther1.drpOther.SelectedValue + "".Trim() + "'";
                DT = Classes.clsGlobal.objCon.Fill_Table(sql);
                grdCash.Rows.Count = 1;
                decimal balance = 0;
                string qry;
                    DateTime dtfrom, dtOpen;
                    if (ForIssued)                        dtfrom = dtpIssuedDate.Value;
                    else if (ucFilterByDate1.chkIByDate.Checked)                        dtfrom = ucFilterByDate1.dtpIFromDate.Value;
                    else
                    {
                        MessageBox.Show("Please Select Filter By Date", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return DT;
                    }
                    string open = Classes.clsGlobal.objCon.Fill_Table("SELECT OpenBalDate FROM comAcc_AccountTypes WHERE ID=18").Rows[0][0] + "".ToString();
                    if (open.Trim() != "")
                    {
                        dtOpen = Convert.ToDateTime(open);
                        if (dtfrom.Date > dtOpen.Date)
                        {
                            qry = "SELECT dbo.fun_get_BroughtForward_Amount('" + dtfrom.AddDays(-1).ToString("yyyy-MM-dd").Trim() + "',18,1)Amt";
                            balance = Convert.ToDecimal(Classes.clsGlobal.objCon.Fill_Table(qry).Rows[0]["Amt"]);
                        }
                        if (balance != 0)
                        {
                            if (balance > 0)                                DT.Rows.Add(0, dtfrom.Date.AddDays(-1), 1, dtpIssuedDate.Value, 0, 0, "", "B/F Balance", "", 0, balance);
                            else                                DT.Rows.Add(0, dtfrom.Date.AddDays(-1),1, dtpIssuedDate.Value ,0, 0, "", "B/F Balance", "", balance, 0);
                        }
                    }
                EnumerableRowCollection<DataRow> drSort = (from curRow in DT.AsEnumerable()                                                           orderby curRow["VoucherID"]                                                           select curRow);
                DT = drSort.AsDataView().ToTable();
                return DT;
            }
            catch (Exception ex)
            {                
                db.MsgERR(ex);
                return DT;
            }
        }
        private void Fill_Grid()
        {
            try
            {
                lblBalanceError.Visible = false;
                string filterrDate="";
                if (ucFilterByDate1.chkIByDate.Checked)
                {
                    string df = ucFilterByDate1.dtpIFromDate.Value.ToString("yyyy-MM-dd").Trim();
                    string dt = ucFilterByDate1.dtpIToDate.Value.ToString("yyyy-MM-dd").Trim();
                    filterrDate += " AND Date>='" + df + "' AND Date<='" + dt + "'";
                }
                DataTable DT = get_DataTable(false,filterrDate);
                int row = 1;
                decimal balance = 0,income = 0, expense = 0;
                foreach (DataRow dr in DT.Rows)
                {
                    grdCash.Rows.Add();
                    grdCash[row, (int)MC.gID] = dr["ID"];
                    if (dr["Date"] + "".Trim() != "")                        grdCash[row, (int)MC.gDate] = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd");
                    grdCash[row, (int)MC.gPOR] = dr["PayableTo"];
                    grdCash[row, (int)MC.gRefNo] = dr["VoucherID"];
                    grdCash[row, (int)MC.gCredit] = dr["Credit"];
                    grdCash[row, (int)MC.gDebit] = dr["Debit"];
                    if (Convert.ToDecimal(dr["ID"])!=0 && Convert.ToBoolean(dr["Issued"]))
                    {
                        grdCash[row, (int)MC.gIssued] = 1;
                        grdCash[row, (int)MC.gIssuedDate] = Convert.ToDateTime(dr["IssuedDate"]);
                    }
                    if (Convert.ToDouble(dr["Debit"]) > 0)                        balance += Convert.ToDecimal(dr["Debit"]);
                    if (Convert.ToDouble(dr["Credit"]) > 0)                        balance -= Convert.ToDecimal(dr["Credit"]);
                    else                        balance += Convert.ToDecimal(dr["Credit"]);
                    income += Convert.ToDecimal(dr["Debit"]);
                    expense += Convert.ToDecimal(dr["Credit"]);
                    grdCash[row, (int)MC.gBalance] = balance.ToString();
                    if (balance < 0)
                        lblBalanceError.Visible = true;
                    row++;
                }
                lblIncome.Text = income.ToString();
                lblExpense.Text = expense.ToString();
                lblBalance.Text = balance.ToString();
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void Fill_Control()
        {
            try
            {                
                ucFilterByOther1.Query = "SELECT ID,Currency FROM mst_Currency WHERE ISNULL(IsActive,0)<>0";
                ucFilterByOther1.Intializer();
                ucFilterByOther1.chkIByOther.Checked = true;
                ucFilterByOther1.chkIByOther.Enabled = false;
                ucFilterByOther1.drpOther.Enabled = false;
                ucFilterByOther1.drpOther.setSelectedValue("1".Trim());
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void Grd_Initializer()
        {
            try
            {
                grdCash.Cols.Count = 9;
                grdCash.Rows.Count = 1;
                grdCash.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                grdCash.Cols[(int)MC.gID].Width = 0;
                grdCash.Cols[(int)MC.gDate].Width = 86;
                grdCash.Cols[(int)MC.gPOR].Width = 160;
                grdCash.Cols[(int)MC.gRefNo].Width = 96;                
                grdCash.Cols[(int)MC.gCredit].Width = 100;
                grdCash.Cols[(int)MC.gDebit].Width = 100;
                grdCash.Cols[(int)MC.gBalance].Width = 100;
                grdCash.Cols[(int)MC.gIssued].Width = 50;
                grdCash.Cols[(int)MC.gIssuedDate].Width = 79;
                grdCash.Cols[(int)MC.gID].Caption = "ID";
                grdCash.Cols[(int)MC.gDate].Caption = "Date";
                grdCash.Cols[(int)MC.gPOR].Caption = "Paid/Received";
                grdCash.Cols[(int)MC.gRefNo].Caption = "Reference No";
                grdCash.Cols[(int)MC.gCredit].Caption = "Expense";
                grdCash.Cols[(int)MC.gDebit].Caption = "Income";
                grdCash.Cols[(int)MC.gBalance].Caption = "Balance";
                grdCash.Cols[(int)MC.gIssued].Caption = "Issued";
                grdCash.Cols[(int)MC.gIssuedDate].Caption = "Issued Date";
                grdCash.Cols[(int)MC.gIssued].DataType = Type.GetType(" System.Boolean");
                grdCash.Cols[(int)MC.gIssuedDate].DataType = Type.GetType(" System.DateTime");
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void btnFilter_Click(object sender, EventArgs e)
        {
            lblIncome.Text =   lblExpense.Text =  lblBalance.Text = "0.00";
            Fill_Grid();
        }
        private void btnPrint_Click(object sender, EventArgs e)        {            Print_Report();        }
        private void Print_Report()
        {
            try
            {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                DataSet DTG = new DataSet();
                ReportDocument ga = new ReportDocument();
                DTG = new DataSets.ds_acc_MainCash();
                ga = new Tourist_Management.Reports.rpt_acc_MainCash();
                string filterrDate = " AND IssuedDate='" + dtpIssuedDate.Value.ToString("yyyy-MM-dd").Trim() + "'";
                DataTable dtPrint = get_DataTable(true,filterrDate);
                sConnection.Print_Via_Datatable(DTG, dtPrint, ga, "");
            }
            catch (Exception ex)      {  db.MsgERR(ex); }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)  return; 
                MessageBox.Show(Save_Pro() ? "Transaction Sucessfully Completed" : "Data Not Saved Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private Boolean Save_Pro()        {                 return Save_Procedure();         }
        private Boolean Save_Procedure()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
            try
            {
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_IssuedPayment";
                int row=1;
                while (grdCash[row, (int)MC.gID] + "".Trim() != "")
                {
                    RtnVal = false;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToDouble(grdCash[row, (int)MC.gID]);
                    if (Convert.ToBoolean(grdCash[row, (int)MC.gIssued]))
                    {
                        sqlCom.Parameters.Add("@Issued", SqlDbType.Int).Value = 1;
                        sqlCom.Parameters.Add("@IssuedDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCash[row, (int)MC.gIssuedDate]);
                    }
                    else
                        sqlCom.Parameters.Add("@Issued", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)  RtnVal = true; 
                    if (!RtnVal)  return false;
                    row++;
                    if (row == grdCash.Rows.Count)   break;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;                
            }
        }
        private void grdCash_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if(formLoaded && e.Col == (int)MC.gIssued)  grdCash[e.Row, (int)MC.gIssuedDate] =(!Convert.ToBoolean(grdCash[e.Row, (int)MC.gIssued]))?  null: (object)Classes.clsGlobal.CurDate();
         }
    }
}
