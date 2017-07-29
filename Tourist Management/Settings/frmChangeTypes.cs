using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Settings
{
    public partial class frmChangeTypes : Form
    {
        private const string msghd = "Change Voucher Type";
        decimal Syscode;
        DataTable DTAmend = new DataTable();
        bool IsAccountDep = false; //CHECK FOR ACCOUTN DEPARTMENT
        bool IsAdmin = false;
        bool IsCompleted = false; //CHECK TOUR ISCOMPLETED
        public enum CI { gDTI, gHID, gVID, gHNM, gSEL, gCON};
        public frmChangeTypes(){InitializeComponent();}
        private void frmChangeTypes_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grid_Initializer();
                Fill_Control();
                Check_User_Group();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Check_User_Group()
        {
                string   ssql = "SELECT UserGroupID FROM mst_UserMaster WHERE ID="+Classes.clsGlobal.UserID.ToString().Trim()+"";
                string Group=Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql).Rows[0][0].ToString().Trim();
                if (Group == "1008") //ACCOUNT GROUP
                {
                    grdCI.Cols[(int)CI.gSEL].Width = 0;
                    grdCI.Cols[(int)CI.gCON].Width = 100;
                    grpSelectType.Enabled = false;
                    rdbVou.Visible = false;
                    IsAccountDep = true;
                    tcChangeTypes.TabPages.RemoveAt(1);
                }
                else if (Group == "1001") //ALL ACCESS
                {
                    grdCI.Cols[(int)CI.gSEL].Width = 103;
                    grdCI.Cols[(int)CI.gCON].Width = 66;
                    grdCI.Cols[(int)CI.gHNM].Width = 224;
                    grpSelectType.Enabled = true;
                    rdbVou.Visible = true;
                    IsAdmin = true;
                    IsAccountDep = true;
                }
                else
                {
                    grdCI.Cols[(int)CI.gSEL].Width = 100;
                    grdCI.Cols[(int)CI.gCON].Width = 0;
                    tcChangeTypes.TabPages.RemoveAt(1);
                    grpSelectType.Enabled = true;
                    rdbVou.Visible = false;
                    IsAccountDep = false;
                }
        }
        private void Fill_Control()
        {
                DTAmend.Columns.Add("ID", typeof(int));
                DTAmend.Columns.Add("Name", typeof(string));
                DTAmend.Rows.Add(0, "A");
                DTAmend.Rows.Add(1, "B");
                DTAmend.Rows.Add(2, "C");
                DTAmend.Rows.Add(3, "D");
                DTAmend.Rows.Add(4, "E");
                DTAmend.Rows.Add(5, "F");
                DTAmend.Rows.Add(6, "G");
                DTAmend.Rows.Add(7, "H");
                DTAmend.Rows.Add(8, "I");
                DTAmend.Rows.Add(9, "J");
                DTAmend.Rows.Add(10, "K");
                DTAmend.Rows.Add(11, "L");
                DTAmend.Rows.Add(12, "M");
                DTAmend.Rows.Add(13, "N");
                DTAmend.Rows.Add(14, "O");
                DTAmend.Rows.Add(15, "P");
                DTAmend.Rows.Add(16, "Q");
                DTAmend.Rows.Add(17, "R");
                DTAmend.Rows.Add(18, "S");
                DTAmend.Rows.Add(19, "T");
                DTAmend.Rows.Add(20, "U");
                DTAmend.Rows.Add(21, "V");
                DTAmend.Rows.Add(22, "W");
                DTAmend.Rows.Add(23, "X");
                DTAmend.Rows.Add(24, "Y");
                DTAmend.Rows.Add(25, "Z"); 
                cmbAmendNo.DataSource = DTAmend;
                drpAgent.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_AgentDetails Where IsNull(IsActive,0)=1 ORDER BY Name");
                drpMarketingDep.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name FROM mst_MarketingDep Where IsNull(IsActive,0)=1 ORDER BY Name");
                cmbCompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
        }
        private void Grid_Initializer()
        {
            db.GridInit(grdCI, 30, true, CI.gDTI, 90, "Date In", Type.GetType(" System.DateTime"), CI.gHID, 0, "Hotel ID", CI.gVID, 90, "Voucher ID", CI.gHNM, 293, "Hotel Name", CI.gSEL, 0, "To Be Changed", Type.GetType(" System.Boolean"), CI.gCON, 0, "Is Confirm", Type.GetType(" System.Boolean")); 
        }
        private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID FROM trn_GroupAmendment";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.ToString().Trim() == "")
                return;
            txtGuest.Text = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Guest FROM trn_GroupAmendment WHERE ID=" + txtTourNo.Text.ToString().Trim() + "").Rows[0]["Guest"].ToString();
            DataTable DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CompID,AgentID,MarketingDep FROM trn_GroupAmendment WHERE ID=" + txtTourNo.Text.ToString().Trim() + "");
            if (DT1.Rows.Count > 0)
            {
                cmbCompany.SelectedValue = Convert.ToInt32(DT1.Rows[0]["CompID"].ToString());
                if(DT1.Rows[0]["AgentID"]+"".Trim()!="")
                    drpAgent.setSelectedValue(DT1.Rows[0]["AgentID"].ToString());
                if (DT1.Rows[0]["MarketingDep"] + "".Trim() != "")
                    drpMarketingDep.setSelectedValue(DT1.Rows[0]["MarketingDep"].ToString());
            }
            Syscode = Convert.ToDecimal(txtTourNo.Text.ToString().Trim());
            Load_Hotel_Details();
            Load_PNL_Details();
        }
        private void Load_Hotel_Details()
        {
                string ssql = "SELECT DateIn,VoucherID,HotelID,HotelName,ISNULL(ConfirmPaid,0)as ConfirmPaid FROM vw_trn_CityItinerary WHERE TransID =" +Syscode + " ORDER BY SrNo";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    int RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["DateIn"].ToString() != "")
                            grdCI[RowNumb + 1, (int)CI.gDTI] = Convert.ToDateTime(DT.Rows[RowNumb]["DateIn"].ToString());
                        if (DT.Rows[RowNumb]["HotelID"].ToString() != "")
                            grdCI[RowNumb + 1, (int)CI.gHID] = Convert.ToInt32(DT.Rows[RowNumb]["HotelID"].ToString());
                        if (DT.Rows[RowNumb]["VoucherID"].ToString() != "")
                            grdCI[RowNumb + 1, (int)CI.gVID] = DT.Rows[RowNumb]["VoucherID"].ToString();
                        if (DT.Rows[RowNumb]["HotelName"].ToString() != "")
                            grdCI[RowNumb + 1, (int)CI.gHNM] = DT.Rows[RowNumb]["HotelName"].ToString();
                        grdCI[RowNumb + 1, (int)CI.gCON] = Convert.ToBoolean(DT.Rows[RowNumb]["ConfirmPaid"]);
                        RowNumb++;
                    }
                }
        }
        private void Load_PNL_Details()
        {
                string sql = "SELECT ISNULL(IsCompleted,0)AS IsCompleted FROM dbo.act_Profit_Lose WHERE TourID =" + Syscode + " ";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0 && DT.Rows[0]["IsCompleted"].ToString() != "")
                {
                    IsCompleted = Convert.ToBoolean(DT.Rows[0]["IsCompleted"]);
                    chkIsCompleted.Checked = IsCompleted;
                }
                if (IsCompleted)
                {
                    chkIsCompleted.Enabled = true;
                    chkIsCompleted.ForeColor = Color.Green;
                }
                else
                {
                    chkIsCompleted.Enabled = false;
                    chkIsCompleted.ForeColor = Color.Black;
                }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void Clear_Contents()
        {
            txtGuest.Text = "";
            grdCI.Rows.Count = 1;
            grdCI.Rows.Count = 30;
            rdbAmend.Checked = true;
            rdbReserv.Checked = false;
            cmbAmendNo.Enabled = true;
            cmbAmendNo.Visible = true;
            drpAgent.setSelectedValue(null);
            drpMarketingDep.setSelectedValue(null);
            IsCompleted = false;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Procedure() == true)
            {
                if (MessageBox.Show("Transaction Completed.Close Window.?", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){this.Close();}
            }
            else
                MessageBox.Show("Transaction Not Completed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (Save_Pro(objCom) == true)
                {
                    objTrn.Commit();
                    objCon.Close();
                    return true;
                }
                else
                {
                    objTrn.Rollback();
                }
                objCon.Close();
                return false;
        }
        private void rdbAmend_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAmend.Checked)
            {
                cmbAmendNo.Visible = true;
                cmbAmendNo.Enabled = true;
            }
            else
                cmbAmendNo.Visible = false;
        }
        private Boolean Save_Pro(System.Data.SqlClient.SqlCommand sqlCom)
        {
                bool Rtn = false;
                if (Save_Basics(sqlCom))
                    Rtn = true;
                if ((IsAdmin) && Save_Others(sqlCom))
                {
                    Rtn = true;
                }
                return Rtn;
        }
        private Boolean Save_Basics(System.Data.SqlClient.SqlCommand sqlCom)
        {
                int RowNumb = 1;
                string vid = "", ato = "";
                bool At_Least_One = false;
                int selected = 0;
                bool RtnVal = false;
                if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gHID].Index] == null))
                {
                    MessageBox.Show("No Hotels Found To Be Changed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (rdbConf.Checked)
                {
                    if (IsAccountDep)
                    {
                        #region CHANGE CONFIRMATION
                        sqlCom.CommandType = CommandType.StoredProcedure;
                        sqlCom.CommandText = "spUpdate_Hotel_Confirmation";
                        while (grdCI[RowNumb, grdCI.Cols[(int)CI.gHID].Index] != null)
                        {
                            sqlCom.Parameters.Clear();
                            sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = Syscode;
                            sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 50).Value = grdCI[RowNumb, (int)CI.gVID].ToString().Trim();
                            sqlCom.Parameters.Add("@ConfirmPaid", SqlDbType.Int).Value = Convert.ToBoolean(grdCI[RowNumb, (int)CI.gCON]);
                            sqlCom.Parameters.Add("@ModifiedBY", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                            sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                            sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                            sqlCom.ExecuteNonQuery();
                            if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                            {
                                return false;
                            }
                            RowNumb++;
                        }
                        RtnVal = true;
                        #endregion
                    }
                }
                else
                {
                    #region CHANGE VOUCHER NO
                    sqlCom.CommandType = CommandType.StoredProcedure;
                    sqlCom.CommandText = "spUpdate_Voucher_Numbers";
                    RowNumb = 1;
                    while (grdCI[RowNumb, grdCI.Cols[(int)CI.gHID].Index] != null)
                    {
                        if (Convert.ToBoolean(grdCI[RowNumb, (int)CI.gSEL]) == false)
                        {
                            RowNumb++;
                            continue;
                        }
                        At_Least_One = true;
                        sqlCom.Parameters.Clear();
                        sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = Syscode;
                        sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                        if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gDTI].Index] != null))
                            sqlCom.Parameters.Add("@DateIn", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gDTI].ToString());
                        if ((grdCI[RowNumb, grdCI.Cols[(int)CI.gHID].Index] != null))
                            sqlCom.Parameters.Add("@HotelID", SqlDbType.Int).Value = Int32.Parse(grdCI[RowNumb, (int)CI.gHID].ToString());
                        if (rdbAmend.Checked)
                        {
                            vid = Syscode + "/" + RowNumb + "/" + cmbAmendNo.Text.Trim().ToString();
                            sqlCom.Parameters.Add("@AmendTime", SqlDbType.Int).Value = Int32.Parse(cmbAmendNo.SelectedValue.ToString());
                            if (Int32.Parse(cmbAmendNo.SelectedValue.ToString()) == 0)
                                sqlCom.Parameters.Add("@AmendmentTo", SqlDbType.NVarChar, 50).Value = Syscode + "/" + RowNumb;
                            else
                            {
                                selected = Int32.Parse(DTAmend.Rows[Int32.Parse(cmbAmendNo.SelectedValue.ToString()) - 1][0].ToString());
                                ato = Syscode + "/" + RowNumb + "/" + DTAmend.Rows[selected]["Name"].ToString().Trim();
                                sqlCom.Parameters.Add("@AmendmentTo", SqlDbType.NVarChar, 50).Value = ato.ToString().Trim();
                            }
                        }
                        else
                        {
                            vid = Syscode + "/" + RowNumb;
                            sqlCom.Parameters.Add("@AmendTime", SqlDbType.Int).Value = 0;
                            sqlCom.Parameters.Add("@AmendmentTo", SqlDbType.NVarChar, 50).Value = "";
                        }
                        sqlCom.Parameters.Add("@AmendNo", SqlDbType.Int).Value = getAmendNo();
                        sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 50).Value = vid.ToString().Trim();
                        sqlCom.Parameters.Add("@OldVoucherID", SqlDbType.NVarChar, 50).Value = grdCI[RowNumb, (int)CI.gVID].ToString().Trim();
                        sqlCom.Parameters.Add("@ModifiedBY", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                        sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                        sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                        sqlCom.ExecuteNonQuery();
                        if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                        {
                            return false;
                        }
                        RowNumb++;
                    }
                    if (At_Least_One)
                        RtnVal = true;
                    else
                    {
                        MessageBox.Show("No Hotels Found To Be Changed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        RtnVal = false;
                    }
                    #endregion
                }
                return RtnVal;
        }
        private int getAmendNo()
        {
            try
            {
                if (rdbReserv.Checked)
                    return 0;
                if (rdbMeal.Checked)
                    return 2;
                if (rdbComplementary.Checked)
                    return 99;
                if (rdbCancellation.Checked)
                    return 9;
                if (rdbAmend.Checked)
                    return 1;
                MessageBox.Show("Cannot be found...", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return 0;
            }
        }
        private Boolean Validate_Other()
        {
                return true;
        }
        private Boolean Save_Others(System.Data.SqlClient.SqlCommand sqlCom)
        {
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "sp_save_stt_Update_Others";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@TransID", SqlDbType.Int).Value = Syscode;
                sqlCom.Parameters.Add("@CompID", SqlDbType.Int).Value = Convert.ToInt32(cmbCompany.SelectedValue.ToString());
                if (drpAgent.SelectedValue+"".Trim() != "")
                    sqlCom.Parameters.Add("@AgentID", SqlDbType.Int).Value = drpAgent.SelectedValue.Trim();
                if (drpMarketingDep.SelectedValue+"".Trim() != "")
                    sqlCom.Parameters.Add("@MarketingDep", SqlDbType.Int).Value = drpMarketingDep.SelectedValue.Trim();
                if (IsCompleted)
                {
                    sqlCom.Parameters.Add("@IsCompleted", SqlDbType.Int).Value = chkIsCompleted.Checked ? 1 : 0;
                }
                sqlCom.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                {
                    return false;
                }
                return true;
        }
        private void rdbConf_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbConf.Checked)
                grpSelectType.Visible = false;
            else
                grpSelectType.Visible = true;
        }
        private void chkIsCompleted_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsCompleted.Checked == true)
            {
                chkIsCompleted.ForeColor = Color.Green;
            }
            else
            {
                chkIsCompleted.ForeColor = Color.Red;
            }
        }
    }
}
