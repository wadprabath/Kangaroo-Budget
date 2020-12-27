using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication3
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
            //Application.Run(new  ReceiptNew());

            Application.Run(new Form4());

            //Application.Run(new Report());
            //Application.Run(new Recipt());
            //Application.Run(new Form12());
            // Application.Run(new Form10());
           // Application.Run(new Form14());
          // Application.Run(new Form23());
        }
    }
}
