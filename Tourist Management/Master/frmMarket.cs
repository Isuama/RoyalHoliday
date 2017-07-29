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
    public partial class frmMarket : Form
    {
        private const string msghd = "Market Details";
        public string SqlQry = "SELECT ID,Code [Market Code],MarketName [Market Name]," +
                       "Isnull(IsActive,0)AS IsActive From mst_HotelMarket"+
                       " Where Isnull([Status],0)<>7 Order By Code";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public frmMarket(){InitializeComponent();}
        private void frmMarket_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            if (Mode == 0)
            {
                Generate_Market_Code();
                txtName.Text = "";
                txtName.Select();
                chkActive.Checked = true;                
            }
            else
            {
                Fill_Data();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void Generate_Market_Code()
        {
            SqlQry = "SELECT MAX(ID) AS ID FROM mst_HotelMarket";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
            if (DT.Rows[0]["ID"] + "".Trim() == "")
                txtCode.Text = "MKT1001";
            else
                txtCode.Text = "MKT" + (Convert.ToInt32(DT.Rows[0]["ID"]) + 1).ToString();            
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Successfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Fill_Data();
                this.Close();
            }
        }
        private void Fill_Data()
        {
            DataTable DT;
            try
            {
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code,"+
                    "MarketName,Isnull(IsActive,0)as IsActive"+
                    " From mst_HotelMarket Where ID=" + SystemCode + "");
                if (DT.Rows.Count > 0)
                {
                    txtCode.Text = DT.Rows[0]["Code"].ToString();
                    txtName.Text = DT.Rows[0]["MarketName"].ToString();
                    chkActive.Checked = System.Convert.ToBoolean(DT.Rows[0]["IsActive"].ToString());
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
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
                if (txtCode.Text.Trim() == "")
                {
                    MessageBox.Show("Code cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Market Name cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Code From mst_HotelMarket Where Code='" + txtCode.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Code already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select MarketName From mst_HotelMarket Where MarketName='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Market Name already exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                sqlCom.CommandText = "spSave_HotelMarket";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Code", SqlDbType.NVarChar, 50).Value = txtCode.Text.Trim();
                sqlCom.Parameters.Add("@MarketName", SqlDbType.NVarChar, 100).Value = txtName.Text.Trim();
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@UserID", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                }
                return RtnVal;
        }
    }
}
