using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
namespace Tourist_Management.Accounts
{
    public partial class frmCashBook : Form
    {
        private const string msghd = "Cash Book";
        DataTable dt = new DataTable();
        public frmCashBook(){InitializeComponent();}
        private void frmCashBook_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Fill_Control();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {                
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Expenses(DataTable DT)
        {
            try
            {
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void LoopingSub(DataTable DT, string real, string temp, int col, int intRowParent)
        {
            try
            {                
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {  
              cmbCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }       
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnPreview_Click(object sender, EventArgs e)
        {
            Account_Reports.frm_WebController wc = new Tourist_Management.Account_Reports.frm_WebController();
            string comp="";
            if (chkComp.Checked)
                comp = cmbCompany.Text.ToUpper();
            wc.ReportName = comp + "\nCASH BOOK FOR THE MONTH OF " + dtpIFromDate.Value.ToString("yyyy-MMMM") + "".ToUpper();
            dt = null;
            dt = set_CashBook();
            if (dt != null)
            {
                wc.dataTable = dt;
                wc.Show();
            }
            else
                MessageBox.Show("No records to be previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private DataTable set_CashBook()
        {
            try
            {
                string sql;
                #region INCOME
                sql = get_Query("cast(Date as Date)[Date],Receipt,Detail,", "vw_acc_CB_Income","Date,Receipt");
                DataTable dtIncome = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                EnumerableRowCollection<DataRow> dr = (from row in dtIncome.AsEnumerable()
                                                       orderby row["Date"]
                                                       select row);
                dtIncome = dr.AsDataView().ToTable();
                DataRow[] foundRows;
                foundRows = dtIncome.Select("", "Date ASC,Receipt ASC");
                dtIncome = foundRows.CopyToDataTable();
                #endregion
                #region EXPENSES
                sql = get_Query("Date,Detail,ChequeNo,VoucherID,", "vw_acc_CB_Expenses", "Date,VoucherID");
                DataTable dtExpense = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                EnumerableRowCollection<DataRow> dr1 = (from row in dtExpense.AsEnumerable()
                                                        orderby row["Date"]
                                                        select row);
                dtExpense = dr1.AsDataView().ToTable();
                DataRow[] foundRows2;
                foundRows2 = dtExpense.Select("", "Date ASC,VoucherID ASC");
                dtExpense = foundRows2.CopyToDataTable();
                #endregion
                #region SET DATATABLE COLUMNS
                dt = new DataTable();
                set_Columns(dtIncome, 0, 3,"INC");
                set_Columns(dtExpense, 0, 4,"EXP");
                #endregion
                #region SET DATA ROWS
                set_bf();
                set_Data(dtIncome, 0,3,"INC");
                set_Data(dtExpense, dtIncome.Columns.Count, 4 + dtIncome.Columns.Count,"EXP");
                #endregion
                #region SET TOTAL
                object sumObject;
                string colName;
                dt.Rows.Add();
                foreach (DataColumn dc in dt.Columns)
                {
                    colName = dc.ToString().Trim();
                    if (dc.DataType==typeof(decimal))
                    {
                        sumObject = dt.Compute("Sum([" + colName + "])", "");                        
                        if(sumObject+"".Trim()!="")
                            dt.Rows[dt.Rows.Count - 1][colName] = Convert.ToDecimal(sumObject).ToString("F");
                    }
                }
                #endregion
                #region REMOVE NULL COLUMN
                #endregion
                return dt;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return null;
            }
        }
        private void set_bf()
        {
            try
            {
                DateTime dtm = new DateTime(dtpIFromDate.Value.Year, dtpIFromDate.Value.Month, 1);
                dtm = dtm.AddDays(-1);
                string name,sql; 
                int accID;
                decimal bf;
                dt.Rows.Add();
                dt.Rows[0]["Detail"] = "B/F".Trim();
                dt.Rows[0]["Detail_2"] = "B/F".Trim();
                dt.Rows[0]["Date"] = dtm.ToString("yyyy-MMM-dd");
                dt.Rows[0]["Date_2"] = dtm.ToString("yyyy-MMM-dd");
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(decimal))
                    {
                        name = dc.ColumnName.Trim();
                        if (name.Substring(name.Length-1, 1) == "2")
                            break;
                        accID = Convert.ToInt32(name);
                        sql = "SELECT dbo.fun_get_BroughtForward_Amount('" + dtm.ToString("yyyy-MM-dd") + "'," + accID + ",1" +")Amt";
                        DataTable dtBF = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql.Trim());
                        if (dtBF.Rows.Count > 0)
                        {
                            if (dtBF.Rows[0]["Amt"] + "".Trim() != "")
                            {
                                bf = Convert.ToDecimal(dtBF.Rows[0]["Amt"]);
                                if (bf == 0)
                                    continue;
                                if (bf > 0)
                                    dt.Rows[0][name] = bf.ToString("F");
                                else
                                    dt.Rows[0][name + "_2".Trim()] = (bf * -1).ToString("F");
                            }
                        }
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void set_Columns(DataTable dtEx,int col,int colChangeToDouble,string typ)
        {
            try
            {
                string name;
                string nm, cap;
                while (dtEx.Columns.Count > col)
                {                    
                    name = dtEx.Columns[col].ToString().Trim();
                    cap = dtEx.Columns[col].ToString().Trim();
                    if (typ == "EXP")
                    {
                        cap = name;
                        name = name + "_2".Trim();
                    }
                    if (col == 0)
                    {
                        set_DataTable_Columns(name, cap, "System.String");//System.DateTime
                    }
                    else if (col < colChangeToDouble)
                        set_DataTable_Columns(name, cap, "System.String");
                    else
                        break;
                    col++;
                }
                DataTable dtC = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,AccountType FROM comAcc_AccountTypes WHERE AllCompany=1");// AND AccountType LIKE '%" + name + "%'");
                foreach (DataRow dr in dtC.Rows)
                {
                    nm = dr["ID"] + "".Trim();
                    if (typ == "EXP")
                        nm = nm + "_2".Trim();
                    cap = dr["AccountType"] + "".Trim();
                    set_DataTable_Columns(nm, cap, "System.Decimal");//System.Double
                }
                DataTable dtB = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,AccountType FROM comAcc_AccountTypes WHERE HasCompany=1 AND CompanyID=" + cmbCompany.SelectedValue + "");// AND AccountType LIKE '%" + name + "%'");
                foreach(DataRow dr in dtB.Rows)
                {
                    nm = dr["ID"] + "".Trim();
                    if (typ == "EXP")
                        nm = nm + "_2".Trim();
                    cap = dr["AccountType"] + "".Trim();
                    set_DataTable_Columns(nm, cap, "System.Decimal");//System.Double
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void set_Data(DataTable dtEx, int col, int colChangeToDouble,string typ)
        {
            try
            {
                int rowNo = 1, colNo;
                string colName;
                DataTable dtACC = new DataTable();
                foreach (DataRow dr in dtEx.Rows)
                {
                    dt.Rows.Add();
                    colNo = col;
                    foreach (DataColumn dc in dtEx.Columns)
                    {
                        if (typ.Trim() == "EXP")
                            colName = dc.ColumnName + "_2".Trim();
                        else
                            colName = dc.ColumnName.Trim();
                        if (colNo == col)//date
                        {
                            if (dr[dc] + "".Trim() != "")
                                dt.Rows[rowNo][colName] = Convert.ToDateTime(dr[dc]).ToString("yyyy-MMM-dd");
                        }
                        else if (colNo < colChangeToDouble)//detail
                            dt.Rows[rowNo][colName] = dr[dc] + "".Trim();
                        else//cash or bank
                        {
                            string qry = "SELECT ID FROM comAcc_AccountTypes WHERE UPPER(AccountType)=UPPER('" + dc.ColumnName.ToString().Trim() + "')";
                            dtACC = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(qry);
                            colName = dtACC.Rows[0]["ID"] + "".Trim();
                            if (typ.Trim() == "EXP")
                                colName = colName + "_2".Trim();
                            if (dr[dc] + "".Trim() == "")
                                dr[dc] = 0;
                                if (dt.Columns.Contains(colName))
                                    dt.Rows[rowNo][colName.Trim()] = Convert.ToDecimal(dr[dc]).ToString("F");
                        }
                        colNo++;
                    }
                    rowNo++;
                }      
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void set_DataTable_Columns(string name,string caption,string type)
        {
            try
            {                
                DataColumn column;
                column = new DataColumn();
                DataColumnCollection collections = dt.Columns;
                column.ColumnName = name.Trim();
                column.DataType = System.Type.GetType(type);
                column.Unique = false;
                column.AutoIncrement = false;
                column.Caption = caption.Trim();
                column.ReadOnly = false;
                dt.Columns.Add(column);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private string get_Query(string value,string view,string orderBy)
        {
            try
            {
                DateTime dtm = new DateTime(dtpIFromDate.Value.Year, dtpIFromDate.Value.Month, 1);
                string dtmFirst = dtm.ToString("yyyy-MM-dd");
                dtm = new DateTime(dtpIFromDate.Value.Year, dtpIFromDate.Value.Month, 1).AddMonths(1).AddDays(-1);                
                string dtmLast = dtm.ToString("yyyy-MM-dd");
                string filter = "WHERE Date>=''" + dtmFirst + "''" +
                         " AND Date<=''" + dtmLast + "''";
                if (chkComp.Checked)
                {
                       filter += " AND CompID=" + cmbCompany.SelectedValue + "";
                }
                string query = " DECLARE @cols AS NVARCHAR(MAX),@query  AS NVARCHAR(MAX) " +
                        "SET @cols = STUFF((SELECT distinct ',' + QUOTENAME(LTRIM(RTRIM(c.AccountType))) " +
                        "FROM " + view + " c " +
                        " FOR XML PATH(''), TYPE " +
                        ").value('.', 'NVARCHAR(MAX)') " +
                        ",1,1,'') " +
                        "set @query = 'SELECT " +
                        "" + value + "" +
                        "' + @cols + ' from " +
                        "(" +
                        "select " +
                        "" + value + "" +
                        "Debit,AccountType " +
                        "from " + view + " " +
                        "" + filter + "" +
                        ") x " +
                        "pivot " +
                        "(" +
                        "max(Debit) " +
                        "for AccountType in (' + @cols + ')" +
                        ") p ' execute(@query)";
                return query;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return "";
            }
        }
        private void chkComp_CheckedChanged(object sender, EventArgs e)
        {
            if(chkComp.Checked)
                cmbCompany.Enabled=true;
            else
                cmbCompany.Enabled=false;
        }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
