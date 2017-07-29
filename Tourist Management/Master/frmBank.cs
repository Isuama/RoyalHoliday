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
    public partial class frmBank : Form
    {
        private const string msghd = "Bank Master";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID, BankCode, BankName, IsActive From mst_BankMaster Where Isnull([Status],0)<>7 Order By BankCode";
        byte[] imageData = null;  //TO KEEP BANK LOGO IMAGE AS A BINARY DATA
        public frmBank(){InitializeComponent();}
        private void Intializer()
        {
            if (Mode == 0)
            {
                txtCode.Text = "";
                txtName.Text = "";
                chkActive.Checked = true;
            }
            else
            {
                Fill_Details();
            }
        }
        private void Fill_Control()
        {
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select BankCode,BankName,BankLogo,LayoutID,Isnull(IsActive,0)as IsActive From mst_BankMaster Where ID=" + SystemCode + "");
                txtCode.Text = DT.Rows[0]["BankCode"].ToString();
                txtName.Text = DT.Rows[0]["BankName"].ToString();
                if (DT.Rows[0]["BankLogo"] != DBNull.Value)
                {
                    byte[] BankLogo = (byte[])DT.Rows[0]["BankLogo"];
                    imageData = BankLogo;
                    MemoryStream ms = new MemoryStream(BankLogo);
                    pbBankImage.Image = Image.FromStream(ms, false, false);
                    lblBankImage.Visible = false;
                }
                else
                    lblBankImage.Visible = true;
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());//== "True" ? true : false;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Code cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select BankCode From mst_BankMaster Where BankCode='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select BankName From mst_BankMaster Where BankName='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_Bank";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@BankName", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                if (imageData == null)
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = null;
                else
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = imageData;
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void frmBank_Load(object sender, EventArgs e)
        {
            Intializer();
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
        private void chkHasLayout_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHasLayout.Checked)
            {
                drpFileLayout.Enabled = true;
            }
            else
            {
                drpFileLayout.Enabled = false;
                drpFileLayout.setSelectedValue("");
            }
        }
        private void drpFileLayout_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmBankFileLayout", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Bank Logo";
            fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (DialogResult.OK == fdLogo.ShowDialog())
            {
                string imageLocation = fdLogo.FileName;
                pbBankImage.ImageLocation = imageLocation;
                lblBankImage.Visible = false;
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
            pbBankImage.Image = null;
            lblBankImage.Visible = true;
            imageData = null;
        }
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    chkActive.Checked = true;
                }
                else
                    return;
            }
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string s = txtName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s,errorProvider1,txtName);
        }
        private void txtName_Leave(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}
