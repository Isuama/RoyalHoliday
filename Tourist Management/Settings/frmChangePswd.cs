using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRPT;
using System.Data.SqlClient;
namespace Tourist_Management.Settings
{
    public partial class frmChangePswd : Form
    {
        private const string msghd = "Change Current Password";
        int InsMode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        int Syscode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public int Mode
        {
            get
            {
                return InsMode;
            }
            set
            {
                InsMode = value;
            }
        }
        public int SystemCode
        {
            get
            {
                return Syscode;
            }
            set
            {
                Syscode = value;
            }
        }
        public frmChangePswd(){InitializeComponent();}
        private void frmChangePswd_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Fill_Control();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public void Fill_Control()
        {
            DataTable DT;
            CRPT.CRPT Crpt;
            DataRow rw;
            string sql;
            try
            {
                int UserID=Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                Crpt = new CRPT.CRPT();
                sql = "SELECT ID,UserName,Password,Hint,[Desc],EmpID,AccCatList," +
                      "IsNull(IsManager,0)AS IsManager,IsNull(IsDirector,0)AS IsDirector," +
                      "IsCanChange,IsMustChange,IsActive,NoOfDays,UserGroupID" +
                      " FROM mst_UserMaster  Where ID=" + UserID + "";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(sql);
                rw = DT.Rows[0];
                txtUserName.Text = rw["UserName"].ToString();
                lblRealPswd.Text = Crpt.DECRYPT(rw["Password"].ToString().Trim(), Tourist_Management.Classes.clsGlobal.RevertME());
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true){this.Close();}
        }
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Procedure() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
        private Boolean Validate_Data()
        {
                if (txtNewPswd.Text.Trim() == "" || txtConPswd.Text.Trim() == "")
                {
                    MessageBox.Show("Current & Confirm Password Fields Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtNewPswd.Text.Trim() != txtConPswd.Text.Trim())
                {
                    MessageBox.Show(" Confirm Password Is Incorrect", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtConPswd.Text = "";
                    return false;
                }
                if (txtCurPswd.Text.Trim() != lblRealPswd.Text.Trim())
                {
                    MessageBox.Show("Current Password Is Incorrect", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCurPswd.Text = "";
                    return false;
                }
                return true;
        }
        private Boolean Save_Procedure()
        {
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
            try
            {
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objComCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if (Save_User_Password(objCom) == true)
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
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_User_Password(SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
            CRPT.CRPT Crpt;
            try
            {
                Crpt = new CRPT.CRPT();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "sp_mst_ChangePassword";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = txtUserName.Text.Trim();
                sqlCom.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = Crpt.ENCRYPT(txtNewPswd.Text.Trim(), Tourist_Management.Classes.clsGlobal.RevertME());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    Syscode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void chkUnmask_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnmask.Checked)
            {
                txtCurPswd.PasswordChar = '\0';
                txtNewPswd.PasswordChar = '\0';
                txtConPswd.PasswordChar = '\0';
            }
            else
            {
                txtCurPswd.PasswordChar = '*';
                txtNewPswd.PasswordChar = '*';
                txtConPswd.PasswordChar = '*';
            }
        }
        private void txtCurPswd_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtCurPswd.Text.Trim() != lblRealPswd.Text.Trim())
                {
                    picCurPswd.Image = imageList1.Images[1];
                }
                else
                    picCurPswd.Image = imageList1.Images[0];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Check_For_New_Confirm_Passwords()
        {
            try
            {
                if(txtNewPswd.Text.Trim()=="" && txtConPswd.Text.Trim()=="")
                    picConPswd.Image = imageList1.Images[1];
                else if (txtNewPswd.Text.Trim() != txtConPswd.Text.Trim())
                {
                    picConPswd.Image = imageList1.Images[1];
                }
                else
                    picConPswd.Image = imageList1.Images[0];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void txtNewPswd_TextChanged(object sender, EventArgs e)
        {
            Check_For_New_Confirm_Passwords();
        }
        private void txtConPswd_TextChanged(object sender, EventArgs e)
        {
            Check_For_New_Confirm_Passwords();
        }
    }
}
