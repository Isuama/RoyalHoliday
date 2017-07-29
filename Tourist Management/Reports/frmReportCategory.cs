using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Reports
{
    public partial class frmReportCategory : Form
    {
        private const string msghd = "Bank Account Type";
        int InsMode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        int Syscode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        string ssql = "SELECT ID,CategoryName,IsActive From mst_Report_Category Order By CategoryName";
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
        private void Intializer()
        {
            if (InsMode == 0)
            {
                txtCode.Text = "";
                chkActive.Checked = true;
            }
            else
            {
                Fill_Details();
            }
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select CategoryName,Isnull(IsActive,0) From mst_Report_Category Where ID=" + Syscode + "");
                txtCode.Text = DT.Rows[0][0].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0][1].ToString());//== "True" ? true : false;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {        
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Code cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select CategoryName From mst_Report_Category Where CategoryName='" + txtCode.Text.Trim() + "' and ID <> " + Syscode).Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_Report_Category";//, 
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = Syscode;
                sqlCom.Parameters.Add("@CategoryName", SqlDbType.VarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = InsMode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        public frmReportCategory(){InitializeComponent();}
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Save_Pro() == true)
            {
                MessageBox.Show("Record sucessfully Updated", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error occured", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void frmReportCategory_Load(object sender, EventArgs e)
        {
            Intializer();
        }
    }
}
