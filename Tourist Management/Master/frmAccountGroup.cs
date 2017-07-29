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
    public partial class frmAccountGroup : Form
    {
        private const string msghd = "Account Groups";
        public int Mode = 0, SystemCode = 0;
        public string SqlQry = "SELECT ID,Name,IsDebit AS Debit,IsActive From mst_AccountGroup Where Isnull([Status],0)<>7 Order By Name";
        private void Fill_Details()
        { 
            try
            {
               DataTable DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Name,Description,Isnull(IsDebit,0)as IsDebit,Isnull(IsActive,0)as IsActive From mst_AccountGroup Where ID=" + SystemCode + "");
                txtName.Text = DT.Rows[0]["Name"].ToString();
                if (Convert.ToBoolean(DT.Rows[0]["IsDebit"])) cmbOpt.Text = "Debit"; else cmbOpt.Text = "Credit";
                txtDesc.Text = DT.Rows[0]["Description"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private Boolean Validate_Data()
        {
            if (txtName.Text.Trim() == "") MessageBox.Show("Account Group Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Name From mst_AccountGroup Where Name='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0) MessageBox.Show("Account Group Name Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else return true;
            return false;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom  = new System.Data.SqlClient.SqlCommand();
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_AccountGroups";
            sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode; 
            sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
            sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = txtDesc.Text.Trim();
            sqlCom.Parameters.Add("@IsDebit", SqlDbType.Int).Value = (cmbOpt.Text.ToString().Trim() == "Debit") ? 1 : 0;
            sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
            sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
            sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
            sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
            sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;  
            return (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteSP(sqlCom) == true);
        }
        private void frmAccountGroup_Load(object sender, EventArgs e)
        {
            cmbOpt.Text = "Debit";
            if (Mode == 0)
            {
                txtName.Text = txtDesc.Text = "";
                chkActive.Checked = true;
            }
            else Fill_Details();
        }
        private Boolean Save_Pro() { return Validate_Data() && Save_Data(); }
        public frmAccountGroup() { InitializeComponent(); }
        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
        private void chkActive_Click(object sender, EventArgs e) { if (chkActive.Checked == false && MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) chkActive.Checked = true; }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
