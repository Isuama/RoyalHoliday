using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
namespace Tourist_Management.Transaction
{
    public partial class frmItinerary : Form
    {
        private const string msghd = "Itinerary Details";
        public int Mode = 0, SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public frmItinerary() { InitializeComponent(); }
        private void frmItinerary_Load(object sender, EventArgs e) { Fill_Control(); }
        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
        private void btnPrint_Click(object sender, EventArgs e) { Print_Transaction_Report(); }
        private void Fill_Control()        {                 db.LoadDropDown(drpDriver, "SELECT ID,Name FROM vw_TR_Driver Where IsNull(IsActive,0)=1 ORDER BY Name");         }
        private void Print_Transaction_Report()
        {
            try
            {
                if (drpDriver.SelectedValue == null && chkAll.Checked == false) return;
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                string sql = "";
                DataTable DT;
                DataSet DTG = new DataSet();
                ReportDocument ga = new ReportDocument();
                if (chkAll.Checked != true) sql = "SELECT Date,Time,CityFrom,CityTo,DriverID,DriverName,VehicleNo,Distance,TourID,Guest,AFlightNo,AFlightTime,Country FROM vw_rpt_Itinerary WHERE DriverID=" + drpDriver.SelectedValue.ToString().Trim() + "";
                else sql = "SELECT Date,Time,CityFrom,CityTo,DriverID,DriverName,VehicleNo,Distance,TourID,Guest,AFlightNo,AFlightTime,Country FROM vw_rpt_Itinerary";
                DTG = new DataSets.Transport_DS.ds_trn_Itinerary();
                ga = new Tourist_Management.Reports.Itinerary();
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0) sConnection.Print_Report(SystemCode.ToString(), sql, DTG, ga, "");
                else MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            drpDriver.setSelectedValue(null);
            drpDriver.Enabled = !chkAll.Checked;
        }
    }
}
