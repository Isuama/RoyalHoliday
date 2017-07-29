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
    public partial class frmSightSeeing : Form
    {
        private const string msghd = "Sight Seeing Details";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Code as PlaceCode,Name as PlaceName,CategoryName,CityName,DistrictName,ProvinceName From vw_SightSeeing Where Isnull([Status],0)<>7 Order By Code";
        byte[] imageData = null;  //TO KEEP BANK LOGO IMAGE AS A BINARY DATA
        int RowNumb;
        Boolean bLoad = false;
        enum PH { gNUM, gDES, gBRW, gIMG };
        private void Intializer()
        {
            Fill_Control();
            Grd_Initializer();
            if (Mode == 0)
            {
                txtCode.Text = "";
                txtName.Text = "";
                txtRemarks.Text = "";
                chkActive.Checked = true;
                Generate_Category_Code();
            }
            else
            {
                Fill_Details();
            }
        }
        private void Generate_Category_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_SightSeeing";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQry);
            if (DT.Rows[0]["ID"].ToString()!="")
                txtCode.Text = "SSE" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
            else
                txtCode.Text = "SSE1001";
        }
        private void Grd_Initializer()
        {
            try
            {
                grdPhoto.Cols.Count = 4;
                grdPhoto.Rows.Count = 500;
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
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {
                DataTable[] DTB;
                DTB = new DataTable[5];
                DTB[0] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_Province ORDER BY ID");
                drpProvince.DataSource = DTB[0];
                DTB[1] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_District ORDER BY ID");
                drpDistrict.DataSource = DTB[1];
                DTB[2] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,City FROM mst_City ORDER BY ID");
                drpCity.DataSource = DTB[2];
                DTB[3] = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Name FROM mst_SightSeeingCat ORDER BY ID");
                drpSSCat.DataSource = DTB[3];
                DTB[4] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Currency FROM mst_Currency Where IsNull(IsActive,0)=1 ORDER BY ID");
                drpCurrency.DataSource = DTB[4];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Details()
        {
            DataTable DT;
            try
            {
                string ssql = "Select ID,CityID,DistrictID,ProvinceID,CatCode,Code,Name,Remarks," +
                              "Currency,SAdult,SChild,NAdult,NChild," +
                              "Isnull(IsActive,0)as IsActive From vw_SightSeeing Where ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
                if (DT.Rows[0]["ProvinceID"].ToString() != "")
                    drpProvince.setSelectedValue(DT.Rows[0]["ProvinceID"].ToString());
                if (DT.Rows[0]["DistrictID"].ToString() != "")
                    drpDistrict.setSelectedValue(DT.Rows[0]["DistrictID"].ToString());
                if (DT.Rows[0]["CityID"].ToString() != "")
                    drpCity.setSelectedValue(DT.Rows[0]["CityID"].ToString());
                if (DT.Rows[0]["CatCode"].ToString() != "")
                    drpSSCat.setSelectedValue(DT.Rows[0]["CatCode"].ToString());
                if (DT.Rows[0]["Code"].ToString() != "")
                    txtCode.Text = DT.Rows[0]["Code"].ToString();
                if (DT.Rows[0]["Name"].ToString() != "")
                    txtName.Text = DT.Rows[0]["Name"].ToString();
                if (DT.Rows[0]["Remarks"].ToString() != "")
                    txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                if (DT.Rows[0]["Currency"].ToString() != "")
                    drpCurrency.setSelectedValue(DT.Rows[0]["Currency"].ToString());
                if (DT.Rows[0]["SAdult"].ToString() != "")
                    txtSAdult.Text = DT.Rows[0]["SAdult"].ToString();
                if (DT.Rows[0]["SChild"].ToString() != "")
                    txtSChild.Text = DT.Rows[0]["SChild"].ToString();
                if (DT.Rows[0]["NAdult"].ToString() != "")
                    txtNAdult.Text = DT.Rows[0]["NAdult"].ToString();
                if (DT.Rows[0]["NChild"].ToString() != "")
                    txtNChild.Text = DT.Rows[0]["NChild"].ToString();
                chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                ssql = "SELECT SightSeenID,SerialNo,Description,Image" +
                       " FROM mst_SightSeenPhotos WHERE SightSeenID=" + SystemCode + " ORDER BY SrNo ";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
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
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Code From mst_SightSeeing Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Place Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select Name From mst_SightSeeing Where Name='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Place Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Validate_TicketCost() == false)
                    return false;
                if (Validate_SightSeen_Photos() == false)
                    return false;
                if(drpCurrency.SelectedValue.ToString()=="")
                {
                    MessageBox.Show("Currency Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
        }
        private Boolean Validate_TicketCost()
        {
            try
            {
                if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtSAdult.Text.ToString()) == false)
                {
                    MessageBox.Show("Please Enter Valid Values For SAARC Adult Ticket Cost", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtSChild.Text.ToString()) == false)
                {
                    MessageBox.Show("Please Enter Valid Values For SAARC Child Ticket Cost", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtNAdult.Text.ToString()) == false)
                {
                    MessageBox.Show("Please Enter Valid Values For Normal Adult Ticket Cost", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtNChild.Text.ToString()) == false)
                {
                    MessageBox.Show("Please Enter Valid Values For Normal Child Ticket Cost", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private Boolean Validate_SightSeen_Photos()
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
                objCon = Tourist_Management.Classes.clsGlobal.objComCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if (Save_SightSeen_Details(objCom) == true && Save_SightSeen_Photos(objCom) == true)
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
        private Boolean Save_SightSeen_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false; 
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_SightSeeing";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if (drpCity.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@CityID", SqlDbType.Int).Value = drpCity.SelectedValue.Trim();
                if (drpSSCat.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@CatCode", SqlDbType.Int).Value = drpSSCat.SelectedValue.Trim();
                if (txtCode.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Code", SqlDbType.Char, 10).Value = txtCode.Text.Trim();
                if (txtName.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Name", SqlDbType.VarChar,50).Value = txtName.Text.Trim();
                if (txtRemarks.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar,3000).Value = txtRemarks.Text.Trim();
                if (drpCurrency.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@Currency", SqlDbType.Int).Value = Convert.ToInt32(drpCurrency.SelectedValue.ToString());
                if (txtSAdult.Text.ToString() != "")
                    sqlCom.Parameters.Add("@SAdult", SqlDbType.Decimal).Value = txtSAdult.Text.Trim();
                if (txtSChild.Text.ToString() != "")
                    sqlCom.Parameters.Add("@SChild", SqlDbType.Decimal).Value = txtSChild.Text.Trim();
                if (txtNAdult.Text.ToString() != "")
                    sqlCom.Parameters.Add("@NAdult", SqlDbType.Decimal).Value = txtNAdult.Text.Trim();
                if (txtNChild.Text.ToString() != "")
                    sqlCom.Parameters.Add("@NChild", SqlDbType.Decimal).Value = txtNChild.Text.Trim();
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
        private Boolean Save_SightSeen_Photos(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1; 
                if ((grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] == null) || (grdPhoto[RowNumb, (int)PH.gNUM].ToString() == ""))
                {
                    return true;
                }
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_SightSeen_Photos";
                while (grdPhoto[RowNumb, grdPhoto.Cols[(int)PH.gNUM].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@SightSeenID", SqlDbType.Int).Value = SystemCode;
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
        public frmSightSeeing(){InitializeComponent();}
        private void frmSightSeeing_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
                if (Save_Pro() == true){this.Close();}
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void tpAddPhotos_Click(object sender, EventArgs e)
        {
            this.Width = 585;
            this.Height = 385;
        }
        private void tcSightSeeing_Click(object sender, EventArgs e)
        {
            if (tcSightSeeing.SelectedTab.Name == "tpSightDetails" || tcSightSeeing.SelectedTab.Name == "tpTicket")
            {
                this.Width = 384;
                this.Height = 385;
                tcSightSeeing.Width=363;
                tcSightSeeing.Height=298;
                chkActive.Location=new Point(134,316);
                btnOk.Location=new Point(205,313);
                btnCancel.Location = new Point(287, 313);
            }
            else
            {
                this.Width = 585;
                this.Height = 385;
                chkActive.Location = new Point(319, 316);
                btnOk.Location = new Point(390, 313);
                btnCancel.Location = new Point(472, 313);
                tcSightSeeing.Width = 565;
                tcSightSeeing.Height = 289;
            }
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
                ssql = "SELECT ID,City FROM mst_City WHERE DistrictID=" + drpDistrict.SelectedValue.Trim() + " ORDER BY ID";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
                drpCity.DataSource = DT;
            }
        }
        private void drpCity_Validating(object sender, CancelEventArgs e)
        {
            DataTable DT;
            string ssql; ;
            int DistID=0;
            if (drpCity.SelectedValue.ToString() != "")
            {
                ssql = "SELECT DistrictID FROM mst_City WHERE ID=" + drpCity.SelectedValue.Trim() + "";
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    DistID = Convert.ToInt32(DT.Rows[0]["DistrictID"].ToString());
                    drpDistrict.setSelectedValue(DistID.ToString());
                }
                ssql = "SELECT ProvinceID FROM mst_District WHERE ID=" + DistID + "";
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
        private void drpCity_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCity", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpSSCat_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmSightSeeingCat", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
    }
}
