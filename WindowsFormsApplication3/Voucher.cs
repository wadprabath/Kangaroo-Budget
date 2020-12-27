using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;


namespace WindowsFormsApplication3
{
    class Voucher
    {
        private string constr1 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        private string constr2 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.Calling_numberConnectionString1"].ConnectionString;
        private string constr5 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CallCenterCityCabConnectionString"].ConnectionString;
        private string constr7 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.budgetConnectionString"].ConnectionString;
        private string constr8 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.bookingsConnectionString"].ConnectionString;
        private string constr9 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.accounts_reportsConnectionString"].ConnectionString;
        int targetIncomePerDay =0; double addComRate = 7.5;
        User us; Taxi t1;
        NewReceiptNumber nrcn;
        ReportsPrint Rpprint;

        public string get_Location()
        {
            return ConfigurationManager.AppSettings["Location"];
        }

        public void fillGrid(DateTime dtFrom,DateTime dtTo,DataGridView dgv1)
        {
          
              
            int dtcount = Convert.ToInt32((dtTo - dtFrom).TotalDays)+1;
            for (int i = 0; i < dtcount; i++) 
            {
                dgv1.Rows.Add();
                dgv1.Rows[i].Cells[0].Value = String.Format("{0:dd-MM-yyyy}", dtFrom.AddDays(i));
            }

        }

        public void getWorkingDays(string cabno, DateTime date, DataGridView dgv1, TextBox tbTotWorkDays, TextBox tbSumAmount, TextBox tbTarget, TextBox tbAddEarn, Control ctrl, TextBox tbYrear, TextBox tbMonth, TextBox tbPerDayLimit, CheckBox chbIgnoreAdd, TextBox tbPreDeduct, TextBox tbPreRefund, TextBox tbCurrentDeduct, TextBox tbCurrentRefund, TextBox tbTotAdditional, TextBox tbFinalDeduction, TextBox tbFinalRefund, TextBox tbNumOfVoucher) 
        {
            dgv1.Rows.Clear();
            //tbTotWorkDays.Text = "0"; tbSumAmount.Text = "0.00"; tbTarget.Text = "0.00"; tbAddEarn.Text = "0.00";
            //clear(ctrl);
            int year = date.Year;
            int month = date.Month;

            //if (month > 9)//this function should run from after 9th month of 2015, so I have used month!=9//  please remove this condition after 10th month
            //{

                if (chbIgnoreAdd.Checked == false)
                {
                    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    string cdate = ""; string wdate = "";
                    fillGrid(firstDayOfMonth, lastDayOfMonth, dgv1);

                    string fdm = String.Format("{0:yyyy-MM-dd}", firstDayOfMonth);
                    string ldm = String.Format("{0:yyyy-MM-dd}", lastDayOfMonth);

                    MySqlConnection connection = new MySqlConnection(constr2);
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT 	Date FROM CallingBdNo WHERE (CabNo='" + cabno + "') AND (Date BETWEEN '" + fdm + "'AND '" + ldm + "')";

                    using (var reader = command.ExecuteReader())
                    {
                        int i = 0;
                        while (reader.Read())
                        {
                            cdate = String.Format("{0:dd-MM-yyyy}", reader["Date"]);
                            for (int n = 0; n < dgv1.RowCount - 1; n++)
                            {
                                wdate = dgv1.Rows[n].Cells[0].Value.ToString();
                                if (wdate == cdate)
                                {
                                    dgv1.Rows[n].Cells[0].Style.BackColor = Color.Yellow;
                                }
                            }

                            ////dgv1.Rows.Add();
                            //dgv1.Rows[i].Cells[0].Value = String.Format("{0:dd-MM-yyyy}", reader["Date"]);//.ToString();                    
                            //i++;
                        }
                    }
                    connection.Close();
                    tbYrear.Text = year.ToString();
                    tbMonth.Text = month.ToString();
                    //getPreDeductionForNewDeduction(cabno, tbMonth, tbYrear, tbPreDeduct);
                    //getPreRefundForNewDeduction(cabno, tbMonth, tbYrear, tbPreRefund);


                    getVoucherAmountWithRef(cabno, fdm, ldm, dgv1, tbTotWorkDays, tbSumAmount, tbTarget, tbAddEarn, tbMonth, tbYrear, tbPerDayLimit, tbPreDeduct, tbPreRefund, tbCurrentDeduct, tbCurrentRefund, tbTotAdditional, tbFinalDeduction, tbFinalRefund, tbNumOfVoucher);
                }
            //}
        }

        public void getVoucherAmountWithRef(string cabno, string fromDate, string toDate, DataGridView dgv1, TextBox tbTotWorkDays, TextBox tbSumAmount, TextBox tbTarget, TextBox tbAddEarn, TextBox tbMonth, TextBox tbYear, TextBox tbPerDayLimit, TextBox tbPreDeduct, TextBox tbPreRefund, TextBox tbCurrentDeduct, TextBox tbCurrentRefund, TextBox tbTotAdditional, TextBox tbFinalDeduct, TextBox tbFinalRefund, TextBox tbNumOfVoucher) 
        {
            string cab = "K" + cabno;
            MySqlConnection connection = new MySqlConnection(constr1);           
            connection.Open();           
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT `VoucherDate`,`cabNo`,count(`voucherRefNo`) as vcount, sum(`VoucherAmount`) as totAmount FROM `TestRefVoucherPay` WHERE (`cabNo`='" + cab + "') and (`VoucherDate` between '" + fromDate + "' and '" + toDate + "') and (`cancel`!='Y') and (spFlag='N') group by `VoucherDate` ";
            
            using (var reader = command.ExecuteReader())
            {
                int i = 0; string vdate = ""; string wdate = "";
                while (reader.Read())
                {
                    vdate = String.Format("{0:dd-MM-yyyy}", reader["VoucherDate"]);
                    for (int n = 0; n < dgv1.RowCount - 1; n++)
                    {
                        wdate = dgv1.Rows[n].Cells[0].Value.ToString();
                        if (wdate == vdate)
                        {
                            dgv1.Rows[n].Cells[1].Value = reader["vcount"].ToString();
                            dgv1.Rows[n].Cells[2].Value = reader["totAmount"].ToString();
                        }
                    }


                    //vdate=String.Format("{0:dd-MM-yyyy}", reader["VoucherDate"]);
                    //wdate = dgv1.Rows[i].Cells[0].Value.ToString();
                    //if (wdate == vdate )
                    //{
                    //    dgv1.Rows[i].Cells[1].Value = reader["vcount"].ToString();
                    //    dgv1.Rows[i].Cells[2].Value = reader["totAmount"].ToString();
                    //}
                    //i++;
                }
            }
            connection.Close();
            getVouchertotAmntWithandNoRef(cabno, fromDate, toDate, dgv1, tbTotWorkDays, tbSumAmount, tbTarget, tbAddEarn, tbMonth, tbYear, tbPerDayLimit, tbPreDeduct, tbPreRefund, tbCurrentDeduct, tbCurrentRefund, tbTotAdditional, tbFinalDeduct, tbFinalRefund, tbNumOfVoucher);
        }

        public void getVouchertotAmntWithandNoRef(string cabno, string fromDate, string toDate, DataGridView dgv1, TextBox tbTotWorkDays, TextBox tbSumAmount, TextBox tbTarget, TextBox tbAddEarn, TextBox tbMonth, TextBox tbyear, TextBox tbPerDayLimit, TextBox tbPreDeduct, TextBox tbPreRefund, TextBox tbCurrentDeduct, TextBox tbCurrentRefund, TextBox tbTotAdditional, TextBox tbFinalDeduct, TextBox tbFinalRefund, TextBox tbNumOfVoucher)
        {
            string cab = "K" + cabno;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT `VoucherDate`,`cabNo`,count(`VoucherNo`) as vcount, sum(`VoucherAmount`) as totAmount FROM `TestNoRefVoucherPay` WHERE (`cabNo`='" + cabno + "' OR `cabNo`='" + cab + "') and (`VoucherDate` between '" + fromDate + "' and '" + toDate + "') and (`cancel`!='Y') and (spFlag='N') group by `VoucherDate` ";

            using (var reader = command.ExecuteReader())
            {
                string vdate = ""; string wdate = ""; 
                while (reader.Read())
                {
                    vdate = String.Format("{0:dd-MM-yyyy}", reader["VoucherDate"]);
                    for (int n = 0; n < dgv1.RowCount-1; n++)
                    {                       
                        wdate = dgv1.Rows[n].Cells[0].Value.ToString();
                        if (wdate == vdate)
                        {
                           
                                if (dgv1.Rows[n].Cells[1].Value == null)
                                    dgv1.Rows[n].Cells[1].Value = "0";
                                if (dgv1.Rows[n].Cells[2].Value==null)
                                    dgv1.Rows[n].Cells[2].Value = "0";

                                dgv1.Rows[n].Cells[1].Value = ((Convert.ToInt32(dgv1.Rows[n].Cells[1].Value.ToString()) + Convert.ToInt32(reader["vcount"].ToString()))).ToString();
                                dgv1.Rows[n].Cells[2].Value = ((Convert.ToInt32(dgv1.Rows[n].Cells[2].Value.ToString()) + Convert.ToInt32(reader["totAmount"].ToString()))).ToString();

                           
                            
                        }
                    } 
                }
            }
            connection.Close();



            countAll(dgv1, tbTotWorkDays, tbSumAmount, tbTarget, tbAddEarn, cabno, tbMonth, tbyear, tbPerDayLimit, tbPreDeduct, tbPreRefund, tbCurrentDeduct, tbCurrentRefund, tbTotAdditional, tbFinalDeduct, tbFinalRefund, tbNumOfVoucher);

        }

