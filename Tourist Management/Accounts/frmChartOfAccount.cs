using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Accounts
{
    public partial class frmChartOfAccount : Form
    {
        private const string msghd = "Chart of account";
        public string SqlQry = "SELECT ID,AccountType,OpenBal [Opening Balance],OpenBalDate [Opening Balance Date],Isnull(IsActive,0)AS IsActive From comAcc_AccountTypes Where Isnull([Status],0)<>7 Order By AccountType";
       public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
       public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        int selectedAccountType = 10; //'INCOME' ParentID IS THE DEFAULT ParentID
        bool formLoad = false;
        public frmChartOfAccount(){InitializeComponent();}
        private void btnCancel1_Click(object sender, EventArgs e){this.Close();}
        private void frmChartOfAccount_Load(object sender, EventArgs e)
        {
            Intializer();           
        }
        private void Intializer()
        {
            try
            {
                tc.SelectTab(0);
                set_Acc_Label(10);
                Fill_Control();
                if (Mode != 0)
                {
                   Fill_Data();
                }
                formLoad = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {
                string sql;
                sql = "SELECT ID,ISNULL(AllCompany,0)AllCompany,ISNULL(HasCompany,0)HasCompany,CompanyID,AccountTypeID,AccountType,Description,SubAccTypeID," +
                      "Note,ISNULL(HasOpenBal,0)HasOpenBal,OpenBal,OpenBalDate,ISNULL(IsActive,0)IsActive,"+
                      "AccountNo,AccountIdentifier" +
                      " FROM comAcc_AccountTypes WHERE ID=" + SystemCode + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    SystemCode = Convert.ToInt32(DT.Rows[0]["ID"]);
                    set_Acc_Label(SystemCode);
                    selectedAccountType = Convert.ToInt32(DT.Rows[0]["AccountTypeID"]);
                    Set_Account_Type();
                    Set_Selected_Account(Convert.ToInt32(DT.Rows[0]["AccountTypeID"]));
                    chkComp.Checked = false;
                    cmbCompany.Enabled = false;
                    chkAllComp.Checked = false;
                    if (Convert.ToBoolean(DT.Rows[0]["AllCompany"]))
                        chkAllComp.Checked = true;
                    if (Convert.ToBoolean(DT.Rows[0]["HasCompany"]))
                    {
                        chkComp.Checked = true;
                        cmbCompany.Enabled = true;
                        cmbCompany.SelectedValue = Convert.ToInt32(DT.Rows[0]["CompanyID"]);                        
                    }                    
                    txtAccName.Text = DT.Rows[0]["AccountType"] + "".Trim();
                    txtDescription.Text = DT.Rows[0]["Description"] + "".Trim();
                    txtNote.Text = DT.Rows[0]["Note"] + "".Trim();
                    txtAccNo.Text = DT.Rows[0]["AccountNo"] + "".Trim();
                    txtAccIden.Text = DT.Rows[0]["AccountIdentifier"] + "".Trim();
                    if (DT.Rows[0]["SubAccTypeID"] + "".Trim() != "")
                    {
                        chkSubAccOf.Checked = true;
                        drpSubOf.Enabled = true;
                        drpSubOf.setSelectedValue(DT.Rows[0]["SubAccTypeID"] + "".Trim());
                    }
                    if (DT.Rows[0]["OpenBal"] + "".Trim() != "")
                    {
                        gbOpenBal.Enabled = true;
                        txtAmount.Text = DT.Rows[0]["OpenBal"]+"".Trim();
                        dtpOB.Value = Convert.ToDateTime(DT.Rows[0]["OpenBalDate"]);
                    }
                    chkActive.Checked = Convert.ToBoolean(DT.Rows[0]["IsActive"]);
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Set_Selected_Account(int id)
        {
            try
            {
                switch (id)
                {
                    case 10: rdbIncome.Checked = true;
                        break;
                    case 12: rdbExpense.Checked = true;
                        break;
                    case 3: rdbFAsset.Checked = true;
                        break;
                    case 1: rdbBank.Checked = true;
                        break;
                    case 15: rdbLoan.Checked = true;
                        break;
                    case 6: rdbCCard.Checked = true;
                        break;
                    case 9: rdbEquity.Checked = true;
                        break;
                    default: rdbOther.Checked = true;
                             drpOtherAccType.Enabled = true;
                             drpOtherAccType.setSelectedValue(id.ToString().Trim());
                             break;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            { 
                drpOtherAccType.DataSource =Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,AccountType FROM comAcc_AccountTypes Where IsNull(IsActive,0)=1 AND ID NOT IN(10,12,3,1,15,6,9) ORDER BY AccountType");
                 drpAccType.DataSource =  drpSubOf.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,AccountType FROM comAcc_AccountTypes Where IsNull(IsActive,0)=1 ORDER BY AccountType");
                 cmbCompany.DataSource= Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnContinue_Click(object sender, EventArgs e)
        {
            tc.SelectTab(1);
            Set_Account_Type();
        }
        private void btnCancel2_Click(object sender, EventArgs e){this.Close();}      
        private void set_Acc_Label(int id)
        {
            try
            {
                string sql;
                sql = "SELECT AccountTypeID,AccountType,Description,ISNULL(HasOpenBal,0)HasOpenBal FROM comAcc_AccountTypes WHERE ID=" + id + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    selectedAccountType = Convert.ToInt32(DT.Rows[0]["AccountTypeID"]);
                    lblAccName.Text = DT.Rows[0]["AccountType"] + " Account";
                    lblDescription.Text = DT.Rows[0]["Description"] + "".Trim();
                    if (Convert.ToBoolean(DT.Rows[0]["HasOpenBal"]))
                        gbOpenBal.Enabled = true;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void rdbIncome_CheckedChanged(object sender, EventArgs e)
        {
            if(rdbIncome.Checked)
                set_Acc_Label(10);
        }
        private void rdbExpense_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbExpense.Checked)
                set_Acc_Label(12);
        }
        private void rdbFAsset_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFAsset.Checked)
                set_Acc_Label(3);
        }
        private void rdbBank_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBank.Checked)
            {
                set_Acc_Label(1);
                txtAccNo.ReadOnly = false;
                txtAccIden.ReadOnly = false;
                chkAllComp.Enabled = true;
                chkComp.Enabled = true;
            }
            else
            {
                txtAccNo.ReadOnly = true;
                txtAccIden.ReadOnly = true;
                chkAllComp.Enabled = false;
                chkComp.Enabled = false;
            }
        }
        private void rdbLoan_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbLoan.Checked)
                set_Acc_Label(15);
        }
        private void rdbCCard_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCCard.Checked)
                set_Acc_Label(6);
        }
        private void rdbEquity_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEquity.Checked)
                set_Acc_Label(9);
        }
        private void rdbOther_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbOther.Checked)
            {
                drpOtherAccType.setSelectedValue(null);
                drpOtherAccType.Enabled = true;
            }
            else
            {
                drpOtherAccType.setSelectedValue(null);
                drpOtherAccType.Enabled = false;
            }
        }
        private void chkSubAccOf_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!formLoad)
                    return;
                if (chkSubAccOf.Checked)
                {
                    drpSubOf.Enabled = true;
                    DataTable DTB;
                    DTB = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,AccountType FROM comAcc_AccountTypes Where IsNull(IsActive,0)=1 ORDER BY AccountType");
                    drpSubOf.DataSource = DTB;
                }
                else
                {
                    drpSubOf.setSelectedValue(null);
                    drpSubOf.Enabled = false;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Save_Pro()
        {
           try
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
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Validate_Data()
        {
            try
            {
                if (drpAccType.SelectedValue+"".Trim() == "")
                {
                    MessageBox.Show("Account type is required.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    drpAccType.Select();
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccountType from dbo.comAcc_AccountTypes WHERE AccountType='" + txtAccName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Account name is already exist.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtAccName.Select();
                    return false;
                }
                if (chkSubAccOf.Checked)
                {
                    if (drpSubOf.SelectedValue + "".Trim() == "")
                    {
                        MessageBox.Show("Sub Account type is required.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        drpSubOf.Select();
                        return false;
                    }
                }
                if (rdbBank.Checked)
                {
                    if (txtAccNo.Text.Trim() == "")
                    {
                        MessageBox.Show("Account Number is required.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtAccNo.Select();
                        return false;
                    }
                    if (txtAccIden.Text.Trim() == "")
                    {
                        MessageBox.Show("Account Identifier is required.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtAccIden.Select();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Procedure()
        {
            System.Data.SqlClient.SqlCommand objCom;
            System.Data.SqlClient.SqlTransaction objTrn;
            System.Data.SqlClient.SqlConnection objCon;
            try
            {
                objCom = new System.Data.SqlClient.SqlCommand();
                objCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                objCon.Open();
                objTrn = objCon.BeginTransaction();
                objCom.Connection = objCon;
                objCom.Transaction = objTrn;
                if (Save_Account_Details(objCom) == true)
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
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Account_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_ComAccountDetails";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@AccountTypeID", SqlDbType.Int).Value = Convert.ToInt32(drpAccType.SelectedValue);
                sqlCom.Parameters.Add("@AccountType", SqlDbType.NVarChar, 100).Value = txtAccName.Text;
                if(chkAllComp.Checked)
                    sqlCom.Parameters.Add("@AllCompany", SqlDbType.Int).Value = 1;
                if (chkComp.Checked)
                {
                    sqlCom.Parameters.Add("@HasCompany", SqlDbType.Int).Value = 1;
                    sqlCom.Parameters.Add("@CompanyID", SqlDbType.Int).Value = Convert.ToInt32(cmbCompany.SelectedValue);
                }
                if(chkSubAccOf.Checked)
                    sqlCom.Parameters.Add("@SubAccTypeID", SqlDbType.Int).Value = Convert.ToInt32(drpSubOf.SelectedValue);
                if (txtAmount.Text.Trim() != "")
                {
                    sqlCom.Parameters.Add("@OpenBal", SqlDbType.Decimal).Value = Convert.ToDecimal(txtAmount.Text);
                    sqlCom.Parameters.Add("@OpenBalDate", SqlDbType.DateTime).Value = dtpOB.Value;
                }
                if(rdbBank.Checked)
                {
                    sqlCom.Parameters.Add("@AccountIdentifier", SqlDbType.NVarChar, 100).Value = txtAccIden.Text;
                    sqlCom.Parameters.Add("@AccountNo", SqlDbType.NVarChar, 100).Value = txtAccNo.Text;
                }
                if (txtDescription.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = txtDescription.Text.Trim();
                if (txtNote.Text.ToString() != "")
                    sqlCom.Parameters.Add("@Note", SqlDbType.NVarChar, 1000).Value = txtNote.Text.Trim();
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
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true){this.Close();}
        }
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                Clear_All();
                SystemCode = 0;
                Mode = 0;
                Intializer();
            }
        }
        private void Clear_All()
        {
            try
            {
                tc.SelectTab(1);
                rdbIncome.Checked = true;
                drpOtherAccType.setSelectedValue(null);
                drpAccType.setSelectedValue(null);
                txtAccName.Text = "";
                chkSubAccOf.Checked = false;
                drpSubOf.setSelectedValue(null);
                txtAmount.Text = "";
                dtpOB.Value = Classes.clsGlobal.CurDate();
                txtDescription.Text = "";
                txtNote.Text = "";
                txtAccIden.Text = "";
                txtAccNo.Text = "";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void tc_Click(object sender, EventArgs e)
        {
            if (tc.SelectedTab.Name == "tp2")
            {
                Set_Account_Type();
            }
        }
        private void Set_Account_Type()
        {
            try
            {
                drpAccType.setSelectedValue(selectedAccountType.ToString().Trim());
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void drpOtherAccType_Selected_TextChanged(object sender, EventArgs e)
        {
            set_Acc_Label(Convert.ToInt32(drpOtherAccType.SelectedValue));
        }
        private void chkComp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkComp.Checked)
            {
                cmbCompany.Enabled = true;
                chkAllComp.Checked = false;
            }
            else
            {
                cmbCompany.Enabled = false;
            }
        }
        private void chkAllComp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllComp.Checked)
            {
                chkComp.Checked = false;
                cmbCompany.Enabled = false;
            }
        }
    }
}
