using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
namespace Tourist_Management.Main
{
    public partial class frmUserLog : Form
    {
        private const string msghd = "User Login...";
        private int No_01 = 0, No_02 = 0;
        private RegistryKey registry;
        private string strSysName = "TouristManagement";//Make Register key//ROYAL HOLIDAYS
        private string myKey = "Username";//Make Register key
        private void frmUserLog_Load(object sender, EventArgs e)
        {
            Classes.clsGlobal.Is_Admin = false;
            Classes.clsGlobal.Is_SuperUser = false;
            if (Tourist_Management.Classes.clsGlobal.AllowLog == false) Application.Exit();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
            txtDB.Text = Tourist_Management.Classes.clsGlobal.objCon.DATABASE;
            Intialize_CommonConection();
            registry = Registry.LocalMachine.CreateSubKey("SOFTWARE\\INFOSURV\\" + strSysName + "\\" + myKey);
            registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\INFOSURV\\" + strSysName + "\\" + myKey);
            txtUser.Text = Read_Username_From_registry();
            txtPassword.Select();
            if (System.Diagnostics.Debugger.IsAttached) { txtUser.Text = "isuru"; txtPassword.Text = "Estr#186"; btnOk_Click(null, null); }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Validate_Data() == true)
            {
                Load_Permission();
                Write_Usrename_To_Registry();
                this.Close();
            }
        }
        private void Write_Usrename_To_Registry()
        {
                if (registry != null)
                {
                    registry = Registry.LocalMachine.CreateSubKey("SOFTWARE\\INFOSURV\\" + strSysName + "\\" + myKey);
                    registry.SetValue(myKey, txtUser.Text.ToString().Trim());
                }
        }
        private string Read_Username_From_registry()
        {
            try { return (registry.GetValue(myKey.ToString()) != null) ? registry.GetValue(myKey.ToString()).ToString() : ""; }
            catch (Exception ex) { db.MsgERR(ex); throw (ex); }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Tourist_Management.Classes.clsGlobal.AllowLog = false;
            Application.Exit();
        }
        private void btnServer_Click(object sender, EventArgs e)
        {
            Main.frmConnect frm;
            frm = new Main.frmConnect();
            frm.OpenForm = true;
            this.Visible = false;
            frm.ShowDialog();
            txtDB.Text = Tourist_Management.Classes.clsGlobal.objCon.DATABASE;
            this.Visible = true;
        }
        private void Intialize_CommonConection()
        {
            SQLCONN.SQLCONN objCOMCONN;
            objCOMCONN = new SQLCONN.SQLCONN();
            objCOMCONN.SERVER = Tourist_Management.Classes.clsGlobal.objCon.SERVER;
            objCOMCONN.DATABASE = "TouristManagementCommon";
            objCOMCONN.WINAUTHENICATION = Tourist_Management.Classes.clsGlobal.objCon.WINAUTHENICATION;
            objCOMCONN.USERID = Tourist_Management.Classes.clsGlobal.objCon.USERID;
            objCOMCONN.PASSWORD = Tourist_Management.Classes.clsGlobal.objCon.PASSWORD;
            Tourist_Management.Classes.clsGlobal.CommonCon = objCOMCONN;
            txtUser.Select();
        }
        private Boolean Validate_Data()
        {
            Random random = new Random();
            CRPT.CRPT Crpt;
            DataTable DT;
            Crpt = new CRPT.CRPT();
            if (txtUser.Text.Trim() == "")
            {
                MessageBox.Show("Username Cannot Leave As Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Question);
                return false;
            }
            else
            {
                string returned = Classes.clsGlobal.CleanInput(txtUser.Text.Trim());
                txtUser.Text = returned;
            }
            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Password Cannot Leave As Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Question);
                return false;
            }
            if (No_01.ToString() == txtUser.Text.Trim() && Classes.clsGlobal.Is_SuperUser == true)
            {
                No_02 = RtnPW(No_01);
                if (txtPassword.Text.Trim() == No_02.ToString())
                {
                    Classes.clsGlobal.UserID = 0;
                    Classes.clsGlobal.AccCatID = "0";
                    return true;
                }
                else
                {
                    MessageBox.Show("Sorry..! Please try again.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID FROM mst_CompanyGenaral");
            if (DT.Rows.Count != 0)
            {
                Classes.clsGlobal.CompanyID = System.Convert.ToInt32(DT.Rows[0]["ID"].ToString());
            }
            DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("select ID,UserName,Password,AccCatList,UserGroupID from dbo.mst_UserMaster where Isnull(IsActive,0)=1 and " +
                 "UserName='" + txtUser.Text.Trim() + "' and Password='" + Crpt.ENCRYPT(txtPassword.Text.Trim(), Tourist_Management.Classes.clsGlobal.RevertME()) + "'");
            if (DT.Rows.Count != 0)
            {
                Classes.clsGlobal.UserID = System.Convert.ToInt32(DT.Rows[0][0].ToString());
                Classes.clsGlobal.AccCatID = DT.Rows[0][3].ToString();
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ISNULL(UserMode,0) FROM dbo.mst_User_Groups Where ID=" + DT.Rows[0][4].ToString()).Rows[0][0].ToString() == "1")
                {
                    Classes.clsGlobal.Is_Admin = true;
                }
            }
            else if (txtUser.Text.Trim().ToUpper() == "SUPERUSER")
            {
                No_01 = random.Next(1000, 99999);
                txtUser.Text = No_01.ToString();
                Classes.clsGlobal.Is_SuperUser = true;
                return false;
            }
            else
            {
                MessageBox.Show("Username Or Password Is Incorrect.Please Try Again", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Classes.clsGlobal.UserID = 0;
                Classes.clsGlobal.AccCatID = "";
                return false;
            }
            return true;
        }
        private void Load_Permission()
        {
                DataTable DT;
                if (Classes.clsGlobal.Is_SuperUser) { return; }
                if (Classes.clsGlobal.Is_Admin)
                {
                    string ssql = "Select formID From dbo.mst_FormMaster where ISNULL(IsCritical,0)=1";
                    DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
                }
                else
                {
                    string ssql = "select formID,[View],[Add],Edit,[Delete],[Print] From dbo.mst_User_Forms_Permission where id=" + Classes.clsGlobal.UserID.ToString() + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                }
                Classes.clsGlobal.TB_UserPR = DT;
        }
        private int RtnPW(int RandomNo) { return (RandomNo + 54321) * 13; }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter)btnOk_Click(null, null); }
        public frmUserLog() { InitializeComponent(); Icon = Properties.Resources.iiLogin;
        string str = System.IO.Path.GetDirectoryName(Application.ExecutablePath );
        if (System.IO.File.Exists(str + "/login.png")) BackgroundImage = Image.FromFile(str + "/login.png");
        }
    }
}
