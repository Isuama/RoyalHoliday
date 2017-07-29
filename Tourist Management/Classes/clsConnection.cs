using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using CRPT;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Win32;
namespace Tourist_Management.Classes
{
    class clsConnection
    {
        public static SqlConnection sqlCon;
        public static string ConnectionString, UN, PW;
        public SqlConnection sqlConReturn_Connection(string str)
        {
            string[] MyArray;
            sqlCon = new SqlConnection();
            CRPT.CRPT Crpt= new CRPT.CRPT();
            str = Crpt.DECRYPT(str, Tourist_Management.Classes.clsGlobal.RevertME());
            str = Crpt.DECRYPT(str, Tourist_Management.Classes.clsGlobal.RevertME());
            MyArray = Regex.Split(str, " `'`' ");
            if (MyArray.Length < 5)
            {
                sqlCon.ConnectionString = "";
                return sqlCon;
            }
            if (MyArray[2] == "1") sqlCon.ConnectionString = "Integrated Security=True;Data Source=" + MyArray[0] + ";Initial Catalog=Master";
          else  sqlCon.ConnectionString = "Data Source=" + MyArray[0] + ";Initial Catalog=" + MyArray[1] + ";User ID=" + (UN = MyArray[3]) + ";Password=" + (PW = MyArray[4]) + "";
           return sqlCon;
        }
        public static string Read_Connection_String()
        {
            string[] MyArray;
            sqlCon = new SqlConnection();
            CRPT.CRPT Crpt = new CRPT.CRPT();
            MyArray = Regex.Split(ConnectionString, " `'`' ");
            if (MyArray.Length < 5)
            {
                sqlCon.ConnectionString = "";
                return "";
            }
            if (MyArray[2] == "1") return "Integrated Security=True;Data Source=" + MyArray[0] + ";Initial Catalog=Master";
            else  return "Data Source=" + MyArray[0] + ";Initial Catalog=" + MyArray[1] + ";User ID=" + MyArray[3] + ";Password=" + MyArray[4] + "";
             }
        public DataTable Fill_Table(string ssql, SqlConnection con)
        {
            try
            {
                if (con.State == ConnectionState.Open)  con.Close();
                con.Open();
            SqlDataAdapter    DA = new SqlDataAdapter(ssql, con);
              DataTable  DT = new DataTable();
                DA.Fill(DT);
                return DT;
            }
            catch (InvalidCastException ex)       {throw (ex);   }
            finally  { con.Close(); }
        }
        public static SqlDataAdapter Fill_DataAdapter(string ssql)
        {
            SqlConnection SqlCon = new SqlConnection(Read_Connection_String());
            try
            {
                if (SqlCon.State == ConnectionState.Open)   SqlCon.Close();
                SqlCon.Open();
                return new SqlDataAdapter(ssql, SqlCon);
            }
            catch (Exception ex)  {throw (ex);   }
            finally   {  SqlCon.Close(); }
        }
         public static string getSingle_Value_Using_Reader(string ssql)
        {
            string val = null;
              SqlConnection con = new SqlConnection(Read_Connection_String());
            try
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Open();
                SqlCommand com = new SqlCommand(ssql, con);
              SqlDataReader  DR = com.ExecuteReader();
                if (DR.Read()) val = DR[0] + "".Trim();
                return val;
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show(ex.Message, "SQL Reader", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally    {  con.Close();  }
        }
        public void Print_Report(string TransID, string ssql, DataSet DTS, CrystalDecisions.CrystalReports.Engine.ReportDocument RPT, string reptype, params SqlParameter[] par)
        {
            TransacReports.frmReportViewer frv = new Tourist_Management.TransacReports.frmReportViewer();
             try
            {
              SqlDataAdapter  DA = Fill_DataAdapter(ssql);
                DA.Fill(DTS.Tables[0]);
                frv.ReportType = reptype;
                frv.Dataset = DTS;
                frv.Tour = TransID;
                frv.Paras = par;
                frv.CrystalObject = RPT;
                frv.ShowDialog();
            }
            catch (Exception ex)  {   throw (ex); }
            finally  { frv = null;   GC.Collect();}
        }
        public void Print_Report_New(DataSet ds, CrystalDecisions.CrystalReports.Engine.ReportDocument RPT, string TransID)
        {
            TransacReports.frmReportViewer frv = new Tourist_Management.TransacReports.frmReportViewer();
            SqlDataAdapter DA = new SqlDataAdapter();
            DataSet DTS = new DataSet();
            try
            {
                DTS = ds;
                frv.ReportType = "";
                frv.Dataset = DTS;
                frv.Tour = TransID;
                frv.CrystalObject = RPT;
                frv.ShowDialog();
            }
            catch (Exception ex)  {throw (ex); }
            finally  { frv = null;   GC.Collect();   }
        }   
        public void Print_Via_Datatable(DataSet DTS, DataTable DT, CrystalDecisions.CrystalReports.Engine.ReportDocument RPT, string reptype, params SqlParameter[] par)
        {
            TransacReports.frmReportViewer frv = new Tourist_Management.TransacReports.frmReportViewer();
            SqlDataAdapter DA = new SqlDataAdapter();
            DTS.Tables[0].Rows.Clear();
            foreach (DataRow dr in DT.Rows) DTS.Tables[0].Rows.Add(dr.ItemArray);
            frv.ReportType = reptype;
            frv.Dataset = DTS;
            frv.Tour = "";
            frv.Paras = par;
            frv.CrystalObject = RPT;
            frv.ShowDialog();
        } 
         }
}
