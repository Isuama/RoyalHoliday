using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Transaction
{
    public partial class frmExpenses : Form
    {
        private const string msghd = "Transport Expenses Details";
        public string SqlQry = "SELECT ID,Name AS[ Expense Name],Isnull(IsActive,0)AS IsActive From mst_TransportExpenses Order By Name";
        public int Mode = 0, SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public frmExpenses(){InitializeComponent();}
        private void frmExpenses_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                if (Mode != 0)
                {
                    Fill_Data();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {
                DataTable DT;
                string ssql;
                ssql = " SELECT ID,Name,ISNULL(UnitCost,0)UnitCost,ISNULL(ShowInTR,0)ShowInTR,IsNull(IsActive,0) AS IsActive FROM mst_TransportExpenses WHERE ID=" + SystemCode + "";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    Mode = 1;
                    txtName.Text = DT.Rows[0]["Name"].ToString();
                    txtUnitCost.Text = DT.Rows[0]["UnitCost"] + "".Trim();
                    chkShowInTR.Checked = Convert.ToBoolean(DT.Rows[0]["ShowInTR"]);
                    if (Convert.ToBoolean(DT.Rows[0]["IsActive"]))
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
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
                if (txtName.Text.Trim() == "")
                {
                    MessageBox.Show("Expense Name Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("Select Name From mst_TransportExpenses Where Name='" + txtName.Text.Trim() + "' and ID <> " + SystemCode + "").Rows.Count > 0)
                {
                    MessageBox.Show("Expenses Name Already Exists", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtUnitCost.Text.Trim() != "")
                {
                    if (!Classes.clsGlobal.IsNumeric(txtUnitCost.Text))
                    {
                        MessageBox.Show("Please Enter Valid Value For Unitcost.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                return true;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Transport_Expenses_Name";
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters.Add("@Name", SqlDbType.NVarChar, 500).Value = txtName.Text.Trim();
                if(txtUnitCost.Text!="")
                sqlCom.Parameters.Add("@UnitCost", SqlDbType.Decimal,18).Value = txtUnitCost.Text.Trim();
                sqlCom.Parameters.Add("@ShowInTR", SqlDbType.Int).Value = chkShowInTR.Checked == true ? "1" : "0";
                sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = chkActive.Checked == true ? "1" : "0";
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
