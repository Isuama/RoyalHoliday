using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Windows.Forms ;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Reflection;
namespace Tourist_Management.Reports
{
    public partial class frmMasterReports : Form
    { 
        public frmMasterReports()
        {
            InitializeComponent();
            this.Controls.Add(CRV);
            CRV.SetBounds(0, 0, 100, 100);
            CRV.Visible = true;
            CRV.Dock = DockStyle.Fill;
            CRV.BringToFront();
            cbReports.Items.Clear();
        }
        private void frmMasterReports_Load(object sender, EventArgs e)
        {
            if (cbReports.Items.Count == 0)
            {
                add(new clrCashCollection(new MasterReports.crCashCollection()));
                add(new clrCashCollection(new MasterReports.crTourAging()));
                add(new clrCashCollection(new MasterReports.crPNLSummary()));
                add(new clrCashCollection(new MasterReports.crIOUSettlement()));
                add(new clrSettlement(new MasterReports.crReceiptSettlement(), "{Command.Type}='REC'", "Receipt Set-Off Report"));
                add(new clrSettlement(new MasterReports.crReceiptSettlement(), "({Command.Type}='CPY' OR {Command.Type}='CHQ')", "Voucher Set-Off Report"));
                add(new clrCashCollection(new MasterReports.crCashPayables()));
                add(new clrGeneralLedger(new MasterReports.crGeneralLedger()));
                add(new clrGeneralLedger(new MasterReports.crLegerTotals()));
                add(new clrGeneralLedger(new MasterReports.crTrialBalance()));
            }
            if (CRV.ReportSource != null)
            {
                ReportDocument cr = (ReportDocument)CRV.ReportSource;
                if (Classes.clsGlobal.Con != null)
                    cr.DataSourceConnections[0].SetConnection(Classes.clsGlobal.Con.SERVER, Classes.clsGlobal.Con.DATABASE, Classes.clsGlobal.Con.USERID, Classes.clsGlobal.Con.PASSWORD);
                else
                    cr.DataSourceConnections[0].SetConnection(".", "TouristManagement", "sa", "saadmin");
            }
        }
        public clrReport add(clrReport c) { cbReports.Items.Add(c); return c; }
        public class clrSettlement : clrReport
        {
            public clrSettlement(ReportDocument report, string filter,string title="")
                : base(report)
            {
                if (filter != "") report.RecordSelectionFormula += (report.RecordSelectionFormula != "" ? " and " : " ") + filter;
                if (title != "") report.SummaryInfo.ReportTitle = title;
            }  
        }
        public class clrCashCollection : clrReport
        {
            public clrCashCollection(ReportDocument report) : base(report) { }  
        }
        public class clrGeneralLedger: clrReport
        {
            public clrGeneralLedger(ReportDocument report) : base(report) { } 
        }
        public class clrReport
        {
            protected ReportDocument Report; public string GroupByFields,ReportTitle;
            public clrReport(ReportDocument report) : this() { Report = report; GroupByFields = ""; }
            public clrReport() { From = System.Diagnostics.Debugger.IsAttached ? DateTime.Parse("01 JAN 2010") : DateTime.Parse("01 APR " +( DateTime.Now.Date.Year-1)); Upto = DateTime.Now.Date; }
            public virtual  ReportDocument GetReport(){ return Report; } 
            public ReportDocument getFinalReport() {
                ReportDocument cr = GetReport();
                foreach (ParameterField p in cr.ParameterFields) switch (p.ParameterValueType)
                    {
                        case ParameterValueKind.BooleanParameter: cr.SetParameterValue(p.Name, false); break;
                        case ParameterValueKind.CurrencyParameter: case ParameterValueKind.NumberParameter: cr.SetParameterValue(p.Name, 0.00); break;
                        case ParameterValueKind.DateParameter: case ParameterValueKind.DateTimeParameter: case ParameterValueKind.TimeParameter: cr.SetParameterValue(p.Name, DateTime.Now); break;
                        default: cr.SetParameterValue(p.Name, ""); break;
                    }
                foreach (System.Reflection.PropertyInfo p in this.GetType().GetProperties()) 
                    if (cr.ParameterFields.Find(p.Name, null) != null) 
                    { 
                        object o = p.GetValue(this, new object[] { });
                        if (o != null) cr.SetParameterValue(p.Name, o is KeyValue ? (p.Name == "FilterBy" || p.Name == "GroupBy" ? (object)((KeyValue)o).Name : (object)((KeyValue)o).ID) : o);
                    } 
                if(Classes.clsGlobal.Con !=null) 
                    cr.DataSourceConnections[0].SetConnection(Classes.clsGlobal.Con.SERVER, Classes.clsGlobal.Con.DATABASE , Classes.clsGlobal.Con.USERID, Classes.clsGlobal.Con.PASSWORD);
                else
                    cr.DataSourceConnections[0].SetConnection(".", "TouristManagement", "sa", "saadmin");
                return cr;
            }
            public KeyValue filter, gb, ob; 
            public static KeyValue company;
            public static DateTime from,upto;
            KeyValue getPara(string p,KeyValue vValue)
            {
                KeyValue x = null;
                if (GetReport().ParameterFields[p] != null) x = new KeyValue(0, (((CrystalDecisions.Shared.ParameterDiscreteValue)GetReport().ParameterFields[p].DefaultValues[0])).Value.ToString());
                return vValue == null ? x : vValue;
            }
            [CategoryAttribute("Period"), Editor(typeof(DateFilter), typeof(UITypeEditor))]
            public KeyValue FilterBy { get { return getPara("FilterBy", filter); } set { filter = value; } }
            [CategoryAttribute("Report View"), Editor(typeof(GroupFilter), typeof(UITypeEditor))]
            public KeyValue GroupBy { get { return getPara("GroupBy", gb); } set { gb = value; } }
            [CategoryAttribute("Period")]  public DateTime From { get { return from; } set { from = value; } }
            [CategoryAttribute("Period")]   public DateTime Upto { get { return upto; } set { upto = value; } }
            [CategoryAttribute("Company View")]
            [Editor(typeof(CompanyEditor), typeof(UITypeEditor))]
            public KeyValue Company { get { return company; } set { company = value; } }
            [CategoryAttribute("Report View"),Editor(typeof(GroupByEditor), typeof(UITypeEditor))]
            public KeyValue OrderBy { get { return ob; } set { ob = value; } }
            public override string ToString() { string n = GetReport().SummaryInfo.ReportTitle; return n == null ? GetReport().Name : n; }
        }
        public class CompanyEditor : SQLEditor { protected override string GetSQL() { return "SELECT 0,'compny';"; } }
        public class MarketingEditor : SQLEditor { protected override string GetSQL() { return "SELECT 0,'Marketing';"; } }
        public class GroupByEditor : SQLEditor { public static string Fields = ""; protected override string GetSQL() { return Fields; } }
        public class DateFilter : SQLEditor { protected override string GetSQL() { return "FilterBy"; } }
        public class GroupFilter : SQLEditor { protected override string GetSQL() { return "GroupBy"; } }
        public class SQLEditor : UITypeEditor
        {
            protected virtual string GetSQL() { return "SELECT 0,'none';"; }
            private IWindowsFormsEditorService _editorService;
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }
            private void OnListBoxSelectedValueChanged(object sender, EventArgs e) { _editorService.CloseDropDown(); }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                ListBox lb = new ListBox();
                lb.SelectionMode = System.Windows.Forms.SelectionMode.One;
                lb.SelectedValueChanged += OnListBoxSelectedValueChanged;
                lb.DisplayMember = "Name";
                string sql= GetSQL(),key=sql;
                if (sql!=null && sql.StartsWith("SELECT"))
                {
                    foreach (KeyValue KV in (new KeyValue[] { new KeyValue(0, "ALL"), new KeyValue(1002, "RHS"), new KeyValue(1006, "INB"), new KeyValue(1007, "ADV") }))
                    {
                        int index = lb.Items.Add(KV);
                        if (KV.Equals(value)) lb.SelectedIndex = index;
                    }
                 } 
                else
                {
                    ReportDocument cr = ((clrReport)context.Instance).GetReport();
                    if (cr.ParameterFields.Find(key, null) != null)
                    {
                        sql = "";
                        foreach (ParameterValue v in cr.ParameterFields[key].DefaultValues)
                            sql +=    ((CrystalDecisions.Shared.ParameterDiscreteValue)v).Value+",";
                        if (key == "GroupBy") sql += "'(none)'"; else sql = sql.Substring(0, sql.Length - 1);
                    }
                    string[] ar = sql.Split(new char[] { ',' });
                    for (int i = 0; i < ar.Length ; i++)
                    {
                        KeyValue KV = new KeyValue(i, ar[i]);
                        int index = lb.Items.Add(KV);
                        if (KV.Equals(value)) lb.SelectedIndex = index; 
                    }
                }
                _editorService.DropDownControl(lb);
                if (lb.SelectedItem == null) return value;
                return (KeyValue)lb.SelectedItem;
            }
        }
        public class KeyValue
        {
            public int ID; public String Name;
            public KeyValue(int id, string name) { ID = id; Name = name; }
            public override string ToString() { return Name; }
        }
        public CrystalReportViewer CRV = new CrystalReportViewer();
        public ReportDocument CR ; 
       private void cbReports_SelectedIndexChanged(object sender, EventArgs e)
       {
           if (cbReports.SelectedIndex == -1) return;
           clrReport cro = (clrReport)cbReports.SelectedItem;
           GroupByEditor.Fields = cro.GroupByFields;
           pg.SelectedObject = cbReports.SelectedItem;
       }
       public void button1_Click(object sender, EventArgs e)
        {
            if (cbReports.SelectedIndex == -1) return; 
            clrReport cro = (clrReport)pg.SelectedObject; 
            CRV.ReportSource = cro.getFinalReport();           
        }
    }
}
