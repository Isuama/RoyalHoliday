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
    public partial class frmTourIDGenerator : Form
    {
        private const string msghd = "Tour ID Generator";
        string ID = "1001", CMP = "CMP", CTRY = "CTY", HPC = "HPC", CPC = "CPC";
        string ccode = "";
        Boolean id = false;
        enum GenID { Field, DefVal};
        public frmTourIDGenerator(){InitializeComponent();}
        private void frmTourIDGenerator_Load(object sender, EventArgs e)
        {
            Initializer();
        }
        private void Initializer()
        {
            Grid_Initializer();
        }
        private void Grid_Initializer()
        {
            grdGenID.Cols.Count = 2;
            grdGenID.Rows.Count = 100;
            grdGenID.Cols[(int)GenID.Field].Width = 150;
            grdGenID.Cols[(int)GenID.DefVal].Width = 140;
            grdGenID.Cols[(int)GenID.Field].Caption = "Field";
            grdGenID.Cols[(int)GenID.DefVal].Caption = "Default Value";
            grdGenID.Cols[(int)GenID.Field].ComboList = "...";
        }
        private void grdGenID_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            DataTable DTField = new DataTable();
            DTField.Columns.Add("id");
            DTField.Columns.Add("Field");
            DTField.Rows.Add(1,"Company Code");
            DTField.Rows.Add(2,"Country Code");
            DTField.Rows.Add(3,"Handled Person Code");
            DTField.Rows.Add(4,"Created Person Code");
            DTField.Rows.Add(5,"ID");
            try
            {
                #region Field
                if (e.Col == grdGenID.Cols[(int)GenID.Field].Index)
                {
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTField;
                    frm.SubForm = new Master.frmCompany();
                    frm.Width = grdGenID.Cols[(int)GenID.Field].Width;
                    frm.Height = grdGenID.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdGenID);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdGenID[grdGenID.Row, (int)GenID.Field] = SelText[1];
                        if ( SelText[1] == "Company Code" )
                            grdGenID[grdGenID.Row, (int)GenID.DefVal] = CMP;
                        if (SelText[1] == "Country Code")
                            grdGenID[grdGenID.Row, (int)GenID.DefVal] = CTRY;
                        if (SelText[1] == "Handled Person Code")
                            grdGenID[grdGenID.Row, (int)GenID.DefVal] = HPC;
                        if (SelText[1] == "Created Person Code")
                            grdGenID[grdGenID.Row, (int)GenID.DefVal] = CPC;
                        if (SelText[1] == "ID")
                        {
                            grdGenID[grdGenID.Row, (int)GenID.DefVal] = ID;
                            id = true;
                        }
                        Create_ID(grdGenID[grdGenID.Row, (int)GenID.DefVal].ToString());
                    }
                }
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Create_ID( string code )
        {
            ccode += "/" + code;
            int lastindex = ccode.LastIndexOf('/');
            if (lastindex == 0)
            {
                ccode = ccode.Substring(1).Trim();
            }
            string FinalID = ccode;     // +"/" + ParentID;
            lblCode.Text = FinalID;
        }
        private Boolean Save()
        {
            Boolean RtnVal = false;
            System.Data.SqlClient.SqlCommand sqlCom = null;
            System.Data.SqlClient.SqlConnection sqlCon = null;
            try
            {
                sqlCon = Tourist_Management.Classes.clsGlobal.objCon.ReturnConnection;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.Connection = sqlCon;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_Tour_ID";
                sqlCon.Open();
                int row = 1;
                while (grdGenID[row, grdGenID.Cols[(int)GenID.Field].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    if (grdGenID[row, grdGenID.Cols[(int)GenID.Field].Index] != null)
                    {
                        sqlCom.Parameters.Add("@Field", SqlDbType.VarChar).Value = grdGenID[row, (int)GenID.Field].ToString().Trim();
                        sqlCom.Parameters.Add("@Code", SqlDbType.VarChar).Value = grdGenID[row, (int)GenID.DefVal].ToString().Trim();
                        sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = row;
                    }
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Decimal).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    row++;
                    if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                    {
                        RtnVal = true;
                    }
                    else
                        return false;
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                throw ex;
            }
            finally
            {
                sqlCon.Close();
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Validate() != true)
            {
                return;
            }
            if (Save() == true)
            {
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Data Not Saved Successfully.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private new Boolean Validate()
        { 
            int row = 1;
            if (id == false)
            {
                MessageBox.Show("Select ID", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            while (grdGenID[row, grdGenID.Cols[(int)GenID.Field].Index] != null)
            {
                if (grdGenID[row, (int)GenID.Field].ToString().Trim() == "ID")
                {
                    if (grdGenID[row, (int)GenID.DefVal].ToString().Trim().Length > 6 || grdGenID[row, (int)GenID.DefVal].ToString().Trim().Length < 0)
                    {
                        MessageBox.Show("Default ID Must be at least 1 character long & should not exceed more than 6 characters.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                else
                {
                    if (grdGenID[row, (int)GenID.DefVal].ToString().Trim().Length > 3 || grdGenID[row, (int)GenID.Field].ToString().Trim().Length < 2)
                    {
                        MessageBox.Show("Default " + grdGenID[row, (int)GenID.DefVal].ToString().Trim() + " Code Must be at least 2 characters long & should not exceed more than 3 characters.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                row++;
            }
            return true;
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
    }
}
