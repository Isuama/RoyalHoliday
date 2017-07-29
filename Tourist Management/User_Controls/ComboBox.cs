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
    public partial class ComboBox : System.Windows.Forms.ComboBox
    { 
        [DefaultValue("")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        public new object DataSource { get{ return base.DataSource; } set{
            DataTable dt=null;
            if (value != null)
            {
                if (value is DataTable) dt = (DataTable)value;
                if (value is DataSet) dt = ((DataSet)value).Tables[0];
                if (dt != null)
                {
                            base.ValueMember=dt.Columns[0].ToString(); 
                    switch (dt.Columns.Count)
                    {
                        case  1:  base.DisplayMember =    dt.Columns[0].ToString(); break;
                        default:  base.DisplayMember  = dt.Columns[1].ToString();      break;
                    }
                } 
            } 
            base.DataSource = value;  
        } }
    }
}
