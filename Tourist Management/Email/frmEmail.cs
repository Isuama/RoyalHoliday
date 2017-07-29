using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
namespace Tourist_Management.Email
{
    public partial class frmEmail : Form
    {
        private const string msghd = "Sending an Email";
        public int Mode = 0; //TO GET TO KNOW WHETHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public frmEmail(){InitializeComponent();}
        private void btnCancel_Click(object sender, EventArgs e){this.Close();}
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validate_Data() == false)
                    return;
                if(txtSubject.Text.Trim()=="")
                {
                     if(MessageBox.Show("Send Message Without A Subject", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        return;
                }
                if (rtbBody.Text.Trim() == "")
                {
                    if (MessageBox.Show("Send Message Without A Body", msghd, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                        return;
                }
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(txtFrom.Text.Trim());
                mail.To.Add(txtTo.Text.Trim());
                mail.Subject = txtSubject.Text.Trim();
                mail.Body = rtbBody.Text;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("isuama.amarathunga@gmail.com", "kishupatti");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private Boolean Validate_Data()
        {
                if (txtFrom.Text.Trim() == "")
                {
                    MessageBox.Show("From Address Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (txtTo.Text.Trim() == "")
                {
                    MessageBox.Show("To Address Cannot Be Blank", msghd, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;
        }
    }
}
