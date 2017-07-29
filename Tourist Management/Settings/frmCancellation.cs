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
    public partial class frmCancellation : Form
    {
        private const string msghd = "Hotel Cancellation";
        String ssql;
        string SqlQuery;
        int RowNumb;
        DataTable DtColour;
        enum DY { gID, gDFR, gDTO, gPID, gPER,gCOL };
        enum DT { gID, gDFR, gDTO, gPID, gNOD, gPER,gCOL };
        C1.Win.C1FlexGrid.CellStyle[] CANCEL;
        public frmCancellation(){InitializeComponent();}
        private void frmCansellation_Load(object sender, EventArgs e)
        {
            Initialize();
            fillData();
        }
        public void Initialize()
        {
            CANCEL = new C1.Win.C1FlexGrid.CellStyle[grdCDate.Rows.Count];
            #region Grid Day
            grdCDay.Cols.Count = 6;
            grdCDay.Rows.Count = 100;
            grdCDay.Cols[(int)DY.gID].Width = 0;
            grdCDay.Cols[(int)DY.gDFR].Width = 90;
            grdCDay.Cols[(int)DY.gDTO].Width = 90;
            grdCDay.Cols[(int)DY.gPID].Width = 0;
            grdCDay.Cols[(int)DY.gPER].Width = 101;
            grdCDay.Cols[(int)DY.gCOL].Width = 80;
            grdCDay.Cols[(int)DY.gID ].Caption = "ID";
            grdCDay.Cols[(int)DY.gDFR].Caption = "Days From";
            grdCDay.Cols[(int)DY.gDTO].Caption = "Days To";
            grdCDay.Cols[(int)DY.gPID].Caption = "Percentage ID";
            grdCDay.Cols[(int)DY.gPER].Caption = "Percentage";
            grdCDay.Cols[(int)DY.gCOL].Caption = "Colour";
            grdCDay.Cols[(int)DY.gDFR].Format = "##";
            grdCDay.Cols[(int)DY.gDTO].Format = "##.##";
            grdCDay.Cols[(int)DY.gPER].ComboList = "...";
            grdCDay.Cols[(int)DY.gCOL].ComboList = "...";
            grdCDay.Rows[1].AllowEditing = true;
            #endregion
            #region Grid Date
            grdCDate.Cols.Count = 7;
            grdCDate.Rows.Count = 100;
            grdCDate.Cols[(int)DT.gID].Width = 0;
            grdCDate.Cols[(int)DT.gDFR].Width = 65;
            grdCDate.Cols[(int)DT.gDTO].Width = 65;
            grdCDate.Cols[(int)DT.gPID].Width = 0;
            grdCDate.Cols[(int)DT.gNOD].Width = 65;
            grdCDate.Cols[(int)DT.gPER].Width = 86;
            grdCDate.Cols[(int)DT.gCOL].Width = 80;
            grdCDate.Cols[(int)DT.gID].Caption = "ID";
            grdCDate.Cols[(int)DT.gDFR].Caption = "Date From";
            grdCDate.Cols[(int)DT.gDTO].Caption = "Date To";
            grdCDate.Cols[(int)DT.gPID].Caption = "Percentage ID";
            grdCDate.Cols[(int)DT.gNOD].Caption = "No Of Days";
            grdCDate.Cols[(int)DT.gPER].Caption = "Percentage";
            grdCDate.Cols[(int)DT.gCOL].Caption = "Colour";
            grdCDate.Cols[(int)DT.gDFR].DataType = Type.GetType("System.DateTime");
            grdCDate.Cols[(int)DT.gDTO].DataType = Type.GetType("System.DateTime");
            grdCDate.Cols[(int)DT.gPER].ComboList = "...";
            grdCDate.Cols[(int)DT.gCOL].ComboList = "...";
            grdCDate.Rows[1].AllowEditing = true;
            #endregion
        }
        public void fillData()
        {
            #region CANCEL BY DAY RANGE
            ssql = "SELECT [ID],[From],[To],Percentage,PercentageID,[Colour] " +
                   "FROM vw_CancelByDay ORDER BY SrNo";
            DataTable DTCBDY = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
           if (DTCBDY.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DTCBDY.Rows.Count > RowNumb)
                {
                    grdCDay[RowNumb + 1, (int)DY.gID] = DTCBDY.Rows[RowNumb]["ID"].ToString();
                    grdCDay[RowNumb + 1, (int)DY.gDFR] = DTCBDY.Rows[RowNumb]["From"].ToString();
                    grdCDay[RowNumb + 1, (int)DY.gDTO] = DTCBDY.Rows[RowNumb]["To"].ToString();
                    grdCDay[RowNumb + 1, (int)DY.gPER] = DTCBDY.Rows[RowNumb]["Percentage"].ToString();
                    grdCDay[RowNumb + 1, (int)DY.gPID ] = DTCBDY.Rows[RowNumb]["PercentageID"].ToString();
                    grdCDay[RowNumb + 1, (int)DY.gCOL] = DTCBDY.Rows[RowNumb]["Colour"].ToString();
                    RowNumb++;
                }
            }
            #endregion
            #region CANCEL BY DATE RANGE
            ssql = "SELECT [ID] ,[From],[To],NoOfDays,Percentage,PercentageID,[Colour] " +
                  "FROM vw_CancelByDate ";
            DataTable DTCBDT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DTCBDT.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DTCBDT.Rows.Count > RowNumb)
                {
                    grdCDate[RowNumb + 1, (int)DT.gID] = DTCBDT.Rows[RowNumb]["ID"].ToString();
                    grdCDate[RowNumb + 1, (int)DT.gDFR] = Convert.ToDateTime(DTCBDT.Rows[RowNumb]["From"].ToString());
                    grdCDate[RowNumb + 1, (int)DT.gDTO] = Convert.ToDateTime(DTCBDT.Rows[RowNumb]["To"].ToString());
                    if (DTCBDT.Rows[RowNumb]["From"].ToString() != "" && DTCBDT.Rows[RowNumb]["To"].ToString() != "")
                    {
                        TimeSpan tspan = Convert.ToDateTime(DTCBDT.Rows[RowNumb]["To"].ToString()) - Convert.ToDateTime(DTCBDT.Rows[RowNumb]["From"].ToString());
                        grdCDate[RowNumb + 1, (int)DT.gNOD] = Convert.ToInt32(tspan.TotalDays.ToString());
                    }
                    grdCDate[RowNumb + 1, (int)DT.gPER] = DTCBDT.Rows[RowNumb]["Percentage"].ToString();
                    grdCDate[RowNumb + 1, (int)DT.gPID] = DTCBDT.Rows[RowNumb]["PercentageID"].ToString();
                    grdCDate[RowNumb + 1, (int)DT.gCOL] = DTCBDT.Rows[RowNumb]["Colour"].ToString();
                    RowNumb++;
                }
            }
            #endregion
        }
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false;
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Procedure() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal;
        }
        private Boolean Validate_Data()
        {
                if (Validate_Hotel_CancelByDay() == false)
                {
                    return false;
                }
                if (Validate_Hotel_CancelByDate() == false)
                {
                    return false;
                }
                return true;
        }
        private Boolean Validate_Hotel_CancelByDay()
        {
                RowNumb = 1;
                if ((grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDFR].Index] == null))
                {
                    return true;
                }
                do
                {
                    if (grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDTO].Index] == null || grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDTO].Index].ToString() == "")
                    {
                        MessageBox.Show("Please Select 'Days To' In Cancel By Day Range.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (grdCDay[RowNumb, grdCDay.Cols[(int)DY.gPER].Index] == null)
                    {
                        MessageBox.Show("Please Select 'Percentage' In Cancel By Day Range.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                } while ((grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDFR].Index] != null));
                return true;
        }
        private Boolean Validate_Hotel_CancelByDate()
        {
                RowNumb = 1;
                if ((grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDFR].Index] == null))
                {
                    return true;
                }
                do
                {
                    if (grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDTO].Index] == null)
                    {
                        MessageBox.Show("Please Select 'Date To' In Cancel By Date Range.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    if (grdCDate[RowNumb, grdCDate.Cols[(int)DT.gPER].Index] == null)
                    {
                        MessageBox.Show("Please Select 'Percentage' In Cancel By Date Range.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    RowNumb++;
                } while ((grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDFR].Index] != null));
                return true;
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
                if (Save_CancelByDays(objCom) == true && Save_CancelByDate(objCom) == true)
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
        private Boolean Save_CancelByDays(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_CancelByDays";
                RowNumb = 1;
                while (grdCDay[RowNumb, grdCDay.Cols[(int)DY.gDFR].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    if (grdCDay[RowNumb, (int)DY.gID] + "".Trim() == "")
                    {
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                    }
                    else
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = grdCDay[RowNumb, (int)DY.gID];
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdCDay[RowNumb, (int)DY.gDFR] + "".Trim() != "")
                        sqlCom.Parameters.Add("@From", SqlDbType.Int).Value = Int32.Parse(grdCDay[RowNumb, (int)DY.gDFR].ToString());
                    if (grdCDay[RowNumb, (int)DY.gDTO] + "".Trim() != "")
                        sqlCom.Parameters.Add("@To", SqlDbType.Int).Value = Int32.Parse(grdCDay[RowNumb, (int)DY.gDTO].ToString());
                    if (grdCDay[RowNumb, (int)DY.gPID] + "".Trim() != "")
                        sqlCom.Parameters.Add("@PercentageID", SqlDbType.Int).Value = Int32.Parse(grdCDay[RowNumb, (int)DY.gPID].ToString());
                    if (grdCDay[RowNumb, (int)DY.gCOL] + "".Trim() != "")
                        sqlCom.Parameters.Add("@Colour", SqlDbType.Text).Value = grdCDay[RowNumb, (int)DY.gCOL].ToString();
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
        private Boolean Save_CancelByDate(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb;
            Boolean RtnVal = true;
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_CancelByDate";
                RowNumb = 1;
                while (grdCDate[RowNumb, grdCDate.Cols[(int)DT.gDFR].Index] != null)
                {
                    sqlCom.Parameters.Clear();
                    if (grdCDate[RowNumb, (int)DT.gID] + "".Trim() == "")
                    {
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = 0;
                    }
                    else
                        sqlCom.Parameters.Add("@ID", SqlDbType.Int).Value = grdCDate[RowNumb, (int)DT.gID];
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    if (grdCDate[RowNumb, (int)DT.gDFR] + "".Trim() != "")
                        sqlCom.Parameters.Add("@From", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCDate[RowNumb, (int)DT.gDFR]);
                    if (grdCDate[RowNumb, (int)DT.gDTO] + "".Trim() != "")
                        sqlCom.Parameters.Add("@To", SqlDbType.DateTime).Value = Convert.ToDateTime(grdCDate[RowNumb, (int)DT.gDTO]);
                    if (grdCDate[RowNumb, (int)DT.gPID] + "".Trim() != "")
                        sqlCom.Parameters.Add("@PercentageID", SqlDbType.Int).Value = Convert.ToInt32(grdCDate[RowNumb, (int)DT.gPID]);
                    if (grdCDate[RowNumb, (int)DT.gCOL] + "".Trim() != "")
                        sqlCom.Parameters.Add("@Colour", SqlDbType.Text).Value = grdCDate[RowNumb, (int)DT.gCOL].ToString();
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
            if (MessageBox.Show("Do you want to save this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            if (Save_Pro() == true)
            {
                fillData();
                this.Close();
            }
        }
        private void grdCDay_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frm;
            Other.frmSearchGrd frmCol;
            DataTable DTPercen;
            if (e.Col == grdCDay.Cols[(int)DY.gCOL].Index)
            {
                SqlQuery = "SELECT [ID],[Color]FROM [TouristManagementCommon].[dbo].[mst_GridColour]";
                DtColour = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                frmCol = new Tourist_Management.Other.frmSearchGrd();
                frmCol.DataSource = DtColour;
                frmCol.Width = grdCDay.Cols[(int)DY.gPER].Width;
                frmCol.Height = grdCDay.Height;
                frmCol.StartPosition = FormStartPosition.Manual;
                frmCol.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCDay);
                SelText = frmCol.Open_Search();
                if (SelText != null)
                {
                    grdCDay[grdCDay.Row, (int)DY.gCOL] = SelText[1];
                }
            }
            if (e.Col == grdCDay.Cols[(int)DY.gPER].Index)
            {
                SqlQuery = "SELECT ID,Percentage FROM mst_Percentage Where IsNull(IsActive,0)=1";
                DTPercen = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                frm = new Tourist_Management.Other.frmSearchGrd();
                frm.DataSource = DTPercen;
                frm.Width = grdCDay.Cols[(int)DY.gPER].Width;
                frm.Height = grdCDay.Height;
                frm.StartPosition = FormStartPosition.Manual;
                frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCDay);
                SelText = frm.Open_Search();
                if (SelText != null)
                {
                    grdCDay[grdCDay.Row, (int)DY.gPID] = SelText[0];
                    grdCDay[grdCDay.Row, (int)DY.gPER] = SelText[1];
                }
            }
        }
        private void grdCDate_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            string[] SelText;
            Other.frmSearchGrd frmm;
            DataTable DTPercen;
            Other.frmSearchGrd frmCol;
            if (e.Col == grdCDate.Cols[(int)DT.gCOL].Index)
            {
                SqlQuery = "SELECT [ID],[Color]FROM [TouristManagementCommon].[dbo].[mst_GridColour]";
                DtColour = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                frmCol = new Tourist_Management.Other.frmSearchGrd();
                frmCol.DataSource = DtColour;
                frmCol.Width = grdCDate.Cols[(int)DY.gCOL].Width;
                frmCol.Height = grdCDate.Height;
                frmCol.StartPosition = FormStartPosition.Manual;
                frmCol.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCDate);
                SelText = frmCol.Open_Search();
                if (SelText != null)
                {
                    grdCDate[grdCDate.Row, (int)DT.gCOL] = SelText[1];
                }
            }
            if (e.Col == grdCDate.Cols[(int)DT.gPER].Index)
            {
                SqlQuery = "SELECT ID,Percentage FROM mst_Percentage Where IsNull(IsActive,0)=1";
                DTPercen = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(SqlQuery);
                frmm = new Tourist_Management.Other.frmSearchGrd();
                frmm.DataSource = DTPercen;
                frmm.Width = grdCDate.Cols[(int)DT.gPER].Width;
                frmm.Height = grdCDate.Height;
                frmm.StartPosition = FormStartPosition.Manual;
                frmm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdCDate);
                SelText = frmm.Open_Search();
                if (SelText != null)
                {
                    grdCDate[grdCDate.Row, (int)DT.gPID] = SelText[0];
                    grdCDate[grdCDate.Row, (int)DT.gPER] = SelText[1];
                }
            }
        }
        private void grdCDate_LeaveCell(object sender, EventArgs e)
        {
             try
            {
                if ((grdCDate[grdCDate.Row, grdCDate.Cols[(int)DT.gDFR].Index] != null) && (grdCDate[grdCDate.Row, grdCDate.Cols[(int)DT.gNOD].Index] != null))
                {
                    if (grdCDate.Col == (int)DT.gNOD)
                    {
                        if (grdCDate[grdCDate.Row, grdCDate.Cols[(int)DT.gNOD].Index] != null)
                        {
                            if (Tourist_Management.Classes.clsGlobal.IsNumeric(grdCDate[grdCDate.Row, (int)DT.gNOD].ToString()) == false)
                            {
                                MessageBox.Show("Please Enter Valid Values For Number of Days", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            else
                            {
                                if (grdCDate[grdCDate.Row, grdCDate.Cols[(int)DT.gDFR].Index] != null)
                                {
                                    DateTime DArr = Convert.ToDateTime(grdCDate[grdCDate.Row, (int)DT.gDFR].ToString());
                                    int NOD = Convert.ToInt32(grdCDate[grdCDate.Row, (int)DT.gNOD].ToString());
                                    if(NOD!=0)
                                        grdCDate[grdCDate.Row, (int)DT.gDTO] = DArr.AddDays(NOD);
                                }
                            }
                        }
                    }}
                    else
                    {
                        if ((grdCDate[grdCDate.Row, grdCDate.Cols[(int)DT.gDFR].Index] != null) && (grdCDate[grdCDate.Row, grdCDate.Cols[(int)DT.gDTO].Index] != null))
                        {
                            if (grdCDate[grdCDate.Row, (int)DT.gDFR].ToString() == grdCDate[grdCDate.Row, (int)DT.gDTO].ToString())
                            {
                                grdCDate[grdCDate.Row, (int)DT.gNOD] = 1;
                                return;
                            }
                            if (Convert.ToDateTime(grdCDate[grdCDate.Row, (int)DT.gDFR].ToString()) < Convert.ToDateTime(grdCDate[grdCDate.Row, (int)DT.gDTO].ToString()))
                            {
                                TimeSpan tspan = Convert.ToDateTime(grdCDate[grdCDate.Row, (int)DT.gDTO].ToString()) - Convert.ToDateTime(grdCDate[grdCDate.Row, (int)DT.gDFR].ToString());
                                grdCDate[grdCDate.Row, (int)DT.gNOD] = Convert.ToInt32(tspan.TotalDays.ToString());
                            }
                            else
                            {
                                MessageBox.Show("Arrival Date Cannot Be Greater Than The Departure Date.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                    }
            }
             catch (Exception ex){db.MsgERR(ex);}
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void grdCDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                grdCDate.Rows.Remove(grdCDate.Row);
                grdCDate.Rows[1].AllowEditing = true;
                grdCDate.Rows.Count += 1;
            }
        }
        private void grdCDate_Click(object sender, EventArgs e)
        {
        }
    }
}
