using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Tourist_Management.Accounts
{
    public partial class frmPaymentReceive2 : Form
    {
        public int Mode = 0;
        public double SystemCode = 0;
        public string SqlQry = "SELECT ID,Code,Name as DriverName,CompanyName,IdentityNo,IsNull(IsActive,0)AS IsActive From vwDriverVsEmployee Where Isnull([Status],0)<>7 Order By Code";
        public DataRow drDoc, drTour, drAgent;
        List<string> Errors = new List<string>();
       new void  Refresh()
        {
            Errors.Clear();
            if (drDoc != null && drTour != null && (tReceipt.Text != drDoc["VoucherID"] + "" || tTour.Text != drTour["TID"] + ""))
                tAmount.Text = Math.Min(db.Val(drDoc["Amount"]), db.Val(drTour["Balance"])).ToString();
            tAgent.Text = drAgent == null ? "" : drAgent["Name"] + "";
            tTour.Text = drTour == null ? "" : drTour["TID"] + "";
            tReceipt.Text = drDoc == null ? "" : drDoc["VoucherID"] + "";
            if (drDoc == null) Errors.Add("Select a Set off By Document.");
            else
            {
                lAmount.Text = db.N2(drDoc["Amount"]);
                lRate.Text = db.N2(drDoc["Rate"]);
                lRef.Text = drDoc["RefNo"] + "";
                lCur.Text = drDoc["Currency"] + "";
                lDate.Text = drDoc["Received Date"] + "";
                lFrom.Text = drDoc["PayableTo"] + "";
                if (dgvc.Rows.Count != 0) tRate.Enabled = false;
                else if (drTour != null && drDoc != null )
                {
                    if (!(tRate.Enabled = (drDoc["CurID"] + "" != drTour["CurID"] + "")))  tRate.Text = "1.00";
                    lblRate.Text = drDoc["Currency"] + " to " + drTour["Currency"] + " Rate"; 
                }
                else { tRate.Enabled = false;   lblRate.Text = "0.00"; } 
                double cTot = 0, oTot = 0,goTot=0,gnTot=0;
                dgvo.Rows.Clear();
                DataTable DT = Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(                    
                    drDoc["Type"]+""=="REC"
                    ?"SELECT TID,Agent,Tour,InvoiceNo,Currency,dbo.R(INVLKR/INV),INV,c.Paid,Balance,s.PaidT,s.PaidR,s.PaidR*" + double.Parse(drDoc["Rate"] + "") + "-s.PaidT*dbo.R(INVLKR/INV) Gain FROM vCashCollection c RIGHT JOIN (SELECT TransID, Sum(SPaid) PaidT,sum(RPaid) PaidR FROM vReceiptSetoff WHERE  ReceiptNo='" + tReceipt.Text + "' GROUP BY TransID) s ON s.TransID=c.TID"
                    : "SELECT ID,PayableTo,Tour,RefNo,Currency,dbo.R(AMTLKR/AMT),AMT,c.Paid,Balance,s.PaidT,s.PaidR,s.PaidR*" + double.Parse(drDoc["Rate"] + "") + "-s.PaidT*dbo.R(AMTLKR/AMT) Gain FROM vCashPayables c RIGHT JOIN (SELECT SetOnID, Sum(SPaid) PaidT,sum(RPaid) PaidR FROM vReceiptSetoff WHERE  ReceiptNo='" + tReceipt.Text + "' GROUP BY SetOnID) s ON s.SetOnID=c.ID");
                foreach (DataRow r in DT.Rows) { dgvo.Rows.Add(r.ItemArray); oTot += db.Val(r["PaidR"]); goTot += db.Val(r["Gain"]); }
                List<string> Codes = new List<string>();
                foreach (DataGridViewRow r in dgvc.Rows)
                {
                    r.Cells[chRSetoff2.Index].Value = db.Val(tRate.Text) * db.Val(r.Cells[chSetOff.Index].Value + "") / db.Val(drDoc["Rate"]);
                    r.Cells[cGain.Index].Value = db.Val(drDoc["Rate"]+"") * db.Val(r.Cells[chRSetoff2.Index].Value + "") - db.Val(r.Cells[chRate.Index].Value + "") * db.Val(r.Cells[chSetOff.Index].Value + "");
                    gnTot +=  db.Val(r.Cells[cGain.Index].Value);
                    cTot += db.Val(r.Cells[chRSetoff2.Index].Value) ;
                    if (db.Val(r.Cells[chRSetoff2.Index].Value) == 0 || db.Val(r.Cells[chSetOff.Index].Value) == 0) Errors.Add(r.Cells[chTour.Index].Value + " Setoff and Receipt amounts must not be zero.");
                    if (db.Val(r.Cells[chSetOff.Index].Value) > db.Val(r.Cells[chBalance.Index].Value)) Errors.Add(r.Cells[chTour.Index].Value + " Setoff amount Exceed Job Balance.");
                    if (Codes.Contains(r.Cells[chTour.Index].Value + "")) Errors.Add(r.Cells[chTour.Index].Value + " Tour Duplicated."); else Codes.Add(r.Cells[chTour.Index].Value + "");
                    if (!db.IsRate(r.Cells[chUSD.Index].Value + "" == "LKR" ? 1 : 100, r.Cells[chRate.Index].Value)) Errors.Add(r.Cells[chTour.Index].Value + " Invoice Currency & Rate not Compatible Please go and edit.");
                    if (db.Int(r.Cells[chCompID.Index].Value) != db.Int(drDoc["CompID"])) Errors.Add(r.Cells[chTour.Index].Value + " has different Company than the Receipt.");
                }
                lRecLKR.Text  = db.N2(db.Val( drDoc["Amount"])*db.Val( drDoc["Rate"]));
                lOldGin.Text = db.N2(goTot);
                lNewGin.Text = db.N2(gnTot);
                lGin.Text = db.N2(goTot+gnTot);
                lNew.Text = db.N2(cTot);
                lOld.Text = db.N2(oTot);
                lBal.Text = db.N2(db.Val(drDoc["Amount"]) - oTot - cTot);
                if (dgvc.Rows.Count <= 0) Errors.Add("Add at least one reocrd Or add a Reason.");
                if (db.Val(lBal.Text) < 0) Errors.Add("Setoff amounts exceed Recipt balance Amount.");
                if (db.Val(lBal.Text) > db.Val(lBal.Text)) Errors.Add("Balance amount exceed Recipt Amount.");
                if (!db.IsRate(drDoc["CurID"], drDoc["Rate"])) Errors.Add("Receipt Currency & Rate not Compatible Please go and edit.");         
            }
        }
        private void bAdd_Click(object sender, EventArgs e)
        {
            double A = 0;
            if (drDoc == null) MessageBox.Show("Select Receipt or Voucher");
            else if (drTour == null) MessageBox.Show("Select Tour or Bil");
            else if (db.Val(tRate.Text)<=0) MessageBox.Show("Enter " + lblRate.Text + " as per "+lDate.Text );
            else if (!double.TryParse(tAmount.Text, out A) || A == 0) MessageBox.Show("Enter Amount");
            else
            {
                foreach (DataRow r in Tourist_Management.Classes.clsGlobal.objCon.Fill_Table(
                    drDoc["Type"]+""=="REC"
                    ?"SELECT TID,Agent,Tour,InvoiceNo,Currency,dbo.R(INVLKR/INV),INV,c.Paid,Balance," + A + ",0,0,CompID FROM vCashCollection c WHERE TID=" + drTour["TID"] + ""
                    : "SELECT ID,PayableTo,Tour,RefNo,Currency,dbo.R(AMTLKR/AMT),AMT,c.Paid,Balance," + A + ",0,0,CompID FROM vCashPayables c WHERE ID=" + drTour["TID"] + ""
                    ).Rows) dgvc.Rows.Add(r.ItemArray);  
                Refresh();
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            Refresh();
            if (db.ShowErrors(Errors)) return;
           db.EXECUTE("act_SetOff", "", "ReceiptNo,Remarks,Amount,Currency,Rate,CreatedBy,CreatedDate", drDoc["VoucherID"], tNote.Text, Convert.ToDouble(lNew.Text), drTour["CurID"], db.Val(tRate.Text), Convert.ToInt32(Classes.clsGlobal.UserID.ToString()), Classes.clsGlobal.CurDate());

            foreach (DataGridViewRow r in dgvc.Rows)  
                if (drDoc["Type"]+"" == "REC")  db.EXECUTE("act_SetOff_ALL", "", "SettOffID,TransID,PaidAmount", db.GetInsertID(), r.Cells[chID.Index].Value, r.Cells[chSetOff.Index].Value);
                else db.EXECUTE("act_SetOff_ALL", "", "SettOffID,SetOnID,TransID,PaidAmount", db.GetInsertID(), r.Cells[chID.Index].Value,0, r.Cells[chSetOff.Index].Value);
               
            MessageBox.Show("Record was Updated!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvc.Rows.Clear(); drTour = null; tTour.Clear(); tRate.Clear(); Refresh(); 
        }
        private void btnReceipt_Click(object sender, EventArgs e)
        {
            dgvc.Rows.Clear();
            dgvo.Rows.Clear();
            tAmount.Text = ""; drDoc = db.Record("SELECT ID,VoucherID,TourID,Guest,PayableTo,PaidDate[Received Date],RefNo,CurID,Currency,Amount,CompID,dbo.R(ReceivedRate) Rate,RTRIM(Type) Type From Fun_ReturnPaySetTot() WHERE [Type] IN ('REC','CHQ','CPY') Order By ID DESC");
            Refresh();
            if (drDoc == null) return;
            chRSetoff1.HeaderText =  chRSetoff2.HeaderText = drDoc["Type"] + "";
            lDocBy.Text = "Document (" + drDoc["Type"] + ")";

        }
        private void btnTourID_Click(object sender, EventArgs e) { 
            if(drDoc != null  )
                drTour = db.Record(drDoc["Type"]+"" =="REC"
                    ?"SELECT TID,Tour,Guest,Agent,DateArrival,DateDeparture,Marketing,Balance,Currency,CurID FROM vCashCollection WHERE INV>0"
                    : "SELECT ID TID,RefNo,PayableTo,Tour,VoucherID Bill, Balance,Currency,CurID From vCashPayables c WHERE Type='BIL' Order By ID DESC"); 
             
            Refresh();
        }
        private void bAgent_Click(object sender, EventArgs e) { drAgent = db.Record("SELECT ID,Name FROM vw_ALL_PERSONS ORDER BY Name"); Refresh(); }
        private void bRemove_Click(object sender, EventArgs e) { while (dgvc.SelectedRows.Count > 0) { dgvc.Rows.Remove(dgvc.SelectedRows[0]); } Refresh(); }
        private void btnCancel_Click(object sender, EventArgs e) { this.Close(); }
        public frmPaymentReceive2() { InitializeComponent(); }
        private void dgvc_CellEndEdit(object sender, DataGridViewCellEventArgs e) { Refresh(); }
        private void bTours_Click(object sender, EventArgs e) { if (drAgent != null) db.showMasterReport(new Tourist_Management.Reports.frmMasterReports.clrCashCollection(new MasterReports.crCashCollection()), "{Command.AgentID} = " + drAgent["ID"] + "-100000"); }
        private void bSetoffs_Click(object sender, EventArgs e) { if (drAgent != null)  db.showMasterReport(new Tourist_Management.Reports.frmMasterReports.clrCashCollection(new MasterReports.crReceiptSettlement()), "{Command.Type}='REC' AND {Command.PayableTo} = " + (db.Int(drAgent["ID"])  ) + ""); }
        private void bSetoffPayables_Click(object sender, EventArgs e) { if (drAgent != null) db.showMasterReport(new Tourist_Management.Reports.frmMasterReports.clrCashCollection(new MasterReports.crCashPayables()), "{Command.PayableToID} = " + drAgent["ID"] + ""); }
        private void bSetoffVoucher_Click(object sender, EventArgs e) { if (drAgent != null)  db.showMasterReport(new Tourist_Management.Reports.frmMasterReports.clrCashCollection(new MasterReports.crReceiptSettlement()), "({Command.Type}='CPY' OR {Command.Type}='CHQ') AND {Command.PayableTo} = " + (db.Int(drAgent["ID"])  ) + ""); }
    }
    public class frmPaymentReceive : frmPaymentReceive2 { }
}
