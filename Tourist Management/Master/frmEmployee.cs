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
    public partial class frmEmployee : Form
    {
        private const string msghd = "Employee Master";
        string SqlQuery;
        public string SqlQry = "SELECT ID,Code,FirstName,NameWithIntials,IdentityNo,Isnull(IsActive,0)AS IsActive From mst_EmployeePersonal Where Isnull([Status],0)<>7 Order By Code";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE 
        int RowNumb,BankID;
        byte[] imageData = null;  //TO KEEP EMPLOYEE IMAGE AS A BINARY DATA
        Boolean bLoad = false;
        enum BD { gCOD, gNME, gBRC, gBRN, gACN, gANO};
        public frmEmployee(){InitializeComponent();}
        private void frmEmployee_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                lblDischarge.Visible = false;
                dtpDischargeDate.Visible = false;
                Fill_Control();
                Grd_Initializer();
                if (Mode != 0)
                {
                    Fill_Data();
                }
                else
                    Generate_Employee_Code();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Generate_Employee_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_EmployeePersonal";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            txtCode.Text = "Emp" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();
        }
        private void Grd_Initializer()
        {
            try
            {
                grdBNK.Cols.Count = 6;
                grdBNK.Rows.Count = 200;
                grdBNK.Cols[(int)BD.gCOD].Width = 0;
                grdBNK.Cols[(int)BD.gNME].Width = 165;
                grdBNK.Cols[(int)BD.gBRC].Width = 0;
                grdBNK.Cols[(int)BD.gBRN].Width = 165;
                grdBNK.Cols[(int)BD.gACN].Width = 123;
                grdBNK.Cols[(int)BD.gANO].Width = 170;
                grdBNK.Cols[(int)BD.gCOD].Caption = "Bank Code";
                grdBNK.Cols[(int)BD.gNME].Caption = "Bank";
                grdBNK.Cols[(int)BD.gBRC].Caption = "Branch ID";
                grdBNK.Cols[(int)BD.gBRN].Caption = "Branch";
                grdBNK.Cols[(int)BD.gACN].Caption = "Account Name";
                grdBNK.Cols[(int)BD.gANO].Caption = "Account No";
                grdBNK.Cols[(int)BD.gNME].ComboList = "...";
                grdBNK.Cols[(int)BD.gBRN].ComboList = "...";
                grdBNK.Rows[1].AllowEditing = true;
                grdBNK.Cols[(int)BD.gANO].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightBottom;
            }
            catch (Exception ex){db.MsgERR(ex);}
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
            if (bLoad == true) return; 
            grdBNK.Rows[1].AllowEditing = true;
            if (grdBNK.Rows.Count < 3)   return; 
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
        private void Fill_Data()
        {
            try
            {
                DataTable DT;
                string ssql;
                ssql = " SELECT ID,Code,Mr_Mrs,FirstName,NameWithIntials,SurName,NickName,EmpPhoto," +
                        "IdentityTypeID,IdentityNo,CountryID,DOB,IsMale," +
                        "MaritalStatus,NoOfDependances,Remarks,Isnull(IsActive,0) AS IsActive " +
                        "FROM mst_EmployeePersonal " +
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
                    if (DT.Rows[0]["EmpPhoto"] != DBNull.Value)
                    {
                        byte[] EmpPhoto = (byte[])DT.Rows[0]["EmpPhoto"];
                        imageData = EmpPhoto;
                        MemoryStream ms = new MemoryStream(EmpPhoto);
                        pbEmpPhoto.Image = Image.FromStream(ms, false, false);
                        lblEmpPhoto.Visible = false;
                    }
                    else
                        lblEmpPhoto.Visible = true;
                    pbEmpPhoto.Refresh();
                    SystemCode = (int)DT.Rows[0]["ID"];
                    txtCode.Text = DT.Rows[0]["Code"].ToString();
                    cmbMr_Mrs.SelectedValue = (DT.Rows[0]["Mr_Mrs"].ToString());
                    txtFirstName.Text = DT.Rows[0]["FirstName"].ToString();
                    txtNameWithIntials.Text = DT.Rows[0]["NameWithIntials"].ToString();
                    txtSurName.Text = DT.Rows[0]["SurName"].ToString();
                    txtNickName.Text = DT.Rows[0]["NickName"].ToString();
                    cmbIdentityTypeID.SelectedValue = (DT.Rows[0]["IdentityTypeID"].ToString());
                    txtIdentityNo.Text = DT.Rows[0]["IdentityNo"].ToString();
                    drpCountryID.setSelectedValue(DT.Rows[0]["CountryID"].ToString());
                    if (DT.Rows[0]["DOB"].ToString() != null && DT.Rows[0]["DOB"].ToString() != "")
                        dtpDOB.Value = System.Convert.ToDateTime(DT.Rows[0]["DOB"].ToString());
                    if (DT.Rows[0]["IsMale"].ToString() != null && DT.Rows[0]["IsMale"].ToString() != "")
                    {
                        if (Convert.ToBoolean(DT.Rows[0]["IsMale"].ToString()))
                            rdbIsMale.Checked = true;
                        else
                            rdbIsFemale.Checked = true;
                    }
                    cmbMaritalStatus.SelectedValue = (DT.Rows[0]["MaritalStatus"].ToString());
                    nudNoOfDependances.Value = Convert.ToInt16(DT.Rows[0]["NoOfDependances"].ToString());
                    txtRemarks.Text = DT.Rows[0]["Remarks"].ToString();
                    if (Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString()))
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                    DataTable DT1;
                    string ssql1;
                    ssql1 = "SELECT PermanantAdd,PostalCode1,City1,PMACountryID," +
                            "PostalAdd,PostalCode2,City2,POACountryID," +
                            "TelHome,TelMobile,FaxNo,Email," +
                            "ContName,ContTel1,ContTel2,ContEmployer,ContEmail " +
                            "FROM mst_EmployeeContact WHERE ID=" + SystemCode + "";
                    DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql1);
                    if (DT1.Rows.Count > 0)
                    {
                        txtPermanantAdd.Text = DT1.Rows[0]["PermanantAdd"].ToString();
                        txtPostalCode1.Text = DT1.Rows[0]["PostalCode1"].ToString();
                        cmbCity1.Text = (DT1.Rows[0]["City1"].ToString());
                        drpPMACountryID.setSelectedValue(DT1.Rows[0]["PMACountryID"].ToString());
                        txtPostalAdd.Text = DT1.Rows[0]["PostalAdd"].ToString();
                        txtPostalCode2.Text = DT1.Rows[0]["PostalCode2"].ToString();
                        cmbCity2.Text = (DT1.Rows[0]["City2"].ToString());
                        drpPOACountryID.setSelectedValue(DT1.Rows[0]["POACountryID"].ToString());
                        txtTelHome.Text = DT1.Rows[0]["TelHome"].ToString();
                        txtTelMobile.Text = DT1.Rows[0]["TelMobile"].ToString();
                        txtFaxNo.Text = DT1.Rows[0]["FaxNo"].ToString();
                        txtEmail.Text = DT1.Rows[0]["Email"].ToString();
                        txtContName.Text = DT1.Rows[0]["ContName"].ToString();
                        txtContTel1.Text = DT1.Rows[0]["ContTel1"].ToString();
                        txtContTel2.Text = DT1.Rows[0]["ContTel2"].ToString();
                        txtContEmployer.Text = DT1.Rows[0]["ContEmployer"].ToString();
                        txtContEmail.Text = DT1.Rows[0]["ContEmail"].ToString();
                    }
                    ssql = "SELECT BankID,BankName,BranchID,BranchName,Account,AccountNo " +
                           "FROM vw_Employee_Bank_Details WHERE EmpID=" + SystemCode + " ORDER BY SrNo ";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            grdBNK[RowNumb + 1, (int)BD.gCOD] = DT.Rows[RowNumb]["BankID"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gNME] = DT.Rows[RowNumb]["BankName"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gBRC] = DT.Rows[RowNumb]["BranchID"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gBRN] = DT.Rows[RowNumb]["BranchName"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gACN] = DT.Rows[RowNumb]["Account"].ToString();
                            grdBNK[RowNumb + 1, (int)BD.gANO] = DT.Rows[RowNumb]["AccountNo"].ToString();
                            RowNumb++;
                        }
                    }
                    ssql = " SELECT EngageDate,DischargeDate,PrvEmployer,EmpStatusID,Designation,BasicSalary " +
                           "FROM mst_EmployeeServiceDetails" +
                           " Where EmpID=" + SystemCode + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        dtpEngageDate.Value = System.Convert.ToDateTime(DT.Rows[0]["EngageDate"].ToString());
                        if (DT.Rows[0]["DischargeDate"].ToString() != "" && DT.Rows[0]["DischargeDate"].ToString() != null)
                            dtpDischargeDate.Value = System.Convert.ToDateTime(DT.Rows[0]["DischargeDate"].ToString());
                        else
                            dtpDischargeDate.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                        if(DT.Rows[0]["EmpStatusID"].ToString()!="")
                            cmbStatus.SelectedValue = (DT.Rows[0]["EmpStatusID"].ToString());
                        txtDesignation.Text = DT.Rows[0]["Designation"].ToString();
                        txtPrvEmployer.Text = DT.Rows[0]["PrvEmployer"].ToString();
                        if (DT.Rows[0]["BasicSalary"].ToString() != "")
                            txtBasicSalary.Text = DT.Rows[0]["BasicSalary"].ToString();
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {
                DataTable[] DTB;
                DTB = new DataTable[9];
                dtpDOB.Value =   dtpEngageDate.Value =    dtpDischargeDate.Value = Tourist_Management.Classes.clsGlobal.CurDate(); 
                drpCountryID.DataSource = drpPMACountryID.DataSource = drpPOACountryID.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Country FROM mst_Country Where IsNull(IsActive,0)=1 ORDER BY ID");
                cmbMr_Mrs.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,Title FROM mst_Title Where IsNull(IsActive,0)=1 ORDER BY ID"); 
                 cmbMaritalStatus.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,MaritalStatus FROM mst_MaritalStatus Where IsNull(IsActive,0)=1 ORDER BY ID"); 
                cmbIdentityTypeID.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,IdentificationType FROM mst_IdentificationType Where IsNull(IsActive,0)=1 ORDER BY ID"); 
                DTB[5] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DISTINCT City1 as CityName FROM mst_EmployeeContact");
                cmbCity1.Items.Clear();
                foreach (DataRow dr in DTB[5].Rows)   cmbCity1.Items.Add(dr["CityName"].ToString()); 
                cmbCity1.AutoCompleteMode = ((System.Windows.Forms.AutoCompleteMode)((System.Windows.Forms.AutoCompleteMode.Suggest | System.Windows.Forms.AutoCompleteMode.Append)));
                cmbCity1.AutoCompleteSource = AutoCompleteSource.ListItems;
                DTB[6] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DISTINCT City2 as CityName FROM mst_EmployeeContact");
                cmbCity2.Items.Clear();
                foreach (DataRow dr in DTB[6].Rows) cmbCity2.Items.Add(dr["CityName"].ToString()); 
                cmbCity2.AutoCompleteMode = ((System.Windows.Forms.AutoCompleteMode)((System.Windows.Forms.AutoCompleteMode.Suggest | System.Windows.Forms.AutoCompleteMode.Append)));
                cmbCity2.AutoCompleteSource = AutoCompleteSource.ListItems; 
                  cmbStatus.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,EmpStatus FROM dbo.mst_EmployementStatus Where Isnull(Status,0)<>7 AND IsNull(IsActive,0)=1");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Validate_Data()
        {
                if (Validate_Personal_Details() == false)
                {
                    return false;
                }
                if (Validate_Service_Details() == false)
                {
                    return false;
                }
                if (Validate_Contact_Data() == false)
                {
                    return false;
                }
                if (Validate_Bank_Details() == false)
                {
                    return false;
                }
                return true;
        }
        private Boolean Validate_Personal_Details()
        {
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Employee-Code", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCode.Select();
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Code from dbo.mst_EmployeePersonal WHERE Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Employee Code is Already Exist.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtNameWithIntials.Text.Trim() == "")
                {
                    MessageBox.Show("Display Name Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtNameWithIntials.Select();
                    return false;
                }
                if (txtIdentityNo.Text.Trim() == "")
                {
                    MessageBox.Show("Invalid Identity Number Code Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtIdentityNo.Select();
                    return false;
                }
                if (Convert.ToBoolean(Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT NeedToCal from dbo.mst_IdentificationType  WHERE ID=" + Convert.ToInt16(cmbIdentityTypeID.SelectedValue.ToString().Trim()) + "").Rows[0]["NeedToCal"].ToString()))
                {
                    if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT IdentityNo from dbo.mst_EmployeePersonal  WHERE IdentityNo='" + txtIdentityNo.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                    {
                        MessageBox.Show("Identity No Is Already Exist", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                return true;
        }
        private Boolean Validate_Contact_Data()
        {
                if (txtPermanantAdd.Text.Trim() == "")
                {
                    MessageBox.Show("Permanant Address Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtPermanantAdd.Select();
                    return false;
                }
                if (txtTelMobile.Text.Trim() == "")
                {
                    MessageBox.Show("Mobile phone No Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtTelMobile.Select();
                    return false;
                }
                if (txtContName.Text.Trim() == "")
                {
                    MessageBox.Show("Contact/Spouse Name Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtContName.Select();
                    return false;
                }
                if (txtContTel1.Text.Trim() == "")
                {
                    MessageBox.Show("Contact/Spouse Telephone No1 Cannot Be Blank.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtContTel1.Select();
                    return false;
                }
                return true;
        }
        private Boolean Validate_Service_Details()
        {
                if (txtBasicSalary.Text != "")
                {
                    if (Tourist_Management.Classes.clsGlobal.IsNumeric(txtBasicSalary.Text.ToString()) == false)
                    {
                        MessageBox.Show("Please Enter Valid Amount for Basic Salary", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
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
                    else if ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gANO].Index] == null) || (grdBNK[RowNumb, (int)BD.gANO].ToString() == ""))
                    {
                        MessageBox.Show("Please Enter Account Number ", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                } while ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gCOD].Index] != null));
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
                if (Save_Employee_Personal_Details(objCom) == true && Save_Contact_Data(objCom) == true && Save_BankDtls(objCom) == true && Save_ServiceDtls(objCom) == true)
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
        private Boolean Save_Employee_Personal_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_EmpPersonal";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@Code", SqlDbType.VarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@Mr_Mrs", SqlDbType.Int).Value = Convert.ToInt16(cmbMr_Mrs.SelectedValue.ToString().Trim());
                if(txtFirstName.Text.ToString()!="")                
                    sqlCom.Parameters.Add("@FirstName", SqlDbType.VarChar, 150).Value = txtFirstName.Text.Trim();
                sqlCom.Parameters.Add("@NameWithIntials", SqlDbType.VarChar, 150).Value = txtNameWithIntials.Text.Trim();
                if (txtSurName.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@SurName", SqlDbType.VarChar, 150).Value = txtSurName.Text.Trim();
                if (txtNickName.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@NickName", SqlDbType.VarChar, 150).Value = txtNickName.Text.Trim();
                sqlCom.Parameters.Add("@IdentityTypeID", SqlDbType.Int).Value = Convert.ToInt16(cmbIdentityTypeID.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@IdentityNo", SqlDbType.VarChar, 50).Value = txtIdentityNo.Text.Trim();
                if (drpCountryID.SelectedValue.ToString() != "")  
                    sqlCom.Parameters.Add("@CountryID", SqlDbType.VarChar, 50).Value = drpCountryID.SelectedValue.Trim();
                sqlCom.Parameters.Add("@DOB", SqlDbType.DateTime).Value = dtpDOB.Value.Date;
                sqlCom.Parameters.Add("@IsMale", SqlDbType.Int).Value = rdbIsMale.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@MaritalStatus", SqlDbType.Int).Value = Convert.ToInt16(cmbMaritalStatus.SelectedValue.ToString().Trim());
                sqlCom.Parameters.Add("@NoOfDependances", SqlDbType.Int).Value = Convert.ToInt16(nudNoOfDependances.Value.ToString().Trim());
                if (txtRemarks.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@Remarks", SqlDbType.VarChar, 250).Value = txtRemarks.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked ? "1" : "0";
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (imageData == null)
                    sqlCom.Parameters.Add("@EmpPhoto", SqlDbType.Image).Value = null;
                else
                    sqlCom.Parameters.Add("@EmpPhoto", SqlDbType.Image).Value = imageData;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
                {
                    SystemCode = (int)sqlCom.Parameters["@ID"].Value;
                    RtnVal = true;
                }
                return RtnVal;
        }
        private Boolean Save_Contact_Data(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_EmpContact_Details";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@PermanantAdd", SqlDbType.VarChar, 250).Value = txtPermanantAdd.Text.Trim();
                if (txtPostalCode1.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@PostalCode1", SqlDbType.VarChar, 10).Value = txtPostalCode1.Text.Trim();
                if (cmbCity1.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@City1", SqlDbType.VarChar, 50).Value = cmbCity1.Text.Trim();
                if (drpPMACountryID.SelectedValue.ToString() != "")  
                    sqlCom.Parameters.Add("@PMACountryID", SqlDbType.Int).Value = drpPMACountryID.SelectedValue.Trim();
                if (txtPostalAdd.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@PostalAdd", SqlDbType.VarChar, 250).Value = txtPostalAdd.Text.Trim();
                if (txtPostalCode2.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@PostalCode2", SqlDbType.VarChar, 10).Value = txtPostalCode2.Text.Trim();
                if (cmbCity2.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@City2", SqlDbType.VarChar, 50).Value = cmbCity2.Text.Trim();
                if (drpPOACountryID.SelectedValue.ToString() != "")  
                    sqlCom.Parameters.Add("@POACountryID", SqlDbType.Int).Value = drpPOACountryID.SelectedValue.Trim();
                if (txtTelHome.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@TelHome", SqlDbType.VarChar, 50).Value = txtTelHome.Text.Trim();
                sqlCom.Parameters.Add("@TelMobile", SqlDbType.VarChar, 50).Value = txtTelMobile.Text.Trim();
                if (txtFaxNo.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@FaxNo", SqlDbType.VarChar, 50).Value = txtFaxNo.Text.Trim();
                if (txtEmail.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = txtEmail.Text.Trim();
                sqlCom.Parameters.Add("@ContName", SqlDbType.VarChar, 50).Value = txtContName.Text.Trim();
                sqlCom.Parameters.Add("@ContTel1", SqlDbType.VarChar, 50).Value = txtContTel1.Text.Trim();
                if (txtContTel2.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@ContTel2", SqlDbType.VarChar, 50).Value = txtContTel2.Text.Trim();
                if (txtContEmployer.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@ContEmployer", SqlDbType.VarChar, 50).Value = txtContEmployer.Text.Trim();
                if (txtContEmail.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@ContEmail", SqlDbType.VarChar, 50).Value = txtContEmail.Text.Trim();
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                {
                    return false;
                }
                RtnVal = true;
                return RtnVal;
        }
        private Boolean Save_BankDtls(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb=1;
                if ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gNME].Index] == null) || (grdBNK[RowNumb, (int)BD.gNME].ToString() == ""))
                {
                    return true;
                }
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_EmployeeBankDetails";
                while (grdBNK[RowNumb, grdBNK.Cols[(int)BD.gCOD].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@EmpID", SqlDbType.Int).Value = SystemCode;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@BranchID", SqlDbType.Int).Value = Int32.Parse(grdBNK[RowNumb, (int)BD.gBRC].ToString());
                    if ((grdBNK[RowNumb, grdBNK.Cols[(int)BD.gACN].Index] != null) && (grdBNK[RowNumb, (int)BD.gACN].ToString() != ""))
                        sqlCom.Parameters.Add("@Account", SqlDbType.VarChar, 50).Value = grdBNK[RowNumb, (int)BD.gACN].ToString();
                    sqlCom.Parameters.Add("@AccountNo", SqlDbType.VarChar, 50).Value = grdBNK[RowNumb, (int)BD.gANO].ToString();
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
        private Boolean Save_ServiceDtls(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_EmpService_Details";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@EngageDate", SqlDbType.DateTime).Value = dtpEngageDate.Value.Date;
                sqlCom.Parameters.Add("@DischargeDate", SqlDbType.DateTime).Value = dtpDischargeDate.Value.Date;
                if (txtPrvEmployer.Text.ToString() != "")  
                    sqlCom.Parameters.Add("@PrvEmployer", SqlDbType.VarChar, 100).Value = txtPrvEmployer.Text.Trim();
                sqlCom.Parameters.Add("@EmpStatusID", SqlDbType.Int).Value = cmbStatus.SelectedValue.ToString().Trim();
                if (txtDesignation.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Designation", SqlDbType.VarChar, 100).Value = txtDesignation.Text.Trim();
                if (txtBasicSalary.Text.ToString() != "")
                    sqlCom.Parameters.Add("@BasicSalary", SqlDbType.NVarChar, 20).Value = txtBasicSalary.Text.Trim();
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                {
                    return false;
                }
                RtnVal = true;
                return RtnVal;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
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
            pbEmpPhoto.Image = null;
            lblEmpPhoto.Visible = true;
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
                pbEmpPhoto.ImageLocation = imageLocation;
                lblEmpPhoto.Visible = false;
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
            dtpDOB.Value = Classes.clsGlobal.CurDate();
        }
        private void txtIdentityNo_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DataTable DTB;
                DTB = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,NeedToCal,IdentificationType FROM mst_IdentificationType Where IsNull(IsActive,0)=1 ORDER BY ID");
                foreach (DataRow DR in DTB.Rows)
                {
                    if (DR["IdentificationType"].ToString() == cmbIdentityTypeID.Text)
                    {
                        if (System.Convert.ToBoolean(DR["NeedToCal"]))
                        {
                            if (txtIdentityNo.Text.Length != 10 || !(Classes.clsGlobal.IsNumeric(txtIdentityNo.Text.Substring(0, txtIdentityNo.Text.Length - 1))))
                                MessageBox.Show("Invalid ID Number.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            else
                                Rtn_NICDetails(txtIdentityNo.Text);
                        }
                        else
                            dtpDOB.Value = Classes.clsGlobal.CurDate();
                        break;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Rtn_NICDetails(string strDte)
        {
            try
            {
                string[] NICdet;
                DateTime Caldate;
                DateTimePicker dtp;
                int YR = 0;
                int MN = 0;
                NICdet = new string[2];
                dtp = new DateTimePicker();
                if (Classes.clsGlobal.IsNumeric(strDte.Substring(0, 2)) == true)
                {
                    YR = System.Convert.ToInt16(strDte.Substring(0, 2));
                    YR = YR - 1;
                }
                else
                {
                    YR = System.Convert.ToInt16(System.DateTime.Today.Year);
                }
                if (Classes.clsGlobal.IsNumeric(strDte.Substring(2, 3)) == true)
                {
                    MN = System.Convert.ToInt16(strDte.Substring(2, 3));
                }
                else
                {
                    MN = System.Convert.ToInt16(System.DateTime.Today.Month);
                }
                dtp.Value = Classes.clsGlobal.CurDate();
                dtp.Text = "01/01/" + YR.ToString();
                Caldate = System.Convert.ToDateTime(dtp.Value.Year.ToString() + "/12/31");
                if (MN < 500)
                {
                    Caldate = Caldate.AddDays(MN);
                    NICdet[1] = "1";
                }
                else
                {
                    Caldate = Caldate.AddDays(MN - 500);
                    NICdet[1] = "0";
                }
                NICdet[0] = Caldate.ToString();
                dtpDOB.Value = System.Convert.ToDateTime(NICdet[0]);
                if (NICdet[1] == "1") { rdbIsMale.Checked = true; } else { rdbIsFemale.Checked = true; }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void drpCountryID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCountry", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpPMACountryID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCountry", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void drpPOACountryID_Click_Open(object sender, EventArgs e)
        {
            Form frm;
            frm = Classes.clsForms.rtnForm("frmCountry", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return;
        }
        private void chkActive_Click(object sender, EventArgs e)
        {
            if (chkActive.Checked == false)
            {
                if (MessageBox.Show("Do You Want To Inactive This Record.", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    chkActive.Checked = true;
                    lblDischarge.Visible = false;
                    dtpDischargeDate.Visible = false;
                }
                else
                {
                    chkActive.Checked = false;
                    lblDischarge.Visible = true;
                    dtpDischargeDate.Visible = true;
                }
            }
            if (chkActive.Checked == true)
            {
                lblDischarge.Visible = false;
                dtpDischargeDate.Visible = false;
            }
        }
    }
}
