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
    public partial class frmCityItinerary : Form
    {
        private const string msghd = "City Itinerary Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,CityFromName AS [From],CityToName AS [To],DistanceKm AS [Distance In Km],DistanceMl AS [Distance In Miles] From vw_CityItinerary Where Isnull([Status],0)<>7 Order By CityFromName,CityToName";
        private void Intializer()
        {
            Fill_Control();
            if (Mode == 0)
            {
               txtDsKm.Text = "";
               txtDsMls.Text = "";
               txtRemarks.Text = "";
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
                DataTable DT;
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,City FROM mst_City ORDER BY ID");
                drpCityFrom.DataSource = DT;
                drpCityTo.DataSource = DT;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select ID,CityFromID,CityToID,DistanceKm,DistanceMl,Remarks,Isnull(IsActive,0)as IsActive From mst_CityItinerary Where ID=" + SystemCode + "");
                if (DT.Rows[0]["CityFromID"].ToString() != "")
                    drpCityFrom.setSelectedValue(DT.Rows[0]["CityFromID"].ToString());
                if (DT.Rows[0]["CityToID"].ToString() != "")
                    drpCityTo.setSelectedValue(DT.Rows[0]["CityToID"].ToString());
                if (DT.Rows[0]["DistanceKm"].ToString() != "")
                    txtDsKm.Text = DT.Rows[0]["DistanceKm"].ToString();
                if (DT.Rows[0]["DistanceMl"].ToString() != "")
                    txtDsMls.Text = DT.Rows[0]["DistanceMl"].ToString();
                if (DT.Rows[0]["Remarks"].ToString() != "")
                    txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (drpCityFrom.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("City From Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (drpCityTo.SelectedValue.ToString() == "")
                {
                    MessageBox.Show("City To Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtDsKm.Text.ToString().Trim() == "")
                {
                    MessageBox.Show("Distance Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                {
                    if (!Convert.ToBoolean(Classes.clsGlobal.IsNumeric(txtDsKm.Text.ToString().Trim())))
                    {
                        MessageBox.Show("Please Enter Valid Value For Distance", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select ID From mst_CityItinerary Where CityFromID='" + drpCityFrom.SelectedValue.Trim() + "' and CityToID="+drpCityTo.SelectedValue.Trim() +" and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("This City From and To Is Already Exist", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select ID From mst_CityItinerary Where CityFromID='" + drpCityTo.SelectedValue.Trim() + "' and CityToID=" + drpCityFrom.SelectedValue.Trim() + " and ID <> " + SystemCode + "").Rows.Count > 0)
                {
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
                sqlCom.CommandText = "spSave_CityItinerary";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                if (drpCityFrom.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@CityFromID", SqlDbType.Int).Value = drpCityFrom.SelectedValue.Trim();
                if (drpCityTo.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@CityToID", SqlDbType.Int).Value = drpCityTo.SelectedValue.Trim();
                if (txtDsKm.Text.ToString() != "")
                    sqlCom.Parameters.Add("@DistanceKm", SqlDbType.Decimal).Value = txtDsKm.Text.Trim();
                if (txtDsMls.Text.ToString() != "")
                    sqlCom.Parameters.Add("@DistanceMl", SqlDbType.Decimal).Value = txtDsMls.Text.Trim();
                if (txtRemarks.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar,250).Value = txtRemarks.Text.Trim();
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
        public frmCityItinerary(){InitializeComponent();}
        private void frmCityItinerary_Load(object sender, EventArgs e)
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
        private void drpCityFrom_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCity", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void txtDsKm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
        private void txtDsMls_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
        private void txtRemarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
