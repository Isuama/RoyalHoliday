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
    public partial class frmVehicleAccess : Form
    {
        private const string msghd = "Vahicle Access Categories";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Code,Name AS [Access Category],IsActive From mst_AccessCategory Where Isnull([Status],0)<>7 Order By Code";
        private void Intializer()
        {
            if (Mode == 0)
            {
                txtCode.Text =     txtName.Text =  txtDesc.Text = "";
                chkActive.Checked = true;
            }
            else   Fill_Details(); 
        }
        private void Fill_Details()
        { 
            try
            {
               DataTable DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code,Name,Description,Isnull(IsActive,0)as IsActive From mst_AccessCategory Where ID=" + SystemCode + "");
                txtCode.Text = DT.Rows[0]["Code"].ToString();
                txtName.Text = DT.Rows[0]["Name"].ToString();
                txtDesc.Text = DT.Rows[0]["Description"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
            }
            catch (Exception ex)   {  db.MsgERR(ex);    }
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
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code From mst_AccessCategory Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Name From mst_AccessCategory Where Name='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true; 
        } 
        private Boolean Save_Data()
        { 
            Boolean RtnVal = false; 
                   System.Data.SqlClient.SqlCommand  sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_AccessCategory";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = txtDesc.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteSP(sqlCom) == true)   RtnVal = true; 
                return RtnVal; 
        }
        public frmVehicleAccess()    {  InitializeComponent();    }
        private void frmVehicleAccess_Load(object sender, EventArgs e)  {  Intializer();  }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)   return;
            if (Validate_Data() && Save_Data())
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Close This Window !!", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) this.Close();  else   return;
        }
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) chkActive.Checked = true;  else    return;
            }
        }
    }
}
