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
    public partial class Form10 : Form
    {
        Taxi t1;
        Ventura ven;
        bool bblink = true;
        bool bblink2 = true;
        User us;
        Voucher vchr;
        public Form10()
        {
            InitializeComponent();
        }

        public string get_LocationName()
        {
            return System.Configuration.ConfigurationManager.AppSettings["LocName"];
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            this.Text = "Voucher Payment  " + "Location - " + get_LocationName();
            t1 = new Taxi();
            t1.paymentCheckerAll(dataGridView1, dateTimePicker1);
            t1.numberOfPaidTaxi(dataGridView1,textBox2,textBox5);
           // t1.getCallingNo(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value),textBox79,textBox6,panel3,textBox58);
            //t1.getCallingNoFromDespatchBooking(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6, panel3, textBox58);
            t1.getCallingNoFromLogsheet(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6, panel3, textBox58);
            //t1.getAllRefForAllCabsFromJob(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
            dataGridView6.Rows.Add(30);
            dataGridView6.Rows[0].Cells[2].Style.Font= new Font("Arial", 9F, FontStyle.Bold);
            dataGridView6.Rows[0].Cells[9].Style.Font= new Font("Arial", 9F, FontStyle.Bold);
            dataGridView6.Rows[0].Cells[2].Value = "Ref No";
            dataGridView6.Rows[0].Cells[9].Value = "Amount";
            panel1.Visible = false;           
            panel2.Visible = false;
            panel3.Visible = false;
            textBox44.Visible = false;
            textBox46.Visible = false;
            tabControl1.SelectedTab = tabPage5;
            tabPage1.Hide(); tabPage2.Hide(); tabPage3.Hide();
            us = new User();
            label64.Text= us.getCurrentUser();
            tabControl1.TabPages.Remove(tabPage6);
            textBox6.ReadOnly = true;
            dateTimePicker5.Enabled = false;
            panel4.Visible = false;
            textBox81.Text = "0.00"; textBox82.Text = "0.00";
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            vchr = new Voucher();
            vchr.ClearDeductionInfo(dataGridView12, textBox99, textBox93, textBox94, textBox95, textBox96, textBox97, textBox98, textBox92, textBox87, textBox88, textBox90, textBox89, textBox91, label119, label121, label120); 
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            t1 = new Taxi();
            if (textBox1.Text.Length < 2) 
            {
                t1.paymentCheckerAll(dataGridView1, dateTimePicker1);
                t1.numberOfPaidTaxi(dataGridView1, textBox2,textBox5);
            }
            if (e.KeyValue == 13) 
            {                
                t1.paymentCheckerSelectedTaxi(dataGridView1, dateTimePicker1, textBox1);
                t1.numberOfPaidTaxi(dataGridView1, textBox2,textBox5);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.paymentCheckerAll(dataGridView1, dateTimePicker1);
            t1.numberOfPaidTaxi(dataGridView1, textBox2,textBox5);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                t1 = new Taxi();
                dataGridView2.DataSource = t1.getPaymentDates("K" + textBox3.Text);
                textBox4.Text = t1.PaidForToday(dataGridView2,dateTimePicker1.Value);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = "";
            dataGridView2.DataSource = null;
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            t1 = new Taxi();
            if (textBox3.Text.Length < 2)
            {
               t1.freeDaysCabs(dataGridView3, dateTimePicker3);
               t1.numberOfFreeTaxi(dataGridView3, textBox7);
            }

            if (e.KeyValue == 13)
             {
                t1.FreeSelectedTaxiChecker(dataGridView3, dateTimePicker3, textBox8);
                t1.numberOfFreeTaxi(dataGridView3, textBox7);
             }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {            
            t1 = new Taxi();
            t1.freeDaysCabs(dataGridView3, dateTimePicker3);
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {            
            dataGridView4.DataSource = null;
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                t1 = new Taxi();
                dataGridView4.DataSource = t1.getFreePromotionDates("K" + textBox9.Text);               
            }
        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            //t1 = new Taxi();
            //t1.callingNumberChecker(dataGridView5, dateTimePicker5);
        }

        //private void textBox79_KeyDown(object sender, KeyEventArgs e)
        //{
           
        //    bool result ;
        //    if (e.KeyValue == 13)
        //    {
        //        //if (10 > dateTimePicker5.Value.Month)
        //        //{
        //        //    textBox72.Text = dateTimePicker5.Value.Month.ToString();
        //        //    textBox73.Text = dateTimePicker5.Value.Year.ToString();
        //        //}
        //        textBox6.ReadOnly = false;
        //        textBox6.Focus();

        //        t1 = new Taxi();
        //        result = t1.findBlockCabByCallCenter(textBox79);
        //        ven = new Ventura();
        //        if (result == false)
        //        {
        //            //if (textBox1.Text.Length < 2)
        //            //{
        //            //    t1.getCallingNo(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6);
        //            //}
        //            //if (e.KeyValue == 13)
        //            //{

        //            //t1.getCallingNoForSelectedTaxi(dataGridView5, textBox79, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), panel3, textBox58);
        //            t1.getRefNoForSelectedCabFromJob(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
        //            t1.displayValidNICForVoucher(textBox79, dataGridView7);
        //            ven.CheckVenturaCab(textBox79.Text, textBox44);
        //            //}
        //            textBox79.ReadOnly = true;
        //        }
        //    }
           
        //}

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
       {
           if (e.KeyValue == 13)
           {
               t1 = new Taxi();
               ven = new Ventura();
               Voucher vr = new Voucher();


               if (textBox79.Text != "")
                   textBox79.ReadOnly = true;
               //if (textBox1.Text.Length < 2)
               //{
               //    t1.getCallingNo(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6);
               //}
               //if (e.KeyValue == 13)
               //{
                 t1 = new Taxi();
                 //if (vr.getCabForRefNo(textBox79, textBox6, dateTimePicker5) == true)
                     if (vr.getCabForRefNoAppLogsheet(textBox79, textBox6, dateTimePicker5) == true)
                 {
                     //t1.getCabNoForSelectedVocher(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
                     //t1.getCabNoForSelectedRefFromJob(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
                     //t1.getCabNoForSelectedRefFromDespatchBooking(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
                     t1.getCabNoForSelectedRefFromLogSheet(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
                     ven.CheckVenturaCab(textBox79.Text, textBox44);

                     if(textBox45.Text=="")
                         vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox96, textBox97, textBox98, textBox92, this, textBox93, textBox94, textBox95, checkBox3, textBox88, textBox90, textBox89, textBox91,textBox87,textBox62,textBox63,textBox21);
                 }
               //}
           }
        }

        private void dateTimePicker5_CloseUp(object sender, EventArgs e)
        {
            Voucher vr=new Voucher();
            t1 = new Taxi();
           

           // t1.getCallingNo(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value),textBox79,textBox6);// server migrations
            if (vr.CheckValidMonth(dateTimePicker5, textBox72, textBox73) == true)
            {
                if (textBox79.Text != "")
                {
                    t1.getRefNoForSelectedCabFromLogsheet(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
                    //t1.getCallingNoForSelectedTaxi(dataGridView5, textBox79, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), panel3, textBox58);

                    if (textBox79.ReadOnly == false) 
                    {
                        vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox96, textBox97, textBox98, textBox92, this, textBox93, textBox94, textBox95, checkBox3, textBox88, textBox90, textBox89, textBox91, textBox87,textBox62,textBox63,textBox21);
                        textBox79.ReadOnly = true;
                    }

                }
                else
                    t1.getCallingNoFromLogsheet(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6, panel3, textBox58);
            }
            //if (10 > dateTimePicker5.Value.Month)
            //{
            //    textBox72.Text = dateTimePicker5.Value.Month.ToString();
            //    textBox73.Text = dateTimePicker5.Value.Year.ToString();
            //}
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //panel1.Visible = true;
                
                //textBox10.Text = dataGridView5.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (checkBox2.Checked != true)
            {
                t1 = new Taxi();
                t1.voucherDetailsView(dataGridView5, sender, e, textBox10, dateTimePicker9, textBox14, textBox13, textBox16, textBox17, panel1, dateTimePicker5, checkBox1, textBox25, panel3, textBox58,label88,button1,dataGridView6,textBox15,textBox100,checkBox2,checkBox3,textBox25);
            }
            else 
            {
                MessageBox.Show("You have Select Without Reference Number option, Please Unselect It.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            t1 = new Taxi();
            if (t1.checkVoucherGrid(dataGridView6, textBox14, textBox27) == false)
            {
                //button1.Enabled = false;

                vr.addVouchersToPaymentGrid(dataGridView8, dateTimePicker5.Value, textBox15, textBox96, textBox97, textBox98, textBox92, textBox79.Text, textBox94, textBox93, textBox95,dateTimePicker9.Value,checkBox2,textBox82,textBox81,textBox89,textBox91,textBox87,textBox62,textBox63,textBox21,textBox100);

                t1.addVouchersToList(textBox10, textBox13, dateTimePicker9, textBox14, textBox17, textBox16, textBox27, textBox15, textBox25, textBox28, textBox26, dataGridView5, dateTimePicker5, dataGridView6, textBox14, textBox26, textBox21, textBox31, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox45, panel3, textBox58, textBox92, textBox62, textBox65, label88,textBox82,textBox81,textBox100,checkBox9);

                //vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox59, textBox60, textBox61, textBox64, this,textBox72,textBox73);

                //t1.addVouchersToList(dataGridView6,textBox14,textBox26,textBox21,textBox31);
                t1.panelClear(textBox10, textBox14, dateTimePicker9, textBox13, textBox17, textBox16, textBox15, textBox27,textBox100);
                panel1.Visible = false;

                if (checkBox3.Checked == false)
                {
                    vr.callAdditionalEarningForNewDedctMethod(textBox97, textBox98, textBox92, textBox79.Text, textBox94, textBox93, textBox88, textBox90, textBox89, textBox91, textBox87, textBox62, textBox63);
                }
                else 
                {
                    textBox62.Text = "0.00"; textBox63.Text = "0.00"; textBox87.Text = "0.00"; textBox88.Text = "0.00";textBox89.Text = "0.00"; 
                    textBox90.Text = "0.00"; textBox91.Text = "0.00"; textBox92.Text = "0.00";
                }
            }
            else
                MessageBox.Show("Sorry !!! Alredy added to the Payment Grid");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.panelClear(textBox10, textBox14, dateTimePicker9, textBox13, textBox17, textBox16, textBox15, textBox27,textBox100);
            panel1.Visible = false;
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Form15 f14 = new Form15();
            //f14.Show();
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.VoucherCommition(textBox15, textBox25, textBox28, textBox26);
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.VoucherCommition(textBox15, textBox25, textBox28, textBox26);
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.VoucherCommition(textBox22, textBox29, textBox23, textBox24);
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.VoucherCommition(textBox22, textBox29, textBox23, textBox24);
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                textBox30.Focus();
            }
        }

        private void dateTimePicker6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox18.Focus();
            }
        }

        private void textBox30_KeyDown(object sender, KeyEventArgs e)
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
                textBox22.Focus();
            }
        }

        //private void textBox21_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyValue == 13)
        //    {
        //        textBox22.Focus();
        //    }
        //}

        private void textBox22_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox29.Focus();
            }
        }

        private void textBox29_KeyDown(object sender, KeyEventArgs e)
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
                textBox24.Focus();
            }
        }

        private void textBox24_KeyDown(object sender, KeyEventArgs e)
        {
            button3.PerformClick();
        }

        private void textBox17_KeyDown(object sender, KeyEventArgs e)
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
                textBox27.Focus();
            }
        }

        private void textBox27_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox15.Focus();
            }
        }

        private void textBox15_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox28.Focus();
            }
        }

        private void textBox25_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox28.Focus();
            }
        }

        private void textBox28_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox26.Focus();
            }
        }

        private void textBox26_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button1.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            t1 = new Taxi();
            t1.vouchersSaveWithoutRef(textBox18,textBox19,dateTimePicker6,textBox20,textBox30,textBox22,textBox29,textBox23,textBox24);

            vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox96, textBox97, textBox98, textBox92, this, textBox93, textBox94, textBox95, checkBox3, textBox88, textBox90, textBox89, textBox91, textBox87,textBox62,textBox63,textBox21);
        }

        private void toolStripButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void normalReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form15 f15 = new Form15();
            f15.Show();
        }

        private void timeRangeReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form16 f16 = new Form16();
            f16.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bblink)
            {
                textBox14.BackColor = Color.White;
                textBox14.ForeColor = Color.Red;
            }
            else
            {
                textBox14.BackColor = Color.Red;              
                textBox14.ForeColor = Color.White;
            }
            bblink = !bblink;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.Visible = false;
            Voucher vr = new Voucher();
            t1 = new Taxi();
            string deposit = "";
            //DialogResult dr1 = MessageBox.Show("මුලු මුදලම තැන්පතුවක් වශයෙන් තබා ගැනීමට අවශ්‍යද?", "තහවුරු කරන්න", MessageBoxButtons.YesNoCancel);
            //if (dr1 == DialogResult.Yes)
            //{
            //    deposit = "Yes";
            //}
            //if (dr1 == DialogResult.No)
            //{
            //    deposit = "No";
            //}

                DialogResult dr = MessageBox.Show("Are You Sure Want To Pay", "Confirm", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    if (false == t1.findDuplicateVoucher(dataGridView6))
                    {

                        vr.SaveVoucherHireRef(dataGridView6, textBox45, textBox34, textBox96, textBox97, textBox62, textBox63, textBox94, textBox93, textBox98, textBox74, textBox92, textBox12, textBox77, checkBox4, dataGridView10, textBox78, label97, textBox11, checkBox6, textBox64, textBox83, textBox99, textBox89, textBox91,deposit,textBox65);


                        t1.updateMobilePhoneLoan(textBox79, label97, textBox80, textBox11);
                        t1.releaseVoucherPayment(dataGridView6, textBox21, textBox31, panel2, textBox32, textBox34, textBox79, dateTimePicker5, textBox45, textBox62, textBox63, textBox65, textBox31, textBox6, checkBox2, checkBox3, checkBox4, textBox78, textBox81, textBox82, checkBox6, checkBox7, textBox83);
                        t1.getCallingNoFromLogsheet(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6, panel3, textBox58);

                        if (Convert.ToDouble(textBox89.Text) > 0.00 || Convert.ToDouble(textBox91.Text) > 0.00)
                        {
                            vr.SaveNewDeductReceipt(textBox99.Text, textBox79, textBox94, textBox93, textBox95, textBox96, textBox97, textBox98, textBox92, textBox87, textBox88, textBox90, textBox89, textBox91, label119, label120, label121);

                            ReportsPrint rp = new ReportsPrint();
                            rp.printNewDeductionReceipt(dataGridView12, textBox95, textBox96, textBox97, textBox98, textBox92, textBox93, textBox94, textBox88, textBox90, textBox89, textBox91, textBox99, label121, label120, label119);
                        }
                        if (textBox44.Visible == true)
                        {
                            t1.setInfoFromVouchers(textBox45.Text, textBox31, textBox44);
                        }
                        textBox79.Clear(); textBox16.Clear(); textBox31.Clear(); textBox81.Text = "0.00"; textBox82.Text = "0.00";

                        button5.Enabled = false;

                        //vr.ClearDeductionInfo(dataGridView12, textBox99, textBox93, textBox94, textBox95, textBox96, textBox97, textBox98, textBox92, textBox87, textBox88, textBox90, textBox89, textBox91, label119, label121, label120); 
                    }
                    else
                    {
                        MessageBox.Show("Duplicate Voucher Entries Detected, Please Re-Enter the Vouchers, Press New and Enter Again ");
                    }
                }
           

        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox32.Text = "";
            panel2.Visible = false;
        }

        //private void textBox79_TextChanged(object sender, EventArgs e)
        //{
        //    if (dataGridView7.Rows.Count >= 1)
        //        dataGridView7.Rows.Clear();
        //    if (dataGridView8.Rows.Count >= 1)
        //    {
        //        dataGridView8.Rows.Clear(); textBox59.Text = "0"; textBox60.Text = "0.00"; textBox61.Text = "0.00"; textBox64.Text = "0.00"; textBox72.Text = "0"; textBox73.Text = "0"; textBox77.Text = "0";
        //    }
        //    if (textBox79.Text == "" || textBox79 == null)            
        //    {
        //        t1.getCallingNo(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6,panel3,textBox58);
        //        textBox44.Visible = false;
        //    }
        //}

        private void button7_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.addValidNICForVoucher(textBox79,textBox33,dataGridView7,textBox76);
        }

        private void dataGridView6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
             
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            t1 = new Taxi();
            t1.addNICfromGridToTextBox(dataGridView7, e, textBox34,textBox12);
        }

        private void textBox36_TextChanged(object sender, EventArgs e)
        {
            if (textBox36.Text == "" || textBox36.Text == null)
            {
                textBox36.Clear(); dateTimePicker7.Value = DateTime.Now; textBox35.Clear(); textBox42.Clear(); textBox39.Clear(); textBox40.Clear(); textBox41.Clear(); textBox43.Clear(); textBox38.Clear(); textBox37.Clear();
            }
        }

        private void dateTimePicker7_CloseUp(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.FindPaidVoucher(textBox36,dateTimePicker7,textBox35,textBox42,textBox39,textBox40,textBox41,textBox43,textBox38,textBox37);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox36.Clear(); dateTimePicker7.Value = DateTime.Now; textBox35.Clear(); textBox42.Clear(); textBox39.Clear(); textBox40.Clear(); textBox41.Clear(); textBox43.Clear(); textBox38.Clear(); textBox37.Clear();
            radioButton3.Checked = false; radioButton4.Checked = false;
        }

        private void textBox35_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                t1 = new Taxi();
                t1.FindPaidVoucherNo(textBox36, dateTimePicker7, textBox35, textBox42, textBox39, textBox40, textBox41, textBox43, textBox38, textBox37, radioButton3, radioButton4, label122,dataGridView13,checkBox8);
            }
        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {
            if (textBox35.Text == "" || textBox35.Text == null)
            {
                textBox36.Clear(); dateTimePicker7.Value = DateTime.Now; textBox35.Clear(); textBox42.Clear(); textBox39.Clear(); textBox40.Clear(); textBox41.Clear(); textBox43.Clear(); textBox38.Clear(); textBox37.Clear();
            }
        }

        private void textBox35_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            //t1 = new Taxi();
            //t1.FindPaidVoucher(textBox36, dateTimePicker7, textBox35, textBox42, textBox39, textBox40, textBox41, textBox43, textBox38, textBox37);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox45.Text == "")
            {
                if (dataGridView8.Rows.Count >= 1)
                {
                    //dataGridView8.Rows.Clear(); textBox59.Text = "0"; textBox60.Text = "0.00"; textBox61.Text = "0.00"; textBox64.Text = "0.00"; textBox72.Text = "0"; textBox73.Text = "0";
                }
                if (textBox6.Text == "" || textBox6.Text == null)
                {
                    t1.getCallingNoFromLogsheet(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6, panel3, textBox58);
                    textBox44.Visible = false;
                }
            }
            if ((textBox79.Text != "") && (textBox6.Text == "") && (textBox45.Text != ""))
            {
                //t1.getCallingNoForSelectedTaxi (dataGridView5, textBox79, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), panel3, textBox58);
                t1.getRefNoForSelectedCabFromLogsheet(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);

                //t1.displayValidNICForVoucher(textBox79, dataGridView7);
                //ven.CheckVenturaCab(textBox79.Text, textBox44);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (bblink)
            {
                textBox44.BackColor = Color.White;
                textBox44.ForeColor = Color.Red;
            }
            else
            {
                textBox44.BackColor = Color.Red;
                textBox44.ForeColor = Color.White;
            }
            bblink = !bblink;
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            Voucher vr = new Voucher();
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                MessageBox.Show("Please Enter Cab No Without 'K', Eg-: If Cab No is K901, You Should Type only 901. No need to type 'K'");
            }
           
            if (e.KeyChar == 13)
            {
                ven = new Ventura();
                ven.CheckVenturaCab(textBox18.Text, textBox46);
                vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox96, textBox97, textBox98, textBox92, this, textBox93, textBox94, textBox95, checkBox3, textBox88, textBox90, textBox89, textBox91, textBox87,textBox62,textBox63,textBox21);
            }

           
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (bblink2)
            {
                textBox46.BackColor = Color.White;
                textBox46.ForeColor = Color.Red;
            }
            else
            {
                textBox46.BackColor = Color.Red;
                textBox46.ForeColor = Color.White;
            }
            bblink2 = !bblink2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox46.Visible = false;
            textBox18.Clear(); textBox30.Clear(); textBox19.Clear(); textBox20.Clear();
            textBox22.Clear(); textBox29.Clear(); textBox23.Clear(); textBox24.Clear();
            //t1 = new Taxi();
            //t1.clearManualVoucher(this);
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView9.Rows.Count >= 1)
            {
                dataGridView9.Rows.Clear(); textBox69.Text = "0"; textBox68.Text = "0.00"; textBox67.Text = "0.00"; textBox64.Text = "0.00";
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox18.Text, "  ^ [0-9]"))
            {
                textBox18.Text = "";
            }

            if(textBox18.TextLength==0)
            {
                textBox46.Visible = false;
            }
        }

        private void venturaReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form24 f24 = new Form24();
            f24.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form23 f23 = new Form23();
            f23.Show();
        }

        

        private void textBox55_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                t1 = new Taxi();
                t1.FindPaidVoucherNo(textBox55, dateTimePicker8, textBox54, textBox52, textBox49, textBox50, textBox51, textBox53, textBox48, textBox47, radioButton1, radioButton2,label122,dataGridView13,checkBox8);
                panel7.Visible = true;
            }
        }

        private void textBox54_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                t1 = new Taxi();
                t1.FindPaidVoucherNo(textBox55, dateTimePicker8, textBox54, textBox52, textBox49, textBox50, textBox51, textBox53, textBox48, textBox47, radioButton1, radioButton2, label122,dataGridView13,checkBox8);
                panel7.Visible = true;
            }
        }

        private void textBox54_TextChanged(object sender, EventArgs e)
        {
             textBox52.Clear(); textBox49.Clear(); textBox50.Clear(); textBox51.Clear(); textBox53.Clear(); textBox48.Clear(); textBox47.Clear(); 
            dateTimePicker8.Value = DateTime.Now;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox55.Clear(); textBox54.Clear(); textBox52.Clear(); textBox49.Clear(); textBox50.Clear(); textBox51.Clear(); textBox53.Clear(); textBox48.Clear(); textBox47.Clear(); radioButton1.Checked = false; radioButton2.Checked = false;
            dateTimePicker8.Value = DateTime.Now;
        }

        private void textBox55_TextChanged(object sender, EventArgs e)
        {
            textBox52.Clear(); textBox49.Clear(); textBox50.Clear(); textBox51.Clear(); textBox53.Clear(); textBox48.Clear(); textBox47.Clear(); 
            dateTimePicker8.Value = DateTime.Now;
            
        }

        private void textBox36_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                t1 = new Taxi();
                t1.FindPaidVoucherNo(textBox36, dateTimePicker7, textBox35, textBox42, textBox39, textBox40, textBox41, textBox43, textBox38, textBox37, radioButton3, radioButton4, label122,dataGridView13,checkBox8);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                textBox25.Enabled = true;
            if(checkBox1.Checked==false)
                textBox25.Enabled = false; textBox25.Text = "7.5";

        }

        private void dataGridView5_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            textBox55.Clear(); textBox54.Clear(); textBox52.Clear(); textBox49.Clear(); textBox50.Clear(); textBox51.Clear(); textBox53.Clear(); textBox48.Clear(); textBox47.Clear(); radioButton1.Checked = false; radioButton2.Checked = false;
            dateTimePicker8.Value = DateTime.Now;
        }

        //private void toolStripButton6_Click(object sender, EventArgs e)
        //{
        //    textBox55.Clear(); textBox54.Clear(); textBox52.Clear(); textBox49.Clear(); textBox50.Clear(); textBox51.Clear(); textBox53.Clear(); textBox48.Clear(); textBox47.Clear(); radioButton1.Checked = false; radioButton2.Checked = false;
        //    dateTimePicker8.Value = DateTime.Now;
        //    panel7.Visible = false; label122.Text = ""; dataGridView13.Rows.Clear(); checkBox8.Checked = false;
        //}

        //private void toolStripButton7_Click(object sender, EventArgs e)
        //{
        //    t1 = new Taxi();
        //    t1.VoucherCancel(textBox55.Text, textBox54.Text, textBox56.Text, textBox52.Text, dateTimePicker8.Value, textBox50.Text, textBox51.Text,textBox48.Text, radioButton1, radioButton2,checkBox8,label122);

        //}

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            textBox36.Clear(); dateTimePicker7.Value = DateTime.Now; textBox35.Clear(); textBox42.Clear(); textBox39.Clear(); textBox40.Clear(); textBox41.Clear(); textBox43.Clear(); textBox38.Clear(); textBox37.Clear();
            radioButton3.Checked = false; radioButton4.Checked = false;

        }

        private void dateTimePicker3_CloseUp(object sender, EventArgs e)
        {

        }

        

        private void button9_Click(object sender, EventArgs e)
        {
            panel3.Visible = false; textBox58.Text = ""; textBox57.Text = "";
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            textBox57.Text = "";
            t1 = new Taxi();
            t1.unblock(textBox57.Text,textBox58.Text,panel3);
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            vr.printVoucherReceipt("VR000031",textBox94,textBox93);
            //vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox59, textBox60, textBox61, textBox64, this);            
            //vr.printVoucher(dataGridView6,textBox21,textBox31,textBox62,textBox63,textBox65,textBox79,textBox34);

            //vr.saveVoucherReceipt(textBox45,textBox34,textBox59,textBox60,textBox62,textBox63);
        }

        //private void textBox79_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //if (10 < dateTimePicker5.Value.Month)
        //    //{
        //    //    textBox72.Text = dateTimePicker5.Value.Month.ToString();
        //    //    textBox73.Text = dateTimePicker5.Value.Year.ToString();
        //    //}

        //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
        //    {
        //        e.Handled = true;
        //        MessageBox.Show("Please Enter Cab No Without 'K', Eg-: If Cab No is K901, You Should Type only 901. No need to type 'K'");
        //    }


        //    Voucher vr = new Voucher();
            
        //    if (e.KeyChar == 13)
        //    {
               
        //        if(textBox45.Text=="")
        //        vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox59, textBox60, textBox61, textBox64, this,textBox72,textBox73,textBox77,checkBox3);
        //        t1=new Taxi();
        //        t1.getPendingPhonebills(dataGridView10, textBox79,textBox78);
        //        dateTimePicker5.Enabled = true;
        //    }
        //}

        

        private void textBox61_TextChanged(object sender, EventArgs e)
        {
            //Voucher vr = new Voucher();           
            //vr.calAdditonalEarning(textBox60, textBox61, textBox64);
        }
        
        private void textBox67_TextChanged(object sender, EventArgs e)
        {
            //Voucher vr = new Voucher();
            //vr.calAdditonalEarning(textBox68, textBox67, textBox66,textBox79.Text,textBox73,textBox72,textBox82,textBox81);
        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            if(checkBox3.Checked==false)
            vr.FineForAppPhone(textBox31, textBox83, checkBox7,textBox25);

            
            //vr.decideRefund(textBox79, textBox60, textBox61, textBox63, textBox73, textBox72,textBox74,textBox62,checkBox6,textBox82,textBox81);
           
            vr.calVoucherNetTotal(textBox31, textBox62, textBox63, textBox65,textBox78,checkBox5,textBox11,textBox80,label99,textBox83);

            //vr.callAdditionalEarningForNewDedctMethod(textBox97, textBox98, textBox92, textBox79.Text, textBox94, textBox93, textBox88, textBox90, textBox89, textBox91, textBox87, textBox89, textBox91);

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void textBox60_TextChanged(object sender, EventArgs e)
        {
            //Voucher vr = new Voucher();
            //vr.calAdditonalEarning(textBox60, textBox61, textBox64);
        }

        private void textBox62_TextChanged(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            vr.calVoucherNetTotal(textBox31, textBox62, textBox63, textBox65,textBox78,checkBox5,textBox11,textBox80,label99,textBox83);
        }

        private void textBox59_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox64_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            vchr = new Voucher();
            textBox79.ReadOnly = false; textBox79.Text = ""; textBox12.Text = ""; dataGridView6.Rows.Clear(); 
            textBox45.Text = ""; textBox34.Text = ""; textBox21.Text = "0"; textBox31.Text = "0.00"; 
            textBox62.Text = "0.00"; textBox63.Text = "0.00"; textBox65.Text = "0.00";
            textBox6.Text = ""; dataGridView6.Rows.Add(30); checkBox2.Checked = false; textBox72.Text = "0"; textBox73.Text = "0"; textBox77.Text = "0";
            dateTimePicker5.Value = DateTime.Now.Date; button5.Enabled = true; textBox6.ReadOnly = true; checkBox3.Checked = false; checkBox4.Checked = false;
            dateTimePicker5.Enabled = true; button5.Visible = true;

            label97.Text = "XXXX"; textBox80.Text = "0.00"; textBox11.Text = "0.00"; label99.Text = "0.00"; panel4.Visible = false;
            textBox81.Text = "0.00"; textBox82.Text = "0.00"; textBox83.Text = "0.00"; checkBox7.Checked = false;
            textBox84.Text = ""; textBox85.Text = "0.00"; textBox86.Text = ""; button17.Enabled = true;
            vchr.ClearDeductionInfo(dataGridView12, textBox99, textBox93, textBox94, textBox95, textBox96, textBox97, textBox98, textBox92, textBox87, textBox88, textBox90, textBox89, textBox91, label119, label121, label120); 

        }

        private void textBox63_TextChanged(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            vr.calVoucherNetTotal(textBox31, textBox62, textBox63, textBox65,textBox78,checkBox5,textBox11,textBox80,label99,textBox83);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox9.Checked = true; //if checked, System think that Voucher does not have a reference number

                //MessageBox.Show("Please Make Sure to select the Voucher Date");
                if (textBox79.Text != "")
                {
                    textBox10.Text = textBox79.Text;
                    textBox10.ReadOnly = true;
                }
                else { textBox10.ReadOnly = false; }
                panel1.Visible = true;
                button1.Enabled = true;

                panel1.BackColor = Color.DarkSlateGray;
                dateTimePicker9.Focus();
                textBox14.ReadOnly = true; textBox14.Text = "XXXXXX";  textBox13.ReadOnly = false;
                dataGridView5.ReadOnly = true;
                textBox25.Enabled = true; textBox25.Text = "0"; textBox100.Text = "0"; textBox16.Text = "Credit";
                
            }
            else 
            {
                panel1.Visible = false;
                panel1.BackColor = Color.Gray;
                textBox79.Focus();
                textBox14.ReadOnly = false; textBox14.Text = ""; textBox10.ReadOnly = true; textBox13.ReadOnly = true;
                dataGridView5.ReadOnly = false;
                textBox25.Enabled = false; textBox25.Text = "7.5";
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox9.Checked = false; // if unchecked, system thinks that voucher has a reference number
            }

        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                textBox13.Focus();
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyValue == 13)
                textBox17.Focus();
        }

        private void dateTimePicker9_CloseUp(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            if (vr.CheckValidMonth(dateTimePicker9, textBox72, textBox73) == true);
            textBox10.Focus();
           
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            Voucher vr = new Voucher();

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                MessageBox.Show("Please Enter Cab No Without 'K', Eg-: If Cab No is K901, You Should Type only 901. No need to type 'K'");
            }

            if (e.KeyChar == 13)
            {
                vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox96, textBox97, textBox98, textBox92, this, textBox93, textBox94, textBox95, checkBox3, textBox88, textBox90, textBox89, textBox91, textBox87,textBox62,textBox63,textBox21);
                textBox79.Text = textBox10.Text;
            }
        }

        private void dateTimePicker9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
                textBox10.Focus();
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox65_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            FrmVoucherRecFind fvf = new FrmVoucherRecFind();
            fvf.Show();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            FrmAdvfind fadvf = new FrmAdvfind();
            fadvf.Show();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                DialogResult dr = MessageBox.Show("Are You Sure Want to Select This Option", "", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    panel1.Visible = true;
                    checkBox4.Checked = true;
                }
                else 
                {
                    checkBox4.Checked = false;
                }
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                panel1.Visible = false;
                            
                //DialogResult dr = MessageBox.Show("Are You Sure Want to Ignore Additional Charges....!", "", MessageBoxButtons.OKCancel);
                //if (dr == DialogResult.OK)
                DialogResult dr = MessageBox.Show("මෙය SLT වව්චර් එකකි, කරුණාකර වෙනම ලදු පතක් ලබා දෙන්න.This is a SLT Voucher, Please pay as a seperate receipt!, Are you sure want to continue ", "SLT Voucher", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                   
                    checkBox3.Checked = true;
                    checkBox4.Checked = true;
                    panel4.Visible = true;
                    panel4.BringToFront();
                    textBox62.Text = "0.00";
                    textBox63.Text = "0.00";
                    textBox64.Text = "0.00";
                }
                else
                {
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    checkBox2.Checked = false;
                    panel4.Visible = false;
                    panel1.Visible = false;

                }
            }
            else
                panel4.Visible = false;
            if (checkBox3.Checked == false) {
                checkBox4.Checked = false;
            }
        }

        private void locationReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLocationVoucherRpt frmVrpt = new FrmLocationVoucherRpt();
            frmVrpt.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //t1 = new Taxi();
            //t1.updatePhoneBillFromVoucher(dataGridView10);
        }

        private void textBox78_TextChanged(object sender, EventArgs e)
        {
            //Voucher vr = new Voucher();
            //vr.calVoucherNetTotal(textBox31, textBox62, textBox63, textBox65, textBox78);
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox79_KeyDown(object sender, KeyEventArgs e)
        {
            bool result; bool loan;
            if (e.KeyValue == 13)
            {
                //if (10 > dateTimePicker5.Value.Month)
                //{
                //    textBox72.Text = dateTimePicker5.Value.Month.ToString();
                //    textBox73.Text = dateTimePicker5.Value.Year.ToString();
                //}
                textBox6.ReadOnly = false;
                textBox6.Focus();

                t1 = new Taxi();
                result = t1.findBlockCabByCallCenter(textBox79);
                loan = t1.findMobilePhoneLoanCab(textBox79,textBox80,checkBox5,label97,label99);
                ven = new Ventura();
                if (result == false)
                {
                    //if (textBox1.Text.Length < 2)
                    //{
                    //    t1.getCallingNo(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6);
                    //}
                    //if (e.KeyValue == 13)
                    //{

                    //t1.getCallingNoForSelectedTaxi(dataGridView5, textBox79, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), panel3, textBox58);
                    //t1.getRefNoForSelectedCabFromJob(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
                    //t1.getRefNoForSelectedCabFromNewDispatch(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);
                    t1.getRefNoForSelectedCabFromLogsheet(dataGridView5, textBox6, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox44, panel3, textBox58, textBox79);

                    t1.displayValidNICForVoucher(textBox79, dataGridView7);
                    ven.CheckVenturaCab(textBox79.Text, textBox44);
                    //}
                    textBox79.ReadOnly = true;
                }
                panel5.Visible = true;
            }
            
        }

        private void textBox79_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (10 < dateTimePicker5.Value.Month)
            //{
            //    textBox72.Text = dateTimePicker5.Value.Month.ToString();
            //    textBox73.Text = dateTimePicker5.Value.Year.ToString();
            //}

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
                MessageBox.Show("Please Enter Cab No Without 'K', Eg-: If Cab No is K901, You Should Type only 901. No need to type 'K'");
            }


            Voucher vr = new Voucher();

            if (e.KeyChar == 13)
            {
                checkBox3.Checked = false;

                if (textBox45.Text == "")
                    vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox96, textBox97, textBox98, textBox92, this, textBox93, textBox94, textBox95, checkBox3, textBox88, textBox90, textBox89, textBox91, textBox87,textBox62,textBox63,textBox21);

                      //comment for new dedcution system 27/07/2017
                //vr.getWorkingDays(textBox79.Text, dateTimePicker5.Value, dataGridView8, textBox59, textBox60, textBox61, textBox64, this, textBox72, textBox73, textBox77, checkBox3, textBox82, textBox81);
                t1 = new Taxi();
                t1.getPendingPhonebills(dataGridView10, textBox79, textBox78);
                t1.findNoAppPhone(textBox79, checkBox7,panel6);
                dateTimePicker5.Enabled = true;
            }
        }

        private void textBox79_TextChanged(object sender, EventArgs e)
        {
            
            if (dataGridView7.Rows.Count >= 1)
                dataGridView7.Rows.Clear();
            if (dataGridView8.Rows.Count >= 1)
            {
                dataGridView8.Rows.Clear(); textBox59.Text = "0"; textBox60.Text = "0.00"; textBox61.Text = "0.00"; textBox64.Text = "0.00"; textBox72.Text = "0"; textBox73.Text = "0"; textBox77.Text = "0";
            }
            if (textBox79.Text == "" || textBox79 == null)
            {
                t1.getCallingNoFromLogsheet(dataGridView5, String.Format("{0:yyyy-MM-dd}", dateTimePicker5.Value), textBox79, textBox6, panel3, textBox58);
                textBox44.Visible = false;
            }
        }

        private void textBox80_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
             t1 = new Taxi();

             t1.calNewMobilePhoneAreas(textBox80, textBox11,label99);
        }

        private void button12_Click(object sender, EventArgs e)
        {
             t1 = new Taxi();
             t1.updateMobilePhoneLoan(textBox79, label97, textBox80, textBox11);
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.updateMobilePhoneLoan(textBox79, label97, textBox80, textBox11);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.findDuplicateVoucher(dataGridView6);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
        }

        private void textBox84_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                vchr = new Voucher();
                vchr.FindAppFineForRefund(textBox84, textBox85);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            button17.Enabled = false;
             vchr = new Voucher();
             vchr.RefundAppFine(textBox84, textBox86); 
        }

        private void button18_Click(object sender, EventArgs e)
        {
            ReportsPrint rp = new ReportsPrint();
            rp.printNewDeductionReceipt(dataGridView12, textBox95, textBox96, textBox97, textBox98, textBox92, textBox93, textBox94, textBox88, textBox90, textBox89, textBox91, textBox99, label121, label120, label119);
            //Voucher vr = new Voucher();
            //vr.breakDownTheAdditionalIncome(dataGridView12,textBox87,textBox88,textBox89,textBox94,textBox93,textBox96,textBox92);
        }

        private void textBox92_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(textBox92.Text) > 0.00)
            {
                if (Convert.ToInt32(textBox21.Text) > 0 && checkBox3.Checked==false)
                {
                    Voucher vr = new Voucher();
                    vr.breakDownTheAdditionalIncome(dataGridView12, textBox87, textBox88, textBox89, textBox94, textBox93, textBox96, textBox92);
                }
            }
        }

        private void textBox87_TextChanged(object sender, EventArgs e)
        {
            //Voucher vr = new Voucher();
            //if (Convert.ToDouble(textBox97.Text) > 0.00)
            //    vr.calCurrentDeduction(textBox89, textBox87, textBox88);
        }

        private void textBox89_TextChanged(object sender, EventArgs e)
        {

        }

        private void label117_Click(object sender, EventArgs e)
        {

        }

        private void textBox99_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Voucher vr = new Voucher();
                vr.findNewDeductReceipt(textBox99, label121, textBox94, textBox93, textBox95, textBox96, textBox97, textBox98, textBox92, textBox87, textBox88, textBox90, textBox89, textBox91, label119, label120);
                vr.breakDownTheAdditionalIncome(dataGridView12, textBox87, textBox88, textBox89, textBox94, textBox93, textBox96, textBox92);
            }
        }

        private void textBox99_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox91_TextChanged(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            vr.printSelectedVoucherReceipt(textBox99.Text,textBox94,textBox93);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            textBox55.Clear(); textBox54.Clear(); textBox52.Clear(); textBox49.Clear(); textBox50.Clear(); textBox51.Clear(); textBox53.Clear(); textBox48.Clear(); textBox47.Clear(); radioButton1.Checked = false; radioButton2.Checked = false;
            dateTimePicker8.Value = DateTime.Now; checkBox8.Checked = false; label122.Text = ""; dataGridView13.Rows.Clear();
        }

        private void toolStripButton4_Click_2(object sender, EventArgs e)
        {
            t1 = new Taxi();
            t1.VoucherCancel(textBox55.Text, textBox54.Text, textBox56.Text, textBox52.Text, dateTimePicker8.Value, textBox50.Text, textBox51.Text,textBox48.Text, radioButton1, radioButton2,checkBox8,label122);

        }

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView8_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView12_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            Taxi t = new Taxi();
            t.saveAdditionComCab(textBox101.Text);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Taxi t = new Taxi();
            t.RemoveAdditionComCab(textBox101.Text);
        }

        
       

        

        

       

        

       

       
        

        


      
        

        
      

        
    }
}
