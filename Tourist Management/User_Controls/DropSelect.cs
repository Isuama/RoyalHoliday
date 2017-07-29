using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
namespace Tourist_Management.DropDowns
{
    public partial class DropSelect : UserControl
    {
        DataTable dt;
        CheckedListBox lb = new CheckedListBox() { BorderStyle = BorderStyle.None, CheckOnClick = true };
        TextBox tb = new TextBox();
        Button c1 = new Button { Text = "All" }, c2 = new Button { Text = "Invert" }, c3 = new Button { Text = "None" };
        ToolStripDropDown dd = new ToolStripDropDown { CanOverflow = true, AutoClose = true, DropShadowEnabled = true };
        List<string> _SelectedList = new List<string>();
        public DataTable DataSource { get { return dt; } set { dt = value; /*string k = _SEL.K; _SEL = item.Empty; cb.Text = ""; setSelectedValue(k); */ } }
        public string[] SelectedList { get { refresh(); return _SelectedList.ToArray(); } set { _SelectedList = value == null ? new List<string>() : new List<string>(value); refresh(); } }
        public void refresh()
        {
            _SelectedList.Remove("");
            _SelectedList.Remove(null);
            if (_SelectedList.Count == 0) cb.Text = "<Not Selected>";
            else if (dt == null || _SelectedList.Count != dt.Rows.Count) cb.Text = "<" + _SelectedList.Count + " Selected>";
            else cb.Text = "<All Selected>";
        }
        public DropSelect()
        {
            InitializeComponent();
            FlowLayoutPanel flp = new FlowLayoutPanel() { Width = this.Width };
            flp.Controls.AddRange(new Control[] { c1, c2, c3 });
            foreach (Control c in new Control[] { c1, c2, c3 }) c.Click += new EventHandler(C_Click);
            dd.Items.Add(new ToolStripControlHost(tb) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false, Size = new Size(this.Size.Width, 20) });
            dd.Items.Add(new ToolStripControlHost(lb) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false, Size = new Size(this.Size.Width, this.Size.Height + 20) });
            dd.Items.Add(new ToolStripControlHost(flp) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false, Size = new Size(this.Size.Width, this.Size.Height + 20) });
            dd.Closed += new ToolStripDropDownClosedEventHandler(_toolStripDropDown_Closed);
            dd.VisibleChanged += new EventHandler(_toolStripDropDown_VisibleChanged);
            tb.TextChanged += new EventHandler(tb_TextChanged);
            tb.KeyUp += new KeyEventHandler(tb_KeyUp);
            lb.ItemCheck += new ItemCheckEventHandler(lb_ItemCheck);
        }
        void lb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string k = ((item)lb.Items[e.Index]).K;
            if ((e.NewValue == CheckState.Checked) && (!_SelectedList.Contains(k))) _SelectedList.Add(k);
            if ((e.NewValue != CheckState.Checked) && (_SelectedList.Contains(k))) _SelectedList.Remove(k);
            refresh();
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
            if (dt != null) foreach (DataRow r in dt.Rows) if ((" " + r[1]).ToLower().Contains(" " + tb.Text.ToLower())) { lb.Items.Add(new item(r), _SelectedList.Contains<string>(r[0] + "")); }
        }
        void _toolStripDropDown_VisibleChanged(object sender, EventArgs e) { if (dd.Visible) cb.Focus(); }
        void _toolStripDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e) { refresh(); }
        void lb_Click(object sender, EventArgs e) { dd.Close(); }
        private void B1_Click(object sender, EventArgs e)
        {
            lb.Items.Clear();
            if (dt != null) foreach (DataRow r in dt.Rows) lb.Items.Add(new item(r), _SelectedList.Contains<string>(r[0] + ""));
            lb.Height = Math.Min(170, lb.Items.Count * lb.ItemHeight + 5);
            dd.Show(this.PointToScreen(new Point(0, this.Height)));
            tb.Text = "";
            tb.Focus();
        }
        private void C_Click(object sender, EventArgs e)
        {
            if (sender == c2) for (int i = 0; i < lb.Items.Count; i++) lb.SetItemChecked(i, !lb.GetItemChecked(i));
            else for (int i = 0; i < lb.Items.Count; i++) lb.SetItemChecked(i, sender == c1);
        }
        private void DropSelect_Resize(object sender, EventArgs e)
        {
            Height = cb.Height;
            cb.Width = Width - 29;
            B1.SetBounds(Width - 25, 0, 21, Height);
        }
        public string SetList { get { return string.Join(",", SelectedList); } set { SelectedList = value.Split(",".ToCharArray()); } }
        public int DropHeight { get { return lb.Height; } set { lb.Height = value; } }
        public int MyWidth { get { return lb.Height; } set { lb.Height = Math.Max(Width, value); } }
    }
}