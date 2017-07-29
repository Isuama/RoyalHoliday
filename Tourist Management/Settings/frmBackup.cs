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
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;
namespace Tourist_Management.Settings
{
    public partial class frmBackup : Form
    {
        private const string msghd = "Make System Backup";
        int InsMode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        int Syscode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        Server srv;
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
        public frmBackup(){InitializeComponent();}
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (opB.Checked == true)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    txtPath.Text = folderBrowserDialog1.SelectedPath;
            }
            if (opR.Checked == true)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    txtPath.Text = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
            }
        }
        private void frmBackup_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void button5_Click(object sender, EventArgs e){this.Close();}
        private void Intializer()
        {
            try
            {
                Make_tree();
                Load_Server_Details();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Load_Server_Details()
        {
            try
            {
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
                dt = SmoApplication.EnumAvailableSqlServers(false);
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
        private void chkManual_CheckedChanged(object sender, EventArgs e)
        {
            if (chkManual.Checked)
                txtserver.Enabled = true;
            else
                txtserver.Enabled = false;
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
                    MessageBox.Show("Please Select a Server", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (txtuserID.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Please Enter The UserID", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (txtpw.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Please Enter The Password", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (make_regString() == "")
                {
                    return;
                }
                clsCon = new Classes.clsConnection();
                System.Data.SqlClient.SqlConnection sqlconCon = clsCon.sqlConReturn_Connection(make_regString()); 
                sqlconCon.Open();
                cmbDB.DataSource = clsCon.Fill_Table("SELECT Name FROM sys.databases Where database_Id > 4", sqlconCon);
                sqlconCon.Close();
            }
            catch (Exception ex){db.MsgERR(ex);}
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
                SqlConnection SqlCon = new SqlConnection(Classes.clsConnection.Read_Connection_String());
                string conn = SqlCon.ConnectionString;
                srv = new Server(conn);
                strString = Crpt.ENCRYPT(strString, Tourist_Management.Classes.clsGlobal.RevertME());
                return Crpt.ENCRYPT(strString, Tourist_Management.Classes.clsGlobal.RevertME());
        }
        private void optWN_CheckedChanged(object sender, EventArgs e)
        {
            if (optWN.Checked)
            {
                txtuserID.Enabled = false;
                txtpw.Enabled = false;
            }
            else
            {
                txtuserID.Enabled = true;
                txtpw.Enabled = true;
            }
        }
        private void btnBackup_Click(object sender, EventArgs e)
        {
            backup();
        }
        public void restore()
        {
            try
            {
                string fileName = txtPath.Text;
                string databaseName = this.cmbDB.SelectedItem.ToString();
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Value = 10;
                string timeF = DateTime.Now.ToString();
                String timeS = timeF.Replace(":", "#");
                string constr = "Data Source=" + txtserver.Text.ToString() + ";Initial Catalog=" + cmbDB.SelectedValue.ToString() + ";User ID=" + txtuserID.Text.ToString() + ";Password=" + txtpw.Text.ToString();
                SqlConnection sqlConnection1 = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd.CommandText = "USE MASTER RESTORE DATABASE [" + cmbDB.Text.ToString() + "] FROM  DISK = N'" + openFileDialog1.InitialDirectory + openFileDialog1.FileName + "' WITH  FILE = 1,  KEEP_REPLICATION,  NOUNLOAD,  REPLACE,  STATS = 10";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();
                reader = cmd.ExecuteReader();
                sqlConnection1.Close();
                MessageBox.Show("Database Restore To: " + fileName, "SMO Demos");
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.progressBar1.Value = 0;
            }
        }
        public void backup()
        {
            Backup bkp = new Backup();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                string fileName = txtPath.Text;
                string databaseName = this.cmbDB.SelectedItem.ToString();
                bkp.Action = BackupActionType.Database;
                bkp.Database = "TouristManagement";
                bkp.Devices.AddDevice(fileName, DeviceType.File);
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Value = 10;
                bkp.PercentCompleteNotification = 10;
                bkp.PercentComplete += new PercentCompleteEventHandler(ProgressEventHandler);
                string timeF = DateTime.Now.ToString();
                String timeS = timeF.Replace(":", "#");
                timeS=timeS.Replace("/", "#");
                string constr = "Data Source=" + txtserver.Text.ToString() + ";Initial Catalog=" + cmbDB.SelectedValue.ToString() + ";User ID=" + txtuserID.Text.ToString() + ";Password=" + txtpw.Text.ToString();
                SqlConnection sqlConnection1 = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                SqlDataReader reader;
                cmd.CommandText = "BACKUP DATABASE [" + cmbDB.SelectedValue.ToString() + "] TO  DISK = N'" + txtPath.Text.ToString() + "\\" + cmbDB.SelectedValue.ToString() + timeS + ".bak' WITH NOFORMAT, NOINIT,  NAME = N'" + cmbDB.SelectedValue.ToString() + "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection1;
                sqlConnection1.Open();
                reader = cmd.ExecuteReader();
                sqlConnection1.Close();
                MessageBox.Show("Database Backed Up To: " + fileName, "SMO Demos");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.progressBar1.Value = 0;
            }
        }
        public void ProgressEventHandler(object sender, PercentCompleteEventArgs e)
        {
            this.progressBar1.Value = e.Percent;
        }
        private void btnVerify_Click(object sender, EventArgs e)
        {
            Restore rest = new Restore();
            string fileName = txtPath.Text;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                rest.Devices.AddDevice(fileName, DeviceType.File);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            restore();
        }
        private void opB_CheckedChanged(object sender, EventArgs e)
        {
            btnRestore.Enabled = false;
            btnBackup.Enabled = true;
        }
        private void opR_CheckedChanged(object sender, EventArgs e)
        {
            btnBackup.Enabled = false;
            btnRestore.Enabled=true ;
        }
    }
}
