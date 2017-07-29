using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Tourist_Management.Master
{
    public partial class frmGuide : Form
    {
        private const string msghd = "Guide Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Code,Name as GuideName,CompanyName,IdentityNo,IsNull(IsActive,0)AS IsActive From vwGuideVsEmployee Where Isnull([Status],0)<>7 Order By Code";
        byte[] imageData = null;  //TO KEEP BANK LOGO IMAGE AS A BINARY DATA
        int RowNumb;
        Boolean bLoad = false;
        enum PH { gNUM, gDES, gBRW, gIMG };
        enum LG { gLID, gLNM, gLOW, gAVG, gFLN, gNTV}
        private void Intializer()
        {
            lblDischarge.Visible = false;
            dtpDateDisc.Visible = false;
            chkActive.Checked = false;
            ckeck_status(false);
            Fill_Control();
            Grd_Initializer();
            Fill_Languages_Grid();
            if (Mode == 0)
            {
                chkActive.Checked = true;
                Generate_Guide_Code();
            }
            else
            {
                Fill_Details(); 
            }
        }
        private void Generate_Guide_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_GuideDetails";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            txtCode.Text = "GDE" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
        }
        private void Grd_Initializer()
        {
            try
            {
                grdPhoto.Cols.Count = 4;
                grdLang.Cols.Count = 6;
                grdPhoto.Rows.Count = 500;
                grdLang.Rows.Count = 500;
                grdLang.Cols[(int)LG.gLID].Width = 0;
                grdLang.Cols[(int)LG.gLNM].Width = 301;
                grdLang.Cols[(int)LG.gLOW].Width = 70;
                grdLang.Cols[(int)LG.gAVG].Width = 70;
                grdLang.Cols[(int)LG.gFLN].Width = 70;
                grdLang.Cols[(int)LG.gNTV].Width = 70;
                grdLang.Cols[(int)LG.gLID].Caption = "Language ID";
                grdLang.Cols[(int)LG.gLNM].Caption = "Language";
                grdLang.Cols[(int)LG.gLOW].Caption = "Low";
                grdLang.Cols[(int)LG.gAVG].Caption = "Average";
                grdLang.Cols[(int)LG.gFLN].Caption = "Fluent";
                grdLang.Cols[(int)LG.gNTV].Caption = "Native";
                grdPhoto.Cols[(int)PH.gNUM].Width = 131;
                grdPhoto.Cols[(int)PH.gDES].Width = 380;
                grdPhoto.Cols[(int)PH.gBRW].Width = 70;
                grdPhoto.Cols[(int)PH.gIMG].Width = 0;
                grdPhoto.Cols[(int)PH.gNUM].Caption = "Serial No";
                grdPhoto.Cols[(int)PH.gDES].Caption = "Description";
                grdPhoto.Cols[(int)PH.gBRW].Caption = "Browse";
                grdPhoto.Cols[(int)PH.gIMG].Caption = "Image/Binary Data";
                grdPhoto.Cols[(int)PH.gBRW].ComboList = "...";
                grdPhoto.Rows[1].AllowEditing = true;
                grdLang.Cols[(int)LG.gLOW].DataType = Type.GetType("System.Boolean");
                grdLang.Cols[(int)LG.gAVG].DataType = Type.GetType("System.Boolean");
                grdLang.Cols[(int)LG.gFLN].DataType = Type.GetType("System.Boolean");
                grdLang.Cols[(int)LG.gNTV].DataType = Type.GetType("System.Boolean");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Languages_Grid()
        {
            DataTable DT;
            string Ssql;
            try
            {
                Ssql = "SELECT ID,Name FROM mst_Language WHERE IsActive=1 ORDER BY Name";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(Ssql);
                grdLang.Rows.Count = DT.Rows.Count + 1;
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdLang[RowNumb + 1, (int)LG.gLID] = DT.Rows[RowNumb]["ID"].ToString();
                        grdLang[RowNumb + 1, (int)LG.gLNM] = DT.Rows[RowNumb]["Name"].ToString();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            DataTable DT;
            if(chkIsEmp.Checked)
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,NameWithIntials FROM mst_EmployeePersonal Where IsNull(IsActive,0)=1 ORDER BY ID");
            else
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM vw_TR_Driver Where IsNull(IsActive,0)=1 ORDER BY ID");
            drpEmpID.DataSource = DT; 
        }
        private void Fill_Details()
        {
            try
            {
                DataTable DT;
                string ssql;
                ssql = " SELECT ID,EmpID,DriverID,EmpPhoto,CompanyName,Fee,Code,Name,IdentityNo,LicenseNo,EngageDate,DischargeDate,"+
                       " PermanantAdd,TelHome,TelMobile,ContName,ContTel1,Remarks,Isnull(IsActive,0) AS IsActive " +
                        "FROM vwGuideVsEmployee " +
                        "Where ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    Mode = 1;
                    if (DT.Rows[0]["EmpID"].ToString() != null && DT.Rows[0]["EmpID"].ToString() != "")
                    {
                        drpEmpID.setSelectedValue(DT.Rows[0]["EmpID"].ToString());
                        chkIsEmp.Checked = true;
                    }
                    if (DT.Rows[0]["DriverID"].ToString() != null && DT.Rows[0]["DriverID"].ToString() != "")
                    {
                        drpEmpID.setSelectedValue(DT.Rows[0]["DriverID"].ToString());
                        chkIsDriver.Checked = true;
                    }
                    if (DT.Rows[0]["CompanyName"].ToString() != null && DT.Rows[0]["CompanyName"].ToString() != "")
                        txtCompany.Text = DT.Rows[0]["CompanyName"].ToString();
                    if (DT.Rows[0]["Fee"].ToString() != null && DT.Rows[0]["Fee"].ToString() != "")
                        txtFee.Text = DT.Rows[0]["Fee"].ToString();
                    if (DT.Rows[0]["Code"].ToString() != null && DT.Rows[0]["Code"].ToString() != "")
                        txtCode.Text = DT.Rows[0]["Code"].ToString();
                    if (DT.Rows[0]["Name"].ToString() != null && DT.Rows[0]["Name"].ToString() != "")
                       txtName.Text = DT.Rows[0]["Name"].ToString();
                    if (DT.Rows[0]["IdentityNo"].ToString() != null && DT.Rows[0]["IdentityNo"].ToString() != "")
                       txtNIC.Text = DT.Rows[0]["IdentityNo"].ToString();
                    if (DT.Rows[0]["LicenseNo"].ToString() != null && DT.Rows[0]["LicenseNo"].ToString() != "")
                        txtLicense.Text = DT.Rows[0]["LicenseNo"].ToString();
                    if (DT.Rows[0]["EngageDate"].ToString() != "")
                        dtpDateEngage.Value = System.Convert.ToDateTime(DT.Rows[0]["EngageDate"].ToString());
                    if (DT.Rows[0]["DischargeDate"].ToString() != "")
                    dtpDateDisc.Value = System.Convert.ToDateTime(DT.Rows[0]["DischargeDate"].ToString());
                    if (DT.Rows[0]["PermanantAdd"].ToString() != null && DT.Rows[0]["PermanantAdd"].ToString() != "")
                        txtAddress.Text = DT.Rows[0]["PermanantAdd"].ToString();
                    if (DT.Rows[0]["TelHome"].ToString() != null && DT.Rows[0]["TelHome"].ToString() != "")
                        txtTel1.Text = DT.Rows[0]["TelHome"].ToString();
                    if (DT.Rows[0]["TelMobile"].ToString() != null && DT.Rows[0]["TelMobile"].ToString() != "")
                        txtTel2.Text = DT.Rows[0]["TelMobile"].ToString();
                    if (DT.Rows[0]["ContName"].ToString() != null && DT.Rows[0]["ContName"].ToString() != "")
                        txtSpouseName.Text = DT.Rows[0]["ContName"].ToString();
                    if (DT.Rows[0]["ContTel1"].ToString() != null && DT.Rows[0]["ContTel1"].ToString() != "")
                        txtSpouseNo.Text = DT.Rows[0]["ContTel1"].ToString();
                    if (DT.Rows[0]["Remarks"].ToString() != null && DT.Rows[0]["Remarks"].ToString() != "")
                        txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                    chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                    if (DT.Rows[0]["EmpPhoto"] != DBNull.Value)
                    {
                        byte[] Photo = (byte[])DT.Rows[0]["EmpPhoto"];
                        imageData = Photo;
                        MemoryStream ms = new MemoryStream(Photo);
                        pbImage.Image = Image.FromStream(ms, false, false);
                        lblImage.Visible = false;
                    }
                    else
                        lblImage.Visible = true;
                    ssql = "SELECT LanguageID,Language,Low,Avg,Fluent,Native" +
                           " FROM vw_Guide_Languages WHERE GuideID=" + SystemCode + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            grdLang[RowNumb + 1, (int)LG.gLID] = Convert.ToInt32(DT.Rows[RowNumb]["LanguageID"].ToString());
                            grdLang[RowNumb + 1, (int)LG.gLNM] = DT.Rows[RowNumb]["Language"].ToString();
                            grdLang[RowNumb + 1, (int)LG.gLOW] = Convert.ToBoolean(DT.Rows[RowNumb]["Low"].ToString());
                            grdLang[RowNumb + 1, (int)LG.gAVG] = Convert.ToBoolean(DT.Rows[RowNumb]["Avg"].ToString());
                            grdLang[RowNumb + 1, (int)LG.gFLN] = Convert.ToBoolean(DT.Rows[RowNumb]["Fluent"].ToString());
                            grdLang[RowNumb + 1, (int)LG.gNTV] = Convert.ToBoolean(DT.Rows[RowNumb]["Native"].ToString());
                            RowNumb++;
                        }
                    }
                    ssql = "SELECT GuideID,SerialNo,Description,Image" +
                           " FROM mst_GuidePhotos WHERE GuideID=" + SystemCode + " ORDER BY SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            grdPhoto[RowNumb + 1, (int)PH.gNUM] = DT.Rows[RowNumb]["SerialNo"].ToString();
                            grdPhoto[RowNumb + 1, (int)PH.gDES] = DT.Rows[RowNumb]["Description"].ToString();
                            byte[] Photo = (byte[])DT.Rows[RowNumb]["Image"];
                            imageData = Photo;
                            grdPhoto[RowNumb + 1, (int)PH.gIMG] = imageData;
                            RowNumb++;
                        }
                    }
                    update_tree_view();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Guide Code Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_Driver Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code Is Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtLicense.Text.Trim() != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select LicenseNo From mst_Driver Where LicenseNo='" + txtLicense.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                    {
                        MessageBox.Show("License Number Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                if (Validate_Guide_Photos() == false)
                    return false;
                return true;
        }
        private Boolean Validate_Guide_Photos()
        {
                RowNumb = 1;
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return true;
                }
                do
                {
                    if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                    {
                        MessageBox.Show("Please Enter Serial No ", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gDES].Index] == null) || (grdPhoto[RowNumb, (int)PH.gDES].ToString() == ""))
                    {
                        MessageBox.Show("Image Description Cannot Be Blank ", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gIMG].Index] == null) || (grdPhoto[RowNumb, (int)PH.gIMG].ToString() == ""))
                    {
                        MessageBox.Show("Image Cannot Be Blank.Please Browse For Image", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                } while ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null));
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
                if (Save_Procedure() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
        private Boolean Save_Procedure()
        {
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if (Save_Guide_Details(objCom) == true && Save_Guide_Languages(objCom) == true && Save_Guide_Photos(objCom) == true)
                {
                    objTrn.Commit();
                    MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    objCon.Close();
                    return true;
                }
                else
                {
                    objTrn.Rollback();
                    MessageBox.Show("Error Occured,Rollbacked", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                objCon.Close();
                return false;
        }
        private Boolean Save_Guide_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Guide_Details";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if (chkIsEmp.Checked==true && drpEmpID.SelectedValue.ToString() != null && drpEmpID.SelectedValue.ToString() != "")
                {
                    sqlCom.Parameters.Add("@EmpID", SqlDbType.Int).Value = drpEmpID.SelectedValue.Trim();
                }
                if (chkIsDriver.Checked == true && drpEmpID.SelectedValue.ToString() != null && drpEmpID.SelectedValue.ToString() != "")
                {
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = drpEmpID.SelectedValue.Trim();
                }
                else
                {                    
                    sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = txtName.Text.Trim();
                    sqlCom.Parameters.Add("@NIC", SqlDbType.VarChar, 20).Value = txtNIC.Text.Trim();
                    sqlCom.Parameters.Add("@DateEnagage", SqlDbType.DateTime).Value = dtpDateEngage.Value;
                    sqlCom.Parameters.Add("@DateDischarge", SqlDbType.DateTime).Value = dtpDateDisc.Value;
                    sqlCom.Parameters.Add("@Address", SqlDbType.VarChar, 250).Value = txtAddress.Text.Trim();
                    sqlCom.Parameters.Add("@Tel1", SqlDbType.VarChar, 20).Value = txtTel1.Text.Trim();
                    sqlCom.Parameters.Add("@Tel2", SqlDbType.VarChar, 20).Value = txtTel2.Text.Trim();
                    sqlCom.Parameters.Add("@SpouseName", SqlDbType.VarChar, 50).Value = txtSpouseName.Text.Trim();
                    sqlCom.Parameters.Add("@SpouseNo", SqlDbType.VarChar, 50).Value = txtSpouseNo.Text.Trim();
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = txtRemarks.Text.Trim();
                    if (imageData == null)
                        sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = null;
                    else
                        sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = imageData;
                }
                sqlCom.Parameters.Add("@Code", SqlDbType.Char, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Company", SqlDbType.VarChar, 50).Value = txtCompany.Text.Trim();
                if(txtFee.Text.ToString().Trim()!="")
                    sqlCom.Parameters.Add("@Fee", SqlDbType.Decimal).Value = Convert.ToDecimal(txtFee.Text.Trim());
                sqlCom.Parameters.Add("@LicenseNo", SqlDbType.VarChar, 50).Value = txtLicense.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
        }
        private Boolean Save_Guide_Languages(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Guide_Languages";
                while (grdLang[RowNumb, grdLang.Cols[(int)LG.gLID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@GuideID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@LanguageID", SqlDbType.Int).Value = Convert.ToInt32(grdLang[RowNumb, (int)LG.gLID].ToString());
                    sqlCom.Parameters.Add("@Low", SqlDbType.Int).Value = Convert.ToBoolean(grdLang[RowNumb, (int)LG.gLOW]);
                    sqlCom.Parameters.Add("@Avg", SqlDbType.Int).Value = Convert.ToBoolean(grdLang[RowNumb, (int)LG.gAVG]);
                    sqlCom.Parameters.Add("@Fluent", SqlDbType.Int).Value = Convert.ToBoolean(grdLang[RowNumb, (int)LG.gFLN]);
                    sqlCom.Parameters.Add("@Native", SqlDbType.Int).Value = Convert.ToBoolean(grdLang[RowNumb, (int)LG.gNTV]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    RowNumb++;
                    if (RowNumb >= grdLang.Rows.Count)
                        break;
                }
                return true;
        }
        private Boolean Save_Guide_Photos(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return true;
                }
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Guide_Photos";
                while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@GuideID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SerialNo", SqlDbType.VarChar, 20).Value = grdPhoto[RowNumb, (int)PH.gNUM].ToString();
                    sqlCom.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = grdPhoto[RowNumb, (int)PH.gDES].ToString();
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = grdPhoto[RowNumb, (int)PH.gIMG];
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    RowNumb++;
                }
                return true;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true){this.Close();}
        }
        private void btnCancel_Click_1(object sender, EventArgs e){this.Close();}
        public frmGuide(){InitializeComponent();}
        private void frmGuide_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void chkIsEmp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsEmp.Checked)
                ckeck_status(true);
            else
                ckeck_status(false);
        }
        private void ckeck_status(Boolean status)
        {
            try
            {
                Fill_Control();
                if (status)
                {
                    drpEmpID.Enabled = true;
                    btnUploadLogo.Enabled = false;
                    btnClearLogo.Enabled = false;
                    txtCompany.Enabled = false;
                    txtName.Enabled = false;
                    txtNIC.Enabled = false;
                    dtpDateEngage.Enabled = false;
                    dtpDateDisc.Enabled = false;
                    txtAddress.Enabled = false;
                    txtTel1.Enabled = false;
                    txtTel2.Enabled = false;
                    txtSpouseName.Enabled = false;
                    txtSpouseNo.Enabled = false;
                }
                else
                {
                    drpEmpID.Enabled = false;
                    btnUploadLogo.Enabled = true;
                    btnClearLogo.Enabled = true;
                    txtCompany.Enabled = true;
                    txtName.Enabled = true;
                    txtNIC.Enabled = true;
                    dtpDateEngage.Enabled = true;
                    dtpDateDisc.Enabled = true;
                    txtAddress.Enabled = true;
                    txtTel1.Enabled = true;
                    txtTel2.Enabled = true;
                    txtSpouseName.Enabled = true;
                    txtSpouseNo.Enabled = true;
                }
                pbImage.Image = null;
                lblImage.Visible = true;
                imageData = null;
                txtCompany.Text = "";
                txtName.Text = "";
                txtNIC.Text = "";
                dtpDateEngage.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                dtpDateDisc.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                txtAddress.Text = "";
                txtTel1.Text = "";
                txtTel2.Text = "";
                txtSpouseName.Text = "";
                txtSpouseNo.Text = "";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Driver Photo";
            fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (DialogResult.OK == fdLogo.ShowDialog())
            {
                string imageLocation = fdLogo.FileName;
                pbImage.ImageLocation = imageLocation;
                lblImage.Visible = false;
                imageData = null;
                FileInfo fileInfo = new FileInfo(imageLocation);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
            }
        }
        private void btnClearLogo_Click(object sender, EventArgs e)
        {
            pbImage.Image = null;
            lblImage.Visible = true;
            imageData = null;
        }
        private void drpEmpID_Selected_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable DT;
                if (drpEmpID.SelectedValue.ToString() == null || drpEmpID.SelectedValue.ToString() == "")
                    return;
                if (chkIsEmp.Checked)
                {
                    SqlQry = "SELECT CompanyName FROM mst_CompanyGenaral";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                    if (DT.Rows[0][0].ToString() != "")
                    {
                        txtCompany.Text = DT.Rows[0]["CompanyName"].ToString();
                    }
                }
                else if (chkIsDriver.Checked)
                {
                    SqlQry = "SELECT ID,OwnerName FROM vw_TR_Driver WHERE ID=" + drpEmpID.SelectedValue.ToString().Trim()+ "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                    if (DT.Rows[0][0].ToString() != "")
                    {
                        txtCompany.Text = DT.Rows[0]["OwnerName"].ToString();
                    }
                }
                if (chkIsEmp.Checked)
                {
                    SqlQry = " SELECT PS.ID,NameWithIntials,EmpPhoto,IdentityNo," +
                               "PermanantAdd,TelHome,TelMobile,ContName,ContTel1," +
                               "EngageDate,DischargeDate" +
                               " FROM mst_EmployeePersonal PS,mst_EmployeeContact CN,mst_EmployeeServiceDetails SV " +
                               "Where PS.ID=" + drpEmpID.SelectedValue.Trim() + " AND PS.ID=SV.EmpID AND PS.ID=CN.ID";
                }
                else if (chkIsDriver.Checked)
                {
                    SqlQry = " SELECT ID,Name AS NameWithIntials,EmpPhoto,IdentityNo," +
                               "PermanantAdd,TelHome,TelMobile,ContName,ContTel1," +
                               "EngageDate,DischargeDate" +
                               " FROM vw_TR_Driver"+
                               " Where ID=" + drpEmpID.SelectedValue.Trim() + "";
                }
                else
                    return;
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                txtName.Text = DT.Rows[0]["NameWithIntials"].ToString();
                txtNIC.Text = DT.Rows[0]["IdentityNo"].ToString();
                if(DT.Rows[0]["EngageDate"].ToString()!="")
                    dtpDateEngage.Value = System.Convert.ToDateTime(DT.Rows[0]["EngageDate"].ToString());
                if (DT.Rows[0]["DischargeDate"].ToString() != "")
                dtpDateDisc.Value = System.Convert.ToDateTime(DT.Rows[0]["DischargeDate"].ToString());
                txtAddress.Text = DT.Rows[0]["PermanantAdd"].ToString();
                txtTel1.Text = DT.Rows[0]["TelHome"].ToString();
                txtTel2.Text = DT.Rows[0]["TelMobile"].ToString();
                txtSpouseName.Text = DT.Rows[0]["ContName"].ToString();
                txtSpouseNo.Text = DT.Rows[0]["ContTel1"].ToString();
                if (DT.Rows[0]["EmpPhoto"] != DBNull.Value)
                {
                    byte[] Photo = (byte[])DT.Rows[0]["EmpPhoto"];
                    imageData = Photo;
                    MemoryStream ms = new MemoryStream(Photo);
                    pbImage.Image = Image.FromStream(ms, false, false);
                    lblImage.Visible = false;
                }
                else
                    lblImage.Visible = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void grdPhoto_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            if (e.Col == grdPhoto.Cols[(int)PH.gBRW].Index)
            {
                OpenFileDialog fdLogo = new OpenFileDialog();
                fdLogo.Title = "Choose a Photo";
                fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (DialogResult.OK == fdLogo.ShowDialog())
                {
                    string imageLocation = fdLogo.FileName;
                    imageData = null;
                    FileInfo fileInfo = new FileInfo(imageLocation);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);
                    grdPhoto[grdPhoto.Row, (int)PH.gIMG] = imageData;
                    update_tree_view();
                }
                return;
            }
        }
        private void grdPhoto_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)
            {
                return;
            }
            grdPhoto.Rows[1].AllowEditing = true;
            if (grdPhoto.Rows.Count < 3)
            {
                return;
            }
            if (grdPhoto[grdPhoto.Row - 1, 0] == null)
            {
                grdPhoto.Rows[grdPhoto.Row].AllowEditing = false;
            }
            else
            {
                grdPhoto.Rows[grdPhoto.Row].AllowEditing = true;
            }
        }
        private void grdPhoto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdPhoto.Rows.Remove(grdPhoto.Row);
                grdPhoto.Rows[1].AllowEditing = true;
            }
        }
        public void update_tree_view()
        {
            try
            {
                tvPhotoDesc.Nodes.Clear();
                RowNumb = 1;
                string TreeName = "";
                if (txtCode.Text.ToString() != "")
                    TreeName = txtCode.Text.Trim();
                if (RowNumb == 1)
                {
                    pbMultiPhotos.Image = null;
                    byte[] Photo = (byte[])grdPhoto[1, (int)PH.gIMG];
                    imageData = Photo;
                    if (imageData != null)
                    {
                        MemoryStream ms = new MemoryStream(Photo);
                        pbMultiPhotos.Image = Image.FromStream(ms, false, false);
                    }
                }
                TreeNode trNode = new TreeNode(TreeName);
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return;
                }
                while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
                {
                    if (grdPhoto[RowNumb, (int)PH.gDES] != null && grdPhoto[RowNumb, (int)PH.gDES].ToString() != "")
                    {
                        TreeNode trn = new TreeNode(grdPhoto[RowNumb, (int)PH.gDES].ToString());
                        trn.Name = RowNumb.ToString();
                        trNode.Nodes.Add(trn);
                    }
                    RowNumb++;
                }
                tvPhotoDesc.Nodes.Add(trNode);
                trNode.Expand();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void tvPhotoDesc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name.ToString() != "")
            {
                pbMultiPhotos.Image = null;
                int NodeID = Convert.ToInt16(e.Node.Name.ToString());
                byte[] Photo = (byte[])grdPhoto[NodeID, (int)PH.gIMG];
                imageData = Photo;
                MemoryStream ms = new MemoryStream(Photo);
                pbMultiPhotos.Image = Image.FromStream(ms, false, false);
            }
        }
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    chkActive.Checked = true;
                    lblDischarge.Visible = false;
                    dtpDateDisc.Visible = false;
                }
                else
                {
                    chkActive.Checked = false;
                    lblDischarge.Visible = true;
                    dtpDateDisc.Visible = true;
                }
            }
            if (chkActive.Checked == true)
            {
                lblDischarge.Visible = false;
                dtpDateDisc.Visible = false;
            }
        }
        private void drpEmpID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmEmployee", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void chkIsDriver_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsDriver.Checked)
                ckeck_status(true);
            else
                ckeck_status(false);
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string s = txtName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s,errorProvider1,txtName);
        }
        private void txtName_Leave(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}
