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
    public partial class Recipt : Form
    {
        string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        MySqlConnection connection;
        MySqlDataAdapter adapter;
        
        int NoOfDays;
        int amountPay = 0;
        int afterPayAreas = 0;
        int firstBalance = 0;
        static bool proceed =false;
        int mainAmount = 0;
        int freeDayCounter = 0;
        int maxfreeDays = 0;
        public Recipt()
        {
            InitializeComponent();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        
        {
            textBox11.Text = "0";
            dataGridView2.Rows.Clear();

            if (e.KeyChar == 13)
            {
                proceed = false;
                amountPay = Convert.ToInt32(textBox5.Text);
                dataGridView2.Rows.Clear();
                textBox8.Text = "";
                textBox9.Text = "";
                try
                {
                    int areas = areasPhoneBill();
                    afterPayAreas = amountPay - areas;//recovered the phone bill areas+this month payment from paid amount
                    textBox6.Text = NumberToText(amountPay);//amount in words
                    textBox3.Text = areas.ToString();
                    textBox7.Text = areas.ToString();
                }
                catch (FormatException) { }

                 int daycnt=(afterPayAreas- (afterPayAreas%300))/300;
                 
                 int df = (dateTimePicker1.Value.AddDays(daycnt).Month) - dateTimePicker1.Value.Month;
                
                ////////////////////////////////////////////////////////////////////////////

                if (dateTimePicker1.Value.Month == dateTimePicker1.Value.AddDays(10).Month)//same month (no need to deduct 400)
                {
                    if (afterPayAreas >= 3000)
                    {
                        gridFill(afterPayAreas);
                    }
                    else //for 10 days, required 10*300=300
                    {
                        int shortamt = 3000 - afterPayAreas;
                        DialogResult dr = MessageBox.Show("You have to pay for atleast 10 days" + " Your short amount is " + shortamt + "\n If you can pay, press YES " + "\n If not Press No", "For New Month", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        if (dr == DialogResult.Yes)
                        {
                            button4.Visible = false;
                            panel1.Visible = true;
                            label18.Visible = false;
                            label15.Visible = true;
                            button5.Visible = true;
                            label15.Text = shortamt.ToString();
                        }
                    }
                }

              
                else if(df>0)//not same month (eg: start month 6th end month 7th- So need 400 for 7th month)
                {
                    if (df > 1) 
                    {
                        if (afterPayAreas >= (daycnt * 300) + (df * 400))//amount is ok for bill payment
                        {
                            gridFill(afterPayAreas);
                        }
                        else 
                        {
                            int shrt = ((daycnt * 300) + (df * 400)) - afterPayAreas;
                            DialogResult dr = MessageBox.Show("You have to pay for atleast 10 days" + " Your short amount is " + shrt + "\n If you can pay, press YES " + "\n If not Press No", "For New Month", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes)
                            {
                                button4.Visible = false;
                                panel1.Visible = true;
                                label18.Visible = false;
                                label15.Visible = true;
                                button5.Visible = true;
                                label15.Text = shrt.ToString();
                            }

                        }
                        
                    }
                    ///////////testing above
                    if (df == 1)//for next month (jump to next month only)
                    {
                        if (afterPayAreas >= 3400)
                        {


                            gridFill(afterPayAreas);
                        }
                        else
                        {
                            int shortamt = 3400 - afterPayAreas;
                            DialogResult dr = MessageBox.Show("You have to pay for atleast 10 days" + " Your short amount is " + shortamt + "\n If you can pay, press YES " + "\n If not Press No", "For New Month", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes)
                            {
                                button4.Visible = false;
                                panel1.Visible = true;
                                label18.Visible = false;
                                label15.Visible = true;
                                button5.Visible = true;
                                label15.Text = shortamt.ToString();
                            }
                        }
                    }
                }
            }
           // textBox13.Text = textBox7.Text;
        }

        public void lastPayment()
        {
            textBox2.Text = "";
            dataGridView1.DataSource = "";
            if (textBox1.Text.Length == 3)
            {
                string taxiNo = "K" + textBox1.Text;

                try
                {
                  
                    DataSet ds = new DataSet();
                    DataTable dtLastPay = new DataTable();

                    MySqlConnection connection1 = new MySqlConnection(constr);
                    connection1.Open();
                    MySqlCommand command = connection1.CreateCommand();
                    command.CommandText = "select ReciptNo,ReciptDate from ReciptHeader where CabNo='" + taxiNo + "'"; ;
                    MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
                    newadp.Fill(ds);
                    dtLastPay = ds.Tables[0];
                    connection1.Close();

                    if (ds.Tables[0] != null)
                    {
                        dataGridView1.DataSource = dtLastPay;
                        if (dataGridView1.RowCount != 1)
                        {

                            DateTime lastPayemtDate = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[1].Value.ToString());
                            textBox2.Text = lastPayemtDate.ToShortDateString();
                            //dateTimePicker1.Value = lastPayemtDate.AddDays(1);
                            dateTimePicker1.Value = DateTime.Now;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Record not Found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        public int areasPhoneBill()
        {

            DataSet ds1 = new DataSet();
            DataTable dt1 = new DataTable();

            MySqlConnection connection1 = new MySqlConnection(constr);
            MySqlCommand command1 = connection1.CreateCommand();
            connection1.Open();
            command1.CommandText = "select count(*) from PayTest where CabNo='" + textBox1.Text + "'";
            int rv = Convert.ToInt32(command1.ExecuteScalar());
            if (rv != 0)
            {
                command1.CommandText = "select DueDate from PayTest where CabNo='" + textBox1.Text + "'";
                MySqlDataAdapter newadp = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)
                newadp.Fill(ds1);
                dt1 = ds1.Tables[0];
                connection1.Close();
                DateTime duedate = Convert.ToDateTime(dt1.Rows[0]["DueDate"]);
                //int areasMonth = dateTimePicker1.Value.Month - duedate.Month;
                int areasMonth =( DateTime.Now.Month - duedate.Month);
                if (areasMonth >= 0)
                {
                    textBox10.Text = areasMonth.ToString();
                }
                return (areasMonth) * 400;
            }
            else
            {
                textBox10.Text = "0";
                return 400;  // for this month    
                connection1.Close();
            }
            connection1.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            proceed = false;//*********

            amountPay = Convert.ToInt32(textBox5.Text);
            dataGridView2.Rows.Clear();
            textBox8.Text = "";
            textBox9.Text = "";
            try
            {
                afterPayAreas = amountPay - areasPhoneBill();//recovered the phone bill areas+this month payment from paid amount
                textBox6.Text = NumberToText(amountPay);//amount in words
                textBox11.Text = (Convert.ToInt32(textBox11.Text) + areasPhoneBill()).ToString();
            }
            catch (FormatException) { }
            if (afterPayAreas >= 300)
            {
                DateTime lastPayemtDate = Convert.ToDateTime(dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[1].Value.ToString());
                DateTime selectedDate = dateTimePicker1.Value;
                if (lastPayemtDate < selectedDate)
                {

                    gridFill(afterPayAreas);
                }
                else 
                {
                    if (afterPayAreas >= 3000) 
                    {
                        if (dateTimePicker1.Value.Month == dateTimePicker1.Value.AddDays(10).Month)
                        {
                            gridFill(afterPayAreas);
                        }
                        else 
                        {
                           MessageBox.Show("short amount is");
                        }
                    }
                }
            }

            textBox13.Text = textBox7.Text;
           //dataGridView2.Rows.Clear();
           //textBox8.Text = "";
           //textBox9.Text = "";
           //gridFill(afterPayAreas);
        }
        public void gridFill(int amount)  //amount means after deducted of areas from pay amount
         {
            mainAmount = amount;
            dataGridView2.Rows.Clear();

            try
            {
                if (textBox5.Text != "")
                {

                    firstBalance = amount % 300;
                    NoOfDays = (amount - firstBalance) / 300;

                    if (proceed == false)
                    {
                        textBox9.Text = firstBalance.ToString();
                    }
                    else if (proceed == true)
                    {
                        textBox9.Text = (amount % 300).ToString();
                             
                    }
                    textBox8.Text = NoOfDays.ToString();
                    DateTime strDate = dateTimePicker1.Value;                  


                    if (NoOfDays > 1)
                    {
                        dataGridView2.Rows.Add(NoOfDays-1);
                    }
                        dataGridView2.Rows[0].Cells[0].Value = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                        dataGridView2.Rows[0].Cells[1].Value = dateTimePicker1.Value.ToString("dddd");
                        dataGridView2.Rows[0].Cells[2].Value = "300";

                        dataGridView2.Rows[0].Cells[3].Style.Font = new Font("Wingdings", 15);
                        dataGridView2.Rows[0].Cells[3].Value = ((char)254).ToString();

                        //dataGridView2.Rows[0].Cells[3].Value = true;

                        if (NoOfDays > 1)
                        {                            
                            for (int r = 1; r < dataGridView2.RowCount; r++)
                            {
                                strDate = strDate.AddDays(1);
                                DateTime lastdate = strDate;  
                              
                                dataGridView2.Rows[r].Cells[0].Value = strDate.ToString("dd-MM-yyyy");
                                dataGridView2.Rows[r].Cells[1].Value = strDate.ToString("dddd");
                                dataGridView2.Rows[r].Cells[2].Value = "300";

                                dataGridView2.Rows[r].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dataGridView2.Rows[r].Cells[3].Value = ((char)254).ToString();

                            }
                           
                        }                        
                }
                else
                {
                    MessageBox.Show("Please Enter Amount");
                }
            }
            catch (ArgumentOutOfRangeException) { }


            if (proceed == false)
            {
                billTest(mainAmount);
            }
        }

        public string NumberToText(int number)
        {
            if (number == 0) return "Zero";
            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
                 "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
                "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
                "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = number % 1000; // units
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones
                t = num[i] / 10;
                h = num[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd()+" "+"Rupees onlly";
        }

     

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ////dataGridView2.Rows[dataGridView2.RowCount - 3].Cells[2].Style.Font = new Font("Wingdings",15);
            ////dataGridView2.Rows[dataGridView2.RowCount - 3].Cells[2].Value = ((char)254).ToString();
            //maxfreeDays = ((NoOfDays - (NoOfDays % 10)) / 10) * 3;//((char)0x254).ToString();
            ////maxfreeDays = ((((afterPayAreas - (afterPayAreas % 300)) / 300) / 10) - ((((afterPayAreas - (afterPayAreas % 300)) / 300) % 10))) * 3;
            ////maxfreeDays = ((((afterPayAreas - (afterPayAreas % 300)) / 300) / 10) - (((afterPayAreas - (afterPayAreas % 300)) / 300) % 10)) * 3;//////

            ////DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
            ////DataGridViewCheckBoxCell ch2 = new DataGridViewCheckBoxCell();
            ////dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Style.Font = new Font("Wingdings", 15);

            //if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value.Equals(((char)254).ToString()))
            //{
            //    dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value = ((char)253).ToString();
            //}
            //else if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value.Equals(((char)253).ToString()))
            //{
            //    dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();
            //}

        //    for (int i = 0; i < dataGridView2.RowCount - 1; i++) 
        //    {                  
        //         ch2= (DataGridViewCheckBoxCell)dataGridView2.Rows[i].Cells[3];
        //          if (ch2.Value.ToString() == "False")
        //         {
        //             freeDayCounter++;                     
        //         }
        //    }

           
        //ch1 = (DataGridViewCheckBoxCell)dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3];

        //       if (ch1.Value == null)
        //       ch1.Value = false;
        //        switch (ch1.Value.ToString())
        //        {
        //            case "True":
        //                if (freeDayCounter <=(maxfreeDays+1) )
        //                {
        //                    gridProcess(afterPayAreas);
        //                   ch1.Value = false;
        //                }
        //                else 
        //                {
        //                    ch1.Value = true;
        //                    MessageBox.Show("Now you are Exceeding free days");                           
        //                    dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value = true;                       
        //                }
        //                break;

        //            case "False":
        //                ch1.Value = true;
        //                int r = dataGridView2.RowCount;
        //                DateTime tempdate = Convert.ToDateTime(dataGridView2.Rows[r - 2].Cells[0].Value);
        //                dataGridView2.Rows.RemoveAt(r - 2);
        //                dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");                        

        //                break;
        //        }               
        }

        public void gridProcess(int amount) 
        {           

            if (dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value == null)
            {                
                int rowcnt = dataGridView2.RowCount;              
                DateTime tempdate = Convert.ToDateTime(dataGridView2.Rows[rowcnt - 2].Cells[0].Value);
                if (tempdate.ToString() != "01-01-0001 12:00:00 AM")
                {
                    if (tempdate.Month < (tempdate.AddDays(1)).Month)
                    {
                        int days=(amount-(amount%300))/300;
                        int remain=amount-((days-1)*300);
                        if (remain < 700)//cant pay 400 phone bill bcoz amount <700
                        {
                            int addpayment = 700 - remain;
                            DialogResult dr = MessageBox.Show("You have to pay " + addpayment + "\n If you can pay, press YES " + "\n If not Press No", "For New Month", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            if (dr == DialogResult.Yes)
                            {
                                button4.Visible = true;
                                panel1.Visible = true;
                                label18.Visible = false;
                                label15.Visible = true;
                                label15.Text = addpayment.ToString();
                                button5.Visible = false;
                                //if (f2.paid == true)
                                //{
                                //    f2.Hide();
                                //    textBox7.Text = (Convert.ToInt32(textBox7.Text) + 400).ToString();
                                //    textBox9.Text = "0";

                                //    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                                //    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                                //    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = "300";
                                //    dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Value = true;
                                //}
                                
                            }
                            else if (dr == DialogResult.No)
                            {                               
                                //
                                MessageBox.Show(""+dataGridView2.RowCount);
                                dataGridView2.Rows.RemoveAt(dataGridView2.RowCount-2);
                                dataGridView2.Rows[dataGridView2.RowCount - 3].Cells[3].Value = true;
                            }
                        }

                        else if (remain >= 700) //can pay phone bill bcoz amount>=700
                        {
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = "300";

                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                            dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Value = ((char)254).ToString();


                        }
                       
                    }
                    else //same month no problem
                    {
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = "300";
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                        dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Value = ((char)254).ToString();       
                    }
                        
                }
            }
            else
            {
                dataGridView2.Rows.Add(1);
                int rowcnt = dataGridView2.RowCount;
                DateTime tempdate = Convert.ToDateTime(dataGridView2.Rows[rowcnt - 1].Cells[0].Value);
                DateTime newday = tempdate.AddDays(1);

                if (tempdate.Month < (tempdate.AddDays(1)).Month)
                {
                    int days=(amount-(amount%300))/300;
                    int remain=amount-((days-1)*300);
                    if (remain < 700)//cant pay 400 phone bill bcoz amount <700
                    {
                        int addpayment = 700 - remain;

                        DialogResult dr = MessageBox.Show("You have to pay " + addpayment + "\n If you can pay, press YES " + "\n If not Press No", "For New Month", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                        if (dr == DialogResult.Yes)
                        {
                            button4.Visible = true;
                            panel1.Visible = true;
                            label18.Visible = true;
                            label15.Visible = false;
                            button5.Visible = false;
                            label18.Text = addpayment.ToString();

                            //dataGridView2.Rows[rowcnt - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                            //dataGridView2.Rows[rowcnt - 2].Cells[1].Value = tempdate.ToString("dddd");
                            //dataGridView2.Rows[rowcnt - 2].Cells[2].Value = "300";
                            //dataGridView2.Rows[rowcnt - 2].Cells[3].Value = true;

                            //dataGridView2.Rows[rowcnt - 1].Cells[0].Value = newday.ToString("dd-MM-yyyy");
                            //dataGridView2.Rows[rowcnt - 1].Cells[1].Value = newday.ToString("dddd");
                            //dataGridView2.Rows[rowcnt - 1].Cells[2].Value = "300";
                            //dataGridView2.Rows[rowcnt - 1].Cells[3].Value = true;
                        }
                        if (dr == DialogResult.No)
                        {
                            dataGridView2.Rows[rowcnt - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                            dataGridView2.Rows[rowcnt - 2].Cells[1].Value = tempdate.ToString("dddd");
                            dataGridView2.Rows[rowcnt - 2].Cells[2].Value = "300";

                            dataGridView2.Rows[rowcnt - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                            dataGridView2.Rows[rowcnt - 2].Cells[3].Value = ((char)254).ToString();

                            dataGridView2.Rows.RemoveAt(dataGridView2.RowCount - 2);
                        }                        
                    }
                    else if (remain >= 700) 
                    {
                        dataGridView2.Rows[rowcnt - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                        dataGridView2.Rows[rowcnt - 2].Cells[1].Value = tempdate.ToString("dddd");
                        dataGridView2.Rows[rowcnt - 2].Cells[2].Value = "300";

                        dataGridView2.Rows[rowcnt - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                        dataGridView2.Rows[rowcnt - 2].Cells[3].Value = ((char)254).ToString();

                        dataGridView2.Rows[rowcnt - 1].Cells[0].Value = newday.ToString("dd-MM-yyyy");
                        dataGridView2.Rows[rowcnt - 1].Cells[1].Value = newday.ToString("dddd");
                        dataGridView2.Rows[rowcnt - 1].Cells[2].Value = "300";

                        dataGridView2.Rows[rowcnt - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                        dataGridView2.Rows[rowcnt - 1].Cells[3].Value = ((char)254).ToString();
                    }
                }
                else //same month no problem
                {
                    dataGridView2.Rows[rowcnt - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                    dataGridView2.Rows[rowcnt - 2].Cells[1].Value = tempdate.ToString("dddd");
                    dataGridView2.Rows[rowcnt - 2].Cells[2].Value = "300";
                    dataGridView2.Rows[rowcnt - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                    dataGridView2.Rows[rowcnt - 2].Cells[3].Value = ((char)254).ToString();

                    dataGridView2.Rows[rowcnt - 1].Cells[0].Value = newday.ToString("dd-MM-yyyy");
                    dataGridView2.Rows[rowcnt - 1].Cells[1].Value = newday.ToString("dddd");
                    dataGridView2.Rows[rowcnt - 1].Cells[2].Value = "300";

                    dataGridView2.Rows[rowcnt - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                    dataGridView2.Rows[rowcnt - 1].Cells[3].Value = ((char)254).ToString();

                }
            } 

          //   DateTime startingDate;
             
          //   DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
          //   bool run = true;

          //for (int i = 0; (i < (dataGridView2.RowCount - 1)) && run == true; i++)
          //   {
          //       ch1 = (DataGridViewCheckBoxCell)dataGridView2.Rows[i].Cells[3];
          //       if (ch1.Value.ToString() == "True")
          //       {
          //           //MessageBox.Show("Checked" + i);
          //           run = false;
          //           startingDate = Convert.ToDateTime(dataGridView2.Rows[i].Cells[0].Value.ToString());
                     
                    

          //       }
                 
               // billTest(mainAmount);           

            textBox13.Text = textBox7.Text;
        }

        private void Recipt_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string billLastDate = (Convert.ToDateTime(dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value)).ToShortDateString();
            string billPayDate = DateTime.Now.ToShortDateString();
            int working = 0;

            for (int i = 0; i <= dataGridView2.RowCount - 1; i++)
            {
                if (dataGridView2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                {
                    working++;
                }
            }




            if (working == NoOfDays)
            {
                MySqlConnection connection1 = new MySqlConnection(constr);
                connection1.Open();
                MySqlCommand command = connection1.CreateCommand();

                for (int i = 0; i <= dataGridView2.RowCount - 1; i++)
                {
                    string date = (Convert.ToDateTime(dataGridView2.Rows[i].Cells[0].Value.ToString())).ToShortDateString();
                    int amount = Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value.ToString());

                    command.CommandText = "INSERT INTO TestPayment VALUES ('" + textBox1.Text + "' ,'" + textBox4.Text + "','" + date + "','300','0') ";
                    //command.CommandText = "DELETE from TestPayment Where CabNo='912'";
                    command.ExecuteNonQuery();
                }

                command.CommandText = "UPDATE PayTest SET DueDate= '" + billLastDate + "',PayDate='" + billPayDate + "' WHERE CabNo='" + textBox1.Text + "'";
                //command.CommandText = "UPDATE PayTest SET Amount='500' WHERE CabNo='912'";
                command.ExecuteNonQuery();

                //updating recipt header

                command.CommandText = "INSERT INTO TestReciptHeader VALUES ('" + textBox4.Text + "','" + DateTime.Now.ToShortDateString() + "','" + textBox5.Text + "','" + textBox1.Text + "','" + dateTimePicker1.Value.ToShortDateString() + "','" + billLastDate + "','" + NoOfDays + "','" + textBox6.Text + "','" + textBox13.Text + "','0','Test')";
                command.ExecuteNonQuery();

                connection1.Close();
                MessageBox.Show("Saved");

                {

                    DataSet1 recprint = new DataSet1();
                    MySqlConnection connection = new MySqlConnection(constr);
                    connection.Open();
                    MySqlCommand command1 = connection1.CreateCommand();
                   // command1.CommandText = "select * from TestReciptHeader where RecNo='" + textBox4.Text + "'"; ;
                   // MySqlDataAdapter newadp = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)
                   // newadp.Fill(recprint, "RecPrint");

 //ReciptNo
//ReciptAmount
//CabNo
//DateFrom
//DateTo
//nDays
//TotalAmountWord
//TotBillRecv
//Flag
//UserID

                    //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate, TestReciptHeader.ReciptAmount,TestReciptHeader,TestReciptHeader.CabNo,TestReciptHeader.DateFrom, TestReciptHeader.DateTo, TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord, TestReciptHeader.TotBillRecv,TestReciptHeader.Flag, TestReciptHeader.UserID,TestPayment.Date  FROM TestReciptHeader  FULL JOIN TestPayment  ON TestReciptHeader.RecNo=TestPayment.RecNo  WHERE TestReciptHeader.RecNo='" + textBox1.Text + "' ORDER BY TestReciptHeader.RecNo";


                    //command1.CommandText = "select * from TestPayment where CabNo='" + textBox1.Text + "'";

                    //command1.CommandText = "SELECT TestReciptHeader.RecNo, TestReciptHeader.nDays,TestPayment.Date  FROM TestReciptHeader FULL JOIN TestPayment  ON TestReciptHeader.RecNo=TestPayment.RecNo  ORDER BY TestReciptHeader.RecNo";

                    command1.CommandText="SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount,   TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord, TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID  FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestPayment.CabNo='912'  ORDER BY TestPayment.Date ";
                    MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
                    newadp1.Fill(recprint, "RecPrint");

                    connection.Close();

                    CrystalReport1 rpt1 = new CrystalReport1();
                    rpt1.SetDataSource(recprint);
                    Form3 f3 = new Form3();
                    f3.crystalReportViewer1.ReportSource = rpt1;
                    
                    f3.Show();   


                }
            }
            else
            {
                MessageBox.Show("Problem in your date selection, pleace check again");
            }
            
            
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            getReciptNo();           
        }
      
        public void getReciptNo()
        {
           
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select InvNo from Para";
                MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
                newadp.Fill(ds);
                dt = ds.Tables[0];
                connection.Close();
            
                int code = Convert.ToInt32(dt.Rows[0]["InvNo"]);
                code = code + 1;
                if (code <= 9)
                {
                    textBox4.Text = "RE00000" + code.ToString();
                }
                if (code >= 10 && code <= 99)
                {
                    textBox4.Text = "RE0000" + code.ToString();
                }
                if (code >= 100 && code <= 999)
                {
                    textBox4.Text = "RE000" + code.ToString();
                }
                if (code >= 1000 && code <= 9999)
                {
                    textBox4.Text = "RE00" + code.ToString();
                }
                if (code >= 10000 && code <= 99999)
                {
                    textBox4.Text = "RE0" + code.ToString();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void phoneBillPayment()
        {
            int paymonth = 0;
            int totalBill = 0;
            DateTime startdate;
            DateTime enddate;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "select count(*) from PayTest where CabNo='" + textBox1.Text + "'";

            startdate = dateTimePicker1.Value;


            int rv = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();

            if (rv == 0)//this is indicationg a new cab
            {
                enddate = Convert.ToDateTime(dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value.ToString());

                if (startdate.Year != enddate.Year)
                {
                    paymonth = ((12 - startdate.Month) + enddate.Month) + 1;// +1 for starting month peymant
                    totalBill = (paymonth) * 400;
                    textBox3.Text = totalBill.ToString();
                    textBox10.Text = (paymonth).ToString();
                }
                else
                {
                    paymonth = (enddate.Month - startdate.Month) + 1; // +1 for this month payement 
                    totalBill = (paymonth) * 400;
                    textBox3.Text = totalBill.ToString();
                    textBox10.Text = (paymonth).ToString();
                }
            }

            else //this is  indicating a bill paid cab
            {
                enddate = Convert.ToDateTime(dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value.ToString());
                if (startdate.Year != enddate.Year)
                {

                    paymonth = ((12 - startdate.Month) + enddate.Month) + 1;// +1 for starting month peymant                    
                    totalBill = (paymonth) * 400;
                    textBox3.Text = totalBill.ToString();
                    textBox10.Text = (paymonth).ToString();
                }
                else
                {
                    paymonth = (enddate.Month - startdate.Month) + 1; // +1 for this month payement                   
                    totalBill = (paymonth) * 400;
                    textBox3.Text = totalBill.ToString();
                    textBox10.Text = (paymonth).ToString();
                }
            }
        }

         
       

         private void textBox1_TextChanged(object sender, EventArgs e)
         {
             if (textBox1.Text.Length == 3)
             {
                 
                 lastPayment();               
                 textBox3.Text= areasPhoneBill().ToString();
             }
             textBox13.Text = textBox7.Text;
         }        

        
         public DateTime findFristWorkingDay() 
         {

             DateTime startingDate;
             
            // DataGridViewCheckBoxCell ch1 = new DataGridViewCheckBoxCell();
             bool run = true;

         outer: for (int i = 0; (i < (dataGridView2.RowCount - 1)) && run == true; i++)
             {
                 dataGridView2.Rows[i].Cells[3].Style.Font = new Font("Wingdings", 15);
                 //if (dataGridView2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                 //{
                 //    MessageBox.Show("OK");
                 //}


                // ch1 = (DataGridViewCheckBoxCell)dataGridView2.Rows[i].Cells[3];
                 //if (ch1.Value.ToString() == "True")
                 //{
                 if (dataGridView2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                 {
                     //MessageBox.Show("Checked" + i);
                     run = false;
                     startingDate = Convert.ToDateTime(dataGridView2.Rows[i].Cells[0].Value.ToString());
                     return startingDate;
                     goto outer;
                 }
                 else
                 {
                     return Convert.ToDateTime("01-01-0001 12:00:00 AM");
                 }
             }
         return Convert.ToDateTime("01-01-0001 12:00:00 AM");
           
         }


         public void billTest(int amount)
         {
             DateTime firstWorkingDate = findFristWorkingDay();
             DateTime lastWorkingDate = Convert.ToDateTime(dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value.ToString());
             int months = lastWorkingDate.Month - firstWorkingDate.Month;
             int newAmount = 0;
             int balance = 0;

             if (months > 0)
             {
                 int dateNumber = lastWorkingDate.Day;

                 if (dateNumber >= (months + 2))
                 {
                     newAmount = amount - (400 * months);
                     textBox13.Text = (Convert.ToInt32(textBox3.Text) + (400 * months)).ToString();
                     textBox7.Text= (400 * months).ToString();
                     proceed = true;
                     gridFill(newAmount);

                 }

                 else if (dateNumber < (months + 2))
                 {
                     if ((dateNumber * 300) - (400 * months) >= 300)
                     {
                         newAmount = amount - 400;

                         textBox13.Text = (Convert.ToInt32(textBox13.Text) + (400 * months)).ToString();
                         textBox7.Text = (400 * months).ToString();

                         proceed = true;
                         gridFill(newAmount);
                     }

                     else
                     {
                         balance = (400 * months) + 300 - (dateNumber * 300);

                         DialogResult dr = MessageBox.Show("Short amont is Rs " + balance + "\n If you can pay press YES " + "\n If not Press No", "Short amount of Phone Bill", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                         if (dr == DialogResult.Yes)
                         {
                             textBox11.Text = (Convert.ToInt32(textBox11.Text) + (400 * months)).ToString();
                             proceed = true;
                             newAmount = (amount - (months * 400)) + balance;
                             dataGridView2.DataSource = null;

                             textBox13.Text = (Convert.ToInt32(textBox13.Text) + (400 * months)).ToString();
                             textBox7.Text = (400 * months).ToString();

                             gridFill(newAmount);

                         }
                         else if (dr == DialogResult.No)
                         {
                             textBox9.Text = (Convert.ToInt32(textBox9.Text) + (dateNumber * 300)).ToString();
                             newAmount = (amount - (dateNumber * 300));
                             proceed = true;
                             gridFill(newAmount);

                         }
                         proceed = true;
                     }
                 }

             }
             else
             {
                 proceed = true;

             }
             proceed = true;
         }

         public void enteringnNewMonthCase1() 
         {
             int rowcnt = dataGridView2.RowCount;
             DateTime tempdate = Convert.ToDateTime(dataGridView2.Rows[rowcnt - 2].Cells[0].Value);

             
             textBox7.Text = (Convert.ToInt32(textBox7.Text) + 400).ToString();
             textBox9.Text = "0";

             dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
             dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
             dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[2].Value = "300";

             dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
             dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[3].Value = ((char)254).ToString();

         }
         public void enteringNewMonthCase2()
         {
             int rowcnt = dataGridView2.RowCount;
             DateTime tempdate = Convert.ToDateTime(dataGridView2.Rows[rowcnt - 1].Cells[0].Value);
             DateTime newday = tempdate.AddDays(1);

             dataGridView2.Rows[rowcnt - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
             dataGridView2.Rows[rowcnt - 2].Cells[1].Value = tempdate.ToString("dddd");
             dataGridView2.Rows[rowcnt - 2].Cells[2].Value = "300";
             dataGridView2.Rows[rowcnt - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
             dataGridView2.Rows[rowcnt - 2].Cells[3].Value = ((char)254).ToString();

             dataGridView2.Rows[rowcnt - 1].Cells[0].Value = newday.ToString("dd-MM-yyyy");
             dataGridView2.Rows[rowcnt - 1].Cells[1].Value = newday.ToString("dddd");
             dataGridView2.Rows[rowcnt - 1].Cells[2].Value = "300";
             dataGridView2.Rows[rowcnt - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
             dataGridView2.Rows[rowcnt - 1].Cells[3].Value = ((char)254).ToString();
         }

         private void button4_Click(object sender, EventArgs e)
         {
             if (Convert.ToInt32(label15.Text)==Convert.ToInt32(textBox14.Text))
             {
                 enteringnNewMonthCase1();
                 textBox5.Text = (Convert.ToInt32(textBox5.Text) + Convert.ToInt32(textBox14.Text)).ToString();
                 panel1.Visible = false;
             }
             else if (Convert.ToInt32(label18.Text) == Convert.ToInt32(textBox14.Text))
             {
                 enteringNewMonthCase2();
                 textBox5.Text = (Convert.ToInt32(textBox5.Text) + Convert.ToInt32(textBox14.Text)).ToString();
                 panel1.Visible = false;
             }
             else 
             {
                  MessageBox.Show("incorrect Amount");
             }
         }

         private void button3_Click(object sender, EventArgs e)
         {
            
         }

         private void button5_Click(object sender, EventArgs e)
         {
             if (Convert.ToInt32(label15.Text) == Convert.ToInt32(textBox14.Text))
             {
                 gridFill(afterPayAreas + Convert.ToInt32(textBox14.Text));
                 textBox5.Text = (Convert.ToInt32(textBox5.Text) + Convert.ToInt32(textBox14.Text)).ToString();
                 panel1.Visible = false;
                  afterPayAreas= afterPayAreas + Convert.ToInt32(textBox14.Text);
             }
             else 
             {
                 MessageBox.Show("incorrect Amount");

             }
         }

         private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
         {
             textBox13.Text = (Convert.ToInt32(textBox3.Text) + Convert.ToInt32(textBox7.Text)).ToString();
             freeDayCounter = 0;
             maxfreeDays = ((NoOfDays - (NoOfDays % 10)) / 10) * 3;//((char)0x254).ToString();

             for (int i = 0; i < dataGridView2.RowCount - 1; i++)
             {
                 if (dataGridView2.Rows[i].Cells[3].Value.Equals(((char)253).ToString()))
               
                 {
                     freeDayCounter++;
                 }
             }
             
             if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value.Equals(((char)254).ToString()))
             {
                 if (freeDayCounter < maxfreeDays)
                 {
                     dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value = ((char)253).ToString();
                     gridProcess(afterPayAreas);
                 }
                 else 
                 {
                     MessageBox.Show("You are Exceeding Free Days");
                 }
             }
             else if (dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value.Equals(((char)253).ToString()))
             {
                 
                 dataGridView2.Rows[dataGridView2.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();

                 int r = dataGridView2.RowCount;
                 DateTime tempdate = Convert.ToDateTime(dataGridView2.Rows[r - 2].Cells[0].Value);
                 dataGridView2.Rows.RemoveAt(r - 2);
                 dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");                        
             }
             textBox13.Text = (Convert.ToInt32(textBox3.Text) + Convert.ToInt32(textBox7.Text)).ToString();
         }

         private void toolStripButton3_Click(object sender, EventArgs e)
         {
             Form3 f3 = new Form3();
             f3.Show();   
             DataSet1 recprint = new DataSet1();
             MySqlConnection connection = new MySqlConnection(constr);
             connection.Open();
             MySqlCommand command1 = connection.CreateCommand();
             command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,  TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord,  TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID  FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestPayment.CabNo='912'  ORDER BY TestPayment.Date DESC ";
             MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
             newadp1.Fill(recprint, "RecPrint");

             connection.Close();

             CrystalReport1 rpt1 = new CrystalReport1();
             rpt1.SetDataSource(recprint);
            
             f3.crystalReportViewer1.ReportSource = rpt1;

             

         }

        
        
    }
}
