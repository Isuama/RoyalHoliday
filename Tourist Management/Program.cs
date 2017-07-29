using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;
namespace Tourist_Management
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.Run(new Main.frmConnect());
            Application.Run(new Main.frmUserLog());
            if(Classes.clsGlobal.UserID!=0)
            Application.Run(new Tourist_Management.Main.frmMDIMain());
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
        }
    }
}
