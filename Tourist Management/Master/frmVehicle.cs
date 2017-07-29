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
    public partial class frmVehicle : Form
    {
        private const string msghd = "Vehicle Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT VH.ID,VehicleNo,InsuranceNo ,Name AS DriverName,VH.IsActive From mst_Vehicle VH,mst_Driver DR Where VH.DriverID=DR.ID AND Isnull(VH.[Status],0)<>7 Order By VH.Code";
        byte[] imageData = null;  //TO KEEP BANK LOGO IMAGE AS A BINARY DATA
        int RowNumb;
        Boolean bLoad = false;
        enum PH { gNUM, gDES, gBRW, gIMG };
        enum AC { gAID, gANM, gEXS };
        public frmVehicle(){InitializeComponent();}
        private void Intializer()
        {
            Fill_Control();
            Grd_Initializer();
            Fill_AccessCategory_Grid();
            if (Mode != 0)
            {
                Fill_Details();
            }
        }
        private void Grd_Initializer()
        {
            try
            {
                grdPhoto.Cols.Count = 4;
                grdACC.Cols.Count = 3;
                grdPhoto.Rows.Count = 500;
                grdACC.Rows.Count = 500;
                grdACC.Cols[(int)AC.gAID].Width = 0;
                grdACC.Cols[(int)AC.gANM].Width = 447;
                grdACC.Cols[(int)AC.gEXS].Width = 100;
                grdACC.Cols[(int)AC.gAID].Caption = "Access Category ID";
                grdACC.Cols[(int)AC.gANM].Caption = "Access Category Name";
                grdACC.Cols[(int)AC.gEXS].Caption = "Existence";
                grdPhoto.Cols[(int)PH.gNUM].Width = 131;
                grdPhoto.Cols[(int)PH.gDES].Width = 330;
                grdPhoto.Cols[(int)PH.gBRW].Width = 70;
                grdPhoto.Cols[(int)PH.gIMG].Width = 0;
                grdPhoto.Cols[(int)PH.gNUM].Caption = "Serial No";
                grdPhoto.Cols[(int)PH.gDES].Caption = "Description";
                grdPhoto.Cols[(int)PH.gBRW].Caption = "Browse";
                grdPhoto.Cols[(int)PH.gIMG].Caption = "Image/Binary Data";
                grdPhoto.Cols[(int)PH.gBRW].ComboList = "...";
                grdPhoto.Rows[1].AllowEditing = true;
                grdACC.Cols[(int)AC.gEXS].DataType = Type.GetType("System.Boolean");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_AccessCategory_Grid()
        {
            DataTable DT;
            string Ssql;
            try
            {
                Ssql = "SELECT ID,Name FROM mst_AccessCategory WHERE IsActive=1 ORDER BY Name ";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(Ssql);
                grdACC.Rows.Count = DT.Rows.Count + 1;
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdACC[RowNumb + 1, (int)AC.gAID] = DT.Rows[RowNumb]["ID"].ToString();
                        grdACC[RowNumb + 1, (int)AC.gANM] = DT.Rows[RowNumb]["Name"].ToString();
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            { 
                drpTypeID.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_VehicleTypes Where IsNull(IsActive,0)=1 ORDER BY Code");
                drpStatusID.DataSource  = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_VehicleStatus Where IsNull(IsActive,0)=1 ORDER BY Code");
                drpDriverID.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM vwDriverVsEmployee Where IsNull(IsActive,0)=1 ORDER BY Name");
                cmbCurrency.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Currency FROM mst_Currency Where IsNull(IsActive,0)=1 ORDER BY ID"); 
                drpCity.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,City FROM mst_City Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                string ssql = "Select Code,VehicleNo,InsuranceNo,DriverID,MaxPassengers,MaxDistance,"+
                               "ChargersPerKm,CurrencyID,TypeID,StatusID,PurchDate,ManuDate,CityID,Remarks,Isnull(IsActive,0)as IsActive From mst_Vehicle Where ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                txtCode.Text = DT.Rows[0]["Code"].ToString();
                txtVehNo.Text = DT.Rows[0]["VehicleNo"].ToString();
                txtInsNo.Text = DT.Rows[0]["InsuranceNo"].ToString();
                nudMaxPas.Value = Convert.ToInt32(DT.Rows[0]["MaxPassengers"]);
                nudMaxDis.Value = Convert.ToInt32(DT.Rows[0]["MaxDistance"]);
                txtCharg.Text = DT.Rows[0]["ChargersPerKm"].ToString();
                cmbCurrency.SelectedValue = (DT.Rows[0]["CurrencyID"].ToString());
                drpTypeID.setSelectedValue(DT.Rows[0]["TypeID"].ToString());
                drpStatusID.setSelectedValue(DT.Rows[0]["StatusID"].ToString());
                drpDriverID.setSelectedValue(DT.Rows[0]["DriverID"].ToString());
                if(DT.Rows[0]["PurchDate"].ToString()!="")
                    dtpPurchDate.Value = Convert.ToDateTime(DT.Rows[0]["PurchDate"].ToString());
                if (DT.Rows[0]["ManuDate"].ToString() != "")
                    dtpManudate.Value = Convert.ToDateTime(DT.Rows[0]["ManuDate"].ToString());
                if (DT.Rows[0]["CityID"].ToString() != "")
                    drpCity.setSelectedValue(DT.Rows[0]["CityID"].ToString());
                txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                ssql = "SELECT VehicleID,AccessCatID,AccessCatName,Existence" +
                       " FROM vw_Vehicle_AccessCAtegories WHERE VehicleID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        grdACC[RowNumb + 1, (int)AC.gAID] = DT.Rows[RowNumb]["AccessCatID"].ToString();
                        grdACC[RowNumb + 1, (int)AC.gANM] = DT.Rows[RowNumb]["AccessCatName"].ToString();
                        grdACC[RowNumb + 1, (int)AC.gEXS] = Convert.ToBoolean(DT.Rows[RowNumb]["Existence"].ToString());
                        RowNumb++;
                    }
                }
                ssql = "SELECT VehicleID,SerialNo,Description,Image" +
                       " FROM mst_VehiclePhotos WHERE VehicleID=" + SystemCode + " ORDER BY SrNo ";
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
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Code cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtVehNo.Text.Trim() == "")
                {
                    MessageBox.Show("Vehicle No cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtInsNo.Text.Trim() == "")
                {
                    MessageBox.Show("Insurance cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtCharg.Text.Trim() == "")
                {
                    MessageBox.Show("Chargers cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_Vehicle Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select VehicleNo From mst_Vehicle Where VehicleNo='" + txtVehNo.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select InsuranceNo From mst_Vehicle Where InsuranceNo='" + txtInsNo.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (drpDriverID.SelectedValue.ToString()=="")
                {
                    MessageBox.Show("Driver Name Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select DriverID From mst_Vehicle Where DriverID='" + drpDriverID.SelectedValue.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Driver Is Already Assigned", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Validate_Vehicle_Photos() == false)
                    return false;
                return true;
        }
        private Boolean Validate_Vehicle_Photos()
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
                if (Save_Vehicle_Details(objCom) == true && Save_Vehicle_AccessCategories(objCom) == true && Save_Vehicle_Photos(objCom) == true)
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
        private Boolean Save_Vehicle_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Vehicle_Details";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@Code", SqlDbType.Char, 10).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@VehicleNo", SqlDbType.VarChar,50).Value = txtVehNo.Text.Trim();
                sqlCom.Parameters.Add("@InsuranceNo", SqlDbType.VarChar, 50).Value = txtInsNo.Text.Trim();
                sqlCom.Parameters.Add("@MaxPassengers", SqlDbType.Int).Value = nudMaxPas.Value;
                sqlCom.Parameters.Add("@MaxDistance", SqlDbType.Int).Value = nudMaxDis.Value;
                if (txtCharg.Text.Trim() != "")
                    sqlCom.Parameters.Add("@ChargersPerKm", SqlDbType.Decimal).Value = Convert.ToDecimal(txtCharg.Text.Trim());
                sqlCom.Parameters.Add("@CurrencyID", SqlDbType.Int).Value = Convert.ToInt16(cmbCurrency.SelectedValue.ToString().Trim());
                if (drpDriverID.SelectedValue.ToString() != "" && drpDriverID.SelectedValue.ToString() != null)
                    sqlCom.Parameters.Add("@DriverID", SqlDbType.Int).Value = drpDriverID.SelectedValue.Trim();
                if (drpTypeID.SelectedValue.ToString() != "" && drpTypeID.SelectedValue.ToString() != null)
                    sqlCom.Parameters.Add("@TypeID", SqlDbType.Int).Value = drpTypeID.SelectedValue.Trim();
                if (drpStatusID.SelectedValue.ToString() != "" && drpStatusID.SelectedValue.ToString() != null)
                    sqlCom.Parameters.Add("@StatusID", SqlDbType.Int).Value = drpStatusID.SelectedValue.Trim();
                sqlCom.Parameters.Add("@PurcDate", SqlDbType.DateTime).Value = dtpPurchDate.Value;
                sqlCom.Parameters.Add("@ManuDate", SqlDbType.DateTime).Value = dtpManudate.Value;
                if (drpCity.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@Location", SqlDbType.Int).Value = Convert.ToInt16(drpCity.SelectedValue.ToString());
                if (txtRemarks.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = txtRemarks.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked ? "1" : "0";
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
        private Boolean Save_Vehicle_AccessCategories(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Vehicle_AccessCategories";
                while (grdACC[RowNumb, grdACC.Cols[(int)AC.gAID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@VehicleID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@AccessCatID", SqlDbType.Int).Value =Convert.ToInt32(grdACC[RowNumb, (int)AC.gAID].ToString());
                    sqlCom.Parameters.Add("@Existence", SqlDbType.Int).Value = Convert.ToBoolean(grdACC[RowNumb, (int)AC.gEXS]);
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    RowNumb++;
                    if (RowNumb >= grdACC.Rows.Count)
                        break;
                }
                return true;
        }
        private Boolean Save_Vehicle_Photos(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return true;
                }
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Vehicle_Photos";
                while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@VehicleID", SqlDbType.Int).Value = SystemCode;
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
        private void frmVehicle_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true){this.Close();}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
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
                }
                else
                    return;
            }
        }
        private void drpTypeID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmVehicleType", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpStatusID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmVehicleStatus", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpDriverID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmDriver", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpCity_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCity", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
    }
}
