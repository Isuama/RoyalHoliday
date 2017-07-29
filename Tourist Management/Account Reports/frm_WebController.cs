using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
namespace Tourist_Management.Account_Reports
{   
    public partial class frm_WebController : Form
    {
        private const string msghd = "Web Controller";
        string htmlPath = @".\WebController\tableDesign.htm";
       public string ReportName="CASH BOOK";
       public DataTable dataTable;
        public frm_WebController(){InitializeComponent();}
        private void frm_WebController_Load(object sender, EventArgs e)
        {
            writeHtml();
            PrintHelpPage();
            webBrowser1.ShowPrintPreviewDialog();//.ShowPrintDialog();
        }
        private void writeHtml()
        {
            try
            {
                StringBuilder val =new StringBuilder();
                    val.Append("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                             "<html><head>" +
                             "<link rel=\"stylesheet\" href=\"dataGrid.css\" type=\"text/css\"/>" +
                             "<link rel=\"stylesheet\" href=\"reportSettings.css\" type=\"text/css\"/>" +
                             "</head><body>" +
                             "<p class=\"mainHeading\">" +
                             "" + ReportName + "" +
                             "</p>");
                    val.Append("<div class=\"datagrid\"><table><thead><tr>"); 
                    foreach (DataColumn dtCol in dataTable.Columns) val.Append("<th>"+dtCol.Caption+"</th>");
                    val.Append("</tr></thead>");
                val.Append("<tbody>");                            
                foreach (DataRow row in dataTable.Rows) // Loop over the rows.
	            {
                    val.Append("<tr>"); 
	                foreach (var item in row.ItemArray) // Loop over the items.
	                {
                        if(Classes.clsGlobal.IsNumeric(item.ToString()))
                            val.Append("<td align=\"right\">" + item + "</td>"); 
                        else
                            val.Append("<td align=\"left\">" + item + "</td>"); 
	                }
                    val.Append("</tr>"); 
	            }
                val.Append("</tbody>");
            using (StreamWriter writer = new StreamWriter(htmlPath))
                writer.Write(val);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void PrintHelpPage()
        {
            try
            {               
                FileInfo f = new FileInfo(htmlPath);
                webBrowser1.Navigate(f.FullName.Trim());
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
