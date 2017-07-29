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
    public partial class frmAgent : Form
    {
        private const string msghd = "Agent Details";
        public string SqlQry = "SELECT ID,Code,Name,Tel1 AS Telephone,Address,Isnull(IsActive,0)AS IsActive From mst_AgentDetails Where Isnull([Status],0)<>7 Order By Code";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE 
        int RowNumb ;
        public static int CountryID = 0;
        public static decimal TourID = 0;
        byte[] imageData = null;  //TO KEEP COMPANY LOGO IMAGE AS A BINARY DATA
        enum CD { gCNM, gTEL, gMOB, gEML, gFax};
        enum RG { gCNT, gIDN, gTID, gGNM, gIAM, gRAM };
        public frmAgent(){InitializeComponent();}
        private void frmAgent_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                chkActive.Checked = true;
                lblDischarge.Visible = false;
                dtpDateDisc.Visible = false;
                Fill_Control();
                Grd_Initializer();
                if (Mode != 0)
                {
                    Fill_Data();
                }
                else
                    Generate_Agent_Code();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Generate_Agent_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_AgentDetails";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            txtCode.Text = "AG" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
        }
        private void Grd_Initializer()
        {
            try
            {
                #region CONTACT
                grdContacts.Cols.Count = 5;
                grdContacts.Rows.Count = 100;
                grdContacts.Cols[(int)CD.gCNM].Width = 200;
                grdContacts.Cols[(int)CD.gTEL].Width = 100;
                grdContacts.Cols[(int)CD.gMOB].Width = 100;
                grdContacts.Cols[(int)CD.gEML].Width = 100;
                grdContacts.Cols[(int)CD.gFax].Width = 97;
                grdContacts.Cols[(int)CD.gCNM].Caption = "Contact Name";
                grdContacts.Cols[(int)CD.gTEL].Caption = "Telephone";
                grdContacts.Cols[(int)CD.gMOB].Caption = "Mobile";
                grdContacts.Cols[(int)CD.gEML].Caption = "Email";
                grdContacts.Cols[(int)CD.gFax].Caption = "Fax";
                grdContacts.Rows[1].AllowEditing = true;
#endregion
                #region REFERENCE
                grdRef.Cols.Count = 6;
                grdRef.Rows.Count = 500;
                grdRef.Cols[(int)RG.gIDN].Width = 0;
                grdRef.Cols[(int)RG.gCNT].Width = 50;
                grdRef.Cols[(int)RG.gTID].Width = 94;
                grdRef.Cols[(int)RG.gGNM].Width = 244;
                grdRef.Cols[(int)RG.gIAM].Width = 100;
                grdRef.Cols[(int)RG.gRAM].Width = 113;
                grdRef.Cols[(int)RG.gIDN].Caption = "ID";
                grdRef.Cols[(int)RG.gCNT].Caption = "#";
                grdRef.Cols[(int)RG.gTID].Caption = "Tour ID";
                grdRef.Cols[(int)RG.gGNM].Caption = "Guest";
                grdRef.Cols[(int)RG.gIAM].Caption = "Invoice Amount";
                grdRef.Cols[(int)RG.gRAM].Caption = "Received Amount";
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {
                DataTable DT;
                string ssql;
                #region FILL AGENT DETAILS 
                ssql = " SELECT ID,Code,Name,Photo,DateEnagae,DateDischarge,IdentityTypeID,IdentityNo,"+
                       "ContName,CountryID,Address,Tel1,Tel2,Email,Fax,Remarks,Isnull(IsActive,0) AS IsActive " +
                        "FROM mst_AgentDetails " +
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
                    if (DT.Rows[0]["Photo"] != DBNull.Value)
                    {
                        byte[] Photo = (byte[])DT.Rows[0]["Photo"];
                        imageData = Photo;
                        MemoryStream ms = new MemoryStream(Photo);
                        pbImage.Image = Image.FromStream(ms, false, false);
                        lblImage.Visible = false;
                    }
                    else
                        lblImage.Visible = true;
                    pbImage.Refresh();
                    SystemCode = (int)DT.Rows[0]["ID"];
                    txtCompany.Text = DT.Rows[0]["Name"].ToString();
                    txtCode.Text = DT.Rows[0]["Code"].ToString();
                    if(DT.Rows[0]["DateEnagae"].ToString()!="")
                        dtpDateEngage.Value = System.Convert.ToDateTime(DT.Rows[0]["DateEnagae"].ToString());
                    if (DT.Rows[0]["DateDischarge"].ToString() != "")
                        dtpDateDisc.Value = System.Convert.ToDateTime(DT.Rows[0]["DateDischarge"].ToString());
                    cmbIdentityTypeID.SelectedValue = (DT.Rows[0]["IdentityTypeID"].ToString());
                    txtIdentityNo.Text = DT.Rows[0]["IdentityNo"].ToString();
                    if(DT.Rows[0]["CountryID"].ToString()!="" && DT.Rows[0]["CountryID"].ToString()!=null)
                        drpCountryID.setSelectedValue(DT.Rows[0]["CountryID"].ToString());
                    txtAddress.Text = DT.Rows[0]["Address"].ToString();
                    txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                    if (Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString()))
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                }
                #endregion
                #region FILL AGENT CONTACT DETAILS
                ssql = "SELECT ContactName,Telephone,Mobile,Email,Fax" +
                       " FROM mst_AgentContactsDetails WHERE AgentID="+SystemCode+" ORDER BY SrNo";
                DataTable DTCon = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows[0][0].ToString() != null && DT.Rows[0][0].ToString() != "")
                {
                    RowNumb = 0;
                    while (DTCon.Rows.Count > RowNumb)
                    {
                        grdContacts[RowNumb + 1, (int)CD.gCNM] = DTCon.Rows[RowNumb]["ContactName"].ToString();
                        grdContacts[RowNumb + 1, (int)CD.gTEL] = DTCon.Rows[RowNumb]["Telephone"].ToString();
                        grdContacts[RowNumb + 1, (int)CD.gMOB] = DTCon.Rows[RowNumb]["Mobile"].ToString();
                        grdContacts[RowNumb + 1, (int)CD.gEML] = DTCon.Rows[RowNumb]["Email"].ToString();
                        grdContacts[RowNumb + 1, (int)CD.gFax] = DTCon.Rows[RowNumb]["Fax"].ToString();
                        RowNumb++;
                    }
                }
                #endregion
                #region FILL AGENT REFERENCES
                ssql = "SELECT TransID,TourID,Guest,ISNULL(InvAmt,0)AS InvAmt,ISNULL(RecAmt,0)AS RecAmt" +
                       " FROM vw_acc_PnL_Basics WHERE AgentID=" + SystemCode + " ORDER BY TransID";
                DataTable DTRef = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                decimal invAmt=0.00m,recAmt=0.00m,totInvoice = 0.00m, totRecAmt=0.00m;
                if (DTRef.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DTRef.Rows.Count > RowNumb)
                    {
                        grdRef.Rows[RowNumb + 1].AllowEditing = false;
                        grdRef[RowNumb + 1, (int)RG.gCNT] = RowNumb + 1;
                        grdRef[RowNumb + 1, (int)RG.gIDN] = DTRef.Rows[RowNumb]["TransID"].ToString();
                        grdRef[RowNumb + 1, (int)RG.gTID] = DTRef.Rows[RowNumb]["TourID"].ToString();
                        grdRef[RowNumb + 1, (int)RG.gGNM] = DTRef.Rows[RowNumb]["Guest"].ToString();
                        invAmt = Convert.ToDecimal(DTRef.Rows[RowNumb]["InvAmt"]);
                        recAmt = Convert.ToDecimal(DTRef.Rows[RowNumb]["RecAmt"]);
                        grdRef[RowNumb + 1, (int)RG.gIAM]= invAmt;
                        grdRef[RowNumb + 1, (int)RG.gRAM] = recAmt;
                        totInvoice += invAmt;
                        totRecAmt += recAmt;
                       RowNumb++;
                    }
                    C1.Win.C1FlexGrid.CellStyle COM = grdRef.Styles.Add("TOT");
                    COM.BackColor = Color.Aqua;
                    grdRef.Rows[RowNumb + 1].Style = grdRef.Styles["TOT"];
                    grdRef[RowNumb + 1, (int)RG.gGNM] = "Total";
                    grdRef[RowNumb + 1, (int)RG.gIAM] = totInvoice;
                    grdRef[RowNumb + 1, (int)RG.gRAM] = totRecAmt;
                    grdRef.Rows.Count = RowNumb + 2;
                }
                else
                    grdRef.Rows.Count = 1;
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            { 
                drpCountryID.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Country FROM mst_Country Where IsNull(IsActive,0)=1 ORDER BY ID");
                cmbIdentityTypeID.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,IdentificationType FROM mst_IdentificationType Where IsNull(IsActive,0)=1 ORDER BY ID");
                if (CountryID != 0)
                {
                    drpCountryID.setSelectedValue(CountryID.ToString());
                    drpCountryID.Enabled = false;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                return Validate_Agent_Details() ;
        }
        private Boolean Validate_Agent_Details()
        {
                if (drpCountryID.SelectedValue+"".Trim() == "")
                {
                    MessageBox.Show("Please Select Agent Country", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                    
                    return false;
                }
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Agent-Code", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCode.Select();
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Code from dbo.mst_AgentDetails WHERE Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Agent Code is Already Exist.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtCompany.Text.Trim() == "")
                {
                    MessageBox.Show("Agent/Company Name Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCompany.Select();
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Name from dbo.mst_AgentDetails WHERE Name='" + txtCompany.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Agent Name is Already Exist.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                if (Save_Agent_Details(objCom) == true && Save_Agent_Contacts(objCom) == true)
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
        private Boolean Save_Agent_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Agent_Details";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = TourID;
                sqlCom.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = txtCompany.Text.Trim();
                sqlCom.Parameters.Add("@DateEnagae", SqlDbType.DateTime).Value = dtpDateEngage.Value.Date;
                sqlCom.Parameters.Add("@DateDischarge", SqlDbType.DateTime).Value = dtpDateDisc.Value.Date;
                sqlCom.Parameters.Add("@IdentityTypeID", SqlDbType.Int).Value = Convert.ToInt16(cmbIdentityTypeID.SelectedValue.ToString().Trim());
                if (txtIdentityNo.Text.ToString() != "")
                    sqlCom.Parameters.Add("@IdentityNo", SqlDbType.NVarChar, 100).Value = txtIdentityNo.Text.Trim();
                if (drpCountryID.SelectedValue.ToString() != "")
                    sqlCom.Parameters.Add("@CountryID", SqlDbType.Int).Value = drpCountryID.SelectedValue.Trim();
                if (txtAddress.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Address", SqlDbType.NVarChar, 1000).Value = txtAddress.Text.Trim();
                if (txtRemarks.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.NVarChar, 250).Value = txtRemarks.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (imageData == null)
                    sqlCom.Parameters.Add("@Photo", SqlDbType.Image).Value = null;
                else
                    sqlCom.Parameters.Add("@Photo", SqlDbType.Image).Value = imageData;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
        }
        private Boolean Save_Agent_Contacts(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_AgentContacts";
                RowNumb = 1;
                while (grdContacts[RowNumb, grdContacts.Cols[(int)CD.gCNM].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdContacts[RowNumb, (int)CD.gCNM] != null)
                        sqlCom.Parameters.Add("@ContactName", SqlDbType.NVarChar, 100).Value = grdContacts[RowNumb, (int)CD.gCNM].ToString();
                    if (grdContacts[RowNumb, (int)CD.gTEL] != null)
                        sqlCom.Parameters.Add("@Telephone", SqlDbType.NVarChar, 100).Value = grdContacts[RowNumb, (int)CD.gTEL].ToString();
                    if (grdContacts[RowNumb, (int)CD.gMOB] != null)
                        sqlCom.Parameters.Add("@Mobile", SqlDbType.NVarChar, 100).Value = grdContacts[RowNumb, (int)CD.gMOB].ToString();
                    if (grdContacts[RowNumb, (int)CD.gEML] != null)
                        sqlCom.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = grdContacts[RowNumb, (int)CD.gEML].ToString();
                    if (grdContacts[RowNumb, (int)CD.gFax] != null)
                        sqlCom.Parameters.Add("@Fax", SqlDbType.NVarChar, 100).Value = grdContacts[RowNumb, (int)CD.gFax].ToString();
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
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnClearLogo_Click(object sender, EventArgs e)
        {
            pbImage.Image = null;
            lblImage.Visible = true;
            imageData = null;
        }
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLogo = new OpenFileDialog();
            fdLogo.Title = "Choose a Company Logo";
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
        private void cmbIdentityTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblID.Text = cmbIdentityTypeID.Text + " No";
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
        private void drpCountryID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCountry", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
    }
}
