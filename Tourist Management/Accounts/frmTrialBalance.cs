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
    public partial class frmTrialBalance : Form
    {
        private const string msghd = "Trial Balance";
        public frmTrialBalance(){InitializeComponent();}
        private void frmTrialBalance_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
            DateTime today = Classes.clsGlobal.CurDate();
            dtpYear.Value = today;            
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            dtpIMonthF.Value= Convert.ToDateTime(Classes.clsGlobal.CurDate().Year+"-04-01");
            dtpIMonthT.Value= endOfMonth;
            cmbCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Print_Report()
        {
            try
            {
                Account_Reports.frm_WebController wc = new Tourist_Management.Account_Reports.frm_WebController();
                string comp = "";
                if (chkComp.Checked)
                    comp = cmbCompany.Text.ToUpper();
                wc.ReportName = comp + "\nTrial Balance as at " + dtpIMonthF.Value.ToString("yyyy-MMMM-dd") + "".ToUpper();
                DataTable dt = new DataTable();
                dt = set_TrialBalance();
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
        private DataTable set_TrialBalance()
        {
            DataTable DTB = new DataTable();
            try
            {             
                DateTime DF = new DateTime(dtpYear.Value.Year, 04, 01);
                DateTime DT = new DateTime(dtpYear.Value.Year, dtpIMonthF.Value.Month, DateTime.DaysInMonth(dtpYear.Value.Year, dtpIMonthF.Value.Month));
                int CompID= Convert.ToInt32( cmbCompany.SelectedValue);
                string qry = "SELECT AccountName,"+
                             " SUM(Credit)Credit,SUM(Debit)Debit FROM vw_TrialBalance" +
                             " WHERE CompID="+CompID+" AND"+
                             " PaidDate>='"+DF.ToString("yyyy-MM-dd").Trim()+"' AND"+
                             " PaidDate<='"+DT.ToString("yyyy-MM-dd").Trim()+"'"+
                             " GROUP BY AccountID,AccountName ORDER BY AccountName";
                DTB = Classes.clsGlobal.objCon.Fill_Table(qry);
                return DTB;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return DTB;
            }
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            Print_Report();
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
    }
}
