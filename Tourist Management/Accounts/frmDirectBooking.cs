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
    public partial class frmDirectBooking : Form
    {
        private const string msghd = "Direct Bokokings";
        enum GRD { ID, TourID, Guest, DateArr, DateDep, HandledBy, Agent, IsDirect };
        int earlyRow = 0;
        DataTable DTBL = new DataTable();
        public frmDirectBooking(){InitializeComponent();}
        private void btnICancel_Click(object sender, EventArgs e){this.Close();}
        private void frmDirectBooking_Load(object sender, EventArgs e)
        {
            Grd_Initializer();
            Fill_Control();
            fill_Data();
        }
        private void Grd_Initializer()
        {
            try
            {                
                grdViewer.Cols.Count = 8;
                grdViewer.Rows.Count = 100;
                grdViewer.Cols[(int)GRD.ID].Width = 00; 
                grdViewer.Cols[(int)GRD.TourID].Width = 100;
                grdViewer.Cols[(int)GRD.Guest].Width = 100;
                grdViewer.Cols[(int)GRD.DateArr].Width = 100;
                grdViewer.Cols[(int)GRD.DateDep].Width = 100;
                grdViewer.Cols[(int)GRD.Agent].Width = 100;
                grdViewer.Cols[(int)GRD.HandledBy].Width = 100;
                grdViewer.Cols[(int)GRD.IsDirect].Width = 100;
                grdViewer.Cols[(int)GRD.ID].Caption = "ID";
                grdViewer.Cols[(int)GRD.TourID].Caption = "TourID";
                grdViewer.Cols[(int)GRD.Guest].Caption = "Guest";
                grdViewer.Cols[(int)GRD.DateArr].Caption = "Arrival";
                grdViewer.Cols[(int)GRD.DateDep].Caption = "Departure";
                grdViewer.Cols[(int)GRD.Agent].Caption = "Agent";
                grdViewer.Cols[(int)GRD.HandledBy].Caption = "Handled";
                grdViewer.Cols[(int)GRD.IsDirect].Caption = "Direct";
                grdViewer.Rows[1].AllowEditing = true;
                grdViewer.Cols[(int)GRD.IsDirect].DataType = Type.GetType(" System.Boolean");
            }
            catch (Exception ex){db.MsgERR(ex);}
        }        
        private void Fill_Control()
        {
            try
            { 
                ucFilterByOther2.drpOther.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name as [AgentName] FROM mst_AgentDetails Where IsNull(IsActive,0)=1 ORDER BY Name");
                 ucFilterByOther1.drpOther.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS [HandledPerson] FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                 ucFilterByCompany1.cmbICompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
                for (int x = 0; x < grdViewer.Cols.Count - 1; x++)  cmbFld.Items.Add(grdViewer[0, x].ToString()); 
                cmbFld.SelectedIndex = 1;
                db.LoadSearch(cmbOp);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void fill_Data()
        {
            try
            {
                string qry = "SELECT ID,CompID,TourID,Guest,AgentID,AgentName,Arrival,Departure," +
                             "IsDirectBooking,HandledByID,HandledBy" +
                             " FROM vw_DirectBookings WHERE 1=1";
                if (chkDirect.Checked)
                    qry += " AND IsDirectBooking=1";
                else
                    qry += " AND IsDirectBooking=0";
                if (ucFilterByDate1.chkIByDate.Checked)
                {
                    string din=ucFilterByDate1.dtpIFromDate.Value.ToString("yyyy-MM-dd");
                    string dou=ucFilterByDate1.dtpIToDate.Value.ToString("yyyy-MM-dd");
                    qry += " AND Arrival>='" + din + "' AND Arrival<='" + dou + "'";
                }
                if (ucFilterByCompany1.chkICmpny.Checked)
                {
                    int compID = Convert.ToInt32(ucFilterByCompany1.cmbICompany.SelectedValue);
                    qry += " AND CompID="+compID+"";
                }
                if (ucFilterByOther1.chkIByOther.Checked)
                {
                    int HID = Convert.ToInt32(ucFilterByOther1.drpOther.SelectedValue);
                    qry += " AND HandledByID=" + HID + "";
                }
                if (ucFilterByOther2.chkIByOther.Checked)
                {
                    int AID = Convert.ToInt32(ucFilterByOther2.drpOther.SelectedValue);
                    qry += " AND AgentID=" + AID + "";
                }
                DTBL = Classes.clsGlobal.objCon.Fill_Table(qry);
                grdViewer.Rows.Count = Convert.ToInt16(DTBL.Rows.Count) + 1;
                foreach (DataRow dr in DTBL.Rows)
                {
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.ID] = dr["ID"];
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.TourID] = dr["TourID"];
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.Guest] = dr["Guest"];
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.DateArr] = dr["Arrival"];
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.DateDep] = dr["Departure"];
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.Agent] = dr["AgentName"];
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.HandledBy] = dr["HandledBy"];
                    grdViewer[(DTBL.Rows.IndexOf(dr)) + 1, (int)GRD.IsDirect] = Convert.ToBoolean(dr["IsDirectBooking"]);
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
            }
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
                if (Save_Data(objCom) == true)
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
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private Boolean Save_Data(System.Data.SqlClient.SqlCommand sqlCom)
        {
            try
            {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_DirectBookings";
                double id;
                int val;
                int row = 1;
                while (grdViewer[row, (int)GRD.ID] + "".Trim() != "")
                {                    
                    sqlCom.Parameters.Clear();
                    id = Convert.ToDouble(grdViewer[row, (int)GRD.ID]);
                    sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = id;
                    val = Convert.ToBoolean(grdViewer[row, (int)GRD.IsDirect]) ? 1 : 0;
                    sqlCom.Parameters.Add("@IsDirect", SqlDbType.Int).Value = val;
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    row++;
                    if (row == grdViewer.Rows.Count)
                        break;
                }               
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        private void btnFilter_Click(object sender, EventArgs e)
        {
            fill_Data();
        }
        private void chkDirect_CheckedChanged(object sender, EventArgs e)
        {
            grdViewer.Rows.Count = 2;
        }
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtAccountName.Text = "";
            if (earlyRow != 0)
                grdViewer.Rows[earlyRow].Style = grdViewer.Styles["transparent"];
        }
        private void txtAccountName_TextChanged(object sender, EventArgs e)
        {
            Apply_Filter();
        }
        private void Apply_Filter()
        {
            DataView   DV = new DataView(DTBL);
                if (txtAccountName.Text.Trim() == "") return;
                switch (cmbOp.Text.Trim())
                {
                    case "Contains":
                    case "Begins with":
                    case "Ends with":
                        DV.RowFilter = cmbFld.Text.Trim() + " " + cmbOp.SelectedValue.ToString().Trim().Replace("##", txtAccountName.Text.Trim()).ToString();
                        break;
                    default:
                        DV.RowFilter = cmbFld.Text.Trim() + " " + cmbOp.SelectedValue.ToString().Trim() + "'" + txtAccountName.Text.Trim() + "'";
                        break;
                }
                DataTable dt = DV.ToTable();
                if (dt.Rows.Count > 0 && dt.Rows[0]["ID"] + "".Trim() != "")
                    search_By_ID(Convert.ToInt64(dt.Rows[0]["ID"]));
        }
        public void search_By_ID(Int64 ID)
        {
            try
            {
                int row = 1;
                int AccID;
                bool found = false;
                C1.Win.C1FlexGrid.CellStyle transparent;
                transparent = grdViewer.Styles.Add("transparent");
                transparent.BackColor = Color.Transparent;
                C1.Win.C1FlexGrid.CellStyle searched;
                searched = grdViewer.Styles.Add("searched");
                searched.BackColor = ColorTranslator.FromHtml("#E1F5A9");
                if (ID + "".Trim() == "")
                {
                    if (earlyRow != 0)
                        grdViewer.Rows[earlyRow].Style = grdViewer.Styles["transparent"];
                    return;
                }
                while (row <= grdViewer.Rows.Count - 1)
                {
                    AccID = Convert.ToInt32(grdViewer[row, (int)GRD.ID]);
                    if (ID == AccID)
                    {
                        if (earlyRow != 0)
                            grdViewer.Rows[earlyRow].Style = grdViewer.Styles["transparent"];
                        grdViewer.Select(row, (int)GRD.ID);
                        grdViewer.Rows[row].Style = grdViewer.Styles["searched"];
                        found = true;
                        earlyRow = row;
                        break;
                    }
                    row++;
                }
                if (!found && earlyRow != 0)
                    grdViewer.Rows[earlyRow].Style = grdViewer.Styles["transparent"];
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
