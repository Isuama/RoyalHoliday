using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Reports
{
    public partial class frmSetReportCategory : Form
    {
        private const string msghd = "Bank Master";
        int InsMode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        int Syscode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        string ssql = "SELECT ID ,ReportCat  ,Report  ,IsActive FROM mst_SetReportCategory Where Isnull([Status],0)<>7 Order By ReportCat";
        public frmSetReportCategory(){InitializeComponent();}
        public int Mode
        {
            get
            {
                return InsMode;
            }
            set
            {
                InsMode = value;
            }
        }
        public int SystemCode
        {
            get
            {
                return Syscode;
            }
            set
            {
                Syscode = value;
            }
        }
        public string SqlQry
        {
            get
            {
                return ssql;
            }
        }
        private void Intializer()
        {
            Fill_Control();
        }
        private void Fill_Control()
        {
            #region //________________________Fill Drp Reports ______________________________________________
             try
            { 
                    drpRpt.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT [ID],[ReportName] FROM [SOARSmartCommon].[dbo].[mst_Report]order by [ReportName]");
                  }
            catch (Exception ex){db.MsgERR(ex);}
            #endregion
            #region //________________________Fill Drp Reports category ______________________________________________ 
            try
            { 
                    drpRptCat.DataSource = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT [ID],[CategoryName],[IsActive] FROM mst_Report_Category  Order By CategoryName");
                   }
            catch (Exception ex){db.MsgERR(ex);}
            #endregion
        }
        private void Fill_Details()
        {
            #region __________________________________________Fill drop Report Category_____________________________________________________________
            DataTable DT;
            DataRow rw;
            try
            {                                                           //SELECT [ParentID],[CategoryName],[IsActive] FROM mst_Report_Category Order By CategoryName
                DT = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT [ID],[CategoryName],[IsActive] FROM mst_Report_Category Where ID=" + drpRptCat.SelectedValue + " Order By CategoryName");
                if (DT.Rows.Count > 0)
                {
                    rw = DT.Rows[0];
                    drpRptCat.SelectedValue = rw["ReportCat"].ToString();
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
            #endregion
            #region ____________________________Fill Report drop_____________________________________________________________________
            DataTable DT2;
            try
            {
                DT2 = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table(" SELECT ID,[ReportName],[CategoryID] FROM mst_Report Where ID=" + drpRptCat.SelectedValue + " ");
                if (DT2.Rows.Count > 0)
                {
                    string[] strAry = new string[DT2.Rows.Count];
                    for (int x = 0; x < DT2.Rows.Count; x++)
                    {
                        strAry[x] = DT2.Rows[x][1].ToString();
                    }
                    drpRpt.SelectedList = strAry;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }
        private Boolean Validate_Data()
        { 
            if (drpRptCat.Text.Trim() == "")
            {
                MessageBox.Show("Report Category cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if(drpRpt.SelectedList == null)
            {
                MessageBox.Show("Selected reports cannot be blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true; 
        }
        private Boolean Save_Pro()
        {
            Boolean rtnVal = false; 
                if (Validate_Data() == false)
                {
                    rtnVal = false;
                    return rtnVal;
                }
                if (Save_Data() == true)
                {
                    rtnVal = true;
                    return rtnVal;
                }
                return rtnVal; 
        }
        private Boolean Save_Data()
        {
            Boolean RtnVal = false; 
                if (Tourist_Management.Classes.clsGlobal.objComCon.ExecuteNonQuery(ssql) == true)
                {
                    RtnVal = true;
                }
                return RtnVal; 
        }
        private string Get_List(string strname)
        {
            string strRtn = "drpRpt";
            try
            {
                switch (strname)
                {
                    case "drpRpt": 
                        strRtn = drpRpt.SetList;
                        break;
                }
            }
            catch (Exception ex)            {                db.MsgERR(ex);            }
            return strRtn;
        }
        private void frmSetReportCategory_Load(object sender, EventArgs e)        {            Intializer();        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Save_Pro() == true)
            {
                MessageBox.Show("Record sucessfully Updated", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error occured", msghd, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void drpRptCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region _____________________________Fill Report1 _____________________________________________________________________
            DataTable DT3;
            try
            {
                drpRpt.SelectedList = null; 
                DT3 = Tourist_Management.Classes.clsGlobal.objComCon.Fill_Table("SELECT ID,[ReportName],[CategoryID] FROM mst_Report Where CategoryID=" + drpRptCat.SelectedValue + " ");
                if (DT3.Rows.Count > 0)
                {
                    string str = "";
                    str = DT3.Rows[0][0].ToString();
                    drpRpt.SetList = str; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }
    }
}
