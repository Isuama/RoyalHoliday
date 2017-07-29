using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Settings
{
    public partial class frmLock : Form
    {
        private const string msghd = "Lock System";
        public frmLock()
        {
            InitializeComponent();
            Icon = Properties.Resources.iiLogin;
        }
        private void frmLock_Load(object sender, EventArgs e)
        {
            Classes.clsGlobal.PasswordOK = false;
            Fill_Control();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Classes.clsGlobal.PasswordOK = false;
            this.Close();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
                if (txtPassword.Text.Trim() == "")
                {
                    MessageBox.Show("Password Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                if (txtPassword.Text.Trim() != lblRealPswd.Text.Trim())
                {
                    MessageBox.Show("Password Is Incorrect", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtPassword.Text = "";
                    txtPassword.Select();
                }
                if (txtPassword.Text.Trim() == lblRealPswd.Text.Trim())
                {
                    Classes.clsGlobal.PasswordOK = true;
                    this.Close();
                }
        }
        public void Fill_Control()
        {
            DataTable DT;
            CRPT.CRPT Crpt;
            DataRow rw;
            string sql;
            try
            {
                int UserID = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                Crpt = new CRPT.CRPT();
                sql = "SELECT ID,UserName,Password,Hint,[Desc],EmpID,AccCatList," +
                      "IsNull(IsManager,0)AS IsManager,IsNull(IsDirector,0)AS IsDirector," +
                      "IsCanChange,IsMustChange,IsActive,NoOfDays,UserGroupID" +
                      " FROM mst_UserMaster  Where ID=" + UserID + "";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(sql);
                rw = DT.Rows[0];
                lblRealPswd.Text = Crpt.DECRYPT(rw["Password"].ToString().Trim(), Tourist_Management.Classes.clsGlobal.RevertME());
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
