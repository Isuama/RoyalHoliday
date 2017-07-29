using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
namespace Tourist_Management.User_Controls
{
    public partial class ucTransEmail : UserControl
    {
        private const string msghd = "Sending an Email";
        string FPath = "";
        string FName = "";
        string Subj = "";
        string cdet = "";
        string VoucherNumber = "";
        public ucTransEmail(){InitializeComponent();}
        public string VoucherNo
        {
            get
            {
                return VoucherNumber;
            }
            set
            {
                VoucherNumber = value;
            }
        }
        public string FilePath
        {
            get
            {
                return FPath;
            }
            set
            {
                FPath = value;
            }
        }
        public string FileName
        {
            get
            {
                return FName;
            }
            set
            {
                FName = value;
            }
        }
        public string Subject
        {
            get
            {
                return Subj;
            }
            set
            {
                Subj = value;
            }
        }
        public string CC
        {
            get
            {
                return cdet;
            }
            set
            {
                cdet = value;
            }
        }
        private void btnEmail_Click(object sender, EventArgs e) {  Send_Email(); }
        private void Send_Email()
        {
                if (Validate_Email_Options() == false)
                    return;
                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.Display(false);
                string Signature="";
                Signature = ReadSignature();
                oMsg.HTMLBody = rtbBody.Text + Signature;
                oMsg.CC = txtCC.Text.Trim();
                String sDisplayName = "MyAttachment";
                int iPosition;
                if (rtbBody.Text.ToString().Trim() != "")
                    iPosition = (int)oMsg.Body.Length + 1;
                else
                    iPosition = 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                Outlook.Attachment oAttach;
                if (chkExSum.Checked)
                {
                    ReportDocument oReport = new ReportDocument();
                    int HotelNo=0;
                    string input = VoucherNumber.ToString().Trim();
                    string[] numbers = System.Text.RegularExpressions.Regex.Split(input, @"\D+");
                    HotelNo = Convert.ToInt32(numbers[1]);
                    string path = Classes.clsGlobal.VoucherPath[HotelNo-1].ToString();
                    string lFileName = path;
                    oAttach = oMsg.Attachments.Add(@path, iAttachType, iPosition, sDisplayName);
                }
                oMsg.Subject = txtSubject.Text;
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                if (txtTo.Text.ToString().Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(txtTo.Text.ToString().Trim());
                    oRecip.Resolve();
                    oRecip = null;
                }
                oRecips = null;
                oMsg = null;
                oApp = null;
        }
        private string ReadSignature()
        {
                string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
                string signature = string.Empty;
                DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
                if (diInfo.Exists)
                {
                    FileInfo[] fisignature = diInfo.GetFiles("*.htm");
                    if (fisignature.Length > 0)
                    {
                        StreamReader sr = new StreamReader(fisignature[0].FullName, Encoding.Default);
                        signature = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(signature))
                        {
                            string filename = fisignature[0].Name.Replace(fisignature[0].Extension, string.Empty);
                            signature = signature.Replace(filename+"_files/",appDataDir+"/"+filename+"_files/");
                        }
                    }
                }
                return signature;
        }
        private Boolean Validate_Email_Options() {  return true;}
        private void btnEmailConts_Click(object sender, EventArgs e)
        {
            Email.frmFilterEmail feml = new Tourist_Management.Email.frmFilterEmail();
            feml.ShowDialog();
            txtTo.Text = "";
            txtTo.Text = feml.SelectedContacts;
        }
        private void ucTransEmail_Load(object sender, EventArgs e){  }
    }
}
