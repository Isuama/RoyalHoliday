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
    public partial class ucFilterByOther : UserControl
    {
        public string sql;
        public ucFilterByOther(){InitializeComponent();}
        public string Query
        {
            set { sql = value; }
            get { return sql; }
        }
        private void chkIByOther_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIByOther.Checked)
                drpOther.Enabled = true;
            else
                drpOther.Enabled = false;
        }
        private void ucFilterByOther_Load(object sender, EventArgs e)
        {
            Intializer();
        }
        public void Intializer()
        {
            if (sql + "".Trim() == "")
                return;
            DataTable DTB = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(sql);
            drpOther.DataSource = DTB;
        }
    }
}
