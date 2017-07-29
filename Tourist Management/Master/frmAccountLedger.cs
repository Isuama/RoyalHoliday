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
    public partial class frmAccountLedger : Form
    {
        private const string msghd = "Account Ledger";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Name,GroupName,OpenBal,IsActive From vw_AccountLedger Where Isnull([Status],0)<>7 Order By Name";
        private void Intializer()
        {
            Fill_Control();
            if (Mode == 0)
            {
                txtName.Text = "";
                txtDesc.Text = "";
                txtOpenBal.Text = "";
                chkActive.Checked = true;
            }
            else
            {
                Fill_Details();
            }
        }
         private void Fill_Control()
        {
            try
            {
                DataTable DTB;
                DTB = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_AccountGroup Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpGroup.DataSource = DTB;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Name,GroupID,IsDebit,OpenBal,Description,Isnull(IsActive,0)as IsActive From vw_AccountLedger Where ID=" + SystemCode + "");
                txtName.Text = DT.Rows[0]["Name"].ToString();
                txtOpenBal.Text = DT.Rows[0]["OpenBal"].ToString();
                if (Convert.ToBoolean(DT.Rows[0]["OpenBal"]))
                    txtOpt.Text = "Debit";
                else
                    txtOpt.Text = "Credit";
                if (DT.Rows[0]["GroupID"].ToString() != "")
                {
                    drpGroup.setSelectedValue(DT.Rows[0]["GroupID"].ToString());
                }
                txtDesc.Text = DT.Rows[0]["Description"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (drpGroup.SelectedValue==null)
                {
                    MessageBox.Show("Group cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Account Ledger Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtOpenBal.Text.ToString()) == false)
                {
                    MessageBox.Show("Please Enter Valid Values For Opening Balance", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Name From mst_AccountLedger Where Name='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Account Ledger Name Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_AccountLedger";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@OpenBal", SqlDbType.Decimal).Value = txtOpenBal.Text.Trim();
                sqlCom.Parameters.Add("@GroupID", SqlDbType.Int).Value = Convert.ToInt16(drpGroup.SelectedValue.ToString());                
                sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 250).Value = txtDesc.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteSP(sqlCom) == true)  RtnVal = true;
                return RtnVal;
        }
        public frmAccountLedger(){InitializeComponent();}
        private void frmAccountLedger_Load(object sender, EventArgs e)
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
        private void drpGroup_Selected_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (drpGroup.SelectedValue == null)
                    return;
                if (Convert.ToBoolean(Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT IsDebit from dbo.mst_AccountGroup WHERE ID=" + drpGroup.SelectedValue.Trim() + "").Rows[0]["IsDebit"]))
                {
                    txtOpt.Text = "Debit";
                }
                else
                {
                    txtOpt.Text = "Credit";
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
