using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Other
{
    public partial class frmCustomizedReports : Form
    { 
    public    int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
     public   double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        private const string msghd = "General Query ";
        public string SqlQry = "SELECT ID,Name [Query Name],Isnull(IsActive,0)AS IsActive From [TouristManagementCommon].[dbo].[mst_GeneralQuery]";
        public frmCustomizedReports(){InitializeComponent();}
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true){this.Close();}
        }
        public  bool Save_Pro()
        {
            Boolean rtnVal = false;
                if (Save_Procedure() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
        private bool Save_Procedure()
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
                if (Save_GeneralQuery(objCom) == true)
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
        private bool Save_GeneralQuery(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "[TouristManagementCommon].dbo.spSave_GeneralQuery";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = txtName.Text.ToString();
                sqlCom.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = txtFilter.Text.ToString();
                sqlCom.Parameters.Add("@Query", SqlDbType.NVarChar, 2000).Value = txtQuery.Text.ToString();
                sqlCom.Parameters.Add("@CompID", SqlDbType.Int).Value = Convert.ToInt32(cmbCompany.SelectedValue.ToString());
                if (chkIsFilterByDate.Checked)
                {
                    sqlCom.Parameters.Add("@IsFilterByDate", SqlDbType.Bit).Value = true;
                    sqlCom.Parameters.Add("@DateCols", SqlDbType.NVarChar, 100).Value = txtDateCol1.Text.Trim() + "," + txtDateCol2.Text.Trim();
                }
                else
                {
                    sqlCom.Parameters.Add("@IsFilterByDate", SqlDbType.Bit).Value = false;
                }
                if(chkIsActive.Checked)
                    sqlCom.Parameters.Add("@IsActive", SqlDbType.Bit).Value = true ;
                else
                    sqlCom.Parameters.Add("@IsActive", SqlDbType.Bit).Value = false;
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        RtnVal = false;
                    }
                return RtnVal;
        }
        private void button1_Click(object sender, EventArgs e){this.Close();}
        private void frmCustomizedReports_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            Fill_Control();
            if (Mode == 0)
            {
                txtName.Text = "";
                txtName.Select();
                chkIsActive.Checked = true;
            }
            else
            {
                Fill_Data();
            }
        }
        private void Fill_Control()
        {
            try
            {
                cmbCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("Select ID," +
                    "Name,Filter,Isnull(IsFilterByDate,0)as IsFilterByDate,DateCols,Query,CompID,Isnull(IsActive,0)as IsActive" +
                    " From mst_GeneralQuery Where ID=" + SystemCode + "AND Isnull([Status],0)<>7");
                if (DT.Rows.Count > 0)
                {
                    cmbCompany.SelectedValue = Convert.ToInt32(DT.Rows[0]["CompID"].ToString());
                    txtName.Text = DT.Rows[0]["Name"].ToString();
                    txtFilter.Text = DT.Rows[0]["Filter"].ToString();
                    chkIsFilterByDate.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsFilterByDate"].ToString());
                    if (chkIsFilterByDate.Checked)
                    {
                        string filter = DT.Rows[0]["DateCols"].ToString();
                        string[] fil = filter.Split(',');
                        txtDateCol1.Text = fil[0];
                        txtDateCol2.Text = fil[1];
                    }
                    txtQuery.Text = DT.Rows[0]["Query"].ToString();
                    chkIsActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void chkIsFilterByDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsFilterByDate.Checked)
            {
                txtDateCol1.Enabled = true;
                txtDateCol2.Enabled = true;
            }
            else
            {
                txtDateCol1.Enabled = false;
                txtDateCol2.Enabled = false;
            }
        }
    }
}
