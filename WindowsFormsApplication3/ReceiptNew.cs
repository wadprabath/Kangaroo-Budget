using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WindowsFormsApplication3
{
    public partial class ReceiptNew : Form
    {
       
        Taxi t1;
        NewReceiptNumber nrecn;
        string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        int phoneBill = 400;
        int workingDays = 0;
        User us;

        public ReceiptNew()
        {
            InitializeComponent();
        }

        public string get_LocationName()
        {
            return ConfigurationManager.AppSettings["LocName"];
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
             panel1.Visible = false;
             nrecn = new NewReceiptNumber();
             textBox1.Text = "XXXXXXXXX"; //nrecn.getReciptNo();
             textBox27.Text = "";
             toolStripButton2.Enabled = true;
             //nrecn.getReciptNo(textBox1);
             //nrecn.decideReceiptNo(textBox1);
            //textBox1.Text= nrecn.getReciptNo();//this is for base
            //textBox1.Text = nrecn.getReciptNoYard();//this is for Yard
             nrecn.clearAll(dataGridView1, dataGridView2, dataGridView3, dateTimePicker1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, textBox8, textBox9, textBox10, textBox11, textBox12, textBox13, textBox14,textBox15,checkBox1,textBox17,textBox18,checkBox2,checkBox3,textBox24,textBox25);
             checkBox4.Checked = false; checkBox5.Checked = false;
             label43.Text = ""; checkBox6.Checked = false; checkBox7.Checked = false; label45.Visible = false; checkBox9.Checked = false; textBox24.Text = "0"; checkBox10.Checked = false; dataGridView5.Rows.Clear();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
       {
           bool result;
            if (e.KeyChar == 13)
            {
                Owner ow = new Owner();
                t1 = new Taxi();
                t1.getSpecialPaymentCab(textBox2, label45, checkBox7);
                result= t1.findBlockCabByCallCenter(textBox2);

                if (result == false)
                {
                    //int rtrn=ow.CheckTaxiAvailability(textBox2);
                    //if (rtrn == 0)
                    //{
                    t1.getDriverMobileNumber(textBox2.Text, textBox17);
                    t1.FindDriverImege(textBox2.Text);
                    dataGridView1.DataSource = t1.getPaymentDates("K" + textBox2.Text);
                    textBox16.Text = ((t1.getLastFreeDate(textBox2, checkBox1)).ToShortDateString());//get last free working date

                    textBox3.Text = (t1.getlastPaymentDate(dataGridView1, dateTimePicker1, textBox15, textBox16, checkBox1,dateTimePicker3)).ToShortDateString();

                    if (textBox3.Text != "1/1/0001")
                    {
                        if (textBox3.Text != "01/01/0001")
                        {
                            dateTimePicker3.Value = Convert.ToDateTime(textBox3.Text);
                        }
                    }
                   

                    textBox4.Text = (t1.getAreasPhoneBill(Convert.ToDateTime(textBox3.Text), dataGridView3, checkBox1, DateTime.Now, textBox18, checkBox3,checkBox2,textBox2,checkBox9)).ToString();
                    textBox5.Text = (Convert.ToInt32(textBox4.Text) / phoneBill).ToString();
                    //}
                }

                textBox6.ReadOnly = true;
            }

            
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
          {
               if (e.KeyChar == 13) 
              {
                t1 = new Taxi();               
                
                 textBox4.Text = (t1.getAreasPhoneBill(Convert.ToDateTime(textBox3.Text), dataGridView3,checkBox1,dateTimePicker1.Value,textBox18,checkBox3,checkBox2,textBox2,checkBox9)).ToString();
                 textBox8.Text= t1.AmountToWords(Convert.ToInt32(textBox25.Text));// amount in words
                 if (checkBox7.Checked)// special cabs
                 {
                     workingDays = t1.SpeccalculateWorkingDays(Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox4.Text), Convert.ToDateTime(textBox3.Text), dateTimePicker1.Value, textBox11, textBox12, textBox13, textBox14, dataGridView3, textBox18, checkBox3, Convert.ToDateTime(textBox16.Text), checkBox5,checkBox2,textBox2,checkBox9);
                     t1.SpecialGridFill(dateTimePicker1.Value, workingDays, dataGridView2, textBox3);
                 }
                 else
                 {
                     workingDays = t1.calculateWorkingDays(Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox4.Text), Convert.ToDateTime(textBox3.Text), dateTimePicker1.Value, textBox11, textBox12, textBox13, textBox14, dataGridView3, textBox18, checkBox3, Convert.ToDateTime(textBox16.Text), checkBox5, checkBox2, dataGridView1, textBox2, checkBox9, textBox9, textBox10, textBox7);
                     t1.gridFill(dateTimePicker1.Value, workingDays, dataGridView2, textBox3);

                     
                 }
                //workingDays=t1.workingdays(Convert.ToInt32(textBox6.Text), Convert.ToInt32(textBox4.Text), Convert.ToDateTime(textBox3.Text),dateTimePicker1.Value,textBox12,textBox13,textBox14,dataGridView3);

                //t1.calculateWorkingDays(t1.getAreasPhoneBill(Convert.ToDateTime(textBox3.Text)), Convert.ToInt32(textBox6.Text));// areas bill and pay amount for calculate the working date
               
                textBox25.Text = (Int32.Parse(textBox24.Text) + Int32.Parse(textBox6.Text)+Int32.Parse(textBox28.Text)).ToString();

                if ((textBox2.TextLength > 2) && (dateTimePicker1.Value.ToShortDateString() == DateTime.Now.ToShortDateString()))
                {
                    t1 = new Taxi();
                    t1.calculateAbsenceCharges(textBox3.Text, textBox16.Text, dateTimePicker1, textBox24, label43,textBox6,textBox25,checkBox6,checkBox1,textBox28);
                }


                t1.claculateTotalPhoneBil(dataGridView3, textBox9, textBox10, textBox7); //display only top right hand side 
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            t1 = new Taxi();
            t1.gridTickorUntick(dateTimePicker1.Value,workingDays, dataGridView2, Convert.ToInt32(textBox12.Text), textBox13, textBox14, textBox6,textBox12,dataGridView3,textBox8,textBox15,dataGridView4,checkBox7,checkBox9);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            
            if (textBox6.Text.Length <= 3)
            {
                dataGridView2.Rows.Clear();
                dataGridView3.Rows.Clear();               
            }
            if(textBox6.Text.Length>=1 )
                textBox25.Text = (Int32.Parse(textBox24.Text) + Int32.Parse(textBox6.Text) + Int32.Parse(textBox28.Text)).ToString();
            if (textBox6.Text.Length == 0)
                textBox25.Text = "0";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            dataGridView1.DataSource = null;
            textBox15.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = false; 
            if (textBox17.Text != "")
            {                
                t1 = new Taxi();
                t1.SaveDetails(dataGridView2, dataGridView3, workingDays, textBox2, textBox1, textBox6, textBox8, textBox17, checkBox2, checkBox4, textBox24, textBox25, label43, toolStripButton7, checkBox7, textBox27,textBox28,label49,dataGridView5,checkBox11);
            }
            else 
            {
                MessageBox.Show("Please Enter the Driver's company mobile Numeber");
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.printReceipt(textBox2,textBox1);          

        }

        private void ReceiptNew_Load(object sender, EventArgs e)
        {
            this.Text ="Daily Payment  "+ "Location - "+get_LocationName();
            textBox2.Enabled = false;
            textBox6.Enabled = false;
            panel1.Visible = false;
            us = new User();
            label40.Text = us.getCurrentUser();
            label45.Visible = false; checkBox7.Checked = false; checkBox7.Visible = false;
            
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //Report r = new Report();
            //r.Show();

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //Form6 f6 = new Form6();
            //f6.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
           // Form7 f7 = new Form7();
           // f7.Show();
            FrmReceiptCancel frmrptcncl = new FrmReceiptCancel();
            frmrptcncl.Show();

        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
    
        
        
        {
            if (e.KeyValue == 13) 
            {
                textBox11.Focus();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //if (textBox15.Text !="")
            //{
            //    t1 = new Taxi();
            //    int test = t1.dateCheckForStartingDate(dataGridView1, dateTimePicker1, textBox3);
            //}
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            
            Form11 frm = new Form11();
            frm.rcn = this;
            frm.Show();         
                
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            //if (textBox11.Text == "25") 
            //{
            //    Form11 f11 = new Form11();
            //    f11.Show();
            //}
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            Form12 f12 = new Form12();
            f12.Show();
        }

        private void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
           
                t1 = new Taxi();        
                textBox4.Text = (t1.getAreasPhoneBill(dateTimePicker3.Value, dataGridView3, checkBox1, dateTimePicker1.Value, textBox18, checkBox3,checkBox2,textBox2,checkBox9)).ToString();
                t1.calculateAbsenceCharges(textBox3.Text, textBox16.Text, dateTimePicker1, textBox24,label43,textBox6,textBox25,checkBox6,checkBox1,textBox28);
                t1.checkAppRental(textBox2, dateTimePicker1, textBox28,dataGridView5,checkBox10,textBox25,checkBox1,checkBox10,label49);
                textBox6.ReadOnly = false;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            Form17 f17=new Form17();
            f17.Show();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                textBox17.Focus();
            }
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox6.Focus();
            }
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            if((textBox17.Text).Length == 10) 
            {
                t1 = new Taxi();
                t1.selectSIMServiceProvider(textBox17, textBox18);
            }
            if ((textBox17.Text).Length < 10) 
            {
                textBox18.Text="";
            }
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Taxi t1 = new Taxi();
                textBox4.Text = (t1.getAreasPhoneBill(Convert.ToDateTime(textBox3.Text), dataGridView3, checkBox1, DateTime.Now, textBox18,checkBox3,checkBox2,textBox2,checkBox9)).ToString();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton4_ButtonClick(object sender, EventArgs e)
        {

        }

        private void brandedCarReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form22 f22 = new Form22();
            f22.Show();
        }

        private void normalCarReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox21_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                maskedTextBox1.Focus();
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                textBox22.Focus();
            }
        }

        private void textBox22_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox23.Focus();
            }

        }

        private void textBox23_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox19.Focus();
            }
        }

        private void textBox19_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox20.Focus();
            }
        }

        private void textBox20_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button1.PerformClick();
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                panel1.Visible = true;
                button1.Enabled = true;
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "dd-MM-yyyy  HH:mm:ss";

            }
            else
            {
                panel1.Visible = false;
                t1 = new Taxi();
                t1.clear_all(this);
                dateTimePicker2.Format = DateTimePickerFormat.Custom;
                dateTimePicker2.CustomFormat = "dd-MM-yyyy  HH:mm:ss";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.SaveBankDepositInfo(textBox21,textBox1, dateTimePicker2,textBox22,textBox23,textBox19,textBox20,button1,panel1);
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                MessageBox.Show("Please Enter The Reason For Ignore and Then Press Ok");
                panel2.Visible = true;
            }
            else
                panel2.Visible =false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(checkBox6.Checked==true)
            {
                label43.Text = textBox26.Text;
                textBox24.Text = "0";
            }
            if(checkBox10.Checked==true)
            {
                label49.Text = textBox26.Text;
                //textBox28.Text = "0";
                dataGridView5.Rows.Clear();
                t1 = new Taxi();
                t1.checkAppRental(textBox2, dateTimePicker1, textBox28, dataGridView5, checkBox10, textBox25, checkBox1, checkBox10,label49);
            }

            panel2.Visible = false; 
            //textBox24.Text = "0"; //textBox25.Text = "0";
        }

        private void toolStripButton5_ButtonClick(object sender, EventArgs e)
        {

        }

        private void timeRangeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRportTime rt = new FrmRportTime();
            rt.Show();
        }

        private void normalReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report r = new Report();
            r.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nrecn = new NewReceiptNumber();
            nrecn.updateReceiptNo(textBox1);
        }

        private void locationReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocationRpt flr = new FrmLocationRpt();
            flr.Visible = true;
        }

        private void specialReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSpReports fsr = new FrmSpReports();
            fsr.Show();
        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                textBox27.Enabled = true;
                textBox27.Text = "";
            }
            if (checkBox8.Checked == false)
            {
                textBox27.Enabled = false;
                textBox27.Text = "";
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {
            textBox25.Text = (Convert.ToInt32(textBox6.Text) + Convert.ToInt32(textBox24.Text) + Convert.ToInt32(textBox28.Text)).ToString();
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            textBox25.Text = (Convert.ToInt32(textBox6.Text) + Convert.ToInt32(textBox24.Text) + Convert.ToInt32(textBox28.Text)).ToString();
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox10.Checked)
            {
                t1 = new Taxi();
                t1.checkAppRental(textBox2, dateTimePicker1, textBox28, dataGridView5, checkBox10, textBox25, checkBox1,checkBox10,label49);
                MessageBox.Show("Please Enter The Reason For Ignore and Then Press Ok");
                panel2.Visible = true;
            }
            else
                panel2.Visible =false;
        
        }

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    //toolStripButton7.PerformClick();
        //}
    
       
        
        
    }
}
