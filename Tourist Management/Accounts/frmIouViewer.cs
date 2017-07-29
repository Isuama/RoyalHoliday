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
    public partial class frmIouViewer : Form
    {
        private const string msghd = "Payment Viewer";
        enum GRD { ID, TourID, Guest, CompID, CompName, VoucherID, RefNo, PayableToID, PayableTo, PaidDate, Amount, Settled, Outstanding };
        DataTable DT = new DataTable();
        public frmIouViewer(){InitializeComponent();}
        private void rdbIOU_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbIOU.Checked)
                rdbUnSettled.Enabled = true;
        }
        private void rdbCash_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCash.Checked)
                rdbUnSettled.Enabled = false;
        }
        private void rdbChk_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbChk.Checked)
                rdbUnSettled.Enabled = false;
        }
        private void btnICancel_Click(object sender, EventArgs e){this.Close();}
        private void frmIouViewer_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grd_Initializer();
                Fill_Control(); 
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                grdViewer.Cols.Count = 13;
                grdViewer.Rows.Count = 2;
                grdViewer.Cols[(int)GRD.ID].Width = 00;
                grdViewer.Cols[(int)GRD.TourID].Width = 100;
                grdViewer.Cols[(int)GRD.Guest].Width = 100;
                grdViewer.Cols[(int)GRD.CompID].Width = 00;
                grdViewer.Cols[(int)GRD.CompName].Width = 00;
                grdViewer.Cols[(int)GRD.VoucherID].Width = 100;
                grdViewer.Cols[(int)GRD.RefNo].Width = 100;
                grdViewer.Cols[(int)GRD.PayableToID].Width = 00;
                grdViewer.Cols[(int)GRD.PayableTo].Width = 100;
                grdViewer.Cols[(int)GRD.PaidDate].Width = 100;
                grdViewer.Cols[(int)GRD.Amount].Width = 100;
                grdViewer.Cols[(int)GRD.Settled].Width = 100;
                grdViewer.Cols[(int)GRD.Outstanding].Width = 100;
                grdViewer.Cols[(int)GRD.ID].Caption = "ID";
                grdViewer.Cols[(int)GRD.TourID].Caption = "Tour ID";
                grdViewer.Cols[(int)GRD.Guest].Caption = "Guest";
                grdViewer.Cols[(int)GRD.CompID].Caption = "Comp ID";
                grdViewer.Cols[(int)GRD.CompName].Caption = "Comp Name";
                grdViewer.Cols[(int)GRD.VoucherID].Caption = "Voucher ID";
                grdViewer.Cols[(int)GRD.RefNo].Caption = "Ref No";
                grdViewer.Cols[(int)GRD.PayableToID].Caption = "Payable To ID";
                grdViewer.Cols[(int)GRD.PayableTo].Caption = "Payable To";
                grdViewer.Cols[(int)GRD.PaidDate].Caption = "Paid Date";
                grdViewer.Cols[(int)GRD.Amount].Caption = "Amount";
                grdViewer.Cols[(int)GRD.Settled].Caption = "Settled";
                grdViewer.Cols[(int)GRD.Outstanding].Caption = "Outstanding";
                grdViewer.Cols[(int)GRD.PaidDate].DataType = Type.GetType("System.DateTime");
                grdViewer.Cols[(int)GRD.Amount].Format = "##.##";
                grdViewer.Cols[(int)GRD.Settled].Format = "##.##";
                grdViewer.Cols[(int)GRD.Outstanding].Format = "##.##";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
         private void Fill_Control()
        {
            try
            { 
                ucFilterByCompany1.cmbICompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
                ucFilterByOther1.drpOther.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT MAX(ID),Name FROM vw_ALL_PERSON_DETAILS WHERE Name<>'' GROUP BY Name ORDER BY Name");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
         private void btnFilter_Click(object sender, EventArgs e)
         {
             try
             {
                 grdViewer.Rows.Count = 1;
                 if (rdbIOU.Checked)
                     fill_IOU();
             }
             catch (Exception ex){db.MsgERR(ex);}
         }
         private void fill_IOU()
         {
             try
             {
                 string qry = "SELECT DISTINCT CP.ID,CP.TransID,ISNULL(TBS.TourID,'X')TourID,TBS.Guest,CP.CompID,"+
                 "CMP.DisplayName,CP.VoucherID,CP.RefNo,CP.PayableTo PayableToID,APD.Name PayableTo,"+
                 "CAST(CP.PaidDate AS DATE)PaidDate,"+
		         "(SELECT SUM(CPD.Debit) FROM dbo.act_CashPayment_Detail CPD WHERE CPD.CashPayID=CP.ID)Amount,"+
		         "(SELECT ISNULL(SUM(CPD.Credit),0) FROM dbo.act_CashPayment_Detail CPD WHERE CPD.IouID=CP.ID)Settled"+		
		         " FROM dbo.act_CashPayment CP LEFT OUTER JOIN dbo.act_CashPayment_Detail CPD2"+
		         " ON CPD2.IouID=CP.ID INNER JOIN"+
		         " vw_ALL_PERSON_DETAILS APD ON APD.ID=CP.PayableTo AND ISNULL(CPD2.Debit,0)=0 LEFT OUTER JOIN"+
		         " vw_TourBasics TBS ON CP.TransID=TBS.ID INNER JOIN"+
		         " dbo.mst_CompanyGenaral CMP ON CP.CompID=CMP.ID AND CP.[Type]='IOU' WHERE 1=1";
                 if (ucFilterByCompany1.chkICmpny.Checked)
                     qry += " AND CP.CompID="+ucFilterByCompany1.cmbICompany.SelectedValue+"";
                 if (ucFilterByDate1.chkIByDate.Checked)
                     qry += " AND CAST(CP.PaidDate AS DATE)>=" + ucFilterByDate1.dtpIFromDate.Value.ToString("yyyy-MM-dd") +
                            " AND CAST(CP.PaidDate AS DATE)<=" + ucFilterByDate1.dtpIToDate.Value.ToString("yyyy-MM-dd") + "";
                 if (ucFilterByOther1.chkIByOther.Checked && ucFilterByOther1.drpOther.SelectedValue+"".Trim()!="")
                     qry += " AND CP.PayableTo="+ucFilterByOther1.drpOther.SelectedValue+"";
                 if (rdbSettled.Checked)
                     qry += " AND (SELECT SUM(CPD.Debit) FROM dbo.act_CashPayment_Detail CPD WHERE CPD.CashPayID=CP.ID" +
                            " AND ISNULL(CPD.IsDeleted,0)<>1) = (SELECT ISNULL(SUM(CPD.Credit),0)" +
                            " FROM dbo.act_CashPayment_Detail CPD WHERE CPD.IouID=CP.ID AND ISNULL(CPD.IsDeleted,0)<>1)";
                 else if (rdbUnSettled.Checked)
                     qry += " AND (SELECT SUM(CPD.Debit) FROM dbo.act_CashPayment_Detail CPD WHERE CPD.CashPayID=CP.ID" +
                            " AND ISNULL(CPD.IsDeleted,0)<>1) <> (SELECT ISNULL(SUM(CPD.Credit),0)" +
                            " FROM dbo.act_CashPayment_Detail CPD WHERE CPD.IouID=CP.ID AND ISNULL(CPD.IsDeleted,0)<>1)";
                 else if (rdbCancelled.Checked)
                     qry += " AND ISNULL(CP.IsCancelled,0)=1";
                 else if (rdbAl.Checked)
                     qry += " AND ISNULL(CP.IsCancelled,0)<>1";
                 qry += " ORDER BY PaidDate";
                 DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(qry);
                 int row=1;
                 decimal amt, set;
                 foreach (DataRow dr in DT.Rows)
                 {
                     grdViewer.Rows.Add();
                     amt = 0; set = 0;
                     grdViewer[row, (int)GRD.ID] = dr["ID"];
                     grdViewer[row, (int)GRD.TourID] = dr["TourID"];
                     grdViewer[row, (int)GRD.Guest] = dr["Guest"];
                     grdViewer[row, (int)GRD.CompID] = dr["CompID"];
                     grdViewer[row, (int)GRD.CompName] = dr["DisplayName"];
                     grdViewer[row, (int)GRD.VoucherID] = dr["VoucherID"];
                     grdViewer[row, (int)GRD.RefNo] = dr["RefNo"];
                     grdViewer[row, (int)GRD.PayableToID] = dr["PayableToID"];
                     grdViewer[row, (int)GRD.PayableTo] = dr["PayableTo"];
                     grdViewer[row, (int)GRD.PaidDate] = dr["PaidDate"] + "".Trim() != "" ? Convert.ToDateTime(dr["PaidDate"]).ToString() : null;
                     amt = dr["Amount"] + "".Trim() != "" ? Convert.ToDecimal(dr["Amount"]) : 0;
                     set = dr["Settled"] + "".Trim() != "" ? Convert.ToDecimal(dr["Settled"]) : 0;
                     grdViewer[row, (int)GRD.Amount] = amt.ToString();
                     grdViewer[row, (int)GRD.Settled] = set.ToString();
                     grdViewer[row, (int)GRD.Outstanding] = (amt-set).ToString();
                     row++;
                 }
             }
             catch (Exception ex){db.MsgERR(ex);}
         }
         private void btnIPreview_Click(object sender, EventArgs e)
         {
             Print_Report();
         }
         private void Print_Report()
         {
             try
             {
                 Account_Reports.frm_WebController wc = new Tourist_Management.Account_Reports.frm_WebController();
                 string comp = "";
                 if (ucFilterByCompany1.chkICmpny.Checked)
                     comp = ucFilterByCompany1.cmbICompany.Text.ToUpper();
                 wc.ReportName = "HAA";// comp + "\nTrial Balance as at " + dtpIMonthF.Value.ToString("yyyy-MMMM-dd") + "".ToUpper();
                 DataTable dt = new DataTable();
                 dt = set_DataTable();
                 if (dt != null)
                 {
                     wc.dataTable = dt;
                     wc.Show();
                 }
                 else
                     MessageBox.Show("No records to be previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
             }
             catch (Exception ex){db.MsgERR(ex);}
         }
         private DataTable set_DataTable()
         {             
             try
             {
                 if (grdViewer.Rows.Count <= 1)
                     return DT;
                 DT = new DataTable();
                 string name,val,type;
                 StringBuilder sb = new StringBuilder();
                 foreach (C1.Win.C1FlexGrid.Column item in grdViewer.Cols)
                 {
                     if (item.Width == 0)
                         continue;
                     name = item[0]+"".Trim();                     
                     val = item[1]+"".Trim();
                     if (Classes.clsGlobal.IsNumeric(val))
                         type = "System.String";
                     else
                         type = "System.String";
                     set_DataTable_Columns(name, name, type);
                 }
                 string value="";
                 if (rdbIOU.Checked)
                 {
                     #region IOU
                     foreach (C1.Win.C1FlexGrid.Row row in grdViewer.Rows)
                     {
                         if (row.Index == 0)
                             continue;
                         foreach (C1.Win.C1FlexGrid.Column col in grdViewer.Cols)
                         {
                             if (col.Width == 0)
                                 continue;
                             if(row[col.Index]+"".Trim()=="")
                                value += "\"X\",".Trim();                                
                             else
                                value += "\""+row[col.Index]+"\",".Trim();
                             if (value.Length > 1)
                                 value = value.Substring(0, value.Length - 1);
                             else
                                 value = "";
                             if (value.Trim() != "")
                                 DT.Rows.Add(value);
                         }                       
                     }
                     #endregion
                 }
                 return DT;
             }
             catch (Exception ex)
             {
                 db.MsgERR(ex);
                 DT = null;
                 return DT;
             }
         }
         private void set_DataTable_Columns(string name, string caption, string type)
         {
             try
             {
                 DataColumn column;
                 column = new DataColumn();
                 DataColumnCollection collections = DT.Columns;
                 column.ColumnName = name.Trim();
                 column.DataType = System.Type.GetType(type);
                 column.Unique = false;
                 column.AutoIncrement = false;
                 column.Caption = caption.Trim();
                 column.ReadOnly = false;
                 DT.Columns.Add(column);
             }
             catch (Exception ex){db.MsgERR(ex);}
         }
    }
}
