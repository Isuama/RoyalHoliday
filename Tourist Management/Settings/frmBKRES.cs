using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
namespace Tourist_Management.Settings
{
    public partial class frmBKRES : Form
    {
        Server srv;
        ServerConnection conn;
        public frmBKRES(){InitializeComponent();}
        private void frmMain_Load(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server");
            String[] instances = (String[])rk.GetValue("InstalledInstances");
            if (instances.Length > 0)
            {
                foreach (String element in instances)
                {
                    if (element == "MSSQLSERVER")
                        lstLocalInstances.Items.Add(System.Environment.MachineName);
                    else
                        lstLocalInstances.Items.Add(System.Environment.MachineName + @"\" + element);
                }
            }
            Thread threadGetNetworkInstances = new Thread(GetNetworkInstances);
            threadGetNetworkInstances.Start();
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                ddlDatabase.Items.Clear();
                string sqlSErverInstance;
                if (this.tabServers.SelectedIndex == 0)
                {
                    sqlSErverInstance = lstLocalInstances.SelectedItem.ToString();
                }
                else
                {
                    sqlSErverInstance = lstNetworkInstances.SelectedItem.ToString();
                }
                if (chkWindowsAuthentication.Checked == true)
                {
                    conn = new ServerConnection();
                    conn.ServerInstance = sqlSErverInstance;
                }
                else
                {
                    conn = new ServerConnection(sqlSErverInstance, txtLogin.Text, txtPassword.Text);
                }
                srv = new Server(conn);
                foreach (Database db in srv.Databases)
                {
                    ddlDatabase.Items.Add(db.Name);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "bak files (*.bak)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName.ToString();
            }
        }
        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
        }
        private void btnBackupDB_Click(object sender, EventArgs e)
        {
            Backup bkp = new Backup();
            this.Cursor = Cursors.WaitCursor;
            this.dataGridView1.DataSource = string.Empty;
            try
            {
                string fileName = this.txtFileName.Text;
                string databaseName = this.ddlDatabase.SelectedItem.ToString();
                bkp.Action = BackupActionType.Database;
                bkp.Database = databaseName;
                bkp.Devices.AddDevice(fileName, DeviceType.File);
                bkp.Incremental = chkIncremental.Checked;
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Value = 10;
                bkp.PercentCompleteNotification = 10;
                bkp.PercentComplete += new PercentCompleteEventHandler(ProgressEventHandler);
                bkp.SqlBackup(srv);
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
        private void btnRestore_Click(object sender, EventArgs e)
        {
            Restore res = new Restore();
            this.Cursor = Cursors.WaitCursor;
            this.dataGridView1.DataSource = string.Empty;
            try
            {
                string fileName = this.txtFileName.Text;
                string databaseName = this.ddlDatabase.SelectedItem.ToString();
                res.Database = databaseName;
                res.Action = RestoreActionType.Database;
                res.Devices.AddDevice(fileName, DeviceType.File);
                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = 100;
                this.progressBar1.Value = 10;
                res.PercentCompleteNotification = 10;
                res.ReplaceDatabase = true;
                res.PercentComplete += new PercentCompleteEventHandler(ProgressEventHandler);
                res.SqlRestore(srv);
                MessageBox.Show("Restore of " + databaseName + " Complete!", "Restore",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (SmoException exSMO)
            {
                MessageBox.Show(exSMO.ToString());
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
        private void btnBackupLog_Click(object sender, EventArgs e)
        {
            Backup bkp = new Backup();
            Cursor = Cursors.WaitCursor;
            dataGridView1.DataSource = "";
            try
            {
                string strFileName = txtFileName.Text.ToString();
                string strDatabaseName = ddlDatabase.SelectedItem.ToString();
                bkp.Action = BackupActionType.Log;
                bkp.Database = strDatabaseName;
                bkp.Devices.AddDevice(strFileName, DeviceType.File);
                progressBar1.Value = 0;
                progressBar1.Maximum = 100;
                progressBar1.Value = 10;
                bkp.PercentCompleteNotification = 10;
                bkp.PercentComplete += new PercentCompleteEventHandler(ProgressEventHandler);
                bkp.SqlBackup(srv);
                MessageBox.Show("Log Backed Up To: " + strFileName, "SMO Demos");
            }
            catch (SmoException exSMO)
            {
                MessageBox.Show(exSMO.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
                progressBar1.Value = 0;
            }
        }
        private void btnVerify_Click(object sender, EventArgs e)
        {
            Restore rest = new Restore();
            string fileName = this.txtFileName.Text;
            this.Cursor = Cursors.WaitCursor;
            this.dataGridView1.DataSource = string.Empty;
            try
            {
                rest.Devices.AddDevice(fileName, DeviceType.File);
                bool verifySuccessful = rest.SqlVerify(srv);
                if (verifySuccessful)
                {
                    MessageBox.Show("Backup Verified!", "SMO Demos");
                    DataTable dt = rest.ReadFileList(srv);
                    this.dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Backup NOT Verified!", "SMO Demos");
                }
            }
            catch (SmoException exSMO)
            {
                MessageBox.Show(exSMO.ToString());
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
        delegate void SetMessageCallback(string text);
        private void AddNetworkInstance(string text)
        {
            if (this.lstNetworkInstances.InvokeRequired)
            {
                SetMessageCallback d = new SetMessageCallback(AddNetworkInstance);
                this.BeginInvoke(d, new object[] { text });
            }
            else
            {
                this.lstNetworkInstances.Items.Add(text);
            }
        }
        private void GetNetworkInstances()
        {
DataTable dt = SmoApplication.EnumAvailableSqlServers(false);
if (dt.Rows.Count > 0)
{
    foreach (DataRow dr in dt.Rows)
    {
        AddNetworkInstance(dr["Name"].ToString());
    }
}
        }
        private void lstLocalInstances_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
