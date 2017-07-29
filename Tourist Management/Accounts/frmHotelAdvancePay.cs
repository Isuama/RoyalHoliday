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
    public partial class frmHotelAdvancePay : Form
    {
        private const string msghd = "Hotel Advance Payments";
        decimal Syscode;
        public enum CI { gDTI, gHID, gVID, gHNM, gAMT,gPDT, gPBY };
        public enum HA { gID, gTID, gGST, gVID, gHNM, gAMT, gPDT };
        public frmHotelAdvancePay(){InitializeComponent();}
        private void frmHotelAdvancePay_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Grid_Initializer();
                Fill_Control();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grid_Initializer()
        {
            grdCI.Cols.Count = 7;
            grdCI.Rows.Count = 30;
            grdCI.Cols[(int)CI.gDTI].Width = 90;
            grdCI.Cols[(int)CI.gHID].Width = 0;
            grdCI.Cols[(int)CI.gVID].Width = 90;
            grdCI.Cols[(int)CI.gHNM].Width = 228;
            grdCI.Cols[(int)CI.gAMT].Width = 100;
            grdCI.Cols[(int)CI.gPDT].Width = 93;
            grdCI.Cols[(int)CI.gPBY].Width = 0;
            grdCI.Cols[(int)CI.gDTI].Caption = "Date In";
            grdCI.Cols[(int)CI.gHID].Caption = "Hotel ID";
            grdCI.Cols[(int)CI.gVID].Caption = "Voucher ID";
            grdCI.Cols[(int)CI.gHNM].Caption = "Hotel Name";
            grdCI.Cols[(int)CI.gAMT].Caption = "Amount";
            grdCI.Cols[(int)CI.gPDT].Caption = "Paid Date";
            grdCI.Cols[(int)CI.gPBY].Caption = "Paid By";
            grdCI.Cols[(int)CI.gAMT].Format = "##.##";
            grdCI.Cols[(int)CI.gPDT].DataType = Type.GetType(" System.DateTime");
            grdCI.Rows[1].AllowEditing = true;
            grdHtlAdv.Cols.Count = 7;
            grdHtlAdv.Rows.Count = 2000;
            grdHtlAdv.Cols[(int)HA.gID].Width = 0;
            grdHtlAdv.Cols[(int)HA.gTID].Width = 90;
            grdHtlAdv.Cols[(int)HA.gGST].Width = 200;
            grdHtlAdv.Cols[(int)HA.gVID].Width = 90;
            grdHtlAdv.Cols[(int)HA.gHNM].Width = 200;
            grdHtlAdv.Cols[(int)HA.gAMT].Width = 93;
            grdHtlAdv.Cols[(int)HA.gPDT].Width = 90;
            grdHtlAdv.Cols[(int)HA.gID].Caption = "ID";
            grdHtlAdv.Cols[(int)HA.gTID].Caption = "Tour ID";
            grdHtlAdv.Cols[(int)HA.gGST].Caption = "Guest";
            grdHtlAdv.Cols[(int)HA.gVID].Caption = "Voucher ID";
            grdHtlAdv.Cols[(int)HA.gHNM].Caption = "Hotel Name";
            grdHtlAdv.Cols[(int)HA.gAMT].Caption = "Amount";
            grdHtlAdv.Cols[(int)HA.gPDT].Caption = "Paid Date";
            grdHtlAdv.Cols[(int)HA.gAMT].Format = "##.##";
            grdHtlAdv.Cols[(int)HA.gPDT].DataType = Type.GetType(" System.DateTime");
            grdHtlAdv.Rows[1].AllowEditing = true;
        }
        private void Fill_Control()
        {
            DataTable DTB;
            DTB = new DataTable();
            DTB = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Name AS HotelName FROM mst_HotelDetails Where ISNULL(PayID,1)=2 AND IsNull(IsActive,0)=1 ORDER BY ID");
            drpHotel.DataSource = DTB;
        }
        private void btnTour_Click(object sender, EventArgs e)
        {            
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID FROM trn_GroupAmendment";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.ToString().Trim() == "")
                return;
            txtGuest.Text = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Guest FROM trn_GroupAmendment WHERE ID=" + txtTourNo.Text.ToString().Trim() + "").Rows[0]["Guest"].ToString();
            Syscode = Convert.ToDecimal(txtTourNo.Text.ToString().Trim());
            Load_Hotel_Details();
        }
        private void Load_Hotel_Details()
        {
                string ssql = "SELECT DateIn,VoucherID,HotelID,HotelName,Advance,"+
                    "AdvanceDate,AdvancePaidBy" +
                    " FROM vw_trn_CityItinerary WHERE TransID=" + Syscode + " ORDER BY SrNo";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    int RowNumb = 0;
                    while (DT.Rows.Count > RowNumb)
                    {
                        if (DT.Rows[RowNumb]["DateIn"]+"".Trim()!="")
                            grdCI[RowNumb + 1, (int)CI.gDTI] = Convert.ToDateTime(DT.Rows[RowNumb]["DateIn"].ToString());
                        if (DT.Rows[RowNumb]["HotelID"] + "".Trim() != "")
                            grdCI[RowNumb + 1, (int)CI.gHID] = Convert.ToInt32(DT.Rows[RowNumb]["HotelID"].ToString());
                        if (DT.Rows[RowNumb]["VoucherID"] + "".Trim() != "")
                            grdCI[RowNumb + 1, (int)CI.gVID] = DT.Rows[RowNumb]["VoucherID"].ToString();
                        if (DT.Rows[RowNumb]["HotelName"] + "".Trim() != "")
                            grdCI[RowNumb + 1, (int)CI.gHNM] = DT.Rows[RowNumb]["HotelName"].ToString();
                        if (DT.Rows[RowNumb]["Advance"] + "".Trim() != "")
                            grdCI[RowNumb + 1, (int)CI.gAMT] = DT.Rows[RowNumb]["Advance"].ToString();
                        if (DT.Rows[RowNumb]["AdvanceDate"] + "".Trim() != "")
                            grdCI[RowNumb + 1, (int)CI.gPDT] = Convert.ToDateTime(DT.Rows[RowNumb]["AdvanceDate"]);
                        if (DT.Rows[RowNumb]["AdvancePaidBy"] + "".Trim() != "")
                            grdCI[RowNumb + 1, (int)CI.gPBY] = DT.Rows[RowNumb]["AdvancePaidBy"].ToString();
                        RowNumb++;
                    }
                }
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
        private Boolean Save_Pro(System.Data.SqlClient.SqlCommand sqlCom)
        {
                bool Rtn = false;
                if (Save_Basics(sqlCom))
                    Rtn = true;
                return Rtn;
        }
        private Boolean Save_Basics(System.Data.SqlClient.SqlCommand sqlCom)
        {
                int RowNumb = 1;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spUpdate_Hotel_AdvancePayments";
                while (grdCI[RowNumb, grdCI.Cols[(int)CI.gHID].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = Syscode;
                    sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 50).Value = grdCI[RowNumb, (int)CI.gVID].ToString().Trim();
                    if (grdCI[RowNumb, (int)CI.gAMT] + "".Trim() != "")
                    {
                        sqlCom.Parameters.Add("@Advance", SqlDbType.Decimal).Value = Convert.ToDecimal(grdCI[RowNumb, (int)CI.gAMT]);
                        if (grdCI[RowNumb, (int)CI.gPDT] + "".Trim() != "")
                            sqlCom.Parameters.Add("@AdvanceDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCI[RowNumb, (int)CI.gPDT]);
                        if (grdCI[RowNumb, (int)CI.gPBY] + "".Trim() != "")
                            sqlCom.Parameters.Add("@AdvancePaidBY", SqlDbType.Int).Value = Convert.ToInt32(grdCI[RowNumb, (int)CI.gPBY]);
                    }
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.ExecuteNonQuery();
                    if ((int)sqlCom.Parameters["@RtnValue"].Value != 1)
                    {
                        return false;
                    }
                    RowNumb++;
                }
                    return true;
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void grdCI_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            decimal advanceAmt=0.00m;
            if (grdCI[grdCI.Row, (int)CI.gAMT] + "".Trim() != "")
            {
                advanceAmt = Convert.ToDecimal(grdCI[grdCI.Row, (int)CI.gAMT]);
                if (advanceAmt > 0.00m && grdCI[grdCI.Row, (int)CI.gPBY] + "".Trim() == "")
                    grdCI[grdCI.Row, (int)CI.gPBY] = Convert.ToInt32(Classes.clsGlobal.UserID);
                if (advanceAmt > 0.00m && grdCI[grdCI.Row, (int)CI.gPDT] + "".Trim() == "")
                    grdCI[grdCI.Row, (int)CI.gPDT] = Convert.ToDateTime(Classes.clsGlobal.CurDate());                    
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print();
        }
        private void Print()
        {
                Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
                string reptype = "HotelAdvancePay";
                int HotelID = 0;
                string format = "yyyy-MM-dd";
                DateTime datefrom = dtpFrom.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpTo.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                if (drpHotel.SelectedValue.ToString().Trim() != "")
                {
                    HotelID = Convert.ToInt32(drpHotel.SelectedValue);
                } 
                string sql = "SELECT TransID,TourID,DateIn,DateOut,VoucherID,Guest," +
                             "HandleBy,HotelID,HotelName,RoomTypeName,RoomBasisName,Occupancy,IsNull(ModifiedCost,0)AS ModifiedCost,GuideCost," +
                             "IsNull(ExtraBed,0)AS Ebed,IsNull(EbedCost,0)AS EbedCost," +
                             "IsNull(Commission,0)AS Commission,ISNULL(Advance,0)AS Advance,IsNull(ConRate,0)AS ConRate,IsNull(GuideConRate,0)AS GuideConRate," +
                             "IsNull(RoomCount,0)AS RoomCount,GuideRooms AS GuideRoomCount,IsNull(FOCRooms,0)AS FOCRooms,IsNull(Nights,1)AS Nights,MealFor," +
                             "IsNull(AdultMealCost,0)AS AdultMealCost,IsNull(ChildMealCost,0)AS ChildMealCost,IsNull(GuideMealCost,0)AS GuideMealCost," +
                             "IsNull(NoOfAdult,0) AS NoOfAdult,IsNull(NoOfChild,0) AS NoOfChild,IsNull(NoOfGuide,0) AS NoOfGuide," +
                             "IsNull(FOCAdult,0) AS FOCAdult,IsNull(FOCChild,0) AS FOCChild," +
                             "PaidDate,PaidBy,PartiallyPaid,CurCode,GuideCurCode,OtherAmt,Remarks, DisplayName, Physical_Address, Telephone, Fax, Web, E_Mail,E_mailTo,UserName,UserGroupID,GroupName, Company_Logo," +
                             "Aname1,Ano1,MDname,MDno,AADname,AADno,IsNull(ConfirmPaid,0)AS ConfirmPaid" +
                             " FROM vw_acc_HotelDailyPayments WHERE Advance <> 0 AND ConfirmPaid = 'True' AND" +
                             " PaidDate BETWEEN '" + DateFrom.Trim() + "' AND '" + DateTo.Trim() + "' AND UserID = " + Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                string filter = " AND HotelID = " + HotelID;
                if (drpHotel.SelectedValue == null || drpHotel.SelectedValue.ToString().Trim() == "")
                {
                    sql = sql + " ORDER BY HandleBy,DateIn";
                }
                else
                {
                    sql = sql + filter + " ORDER BY HandleBy,DateIn";
                }
                DataSets.ds_acc_DailyDueHotel DTG = new DataSets.ds_acc_DailyDueHotel();
                Tourist_Management.Reports.HotelAdvancePay ga = new Tourist_Management.Reports.HotelAdvancePay();
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                {
                    sConnection.Print_Report("", sql, DTG, ga, reptype);
                }
                else
                    MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Fill_Grid()
        {
            try
            {
                DataTable DT;
                string ssql, filter="";
                string format = "yyyy-MM-dd";
                DateTime datefrom = dtpFrom.Value;
                string DateFrom = datefrom.ToString(format).Substring(0, 10);
                DateTime dateto = dtpTo.Value;
                string DateTo = dateto.ToString(format).Substring(0, 10);
                int HotelID = 0;
                if (drpHotel.SelectedValue + "" != "")
                    HotelID = Convert.ToInt32(drpHotel.SelectedValue.Trim());
                ssql = "SELECT DISTINCT TransID,VoucherID,Guest," +
                       "HotelName,IsNull(Advance,0)AS Advance,PaidDate," +
                       "IsNull(ConfirmPaid,0)AS ConfirmPaid"+
                       " FROM vw_acc_HotelDailyPayments WHERE ConfirmPaid = 'True' AND Advance <> 0 AND PaidDate BETWEEN '" + DateFrom + "' AND '" + DateTo + "'";
                filter = " AND HotelID = " + HotelID;
                if (drpHotel.SelectedValue + "" != "")
                    ssql = ssql + filter;
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                int count = 0;
                if (DT.Rows.Count != 0)
                {
                    while (count < DT.Rows.Count)
                    {
                        grdHtlAdv[count + 1, (int)HA.gTID] = DT.Rows[count]["TransID"];
                        grdHtlAdv[count + 1, (int)HA.gGST] = DT.Rows[count]["Guest"];
                        grdHtlAdv[count + 1, (int)HA.gVID] = DT.Rows[count]["VoucherID"];
                        grdHtlAdv[count + 1, (int)HA.gHNM] = DT.Rows[count]["HotelName"];
                        grdHtlAdv[count + 1, (int)HA.gAMT] = DT.Rows[count]["Advance"];
                        grdHtlAdv[count + 1, (int)HA.gPDT] = DT.Rows[count]["PaidDate"];
                        count++;
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void drpHotel_Selected_TextChanged(object sender, EventArgs e)
        {
            grdHtlAdv.Rows.Count = 1;
            grdHtlAdv.Rows.Count = 2000;
            Fill_Grid();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            grdHtlAdv.Rows.Count = 1;
            grdHtlAdv.Rows.Count = 2000;
            Fill_Grid();
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            grdHtlAdv.Rows.Count = 1;
            grdHtlAdv.Rows.Count = 2000;
            Fill_Grid();
        }
    }
}
