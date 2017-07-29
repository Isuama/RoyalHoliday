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
    public partial class frmCity : Form
    {
        private const string msghd = "City Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Code as PostalCode,City,District,Province From vwCityDetails Where Isnull([Status],0)<>7 Order By District";
        private void Intializer()
        {
            Fill_Control();
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
        private void Fill_Control()
        {
            try
            {
                DataTable[] DTB;
                DTB = new DataTable[2];
                DTB[0] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_Province ORDER BY ID");
                drpProvince.DataSource = DTB[0];
                DTB[1] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_District ORDER BY ID");
                drpDistrict.DataSource = DTB[1];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code,ProvinceID,DistrictID,City,Isnull(IsActive,0)as IsActive From vwCityDetails Where ID=" + SystemCode + "");
                if (DT.Rows[0]["ProvinceID"].ToString()!="")
                    drpProvince.setSelectedValue(DT.Rows[0]["ProvinceID"].ToString());
                if (DT.Rows[0]["DistrictID"].ToString() != "")
                    drpDistrict.setSelectedValue(DT.Rows[0]["DistrictID"].ToString());
                if (DT.Rows[0]["Code"].ToString()!="")
                    txtCode.Text = DT.Rows[0]["Code"].ToString();
                txtName.Text = DT.Rows[0]["City"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
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
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code From mst_City Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Postal Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select City From mst_City Where City='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("City Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_City";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                if(txtCode.Text.ToString()!="")
                    sqlCom.Parameters.Add("@Code", SqlDbType.Char, 10).Value = txtCode.Text.Trim();
                if (drpDistrict.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@DistrictID", SqlDbType.Int).Value = drpDistrict.SelectedValue.Trim();
                sqlCom.Parameters.Add("@City", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
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
        public frmCity(){InitializeComponent();}
        private void frmCity_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void drpProvince_Validating(object sender, CancelEventArgs e)
        {
            DataTable DT;
            string ssql;
            if (drpProvince.SelectedValue.ToString() != "")
            {
                ssql = "SELECT ID,Name FROM mst_District WHERE ProvinceID=" + drpProvince.SelectedValue.Trim() + " ORDER BY ID";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
                drpDistrict.DataSource = DT;
            }
        }
        private void drpDistrict_Validating(object sender, CancelEventArgs e)
        {
            DataTable DT;
            string ssql; ;
            if (drpDistrict.SelectedValue.ToString() != "")
            {
                ssql = "SELECT ProvinceID FROM mst_District WHERE ID=" + drpDistrict.SelectedValue.Trim() + "";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                    drpProvince.setSelectedValue(DT.Rows[0]["ProvinceID"].ToString());
            }
        }
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
