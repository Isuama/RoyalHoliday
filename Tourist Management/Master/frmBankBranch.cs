using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Master
{
 public partial class frmBankBranch : Form
    {
        private const string msghd = "Bank Branches";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT BR.ID, BankCode,BankName,BranchCode,BranchName,BR.IsActive FROM mst_BankBranchMaster BR LEFT OUTER JOIN  mst_BankMaster BM ON BR.BankID=BM.ID Where Isnull(BR.Status,0)<>7 AND Isnull(BM.Status,0)<>7 Order By BR.ID";
        public frmBankBranch(){InitializeComponent();}
        private void Intializer()
        {
            if (Mode == 0)
            { 
                fill_control();
                txtCode.Text = "";
                txtName.Text = "";
                txtAddress.Text = "";
                txtTel1.Text = "";
                txtTel2.Text = "";
                txtFax.Text = "";
                chkActive.Checked = true;
                drpBank.Enabled = true;
            }
            else
            {
                drpBank.Enabled = false;
                fill_control();
                Fill_Details();
            }
        }
        private void fill_control()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select ID,BankName From [mst_BankMaster] Where Isnull(IsActive,0)=1 AND Isnull(Status,0)<>7");
                drpBank.DataSource = DT;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select BankID,BranchCode,BranchName,Address,Tel1,Tel2,Fax,Isnull(IsActive,0)AS IsActive From [mst_BankBranchMaster] Where ID=" + SystemCode + "");
                drpBank.setSelectedValue(DT.Rows[0]["BankID"].ToString());
                txtCode.Text = DT.Rows[0]["BranchCode"].ToString();
                txtName.Text = DT.Rows[0]["BranchName"].ToString();
                txtAddress.Text = DT.Rows[0]["Address"].ToString();
                txtTel1.Text = DT.Rows[0]["Tel1"].ToString();
                txtTel2.Text = DT.Rows[0]["Tel2"].ToString();
                txtFax.Text = DT.Rows[0]["Fax"].ToString();
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
                sqlCom.CommandText = "spSave_BankBranch";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = drpBank.SelectedValue;
                sqlCom.Parameters.Add("@BranchCode", SqlDbType.VarChar, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@BranchName", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@Address", SqlDbType.VarChar, 250).Value = txtAddress.Text.Trim();
                sqlCom.Parameters.Add("@Tel1", SqlDbType.VarChar, 50).Value = txtTel1.Text.Trim();
                sqlCom.Parameters.Add("@Tel2", SqlDbType.VarChar, 50).Value = txtTel2.Text.Trim();
                sqlCom.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = txtFax.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;//??
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;//??
                string sql11 = sqlCom.ToString();
                if (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal; 
        }
        private void frmBankBranch_Load_1(object sender, EventArgs e)
        {
            Intializer();
        }
        private void btnOk_Click_1(object sender, EventArgs e)
        {
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
        private void drpBank_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmBank", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            fill_control();
            return;
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
