using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Email
{
    public partial class frmFilterEmail : Form
    {
        private const string msghd = "Select Email Contacts";
        public string SelectedContacts;
        enum CD { gNME,gEML,gSEL };
        public frmFilterEmail(){InitializeComponent();}
        private void frmFilterEmail_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        private void Intializer()
        {
            try
            {
                Fill_Control();
                Grd_Initializer();
                Fill_Grid();
                this.cmbContType.Select();
                SelectedContacts = "";
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Control()
        {
            try
            { 
                cmbContType.DataSource  = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,GroupByName FROM eml_FilterContacts Where IsNull(IsActive,0)=1 ORDER BY ID"); 
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Grd_Initializer()
        {
            try
            {
                grdContact.Cols.Count = 3;
                grdContact.Cols[(int)CD.gNME].Width = 394;
                grdContact.Cols[(int)CD.gEML].Width = 0;
                grdContact.Cols[(int)CD.gSEL].Width = 50;
                grdContact.Cols[(int)CD.gNME].Caption = "Contact Name";
                grdContact.Cols[(int)CD.gEML].Caption = "Email Address";
                grdContact.Cols[(int)CD.gSEL].Caption = "Select";
                grdContact.Cols[(int)CD.gSEL].DataType = Type.GetType(" System.Boolean");
                grdContact.Rows[1].AllowEditing = true;
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void Fill_Grid()
        {
            int RowNumb;
            int CMB=0;
            DataTable DTCont,DTFill;
            string sql, ssql, TblName, FldName1, FldName2;
            CMB=Convert.ToInt16(cmbContType.SelectedValue.ToString().Trim());
            if (CMB == 1)
            {
                return;
            }
            else
            {
            ssql = "SELECT ID,GroupByName,TableName,FieldName1,FieldName2,IsNull(IsActive,0)AS IsActive" +
                " FROM eml_FilterContacts WHERE IsActive=1 AND ID=" + CMB + " ORDER BY ID";
                DTCont = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(ssql);
            }
                if (DTCont.Rows.Count > 0)
                {
                    TblName = DTCont.Rows[0]["TableName"].ToString();
                    FldName1 = DTCont.Rows[0]["FieldName1"].ToString();
                    FldName2 = DTCont.Rows[0]["FieldName2"].ToString();
                    sql = "SELECT " + FldName1 + "," + FldName2 + " FROM " + TblName + " ORDER BY " + FldName1 + "  ";
                    DTFill = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
                    if (DTFill.Rows.Count > 0)
                    {
                        grdContact.Rows.Count = 1;
                        grdContact.Rows.Count = DTFill.Rows.Count+1;
                        RowNumb = 0;
                        while (DTFill.Rows.Count > RowNumb)
                        {
                            grdContact[RowNumb + 1, (int)CD.gNME] = DTFill.Rows[RowNumb][0].ToString();
                            grdContact[RowNumb + 1, (int)CD.gEML] = DTFill.Rows[RowNumb][1].ToString();
                            RowNumb++;
                        }
                    }
                }
        }
        private void cmbContType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Fill_Grid();
        }
        private void chkShowEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowEmail.Checked)
                grdContact.Cols[(int)CD.gEML].Width = 100;
            else
                grdContact.Cols[(int)CD.gEML].Width = 0;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            Get_Selected_Contacts();
            this.Close();
        }
        public void Get_Selected_Contacts()
        {
            try
            {
                int RowNumb = 1;
                SelectedContacts = "";
                while (grdContact.Rows.Count!=RowNumb)//[RowNumb, grdContact.Cols[(int)CD.gNME].Index] != null)
                {
                    if (Convert.ToBoolean(grdContact[RowNumb, (int)CD.gSEL]))
                    {
                        if (grdContact[RowNumb, (int)CD.gEML] != null && grdContact[RowNumb, (int)CD.gEML].ToString() != "")
                            SelectedContacts += grdContact[RowNumb, (int)CD.gEML].ToString() + ";";
                    }
                    RowNumb++;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
