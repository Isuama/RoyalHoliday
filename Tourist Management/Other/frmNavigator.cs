using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Other
{
    public partial class frmNavigator : Form
    {
        private const string msghd = "Navigator Component";
        public int ParentID = 0;
        public string ParentName = "", ParentText = "";
        public byte[] ParentImage;
        DataTable DT ; string stat="";
        public frmNavigator()        {            InitializeComponent();        } 
        private void frmNavigator_Activated(object sender, EventArgs e) { ((Main.frmMDIMain)MdiParent).Resize_Main(); }
        public  void Fill_Records()
        { string gSQL, n;
            try
            {  
                 if ((n = Classes.clsGlobal.Is_Admin.ToString() + Classes.clsGlobal.Is_SuperUser.ToString()) != stat)
                 {
                     stat = n;
                     if (Classes.clsGlobal.Is_Admin == true) gSQL = ("SELECT Name,[Desc],Img,ParentID,Mode,ModuleID FROM dbo.[Fun_ReturnUserModule](0,0,0) Where  ISNULL(IsCritical,0)=0 Order By SortOrder");
                     else if (Classes.clsGlobal.Is_SuperUser == true) gSQL = ("SELECT Name,[Desc],Img,ParentID,Mode,ModuleID FROM dbo.[Fun_ReturnUserModule](0,1,-1)     Order By SortOrder");
                     else gSQL = ("SELECT Name,[Desc],Img,ParentID,Mode,ModuleID FROM dbo.[Fun_ReturnUserModule](" + Classes.clsGlobal.UserID.ToString() + ",0,0) Where   GroupID=" + Classes.clsGlobal.UserID.ToString() + " and ISNULL(IsCritical,0)=0 Order By SortOrder");
                     DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(gSQL); 
                 } 
                this.SuspendLayout();
                while (this.Controls.Count > 0) this.Controls.RemoveAt(0);
                foreach (DataRow r in DT.Select("Mode=0 AND ParentID="+ParentID))
                { 
                    Tourist_Management.User_Controls.ucNavComponents UC = new Tourist_Management.User_Controls.ucNavComponents();
                    UC.ParentName = r[0]+"";
                    UC.ParentText = r[1] + "";  
                    UC.ParentImage = Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])r[2]); 
                    UC.DataSource = DT.Select("ParentID=" + r[5] );
                    UC.SetBounds(0, this.Controls.Count * 150, this.Width - 50, 125);
                    this.Controls.Add(UC);
                }
                Tourist_Management.User_Controls.ucNavComponents UCN;
                UCN = new Tourist_Management.User_Controls.ucNavComponents();
                UCN.ParentName = ParentName;
                UCN.ParentText = ParentText;
                UCN.ParentImage = Tourist_Management.Classes.clsGlobal.byteArrayToImage(ParentImage); 
                UCN.SetBounds(0, 0, this.Width - 50, 125); 
                UCN.DataSource = DT.Select("Mode<>0 AND ParentID=" + ParentID);
                if (UCN.DataSource.Length > 0)
                {
                    UCN.Location = new Point(0, this.Controls.Count * 150);
                    this.Controls.Add(UCN);
                } 
            }
            catch (Exception ex) { stat = ""; db.MsgERR(ex); }
            finally { this.ResumeLayout(); }
        } 
    }
}
