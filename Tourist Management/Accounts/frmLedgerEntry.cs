using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Accounts
{
    public partial class frmLedgerEntry : Form
    {
        private const string msghd = "Ledger Entry";
        public int Mode = 0; //TO GET TO KNOW WEATHER INSERTION OR UPDATION
        public int SystemCode = 0; // TO KEEP THE SYSTEM GENERATED CODE
        public string SqlQry = "SELECT ID,Code,Name as DriverName,CompanyName,IdentityNo,IsNull(IsActive,0)AS IsActive From vwDriverVsEmployee Where Isnull([Status],0)<>7 Order By Code";
        private void frmLedgerEntry_Load(object sender, EventArgs e)
        {
        }
    }
}
