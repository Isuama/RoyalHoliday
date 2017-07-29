using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Accounts
{
    public partial class frmOtherIncome : Form
    {
        private const string msghd = "Other Income";
        public string SqlQry = "SELECT ID,Code [Income Code],IncomeName [Income Name]," +
                       "Isnull(IsActive,0)AS IsActive From mst_OtherIncome" +
                       " Where Isnull([Status],0)<>7 Order By Code";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public frmOtherIncome(){InitializeComponent();}
        private void Intializer()
        {
            if (Mode == 0)
            {
                Generate_Income_Code();
                txtName.Text = "";
                txtName.Select();
                chkActive.Checked = true;
            }
            else
            {
                Fill_Data();
            }
        }
        private void Generate_Income_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_OtherIncome";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            if (DT.Rows[0]["ID"] + "".Trim() == "")
                txtCode.Text = "IN1001";
            else
                txtCode.Text = "IN" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
        }
        private void frmOtherIncome_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Fill_Data()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code," +
                    "IncomeName,Isnull(IsActive,0)as IsActive" +
                    " From mst_OtherIncome Where ID=" + SystemCode + "");
                if (DT.Rows.Count > 0)
                {
                    txtCode.Text = DT.Rows[0]["Code"].ToString();
                    txtName.Text = DT.Rows[0]["IncomeName"].ToString();
                    chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Successfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Fill_Data();
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
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
        private Boolean Validate_Data()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Code cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Income Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_OtherIncome Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select IncomeName From mst_OtherIncome Where IncomeName='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Income Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_OtherIncome";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@IncomeName", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Bit).Value = chkActive.Checked == true ? true : false;
                sqlCom.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
    }
}
