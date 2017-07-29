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
    public partial class frmDefaultExpences : Form
    {
        private const string msghd = "Default Expenses Details";
        public string SqlQry = "SELECT ID,Name AS[ Expense Name],Isnull(IsActive,0)AS IsActive, IsNull(IsDefault,0) AS IsDefault From mst_TransportExpenses Order By Name";
        public int Mode = 0, SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        enum DE { gEID, gENM, gISDEF};
        public frmDefaultExpences(){InitializeComponent();}
        private void frmDefaultExpences_Load(object sender, EventArgs e)
        {
            Initializer();
        }
        private void Initializer()
        {
            Grid_Initializer();
            Get_Details();
        }
        private void Grid_Initializer()
        {
            try
            {
                grdDefEx.Cols.Count = 3;
                grdDefEx.Cols[(int)DE.gEID].Width = 0;
                grdDefEx.Cols[(int)DE.gENM].Width = 320;
                grdDefEx.Cols[(int)DE.gISDEF].Width = 100;
                grdDefEx.Cols[(int)DE.gEID].Caption = "Expence ID";
                grdDefEx.Cols[(int)DE.gENM].Caption = "Expence Name";
                grdDefEx.Cols[(int)DE.gISDEF].Caption = "Is Default";
                grdDefEx.Cols[(int)DE.gISDEF].DataType = Type.GetType("System.Boolean");
                grdDefEx.Cols[(int)DE.gENM].ComboList = "...";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Get_Details()
        {
            try
            {
                string ssql;
                int RowNumb;
                ssql = "SELECT ID, Name, IsNull(IsDefault,0) AS IsDefault FROM mst_TransportExpenses WHERE IsNull(IsActive,0)<>0 AND IsNull(IsDefault,0)<>0";
                DataTable DEX = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DEX.Rows.Count > 0)
                {
                    RowNumb = 0;
                    while (DEX.Rows.Count > RowNumb)
                    {
                        grdDefEx[RowNumb + 1, (int)DE.gEID] = DEX.Rows[RowNumb]["ID"].ToString();
                        grdDefEx[RowNumb + 1, (int)DE.gENM] = DEX.Rows[RowNumb]["Name"].ToString();
                        grdDefEx[RowNumb + 1, (int)DE.gISDEF] = Convert.ToBoolean(DEX.Rows[RowNumb]["IsDefault"]);
                        RowNumb++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void grdDefEx_CellButtonClick_1(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTExpense;
                if (e.Col == grdDefEx.Cols[(int)DE.gENM].Index)
                {
                    DTExpense = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM mst_TransportExpenses WHERE IsNull(IsActive,0)=1 ORDER BY Name");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTExpense;
                    frm.SubForm = new Transaction.frmExpenses();
                    frm.Width = grdDefEx.Cols[(int)DE.gENM].Width;
                    frm.Height = grdDefEx.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdDefEx);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        if (DTExpense.Rows[0]["ID"].ToString() != "")
                            grdDefEx[grdDefEx.Row, (int)DE.gEID] = SelText[0].ToString();
                        if (DTExpense.Rows[0]["Name"].ToString() != "")
                            grdDefEx[grdDefEx.Row, (int)DE.gENM] = SelText[1].ToString();
                    }
                }
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
                if (Save_DefExpences(objCom) == true)
                {
                    objTrn.Commit();
                    MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    objCon.Close();
                    return true;
                }
                else
                {
                    objTrn.Rollback();
                    MessageBox.Show("Data Not Saved Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                objCon.Close();
                return false;
        }
        private Boolean Save_DefExpences(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "sp_Save_Default_Expences";
                RowNumb = 1;
                while (grdDefEx[RowNumb, grdDefEx.Cols[(int)DE.gENM].Index] != null)
                {
                    if (grdDefEx[RowNumb, (int)DE.gENM].ToString().Trim() == "")
                        return true;
                    sqlCom.Parameters.Clear();
                    if (RowNumb == 1)
                        sqlCom.Parameters.Add("@Flag", SqlDbType.Int).Value = 1;
                    sqlCom.Parameters.Add("@ExpenceID", SqlDbType.Int).Value = Convert.ToInt32(grdDefEx[RowNumb, (int)DE.gEID]);
                    sqlCom.Parameters.Add("@IsDefault", SqlDbType.Int).Value = Convert.ToBoolean(grdDefEx[RowNumb, (int)DE.gISDEF]);
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
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
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            Save_Procedure();
        }
    }
}
