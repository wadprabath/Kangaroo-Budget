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

    public partial class Form17 : Form
    {
        NewReceiptNumber rcpt;
        Taxi t1;
        public Form17()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rcpt = new NewReceiptNumber();
            DialogResult dr=  MessageBox.Show("Are You Sure Want to Save ?","Confirm",MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string recno = rcpt.generateOtherReceiptNo();
                textBox12.Text= t1.saveOtherPayment(recno, textBox1,textBox11,textBox13,textBox7,dateTimePicker1,textBox5,textBox6, textBox2, textBox3,textBox4,textBox8,textBox14,textLdeposit,textBox15,textBox16,textBox10,textBox9,groupBox1,textBox11,textBox17);
            }
        }

        private void Form17_Load(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
           
            t1 =new Taxi();
            t1.displayOtherPayment(label13,label14,label15,label17);
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                textBox1.Focus();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox11.Focus();
            }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox4.Focus();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox8.Focus();
            }

        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyValue == 13)
            {
                textBox9.Focus();
            }
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox2.Focus();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form18 f18 = new Form18();
            f18.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            t1 = new Taxi();
            t1.clearOtherPayment(dateTimePicker1, textBox1, textBox2, textBox3, textBox4, textBox5, textBox6,textBox9,textBox10,textBox13,textBox11,textBox7,textBox12,textBox8,textBox14,textLdeposit,textBox15,textBox16,textBox17);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox13.Focus();
            }
        }

        

        //private void textBox8_KeyDown(object sender, KeyEventArgs e)
        //{
        //    //if (e.KeyValue == 13)
        //    //{
        //    //    textBox6.Focus();
        //    //}
        //}

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox5.Focus();
            }
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button1.PerformClick();
            }
        }

        

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox7.Focus();
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox17.Focus();
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6,textLdeposit,textBox17);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            FrmRefund fr = new FrmRefund();
            fr.Show();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox14.Focus();
            }
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textLdeposit.Focus();
            }
        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox16.Focus();
            }

        }

        private void textBox16_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox10.Focus();
            }

        }

        private void textLdeposit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox15.Focus();
            }
        }

        private void textLdeposit_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit,textBox17);
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.calTotOtherPayment(textBox5, textBox2, textBox3, textBox4, textBox8, textBox14, textBox15, textBox16, textBox10, textBox6, textLdeposit, textBox17);
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox6.Focus();
            }
        }

       

       
    }
}
