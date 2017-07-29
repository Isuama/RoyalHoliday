using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.User_Controls
{
    public partial class ucFilterByCompany : UserControl
    {        
        public ucFilterByCompany(){InitializeComponent();}
        private void chkICmpny_CheckedChanged(object sender, EventArgs e)
        {
            if (chkICmpny.Checked)
                cmbICompany.Enabled = true;
            else
                cmbICompany.Enabled = false;
        }
        public void Intializer()
        { 
            cmbICompany.DataSource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT ID,CompanyName FROM mst_CompanyGenaral Where IsNull(IsActive,0)=1 ORDER BY ID");
        }
    }
}
