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
    public partial class Form23 : Form
    {
        Ventura ven;
        bool bblink = true;
        NewReceiptNumber nrecn;
        public Form23()
        {
            InitializeComponent();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                ven=new Ventura();
                ven.getVenturaCabDetails(textCabNo.Text, textPlateNo, textOwner);
                textLastDate.Text = ven.getLastVenturaPaidDate(textCabNo.Text);
                ven.viewAreas(textCabNo.Text,dataGridView2,textTotAreas);
            }
        }

        private void textCabNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                textAmount.Focus();
            }
        }

        private void textDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
            //    ven = new Ventura();
            //    ven.fillGridForPayment(textCabNo.Text, dataGridView1, textDays);
            //}
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ven = new Ventura();
            ven.saveVenturaPayment(textCabNo,textPlateNo,textOwner,textPayment,textDays,dataGridView1,textRecNo);
        }

        private void textAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) 
            {
                ven = new Ventura();
                //ven.calTotalDays(textAmount,textPayment,textBalance,textDays,textCabNo,dataGridView1);
                ven.datagridFillForPayment(textCabNo.Text, dataGridView1, textAmount,textPayment,textBalance,textDays);
            }
        }

        private void textAmount_TextChanged(object sender, EventArgs e)
        {
            if (textAmount.TextLength <2) 
            {
                textBalance.Clear(); textPayment.Clear(); textDays.Clear(); dataGridView1.Rows.Clear();
            }
            if (textAmount.TextLength == 4)
            {
                ven = new Ventura();
                //ven.calTotalDays(textAmount, textPayment, textBalance, textDays, textCabNo, dataGridView1);
            }
        }

        private void textCabNo_TextChanged(object sender, EventArgs e)
        {
            if (textCabNo.TextLength < 1) 
            {
                textPlateNo.Clear(); textOwner.Clear(); textLastDate.Clear(); textAmount.Clear();
                textBalance.Clear(); textPayment.Clear(); textDays.Clear(); dataGridView1.Rows.Clear();
            }
           if (textCabNo.TextLength == 4) 
            {
                ven = new Ventura();
                ven.getVenturaCabDetails(textCabNo.Text, textPlateNo, textOwner);
                textLastDate.Text = ven.getLastVenturaPaidDate(textCabNo.Text);
                ven.viewAreas(textCabNo.Text, dataGridView2, textTotAreas);
            }
        }

        

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ven = new Ventura();
            ven.clearAll(this);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Form24 f24 = new Form24();
            f24.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (textBalance.Text != "")
            {
                if (bblink)
                {
                    textBalance.BackColor = Color.White;
                    textBalance.ForeColor = Color.Red;
                }
                else
                {
                    textBalance.BackColor = Color.Red;
                    textBalance.ForeColor = Color.White;
                }
                bblink = !bblink;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            FrmFindVentura frmven = new FrmFindVentura();
            frmven.Show();
        }
    }
}
