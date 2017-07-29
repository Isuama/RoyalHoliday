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
    public partial class frmOtherIns : Form
    {
        private const string msghd = "Other Instructions Setup";
        public string SqlQry = "SELECT Instruc,IsActive From mst_OtherIns Order By SrNo";
        public int Mode = 0,SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        Boolean bLoad = false;
        enum CC { gEAD, gIAT };
        public frmOtherIns(){InitializeComponent();}
        private void frmOtherIns_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            Grd_Initializer();
            Fill_Data();
        }
        private void Grd_Initializer()
        {
            try
            {
                grdCC.Cols.Count = 2;
                grdCC.Rows.Count = 200;
                grdCC.Cols[(int)CC.gEAD].Caption = "Instruction";
                grdCC.Cols[(int)CC.gIAT].Caption = "Active";
                grdCC.Rows[1].AllowEditing = true;
                grdCC.Cols[(int)CC.gEAD].Width = 700;
                grdCC.Cols[(int)CC.gIAT].Width = 100;
                grdCC.Cols[(int)CC.gIAT].DataType = Type.GetType("System.Boolean");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
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
        private void Fill_Data()
        {
            try
            {
                DataTable DT;
                string ssql; 
                int RowNumb;
                ssql = "SELECT Instruc,IsActive From mst_OtherIns Order By SrNo";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count == 0)
                {
                    SystemCode = 0;
                    Mode = 0;
                }
                else
                {
                    Mode = 1;
                    if (DT.Rows[0][0].ToString() != null && DT.Rows[0][0].ToString() != "")
                    {
                        RowNumb = 0;
                        while (DT.Rows.Count > RowNumb)
                        {
                            if (DT.Rows[RowNumb]["Instruc"].ToString() != "")
                                grdCC[RowNumb + 1, (int)CC.gEAD] = DT.Rows[RowNumb]["Instruc"].ToString();
                            if (DT.Rows[RowNumb]["IsActive"].ToString() != "")
                                grdCC[RowNumb + 1, (int)CC.gIAT] = DT.Rows[RowNumb]["IsActive"].ToString();
                            RowNumb++;
                        }
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
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
                if (Save_CCEmails(objCom) == true)
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
        private Boolean Save_CCEmails(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_mst_OtherIns";
                RowNumb = 1;
                while (grdCC[RowNumb, grdCC.Cols[(int)CC.gEAD].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdCC[RowNumb, (int)CC.gEAD] != null)
                        sqlCom.Parameters.Add("@Instruc", SqlDbType.NVarChar, 200).Value = grdCC[RowNumb, (int)CC.gEAD].ToString();
                    if (grdCC[RowNumb, (int)CC.gEAD] != null)
                        sqlCom.Parameters.Add("@IsActive", SqlDbType.Int).Value = Convert.ToBoolean(grdCC[RowNumb, (int)CC.gIAT]);
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
        private void grdCC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdCC.Rows.Remove(grdCC.Row);
                grdCC.Rows[1].AllowEditing = true;
            }
        }
        private void grdCC_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true)
            {
                return;
            }
            grdCC.Rows[1].AllowEditing = true;
            if (grdCC.Rows.Count < 3)
            {
                return;
            }
            if (grdCC[grdCC.Row - 1, 0] == null)
            {
                grdCC.Rows[grdCC.Row].AllowEditing = false;
            }
            else
            {
                grdCC.Rows[grdCC.Row].AllowEditing = true;
            }
        }
    }
}
