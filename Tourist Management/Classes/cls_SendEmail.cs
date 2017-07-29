using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Tourist_Management.Classes
{
    class cls_SendEmail
    {
        const string msghd = "Sending Mail";
        #region CHECK FOR NON CONFIRMED BOOKINGS
        public void Non_Confirmed_Bookings()
        {
            try
            {
            }
            catch (Exception ex){db.MsgERR(ex);}
        }
        #endregion
    }
}
