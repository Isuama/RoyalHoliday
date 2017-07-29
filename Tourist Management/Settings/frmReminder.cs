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
    public partial class frmReminder : Form
    {
        private const string msghd = "Reminder"; 
        string ssql = "SELECT ID,RemName AS 'Reminder Name',RTime AS 'Time'," +
                       "Type = (case Type When 1 then 'Daily' When 2 then 'Monthly' When 3 then 'Custom' else 'None' end)," +
                       "Isnull(IsActive,0)AS IsActive From mst_Reminder" +
                       " Where UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID) + " AND Isnull([Status],0)<>7 Order By ID";
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
        public string SqlQry
        {
            get
            {
                return ssql;
            }
        }
        public frmReminder(){InitializeComponent();}
        private void frmReminder_Load(object sender, EventArgs e)
        {
            dtpTime.CustomFormat = "HH:mm:ss";
            dtpTime.ShowUpDown = true;
            Intializer();
        }
        private void Intializer()
        {
            if (InsMode == 0)
            {
                txtMessage.Text = "";
                txtMessage.Select();
                chkActive.Checked = true;
            }
            else
            {
                Fill_Data();
            }
        }
        private void Fill_Data()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select RemName," +
                    "Message,RTime,RDate,Rday,Type,Isnull(IsActive,0)as IsActive" +
                    " From mst_Reminder Where ID=" + Syscode + "");
                if (DT.Rows.Count > 0)
                {
                    txtRemName.Text = DT.Rows[0]["RemName"].ToString();
                    txtMessage.Text = DT.Rows[0]["Message"].ToString();
                    if (DT.Rows[0]["RDate"].ToString() != "")
                    dtpDate.Value = Convert.ToDateTime(DT.Rows[0]["RDate"].ToString());
                    dtpTime.Value = DateTime.Today + (TimeSpan)DT.Rows[0]["RTime"];
                    if (Convert.ToInt32(DT.Rows[0]["Type"].ToString()) == 1)
                    {
                        rdbDaily.Checked = true;
                    }
                    else if (Convert.ToInt32(DT.Rows[0]["Type"].ToString()) == 2)
                    {
                        rdbMonthly.Checked = true;
                        dtpDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(DT.Rows[0]["RDay"].ToString()));
                    }
                    else
                    {
                        rdbCustom.Checked = true;
                        dtpDate.Value = Convert.ToDateTime(DT.Rows[0]["RDate"].ToString());
                    }
                    chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this reminder", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Reminder Successfully Saved", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private bool Validate_Data()   {   return true;    }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false; 
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Reminder";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = Syscode;
                sqlCom.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@RemName", SqlDbType.NVarChar, 50).Value = txtRemName.Text.Trim();
                sqlCom.Parameters.Add("@Message", SqlDbType.NVarChar, 50).Value = txtMessage.Text.Trim();
                if (rdbDaily.Checked)
                {
                    sqlCom.Parameters.Add("@Type", SqlDbType.Int).Value = 1;
                }
                if (rdbMonthly.Checked)
                {
                    sqlCom.Parameters.Add("@Type", SqlDbType.Int).Value = 2;
                    sqlCom.Parameters.Add("@Day", SqlDbType.Int).Value = Convert.ToInt32(dtpDate.Text);
                }
                if (rdbCustom.Checked)
                {
                    sqlCom.Parameters.Add("@Date", SqlDbType.Date).Value = dtpDate.Value.Date;
                    sqlCom.Parameters.Add("@Type", SqlDbType.Int).Value = 3;
                }
                sqlCom.Parameters.Add("@Time", SqlDbType.Time).Value = dtpTime.Value.TimeOfDay;
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Bit).Value = chkActive.Checked == true ? true : false;
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = InsMode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void rdbDaily_CheckedChanged(object sender, EventArgs e)
        {
            dtpDate.Enabled = false;
        }
        private void rdbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            dtpDate.Enabled = true;
            dtpDate.Format = DateTimePickerFormat.Custom;
            dtpDate.CustomFormat = "dd";
            dtpDate.ShowUpDown = true;
        }
        private void rdbCustom_CheckedChanged(object sender, EventArgs e)
        {
            dtpDate.Value = DateTime.Now;
            dtpDate.Enabled = true;
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.ShowUpDown = false;
        }
    }
}
