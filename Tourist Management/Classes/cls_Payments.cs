using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace Tourist_Management.Classes
{
    class cls_Payments
    {
        private const string msghd = "Payment List";
        public decimal Hotel_Payment(string VoucherID)
        {
            try
            {
                #region pay
                #endregion
                return 0;
            }
            catch (Exception ex)
            {
                db.MsgERR(ex);
                return 0;
            }
        }
    }
}
