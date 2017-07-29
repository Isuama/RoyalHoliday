using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
namespace Tourist_Management.Settings
{
    public partial class frmFilterDrvGuide : Form
    {
        public static bool outsideLoaded;
        enum DF { gIDN,gNME, gRTE, gNIC, gDRC, gOWN, gLIC, gVEH,gMOD,gMODN,gVEHN,gBRD,gPSG,gTP };
        enum GF { gIDN,gNME, gCOM, gNIC, gLIC, gTEL };
        enum LG { lID, lNME, lSEL }
        String[] driverFilter = new string[] { "DriverCode", "DriverName", "OwnerName", "NIC", "Priority", "LicenseNo", "GuideLicenseNo", "Tel1", "Tel2", "VehicleNo", "InsuranceNo", "Type", "Brand", "Model", "ModelNo", "City", "Province", "District", "MaxPassengers", "ChargersPerKm", "RegYear", "ManuYear", "Address", "SpouseName", "SpouseNo", "Email",  };
        String[] guideFilter = new string[] { "GuideName", "Code", "NIC", "LicenseNo", "Tel1" };
        String[] driverSelector=new string[] { "Name", "Company", "Rate", "NIC", "Driver Code", "Owner Name", "licence No", "Vehicle Type", "Model", "Model No", "Vehicle No", "Brand", "Passenger","Telephone" };
        String[] guideSelector=new string[] { "Name", "Code", "NIC", "Licence", "Telephone" };
        String[] clear = new string[] { };
        String grdSel=""; 
        int RowNumb;
        DataTable[] DTB;
        string qry; //filter query
        String drv_sql="";
        String guid_sql = "";
        String sqql_org_driv = "SELECT DriverID,DriverCode,DriverName,OwnerName,NIC,Priority," +
                "LicenseNo,GuideLicenseNo,Tel1,Tel2,VehicleNo,InsuranceNo,Type,Brand,Model,ModelNo,"+
                "City,Province,District,MaxPassengers,ChargersPerKm,RegYear,ManuYear,Address,SpouseName,SpouseNo,Email " +
                "FROM TouristManagement.dbo.vw_ALL_DRIVER_VEHICLE_DETAILS Where DriverID>0";
        String sqql_org_guide  = "SELECT ID,GuideName,NIC,Code,LicenseNo,Address,Tel1,Tel2 " +
               "FROM TouristManagement.dbo.vw_ALL_GUIDE_DETAILS Where ID>0";
        public frmFilterDrvGuide(){InitializeComponent();}
        public void  clear_all()
        {
            cmdFilter1.Items.Clear();
            cmdFilter2.Items.Clear();
            cmdFilter3.Items.Clear();
            cmdFilter4.Items.Clear();
            cmdFilter5.Items.Clear();
            cmdFilter1.Text = "";
            cmdFilter2.Text = "";
            cmdFilter3.Text = "";
            cmdFilter4.Text = "";
            cmdFilter5.Text = "";
            txtValue1.Text = "";
            txtValue2.Text = "";
            txtValue3.Text = "";
            txtValue4.Text = "";
            txtValue5.Text = "";
            cmdFilter1.Enabled = true;
            cmdFilter2.Enabled = false;
            cmdFilter3.Enabled = false;
            cmdFilter4.Enabled = false;
            cmdFilter5.Enabled = false;
            txtValue1.Enabled = false;
            txtValue2.Enabled = false;
            txtValue3.Enabled = false;
            txtValue4.Enabled = false;
            txtValue5.Enabled = false;
            int row = 1;
            while (row < grdLang.Rows.Count)
            {
                grdLang[row, (int)LG.lSEL] = 0;
                row++;
            }
            rdbAll.Checked = true;
            if (cmbMainFilter.SelectedIndex == 0)
            {
                cmbMainFilter.SelectedIndex = 0;
                Fill_Driver_Conrol();
            }
            else
            {
                cmbMainFilter.SelectedIndex = 1;
                Fill_Guide_Conrol();
            }
            loadList();            
        }
        public void loadList()
        {
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                cmdFilter1.Items.Clear();
                cmdFilter1.Items.AddRange(driverFilter);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                cmdFilter1.Items.Clear();
                cmdFilter1.Items.AddRange(guideFilter);
            }
        }
        public void loadList1()
        {
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                driverFilter = driverFilter.Where(val => val != cmdFilter1.SelectedItem .ToString ()).ToArray();                           
                cmdFilter1.Enabled = false;
                cmdFilter2.Items.Clear();
                cmdFilter2.Items.AddRange(driverFilter);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                guideFilter = guideFilter.Where(val => val != cmdFilter1.SelectedItem.ToString()).ToArray();
                cmdFilter1.Enabled = false;
                cmdFilter2.Items.Clear();
                cmdFilter2.Items.AddRange(guideFilter);
            }
        }
       public void loadList2()
        {
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                driverFilter = driverFilter.Where(val => val != cmdFilter2.SelectedItem.ToString()).ToArray();
                cmdFilter2.Enabled = false;
                cmdFilter3.Items.Clear();
                cmdFilter3.Items.AddRange(driverFilter);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                guideFilter = guideFilter.Where(val => val != cmdFilter2.SelectedItem.ToString()).ToArray();
                cmdFilter2.Enabled = false;
                cmdFilter3.Items.Clear();
                cmdFilter3.Items.AddRange(guideFilter);
            }
        }
        public void loadList3()
        {
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                driverFilter = driverFilter.Where(val => val != cmdFilter3.SelectedItem.ToString()).ToArray();
                cmdFilter3.Enabled = false;
                cmdFilter4.Items.Clear();
                cmdFilter4.Items.AddRange(driverFilter);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                guideFilter = guideFilter.Where(val => val != cmdFilter3.SelectedItem.ToString()).ToArray();
                cmdFilter3.Enabled = false;
                cmdFilter4.Items.Clear();
                cmdFilter4.Items.AddRange(guideFilter);
            }
        }
        public void loadList4()
        {
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                driverFilter = driverFilter.Where(val => val != cmdFilter4.SelectedItem.ToString()).ToArray();
                cmdFilter4.Enabled = false;
                cmdFilter5.Items.Clear();
                cmdFilter5.Items.AddRange(driverFilter);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                guideFilter = guideFilter.Where(val => val != cmdFilter4.SelectedItem.ToString()).ToArray();
                cmdFilter4.Enabled = false;
                cmdFilter5.Items.Clear();
                cmdFilter5.Items.AddRange(guideFilter);
            }
        }
        private void cmbMainFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            clear_all();
            loadList();        
            cmdFilter1.Enabled = true;
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
            Fill_Driver_Conrol();
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                Fill_Guide_Conrol();
            }
        }
        private void cmdFilter1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbMainFilter.SelectedItem .ToString() == "Driver")
            {
                loadList1();
                txtValue1.Enabled = true;
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
              loadList1();
               txtValue1.Enabled = true;
            }
        }
        private void cmdFilter2_SelectedValueChanged(object sender, EventArgs e)
        {
            loadList2();
            txtValue2.Enabled = true;
        }
        private void cmdFilter4_SelectedValueChanged(object sender, EventArgs e)
        {
            loadList4();
            txtValue4.Enabled = true;
        }
        private void cmdFilter3_SelectedValueChanged(object sender, EventArgs e)
        {
            loadList3();
            txtValue3.Enabled = true;
        }
        private void frmFilterDrvGuide_Load(object sender, EventArgs e)
        {
            grdLang.Location = new Point(1300, 800);
            comboInitializer();
            gridInitializer();
            DTB = new DataTable[5];
            Fill_Languages_Grid();
            if (Classes.clsGlobal.filterOutsideLoad)
            {
                cmbMainFilter.Enabled = false;
                if (Classes.clsGlobal.filterDrivers)
                    cmbMainFilter.SelectedIndex=0;
                else
                    cmbMainFilter.SelectedIndex = 1;
            }
        }
        private void gridInitializer()
        {
            try
            {
                grdLang.Cols.Count = 3;
                grdLang.Rows.Count = 500;
                grdLang.Cols[(int)LG.lID].Width = 0;
                grdLang.Cols[(int)LG.lNME].Width = 209;
                grdLang.Cols[(int)LG.lSEL].Width = 73;
                grdLang.Cols[(int)LG.lID].Caption = "Language ID";
                grdLang.Cols[(int)LG.lNME].Caption = "Language";
                grdLang.Cols[(int)LG.lSEL].Caption = "Search For";
                grdLang.Cols[(int)LG.lID ].DataType = Type.GetType("System.Integer");
                grdLang.Cols[(int)LG.lNME].DataType = Type.GetType("System.String");
                grdLang.Cols[(int)LG.lSEL].DataType = Type.GetType("System.Boolean");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Guide_Conrol()
        { 
            DTB[1] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sqql_org_guide );
            grdFilter.DataSource = DTB[1];
        }
        private void Fill_Driver_Conrol()
        {  
            DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sqql_org_driv);
            grdFilter.DataSource = DTB[0];
        }
        private void drivFilter(int txtBoxNum,TextBox txt)
        {
            try
            {
                DataView DV;
                DV = new DataView(DTB[0]);
                if (txt.Text.Trim() == "")
                    return;
                if (txtBoxNum == 1)
                {
                    drv_sql ="";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim() + "%'";
                    drv_sql = qry;
                }
                else if (txtBoxNum == 2)
                {
                    drv_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim() 
                        + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim() + "%'";
                    drv_sql = qry;
                }
                else if (txtBoxNum == 3)
                {
                    drv_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim()
                         + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim() 
                         + "%'AND " + cmdFilter3.SelectedItem.ToString() + " Like '%" + txtValue3.Text.ToString().Trim() + "%'";
                    drv_sql = qry;
                }
                else if (txtBoxNum == 4)
                {
                    drv_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim()
                          + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim()
                          + "%'AND " + cmdFilter3.SelectedItem.ToString() + " Like '%" + txtValue3.Text.ToString().Trim() + "%' AND " 
                          + cmdFilter4.SelectedItem.ToString() + " Like '%" + txtValue4.Text.ToString().Trim() + "%'";
                    drv_sql = qry;                  
                }
                else if (txtBoxNum == 5)
                {
                    drv_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim()
                          + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim()
                          + "%'AND " + cmdFilter3.SelectedItem.ToString() + " Like '%" + txtValue3.Text.ToString().Trim() + "%' AND "
                          + cmdFilter4.SelectedItem.ToString() + " Like '%" + txtValue4.Text.ToString().Trim()
                          + "%' AND " + cmdFilter5.SelectedItem.ToString() + " Like '%" + txtValue5.Text.ToString().Trim() + "%'";
                    drv_sql = qry;
                }
                String drivSql1 = sqql_org_driv + qry;
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(drivSql1);
                grdFilter.DataSource = DTB[0];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void gideFilter(int txtBoxNum)
        {
            try
            {
                DataView DV;
                DV = new DataView(DTB[0]);
                if (txtBoxNum == 1)
                {
                    guid_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim() + "%'";
                    guid_sql = qry;
                }
                else if (txtBoxNum == 2)
                {
                    guid_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim()
                        + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim() + "%'";
                    guid_sql = qry;
                }
                else if (txtBoxNum == 3)
                {
                    guid_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim()
                         + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim()
                         + "%'AND " + cmdFilter3.SelectedItem.ToString() + " Like '%" + txtValue3.Text.ToString().Trim() + "%'";
                    guid_sql = qry;
                }
                else if (txtBoxNum == 4)
                {
                    guid_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim()
                          + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim()
                          + "%'AND " + cmdFilter3.SelectedItem.ToString() + " Like '%" + txtValue3.Text.ToString().Trim() + "%' AND "
                          + cmdFilter4.SelectedItem.ToString() + " Like '%" + txtValue4.Text.ToString().Trim() + "%'";
                    guid_sql = qry;
                }
                else if (txtBoxNum == 5)
                {
                    guid_sql = "";
                    qry = "AND " + cmdFilter1.SelectedItem.ToString() + " Like '%" + txtValue1.Text.ToString().Trim()
                          + "%' AND " + cmdFilter2.SelectedItem.ToString() + " Like '%" + txtValue2.Text.ToString().Trim()
                          + "%'AND " + cmdFilter3.SelectedItem.ToString() + " Like '%" + txtValue3.Text.ToString().Trim() + "%' AND "
                          + cmdFilter4.SelectedItem.ToString() + " Like '%" + txtValue4.Text.ToString().Trim()
                          + "%' AND " + cmdFilter5.SelectedItem.ToString() + " Like '%" + txtValue5.Text.ToString().Trim() + "%'";
                    guid_sql = qry;
                }
                String ww = sqql_org_guide  + qry;
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ww);
                grdFilter.DataSource = DTB[0];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void comboInitializer()
        {
            cmdFilter1.Enabled = false;
            cmdFilter2.Enabled = false;
            cmdFilter3.Enabled = false;
            cmdFilter4.Enabled = false;
            cmdFilter5.Enabled = false;
            txtValue1.Enabled = false;
            txtValue2.Enabled = false;
            txtValue3.Enabled = false;
            txtValue4.Enabled = false;
            txtValue5.Enabled = false;
        }
        private void txtValue1_TextChanged(object sender, EventArgs e)
        {
            cmdFilter2.Enabled = true;
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                drivFilter(1,txtValue1);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                gideFilter(1);
            }
        }
        private void txtValue2_TextChanged(object sender, EventArgs e)
        {
            cmdFilter3.Enabled = true;
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                drivFilter(2, txtValue2);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                gideFilter(2);
            }
        }
        private void txtValue3_TextChanged(object sender, EventArgs e)
        {
            cmdFilter4.Enabled = true;
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                drivFilter(3, txtValue3);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                gideFilter(3);
            }
        }
        private void txtValue4_TextChanged(object sender, EventArgs e)
        {
            cmdFilter5.Enabled = true;
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                drivFilter(4, txtValue4);
            }
            if (cmbMainFilter.SelectedItem.ToString() == "Guide")
            {
                gideFilter(4);
            }
        }
        private void cmdFilter5_SelectedValueChanged(object sender, EventArgs e)
        {
            txtValue5.Enabled = true;
        }
        public void driverInitializer()
        {
            grdFilter.Cols.Count = 14;
            grdFilter.Cols[(int)DF.gNME].Width = 80;        
            grdFilter.Cols[(int)DF.gRTE].Width = 80;
            grdFilter.Cols[(int)DF.gNIC].Width = 80;
            grdFilter.Cols[(int)DF.gDRC].Width = 80;
            grdFilter.Cols[(int)DF.gOWN].Width = 80;
            grdFilter.Cols[(int)DF.gLIC].Width = 80;
            grdFilter.Cols[(int)DF.gVEH].Width = 80;
            grdFilter.Cols[(int)DF.gMOD].Width = 80;
            grdFilter.Cols[(int)DF.gMODN].Width = 80;
            grdFilter.Cols[(int)DF.gVEHN].Width = 80;
            grdFilter.Cols[(int)DF.gBRD].Width = 80;
            grdFilter.Cols[(int)DF.gPSG].Width = 80;
            grdFilter.Cols[(int)DF.gTP].Width = 80;
            grdFilter.Cols[(int)DF.gNME].Caption = "Name";
            grdFilter.Cols[(int)DF.gRTE].Caption = "Rate";
            grdFilter.Cols[(int)DF.gNIC].Caption = "NIC";
            grdFilter.Cols[(int)DF.gDRC].Caption = "Driver Code";
            grdFilter.Cols[(int)DF.gOWN].Caption = "Owner Name";
            grdFilter.Cols[(int)DF.gLIC].Caption = "Licence No";
            grdFilter.Cols[(int)DF.gVEH].Caption = "Vehicle Type";
            grdFilter.Cols[(int)DF.gMOD].Caption = "Model";
            grdFilter.Cols[(int)DF.gMODN].Caption = "Model No";
            grdFilter.Cols[(int)DF.gVEHN].Caption = "Vehicle No";
            grdFilter.Cols[(int)DF.gBRD].Caption = "Brand";
            grdFilter.Cols[(int)DF.gPSG].Caption = "Passenger";
            grdFilter.Cols[(int)DF.gTP].Caption = "Telephone";
            grdFilter.Rows[1].AllowEditing = true;
       }
        public void guideInitializer()
        {
            grdFilter.Cols.Count = 5;
            grdFilter.Cols[(int)GF.gNME].Width = 100;
            grdFilter.Cols[(int)GF.gCOM].Width = 100;
            grdFilter.Cols[(int)GF.gNIC].Width = 100;
            grdFilter.Cols[(int)GF.gLIC].Width = 100;
            grdFilter.Cols[(int)GF.gTEL].Width = 100;
            grdFilter.Cols[(int)GF.gNME].Caption = "Name";
            grdFilter.Cols[(int)GF.gCOM].Caption = "Code";
            grdFilter.Cols[(int)GF.gNIC].Caption = "NIC";
            grdFilter.Cols[(int)GF.gLIC].Caption = "Licence";
            grdFilter.Cols[(int)GF.gTEL].Caption = "Telephone";
            grdFilter.Rows[1].AllowEditing = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clear_all();
           sqql_org_driv = "SELECT DriverCode,DriverName,OwnerName,NIC,Priority," +
                "LicenseNo,GuideLicenseNo,Tel1,Tel2,VehicleNo,InsuranceNo,Type,Brand,Model,ModelNo," +
                "City,Province,District,MaxPassengers,ChargersPerKm,RegYear,ManuYear,Address,SpouseName,SpouseNo,Email " +
                "FROM TouristManagement.dbo.vw_ALL_DRIVER_VEHICLE_DETAILS Where DriverID>0";
           sqql_org_guide = "SELECT GuideName,NIC,Code,LicenseNo,Address,Tel1,Tel2 " +
                   "FROM TouristManagement.dbo.vw_ALL_GUIDE_DETAILS Where ID>0";
        }
        private void txtValue5_TextChanged(object sender, EventArgs e)
        {
            if (cmbMainFilter.SelectedItem.ToString() == "Driver")
            {
                drivFilter(5, txtValue5);
            }
        }
        private void filterButton()
        {
            grdLang .Visible = false;
            grdLang.Location = new Point(1300, 800);
            enableTxts();
            btnFilter.Text = "";
            btnGrid.Text = "";
        }
        private void gridButton()
        {
            disableTxts();
            grdLang.Visible = true;
            grdLang.Location = new Point(6, 12);
            btnGrid.Text = "";
            btnFilter.Text = "";
        }
        private void disableTxts()
        {
            cmdFilter1.Visible = false ;
            cmdFilter2.Visible = false;
            cmdFilter3.Visible = false;
            cmdFilter4.Visible = false;
            cmdFilter5.Visible = false;
            txtValue1.Visible = false;
            txtValue2.Visible = false;
            txtValue3.Visible = false;
            txtValue4.Visible = false;
            txtValue5.Visible = false;
        }
        private void enableTxts()
        {
            cmdFilter1.Visible = true;
            cmdFilter2.Visible = true;
            cmdFilter3.Visible = true;
            cmdFilter4.Visible = true;
            cmdFilter5.Visible = true;
            txtValue1.Visible = true;
            txtValue2.Visible = true;
            txtValue3.Visible = true;
            txtValue4.Visible = true;
            txtValue5.Visible = true;
        }
        private void Fill_Languages_Grid()
        {
            DataTable DT;
            string Ssql;
            try
            {
                Ssql = "SELECT ID,Name FROM mst_Language WHERE IsActive=1 ORDER BY ID";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(Ssql);
                grdLang.Rows.Count = DT.Rows.Count + 1;
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdLang[RowNumb + 1, (int)LG.lID ] = DT.Rows[RowNumb]["ID"].ToString();
                        grdLang[RowNumb + 1, (int)LG.lNME ] = DT.Rows[RowNumb]["Name"].ToString();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnFilter_Click(object sender, EventArgs e)
        {
            gbSpeaks.Visible = false;
            filterButton();
        }
        private void btnGrid_Click(object sender, EventArgs e)
        {
            gbSpeaks.Visible = true;
            gridButton();
        }
        private void txtValue1_TextChanged_1(object sender, EventArgs e)
        {
        }
        private void grdLang_Click(object sender, EventArgs e)
        {
            grdLangFilter();  
        }
        private void grdLangFilter()
        {
            try
            {
                grdSel = "";
                for (int a = 1; a < grdLang.Rows.Count ; a++)
                {
                    if (Convert.ToBoolean(grdLang[a, (int)LG.lSEL]))
                    {
                        grdSel += grdLang[a, (int)LG.lID]+",".Trim(); 
                    }
                }
                if (grdSel.Length == 0)
                    return;
                grdSel = grdSel.Substring(0, grdSel.Length - 1);
                string[] Filter = null;
                Filter = Regex.Split(grdSel, ",");
                string fil;
                if (rdbAll.Checked)
                    fil = "INTERSECT";
                else
                    fil = "UNION";
                string qry = "";
                if (cmbMainFilter.SelectedItem.ToString() == "Driver")
                {
                    foreach (string value in Filter)
                    {
                        qry += "SELECT DriverID FROM dbo.vw_TR_Driver_Languages WHERE LanguageID=" + value + "".Trim();
                        qry += " " + fil + " ";
                    }
                }
                else
                {
                    foreach (string value in Filter)
                    {
                        qry += "SELECT GuideID FROM mst_Guide_Languages WHERE ([Low]=1 or [Avg]=1 or [Fluent]=1 or [Native]=1) and LanguageID=" + value + "".Trim();
                        qry += " " + fil + " ";
                    }
                }
                qry = qry.Trim();
                if (qry.Length > 0)
                {
                    if (rdbAll.Checked)
                        qry = qry.Substring(0, qry.Length - 9);
                    else
                        qry = qry.Substring(0, qry.Length - 5);
                }
                grdSel = qry;
                if (cmbMainFilter.SelectedItem.ToString() == "Driver")
                {
                    DriverFilterByLanguage();
                }
                if (cmbMainFilter.SelectedItem.ToString() == "Guide")
                {
                    GuideFilterByLanguage();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        private void DriverFilterByLanguage()
        {
            try
            {
                String driversID = "";
                String drivSql1 = "";
                if (grdSel != "")
                {
                    drivSql1 = grdSel.Trim();
                    DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(drivSql1);
                    int ct = DTB[0].Rows.Count;
                    driversID = "";
                    for (int c = 0; c < ct; c++)
                    {
                        if (c == 0)
                        {
                            driversID = DTB[0].Rows[c]["DriverID"].ToString().Trim();
                        }
                        else
                        {
                            driversID += "," + DTB[0].Rows[c]["DriverID"].ToString().Trim();
                        }
                    }
                }
                string finalQuery = "";
                if (grdSel != "" && driversID != "")
                {
                    finalQuery = sqql_org_driv + " AND DriverID in ( " + driversID + ")";
                    DTB[1] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(finalQuery);
                }
                else if (driversID == "")
                {
                    DTB[1] = null;
                }
                grdFilter.DataSource = null;
                grdFilter.DataSource = DTB[1];
                grdSel = "";
                driversID = "";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
        }
        private void GuideFilterByLanguage()
        {
            try
            {   String guidesID = "";
                String ssqlT = "";
                if (grdSel != "")
                {
                    ssqlT = grdSel;
                    DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssqlT);
                    int ct = DTB[0].Rows.Count;
                    for (int c = 0; c < ct; c++)
                    {
                        if (c == 0)
                        {
                            guidesID = DTB[0].Rows[c]["GuideID"].ToString().Trim();
                        }
                        else
                        {
                            guidesID += "," + DTB[0].Rows[c]["GuideID"].ToString().Trim();
                        }
                    }
                }
                string finalQuery;
                if (grdSel != "" && guidesID != "")
                {
                    finalQuery = sqql_org_guide + " AND ID in ( " + guidesID + ")";
                    DTB[1] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(finalQuery);
                }
                else
                DTB[1] = null;
                grdFilter.DataSource = DTB[1];
                grdSel = "";
                guidesID = "";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
        }
        private void grdLang_Click(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e){this.Close();}
        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            grdLangFilter();
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            retrieveSelected();
        }
        public void retrieveSelected()
        {
            try
            {
                Classes.clsGlobal.selectedPersonID = Convert.ToInt32(grdFilter[grdFilter.Row, (int)0]);
                if (cmbMainFilter.SelectedIndex==0) //IF IS DRIVER SELECTED
                     Classes.clsGlobal.SelectedPersonName = grdFilter[grdFilter.Row, 2].ToString();
                else
                    Classes.clsGlobal.SelectedPersonName = grdFilter[grdFilter.Row, 1].ToString();
                this.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
