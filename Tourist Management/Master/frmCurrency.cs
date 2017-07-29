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
    public partial class frmCurrency : Form
    {
        private const string msghd = "Currency Details";
        public string SqlQry = "SELECT ID,Code,Currency,Isnull(IsActive,0)AS IsActive From mst_Currency Order By ID";
        public int Mode = 0, SystemCode = 0;
        public frmCurrency()        {            InitializeComponent();        }
        private void frmCurrency_Load(object sender, EventArgs e)        {            Intializer();        }
        private void Intializer()   {   if (Mode != 0)  Fill_Data();   }
        private void Fill_Data()
        {
            try
            { 
                string   ssql = " SELECT ID,Code,Currency,IsNull(IsActive,0) AS IsActive FROM mst_Currency WHERE ID=" + SystemCode + "";
              DataTable  DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)  SystemCode =  Mode = 0; 
                else
                {
                    Mode = 1;
                    txtCode.Text = DT.Rows[0]["Code"] + "".Trim();
                    txtName.Text = DT.Rows[0]["Currency"].ToString();           
                        chkActive.Checked = (Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString()));
                }
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
        }
        private void btnCancel_Click(object sender, EventArgs e)        {            this.Close();        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)   return; 
            if (Validate_Data() && Save_Data())
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private Boolean Validate_Data()
        {
            string m = "";
            if (txtCode.Text.Trim() == "") m = "Code Cannot Be Blank";
            if (txtName.Text.Trim() == "") m = "Currency Cannot Be Blank";
            if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_Currency Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0) m = "Code already exists";
            if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Currency From mst_Currency Where Currency='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0) m = "Currency already exists";
            if (m != "") MessageBox.Show("Currency already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return m == "";
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
            sqlCom = new System.Data.SqlClient.SqlCommand();
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_Currency";
            sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
            sqlCom.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCode.Text.Trim();
            sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
            sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
            sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
            sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
            sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
            if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true) RtnVal = true;
            return RtnVal;
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string s = txtName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s, errorProvider1, txtName);
        }
        private void txtName_Leave_1(object sender, EventArgs e)        {            errorProvider1.Clear();        }
    }
}
