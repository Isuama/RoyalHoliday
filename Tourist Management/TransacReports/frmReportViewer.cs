using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Diagnostics;
namespace Tourist_Management.TransacReports
{
    public partial class frmReportViewer : Form
    { 
        CrystalDecisions.CrystalReports.Engine.ReportDocument  objRpt; 
        DataSet DS; 
          string msghd = "Preview Transaction",RType,InvNo="",ToutID,BookingName; 
        static string[] Hotels,VoucherID;
        static DateTime[] CheckIn;
        static bool AutoExport = true;
        public   SqlParameter[] Paras = { }; 
        public frmReportViewer()   { InitializeComponent(); }
        public CrystalDecisions.CrystalReports.Engine.ReportDocument CrystalObject { get { return objRpt; } set { objRpt = value; } }
        public string Tour { get { return ToutID; } set { ToutID = value; } }
        public string ReportType { get { return RType; } set { RType = value; } }  
        public DataSet Dataset { get { return DS; } set { DS = value; } }
        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            try
            { 
                ParameterPass(); 
                if (DS.Tables.Count == 1)
                {
                    DataTable dt = DS.Tables[0];
                    objRpt.SetDataSource(DS.Tables[0]);
                }
                else if (DS.Tables.Count>1) objRpt.SetDataSource(DS); 
                else  objRpt.SetDataSource(DS.Tables[1]); 
                foreach (SqlParameter p in Paras) objRpt.SetParameterValue(p.ParameterName, p.Value);
                if (objRpt is Reports.ProfitNloss) objRpt.SetParameterValue("TTID", DS.Tables[0].Rows[0]["TransID"]);     
                TransacReports.frmReportViewer frv = new Tourist_Management.TransacReports.frmReportViewer();
                int rownumb = 0;//, arnumb = 0;
                btnExport.Visible = false;
                if (RType == "RESERVATION")
                {
                    ToutID = DS.Tables[0].Rows[rownumb]["TransID"].ToString();  //tbl  1 -> 0
                    BookingName = DS.Tables[0].Rows[rownumb]["Guest"].ToString();  //tbl  1 -> 0
                    dores(DS);
                    cmbEXType.Items.Add("PDF");
                    cmbEXType.Items.Add("WORD");  
                    CRViewer.ReportSource = objRpt;
                    CRViewer.Refresh(); 
                    cmbEXType.SelectedIndex = 0;
                    btnExport.Visible = true;
                    btnExport.Enabled = true;
                    Export(CRViewer, DS.Tables[0], cmbEXType.Text.ToString().Trim(), RType);                    
                }
                else if (RType == "INVOICE")
                {
                    Set_Invoice_No(); 
                    cmbEXType.Items.Add("PDF");
                    cmbEXType.Items.Add("WORD");  
                    CRViewer.ReportSource = objRpt;
                    CRViewer.Refresh(); 
                    cmbEXType.SelectedIndex = 0;
                    btnExport.Visible = true;
                    btnExport.Enabled = true;
                    Export(CRViewer, DS.Tables[0], cmbEXType.Text.ToString().Trim(), RType);                    
                }
                else if (RType == "TRDRIVER")
                {
                    btnExport.Visible = false;
                    cmbEXType.Visible = false;
                    CRViewer.ReportSource = objRpt;
                    Export("C:\\Temp\\trDriverDetailsRpt","TransportDriverDetails.pdf");
                    CRViewer.Refresh();
                }
                else if (RType == "TRVOUCHER")
                {
                    btnExport.Visible = false;
                    cmbEXType.Visible = false;
                    CRViewer.ReportSource = objRpt;
                    Export("C:\\Temp\\trVoucherDetails", "TransportVoucherDetails.pdf"); 
                    CRViewer.Refresh();
                }
                else if (RType == "DEBTER")
                {
                    btnExport.Visible = false;
                    cmbEXType.Visible = false;
                    CRViewer.ReportSource = objRpt;
                    Export("C:\\Temp\\debterDetails", "DebterDetailsRpt.pdf"); 
                    CRViewer.Refresh();
                }
                else if (RType == "HotelMonthlyPay")
                { 
                    cmbEXType.Items.Add("PDF");
                    cmbEXType.Items.Add("WORD");
                    btnExport.Visible = true;
                    cmbEXType.Visible = true;
                    btnExport.Enabled = true;
                    CRViewer.ReportSource = objRpt;
                    Export("C:\\Temp\\HotelPayments", "HotelMonthlyPayments.pdf");
                    CRViewer.Refresh();
                }
                else if (RType == "HotelDailyPay")
                { 
                    cmbEXType.Items.Add("PDF");
                    cmbEXType.Items.Add("WORD"); 
                    btnExport.Visible = true;
                    cmbEXType.Visible = true;
                    btnExport.Enabled = true;
                    CRViewer.ReportSource = objRpt;
                    Export("C:\\Temp\\HotelPayments", "HotelDailyPayments.pdf"); 
                    CRViewer.Refresh();
                }
                else
                {
                    btnExport.Visible = false;
                    cmbEXType.Visible = false;
                    CRViewer.ReportSource = objRpt; 
                    CRViewer.Refresh();
                } 
            }
            catch (Exception ex)    {  throw (ex);  }
        }
        public static void dores(DataSet DS)
        {

            #region RESERVATION..AMENDMENT VOUCHERS.....
            DataTable dt = DS.Tables[0]; //tbl  1 -> 0
            var uniqueTouts = from val in DS.Tables[0].AsEnumerable() //tbl  1 -> 0
                              group val by val["UniqueHotelID"] into newhotel
                              select new
                              {
                                  ID = newhotel.Key,
                                  hotelName = newhotel.Max(id => id["HotelName"]),
                                  dateArrival = newhotel.Max(id => id["DateArrival"]),
                                  voucherID = newhotel.Max(id => id["VoucherNo"]),
                                  amendTime = newhotel.Max(id => id["AmendTime"])
                              };
            CheckIn = new DateTime[uniqueTouts.ToList().Count];
            Hotels = new string[uniqueTouts.ToList().Count];
            VoucherID = new string[uniqueTouts.ToList().Count];
            int count = 0;
            string vid = "";
            foreach (var items in uniqueTouts)
            {
                if (items.voucherID + "".Trim() != "") CheckIn[count] = Convert.ToDateTime(items.dateArrival);
                if (items.hotelName.ToString().Trim() == "") continue;
                Hotels[count] = items.hotelName.ToString().Trim();
                vid = items.voucherID.ToString().Trim();
                VoucherID[count] = vid.Replace("/", "_").Trim();
                count++;
            }
            #endregion
        }

