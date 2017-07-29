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
    public partial class frmBasisTypes : Form
    {
        private const string msghd = "Basis Type Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Code,Name,IsActive From mst_BasisTypes Where Isnull([Status],0)<>7 Order By Code";
        private void Intializer()
        {
            if (Mode == 0)
            {
                txtCode.Text = "";
                txtName.Text = "";
                txtDesc.Text = "";
                chkActive.Checked = true;
                Generate_Basis_Code();
            }
            else
            {
                Fill_Details();
            }
        }
        private void Generate_Basis_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_BasisTypes";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQry);
            txtCode.Text = "BST" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code,Name,Description,Isnull(IsActive,0)as IsActive From mst_BasisTypes Where ID=" + SystemCode + "");
                txtCode.Text = DT.Rows[0]["Code"].ToString();
                txtName.Text = DT.Rows[0]["Name"].ToString();
                txtDesc.Text = DT.Rows[0]["Description"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
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
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code From mst_BasisTypes Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Name From mst_BasisTypes Where Name='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
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
                sqlCom.CommandText = "spSave_Basis_Types";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = txtDesc.Text.Trim();
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
        public frmBasisTypes(){InitializeComponent();}
        private void frmBasisTypes_Load(object sender, EventArgs e)
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
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
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
    }
}
