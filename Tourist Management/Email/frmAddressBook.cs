using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Tourist_Management.Email
{
    public partial class frmAddressBook : Form
    {
        private const string msghd = "Address Book";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Name,Email,Tel1 AS Telephone1,Tel2 AS Telephone2,Fax From mst_AddressBook Where Isnull([Status],0)<>7 Order By Name";
        byte[] imageData = null;  //TO KEEP COMPANY LOGO IMAGE AS A BINARY DATA
        private void Intializer()
        {
            if (Mode == 0)
            {
                txtName.Text = "";
                txtEmail.Text = "";
                txtTel1.Text = "";
                txtTel2.Text = "";
                txtEmail.Text = "";
                txtFax.Text = "";
                txtAddress.Text = "";
                txtRemarks.Text = "";
                pbImage.Image = null;
                lblImage.Visible = true;
                imageData = null;
                chkActive.Checked = true;
            }
            else
            {
                Fill_Details();
            }
        }
        public frmAddressBook(){InitializeComponent();}
        private void frmAddressBook_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select ID,Name,Email,Tel1,Tel2,Mobile,Fax,Address,Image,Remarks,Isnull(IsActive,0)AS IsActive FROM mst_AddressBook Where ID=" + SystemCode + "");
                txtName.Text = DT.Rows[0]["Name"].ToString();
                txtEmail.Text = DT.Rows[0]["Email"].ToString();
                txtTel1.Text = DT.Rows[0]["Tel1"].ToString();
                txtTel2.Text = DT.Rows[0]["Tel2"].ToString();
                txtMobile.Text = DT.Rows[0]["Mobile"].ToString();
                txtFax.Text = DT.Rows[0]["Fax"].ToString();
                txtAddress.Text = DT.Rows[0]["Address"].ToString();
                txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                if (DT.Rows[0]["Image"] != DBNull.Value)
                {
                    byte[] Photo = (byte[])DT.Rows[0]["Image"];
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
        private Boolean Validate_Data()
        {
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_AddressBook";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 400).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@Email", SqlDbType.VarChar, 400).Value = txtEmail.Text.Trim();
                sqlCom.Parameters.Add("@Tel1", SqlDbType.VarChar, 100).Value = txtTel1.Text.Trim();
                sqlCom.Parameters.Add("@Tel2", SqlDbType.VarChar, 100).Value = txtTel2.Text.Trim();
                sqlCom.Parameters.Add("@Mobile", SqlDbType.VarChar, 100).Value = txtMobile.Text.Trim();
                sqlCom.Parameters.Add("@Fax", SqlDbType.VarChar, 100).Value = txtFax.Text.Trim();
                sqlCom.Parameters.Add("@Address", SqlDbType.VarChar, 400).Value = txtAddress.Text.Trim();
                sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 400).Value = txtRemarks.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = 0;
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (imageData == null)
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = null;
                else
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = imageData;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void btnOk_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Record sucessfully Updated", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error occured", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Company Logo";
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
    }
}
