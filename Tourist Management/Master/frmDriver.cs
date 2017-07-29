using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Tourist_Management.Master
{
    public partial class frmDriver : Form
    {
        private const string msghd = "Driver Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Code,Name as DriverName,CompanyName,IdentityNo,IsNull(IsActive,0)AS IsActive From vwDriverVsEmployee Where Isnull([Status],0)<>7 Order By Code";
        byte[] imageData = null;  //TO KEEP BANK LOGO IMAGE AS A BINARY DATA
        public frmDriver(){InitializeComponent();}
        private void Intializer()
        {
            lblDischarge.Visible = false;
            dtpDateDisc.Visible = false;
            chkActive.Checked = true;
            ckeck_status(false);
            Fill_Control();
            if (Mode == 0)
            {
                chkActive.Checked = true;
            }
            else
            {
                Fill_Details(); 
            }
        }
        private void Fill_Control()
        { 
            drpEmpID.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Code,NameWithIntials FROM mst_EmployeePersonal Where IsNull(IsActive,0)=1 ORDER BY Code"); ; 
        }
        private void Fill_Details()
        {
            try
            {
                DataTable DT;
                string ssql;
                ssql = " SELECT ID,EmpID,EmpPhoto,CompanyName,Code,Name,IdentityNo,LicenseNo,Email,EngageDate,DischargeDate,"+
                       " PermanantAdd,TelHome,TelMobile,ContName,ContTel1,Remarks,Isnull(IsActive,0) AS IsActive " +
                        "FROM vwDriverVsEmployee " +
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
                    if (DT.Rows[0]["CompanyName"].ToString() != null && DT.Rows[0]["CompanyName"].ToString() != "")
                        txtCompany.Text = DT.Rows[0]["CompanyName"].ToString();
                    if (DT.Rows[0]["Code"].ToString() != null && DT.Rows[0]["Code"].ToString() != "")
                        txtCode.Text = DT.Rows[0]["Code"].ToString();
                    if (DT.Rows[0]["Name"].ToString() != null && DT.Rows[0]["Name"].ToString() != "")
                       txtName.Text = DT.Rows[0]["Name"].ToString();
                    if (DT.Rows[0]["IdentityNo"].ToString() != null && DT.Rows[0]["IdentityNo"].ToString() != "")
                       txtNIC.Text = DT.Rows[0]["IdentityNo"].ToString();
                    if (DT.Rows[0]["LicenseNo"].ToString() != null && DT.Rows[0]["LicenseNo"].ToString() != "")
                        txtLicense.Text = DT.Rows[0]["LicenseNo"].ToString();
                    if (DT.Rows[0]["Email"].ToString() != null && DT.Rows[0]["Email"].ToString() != "")
                        txtEmail.Text = DT.Rows[0]["Email"].ToString();
                    if (DT.Rows[0]["EngageDate"].ToString()!="")
                        dtpDateEngage.Value = System.Convert.ToDateTime(DT.Rows[0]["EngageDate"].ToString());
                    if (DT.Rows[0]["DischargeDate"].ToString() != "")
                    dtpDateDisc.Value = System.Convert.ToDateTime(DT.Rows[0]["DischargeDate"].ToString());
                    if (DT.Rows[0]["PermanantAdd"].ToString() != null && DT.Rows[0]["PermanantAdd"].ToString() != "")
                        txtAddress.Text = DT.Rows[0]["PermanantAdd"].ToString();
                    if (DT.Rows[0]["TelHome"].ToString() != null && DT.Rows[0]["TelHome"].ToString() != "")
                        txtTel1.Text = DT.Rows[0]["TelHome"].ToString();
                    if (DT.Rows[0]["TelMobile"].ToString() != null && DT.Rows[0]["TelMobile"].ToString() != "")
                        txtTel2.Text = DT.Rows[0]["TelMobile"].ToString();
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
                        imageData = Photo;
                        MemoryStream ms = new MemoryStream(Photo);
                        pbImage.Image = Image.FromStream(ms, false, false);
                        lblImage.Visible = false;
                    }
                    else
                        lblImage.Visible = true;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Driver Code Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_Driver Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code Is Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
        }
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Data() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Driver_Details";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                if (drpEmpID.SelectedValue.ToString() != null && drpEmpID.SelectedValue.ToString() != "")
                {
                    sqlCom.Parameters.Add("@EmpID", SqlDbType.Int).Value = drpEmpID.SelectedValue.Trim();
                }
                else
                {                    
                    sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                    sqlCom.Parameters.Add("@NIC", SqlDbType.VarChar, 20).Value = txtNIC.Text.Trim();
                    sqlCom.Parameters.Add("@DateEnagage", SqlDbType.DateTime).Value = dtpDateEngage.Value;
                    sqlCom.Parameters.Add("@DateDischarge", SqlDbType.DateTime).Value = dtpDateDisc.Value;
                    sqlCom.Parameters.Add("@Address", SqlDbType.VarChar, 250).Value = txtAddress.Text.Trim();
                    sqlCom.Parameters.Add("@Tel1", SqlDbType.VarChar, 20).Value = txtTel1.Text.Trim();
                    sqlCom.Parameters.Add("@Tel2", SqlDbType.VarChar, 20).Value = txtTel2.Text.Trim();
                    sqlCom.Parameters.Add("@SpouseName", SqlDbType.VarChar, 50).Value = txtSpouseName.Text.Trim();
                    sqlCom.Parameters.Add("@SpouseNo", SqlDbType.VarChar, 50).Value = txtSpouseNo.Text.Trim();
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = txtRemarks.Text.Trim();
                    if (imageData == null)
                        sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = null;
                    else
                        sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = imageData;
                }
                sqlCom.Parameters.Add("@Code", SqlDbType.Char, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Company", SqlDbType.VarChar, 50).Value = txtCompany.Text.Trim();
                sqlCom.Parameters.Add("@LicenseNo", SqlDbType.VarChar, 50).Value = txtLicense.Text.Trim();
                sqlCom.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = txtEmail.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private void btnCancel_Click_1(object sender, EventArgs e){this.Close();}
        private void frmDriver_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void chkIsEmp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsEmp.Checked)
                ckeck_status(true);
            else
                ckeck_status(false);
        }
        private void ckeck_status(Boolean status)
        {
            try
            {
                if (status)
                {
                    drpEmpID.Enabled = true;
                    btnUploadLogo.Enabled = false;
                    btnClearLogo.Enabled = false;
                    txtCompany.Enabled = false;
                    txtName.Enabled = false;
                    txtNIC.Enabled = false;
                    dtpDateEngage.Enabled = false;
                    dtpDateDisc.Enabled = false;
                    txtAddress.Enabled = false;
                    txtTel1.Enabled = false;
                    txtTel2.Enabled = false;
                    txtSpouseName.Enabled = false;
                    txtSpouseNo.Enabled = false;
                }
                else
                {
                    drpEmpID.Enabled = false;
                    btnUploadLogo.Enabled = true;
                    btnClearLogo.Enabled = true;
                    txtCompany.Enabled = true;
                    txtName.Enabled = true;
                    txtNIC.Enabled = true;
                    dtpDateEngage.Enabled = true;
                    dtpDateDisc.Enabled = true;
                    txtAddress.Enabled = true;
                    txtTel1.Enabled = true;
                    txtTel2.Enabled = true;
                    txtSpouseName.Enabled = true;
                    txtSpouseNo.Enabled = true;
                }
                    pbImage.Image = null;
                    lblImage.Visible = true;
                    imageData = null;
                txtCompany.Text = "";
                txtName.Text = "";
                txtNIC.Text = "";
                dtpDateEngage.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                dtpDateDisc.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                txtAddress.Text = "";
                txtTel1.Text = "";
                txtTel2.Text = "";
                txtSpouseName.Text = "";
                txtSpouseNo.Text = "";
            }
            catch (Exception ex){db.MsgERR(ex);}
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
                imageData = null;
                FileInfo fileInfo = new FileInfo(imageLocation);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
            }
        }
        private void btnClearLogo_Click(object sender, EventArgs e)
        {
            pbImage.Image = null;
            lblImage.Visible = true;
            imageData = null;
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
                        txtCompany.Text = DT.Rows[0]["CompanyName"].ToString();
                    }
                SqlQry = " SELECT PS.ID,NameWithIntials,EmpPhoto,IdentityNo," +
                           "PermanantAdd,TelHome,TelMobile,ContName,ContTel1," +
                           "EngageDate,DischargeDate" +
                           " FROM mst_EmployeePersonal PS,mst_EmployeeContact CN,mst_EmployeeServiceDetails SV " +
                           "Where PS.ID=" + drpEmpID.SelectedValue.Trim() + " AND PS.ID=SV.EmpID AND PS.ID=CN.ID";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                txtName.Text = DT.Rows[0]["NameWithIntials"].ToString();
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
                    lblImage.Visible = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    chkActive.Checked = true;
                    lblDischarge.Visible = false;
                    dtpDateDisc.Visible = false;
                }
                else
                {
                    chkActive.Checked = false;
                    lblDischarge.Visible = true;
                    dtpDateDisc.Visible = true;
                }
            }
            if (chkActive.Checked == true)
            {
                lblDischarge.Visible = false;
                dtpDateDisc.Visible = false;
            }
        }
        private void drpEmpID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmEmployee", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
    }
}
