using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
namespace Tourist_Management.Reports
{
    public partial class frmFuelMeter : Form
    {
        private const string msghd = "Fuel Meter";
        string DateFrom, DateTo, YST, YEN;
        enum FM { gTID, gDID, gDNM, gDTE, gSTM, gEDM, gBNM, gAKM, gBKM, gRPKM, gINC, gFUEL, gFRAT, gINS, gLSE, gBSC };
        public frmFuelMeter(){InitializeComponent();}
        private void frmFuelMeter_Load(object sender, EventArgs e)
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
                grdFM.Cols.Count = 16;
                grdFM.Rows.Count = 3000;
                grdFM.Cols[(int)FM.gTID].Width = 0;
                grdFM.Cols[(int)FM.gDID].Width = 0;
                grdFM.Cols[(int)FM.gDNM].Width = 0;
                grdFM.Cols[(int)FM.gDTE].Width = 100;
                grdFM.Cols[(int)FM.gSTM].Width = 100;
                grdFM.Cols[(int)FM.gEDM].Width = 100;
                grdFM.Cols[(int)FM.gBNM].Width = 150;
                grdFM.Cols[(int)FM.gAKM].Width = 100;
                grdFM.Cols[(int)FM.gBKM].Width = 100;
                grdFM.Cols[(int)FM.gRPKM].Width = 100;
                grdFM.Cols[(int)FM.gINC].Width = 100;
                grdFM.Cols[(int)FM.gFUEL].Width = 100;
                grdFM.Cols[(int)FM.gFRAT].Width = 0;
                grdFM.Cols[(int)FM.gINS].Width = 0;
                grdFM.Cols[(int)FM.gLSE].Width = 0;
                grdFM.Cols[(int)FM.gBSC].Width = 0;
                grdFM.Cols[(int)FM.gDID].Caption = "Tour ID";
                grdFM.Cols[(int)FM.gDID].Caption = "Driver ID";
                grdFM.Cols[(int)FM.gDNM].Caption = "Driver Name";
                grdFM.Cols[(int)FM.gDTE].Caption = "Date";
                grdFM.Cols[(int)FM.gSTM].Caption = "Start Meter";
                grdFM.Cols[(int)FM.gEDM].Caption = "End Meter";
                grdFM.Cols[(int)FM.gBNM].Caption = "Booking Name";
                grdFM.Cols[(int)FM.gAKM].Caption = "Actual KM";
                grdFM.Cols[(int)FM.gBKM].Caption = "Bill KM";
                grdFM.Cols[(int)FM.gRPKM].Caption = "Rate Per KM";
                grdFM.Cols[(int)FM.gINC].Caption = "Income (LKR)";
                grdFM.Cols[(int)FM.gFUEL].Caption = "Fuel (LKR)";
                grdFM.Cols[(int)FM.gFRAT].Caption = "Fuel Rate";
                grdFM.Cols[(int)FM.gINS].Caption = "Insurance";
                grdFM.Cols[(int)FM.gLSE].Caption = "Leasing";
                grdFM.Cols[(int)FM.gBSC].Caption = "Basic Salary";
                grdFM.Cols[(int)FM.gDTE].Format = "yyyy-MMM-dd";
                grdFM.Cols[(int)FM.gINC].Format = "##.##";
                grdFM.Cols[(int)FM.gFUEL].Format = "##.##";
                grdFM.Cols[(int)FM.gFRAT].Format = "##.##";
                grdFM.Cols[(int)FM.gINS].Format = "##.##";
                grdFM.Cols[(int)FM.gLSE].Format = "##.##";
                grdFM.Cols[(int)FM.gBSC].Format = "##.##";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Fill_Control()
        {
            try
            {
                DataTable[] DTB;
                DTB = new DataTable[1];
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM vw_TR_Driver Where IsNull(IsActive,0)=1 AND IsNull(Nullif(Name,''),'')<>'' ORDER BY Name");
                if (DTB[0].Rows.Count > 0)
                {
                    drpDriver.DataSource = DTB[0];
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {
                string format = "yyyy-MM-dd";
                string ssql = "";
                string filter = "";
                DateTime datefrom = dtpIFromDate.Value;
                DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpIToDate.Value;
                DateTo = dateto.ToString(format).Substring(0, 10);
                DateTime yst = new DateTime(DateTime.Now.Year,1,1);
                YST = yst.ToString(format).Substring(0, 10);
                DateTime yen = new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);
                YEN = yen.ToString(format).Substring(0, 10);
                ssql = "SELECT ID,DriverID,DriverName,DeprtDate,StartMeter,EndMeter,Guest,ActualKM,BillingKM," +
                       "RatePerKm,Income,Fuel,InsuranceAmt,Lease,BasicSalary " +
                       "FROM vw_Fuel_Meter";
                if (rdbMonthly.Checked)
                {
                    filter = " WHERE (DeprtDate BETWEEN '" + DateFrom.Trim() + "' AND '" + DateTo.Trim() + "')";
                }
                else
                {
                    filter = " WHERE (DeprtDate BETWEEN '" + YST.ToString() + "' AND '" + YEN.ToString() + "')";
                }
                if (rdbSelected.Checked)
                {
                    if (drpDriver.SelectedValue != null && drpDriver.SelectedText != "")
                    {
                        filter += " AND DriverID=" + Convert.ToInt32(drpDriver.SelectedValue) + "";
                    }
                    else 
                    {
                        return;
                    }
                }
                filter += " Order By DeprtDate";
                ssql = ssql + filter;
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                int RowNumb = 0;
                grdFM.Rows.Count = 1;
                double AKM, BKM, Inc, Fuel, RPKM, Ins, Lse, Bsc;
                double TotAKM = 0, TotBKM = 0, TotIncome = 0, TotFuel=0;
                C1.Win.C1FlexGrid.CellStyle TOT = grdFM.Styles.Add("TOT");
                TOT.BackColor = Color.Aqua;
                while (DT.Rows.Count > RowNumb)
                {
                    grdFM.Rows.Count = DT.Rows.Count + 3;
                    AKM = 0; BKM = 0; Inc = 0; Fuel = 0; RPKM = 0; Ins = 0; Lse = 0; Bsc = 0;
                    grdFM[RowNumb + 1, (int)FM.gTID] = DT.Rows[RowNumb]["ID"].ToString();
                    grdFM[RowNumb + 1, (int)FM.gDID] = DT.Rows[RowNumb]["DriverID"].ToString();
                    grdFM[RowNumb + 1, (int)FM.gDNM] = DT.Rows[RowNumb]["DriverName"].ToString();
                    grdFM[RowNumb + 1, (int)FM.gDTE] = Convert.ToDateTime(DT.Rows[RowNumb]["DeprtDate"].ToString());
                    grdFM[RowNumb + 1, (int)FM.gSTM] = DT.Rows[RowNumb]["StartMeter"].ToString();
                    grdFM[RowNumb + 1, (int)FM.gEDM] = DT.Rows[RowNumb]["EndMeter"].ToString();
                    grdFM[RowNumb + 1, (int)FM.gBNM] = DT.Rows[RowNumb]["Guest"].ToString();
                    if (DT.Rows[RowNumb]["ActualKM"].ToString() != "")
                    {
                        AKM = Convert.ToDouble(DT.Rows[RowNumb]["ActualKM"].ToString());
                    }
                    grdFM[RowNumb + 1, (int)FM.gAKM] = AKM.ToString();
                    TotAKM += AKM;
                    if (DT.Rows[RowNumb]["BillingKM"].ToString() != "")
                    {
                        BKM = Convert.ToDouble(DT.Rows[RowNumb]["BillingKM"].ToString());
                    }
                    grdFM[RowNumb + 1, (int)FM.gBKM] = BKM.ToString(); ;
                    TotBKM += BKM;
                    if (DT.Rows[RowNumb]["RatePerKm"].ToString() != "")
                    {
                         RPKM = Convert.ToDouble(DT.Rows[RowNumb]["RatePerKm"].ToString());
                    }
                    grdFM[RowNumb + 1, (int)FM.gRPKM] = RPKM;
                    if (DT.Rows[RowNumb]["Income"].ToString() != "")
                    {
                        Inc = Convert.ToDouble(DT.Rows[RowNumb]["Income"]);
                    }
                    grdFM[RowNumb + 1, (int)FM.gINC] = Inc.ToString();
                    TotIncome += Inc;
                    if (DT.Rows[RowNumb]["Fuel"].ToString() != "")
                    {
                        Fuel = Convert.ToDouble(DT.Rows[RowNumb]["Fuel"].ToString());
                    }
                    grdFM[RowNumb + 1, (int)FM.gFUEL] = Fuel.ToString();
                    TotFuel += Fuel;
                    if (DT.Rows[RowNumb]["InsuranceAmt"].ToString() != "")
                    {
                        Ins = Convert.ToDouble(DT.Rows[RowNumb]["InsuranceAmt"].ToString());
                    }
                        grdFM[RowNumb + 1, (int)FM.gINS] = Ins.ToString();
                    if (DT.Rows[RowNumb]["Lease"].ToString() != "")
                    {
                        Lse = Convert.ToDouble(DT.Rows[RowNumb]["Lease"].ToString());
                    }
                        grdFM[RowNumb + 1, (int)FM.gLSE] = Lse.ToString();
                    if (DT.Rows[RowNumb]["BasicSalary"].ToString() != "")
                    {
                        Bsc = Convert.ToDouble(DT.Rows[RowNumb]["BasicSalary"].ToString());
                    }
                        grdFM[RowNumb + 1, (int)FM.gBSC] = Bsc.ToString();
                    RowNumb++;
                }
                if (DT.Rows.Count > 0)
                {
                    grdFM[RowNumb + 2, (int)FM.gAKM] = TotAKM.ToString();
                    grdFM[RowNumb + 2, (int)FM.gBKM] = TotBKM.ToString();
                    grdFM[RowNumb + 2, (int)FM.gINC] = TotIncome.ToString();
                    grdFM[RowNumb + 2, (int)FM.gFUEL] = TotFuel.ToString();
                    grdFM.Rows[RowNumb + 2].Style = TOT;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Clear_Grid()
        {
            grdFM.Rows.RemoveRange(1, grdFM.Rows.Count - 1);
        }
        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            Clear_Grid();
            drpDriver.Enabled = false;
            grdFM.Cols[(int)FM.gDID].Width = 100;
            grdFM.Cols[(int)FM.gDNM].Width = 150;
            Fill_Data();
        }
        private void rdbSelected_CheckedChanged(object sender, EventArgs e)
        {
            Clear_Grid();
            drpDriver.Enabled = true;
            grdFM.Cols[(int)FM.gDID].Width = 0;
            grdFM.Cols[(int)FM.gDNM].Width = 0;
            Fill_Data();
        }
        private void dtpIFromDate_ValueChanged(object sender, EventArgs e)
        {
            Clear_Grid();
            Fill_Data();
        }
        private void dtpIToDate_ValueChanged(object sender, EventArgs e)
        {
            Clear_Grid();
            Fill_Data();
        }
        private void drpDriver_Selected_TextChanged(object sender, EventArgs e)
        {
            Clear_Grid();
            Fill_Data();
        }
        private void rdbAnnual_CheckedChanged(object sender, EventArgs e)
        {
            Clear_Grid();
            Fill_Data();
        }
        private void rdbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            Clear_Grid();
            Fill_Data();
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            Print_Report();
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void Print_Report()
        {
            try
            {
                if (chkDefault.Checked && txtFuelRate.Text == "")
                {
                    if (DialogResult.No == MessageBox.Show("Do you want to Preview without Fuel Rate", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        return;
                }
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                DataTable DT;
                DataSet DTG = new DataSet();
                ReportDocument ga = new ReportDocument();
                if (rdbSelected.Checked)
                {
                    if (drpDriver.SelectedValue == null || drpDriver.SelectedText == "")
                    {
                        MessageBox.Show("Please Select a Driver", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    DTG = new DataSets.Transport_DS.ds_trn_FuelMeter_Selected();
                    ga = new Tourist_Management.Reports.rpt_trn_FuelMeter_Selected();
                    DT = new DataTable();
                    DT = Get_FuelMeter_DataTable();
                }
                else
                {
                    DTG = new DataSets.Transport_DS.ds_trn_FuelMeter_Selected();
                    ga = new Tourist_Management.Reports.rpt_trn_FuelMeter_All();
                    DT = new DataTable();
                    DT = Get_FuelMeter_DataTable();
                }
                if (DT.Rows.Count > 0)
                {
                    DT.Columns.Add("CreatedBy", typeof(string));
                    DT.Columns.Add("MDname", typeof(string));
                    DT.Columns.Add("SDate", typeof(DateTime));
                    DT.Columns.Add("EDate", typeof(DateTime));
                    DataTable dt = Classes.clsGlobal.Get_Company_ContactPersons_Details();
                    foreach (DataRow dr in DT.Rows)
                    {
                        dr["CreatedBy"] = dt.Rows[0]["CreatedBy"].ToString();
                        dr["MDname"] = dt.Rows[0]["MDname"].ToString();
                        if (rdbMonthly.Checked)
                        {
                            dr["SDate"] = Convert.ToDateTime(DateFrom);
                            dr["EDate"] = Convert.ToDateTime(DateTo);
                        }
                        else
                        {
                            dr["SDate"] = Convert.ToDateTime(YST);
                            dr["EDate"] = Convert.ToDateTime(YEN);
                        }
                    }
                    sConnection.Print_Via_Datatable(DTG, DT, ga, "");
                }
                else
                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private DataTable Get_FuelMeter_DataTable()
        {
            try
            {
                int RowNumb = 1;
                DataTable table = new DataTable();
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("DriverID", typeof(int));
                table.Columns.Add("DriverName", typeof(string));
                table.Columns.Add("DeprtDate", typeof(string));
                table.Columns.Add("StartMeter", typeof(double));
                table.Columns.Add("EndMeter", typeof(double));
                table.Columns.Add("Guest", typeof(string));
                table.Columns.Add("ActualKM", typeof(double));
                table.Columns.Add("BillingKM", typeof(double));
                table.Columns.Add("RatePerKm", typeof(double));
                table.Columns.Add("Income", typeof(double));
                table.Columns.Add("Fuel", typeof(double));
                table.Columns.Add("FuelRate", typeof(double));
                table.Columns.Add("InsuranceAmt", typeof(double));
                table.Columns.Add("Lease", typeof(double));
                table.Columns.Add("BasicSalary", typeof(double));
                table.Columns.Add("RepType", typeof(string));
                int tourid, driverid;
                double sm, em, akm, bkm, rpkm, inc, fuel, frate=0, ins, lse, bsc;
                string drivername, guest, reptype;
                DateTime depdate;
                while (grdFM.Rows.Count-2 > RowNumb)
                {
                    tourid = Convert.ToInt32(grdFM[RowNumb, (int)FM.gTID].ToString());
                    driverid = Convert.ToInt32(grdFM[RowNumb, (int)FM.gDID].ToString());
                    drivername = grdFM[RowNumb, (int)FM.gDNM].ToString();
                    depdate = Convert.ToDateTime(grdFM[RowNumb, (int)FM.gDTE].ToString());
                    sm = Convert.ToDouble(grdFM[RowNumb, (int)FM.gSTM].ToString());
                    em = Convert.ToDouble(grdFM[RowNumb, (int)FM.gEDM].ToString());
                    guest = grdFM[RowNumb, (int)FM.gBNM].ToString();
                    akm = Convert.ToDouble(grdFM[RowNumb, (int)FM.gAKM].ToString());
                    bkm = Convert.ToDouble(grdFM[RowNumb, (int)FM.gBKM].ToString());
                    rpkm = Convert.ToDouble(grdFM[RowNumb, (int)FM.gRPKM].ToString());
                    inc = Convert.ToDouble(grdFM[RowNumb, (int)FM.gINC].ToString());
                    fuel = Convert.ToDouble(grdFM[RowNumb, (int)FM.gFUEL].ToString());
                    if (chkDefault.Checked)
                    {
                        if (txtFuelRate.Text != "")
                        {
                            frate = Convert.ToDouble(txtFuelRate.Text);
                        }
                    }
                    ins = Convert.ToDouble(grdFM[RowNumb, (int)FM.gINS].ToString());
                    lse = Convert.ToDouble(grdFM[RowNumb, (int)FM.gLSE].ToString());
                    bsc = Convert.ToDouble(grdFM[RowNumb, (int)FM.gBSC].ToString());
                    if (rdbMonthly.Checked)
                    {
                        reptype = DateFrom + " TO " + DateTo;
                    }
                    else
                    {
                        reptype = YST + " - " + YEN + " (ANNUAL)";
                    }
                    table.Rows.Add(tourid, driverid, drivername, depdate, sm, em, guest, akm, bkm, rpkm, inc, fuel, frate, ins, lse, bsc, reptype);
                    RowNumb++;
                }
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void txtFuelRate_TextChanged(object sender, EventArgs e)
        {
            if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtFuelRate.Text.Trim()) != true)
            {
                string s = txtFuelRate.Text;
                if (s.Length > 0)
                {
                    MessageBox.Show("Please Enter Valid Rate", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    s = s.Remove(s.Length - 1);
                    txtFuelRate.Text = s;
                }
                txtFuelRate.SelectionStart = txtFuelRate.Text.Length;
            }
        }
    }
}
