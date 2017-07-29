using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SQLCONN;
using System.Drawing;
using C1.Win.C1FlexGrid;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Threading;
namespace Tourist_Management.Classes
{
    static class clsGlobal
    {        
        public static int selectedPersonID = 0;
        public static string SelectedPersonName = "";
        public static bool filterOutsideLoad;
        public static bool filterDrivers;
        public static string[] path = new string[50];
        public static string[] FileName = new string[50];
        public static string Invpath;
        public static string InvName;
        private static bool IsAdmin = false;
        private static bool IsSuper = false;
        private static bool m_CanLog = false;
        public static bool PasswordOK = false;
        private static int USERID = 0, CompID = 0;
        private static string LogInDate = "Unknown";
        private static string ACCCATID = "";
        private const string msghd = "Global..";
        private static bool NOCHECK = false;
        private static DataTable dtPermission;
        public static DataTable SelectedHotels = new DataTable();
        public static SQLCONN.SQLCONN Con;
        public static SQLCONN.SQLCONN CommonCon;
        public static bool AllowLog
        {
            get { return clsGlobal.m_CanLog; }
            set { clsGlobal.m_CanLog = value; }
        }
        public static SQLCONN.SQLCONN objCon
        {
            get { return Con; }
            set { Con = value; }
        }
        public static SQLCONN.SQLCONN objComCon
        {
            get { return CommonCon; }
            set { CommonCon = value; }
        }
        public static int UserID
        {
            get { return USERID; }
            set { USERID = value; }
        }
        public static int CompanyID
        {
            get { return CompID; }
            set { CompID = value; }
        }
        public static string[] VoucherPath
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        public static string InvoicePath
        {
            get
            {
                return Invpath;
            }
            set
            {
                Invpath = value;
            }
        }
        public static string[] VoucherFileName
        {
            get
            {
                return FileName;
            }
            set
            {
                FileName = value;
            }
        }
        public static string InvoiceName
        {
            get
            {
                return InvName;
            }
            set
            {
                InvName = value;
            }
        }
        public static string LogInDetails
        {
            get { return LogInDate; }
            set { LogInDate = value; }
        }
        public static string AccCatID
        {
            get { return ACCCATID; }
            set { ACCCATID = value; }
        }
        public static bool NoPermisionCheck
        {
            get { return NOCHECK; }
            set { NOCHECK = value; }
        }
        public static bool Is_SuperUser
        {
            get { return IsSuper; }
            set { IsSuper = value; }
        }
        public static bool Is_Admin
        {
            get { return IsAdmin; }
            set { IsAdmin = value; }
        }
        public static DataTable TB_UserPR
        {
            get { return dtPermission; }
            set { dtPermission = value; }
        }
        public static DateTime CurDate()
        {
            return System.Convert.ToDateTime(Con.Fill_Table("SELECT GetDate()").Rows[0][0].ToString());
        }
        public static int TaxYearID()
        {
            return System.Convert.ToInt16(Con.Fill_Table("SELECT ID FROM dbo.mst_CompanyTaxYears WHERE ISNULL(IsCompleted,0)=0").Rows[0][0].ToString());
        }
        public static Point GetCellLocation(C1FlexGrid flxGrd)
        {
            Rectangle rc = flxGrd.GetCellRect(flxGrd.Row, flxGrd.Col, true);
            rc = flxGrd.RectangleToScreen(rc);
            return new Point(rc.Left, rc.Bottom);
        }
        public static decimal ExpenseUnitCost(string ID)
        {
            try
            {
                string qry = "SELECT ISNULL(UnitCost,0)UnitCost FROM mst_TransportExpenses WHERE ID="+ID.Trim()+"";
                DataTable DT = Classes.clsGlobal.objCon.Fill_Table(qry);
                foreach (DataRow dr in DT.Rows)
                    return Convert.ToDecimal(dr["UnitCost"]);
                return 0;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return 0;
            }
        }
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }
        public static bool Instr(string str, string search)
        {
            string[] strArr;
            strArr = str.Split(new Char[] { ' ' });
            for (int x = 0; x < strArr.Length; x++)
            {
                if (strArr[x].Trim() == search) { return true; }
            }
            return false;
        }
        public static DropDowns.DropSearch[] Sort_ControlArray(DropDowns.DropSearch[] ARRY)
        {
            DropDowns.DropSearch[] RtnArray;
            string[] strArray;
            RtnArray = new DropDowns.DropSearch[ARRY.Length];
            strArray = new string[ARRY.Length];
            for (int x = 0; x < ARRY.Length; x++)
            {
                strArray[x] = ARRY[x].Name.ToString();
            }
            Array.Sort(strArray);
            for (int x = 0; x < strArray.Length; x++)
            {
                for (int Y = 0; Y < ARRY.Length; Y++)
                {
                    if (strArray[x] == ARRY[Y].Name.ToString())
                    {
                        RtnArray[x] = ARRY[Y];
                        break;
                    }
                }
            }
            return RtnArray;
        }
        public static int[,] Sort_Int2DArray(int[,] ARRY)
        {
            bool blnEQSRT = true;
            int[,] RtnArray;
            int[] strArray;
            RtnArray = new int[ARRY.Length / 2, 2];
            strArray = new int[ARRY.Length / 2];
            for (int x = 0; x < ARRY.Length / 2; x++)
            {
                strArray[x] = ARRY[x, 1];
                if (x != 0)
                {
                    if (ARRY[x, 1] != ARRY[x - 1, 1])
                    {
                        blnEQSRT = false;
                    }
                }
            }
            if (blnEQSRT == true)
            {
                return ARRY;
            }
            Array.Sort(strArray);
            for (int x = 0; x < strArray.Length; x++)
            {
                for (int Y = 0; Y < ARRY.Length / 2; Y++)
                {
                    if (strArray[x] == ARRY[Y, 1])
                    {
                        RtnArray[x, 0] = ARRY[Y, 0];
                        RtnArray[x, 1] = ARRY[Y, 1];
                        break;
                    }
                }
            }
            return RtnArray;
        }
        public static Boolean IsNumeric(string stringToTest)
        {
            decimal result;
            if (decimal.TryParse(stringToTest, out result))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool InstrString(string strText, string strChar)
        {
            if (strText.Length > strText.Replace(strChar, "").Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string Rtn_DatabaseID()
        {
            string abc = objComCon.Fill_Table("SELECT [DBID] FROM  dbo.meta_Databases  WHERE DBName='" + objCon.DATABASE + "'").Rows[0][0].ToString();
            return abc;
        }
        public static string RevertME()
        {
            return "AREYOUCRAZYTOUSETHISFUNCTION:)";
        }
        public static bool IsApplyCategory(string sID)
        {
            {
                string[] varCon;
                varCon = Regex.Split(AccCatID, ",");
                for (int x = 0; x < varCon.Length; x++)
                {
                    if (sID.Trim() == varCon[x].Trim())
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public static bool Is_Permited(string frmName, int int_event)
        { 
                string frmID;
                    frmID = objComCon.Fill_Table("Select formID From dbo.mst_FormMaster where formName='" + frmName + "'").Rows[0][0].ToString();
                if (Is_SuperUser == true)
                {
                    return true;
                }
                if (Is_Admin == true)
                {
                    foreach (DataRow DR in TB_UserPR.Rows)
                    {
                        if (DR[0].ToString() == frmID) { return false; }
                    }
                    return true;
                }
                else //Normal User
                {
                    foreach (DataRow DR in TB_UserPR.Rows)
                    {
                        if (DR[0].ToString() == frmID && DR[int_event].ToString() == "True") { return true; }
                    }
                }
                return false; 
        }
        public static bool ExporttoExceL(C1FlexGrid flex, int HIDECols, bool isGroup)
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = "xls";
                dlg.FileName = "*.xls";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return false;
                SaveSheet(flex, dlg.FileName.ToString(), HIDECols, isGroup);
                return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        public static void SaveSheet(C1FlexGrid flex, string strPath, int HIDECols, bool isGroup)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            try
            {
                object misValue = System.Reflection.Missing.Value;
                xlApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                int i = 0;
                int j = 1;
                int CL = 0;
                string sql = "select company_logo from mst_CompanyGenaral";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                byte[] CompLogo = (byte[])DT.Rows[0]["Company_Logo"];
                xlWorkSheet.Cells[1, 20] = byteArrayToImage((byte[])DT.Rows[0]["Company_Logo"]);
                xlWorkSheet.Cells[1, 10] = "Royal Holidays Pvt Ltd";
                Microsoft.Office.Interop.Excel.Range curcell = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 10];
                curcell.EntireRow.Font.Bold = true;
                curcell.Cells.Interior.Color = 36;
                for (i = HIDECols; i <= flex.Cols.Count - 1; i++)
                {
                    xlWorkSheet.Cells[1, j] = flex.Cols[i].Caption.ToString();
                    j = j + 1;
                }
                for (i = isGroup?2:1; i <= flex.Rows.Count - 1; i++)
                {
                    j = 1;
                    for (CL = HIDECols; CL <= flex.Cols.Count - 1; CL++)
                    {
                        if (isGroup == false)
                        {
                            xlWorkSheet.Cells[i + 1, j] = flex[i, CL];
                        }
                        else
                        {
                            xlWorkSheet.Cells[i, j] = flex[i, CL];
                        }
                        j = j + 1;
                    }
                }
                xlWorkBook.SaveAs(strPath, xlWorkBook.FileFormat, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
                MessageBox.Show("Exported Sucessfully", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public static void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        public static string CleanInput(string strIn)
        {
                return Regex.Replace(strIn, @"[^\w\.@-]", "", RegexOptions.None);
        }
        static int i = 0;
        public static void FilterCharacters(string s, ErrorProvider e, TextBox tb)
        {
            if (Regex.IsMatch(s, "[/*?\"<>%:|]") || Regex.IsMatch(s, @"\\"))
            {
                i = 1;
                e.Clear();
                e.SetError(tb, "Name can't contain any of the following characters: \n\t\t\\ / \\ : * ? \" < > %");
                string[] toRplace = { "*", "/", "<", ">", "%", ":", "|", "?", "\"", "\\" };
                foreach (string str in toRplace)
                {
                    s = s.Replace(str, "");
                }
                tb.Text = s.Trim();
                tb.SelectionStart = s.Length;
            }
            else
            {
                if (i == 0)
                {
                    e.Clear();
                }
                else
                {
                    i = 0;
                }
            }
        }
        public static Boolean Check_For_Is_Director(int UserID)
        {
                string ssql = "SELECT IsNull(IsDirector,0)AS IsDirector FROM mst_UserMaster " +
                        "Where ID=" + UserID + "";
                if (Convert.ToBoolean(Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(ssql).Rows[0]["IsDirector"]))
                    return true;
                else
                    return false;
        }
        public static string Get_Driver_Name(Boolean IsDriver, int DriverID)
        {
                string name = "";
                if (IsDriver)
                {
                    name = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,DriverName FROM vw_TR_DriverVSVehicle WHERE ID=" + DriverID + " AND IsNull(IsActive,0)=1 ORDER BY DriverName").Rows[0]["DriverName"].ToString();
                }
                else //RETRIEVE GUIDE DETAILS
                {
                    name = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT  ID,Name FROM vwGuideVsEmployee WHERE ID=" + DriverID + " AND IsNull(IsActive,0)=1 ORDER BY Name").Rows[0]["Name"].ToString();
                }
                return name;
        }
        public static string Check_For_Valid_NIC(string NIC)
        {
                return "";
        }
        public static DataTable rates_MarketAndSeason_Wise(int Hotel, int Market, string Date,
        int RoomType, int Basis, int Occupancy)
        {
            string sql = "";
            DataTable DT = new DataTable();
            try
            {
                if (RoomType == 0)
                    RoomType = DefaultHotelRoomType(Hotel);
                if (Basis == 0)
                    Basis = DefaultHotelBasis(Hotel);
                sql = "SELECT SeasonID,Season,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "RoomTypeID,RoomTypeName,BasisID,BasisTypeName,OccupancyID,Occupancy," +
                "ISNULL(Tax,0)AS Tax,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "ISNULL(GuideRoomCost,0)AS GuideRoomCost,SeasonFrom,SeasonTo" +
                " FROM vwHotelReferance" +
                " WHERE HotelID=" + Hotel + " AND MarketID=" + Market + "" +
                " AND SeasonFrom <='" + Date + "' AND SeasonTo>='" + Date + "'" +
                " AND RoomTypeID=" + RoomType + " AND BasisID=" + Basis + "" +
                " AND OccupancyID=" + Occupancy + "";
                return DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return DT;
            }
        }
        public static DataTable rates_Market_Wise(int Hotel, int Market, int Occupancy,
            int RoomType, int Basis)
        {
            string sql = "";
            DataTable DT = new DataTable();
            try
            {
                if (RoomType == 0)
                    RoomType = DefaultHotelRoomType(Hotel);
                if (Basis == 0)
                    Basis = DefaultHotelBasis(Hotel);
                sql = "SELECT SeasonID,Season,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "RoomTypeID,RoomTypeName,BasisID,BasisTypeName,OccupancyID,Occupancy," +
                "ISNULL(Tax,0)AS Tax,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "ISNULL(GuideRoomCost,0)AS GuideRoomCost,SeasonFrom,SeasonTo" +
                " FROM vwHotelReferance" +
                " WHERE HotelID=" + Hotel + " AND MarketID=" + Market + "" +
                " AND RoomTypeID=" + RoomType + " AND BasisID=" + Basis + "" +
                " AND OccupancyID=" + Occupancy + "";
                return DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return DT;
            }
        }
        public static DataTable rates_Season_Wise(int Hotel, string Date,
            int RoomType, int Basis, int Occupancy)
        {
            string sql = "";
            DataTable DT = new DataTable();
            try
            {
                if (RoomType == 0)
                    RoomType = DefaultHotelRoomType(Hotel);
                if (Basis == 0)
                    Basis = DefaultHotelBasis(Hotel);
                sql = "SELECT SeasonID,Season,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "RoomTypeID,RoomTypeName,BasisID,BasisTypeName,OccupancyID,Occupancy," +
                "ISNULL(Tax,0)AS Tax,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "ISNULL(GuideRoomCost,0)AS GuideRoomCost,SeasonFrom,SeasonTo" +
                " FROM vwHotelReferance" +
                " WHERE HotelID=" + Hotel + "" +
                " AND SeasonFrom <='" + Date + "' AND SeasonTo>='" + Date + "'" +
                " AND RoomTypeID=" + RoomType + " AND BasisID=" + Basis + "" +
                " AND OccupancyID=" + Occupancy + "";
                return DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return DT;
            }
        }
        public static DataTable rates_Just_Hotel(int Hotel, int RoomType, int Basis, int Occupancy)
        {
            string sql = "";
            DataTable DT = new DataTable();
            try
            {
                if (RoomType == 0)
                    RoomType = DefaultHotelRoomType(Hotel);
                if (Basis == 0)
                    Basis = DefaultHotelBasis(Hotel);
                sql = "SELECT SeasonID,Season,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "RoomTypeID,RoomTypeName,BasisID,BasisTypeName,OccupancyID,Occupancy," +
                "ISNULL(Tax,0)AS Tax,ISNULL(PriceWithoutTax,0)AS PriceWithoutTax," +
                "ISNULL(GuideRoomCost,0)AS GuideRoomCost,SeasonFrom,SeasonTo" +
                " FROM vwHotelReferance" +
                " WHERE HotelID=" + Hotel + "" +
                " AND RoomTypeID=" + RoomType + " AND BasisID=" + Basis + "" +
                " AND OccupancyID=" + Occupancy + "";
                return DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return DT;
            }
        }
        public static Int32 DefaultHotelRoomType(int HotelID)
        {
            try
            {
                string ssql = "SELECT DefRoomTypeID,DefBasisID FROM mst_HotelDetails" +
                              " WHERE ID=" + HotelID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    return Convert.ToInt32(DT.Rows[0]["DefRoomTypeID"].ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }
        public static Int32 DefaultHotelBasis(int HotelID)
        {
            try
            {
                string ssql = "SELECT DefRoomTypeID,DefBasisID FROM mst_HotelDetails" +
                              " WHERE ID=" + HotelID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0)
                {
                    return Convert.ToInt32(DT.Rows[0]["DefBasisID"].ToString());
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }
        public static DataTable getCompanyDetails(int CompID)
        {
                string sql = "SELECT ID,Company_Logo,DisplayName,Telephone,Fax,E_Mail,E_MailTo,Web,Physical_Address" +
                           " FROM mst_CompanyGenaral WHERE ID=" + CompID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                return DT;
        }
        public static DataTable Get_Company_ContactPersons_Details()
        {
                string sql = "SELECT * FROM mst_OtherSettings";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                DataTable dt = Get_UserName();
                DT.Columns.Add("CreatedBy", typeof(string));
                foreach (DataRow dr in DT.Rows)
                {
                    dr["CreatedBy"] = dt.Rows[0]["UserName"].ToString();
                }
                return DT;
        }
        public static DataTable Get_UserName()
        {
            string sql = "SELECT UserName FROM mst_UserMaster WHERE ID =" + UserID + "";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(sql);
            return DT;
        }
        public static Boolean validateEmptyTextBox(TextBox txtBox, string Name)
        {
            try
            {
                if (txtBox.Text.Trim() == "")
                {
                    MessageBox.Show("" + Name.Trim() + " is a required Filed", "Validating", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBox.Select();
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
        public static Boolean validateEmptyDropdownBox(DropDowns.DropSearch drpDown, string Name)
        {
            try
            {
                if (drpDown.SelectedValue + "".Trim() == "")
                {
                    MessageBox.Show("" + Name.Trim() + " is a required Filed", "Validating", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    drpDown.Focus();
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
        public static void clearFlexGrid_Color(C1FlexGrid grd)
        {
            try
            {
                C1.Win.C1FlexGrid.CellStyle DEF = grd.Styles.Add("DEF");
                DEF.BackColor = ColorTranslator.FromHtml("#FBFBEF");
                for (int c = 1; c < grd.Rows.Count - 1; c++)
                {
                    grd.Rows[c].Style = DEF;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public static decimal check_For_Prepayments(int HotelID)
        {
            decimal val = 0.00m;
            try
            {
                string ssql = "SELECT (SUM(ISNULL(TotPaid,0))- (SUM(ISNULL(ActualPayable,0)) + SUM(ISNULL(Settled,0))))Balance" +
                              " FROM vw_ManagePaidHotels WHERE ISNULL(TotPaid,0)!=ISNULL(Settled,0)" +
                               " AND ISNULL(TotPaid,0)<>0 AND HotelID='" + HotelID.ToString().Trim() + "'";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if(DT.Rows.Count>0)
                {
                    if (DT.Rows[0]["Balance"] + "".Trim() != "")
                    {
                        val = Convert.ToDecimal(DT.Rows[0]["Balance"]);
                    }
                }
                return val;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return val;
            }
        }
        public static void selected_Row_Colour_Change(C1FlexGrid grd, string voucherID, int Col)
        {
            try
            {
                C1.Win.C1FlexGrid.CellStyle SEL = grd.Styles.Add("SEL");
                SEL.BackColor = ColorTranslator.FromHtml("#BCF5A9");
                C1.Win.C1FlexGrid.CellStyle PAD = grd.Styles.Add("PAD");
                PAD.BackColor = ColorTranslator.FromHtml("#F3F781");
                C1.Win.C1FlexGrid.CellStyle DEF = grd.Styles.Add("DEF");
                DEF.BackColor = ColorTranslator.FromHtml("#FBFBEF");
                decimal val = 0.00m;
                string curVID = "";
                int HotelID= Convert.ToInt32(Classes.clsGlobal.objCon.Fill_Table("SELECT HotelID FROM dbo.trn_CityItinerary WHERE VoucherID='"+voucherID.Trim()+"'").Rows[0]["HotelID"]);
                for (int c = 1; c < grd.Rows.Count - 1; c++)
                {
                    val = check_For_Prepayments(HotelID);
                    if (val > 0)
                    {
                        curVID = grd[c, Col] + "".ToString().Trim();
                        if (curVID == "")
                            continue;
                        if (voucherID == curVID)//SHOULD COLOUR
                        {
                            grd.Rows[c].Style = PAD;
                        }
                        else
                        {
                            grd.Rows[c].Style = DEF;
                        }
                    }
                    else
                    {
                        curVID = grd[c, Col] + "".ToString().Trim();
                        if (curVID == "")
                            continue;
                        if (voucherID == curVID)//SHOULD COLOUR
                        {
                            grd.Rows[c].Style = SEL;
                        }
                        else
                        {
                            grd.Rows[c].Style = DEF;
                        }
                    }
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public static string Prepayments_Status(string voucherID, int HotelID)
        {
            string val = "";
            decimal amt=0.00m;
            try
            {
                if (voucherID.Trim() == "")
                    return val;
                string ssql = "SELECT SUM(ISNULL(TotPaid,0))TotPaid" +
                              " FROM act_ChangeHotelPayments WHERE VoucherID='" + voucherID.ToString().Trim() + "'";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DT.Rows.Count > 0 && DT.Rows[0]["TotPaid"] + "".Trim() != "")
                {
                    amt = Convert.ToDecimal(DT.Rows[0]["TotPaid"]);
                    val = "This booking has already paid " + amt + " and please paid the rest.";
                }
                else
                {
                    amt = check_For_Prepayments(HotelID);
                    if (amt > 0)
                    {
                        val = "There Is a " + amt + " Unsettled amount For This Hotel";
                    }
                }                               
                return val;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return val;
            }
        }
        public static C1.Win.C1FlexGrid.Column AddCol(C1FlexGrid dg, int colID, string caption, int width=0)
        {
            return AddCol(dg, colID, caption, width, "",false);
        }
        public static C1.Win.C1FlexGrid.Column AddCol(C1FlexGrid dg, int colID, string caption, int width, Type type)
        {
            C1.Win.C1FlexGrid.Column c = AddCol(dg, colID, caption, width, "", false);
            c.DataType = type;
            return c;
        }
        public static C1.Win.C1FlexGrid.Column AddCol(C1FlexGrid dg, int colID, string caption, int width, bool IsCombo)
        {
            C1.Win.C1FlexGrid.Column c = AddCol(dg, colID, caption, width, "", true);
            return c;
        }
        public static C1.Win.C1FlexGrid.Column AddCol(C1FlexGrid dg, int colID, string caption, int width, string format)
        {
            return AddCol(dg, colID, caption, width, format, false);
        }
        public static C1.Win.C1FlexGrid.Column AddCol(C1FlexGrid dg, int colID, string caption, int width, string format, bool IsCombo)
        { 
            if (dg.Cols.Count <= colID)    dg.Cols.Count = colID + 1;  
            dg.Cols[colID].Width = width;
            dg.Cols[colID].Caption = caption;
            dg.Cols[colID].Format = format; 
            if (IsCombo)  dg.Cols[colID].ComboList = "..."; 
            return dg.Cols[colID];
        }
        public static Boolean Check_For_TourCompleteness(string TourID)
        {
            try
            {
                string ssql = "SELECT ISNULL(IsCompleted,0)IsCompleted FROM act_Profit_Lose WHERE TourID=" + TourID + "";
                DataTable dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (dt.Rows.Count > 0)  return Convert.ToBoolean(dt.Rows[0]["IsCompleted"]);  else  return false;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        public static Boolean Check_For_Advances_Settled(string tourid, string personID, bool IsDriver)
        {
            try
            {                
                string ssql = "SELECT ISNULL(IsSettled,0)IsSettled FROM trn_TourAdvance WHERE TransID=" + tourid + ""+
                              " AND IsDriver=" + IsDriver + " AND DriverID="+personID+"";
                DataTable dt = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsSettled"]))
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return false;
            }
        }
        public static DataTable get_Default_Account_Details(string name)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "SELECT AccID FROM def_Account WHERE AccName='"+name+"'";
                string accID = Classes.clsConnection.getSingle_Value_Using_Reader(sql);
                dt = Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Account,AccountTypeID,AccountType FROM vw_acc_Acounts WHERE ID=" + accID + "");
                return dt;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return dt;
            }
        }
        public static DataTable get_Default_Account_Details_ByName(string name)
        { 
            try
            { 
                return Classes.clsGlobal.objCon.Fill_Table( "SELECT ID,Account,AccountTypeID,AccountType FROM vw_acc_Acounts WHERE Account LIKE '"+name.Trim()+"'");
                   }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return new DataTable();
            }
        }
    }
}
