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
    public partial class ucNavComponents : UserControl
    {
        private const string msghd = "Navigator Component";
        string strName = "";
        string strCaption = "";
        DataRow[] DT;
        Image Img;
        ImageList ImgList; 
        public string ParentName
        {
            get { return strName; }
            set { strName = value; }
        } 
        public string ParentText
        {
            get { return strCaption; }
            set { strCaption = value; }
        } 
        public DataRow[]  DataSource
        {
            get { return DT; }
            set { DT = value; }
        } 
        public Image ParentImage
        {
            get { return Img; }
            set { Img = value; }
        }
        public ucNavComponents()   {  InitializeComponent();   } 
        private void UserControl1_Load(object sender, EventArgs e)
        {
            Resize_Contrls();
            Make_Images();
            Make_Tree();
        } 
        private void Resize_Contrls()
        {
            this.Width = Width;
            this.Height = Height;
            lstvwTree.Width = Width - (picExpnd.Left +  picExpnd.Width);
            lstvwTree.Height = (Height - (lstvwTree.Top+5));
            pnlEnd.Top = (lstvwTree.Height + lstvwTree.Top);
            picParent.BackgroundImage = ParentImage;
            picParent.BackgroundImageLayout = ImageLayout.Zoom;
            pnlEnd.Width = picParent.Width + picExpnd.Width + lstvwTree.Width;
            lblParent.Left = picParent.Left;
        } 
        private void Make_Images()
        {
            try
            { 
            ImgList = new ImageList();
            ImgList.ImageSize = new Size(32, 32);
            foreach (DataRow dr in DT)  ImgList.Images.Add(Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])dr[2])); 
            }
            catch (Exception ex)
            {  db.MsgERR(ex);  }
        }
        private void Make_Tree()
        {
            try
            {
                int x = 0;
                lblParent.Text = ParentName;
                lstvwTree.BeginUpdate();
                lstvwTree.LargeImageList = ImgList;
               foreach (DataRow dr in DT)
                {
                    ListViewItem listItem = new ListViewItem(dr[1].ToString());
                    listItem.ImageIndex = x;
                    listItem.Name = dr[0].ToString();
                    listItem.ToolTipText = dr[1].ToString();
                    lstvwTree.Items.Add(listItem);
                    x += 1;
                }
                lstvwTree.EndUpdate(); 
            } 
            catch (Exception ex)
            {  db.MsgERR(ex); }
        } 
        private void lstvwTree_ItemActivate(object sender, EventArgs e)
        { 
            Tourist_Management.Main.frmMDIMain.MDI.open_Form_Navigator(lstvwTree.FocusedItem.Name, lstvwTree.FocusedItem.Text);
        } 
    }
}