        public void countAll(DataGridView dgv1, TextBox tbTotWorkDays, TextBox tbSumAmount, TextBox tbTarget, TextBox tbAddEarn, string CabNo, TextBox tbMonth, TextBox tbYear, TextBox tbPerDayLimit, TextBox tbPreDuduct, TextBox tbPreRefund, TextBox tbCurrentDeduct, TextBox tbCurrentRefund, TextBox tbTotAdditional, TextBox tbFinalDeduct, TextBox tbFinalRefund,TextBox tbNumOfVoucher) 
        {
            targetIncomePerDay = getTargetIncomePerDay(tbMonth, tbYear);

            if (targetIncomePerDay > 0)
            {
                tbPerDayLimit.Text = targetIncomePerDay.ToString();

                int countDate = 0; int sumAmount = 0;
                for (int i = 0; i < dgv1.RowCount; i++)
                {
                    if (dgv1.Rows[i].Cells[0].Style.BackColor == Color.Yellow)
                        countDate++;
                    if (dgv1.Rows[i].Cells[0].Style.BackColor != Color.Yellow && dgv1.Rows[i].Cells[2].Value != null)//if someone worked without calling number
                        countDate++;

                    if (dgv1.Rows[i].Cells[2].Value != null)
                        sumAmount = sumAmount + Convert.ToInt32(dgv1.Rows[i].Cells[2].Value.ToString());
                }
                tbTotWorkDays.Text = countDate.ToString();
                tbSumAmount.Text = sumAmount.ToString();
                tbTarget.Text = (targetIncomePerDay * countDate).ToString();

                //if(Convert.ToInt32(tbNumOfVoucher.Text)>0)
                //callAdditionalEarningForNewDedctMethod(tbSumAmount, tbTarget, tbAddEarn, CabNo, tbMonth, tbYear, tbPreDuduct, tbPreRefund, tbCurrentDeduct, tbCurrentRefund, tbTotAdditional, tbFinalDeduct, tbFinalRefund);
                // comment for new Deduction method 27/07/2017
                //calAdditonalEarning(tbSumAmount, tbTarget, tbAddEarn, CabNo, tbMonth, tbYear,tbPreDuduct,tbPreRefund);
            }
        }

        public int getTargetIncomePerDay(TextBox tbMonth,TextBox tbYear) 
        {
            int amount = 0;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT amount FROM TestTarget WHERE (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "')";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        amount = Convert.ToInt32(reader["amount"].ToString());
                    }
                }
            }

