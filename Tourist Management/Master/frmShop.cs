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
    public partial class frmShop : Form
    {
        private const string msghd = "Shop Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Name AS [Shop Name],Rate,Discount,IsActive From vw_ShopDetails Where Isnull([Status],0)<>7 Order By Name";
        enum CG { gCID, gCNM, gVAL }
        byte[] imageData = null;  //TO KEEP BANK LOGO IMAGE AS A BINARY DATA
        int RowNumb;
        Boolean bLoad = false;
        private void Intializer()
        {
            Fill_Control();
            Grd_Initializer();
            if (Mode == 0)
            {
                Generate_Shop_Code();
            }
            else
            {
                Fill_Details();
            }
        }
        private void Generate_Shop_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_ShopDetails";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            if (DT.Rows[0]["ID"].ToString() != "")
                txtCode.Text = "SDT" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
            else
                txtCode.Text = "SDT1001";
        }
        private void Fill_Control()
        {
            try
            {
                DataTable[] DTB;
                DTB = new DataTable[1];
                DTB[0] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Percentage FROM mst_Percentage Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpDiscount.DataSource = DTB[0];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                grdCities.Cols.Count = 3;
                grdCities.Rows.Count = 100;
                grdCities.Cols[(int)CG.gCID].Width = 0;
                grdCities.Cols[(int)CG.gCNM].Width = 288;
                grdCities.Cols[(int)CG.gVAL].Width = 287;
                grdCities.Cols[(int)CG.gCID].Caption = "City ID";
                grdCities.Cols[(int)CG.gCNM].Caption = "City Name";
                grdCities.Cols[(int)CG.gVAL].Caption = "Validity";
                grdCities.Cols[(int)CG.gCNM].ComboList = "...";
                grdCities.Cols[(int)CG.gVAL].DataType = Type.GetType("System.Boolean");
                grdCities.Rows[1].AllowEditing = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public frmShop(){InitializeComponent();}
        private void frmShop_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Fill_Details()
        {
            try
            {
                DataTable DT;
                string ssql;
                ssql = " SELECT ID,Code,Logo,Rate,Name,RegNo,Address,Fax,Web,Email,ContPerson,Tel1,Tel2,Tel3,"+
                       " PercentageID,Description,Isnull(IsActive,0) AS IsActive " +
                        "FROM mst_ShopDetails " +
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
                    if (DT.Rows[0]["Code"].ToString() != null && DT.Rows[0]["Code"].ToString() != "")
                        txtCode.Text = DT.Rows[0]["Code"].ToString();
                    if (DT.Rows[0]["Rate"].ToString() != null && DT.Rows[0]["Rate"].ToString() != "")
                        srcRate.SelectedStar = Convert.ToInt16(DT.Rows[0]["Rate"]);
                    if (DT.Rows[0]["PercentageID"].ToString() != null && DT.Rows[0]["PercentageID"].ToString() != "")
                        drpDiscount.setSelectedValue(DT.Rows[0]["PercentageID"].ToString());
                    if (DT.Rows[0]["Name"].ToString() != null && DT.Rows[0]["Name"].ToString() != "")
                        txtName.Text = DT.Rows[0]["Name"].ToString();
                    if (DT.Rows[0]["RegNo"].ToString() != null && DT.Rows[0]["RegNo"].ToString() != "")
                        txtRegNo.Text = DT.Rows[0]["RegNo"].ToString();
                    if (DT.Rows[0]["Address"].ToString() != null && DT.Rows[0]["Address"].ToString() != "")
                        txtAddress.Text = DT.Rows[0]["Address"].ToString();
                    if (DT.Rows[0]["Web"].ToString() != null && DT.Rows[0]["Web"].ToString() != "")
                        txtWeb.Text = DT.Rows[0]["Web"].ToString();
                    if (DT.Rows[0]["Fax"].ToString() != null && DT.Rows[0]["Fax"].ToString() != "")
                        txtFax.Text = DT.Rows[0]["Fax"].ToString();
                    if (DT.Rows[0]["Email"].ToString() != null && DT.Rows[0]["Email"].ToString() != "")
                        txtEmail.Text = DT.Rows[0]["Email"].ToString();
                    if (DT.Rows[0]["ContPerson"].ToString() != null && DT.Rows[0]["ContPerson"].ToString() != "")
                        txtContPerson.Text = DT.Rows[0]["ContPerson"].ToString();
                    if (DT.Rows[0]["Tel1"].ToString() != null && DT.Rows[0]["Tel1"].ToString() != "")
                        txtTel1.Text = DT.Rows[0]["Tel1"].ToString();
                    if (DT.Rows[0]["Tel2"].ToString() != null && DT.Rows[0]["Tel2"].ToString() != "")
                        txtTel2.Text = DT.Rows[0]["Tel2"].ToString();
                    if (DT.Rows[0]["Tel3"].ToString() != null && DT.Rows[0]["Tel3"].ToString() != "")
                        txtTel3.Text = DT.Rows[0]["Tel3"].ToString();
                    if (DT.Rows[0]["Description"].ToString() != null && DT.Rows[0]["Description"].ToString() != "")
                        txtDescription.Text = DT.Rows[0]["Description"].ToString();
                    chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                    if (DT.Rows[0]["Logo"] != DBNull.Value)
                    {
                        byte[] Photo = (byte[])DT.Rows[0]["Logo"];
                        imageData = Photo;
                        MemoryStream ms = new MemoryStream(Photo);
                        pbImage.Image = Image.FromStream(ms, false, false);
                        lblImage.Visible = false;
                    }
                    else
                        lblImage.Visible = true;
                    ssql = "SELECT CityID,City,IsValid" +
                           " FROM vw_ShopCities WHERE ShopID=" + SystemCode + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            grdCities[RowNumb + 1, (int)CG.gCID] = Convert.ToInt32(DT.Rows[RowNumb]["CityID"].ToString());
                            grdCities[RowNumb + 1, (int)CG.gCNM] = DT.Rows[RowNumb]["City"].ToString();
                            grdCities[RowNumb + 1, (int)CG.gVAL] = Convert.ToBoolean(DT.Rows[RowNumb]["IsValid"].ToString());
                            RowNumb++;
                        }
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Shop Code Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Shop Name Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_ShopDetails Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Shop Code Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Name From mst_ShopDetails Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Shop Name Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
        }
        private Boolean Save_Pro()
        { 
                if (Validate_Data() == false)
                {
                    return false;
                }
                if (Save_Procedure() == false)
                {
                    return false;
                }
                return true;
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
                if (Save_Shop_Details(objCom) == true && Save_Shop_Cities(objCom) == true)
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
        private Boolean Save_Shop_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Shop_Details";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@Code", SqlDbType.NVarChar,50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Rate", SqlDbType.Int).Value = srcRate.SelectedStar;
                sqlCom.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = txtName.Text.Trim();
                if (drpDiscount.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@PercentageID", SqlDbType.Int).Value =Convert.ToInt16(drpDiscount.SelectedValue.Trim());
                if(txtRegNo.Text.ToString()!="" )
                    sqlCom.Parameters.Add("@RegNo", SqlDbType.NVarChar, 50).Value = txtRegNo.Text.Trim();
                if (txtAddress.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = txtAddress.Text.Trim();
                if (txtFax.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Fax", SqlDbType.NVarChar, 100).Value = txtFax.Text.Trim();
                if (txtWeb.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Web", SqlDbType.NVarChar, 100).Value = txtWeb.Text.Trim();
                if (txtEmail.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = txtEmail.Text.Trim();
                if (txtTel1.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Tel1", SqlDbType.NVarChar, 100).Value = txtTel1.Text.Trim();
                if (txtTel2.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Tel2", SqlDbType.NVarChar, 100).Value = txtTel2.Text.Trim();
                if (txtTel3.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Tel3", SqlDbType.NVarChar, 100).Value = txtTel3.Text.Trim();
                if (txtContPerson.Text.ToString() != "")
                    sqlCom.Parameters.Add("@ContPerson", SqlDbType.NVarChar, 100).Value = txtContPerson.Text.Trim();
                    if (imageData != null)
                        sqlCom.Parameters.Add("@Logo", SqlDbType.Image).Value = imageData;
                sqlCom.Parameters.Add("@Description", SqlDbType.NVarChar,1000).Value = txtDescription.Text.Trim();
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
        private Boolean Save_Shop_Cities(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_ShopCities";
                while (grdCities[RowNumb, grdCities.Cols[(int)CG.gCID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ShopID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@CityID", SqlDbType.Int).Value = Convert.ToInt32(grdCities[RowNumb, (int)CG.gCID].ToString());
                    sqlCom.Parameters.Add("@IsValid", SqlDbType.Int).Value = Convert.ToBoolean(grdCities[RowNumb, (int)CG.gVAL]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    RowNumb++;
                    if (RowNumb >= grdCities.Rows.Count)
                        break;
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
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Close This Window !!", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){this.Close();}
            else
                return;
        }
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Shop Logo";
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
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    chkActive.Checked = true;
                }
                else
                {
                    chkActive.Checked = false;
                }
            }
        }
        private void grdCities_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdCities.Rows.Remove(grdCities.Row);
                grdCities.Rows[1].AllowEditing = true;
            }
        }
        private void grdCities_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)
            {
                return;
            }
            grdCities.Rows[1].AllowEditing = true;
            if (grdCities.Rows.Count < 3)
            {
                return;
            }
            if (grdCities[grdCities.Row - 1, 0] == null)
            {
                grdCities.Rows[grdCities.Row].AllowEditing = false;
            }
            else
            {
                grdCities.Rows[grdCities.Row].AllowEditing = true;
            }
        }
        private void grdCities_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTCity;
                string SqlQuery;
                #region CITY 
                if (e.Col == grdCities.Cols[(int)CG.gCNM].Index)
                {
                    SqlQuery = "SELECT ID,City FROM mst_City Where IsNull(IsActive,0)=1";
                    DTCity = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTCity;
                        frm.SubForm = new Master.frmCity();
                        frm.Width = grdCities.Cols[(int)CG.gCNM].Width;
                        frm.Height = grdCities.Height;
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCities);
                        SelText = frm.Open_Search();
                        if (SelText != null)
                        {
                            grdCities[grdCities.Row, (int)CG.gCID] = SelText[0];
                            grdCities[grdCities.Row, (int)CG.gCNM] = SelText[1];
                        }                    
                }
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
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
