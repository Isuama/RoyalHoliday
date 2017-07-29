using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
namespace Tourist_Management.DropDowns
{
    public partial class DropSearch : UserControl
    {
        DataTable dt;
        public String FormName;
        ListBox lb = new ListBox() { BorderStyle = BorderStyle.None };
        TextBox tb = new TextBox();
        ToolStripDropDown dd = new ToolStripDropDown { CanOverflow = true, AutoClose = true, DropShadowEnabled = true };
        item _SEL = item.Empty;
        public event EventHandler Click_Open, Selected_TextChanged;
        item SEL { get { return _SEL; } set { _SEL = value; cb.Text = _SEL.V; if (Selected_TextChanged != null)  Selected_TextChanged(this, new EventArgs()); } }
        public DataTable DataSource { get { return dt; } set { dt = value; /*string k = _SEL.K; _SEL = item.Empty; cb.Text = ""; setSelectedValue(k); */ } }
        public void setSelectedValue(string str) { if (SEL.K == str) return; if (dt != null) foreach (DataRow r in dt.Rows) if (r[0] + "" == str + "") { _SEL = new item(r); cb.Text = _SEL.V; return; } _SEL = item.Empty; cb.Text = _SEL.V; }
        private void B2_Click(object sender, EventArgs e) { if (Click_Open != null) Click_Open(this, e); }
        private void DropSearch_Resize(object sender, EventArgs e)
        {
            Height = cb.Height;
            cb.Width = Width - 54;
            B1.SetBounds(Width - 50, 0, 21, Height);
            B2.SetBounds(Width - 25, 0, 21, Height);
            dd.Size = this.Size;
        }
        public DropSearch()
        {
            InitializeComponent();
            dd.Items.Add(new ToolStripControlHost(tb) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false, Size = new Size(this.Size.Width, 20) });
            dd.Items.Add(new ToolStripControlHost(lb) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false, Size = new Size(this.Size.Width, this.Size.Height + 20) });
            lb.Click += new EventHandler(lb_Click);
            dd.Closed += new ToolStripDropDownClosedEventHandler(_toolStripDropDown_Closed);
            dd.VisibleChanged += new EventHandler(_toolStripDropDown_VisibleChanged);
            tb.TextChanged += new EventHandler(tb_TextChanged);
            tb.KeyUp += new KeyEventHandler(tb_KeyUp);
        }
        void tb_KeyUp(object sender, KeyEventArgs e)
        {
            if (lb.Items.Count > 0)
                switch (e.KeyCode)
                {
                    case Keys.Up: lb.SelectedIndex = Math.Max(0, lb.SelectedIndex - 1); break;
                    case Keys.Down: lb.SelectedIndex = Math.Min(lb.Items.Count - 1, lb.SelectedIndex + 1); break;
                    case Keys.Enter:
                    case Keys.Escape: dd.Close(); break;
                }
        }
        void tb_TextChanged(object sender, EventArgs e)
        {
            lb.Items.Clear();
            if (dt != null) foreach (DataRow r in dt.Rows) if ((" " + r[1]).ToLower().Contains(" " + tb.Text.ToLower())) { lb.Items.Add(new item(r)); if (r[0] + "" == SEL.K) lb.SelectedIndex = lb.Items.Count - 1; }
        }
        void _toolStripDropDown_VisibleChanged(object sender, EventArgs e) { if (dd.Visible) cb.Focus(); }
        void _toolStripDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e) { if (lb.SelectedIndex != -1)  SEL = (item)lb.SelectedItem; }
        void lb_Click(object sender, EventArgs e) { dd.Close(); }
       // public int DropHeight { get { return lb.Height; } set { lb.Height = Math.Max(50, value); } }
       // public int MyWidth { get { return lb.Width; } set { lb.Width = Math.Max(Width, value); } }
        public string SelectedText { get { return SEL.V; } set { } }
        public string SelectedValue { get { return SEL.K; } set { setSelectedValue(value); } }
        //public void Populate_Drop() { }
        private void cb_MouseDown(object sender, MouseEventArgs e) { B1_Click(null, null); }
        private void B1_Click(object sender, EventArgs e)
        {
            lb.Items.Clear();
            if (dt != null) foreach (DataRow r in dt.Rows) { lb.Items.Add(new item(r)); if (r[0] + "" == SEL.K) lb.SelectedIndex = lb.Items.Count - 1; }
            lb.Height = Math.Min(170, lb.Items.Count * lb.ItemHeight + 5);
            dd.Show(this.PointToScreen(new Point(0, this.Height)));
            tb.Text = "";
            tb.Focus();
        }
    }
    class item
    {
        public string K, V;
        public static item Empty = new item("", "");
        public item(string k, string v) { K = k; V = v; }
        public item(DataRow r) { K = r[0] + ""; V = r[1] + ""; }
        public override string ToString() { return V; }
    }
}
