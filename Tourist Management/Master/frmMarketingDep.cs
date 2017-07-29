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
    public partial class frmMarketingDep : Form
    {
        private const string msghd = "Marketing Manager Details";
        public string SqlQry = "SELECT ID,Name AS[ Marketing Manager],ContactNo AS [Mobile No],Email,Isnull(IsActive,0)AS IsActive From mst_MarketingDep Order By Name";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        int earlyRow = 0;
        enum HTL { HotelID, HotelName, Days, Select };
        C1.Win.C1FlexGrid.CellStyle searched;
        C1.Win.C1FlexGrid.CellStyle transparent;
        public frmMarketingDep(){InitializeComponent();}
        private void frmMarketingDep_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                searched = grdHotels.Styles.Add("searched");
                searched.BackColor = ColorTranslator.FromHtml("#E1F5A9");
                transparent = grdHotels.Styles.Add("transparent");
                transparent.BackColor = Color.Transparent;
                Grd_Initializer();
                Fill_Hotels();
                if (Mode != 0)
                {
                    Fill_Data();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                grdHotels.Cols.Count = 4;
                grdHotels.Rows.Count = 2;
                grdHotels.Cols[(int)HTL.HotelID].Width = 100;
                grdHotels.Cols[(int)HTL.HotelName].Width = 140;
                grdHotels.Cols[(int)HTL.Days].Width = 60;
                grdHotels.Cols[(int)HTL.Select].Width = 100;
                grdHotels.Cols[(int)HTL.HotelID].Caption = "ID";
                grdHotels.Cols[(int)HTL.HotelName].Caption = "Hotel Name";
                grdHotels.Cols[(int)HTL.Days].Caption = "Days";
                grdHotels.Cols[(int)HTL.Select].Caption = "Select";
                grdHotels.Cols[(int)HTL.Select].DataType = Type.GetType("System.Boolean");
                grdHotels.Rows[1].AllowEditing = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Hotels()
        {
            try
            {
                string qry ="SELECT ID,Name FROM mst_HotelDetails WHERE ISNULL(IsActive,0)<>0";
                DataTable DT = Classes.clsGlobal.objCon.Fill_Table(qry);
                grdHotels.Rows.Count = DT.Rows.Count+2;
                foreach (DataRow dr in DT.Rows)
                {
                    grdHotels[DT.Rows.IndexOf(dr)+1, (int)HTL.HotelID] = dr["ID"];
                    grdHotels[DT.Rows.IndexOf(dr)+1, (int)HTL.HotelName] = dr["Name"];
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {
                DataTable DT;
                string ssql;
                ssql = " SELECT ID,Code,Name,ContactNo,Email,IsNull(IsActive,0) AS IsActive FROM mst_MarketingDep WHERE ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    Mode = 1;
                    txtName.Text = DT.Rows[0]["Name"].ToString();
                    txtMobile.Text = DT.Rows[0]["ContactNo"].ToString();
                    txtEmail.Text = DT.Rows[0]["Email"].ToString();
                    txtCode.Text = DT.Rows[0]["Code"] + "".Trim();
                    if (Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString()))
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
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
        private Boolean Validate_Data()
        {
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Marketing Name Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Name From mst_MarketingDep Where Name='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtCode.Text.Trim().Length > 3)
                {
                    MessageBox.Show("Code Cannot have more than 3 Characters", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_MarketingDep";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@ContactNo", SqlDbType.NVarChar, 50).Value = txtMobile.Text.Trim();
                sqlCom.Parameters.Add("@Email", SqlDbType.NVarChar, 50).Value = txtEmail.Text.Trim();
                sqlCom.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";                
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string s = txtName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s,errorProvider1,txtName);
        }
        private void txtName_Leave_1(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
        private void nudDays_ValueChanged(object sender, EventArgs e)
        {
            if (nudDays.Value + "".Trim() == "")
                return;
            if (nudDays.Value == 0)
                lblAlert.Visible = true;
            else
                lblAlert.Visible = false;
        }
        private void nudDays_Leave(object sender, EventArgs e)
        {
            if (nudDays.Value + "".Trim() == "")
            {
                nudDays.Value = 0;
                lblAlert.Visible = true;
            }
        }
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtHotelName.Text = "";
            txtHotelName.Focus();
        }
        private void txtHotelName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int row = 1;
                string HotelName;
                bool found = false;
                if (txtHotelName.Text.Trim() == "")
                {
                    if (earlyRow != 0)
                        grdHotels.Rows[earlyRow].Style = grdHotels.Styles["transparent"];
                    return;
                }
                while (row < grdHotels.Rows.Count - 1)
                {
                    HotelName = grdHotels[row, (int)HTL.HotelName]+"".Trim();
                    if (HotelName.Contains(txtHotelName.Text.Trim()))
                    {
                        if (earlyRow != 0)
                            grdHotels.Rows[earlyRow].Style = grdHotels.Styles["transparent"];
                        grdHotels.Select(row, (int)HTL.HotelID);
                        grdHotels.Rows[row].Style = grdHotels.Styles["searched"];
                        found = true;
                        earlyRow = row;
                        break;
                    }
                    row++;
                }
                if (!found && earlyRow != 0)
                    grdHotels.Rows[earlyRow].Style = grdHotels.Styles["transparent"];
            }
            catch (Exception ex){db.MsgERR(ex);}       
        }
    }
}
