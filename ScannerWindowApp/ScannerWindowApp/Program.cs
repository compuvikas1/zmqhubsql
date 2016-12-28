using System;
using System.Windows.Forms;


namespace ScannerDashBoard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ScannerBox());
            Application.Run(new ScannerDashboard());
        }
    }
}
