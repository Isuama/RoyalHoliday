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
    public partial class frmFormatPrint : Form
    {
        public Tourist_Management.User_Controls.ucReportViewer RPViewer;
        FontDialog FD = new FontDialog();
        ColorDialog CD = new ColorDialog();
        public frmFormatPrint(){InitializeComponent();}
        private void frmFormatPrint_Load(object sender, EventArgs e)
        {
            textBox1.ForeColor=RPViewer.COLOR01;
            textBox2.ForeColor = RPViewer.COLOR02 ;
            textBox4.ForeColor = RPViewer.COLOR03;
            textBox1.Font = RPViewer.Font01;
            textBox2.Font = RPViewer.Font02;
            textBox4.Font = RPViewer.Font03;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result=CD.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.ForeColor = CD.Color;
                textBox6.ForeColor = CD.Color;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = CD.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.ForeColor = CD.Color;
                textBox5.ForeColor = CD.Color;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = CD.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox4.ForeColor = CD.Color;
                textBox3.ForeColor = CD.Color;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = FD.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Font = FD.Font;
                textBox6.Font = FD.Font;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = FD.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Font = FD.Font;
                textBox5.Font = FD.Font;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = FD.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox4.Font = FD.Font;
                textBox3.Font = FD.Font;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            RPViewer.COLOR01 = textBox1.ForeColor;
            RPViewer.COLOR02 = textBox2.ForeColor;
            RPViewer.COLOR03 = textBox4.ForeColor;
            RPViewer.Font01 = textBox1.Font;
            RPViewer.Font02 = textBox2.Font;
            RPViewer.Font03 = textBox4.Font;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCan_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
