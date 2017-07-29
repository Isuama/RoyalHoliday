using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Tourist_Management.User_Controls
{
    public partial class ucPayments : UserControl
    {
        enum GRD { ID, accountTypeID, accountType, accountID, accountName, IouID, IouVoucherID, Memo, debit, credit, billable, IsDeleted };
        DataTable dtIOU, dtPayment = new DataTable();
        string VoucherID, fType = "";
        private const string msghd = "Payment";
        decimal creditTotal = 0;
        double Syscode = 0;
        int InsMode = 0;
        Boolean bLoad = false, filterByPerson = false;
        Form frm;
        private void btnCancel_Click(object sender, EventArgs e) { frm.Close(); }
        public ucPayments() { InitializeComponent(); }
        public string FormType { set { fType = value; } }
        public Form form { set { frm = value; } }
        public int Mode { get { return InsMode; } set { InsMode = value; } }
        public double SystemCode { get { return Syscode; } set { Syscode = value; } }
        private void ucPayments_Load(object sender, EventArgs e) { }
        public void Intializer()
        {
            try
            {
                lblCredit.Text = lblDebit.Text = "";
                dtIOU = new DataTable();
                dtIOU.Columns.Add("ID", typeof(double));
                dtIOU.Columns.Add("VoucherID", typeof(string));
                Fill_Control(); Grd_Initializer(); Set_Form_Type(); Set_Voucher_No();
                if (InsMode == 0 && fType.Trim() == "IOU".Trim())
                {
                    int accTypeID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccID FROM def_Account WHERE AccName='Advance'").Rows[0]["AccID"]);
                    string SqlQuery = "SELECT ID,Account,AccountTypeID,AccountType FROM vw_acc_Acounts WHERE ID=" + accTypeID + "";
                    DataTable DTAll = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    grdPay[grdPay.Row, (int)GRD.accountTypeID] = DTAll.Rows[0]["AccountTypeID"];
                    grdPay[grdPay.Row, (int)GRD.accountType] = DTAll.Rows[0]["AccountType"];
                    grdPay[grdPay.Row, (int)GRD.accountID] = DTAll.Rows[0]["ID"];
                    grdPay[grdPay.Row, (int)GRD.accountName] = DTAll.Rows[0]["Account"];
                    grdPay[grdPay.Row, (int)GRD.Memo] = "Advance Payment";
                    grdPay[grdPay.Row, (int)GRD.debit] = "0".Trim();
                }
                if (InsMode != 0) { Fill_Data(); dtpPaidDate.Enabled = drpPayableTo.Enabled = false; }
                set_Authority();
                if (fType.Trim() == "OIE".Trim())
                {
                    chkPrint.Checked = chkPrint.Enabled = btnPrint.Enabled = false;
                    grdPay.Cols[(int)GRD.Memo].Width = 270;
                    grdPay.Cols[(int)GRD.IouVoucherID].Width = 0;
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Fill_Control()
        {
            try
            {
                FillDropDown(cmbCompany, "SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
                FillDropDown(drpPayableTo, "SELECT ID,Name + ' (' + Type + ')' FROM vw_ALL_PERSONS WHERE Name<>'' ORDER BY Name");
                FillDropDown(drpPayee, "SELECT ID,Name + ' (' + Type + ')' FROM vw_ALL_PERSONS WHERE Name<>'' ORDER BY Name");
                FillDropDown(cmbCurrency, "SELECT ID,Currency FROM mst_Currency Where IsNull(IsActive,0)=1 ORDER BY ID");
                FillDropDown(cmbExchangeToAccount, "SELECT ID,AccountType FROM comAcc_AccountTypes Where IsNull(IsActive,0)=1 AND AccountTypeID=1 AND ID<>1 ORDER BY ID");
                dtpChkDate.Value = dtpPaidDate.Value = Tourist_Management.Classes.clsGlobal.CurDate();
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private decimal get_Advance_Amount()
        {
            try
            {
                decimal amt = 0;
                string personName = drpPayableTo.SelectedText + "", qry;
                int personID = Convert.ToInt32(drpPayableTo.SelectedValue);
                if (rdbDriver.Checked) qry = "SELECT ISNULL(SUM(Amount),0)Amount FROM vw_trn_Tour_Advance_Driver WHERE DriverName LIKE '" + personName.Replace("(Driver)", "").Replace("(Guid)", "").Trim() + "' AND TransID=" + txtTourNo.Text.Trim() + "";
                else qry = "SELECT ISNULL(SUM(Amount),0)Amount FROM vw_trn_Tour_Advance WHERE DriverName LIKE '" + personName.Replace("(Driver)", "").Replace("(Guid)", "").Trim() + "' AND TransID=" + txtTourNo.Text.Trim() + "";
                string amount = Classes.clsConnection.getSingle_Value_Using_Reader(qry);
                if (amount + "" != "") amt = Convert.ToDecimal(amount);
                return amt;
            }
            catch (Exception ex) { db.MsgERR(ex); return 0; }
        }
        private void set_Default_IOU()
        {
            try
            {
                grdPay.Rows.Count = 1;
                grdPay.Rows.Count = 100;
                if (drpPayableTo.SelectedValue + "" == "") return;
                decimal amount = get_Advance_Amount();
                if (amount > 0)
                {
                    DataTable DT;
                    if (rdbDriver.Checked) DT = Classes.clsGlobal.get_Default_Account_Details("Tour Advance");
                    else DT = Classes.clsGlobal.get_Default_Account_Details("Guide Advance");
                    if (DT.Rows.Count > 0)
                    {
                        grdPay[1, (int)GRD.ID] = 0;
                        grdPay[1, (int)GRD.accountTypeID] = DT.Rows[0]["AccountTypeID"];
                        grdPay[1, (int)GRD.accountType] = DT.Rows[0]["AccountType"];
                        grdPay[1, (int)GRD.accountID] = DT.Rows[0]["ID"];
                        grdPay[1, (int)GRD.accountName] = DT.Rows[0]["Account"];
                        grdPay[1, (int)GRD.Memo] = "Advance Payment";
                        grdPay[1, (int)GRD.debit] = amount.ToString();
                        grdPay[1, (int)GRD.billable] = 1;
                        btnSettleRest_Click(null, null);
                    }
                    lblError.Visible = false;
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void set_Default_CPY()
        {
            try
            {
                grdPay.Rows.Count = 1;
                grdPay.Rows.Count = 100;
                if (drpPayableTo.SelectedValue + "" == "") return;
                int personID = Convert.ToInt32(drpPayableTo.SelectedValue);
                string val, name = drpPayableTo.SelectedText;
                if (name + "" == "") return;
                if (rdbDriver.Checked)
                val = Classes.clsConnection.getSingle_Value_Using_Reader("SELECT DriverID FROM vw_trn_BasicTransport WHERE TransID=" + txtTourNo.Text.Trim() + " AND Name LIKE '" + name + "'");
                else val = Classes.clsConnection.getSingle_Value_Using_Reader("SELECT GuideID FROM vw_trn_GuideDetails WHERE TransID=" + txtTourNo.Text.Trim() + " AND Name LIKE '" + name + "'");
                if (Convert.ToInt32(val) <= 0) return;
                personID = Convert.ToInt32(val);
                decimal amount;
                if (rdbDriver.Checked) amount = Convert.ToDecimal(Classes.clsConnection.getSingle_Value_Using_Reader("select dbo.fun_CalculateDriverPayments(" + txtTourNo.Text + "," + personID + ")"));
                else amount = Convert.ToDecimal(Classes.clsConnection.getSingle_Value_Using_Reader("select dbo.fun_CalculateGuidePayments(" + txtTourNo.Text + "," + personID + ")"));
                if (amount > 0)
                {
                    dtPayment.Rows.Clear();
                    string[] vid = null;
                    if (dtPayment.Columns.Count == 0)
                    {
                        dtPayment.Columns.Add("AccountTypeID", typeof(string));
                        dtPayment.Columns.Add("AccountType", typeof(string));
                        dtPayment.Columns.Add("ID", typeof(string));
                        dtPayment.Columns.Add("Account", typeof(string));
                        dtPayment.Columns.Add("Amount", typeof(double));
                    }
                    DataTable dtIOUs = Classes.clsGlobal.objCon.Fill_Table("SELECT VoucherID FROM act_CashPayment WHERE TransID=" + txtTourNo.Text + " AND PayableTo=" + drpPayableTo.SelectedValue + " AND [Type]='IOU'");
                    if (dtIOUs.Rows.Count > 0)
                    {
                        vid = new string[dtIOUs.Rows.Count];
                        foreach (DataRow dr in dtIOUs.Rows)
                        {
                            vid[dtIOUs.Rows.IndexOf(dr)] = dr[0] + "";
                            set_Temp_DataTable("SELECT Amount,Settled FROM dbo.Fun_ReturnIOUTot() WHERE VoucherID='" + vid[dtIOUs.Rows.IndexOf(dr)].Trim() + "'", "Advance");
                        }
                    }
                    if (rdbDriver.Checked)
                    {
                        set_Temp_DataTable("SELECT ISNULL(PaidForKm,0)PaidForKm FROM trn_BasicTransport WHERE TransID=" + txtTourNo.Text + " AND DriverID=" + personID + "", "Tour Transport");
                        set_Temp_DataTable("SELECT ISNULL(PaidForBata,0)PaidForBata FROM trn_BasicTransport WHERE TransID=" + txtTourNo.Text + " AND DriverID=" + personID + "", "Batta");
                    }
                    else set_Temp_DataTable("SELECT ISNULL(PaidAmount,0)PaidAmount FROM trn_GuideDetails WHERE TransID=" + txtTourNo.Text + " AND GuideID=" + personID + "", "Guide Fee");
                    set_Other_Payment(rdbDriver.Checked, personID);
                    int row = 0;
                    string iouID, iouVoucher = "";
                    foreach (DataRow dr in dtPayment.Rows)
                    {
                        row = dtPayment.Rows.IndexOf(dr) + 1;
                        grdPay[row, (int)GRD.ID] = 0;
                        grdPay[row, (int)GRD.accountTypeID] = dr["AccountTypeID"];
                        grdPay[row, (int)GRD.accountType] = dr["AccountType"];
                        grdPay[row, (int)GRD.accountID] = dr["ID"];
                        grdPay[row, (int)GRD.accountName] = dr["Account"];
                        if (dr["Account"] + "" == "Advance")
                        {
                            if (vid[dtPayment.Rows.IndexOf(dr)] + "" != "") iouVoucher = vid[dtPayment.Rows.IndexOf(dr)].Trim();
                            iouID = Classes.clsConnection.getSingle_Value_Using_Reader("SELECT ID FROM act_CashPayment WHERE VoucherID='" + iouVoucher + "'");
                            if (iouID + "" != "" && Convert.ToDecimal(dr["Amount"]) > 0)
                            {
                                grdPay[row, (int)GRD.IouID] = iouID;
                                grdPay[row, (int)GRD.IouVoucherID] = iouVoucher;
                                grdPay[row, (int)GRD.Memo] = "Less - IOU No :" + iouVoucher;
                                grdPay[row, (int)GRD.credit] = dr["Amount"];
                            }
                        }
                        else
                        {
                            grdPay[row, (int)GRD.Memo] = dr["Account"];
                            grdPay[row, (int)GRD.debit] = dr["Amount"];
                        }
                        grdPay[row, (int)GRD.billable] = 1;
                    }
                    btnSettleRest_Click(null, null);
                    lblError.Visible = false;
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void set_Other_Payment(bool isDriver, int personID)
        {
            try
            {
                string val,name;
                if (isDriver) val = "1"; else val = "0";
                DataTable dtPay = Classes.clsGlobal.objCon.Fill_Table("SELECT Expense,ISNULL(Amount,0)Amount FROM vw_trn_Travel_Expenses WHERE ISNULL(IsPaid,0)=1 AND ISNULL(IsDeleted,0)<>1 AND ISNULL(IsDriver,0)=" + val.Trim() + " AND TransID=" + txtTourNo.Text + " AND DriverID=" + personID + "");
                DataTable dtTemp;
                decimal amt;
                foreach (DataRow dr in dtPay.Rows)
                {
                    name = dr[0] + "";
                    if (name == "") continue;
                    dtTemp = Classes.clsGlobal.get_Default_Account_Details_ByName(name);
                    amt = Convert.ToDecimal(dr[1]);
                    set_DataTable(dtTemp, amt);
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void set_Temp_DataTable(string qry, string name)
        {
            try
            {
                DataTable tempDT = Classes.clsGlobal.get_Default_Account_Details(name);
                decimal amt;
                string val = Classes.clsConnection.getSingle_Value_Using_Reader(qry);
                if ((val + "").Trim() == "") amt = 0; else amt = Convert.ToDecimal(val);
                if (tempDT.Rows.Count > 0) set_DataTable(tempDT, amt);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void set_DataTable(DataTable dtTemp, decimal amt)
        {
            try
            {
                string AccountTypeID = "", AccountType = "", ID = "", Account = "";
                if (dtTemp.Rows.Count <= 0) return;
                if (dtTemp.Rows[0]["AccountTypeID"] + "" == "") return;
                AccountTypeID = dtTemp.Rows[0]["AccountTypeID"] + "";
                AccountType = dtTemp.Rows[0]["AccountType"] + "";
                ID = dtTemp.Rows[0]["ID"] + "";
                Account = dtTemp.Rows[0]["Account"] + "";
                dtPayment.Rows.Add(AccountTypeID, AccountType, ID, Account, amt);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void set_Authority()
        {
            try
            {
                int userID = Classes.clsGlobal.UserID;
                bool isAdmin = Convert.ToBoolean(Classes.clsGlobal.objComCon.Fill_Table("SELECT ISNULL(IsAdmin,0) FROM mst_UserMaster WHERE ID=" + userID + "").Rows[0][0]);
                if (isAdmin) btnOk.Enabled = true;
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        public bool IN(object value, params object[] values) {
            foreach (object o in values) if (value.Equals(o)) return true; 
            return false;
        }
        public void FillDropDown(ComboBox cb, string sql)
        {  
            cb.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
        }
        public void FillDropDown(DropDowns.DropSearch cb, string sql)
        {
            FillDropDown(cb, Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql));
        }
        public void FillDropDown(DropDowns.DropSearch cb, DataTable dt)
        {
            cb.DataSource = dt;
        } 
        private void Grd_Initializer()
        {
            try
            {
                grdPay.Cols.Count = 12;
                grdPay.Rows.Count = 100;
                grdPay.Cols[(int)GRD.ID].Width = 00;///////
                grdPay.Cols[(int)GRD.ID].Caption = "ID";
                grdPay.Cols[(int)GRD.accountTypeID].Width = 00;///////
                grdPay.Cols[(int)GRD.accountTypeID].Caption = "Account Type ID";
                grdPay.Cols[(int)GRD.accountType].Width = 145;
                grdPay.Cols[(int)GRD.accountType].Caption = "Account Type";
                grdPay.Cols[(int)GRD.accountID].Width = 00;///////
                grdPay.Cols[(int)GRD.accountID].Caption = "Account Name ID";
                grdPay.Cols[(int)GRD.accountName].Width = 145;
                grdPay.Cols[(int)GRD.accountName].Caption = "Account Name";
                grdPay.Cols[(int)GRD.IouID].Width = 00;//////
                grdPay.Cols[(int)GRD.IouID].Caption = "IOU ID";
                grdPay.Cols[(int)GRD.IouVoucherID].Width = 103;
                grdPay.Cols[(int)GRD.IouVoucherID].Caption = "IOU VoucherID";
                grdPay.Cols[(int)GRD.Memo].Width = 167;
                grdPay.Cols[(int)GRD.Memo].Caption = "Memo";
                grdPay.Cols[(int)GRD.credit].Width = 85;
                grdPay.Cols[(int)GRD.credit].Caption = "Credit";
                grdPay.Cols[(int)GRD.debit].Width = 85;
                grdPay.Cols[(int)GRD.debit].Caption = "Debit";
                grdPay.Cols[(int)GRD.billable].Width = 61;
                grdPay.Cols[(int)GRD.billable].Caption = "Billable";
                grdPay.Cols[(int)GRD.IsDeleted].Width = 0;
                grdPay.Cols[(int)GRD.IsDeleted].Caption = "Is Deleted";
                grdPay.Rows[0].TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter;
                grdPay.Cols[(int)GRD.credit].Format = "##.##";
                grdPay.Cols[(int)GRD.debit].Format = "##.##";
                grdPay.Cols[(int)GRD.billable].DataType = Type.GetType(" System.Boolean");
                grdPay.Cols[(int)GRD.IsDeleted].DataType = Type.GetType(" System.Boolean");
                grdPay.Cols[(int)GRD.accountType].ComboList = "...";
                grdPay.Cols[(int)GRD.accountName].ComboList = "...";
                grdPay.Cols[(int)GRD.IouVoucherID].ComboList = "...";
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Clear_All()
        {
            try
            {
                lblCredit.Text = lblDebit.Text = txtIOUNo.Text = txtSettledIOU.Text = txtBalanceIOU.Text = txtRefNo.Text = txtDescription.Text = txtChkNo.Text = "";
                rdbDriver.Visible = rdbGuide.Visible = rdbAgent.Visible = rdbHotel.Visible = rdbOther.Visible = false;
                drpPayableTo.setSelectedValue(null);
                drpPayee.setSelectedValue(null);
                cmbCompany.Enabled = true;
                dtpChkDate.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                dtpPaidDate.Value = Tourist_Management.Classes.clsGlobal.CurDate();
                grdPay.Rows.Count = 1;
                grdPay.Rows.Count = 100;
                lblVoucherNo.Text = "";
                Fill_Control();
                Set_Voucher_No();
                Set_Form_Type();
                dtIOU.Rows.Clear();
                grdPay.Cols[(int)GRD.Memo].Width = 270;
                grdPay.Cols[(int)GRD.IouVoucherID].Width =103;//chathuri
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        public void Set_Voucher_No()
        {
            try
            {
                int compID = Convert.ToInt32(cmbCompany.SelectedValue);
                string code = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CompanyCode FROM mst_CompanyGenaral WHERE ID=" + compID + "").Rows[0]["CompanyCode"].ToString().Trim(); 
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT MAX(UniqueID)ID FROM act_CashPayment WHERE CompID=" + compID + " AND Type='" + fType + "' GROUP BY [Type]");
                if (DT.Rows.Count > 0 && DT.Rows[0]["ID"] + "" != "") VoucherID = (Convert.ToDouble(DT.Rows[0]["ID"]) + 1).ToString().Trim(); else VoucherID = "1001".Trim();
                VoucherID = (code + "/" + fType + "/" + VoucherID).Trim();
                lblVoucherNo.Text = VoucherID;
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        void setView(string Main, string Type, string color, string payrec = "Payable To")
        {
            lblMain.Text = Main;
            lblType.Text = Type;
            this.BackColor = ColorTranslator.FromHtml(color);
            lblPayRec.Text = payrec;        
        }
        private void Set_Form_Type()
        { 
                gbIOU.Enabled = true;
                gbChk.Enabled = gbCurrency.Enabled = false;
                lblPayRec.Text = "Payable To";
                if (fType == "IOU")
                {
                    grdPay.Cols[(int)GRD.Memo].Width = 270;
                    grdPay.Cols[(int)GRD.IouVoucherID].Width = 0;
                    gbIOU.Enabled = false;
                    setView("IOU Settlement", "IOU No :", "#FBEFEF");// light red 
                }
                else if (fType == "CPY")
                {
                    setView("Cash Payment Voucher", "Voucher No :", "#EFFBF5");// light green 
                }
                else if (fType == "REC")
                {
                    gbCurrency.Enabled = true;
                    setView("Receipt", "Receipt No :", "#EFF5FB", "Received From");// light blue 
                }
                else if (fType == "CHQ")
                {
                    btnSettleRest.Visible = false;                    gbChk.Enabled = true;
                    setView("Cheque", "Cheque Voucher No :", "#F2F2F2", "Paid To");// light grey 
                }
                else if (fType == "CTN")
                {
                    gbCurrency.Enabled = true;
                    setView("Credit Transfer Note", "CTN No :", "#EFF5FB", "Received From");// light blue 
                }
                else if (fType == "DTN")
                {
                    gbCurrency.Enabled = true;
                    setView("Debit Transfer Note", "DTN No :", "#EFF5FB", "Received From");// light blue 
                }
                else if (fType == "BIL")
                {
                    gbCurrency.Enabled = true;
                    setView("BIL Payable", "BIL No :", "#ccF5FB", "Received From");// light blue 
                }
        }
        private void grdPay_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                string[] SelText;
                Other.frmSearchGrd frm;
                DataTable DTAccType, DTAcc, DTAll;
                string SqlQuery;
                if (e.Col == grdPay.Cols[(int)GRD.accountType].Index)
                {
                    SqlQuery = "SELECT ID,AccountType [Type] FROM comAcc_AccountTypes Where ID<=16";
                    DTAccType = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTAccType;
                    frm.SubForm = new Accounts.frmChartOfAccount();
                    frm.Width = grdPay.Cols[(int)GRD.accountType].Width;
                    frm.Height = grdPay.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdPay);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdPay[grdPay.Row, (int)GRD.accountTypeID] = SelText[0];
                        grdPay[grdPay.Row, (int)GRD.accountType] = SelText[1];
                        if (grdPay[grdPay.Row, (int)GRD.ID] + "" == "") grdPay[grdPay.Row, (int)GRD.ID] = 0;
                    }
                    grdPay[grdPay.Row, (int)GRD.accountID] = "";
                    grdPay[grdPay.Row, (int)GRD.accountName] = "";
                    grdPay[grdPay.Row, (int)GRD.IouID] = "";
                    grdPay[grdPay.Row, (int)GRD.IouVoucherID] = "";
                    grdPay[grdPay.Row, (int)GRD.Memo] = "";
                }
                else if (e.Col == grdPay.Cols[(int)GRD.accountName].Index)
                {
                    int accountType, compID = Convert.ToInt32(cmbCompany.SelectedValue);
                    SqlQuery = "SELECT ID,AccountType [Name] FROM comAcc_AccountTypes WHERE ISNULL(IsActive,0)=1 AND (ISNULL(CompanyID,0)=0 OR CompanyID=" + compID + ")";
                    if (grdPay[grdPay.Row, (int)GRD.accountTypeID] + "" != "")
                    {
                        accountType = Convert.ToInt32(grdPay[grdPay.Row, (int)GRD.accountTypeID]);
                        SqlQuery += " AND AccountTypeID=" + accountType + "";
                    }
                    DTAcc = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = DTAcc;
                    frm.SubForm = new Accounts.frmChartOfAccount();
                    frm.Width = grdPay.Cols[(int)GRD.accountName].Width;
                    frm.Height = grdPay.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdPay);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdPay[grdPay.Row, (int)GRD.accountID] = SelText[0];
                        grdPay[grdPay.Row, (int)GRD.accountName] = SelText[1];
                        grdPay[grdPay.Row, (int)GRD.Memo] = SelText[1];
                        int accTypeID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccountTypeID FROM comAcc_AccountTypes WHERE ID=" + SelText[0] + "").Rows[0][0]);
                        SqlQuery = "SELECT ID,AccountType FROM comAcc_AccountTypes WHERE AccountTypeID=" + accTypeID + "";
                        DTAccType = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        grdPay[grdPay.Row, (int)GRD.accountTypeID] = DTAccType.Rows[0]["ID"];
                        grdPay[grdPay.Row, (int)GRD.accountType] = DTAccType.Rows[0]["AccountType"];
                        if (accTypeID == 1 && fType.Trim() != "CHQ") grdPay[grdPay.Row, (int)GRD.billable] = 0; else grdPay[grdPay.Row, (int)GRD.billable] = 1;
                        if (grdPay[grdPay.Row, (int)GRD.ID] + "" == "") grdPay[grdPay.Row, (int)GRD.ID] = 0;
                        grdPay[grdPay.Row, (int)GRD.IouID] = "";
                        grdPay[grdPay.Row, (int)GRD.IouVoucherID] = "";
                    }
                }
                else if (e.Col == grdPay.Cols[(int)GRD.IouVoucherID].Index)
                {
                    if (dtIOU.Rows.Count == 0) Add_IOU("");
                    frm = new Tourist_Management.Other.frmSearchGrd();
                    frm.DataSource = dtIOU;
                    frm.SubForm = new Accounts.frmPaymentsIOU();
                    frm.Width = grdPay.Cols[(int)GRD.IouVoucherID].Width;
                    frm.Height = grdPay.Height;
                    frm.StartPosition = FormStartPosition.Manual;
                    frm.Location = Tourist_Management.Classes.clsGlobal.GetCellLocation(grdPay);
                    SelText = frm.Open_Search();
                    if (SelText != null)
                    {
                        grdPay[grdPay.Row, (int)GRD.IouID] = SelText[0];
                        grdPay[grdPay.Row, (int)GRD.IouVoucherID] = SelText[1];
                        if (grdPay[grdPay.Row, (int)GRD.accountTypeID] + "" != "") return;
                        DataTable dt = Classes.clsGlobal.objCon.Fill_Table("SELECT Amount,Settled FROM dbo.Fun_ReturnIOUTot() WHERE VoucherID='" + SelText[1].Trim() + "'");
                        if (dt.Rows.Count > 0 && fType.Trim() != "IOU") grdPay[grdPay.Row, (int)GRD.credit] = (Convert.ToDecimal(dt.Rows[0]["Amount"]) - Convert.ToDecimal(dt.Rows[0]["Settled"])).ToString().Trim();
                        grdPay[grdPay.Row, (int)GRD.Memo] = "Less - IOU No :" + SelText[1];
                        int accTypeID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccID FROM def_Account WHERE AccName='Advance'").Rows[0]["AccID"]);
                        SqlQuery = "SELECT ID,Account,AccountTypeID,AccountType FROM vw_acc_Acounts WHERE ID=" + accTypeID + "";
                        DTAll = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                        grdPay[grdPay.Row, (int)GRD.accountTypeID] = DTAll.Rows[0]["AccountTypeID"];
                        grdPay[grdPay.Row, (int)GRD.accountType] = DTAll.Rows[0]["AccountType"];
                        grdPay[grdPay.Row, (int)GRD.accountID] = DTAll.Rows[0]["ID"];
                        grdPay[grdPay.Row, (int)GRD.accountName] = DTAll.Rows[0]["Account"];
                        if (grdPay[grdPay.Row, (int)GRD.ID] + "" == "") grdPay[grdPay.Row, (int)GRD.ID] = 0;
                    }
                }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void grdPay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Do you really want to delete this record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;
                grdPay.Rows[grdPay.Row][(int)GRD.credit] = "0.00".Trim();
                grdPay.Rows[grdPay.Row][(int)GRD.debit] = "0.00".Trim();
                string val = grdPay.Rows[grdPay.Row][(int)GRD.ID] + "";
                if (val != "" && val != "0")
                {
                    grdPay.Rows[grdPay.Row][(int)GRD.IsDeleted] = true;
                    C1.Win.C1FlexGrid.CellStyle deleted = grdPay.Styles.Add("deleted");
                    deleted.BackColor = ColorTranslator.FromHtml("#F78181");
                    grdPay.Rows[grdPay.Row].Style = grdPay.Styles["deleted"];
                }
                else
                {
                    if (grdPay.Rows[grdPay.Row][(int)GRD.IouID] + "" != "")
                    {
                        grdPay.Rows[grdPay.Row][(int)GRD.IouID] = "";
                        grdPay.Rows[grdPay.Row][(int)GRD.IouVoucherID] = "";
                    }
                    else grdPay.Rows.Remove(grdPay.Row);
                    grdPay.Rows[1].AllowEditing = true;
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                grdPay.Rows[grdPay.Row][(int)GRD.IsDeleted] = false;
                C1.Win.C1FlexGrid.CellStyle undoDeleted = grdPay.Styles.Add("undoDeleted");
                undoDeleted.BackColor = Color.Transparent;
                grdPay.Rows[grdPay.Row].Style = grdPay.Styles["undoDeleted"];
            }
        }
        private void grdPay_RowColChange(object sender, EventArgs e)
        {
            if (bLoad == true || grdPay.Rows.Count == 1) return;
            grdPay.Rows[1].AllowEditing = true;
            grdPay.Rows[grdPay.Row].AllowEditing = !(grdPay.Row != 1 && grdPay[grdPay.Row - 1, 0] == null);
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do You Want To Save This Record", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;
            if (!(Validate_Data() && Save_Procedure())) return;
            InsMode = 1;
            Fill_Data();
            set_Authority();
            if (chkPrint.Checked) Print_Payment();
        }
        private void Fill_Data()
        {
            try
            {
                string SqlQuery = "SELECT ID,TransID,CompID,VoucherID,PayableTo,CurrencyID,ReceivedRate,ExchangeRate, ExchangeDate,ExchangeToAccount,PaidDate,RefNo, Description,ChkNo,ChkDate,Payee,ISNULL(IsCancelled,0)IsCancelled, ISNULL(IsConfirm,0)IsConfirm,ISNULL(IsCompleted,0)IsCompleted FROM act_CashPayment Where ID=" + Syscode + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                btnOk.Enabled = true;
                lblStatus.Visible = cmbCompany.Enabled = rdbCancel.Checked = rdbConfirm.Checked = false;
                if (DT.Rows[0]["TransID"] + "" != "")
                {
                    Fill_For_Tour(DT.Rows[0]["TransID"] + "");
                    btnTour.Enabled = false;
                }
                if (Convert.ToBoolean(DT.Rows[0]["IsCancelled"]))
                {
                    rdbCancel.Checked = lblStatus.Visible = true;
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "THIS HAS BEEN CANCELLED";
                    btnOk.Enabled = false;
                }
                if (Convert.ToBoolean(DT.Rows[0]["IsConfirm"])) { rdbConfirm.Checked = lblStatus.Visible = true; lblStatus.ForeColor = Color.Green; lblStatus.Text = "THIS HAS BEEN CONFIRMED"; btnOk.Enabled = false; }
                if (Convert.ToBoolean(DT.Rows[0]["IsCompleted"])) { lblStatus.Visible = true; lblStatus.ForeColor = Color.Green; lblStatus.Text = "THIS IOU IS SETTLED"; btnOk.Enabled = false; }
                cmbCompany.SelectedValue = Convert.ToInt32(DT.Rows[0]["CompID"]);
                VoucherID = DT.Rows[0]["VoucherID"].ToString().Trim();
                lblVoucherNo.Text = VoucherID;
                drpPayableTo.setSelectedValue(DT.Rows[0]["PayableTo"].ToString().Trim());
                dtpPaidDate.Value = Convert.ToDateTime(DT.Rows[0]["PaidDate"]);
                cmbCurrency.SelectedValue = Convert.ToInt32(DT.Rows[0]["CurrencyID"]);
                txtRrate.Text = DT.Rows[0]["ReceivedRate"] + "";
                txtErate.Text = DT.Rows[0]["ExchangeRate"] + "";
                if (DT.Rows[0]["ExchangeDate"] + "" != "")
                {
                    dtpExchangeDate.Value = Convert.ToDateTime(DT.Rows[0]["ExchangeDate"]);
                    cmbExchangeToAccount.SelectedValue = Convert.ToInt32(DT.Rows[0]["ExchangeToAccount"]);
                }
                txtRefNo.Text = DT.Rows[0]["RefNo"].ToString().Trim();
                txtDescription.Text = DT.Rows[0]["Description"].ToString().Trim();
                if (DT.Rows[0]["ChkNo"] + "" != "")
                {
                    txtChkNo.Text = DT.Rows[0]["ChkNo"].ToString().Trim();
                    dtpChkDate.Value = Convert.ToDateTime(DT.Rows[0]["ChkDate"]);
                    drpPayee.setSelectedValue(DT.Rows[0]["Payee"].ToString().Trim());
                }
                SqlQuery = "SELECT ID,CashPayID,AccountTypeID,AccountType,AccountNameID,AccountName,IouID,VoucherID,Memo,Debit,Credit,ISNULL(Billable,0)Billable FROM vw_act_Payment_Details Where CashPayID=" + Syscode + " AND ISNULL(IsDeleted,0)<>1 ORDER BY SrNo";
                DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                int RowNumb = 1;
                if (DT.Rows.Count > 0 && DT.Rows[0][0] + "" != "")
                    foreach (DataRow r in DT.Rows)
                    {
                        C1.Win.C1FlexGrid.Row R = grdPay.Rows[RowNumb++];
                        R[(int)GRD.ID] = r["ID"];
                        R[(int)GRD.accountTypeID] = r["AccountTypeID"];
                        R[(int)GRD.accountType] = r["AccountType"];
                        R[(int)GRD.accountID] = r["AccountNameID"];
                        R[(int)GRD.accountName] = r["AccountName"];
                        if (  (r["IouID"] + "" != ""))
                        {
                            R[(int)GRD.IouID] = r["IouID"];
                            Add_IOU(r["IouID"].ToString());
                            R[(int)GRD.IouVoucherID] = Classes.clsGlobal.objCon.Fill_Table("SELECT VoucherID FROM act_CashPayment WHERE ID=" + r["IouID"].ToString().Trim() + "").Rows[0]["VoucherID"].ToString().Trim();
                        }
                        if (r["Debit"] + "" != "") R[(int)GRD.debit] = r["Debit"];
                        if (r["Credit"] + "" != "") R[(int)GRD.credit] = r["Credit"];
                        R[(int)GRD.Memo] = r["Memo"];
                        R[(int)GRD.billable] = Convert.ToBoolean(r["Billable"]);
                    }
            }
            catch (Exception ex) { db.MsgERR(ex); btnOk.Enabled = false; }
        }
        public void Set_IOU_Setlled_Balance(string iouNo)
        {
            try { txtIOUNo.Text = iouNo.Trim(); }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private Boolean Validate_Data()
        {
            try
            {
                string e = ""; Control c=null;
                if (fType.Trim() == "IOU" && txtTourNo.Text.Trim() != "" && (rdbDriver.Checked || rdbGuide.Checked))
                {
                    decimal amt = get_Advance_Amount();
                    calculate_Total();
                    if (creditTotal > amt)
                    {
                        MessageBox.Show("Maximum settle amount is " + amt.ToString() + "", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                string val;
                if (fType.Trim() == "CHQ" && txtChkNo.Text.Trim() == "") { e = "'Cheque No' cannot be blank."; c = txtChkNo; }
                else if (fType.Trim() == "CHQ" && drpPayee.SelectedValue + "" == "") { e = "'Payee' cannot be blank."; c = drpPayee; }
                else if (fType.Trim() == "CHQ" && (val = Classes.clsConnection.getSingle_Value_Using_Reader("SELECT VoucherID FROM act_CashPayment WHERE ChkNo='" + txtChkNo.Text.Trim() + "' AND ID<>" + Syscode + " AND ISNULL(IsCancelled,0)<>1") + "") != "")
                { e = "'Cheque No' is already exist in Voucher '" + val + "'."; c = txtChkNo; }
                else if (cmbCurrency.SelectedValue + "" != "1" && IN(fType.Trim()  , "REC","CTN","DTN") && txtRrate.Text.Trim() == "") { e = "'Received Rate' cannot be blank."; c = txtRrate; }
                else if (fType.Trim() != "OIE".Trim() && drpPayableTo.SelectedValue.ToString() == "") { e = "'Payable To' name cannot be blank."; c = drpPayableTo; }
                else if (txtChkNo.Text.Trim() != "" && drpPayee.SelectedValue.ToString() == "") { e = "'Payee' name cannot be blank."; c = drpPayee; }
                else if (!calculate_Total()) { e = "Unbalance in Credit And Debit Amount"; }
                if (e != "") { MessageBox.Show(e, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); if (c != null) c.Select(); return false; }
                int RowNumb = 1, col=0;
                while (grdPay[RowNumb, (int)GRD.accountTypeID] + "" != "")
                {
                    if (grdPay[RowNumb, (int)GRD.accountID] + "" == "") { e = "Please select an account for " + grdPay[RowNumb, (int)GRD.accountID].ToString().Trim() + ""; col = (int)GRD.accountName; }
                    else if (grdPay[RowNumb, (int)GRD.credit] + "" == "" && grdPay[RowNumb, (int)GRD.debit] + "" == "") { e = "Please enter CREDIT or DEBIT amount"; col = (int)GRD.credit; }
                    else if ((grdPay[RowNumb, (int)GRD.credit] + "" != "") && (!Classes.clsGlobal.IsNumeric(grdPay[RowNumb, (int)GRD.credit] + ""))) { e = "Please enter valid credit amount"; col = (int)GRD.credit; }
                    else if ((grdPay[RowNumb, (int)GRD.debit] + "" != "") && (!Classes.clsGlobal.IsNumeric(grdPay[RowNumb, (int)GRD.debit] + ""))) { e = "Please enter valid debit amount"; col = (int)GRD.debit; }
                    else if ((grdPay[RowNumb, (int)GRD.accountName] + "" == "Expense") && (grdPay[RowNumb, (int)GRD.credit] + "" != "")) { e = "Expense Account Cannot be Credited."; col = (int)GRD.credit; }
                    if (e != "")                    {                        MessageBox.Show(e, msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); if (c != null) c.Select();                        grdPay.Select(RowNumb, col); return false;                    }
                                        RowNumb++;
                }
                return true;
            }
            catch (Exception ex) { db.MsgERR(ex); return false; }
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
                if (Save_Tabs(objCom) == true)
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
            catch (Exception ex) { db.MsgERR(ex); return false; }
        }
        private Boolean Save_Tabs(System.Data.SqlClient.SqlCommand sqlCom)
        {
            try { return Save_Basic_Details(sqlCom) && Save_Pay_Details(sqlCom); }
            catch (Exception ex) { db.MsgERR(ex); return false; }
        }
        private Boolean Save_Basic_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            Boolean RtnVal = false;
            int PayAccount;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_act_CashPayment";
            sqlCom.Parameters.Clear();
            sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Syscode;
            sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
            sqlCom.Parameters.Add("@Type", SqlDbType.NChar, 10).Value = fType.Trim();
            sqlCom.Parameters.Add("@CompID", SqlDbType.Int).Value = Convert.ToInt32(cmbCompany.SelectedValue.ToString());
            if (txtTourNo.Text.Trim() != "") sqlCom.Parameters.Add("@TransID", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTourNo.Text);
            if (drpPayableTo.SelectedValue + "" == "" && fType.Trim() == "OIE".Trim()) sqlCom.Parameters.Add("@PayableTo", SqlDbType.Decimal).Value = 0;
            else sqlCom.Parameters.Add("@PayableTo", SqlDbType.Decimal).Value = Convert.ToDecimal(drpPayableTo.SelectedValue);
            PayAccount = get_PayAccount();
            if ((PayAccount != 0) == !IN(fType, "CTN", "DTN","BIL")) sqlCom.Parameters.Add("@PayAccount", SqlDbType.Int).Value = PayAccount;
            else
            {
                MessageBox.Show(IN(fType, "CTN", "DTN","BIL")?"Don't Select Cash Accounts":"Payable account could not found.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            sqlCom.Parameters.Add("@CurrencyID", SqlDbType.Int).Value = Convert.ToInt32(cmbCurrency.SelectedValue.ToString());
            if (txtRrate.Text.Trim() != "") sqlCom.Parameters.Add("@ReceivedRate", SqlDbType.Decimal).Value = Convert.ToDecimal(txtRrate.Text);
            if (txtErate.Text.Trim() != "")
            {
                sqlCom.Parameters.Add("@ExchangeRate", SqlDbType.Decimal).Value = Convert.ToDecimal(txtErate.Text);
                sqlCom.Parameters.Add("@ExchangeDate", SqlDbType.DateTime).Value = dtpExchangeDate.Value;
                sqlCom.Parameters.Add("@ExchangeToAccount", SqlDbType.Int).Value = Convert.ToInt32(cmbExchangeToAccount.SelectedValue.ToString());
            }
            sqlCom.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = dtpPaidDate.Value;
            sqlCom.Parameters.Add("@RefNo", SqlDbType.NVarChar, 50).Value = txtRefNo.Text;
            sqlCom.Parameters.Add("@Description", SqlDbType.NVarChar, 200).Value = txtDescription.Text;
            sqlCom.Parameters.Add("@VoucherID", SqlDbType.NVarChar, 50).Value = lblVoucherNo.Text;
            if (txtChkNo.Text.Trim() != "")
            {
                sqlCom.Parameters.Add("@ChkNo", SqlDbType.NVarChar, 50).Value = txtChkNo.Text;
                sqlCom.Parameters.Add("@ChkDate", SqlDbType.DateTime).Value = dtpPaidDate.Value;
                sqlCom.Parameters.Add("@Payee", SqlDbType.Decimal).Value = Convert.ToDecimal(drpPayee.SelectedValue.ToString().Trim());
            }
            sqlCom.Parameters.Add("@IsConfirm", SqlDbType.Int).Value = rdbConfirm.Checked ? "1" : "0";
            sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = rdbCancel.Checked ? "1" : "0";
            sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
            sqlCom.Parameters.Add("@InsMode", SqlDbType.Int).Value = InsMode;
            sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
            sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
            sqlCom.ExecuteNonQuery();
            if ((int)sqlCom.Parameters["@RtnValue"].Value == 1)
            {
                Syscode = Convert.ToDouble(sqlCom.Parameters["@ID"].Value);
                VoucherID = sqlCom.Parameters["@VoucherID"].Value.ToString();
                lblVoucherNo.Text = VoucherID.Trim();
                RtnVal = true;
            }
            return RtnVal;
        }
        private Int32 get_PayAccount()
        {
            try
            {
                int RowNumb = 1, payid = 0;
                while (grdPay[RowNumb, (int)GRD.accountTypeID] + "" != "")
                {
                    if ((Convert.ToInt32(grdPay[RowNumb, (int)GRD.accountTypeID]) == 1) )
                    {
                        payid = Convert.ToInt32(grdPay[RowNumb, (int)GRD.accountID]);
                        return payid;
                    }
                    RowNumb++;
                }
                return payid;
            }
            catch (Exception ex) { db.MsgERR(ex); return 0; }
        }
        private Boolean Save_Pay_Details(System.Data.SqlClient.SqlCommand sqlCom)
        {
            int RowNumb = 1;
            Boolean RtnVal = true;
            sqlCom.CommandType = CommandType.StoredProcedure;
            sqlCom.CommandText = "spSave_act_CashPayment_Detail";
            while (grdPay[RowNumb, (int)GRD.accountTypeID] + "" != "")
            {
                sqlCom.Parameters.Clear();
                sqlCom.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.ID]);
                sqlCom.Parameters["@ID"].Direction = ParameterDirection.InputOutput;
                sqlCom.Parameters.Add("@CashPayID", SqlDbType.Decimal).Value = Syscode;
                sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                sqlCom.Parameters.Add("@AccountNameID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.accountID]);
                sqlCom.Parameters.Add("@Memo", SqlDbType.NVarChar, 100).Value = grdPay[RowNumb, (int)GRD.Memo] + "";
                if (!rdbCancel.Checked && grdPay[RowNumb, (int)GRD.IouID] + "" != "") sqlCom.Parameters.Add("@IouID", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.IouID]);
                else if (rdbCancel.Checked && grdPay[RowNumb, (int)GRD.IouID] + "" != "") sqlCom.Parameters.Add("@IouID", SqlDbType.Decimal).Value = 0;
                if (grdPay[RowNumb, (int)GRD.credit] + "" != "") sqlCom.Parameters.Add("@Credit", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.credit]);
                if (grdPay[RowNumb, (int)GRD.debit] + "" != "") sqlCom.Parameters.Add("@Debit", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPay[RowNumb, (int)GRD.debit]);
                sqlCom.Parameters.Add("@Billable", SqlDbType.Int).Value = Convert.ToBoolean(grdPay[RowNumb, (int)GRD.billable]);
                sqlCom.Parameters.Add("@IsDeleted", SqlDbType.Int).Value = Convert.ToBoolean(grdPay[RowNumb, (int)GRD.IsDeleted]);
                sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                sqlCom.ExecuteNonQuery();
                if ((int)sqlCom.Parameters["@RtnValue"].Value != 1) { RtnVal = false; }
                RowNumb++;
            }
            return RtnVal;
        }
        private void btnTour_Click(object sender, EventArgs e)
        {
            filterByPerson = true;
            if (InsMode == 0 && fType.Trim() != "IOU".Trim()) Clear_All();
            string sql = "SELECT ID,TourID,Guest,AgentID FROM vw_TourBasics WHERE ISNULL(IsCancelled,0)<>1";// AND CompID='"+cmbCompany.SelectedValue.ToString().Trim()+"'";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.Trim() == "") { lblGuest.Text = ""; return; }
            Fill_For_Tour(txtTourNo.Text.Trim());
        }
        private void Fill_For_Tour(string TransID)
        {
            if (TransID.Trim() == "") return;
            txtTourNo.Text = TransID.Trim();
            string sql = "SELECT ID,TourID,Guest,AgentID FROM vw_TourBasics WHERE ID=" + TransID.Trim() + " AND ISNULL(IsCancelled,0)<>1";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            if (DT.Rows.Count == 0 && DT.Rows[0]["ID"] + "" == "") return;
            lblGuest.Text = DT.Rows[0]["Guest"] + "";
            if (txtTourNo.Text.Trim() == "") return;
            Fill_Control_For_Tour();
        }
        public void Fill_Control_For_Tour()
        {
            try
            {
                if (!filterByPerson) return;
                int type = 0; 
                if (rdbDriver.Checked) type = 2; else if (rdbGuide.Checked) type = 3; else if (rdbAgent.Checked) type = 1; else if (rdbHotel.Checked) type = 5; ;
                rdbDriver.Visible = rdbGuide.Visible = rdbHotel.Visible = rdbAgent.Visible = true;
                string s1 = "SELECT * FROM (SELECT   p.ID ID,p.Name+ (CASE WHEN isnull(e.IsCancelled,0)=1 THEN ' (Cancelled)' ELSE '' END) Name FROM vw_ALL_PERSONS p LEFT JOIN (SELECT * FROM vw_ALL_TourEntities WHERE TransID=" + txtTourNo.Text.Trim() + ") e ON p.ID=e.ID WHERE p.Name<>'' AND floor(p.ID/100000)=" + type + " AND (TransID IS NOT NULL OR ( p.ID IN (SELECT [PayableTo] FROM act_CashPayment WHERE TransID=" + txtTourNo.Text.Trim() + ")))) xx  ORDER BY xx.Name ;";
              DataTable  DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table( s1);
                FillDropDown(drpPayableTo, DT);
                FillDropDown(drpPayee, DT);
                string compID = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT CompID FROM trn_GroupAmendment WHERE ID=" + txtTourNo.Text.Trim() + "").Rows[0]["CompID"] + "";
                if (compID != "")
                {
                    cmbCompany.SelectedValue = compID.Trim();
                    cmbCompany.Enabled = false;
                }
                if (InsMode == 0) Set_Voucher_No();
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Set_Voucher_No();
            grdPay.Rows.Count = 1;
            grdPay.Rows.Count = 100;
            get_IOU("");
        }
        private void btnClearEntities_Click(object sender, EventArgs e)
        {
            rdbDriver.Checked = rdbGuide.Checked = rdbHotel.Checked = rdbAgent.Checked = rdbOther.Visible = false;
            Fill_Control_For_Tour();
        }
        private void btnPrint_Click(object sender, EventArgs e) { Print_Payment(); }
        private void Print_Payment()
        {
            Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
            string sql = "SELECT TransactionID,VoucherID, PaybleTo,PaidDate,RefNo,Description,ChkNo,ChkDate,Payee,IsConfirm,IsCancelled,IsCompleted,Memo,CurrencyCode,Currency,ISNULL(Credit,0)Credit,ISNULL(Debit,0)Debit,Outstanding,OutstandingAmt,SettledFrom,AccountTypeID,AccountID,AccountType,AccountNo,AccountIdentifier,TourID,Guest,DateArrival,DateDeparture,HandledBy,Company_Logo, Telephone, Fax, E_Mail, Web,DisplayName,Physical_Address, AADname,MDname,LastModifiedBy From vw_acc_Payments WHERE ID = '" + Syscode.ToString().Trim() + "' AND IsDeleted<>1 AND Billable=1 ORDER BY SrNo";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            if (DT.Rows.Count > 0)
            {
                DataSets.ds_acc_PAY DTP;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rpt;
                DTP = new Tourist_Management.DataSets.ds_acc_PAY();
                if (fType.Trim() == "IOU") rpt = new Tourist_Management.TransacReports.IOU_Settlement();
                else if (fType.Trim() == "CPY") rpt = new Tourist_Management.TransacReports.rpt_CashPaymentVoucher_1();
                else if (fType.Trim() == "REC") rpt = new Tourist_Management.TransacReports.rpt_Receipt();
                else if (fType.Trim() == "BIL") return;
                else rpt = new Tourist_Management.TransacReports.rpt_ChequeVoucher();
                sConnection.Print_Report(Syscode.ToString(), sql, DTP, rpt, "");
            }
            else MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnIOU_Click(object sender, EventArgs e)
        {
            try
            {
                string CurVoucherIDs = "", sql = "SELECT ID,TourID,DisplayName Company,VoucherID,PayableTo,PaidDate,RefNo,Amount,Settled From Fun_ReturnIOUTot() WHERE Amount<>Settled AND CompID=" + cmbCompany.SelectedValue.ToString().Trim() + "";
                foreach (DataRow r in dtIOU.Rows)
                {
                    if (CurVoucherIDs.Trim() != "") CurVoucherIDs += ",";
                    if (dtIOU.Rows[0]["VoucherID"] + "" != "") CurVoucherIDs += dtIOU.Rows[0]["VoucherID"] + "";
                }
                if (CurVoucherIDs.Trim() != "") sql += "AND VoucherID NOT IN('" + CurVoucherIDs + "')";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql + " Order By PaidDate");
                Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
                txtIOUNo.Text = finder.Load_search(DT);
                if (txtIOUNo.Text.Trim() == "") return;
                DataTable DT1 = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Amount,Settled From Fun_ReturnIOUTot() WHERE ID=" + txtIOUNo.Text.Trim() + "");
                txtSettledIOU.Text = DT1.Rows[0]["Amount"] + "";
                txtBalanceIOU.Text = (Convert.ToDecimal(DT1.Rows[0]["Amount"]) - Convert.ToDecimal(DT1.Rows[0]["Settled"])).ToString().Trim();
                chkMulIOU.Enabled = (dtIOU.Rows.Count > 1);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void Change_For_Multiple_IOUs()
        {
            if (chkMulIOU.Checked) grdPay.Cols[(int)GRD.accountType].Width = grdPay.Cols[(int)GRD.accountName].Width = grdPay.Cols[(int)GRD.IouVoucherID].Width = 100;
            else
            {
                grdPay.Cols[(int)GRD.accountType].Width = grdPay.Cols[(int)GRD.accountName].Width = 150;
                grdPay.Cols[(int)GRD.IouVoucherID].Width = 0;
            }
        }
        private void Add_IOU(string iouNo)
        {
            try
            {
                dtIOU.Rows.Clear();
                if(drpPayableTo.SelectedValue != null)foreach (DataRow r in Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DISTINCT cp.ID,cp.VoucherID + ' - '+CONVERT(VARCHAR(20), dd.Amount)+ ( CASE WHEN IsCancelled=1 THEN ' (Canceled)' WHEN dt.Amount >=dd.Amount THEN ' (Settled)' WHEN dd.Amount=0 THEN ' (Zero IOU)' WHEN IsConfirm!=1 THEN ' (Not Confirmed)' ELSE '' END) FROM act_CashPayment cp LEFT JOIN (SELECT CashPayID, SUM(Credit) Amount FROM dbo.act_CashPayment_Detail GROUP BY CashPayID) dd ON cp.ID = dd.CashPayID LEFT JOIN (SELECT cpd.IouID, SUM(cpd.Credit) Amount FROM dbo.act_CashPayment cp LEFT JOIN dbo.act_CashPayment_Detail cpd ON cp.ID = cpd.CashPayID WHERE VoucherID!='" + VoucherID + "' GROUP BY cpd.IouID) dt ON dt.IouID =cp.ID WHERE Type ='IOU' AND CONVERT(int,'0" + txtTourNo.Text + "') =(isnull(TransID,0)) " + " AND PayableTo='" + drpPayableTo.SelectedValue.Trim() + "'").Rows) dtIOU.Rows.Add(Convert.ToDouble(r[0]), r[1]);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Add_IOU(txtIOUNo.Text);
            txtIOUNo.Text = txtBalanceIOU.Text = txtSettledIOU.Text = "";
        }
        private void grdPay_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            try
            {
                if (e.Col == (int)GRD.credit)
                {
                    if (grdPay[grdPay.Row, (int)GRD.credit] + "" != "" && !Classes.clsGlobal.IsNumeric(grdPay[grdPay.Row, (int)GRD.credit].ToString().Trim()))
                    {
                        MessageBox.Show("Please enter a valid credit value.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdPay[grdPay.Row, (int)GRD.credit] = "0.00".Trim();
                        return;
                    }
                    if (grdPay[grdPay.Row, (int)GRD.IouID] + "" != "" && fType.Trim() == "CPY")
                    {
                        decimal val, bal;
                        val = Convert.ToDecimal(grdPay[grdPay.Row, (int)GRD.credit]);
                        DataTable dt = Classes.clsGlobal.objCon.Fill_Table("SELECT Amount,Settled FROM Fun_ReturnIOUTot() WHERE ID=" + grdPay[grdPay.Row, (int)GRD.IouID].ToString().Trim() + "");
                        if (dt.Rows.Count > 0)
                        {
                            bal = Convert.ToDecimal(dt.Rows[0]["Amount"]) - Convert.ToDecimal(dt.Rows[0]["Settled"]);
                            if (val > bal)
                            {
                                MessageBox.Show("Maximum settle amount is " + bal + "", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                grdPay[grdPay.Row, (int)GRD.credit] = bal.ToString().Trim();
                            }
                        }
                    }
                }
                if (e.Col == (int)GRD.debit)
                {
                    if (grdPay[grdPay.Row, (int)GRD.debit] + "" != "" && !Classes.clsGlobal.IsNumeric(grdPay[grdPay.Row, (int)GRD.debit].ToString().Trim()))
                    {
                        MessageBox.Show("Please enter a valid debit value.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grdPay[grdPay.Row, (int)GRD.debit] = "0.00".Trim();
                        return;
                    }
                }
                if (calculate_Total()) { }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private Boolean calculate_Total()
        {
            try
            {
                int RowNumb = 1;
                decimal totCredit = 0.00m, totDebit = 0.00m;
                while (grdPay[RowNumb, (int)GRD.accountTypeID] + "" != "")
                {
                    if (grdPay[RowNumb, (int)GRD.credit] + "" != "") totCredit += Convert.ToDecimal(grdPay[RowNumb, (int)GRD.credit]);
                    if (grdPay[RowNumb, (int)GRD.debit] + "" != "") totDebit += Convert.ToDecimal(grdPay[RowNumb, (int)GRD.debit]);
                    RowNumb++;
                }
                creditTotal = totCredit;
                lblCredit.Text = totCredit.ToString().Trim();
                lblDebit.Text = totDebit.ToString().Trim();
                lblCredit.ForeColor = lblDebit.ForeColor = (totCredit == totDebit) ? Color.Green : Color.Red;
                if (lblDifDebit.Visible = totDebit > totCredit) lblDifDebit.Text = (totDebit - totCredit).ToString().Trim();
                if (lblDifCredit.Visible = totDebit < totCredit) lblDifCredit.Text = (totCredit - totDebit).ToString().Trim();
                return (totCredit == totDebit);
            }
            catch (Exception ex) { db.MsgERR(ex); return false; }
        }
        private void btnSettleRest_Click(object sender, EventArgs e)
        {
            try
            {
                decimal totCredit = 0.00m, totDebit = 0.00m;
                int RowNumb = 1;
                while (grdPay[RowNumb, (int)GRD.accountTypeID] + "" != "")
                {
                    if (grdPay[RowNumb, (int)GRD.credit] + "" != "") totCredit += Convert.ToDecimal(grdPay[RowNumb, (int)GRD.credit]);
                    if (grdPay[RowNumb, (int)GRD.debit] + "" != "") totDebit += Convert.ToDecimal(grdPay[RowNumb, (int)GRD.debit]);
                    RowNumb++;
                }
                int accTypeID = Convert.ToInt32(Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT AccID FROM def_Account WHERE AccName='Petty Cash'").Rows[0]["AccID"]);
                string SqlQuery = "SELECT ID,Account,AccountTypeID,AccountType FROM vw_acc_Acounts WHERE ID=" + accTypeID + "";
                DataTable DTAll = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                grdPay[RowNumb, (int)GRD.ID] = 0;
                grdPay[RowNumb, (int)GRD.accountTypeID] = DTAll.Rows[0]["AccountTypeID"];
                grdPay[RowNumb, (int)GRD.accountType] = DTAll.Rows[0]["AccountType"];
                grdPay[RowNumb, (int)GRD.accountID] = DTAll.Rows[0]["ID"];
                grdPay[RowNumb, (int)GRD.accountName] = DTAll.Rows[0]["Account"];
                grdPay[RowNumb, (int)GRD.Memo] = "Petty Cash";
                if (totDebit > totCredit) grdPay[RowNumb, (int)GRD.credit] = (totDebit - totCredit).ToString().Trim();
                else grdPay[RowNumb, (int)GRD.debit] = (totCredit - totDebit).ToString().Trim();
                if (calculate_Total()) { }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void drpPayableTo_Selected_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int row = 1;
                string selectedIOUs = "";
                while (grdPay[row, (int)GRD.accountTypeID] + "" != "")
                {
                    if (grdPay[row, (int)GRD.IouID] + "" != "")
                    {
                        if (selectedIOUs.Trim() == "") selectedIOUs += grdPay[row, (int)GRD.IouID] + "";
                        else selectedIOUs += "," + grdPay[row, (int)GRD.IouID] + "";
                    }
                    row++;
                }
                get_IOU(selectedIOUs);
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void get_IOU(string selectedIOUs)
        {
            try
            {
                if (drpPayableTo.SelectedValue + "" == "") return;
                int compid = Convert.ToInt32(cmbCompany.SelectedValue);
                string SqlQuery = "SELECT ID FROM dbo.Fun_ReturnIOUTot() WHERE Amount<>Settled AND CompID=" + compid + " AND PayableToID='" + drpPayableTo.SelectedValue.Trim() + "'";
                if (selectedIOUs.Trim() != "") SqlQuery += " AND ID NOT IN(" + selectedIOUs + ")";
                dtIOU.Rows.Clear();
                foreach (DataRow r in Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery).Rows) if (r["ID"] + "" != "") Add_IOU(r["ID"] + "");
                if ( !IN(fType.Trim(), "REC","CTN","DTN"))
                {
                    if (txtTourNo.Text.Trim() == "") return;
                    SqlQuery = "SELECT VoucherID FROM dbo.Fun_ReturnCPYTot() WHERE TransID='" + txtTourNo.Text.Trim() + "' AND PayableToID='" + drpPayableTo.SelectedValue.Trim() + "'";
                    DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(SqlQuery);
                    if (DT.Rows.Count > 0 && DT.Rows[0]["VoucherID"] + "" != "")
                    { 
                        lblStatus.ForeColor = Color.Red;
                        lblStatus.Text = drpPayableTo.SelectedText + " has already paid \nfor this tour in Voucher ID : " + DT.Rows[0]["VoucherID"] + "" + "";
                        lblStatus.Visible = true;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        lblStatus.Visible = false;
                    }
                }
                if (fType.Trim() == "IOU" && txtTourNo.Text.Trim() != "" && (rdbDriver.Checked || rdbGuide.Checked)) { lblError.Visible = true; set_Default_IOU(); }
                if ((fType.Trim() == "CPY" || fType.Trim() == "CHQ") && txtTourNo.Text.Trim() != "" && (rdbDriver.Checked || rdbGuide.Checked)) { lblError.Visible = true; set_Default_CPY(); }
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void encash_Money()
        {
            try
            {
                bool IsConfirm = Convert.ToBoolean(Classes.clsGlobal.objCon.Fill_Table("SELECT ISNULL(IsConfirm,0) FROM act_CashPayment WHERE ID=" + Syscode + "").Rows[0][0]);
                bool IsCancelled = Convert.ToBoolean(Classes.clsGlobal.objCon.Fill_Table("SELECT ISNULL(IsCancelled,0) FROM act_CashPayment WHERE ID=" + Syscode + "").Rows[0][0]);
                if (!IsConfirm) { MessageBox.Show("Non-confirm receipts cannot be encashed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (IsCancelled) { MessageBox.Show("Cancelled receipts cannot be encashed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                Accounts.frmEncashMoney fem = new Tourist_Management.Accounts.frmEncashMoney();
                fem.CashID = Syscode;
                fem.ShowDialog();
            }
            catch (Exception ex) { db.MsgERR(ex); }
        }
        private void dtpPaidDate_ValueChanged(object sender, EventArgs e)
        {
            if ((Syscode != 0) || (cmbCompany.SelectedValue + "" == "") || (fType.Trim() == "OIE".Trim())) return;
            int compID = Convert.ToInt32(cmbCompany.SelectedValue);
            string qryID = "(SELECT MAX(UniqueID) FROM act_CashPayment WHERE CompID=" + compID + " AND [Type]='" + fType.Trim() + "' GROUP BY CompID)";
            string qry = "SELECT CAST(PaidDate AS Date)PaidDate FROM act_CashPayment WHERE CompID=" + compID + " AND [Type]='" + fType.Trim() + "' AND UniqueID=" + qryID.Trim() + "";
            string val = Classes.clsConnection.getSingle_Value_Using_Reader(qry);
            if (val + "" == "") return;
            DateTime payDate = Convert.ToDateTime(val);
            if (dtpPaidDate.Value >= payDate || fType=="BIL") return;
            MessageBox.Show("Cannot backdate this transaction", msghd, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            dtpPaidDate.Value = payDate;
        }
        private void cmbCurrency_SelectedIndexChanged(object sender, EventArgs e) { txtRrate.Enabled = txtErate.Enabled = (cmbCurrency.SelectedValue.ToString().Trim() != "1"); }
        private void txtErate_TextChanged(object sender, EventArgs e) { dtpExchangeDate.Enabled = txtErate.Text.Trim() != ""; }
        private void btnEncash_Click(object sender, EventArgs e) { encash_Money(); }
        private void rdbOther_CheckedChanged(object sender, EventArgs e) { Fill_Control_For_Tour(); }
        private void grdPay_Click(object sender, EventArgs e) { if (calculate_Total()) { } }
        private void txtIOUNo_TextChanged(object sender, EventArgs e) { btnAdd.Enabled = txtIOUNo.Text.Trim() != ""; }
        private void chkMulIOU_CheckedChanged(object sender, EventArgs e) { Change_For_Multiple_IOUs(); }
        private void rdb_CheckedChanged(object sender, EventArgs e) { Fill_Control_For_Tour(); filterByPerson = true; }
        private void btnClearStatus_Click(object sender, EventArgs e) { rdbCancel.Checked = rdbConfirm.Checked = false; }
        private void lblType_Click(object sender, EventArgs e)
        {
        }
        private void lblMain_Click(object sender, EventArgs e)
        {
        }
        private void drpPayableTo_Load(object sender, EventArgs e)
        {
        }
    }
}