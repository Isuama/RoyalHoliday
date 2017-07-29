using System;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Diagnostics;
namespace Tourist_Management.Classes
{
    static class clsForms
    {
        static string SQry = "";
        public static Form ShowDialog(string strFormName, int mode, double SystemCode)
        {
            Form frm = Classes.clsForms.rtnForm("frmCity", 0, 0);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            return frm;
        }
        public static Form rtnForm(string strFormName, int mode, double SystemCode)
        {
            string[] frmName = strFormName.Split(new Char[] { '_' });
            Form f = null;
            try
            {
                foreach (string s in "Settings,Accounts,Master,Email,Settings,Transaction,OtherFunc,Pre_Tour_Cost,Reports,Main,Other,Transport_Report,Account_Reports,".Split(new Char[] { ',' }))
                    if (null != (f = Assembly.GetExecutingAssembly().CreateInstance("Tourist_Management" + (s.Length > 0 ? "." + s : "") + "." + frmName[0].ToString()) as Form)) break;
            }
            catch (System.NullReferenceException) { MessageBox.Show("Invalid Form name. Please note that Form names are case sensitive.", "Form not found"); return null; }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); return null; }
            if (f != null)
            {
                Debug.WriteLine("Loaded: " + f.GetType().ToString());
                object[] oo = { };
                BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;
                Binder bi = Type.DefaultBinder; FieldInfo fi;
                Type T = f.GetType();
                if (null != (fi = T.GetField("Mode"))) fi.SetValue(f, mode, bf, bi, System.Globalization.CultureInfo.CurrentCulture);
                if (null != (fi = T.GetField("SystemCode"))) fi.SetValue(f, Convert.ToInt32(SystemCode), bf, bi, System.Globalization.CultureInfo.CurrentCulture);
                if (null != (fi = T.GetField("SqlQry"))) SQry = fi.GetValue(f) + "";
                if (null != T.GetProperty("Mode")) T.GetProperty("Mode").SetValue(f,mode, bf, bi, oo, System.Globalization.CultureInfo.CurrentCulture) ;
                if (null != T.GetProperty("SystemCode")) T.GetProperty("SystemCode").SetValue(f, Convert.ToInt32(SystemCode), bf, bi, oo, System.Globalization.CultureInfo.CurrentCulture);
                if (null != T.GetProperty("SqlQry")) SQry = T.GetProperty("SqlQry").GetValue(f, bf, bi, oo, System.Globalization.CultureInfo.CurrentCulture) + "";
                f.Text = Set_FormName(frmName[0].ToString());
                f.Name = strFormName;
            }
            switch (frmName[0].ToString())
            {
                case "frmList":
                    Tourist_Management.Other.frmList frmLS;
                    frmLS = new Other.frmList();
                    frmLS.Name = strFormName;
                    frmLS.OpenForm = rtnForm(frmName[1], mode, SystemCode);
                    frmLS.Text = frmLS.OpenForm.Text;
                    frmLS.SqlQuery = SQry;
                    return frmLS;
                default: if (f != null) return f; else MessageBox.Show("Form Not Found: " + frmName[0].ToString()); return new Form();
            }
        }
        public static Boolean delete_Records(string strForm, int SystemCode)
        {
            if (Delete_Validation(strForm, SystemCode) == true) return false;
            int UserID = Convert.ToInt32(clsGlobal.UserID.ToString());
            switch (strForm.ToString())
            {
                case "frmDistrict": return clsGlobal.CommonCon.ExecuteNonQuery("Update mst_DistrictMaster set status=7,DeletedBy=" + UserID + ",DeletedDate=Getdate() where id=" + SystemCode.ToString() + "");
                case "frmCityItinerary": return clsGlobal.CommonCon.ExecuteNonQuery("Update mst_CityItinerary set status=7,DeletedBy=" + UserID + ",DeletedDate=Getdate() where id=" + SystemCode.ToString() + "");
                default: return false;
            }
        }
        public static Boolean Delete_Validation(string frm, int Syscode)
        {
            switch (frm.ToString())
            {
                case "frmBank": return Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,BankID,BranchCode,Status FROM mst_BankBranchMaster where Isnull([Status],0)<>7 and BankID =" + Syscode + "").Rows.Count != 0;
                case "frmBankBranch": return (int)(Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("EXEC sp_ValidateBeforeDelete 'mst_CompanyBankDetails','BranchID=" + Syscode + "'").Rows[0][0]) != 0;
            }
            return false;
        }
        public static Boolean Is_Common(string frm)// Stat and end with ","
        {
            return ",frmAirport,frmBasisTypes,frmRoomTypes,frmBank,frmBankBranch,frmCountry,frmCity,frmCityItinerary,frmSightSeeing,frmSightSeeingCat,frmLanguages,frmVehicleAccess,frmBankAccType,frmNewUser,frmBankFileLayout, frmReportCategory,frmAccountGroup,frmAccountLedger,".Contains("," + frm + ",");
        }
        public static string Set_FormName(string strName)
        {
            DataTable DT = Classes.clsGlobal.objComCon.Fill_Table("SELECT DisplayName FROM dbo.vwForms Where formName='" + strName + "'");
            if (DT.Rows.Count > 0) { return DT.Rows[0][0].ToString(); }
            return "Reference Missed";
        }    
    }
}