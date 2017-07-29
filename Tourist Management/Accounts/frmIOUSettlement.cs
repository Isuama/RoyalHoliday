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
    public partial class frmIOUSettlement : Form
    {
        private const string msghd = "IOU Settlement Details";
        public string SqlQry = "SELECT DISTINCT ID, Company, DisplayName, Name, Amount, Description, Date From vw_acc_IOU_Settlement Where Isnull([Status],0)<>7 Order By ID";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        int RowNumb = 0;
        public frmIOUSettlement(){InitializeComponent();}
        private void frmIOU_Settlement_Load(object sender, EventArgs e)
        {
            Initializer();
        }
        private void Initializer()
        {
            Fill_Control();
            if (Mode != 0)
            {
                Fill_Data();
            }
        }
        private void Fill_Data()
        {
            try
            {
                SqlQry = "SELECT ID, TransID, Company, PaidTo PaymentTo, Amount, Description, Date FROM vw_acc_IOU_Settlement " +
                        "WHERE ID = " + SystemCode;
                DataTable DTIOU = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQry);
                if (DTIOU.Rows.Count > 0)
                {
                    RowNumb = 0;
                    Mode = 1;
                    txtTourNo.Text = DTIOU.Rows[RowNumb]["TransID"] + "";
                    if (txtTourNo.Text != "")
                    {
                        drpCompany.Enabled = false;
                        rdbDriver.Visible = true;
                        rdbGuide.Visible = true;
                        rdbHotel.Visible = true;
                        rdbAgent.Visible = true;
                    }
                    txtSettlementNo.Text = DTIOU.Rows[RowNumb]["ID"].ToString();
                    dtpSettlement.Value = Convert.ToDateTime(DTIOU.Rows[RowNumb]["Date"]);
                    drpPaymentTo.setSelectedValue(DTIOU.Rows[RowNumb]["PaymentTo"].ToString());
                    txtAmount.Text = DTIOU.Rows[RowNumb]["Amount"].ToString();
                    rtxtDescription.Text = DTIOU.Rows[RowNumb]["Description"].ToString();
                    drpCompany.setSelectedValue(DTIOU.Rows[RowNumb]["Company"].ToString());
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            {
                DataTable[] DTB;
                DTB = new DataTable[3];
                int newid = 0;
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ISNULL(MAX(ID),0) AS ID FROM act_IOU_Settlement");
                newid = Convert.ToInt32(DTB[0].Rows[0]["ID"]) + 1;
                txtSettlementNo.Text = newid.ToString();
                dtpSettlement.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                DTB[1] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID, Name FROM vw_ALL_PERSON_DETAILS");
                drpPaymentTo.DataSource = DTB[1];
                DTB[2] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID, DisplayName FROM mst_CompanyGenaral");
                drpCompany.DataSource = DTB[2];
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
                MessageBox.Show("Record Saved Sucessfully", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (chkPrint.Checked)
                {
                    Print_Invoice();
                }
                this.Close();
            }
        }
        private Boolean Save_Pro()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
            try
            {
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "sp_Save_IOU_Settlement";
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = Mode;
                sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = SystemCode;
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                if(txtTourNo.Text!="")
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTourNo.Text.Trim());
                sqlCom.Parameters.Add("@Company", SqlDbType.Int).Value = drpCompany.SelectedValue;
                sqlCom.Parameters.Add("@PaymentTo", SqlDbType.NVarChar).Value = drpPaymentTo.SelectedValue;
                sqlCom.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = Convert.ToDouble(txtAmount.Text);
                sqlCom.Parameters.Add("@Description", SqlDbType.NVarChar).Value = rtxtDescription.Text.Trim();
                sqlCom.Parameters.Add("@Date", SqlDbType.DateTime).Value = dtpSettlement.Value;
                sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = Classes.clsGlobal.CurDate();
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                {
                    RtnVal = true;
                    SystemCode = Convert.ToDouble(sqlCom.Parameters["@ID"].Value);
                }
                return RtnVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Print_Invoice()
        {
            Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
            string sql;
            sql = "SELECT ID, Name, PaidTo PaymentTo, Amount, Description, Date, DisplayName, Physical_Address, " +
                   "Company_Logo, Telephone, Fax, E_Mail, Web," +
                   "TourID,Guest,DateArrival,DateDeparture,HandledBy," +
                   "AADname,MDname,LastModifiedBy" +
                    " From vw_acc_IOU_Settlement WHERE ID = " + SystemCode;
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            if (DT.Rows.Count > 0)
            {
                DataSets.ds_acc_IOU DTP = new Tourist_Management.DataSets.ds_acc_IOU();
                Tourist_Management.TransacReports.IOU_Settlement rptIOU = new Tourist_Management.TransacReports.IOU_Settlement();
                sConnection.Print_Report(SystemCode.ToString(), sql, DTP, rptIOU, "IOU SETTLEMENT INVOICE");
            }
            else
                MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Invoice();
        }
        private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID FROM vw_TourBasics WHERE ISNULL(IsCompleted,0)<>1 AND ISNULL(IsCancelled,0)<>1";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.Trim() == "")
                return;
            SystemCode = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
            Fill_Control_For_Tour();
        }
        public void Clear_Contents()
        {
            try
            {
                dtpSettlement.Value = Classes.clsGlobal.CurDate();
                drpCompany.setSelectedValue(null);
                drpCompany.Enabled = true;
                drpPaymentTo.setSelectedValue(null);
                rdbDriver.Visible = false;
                rdbGuide.Visible = false;
                rdbAgent.Visible = false;
                rdbHotel.Visible = false;
                txtAmount.Text = "";
                rtxtDescription.Text = "";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public void Fill_Control_For_Tour()
        {
            try
            {
                string sql;
                DataTable DT;
                sql = "SELECT ID,Name FROM vw_trn_TourEntities WHERE TransID=" + SystemCode + "";
                if (rdbDriver.Checked)
                {
                    sql += " AND Type='Driver'";
                }
                else if (rdbGuide.Checked)
                {
                    sql += " AND Type='Guide'";
                }
                else if (rdbAgent.Checked)
                {
                    sql += " AND Type='Agent'";
                }
                else if (rdbHotel.Checked)
                {
                    sql += " AND Type='Hotel'";
                }
                rdbDriver.Visible = true;
                rdbGuide.Visible = true;
                rdbHotel.Visible = true;
                rdbAgent.Visible = true;
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                drpPaymentTo.setSelectedValue(null);
                drpPaymentTo.DataSource = DT;
                string compID = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CompID FROM trn_GroupAmendment").Rows[0]["CompID"]+"".Trim();
                if (compID != "")
                {
                    drpCompany.setSelectedValue(compID);
                    drpCompany.Enabled = false;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void rdbDriver_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Control_For_Tour();
        }
        private void rdbGuide_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Control_For_Tour();
        }
        private void rdbAgent_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Control_For_Tour();
        }
        private void rdbHotel_CheckedChanged(object sender, EventArgs e)
        {
            Fill_Control_For_Tour();
        }
    }
}
