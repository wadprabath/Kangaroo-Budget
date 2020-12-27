using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication3.Properties;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace WindowsFormsApplication3
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        //    foreach (Control control in crystalReportViewer1.Controls) {
        //        if (control is System.Windows.Forms.ToolStrip) {

        //            Default Print Button
        //            ToolStripItem tsItem = ((ToolStrip)control).Items[1];
        //            tsItem.Click += new EventHandler(tsItem_Click);
                    
        //            Custom Button
        //            ToolStripItem tsNewItem = ((ToolStrip)control).Items.Add("");
        //            tsNewItem.ToolTipText = "Custom Print Button";
        //            tsNewItem.Image = Resources.;
        //            tsNewItem.Tag = "99";
        //            ((ToolStrip)control).Items.Insert(0, tsNewItem);
        //            tsNewItem.Click += new EventHandler(tsNewItem_Click);
        //        }
        //    }
        }





        //public delegate void CustomPrintDelegate();

        //public Delegate CustomPrintMethod { get; set; }



        //void tsNewItem_Click(object sender, EventArgs e)
        //{
        //    if (CustomPrintMethod != null)
        //    {
        //        CustomPrintMethod.DynamicInvoke(null);
        //    }
        //}

        //void tsItem_Click(object sender, EventArgs e)
        //{
        //    if (CustomPrintMethod != null)
        //    {
        //        CustomPrintMethod.DynamicInvoke(null);
        //    }
        //}

        //private void CustomReportViewer_Load(object sender, EventArgs e)
        //{
        //    CrystalReport2 report = new CrystalReport2();
        //    crystalReportViewer1.ReportSource = report;
        //    crystalReportViewer1.Refresh();
        //}
    }
}




       

     
       
   