            connection.Close();
            return amount;
        }

        public int getTargetAboveLimiFroMonth(TextBox tbMonth, TextBox tbYear)
        {
            int above = 0;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT Above FROM TestTarget WHERE (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "')";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        above = Convert.ToInt32(reader["Above"].ToString());
                    }
                }
            }

            connection.Close();
            return above;
        }

        //coment for new Deduction System 28/07/2017
        //public double calAdditonalEarning(TextBox tbTotalEarn, TextBox tbTargetEarn, TextBox tbAddtionalEarn, string CabNo, TextBox tbMonth, TextBox tbYear,TextBox tbPreDeduct,TextBox tbPreRefund) 
        //{
        //    double totEarn = 0.00; double targetEarn = 0.00; double addEarn = 0.00; double preAddEarn = 0.00; double earning = 0.00;

        //    totEarn=Convert.ToDouble(tbTotalEarn.Text); 
        //    targetEarn=Convert.ToDouble(tbTargetEarn.Text);
        //    preAddEarn = getPreAdditionalEarning(CabNo,tbMonth,tbYear,tbPreDeduct,tbPreRefund);

        //    double preDeduct = 0.00; double preRefund = 0.00; double refundDeductDiff=0.00;
        //    preDeduct = Convert.ToDouble(tbPreDeduct.Text);
        //    preRefund = Convert.ToDouble(tbPreRefund.Text);

        //    refundDeductDiff=preDeduct-preRefund;
        //    tbAddtionalEarn.Text = "0.00";

        //    if (targetEarn > 0.00)
        //    {
        //       earning= totEarn - preAddEarn;//in the same month, if there is a additional earning, to calculate additional earning, fist deduct it,

        //       if (earning > targetEarn)
        //        {
        //            addEarn = earning - targetEarn;
        //            tbAddtionalEarn.Text = addEarn.ToString();

        //        }
        //       else if (refundDeductDiff > 0)
        //       {
        //           if (totEarn > targetEarn)
        //           {
        //               addEarn = earning - targetEarn;
        //               if (addEarn > 0)
        //               {
        //                   tbAddtionalEarn.Text = addEarn.ToString();
        //               }
        //           }
        //       }
        //    }

        //    return Convert.ToDouble(tbAddtionalEarn.Text);
        //}

        //for new deduction method

        public void callAdditionalEarningForNewDedctMethod(TextBox tbTotalEarn, TextBox tbTargetEarn, TextBox tbAddtionalEarn, string CabNo, TextBox tbMonth, TextBox tbYear, TextBox tbPreDeduct, TextBox tbPreRefund, TextBox tbCurrentDeduct, TextBox tbCurrentRefund, TextBox tbTotDeduction, TextBox tbFinalDeduct, TextBox tbFinalRefund)
        {
            tbCurrentDeduct.Text = "0.00"; tbCurrentRefund.Text = "0.00"; tbAddtionalEarn.Text = "0.00"; tbTotDeduction.Text = "0.00"; tbFinalDeduct.Text = "0.00"; tbFinalRefund.Text = "0.00";
            if (Convert.ToDouble(tbTotalEarn.Text) > Convert.ToDouble(tbTargetEarn.Text))
            {
                tbAddtionalEarn.Text = (Convert.ToDouble(tbTotalEarn.Text) - Convert.ToDouble(tbTargetEarn.Text)).ToString();
            }
           
            getPreDeductionForNewDeduction(CabNo, tbMonth, tbYear, tbPreDeduct);
            getPreRefundForNewDeduction(CabNo, tbMonth, tbYear, tbPreRefund);
            calCurrentDeductionOrRefund(tbCurrentDeduct, tbCurrentRefund, tbTotDeduction, tbPreDeduct, tbPreRefund, tbFinalDeduct,tbFinalRefund);
        }

        //for New deduction method
        public void calCurrentDeductionOrRefund(TextBox tbCurrentDeduct, TextBox tbCurentRefund, TextBox tbTotalDeduct, TextBox tbPreviousDeduct, TextBox tbPreviousRefund, TextBox tbFinalDeduct,TextBox tbFinalRefund)
        {
            double currentDeduction = 0.00; double currentRefund = 0.00; double totalDeduction = 0.00; double preDeduction = 0.00; double preRefund = 0.00;
            double sum=0.00;
            totalDeduction = Convert.ToDouble(tbTotalDeduct.Text);preDeduction = Convert.ToDouble(tbPreviousDeduct.Text); preRefund = Convert.ToDouble(tbPreviousRefund.Text);
           
            sum=Convert.ToDouble(tbPreviousDeduct.Text)-Convert.ToDouble(tbPreviousRefund.Text);
            currentRefund = 0.00;


            if (totalDeduction > sum)
            {
                currentDeduction = totalDeduction - sum;
                tbCurrentDeduct.Text = currentDeduction.ToString();
                tbFinalDeduct.Text = currentDeduction.ToString();

            }
            else 
            {
                currentRefund = sum - totalDeduction;
                tbCurentRefund.Text = currentRefund.ToString();
                tbFinalRefund.Text = currentRefund.ToString();
            }
        }

        public double getPreAdditionalEarning(string CabNo, TextBox tbMonth, TextBox tbYear,TextBox tbPreDeduct,TextBox tbPreRefund)
        {
            double preAddEarn = 0.00; string cab = 'K' + CabNo;
            double preDeduction = 0.00; double preRefund = 0.00;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT deAmount,reAmount,addEarning FROM TestVoucherDeduct WHERE (cabNo='" + cab + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "')";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        preAddEarn = Convert.ToDouble(reader["addEarning"].ToString()); 
                        preDeduction=Convert.ToDouble(reader["deAmount"].ToString());
                        preRefund = Convert.ToDouble(reader["reAmount"].ToString()); 
                  
                    }
                }
            }

            connection.Close();
            tbPreDeduct.Text = preDeduction.ToString();
            tbPreRefund.Text = preRefund.ToString();
            
            return preAddEarn;
        }

        //for new deduction method
        public double getPreDeductionForNewDeduction(string cabNo, TextBox tbMonth,TextBox tbYear,TextBox tbPreDeduct)
        {
            string cab = "K" + cabNo;
            double preDeduct = 0.00;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT SUM(Deduction) as Deduction FROM TestNewDeductInfo WHERE (CabNo='" + cab + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "') AND (Flag=0)";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {                       
                        while (reader.Read())
                        {
                            if (reader["Deduction"] != DBNull.Value)
                            preDeduct = Convert.ToDouble(reader["Deduction"].ToString());
                        }
                    
                }
                
            }

            connection.Close();
            tbPreDeduct.Text = preDeduct.ToString();
            return preDeduct;
 
        }


        //for new deduction method
        public double getPreRefundForNewDeduction(string cabNo, TextBox tbMonth, TextBox tbYear, TextBox tbPreRefund)
        {
            string cab = "K" + cabNo;
            double preRefund = 0.00;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT SUM(Refund) as Refund FROM TestNewRefundInfo WHERE (CabNo='" + cab + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "') AND (Flag=0)";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["Refund"] != DBNull.Value)
                            preRefund = Convert.ToDouble(reader["Refund"].ToString());
                    }

                }

            }

            connection.Close();
            tbPreRefund.Text = preRefund.ToString();
            return preRefund;

        }

        public void calTotalVoucherAmount(DataGridView dgv6, TextBox tbNumOfVoucher, TextBox tbTotalAmount, TextBox tbAllCab, TextBox tbcabno, TextBox tbAddtionalEarning, TextBox tbDeduction, TextBox tbNetAmount,TextBox tbPreDeduct,TextBox tbPreRefund)
        {

            double amount = 0.00;
            int count = 0;

            string part1 = tbcabno.Text.Substring(0, 1);

            if (part1 != "K")
                tbcabno.Text = "K" + tbcabno.Text;

            for (int i = 1; i < dgv6.Rows.Count; i++)
            {
                if (dgv6.Rows[i].Cells[0].Value != null)
                {
                    try
                    {
                        amount = amount + Convert.ToDouble(dgv6.Rows[i].Cells[9].Value.ToString());
                        count++;
                    }
                    catch (Exception) { };
                    tbTotalAmount.Text = amount.ToString();
                    tbNumOfVoucher.Text = count.ToString();
                    tbAllCab.Text = tbcabno.Text;
                }
            }

            //coment for new Deduction Method 28/07/2017
           // commitionForAdditionalEarning(tbAddtionalEarning, tbTotalAmount, tbDeduction, tbNetAmount, tbPreDeduct, tbPreRefund);

            

        }
        
        //coment for new deduction method 28/07/2017
        //public void commitionForAdditionalEarning(TextBox tbAddtionalEarning,TextBox tbVoucherTotal,TextBox tbDeduction,TextBox tbNetAmount,TextBox tbPreDeduct,TextBox tbPreRefund)
        //{
            

        //    double dedction=0.00; double netAmount=0.00;

        //    double preDeduct = 0.00; double preRefund = 0.00;

        //    preDeduct = Convert.ToDouble(tbPreDeduct.Text);
        //    preRefund = Convert.ToDouble(tbPreRefund.Text);

        //    if (tbVoucherTotal.TextLength > 1)
        //    {
               
        //        dedction = ((Convert.ToDouble(tbAddtionalEarning.Text) / 100) * addComRate);

        //        if (dedction > 0)
        //        {
        //            if (preRefund == 0)
        //            {
        //                dedction = dedction;
        //            }
        //            else if (preRefund < preDeduct)//edited 16 05 2017
        //            {
        //                dedction = dedction;
        //            }
        //        }

        //            netAmount = Convert.ToDouble(tbVoucherTotal.Text) - dedction;
        //    }

        //    tbDeduction.Text=dedction.ToString();
        //    //tbNetAmount.Text = netAmount.ToString();
        //}

        public void decideRefund(TextBox tbCabNo, TextBox tbTotalearning, TextBox tbTarget, TextBox tbRefund, TextBox tbMonth, TextBox tbYear,TextBox tbDeductref,TextBox tbDeduct,CheckBox chbFullRefund, TextBox tbPreDeduct, TextBox tbPreRefund) 
        {
            double totEarn = Convert.ToDouble(tbTotalearning.Text); double target = Convert.ToDouble(tbTarget.Text);
            double dAmount=Convert.ToDouble(tbDeduct.Text);
            double deduction = getDeduction(tbCabNo, tbMonth, tbYear,tbDeductref); double refundAmount = 0.00;
            double currentRefund = 0.00;
            chbFullRefund.Checked = false;

            double preDeduct = 0.00; double preRefund = 0.00; double refundDeductDiff = 0.00;
            preDeduct = Convert.ToDouble(tbPreDeduct.Text);
            preRefund = Convert.ToDouble(tbPreRefund.Text);

            refundDeductDiff = preDeduct - preRefund;

            if (totEarn < target)
            {
                if (deduction > 0.00)
                {
                    //double dif = target - totEarn;

                    //if (dif <= deduction)
                    //    refundAmount = dif;
                    //else if (dif > deduction)
                        refundAmount = deduction;
                        chbFullRefund.Checked = true;
                }
            }
            else // refund before low limit, if there is no deduction at the moment
            {
                if (refundDeductDiff > 0.00) //if there is sum deducted amount
                {
                    if (dAmount == 0.00)
                    {
                        currentRefund = deduction - (((totEarn - target) / 100) * 7.5);
                        if (currentRefund > 0)
                        {
                            refundAmount = currentRefund;
                            chbFullRefund.Checked = false;
                        }
                    }
                }
            }
            tbRefund.Text = refundAmount.ToString();

            //decideSpecialRefund(tbCabNo, tbTotalearning, tbTarget, tbRefund, tbMonth, tbYear);
                
        }

        //public void decideSpecialRefund(TextBox tbCabNo, TextBox tbTotalearning, TextBox tbTarget, TextBox tbRefund, TextBox tbMonth, TextBox tbYear) 
        //{
        //    double preAddEarn = 0.00; double totEarn = 0.00; double targetEarn = 0.00; double currentAddEarn = 0.00;

        //    totEarn = Convert.ToDouble(tbTotalearning.Text);
        //    targetEarn = Convert.ToDouble(tbTarget.Text);

        //    preAddEarn = getPreAdditionalEarning(tbCabNo.Text, tbMonth, tbYear);
        //    currentAddEarn = totEarn - targetEarn;

        //    if (currentAddEarn < preAddEarn)
        //    {
        //        tbRefund.Text = (((preAddEarn - currentAddEarn) / 100) * 7.5).ToString();
        //    }
        //    else
        //        tbRefund.Text = "0.00";


        //}


        public void saveNewDeduction(string recno, TextBox tbCabno, TextBox tbDeduction, TextBox tbRefund, TextBox tbTotDays, TextBox tbTotEarning, TextBox tbMonth, TextBox tbYear, TextBox tbTarget, TextBox tbAddEarn, TextBox tbPerDayLimit)
        {
            us = new User();
            string user = us.getCurrentUser();
            string location = get_Location();
            string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);
            string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "INSERT INTO TestNewDeductInfo(VrecNo,CabNo,Month,Year,TotIncome,NoOfDays,Target,AddIncome,Deduction,Date,DateTime,User,Location,Flag) VALUES('"+recno+"','"+tbCabno.Text+"','"+tbMonth.Text+"','"+tbYear.Text+"','"+tbTotEarning.Text+"','"+tbTotDays.Text+"','"+tbTarget.Text+"','"+tbAddEarn.Text+"','"+tbDeduction.Text+"','"+date+"','"+datetime+"','"+user+"','"+location+"','0')";
            command1.ExecuteNonQuery();
            connection.Close();

        }


        public void saveNewRefund(string recno, TextBox tbCabno, TextBox tbDeduction, TextBox tbRefund, TextBox tbTotDays, TextBox tbTotEarning, TextBox tbMonth, TextBox tbYear, TextBox tbTarget, TextBox tbAddEarn, TextBox tbPerDayLimit)
        {
            us = new User();
            string user = us.getCurrentUser();
            string location = get_Location();
            string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);
            string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "INSERT INTO TestNewRefundInfo(VrecNo,CabNo,Month,Year,TotIncome,NoOfDays,Target,Refund,Date,DateTime,User,Location,Flag) VALUES('" + recno + "','" + tbCabno.Text + "','" + tbMonth.Text + "','" + tbYear.Text + "','" + tbTotEarning.Text + "','" + tbTotDays.Text + "','" + tbTarget.Text + "','" + tbRefund.Text + "','" + date + "','" + datetime + "','" + user + "','" + location + "','0')";
            command1.ExecuteNonQuery();
            connection.Close();

        }

        public void SaveNewDeductReceipt(string recno, TextBox tbCabNo,TextBox tbMonth, TextBox tbYear, TextBox tbCreditLimit, TextBox tbNumDays,TextBox tbTotEarn,TextBox tbTarget,TextBox tbAddEarn,TextBox tbTotDeduct,TextBox tbPreDeduct,TextBox tbPreRefund,TextBox tbCurrentDeduct,TextBox tbCurrentRefund,Label lbUser,Label lbDateTime,Label lbCabNo) 
        {
            us = new User();
            string user = us.getCurrentUser();
            string location = get_Location();
            string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);
            string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            lbCabNo.Text = tbCabNo.Text; lbUser.Text = user; lbDateTime.Text = datetime;

            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "INSERT INTO TestNewDeductReceipt(RecNo,CabNo,Month,Year,CreditLimit,NumOfDays,TotEarning,Target,AddEarning,TotDeduct,PreDeduct,PreRefund,CurrentDeduct,CurrentRefund,User,Date,DateTime,Location,Flag) VALUES ('"+recno+"','"+tbCabNo.Text+"','"+tbMonth.Text+"','"+tbYear.Text+"','"+tbCreditLimit.Text+"','"+tbNumDays.Text+"','"+tbTotEarn.Text+"','"+tbTarget.Text+"','"+tbAddEarn.Text+"','"+tbTotDeduct.Text+"','"+tbPreDeduct+"','"+tbPreRefund.Text+"','"+tbCurrentDeduct.Text+"','"+tbCurrentRefund.Text+"','"+user+"','"+date+"','"+datetime+"','"+location+"','0')";
            command1.ExecuteNonQuery();
            connection.Close();
        }

        public void findNewDeductReceipt(TextBox tbRecno, Label LbCabNo, TextBox tbMonth, TextBox tbYear, TextBox tbCreditLimit, TextBox tbNumDays, TextBox tbTotEarn, TextBox tbTarget, TextBox tbAddEarn, TextBox tbTotDeduct, TextBox tbPreDeduct, TextBox tbPreRefund, TextBox tbCurrentDeduct, TextBox tbCurrentRefund,Label LbUser, Label LbDateTime)
        {
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM TestNewDeductReceipt WHERE RecNo='"+tbRecno.Text+"' AND Flag='0'";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["RecNo"] != DBNull.Value)
                            tbRecno.Text = reader["RecNo"].ToString();
                            LbCabNo.Text = reader["CabNo"].ToString();
                            tbMonth.Text = reader["Month"].ToString();
                            tbYear.Text = reader["Year"].ToString();
                            tbCreditLimit.Text = reader["CreditLimit"].ToString();
                            tbNumDays.Text = reader["NumOfDays"].ToString();
                            tbTotEarn.Text = reader["TotEarning"].ToString();
                            tbTarget.Text = reader["Target"].ToString();
                            tbAddEarn.Text = reader["AddEarning"].ToString();
                            tbTotDeduct.Text = reader["TotDeduct"].ToString();
                            tbPreDeduct.Text = reader["PreDeduct"].ToString();
                            tbPreRefund.Text = reader["PreRefund"].ToString();
                            tbCurrentDeduct.Text = reader["CurrentDeduct"].ToString();
                            tbCurrentRefund.Text = reader["CurrentRefund"].ToString();
                            LbUser.Text = reader["User"].ToString();
                            LbDateTime.Text = reader["DateTime"].ToString();
                           

                            
                    }

                }

            }

            connection.Close();

        }


        public void saveDeduction(string recno, TextBox tbCabno, TextBox tbDeduction, TextBox tbRefund, TextBox tbTotDays, TextBox tbTotEarning, TextBox tbMonth, TextBox tbYear, TextBox tbTarget,TextBox tbAddEarn,TextBox tbPerDayLimit)
        {
            //string part1 = tbCabno.Text.Substring(1, 1);

            //if (part1 != "K")
            //    tbCabno.Text = "K" + tbCabno.Text;
            string location = get_Location();

            double preDeduct = 0.00; double deduction = 0.00; double preAddEarn = 0.00; double additional=0.00;

            us = new User();
            string user = us.getCurrentUser();
            string date = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);


            deduction = Convert.ToDouble(tbDeduction.Text);
            additional=   Convert.ToDouble(tbAddEarn.Text);

            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
        
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            MySqlCommand command3 = connection.CreateCommand();
            MySqlCommand command4 = connection.CreateCommand();

            command2.CommandText = "SELECT deAmount,totDays,totEarning,deTarget,addEarning,month,year FROM TestVoucherDeduct WHERE (cabNo='" + tbCabno.Text + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "')";

            using (var reader = command2.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    preDeduct = Convert.ToDouble(reader["deAmount"].ToString());
                    preAddEarn = Convert.ToDouble(reader["addEarning"].ToString());
                    connection.Close();

                    connection.Open();
                    command1.CommandText = "UPDATE TestVoucherDeduct SET deAmount='" + (deduction + preDeduct) + "',totDays='" + tbTotDays.Text + "',perDayLimit='"+tbPerDayLimit.Text+"',deTarget='" + tbTarget.Text + "',totEarning='" + tbTotEarning.Text + "',addEarning='" + (additional + preAddEarn) + "',month='" + tbMonth.Text + "',year='" + tbYear.Text + "',deDate='" + date + "',user='" + user + "',cancel='N' WHERE (cabNo='" + tbCabno.Text + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "')";
                    command1.ExecuteNonQuery();
               
                }
                else
                {
                    connection.Close();

                    connection.Open();
                    command1.CommandText = "INSERT INTO TestVoucherDeduct(vrecno,cabNo,deAmount,totDays,perDayLimit,deTarget,totEarning,addEarning,month,year,deDate,user,cancel) VALUES('" + recno + "','" + tbCabno.Text + "','" + tbDeduction.Text + "','" + tbTotDays.Text + "','"+tbPerDayLimit.Text+"','" + tbTarget.Text + "','" + tbTotEarning.Text + "','" + tbAddEarn.Text + "','" + tbMonth.Text + "','" + tbYear.Text + "','" + date + "','" + user + "','N')";
                    command1.ExecuteNonQuery();
                //    connection.Close();
                }
            }
            //connection.Close();

            
            ////insert into detail deduct table
            //connection.Open();
            command3.CommandText = "INSERT INTO TestDeductInfo(vrecno,cabNo,deAmount,totDays,perDayLimit,deTarget,totEarning,month,year,deDate,user,cancel,location) VALUES('" + recno + "','" + tbCabno.Text + "','" + tbDeduction.Text + "','" + tbTotDays.Text + "','" + tbPerDayLimit.Text + "','" + tbTarget.Text + "','" + tbTotEarning.Text + "','" + tbMonth.Text + "','" + tbYear.Text + "','" + date + "','" + user + "','N','"+location+"')";
            command3.ExecuteNonQuery();
            //connection.Close();

            //insert into general Daily information
            //connection.Open();
            command4.CommandText = "INSERT INTO TestDeReInfo(vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,month,year,user,cancel,location) VALUES('" + recno + "','" + tbCabno.Text + "','" + tbDeduction.Text + "','0.00','" + tbTotDays.Text + "', '" + tbPerDayLimit.Text + "','" + tbTarget.Text + "','" + tbTotEarning.Text + "', '" + date + "','" + tbMonth.Text + "','" + tbYear.Text + "','" + user + "','N','"+location+"')";
            command4.ExecuteNonQuery();

            connection.Close();
        }

        public void saveRefund(string recno, TextBox tbCabno, TextBox tbRefund, TextBox tbYaer, TextBox tbMonth, TextBox tbTarget, TextBox tbDeductRef, TextBox tbTotDays, TextBox tbTotEarn, TextBox tbPerDayLimit,CheckBox chbFullREfund, TextBox tbAddEarning)
        {
            string location = get_Location();

            us = new User();
            string user = us.getCurrentUser();

            double lastRefund = 0.00; double totRefund=0.00;
            double updatedAddEarn = 0.00;

            if (chbFullREfund.Checked == true)
                updatedAddEarn = 0.00;
            else
            {
                updatedAddEarn = Convert.ToDouble(tbTotEarn.Text) - Convert.ToDouble(tbTarget.Text);
            }

            lastRefund=getRefund(tbCabno, tbMonth, tbYaer);

            totRefund = lastRefund + Convert.ToDouble(tbRefund.Text);

            string date = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            MySqlCommand command3 = connection.CreateCommand();

            command1.CommandText = "UPDATE TestVoucherDeduct SET reAmount='" + totRefund + "',totDays='" + tbTotDays.Text + "',perDayLimit='" + tbPerDayLimit.Text + "' ,totEarning='" + tbTotEarn.Text + "',reDate='" + date + "',reTarget='" + tbTarget.Text + "',addEarning='"+updatedAddEarn+"' WHERE (cabNo='" + tbCabno.Text + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYaer.Text + "')";
            command1.ExecuteNonQuery();
            //connection.Close();

            //connection.Open();
            command2.CommandText = "INSERT INTO TestRefundInfo(vrecno,cabNo,reAmount,deductRecNo,totDays,perDayLimit,totEarning,reTarget,month,year,reDate,user,cancel,location) VALUES('" + recno + "','" + tbCabno.Text + "','" + tbRefund.Text + "','" + tbDeductRef.Text + "','" + tbTotDays.Text + "', '" + tbPerDayLimit.Text + "','" + tbTotEarn.Text + "','" + tbTarget.Text + "','" + tbMonth.Text + "','" + tbYaer.Text + "','" + date + "','" + user + "','N','"+location+"')";
            command2.ExecuteNonQuery();
            //connection.Close();


            //connection.Open();
            command3.CommandText = "INSERT INTO TestDeReInfo(vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,month,year,user,cancel,location) VALUES('" + recno + "','" + tbCabno.Text + "','0.00','" + tbRefund.Text + "','" + tbTotDays.Text + "','" + tbPerDayLimit.Text + "','" + tbTarget.Text + "','" + tbTotEarn.Text + "',  '" + date + "','" + tbMonth.Text + "','" + tbYaer.Text + "','" + user + "','N','"+location+"')";
            command3.ExecuteNonQuery();
            connection.Close();
        }

        public double getDeduction(TextBox tbCabNo, TextBox tbMonth,TextBox tbYear,TextBox tbDeductRef) 
        {
            double damount = 0.00; double ramount = 0.00; string cab = 'K' + tbCabNo.Text;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT deAmount,reAmount,vrecno FROM TestVoucherDeduct WHERE (cabNo='" + cab + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "')";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {   
                        damount = Convert.ToDouble(reader["deAmount"].ToString());
                        ramount = Convert.ToDouble(reader["reAmount"].ToString());
                        tbDeductRef.Text = reader["vrecno"].ToString();
                    }
                }
            }

            connection.Close();
            return damount-ramount;
        }

        public double getRefund(TextBox tbCabNo, TextBox tbMonth, TextBox tbYear)//get recently refunded amount
        {
            double reAmount = 0.00; string cab = tbCabNo.Text;
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT reAmount FROM TestVoucherDeduct WHERE (cabNo='" + cab + "') AND (month='" + tbMonth.Text + "' AND year='" + tbYear.Text + "')";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                       
                        reAmount = Convert.ToDouble(reader["reAmount"].ToString());
                       
                    }
                }
            }

            connection.Close();
            return reAmount;
            
        }

        public void clear(Control control) 
        {
       
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text="0.00";
                }
                //if (c is DataGridView)
                //{
                //    ((DataGridView)c).DataSource = null;
                //}
                if (c.HasChildren)
                {
                    clear(c);
                }
            }
        }

        //public void printVoucher(DataGridView dgv1,TextBox tbNoOfVoucher,TextBox tbVoucherAmount,TextBox tbDeduction,TextBox tbRefund,TextBox tbNetAmount,TextBox tbCabno,TextBox tbNic) 
        //{
          

        //    DataSet1 ds = new DataSet1();
        //    DataTable dt = new DataTable();
        //    dt = ds.VoucherPrint;
        //    DataRow newRow;

        //    for (int n = 1; n < dgv1.RowCount; n++)
        //    {
        //        if (dgv1.Rows[n].Cells[0].Value != null)
        //        {
        //            newRow = ds.VoucherPrint.NewRow();

        //            newRow[0] = dgv1.Rows[n].Cells[0].Value.ToString();
        //            newRow[1] = dgv1.Rows[n].Cells[1].Value.ToString();
                  
        //            dt.Rows.Add(newRow);
        //        }
        //    }


        //    Form3 f3 = new Form3();
        //    CryVoucherReceipt rpt = new CryVoucherReceipt();
        //    TextObject txtNic = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
        //    TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtCabno = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
        //    TextObject txtDate = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
        //    TextObject txtNoOfVouchers = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];
        //    TextObject txtVoucherTotal = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
        //    TextObject txtDeduction = (TextObject)rpt.ReportDefinition.ReportObjects["Text30"];
        //    TextObject txtRefund= (TextObject)rpt.ReportDefinition.ReportObjects["Text31"];
        //    TextObject txtNetAmount = (TextObject)rpt.ReportDefinition.ReportObjects["Text32"];


        //    txtNic.Text = tbNic.Text;
        //    //txtuser.Text = user;
        //    txtCabno.Text = tbCabno.Text;
        //    txtDate.Text = String.Format("{0:dd-MM-yyyy}", DateTime.Now);
        //    txtNoOfVouchers.Text = tbNoOfVoucher.Text;
        //    txtVoucherTotal.Text = tbVoucherAmount.Text;
        //    txtDeduction.Text = tbDeduction.Text;
        //    txtRefund.Text = tbRefund.Text;
        //    txtNetAmount.Text = tbNetAmount.Text;
            

        //    rpt.SetDataSource(ds);
        //    f3.crystalReportViewer1.ReportSource = rpt;
        //    f3.Show();
        //    rpt.PrintToPrinter(1, false, 1, 1);

        //}

        //public string getVoucherRecNo() 
        //{
          
        //    string receiptCode = "";

        //    try
        //    {

        //        System.Data.DataSet ds = new System.Data.DataSet();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        MySqlConnection connection = new MySqlConnection(constr1);
        //        connection.Open();
        //        MySqlCommand command = connection.CreateCommand();
        //        command.CommandText = "select VoucherRecNo from TestPara";
        //        MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
        //        newadp.Fill(ds);
        //        dt = ds.Tables[0];
        //        connection.Close();

        //        int code = Convert.ToInt32(dt.Rows[0]["VoucherRecNo"]);
        //        code = code + 1;
        //        if (code <= 9)
        //        {
        //            return receiptCode = "VR00000" + code.ToString();
        //        }
        //        if (code >= 10 && code <= 99)
        //        {
        //            return receiptCode = "VR0000" + code.ToString();
        //        }
        //        if (code >= 100 && code <= 999)
        //        {
        //            return receiptCode = "VR000" + code.ToString();
        //        }
        //        if (code >= 1000 && code <= 9999)
        //        {
        //            return receiptCode = "VR00" + code.ToString();
        //        }
        //        if (code >= 10000 && code <= 99999)
        //        {
        //            return receiptCode = "VR0" + code.ToString();
        //        }
        //        return receiptCode = "----";
        //    }
        //    catch (Exception ex)
        //    {
        //        return receiptCode = "----";
        //    }
        //}

        //public void updateRecNo(string vrecno) 
        //{
        //    string[] split = vrecno.Split(new string[] { "VR" }, StringSplitOptions.RemoveEmptyEntries);
        //    int recno = Convert.ToInt32(split[0]);
        //    MySqlConnection connection = new MySqlConnection(constr1);
        //    connection.Open();
        //    MySqlCommand command = connection.CreateCommand();
        //    command.CommandText = "UPDATE TestPara SET VoucherRecNo='" + recno + "' WHERE ID=0";
        //    command.ExecuteNonQuery();
        //    connection.Close();
        //}

        public void SaveVoucherHireRef(DataGridView dgv6, TextBox tbCabNo,TextBox tbNic, TextBox tbTotDays, TextBox tbTotEarning, TextBox tbDeduction, TextBox tbRefund,TextBox tbMonth ,TextBox tbYear,TextBox tbTarget,TextBox tbDeductref,TextBox tbAddEarn,TextBox tbName,TextBox tbPerDayLimit,CheckBox chbSpecial,DataGridView dgv10,TextBox tbPhonebill,Label lbMobileLoanNo,TextBox tbMobileLOan,CheckBox chbFullRefund,TextBox addEarning ,TextBox tbAppPhone,TextBox tbFindRecNo,TextBox tbCurrntDeduction,TextBox tbCurrentRefund,string deposit,TextBox tbNetAmount)
        {
            nrcn = new NewReceiptNumber();

            //DialogResult dr = MessageBox.Show("Are You Sure Want To Pay", "Confirm", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{

                us = new User();
                t1 = new Taxi();

                string user = us.getCurrentUser();
                string branchCode = us.getBranchCode();
                string branchName = us.getBranchName();

                //string vloc = t1.EnteredLocation();
                string vloc = get_Location();

                string recno = nrcn.getVoucherRecNo();
                tbFindRecNo.Text = recno;

                string spflag="N";

                if (chbSpecial.Checked == true)
                    spflag = "Y";
                else if (chbSpecial.Checked==false)
                    spflag = "N";

                nrcn.updateVoucherRecNo(recno);

                MySqlConnection connection = new MySqlConnection(constr1);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                MySqlCommand command2 = connection.CreateCommand();


                //SqlConnection con = new SqlConnection(constr5);//call center DB table update
                //con.Open();
                //SqlCommand cmd = con.CreateCommand();

                for (int i = 1; i < dgv6.Rows.Count; i++)
                {
                    if (dgv6.Rows[i].Cells[0].Value != null)
                    {

                        string cab = dgv6.Rows[i].Cells[0].Value.ToString();
                        string voucherdate = dgv6.Rows[i].Cells[1].Value.ToString();
                        string refno = dgv6.Rows[i].Cells[2].Value.ToString();

                        //..convert reference number to ascii....//
                        char char1= Convert.ToChar(refno.Substring(0, 1));
                        char char2 = Convert.ToChar(refno.Substring(1, 1));
                        string str = refno.Substring(2, 4);                        

                        string ascii1=((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
                        string asciiRefNo = ascii1 + ascii2 + str;
                        //..end of ascii convertion...........//

                        string company = dgv6.Rows[i].Cells[3].Value.ToString();
                        string paytype = dgv6.Rows[i].Cells[4].Value.ToString();
                        string vno = dgv6.Rows[i].Cells[5].Value.ToString();
                        double vamount = Convert.ToDouble(dgv6.Rows[i].Cells[6].Value.ToString());
                        double comrate = Convert.ToDouble(dgv6.Rows[i].Cells[7].Value.ToString());
                        double com = Convert.ToDouble(dgv6.Rows[i].Cells[8].Value.ToString());
                        double bal = Convert.ToDouble(dgv6.Rows[i].Cells[9].Value.ToString());
                        double appAmnt = Convert.ToDouble(dgv6.Rows[i].Cells[10].Value.ToString());

                        string exComm = dgv6.Rows[i].Cells[12].Value.ToString();

                        string ss = cab.Substring(1);
                        if (dgv6.Rows[i].Cells[2].Value.ToString() != "XXXXXX")//With reference number
                       // if (dgv6.Rows[i].Cells[11].Value.ToString() == "Y")
                        {
                            command.CommandText = "INSERT INTO TestRefVoucherPay (vrecno,cabNo,Cnumber,VoucherDate,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,branchCode,branchName,Location,spFlag,exCom)  VALUES('" + recno + "','" + cab + "','','" + voucherdate + "','" + refno + "','" + company + "','" + paytype + "','" + vno + "','" + appAmnt + "','" + vamount + "','" + comrate + "','" + com + "','" + bal + "','" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "', '" + String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now) + "','" + user + "', '"+branchCode+"','"+branchName+"', '" + vloc + "','" + spflag + "','"+exComm+"')";
                            command.ExecuteNonQuery();

                            //cmd.CommandText = "UPDATE VoucherRef SET paid=1 WHERE (CabNo='" + cab + "' AND voucherDate='" + voucherdate + "') AND voucherRefNo= '" + recno + "'  ";
                            //cmd.CommandText = "UPDATE Job SET Paid=1 WHERE (CabNo='" + cab.Substring(1) + "') AND (CAST(DispatchTime AS DATE)='" + voucherdate + "') AND (JobID = '" + asciiRefNo + "')";
                            //cmd.ExecuteNonQuery();

                            updateLogsheet(refno, tbCabNo,recno); 
                            updateDespatchBooking(refno, tbCabNo,recno); 
                        }
                        else if (dgv6.Rows[i].Cells[2].Value.ToString() == "XXXXXX") //without reference number
                         //else if (dgv6.Rows[i].Cells[11].Value.ToString() == "N")
                       
                        {
                            command2.CommandText = "INSERT INTO TestNoRefVoucherPay (vrecno,cabNo,Cnumber,VoucherDate,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,branchCode,branchName,Location,spFlag,exCom) VALUES('" + recno + "','" + cab + "','','" + voucherdate + "','" + refno + "', '" + company + "','" + paytype + "','" + vno + "','" + vamount + "','" + comrate + "','" + com + "','" + bal + "','" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "','" + String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now) + "','" + user + "','" + branchCode + "','" + branchName + "','" + vloc + "','" + spflag + "','"+exComm+"')";
                            command2.ExecuteNonQuery();
                        }
                    }

                }
                connection.Close();
                //con.Close();

                if (Convert.ToInt32(tbPhonebill.Text) > 0)
                {
                    t1 = new Taxi();
                    t1.updatePhoneBillFromVoucher(dgv10, recno);
                }

                saveVoucherReceipt(tbCabNo, tbNic, tbTotDays, tbTotEarning, tbDeduction, tbRefund, tbTarget, recno, tbName,dgv10,tbPhonebill,lbMobileLoanNo,tbMobileLOan,tbAppPhone,tbMonth,tbYear,deposit);


                if (Convert.ToDouble(tbDeduction.Text) > 0.00)
                    //coment for new deduction method 29/07/2017
                    //saveDeduction(recno, tbCabNo, tbDeduction, tbRefund, tbTotDays, tbTotEarning, tbMonth, tbYear, tbTarget, tbAddEarn, tbPerDayLimit);
                 saveNewDeduction(recno, tbCabNo, tbDeduction, tbRefund, tbTotDays, tbTotEarning, tbMonth, tbYear, tbTarget, tbAddEarn, tbPerDayLimit);

                if (Convert.ToDouble(tbRefund.Text) > 0.00)
                   //coment for new deduction method
                    //saveRefund(recno, tbCabNo, tbRefund, tbYear, tbMonth, tbTarget, tbDeductref, tbTotDays, tbTotEarning, tbPerDayLimit,chbFullRefund,addEarning);
                    saveNewRefund(recno, tbCabNo, tbDeduction, tbRefund, tbTotDays, tbTotEarning, tbMonth, tbYear, tbTarget, tbAddEarn, tbPerDayLimit);
            //}
                if (deposit == "Yes")
                    depositVoucherAmount(tbCabNo,recno,tbMonth,tbYear,tbNetAmount);
        }

        public void updateDespatchBooking(string refno,TextBox tbCabNo,string recno) 
        {
            MySqlConnection connection = new MySqlConnection(constr7);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE DespatchBooking SET Paid='1',PaidRef='" + recno + "' WHERE RefNo='" + refno + "' AND CabNo='" + tbCabNo.Text.Substring(1) + "'";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void updateLogsheet(string refno, TextBox tbCabNo,string recno)
        {
            char char1 = Convert.ToChar(refno.Substring(0, 1));
            char char2 = Convert.ToChar(refno.Substring(1, 1));
            string str = refno; //refno.Substring(2, 4);

            string ascii1 = ((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
            string asciiRefNo = refno; //ascii1 + ascii2 + str;


            //MySqlConnection connection = new MySqlConnection(constr7);
            //MySqlConnection connection = new MySqlConnection(constr8);
            MySqlConnection connection = new MySqlConnection(constr9);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            //command.CommandText = "UPDATE LogSheet SET 	Paid='1',PaidRef='"+recno+"' WHERE BookingID='" + asciiRefNo + "' AND VehiclID='" + tbCabNo.Text.Substring(1) + "'";
            command.CommandText = "UPDATE bookings SET 	Paid='1',PaidRef='" + recno + "' WHERE 	refID='" + asciiRefNo + "' AND 	cabNo='" + tbCabNo.Text.Substring(1) + "'";

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void saveVoucherReceipt(TextBox tbCabNo,TextBox tbNic,TextBox tbTotDays,TextBox tbTotEarning,TextBox tbDeduction,TextBox tbRefund,TextBox tbtarget, string recno,TextBox tbName,DataGridView dgv10,TextBox tbPhoneBil,Label lbMobileLoanNo, TextBox tbMobileLoan,TextBox tbAppPhone,TextBox tbMonth,TextBox tbYear,string deposit)
        {
            
            string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            us = new User();
            string user = us.getCurrentUser();

            //get phone bill recovery data
       
            string billYear = ""; string billMonth = ""; string monthFeild = "";

            if (Convert.ToInt32(tbPhoneBil.Text) > 0)
            {
                for (int i = 0; i <= dgv10.RowCount - 1; i++)
                {
                    if (dgv10.Rows[i].Cells[0].Value != null)
                    {
                        billYear = dgv10.Rows[i].Cells[2].Value.ToString();
                        billMonth = dgv10.Rows[i].Cells[3].Value.ToString();                        
                        //billformonth = Convert.ToInt32(dgv3.Rows[i].Cells[2].Value.ToString());
                        monthFeild = monthFeild + "," + billMonth;
                    }
                }
            }
        // end of phone bill details
           
            MySqlConnection connection = new MySqlConnection(constr1);           
            connection.Open();           
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO TestVoucherReceipt(vrecno,cabno,name,nic,totdays,totearning,deduction,refund,phoneBil,BillMonth,BilYear,date,user,flag,mobileLoanNo,mobileLoan,AppFine,Month,Year) VALUES('" + recno + "','" + tbCabNo.Text + "','" + tbName.Text + "','" + tbNic.Text + "','" + tbTotDays.Text + "','" + tbTotEarning.Text + "','" + tbDeduction.Text + "','" + tbRefund.Text + "','" + tbPhoneBil.Text + "','" + monthFeild + "','" + billYear + "','" + date + "','" + user + "','N','" + lbMobileLoanNo.Text + "','" + tbMobileLoan.Text + "','"+tbAppPhone.Text+"','"+tbMonth.Text+"','"+tbYear.Text+"')";
            command.ExecuteNonQuery();
            connection.Close();

            if (Convert.ToDouble(tbAppPhone.Text) > 0)
            saveAppPhonefineCharges(tbCabNo, tbAppPhone,recno);
 
            printVoucherReceipt(recno,tbMonth,tbYear);

        }

        public void saveAppPhonefineCharges(TextBox tbCabNo,TextBox tbAppFine,string recno) 
        {
            us = new User();
            string user = us.getCurrentUser();

            string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            string dateTime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);

            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO TestAppFine(CabNo,recNo,Amount,Date,DateTime,USer,Flag,Refund,RefundBy,RefunddateTime) VALUES('" + tbCabNo.Text + "','"+recno+"','" + tbAppFine.Text + "','" + date + "','" + dateTime + "','" + user + "','0','','','')";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void findVoucherReceipt(TextBox tbrecNo, DataGridView dgv1)
        {
            if (tbrecNo.Text.Length>=13)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataSet1 recfd = new DataSet1();
                MySqlConnection connection = new MySqlConnection(constr1);
                connection.Open();
                MySqlCommand command1 = connection.CreateCommand();
                command1.CommandText = "SELECT DISTINCT cabNo,vrecno,Month,Year FROM voucherreceipt WHERE (vrecno LIKE '" + tbrecNo.Text + "%') OR (cabNo LIKE '" + tbrecNo.Text + "%' ) ORDER BY date DESC";
                MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

                newadp1.Fill(ds);
                dt = ds.Tables[0];
                dgv1.DataSource = dt;
                connection.Close();

            }

            else
            {
                dgv1.DataSource = null;
            }
        }

        //public int get_creditLimit()
        //{
        //    return Convert.ToInt32(ConfigurationManager.AppSettings["creditLimitPerDay"]);
        //}

        public void printVoucherReceipt(string recno,TextBox tbMonth,TextBox tbYear)
        {
            int totDays = 0; int targetIncome = 0;
            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT * FROM allvoucherreceipt WHERE 	vrecno='" + recno + "'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherPrint");
            connection.Close();

            DataTable vchrPrint = recds.Tables["VoucherPrint"];
            totDays = Convert.ToInt32(vchrPrint.Rows[0]["totdays"]);
            targetIncome = totDays * getTargetIncomePerDay(tbMonth, tbYear);   // get_creditLimit();

            CryVoucherReceipt rpt = new CryVoucherReceipt();

           
            TextObject target = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            target.Text = targetIncome.ToString();



            
            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;           
            f3.crystalReportViewer1.ReportSource = rpt;
            rpt.PrintToPrinter(1, false, 1, 1);


        }

        public void printSelectedVoucherReceipt(string recno, TextBox tbMonth, TextBox tbyear)
        //public void printSelectedVoucherReceipt(string recno)
        {
            int totDays = 0; int targetIncome = 0;
            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT * FROM allvoucherreceipt WHERE 	vrecno='" + recno + "'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherPrint");
            connection.Close();

            DataTable vchrPrint = recds.Tables["VoucherPrint"];
            totDays = Convert.ToInt32(vchrPrint.Rows[0]["totdays"]);
            //targetIncome = totDays * getTargetIncomePerDay(tbMonth, tbyear); //get_creditLimit();

            CryVoucherReceipt rpt = new CryVoucherReceipt();

            TextObject target = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            target.Text = targetIncome.ToString();

            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            f3.crystalReportViewer1.ReportSource = rpt;
            rpt.PrintToPrinter(1, false, 1, 1);


        }

        public void addVouchersToPaymentGrid(DataGridView dgv, DateTime dtVdate, TextBox tbVoucherAmount, TextBox tbTotWorkDays, TextBox tbSumAmount, TextBox tbTarget, TextBox tbAddEarn, string CabNo, TextBox tbMonth, TextBox tbYear, TextBox tbPerDayLimit, DateTime dtManualVdate, CheckBox chbManual, TextBox tbPreDeduct, TextBox tbPreRefund, TextBox tbCurrentDeduct, TextBox tbCurrentRefund, TextBox tbTotAdditional, TextBox tbFinalDeduct, TextBox tbFinalRefund, TextBox tbNumOfVoucher,TextBox tbAppAmount) 
        {
            string vDate = "";
            string rvDate = String.Format("{0:dd-MM-yyyy}", dtVdate); // with reference number voucher date
            string mvDate = String.Format("{0:dd-MM-yyyy}", dtManualVdate);//without reference number Voucher Date
            double vamount = 0.00; int noVouchers = 0; double amount=0.00; 

            if (chbManual.Checked == true)
                vDate = mvDate;
            else
                vDate = rvDate;

            vamount = (Convert.ToDouble(tbVoucherAmount.Text));

            for(int i=0;i<dgv.Rows.Count-1;i++)
            {
                if (vDate == dgv.Rows[i].Cells[0].Value.ToString()) 
                {
                    if(dgv.Rows[i].Cells[1].Value!=null)
                        noVouchers = Convert.ToInt32(dgv.Rows[i].Cells[1].Value.ToString());
                   if(dgv.Rows[i].Cells[2].Value!=null)
                        amount = Convert.ToDouble(dgv.Rows[i].Cells[2].Value.ToString());

                    noVouchers = noVouchers + 1;
                    amount = amount + vamount;

                    dgv.Rows[i].Cells[1].Value = noVouchers.ToString();
                    dgv.Rows[i].Cells[2].Value = amount.ToString();                    
                }

            }
            countAll(dgv, tbTotWorkDays, tbSumAmount, tbTarget, tbAddEarn, CabNo, tbMonth, tbYear, tbPerDayLimit, tbPreDeduct, tbPreRefund, tbCurrentDeduct, tbCurrentRefund, tbTotAdditional, tbFinalDeduct, tbFinalRefund, tbNumOfVoucher);

        }
        
        public bool getCabForRefNo(TextBox tbCabno, TextBox tbRefNo, DateTimePicker dtRefDate)
        {
            string date = String.Format("{0:yyyy-MM-dd}", dtRefDate.Value); string cab="";
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();
            SqlCommand command2 = connection.CreateCommand();
            //command1.CommandText = "SELECT CabNo FROM VoucherRef WHERE voucherDate='" + date + "' AND RIGHT(voucherRefNo,4)='" + tbRefNo.Text + "' ORDER BY CabNo";
            command1.CommandText = "SELECT TOP 1000 CabNo FROM Job WHERE  (RIGHT(JobID,4)='" + tbRefNo.Text + "') AND (CAST(DispatchTime AS DATE)='" + date + "') AND (Flag='DE') ORDER BY JobID DESC";
            command2.CommandText =  "SELECT  CabNo FROM CancelJob WHERE  RIGHT(JobID,4)='" + tbRefNo.Text + "' AND CAST(DispatchTime AS DATE)='" + date + "' ORDER BY DispatchTime DESC";
            
            var reader1 = command1.ExecuteReader();
            if (reader1.HasRows)
            {   
                reader1.Read();
                cab= (reader1["CabNo"].ToString()).Trim();                
                connection.Close();
                return  CheckCabValidity(cab, tbCabno);
            }
            else
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Open();
                }

                var reader2 = command2.ExecuteReader();
                if (reader2.HasRows)
                {
                    reader2.Read();
                    cab = (reader2["CabNo"].ToString()).Trim();
                    connection.Close();
                    return CheckCabValidity(cab, tbCabno);
                }
                else
                    return false;
            }
            

        }

        public bool getCabForRefNoApp(TextBox tbCabno, TextBox tbRefNo, DateTimePicker dtRefDate)
        {
            string date = String.Format("{0:yyyy-MM-dd}", dtRefDate.Value); string cab = "";
            MySqlConnection connection = new MySqlConnection(constr7);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            //command1.CommandText = "SELECT CabNo FROM VoucherRef WHERE voucherDate='" + date + "' AND RIGHT(voucherRefNo,4)='" + tbRefNo.Text + "' ORDER BY CabNo";
            command1.CommandText = "SELECT  CabNo  FROM DespatchBooking WHERE  (RefNo='" + tbRefNo.Text + "') AND DATE(RecordInsertTime) ='" + date + "'  ORDER BY AutoID DESC";
            //command2.CommandText = "SELECT  CabNo FROM CancelJob WHERE  RIGHT(JobID,4)='" + tbRefNo.Text + "' AND CAST(DispatchTime AS DATE)='" + date + "' ORDER BY DispatchTime DESC";

            var reader1 = command1.ExecuteReader();
            if (reader1.HasRows)
            {
                reader1.Read();
                cab = (reader1["CabNo"].ToString()).Trim();
                connection.Close();
                return CheckCabValidity(cab, tbCabno);
            }
            else
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Open();
                }

                var reader2 = command2.ExecuteReader();
                if (reader2.HasRows)
                {
                    reader2.Read();
                    cab = (reader2["CabNo"].ToString()).Trim();
                    connection.Close();
                    return CheckCabValidity(cab, tbCabno);
                }
                else
                    return false;
            }


        }

        public bool getCabForRefNoAppLogsheet(TextBox tbCabno, TextBox tbRefNo, DateTimePicker dtRefDate)
        {
            string key=tbRefNo.Text;
            char char1 = Convert.ToChar(key.Substring(0, 1));
            char char2 = Convert.ToChar(key.Substring(1, 1));
            string str = key.Substring(2, 4);

            string ascii1 = ((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
            string asciiRefNo = key;//ascii1 + ascii2+ str;

            string date = String.Format("{0:yyyy-MM-dd}", dtRefDate.Value); string cab = "";
            //MySqlConnection connection = new MySqlConnection(constr7);
            //MySqlConnection connection = new MySqlConnection(constr8);

            MySqlConnection connection = new MySqlConnection(constr9);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            //command1.CommandText = "SELECT CabNo FROM VoucherRef WHERE voucherDate='" + date + "' AND RIGHT(voucherRefNo,4)='" + tbRefNo.Text + "' ORDER BY CabNo";
           // command1.CommandText = "SELECT  	vehicl_id as CabNo  FROM budget_bookings WHERE  (booking_id='" + asciiRefNo + "') AND DATE(meter_off_time) ='" + date + "'  ORDER BY booking_id DESC";
            //command2.CommandText = "SELECT  CabNo FROM CancelJob WHERE  RIGHT(JobID,4)='" + tbRefNo.Text + "' AND CAST(DispatchTime AS DATE)='" + date + "' ORDER BY DispatchTime DESC";

            command1.CommandText = "SELECT cabNo as CabNo  FROM bookings WHERE  (refID='" + asciiRefNo + "') AND DATE(endTime) ='" + date + "'  ORDER BY refID DESC";

            var reader1 = command1.ExecuteReader();
            if (reader1.HasRows)
            {
                reader1.Read();
                cab = (reader1["CabNo"].ToString()).Trim();
                connection.Close();
                return CheckCabValidity(cab, tbCabno);
            }
            else
            {
                //if (connection.State == ConnectionState.Closed)
                //    connection.Open();
                //if (connection.State == ConnectionState.Open)
                //{
                //    connection.Close();
                //    connection.Open();
                //}

                //var reader2 = command2.ExecuteReader();
                //if (reader2.HasRows)
                //{
                //    reader2.Read();
                //    cab = (reader2["CabNo"].ToString()).Trim();
                //    connection.Close();
                //    return CheckCabValidity(cab, tbCabno);
                //}
                //else
                    return false;
            }


        }
                
        public bool CheckCabValidity(string cab,TextBox tbCab)
        {
            string part1="";string cabnumber="";

             part1 = cab.Substring(1,1);

            if(part1=="K" ||part1=="k")
                cabnumber = cab.Substring(1);
            else
                cabnumber = cab.Trim(); 
            
            if (cabnumber == tbCab.Text)
            {
                tbCab.Text = cabnumber;
                return true;
            }
            else
            {
                MessageBox.Show("This Reference No is Allocated to " + cabnumber);
                return false;
                //tbCab.Text = "";
               
            }
        }
        
        public bool CheckValidMonth(DateTimePicker dtVdate,TextBox tbYear,TextBox tbMonth)
        {
            int vMonth = dtVdate.Value.Month; int vYear = dtVdate.Value.Year;
            int month = Convert.ToInt32(tbMonth.Text); int year = Convert.ToInt32(tbYear.Text); 

            
                if (month != 0)
                {
                    if (vMonth != month)
                    {
                        MessageBox.Show("This Voucher is belongs to an another month, Please settle the previous Vouchers");
                        dtVdate.Value = DateTime.Now;
                        return false;

                    }

                    if (vYear != year)
                    {
                        MessageBox.Show("This Voucher is belongs to an another year, Please settle the previous Vouchers");
                        dtVdate.Value = DateTime.Now;
                        return false;
                    }
                }
            

            return true;



        }

        public void calVoucherNetTotal(TextBox tbVoucherTotal,TextBox tbDeduction,TextBox tbRefund,TextBox tbNetAmount,TextBox tbPhoneBill,CheckBox chbMobileLoan, TextBox tbMobileLoan,TextBox tbMobileAreas,Label lbopenAreas,TextBox tbAppFine)
        
        {
            double netAmount=0.00;
            if (tbVoucherTotal.Text != "")
                netAmount = (Convert.ToDouble(tbVoucherTotal.Text)) -(Convert.ToDouble(tbAppFine.Text))-(Convert.ToDouble(tbDeduction.Text)) + (Convert.ToDouble(tbRefund.Text));
            //tbNetAmount.Text = netAmount.ToString();
              
            if (Convert.ToDouble(tbPhoneBill.Text) > 0.00) //deduction of phone bills
            {
                if (netAmount > Convert.ToDouble(tbPhoneBill.Text))
                {
                    netAmount = netAmount - Convert.ToDouble(tbPhoneBill.Text);
                    //tbNetAmount.Text = netAmount.ToString();
                }
            }

            if (chbMobileLoan.Checked == true) // deduction of mobile loan
            {
              
                double areas = Convert.ToDouble(lbopenAreas.Text);

                if (areas >= (netAmount / 2))
                {
                    tbMobileLoan.Text = (Math.Round(( netAmount / 2),2)).ToString();

                    tbMobileAreas.Text = (Math.Round((Convert.ToDouble(lbopenAreas.Text) - (netAmount / 2)),2)).ToString();

                    //tbNetAmount.Text = (Math.Round((netAmount / 2), 2)).ToString();
                    netAmount = Math.Round((netAmount / 2), 2);
                }

                else 
                {
                    tbMobileLoan.Text = areas.ToString();

                    tbMobileAreas.Text = "0.00";

                    //tbNetAmount.Text = (netAmount - areas).ToString();
                    netAmount = (netAmount - areas);
                }                         

              
                
            }
                tbNetAmount.Text = netAmount.ToString();

                //tbNetAmount.Text = tbVoucherTotal.Text;//We can use this to ignore deduction and refund
        }
        
        public void advancedFindHiresEndRef(DataGridView dgv1,DateTimePicker dtDate, string key,RadioButton rbDe,RadioButton rbCa,RadioButton rbSp) 
        {
               string date=String.Format("{0:yyyy-MM-dd}", dtDate.Value);
           
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataSet1 recfd = new DataSet1();
                SqlConnection connection = new SqlConnection(constr5);
                connection.Open();
                SqlCommand command1 = connection.CreateCommand();
                if(rbDe.Checked)
                    command1.CommandText = "SELECT RIGHT(JobID,4) AS JobID ,DispatchTime,CabNo,Name,Organization,Flag FROM JOB WHERE  (RIGHT(JobID,4)='" + key + "') AND (CAST(DispatchTime AS DATE)='" + date + "') ";
                if (rbCa.Checked)
                    command1.CommandText = "SELECT RIGHT(JobID,4) AS JobID ,DispatchTime,CabNo,Name,Organization,Flag FROM CancelJob WHERE  (RIGHT(JobID,4)='" + key + "') AND (CAST(DispatchTime AS DATE)='" + date + "') ";
                if(rbSp.Checked)
                    command1.CommandText = "SELECT RIGHT(voucherRefNo,4) AS JobID ,voucherDate AS DispatchTime,CabNo,client AS Name,Organization,type AS Flag FROM VoucherRef WHERE  (RIGHT(voucherRefNo,4)='" + key + "') AND (voucherDate='" + date + "') ";
                SqlDataAdapter newadp1 = new SqlDataAdapter(command1);//to retrive data (we can use data reader)  

                newadp1.Fill(ds);
                dt = ds.Tables[0];
                dgv1.DataSource = dt;
                          
                connection.Close();

        }

        public void advancedFindHiresFullRef(DataGridView dgv1, DateTimePicker dtDate, string key, RadioButton rbDe, RadioButton rbCa,RadioButton rbSp)
        {
            string date = String.Format("{0:yyyy-MM-dd}", dtDate.Value);

            char char1 = Convert.ToChar(key.Substring(0, 1));
            char char2 = Convert.ToChar(key.Substring(1, 1));
            string str = key.Substring(2, 4);

            string ascii1 = ((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
            string asciiRefNo = ascii1 + ascii2 + str;
            //string ascii1 = ((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
            //string asciiRefNo = ascii1 + ascii2 + str;

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();

            if (rbDe.Checked)
                command1.CommandText = "SELECT RIGHT(JobID,4) AS JobID ,DispatchTime,CabNo,Name,Organization,Flag FROM JOB WHERE  (JobID='" + asciiRefNo + "') AND (CAST(DispatchTime AS DATE)='" + date + "') ";
            if (rbCa.Checked)
                command1.CommandText = "SELECT RIGHT(JobID,4) AS JobID ,DispatchTime,CabNo,Name,Organization,Flag FROM CancelJob WHERE  (JobID='" + asciiRefNo + "') AND (CAST(DispatchTime AS DATE)='" + date + "') ";
            if (rbSp.Checked)
                command1.CommandText = "SELECT RIGHT(voucherRefNo,4) AS JobID ,voucherDate AS DispatchTime,CabNo,client AS Name,Organization,type AS Flag  FROM VoucherRef WHERE  (voucherRefNo='" + key + "') AND (voucherDate='" + date + "')";


            SqlDataAdapter newadp1 = new SqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(ds);
            dt = ds.Tables[0];
            dgv1.DataSource = dt;

            connection.Close();

            
        }
        
        public void advancedFindHiresCab(DataGridView dgv1, DateTimePicker dtDate, string key, RadioButton rbDe, RadioButton rbCa,RadioButton rbSp)
        {
            string date = String.Format("{0:yyyy-MM-dd}", dtDate.Value);
            string cabNo = "K" + key;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();
            if (rbDe.Checked)
                command1.CommandText = "SELECT RIGHT(JobID,4) AS JobID ,DispatchTime,CabNo,Name,Organization,Flag FROM JOB WHERE  (CabNo='" + key + "') AND (CAST(DispatchTime AS DATE)='" + date + "') ";
            if (rbCa.Checked)
                command1.CommandText = "SELECT RIGHT(JobID,4) AS JobID ,DispatchTime,CabNo,Name,Organization,Flag FROM CancelJob WHERE  (CabNo='" + key + "') AND (CAST(DispatchTime AS DATE)='" + date + "') ";
            if (rbSp.Checked)
                command1.CommandText = "SELECT RIGHT(voucherRefNo,4) AS JobID ,voucherDate AS DispatchTime,CabNo,client AS Name,Organization,type AS Flag  FROM VoucherRef WHERE  (CabNo='" + cabNo + "') AND (voucherDate='" + date + "')";
            SqlDataAdapter newadp1 = new SqlDataAdapter(command1);//to retrive data (we can use data reader)  

            newadp1.Fill(ds);
            dt = ds.Tables[0];
            dgv1.DataSource = dt;

            connection.Close();

        }

        public void advancedFindInMessageTable(string key,DateTimePicker dtDate, DataGridView dgv2,TextBox tbMob) 
        {
              string date = String.Format("{0:yyyy-MM-dd}", dtDate.Value);

              DataSet ds = new DataSet();
              DataTable dt = new DataTable();
              SqlConnection connection = new SqlConnection(constr5);
              connection.Open();
              SqlCommand command = connection.CreateCommand();
             if(tbMob.Text.Length<1)
                 command.CommandText = "SELECT Message FROM Message WHERE (Message LIKE '%" + key + "%') AND (CusOrDriver='D')  AND (((CAST(DDateTime AS DATE))='" + date + "') OR ((CAST(SDateTime AS DATE))='" + date + "'))";
             else
                 command.CommandText = "SELECT Message FROM Message WHERE (Message LIKE '%" + key + "%' AND Mobile='" + tbMob.Text + "') AND (CusOrDriver='D') AND (((CAST(DDateTime AS DATE))='" + date + "') OR ((CAST(SDateTime AS DATE))='" + date + "'))";
              SqlDataAdapter newadp1 = new SqlDataAdapter(command);//to retrive data (we can use data reader)  

              newadp1.Fill(ds);
              dt = ds.Tables[0];
              dgv2.DataSource = dt;
              dgv2.Columns[0].Width = 672;
              connection.Close();
              
            }

        public void FineForAppPhone(TextBox tbVoucherTotal, TextBox tbAppPhoneFine, CheckBox chbAppFine, TextBox tbComComition)
        {
            if (chbAppFine.Checked == true)
            {
                double voucherTotal = 0.00; double totalEarn = 0.00; double appFineCharges = 0.00; double commition = 0.00;
                commition = Convert.ToDouble(tbComComition.Text);

                voucherTotal = Convert.ToDouble(tbVoucherTotal.Text);


                totalEarn = (voucherTotal / (100 - commition)) * 100; //voucher total mean , total earn deduct 7.5% , to find total earn voucher total divided by 92.5 and multiply by 100.


                appFineCharges = (totalEarn / 100) * 10;
                tbAppPhoneFine.Text = Math.Round(appFineCharges, 1).ToString();
            }
        }


        public void FindAppFineForRefund(TextBox tbCabNo,TextBox tbDeAmount) 
        {
            string cabno = ""; double amount = 0.00;
            cabno = "K" + tbCabNo.Text;

            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT sum(`Amount`) as amount FROM `TestAppFine` WHERE (`CabNo`='" + cabno + "') AND (Flag='0')";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                   
                    while (reader.Read())
                    {
                        if (reader["amount"] != null && reader["amount"] != DBNull.Value)
                        {
                            amount = Convert.ToDouble(reader["amount"].ToString());
                            tbDeAmount.Text = amount.ToString();
                        }
                        else
                            tbDeAmount.Text = "0.00";
                    }
                }
                else 
                {
                    tbDeAmount.Text = "0.00";
                }
            }

            connection.Close();
        }

        public void RefundAppFine(TextBox tbCabNo,TextBox tbRemark) 
        {
            if (tbRemark.Text != "")
            {
                string cabno = ""; string recno="";
                cabno = "K" + tbCabNo.Text;

                nrcn = new NewReceiptNumber();
                recno=nrcn.getAppFineRecNo();


                us = new User();
                string user = us.getCurrentUser();

                string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string dateTime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);

                MySqlConnection connection = new MySqlConnection(constr1);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE `TestAppFine` SET Flag='1',Refund='" + tbRemark.Text + "',RefundBy='" + user + "',refundDate='" + date + "',RefunddateTime='" + dateTime + "',refundRecNo='" + recno + "' WHERE (`CabNo`='" + cabno + "') AND (Flag='0')";
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Successfully Refunded !! ");

                nrcn.updateAppFineRecNo(recno);
                Rpprint = new ReportsPrint();
                Rpprint.printAppFineRefundReceipt(recno);
            }
            else 
            {
                MessageBox.Show("Plase Enter The Remark");
            }
        }

        public void breakDownTheAdditionalIncome(DataGridView dgv1,TextBox tbToteduction,TextBox tbPreviousDeduct,TextBox tbCurrentDeduct,TextBox tbMonth,TextBox tbYear,TextBox tbTotDays,TextBox tbAdditionalEarn)
        {

            int perDayLimit = 0;
            int PerDayConstnt = 500;
            int perDayMaxMargin = 0; //5100;
            int numberofDays = 0;
            int numOfStep = 6;
            double ballance = 0.00;
            double AdditionalEarning = 0;

            perDayLimit = getTargetIncomePerDay(tbMonth,tbYear);
            perDayMaxMargin = getTargetAboveLimiFroMonth(tbMonth, tbYear);
            numberofDays = Convert.ToInt32(tbTotDays.Text);
            AdditionalEarning = Convert.ToDouble(tbAdditionalEarn.Text);

            int maxLimit = perDayLimit * numberofDays;
            int margin = (PerDayConstnt * numberofDays) * numOfStep;

            gridFormation(dgv1, perDayLimit, PerDayConstnt, numberofDays, perDayMaxMargin);

            if (margin < AdditionalEarning)
            {
                ballance = AdditionalEarning - margin;
                for (int i = 0; i < 6; i++)
                {
                    dgv1.Rows[i].Cells[2].Value = PerDayConstnt * numberofDays;
                }

                dgv1.Rows[6].Cells[2].Value = ballance;
            }
            else
            {
                int position = 0;
                double bal = AdditionalEarning % (PerDayConstnt * numberofDays);
                int step = Convert.ToInt32((AdditionalEarning - bal) / (PerDayConstnt * numberofDays));
                ballance = AdditionalEarning - (step * (PerDayConstnt * numberofDays));

                if (step != 0)// step =0 mean ( bal <500)
                {
                    for (int i = 0; i < step; i++)
                    {
                        dgv1.Rows[i].Cells[2].Value = PerDayConstnt * numberofDays;
                        position = i;
                    }

                    dgv1.Rows[position + 1].Cells[2].Value = ballance;


                }
                else
                {
                    dgv1.Rows[0].Cells[2].Value = ballance;
                    for (int i = 1; i < dgv1.RowCount - 1; i++)
                    {
                        dgv1.Rows[i].Cells[2].Value = "0";
                    }
                }

                for (int i = position + 2; i < dgv1.RowCount - 1; i++)
                {
                    dgv1.Rows[i].Cells[2].Value = "0";
                }
                
            }
            calStepInterest(dgv1,tbToteduction);
        }

        public void gridFormation(DataGridView dgv1, int perDayLimit, int constant, int numberOfDays, int perDayMaxMargin)
        {

            int increment = 0;
            double intrest = 7.50;
            int step = 6;
            int valueA = 0; int valueB = 0;

            dgv1.Rows.Clear();
            dgv1.Rows.Add(7);
            increment = (perDayLimit * numberOfDays) + (constant * numberOfDays);

            for (int i = 0; i < 6; i++)
            {
                dgv1.Rows[i].Cells[0].Value = i + 1;
                dgv1.Rows[i].Cells[3].Value = intrest;
                intrest = intrest + 2.5;

                if (i == 0)
                {
                    valueA = perDayLimit * numberOfDays;
                    valueB = valueA + (constant * numberOfDays);
                    dgv1.Rows[i].Cells[1].Value = valueA + " --> " + valueB;
                }
                else
                {
                    valueA = increment;
                    valueB = valueA + (constant * numberOfDays);
                    dgv1.Rows[i].Cells[1].Value = valueA + " --> " + valueB;
                    increment = increment + (constant * numberOfDays);
                }
            }
            dgv1.Rows[6].Cells[0].Value = 7;
            dgv1.Rows[6].Cells[1].Value = (perDayMaxMargin * numberOfDays) + " Above";
            dgv1.Rows[6].Cells[3].Value = 22.50;


        }
        
        public void calStepInterest(DataGridView dgv1,TextBox tbTotDeduction)
        {
            for (int i = 0; i < 7; i++)
            {
                if (string.IsNullOrEmpty(dgv1.Rows[0].Cells[2].Value as string))
                {
                    double rate = Convert.ToDouble(dgv1.Rows[i].Cells[3].Value);
                    double value = Convert.ToDouble(dgv1.Rows[i].Cells[2].Value);
                    double interest = (value / 100) * rate;
                    dgv1.Rows[i].Cells[4].Value = interest;
                }
            }
            calTotalDedution(dgv1, tbTotDeduction); 
        }

        public void calTotalDedution(DataGridView dgv1,TextBox tbTotDeduction) 
        {
            double sum = 0.00;
            double totSum = 0.00;
            for (int i = 0; i < 7; i++) 
            {
                if (string.IsNullOrEmpty(dgv1.Rows[i].Cells[4].Value as string))
                { 
                    sum =Convert.ToDouble(dgv1.Rows[i].Cells[4].Value.ToString());
                    totSum = totSum + sum;
 
                }
            }
            tbTotDeduction.Text=totSum.ToString();
            
        }

        

        public void ClearDeductionInfo(DataGridView dgv, TextBox tbRecNo,TextBox tbYear,TextBox tbMonth,TextBox tbCreditLimit,TextBox tbNumOfDays,TextBox tbTotEarn,TextBox tbTarget,TextBox tbAddEarn,TextBox tbTotDeduction,TextBox tbPreDeduction,TextBox tbPreRefund,TextBox tbCurrentDeduction,TextBox tbCurrentRefund,Label lbUser,Label lbCabNo,Label lbDate) 
        {
            dgv.Rows.Clear(); tbRecNo.Text = ""; tbYear.Text = ""; tbMonth.Text = ""; tbCreditLimit.Text = "0.00"; tbNumOfDays.Text = "0.00"; tbTotEarn.Text = "0.00"; tbTarget.Text = "0.00"; tbAddEarn.Text = "0.00"; tbTotDeduction.Text = "0.00"; tbPreDeduction.Text = "0.00"; tbPreRefund.Text = "0.00"; tbCurrentDeduction.Text = "0.00"; tbCurrentRefund.Text = "0.00"; lbUser.Text = ""; lbCabNo.Text = ""; lbDate.Text = "";
        }


        public void depositVoucherAmount(TextBox tbCabNo, string recno,TextBox tbMonth,TextBox tbYear,TextBox tbNetAmount) 
        {
            us = new User();
            string user = us.getCurrentUser();

            string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            string dateTime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);

            MySqlConnection connection = new MySqlConnection(constr1);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO TestVoucherDeposit(recNo,cabNo,depAmount,month,Year,depDate,depDateTime,depUser,flag) VALUES('"+recno+"','"+tbCabNo.Text+"','"+tbNetAmount.Text+"','"+tbMonth.Text+"','"+tbYear.Text+"','"+date+"','"+dateTime+"','"+user+"','N')";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
