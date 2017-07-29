using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Threading;
namespace Tourist_Management.Reports
{
    public partial class frmObjectives : Form
    {
        public delegate void FillComparisonData();
        Thread thread; 
        private const string msghd = "Objectives"; 
        enum Est { gEID, gCID, gYID, gMID, gMON, gPAX, gTRN, gERN }
        enum Comp { gEID, gYID, gMID, gMON, gEPAX, gAPAX, gETRN, gATRN, gEERN, gAERN, gAERN1, gAERN2, gPERN, gDPRF, gDPAX }
        public frmObjectives()        {            InitializeComponent();        }
        private void frmObjectives_Load(object sender, EventArgs e)
        {
            Intializer();
            dtpMonthFrom.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpMonthFrom.CustomFormat = "yyyy-MMM";
            dtpMonthFrom.ShowUpDown = true;
            dtpMonthTo.Value = new DateTime(DateTime.Now.Year, 12, 1);
            dtpMonthTo.CustomFormat = "yyyy-MMM";
            dtpMonthTo.ShowUpDown = true;
            chkCompany.Checked = true;
            dtpYear.CustomFormat = "yyyy";
            dtpYear.ShowUpDown = true;
        }
        private void Intializer()
        {
            try
            {
                Fill_Control();
                Grd_Initializer();
                Fill_Default_Values();
            }
            catch (Exception ex)        { db.MsgERR(ex);    }
        }
        private void Fill_Control()
        {
            try
            { 
                drpEHandled.DataSource = drpCHandled.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS [HandledPerson] FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                cmbECompany.DataSource =  cmbCCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID"); 
            }
            catch (Exception ex)   {  db.MsgERR(ex);   }
        }
        private void Grd_Initializer()
        {
            db.GridInit(grdEst, 13, false, Est.gEID, 0, "ID", Est.gCID, 0, "CompanyID", Est.gMID, 0, "Month ID", Est.gYID, 100, "Year", Est.gMON, 150, "Month", Est.gPAX, 150, "PAX #", Est.gTRN, 150, "Turn Over (Rs)", Est.gERN, 150, "Earnings (Rs)");
            db.GridInit(grdComp, 13, Comp.gEID, 0, "ID", Comp.gYID, 45, "Year", Comp.gMID, 0, "Month ID", Comp.gMON, 81, "Month", Comp.gEPAX, 52, "Est Pax", Comp.gAPAX, 52, "Act Pax", Comp.gETRN, 79, "Est T/O", "##.##", Comp.gATRN, 95, "Act T/O", Comp.gEERN, 88, "Est Earn", "##.##", Comp.gAERN, 97, "Act Earn", Comp.gAERN1, 97, "Vat%", Comp.gAERN2, 97, "Without Vat", Comp.gPERN, 84, "% Earn", Comp.gDPRF, 100, "Profit Dif", Comp.gDPAX, 51, "Pax Dif"); 
        }
        private void Fill_Estimated_Data()
        {
            int RowNumb = 0;
            try
            {
                DataTable DT;
                string sql = "SELECT ID,CompID,Year,Month,Pax,TOver,Earnings FROM dbo.mst_Objectives_Estimated " +
                             "WHERE HandledID=" + Convert.ToInt32(drpEHandled.SelectedValue) + " " +
                             "AND CompID=" + Convert.ToInt32(cmbECompany.SelectedValue.ToString()) + " " +
                             "AND Year=" + Convert.ToInt32(dtpYear.Value.Year.ToString()) + " " +
                             " ORDER BY Month";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["ID"].ToString() != "")
                            grdEst[RowNumb + 1, (int)Est.gEID] = Convert.ToInt32(DT.Rows[RowNumb]["ID"].ToString());
                        if (DT.Rows[RowNumb]["CompID"].ToString() != "")
                            grdEst[RowNumb + 1, (int)Est.gCID] = Convert.ToInt32(DT.Rows[RowNumb]["CompID"].ToString());
                        if (DT.Rows[RowNumb]["Year"].ToString() != "")
                            grdEst[RowNumb + 1, (int)Est.gYID] = Convert.ToInt32(DT.Rows[RowNumb]["Year"].ToString());
                        if (DT.Rows[RowNumb]["Month"].ToString() != "")
                            grdEst[RowNumb + 1, (int)Est.gMID] = Convert.ToInt32(DT.Rows[RowNumb]["Month"].ToString());
                        if (DT.Rows[RowNumb]["Pax"].ToString() != "")
                            grdEst[RowNumb + 1, (int)Est.gPAX] = Convert.ToInt32(DT.Rows[RowNumb]["Pax"].ToString());
                        if (DT.Rows[RowNumb]["TOver"].ToString() != "")
                            grdEst[RowNumb + 1, (int)Est.gTRN] = Convert.ToDouble(DT.Rows[RowNumb]["TOver"].ToString());
                        if (DT.Rows[RowNumb]["Earnings"].ToString() != "")
                            grdEst[RowNumb + 1, (int)Est.gERN] = Convert.ToDouble(DT.Rows[RowNumb]["Earnings"].ToString());
                        RowNumb++;
                    }
                }
                else
                {
                    MessageBox.Show("No Records !");
                    RowNumb = 0;
                    while (grdEst.Rows.Count - 1 > RowNumb)
                    {
                        grdEst[RowNumb + 1, (int)Est.gEID] = null;
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        
        private void Fill_Comparison_Data()
        {
            int RowNumb = 0;
            try
            {
                string format = "yyyy-MM-dd";
                string DateFrom = "";
                string DateTo = "";
                DateTime datefrom = dtpMonthFrom.Value;
                DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpMonthTo.Value;
                DateTo = dateto.ToString(format).Substring(0, 10);
                int yearFrom = dtpMonthFrom.Value.Year;
                int yearTo = dtpMonthTo.Value.Year;
                int HandledID = 0;
                drpCHandled.Invoke(new Action(() => HandledID = Convert.ToInt32(drpCHandled.SelectedValue)));
                int CompID = 0;
                cmbCCompany.Invoke(new Action(() => CompID = Convert.ToInt32(cmbCCompany.SelectedValue.ToString())));
                

                DataTable DT;
                string sql = "";
                if (chkCompany.Checked)
                { 
                    sql = "SELECT TOP 36 ID,HandledBy,Year,MonthID,Month,E_Pax,A_Pax,E_Tover,A_TOver," +
                         "VA_Earnings,WV_Earnings,E_Earnings,A_Earnings,Earning_Perc,Dif_Profit,Dif_Pax " +//chathuri
                         "FROM vw_Objectives_Sum " +
                         "WHERE HandledID=" + HandledID + " " +
                         "AND CompID=" + CompID + " " +
                         "AND (FullDate BETWEEN '" + DateFrom + "' AND '" + DateTo + "') " +
                         "Order By Year,MonthID"; 
                }
                else
                {
                    sql = "SELECT TOP 36 HandledBy,Year,MonthID,Month,E_Pax,A_Pax,E_Tover,A_TOver," +//chathuri
                          "VA_Earnings,WV_Earnings,E_Earnings,A_Earnings,Earning_Perc,Dif_Profit,Dif_Pax " +//chathuri
                          "FROM vw_Objectives_Sum_ALL " +
                          "WHERE HandledID=" + HandledID + " " +
                          "AND (FullDate BETWEEN '" + DateFrom + "' AND '" + DateTo + "') " +
                          "Order By Year,MonthID";
                }
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    grdComp.Rows.Count = DT.Rows.Count + 1;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (chkCompany.Checked)
                        {
                            if (DT.Rows[RowNumb]["ID"].ToString() != "") grdComp[RowNumb + 1, (int)Comp.gEID] = Convert.ToInt32(DT.Rows[RowNumb]["ID"].ToString());
                        }
                        else
                        {
                            grdComp[RowNumb + 1, (int)Comp.gEID] = RowNumb + 1;
                        }
                        if (DT.Rows[RowNumb]["Year"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gYID] = Convert.ToInt32(DT.Rows[RowNumb]["Year"].ToString());
                        if (DT.Rows[RowNumb]["MonthID"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gMID] = Convert.ToInt32(DT.Rows[RowNumb]["MonthID"].ToString());
                        if (DT.Rows[RowNumb]["Month"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gMON] = DT.Rows[RowNumb]["Month"].ToString();
                        if (DT.Rows[RowNumb]["E_Pax"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gEPAX] = Convert.ToDouble(DT.Rows[RowNumb]["E_Pax"].ToString());
                        if (DT.Rows[RowNumb]["A_Pax"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gAPAX] = Convert.ToDouble(DT.Rows[RowNumb]["A_Pax"].ToString());
                        if (DT.Rows[RowNumb]["E_Tover"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gETRN] = Convert.ToDouble(DT.Rows[RowNumb]["E_Tover"].ToString());
                        if (DT.Rows[RowNumb]["A_TOver"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gATRN] = Convert.ToDouble(DT.Rows[RowNumb]["A_TOver"].ToString());
                        if (DT.Rows[RowNumb]["E_Earnings"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gEERN] = Convert.ToDouble(DT.Rows[RowNumb]["E_Earnings"].ToString());
                        if (DT.Rows[RowNumb]["A_Earnings"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gAERN] = Convert.ToDouble(DT.Rows[RowNumb]["A_Earnings"].ToString());//chathuri
                        if (DT.Rows[RowNumb]["VA_Earnings"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gAERN1] = Convert.ToDouble(DT.Rows[RowNumb]["VA_Earnings"].ToString());//chathuri
                        if (DT.Rows[RowNumb]["WV_Earnings"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gAERN2] = Convert.ToDouble(DT.Rows[RowNumb]["WV_Earnings"].ToString());//chathuri
                        if (DT.Rows[RowNumb]["Earning_Perc"].ToString() != "")
                        {
                            if (Convert.ToDouble(DT.Rows[RowNumb]["Earning_Perc"].ToString()) < 0)
                                grdComp[RowNumb + 1, (int)Comp.gPERN] = "0.00";
                            else
                                grdComp[RowNumb + 1, (int)Comp.gPERN] = Convert.ToDouble(DT.Rows[RowNumb]["Earning_Perc"].ToString());
                        }
                        if (DT.Rows[RowNumb]["Dif_Profit"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gDPRF] = Convert.ToDouble(DT.Rows[RowNumb]["Dif_Profit"].ToString());
                        if (DT.Rows[RowNumb]["Dif_Pax"].ToString() != "")                            grdComp[RowNumb + 1, (int)Comp.gDPAX] = Convert.ToDouble(DT.Rows[RowNumb]["Dif_Pax"].ToString());
                        RowNumb++;
                    }
                }
                else
                {
                    MethodInvoker action = delegate
                    { pbLoad.Visible = false; };
                    pbLoad.BeginInvoke(action);
                    MessageBox.Show("No Records !");
                } 
            }
            catch (Exception ex) {  db.MsgERR(ex); }
        }
        private void Fill_Default_Values()
        {
            int RowNumb = 0;
            while (RowNumb < 12)//chathuri now
            {
                grdEst[RowNumb + 1, (int)Est.gCID] = null;
                grdEst[RowNumb + 1, (int)Est.gMID] = RowNumb + 1;
                grdEst[RowNumb + 1, (int)Est.gYID] = dtpYear.Value.Year;
                grdEst[RowNumb + 1, (int)Est.gMON] = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(RowNumb + 1);
                if (chkDefault.Checked)
                {
                    grdEst[RowNumb + 1, (int)Est.gPAX] = db.Val(txtPax.Text);
                    grdEst[RowNumb + 1, (int)Est.gTRN] = db.Val(txtTOver.Text);
                    grdEst[RowNumb + 1, (int)Est.gERN] = db.Val(txtEarnings.Text);
                }
                else
                    grdEst[RowNumb + 1, (int)Est.gPAX] = grdEst[RowNumb + 1, (int)Est.gTRN] = grdEst[RowNumb + 1, (int)Est.gTRN] = grdEst[RowNumb + 1, (int)Est.gERN] = 0;
                grdEst.Rows[RowNumb].AllowEditing = true;
                  RowNumb += 1;
          }
        }
        private void chkDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDefault.Checked)  Fill_Default_Values(); 
            else if (drpEHandled.SelectedValue.ToString() != "")  Fill_Estimated_Data(); 
        }
        private void chkCompany_CheckedChanged(object sender, EventArgs e) {  cmbCCompany.Enabled = chkCompany.Checked;  } 
        private void txtPax_TextChanged(object sender, EventArgs e)
        {
            if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtPax.Text.Trim()) != true)
            {
                string s = txtPax.Text;
                if (s.Length > 0)
                {
                    MessageBox.Show("Please Enter Valid Amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    s = s.Remove(s.Length - 1);
                    txtPax.Text = s;
                }
                txtPax.SelectionStart = txtPax.Text.Length;
                return;
            }
            if (chkDefault.Checked)   Fill_Default_Values(); 
        }
        private void txtTOver_TextChanged(object sender, EventArgs e)
        {
            if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtTOver.Text.Trim()) != true)
            {
                string s = txtTOver.Text;
                if (s.Length > 0)
                {
                    MessageBox.Show("Please Enter Valid Amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    s = s.Remove(s.Length - 1);
                    txtTOver.Text = s;
                }
                txtTOver.SelectionStart = txtTOver.Text.Length;
                return;
            } 
            if (chkDefault.Checked)   Fill_Default_Values(); 
        }
        private void Check_IsNumeric(TextBox tb)
        { }
        private void txtEarnings_TextChanged(object sender, EventArgs e)
        {
            if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtEarnings.Text.Trim()) != true)
            {
                string s = txtEarnings.Text;
                if (s.Length > 0)
                {
                    MessageBox.Show("Please Enter Valid Amount", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    s = s.Remove(s.Length - 1);
                    txtEarnings.Text = s;
                }
                txtEarnings.SelectionStart = txtEarnings.Text.Length;
                return;
            } 
            if (chkDefault.Checked)   Fill_Default_Values(); 
        }
        private void drpEHandled_Selected_TextChanged(object sender, EventArgs e)
        {
            if (drpEHandled.SelectedValue.ToString() != "")
            {
                chkDefault.Checked = false;
                Fill_Default_Values();
                Fill_Estimated_Data();
            }
        }
        private void cmbECompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpEHandled.SelectedValue.ToString() == "") return;
                chkDefault.Checked = false;
                Fill_Default_Values();
                Fill_Estimated_Data(); 
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)  return; 
            if (drpEHandled.SelectedValue.ToString() != "") Save_Pro();   else  MessageBox.Show("Please Select a HandledBy Person");
        }
        private void btnCancel_Click(object sender, EventArgs e)   {   this.Close();    }
        private Boolean Save_Pro()
        { 
                if (Validate_Data() == false)  return false; 
                if (Save_Procedure() == false)   return false;  
                return true;  
        }
        private Boolean Validate_Data()
        {
            int RowNumb = 0;
            while (RowNumb < 12)
            {
                if (grdEst[RowNumb + 1, (int)Est.gPAX] == null || Tourist_Management.Classes.clsGlobal.IsNumeric(grdEst[RowNumb + 1, (int)Est.gPAX].ToString()) != true)
                {
                    MessageBox.Show("Please Enter Valid Values for Pax", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (grdEst[RowNumb + 1, (int)Est.gTRN] == null || Tourist_Management.Classes.clsGlobal.IsNumeric(grdEst[RowNumb + 1, (int)Est.gTRN].ToString()) != true)
                {
                    MessageBox.Show("Please Enter Valid Values for T/Over", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (grdEst[RowNumb + 1, (int)Est.gERN] == null || Tourist_Management.Classes.clsGlobal.IsNumeric(grdEst[RowNumb + 1, (int)Est.gERN].ToString()) != true)
                {
                    MessageBox.Show("Please Enter Valid Values for Earnings", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                RowNumb++;
            }
            return true;
        }
        private Boolean Save_Procedure()
        {
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if (Save_Objectives_Estimated(objCom) == true)
                {
                    objTrn.Commit();
                    MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    objCon.Close();
                    Fill_Estimated_Data();
                    return true;
                }
                else
                {
                    objTrn.Rollback();
                    MessageBox.Show("Data Not Saved Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                objCon.Close();
                return false;
        }
        private Boolean Save_Objectives_Estimated(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Objectives_Estimated";
                RowNumb = 1;
                while (RowNumb < 13)
                {
                    sqlCom.Parameters.Clear();
                    int id;
                    if (grdEst[RowNumb, (int)Est.gEID] != null)
                    {
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(grdEst[RowNumb, (int)Est.gEID]);
                        id = Convert.ToInt32(grdEst[RowNumb, (int)Est.gEID]);
                    }
                    else
                    {
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                        id = 0;
                    }
                    sqlCom.Parameters.Add("@HandledID", SqlDbType.Int).Value = Convert.ToInt32(drpEHandled.SelectedValue);
                    sqlCom.Parameters.Add("@CompanyID", SqlDbType.Int).Value = Convert.ToInt32(cmbECompany.SelectedValue.ToString());
                    sqlCom.Parameters.Add("@Year", SqlDbType.Int).Value = Convert.ToInt32(grdEst[RowNumb, (int)Est.gYID]);
                    sqlCom.Parameters.Add("@Month", SqlDbType.Int).Value = Convert.ToInt32(grdEst[RowNumb, (int)Est.gMID]);
                    sqlCom.Parameters.Add("@Pax", SqlDbType.Int).Value = Convert.ToInt32(grdEst[RowNumb, (int)Est.gPAX]);
                    sqlCom.Parameters.Add("@TOver", SqlDbType.Decimal).Value = Convert.ToDouble(grdEst[RowNumb, (int)Est.gTRN]);
                    sqlCom.Parameters.Add("@Earnings", SqlDbType.Decimal).Value = Convert.ToDouble(grdEst[RowNumb, (int)Est.gERN]);
                    sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput; 
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)    {    RtnVal = false;  }
                    RowNumb++;
                }
                return RtnVal; 
        }
        private void btnPreview_Click(object sender, EventArgs e)  {  Print_Report();   } 
        private void Print_Report()
        {
            if (drpCHandled.SelectedValue.ToString() == "") 
            {
                MessageBox.Show("Please Select a Handled By Person to Compare");
                return;
            }
            try
            {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection(); 
                DataSet DTG = new DataSets.ds_rpt_Objectives();
           ReportDocument    ga = new Tourist_Management.Reports.rpt_Objectives(); 
                DataTable DT = Get_Comparison_DataTable();
                if (DT.Rows.Count > 0)
                {
                    DT.Columns.Add("CreatedBy", typeof(string));
                    DT.Columns.Add("MDname", typeof(string));
                    DataTable dt = Classes.clsGlobal.Get_Company_ContactPersons_Details();
                    foreach (DataRow dr in DT.Rows)
                    {
                        dr["CreatedBy"] = dt.Rows[0]["CreatedBy"].ToString();
                        dr["MDname"] = dt.Rows[0]["MDname"].ToString();
                    }
                    sConnection.Print_Via_Datatable(DTG, DT, ga, "", new System.Data.SqlClient.SqlParameter("comp", chkCompany.Checked)); 
                }
                else
                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private DataTable Get_Comparison_DataTable()
        { 
                int RowNumb = 1; 
                DataTable table = new DataTable();
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("HandledBy", typeof(string));
                table.Columns.Add("DateFrom", typeof(string));
                table.Columns.Add("DateTo", typeof(string));
                table.Columns.Add("Year", typeof(int));
                table.Columns.Add("MonthID", typeof(int));
                table.Columns.Add("Month", typeof(string));
                table.Columns.Add("E_Pax", typeof(double));
                table.Columns.Add("A_Pax", typeof(double));
                table.Columns.Add("E_Tover", typeof(double));
                table.Columns.Add("A_TOver", typeof(double));
                table.Columns.Add("VA_Earnings", typeof(double));//chathuri
                table.Columns.Add("WV_Earnings", typeof(double));//chathuri
                table.Columns.Add("E_Earnings", typeof(double));
                table.Columns.Add("A_Earnings", typeof(double));
                table.Columns.Add("Earning_Perc", typeof(double));
                table.Columns.Add("Dif_Profit", typeof(double));
                table.Columns.Add("Dif_Pax", typeof(double));
                table.Columns.Add("CompID", typeof(int));
                table.Columns.Add("DisplayName", typeof(string));
                table.Columns.Add("Telephone", typeof(string));
                table.Columns.Add("Fax", typeof(string));
                table.Columns.Add("E_Mail", typeof(string));
                table.Columns.Add("Web", typeof(string));
                table.Columns.Add("Physical_Address", typeof(string));
                table.Columns.Add("Company_Logo", typeof(byte[]));
                int id, year, monthid, compID;
                double epax, apax, eto, ato, vat, eern, aern, aern1, pern, prodif, paxdif;
                string handledby, datefrom, dateto, month, displayName, Address, tel, fax, email, web;
                byte[] comLogo;
                while (grdComp.Rows.Count > RowNumb) 
                {
                    if (grdComp[RowNumb, (int)Comp.gEID] != null)     id = Convert.ToInt32(grdComp[RowNumb, (int)Comp.gEID].ToString()); 
                    else  return table; 
                    
                    handledby = drpCHandled.SelectedText.ToString();
                    datefrom = dtpMonthFrom.Text.ToString();
                    dateto = dtpMonthTo.Text.ToString();
                    year = Convert.ToInt32(grdComp[RowNumb, (int)Comp.gYID].ToString());
                    monthid = Convert.ToInt32(grdComp[RowNumb, (int)Comp.gMID].ToString());
                    month = grdComp[RowNumb, (int)Comp.gMON].ToString();
                    epax = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gEPAX].ToString());
                    apax = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gAPAX].ToString());
                    eto = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gETRN].ToString());
                    ato = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gATRN].ToString());
                    eern = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gEERN].ToString());
                    aern1 = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gAERN].ToString());//chathuri
                    vat = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gAERN1].ToString());//chathuri
                    aern = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gAERN2].ToString());//chathuri
                    pern = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gPERN].ToString());
                    prodif = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gDPRF].ToString());
                    paxdif = Convert.ToDouble(grdComp[RowNumb, (int)Comp.gDPAX].ToString());
                    if (RowNumb == 1)
                    {
                        DataTable dt = Classes.clsGlobal.getCompanyDetails(Convert.ToInt32(cmbCCompany.SelectedValue));
                        displayName = dt.Rows[0]["DisplayName"].ToString().Trim();
                        compID = Convert.ToInt32(dt.Rows[0]["ID"]);
                        Address = dt.Rows[0]["Physical_Address"].ToString().Trim();
                        tel = dt.Rows[0]["Telephone"].ToString().Trim();
                        fax = dt.Rows[0]["Fax"].ToString().Trim();
                        email = dt.Rows[0]["E_Mail"].ToString().Trim();
                        web = dt.Rows[0]["Web"].ToString().Trim();
                        comLogo = (byte[])dt.Rows[0]["Company_Logo"]; 
                        table.Rows.Add(id, handledby, datefrom, dateto, year, monthid, month, epax, apax, eto, ato, vat, aern, eern, aern1, pern, prodif, paxdif, compID, displayName, tel, fax, email, web, Address, comLogo);
                    } 
                    else
                    { 
                        table.Rows.Add(id, handledby, datefrom, dateto, year, monthid, month, epax, apax, eto, ato, vat, aern, eern, aern1, pern, prodif, paxdif);
                    }
                    RowNumb++;
                } 
                return table; 
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])
            {
                btnPreview.Visible = true;
                btnSave.Enabled = false;
            }
            else
            {
                btnPreview.Visible = false;
                btnSave.Enabled = true;
            }
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            if (drpCHandled.SelectedValue.ToString() != "")
            {
                if (grdComp.Rows.Count > 0) 
                    grdComp.Rows.RemoveRange(1, grdComp.Rows.Count - 1); 

                thread = new Thread(displayWait); 
                thread.IsBackground = true;
                thread.Start();
                pbLoad.Visible = true;
                tabPage2.BackgroundImage = null;
                grdComp.Visible = false;
                btnShow.Enabled = false; 
            }
            else
            { MessageBox.Show("Please Select a Handled By Person to Compare");    }
        }
        public void displayWait()
        {
            try
            {
                FillComparisonData fcd = Fill_Comparison_Data; 
                fcd.Invoke();
                thread.Abort();
            }
            catch (ThreadAbortException  )
            {
                Invoke(new Action(() =>  
                {
                    pbLoad.Visible = false;
                    tabPage2.BackgroundImage = Properties.Resources.formbak1;
                    tabPage2.BackgroundImageLayout = ImageLayout.Stretch;
                    grdComp.Visible = true;
                    btnShow.Enabled = true;
                }));
            }
            catch (Exception ex)  {   db.MsgERR(ex);  }
        }
        private void dtpYear_ValueChanged(object sender, EventArgs e)
        {
            if (drpEHandled.SelectedValue.ToString() != "")
            {
                chkDefault.Checked = false;
                Fill_Default_Values();
                Fill_Estimated_Data();
            }
        } 
    }
}