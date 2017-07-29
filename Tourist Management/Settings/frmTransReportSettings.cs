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
    public partial class frmTransReportSettings : Form
    {
        private const string msghd = "Transaction Report Settings";
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
        public frmTransReportSettings(){InitializeComponent();}
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtPath.Text = folderBrowserDialog1.SelectedPath;
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Save_Pro() == true)
            {
                MessageBox.Show("Record sucessfully Updated", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private Boolean Save_Pro()
        {
                if (Validate_Data() == false)
                {
                    return false;
                }
                if (Save_Data() == false)
                {
                    return false;
                }
                return true;
        }
        private Boolean Validate_Data()
        {
                if (txtPath.Text.Trim() == "")
                {
                    MessageBox.Show("Path cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_TransReportSettings";
                sqlCom.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@Path", SqlDbType.VarChar, 200).Value = txtPath.Text.Trim();
                sqlCom.Parameters.Add("@IsExport", SqlDbType.Int).Value = chkExPlace.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@InvoicePath", SqlDbType.VarChar, 200).Value = txtPath2.Text.Trim();
                sqlCom.Parameters.Add("@InvoiceExport", SqlDbType.Int).Value = chkExPlace2.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@Email_To", SqlDbType.VarChar, 50).Value = txtEmailTo.Text.Trim();
                sqlCom.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = Classes.clsGlobal.UserID;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void frmTransReportSettings_Load(object sender, EventArgs e)
        {
            Initialize();
        }
        private void Initialize()
        {
            Fill_Data();
        }
        private void Fill_Data()
        {
            try
            {
                string ssql = "SELECT UserName FROM mst_UserMaster Where ID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "";
                txtUserName.Text = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql).Rows[0]["UserName"].ToString();
                string sql = "SELECT Path,IsNull(IsExport,0)AS IsExport,InvoicePath,IsNull(InvoiceExport,0)AS InvoiceExport, Email_To"+
                             " FROM mst_TransReportSettings WHERE UserID=" + Convert.ToInt32(Classes.clsGlobal.UserID.ToString()) + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    txtPath.Text = DT.Rows[0]["Path"].ToString();
                    chkExPlace.Checked = Convert.ToBoolean(DT.Rows[0]["IsExport"].ToString());
                    txtPath2.Text = DT.Rows[0]["InvoicePath"].ToString();
                    chkExPlace2.Checked = Convert.ToBoolean(DT.Rows[0]["InvoiceExport"].ToString());
                    txtEmailTo.Text = DT.Rows[0]["Email_To"].ToString();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtPath2.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
