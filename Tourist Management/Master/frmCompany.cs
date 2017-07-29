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
    public partial class frmCompany : Form
    {
        private const string msghd = "Company Setup";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        Boolean bLoad = false;
        enum BD { gCOD, gNME, gBRC, gBRN, gACN, gANO, gSFC, gIDF, gALM };
        int BankID = 0,RowNumb = 1;
          string SqlQuery;
        byte[] imageData = null;  //TO KEEP COMPANY LOGO IMAGE AS A BINARY DATA
        public string SqlQry = "SELECT ID, CompanyCode, CompanyName,ContactPerson,Telephone,IsActive From mst_CompanyGenaral Where Isnull([Status],0)<>7 Order By ID";
        public frmCompany(){InitializeComponent();}
        private void Intializer()
        {
            Grd_Initializer();
            if (Mode != 0)
            {
                Fill_Data();
            }
        }
        private void frmCompany_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Grd_Initializer()
        {
            try
            {
                grdBNK.Cols.Count = 9;
                grdBNK.Rows.Count = 200;
                grdBNK.Cols[(int)BD.gCOD].Caption = "Bank Code";
                grdBNK.Cols[(int)BD.gNME].Caption = "Bank";
                grdBNK.Cols[(int)BD.gBRC].Caption = "Branch ID";
                grdBNK.Cols[(int)BD.gBRN].Caption = "Branch";
                grdBNK.Cols[(int)BD.gACN].Caption = "Account";
                grdBNK.Cols[(int)BD.gANO].Caption = "AccountNo";
                grdBNK.Cols[(int)BD.gSFC].Caption = "SWIFT Code";
                grdBNK.Cols[(int)BD.gALM].Caption = "AccountLimit";
                grdBNK.Cols[(int)BD.gIDF].Caption = "Identifier";
                grdBNK.Cols[(int)BD.gNME].ComboList = "...";
                grdBNK.Cols[(int)BD.gBRN].ComboList = "...";
                grdBNK.Rows[1].AllowEditing = true;
                grdBNK.Cols[(int)BD.gALM].DataType = Type.GetType("System.Double");
                grdBNK.Cols[(int)BD.gALM].Format = "0.00";
                grdBNK.Cols[(int)BD.gANO].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightBottom;
                grdBNK.Cols[(int)BD.gCOD].Width = 0;
                grdBNK.Cols[(int)BD.gNME].Width = 123;
                grdBNK.Cols[(int)BD.gBRC].Width = 0;
                grdBNK.Cols[(int)BD.gBRN].Width = 120;
                grdBNK.Cols[(int)BD.gACN].Width = 120;
                grdBNK.Cols[(int)BD.gANO].Width = 120;
                grdBNK.Cols[(int)BD.gSFC].Width = 108;
                grdBNK.Cols[(int)BD.gALM].Width = 0;
                grdBNK.Cols[(int)BD.gIDF].Width = 120;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
                DataTable DT, DTBank;
                string ssql; 
                ssql = " SELECT ID, CompanyCode, CompanyName, DisplayName, ContactPerson, Telephone, Mobile," +
                     " Fax, E_Mail, E_MailTo, Web, Physical_Address, Postal_Addres, Postal_Code1, Postal_Code2,Company_Logo" +
                     " FROM dbo.mst_CompanyGenaral WHERE ID="+SystemCode+"";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    Mode = 1;
                    SystemCode = (int)DT.Rows[0]["ID"];
                    txtComCode.Text = DT.Rows[0]["CompanyCode"].ToString();
                    txtComName.Text = DT.Rows[0]["CompanyName"].ToString();
                    txtDisName.Text = DT.Rows[0]["DisplayName"].ToString();
                    txtConperson.Text = DT.Rows[0]["ContactPerson"].ToString();
                    if(DT.Rows[0]["Telephone"].ToString()!="")
                        txtTel.Text = DT.Rows[0]["Telephone"].ToString();
                    if (DT.Rows[0]["Mobile"].ToString() != "")
                        txtMob.Text = DT.Rows[0]["Mobile"].ToString();
                    if (DT.Rows[0]["Fax"].ToString() != "")
                        txtFax.Text = DT.Rows[0]["Fax"].ToString();
                    if (DT.Rows[0]["E_Mail"].ToString() != "")
                        txtEmail.Text = DT.Rows[0]["E_Mail"].ToString();
                    if (DT.Rows[0]["E_MailTo"].ToString() != "")
                        txtEmailTo.Text = DT.Rows[0]["E_MailTo"].ToString();
                    if (DT.Rows[0]["Web"].ToString() != "")
                        txtWeb.Text = DT.Rows[0]["Web"].ToString();
                    if (DT.Rows[0]["Physical_Address"].ToString() != "")
                        txtPhysical.Text = DT.Rows[0]["Physical_Address"].ToString();
                    if (DT.Rows[0]["Postal_Addres"].ToString() != "")
                        txtPostal.Text = DT.Rows[0]["Postal_Addres"].ToString();
                    if (DT.Rows[0]["Postal_Code1"].ToString() != "")
                        txtPostal1.Text = DT.Rows[0]["Postal_Code1"].ToString();
                    if (DT.Rows[0]["Postal_Code2"].ToString() != "")
                        txtPostal2.Text = DT.Rows[0]["Postal_Code2"].ToString();
                    if (DT.Rows[0]["Company_Logo"] != DBNull.Value)
                    {
                        byte[] CompLogo = (byte[])DT.Rows[0]["Company_Logo"];
                        imageData = CompLogo;
                        MemoryStream ms = new MemoryStream(CompLogo);
                        pbCompLogo.Image = Image.FromStream(ms, false, false);
                        lblCompLogo.Visible = false;
                    }
                    else
                        lblCompLogo.Visible = true;
                    pbCompLogo.Refresh();
                    ssql = "SELECT BankID,BankName,BranchID,BranchName,Account,AccountNo,Swift,Identifier,Limit " +
                           "FROM vw_Company_Bank_Details WHERE CompanyID="+SystemCode+" ORDER BY SrNo";
                    DTBank = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows[0][0].ToString() != null && DT.Rows[0][0].ToString() != "")
                    {
                        RowNumb = 0;
                        while (DTBank.Rows.Count > RowNumb)
                        {
                            grdBNK[RowNumb + 1, (int)BD.gCOD] = DTBank.Rows[RowNumb]["BankID"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gNME] = DTBank.Rows[RowNumb]["BankName"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gBRC] = DTBank.Rows[RowNumb]["BranchID"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gBRN] = DTBank.Rows[RowNumb]["BranchName"].ToString();
                            if(DTBank.Rows[RowNumb]["BranchName"].ToString()!="")
                                grdBNK[RowNumb + 1, (int)BD.gACN] = DTBank.Rows[RowNumb]["Account"].ToString();
                            if (DTBank.Rows[RowNumb]["AccountNo"].ToString() != "")
                                grdBNK[RowNumb + 1, (int)BD.gANO] = DTBank.Rows[RowNumb]["AccountNo"].ToString();
                            if (DTBank.Rows[RowNumb]["Swift"].ToString() != "")
                                grdBNK[RowNumb + 1, (int)BD.gSFC] = DTBank.Rows[RowNumb]["Swift"].ToString();
                            if (DTBank.Rows[RowNumb]["Identifier"].ToString() != "")
                                grdBNK[RowNumb + 1, (int)BD.gIDF] = DTBank.Rows[RowNumb]["Identifier"].ToString();
                            RowNumb++;
                        }
                    }
                }
        }
        private void Fill_Control()
        {
        }
        private void grdBNK_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTBankName, DTBranchName;
            DTBankName = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,BankName FROM mst_BankMaster Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
            frm = new Tourist_Management.Other.frmSearchGrd();
            frm.DataSource = DTBankName;
            if (grdBNK[grdBNK.Row, grdBNK.Cols[(int)BD.gCOD].Index] != null)
                BankID = Convert.ToInt32(grdBNK[grdBNK.Row, (int)BD.gCOD].ToString());
            if (e.Col == grdBNK.Cols[(int)BD.gNME].Index)
            {
                frm.SubForm = new Master.frmBank();
                frm.Width = grdBNK.Cols[(int)BD.gNME].Width;
                frm.Height = grdBNK.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdBNK);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdBNK[grdBNK.Row, (int)BD.gCOD] = SelText[0];
                    grdBNK[grdBNK.Row, (int)BD.gNME] = SelText[1];
                    grdBNK[grdBNK.Row, (int)BD.gBRC] = null;
                    grdBNK[grdBNK.Row, (int)BD.gBRN] = null;
                }
            }
            SqlQuery = "SELECT ID,BranchName FROM mst_BankBranchMaster Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1 AND BankID= " + BankID + "";
            DTBranchName = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
            frm = new Tourist_Management.Other.frmSearchGrd();
            frm.DataSource = DTBranchName;
            if (e.Col == grdBNK.Cols[(int)BD.gBRN].Index)
            {
                if (grdBNK[grdBNK.Row, grdBNK.Cols[(int)BD.gCOD].Index] != null)
                {
                    frm.SubForm = new Master.frmBankBranch();
                    frm.Width = grdBNK.Cols[(int)BD.gBRN].Width;
                    frm.Height = grdBNK.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdBNK);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdBNK[grdBNK.Row, (int)BD.gBRC] = SelText[0];
                        grdBNK[grdBNK.Row, (int)BD.gBRN] = SelText[1];
                    }
                }
            }
        }
        private void grdBNK_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)
            {
                return;
            }
            grdBNK.Rows[1].AllowEditing = true;
            if (grdBNK.Rows.Count < 3)
            {
                return;
            }
            if (grdBNK[grdBNK.Row - 1, 0] == null)
            {
                grdBNK.Rows[grdBNK.Row].AllowEditing = false;
            }
            else
            {
                grdBNK.Rows[grdBNK.Row].AllowEditing = true;
            }
        }
        private void grdBNK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdBNK.Rows.Remove(grdBNK.Row);
                grdBNK.Rows[1].AllowEditing = true;
            }
        }
        private Boolean Validate_Company_Data()
        {
                if (txtComCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter the Company-Code", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtComCode.Select();
                    return false;
                }
                if (txtComName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter the Company-Name", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtComName.Select();
                    return false;
                }
                if (txtDisName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter the Company Dispaly Name", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtDisName.Select();
                    return false;
                }
                if (txtConperson.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter the Contact person", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtConperson.Select();
                    return false;
                }
                if (txtPhysical.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter the physical address", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtPhysical.Select();
                    return false;
                }
                return true;
        }
        private Boolean Validate_Bank_Details()
        {
                RowNumb = 1;
                if ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gNME].Index] == null) || (grdBNK[RowNumb, (int)BD.gNME].ToString() == ""))
                {
                    return true;
                }
                do
                {
                    if ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gNME].Index] == null) || (grdBNK[RowNumb, (int)BD.gNME].ToString() == ""))
                    {
                        MessageBox.Show("Please Select Bank Name ", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    else if ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gBRN].Index] == null) || (grdBNK[RowNumb, (int)BD.gBRN].ToString() == ""))
                    {
                        MessageBox.Show("Please Select Bank Branch Name ", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                } while ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gCOD].Index] != null));
                return true;
        }
        private Boolean Validate_Data()
        {
                if (Validate_Company_Data() == false)
                {
                    return false;
                }
                if (Validate_Bank_Details() == false)
                {
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
                if (Save_CompanyGEN(objCom) == true && Save_BankDtls(objCom) == true)
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
        private Boolean Save_CompanyGEN(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_CompanyGenaral";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@CompanyCode", SqlDbType.VarChar, 50).Value = txtComCode.Text.Trim();
                sqlCom.Parameters.Add("@CompanyName", SqlDbType.VarChar, 50).Value = txtComName.Text.Trim();
                sqlCom.Parameters.Add("@DisplayName", SqlDbType.VarChar, 50).Value = txtDisName.Text.Trim();
                sqlCom.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 50).Value = txtConperson.Text.Trim();
                if(txtTel.Text.ToString().Trim()!="")
                    sqlCom.Parameters.Add("Telephone", SqlDbType.VarChar, 50).Value = txtTel.Text.Trim();
                if (txtMob.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = txtMob.Text.Trim();
                if (txtFax.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = txtFax.Text.Trim();
                if (txtEmail.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@E_Mail", SqlDbType.VarChar, 50).Value = txtEmail.Text.Trim();
                if (txtEmailTo.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@E_MailTo", SqlDbType.VarChar, 50).Value = txtEmailTo.Text.Trim();
                if (txtWeb.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@Web", SqlDbType.VarChar, 50).Value = txtWeb.Text.Trim();
                sqlCom.Parameters.Add("@Physical_Address", SqlDbType.VarChar, 250).Value = txtPhysical.Text.Trim();
                if (txtPostal.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@Postal_Addres", SqlDbType.VarChar, 250).Value = txtPostal.Text.Trim();
                if (txtPostal1.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@Postal_Code1", SqlDbType.VarChar, 50).Value = txtPostal1.Text.Trim();
                if (txtPostal2.Text.ToString().Trim() != "")
                    sqlCom.Parameters.Add("@Postal_Code2", SqlDbType.VarChar, 50).Value = txtPostal2.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (imageData == null)
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = null;
                else
                    sqlCom.Parameters.Add("@Image", SqlDbType.Image).Value = imageData;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
        }
        private Boolean Save_BankDtls(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_CompanyBankDetails";
                RowNumb = 1;
                while (grdBNK[RowNumb, grdBNK.Cols[(int)BD.gCOD].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@BranchID", SqlDbType.Int).Value = Int32.Parse(grdBNK[RowNumb, (int)BD.gBRC].ToString());
                    if(grdBNK[RowNumb, (int)BD.gACN]!=null)
                        sqlCom.Parameters.Add("@Account", SqlDbType.VarChar, 50).Value = grdBNK[RowNumb, (int)BD.gACN].ToString();
                    if (grdBNK[RowNumb, (int)BD.gANO] != null)
                        sqlCom.Parameters.Add("@AccountNo", SqlDbType.VarChar, 50).Value = grdBNK[RowNumb, (int)BD.gANO].ToString();
                    if (grdBNK[RowNumb, (int)BD.gSFC] != null)
                        sqlCom.Parameters.Add("@Swift", SqlDbType.VarChar, 50).Value = grdBNK[RowNumb, (int)BD.gSFC].ToString();
                    if (grdBNK[RowNumb, (int)BD.gIDF] != null)
                        sqlCom.Parameters.Add("@Identifier", SqlDbType.NVarChar,100).Value = grdBNK[RowNumb, (int)BD.gIDF].ToString();
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                    }
                    RowNumb++;
                }
                return RtnVal;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                Fill_Data();
                this.Close();
            }
        }
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Company Logo";
            fdLogo.Filter = "Picture Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            if (DialogResult.OK == fdLogo.ShowDialog())
            {
                string imageLocation = fdLogo.FileName;
                pbCompLogo.ImageLocation = imageLocation;
                lblCompLogo.Visible = false;
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
            pbCompLogo.Image = null;
            lblCompLogo.Visible = true;
            imageData = null;
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void txtComName_TextChanged(object sender, EventArgs e)
        {
            string s = txtComName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s,errorProvider1,txtComName);
        }
        private void txtDisName_TextChanged(object sender, EventArgs e)
        {
            string s = txtDisName.Text.Trim();
            Tourist_Management.Classes.clsGlobal.FilterCharacters(s,errorProvider1,txtDisName);
        }
        private void txtComName_Leave(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
        private void txtDisName_Leave(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }
    }
}
