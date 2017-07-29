using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using C1.Win.C1FlexGrid;
using System.Collections;
namespace Tourist_Management.User_Controls
{
    public partial class ucReportViewer : UserControl
    {
        DataTable SRC;
        string rptID = "";
        int LogoPos = 1;
        int firstRPP = 0;
        int NoOfHeadLine = 2;
        int intLoGoSize = 4;
        int cpp=0;
        int rpp=100;
        Size PGSZ=new Size(8,11);
        int RWHGT=8;
        int MyH = 0;
        int MyW = 0;
        Font Fnt01 = new Font("Times New Roman", 8);
        Font Fnt02 = new Font("Times New Roman", 8);
        Font Fnt03 = new Font("Tahoma", 8);
        Color Color01 = Color.Black;
        Color Color02 = Color.Black;
        Color Color03=Color.Black;
        bool isGrid = false;
        bool IsAllowDetailFontchange = false;
        Other.frmList frmParent;
        PrintPreviewDialog dlg = new PrintPreviewDialog();
        public C1FlexGroup.C1FlexGroup fG1;
        private const string msghd = "Report Viewer Control";
        public DataTable MySource
        {
            get { return SRC; }
            set { SRC = value; }
        }
        public Other.frmList MyParent
        {
            set { frmParent = value; }
        }
        public string ReportID
        {
            get { return rptID; }
            set { rptID = value; }
        }
        public int MyCOL
        {
            get { return cpp; }
            set { cpp = value; }
        }
        public int MyRW
        {
            get { return rpp; }
            set { rpp = value; }
        }
        public int NoOfHeadLines
        {
            get { return NoOfHeadLine; }
            set { NoOfHeadLine = value; }
        }
        public int LogoSize
        {
            get { return intLoGoSize; }
            set { intLoGoSize = value; }
        }
        public int LogoPosition
        {
            get { return LogoPos; }
            set { LogoPos = value; }
        }
        public Size MyPGSZ
        {
            get { return PGSZ; }
            set { PGSZ = value; }
        }
        public int MyRowHeight
        {
            get { return RWHGT; }
            set { RWHGT = value; }
        }
        public int MyHeight
        {
            get { return MyH; }
            set { MyH = value; }
        }
        public int MyWidth
        {
            get { return MyW; }
            set { MyW = value; }
        }
        public Font Font01
        {
            get { return Fnt01; }
            set { Fnt01 = value; }
        }
        public Font Font02
        {
            get { return Fnt02; }
            set { Fnt02 = value; }
        }
        public Font Font03
        {
            get { return Fnt03; }
            set { Fnt03 = value; }
        }
        public Color COLOR01
        {
            get { return Color01; }
            set { Color01 = value; }
        }
        public Color COLOR02
        {
            get { return Color02; }
            set { Color02 = value; }
        }
        public Color COLOR03
        {
            get { return Color03; }
            set { Color03 = value; }
        }
        public ucReportViewer(){InitializeComponent();}
        private void ucReportViewer_Load(object sender, EventArgs e)
        {
            Initializer();
            Set_DataSource();
            fG1 = flxGroup;
        }
        private void Initializer()
        {
            try
            {
            this.Height = MyH;
            this.Width = MyW;
                switch (LogoPosition.ToString())
                {
                    case "0":
                        firstRPP = rpp;
                        break;
                    case "1":
                        firstRPP = rpp - (NoOfHeadLine + intLoGoSize+1);
                        break;
                    default:
                        if (LogoPos > 1 && intLoGoSize < NoOfHeadLine)
                        {
                            intLoGoSize = NoOfHeadLine;
                        }
                        firstRPP = rpp -intLoGoSize;
                        break;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void toolBarRP_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            try
            {
                switch (e.Button.Name.ToString())
                {
                    case "toolBarButton1"://tbSetup":
                        page_Setup();
                        break;
                    case "BEdit"://tbPreview":
                        print_report();
                        break;
                    case "BSearch"://tbPrinter":
                        Print_Setup();
                        break;
                    case "BRefresh":
                        Refresh_reports();
                        break;
                    case "FINDER":
                        break;
                    case "BPrint"://tbFormat":
                       Other.frmFormatPrint frmFMP;
                       frmFMP = new Tourist_Management.Other.frmFormatPrint();
                       frmFMP.RPViewer = this;
                       frmFMP.StartPosition = FormStartPosition.CenterScreen;
                       frmFMP.ShowDialog();
                       if (frmFMP.ShowDialog() == DialogResult.OK)
                       {
                           Format_Grid(LogoPos);
                       }
                        break;
                    case "BExport":
                        SaveFileDialog save = new SaveFileDialog();
                        save.Filter = "Excel File | *.xls, *.xlsx";
                        if (save.ShowDialog() == DialogResult.OK)
                        {
                            flxGroup.Grid.SaveExcel(save.FileName);
                        }
                        break;
                    case "BDelete"://tbCloseRpt":
                        frmParent.Remove_Control();
                        break;
                    case "tbSetPrint":
                        Other.frmSetPrintArea frmSETAREA;
                        frmSETAREA = new Tourist_Management.Other.frmSetPrintArea();
                        frmSETAREA.RPViewer = this;
                        frmSETAREA.StartPosition = FormStartPosition.CenterScreen;
                        frmSETAREA.ShowDialog();
                        if (frmSETAREA.DialogResult == DialogResult.OK)
                        {
                            Set_Report();
                        }
                        break;
                    case "tbMemo":
                        break;
                }
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        private void ucReportViewer_Resize(object sender, EventArgs e)
        {
            flxGroup.Width = this.Width;
            flxGroup.Grid.Width = this.Width; ;
            flxGroup.Height = this.Height - flxGroup.Top;
        }
        private void Set_DataSource()
        {
            flxGroup.Grid.DataSource = MySource;
            cpp = flxGroup.Grid.Cols.Count;
            Format_Grid(LogoPos);
            Set_Report();
        }
        private void Format_Grid(int intPOS)
        {
            CellRange rng1;
            CellRange rng2;
            CellStyle CS1;
            CellStyle CS2;
            CellStyle CS3;
            CellStyle CS4;
            try
            {
                flxGroup.Grid.Rows.Fixed = 5;
                RWHGT = 12;// flxGroup.Grid.Rows[0].Height;
                flxGroup.Grid.AllowMerging = AllowMergingEnum.Free;
                CS1 = flxGroup.Grid.Styles.Add("CS1");
                CS2 = flxGroup.Grid.Styles.Add("CS2");
                CS3 = flxGroup.Grid.Styles.Add("CS3");
                CS4 = flxGroup.Grid.Styles.Add("CS4");
                CS1.BackColor = Color.Green;
                CS1.Border.Color = Color.White;
                CS2.TextAlign = TextAlignEnum.CenterCenter;
                CS2.BackColor = Color.White;
                CS2.Border.Color = Color.White;
                CS2.Border.Direction = BorderDirEnum.Both;
                CS2.Font = Font01;
                CS2.ForeColor = COLOR01;
                CS3.TextAlign = TextAlignEnum.CenterCenter;
                CS3.BackColor = Color.White;
                CS3.Border.Direction = BorderDirEnum.Both;
                CS3.Border.Color = Color.Black;
                CS3.Font = Font02;
                CS3.ForeColor = COLOR02;
                CS4.TextAlign = TextAlignEnum.CenterCenter;
                CS4.BackColor = Color.White;
                if (isGrid == false)
                {
                    CS4.Border.Color = Color.White;
                }
                else
                {
                    CS4.Border.Color = Color.Black;
                }
                CS4.Font = Font03;
                CS4.ForeColor = COLOR03;
                for (int x = 0; x <= flxGroup.Grid.Cols.Count - 1; x++)
                {
                    flxGroup.Grid.Cols[x].AllowMerging = true;
                }
                for (int x = 0; x <= flxGroup.Grid.Cols.Count - 1; x++)
                {
                    flxGroup.Grid.Rows[4][x] = flxGroup.Grid.Rows[0][x];
                    flxGroup.Grid.SetCellStyle(4, x, CS3);
                    flxGroup.Grid.SetCellStyle(3, x, CS2);
                    flxGroup.Grid.SetCellStyle(2, x, CS2);
                    flxGroup.Grid.SetCellStyle(1, x, CS2);
                    flxGroup.Grid.SetCellStyle(0, x, CS1);
                }
                if (IsAllowDetailFontchange == true)
                {
                    for (int x = 0; x <= flxGroup.Grid.Cols.Count - 1; x++)
                    {
                        for (int y = flxGroup.Grid.Rows.Fixed; y <= flxGroup.Grid.Rows.Count - 1; y++)
                        {
                            flxGroup.Grid.SetCellStyle(y, x, CS4);
                        }
                    }
                }
                if (intPOS > 1 && intLoGoSize < NoOfHeadLine)
                {
                    intLoGoSize = NoOfHeadLine;
                }
                flxGroup.Grid.Rows[0].Height = 5;
                flxGroup.Grid.Rows[2].AllowMerging = true;
                flxGroup.Grid.Rows[3].AllowMerging = true;
                rng1 = flxGroup.Grid.GetCellRange(2, 0, 2, flxGroup.Grid.Cols.Count - 1);
                rng2 = flxGroup.Grid.GetCellRange(3, 0, 3, flxGroup.Grid.Cols.Count - 1);
                switch (intPOS.ToString())
                {
                    case "0" :
                        rng2.Data = " FILTER2 GOES HERE ";
                        rng1.Data = "SAMPLE REPORT \n FILTER GOES HERE";
                        flxGroup.Grid.Rows[2].TextAlign = TextAlignEnum.CenterCenter;
                        flxGroup.Grid.Rows[3].TextAlign = TextAlignEnum.CenterCenter;
                        rng1.Image = null;
                        flxGroup.Grid.Rows[2].Height = NoOfHeadLine*RWHGT ;
                        flxGroup.Grid.Rows[3].Height = RWHGT;
                        break;
                   case "1":
                        rng1.Data = " ";
                        rng2.Data = "SAMPLE REPORT \n FILTER GOES HERE";
                        flxGroup.Grid.Rows[3].TextAlign = TextAlignEnum.CenterCenter;
                        flxGroup.Grid.Rows[2].ImageAlign = ImageAlignEnum.CenterCenter;
                        rng1.Image = Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Company_Logo FROM dbo.mst_CompanyGenaral").Rows[0][0]);
                        rng1.Image = rng1.Image.GetThumbnailImage(RWHGT * intLoGoSize, RWHGT * intLoGoSize, null, IntPtr.Zero);
                        flxGroup.Grid.Rows[2].Height = RWHGT*intLoGoSize;
                        flxGroup.Grid.Rows[3].Height = RWHGT * (NoOfHeadLine+1);
                        break;
                        case "2":
                        rng2.Data = "FILTER2 GOES HERE ";
                        rng1.Data = "SAMPLE REPORT \n FILTER GOES HERE";
                        flxGroup.Grid.Rows[2].TextAlign = TextAlignEnum.LeftCenter;
                        flxGroup.Grid.Rows[2].ImageAlign = ImageAlignEnum.RightCenter;
                        flxGroup.Grid.Rows[3].TextAlign = TextAlignEnum.CenterCenter;
                        rng1.Image = Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Company_Logo FROM dbo.mst_CompanyGenaral").Rows[0][0]);
                        rng1.Image = rng1.Image.GetThumbnailImage(RWHGT * intLoGoSize, RWHGT * intLoGoSize, null, IntPtr.Zero);
                        flxGroup.Grid.Rows[2].Height = RWHGT * intLoGoSize;
                        flxGroup.Grid.Rows[3].Height = RWHGT ;
                        break;
                        case "3":
                        rng2.Data = "FILTER2 GOES HERE ";
                        rng1.Data = "SAMPLE REPORT \n FILTER GOES HERE";
                        flxGroup.Grid.Rows[2].TextAlign = TextAlignEnum.RightCenter;
                        flxGroup.Grid.Rows[2].ImageAlign = ImageAlignEnum.LeftCenter;
                        flxGroup.Grid.Rows[3].TextAlign = TextAlignEnum.CenterCenter;
                        rng1.Image = Tourist_Management.Classes.clsGlobal.byteArrayToImage((byte[])Tourist_Management.Classes.clsGlobal.objCon.Fill_Table("SELECT Company_Logo FROM dbo.mst_CompanyGenaral").Rows[0][0]);
                        rng1.Image = rng1.Image.GetThumbnailImage(RWHGT * intLoGoSize, RWHGT * intLoGoSize, null, IntPtr.Zero);
                        flxGroup.Grid.Rows[2].Height = RWHGT * intLoGoSize;
                        flxGroup.Grid.Rows[3].Height = RWHGT;
                        break;
                }
                for (int x = 1; x < flxGroup.Grid.Cols.Count; x++)
                {
                    flxGroup.Grid.Cols[x].Width = (this.Width - 10) / (flxGroup.Grid.Cols.Count - 1);
                }
                flxGroup.Grid.Cols[0].Width = 0;
                flxGroup.Grid.Cols[1].Width = 0;
                flxGroup.Grid.Update();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        public class FlexPrintDocument : PrintDocument
        {
            private ArrayList _images;
            private int _currentPage;
            private float _zoom;
            public FlexPrintDocument(C1FlexGrid flex)
            {
                DocumentName = flex.Name;
                _zoom = 1;
                _images = new ArrayList();
                int r1, c1, r2, c2;
                c1 = c2 = flex.Cols.Fixed;
                for (int c = flex.Cols.Fixed; c < flex.Cols.Count; c++)
                {
                    if (c == flex.Cols.Count - 1 || (flex.Cols[c].UserData != null && flex.Cols[c].UserData.ToString() == "*"))
                    {
                        c2 = c;
                        r1 = r2 = flex.Rows.Fixed;
                        for (int r = flex.Rows.Fixed; r < flex.Rows.Count; r++)
                        {
                            if (r == flex.Rows.Count - 1 || (flex.Rows[r].UserData != null && flex.Rows[r].UserData.ToString() == "*"))
                            {
                                r2 = r;
                                _images.Add(flex.CreateImage(r1, c1, r2, c2));
                                r1 = r + 1;
                            }
                        }
                        c1 = c + 1;
                    }
                }
            }
            override protected void OnBeginPrint(PrintEventArgs e)
            {
                _currentPage = 0;
                base.OnBeginPrint(e);
            }
            override protected void OnPrintPage(PrintPageEventArgs e)
            {
                if (_currentPage >= _images.Count)
                {
                    e.HasMorePages = false;
                    e.Cancel = true;
                    return;
                }
                if (_currentPage == 0)
                {
                    SizeF szMax = Size.Empty;
                    foreach (Image page in _images)
                    {
                        szMax.Height = Math.Max(szMax.Height, page.Height);
                        szMax.Width = Math.Max(szMax.Width, page.Width);
                    }
                    SizeF szPage = e.MarginBounds.Size;
                    _zoom = 1;
                    float zh = szPage.Width / szMax.Width;
                    float zv = szPage.Height / szMax.Height;
                    if (zh < 1 || zv < 1)
                        _zoom = Math.Min(zh, zv);
                }
                Graphics g = e.Graphics;
                Rectangle rc = e.MarginBounds;
                Image img = _images[_currentPage] as Image;
                rc.Width = (int)(img.Width * _zoom);
                rc.Height = (int)(img.Height * _zoom);
                g.DrawImage(img, rc);
                _currentPage++;
                e.HasMorePages = _currentPage < _images.Count;
                base.OnPrintPage(e);
            }
        }
        private void Set_Report()
        {
            bool IsFirst = true;
            int tempVal = rpp;
            try
            {
                if (flxGroup.Grid.Rows.Count > 1)
                {
                    flxGroup.Grid.Rows[0].Visible = false;
                }
                for (int r = flxGroup.Grid.Rows.Fixed; r < flxGroup.Grid.Rows.Count; r++)
                {
                    if (IsFirst == true)
                    {
                        rpp = firstRPP;
                    }
                    else
                    {
                        rpp = tempVal;
                    }
                    flxGroup.Grid.Rows[r].UserData = (r % rpp == 0)
                        ? "*"
                        : null;
                    if (r % rpp == 0) { IsFirst = false; }
                }
                IsFirst = true;
                tempVal = 0;
                for (int c = flxGroup.Grid.Cols.Fixed; c < flxGroup.Grid.Cols.Count; c++)
                {
                    flxGroup.Grid.Cols[c].UserData = (c % cpp == 0)
                        ? "*"
                        : null;
                }
                FlexPrintDocument PD;
                PD = new FlexPrintDocument(flxGroup.Grid);
                printDocument1 = PD;
                dlg.Document = printDocument1;
            }
            catch (Exception ex){db.MsgERR(ex);}
            finally
            {
                if (flxGroup.Grid.Rows.Count > 1)
                {
                    flxGroup.Grid.Rows[0].Visible = true;
                }
            }
        }
        private void page_Setup()
        {
            PageSetupDialog pageSetup = new PageSetupDialog();
            pageSetup.Document = printDocument1;
            pageSetup.ShowDialog();
        }
        private void Print_Setup()
        {
            PrintDialog PrintSetup = new PrintDialog();
            PrintSetup.Document = printDocument1;
            PrintSetup.ShowDialog();
        }
        private void print_report()
        {
            ((Form)dlg).WindowState = FormWindowState.Maximized;
            dlg.ShowDialog();
        }
        private void Refresh_reports()
        { 
            try
            { 
               MySource = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(" EXEC [spReturn_Reports] " + ReportID);
                Initializer();
                Set_DataSource();
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
    }
}
