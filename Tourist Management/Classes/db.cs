using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Tourist_Management.Reports;
namespace Tourist_Management
{
    class db
    {
        public static SqlConnection  dCon;
        public static int EXECUTE(string table, string where, string fields, params object[] para)
        {
            string sql,isql="",usql="";
            int i=0;
            foreach (string s in fields.Split(",".ToCharArray())) { isql += ",@" + i; usql += "," + s + "=@" + i++; }
            if (where != "") sql = "UPDATE [" + table + "] SET " + usql.Substring(1) + "=? WHERE " + where;
            else sql = "INSERT INTO [" + table + "] (" + fields + ") VALUES (" + isql.Substring (1)  + ");";
            if(dCon==null){
                dCon= Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                if (dCon.State == ConnectionState.Closed) dCon.Open(); 
            }
            SqlCommand com = new SqlCommand(sql,dCon);
            i = 0; foreach (object o in para) com.Parameters.AddWithValue("@" + i++, o); 
            return com.ExecuteNonQuery();
        }
        public static object GetInsertID() { return (new SqlCommand("SELECT @@IDENTITY ", dCon)).ExecuteScalar(); }
        public static DataRow Record(string sql)
        {
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            string id = (new Tourist_Management.Other.frmFilterRecords()).Load_search(DT);
            return (id + "".Trim() == "")  ? null : DT.Select(DT.Columns[0].ColumnName+ "=" + id)[0]; 
        }
        public static object Scaler(string sql)
        {
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql); 
            return DT.Rows.Count == 0 ? (object)null : (object)DT.Rows[0].ItemArray[0]; 
        }
        public static Boolean ShowErrors(List<string> Errors)
        {
            if (Errors.Count <= 0) return false ;
            string Msg = "Please fix these errors.\r\n\r\n";
            foreach (string s in Errors) Msg += "- " + s + "\r\n";
            MessageBox.Show(Msg,"Errors Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return true ;
        } 
        public static void showReport(ReportDocument cr, params object[] pr)
        {
            int i = 0;
            foreach (CrystalDecisions.Shared.ParameterField p in cr.ParameterFields) if (pr.Length > i) cr.SetParameterValue(p.Name, pr[i++]);
            if (Classes.clsGlobal.Con != null)
                cr.DataSourceConnections[0].SetConnection(Classes.clsGlobal.Con.SERVER, Classes.clsGlobal.Con.DATABASE, Classes.clsGlobal.Con.USERID, Classes.clsGlobal.Con.PASSWORD);
            frmMasterReports f = new frmMasterReports();
            f.CRV.ReportSource = cr;
            f.pan.Visible = false;
            f.ShowDialog();
        }
        public static void showReportExport(ReportDocument cr, string sql, string ty, params object[] pr)
        {
            
            DataSet DTP = new DataSet();
            Classes.clsConnection.Fill_DataAdapter(sql).Fill(DTP);
            if (ty != "INVOICE") cr.SetDataSource(DTP.Tables[0]);
            else if (Classes.clsGlobal.Con != null)
                cr.DataSourceConnections[0].SetConnection(Classes.clsGlobal.Con.SERVER, Classes.clsGlobal.Con.DATABASE, Classes.clsGlobal.Con.USERID, Classes.clsGlobal.Con.PASSWORD);
      
            frmMasterReports f = new frmMasterReports();
            f.CRV.ReportSource = cr;
            f.pan.Visible = false;  int i = 0;
            foreach (CrystalDecisions.Shared.ParameterField p in cr.ParameterFields) if (pr.Length > i) cr.SetParameterValue(p.Name, pr[i++]);
        
        //    f.CRV.RefreshReport();

            //cr.Refresh(); 
            if (ty != "INVOICE") TransacReports.frmReportViewer.dores(DTP); 
            TransacReports.frmReportViewer.Export (f.CRV,DTP.Tables[0],"PDF",ty  );

            f.ShowDialog();
        }
        public static void showReport(ReportDocument cr, string sql)
        {
            DataSet DTP = new DataSet();  
           Classes.clsConnection.Fill_DataAdapter(sql).Fill(DTP); 
           showReport(cr, DTP.Tables[0]);
        }
        public static void showReport2(ReportDocument cr, string sql)
        {
            DataSet DTP = new DataSet();
            Classes.clsConnection.Fill_DataAdapter(sql).Fill(DTP);
            cr.SetDataSource(DTP.Tables[0] );

            for (int i = 0; i < cr.Subreports.Count; i++)
            {

                  DTP = new DataSet();
                Classes.clsConnection.Fill_DataAdapter(sql).Fill(DTP);
                cr.Subreports[i].SetDataSource(DTP.Tables[0] );
                
            }
            //showReport(cr );

            frmMasterReports f = new frmMasterReports();
            f.CRV.ReportSource = cr;
            f.pan.Visible = false;
            f.ShowDialog();
        }
        public static void showReport(ReportDocument cr, DataSet ds)
        {
            cr.SetDataSource(ds);
            showReport(cr);
        }
        public static void showReport(ReportDocument cr, DataTable dt)
        {
            cr.SetDataSource(dt);
            showReport(cr);
        }
        public static void showReport(ReportDocument cr)
        { 
            frmMasterReports f = new frmMasterReports();
            f.CRV.ReportSource = cr;
            f.pan.Visible = false;
            f.ShowDialog();
        }
        public static void showMasterReport( Tourist_Management.Reports.frmMasterReports.clrReport cr, string filter = "", params object[] pr)
        {
            if (filter != "") cr.GetReport().RecordSelectionFormula += (cr.GetReport().RecordSelectionFormula != "" ? " and " : " ") + filter;
            frmMasterReports f = new frmMasterReports();
            f.add( cr); 
            f.cbReports.SelectedIndex = 0;
            f.button1_Click(null, null);
            f.ShowDialog(); 
        }
        public static bool IsRate(object cur,object v) {
            return !((db.Val(cur) <= 0 && db.Val(v) != 1) || (db.Val(cur) > 1 && (db.Val(v) <0 || db.Val(v) > 500)));
        }
        public static void LoadDropDown(DropDowns.DropSearch c, string sql) { LoadDropDown(c, Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql)); }
        public static void LoadDropDown(DropDowns.DropSearch c, DataTable dt) { c.DataSource = dt; }
        public static void LoadDropDown(ComboBox c, string sql) { LoadDropDown(c, Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql)); }
        public static void LoadDropDown(ComboBox c, DataTable dt) { c.DataSource = dt; }
        public static void LoadSearch(ComboBox c) { c.DisplayMember = "j";c.ValueMember="i"; c.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select * FROM (Select 'Like'+''''+'%##%'+''''+'' i,'Contains' j,1 x Union Select 'Like'+''''+'##%'+''''+'','Begins with',2 Union Select 'Like'+''''+'%##'+''''+'','Ends with',3 Union Select '=','Equal',4 Union Select '<>','Not Equal',5 Union Select '>=','Greater than or Equal',6 Union Select '>','Greater than',7 Union Select '<=','Less than or Equal',8 Union Select '<','Less than',9)v order by v.x"); c.SelectedIndex = 0; }

        public static double Val(object v) { double d = 0; return (double.TryParse("" + v, out d)) ? d : 0d; }
        public static int Int(object v) { int d = 0; return (int.TryParse("" + v, out d)) ? d : 0; }
        public static string N2(object v) { return string.Format("{0:n2}", Val(v)); }

        public static void GridInit(params object[] ar) {
            C1.Win.C1FlexGrid.C1FlexGrid dg=null;
            C1.Win.C1FlexGrid.Column c = null;
  
            foreach (object i in ar)
                if (i is C1.Win.C1FlexGrid.C1FlexGrid) { dg = (C1.Win.C1FlexGrid.C1FlexGrid)i; c = null;           int Count = 0;
            foreach (object j in ar)  if (j is Enum)   if (Count <= (int)j) Count = (int)j + 1; dg.Cols.Count = Count; }
                else if (i is Enum)
                {
                    //if (dg.Cols.Count <= (int)i) dg.Cols.Count = (int)i + 1;
                    c = dg.Cols[(int)i];
                }
                else if (i is int) { if (c == null) dg.Rows.Count = (int)i; else c.Width = (int)i; }
                else if (i is string) { string s = (string)i; if (s.Contains("#")) c.Format = s; else c.Caption = s; }
                else if (i is bool) { if (c == null)dg.Rows[1].AllowEditing = (bool)i; else  c.ComboList = "..."; }
                else if (i is Type) c.DataType = (Type)i;
                else { MessageBox.Show(i + " not identified whiloading grid."); } 
        }


        public static void GridLoad(C1.Win.C1FlexGrid.C1FlexGrid grdGuide, DataTable DTGudie, params object[] ar)
        {
            List<int> iar = new List<int>();
            List<string> sar = new List<string>();
            foreach (object o in ar) if (o is string) sar.Add(o + ""); else iar.Add((int)o);
            if (DTGudie.Rows.Count > 0)
            {
               int RowNumb = 0;
                while (DTGudie.Rows.Count > RowNumb)
                {
                    foreach (int i in iar)    grdGuide[RowNumb + 1, i] = DTGudie.Rows[RowNumb][sar[i]]+"";
                    RowNumb++;
                }
            }
        }

        public static void MsgERR(Exception ex)
        {
            MessageBox.Show(ex.Message, Form.ActiveForm == null ? "" : Form.ActiveForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
