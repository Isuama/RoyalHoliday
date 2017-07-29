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
    public partial class frmOtherSettings : Form
    {
        int InsMode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        double Syscode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        private const string msghd = "Other Settings"; 
        public frmOtherSettings(){InitializeComponent();}
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
        public double SystemCode
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
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Save_Pro() == true)
            {
                MessageBox.Show("Record sucessfully Updated", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void label5_Click(object sender, EventArgs e)
        {
        }
        private void frmOtherSettings_Load(object sender, EventArgs e)
        {
            Initialize();
        }
        private void Initialize()
        {
            Fill_Data();
            Fill_Control();
        }
        private void Fill_Data()
        {
            try
            {
                string ssql = "SELECT * FROM mst_OtherSettings ";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    txtRname1.Text = DT.Rows[0]["Rname1"].ToString();
                    txtRno1.Text = DT.Rows[0]["Rno1"].ToString();
                    txtRname2.Text = DT.Rows[0]["Rname2"].ToString();
                    txtRno2.Text = DT.Rows[0]["Rno2"].ToString();
                    txtAname1.Text = DT.Rows[0]["Aname1"].ToString();
                    txtAno1.Text = DT.Rows[0]["Ano1"].ToString();
                    txtAname2.Text = DT.Rows[0]["Aname2"].ToString();
                    txtAno2.Text = DT.Rows[0]["Ano2"].ToString();
                    txtTname1.Text = DT.Rows[0]["Tname1"].ToString();
                    txtTno1.Text = DT.Rows[0]["Tno1"].ToString();
                    txtTname2.Text = DT.Rows[0]["Tname2"].ToString();
                    txtTno2.Text = DT.Rows[0]["Tno2"].ToString();
                    txtMDname.Text = DT.Rows[0]["MDname"].ToString();
                    txtMDno.Text = DT.Rows[0]["MDno"].ToString();
                    txtAADname.Text = DT.Rows[0]["AADname"].ToString();
                    txtAADno.Text = DT.Rows[0]["AADno"].ToString();
                    DateTime dt;
                    if (DateTime.TryParse(DT.Rows[0]["DateFrom"].ToString(), out dt))
                    dtpDateFrom.Value = Convert.ToDateTime(DT.Rows[0]["DateFrom"].ToString());
                    txtNoOfDays.Text = DT.Rows[0]["NoOfDays"].ToString();
                    txtConRate.Text = DT.Rows[0]["ConRate"].ToString();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
                cmbRate.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM stt_HotelRates ORDER BY ID"); 
                cmbRate.SelectedValue = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID FROM stt_HotelRates WHERE IsActive=1").Rows[0]["ID"]);
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
                decimal n;
                if (txtConRate.Text != "" && !Decimal.TryParse(txtConRate.Text, out n))
                {
                    MessageBox.Show("Please Enter Valid Conversion Rate",msghd,MessageBoxButtons.OK,MessageBoxIcon.Warning);
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
                sqlCom.CommandText = "spSave_OtherSettings";
                sqlCom.Parameters.Add("@Rname1", SqlDbType.NVarChar, 50).Value = txtRname1.Text.Trim();
                sqlCom.Parameters.Add("@Rno1", SqlDbType.NVarChar, 50).Value = txtRno1.Text.Trim();
                sqlCom.Parameters.Add("@Rname2", SqlDbType.VarChar, 50).Value = txtRname2.Text.Trim();
                sqlCom.Parameters.Add("@Rno2", SqlDbType.NVarChar, 50).Value = txtRno2.Text.Trim();
                sqlCom.Parameters.Add("@Aname1", SqlDbType.NVarChar, 50).Value = txtAname1.Text.Trim();
                sqlCom.Parameters.Add("@Ano1", SqlDbType.NVarChar, 50).Value = txtAno1.Text.Trim();
                sqlCom.Parameters.Add("@Aname2", SqlDbType.VarChar, 50).Value = txtAname2.Text.Trim();
                sqlCom.Parameters.Add("@Ano2", SqlDbType.NVarChar, 50).Value = txtAno2.Text.Trim();
                sqlCom.Parameters.Add("@Tname1", SqlDbType.NVarChar, 50).Value = txtTname1.Text.Trim();
                sqlCom.Parameters.Add("@Tno1", SqlDbType.NVarChar, 50).Value = txtTno1.Text.Trim();
                sqlCom.Parameters.Add("@Tname2", SqlDbType.VarChar, 50).Value = txtTname2.Text.Trim();
                sqlCom.Parameters.Add("@Tno2", SqlDbType.NVarChar, 50).Value = txtTno2.Text.Trim();
                sqlCom.Parameters.Add("@MDname", SqlDbType.NVarChar, 50).Value = txtMDname.Text.Trim();
                sqlCom.Parameters.Add("@MDno", SqlDbType.NVarChar, 50).Value = txtMDno.Text.Trim();
                sqlCom.Parameters.Add("@AADname", SqlDbType.VarChar, 50).Value = txtAADname.Text.Trim();
                sqlCom.Parameters.Add("@AADno", SqlDbType.NVarChar, 50).Value = txtAADno.Text.Trim();
                sqlCom.Parameters.Add("@RateID", SqlDbType.Int).Value = Convert.ToInt32(cmbRate.SelectedValue);
                sqlCom.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtpDateFrom.Value);
                int n;
                if(txtNoOfDays.Text != "" && Int32.TryParse(txtNoOfDays.Text, out n))
                sqlCom.Parameters.Add("@NoOfDays", SqlDbType.Int).Value = Convert.ToInt32(txtNoOfDays.Text.Trim());
                if(txtConRate.Text != "")
                sqlCom.Parameters.Add("@ConRate", SqlDbType.Decimal).Value = Convert.ToDecimal(txtConRate.Text.Trim());
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void txtConRate_TextChanged(object sender, EventArgs e)
        {
            if (!Tourist_Management.Classes.clsGlobal.IsNumeric(txtConRate.Text.Trim()))
            {
                errorProvider1.Clear();
                if (txtConRate.Text.Length > 0)
                {
                    txtConRate.Text = txtConRate.Text.Remove(txtConRate.Text.Length - 1);
                    txtConRate.SelectionStart = txtConRate.Text.Length;
                    errorProvider1.SetError(txtConRate, "Please Enter Valid Conversion Rate");
                }
            }
        }
    }
}
