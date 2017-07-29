using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Reports
{
    public partial class frmReportType : Form
    {
        static DataTable DT;
        static double tourid;
        public static double TourID
        {
            get
            {
                return tourid;
            }
            set
            {
                tourid = value;
            }
        }
        public static DataTable DataTable
        {
            get
            {
                return DT;
            }
            set
            {
                DT = value;
            }
        }
        public frmReportType(){InitializeComponent();}
        private void frmReportType_Load(object sender, EventArgs e)
        {
            Fill_Control();
        }
        private void Fill_Control()
        {       
                cmbReportType.DataSource = DT; 
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Transaction.frmGroupAmend fga = new Tourist_Management.Transaction.frmGroupAmend();
            int reportid = Convert.ToInt32(cmbReportType.SelectedValue.ToString().Trim());
            fga.Print_Transaction_Report(tourid,reportid);
        }
    }
}
