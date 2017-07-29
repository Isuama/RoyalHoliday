using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
namespace Tourist_Management.Transaction
{
    public partial class frmVoucherType : Form
    {
        private string msghd = "Voucher Type";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public double SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        byte[] imageData = null;
        int RetrieveNo = 0, AmendNo=-1;
        bool IsRetrive = false; 
        string retrievedVoucherID;
        enum VD { TourId, VoucherId, Guest, FromDate, ToDate, VoucherTypeId, VoucherName, NoOfChildren, NoOfAdults, IsDriver, ResponsibleId, Code, ResposibleName, EmpPhoto, VehicleNo, Brand, Model, Amount,BillIns,OtherIns,Conf,AmendTo,Ref,NoOfVehicles,AmendNo,Cancel };
        DataTable[] DTB = new DataTable[2];
        public string SqlQry = "SELECT TransID,VoucherId,Guest,VoucherTypeName AS VoucherName,Amount FROM vw_TR_VoucherDetails";
        public frmVoucherType(){InitializeComponent();}
        private void frmVoucherType_Load(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            btnCancel.Enabled = false;
            btnOk.Enabled = false;
            Fill_VTypes();
            Grd_Initializer();
        }
        private void btnTour_Click(object sender, EventArgs e)
        {
            Clear_Contents();
            string sql;
            sql = "SELECT ID,TourID,Guest,AgentID FROM trn_GroupAmendment";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            Tourist_Management.Other.frmFilterRecords finder = new Tourist_Management.Other.frmFilterRecords();
            txtTourNo.Text = finder.Load_search(DT);
            if (txtTourNo.Text.Trim() == "")
                return;
            SystemCode = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
            Fill_Control();
            if (cmbDriver.Items.Count == 0)
                return;
            Get_Details();
            Load_DriverRelated();
            Fill_Data();
            string qry = "SELECT ISNULL(IsCompleted,0)IsConfirm FROM act_Profit_Lose WHERE TourID=" + SystemCode + "";
            string val = Classes.clsConnection.getSingle_Value_Using_Reader(qry);
            btnOk.Enabled = true;
            lblComplete.Visible = false;
            if (val + "".Trim() != "")
            {
                btnOk.Enabled = !Convert.ToBoolean(val);
                lblComplete.Visible = !Convert.ToBoolean(val);
            }
        }
        private void Grd_Initializer()
        {
            try
            {
                #region INVOICE GRID DETAILS
                grdPayments.Cols.Count = 26;
                grdPayments.Cols[(int)VD.TourId].Width = 0;
                grdPayments.Cols[(int)VD.VoucherId].Width = 0;
                grdPayments.Cols[(int)VD.Guest].Width = 0;
                grdPayments.Cols[(int)VD.FromDate].Width = 68;
                grdPayments.Cols[(int)VD.ToDate].Width = 68;
                grdPayments.Cols[(int)VD.VoucherTypeId].Width = 0;
                grdPayments.Cols[(int)VD.VoucherName].Width =122;
                grdPayments.Cols[(int)VD.NoOfChildren].Width = 57;
                grdPayments.Cols[(int)VD.NoOfAdults].Width = 49;
                grdPayments.Cols[(int)VD.IsDriver].Width = 0;
                grdPayments.Cols[(int)VD.ResponsibleId].Width = 0;
                grdPayments.Cols[(int)VD.Code].Width =0;
                grdPayments.Cols[(int)VD.ResposibleName].Width = 134;
                grdPayments.Cols[(int)VD.EmpPhoto].Width = 0;
                grdPayments.Cols[(int)VD.VehicleNo].Width = 0;
                grdPayments.Cols[(int)VD.Brand].Width = 0;
                grdPayments.Cols[(int)VD.Model].Width = 0;
                grdPayments.Cols[(int)VD.Amount].Width = 100;
                grdPayments.Cols[(int)VD.BillIns].Width = 0;
                grdPayments.Cols[(int)VD.OtherIns].Width = 0;
                grdPayments.Cols[(int)VD.Conf].Width = 0;
                grdPayments.Cols[(int)VD.NoOfVehicles].Width = 0;
                grdPayments.Cols[(int)VD.AmendTo].Width = 0;
                grdPayments.Cols[(int)VD.Ref].Width = 0;
                grdPayments.Cols[(int)VD.AmendNo].Width = 0;
                grdPayments.Cols[(int)VD.Cancel].Width = 0;
                grdPayments.Cols[(int)VD.TourId].Caption = "Tour ID";
                grdPayments.Cols[(int)VD.VoucherId].Caption = "Voucher ID";
                grdPayments.Cols[(int)VD.Guest].Caption = "Guest Name";
                grdPayments.Cols[(int)VD.FromDate].Caption = "From Date";
                grdPayments.Cols[(int)VD.ToDate].Caption = "To Date";
                grdPayments.Cols[(int)VD.VoucherTypeId].Caption = "Voucher Type Id";
                grdPayments.Cols[(int)VD.VoucherName].Caption = "Voucher Name";
                grdPayments.Cols[(int)VD.NoOfChildren].Caption = "Children";
                grdPayments.Cols[(int)VD.NoOfAdults].Caption = "Adults";
                grdPayments.Cols[(int)VD.ResponsibleId].Caption = "Resposible ID";
                grdPayments.Cols[(int)VD.Code].Caption = "Resposible Code";
                grdPayments.Cols[(int)VD.ResposibleName].Caption = "Resposible";
                grdPayments.Cols[(int)VD.EmpPhoto].Caption = "Resposible Photo";
                grdPayments.Cols[(int)VD.VehicleNo].Caption = "Vehicle No";
                grdPayments.Cols[(int)VD.Brand].Caption = "Vehicle Brand";
                grdPayments.Cols[(int)VD.Model].Caption = "Model";
                grdPayments.Cols[(int)VD.Amount].Caption = "Amount";
                grdPayments.Cols[(int)VD.BillIns].Caption = "Billing";
                grdPayments.Cols[(int)VD.OtherIns].Caption = "Other";
                grdPayments.Cols[(int)VD.Conf].Caption = "Confirmation";
                grdPayments.Cols[(int)VD.NoOfVehicles].Caption = "NoOfVehicle";
                grdPayments.Cols[(int)VD.AmendTo].Caption = "AmendTo";
                grdPayments.Cols[(int)VD.Ref].Caption = "Reference";
                grdPayments.Cols[(int)VD.AmendNo].Caption = "Type";
                grdPayments.Cols[(int)VD.Cancel].Caption = "Cancel";
                grdPayments.Rows[1].AllowEditing = false;
                grdPayments.Cols[(int)VD.Cancel].DataType = Type.GetType(" System.Boolean");
                grdPayments.Cols[(int)VD.IsDriver].DataType = Type.GetType(" System.Boolean");
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Data()
        {
            try
            {
                #region PAYMENT DETAILS DETAILS
                int RowNumb;
                String ssql = "SELECT FromDate, Amount, VoucherTypeId, ToDate,Children As NoOfChildren, "+
                    "Adult AS NoOfAdults, VehicleNo, Model, Brand, Guest,"+
                    "ResponsibleID,ISNULL(IsDriver,1)AS IsDriver,"+
                    "DriverName,DrvPhoto,DriverCode,DriverTel," +
                    "GuideName,GuideCode,GuideTel," +
                    "VoucherTypeName AS VoucherName, TourId ,VoucherId,SrNo," +
                    "AmendNo,BillingIns,OtherIns,Confirmation,NoOfVehicles,AmendTo,Reference,ISNULL(IsCancelled,0) AS IsCancelled " +
                    "FROM vw_TR_VoucherDetails WHERE TransID=" + SystemCode + " ORDER BY srno";
                DataTable DTPay = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                if (DTPay.Rows.Count > 0)
                {
                    RowNumb = 0;
                    Mode = 1;
                    while (DTPay.Rows.Count > RowNumb)
                    {                     
                        grdPayments[RowNumb + 1, (int)VD.TourId] = DTPay.Rows[RowNumb]["TourId"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.VoucherId] = DTPay.Rows[RowNumb]["VoucherId"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.Guest] = DTPay.Rows[RowNumb]["Guest"].ToString(); 
                        grdPayments[RowNumb + 1, (int)VD.FromDate] = DTPay.Rows[RowNumb]["FromDate"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.ToDate] = DTPay.Rows[RowNumb]["ToDate"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.Amount] = DTPay.Rows[RowNumb]["Amount"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.VoucherTypeId] = DTPay.Rows[RowNumb]["VoucherTypeId"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.VoucherName] = DTPay.Rows[RowNumb]["VoucherName"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.NoOfChildren] = DTPay.Rows[RowNumb]["NoOfChildren"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.NoOfAdults] = DTPay.Rows[RowNumb]["NoOfAdults"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.ResponsibleId] = DTPay.Rows[RowNumb]["ResponsibleID"].ToString();
                        if (Convert.ToBoolean(DTPay.Rows[RowNumb]["IsDriver"]))
                        {
                            rdbDriver.Checked = true;
                            grdPayments[RowNumb + 1, (int)VD.Code] = DTPay.Rows[RowNumb]["DriverCode"].ToString();
                            grdPayments[RowNumb + 1, (int)VD.ResposibleName] = DTPay.Rows[RowNumb]["DriverName"].ToString();
                            grdPayments[RowNumb + 1, (int)VD.EmpPhoto] = DTPay.Rows[RowNumb]["DrvPhoto"].ToString();
                            grdPayments[RowNumb + 1, (int)VD.VehicleNo] = DTPay.Rows[RowNumb]["VehicleNo"].ToString();
                            grdPayments[RowNumb + 1, (int)VD.Brand] = DTPay.Rows[RowNumb]["Brand"].ToString();
                            grdPayments[RowNumb + 1, (int)VD.Model] = DTPay.Rows[RowNumb]["Model"].ToString();
                        }
                        else
                        {
                            rdbGuide.Checked = true;
                            grdPayments[RowNumb + 1, (int)VD.Code] = DTPay.Rows[RowNumb]["GuideCode"].ToString();
                            grdPayments[RowNumb + 1, (int)VD.ResposibleName] = DTPay.Rows[RowNumb]["GuideName"].ToString();                            
                        }
                        grdPayments[RowNumb + 1, (int)VD.Amount] = DTPay.Rows[RowNumb]["Amount"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.NoOfVehicles] = DTPay.Rows[RowNumb]["NoOfVehicles"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.AmendNo] = DTPay.Rows[RowNumb]["AmendNo"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.BillIns] = DTPay.Rows[RowNumb]["BillingIns"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.OtherIns] = DTPay.Rows[RowNumb]["OtherIns"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.Conf] = DTPay.Rows[RowNumb]["Confirmation"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.AmendTo] = DTPay.Rows[RowNumb]["AmendTo"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.Ref] = DTPay.Rows[RowNumb]["Reference"].ToString();
                        grdPayments[RowNumb + 1, (int)VD.Cancel] = Convert.ToBoolean(DTPay.Rows[RowNumb]["IsCancelled"]);
                        RowNumb++;
                    }
                    btnTour.Enabled = false;
                    txtTourNo.Text = DTPay.Rows[0]["TourId"].ToString();
                }
                #endregion
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Clear_Contents()
        {
            txtClientName.Text = "";
            driverCodeLbl.Text = "";                
            driverNameLbl.Text = "";                
            vehicleTypeLbl.Text = "";                
            vehicleNoLbl.Text = "";
            modelLbl.Text = "";
            brandLbl.Text = "";
            noOfAdultsTxt.Text="";
            noOfChilderenTxt.Text="";
            amountTxt.Text="";
            txtBilling.Text = "All extras to be collected directly";
            txtOther.Text = "";
            txtConf.Text = "Above arrangement is confirmed on the telephone by Kumara to Lakshan ";
            txtAmenTo.Text = "";
            txtRef.Text = "";
            fromDate.Value = Classes.clsGlobal.CurDate();
            toDate.Value = Classes.clsGlobal.CurDate();
            rdbDriver.Checked = true;
        }
        private void Fill_Control()
        {
            try
            {
                if (txtTourNo.Text.ToString().Trim() == "")
                {
                    Clear_Contents();
                    return;
                }
                else
                {
                    SystemCode = Convert.ToDouble(txtTourNo.Text.ToString().Trim());
                    cmbDriver.Enabled = true;
                    btnShow.Enabled = true;
                }  
                cmbDriver.DataSource  = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT DriverID,DriverName FROM vw_trn_DriverDetails WHERE TransID="+txtTourNo.Text.Trim()+"");
                cmbGuide.DataSource= Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT GuideID,Name GuideName FROM vw_trn_GuideDetails WHERE TransID=" + txtTourNo.Text.Trim() + "");
                cmbDriver.Enabled = (DTB[0].Rows.Count > 0);
                    cmbGuide.Enabled = (DTB[1].Rows.Count > 0);
                cmbGuide.Visible = false;                
                btnShow.Enabled = true;
                btnOk.Enabled = false;                
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_VTypes()
        {
            try
            {
                cmbVoucherType.Enabled = true;
                int rownumb = 1;                
                DTB[0] = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,Company FROM mst_SafariCompanies WHERE ISNULL(IsActive,0) = 1 ");
                if (DTB[0].Rows.Count < rownumb)   return; 
                cmbVoucherType.DataSource = DTB[0];
                cmbVoucherType.Enabled = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Get_Details()
        {
            string ssql;
            int RowNumb;
            DataTable DT;
            #region Fill Transport Details
            ssql = "SELECT guest,VehicleNo, NoOfAdult, NoOfChild FROM vw_trn_Tansport_TR  WHERE TransID=" + SystemCode + "";
            DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            if (DT.Rows.Count > 0)
            {
                RowNumb = 0;
                while (DT.Rows.Count > RowNumb)
                {
                    if (DT.Rows[RowNumb]["Guest"].ToString() != "")
                        txtClientName.Text = DT.Rows[RowNumb]["Guest"].ToString();
                    if (DT.Rows[RowNumb]["NoOfAdult"].ToString() != "")
                        noOfAdultsTxt.Text = DT.Rows[RowNumb]["NoOfAdult"].ToString();
                    if (DT.Rows[RowNumb]["NoOfChild"].ToString() != "")
                        noOfChilderenTxt.Text = DT.Rows[RowNumb]["NoOfChild"].ToString();
                    RowNumb++;
                }
            }
            #endregion
        }
        private void Load_DriverRelated()
        {
            try
            {
                DataTable DT;
                string ssql;
                int driverId=0;
                if (rdbDriver.Checked)
                {
                    #region
                    ssql = " SELECT EmpPhoto,ID,code,name FROM vwDriverVsEmployee " +
                            "Where ID=" + cmbDriver.SelectedValue.ToString().Trim() + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows[0]["EmpPhoto"] != DBNull.Value)
                    {
                        byte[] Photo = (byte[])DT.Rows[0]["EmpPhoto"];
                        imageData = Photo;
                        MemoryStream ms = new MemoryStream(Photo);
                        pbCompLogo.Image = Image.FromStream(ms, false, false);
                    }
                    else
                        pbCompLogo.Image = global::Tourist_Management.Properties.Resources.noimage;
                    if (DT.Rows[0]["ID"] != DBNull.Value)
                        driverId = Convert.ToInt32(DT.Rows[0]["ID"].ToString());
                    driverNameLbl.Text = DT.Rows[0]["name"].ToString();
                    driverCodeLbl.Text = DT.Rows[0]["code"].ToString();
                    #endregion
                    #region Fill Vehicle Details
                    ssql = "SELECT  VehicleNo, InsuranceNo, Type, Brand, Model, ModelNo FROM vw_TR_Vehicle_Details  WHERE driverId=" + driverId + "";
                    DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
                    if (DT.Rows.Count > 0)
                    {
                        if (DT.Rows[0]["Type"].ToString() != "")
                            vehicleTypeLbl.Text = DT.Rows[0]["Type"].ToString();
                        if (DT.Rows[0]["VehicleNo"].ToString() != "")
                            vehicleNoLbl.Text = DT.Rows[0]["VehicleNo"].ToString();
                        if (DT.Rows[0]["Model"].ToString() != "")
                            modelLbl.Text = DT.Rows[0]["Model"].ToString();
                        if (DT.Rows[0]["Brand"].ToString() != "")
                            brandLbl.Text = DT.Rows[0]["Brand"].ToString();
                    }
                    #endregion
                }
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
                btnTour.Enabled = true;
                txtTourNo.Text = "";
                MessageBox.Show("Transaction Sucessfully Completed", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Print_Invoice();
            }
        }
        private Boolean Save_Pro()
        {         
                if (Save_Data() == false)
                {
                    return false;
                }
                return true;
        }
        private Boolean Validate_Data()
        {
                if (txtTourNo.Text.Trim() == "" )
                {
                    MessageBox.Show("'TOUR NUMBER' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (noOfAdultsTxt.Text.Trim() == "")
                {
                    MessageBox.Show("'NUMBER OF ADULTS' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (noOfChilderenTxt.Text.Trim() == "" )
                {
                    MessageBox.Show("'NO OF CHILDREN' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (txtNoOfVehicles.Text.Trim() == "")
                {
                    MessageBox.Show("'NO OF VEHICLES' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if( amountTxt.Text.Trim() == "")
                {
                    MessageBox.Show("'AMOUNT' Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                if (rdbAmend.Checked && txtRef.Text.Trim()=="")
                {
                    MessageBox.Show("'REFERENCE' Cannot Be Blank When Make an Amendment", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
        }
        private Boolean Save_Data()
        {
            System.Data.SqlClient.SqlCommand sqlCom;
            Boolean RtnVal = false;
                int RowNumb = 1,AmendNo;
                string vid;
                sqlCom = new System.Data.SqlClient.SqlCommand();
                sqlCom.CommandType = CommandType.StoredProcedure;
                sqlCom.CommandText = "spSave_VoucherDetails";
                while (grdPayments[RowNumb, grdPayments.Cols[(int)VD.TourId].Index] != null)
                { 
                    RtnVal = false;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add("@RtnValue", SqlDbType.Int).Value = 0;
                    sqlCom.Parameters["@RtnValue"].Direction = ParameterDirection.InputOutput;
                    sqlCom.Parameters.Add("@TransId", SqlDbType.Decimal).Value = SystemCode;
                    sqlCom.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdPayments[RowNumb, (int)VD.FromDate].ToString().Trim());
                    sqlCom.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = Convert.ToDateTime(grdPayments[RowNumb, (int)VD.ToDate].ToString().Trim());
                    if (grdPayments[RowNumb, (int)VD.Amount].ToString() != "")
                        sqlCom.Parameters.Add("@Amount", SqlDbType.Decimal).Value = Convert.ToDecimal(grdPayments[RowNumb, (int)VD.Amount].ToString().Trim());
                    sqlCom.Parameters.Add("@IsDriver", SqlDbType.Int).Value = Convert.ToBoolean(grdPayments[RowNumb, (int)VD.IsDriver]);
                    sqlCom.Parameters.Add("@ResponsibleID", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)VD.ResponsibleId].ToString().Trim());
                    if (grdPayments[RowNumb, (int)VD.NoOfChildren] + "".Trim() != "")
                        sqlCom.Parameters.Add("Children", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)VD.NoOfChildren].ToString().Trim());
                    if (grdPayments[RowNumb, (int)VD.NoOfAdults]+"".Trim() != "")
                        sqlCom.Parameters.Add("@Adult", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)VD.NoOfAdults].ToString().Trim());
                    sqlCom.Parameters.Add("@VoucherTypeId", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)VD.VoucherTypeId].ToString().Trim());
                    AmendNo=Convert.ToInt32(grdPayments[RowNumb, (int)VD.AmendNo]);
                    if (AmendNo == 0)//RESERVATION
                    {
                        vid = (SystemCode + "/" + RowNumb).ToString().Trim();
                    }
                    else //AMENDMENT
                    {
                        vid = (SystemCode + "/" + RowNumb +"/"+ (char)(AmendNo+64)).ToString().Trim();
                    }
                    sqlCom.Parameters.Add("@VoucherId", SqlDbType.VarChar).Value = vid;
                    sqlCom.Parameters.Add("@NoOfVehicles", SqlDbType.NVarChar, 100).Value = grdPayments[RowNumb, (int)VD.NoOfVehicles].ToString().Trim();
                    sqlCom.Parameters.Add("@AmendNo", SqlDbType.Int).Value = Convert.ToInt32(grdPayments[RowNumb, (int)VD.AmendNo]);
                    sqlCom.Parameters.Add("@BillingIns", SqlDbType.NVarChar, 300).Value = grdPayments[RowNumb, (int)VD.BillIns].ToString().Trim();
                    sqlCom.Parameters.Add("@OtherIns", SqlDbType.NVarChar, 300).Value = grdPayments[RowNumb, (int)VD.OtherIns].ToString().Trim();
                    sqlCom.Parameters.Add("@Confirmation", SqlDbType.NVarChar, 300).Value = grdPayments[RowNumb, (int)VD.Conf].ToString().Trim();
                    sqlCom.Parameters.Add("@AmendTo", SqlDbType.NVarChar, 50).Value = grdPayments[RowNumb, (int)VD.AmendTo].ToString().Trim();
                    sqlCom.Parameters.Add("@Reference", SqlDbType.NVarChar, 300).Value = grdPayments[RowNumb, (int)VD.Ref].ToString().Trim();
                    sqlCom.Parameters.Add("@IsCancelled", SqlDbType.Int).Value = Convert.ToBoolean(grdPayments[RowNumb, (int)VD.Cancel]) ? 1 : 0;
                    sqlCom.Parameters.Add("@SrNo", SqlDbType.Int).Value = RowNumb;
                    sqlCom.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Convert.ToInt32(Classes.clsGlobal.UserID.ToString());
                    RowNumb++;
                    if (Tourist_Management.Classes.clsGlobal.objCon.ExecuteSP(sqlCom) == true)
                    {
                        RtnVal = true; 
                    }
                }
                return RtnVal;
        }
        private void changeDriverImg(object sender, EventArgs e)
        {
            Load_DriverRelated();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Print_Invoice();
        }
        private void Print_Invoice()
        {
            Classes.clsConnection sConnection = new Tourist_Management.Classes.clsConnection();
            String sql;
            sql="SELECT Company_Logo,DisplayName,Telephone,Mobile,Fax,E_mail,Web,Physical_Address," +
           "FromDate, Amount, VoucherTypeId, ToDate, DriverId,Children AS NoOfChildren, " +
           "Adult AS NoOfAdults, VehicleNo, Model, Brand, Guest,GuestMobile,"+
           "ISNULL(IsDriver,1) AS IsDriver,ISNULL(DriverName,'')AS DriverName,DrvPhoto,DriverCode AS Code,ISNULL(DriverTel,'')AS DriverTel," +
           "ISNULL(GuideName,'')AS GuideName,ISNULL(GuideTel,'')AS GuideTel," +
           "VoucherTypeName AS VoucherName, TourId ,VoucherId,"+          
           "AmendNo,BillingIns,OtherIns,Confirmation,NoOfVehicles,AmendTo,"+
           "Reference,ISNULL(IsCancelled,0)AS IsCancelled,srno," +
           "VehicleType,ContactPerson1,ContactNo1,ContactPerson2,ContactNo2," +
           "HandledBy1,HandledBy1Mob,HandledBy2,HandledBy2Mob,Remarks,OutSideComp" +
           " FROM vw_TR_VoucherDetails WHERE TransID=" + SystemCode+" ORDER BY srno";
            DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            if (DT.Rows.Count > 0)
            {
                DataSets.ds_acc_TrVoucherDetails DTP = new Tourist_Management.DataSets.ds_acc_TrVoucherDetails();
                Tourist_Management.Reports.TrVoucherDetails pia = new Tourist_Management.Reports.TrVoucherDetails();
                pia.SetDataSource(DTP);
                sConnection.Print_Report(SystemCode.ToString(), sql, DTP, pia, "TRVOUCHER");
            }
            else
                MessageBox.Show("No Records To Be Previewed.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnEmail_Click(object sender, EventArgs e)
        {
            Send_Email();
        }
        private void Send_Email()
        { 
                if (Validate_Email_Options() == false)  return;
                if (!System.IO.Directory.Exists("C:\\Temp\\trVoucherDetails"))
                {
                    MessageBox.Show("Click the preview button before send the mail", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.Display(false);
                string Signature = "";
                Signature = ReadSignature();
                oMsg.HTMLBody = rtbBody.Text + Signature;
                oMsg.CC = txtCC.Text;
                String sDisplayName = "MyAttachment";
                int iPosition;
                if (rtbBody.Text.ToString().Trim() != "")
                    iPosition = (int)oMsg.Body.Length + 1;
                else
                    iPosition = 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                Outlook.Attachment oAttach;
                if (chkExSum.Checked)
                {
                    ReportDocument oReport = new ReportDocument();
                    string path = "C:\\Temp\\trVoucherDetails\\TransportVoucherDetails.pdf";
                    string lFileName = path;
                    oAttach = oMsg.Attachments.Add(@path, iAttachType, iPosition, sDisplayName);
                }
                oMsg.Subject = txtSubject.Text;
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                if (txtTo.Text.ToString().Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(txtTo.Text.ToString().Trim());
                    oRecip.Resolve();
                    oRecip = null;
                }
                oRecips = null;
                oMsg = null;
                oApp = null;
        }
        private Boolean Validate_Email_Options()  {   return true;    }
        private string ReadSignature()
        {
                string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
                string signature = string.Empty;
                DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
                if (diInfo.Exists)
                {
                    FileInfo[] fisignature = diInfo.GetFiles("*.htm");
                    if (fisignature.Length > 0)
                    {
                        StreamReader sr = new StreamReader(fisignature[0].FullName, Encoding.Default);
                        signature = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(signature))
                        {
                            string filename = fisignature[0].Name.Replace(fisignature[0].Extension, string.Empty);
                            signature = signature.Replace(filename + "_files/", appDataDir + "/" + filename + "_files/");
                        }
                    }
                }
                return signature;
        }
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            if ((grdPayments[grdPayments.Row, (int)VD.TourId] + "").ToString().Trim() != "")
            {
                btnOk.Enabled = false;
                btnCancel.Enabled = true;
                if (grdPayments[grdPayments.Row, grdPayments.Cols[(int)VD.TourId].Index] == null)
                {
                    MessageBox.Show("No Values Found To Retrieve.", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                RetrieveNo = grdPayments.Row;
                IsRetrive = true;
                #region RETRIEVE VALUES FORM SELECTED ROW
                txtTourNo.Text = grdPayments[RetrieveNo, (int)VD.TourId].ToString();
                noOfAdultsTxt.Text = grdPayments[RetrieveNo, (int)VD.NoOfAdults].ToString();
                noOfChilderenTxt.Text = grdPayments[RetrieveNo, (int)VD.NoOfChildren].ToString();
                amountTxt.Text = grdPayments[RetrieveNo, (int)VD.Amount].ToString();
                fromDate.Value = Convert.ToDateTime(grdPayments[RetrieveNo, (int)VD.FromDate].ToString());
                toDate.Value = Convert.ToDateTime(grdPayments[RetrieveNo, (int)VD.ToDate].ToString());
                retrievedVoucherID = grdPayments[RetrieveNo, (int)VD.VoucherId].ToString();
                AmendNo = Convert.ToInt32(grdPayments[RetrieveNo, (int)VD.AmendNo]);
                if (AmendNo > 0)
                    rdbAmend.Checked = true;
                if(Convert.ToBoolean(grdPayments[RetrieveNo, (int)VD.Cancel]))
                    rdbCancell.Checked = true;
                if (Convert.ToBoolean(grdPayments[RetrieveNo, (int)VD.IsDriver]))
                {
                   rdbDriver.Checked = true;
                }
                else
                {
                    rdbGuide.Checked = true;
                }
                txtNoOfVehicles.Text = grdPayments[RetrieveNo, (int)VD.NoOfVehicles].ToString();
                txtBilling.Text = grdPayments[RetrieveNo, (int)VD.BillIns].ToString();
                txtOther.Text = grdPayments[RetrieveNo, (int)VD.OtherIns].ToString();
                txtConf.Text = grdPayments[RetrieveNo, (int)VD.Conf].ToString();
                txtAmenTo.Text = grdPayments[RetrieveNo, (int)VD.AmendTo].ToString();
                txtRef.Text = grdPayments[RetrieveNo, (int)VD.Ref].ToString();
                #endregion
                #region CLEAR EXISTING ROW VALUES
                clearGrid(RetrieveNo);
                #endregion
                btnRetrieve.Enabled = false;
                rdbAmend.Enabled = true;
                rdbCancell.Enabled = true;
                this.tcVoucher.SelectedTab = tabAddV;
            }
            else
            {
                MessageBox.Show("Please Select a Correct Row", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Add_Data())
            {
                rdbAmend.Enabled = false;
                btnOk.Enabled = true;
                btnPrint.Enabled = true;
                btnEmail.Enabled = true;
                btnRetrieve.Enabled = true;
                IsRetrive = false;
                RetrieveNo = 0;
                Clear_Contents();
                amountTxt.Text = "";
                fromDate.Value = Classes.clsGlobal.CurDate();
                toDate.Value = Classes.clsGlobal.CurDate();
                this.tcVoucher.SelectedTab = tbVV;
            }
        }
        private Boolean Add_Data()
        {
                int RowNumb = 1;
                if (Validate_Data() == false)
                {
                    return false;
                }
                if (IsRetrive)
                    RowNumb = RetrieveNo;
                else
                {
                    while (grdPayments[RowNumb, grdPayments.Cols[(int)VD.TourId].Index] != null)
                    {
                        RowNumb++;
                    }
                }
                grdPayments[RowNumb, (int)VD.TourId] =txtTourNo.Text.Trim();
                grdPayments[RowNumb, (int)VD.NoOfAdults] = noOfAdultsTxt.Text.Trim();
                grdPayments[RowNumb, (int)VD.NoOfChildren] = noOfChilderenTxt.Text.Trim();
                grdPayments[RowNumb, (int)VD.Amount] = amountTxt.Text.Trim();
                grdPayments[RowNumb, (int)VD.FromDate] = fromDate.Value;
                grdPayments[RowNumb, (int)VD.ToDate] = toDate.Value;
                grdPayments[RowNumb, (int)VD.VoucherTypeId] =cmbVoucherType.SelectedValue.ToString().Trim();                      
                grdPayments[RowNumb, (int)VD.Guest] = txtClientName.Text.Trim();
                grdPayments[RowNumb, (int)VD.VoucherName] = cmbVoucherType.Text.ToString().Trim();
                if (rdbDriver.Checked)
                    grdPayments[RowNumb, (int)VD.IsDriver] = true;
                else
                    grdPayments[RowNumb, (int)VD.IsDriver] = false;
                if (rdbDriver.Checked)
                {
                    grdPayments[RowNumb, (int)VD.ResponsibleId] = cmbDriver.SelectedValue.ToString().Trim();
                    grdPayments[RowNumb, (int)VD.ResposibleName] = cmbDriver.Text.ToString().Trim();
                }
                else
                {
                    grdPayments[RowNumb, (int)VD.ResponsibleId] = cmbGuide.SelectedValue.ToString().Trim();
                    grdPayments[RowNumb, (int)VD.ResposibleName] = cmbGuide.Text.ToString().Trim();
                }
                grdPayments[RowNumb, (int)VD.NoOfVehicles] = txtNoOfVehicles.Text.ToString().Trim();
                if (rdbAmend.Checked)
                {
                    grdPayments[RowNumb, (int)VD.AmendNo] = AmendNo + 1;
                    grdPayments[RowNumb, (int)VD.AmendTo] = retrievedVoucherID;
                }
                else
                {
                    if (AmendNo > 0)
                    {
                        grdPayments[RowNumb, (int)VD.AmendNo] = AmendNo;
                        grdPayments[RowNumb, (int)VD.AmendTo] = retrievedVoucherID;
                    }
                    else
                    {
                        grdPayments[RowNumb, (int)VD.AmendTo] = "";
                        grdPayments[RowNumb, (int)VD.AmendNo] = 0;
                    }
                }
                grdPayments[RowNumb, (int)VD.BillIns] = txtBilling.Text.ToString().Trim();
                grdPayments[RowNumb, (int)VD.OtherIns] = txtOther.Text.ToString().Trim();
                grdPayments[RowNumb, (int)VD.Conf] = txtConf.Text.ToString().Trim();                
                grdPayments[RowNumb, (int)VD.Ref] = txtRef.Text.ToString().Trim();
                grdPayments[RowNumb, (int)VD.Cancel] = rdbCancell.Checked ? 1 : 0;
                return true;
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Clear_Contents();
        }
        private void clearGrid(int RowNumb) 
        {
            grdPayments[RowNumb, (int)VD.TourId] = "";
            grdPayments[RowNumb, (int)VD.Guest] = "";
            grdPayments[RowNumb, (int)VD.FromDate] = "";
            grdPayments[RowNumb, (int)VD.ToDate] = "";
            grdPayments[RowNumb, (int)VD.VoucherTypeId] = "";
            grdPayments[RowNumb, (int)VD.VoucherName] = "";
            grdPayments[RowNumb, (int)VD.NoOfChildren] = "";
            grdPayments[RowNumb, (int)VD.NoOfAdults] = "";
            grdPayments[RowNumb, (int)VD.ResponsibleId] = "";
            grdPayments[RowNumb, (int)VD.Code] = "";
            grdPayments[RowNumb, (int)VD.ResposibleName] = "";
            grdPayments[RowNumb, (int)VD.EmpPhoto] = "";
            grdPayments[RowNumb, (int)VD.VehicleNo] = "";
            grdPayments[RowNumb, (int)VD.Brand] = "";
            grdPayments[RowNumb, (int)VD.Model] = "";
            grdPayments[RowNumb, (int)VD.Amount] = "";
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
                if (btnShow.Text == "")
                {
                    btnShow.Text = "";
                    this.Width = 644; 
                }
                else
                {
                    btnShow.Text = "";
                    this.Width = 905;
                }
        }
        private void cmbVoucherType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVoucherType.SelectedValue+"".Trim() != "")
            {
                int ComID = Convert.ToInt32(cmbVoucherType.SelectedValue);
                string sql = "SELECT VehicleType FROM mst_SafariCompanies WHERE ID=" + ComID + "";
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                if (DT.Rows.Count > 0)
                    lblVehicle.Text = "No Of " + DT.Rows[0]["VehicleType"].ToString().Trim();
                else
                    lblVehicle.Text = "Vehicel Type";
            }
        }
        private void rdbGuide_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtTourNo.Text.Trim() == "")
                    return;
                if (rdbGuide.Checked)
                {
                    setCombo(false);
                }
                else
                {
                    setCombo(true);
                }
            }
            catch (Exception ex){db.MsgERR(ex);}            
        }  
        private void setCombo(bool IsDriver)
        {
            try
            {
                if (IsDriver)
                {
                    lblResposible.Text = "Driver";
                    cmbDriver.Width = 182;
                    cmbDriver.Height = 21;
                    cmbDriver.Location = new Point(383, 16);
                    cmbGuide.Visible = false;
                    cmbDriver.Visible = true;
                }
                else
                {
                    lblResposible.Text = "Guide";
                    cmbGuide.Width = 182;
                    cmbGuide.Height = 21;
                    cmbGuide.Location = new Point(383, 16);
                    cmbDriver.Visible = false;
                    cmbGuide.Visible = true;
                }
            }
            catch (Exception ex){db.MsgERR(ex);} 
        }
    }
}
