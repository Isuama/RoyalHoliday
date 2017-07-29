using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RatingControls;
using System.IO;
using Touchless.Vision.Camera;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Outlook = Microsoft.Office.Interop.Outlook;
namespace Tourist_Management.Master
{
    public partial class frmTRDetails : Form
    { 
        private const string msghd = "Transportation Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Name AS DriverName,OwnerName,TelMobile,VehicleNo,InsuranceNo,Type,Brand,Model,ModelNo From vw_Total_TR_Details Where Isnull([Status],0)<>7 Order By Name";
        byte[] imageData = null;  //TO KEEP VEHICLE PHOTOS AS A BINARY DATA
        byte[] imageData1 = null;  //TO KEEP DRIVER IMAGE AS A BINARY DATA
        int RowNumb;
        Boolean bLoad = false;
        enum PH { gNUM, gDES, gBRW, gIMG };
        enum LG { gLID, gLNM, gLOW, gAVG, gFLN }
        enum AC { gAID, gANM, gEXS };
        enum RG { gCNT, gIDN, gTID, gGNM, gAAM, gSAM };
        private CameraFrameSource _frameSource;
        private static Bitmap _latestFrame;
        public frmTRDetails(){InitializeComponent();}
        private void frmTRDetails_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            dtpRegYear.Format = DateTimePickerFormat.Custom;
            dtpRegYear.CustomFormat = "yyyy";
            dtpManuYear.Format = DateTimePickerFormat.Custom;
            dtpManuYear.CustomFormat = "yyyy";
            lblDischarge.Visible = false;
            dtpDateDisc.Visible = false;
            chkActive.Checked = false;
            Fill_Control();
            Grd_Initializer();
            Fill_Languages_Grid();
            Fill_AccessCategory_Grid();
            if (Mode == 0)
            {
                Generate_Driver_Code();
                chkActive.Checked = true;
            }
            else
            {
                Fill_Details();
            }
        }
        private void Generate_Driver_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_DriverDtls";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            txtCode.Text= "DRV" + (Convert.ToInt32(DT.Rows[0]["ID"])+1).ToString();
        }
        private void Fill_Control()
        {
            DataTable[] DTB;
            DTB = new DataTable[6];
            DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_VehicleTypes Where IsNull(IsActive,0)=1 ORDER BY Code");
            drpTypeID.DataSource = DTB[0];
            DTB[1] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,City FROM mst_City Where IsNull(IsActive,0)=1 ORDER BY ID");
            drpCity.DataSource = DTB[1];
            DTB[2] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,NameWithIntials FROM mst_EmployeePersonal Where IsNull(IsActive,0)=1 ORDER BY Code");
            drpEmpID.DataSource = DTB[2]; 
            DTB[3] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_VehicleOwners Where IsNull(IsActive,0)=1 ORDER BY Code");
            drpOwnerID.DataSource = DTB[3]; 
            DTB[4] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Country FROM mst_Country Where IsNull(IsActive,0)=1 ORDER BY ID");
            drpCountry.DataSource = DTB[4];
            DTB[5] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Brand FROM mst_VehicleBrands Where IsNull(IsActive,0)=1 ORDER BY ID");
            drpBrand.DataSource = DTB[5];
        }
        private void Grd_Initializer()
        {
            try
            {
                #region GRID
                grdPhoto.Cols.Count = 4;
                grdLang.Cols.Count = 5;
                grdACC.Cols.Count = 3;
                grdPhoto.Rows.Count = 500;
                grdLang.Rows.Count = 500;
                grdACC.Rows.Count = 500;
                grdLang.Cols[(int)LG.gLID].Width = 0;
                grdLang.Cols[(int)LG.gLNM].Width = 311;
                grdLang.Cols[(int)LG.gLOW].Width = 90;
                grdLang.Cols[(int)LG.gAVG].Width = 90;
                grdLang.Cols[(int)LG.gFLN].Width = 90;
                grdLang.Cols[(int)LG.gLID].Caption = "Language ID";
                grdLang.Cols[(int)LG.gLNM].Caption = "Language";
                grdLang.Cols[(int)LG.gLOW].Caption = "Low";
                grdLang.Cols[(int)LG.gAVG].Caption = "Average";
                grdLang.Cols[(int)LG.gFLN].Caption = "Fluent";
                grdACC.Cols[(int)AC.gAID].Width = 0;
                grdACC.Cols[(int)AC.gANM].Width = 501;
                grdACC.Cols[(int)AC.gEXS].Width = 100;
                grdACC.Cols[(int)AC.gAID].Caption = "Access Category ID";
                grdACC.Cols[(int)AC.gANM].Caption = "Access Category Name";
                grdACC.Cols[(int)AC.gEXS].Caption = "Existence";
                grdPhoto.Cols[(int)PH.gNUM].Width = 131;
                grdPhoto.Cols[(int)PH.gDES].Width = 380;
                grdPhoto.Cols[(int)PH.gBRW].Width = 70;
                grdPhoto.Cols[(int)PH.gIMG].Width = 0;
                grdPhoto.Cols[(int)PH.gNUM].Caption = "Serial No";
                grdPhoto.Cols[(int)PH.gDES].Caption = "Description";
                grdPhoto.Cols[(int)PH.gBRW].Caption = "Browse";
                grdPhoto.Cols[(int)PH.gIMG].Caption = "Image/Binary Data";
                grdPhoto.Cols[(int)PH.gBRW].ComboList = "...";
                grdPhoto.Rows[1].AllowEditing = true;
                grdLang.Cols[(int)LG.gLOW].DataType = Type.GetType("System.Boolean");
                grdLang.Cols[(int)LG.gAVG].DataType = Type.GetType("System.Boolean");
                grdLang.Cols[(int)LG.gFLN].DataType = Type.GetType("System.Boolean");
                grdACC.Cols[(int)AC.gEXS].DataType = Type.GetType("System.Boolean");
#endregion
                #region REFERENCE
                grdRef.Cols.Count = 6;
                grdRef.Rows.Count = 500;
                grdRef.Cols[(int)RG.gIDN].Width = 0;
                grdRef.Cols[(int)RG.gCNT].Width = 50;
                grdRef.Cols[(int)RG.gTID].Width = 94;
                grdRef.Cols[(int)RG.gGNM].Width = 244;
                grdRef.Cols[(int)RG.gSAM].Width = 100;
                grdRef.Cols[(int)RG.gAAM].Width = 113;
                grdRef.Cols[(int)RG.gIDN].Caption = "ID";
                grdRef.Cols[(int)RG.gCNT].Caption = "#";
                grdRef.Cols[(int)RG.gTID].Caption = "Tour ID";
                grdRef.Cols[(int)RG.gGNM].Caption = "Guest";
                grdRef.Cols[(int)RG.gAAM].Caption = "Advance";
                grdRef.Cols[(int)RG.gSAM].Caption = "Settlement";
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
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
                        grdLang[RowNumb + 1, (int)LG.gLID] = DT.Rows[RowNumb]["ID"].ToString();
                        grdLang[RowNumb + 1, (int)LG.gLNM] = DT.Rows[RowNumb]["Name"].ToString();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_AccessCategory_Grid()
        {
            DataTable DT;
            string Ssql;
            try
            {
                Ssql = "SELECT ID,Name FROM mst_AccessCategory WHERE IsActive=1 ORDER BY ID ";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(Ssql);
                grdACC.Rows.Count = DT.Rows.Count + 1;
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdACC[RowNumb + 1, (int)AC.gAID] = DT.Rows[RowNumb]["ID"].ToString();
                        grdACC[RowNumb + 1, (int)AC.gANM] = DT.Rows[RowNumb]["Name"].ToString();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkEngageDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEngageDate.Checked)
                dtpDateEngage.Visible = true;
            else
                dtpDateEngage.Visible = false;
        }
        private void chkRegYear_CheckedChanged(object sender, EventArgs e)
        {
            if(chkRegYear.Checked)
                dtpRegYear.Visible = true;
            else
                dtpRegYear.Visible = false;
        }
        private void chkManuYear_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManuYear.Checked)
                dtpManuYear.Visible = true;
            else
                dtpManuYear.Visible = false;
        }
        private void chkIsEmp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsEmp.Checked)
            {
                gbImage.Enabled = false;
                dtpDateEngage.Enabled = false;
                dtpDateDisc.Enabled = false;
                drpOwnerID.Enabled = false;
                txtOwnerName.ReadOnly = true;
                txtDriverName.ReadOnly = true;
                txtNIC.ReadOnly = true;
                drpEmpID.Enabled = true;
                txtAddress.ReadOnly = true;
                txtTel1.ReadOnly = true;
                txtTel2.ReadOnly = true;
                txtEmail.ReadOnly = true;
                txtSpouseName.ReadOnly = true;
                txtSpouseNo.ReadOnly = true;
                txtRemarks.ReadOnly = true;
            }
            else
            {
                gbImage.Enabled = true;
                dtpDateEngage.Enabled = true;
                dtpDateDisc.Enabled = true;
                drpOwnerID.Enabled = true;
                txtOwnerName.ReadOnly = false;
                txtDriverName.ReadOnly = false;
                txtNIC.ReadOnly = false;
                drpEmpID.Enabled = false;
                txtAddress.ReadOnly = false;
                txtTel1.ReadOnly = false;
                txtTel2.ReadOnly = false;
                txtEmail.ReadOnly = false;
                txtSpouseName.ReadOnly = false;
                txtSpouseNo.ReadOnly = false;
                txtRemarks.ReadOnly = false;
            }
        }
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Driver Photo";
            fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (DialogResult.OK == fdLogo.ShowDialog())
            {
                string imageLocation = fdLogo.FileName;
                pbImage.ImageLocation = imageLocation;
                lblImage.Visible = false;
                imageData1 = null;
                FileInfo fileInfo = new FileInfo(imageLocation);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData1 = br.ReadBytes((int)imageFileLength);
            }
        }
        private void btnClearLogo_Click(object sender, EventArgs e)
        {
            pbImage.Image = null;
            lblImage.Visible = true;
            imageData = null;
        }
        private void grdPhoto_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdPhoto.Cols[(int)PH.gBRW].Index)
            {
                OpenFileDialog fdLogo = new OpenFileDialog();
                fdLogo.Title = "Choose a Photo";
                fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (DialogResult.OK == fdLogo.ShowDialog())
                {
                    string imageLocation = fdLogo.FileName;
                    imageData = null;
                    FileInfo fileInfo = new FileInfo(imageLocation);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);
                    grdPhoto[grdPhoto.Row, (int)PH.gIMG] = imageData;
                    update_tree_view();
                }
                return;
            }
        }
        private void grdPhoto_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)
            {
                return;
            }
            grdPhoto.Rows[1].AllowEditing = true;
            if (grdPhoto.Rows.Count < 3)
            {
                return;
            }
            if (grdPhoto[grdPhoto.Row - 1, 0] == null)
            {
                grdPhoto.Rows[grdPhoto.Row].AllowEditing = false;
            }
            else
            {
                grdPhoto.Rows[grdPhoto.Row].AllowEditing = true;
            }
        }
        private void grdPhoto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdPhoto.Rows.Remove(grdPhoto.Row);
                grdPhoto.Rows[1].AllowEditing = true;
            }
        }
        public void update_tree_view()
        {
            try
            {
                tvPhotoDesc.Nodes.Clear();
                RowNumb = 1;
                string TreeName = "";
                if (txtCode.Text.ToString() != "")
                    TreeName = txtCode.Text.Trim();
                if (RowNumb == 1)
                {
                    pbMultiPhotos.Image = null;
                    byte[] Photo = (byte[])grdPhoto[1, (int)PH.gIMG];
                    imageData = Photo;
                    if (imageData != null)
                    {
                        MemoryStream ms = new MemoryStream(Photo);
                        pbMultiPhotos.Image = Image.FromStream(ms, false, false);
                    }
                }
                TreeNode trNode = new TreeNode(TreeName);
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return;
                }
                while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
                {
                    if (grdPhoto[RowNumb, (int)PH.gDES] != null && grdPhoto[RowNumb, (int)PH.gDES].ToString() != "")
                    {
                        TreeNode trn = new TreeNode(grdPhoto[RowNumb, (int)PH.gDES].ToString());
                        trn.Name = RowNumb.ToString();
                        trNode.Nodes.Add(trn);
                    }
                    RowNumb++;
                }
                tvPhotoDesc.Nodes.Add(trNode);
                trNode.Expand();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void tvPhotoDesc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name.ToString() != "")
            {
                pbMultiPhotos.Image = null;
                int NodeID = Convert.ToInt16(e.Node.Name.ToString());
                byte[] Photo = (byte[])grdPhoto[NodeID, (int)PH.gIMG];
                imageData = Photo;
                MemoryStream ms = new MemoryStream(Photo);
                pbMultiPhotos.Image = Image.FromStream(ms, false, false);
            }
        }
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    chkActive.Checked = true;
                }
                else
                    return;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true){this.Close();}
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Close This Window !!", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (System.IO.Directory.Exists("C:\\Temp\\trDriverDetailsRpt"))
                {
                    System.IO.Directory.Delete("C:\\Temp\\trDriverDetailsRpt", true);
                }
                this.Close();
            }
            else
                return;
        }
        private void drpTypeID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmVehicleType", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            Fill_Control();
            return;
        }
        private void drpEmpID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmEmployee", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpOwnerID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmVehicleOwners", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpEmpID_Selected_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable DT;
                if (drpEmpID.SelectedValue.ToString() == null || drpEmpID.SelectedValue.ToString() == "")
                    return;
                SqlQry = "SELECT CompanyName FROM mst_CompanyGenaral";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                if (DT.Rows[0][0].ToString() != "")
                {
                    txtOwnerName.Text = DT.Rows[0]["CompanyName"].ToString();
                }
                SqlQry = " SELECT PS.ID,NameWithIntials,EmpPhoto,IdentityNo," +
                           "PermanantAdd,TelHome,TelMobile,ContName,ContTel1," +
                           "EngageDate,DischargeDate" +
                           " FROM mst_EmployeePersonal PS,mst_EmployeeContact CN,mst_EmployeeServiceDetails SV " +
                           "Where PS.ID=" + drpEmpID.SelectedValue.Trim() + " AND PS.ID=SV.EmpID AND PS.ID=CN.ID";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                txtDriverName.Text = DT.Rows[0]["NameWithIntials"].ToString();
                txtNIC.Text = DT.Rows[0]["IdentityNo"].ToString();
                dtpDateEngage.Value = System.Convert.ToDateTime(DT.Rows[0]["EngageDate"].ToString());
                dtpDateDisc.Value = System.Convert.ToDateTime(DT.Rows[0]["DischargeDate"].ToString());
                txtAddress.Text = DT.Rows[0]["PermanantAdd"].ToString();
                txtTel1.Text = DT.Rows[0]["TelHome"].ToString();
                txtTel2.Text = DT.Rows[0]["TelMobile"].ToString();
                txtSpouseName.Text = DT.Rows[0]["ContName"].ToString();
                txtSpouseNo.Text = DT.Rows[0]["ContTel1"].ToString();
                if (DT.Rows[0]["EmpPhoto"] != DBNull.Value)
                {
                    byte[] Photo = (byte[])DT.Rows[0]["EmpPhoto"];
                    imageData = Photo;
                    MemoryStream ms = new MemoryStream(Photo);
                    pbImage.Image = Image.FromStream(ms, false, false);
                    lblImage.Visible = false;
                }
                else
                {
                    pbImage.Image = null;
                    lblImage.Visible = true;
                    imageData = null;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void drpCity_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCity", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            Fill_Control();
            return;
        }
        private Boolean Save_Pro()
        {
                if (Validate_Data() == false)
                {
                    return false;
                }
                if (Save_Procedure() == false)
                {
                    return false;
                }
                return true;
        }
        private Boolean Validate_Data()
        {
                if (Validate_Driver_Details() == false)
                    return false;
                if (Validate_Vehicle_Details() == false)
                    return false;
                if (Validate_Photos() == false)
                    return false;
                return true;
        }
        private Boolean Validate_Driver_Details()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Driver Code Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_DriverDtls Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Driver Code Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select DriverName From mst_DriverDtls Where DriverName='" + txtDriverName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Driver Name Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtNIC.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select NIC From mst_DriverDtls Where NIC='" + txtNIC.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                    {
                        MessageBox.Show("NIC Number Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (txtLicense.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select LicenseNo From mst_DriverDtls Where LicenseNo='" + txtLicense.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                    {
                        MessageBox.Show("Driving License Number Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                return true;
        }
        private Boolean Validate_Vehicle_Details()
        {
                if (txtVehNo.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select VehicleNo From mst_TRVehicle Where VehicleNo='" + txtVehNo.Text.Trim() + "' and DriverID <> " + SystemCode + "").Rows.Count > 0)
                    {
                        MessageBox.Show("Vehicle Number Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (txtInsNo.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select VehicleNo From mst_TRVehicle Where VehicleNo='" + txtInsNo.Text.Trim() + "' and DriverID <> " + SystemCode + "").Rows.Count > 0)
                    {
                        MessageBox.Show("Vehicle Insurance Number Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (drpCity.SelectedValue + "".Trim() == "")
                {
                    MessageBox.Show("Please Enter Vehicle Location", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtCharg.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtCharg.Text.ToString()) == false)
                    {
                        MessageBox.Show("Please Enter Valid Values For Chargers Per Km", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (txtInsurance.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtInsurance.Text.Trim()) == false)
                    {
                        MessageBox.Show("Please Enter Valid Amount for Insurance", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (txtLeasePayment.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtLeasePayment.Text.ToString()) == false)
                    {
                        MessageBox.Show("Please Enter Valid Amount for Lease Payment", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                return true;
        }
        private Boolean Validate_Photos()
        {
                RowNumb = 1;
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return true;
                }
                do
                {
                    if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                    {
                        MessageBox.Show("Please Enter Serial No ", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gDES].Index] == null) || (grdPhoto[RowNumb, (int)PH.gDES].ToString() == ""))
                    {
                        MessageBox.Show("Image Description Cannot Be Blank ", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gIMG].Index] == null) || (grdPhoto[RowNumb, (int)PH.gIMG].ToString() == ""))
                    {
                        MessageBox.Show("Image Cannot Be Blank.Please Browse For Image", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                } while ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null));
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
                if (save_Tabs(objCom))
                {
                    objTrn.Commit();
                    MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    objCon.Close();
                    return true;
                }
                else
                {
                    objTrn.Rollback();
                    MessageBox.Show("Error Occured,Rollbacked", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                objCon.Close();
                return false;
        }
        private Boolean save_Tabs(System.Data.SqlClient.SqlCommand objCom)
        {
            try
            {
                if(!Save_Driver_Details(objCom))
                    return false;
                if(!Save_Driver_Languages(objCom))
                    return false;
                if(!Save_Vehicle_Details(objCom))
                    return false;
                if(!Save_Vehicle_AccessCategories(objCom))
                    return false;
                if (!Save_Vehicle_Photos(objCom))
                    return false;
                return true;
            }
            catch (Exception  )
            {
                return false;
            }
        }
        private void Fill_Details()
        {
            try
            {
                DataTable DT;
                string ssql;
                int grdCount;
                #region DRIVER DETAILS
                ssql = " SELECT EmpID,EmpPhoto,Priority,Code,CompanyID,OwnerName,Name,IdentityNo,LicenseNo,GuideLicenseNo,Email,EngageDate,DischargeDate," +
                        "PermanantAdd,TelHome,TelMobile,Tel3,ContName,ContTel1,Remarks,Isnull(IsActive,0) AS IsActive " +
                        "FROM vw_TR_Driver " +
                        "Where ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    Mode = 1;
                    if (DT.Rows[0]["EmpID"].ToString() != null && DT.Rows[0]["EmpID"].ToString() != "")
                    {
                        drpEmpID.setSelectedValue(DT.Rows[0]["EmpID"].ToString());
                        chkIsEmp.Checked = true;
                    }
                    if (DT.Rows[0]["Priority"].ToString() != "")
                    {
                        srcPriority.SelectedStar = Convert.ToInt16(DT.Rows[0]["Priority"].ToString());
                    }
                    if (DT.Rows[0]["Code"].ToString() != null && DT.Rows[0]["Code"].ToString() != "")
                        txtCode.Text = DT.Rows[0]["Code"].ToString();
                    if (DT.Rows[0]["CompanyID"].ToString() != "")
                        drpOwnerID.setSelectedValue(DT.Rows[0]["CompanyID"].ToString());
                    if (DT.Rows[0]["OwnerName"].ToString() != null && DT.Rows[0]["OwnerName"].ToString() != "")
                       txtOwnerName.Text = DT.Rows[0]["OwnerName"].ToString();
                    if (DT.Rows[0]["Name"].ToString() != null && DT.Rows[0]["Name"].ToString() != "")
                        txtDriverName.Text = DT.Rows[0]["Name"].ToString();
                    if (DT.Rows[0]["IdentityNo"].ToString() != null && DT.Rows[0]["IdentityNo"].ToString() != "")
                        txtNIC.Text = DT.Rows[0]["IdentityNo"].ToString();
                    if (DT.Rows[0]["LicenseNo"].ToString() != null && DT.Rows[0]["LicenseNo"].ToString() != "")
                        txtLicense.Text = DT.Rows[0]["LicenseNo"].ToString();
                    if (DT.Rows[0]["GuideLicenseNo"].ToString() != null && DT.Rows[0]["GuideLicenseNo"].ToString() != "")
                        txtGuideLicense.Text = DT.Rows[0]["GuideLicenseNo"].ToString();
                    if (DT.Rows[0]["Email"].ToString() != null && DT.Rows[0]["Email"].ToString() != "")
                        txtEmail.Text = DT.Rows[0]["Email"].ToString();
                    if (DT.Rows[0]["EngageDate"].ToString() != "")
                    {
                        dtpDateEngage.Value = System.Convert.ToDateTime(DT.Rows[0]["EngageDate"].ToString());
                        chkEngageDate.Checked = true;
                    }
                    else
                        chkEngageDate.Checked = false;
                    if (DT.Rows[0]["DischargeDate"].ToString() != "")
                    {
                        dtpDateDisc.Value = System.Convert.ToDateTime(DT.Rows[0]["DischargeDate"].ToString());
                        chkActive.Checked = true;
                    }
                    else
                        chkActive.Checked = false;
                    if (DT.Rows[0]["PermanantAdd"].ToString() != null && DT.Rows[0]["PermanantAdd"].ToString() != "")
                        txtAddress.Text = DT.Rows[0]["PermanantAdd"].ToString();
                    if (DT.Rows[0]["TelHome"].ToString() != null && DT.Rows[0]["TelHome"].ToString() != "")
                        txtTel1.Text = DT.Rows[0]["TelHome"].ToString();
                    if (DT.Rows[0]["TelMobile"].ToString() != null && DT.Rows[0]["TelMobile"].ToString() != "")
                        txtTel2.Text = DT.Rows[0]["TelMobile"].ToString();
                    if (DT.Rows[0]["Tel3"].ToString() != null && DT.Rows[0]["Tel3"].ToString() != "")
                        txtTel3.Text = DT.Rows[0]["Tel3"].ToString();
                    if (DT.Rows[0]["ContName"].ToString() != null && DT.Rows[0]["ContName"].ToString() != "")
                        txtSpouseName.Text = DT.Rows[0]["ContName"].ToString();
                    if (DT.Rows[0]["ContTel1"].ToString() != null && DT.Rows[0]["ContTel1"].ToString() != "")
                        txtSpouseNo.Text = DT.Rows[0]["ContTel1"].ToString();
                    if (DT.Rows[0]["Remarks"].ToString() != null && DT.Rows[0]["Remarks"].ToString() != "")
                        txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                    chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                    if (DT.Rows[0]["EmpPhoto"] != DBNull.Value)
                    {
                        byte[] Photo = (byte[])DT.Rows[0]["EmpPhoto"];
                        imageData1 = Photo;
                        MemoryStream ms = new MemoryStream(Photo);
                        pbImage.Image = Image.FromStream(ms, false, false);
                        lblImage.Visible = false;
                    }
                    else
                        lblImage.Visible = true;
                }
                #endregion
                #region LANGUAGE DETAILS
                ssql = "SELECT LanguageID,Language,Low,Avg,Fluent" +
                       " FROM vw_TR_Driver_Languages WHERE DriverID=" + SystemCode + " ORDER BY LanguageID";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    grdCount = 0;
                    while (RowNumb < grdLang.Rows.Count)
                    {
                        if (Convert.ToInt32(DT.Rows[grdCount]["LanguageID"].ToString()) == Convert.ToInt32(grdLang[RowNumb + 1, (int)LG.gLID]))
                        {
                            grdLang[RowNumb + 1, (int)LG.gLID] = Convert.ToInt32(DT.Rows[grdCount]["LanguageID"].ToString());
                            grdLang[RowNumb + 1, (int)LG.gLNM] = DT.Rows[grdCount]["Language"].ToString();
                            grdLang[RowNumb + 1, (int)LG.gLOW] = Convert.ToBoolean(DT.Rows[grdCount]["Low"].ToString());
                            grdLang[RowNumb + 1, (int)LG.gAVG] = Convert.ToBoolean(DT.Rows[grdCount]["Avg"].ToString());
                            grdLang[RowNumb + 1, (int)LG.gFLN] = Convert.ToBoolean(DT.Rows[grdCount]["Fluent"].ToString());
                            grdCount++;
                            if (grdCount == DT.Rows.Count)
                                break;
                        }
                        RowNumb++;
                    }
                }
                #endregion
                #region VEHICLE DETAILS
                ssql = "Select VehicleNo,InsuranceNo,InsuranceAmt,Lease,TypeID,LocationID,BrandID,CountryID,Model,ModelNo,MaxPassengers,ChargersPerKm," +
                       "RegYear,ManuYear,Remarks From mst_TRVehicle Where DriverID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    txtVehNo.Text = DT.Rows[0]["VehicleNo"].ToString();
                    txtInsNo.Text = DT.Rows[0]["InsuranceNo"].ToString();
                    if (DT.Rows[0]["InsuranceAmt"].ToString() != null)
                    txtInsurance.Text = DT.Rows[0]["InsuranceAmt"].ToString();
                    if (DT.Rows[0]["Lease"].ToString() != null)
                    txtLeasePayment.Text = DT.Rows[0]["Lease"].ToString();
                    if (DT.Rows[0]["TypeID"].ToString() != null)
                        drpTypeID.setSelectedValue(DT.Rows[0]["TypeID"].ToString());
                    if (DT.Rows[0]["LocationID"].ToString() != "")
                        drpCity.setSelectedValue(DT.Rows[0]["LocationID"].ToString());
                    if (DT.Rows[0]["BrandID"].ToString() != "")
                        drpBrand.setSelectedValue(DT.Rows[0]["BrandID"].ToString());
                    if (DT.Rows[0]["CountryID"].ToString() != "")
                        drpCountry.setSelectedValue(DT.Rows[0]["CountryID"].ToString());
                    txtModel.Text = DT.Rows[0]["Model"].ToString();
                    txtModelNo.Text = DT.Rows[0]["ModelNo"].ToString();
                    nudMaxPas.Value = Convert.ToInt32(DT.Rows[0]["MaxPassengers"]);
                    txtCharg.Text = DT.Rows[0]["ChargersPerKm"].ToString();
                    if (DT.Rows[0]["RegYear"].ToString() != "")
                    {
                        chkRegYear.Checked = true;
                        dtpRegYear.Value = Convert.ToDateTime(DT.Rows[0]["RegYear"].ToString());
                    }
                    if (DT.Rows[0]["ManuYear"].ToString() != "")
                    {
                        chkManuYear.Checked = true;
                        dtpManuYear.Value = Convert.ToDateTime(DT.Rows[0]["ManuYear"].ToString());
                    }
                    txtVehiRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                }
                #endregion
                #region VEHICLE ACCESS CATEGORIES
                ssql = "SELECT AccessCatID,AccessCatName,Existence" +
                       " FROM vw_TR_Vehicle_AccessCategories WHERE DriverID=" + SystemCode + " ORDER BY AccessCatID";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    grdCount = 0;
                    while (RowNumb < grdLang.Rows.Count)
                    {
                        string a = DT.Rows[grdCount]["AccessCatID"].ToString();
                        string b = grdACC[RowNumb + 1, (int)AC.gAID].ToString();
                        if (DT.Rows[grdCount]["AccessCatID"].ToString() == grdACC[RowNumb + 1, (int)AC.gAID].ToString())
                        {
                            grdACC[RowNumb + 1, (int)AC.gAID] = DT.Rows[grdCount]["AccessCatID"].ToString();
                            grdACC[RowNumb + 1, (int)AC.gANM] = DT.Rows[grdCount]["AccessCatName"].ToString();
                            grdACC[RowNumb + 1, (int)AC.gEXS] = Convert.ToBoolean(DT.Rows[grdCount]["Existence"].ToString());
                            grdCount++;
                            if (grdCount == DT.Rows.Count)
                                break;
                        }
                        RowNumb++;
                    }
                }
                #endregion
                #region VEHICLE PHOTOS
               ssql = "SELECT SerialNo,Description,Image" +
                       " FROM mst_TRVehiclePhotos WHERE DriverID=" + SystemCode + " ORDER BY SrNo ";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdPhoto[RowNumb + 1, (int)PH.gNUM] = DT.Rows[RowNumb]["SerialNo"].ToString();
                        grdPhoto[RowNumb + 1, (int)PH.gDES] = DT.Rows[RowNumb]["Description"].ToString();
                        byte[] Photo = (byte[])DT.Rows[RowNumb]["Image"];
                        imageData = Photo;
                        grdPhoto[RowNumb + 1, (int)PH.gIMG] = imageData;
                        RowNumb++;
                    }
                }
                update_tree_view();
                #endregion
                #region FILL DRIVER REFERENCES
                ssql = "SELECT TransID,TourID,Guest,ISNULL(InvAmt,0)AS InvAmt,ISNULL(RecAmt,0)AS RecAmt" +
                       " FROM vw_acc_PnL_Basics WHERE AgentID=" + SystemCode + " ORDER BY TransID";
                DataTable DTRef = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                decimal setAmt = 0.00m, advAmt = 0.00m, totSettled = 0.00m, totAdvAmt = 0.00m;
                if (DTRef.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DTRef.Rows.Count > RowNumb)
                    {
                        grdRef.Rows[RowNumb + 1].AllowEditing = false;
                        grdRef[RowNumb + 1, (int)RG.gCNT] = RowNumb + 1;
                        grdRef[RowNumb + 1, (int)RG.gIDN] = DTRef.Rows[RowNumb]["TransID"].ToString();
                        grdRef[RowNumb + 1, (int)RG.gTID] = DTRef.Rows[RowNumb]["TourID"].ToString();
                        grdRef[RowNumb + 1, (int)RG.gGNM] = DTRef.Rows[RowNumb]["Guest"].ToString();
                        setAmt = Convert.ToDecimal(DTRef.Rows[RowNumb]["InvAmt"]);
                        advAmt = Convert.ToDecimal(DTRef.Rows[RowNumb]["RecAmt"]);
                        grdRef[RowNumb + 1, (int)RG.gSAM] = setAmt;
                        grdRef[RowNumb + 1, (int)RG.gSAM] = advAmt;
                        totSettled += setAmt;
                        totAdvAmt += advAmt;
                        RowNumb++;
                    }
                    C1.Win.C1FlexGrid.CellStyle COM = grdRef.Styles.Add("TOT");
                    COM.BackColor = Color.Aqua;
                    grdRef.Rows[RowNumb + 1].Style = grdRef.Styles["TOT"];
                    grdRef[RowNumb + 1, (int)RG.gGNM] = "Total";
                    grdRef[RowNumb + 1, (int)RG.gSAM] = totSettled;
                    grdRef[RowNumb + 1, (int)RG.gSAM] = totAdvAmt;
                    grdRef.Rows.Count = RowNumb + 2;
                }
                else
                    grdRef.Rows.Count = 1;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Save_Driver_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
                Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_TR_Driver_Details";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value =Convert.ToInt32(SystemCode);
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if (drpEmpID.SelectedValue.ToString() != null && drpEmpID.SelectedValue.ToString() != "")
                {
                    sqlCom.Parameters.Add("@EmpID", SqlDbType.Int).Value = drpEmpID.SelectedValue.Trim();
                }
                else
                {
                    if(drpOwnerID.SelectedValue.ToString().Trim()!="")
                        sqlCom.Parameters.Add("@CompanyID", SqlDbType.Int).Value = drpOwnerID.SelectedValue.Trim();
                    sqlCom.Parameters.Add("@DriverName", SqlDbType.NVarChar, 100).Value = txtDriverName.Text.Trim();
                    sqlCom.Parameters.Add("@NIC", SqlDbType.NVarChar, 10).Value = txtNIC.Text.Trim();
                    if(chkEngageDate.Checked)
                        sqlCom.Parameters.Add("@DateEnagage", SqlDbType.DateTime).Value = dtpDateEngage.Value;
                    if(chkActive.Checked==false)
                        sqlCom.Parameters.Add("@DateDischarge", SqlDbType.DateTime).Value = dtpDateDisc.Value;
                    sqlCom.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = txtAddress.Text.Trim();
                    sqlCom.Parameters.Add("@Tel1", SqlDbType.NVarChar, 100).Value = txtTel1.Text.Trim();
                    sqlCom.Parameters.Add("@Tel2", SqlDbType.NVarChar, 100).Value = txtTel2.Text.Trim();
                    sqlCom.Parameters.Add("@Tel3", SqlDbType.NVarChar, 100).Value = txtTel3.Text.Trim();
                    sqlCom.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = txtEmail.Text.Trim();
                    sqlCom.Parameters.Add("@SpouseName", SqlDbType.NVarChar, 100).Value = txtSpouseName.Text.Trim();
                    sqlCom.Parameters.Add("@SpouseNo", SqlDbType.NVarChar, 100).Value = txtSpouseNo.Text.Trim();
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 250).Value = txtRemarks.Text.Trim();
                    if (imageData == null)
                        sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = null;
                    else
                        sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = imageData1;
                }
                sqlCom.Parameters.Add("@Priority", SqlDbType.Int).Value = srcPriority.SelectedStar;
                sqlCom.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@OwnerName", SqlDbType.NVarChar, 100).Value = txtOwnerName.Text.Trim();
                sqlCom.Parameters.Add("@DrivingLicenseNo", SqlDbType.NVarChar, 50).Value = txtLicense.Text.Trim();
                sqlCom.Parameters.Add("@GuideLicenseNo", SqlDbType.NVarChar, 50).Value = txtGuideLicense.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
        }
        private Boolean Save_Driver_Languages(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_TR_Driver_Languages";
                while (grdLang[RowNumb, grdLang.Cols[(int)LG.gLID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Decimal).Value = SystemCode;
                    sqlCom.Parameters.Add("@LanguageID", SqlDbType.Int).Value = Convert.ToInt32(grdLang[RowNumb, (int)LG.gLID].ToString());
                    if (Convert.ToBoolean(grdLang[RowNumb, (int)LG.gLOW]) || Convert.ToBoolean(grdLang[RowNumb, (int)LG.gAVG]) || Convert.ToBoolean(grdLang[RowNumb, (int)LG.gFLN]))
                    {
                        sqlCom.Parameters.Add("@Low", SqlDbType.Int).Value = Convert.ToBoolean(grdLang[RowNumb, (int)LG.gLOW]);
                        sqlCom.Parameters.Add("@Avg", SqlDbType.Int).Value = Convert.ToBoolean(grdLang[RowNumb, (int)LG.gAVG]);
                        sqlCom.Parameters.Add("@Fluent", SqlDbType.Int).Value = Convert.ToBoolean(grdLang[RowNumb, (int)LG.gFLN]);
                        sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                        sqlCom.ExecuteNonQuery();
                        if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                        {
                            return false;
                        }
                    }
                    RowNumb++;
                    if (RowNumb >= grdLang.Rows.Count)
                        break;
                }
                return true;
        }
        private Boolean Save_Vehicle_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_TR_Vehicle_Details";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@DriverID", SqlDbType.Decimal).Value = SystemCode;
                sqlCom.Parameters.Add("@VehicleNo", SqlDbType.NVarChar, 50).Value = txtVehNo.Text.Trim();
                sqlCom.Parameters.Add("@InsuranceNo", SqlDbType.NVarChar, 50).Value = txtInsNo.Text.Trim();
                if(txtInsurance.Text.Trim()!="")
                    sqlCom.Parameters.Add("@InsuranceAmt", SqlDbType.Decimal).Value =Convert.ToDecimal(txtInsurance.Text.Trim());
                if (txtLeasePayment.Text.Trim() != "")
                    sqlCom.Parameters.Add("@Lease", SqlDbType.Decimal).Value = Convert.ToDecimal(txtLeasePayment.Text.Trim());
                if (drpTypeID.SelectedValue.ToString() != "" && drpTypeID.SelectedValue.ToString() != null)
                    sqlCom.Parameters.Add("@TypeID", SqlDbType.Int).Value = drpTypeID.SelectedValue.Trim();
                if (drpCity.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@LocationID", SqlDbType.Int).Value = Convert.ToInt16(drpCity.SelectedValue.ToString());
                if (drpBrand.SelectedValue.ToString() != "" && drpBrand.SelectedValue.ToString() != null)
                    sqlCom.Parameters.Add("@BrandID", SqlDbType.Int).Value = drpBrand.SelectedValue.Trim();
                if (drpCountry.SelectedValue.ToString() != "" && drpCountry.SelectedValue.ToString() != null)
                    sqlCom.Parameters.Add("@CountryID", SqlDbType.Int).Value = drpCountry.SelectedValue.Trim();
                sqlCom.Parameters.Add("@Model", SqlDbType.NVarChar, 100).Value = txtModel.Text.Trim();
                sqlCom.Parameters.Add("@ModelNo", SqlDbType.NVarChar, 100).Value = txtModelNo.Text.Trim();
                sqlCom.Parameters.Add("@MaxPassengers", SqlDbType.Int).Value = nudMaxPas.Value;
                if (txtCharg.Text.Trim() != "")
                    sqlCom.Parameters.Add("@ChargersPerKm", SqlDbType.Decimal).Value = Convert.ToDecimal(txtCharg.Text.Trim());
                if (chkRegYear.Checked)
                    sqlCom.Parameters.Add("@RegYear", SqlDbType.DateTime).Value = dtpRegYear.Value;
                if(chkManuYear.Checked)
                    sqlCom.Parameters.Add("@ManuYear", SqlDbType.DateTime).Value = dtpManuYear.Value;
                if (txtVehiRemarks.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 250).Value = txtVehiRemarks.Text.Trim();
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                {
                    return false;
                }
                return true;
        }
        private Boolean Save_Vehicle_AccessCategories(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_TR_Vehicle_AccessCategories";
                while (grdACC[RowNumb, grdACC.Cols[(int)AC.gAID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    if (Convert.ToBoolean(grdACC[RowNumb, (int)AC.gEXS]))
                    {
                        sqlCom.Parameters.Add("@DriverID", SqlDbType.Decimal).Value = SystemCode;
                        sqlCom.Parameters.Add("@AccessCatID", SqlDbType.Int).Value = Convert.ToInt32(grdACC[RowNumb, (int)AC.gAID].ToString());
                        sqlCom.Parameters.Add("@Existence", SqlDbType.Int).Value = Convert.ToBoolean(grdACC[RowNumb, (int)AC.gEXS]);
                        sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                        sqlCom.ExecuteNonQuery();
                        if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                        {
                            return false;
                        }
                    }
                    RowNumb++;
                    if (RowNumb >= grdACC.Rows.Count)
                        break;
                }
                return true;
        }
        private Boolean Save_Vehicle_Photos(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return true;
                }
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_TR_Vehicle_Photos";
                while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Decimal).Value = SystemCode;
                    sqlCom.Parameters.Add("@SerialNo", SqlDbType.VarChar, 20).Value = grdPhoto[RowNumb, (int)PH.gNUM].ToString();
                    sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = grdPhoto[RowNumb, (int)PH.gDES].ToString();
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = grdPhoto[RowNumb, (int)PH.gIMG];
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    RowNumb++;
                }
                return true;
        }
        private void btnTakeaPic_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DesignMode)
                {
                    comboBoxCameras.Items.Clear();
                    foreach (Camera cam in CameraService.AvailableCameras)
                        comboBoxCameras.Items.Add(cam);
                    if (comboBoxCameras.Items.Count > 0)
                        comboBoxCameras.SelectedIndex = 0;
                }
                if (_frameSource != null && _frameSource.Camera == comboBoxCameras.SelectedItem)
                    return;
                thrashOldCamera();
                startCapturing();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void thrashOldCamera()
        {
            if (_frameSource != null)
            {
                _frameSource.NewFrame -= OnImageCaptured;
                _frameSource.Camera.Dispose();
                setFrameSource(null);
                pbImage.Paint -= new PaintEventHandler(drawLatestImage);
            }
        }
        private void startCapturing()
        {
            try
            {
                Camera c = (Camera)comboBoxCameras.SelectedItem;
                setFrameSource(new CameraFrameSource(c));
                _frameSource.Camera.CaptureWidth = 320;
                _frameSource.Camera.CaptureHeight = 240;
                _frameSource.Camera.Fps = 20;
                _frameSource.NewFrame += OnImageCaptured;
                pbImage.Paint += new PaintEventHandler(drawLatestImage);
                _frameSource.StartFrameCapture();
            }
            catch (Exception ex)
            {
                comboBoxCameras.Text = "Select A Camera";
                db.MsgERR(ex);
            }
        }
        public void OnImageCaptured(Touchless.Vision.Contracts.IFrameSource frameSource, Touchless.Vision.Contracts.Frame frame, double fps)
        {
            _latestFrame = frame.Image;
            pbImage.Image = frame.Image;
            pbImage.Invalidate();
        }
        private void setFrameSource(CameraFrameSource cameraFrameSource)
        {
            if (_frameSource == cameraFrameSource)
                return;
            _frameSource = cameraFrameSource;
        }
        private void drawLatestImage(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                e.Graphics.DrawImage(_latestFrame, 0, 0, _latestFrame.Width, _latestFrame.Height);
            }
        }
        private void drpBrand_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmVehicleBrand", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            Fill_Control();
            return;
        }
        private void drpCountry_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCountry", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            Fill_Control();
            return;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Invoice();
        }
        private void Print_Invoice()
        {
            Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
            string sql;
            sql = " SELECT  ID, Priority, GuideLicenseNo, EmpPhoto, Name, CompanyID, OwnerName, IdentityNo, EmpID, Code, LicenseNo, Email, EngageDate, DischargeDate, "+
                      "PermanantAdd, TelHome, TelMobile, Tel3, ContName, ContTel1, Remarks, IsActive, Status, DriverID, AccessCatID, AccessCatName, Existence, LanguageID, Language, "+
                      "Low, Avg, Fluent, ManuYear, RegYear, MaxPassengers, ChargersPerKm, ModelNo, Model, InsuranceNo, VehicleNo, SerialNo, Description, Image, Brand, VehilceType, "+
                      "Country, City,CompanyName FROM vw_TransportDetails   " +
                      "Where ID=" + SystemCode + "";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            if (DT.Rows.Count > 0)
            {
                DataSets.ds_trn_TRDriverDetails DTP = new Tourist_Management.DataSets.ds_trn_TRDriverDetails();
                Tourist_Management.Reports.TRDriverDetails pia = new Tourist_Management.Reports.TRDriverDetails();
                pia.SetDataSource(DTP);
                sConnection.Print_Report(SystemCode.ToString(), sql, DTP, pia, "");
            }
            else
                MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnEmail_Click(object sender, EventArgs e)
        {
            Send_Email();
        }
        private void Send_Email()
        { 
                if (Validate_Email_Options() == false)   return;
                if (!System.IO.Directory.Exists("C:\\Temp\\trDriverDetailsRpt"))
                {
                    MessageBox.Show("Click the preview button before send the mail", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.Display(false);
                string Signature = "";
                Signature = ReadSignature();
                oMsg.HTMLBody = rtbBody.Text + Signature;
                oMsg.CC = txtCC.Text;
                String sDisplayName = "MyAttachment";
                int iPosition;
                if (rtbBody.Text.ToString().Trim() != "")
                    iPosition = (int)oMsg.Body.Length + 1;
                else
                    iPosition = 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                Outlook.Attachment oAttach;
                if (chkExSum.Checked)
                {
                    ReportDocument oReport = new ReportDocument();
                    string path = "C:\\Temp\\trDriverDetailsRpt\\TransportDriverDetails.pdf";
                    string lFileName = path;
                    oAttach = oMsg.Attachments.Add(@path, iAttachType, iPosition, sDisplayName);
                }
                oMsg.Subject = txtSubject.Text;
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                if (txtTo.Text.ToString().Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(txtTo.Text.ToString().Trim());
                    oRecip.Resolve();
                    oRecip = null;
                }
                oRecips = null;
                oMsg = null;
                oApp = null;
        }
        private Boolean Validate_Email_Options()  {  return true;   }
        private string ReadSignature()
        {
                string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
                string signature = string.Empty;
                DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
                if (diInfo.Exists)
                {
                    FileInfo[] fisignature = diInfo.GetFiles("*.htm");
                    if (fisignature.Length > 0)
                    {
                        StreamReader sr = new StreamReader(fisignature[0].FullName, Encoding.Default);
                        signature = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(signature))
                        {
                            string filename = fisignature[0].Name.Replace(fisignature[0].Extension, string.Empty);
                            signature = signature.Replace(filename + "_files/", appDataDir + "/" + filename + "_files/");
                        }
                    }
                }
                return signature;
        }
        private void txtOwnerName_TextChanged(object sender, EventArgs e)
        {
            string s = txtOwnerName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s,errorProvider1,txtOwnerName);
        }
        private void txtOwnerName_Leave(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
        private void txtDriverName_TextChanged(object sender, EventArgs e)
        {
            string s = txtDriverName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s,errorProvider1,txtDriverName);
        }
        private void txtDriverName_Leave(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}
