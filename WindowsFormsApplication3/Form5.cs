using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form5 : Form
    {
        User us;
        public Form5()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReceiptNew rcn = new ReceiptNew();
            rcn.MdiParent = this;
            rcn.Top = 0;
            rcn.Left = 0;

            rcn.Show();
            pictureBox1.SendToBack();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Form6 f6 = new Form6();
            //f6.MdiParent = this;
            //f6.Top = 0;
            //f6.Left = 0;
            //f6.Show();
            //pictureBox1.SendToBack();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            FrmReceiptCancel frmrptcncl = new FrmReceiptCancel();
            frmrptcncl.MdiParent = this;
            frmrptcncl.Top = 0;
            frmrptcncl.Left = 0;
            frmrptcncl.Show();
            //Form7 f7 = new Form7();
            //f7.MdiParent = this;
            //f7.Top = 0;
            //f7.Left = 0;
            //f7.Show();
            pictureBox1.SendToBack();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.MdiParent = this;
            f9.Show();
            pictureBox1.SendToBack();
        }

        private void systemLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            us=new User();
            Log l = new Log();             
            l.MdiParent = this;
            l.Show();
            pictureBox1.SendToBack();
            us.SystemLogDisplay(l.richTextBox1);
        }       

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            us=new User();
            us.SystemLog(DateTime.Now,us.getCurrentUser(),"Log Out");
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Form17 f17 = new Form17();
            f17.Show();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            FrmCabList frmcab = new FrmCabList();
            frmcab.Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Form23 f23 = new Form23();
            f23.Show();
        }

        private void normalCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
        }

        private void brandedCarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form22 f22 = new Form22();
            f22.Show();
        }

        private void toolStripButton2_ButtonClick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        { 

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            us = new User();
            label2.Text = us.getCurrentUser();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timeRangeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRportTime rt = new FrmRportTime();
            rt.MdiParent = this;
            rt.Top = 0;
            rt.Left = 0;
            rt.Show();
            pictureBox1.SendToBack();
           

        }

        private void normalReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report r = new Report();
            r.MdiParent = this;
            r.Top = 0;
            r.Left = 0;
            r.Show();
            pictureBox1.SendToBack();

        }

        private void locationReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocationRpt flr = new FrmLocationRpt();
            flr.Visible = true;
        }

    }
}
