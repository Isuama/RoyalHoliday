using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using SQLCONN;
using CRPT;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
namespace Tourist_Management.Main
{
    public partial class frmConnect : Form
    {
        private const string msghd = "Connecting the Server";
        public string Server = "";//srver name
        public string DataBase = "";//DB name
        public bool IsWinAuthenication = true;//Authenication Mode
        public string UserID = " ";//DB name
        public string PassWord = " ";//DB name
        private string strSysName = "TouristManagement";//Make Register key
        private string myKey = "Verification";//Make Register key
        public bool OpenForm = false;//Authenication Mode
        private RegistryKey registry;
        private SQLCONN.SQLCONN objCONN;
        public frmConnect(){InitializeComponent();}
        private void frmConnect_Load(object sender, EventArgs e)
        {
            try
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
                RegistryKey registry = Registry.CurrentUser;
                open_reg();
                Make_tree();
                btnOk.Enabled = true;
                if (Read_Register().ToString().Trim() == "")
                {
                    txtserver.Enabled = false;
                    Tourist_Management.Classes.clsGlobal.AllowLog = false;
                    return;
                }
                Assign_Variables(Read_Register());
                Classes.clsConnection.ConnectionString = Read_Register();
                registry.Close();
                if (OpenForm != true){this.Close();}
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Make_tree()
        {
            try
            {
                DataTable dt;
                TreeNode trNode = new TreeNode("AVAILABLE SERVER LIST");
                tv.ImageList = imgLst;
                tv.Nodes.Add(trNode);
                trNode.ImageIndex = 0;
                trNode.SelectedImageIndex = 0;
                dt = Microsoft.SqlServer.Management.Smo.SmoApplication.EnumAvailableSqlServers(false);
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode trn = new TreeNode(dr[0].ToString());
                    trn.ImageIndex = 1;
                    trn.SelectedImageIndex = 1;
                    trNode.Nodes.Add(trn);
                }
                tv.Nodes[0].Expand();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void open_reg()
        {
            try
            {
                registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\INFOSURV\\" + strSysName + "\\" + myKey);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void edit_registry()
        {
            try
            {
                registry = Registry.LocalMachine.CreateSubKey("SOFTWARE\\INFOSURV\\" + strSysName);
                registry = Registry.LocalMachine.CreateSubKey("SOFTWARE\\INFOSURV\\" + strSysName + "\\" + myKey);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private string Read_Register()
        {
            CRPT.CRPT Crpt; 
                string rtnValue = "";
                Crpt = new CRPT.CRPT();
                if (registry != null)
                {
                    if (registry.GetValue(myKey.ToString()) != null)
                    {
                        rtnValue = registry.GetValue(myKey.ToString()).ToString();
                        rtnValue = Crpt.DECRYPT(rtnValue, Tourist_Management.Classes.clsGlobal.RevertME());
                        return Crpt.DECRYPT(rtnValue, Tourist_Management.Classes.clsGlobal.RevertME());
                    }
                    else
                    {
                        edit_registry();
                        return rtnValue;
                    }
                }
                else
                {
                    edit_registry();
                    return rtnValue;
                } 
        }
        private Boolean write_register()
        { 
                bool rtnVal = false;
                if (registry != null)
                {
                    edit_registry();
                    registry.SetValue(myKey, make_regString());
                    rtnVal = true;
                }
                return rtnVal; 
        }
        private string make_regString()
        {
            CRPT.CRPT Crpt; 
                Crpt = new CRPT.CRPT();
                string strString = optWN.Checked == true ? "1" : "0";
                if (cmbDB.Text.ToString() == "")
                {
                    strString = txtserver.Text + " `'`' master `'`' " + strString + " `'`' " + txtuserID.Text.Trim() + " `'`' " + txtpw.Text;
                }
                else
                {
                    strString = txtserver.Text + " `'`' " + cmbDB.Text + " `'`' " + strString + " `'`' " + txtuserID.Text.Trim() + " `'`' " + txtpw.Text;
                }
                strString = Crpt.ENCRYPT(strString, Tourist_Management.Classes.clsGlobal.RevertME());
                return Crpt.ENCRYPT(strString, Tourist_Management.Classes.clsGlobal.RevertME()); 
        }
        private void update_Variable()
        {
            try
            {
                Server = txtserver.Text.ToString().Trim();
                DataBase = cmbDB.Text.ToString().Trim(); 
                IsWinAuthenication = (optWN.Checked == true);
                UserID = txtpw.Text;
                PassWord = txtpw.Text;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Assign_Variables(string str)
        {
            try
            {
                string[] varCon;
                varCon = Regex.Split(str, " `'`' ");
                Server = varCon[0];
                DataBase = varCon[1];
                IsWinAuthenication = varCon[2] == "1" ? true : false;
                UserID = varCon[3];
                PassWord = varCon[4];
                txtserver.Text = Server;
                if (IsWinAuthenication == true)   optWN.Checked = true; 
                add_subNotes();
                objCONN = new SQLCONN.SQLCONN();
                objCONN.SERVER = Server;
                objCONN.DATABASE = DataBase;
                objCONN.WINAUTHENICATION = IsWinAuthenication;
                objCONN.USERID = UserID;
                objCONN.PASSWORD = PassWord;
                Tourist_Management.Classes.clsGlobal.objCon = objCONN;
                Tourist_Management.Classes.clsGlobal.AllowLog = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (chkManual.Checked == true)
                {
                    txtserver.Text = "";
                    chkManual.Checked = false;
                    return;
                }
                if (tv.SelectedNode.Level == 1)
                {
                    txtserver.Text = tv.SelectedNode.Text.ToString();
                }
                else
                {
                    txtserver.Text = "";
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            { 
                Classes.clsConnection clsCon;
                CRPT.CRPT Crpt;
                Crpt = new CRPT.CRPT();
                if (txtserver.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Please select the server", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (optUS.Checked == true)
                {
                    if (txtuserID.Text.ToString().Trim() == "")
                    {
                        MessageBox.Show("Please enter the userid", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (txtpw.Text.ToString().Trim() == "")
                    {
                        MessageBox.Show("Please enter the password", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                if (make_regString() == "")    return; 
                clsCon = new Classes.clsConnection();
                System.Data.SqlClient.SqlConnection sqlconCon = clsCon.sqlConReturn_Connection(make_regString()); 
                sqlconCon.Open(); 
                cmbDB.DataSource = clsCon.Fill_Table("SELECT Name FROM sys.databases Where database_Id > 4", sqlconCon);
                Assign_Variables(Crpt.DECRYPT(Crpt.DECRYPT(make_regString(), Tourist_Management.Classes.clsGlobal.RevertME()), Tourist_Management.Classes.clsGlobal.RevertME()));
                if (sqlconCon.State == ConnectionState.Open)   btnOk.Enabled = true; 
                sqlconCon.Close();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void optWN_CheckedChanged(object sender, EventArgs e)
        {
            apply_cntrl();
        }
        private void apply_cntrl()
        {
            if (optWN.Checked == true)
            {
                IsWinAuthenication = true;
                txtuserID.Text = "";
                txtpw.Text = "";
                txtuserID.Enabled = false;
                txtpw.Enabled = false;
            }
            else
            {
                IsWinAuthenication = false;
                txtuserID.Text = "";
                txtpw.Text = "";
                txtuserID.Enabled = true;
                txtpw.Enabled = true;
            }
        }
        private void cmbDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (TreeNode TN in tv.Nodes)
                {
                    if (TN.Text.ToString() == cmbDB.Text.ToString().Trim())
                    {
                        tv.SelectedNode = TN;
                        return;
                    }
                }
                Server = txtserver.Text;
                DataBase = cmbDB.Text;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            CRPT.CRPT Crpt;
            Crpt = new CRPT.CRPT();
            write_register();
            Assign_Variables(Crpt.DECRYPT(Crpt.DECRYPT(make_regString(), Tourist_Management.Classes.clsGlobal.RevertME()), Tourist_Management.Classes.clsGlobal.RevertME()));
            this.Close();
        }
        private void add_subNotes()
        {
            try
            {
                int y = -1;
                tv.ExpandAll();
                foreach (TreeNode TN in tv.Nodes)
                {
                    for (int x = 0; x < TN.Nodes.Count; x++)
                    {
                        if (TN.Nodes[x].Text.ToString() == txtserver.Text.ToString().Trim())
                        {
                            y = x;
                            break;
                        }
                    }
                }
                if (y > -1)
                {
                    txtserver.Enabled = false;
                    tv.SelectedNode = tv.Nodes[0].Nodes[y];
                }
                else
                {
                    chkManual.Checked = true;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkManual_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManual.Checked == true)
            {
                txtserver.Text = "";
                txtserver.Enabled = true;
            }
            else
            {
                txtserver.Text = "";
                txtserver.Enabled = false;
            }
        }
        private void txtserver_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            cmbDB.DataSource = null;
        }      
    }
}
