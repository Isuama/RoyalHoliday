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
    public partial class frmSafariCompanies : Form
    {
        private const string msghd = "Manage Safari";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Company,VoucherName,VehicleType,ContactPerson1 ContactPerson,ContactNo1 ContactNo From mst_SafariCompanies Where Isnull([Status],0)<>7 Order By ID";
        public frmSafariCompanies(){InitializeComponent();}
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void frmSafariCompanies_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            fill_control();
            if (Mode == 0)
            {                
                clearContent();
            }
            else
            {
                Fill_Details();
            }
        }
        private void fill_control()
        {
            try
            {
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [ID],[FirstName] FROM mst_EmployeePersonal Where Isnull([Status],0)<>7");
                drpHandled1.DataSource = DT;
                drpHandled2.DataSource = DT;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void clearContent()
        {
                txtName.Text = "";
                txtVoucherName.Text = "";
                txtVehicleType.Text = "";
                txtRemarks.Text = "";
                txtConName1.Text = "";
                txtConName2.Text = "";
                txtConNumb1.Text = "";
                txtConNumb2.Text = "";
                lblHandMobile1.Text = "";
                lblHandMobile2.Text = "";
                drpHandled1.setSelectedValue(null);
                drpHandled2.setSelectedValue(null);
                chkActive.Checked = false;
        }
        private void Fill_Details()
        {
            DataTable DT;
                string sql = "SELECT ID,Company,VoucherName,VehicleType,ContactPerson1,ContactPerson2,"+
                             "ContactNo1,ContactNo2,HandledID1,HandledID2,Remarks,ISNULL(IsActive,0) IsActive" +
                             " FROM mst_SafariCompanies"+
                             " Where ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows[0]["Company"] + "".Trim() != "")
                    txtName.Text = DT.Rows[0]["Company"].ToString();
                if (DT.Rows[0]["VoucherName"] + "".Trim() != "")
                    txtVoucherName.Text = DT.Rows[0]["VoucherName"].ToString();
                if (DT.Rows[0]["VehicleType"] + "".Trim() != "")
                    txtVehicleType.Text = DT.Rows[0]["VehicleType"].ToString();
                if (DT.Rows[0]["Remarks"] + "".Trim() != "")
                    txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                if (DT.Rows[0]["ContactPerson1"] + "".Trim() != "")
                    txtConName1.Text = DT.Rows[0]["ContactPerson1"].ToString();
                if (DT.Rows[0]["ContactPerson2"] + "".Trim() != "")
                    txtConName2.Text = DT.Rows[0]["ContactPerson2"].ToString();
                if (DT.Rows[0]["ContactNo1"] + "".Trim() != "")
                    txtConNumb1.Text = DT.Rows[0]["ContactNo1"].ToString();
                if (DT.Rows[0]["ContactNo2"] + "".Trim() != "")
                    txtConNumb2.Text = DT.Rows[0]["ContactNo2"].ToString();
                if (DT.Rows[0]["HandledID1"] + "".Trim() != "")
                {
                    drpHandled1.setSelectedValue(DT.Rows[0]["HandledID1"].ToString());
                    lblHandMobile1.Text = getEmployeeMobileNo(Convert.ToInt32(DT.Rows[0]["HandledID1"]));
                }
                if (DT.Rows[0]["HandledID2"] + "".Trim() != "")
                {
                    drpHandled2.setSelectedValue(DT.Rows[0]["HandledID2"].ToString());
                    lblHandMobile2.Text = getEmployeeMobileNo(Convert.ToInt32(DT.Rows[0]["HandledID2"]));
                }
                chkActive.Checked = Convert.ToBoolean(DT.Rows[0]["IsActive"]);
        }
        private Boolean Validate_Data()
        {
            try
            {
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtName.Focus();
                    return false;
                }
                else if (txtVoucherName.Text.Trim() == "")
                {
                    MessageBox.Show("Voucher Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtVoucherName.Focus();
                    return false;
                }
                else if (txtVehicleType.Text.Trim() == "")
                {
                    MessageBox.Show("Vehicle cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtVehicleType.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
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
            try
            {
                System.Data.SqlClient.SqlCommand sqlCom;
                Boolean RtnVal = false;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_SafariCompanies";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Company", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@VoucherName", SqlDbType.NVarChar, 100).Value = txtVoucherName.Text.Trim();
                sqlCom.Parameters.Add("@VehicleType", SqlDbType.NVarChar, 100).Value = txtVehicleType.Text.Trim();
                sqlCom.Parameters.Add("@ContactPerson1", SqlDbType.NVarChar, 100).Value = txtConName1.Text.Trim();
                sqlCom.Parameters.Add("@ContactPerson2", SqlDbType.NVarChar, 100).Value = txtConName2.Text.Trim();
                sqlCom.Parameters.Add("@ContactNo1", SqlDbType.NVarChar, 100).Value = txtConNumb1.Text.Trim();
                sqlCom.Parameters.Add("@ContactNo2", SqlDbType.NVarChar, 100).Value = txtConNumb2.Text.Trim();
                sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 250).Value = txtRemarks.Text.Trim();
                if (drpHandled1.SelectedValue+"".Trim() != "")
                    sqlCom.Parameters.Add("@HandledID1", SqlDbType.Int).Value = drpHandled1.SelectedValue.Trim();
                if (drpHandled2.SelectedValue + "".Trim() != "")
                    sqlCom.Parameters.Add("@HandledID2", SqlDbType.Int).Value = drpHandled2.SelectedValue.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
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
        private void drpHandled1_Selected_TextChanged(object sender, EventArgs e)
        {
            if (drpHandled1.SelectedValue + "".Trim() == "")
                return;
            lblHandMobile1.Text = getEmployeeMobileNo(Convert.ToInt32(drpHandled1.SelectedValue));
        }
        private void drpHandled2_Selected_TextChanged(object sender, EventArgs e)
        {
            if (drpHandled2.SelectedValue + "".Trim() == "")
                return;
            lblHandMobile2.Text = getEmployeeMobileNo(Convert.ToInt32(drpHandled2.SelectedValue));
        }
        private string getEmployeeMobileNo(int EmpID)
        {
            try
            {
                string sql = "SELECT TelMobile FROM mst_EmployeeContact WHERE ID="+EmpID+"";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                    return DT.Rows[0]["TelMobile"].ToString().Trim();
                else
                    return "";
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return "";
            }
        }
    }
}