        private void ParameterPass()     {   }
        private void btnExport_Click(object sender, EventArgs e)
        {
            AutoExport = false;
            if (ReportType == "HotelMonthlyPay")  Export("C:\\Temp\\HotelPayments", "HotelMonthlyPayments.pdf"); 
            else if (ReportType == "HotelDailyPay")   Export("C:\\Temp\\HotelPayments", "HotelDailyPayments.pdf");
            Export(CRViewer,DS.Tables[0], cmbEXType.Text.ToString().Trim(), RType );
        }

        private static string Get_Report_Path(string RType)
        {
                string sql;
                DataTable DT;
                if (RType == "RESERVATION")
                { 
                    #region FOR VOUCHERS
                    sql = "SELECT Path,IsNull(IsExport,0)AS IsExport FROM mst_TransReportSettings WHERE UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                    if (DT.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(DT.Rows[0]["IsExport"]) && DT.Rows[0]["Path"].ToString().Trim() != "") return DT.Rows[0]["Path"].ToString(); else   return "";
                    }
                    #endregion
                }
                else if (RType == "INVOICE")
                {
                    #region FOR INVOICES
                    sql = "SELECT InvoicePath,IsNull(InvoiceExport,0)AS InvoiceExport FROM mst_TransReportSettings WHERE UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                    if (DT.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(DT.Rows[0]["InvoiceExport"]) && DT.Rows[0]["InvoicePath"].ToString().Trim() != "")   return DT.Rows[0]["InvoicePath"].ToString();   else  return "";
                    }
                    #endregion
                } 
                return ""; 
        }
        private static string Get_File_Name(int i,string    RType,  string BookingName,string  InvNo)
        { 
                string rtn = ""; 
                if (RType == "RESERVATION")
                {
                    if (Hotels == null || Hotels[i - 1] == null)   {   return "";  }
                    rtn= Hotels[i - 1].Trim() + "-" + BookingName.Trim() + "-" + String.Format("{0:ddd,MMM d, yyyy}", CheckIn[i - 1]).Trim() + "-" + VoucherID[i - 1].Trim();
                } 
                if (RType == "INVOICE")     { rtn= "Invoice - ".Trim() + InvNo.Trim() + " - ".Trim() + BookingName.Trim();  } 
                return rtn.Trim(); 
        } 
        private void Set_Invoice_No()
        { 
                string ssql = "SELECT InvoiceNo FROM act_PaymentIssued WHERE  TransID=" + ToutID + " ORDER BY SrNo";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql); 
                if (DT.Rows.Count > 0)
                { 
                    InvNo = DT.Rows[DT.Rows.Count-1]["InvoiceNo"].ToString().Trim(); 
                    InvNo = InvNo.Replace("/", "_").Trim(); 
                    BookingName = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Guest from trn_GroupAmendment WHERE ID=" + ToutID + "").Rows[0]["Guest"].ToString();
                } 
        }        
        private  void Export(string path,String fileName)
        {
            try
            {
                bool IsExported = false; 
                ExportOptions ExportOpts = new ExportOptions();
                PdfRtfWordFormatOptions PdfRtfWordFormatOpts = new PdfRtfWordFormatOptions();
                DiskFileDestinationOptions DestinationOpts = new DiskFileDestinationOptions();
                System.Drawing.Printing.PrintDocument Doc = new System.Drawing.Printing.PrintDocument(); 
                if (path == "") return; 
                string newpath; 
                newpath = path; 
                System.IO.Directory.CreateDirectory(newpath); 
                ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                    IsExported = false;
                    if (fileName != "")
                    {
                        DestinationOpts.DiskFileName = (newpath.Trim() + "\\".Trim() + fileName.Trim()).Trim();
                        ExportOpts.ExportDestinationOptions = DestinationOpts;
                        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                        
                        objRpt.Export(ExportOpts);
                        IsExported = true;
                    } 
                if (IsExported == true && AutoExport == false)   MessageBox.Show("Exported Successfully"); 
            }
            catch(System.IO.IOException io){
                MessageBox.Show("Cannot access trDriverDetails.pdf please close the pdf file and try again.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw (io);
            }   catch( Exception ex){  db.MsgERR(ex); throw (ex);}

        }
        public static void ExportSet(string inv,string reportType)
        {
            AutoExport = true; 
        }
        public static void Export(CrystalDecisions.Windows.Forms.CrystalReportViewer crv,DataTable dt,string exty,string RType )
        { 

                bool IsExported = false; 
                ExportOptions ExportOpts = new ExportOptions();
                PdfRtfWordFormatOptions PdfRtfWordFormatOpts = new PdfRtfWordFormatOptions();
                DiskFileDestinationOptions DestinationOpts = new DiskFileDestinationOptions();
                System.Drawing.Printing.PrintDocument Doc = new System.Drawing.Printing.PrintDocument(); 
                crv.ShowLastPage();


                if (RType == "RESERVATION") dores(dt.DataSet);
                string BookingName = dt.Rows[0]["Guest"]+"";
                string ToutID = dt.Rows[0]["TransID"] + "";
                string InvNo =dt.Columns.Contains("InvoiceNo") ?(dt.Rows[0]["InvoiceNo"] + "").Replace("/", "_").Trim() :"";
             

              int  PageCount = crv.GetCurrentPageNumber(); 
                crv.ShowFirstPage(); 
                string Path = Get_Report_Path( RType );
                if (Path == "")    return;
                string newpath, filename; 
                for (int i = 1; i <= PageCount; i++)
                { 
                    if(Debugger.IsAttached)   newpath = System.IO.Path.Combine("c:\\rh", ToutID.Trim() + "-".Trim() + BookingName.Trim()); 
                    else newpath = System.IO.Path.Combine(Path.Trim(), ToutID.Trim() + "-".Trim() + BookingName.Trim()); 
                    System.IO.Directory.CreateDirectory(newpath);
                    #region EXPORT
                    IsExported = false;
                    PdfRtfWordFormatOpts.FirstPageNumber = i;
                    PdfRtfWordFormatOpts.LastPageNumber = i;
                    PdfRtfWordFormatOpts.UsePageRange = true;
                    ExportOpts.ExportFormatOptions = PdfRtfWordFormatOpts;
                            filename = Get_File_Name(i,RType, BookingName, InvNo); 
                            if (filename == "")   {  MessageBox.Show("NOT EXPORTED.");      break;  } 
                    switch (exty )
                    {
                        case "WORD":
                            ExportOpts.ExportFormatType = ExportFormatType.WordForWindows;
                            filename =(Hotels[i - 1].Trim() + "-".Trim() + BookingName.Trim() + "-" + String.Format("{0:ddd,MMM d, yyyy}", CheckIn[i - 1]) + "-" + VoucherID[i - 1].Trim() + ".doc").Trim();
                            filename += ".doc";
                            break;
                        default:
                            ExportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                            filename += ".pdf";                            
                            break; 
                    }
                    IsExported = false;
                    if (filename != "")
                    {
                        DestinationOpts.DiskFileName = (newpath.Trim() + "\\".Trim() + filename.Trim()).Trim();
                        ExportOpts.ExportDestinationOptions = DestinationOpts;
                        ExportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                        (( CrystalDecisions.CrystalReports.Engine.ReportDocument ) crv.ReportSource).Export(ExportOpts);
                        IsExported = true; 
                    } 
                    #endregion
                    #region SET EMAIL SETTINGS
                    if (filename.Trim() != "")
                    {
                        if (RType == "RESERVATION")
                        { 
                            Classes.clsGlobal.VoucherPath[i - 1] = (newpath.Trim() + "\\".Trim() + filename.Trim()).Trim();
                            Classes.clsGlobal.VoucherFileName[i - 1] = filename.Trim();
                        }
                        else if (RType == "INVOICE")
                        {
                            Classes.clsGlobal.InvoicePath = (newpath.Trim() + "\\".Trim() + filename.Trim()).Trim();
                            Classes.clsGlobal.InvoiceName = filename.Trim();
                        }
                    } 
                    #endregion
                }
                if (IsExported == true && AutoExport == false)
                    MessageBox.Show("Exported Successfully");
        }
    }
}
