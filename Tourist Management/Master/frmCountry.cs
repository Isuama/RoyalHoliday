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
    public partial class frmCountry : Form
    {
        private const string msghd = "Country";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID, Code, Country, IsActive  FROM mst_Country Where Isnull([Status],0)<>7 Order By Code";
        public frmCountry(){InitializeComponent();}
        private void Intializer()
        {
            if (Mode == 0)
            {
                txtCode.Text = "";
                txtName.Text = "";
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
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code, Country,Isnull(IsSaarc,0),Isnull(IsActive,0) FROM mst_Country Where ID=" + SystemCode + "");
                txtCode.Text = DT.Rows[0][0].ToString();
                txtName.Text = DT.Rows[0][1].ToString();
                chkIsSaarc.Checked = System.Convert.ToBoolean(DT.Rows[0][2].ToString());
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0][3].ToString());//== "True" ? true : false;
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
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code From mst_Country Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + " and Isnull([Status],0)<>7").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Country From mst_Country Where Country='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + " and Isnull([Status],0)<>7 ").Rows.Count > 0)
                {
                    MessageBox.Show("Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_Country";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Country", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@IsSaarc", SqlDbType.Int).Value = chkIsSaarc.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void frmCountry_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void btnOk_Click_1(object sender, EventArgs e)
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
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    chkActive.Checked = true;
                }
                else
                    return;
            }
        }
    }
}
