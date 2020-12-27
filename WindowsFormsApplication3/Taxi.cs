using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Globalization;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication3
{
    class Taxi
    {
        Ventura ven = new Ventura();
        ReceiptNew rn;
        NewReceiptNumber nrcn;
        User us;
        private string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        private string constr2 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.AmilaConnectionString"].ConnectionString;
        private string constr3 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString2"].ConnectionString;//yard server Wijerama
        private string constr4 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.PaymentConnectionString"].ConnectionString;
        private string constr5 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CallCenterCityCabConnectionString"].ConnectionString;
        private string constr6 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.Calling_numberConnectionString1"].ConnectionString;
        private string constr7 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.budgetConnectionString"].ConnectionString;
        private string constr8 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.bookingsConnectionString"].ConnectionString;
        private string constr9 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.accounts_reportsConnectionString"].ConnectionString;
        
        string taxiNo; DateTime lastPaymentDate; string reciptNo; int perDayCharge = 300; int SpPerDayCharge = 350; int minimumDays = 10; int phoneBil = 400; int workingDays = 0; int ph20Bill = 200;
        int ph25Bill = 100;
        string location = "H";//Y for Yard
        static int blockID = 0;
        int appRent = 900;

        public string get_Location()
        {
            return ConfigurationManager.AppSettings["Location"];
        }

        public DataTable getPaymentDates(string taxi)
        {
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();

                MySqlConnection connection1 = new MySqlConnection(constr);
                connection1.Open();
                MySqlCommand command = connection1.CreateCommand();
                // command.CommandText = "select ReciptNo,ReciptDate from ReciptHeader where CabNo='" + taxi + "' order by ReciptDate DESC";
                command.CommandText = "select RecNo,Date,Cancel from TestPayment where (CabNo='" + taxi + "') AND (TestPayment.Delete!='Y')  order by Date DESC";
                MySqlDataAdapter newadp = new MySqlDataAdapter(command);
                newadp.Fill(ds);
                dt = ds.Tables[0];
                connection1.Close();

                if (ds.Tables[0] != null)
                {
                    return dt;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public DataTable getFreePromotionDates(string taxi)
        {
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();

                MySqlConnection connection1 = new MySqlConnection(constr);
                connection1.Open();
                MySqlCommand command = connection1.CreateCommand();
                // command.CommandText = "select ReciptNo,ReciptDate from ReciptHeader where CabNo='" + taxi + "' order by ReciptDate DESC";
                command.CommandText = "select RecNo,Date from TestFreePayment where (CabNo='" + taxi + "') AND (Cancel!='Y') order by Date DESC";
                MySqlDataAdapter newadp = new MySqlDataAdapter(command);
                newadp.Fill(ds);
                dt = ds.Tables[0];
                connection1.Close();

                if (ds.Tables[0] != null)
                {
                    return dt;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public DateTime getLastFreeDate(TextBox tbTaxiNo, CheckBox chbox)
        {
            if (chbox.Checked == false)
            {
                string taxiNo = "K" + tbTaxiNo.Text;
                DateTime lastFreeDate = DateTime.MinValue;
                try
                {
                    MySqlConnection connection1 = new MySqlConnection(constr);
                    connection1.Open();
                    MySqlCommand command = connection1.CreateCommand();
                    command.CommandText = "SELECT Date FROM TestFreePayment WHERE (CabNo='" + taxiNo + "') AND (TestFreePayment.Cancel!='Y')";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lastFreeDate = Convert.ToDateTime(reader["Date"].ToString());
                        }
                    }
                    connection1.Close();
                }
                catch (MySqlException ex) { MessageBox.Show(ex.Message); }
                return lastFreeDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public DateTime getlastPaymentDate(System.Windows.Forms.DataGridView dg, DateTimePicker dtp, TextBox tbSunday, TextBox tbLastFreeDate, CheckBox chbox,DateTimePicker dtLastDateBackup)
        {
            if (chbox.Checked == false)
            {
                try
                {
                    DateTime ldate = DateTime.MinValue;
                    //DateTime ldate = Convert.ToDateTime(dg.Rows[0].Cells[1].Value.ToString());
                    DateTime lastFreeDate = Convert.ToDateTime(tbLastFreeDate.Text);
                    string paid = dg.Rows[0].Cells[2].Value.ToString();
                    if (paid == "0" || paid == "B")
                    {
                        ldate = Convert.ToDateTime(dg.Rows[0].Cells[1].Value.ToString());
                    }
                    else
                    {
                        ldate = getLastPhoneBillPayementDate(dg);
                    }

                    if (ldate > lastFreeDate)
                    {
                        if (ldate > DateTime.Now)
                        {
                            dtp.Value = ldate.AddDays(1);

                            dtLastDateBackup.Value = ldate.AddDays(1);

                            
                        }
                        if (ldate.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                        {
                            dtp.Value = ldate.AddDays(1);
                            dtLastDateBackup.Value = ldate.AddDays(1);
                        }
                        else
                        {
                            dtp.Value = DateTime.Now;
                            dtLastDateBackup.Value = DateTime.Now;
                        }
                    }
                    else if (ldate < lastFreeDate)
                    {
                        if (lastFreeDate > DateTime.Now)
                        {
                            dtp.Value = lastFreeDate.AddDays(1);
                            dtLastDateBackup.Value = lastFreeDate.AddDays(1);
                        }
                        if (ldate.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                        {
                            dtp.Value = ldate.AddDays(1);
                            dtLastDateBackup.Value = ldate.AddDays(1);
                        }
                        else
                        {
                            dtp.Value = DateTime.Now;
                            dtLastDateBackup.Value = DateTime.Now;
                        }
                    }
                    else
                    {
                        dtp.Value = DateTime.Now;
                        dtLastDateBackup.Value = DateTime.Now;
                    }

                    tbSunday.Text = checkLastSunday(ldate, dg);//check whether last sunday is worked or not
                    return ldate;
                }
                catch (NullReferenceException) { return DateTime.MinValue; }
            }//new taxi
            else
            {
                return DateTime.MinValue;
            }

        }

        public DateTime getLastPhoneBillPayementDate(DataGridView dgv1)
        {
            DateTime ldate = DateTime.MinValue;
            for (int i = 0; i < 10; i++)
            {
                if (dgv1.Rows[i].Cells[2].Value.ToString() == "0")
                {
                    ldate = Convert.ToDateTime(dgv1.Rows[i].Cells[1].Value.ToString());
                    return ldate;
                }
            }
            return ldate;
        }

        public void getDriverMobileNumber(string cabno, TextBox tbMobileNo)
        {

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT MobileNo FROM TestDriverMobile WHERE cabNo='" + cabno + "'";
            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    //dtOpDate.Value = Convert.ToDateTime(reader["OpDate"].ToString());
                    tbMobileNo.Text = reader["MobileNo"].ToString();
                }
            }
            connection.Close();
        }


        public void FindDriverImege(string cabno)
        {
            //string bno = "";
            //bno = findBnofromCallingNo(cabno);

            //MySqlConnection connection = new MySqlConnection(constr2);
            //connection.Open();
            //MySqlCommand command = connection.CreateCommand();

            //command.CommandText = "SELECT Dimage FROM DriverBudget WHERE CabNo='" + cabno + "' and Dno='"+bno+"'";
            //using (var reader = command.ExecuteReader())
            //{
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read()) 
            //        {
                       
            //            String imege = reader["Dimage"].ToString();
            //            if (imege.Length < 5)
            //                MessageBox.Show("Imege Not Found");
            //        }
            //    }
            //}
            //connection.Close();
        }

        public string findBnofromCallingNo(string cabno) 
        {
            string bno = "";
            string callingDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            MySqlConnection connection = new MySqlConnection(constr6);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT `Dno` FROM `CallingBdNo` WHERE `CabNo` ='"+cabno+"' order by `Date` desc limit 1 ";
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {                       
                        bno = reader["Dno"].ToString();                        
                    }
                }
            }
            connection.Close();

            return bno;
        }

        public double getAreasPhoneBill(DateTime PayLastDate, System.Windows.Forms.DataGridView dgv3, CheckBox chbox, DateTime newTaxiOpenDate, TextBox tbserviceprovider, CheckBox chb3, CheckBox chbBranding,TextBox tbCabNo,CheckBox chbPresonalSIM)
        {
            //if (chbBranding.Checked == false)
            //{
            double areasMonth = 0;
            double areasPhoneBill = 0;
            DateTime newTaxiDateOpen = newTaxiOpenDate;
            dgv3.Rows.Clear();

            if (findCabNoToIgnorePhoneBill(tbCabNo,chbPresonalSIM)==false)// if it is false, normal cab, need to charge phone bill
            {
            if (tbserviceprovider.Text != "Etisalat" || chb3.Checked == false )
            {
                if (DateTime.MinValue != PayLastDate && chbox.Checked == false)//if last payment date is 01/01/0001 it means a new taxi , this if for existing taxi
                {
                    if (PayLastDate < newTaxiDateOpen.Date)  //if ( newTaxiDateOpen.Date) /new taxi open date men, start date to for payment 
                    {
                        //if (PayLastDate.Year == DateTime.Now.Year)//for same yaer
                        if (PayLastDate.Year == newTaxiDateOpen.Date.Year)//for same yaer
                        {
                            //********************************************************************
                            if (PayLastDate.Month < newTaxiOpenDate.Month)    //aded 29/10/2014
                            {
                                areasMonth = newTaxiOpenDate.Month - PayLastDate.Month;//aded 29/10/2014
                            }
                            //********************************************************************
                            else if (PayLastDate.Month == newTaxiOpenDate.Month)
                            {
                                areasMonth = 0;
                            }
                            else
                            {
                                areasMonth = DateTime.Now.Month - PayLastDate.Month;
                            }

                            //to display the bil recovered month
                            if (areasMonth > 0)
                            {
                                AreasMontDetail(dgv3, PayLastDate, areasMonth, DateTime.Now.Year);
                            }
                            return areasPhoneBill = areasMonth * phoneBil;
                        }
                        else//for difference year
                        {
                            areasMonth = (12 - PayLastDate.Month) + newTaxiOpenDate.Month;  //DateTime.Now.Month; //12 for months for year 
                            AreasMontDetail(dgv3, PayLastDate, areasMonth, DateTime.Now.Year);
                            return areasPhoneBill = areasMonth * phoneBil;
                        }
                    }//already paid the phone bill
                    else
                    {
                        return areasPhoneBill = 0;
                    }
                }
                else //for new taxi
                {
                    // if (DateTime.Now.Day >= 20 && DateTime.Now.Day<25)//if date is less than 25, charge phone bill for that month 
                    if (newTaxiDateOpen.Day >= 20 && newTaxiDateOpen.Day < 25)
                    {
                        areasMonth = 0.5; //areas month 1 for current month

                        PayLastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//no last payment date, this is new cab. so to get previous month 
                        PayLastDate = PayLastDate.AddDays(-1);// we send previous month, then (prevoius month++) get curent month

                        AreasMontDetail(dgv3, PayLastDate, areasMonth, DateTime.Now.Year);
                        return areasPhoneBill = ph20Bill;

                    }
                    //else if (DateTime.Now.Day >= 25 && DateTime.Now.Day < 31) 
                    else if (newTaxiDateOpen.Day >= 25 && newTaxiDateOpen.Day < 31)
                    {
                        areasMonth = 0.25; //areas month 1 for current month

                        PayLastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//no last payment date, this is new cab. so to get previous month 
                        PayLastDate = PayLastDate.AddDays(-1);// we send previous month, then (prevoius month++) get curent month

                        AreasMontDetail(dgv3, PayLastDate, areasMonth, DateTime.Now.Year);
                        return areasPhoneBill = ph25Bill;
                    }
                    else if (newTaxiOpenDate.Month == (DateTime.Now.Month) + 1)
                    {
                        areasMonth = 1;
                        PayLastDate = DateTime.Now.Date;
                        AreasMontDetail(dgv3, PayLastDate, areasMonth, DateTime.Now.Year);
                        return areasPhoneBill = phoneBil;
                    }
                    else //if date is grater than 25 ignore phone bill
                    {
                        areasMonth = 1;

                        PayLastDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);//no last payment date, this is new cab. so to get previous month 
                        PayLastDate = PayLastDate.AddDays(-1);

                        AreasMontDetail(dgv3, PayLastDate, areasMonth, DateTime.Now.Year);
                        return areasPhoneBill = phoneBil;
                    }
                }
            }
            
            else
            {
                return 0.0;
            }
        }
            else{return 0.0;}
            //}
            //return 0.0;
        }


        public void checkAppRental(TextBox tbCabNo,DateTimePicker dtStartDate,TextBox tbAppRent,DataGridView dgv,CheckBox chbIgnoreRental,TextBox tbTotAmount,CheckBox chbNewTaxi,CheckBox chbIgnoreAppAreas,Label lbMonth) 
        {
            if (chbNewTaxi.Checked == false && chbIgnoreAppAreas.Checked==false)
            {

                if (chbIgnoreRental.Checked == false)
                {
                    int currentMonth = dtStartDate.Value.Month;
                    int CurrentYear = dtStartDate.Value.Year;
                    MySqlConnection connection = new MySqlConnection(constr);
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT CabNo FROM TestAppRental WHERE ((CabNo='" + tbCabNo + "' AND (Month='" + currentMonth + "' AND Year='" + CurrentYear + "' )) AND Flag='N')";


                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {                       

                        tbAppRent.Text = "0";
                        connection.Close();

                    }
                    else
                    {
                        //string fixedDate = "4/1/2018";
                        //string dateTimeNow = DateTime.Now.ToShortDateString();
                        //if (DateTime.Parse(dateTimeNow) > DateTime.Parse(fixedDate))
                        //{
                            findLastAppRentPaid(tbCabNo, tbAppRent, currentMonth, CurrentYear, dgv, tbTotAmount, dtStartDate, lbMonth);
                        //}
                        //else 
                        //{
                        //    appRentalOnlyCurrentMonth(dtStartDate, dgv, tbTotAmount, tbAppRent, lbMonth); 
                        //}
                       
                    }
                    connection.Close();
                }
            }
            else 
            {
                appRentalOnlyCurrentMonth(dtStartDate, dgv, tbTotAmount, tbAppRent, lbMonth); 
                //if (dtStartDate.Value.Day < 25)
                //{
                //    string monthName = "";
                //    dgv.Rows.Add(1);
                //    monthName = monthNameInWords(DateTime.Now.Month);
                //    dgv.Rows[0].Cells[0].Value = monthName;
                //    dgv.Rows[0].Cells[1].Value = DateTime.Now.Year.ToString();
                //    dgv.Rows[0].Cells[2].Value = appRent.ToString();
                //    dgv.Rows[0].Cells[3].Value = (DateTime.Now.Month).ToString();

                //    calTotalAppRental(dgv, tbAppRent, tbTotAmount,lbMonth);
                //}
                //else 
                //{
                //    dgv.Rows.Clear();
                //    tbAppRent.Text = "0"; 

                //}
            }
        }


        public void appRentalOnlyCurrentMonth(DateTimePicker dtStartDate,DataGridView dgv,TextBox tbTotAmount,TextBox tbAppRent,Label lbMonth) 
        {
            //if (dtStartDate.Value.Day < 25)
            //{
                string monthName = "";
                dgv.Rows.Add(1);
                monthName = monthNameInWords(DateTime.Now.Month);
                dgv.Rows[0].Cells[0].Value = monthName;
                dgv.Rows[0].Cells[1].Value = DateTime.Now.Year.ToString();
                dgv.Rows[0].Cells[2].Value = appRent.ToString();
                dgv.Rows[0].Cells[3].Value = (DateTime.Now.Month).ToString();

                calTotalAppRental(dgv, tbAppRent, tbTotAmount, lbMonth);
            //}
            //else
            //{
            //    dgv.Rows.Clear();
            //    tbAppRent.Text = "0";

            //}
        }
        public void findLastAppRentPaid(TextBox tbCabNo,TextBox tbAppRent,int currentMonth,int currentYear, DataGridView dgv,TextBox tbTotAmount, DateTimePicker dtStartDate,Label lbMonth) 
        {
            int paidMonth = 0; int paidYear = 0; string monthName = "";
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT CabNo,Month,Year FROM TestAppRentSum WHERE (CabNo='" + tbCabNo.Text + "' AND Flag='N')";
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    paidMonth = Convert.ToInt32(reader["Month"].ToString());
                    paidYear = Convert.ToInt32(reader["Year"].ToString());
                }
                connection.Close();
                AreasAppRentMontDisplay(paidMonth, paidYear, currentMonth, currentYear, dgv,tbAppRent,tbTotAmount,lbMonth);

            }
            else
            {
                if (dtStartDate.Value.Day < 25)
                {

                    monthName = monthNameInWords(currentMonth);
                    dgv.Rows.Add(1);
                    dgv.Rows[0].Cells[0].Value = monthName.ToString();
                    dgv.Rows[0].Cells[1].Value = currentYear.ToString();
                    dgv.Rows[0].Cells[2].Value = appRent.ToString();
                    dgv.Rows[0].Cells[3].Value = currentMonth.ToString();

                    calTotalAppRental(dgv, tbAppRent, tbTotAmount,lbMonth);
                }
                else 
                {
                    dgv.Rows.Clear();
                    tbAppRent.Text = "0"; 
                }

            }
        }

      
        public void AreasAppRentMontDisplay(int paidMonth, int PaidYear, int currentMonth, int currentYear, DataGridView dgv,TextBox tbAppRent,TextBox tbTotAmount,Label lbMonth)
        {
            int monthDif = 0; int yearDif =0; string monthName="";

            yearDif=currentYear-PaidYear;
            dgv.Rows.Add(12);

            if (yearDif==0)//current year
            {
                monthDif = currentMonth - paidMonth;
                if (monthDif == 1)
                {
                    //dgv.Rows.Add(monthDif);
                    monthName = monthNameInWords(currentMonth);
                   
                    dgv.Rows[0].Cells[0].Value = monthName.ToString();
                    dgv.Rows[0].Cells[1].Value = currentYear.ToString();
                    dgv.Rows[0].Cells[2].Value = appRent.ToString();
                    dgv.Rows[0].Cells[3].Value = currentMonth.ToString();
                }
                else if (monthDif > 1)
                {
                    //dgv.Rows.Add(monthDif);
                    for (int i = 0; i < monthDif; i++)
                    {
                        monthName = monthNameInWords(paidMonth + (i + 1));
                        dgv.Rows[i].Cells[0].Value = monthName.ToString();
                        dgv.Rows[i].Cells[1].Value = currentYear.ToString();
                        dgv.Rows[i].Cells[2].Value = appRent.ToString();
                        dgv.Rows[i].Cells[3].Value = (paidMonth + (i + 1)).ToString();
                    }
                }
            }
            else 
            {
               
                if (yearDif == 1)//lastyear
                {
                    monthDif = 12 - paidMonth;
                    //dgv.Rows.Add(monthDif);
                    for (int i = 0; i < monthDif; i++)//last year Ares
                    {
                        monthName = monthNameInWords(paidMonth + (i + 1));
                        dgv.Rows[i].Cells[0].Value = monthName.ToString();
                        dgv.Rows[i].Cells[1].Value = PaidYear.ToString();
                        dgv.Rows[i].Cells[2].Value = appRent.ToString();
                        dgv.Rows[i].Cells[3].Value = (paidMonth + (i + 1)).ToString();
                    }

                    int k = 0; int l = 1;
                    for (int j = currentMonth; j > 0; j--) //this year Ares
                    {
                        
                        monthName = monthNameInWords(l);
                        dgv.Rows[monthDif+k].Cells[0].Value = monthName.ToString();
                        dgv.Rows[monthDif + k].Cells[1].Value = currentYear.ToString();
                        dgv.Rows[monthDif + k].Cells[2].Value = appRent.ToString();
                        dgv.Rows[monthDif + k].Cells[3].Value = l.ToString();
                        k++; l++;
                        //dgv.Rows.Add(k);

                    }
                }               
                               

            }
            calTotalAppRental(dgv,tbAppRent,tbTotAmount, lbMonth);
          

        }

        public void calTotalAppRental(DataGridView dgv,TextBox tbAppRent,TextBox tbTotAmount,Label lbMonths)
        {
            int rental = 0; string monthName = ""; string monthFeild = "";
            if (dgv.Rows.Count > 1) 
            {
                for (int i = 0; i < dgv.Rows.Count - 1; i++) 
                {
                    if (dgv.Rows[i].Cells[2].Value != null)
                    {
                        monthName = dgv.Rows[i].Cells[0].Value.ToString();
                        rental = rental + Convert.ToInt32(dgv.Rows[i].Cells[2].Value.ToString());
                        monthFeild=monthFeild+","+ monthName;
                    }
                }

            }
            tbAppRent.Text = rental.ToString();
            lbMonths.Text = monthFeild;
            //tbTotAmount.Text = (Convert.ToInt32(tbAppRent.Text) + Convert.ToInt32(tbTotAmount.Text)).ToString();

             //for (int i = 0; i <= dgv5.RowCount - 1; i++)
             //   {
             //       if (dgv5.Rows[i].Cells[0].Value != null)
             //       {
             //           monthName = dgv5.Rows[i].Cells[0].Value.ToString();
             //           Year = Convert.ToInt32(dgv5.Rows[i].Cells[1].Value.ToString());
             //           amount = Convert.ToInt32(dgv5.Rows[i].Cells[2].Value.ToString());
             //           month = Convert.ToInt32(dgv5.Rows[i].Cells[3].Value.ToString());
             //           //monthFeild = monthFeild + "," + billMonth;
        }


        public void saveAppRental(string recno, TextBox tbCabNo,TextBox tbAmount,Label lbReason ,DataGridView dgv5,string user,string location,string date,string datetime )
        {
            string monthName = ""; int month = 0; int Year = 0; int amount = 0; int k = 0;

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            MySqlCommand command3 = connection.CreateCommand();
            if (dgv5.Rows[0].Cells[0].Value != null || dgv5.Rows[1].Cells[0].Value != null)
            {

                for (int i = 0; i <= dgv5.RowCount - 1; i++)
                {
                    if (dgv5.Rows[i].Cells[0].Value != null )
                    {
                        monthName = dgv5.Rows[i].Cells[0].Value.ToString();
                        Year = Convert.ToInt32(dgv5.Rows[i].Cells[1].Value.ToString());
                        amount = Convert.ToInt32(dgv5.Rows[i].Cells[2].Value.ToString());
                        month = Convert.ToInt32(dgv5.Rows[i].Cells[3].Value.ToString());
                        //monthFeild = monthFeild + "," + billMonth;

                        command1.CommandText = "INSERT INTO TestAppRental (RecNo,CabNo,Amount,MontName,	Month,Year,Date,DateTime,User,Location,Flag) VALUES('" + recno + "','" + tbCabNo.Text + "','" + amount + "','" + monthName + "','" + month + "','" + Year + "','" + date + "','" + datetime + "','" + user + "','" + location + "','N')";
                        command1.ExecuteNonQuery();
                        k++;
                    }
                }

               
                Year = Convert.ToInt32(dgv5.Rows[k-1].Cells[1].Value.ToString());
                amount = Convert.ToInt32(dgv5.Rows[k-1].Cells[2].Value.ToString());
                month = Convert.ToInt32(dgv5.Rows[k-1].Cells[3].Value.ToString());

            command2.CommandText = "SELECT CabNo FROM TestAppRentSum WHERE (CabNo='" + tbCabNo.Text + "' AND Flag='N')";
            var reader = command2.ExecuteReader();
            if (reader.HasRows)
            {
                connection.Close();
                command3.CommandText = "UPDATE TestAppRentSum SET RecNo='" + recno + "',CabNo='" + tbCabNo.Text + "',Month='" + month + "',Year='" + Year + "',Amount='" + amount + "',Flag='N' WHERE CabNo='"+tbCabNo.Text+"'";
                connection.Open();
                command3.ExecuteNonQuery();
            }
            else 
            {
                connection.Close();
                command3.CommandText = "INSERT INTO TestAppRentSum (RecNo,CabNo,Month,Year,Amount,Flag) VALUES('" + recno + "','" + tbCabNo.Text + "','" + month + "','" + Year + "','" + amount + "','N')";
                connection.Open();
                command3.ExecuteNonQuery();
            }

                connection.Close();
            }
        }

        public bool findCabNoToIgnorePhoneBill(TextBox tbCabNo,CheckBox chbPersonalSIM) 
        {

            string cabno = tbCabNo.Text;

            MySqlConnection connection = new MySqlConnection(constr2);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT CabNo FROM CarOwner WHERE ((CabNo='" + cabno + "' AND wflag='AC') AND  sim_office='N')";


            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
               
                connection.Close();
                //MessageBox.Show("ජංගම දුරකතන බිල සඳහා මුදල් අය කරන්නේ නැත !!");
                chbPersonalSIM.Checked = true;
                return true;

            }
            connection.Close();

            return false;


            return false;
 
        }

        public string AmountToWords(int amount)
        {
            if (amount == 0) return "Zero";
            if (amount == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (amount < 0)
            {
                sb.Append("Minus ");
                amount = -amount;
            }
            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
                 "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
                "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
                "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = amount % 1000; // units
            num[1] = amount / 1000;
            num[2] = amount / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = amount / 10000000; // crores
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
            return sb.ToString().TrimEnd() + " " + "Rupees onlly";
        }

        public bool getSpecialPaymentCab(TextBox tbCabNo, Label lbSpecial, CheckBox chbSpecial)
        {
            string cabno = tbCabNo.Text; string date = ""; string boperater = "";

            MySqlConnection connection = new MySqlConnection(constr2);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT CabNo FROM CarOwner WHERE (CabNo='" + cabno + "' AND UnmarkTaxi='Y') AND( wflag='AC') ";


            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                chbSpecial.Checked = true;
                chbSpecial.Visible = true;

                lbSpecial.Text = "LKR 350.00 Per Day(Unmark)";
                lbSpecial.Visible = true;
                //reader.Read();
                //date = reader["Date"].ToString();

                //reader.Read();
                //boperater = reader["Boperator"].ToString();

                //connection.Close();
                MessageBox.Show("This Cab Is a Unmark Cab, you should charge LKR 350.00 per Day and Mobitel Bill Ex- For 15 Days With Phone Bill - LKR 5650.00,  For 30 days with Phone bill 10900. No free Days ");
                return true;

            }
            connection.Close();
            return false;
        }

        //public bool getPhoneNumberFromRegister(TextBox tbCabNo, Label lbSpecial, CheckBox chbSpecial)
        //{
        //    string cabno = tbCabNo.Text; string date = ""; string boperater = "";

        //    MySqlConnection connection = new MySqlConnection(constr2);
        //    connection.Open();
        //    MySqlCommand command = connection.CreateCommand();
        //    command.CommandText = "SELECT CabNo FROM CarOwner WHERE CabNo='" + cabno + "' AND UnmarkTaxi='Y') ";


        //    var reader = command.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        chbSpecial.Checked = true;
        //        chbSpecial.Visible = true;

        //        lbSpecial.Text = "LKR 350.00 Per Day(Unmark)";
        //        lbSpecial.Visible = true;
        //        //reader.Read();
        //        //date = reader["Date"].ToString();

        //        //reader.Read();
        //        //boperater = reader["Boperator"].ToString();

        //        //connection.Close();
        //        MessageBox.Show("This Cab Is a Unmark Cab, you should charge LKR 350.00 per Day and Mobitel Bill Ex- For 15 Days With Phone Bill - LKR 5650.00,  For 30 days with Phone bill 10900. No free Days ");
        //        return true;

        //    }
        //    connection.Close();
        //    return false;
        //}

        public int calculateWorkingDays(int paidAmount, int areas, DateTime lastdate, DateTime startDate, TextBox tbNofDay, System.Windows.Forms.TextBox tbBal, System.Windows.Forms.TextBox tbShort, System.Windows.Forms.TextBox tbrequired, System.Windows.Forms.DataGridView dgv3, TextBox tbServiceProvider, CheckBox chb3, DateTime lastFreeDate, CheckBox chOneday, CheckBox chbBranding,DataGridView dgvDates,TextBox tbCabNo,CheckBox chbPresonalSIM,TextBox tbTotalPhoneBill,TextBox tbTotalMonth,TextBox tbTotalPhoneBillToRecover)
        {
            int shortAmount = 0;
            int balance = 0;
            int balanceTobeRefund = 0;
            int commingNextMonth = 0;
            int commingNextMonthPhoneBil = 0;
            int tempMonthDiff = 0;
            bool resul=true;
         
          
            if (((lastdate.Date < startDate.Date) && (lastFreeDate.Date < startDate.Date)) || (chOneday.Checked == true))
            {
                if (AvoidOneDayDuplicate(dgvDates, startDate, chOneday) == true)
                {


                    if (tbServiceProvider.Text == "Mobitel" && chb3.Checked == false)
                    {

                        if (startDate > lastdate)
                        {
                            if (paidAmount >= (areas + (perDayCharge * minimumDays)))
                            {
                                balance = paidAmount - areas;

                                workingDays = (balance - (balance % perDayCharge)) / perDayCharge;
                                balanceTobeRefund = balance % perDayCharge;


                                if (startDate.Year == (startDate.AddDays(workingDays - 1)).Year)//workingDays-1 (-1 for starting day)
                                {
                                    if (startDate.Month == startDate.AddDays(workingDays - 1).Month)
                                    {
                                        workingDays = (balance - (balance % perDayCharge)) / perDayCharge;
                                        balanceTobeRefund = balance % perDayCharge;

                                        tbBal.Text = balanceTobeRefund.ToString();

                                        incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPresonalSIM);
                                        tbShort.Text = "0";
                                        tbrequired.Text = "0";
                                        tbNofDay.Text = workingDays.ToString();
                                        return workingDays;
                                    }
                                    else //not in same month
                                    {

                                        if (findCabNoToIgnorePhoneBill(tbCabNo,chbPresonalSIM) == false) // ignore next month phone bill for special cab
                                        {

                                            commingNextMonth = startDate.AddDays(workingDays - 1).Month - startDate.Month;
                                            commingNextMonthPhoneBil = commingNextMonth * phoneBil;
                                        }

                                        //*********************for testing**********************************
                                        if (commingNextMonth >= 2)
                                        {

                                            for (int i = 0; i < commingNextMonth; i++)
                                            {
                                                balance = balance - phoneBil;

                                                workingDays = (balance - (balance % perDayCharge)) / perDayCharge;
                                                balanceTobeRefund = balance % perDayCharge;
                                                tempMonthDiff = startDate.AddDays(workingDays - 1).Month - startDate.Month;

                                                if (commingNextMonth == tempMonthDiff)
                                                {

                                                }
                                                else
                                                {
                                                    if (balance > 0)
                                                    {
                                                        shortAmount = (phoneBil + perDayCharge) - balanceTobeRefund;
                                                        tbShort.Text = shortAmount.ToString();
                                                        tbrequired.Text = (paidAmount + shortAmount).ToString();
                                                        System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                                        return 0;
                                                    }
                                                    else
                                                    {
                                                        incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPresonalSIM);
                                                        tbNofDay.Text = workingDays.ToString();
                                                        balanceTobeRefund = balance % perDayCharge;
                                                        tbBal.Text = balanceTobeRefund.ToString();
                                                        return workingDays;
                                                    }
                                                }

                                            }
                                            incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPresonalSIM);
                                            balanceTobeRefund = balance % perDayCharge;
                                            tbBal.Text = balanceTobeRefund.ToString();
                                            tbNofDay.Text = workingDays.ToString();
                                            return workingDays;
                                        }

                                        //testing 27/08/2012***********************************
                                        //if (commingNextMonth == 1) 
                                        //{
                                        //    balance = balance - phoneBil;
                                        //    workingDays = (balance - (balance % perDayCharge)) / perDayCharge;
                                        //    balanceTobeRefund = balance % perDayCharge;

                                            //if (workingDays < 10) 
                                        //{
                                        //    shortAmount = (perDayCharge * 10) - balance;
                                        //    tbShort.Text = shortAmount.ToString();
                                        //    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                        //    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                        //    return 0;
                                        //}
                                        //else
                                        //    {
                                        //        incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3);
                                        //        tbNofDay.Text = workingDays.ToString();
                                        //        balanceTobeRefund = balance % perDayCharge;
                                        //        tbBal.Text = balanceTobeRefund.ToString();
                                        //        return workingDays;
                                        //    }

                                        //}//end of testing 27/08/2012***************************************************

                                        else//this else for testing code
                                        {//this scope for testing but onle for "else" word.. code is ok

                                            if ((((startDate.AddDays(workingDays - 1).Day) * perDayCharge) + balanceTobeRefund) >= (commingNextMonthPhoneBil + perDayCharge))//reduce some days and dedcuct phone bill
                                            {
                                                balance = balance - (commingNextMonthPhoneBil);
                                                workingDays = (balance - (balance % perDayCharge)) / perDayCharge;

                                                if (workingDays >= minimumDays)
                                                {
                                                    balanceTobeRefund = balance % perDayCharge;

                                                    tbBal.Text = balanceTobeRefund.ToString();
                                                    tbShort.Text = "0";
                                                    tbrequired.Text = "0";

                                                    incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPresonalSIM);
                                                    tbNofDay.Text = workingDays.ToString();
                                                    tbBal.Text = balanceTobeRefund.ToString();
                                                    return workingDays;
                                                }
                                                else
                                                {
                                                    shortAmount = (perDayCharge * minimumDays) - balance;
                                                    tbShort.Text = shortAmount.ToString();
                                                    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                                    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                                    return 0;
                                                }

                                            }

                                            else //get from payee
                                            {

                                                // shortAmount = ((commingNextMonth * phoneBil) + perDayCharge) - (((startDate.AddDays(workingDays - 1).Day) * perDayCharge)+balanceTobeRefund);
                                                shortAmount = (commingNextMonth * phoneBil) - balanceTobeRefund;
                                                tbShort.Text = shortAmount.ToString();
                                                tbrequired.Text = (paidAmount + shortAmount).ToString();
                                                System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                                return 0;
                                            }

                                        }
                                    }//this scope for testing code
                                }
                                else //if not in same yaer
                                {
                                    if (findCabNoToIgnorePhoneBill(tbCabNo,chbPresonalSIM) == false)
                                    {
                                        commingNextMonth = (12 - startDate.Month) + startDate.AddDays(workingDays - 1).Month;
                                        commingNextMonthPhoneBil = commingNextMonth * phoneBil;
                                    }

                                    if ((((startDate.AddDays(workingDays - 1).Day) * perDayCharge) + balanceTobeRefund) >= (commingNextMonthPhoneBil + perDayCharge))
                                    {
                                        balance = balance - (commingNextMonthPhoneBil);
                                        workingDays = (balance - (balance % perDayCharge)) / perDayCharge;
                                        if (workingDays >= minimumDays)
                                        {
                                            balanceTobeRefund = balance % perDayCharge;

                                            tbBal.Text = balanceTobeRefund.ToString();
                                            tbShort.Text = "0";
                                            tbrequired.Text = "0";
                                            incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPresonalSIM);
                                            tbNofDay.Text = workingDays.ToString();
                                            return workingDays;
                                        }
                                        else
                                        {
                                            //shortAmount = ((perDayCharge * 10)+phoneBil) - balance;/19/12/2012 YEAR PROBLEM
                                            shortAmount = ((perDayCharge * minimumDays) + phoneBil) - paidAmount;
                                            tbShort.Text = shortAmount.ToString();
                                            tbrequired.Text = (paidAmount + shortAmount).ToString();
                                            System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                            return 0;
                                        }


                                    }
                                    else
                                    {
                                        shortAmount = ((commingNextMonth * phoneBil) + perDayCharge) - (((startDate.AddDays(workingDays - 1).Day) * perDayCharge) + balanceTobeRefund);
                                        tbShort.Text = shortAmount.ToString();
                                        tbrequired.Text = (paidAmount + shortAmount).ToString();
                                        System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                        return 0;
                                    }
                                }
                            }

                            else//paid amount not ok for to recover areas an min amount
                            {
                                if (startDate.Year == startDate.AddDays(minimumDays - 1).Year)//add 9 mean add 10 days.. if add 10, it will become 11 days,becoz with starting day
                                {
                                    if (startDate.Month == startDate.AddDays(minimumDays - 1).Month)
                                    {
                                        shortAmount = ((areas + (perDayCharge * minimumDays)) - paidAmount);
                                        tbShort.Text = shortAmount.ToString();
                                        tbrequired.Text = (paidAmount + shortAmount).ToString();
                                        System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                        return 0;
                                    }
                                    else
                                    {
                                        if (findCabNoToIgnorePhoneBill(tbCabNo,chbPresonalSIM) == false)
                                        {
                                            commingNextMonth = startDate.AddDays(minimumDays - 1).Month - startDate.Month;
                                            commingNextMonthPhoneBil = phoneBil * commingNextMonth;
                                        }

                                        shortAmount = ((areas + (perDayCharge * minimumDays)) + commingNextMonthPhoneBil) - paidAmount;

                                        tbShort.Text = shortAmount.ToString();
                                        tbrequired.Text = (paidAmount + shortAmount).ToString();
                                        System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                        return 0;
                                    }
                                }
                                else
                                {
                                    if (findCabNoToIgnorePhoneBill(tbCabNo,chbPresonalSIM) == false)
                                    {
                                        commingNextMonth = (12 - startDate.Month) + (startDate.AddDays(minimumDays).Month);
                                        commingNextMonthPhoneBil = commingNextMonth * phoneBil;
                                    }
                                    shortAmount = (areas + commingNextMonthPhoneBil + (perDayCharge * minimumDays)) - paidAmount;
                                    tbShort.Text = shortAmount.ToString();
                                    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                    return 0;
                                }

                            }
                        }
                        else//payment for missing date
                        {
                            if (paidAmount == perDayCharge)
                            {
                                tbBal.Text = balanceTobeRefund.ToString();
                                tbShort.Text = "0";
                                tbrequired.Text = "0";
                                tbNofDay.Text = "1";
                                return 1;
                            }
                            else
                            {
                                shortAmount = perDayCharge - paidAmount;
                                tbShort.Text = shortAmount.ToString();
                                tbrequired.Text = (paidAmount + shortAmount).ToString();
                                System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                return 0;
                            }
                        }
                    }
                    else if (tbServiceProvider.Text == "Etisalat" || chb3.Checked == true)
                    {
                        //return 0;
                        if (paidAmount >= (perDayCharge * minimumDays))
                        {
                            tbBal.Text = ((paidAmount % perDayCharge)).ToString();
                            workingDays = (paidAmount - Convert.ToInt32(tbBal.Text)) / perDayCharge;
                            tbNofDay.Text = workingDays.ToString();
                            tbShort.Text = "0";
                            tbrequired.Text = "0";

                            return workingDays;
                        }
                        else if (paidAmount == perDayCharge)
                        {
                            tbBal.Text = "0";
                            tbNofDay.Text = "1";
                            tbShort.Text = "0";
                            tbrequired.Text = "0";
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    MessageBox.Show("Already Paid !!!!!!!, Please select a another Date");
                    return 0;
                }
                
            }
            else
            {
                MessageBox.Show("Please Check Last Free Date And Last Paid Date");
                return 0;
            }
        }


        public void claculateTotalPhoneBil(DataGridView dgv3,TextBox tbTotalPhoneBil, TextBox tbToatalMonth,TextBox tbTotalPhoneBillToRecover)
        {
            int totalPhoneBil = 0;
            int totalMonth = 0; int m = 0;
            if (dgv3.Rows[0].Cells[0].Value != null)
            {
                for (int i = 0; i <= dgv3.RowCount - 1; i++)
                {
                    if (dgv3.Rows[i].Cells[0].Value != null)
                    {
                        totalPhoneBil = totalPhoneBil + Convert.ToInt32(dgv3.Rows[i].Cells[2].Value.ToString());                        
                        m++;
                    }
                }

                totalMonth = m;

                tbTotalPhoneBil.Text = totalPhoneBil.ToString();
                tbTotalPhoneBillToRecover.Text = totalPhoneBil.ToString();
                tbToatalMonth.Text = totalMonth.ToString();
            }
            else
            {
                totalPhoneBil = 0;
                tbTotalPhoneBil.Text = "0";
                tbTotalPhoneBillToRecover.Text = "0";
                tbToatalMonth.Text = "0";
            }

            
        }
        public bool AvoidOneDayDuplicate(DataGridView dgvDate,DateTime dtDate,CheckBox chonday) 
        {
            if (chonday.Checked == true)
            {
                string selectedDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dtDate));
                for (int i = 0; i< (dgvDate.RowCount-1) ; i++)
                {

                    string paidDate= String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dgvDate.Rows[i].Cells[1].Value.ToString()));
                    

                    if (paidDate==selectedDate)
                        return false;
                }
            }

            return true;

        }
        public int SpeccalculateWorkingDays(int paidAmount, int areas, DateTime lastdate, DateTime startDate, TextBox tbNofDay, System.Windows.Forms.TextBox tbBal, System.Windows.Forms.TextBox tbShort, System.Windows.Forms.TextBox tbrequired, System.Windows.Forms.DataGridView dgv3, TextBox tbServiceProvider, CheckBox chb3, DateTime lastFreeDate, CheckBox chOneday, CheckBox chbBranding,TextBox tbCabNo,CheckBox chbPersonalSIM)
        {
            int shortAmount = 0;
            int balance = 0;
            int balanceTobeRefund = 0;
            int commingNextMonth = 0;
            int commingNextMonthPhoneBil = 0;
            int tempMonthDiff = 0;

            if (((lastdate.Date < startDate.Date) && (lastFreeDate.Date < startDate.Date)) || (chOneday.Checked == true))
            {

                if (tbServiceProvider.Text == "Mobitel" && chb3.Checked == false)
                {

                    if (startDate > lastdate)
                    {
                        if (paidAmount >= (areas + (SpPerDayCharge * minimumDays)))
                        {
                            balance = paidAmount - areas;

                            workingDays = (balance - (balance % SpPerDayCharge)) / SpPerDayCharge;
                            balanceTobeRefund = balance % SpPerDayCharge;


                            if (startDate.Year == (startDate.AddDays(workingDays - 1)).Year)//workingDays-1 (-1 for starting day)
                            {
                                if (startDate.Month == startDate.AddDays(workingDays - 1).Month)
                                {
                                    workingDays = (balance - (balance % SpPerDayCharge)) / SpPerDayCharge;
                                    balanceTobeRefund = balance % SpPerDayCharge;

                                    tbBal.Text = balanceTobeRefund.ToString();

                                    incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPersonalSIM);
                                    tbShort.Text = "0";
                                    tbrequired.Text = "0";
                                    tbNofDay.Text = workingDays.ToString();
                                    return workingDays;
                                }
                                else //not in same month
                                {
                                    if (findCabNoToIgnorePhoneBill(tbCabNo,chbPersonalSIM) == false) // ignore next month phone bill for special cab
                                    {
                                        commingNextMonth = startDate.AddDays(workingDays - 1).Month - startDate.Month;
                                        commingNextMonthPhoneBil = commingNextMonth * phoneBil;
                                    }

                                    //*********************for testing**********************************
                                    if (commingNextMonth >= 2)
                                    {

                                        for (int i = 0; i < commingNextMonth; i++)
                                        {
                                            balance = balance - phoneBil;

                                            workingDays = (balance - (balance % SpPerDayCharge)) / SpPerDayCharge;
                                            balanceTobeRefund = balance % SpPerDayCharge;
                                            tempMonthDiff = startDate.AddDays(workingDays - 1).Month - startDate.Month;

                                            if (commingNextMonth == tempMonthDiff)
                                            {

                                            }
                                            else
                                            {
                                                if (balance > 0)
                                                {
                                                    shortAmount = (phoneBil + SpPerDayCharge) - balanceTobeRefund;
                                                    tbShort.Text = shortAmount.ToString();
                                                    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                                    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                                    return 0;
                                                }
                                                else
                                                {
                                                    incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPersonalSIM);
                                                    tbNofDay.Text = workingDays.ToString();
                                                    balanceTobeRefund = balance % SpPerDayCharge;
                                                    tbBal.Text = balanceTobeRefund.ToString();
                                                    return workingDays;
                                                }
                                            }

                                        }
                                        incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPersonalSIM);
                                        balanceTobeRefund = balance % SpPerDayCharge;
                                        tbBal.Text = balanceTobeRefund.ToString();
                                        tbNofDay.Text = workingDays.ToString();
                                        return workingDays;
                                    }

                                    //testing 27/08/2012***********************************
                                    //if (commingNextMonth == 1) 
                                    //{
                                    //    balance = balance - phoneBil;
                                    //    workingDays = (balance - (balance % perDayCharge)) / perDayCharge;
                                    //    balanceTobeRefund = balance % perDayCharge;

                                        //if (workingDays < 10) 
                                    //{
                                    //    shortAmount = (perDayCharge * 10) - balance;
                                    //    tbShort.Text = shortAmount.ToString();
                                    //    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                    //    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                    //    return 0;
                                    //}
                                    //else
                                    //    {
                                    //        incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3);
                                    //        tbNofDay.Text = workingDays.ToString();
                                    //        balanceTobeRefund = balance % perDayCharge;
                                    //        tbBal.Text = balanceTobeRefund.ToString();
                                    //        return workingDays;
                                    //    }

                                    //}//end of testing 27/08/2012***************************************************

                                    else//this else for testing code
                                    {//this scope for testing but onle for "else" word.. code is ok

                                        if ((((startDate.AddDays(workingDays - 1).Day) * SpPerDayCharge) + balanceTobeRefund) >= (commingNextMonthPhoneBil + SpPerDayCharge))//reduce some days and dedcuct phone bill
                                        {
                                            balance = balance - (commingNextMonthPhoneBil);
                                            workingDays = (balance - (balance % SpPerDayCharge)) / SpPerDayCharge;

                                            if (workingDays >= minimumDays)
                                            {
                                                balanceTobeRefund = balance % SpPerDayCharge;

                                                tbBal.Text = balanceTobeRefund.ToString();
                                                tbShort.Text = "0";
                                                tbrequired.Text = "0";

                                                incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPersonalSIM);
                                                tbNofDay.Text = workingDays.ToString();
                                                tbBal.Text = balanceTobeRefund.ToString();
                                                return workingDays;
                                            }
                                            else
                                            {
                                                shortAmount = (SpPerDayCharge * minimumDays) - balance;
                                                tbShort.Text = shortAmount.ToString();
                                                tbrequired.Text = (paidAmount + shortAmount).ToString();
                                                System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                                return 0;
                                            }

                                        }

                                        else //get from payee
                                        {

                                            // shortAmount = ((commingNextMonth * phoneBil) + perDayCharge) - (((startDate.AddDays(workingDays - 1).Day) * perDayCharge)+balanceTobeRefund);
                                            shortAmount = (commingNextMonth * phoneBil) - balanceTobeRefund;
                                            tbShort.Text = shortAmount.ToString();
                                            tbrequired.Text = (paidAmount + shortAmount).ToString();
                                            System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                            return 0;
                                        }

                                    }
                                }//this scope for testing code
                            }
                            else //if not in same yaer
                            {
                                if (findCabNoToIgnorePhoneBill(tbCabNo,chbPersonalSIM) == false) // ignore next month phone bill for special cab
                                {
                                    commingNextMonth = (12 - startDate.Month) + startDate.AddDays(workingDays - 1).Month;
                                    commingNextMonthPhoneBil = commingNextMonth * phoneBil;
                                }

                                if ((((startDate.AddDays(workingDays - 1).Day) * SpPerDayCharge) + balanceTobeRefund) >= (commingNextMonthPhoneBil + SpPerDayCharge))
                                {
                                    balance = balance - (commingNextMonthPhoneBil);
                                    workingDays = (balance - (balance % SpPerDayCharge)) / SpPerDayCharge;
                                    if (workingDays >= minimumDays)
                                    {
                                        balanceTobeRefund = balance % SpPerDayCharge;

                                        tbBal.Text = balanceTobeRefund.ToString();
                                        tbShort.Text = "0";
                                        tbrequired.Text = "0";
                                        incommingMonthPhoneBillDetails(commingNextMonth, startDate, startDate.AddDays(workingDays - 1), startDate.Year, dgv3, chbBranding,tbCabNo,chbPersonalSIM);
                                        tbNofDay.Text = workingDays.ToString();
                                        return workingDays;
                                    }
                                    else
                                    {
                                        //shortAmount = ((perDayCharge * 10)+phoneBil) - balance;/19/12/2012 YEAR PROBLEM
                                        shortAmount = ((SpPerDayCharge * minimumDays) + phoneBil) - paidAmount;
                                        tbShort.Text = shortAmount.ToString();
                                        tbrequired.Text = (paidAmount + shortAmount).ToString();
                                        System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                        return 0;
                                    }


                                }
                                else
                                {
                                    shortAmount = ((commingNextMonth * phoneBil) + SpPerDayCharge) - (((startDate.AddDays(workingDays - 1).Day) * SpPerDayCharge) + balanceTobeRefund);
                                    tbShort.Text = shortAmount.ToString();
                                    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                    return 0;
                                }
                            }
                        }

                        else//paid amount not ok for to recover areas an min amount
                        {
                            if (startDate.Year == startDate.AddDays(minimumDays - 1).Year)//add 9 mean add 10 days.. if add 10, it will become 11 days,becoz with starting day
                            {
                                if (startDate.Month == startDate.AddDays(minimumDays - 1).Month)
                                {
                                    shortAmount = ((areas + (SpPerDayCharge * minimumDays)) - paidAmount);
                                    tbShort.Text = shortAmount.ToString();
                                    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                    return 0;
                                }
                                else
                                {
                                    if (findCabNoToIgnorePhoneBill(tbCabNo,chbPersonalSIM) == false) // ignore next month phone bill for special cab
                                    {
                                        commingNextMonth = startDate.AddDays(minimumDays - 1).Month - startDate.Month;
                                        commingNextMonthPhoneBil = phoneBil * commingNextMonth;
                                    }

                                    shortAmount = ((areas + (SpPerDayCharge * minimumDays)) + commingNextMonthPhoneBil) - paidAmount;

                                    tbShort.Text = shortAmount.ToString();
                                    tbrequired.Text = (paidAmount + shortAmount).ToString();
                                    System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                    return 0;
                                }
                            }
                            else
                            {
                                if (findCabNoToIgnorePhoneBill(tbCabNo,chbPersonalSIM) == false) // ignore next month phone bill for special cab
                                {

                                    commingNextMonth = (12 - startDate.Month) + (startDate.AddDays(minimumDays).Month);
                                    commingNextMonthPhoneBil = commingNextMonth * phoneBil;
                                }
                                shortAmount = (areas + commingNextMonthPhoneBil + (SpPerDayCharge * minimumDays)) - paidAmount;
                                tbShort.Text = shortAmount.ToString();
                                tbrequired.Text = (paidAmount + shortAmount).ToString();
                                System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                                return 0;
                            }

                        }
                    }
                    else//payment for missing date
                    {
                        if (paidAmount == SpPerDayCharge)
                        {
                            tbBal.Text = balanceTobeRefund.ToString();
                            tbShort.Text = "0";
                            tbrequired.Text = "0";
                            tbNofDay.Text = "1";
                            return 1;
                        }
                        else
                        {
                            shortAmount = SpPerDayCharge - paidAmount;
                            tbShort.Text = shortAmount.ToString();
                            tbrequired.Text = (paidAmount + shortAmount).ToString();
                            System.Windows.Forms.MessageBox.Show("Your Short Amount is " + shortAmount);
                            return 0;
                        }
                    }
                }
                else if (tbServiceProvider.Text == "Etisalat" || chb3.Checked == true)
                {
                    //return 0;
                    if (paidAmount >= (SpPerDayCharge * minimumDays))
                    {
                        tbBal.Text = ((paidAmount % SpPerDayCharge)).ToString();
                        workingDays = (paidAmount - Convert.ToInt32(tbBal.Text)) / SpPerDayCharge;
                        tbNofDay.Text = workingDays.ToString();
                        tbShort.Text = "0";
                        tbrequired.Text = "0";

                        return workingDays;
                    }
                    else if (paidAmount == SpPerDayCharge)
                    {
                        tbBal.Text = "0";
                        tbNofDay.Text = "1";
                        tbShort.Text = "0";
                        tbrequired.Text = "0";
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("Please Check Last Free Date And Last Paid Date");
                return 0;
            }
        }


        public void gridFill(DateTime startDate, int noOfDays, System.Windows.Forms.DataGridView dgv, TextBox tbLastPayDate)
        {
            DateTime lastPay = Convert.ToDateTime(tbLastPayDate.Text);
            if (startDate > lastPay)
            {
                try
                {

                    dgv.Rows.Add(noOfDays - 1);

                    //dgv.Rows[0].Cells[0].Value = startDate.ToString("dd-MM-yyyy");
                    //dgv.Rows[0].Cells[0].Value = startDate.ToString("MM-dd-yyyy");
                    dgv.Rows[0].Cells[0].Value = startDate.ToShortDateString();
                    dgv.Rows[0].Cells[1].Value = startDate.ToString("dddd");
                    dgv.Rows[0].Cells[2].Value = "300";
                    dgv.Rows[0].Cells[3].Style.Font = new System.Drawing.Font("Wingdings", 15);
                    dgv.Rows[0].Cells[3].Value = ((char)254).ToString();

                    for (int r = 1; r < dgv.RowCount; r++)
                    {
                        startDate = startDate.AddDays(1);
                        DateTime lastdate = startDate;

                        if (startDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dgv.Rows[r].Cells[0].Style.BackColor = Color.Red;
                            dgv.Rows[r].Cells[1].Style.BackColor = Color.Red;
                        }

                        //dgv.Rows[r].Cells[0].Value = startDate.ToString("dd-MM-yyyy");
                        //dgv.Rows[r].Cells[0].Value = startDate.ToString("MM-dd-yyyy");
                        dgv.Rows[r].Cells[0].Value = startDate.ToShortDateString();
                        dgv.Rows[r].Cells[1].Value = startDate.ToString("dddd");
                        dgv.Rows[r].Cells[2].Value = "300";
                        dgv.Rows[r].Cells[3].Style.Font = new System.Drawing.Font("Wingdings", 15);
                        dgv.Rows[r].Cells[3].Value = ((char)254).ToString();

                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
            else if (startDate < lastPay && noOfDays == 1)
            {
                //dgv.Rows[0].Cells[0].Value = startDate.ToString("dd-MM-yyyy");
                //dgv.Rows[0].Cells[0].Value = startDate.ToString("MM-dd-yyyy");
                dgv.Rows[0].Cells[0].Value = startDate.ToShortDateString();
                dgv.Rows[0].Cells[1].Value = startDate.ToString("dddd");
                dgv.Rows[0].Cells[2].Value = "300";
                dgv.Rows[0].Cells[3].Style.Font = new System.Drawing.Font("Wingdings", 15);
                dgv.Rows[0].Cells[3].Value = ((char)254).ToString();
            }

        }

        public void SpecialGridFill(DateTime startDate, int noOfDays, System.Windows.Forms.DataGridView dgv, TextBox tbLastPayDate)
        {
            DateTime lastPay = Convert.ToDateTime(tbLastPayDate.Text);
            if (startDate > lastPay)
            {
                try
                {

                    dgv.Rows.Add(noOfDays - 1);

                    //dgv.Rows[0].Cells[0].Value = startDate.ToString("dd-MM-yyyy");
                    //dgv.Rows[0].Cells[0].Value = startDate.ToString("MM-dd-yyyy");
                    dgv.Rows[0].Cells[0].Value = startDate.ToShortDateString();
                    dgv.Rows[0].Cells[1].Value = startDate.ToString("dddd");
                    dgv.Rows[0].Cells[2].Value = "350";
                    dgv.Rows[0].Cells[3].Style.Font = new System.Drawing.Font("Wingdings", 15);
                    dgv.Rows[0].Cells[3].Value = ((char)254).ToString();

                    for (int r = 1; r < dgv.RowCount; r++)
                    {
                        startDate = startDate.AddDays(1);
                        DateTime lastdate = startDate;

                        if (startDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            dgv.Rows[r].Cells[0].Style.BackColor = Color.Red;
                            dgv.Rows[r].Cells[1].Style.BackColor = Color.Red;
                        }

                        //dgv.Rows[r].Cells[0].Value = startDate.ToString("dd-MM-yyyy");
                        //dgv.Rows[r].Cells[0].Value = startDate.ToString("MM-dd-yyyy");
                        dgv.Rows[r].Cells[0].Value = startDate.ToShortDateString();
                        dgv.Rows[r].Cells[1].Value = startDate.ToString("dddd");
                        dgv.Rows[r].Cells[2].Value = "350";
                        dgv.Rows[r].Cells[3].Style.Font = new System.Drawing.Font("Wingdings", 15);
                        dgv.Rows[r].Cells[3].Value = ((char)254).ToString();

                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
            else if (startDate < lastPay && noOfDays == 1)
            {
                //dgv.Rows[0].Cells[0].Value = startDate.ToString("dd-MM-yyyy");
                //dgv.Rows[0].Cells[0].Value = startDate.ToString("MM-dd-yyyy");
                dgv.Rows[0].Cells[0].Value = startDate.ToShortDateString();
                dgv.Rows[0].Cells[1].Value = startDate.ToString("dddd");
                dgv.Rows[0].Cells[2].Value = "350";
                dgv.Rows[0].Cells[3].Style.Font = new System.Drawing.Font("Wingdings", 15);
                dgv.Rows[0].Cells[3].Value = ((char)254).ToString();
            }

        }

        public void gridTickorUntick(DateTime startDate, int noOfDays, System.Windows.Forms.DataGridView dgv, int rfndBalnce, TextBox tbShort, TextBox tbReq, TextBox tbPaid, TextBox tbBal, DataGridView dgv3, TextBox amtWord, TextBox tbSun, DataGridView dgv4, CheckBox chpSpcl,CheckBox chbPersonalSIM)
        {
            bool sun1 = false;
            if (dgv.CurrentCell.ColumnIndex == 3)
            {
                int freeDayCounter = 0;
                int maxfreeDays = 0;
                string nightDate = "";

                maxfreeDays = ((noOfDays - (noOfDays % minimumDays)) / minimumDays) * 2; // "maximum freedays=2 (*2)"

                for (int i = 0; i < dgv.RowCount - 1; i++)
                {
                    if (dgv.Rows[i].Cells[3].Value.Equals(((char)253).ToString()))
                    {
                        freeDayCounter++;
                    }
                }

                if (dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value.Equals(((char)254).ToString()))
                {
                    if (freeDayCounter < maxfreeDays)
                    {

                        if ("Sunday" == dgv.Rows[dgv.CurrentRow.Index].Cells[1].Value.ToString())
                        {
                            if (7 > dgv.CurrentRow.Index)//within 7 days we can find a sunday
                            {

                                if (sundayOffWitrhLast(dgv, tbSun) == true)
                                {
                                    MessageBox.Show("You have to work on this Sunday");
                                }
                                else
                                {
                                    dgv.Rows[dgv.CurrentRow.Index].Cells[3].Style.ForeColor = Color.Red;
                                    dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)253).ToString();

                                    if (chpSpcl.Checked == true)//for special 350 cab
                                        SpecialgridRowAddOrRemove(dgv, noOfDays, rfndBalnce, tbShort, tbReq, tbPaid, tbBal, dgv3, amtWord, chpSpcl,chbPersonalSIM);
                                    else
                                        gridRowAddOrRemove(dgv, noOfDays, rfndBalnce, tbShort, tbReq, tbPaid, tbBal, dgv3, amtWord, chpSpcl,chbPersonalSIM);

                                    //geting night working date (if some day is free, before date is a night day)
                                    nightDate = dgv.Rows[dgv.CurrentRow.Index - 1].Cells[0].Value.ToString();
                                    identifyNightWorkingDays(nightDate, dgv4);
                                }
                            }
                            else if (7 <= dgv.CurrentRow.Index)// after 7 days definetely we can find a onother sunday
                            {
                                bool anotherSunday = findFirstSunday(dgv, dgv.CurrentRow.Index, tbSun);
                                if (anotherSunday == false)
                                {
                                    dgv.Rows[dgv.CurrentRow.Index].Cells[3].Style.ForeColor = Color.Red;
                                    dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)253).ToString();

                                    if (chpSpcl.Checked == true)//for special 350
                                        SpecialgridRowAddOrRemove(dgv, noOfDays, rfndBalnce, tbShort, tbReq, tbPaid, tbBal, dgv3, amtWord, chpSpcl,chbPersonalSIM);
                                    else
                                        gridRowAddOrRemove(dgv, noOfDays, rfndBalnce, tbShort, tbReq, tbPaid, tbBal, dgv3, amtWord, chpSpcl,chbPersonalSIM);

                                    //geting night working date (if some day is free, before date is a night day)
                                    nightDate = dgv.Rows[dgv.CurrentRow.Index - 1].Cells[0].Value.ToString();
                                    identifyNightWorkingDays(nightDate, dgv4);
                                }
                                else
                                {
                                    MessageBox.Show("You have have to work on this Sunday");
                                }
                            }

                        }

                        else
                        {
                            dgv.Rows[dgv.CurrentRow.Index].Cells[3].Style.ForeColor = Color.Red;
                            dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)253).ToString();

                            if (chpSpcl.Checked == true)
                                SpecialgridRowAddOrRemove(dgv, noOfDays, rfndBalnce, tbShort, tbReq, tbPaid, tbBal, dgv3, amtWord, chpSpcl,chbPersonalSIM);
                            else
                                gridRowAddOrRemove(dgv, noOfDays, rfndBalnce, tbShort, tbReq, tbPaid, tbBal, dgv3, amtWord, chpSpcl,chbPersonalSIM);

                            //geting night working date (if some day is free, before date is a night day)
                            nightDate = dgv.Rows[dgv.CurrentRow.Index - 1].Cells[0].Value.ToString();
                            identifyNightWorkingDays(nightDate, dgv4);
                        }

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("You are Exceeding Free Days");
                    }


                }
                else if (dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value.Equals(((char)253).ToString()))
                {
                    int r = dgv.RowCount;
                    DateTime tempdate = Convert.ToDateTime(dgv.Rows[r - 2].Cells[0].Value);

                    if (dgv.Rows[r - 2].Cells[3].Value.Equals(((char)253).ToString()))
                    {
                        tempdate = Convert.ToDateTime(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value);

                        dgv.Rows[dgv.CurrentRow.Index].Cells[3].Style.ForeColor = Color.Black;
                        dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();

                        dgv.Rows[r - 1].Cells[3].Style.ForeColor = Color.Red;
                        dgv.Rows[r - 1].Cells[3].Value = ((char)253).ToString();

                        for (int i = (dgv.RowCount - 1); i > 0; i--)
                        {
                            if (dgv.Rows[dgv.RowCount - 2].Cells[3].Value.Equals(((char)253).ToString()))
                            {
                                dgv.Rows.RemoveAt(dgv.RowCount - 2);// C# grid doesnt remove (rcont-1) row
                            }
                            else
                            {
                                i = -1;//exit from for loop
                            }
                        }
                        dgv.Rows.RemoveAt(dgv.RowCount - 2);

                        //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                        //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                        dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToShortDateString();
                        dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.ToString("dddd");
                        dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;
                        dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();

                    }
                    else
                    {
                        dgv.Rows[dgv.CurrentRow.Index].Cells[3].Style.ForeColor = Color.Black;
                        dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();
                        dgv.Rows.RemoveAt(r - 2);
                        //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                        //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                        dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToShortDateString();
                    }
                    refreshNightWorikngDays(dgv, dgv4);
                }


            }
        }

        public void gridRowAddOrRemove(System.Windows.Forms.DataGridView dgv, int wrkdays, int bal, TextBox tbshrt, TextBox tbreq, TextBox tbpay, TextBox tbbal, DataGridView dgv3, TextBox amtword, CheckBox chb7,CheckBox chbPersonalSIM)
        {
            int curentAmount = (wrkdays * perDayCharge) + bal;
            int shortAmount = 0;
            string txtAmount = "0";

            if (dgv.Rows[dgv.RowCount - 1].Cells[0].Value != null)
            {
                //
                DateTime tempdate = Convert.ToDateTime(dgv.Rows[dgv.RowCount - 1].Cells[0].Value);
                dgv.Rows.Add(1);
                if (tempdate.ToString() != "01-01-0001 12:00:00 AM")
                {

                    if (tempdate.Year == tempdate.AddDays(1).Year)
                    {
                        if (tempdate.Month == (tempdate.AddDays(1)).Month)//same month
                        {
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                            dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                            dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                            dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "300";
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                            if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                            {

                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                            }
                            else
                            {
                                // dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                // dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                            }
                        }
                        else //differance month
                        {
                            shortAmount = ((wrkdays * perDayCharge) + (((tempdate.AddDays(1)).Month - tempdate.Month) * phoneBil)) - curentAmount;

                            if (chbPersonalSIM.Checked == true)//for special cabs, no phone bill
                                shortAmount = 0;

                            if (shortAmount == 0)
                            {
                                //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                                // dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                                dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                                dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "300";
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                if (dgv.CurrentCell.RowIndex == (dgv.RowCount - 1))
                                {
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                                }
                                else
                                {

                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                                }

                                //phone bill table update
                                if (chbPersonalSIM.Checked == false)
                                setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                                //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                                //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                                //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();

                            }// short amount
                            else
                            {

                                //DialogResult dr = MessageBox.Show("Your Short Amount is  " + shortAmount + "\n If you want to Cancel, press YES " + "\n If not Press No", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                                //if (dr == DialogResult.Yes)
                                txtAmount = Microsoft.VisualBasic.Interaction.InputBox("Your Short Amount is " + shortAmount + "\n If you can Pay, press OK " + "\n If not Press Cancel", "Short Amount", "0");
                                if (txtAmount == "")
                                    txtAmount = "0";
                                if (Convert.ToInt32(txtAmount) == shortAmount)
                                {
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "300";
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                    if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                    {
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                        dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                        dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                        dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                        dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                                    }
                                    tbpay.Text = (Convert.ToInt32(tbpay.Text) + shortAmount).ToString();
                                    AmountToWords(Convert.ToInt32(tbpay.Text));
                                    tbbal.Text = "0";
                                    //phone bill table update  
                                    if (chbPersonalSIM.Checked == false)
                                    setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                                    //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                                    //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                                    //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();

                                }
                                else
                                {
                                    if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                    {
                                        dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;
                                    }
                                    else
                                    {
                                        dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                        dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();
                                        // dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                        //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                        dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToShortDateString();
                                        dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.ToString("dddd");
                                        dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "300";
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                    }
                                }

                            }
                        }

                    }
                    else// differance year 
                    {

                        shortAmount = ((curentAmount - (wrkdays * perDayCharge) + (((12 - tempdate.Month) + tempdate.AddDays(1).Month) * phoneBil)));//(curent amount-((workdays*300)+(newMonth*400)))
                        // if (shortAmount == 0)

                        if (chbPersonalSIM.Checked == true)//for special cabs, no phone bill
                        {
                            shortAmount = 0;
                            txtAmount = "0";
                        }

                        if(shortAmount>0)
                        txtAmount = Microsoft.VisualBasic.Interaction.InputBox("Your Short Amount is " + shortAmount + "\n If you can Pay, press OK " + "\n If not Press Cancel", "Short Amount", "0");

                        if (txtAmount == "")
                            txtAmount = "0";
                        if (Convert.ToInt32(txtAmount) == shortAmount)
                        {
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");                           
                            dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                            dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                            dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "300";
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                            if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                            {
                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                            }
                            else
                            {

                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                // dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                            }

                            tbpay.Text = (Convert.ToInt32(tbpay.Text) + shortAmount).ToString();
                            AmountToWords(Convert.ToInt32(tbpay.Text));
                            tbbal.Text = "0";

                            //mobitel bill update
                            if (chbPersonalSIM.Checked == false)
                            setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                            //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                            //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                            //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();
                        }
                        else
                        {
                            //DialogResult dr = MessageBox.Show("Your Short Amount is  " + shortAmount + "\n If you want to Cancel, press YES " + "\n If not Press No", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            //if (dr == DialogResult.Yes)
                            txtAmount = Microsoft.VisualBasic.Interaction.InputBox("Your Short Amount is " + shortAmount + "\n If you can Pay, press OK " + "\n If not Press Cancel", "Short Amount", "0");
                            if (txtAmount == "")
                                txtAmount = "0";
                            if (Convert.ToInt32(txtAmount) == shortAmount)
                            {
                                // dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                                //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                                dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                                dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "300";
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                {
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                                }
                                else//no
                                {

                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "300";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                                }
                                tbpay.Text = (Convert.ToInt32(tbpay.Text) + shortAmount).ToString();
                                AmountToWords(Convert.ToInt32(tbpay.Text) + shortAmount);
                                tbbal.Text = "0";
                                //mobitel bill update
                                if (chbPersonalSIM.Checked == false)
                                setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                                //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                                //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                                //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();
                            }
                            else
                            {
                                if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                {
                                    dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;
                                }
                                else
                                {
                                    dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                    dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "300";
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                }

                            }

                        }
                    }

                }
            }
        }

        public void SpecialgridRowAddOrRemove(System.Windows.Forms.DataGridView dgv, int wrkdays, int bal, TextBox tbshrt, TextBox tbreq, TextBox tbpay, TextBox tbbal, DataGridView dgv3, TextBox amtword,CheckBox chb7, CheckBox chbPersonalSIM)
        {
            int curentAmount = (wrkdays * SpPerDayCharge) + bal;
            int shortAmount = 0;
            string txtAmount = "0";

            //if (findCabNoToIgnorePhoneBill(tbCabNo, chbPersonalSIM) == true)
            //{
            //    phoneBil = 0;
            //}

            if (dgv.Rows[dgv.RowCount - 1].Cells[0].Value != null)
            {
                //
                DateTime tempdate = Convert.ToDateTime(dgv.Rows[dgv.RowCount - 1].Cells[0].Value);
                dgv.Rows.Add(1);
                if (tempdate.ToString() != "01-01-0001 12:00:00 AM")
                {

                    if (tempdate.Year == tempdate.AddDays(1).Year)
                    {
                        if (tempdate.Month == (tempdate.AddDays(1)).Month)//same month
                        {
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                            dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                            dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                            dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "350";
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                            if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                            {

                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                            }
                            else
                            {
                                // dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                // dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                            }
                        }
                        else //differance month
                        {
                            

                            shortAmount = ((wrkdays * SpPerDayCharge) + (((tempdate.AddDays(1)).Month - tempdate.Month) * phoneBil)) - curentAmount;
                            if (chbPersonalSIM.Checked == true)
                                shortAmount = 0;
                            if (shortAmount == 0)
                            {
                                //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                                // dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                                dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                                dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "350";
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                if (dgv.CurrentCell.RowIndex == (dgv.RowCount - 1))
                                {
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                                }
                                else
                                {

                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                                }

                                //phone bill table update
                                if (chbPersonalSIM.Checked == false)
                                setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                                //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                                //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                                //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();

                            }// short amount
                            else
                            {

                                //DialogResult dr = MessageBox.Show("Your Short Amount is  " + shortAmount + "\n If you want to Cancel, press YES " + "\n If not Press No", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                                //if (dr == DialogResult.Yes)
                                txtAmount = Microsoft.VisualBasic.Interaction.InputBox("Your Short Amount is " + shortAmount + "\n If you can Pay, press OK " + "\n If not Press Cancel", "Short Amount", "0");
                                if (txtAmount == "")
                                    txtAmount = "0";
                                if (Convert.ToInt32(txtAmount) == shortAmount)
                                {
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "350";
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                    if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                    {
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                        dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                        dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                        //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                        dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                        dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                        dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                                    }
                                    tbpay.Text = (Convert.ToInt32(tbpay.Text) + shortAmount).ToString();
                                    AmountToWords(Convert.ToInt32(tbpay.Text));
                                    tbbal.Text = "0";
                                    //phone bill table update 
 
                                    if (chbPersonalSIM.Checked == false)
                                    setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                                    //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                                    //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                                    //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();

                                }
                                else
                                {
                                    if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                    {
                                        dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;
                                    }
                                    else
                                    {
                                        dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                        dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();
                                        // dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                        //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                        dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToShortDateString();
                                        dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.ToString("dddd");
                                        dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "350";
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                        dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                    }
                                }

                            }
                        }

                    }
                    else// differance year 
                    {

                        shortAmount = ((curentAmount - (wrkdays * SpPerDayCharge) + (((12 - tempdate.Month) + tempdate.AddDays(1).Month) * phoneBil)));//(curent amount-((workdays*300)+(newMonth*400)))
                        // if (shortAmount == 0)
                        if (chbPersonalSIM.Checked == true)
                        {
                            shortAmount = 0;
                            txtAmount = "0";
                        }
                        if(shortAmount>0)
                        txtAmount = Microsoft.VisualBasic.Interaction.InputBox("Your Short Amount is " + shortAmount + "\n If you can Pay, press OK " + "\n If not Press Cancel", "Short Amount", "0");

                        if (txtAmount == "")
                            txtAmount = "0";
                        if (Convert.ToInt32(txtAmount) == shortAmount)
                        {
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                            //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");                           
                            dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                            dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                            dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "350";
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                            dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                            if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                            {
                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                            }
                            else
                            {

                                //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                // dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                            }

                            tbpay.Text = (Convert.ToInt32(tbpay.Text) + shortAmount).ToString();
                            AmountToWords(Convert.ToInt32(tbpay.Text));
                            tbbal.Text = "0";

                            //mobitel bill update
                            if (chbPersonalSIM.Checked == false)
                            setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                            //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                            //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                            //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();
                        }
                        else
                        {
                            //DialogResult dr = MessageBox.Show("Your Short Amount is  " + shortAmount + "\n If you want to Cancel, press YES " + "\n If not Press No", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            //if (dr == DialogResult.Yes)
                            txtAmount = Microsoft.VisualBasic.Interaction.InputBox("Your Short Amount is " + shortAmount + "\n If you can Pay, press OK " + "\n If not Press Cancel", "Short Amount", "0");
                            if (txtAmount == "")
                                txtAmount = "0";
                            if (Convert.ToInt32(txtAmount) == shortAmount)
                            {
                                // dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("dd-MM-yyyy");
                                //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToString("MM-dd-yyyy");
                                dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.AddDays(1).ToShortDateString();
                                dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.AddDays(1).ToString("dddd");
                                dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "350";
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                {
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)253).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Red;
                                }
                                else//no
                                {

                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 2].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 2].Cells[2].Value = "350";
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 2].Cells[3].Style.ForeColor = Color.Black;
                                }
                                tbpay.Text = (Convert.ToInt32(tbpay.Text) + shortAmount).ToString();
                                AmountToWords(Convert.ToInt32(tbpay.Text) + shortAmount);
                                tbbal.Text = "0";
                                //mobitel bill update
                                if (chbPersonalSIM.Checked == false)
                                setDataForMobitelbill(dgv3, monthNameInWords(tempdate.AddDays(1).Month), tempdate.AddDays(1).Year.ToString());
                                //dgv3.Rows[0].Cells[0].Value = monthNameInWords(tempdate.AddDays(1).Month);
                                //dgv3.Rows[0].Cells[1].Value = tempdate.AddDays(1).Year.ToString();
                                //dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();
                            }
                            else
                            {
                                if (dgv.CurrentCell.RowIndex == dgv.RowCount - 1)
                                {
                                    dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;
                                }
                                else
                                {
                                    dgv.Rows.RemoveAt(dgv.RowCount - 2);
                                    dgv.Rows[dgv.CurrentRow.Index].Cells[3].Value = ((char)254).ToString();
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("dd-MM-yyyy");
                                    //dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToString("MM-dd-yyyy");
                                    dgv.Rows[dgv.RowCount - 1].Cells[0].Value = tempdate.ToShortDateString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[1].Value = tempdate.ToString("dddd");
                                    dgv.Rows[dgv.RowCount - 1].Cells[2].Value = "350";
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.Font = new Font("Wingdings", 15);
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Value = ((char)254).ToString();
                                    dgv.Rows[dgv.RowCount - 1].Cells[3].Style.ForeColor = Color.Black;

                                }

                            }

                        }
                    }

                }
            }
        }

        public void AreasMontDetail(System.Windows.Forms.DataGridView dgv3, DateTime last, double month, int year)
        {

            string monthName = "";
            int armonth = last.Month;

            if (month >= 2)
            {
                dgv3.Rows.Add(Convert.ToInt32(month) - 1);
            }


            for (int i = 0; i < month; i++)
            {
                armonth++;

                if (armonth > 12) //new year
                {
                    armonth = 1;
                    year = (last.Year) + 1;
                }

                monthName = monthNameInWords(armonth);

                dgv3.Rows[i].Cells[0].Value = monthName.ToString();
                dgv3.Rows[i].Cells[1].Value = year.ToString();

                if (month >= 1)
                {
                    dgv3.Rows[i].Cells[2].Value = phoneBil.ToString();
                }
                else if (month == 0.5)
                {
                    dgv3.Rows[i].Cells[2].Value = ph20Bill.ToString();
                }
                else if (month == 0.25)
                {
                    dgv3.Rows[i].Cells[2].Value = ph25Bill.ToString();
                }
            }

        }

        public void incommingMonthPhoneBillDetails(int month, DateTime start, DateTime end, int year, System.Windows.Forms.DataGridView dgv3, CheckBox chbBranding,TextBox tbCabNo,CheckBox chbPersonalSIM)
        {

            //if (chbBranding.Checked == false)
            //{
            string monthName = "";
            int armonth = start.Month;
            int row = 0;
            if (findCabNoToIgnorePhoneBill(tbCabNo,chbPersonalSIM) == false)
            {
                if (month > 0)
                {
                    row = month;
                    int initialRowCount = dgv3.RowCount;

                    if (dgv3.Rows[0].Cells[0].Value != null)
                    {
                        string tempMonth = dgv3.Rows[initialRowCount - 1].Cells[0].Value.ToString();
                        string tempYear = dgv3.Rows[initialRowCount - 1].Cells[1].Value.ToString();
                        string tempAmount = dgv3.Rows[initialRowCount - 1].Cells[2].Value.ToString();

                        dgv3.Rows.Add(row);

                        dgv3.Rows[dgv3.RowCount - (row + 1)].Cells[0].Value = tempMonth;
                        dgv3.Rows[dgv3.RowCount - (row + 1)].Cells[1].Value = tempYear;
                        dgv3.Rows[dgv3.RowCount - (row + 1)].Cells[2].Value = tempAmount;

                        dgv3.Rows[dgv3.RowCount - 1].Cells[0].Value = "";
                        dgv3.Rows[dgv3.RowCount - 1].Cells[1].Value = "";
                        dgv3.Rows[dgv3.RowCount - 1].Cells[2].Value = "";


                        // for new data... data grid should start from  dgv3.Rows[initialRowCount]

                        for (int i = 0; i < month; i++)
                        {
                            armonth++;
                            if (armonth > 12) //new year
                            {
                                armonth = 1;
                                year = (start.Year) + 1;
                            }
                            monthName = monthNameInWords(armonth);

                            dgv3.Rows[initialRowCount + i].Cells[0].Value = monthName;
                            dgv3.Rows[initialRowCount + i].Cells[1].Value = year.ToString();
                            dgv3.Rows[initialRowCount + i].Cells[2].Value = phoneBil.ToString();

                        }
                    }
                    else
                    {
                        dgv3.Rows.Add(row);

                        for (int i = 0; i < month; i++)
                        {
                            armonth++;
                            if (armonth > 12) //new year
                            {
                                armonth = 1;
                                year = (start.Year) + 1;
                            }
                            monthName = monthNameInWords(armonth);

                            dgv3.Rows[i].Cells[0].Value = monthName;
                            dgv3.Rows[i].Cells[1].Value = year.ToString();
                            dgv3.Rows[i].Cells[2].Value = phoneBil.ToString();

                        }


                    }
                }
                //}
            }
        }

        public string monthNameInWords(int armonth)
        {
            string monthName = "";

            switch (armonth)
            {
                case 1:
                    return monthName = "January";
                    break;
                case 2:
                    return monthName = "February";
                    break;
                case 3:
                    return monthName = "March";
                    break;
                case 4:
                    return monthName = "April";
                    break;
                case 5:
                    return monthName = "May";
                    break;
                case 6:
                    return monthName = "June";
                    break;
                case 7:
                    return monthName = "July";
                    break;
                case 8:
                    return monthName = "Augast";
                    break;
                case 9:
                    return monthName = "September";
                    break;
                case 10:
                    return monthName = "Octomber";
                    break;
                case 11:
                    return monthName = "November";
                    break;
                case 12:
                    return monthName = "December";
                    break;

            }
            return null;
        }

        public void SaveDetails(DataGridView dgv2, DataGridView dgv3, int workDays, TextBox tbTaxi, TextBox tbRecNo, TextBox tbAmount, TextBox tbWordAmount, TextBox tbPhoneNumber, CheckBox chb2, CheckBox chb4, TextBox tbFine, TextBox tbTotAmount, Label lbFine, ToolStripButton tsbFree, CheckBox chbSpcl,TextBox tbCancelRecNo,TextBox tbAprent,Label lbAppReason,DataGridView dgv5,CheckBox chbCrddit)
        {
            string location = "";
            string spFlag = "";
            nrcn = new NewReceiptNumber();
            tbRecNo.Text = nrcn.getReciptNo();
            location = get_Location();

            string monthFeild = "";
            string cUser = "";
            string tDate = "0000-00-00";
            string frmDate = "0000-00-00";
            string dateNow = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            string taxiNumber = "K" + tbTaxi.Text;
            string flag = "";
            string nightFlag = "";
            string deposit = "N";
            string creditCard = "N";
            int numofdays = 0;
            if (tbCancelRecNo.Text == "")
                tbCancelRecNo.Text = "XXXXX";
            
            string enteredDateTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            us = new User();
            cUser = us.getCurrentUser();
            string branchCode = us.getBranchCode();
            string branchName = us.getBranchName();


            //saveAppRental(tbRecNo.Text, tbTaxi, tbAprent, lbAppReason, dgv5, cUser, location, dateNow, enteredDateTime);

            if (chb2.Checked == true)//branding cars
                flag = "B";
            if (chb2.Checked == false)//normal cars
                flag = "0";
            if (chb4.Checked == true)//bank Deposit
                deposit = "Y";
            if (chbCrddit.Checked == true)//Credit card payment
                creditCard = "Y";

            if (chbSpcl.Checked == true)
                spFlag = "Y";
            else
                spFlag = "N";


            if (dgv2.Rows[0].Cells[0].Value != null)
            {
                //string monthFeild = "";
                //string cUser = "";
                //string tDate = "0000-00-00";
                //string frmDate = "0000-00-00";
                //string dateNow = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                //string taxiNumber = "K" + tbTaxi.Text;
                try
                {
                    DateTime toDate = DateTime.MinValue;
                    DateTime fromDate = DateTime.MinValue;

                    for (int i = (dgv2.RowCount - 1); i >= 0; i--)
                    {
                        if (dgv2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                        {
                            toDate = Convert.ToDateTime(dgv2.Rows[i].Cells[0].Value.ToString());
                            tDate = String.Format("{0:yyyy-MM-dd}", toDate);
                            i = -1;
                        }
                    }

                    int totalPhoneBil = 0;
                    for (int i = 0; i <= dgv2.RowCount - 1; i++)
                    {
                        if (dgv2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                        {
                            fromDate = Convert.ToDateTime(dgv2.Rows[i].Cells[0].Value.ToString());
                            frmDate = String.Format("{0:yyyy-MM-dd}", fromDate);
                            i = (dgv2.RowCount);
                        }
                    }

                    int working = 0;

                    for (int i = 0; i <= dgv2.RowCount - 1; i++)
                    {
                        if (dgv2.Rows[0].Cells[0].Value != null)
                        {
                            if (dgv2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                            {
                                working++;
                            }
                        }
                    }

                    if (working == workDays)
                    {

                        DialogResult dr = MessageBox.Show("You have Selected " + working + " Days", "Working Days", MessageBoxButtons.OKCancel);
                        if (dr == DialogResult.OK)
                        {

                            if (dgv3.Rows[0].Cells[0].Value != null)
                            {
                                for (int i = 0; i <= dgv3.RowCount - 1; i++)
                                {
                                    if (dgv3.Rows[i].Cells[0].Value != null)
                                    {
                                        totalPhoneBil = totalPhoneBil + Convert.ToInt32(dgv3.Rows[i].Cells[2].Value.ToString());
                                    }
                                }
                            }
                            else
                            {
                                totalPhoneBil = 0;
                            }

                            MySqlConnection connection1 = new MySqlConnection(constr);
                            connection1.Open();
                            MySqlCommand command = connection1.CreateCommand();
                            MySqlCommand command1 = connection1.CreateCommand();
                            //newly added
                            if (dgv3.Rows[0].Cells[0].Value != null)
                            {
                                string billMonth = "";
                                string billYear = "";
                                int billformonth = 0;

                                for (int i = 0; i <= dgv3.RowCount - 1; i++)
                                {
                                    if (dgv3.Rows[i].Cells[0].Value != null)
                                    {
                                        billMonth = dgv3.Rows[i].Cells[0].Value.ToString();
                                        billYear = dgv3.Rows[i].Cells[1].Value.ToString();
                                        billformonth = Convert.ToInt32(dgv3.Rows[i].Cells[2].Value.ToString());
                                        monthFeild = monthFeild + "," + billMonth;

                                        if (chb2.Checked == true)// if it is a branded car
                                        {

                                            command.CommandText = "INSERT INTO TestPhoneBillDetail (RecNo,CabNo,Month,Year,Amount,Pending,PayDate,Flag,user,location,branded,SpFlag) VALUES('" + tbRecNo.Text + "','" + taxiNumber + "','" + billMonth + "','" + billYear + "','0','" + billformonth + "','" + dateNow + "','A','" + cUser + "','" + location + "','Y','" + spFlag + "')";
                                        }
                                        else
                                        {
                                            command.CommandText = "INSERT INTO TestPhoneBillDetail (RecNo,CabNo,Month,Year,Amount,PayDate,Flag,user,location,SpFlag) VALUES('" + tbRecNo.Text + "','" + taxiNumber + "','" + billMonth + "','" + billYear + "','" + billformonth + "','" + dateNow + "','A','" + cUser + "','" + location + "','" + spFlag + "')";
                                        }
                                        command.ExecuteNonQuery();
                                    }
                                }

                            }
                            else if (dgv3.Rows[0].Cells[0].Value == null)
                            {
                                command.CommandText = "INSERT INTO TestPhoneBillDetail (RecNo,CabNo,Month,Year,Amount,PayDate,Flag,user,location,SpFlag) VALUES('" + tbRecNo.Text + "','" + taxiNumber + "','','','','','','" + cUser + "','" + location + "','"+spFlag+"')";
                                command.ExecuteNonQuery();
                            }
                            //end of newly added



                            if (chb2.Checked == true)//branded car free of charge
                            {
                                int brandReceiptAmount = Convert.ToInt32(tbAmount.Text) - totalPhoneBil;
                                int brandTotAmount = Convert.ToInt32(tbAmount.Text) - totalPhoneBil;
                                int brandTotalPhoneBil = 0;
                                string BrandMonthFeild = "";

                                command.CommandText = "INSERT INTO TestReciptHeader (RecNo,ReciptDate,ReciptAmount,CabNo,DateFrom,DateTo,nDays,TotalAmount,TotalAmountWord,Fine,TotBillRecv,Flag,UserID,monthFd,Deposited,FineRemark,EnteredDateTime,Location,cancellRecNoForNew,SpFlag,appRental,AppRemark,CreditCard,branchCode,branchName) VALUES ('" + tbRecNo.Text + "','" + dateNow + "','" + brandReceiptAmount + "','" + taxiNumber + "','" + frmDate + "','" + tDate + "','" + workingDays + "','" + brandTotAmount + "' ,'','" + tbFine.Text + "','" + brandTotalPhoneBil + "','" + flag + "','" + cUser + "','" + monthFeild + "','" + deposit + "','" + lbFine.Text + "','" + enteredDateTime + "','" + location + "','" + tbCancelRecNo.Text + "',   '" + spFlag + "','" + tbAprent.Text + "','" + lbAppReason.Text + "','" + creditCard + "','"+branchCode+"','"+branchName+"')";
                                //command.CommandText = "INSERT INTO TestReciptHeader (RecNo,ReciptDate,ReciptAmount,CabNo,DateFrom,DateTo,nDays,TotalAmount,TotalAmountWord,Fine,TotBillRecv,Flag,UserID,monthFd,Deposited,FineRemark,EnteredDateTime,Location,SpFlag) VALUES ('" + tbRecNo.Text + "','" + dateNow + "','" + tbAmount.Text + "','" + taxiNumber + "','" + frmDate + "','" + tDate + "','" + workingDays + "','" + tbTotAmount.Text + "' ,'','" + tbFine.Text + "','" + totalPhoneBil + "','" + flag + "','" + cUser + "','" + monthFeild + "','" + deposit + "','" + lbFine.Text + "','" + enteredDateTime + "','" + location + "','" + spFlag + "')";
                            }
                            else
                                command.CommandText = "INSERT INTO TestReciptHeader (RecNo,ReciptDate,ReciptAmount,CabNo,DateFrom,DateTo,nDays,TotalAmount,TotalAmountWord,Fine,TotBillRecv,Flag,UserID,monthFd,Deposited,FineRemark,EnteredDateTime,Location,cancellRecNoForNew,SpFlag,appRental,AppRemark,CreditCard,branchCode,branchName) VALUES ('" + tbRecNo.Text + "','" + dateNow + "','" + tbAmount.Text + "','" + taxiNumber + "','" + frmDate + "','" + tDate + "','" + workingDays + "','" + tbTotAmount.Text + "' ,'" + tbWordAmount.Text + "','" + tbFine.Text + "','" + totalPhoneBil + "','" + flag + "','" + cUser + "','" + monthFeild + "','" + deposit + "','" + lbFine.Text + "','" + enteredDateTime + "','" + location + "','" + tbCancelRecNo.Text + "', '" + spFlag + "','" + tbAprent.Text + "','" + lbAppReason.Text + "','" + creditCard + "','" + branchCode + "','" + branchName + "')";


                            command.ExecuteNonQuery();


                            for (int i = 0; i <= dgv2.RowCount - 1; i++)
                            {
                                nightFlag = "N";

                                if (dgv2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                                {
                                    if (i < dgv2.RowCount - 1)
                                    {
                                        if (dgv2.Rows[i + 1].Cells[3].Value.Equals(((char)253).ToString()))
                                            nightFlag = "Y";//to identify night cars for database
                                    }
                                    else if (i == dgv2.RowCount - 1)
                                    {
                                        nightFlag = "Y";//to identify night cars for database (last date is a night day)
                                    }
                                    string date = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dgv2.Rows[i].Cells[0].Value));
                                    int amount = Convert.ToInt32(dgv2.Rows[i].Cells[2].Value.ToString());

                                    if (chb2.Checked == true)// branded car free of charge
                                        command.CommandText = "INSERT INTO TestPayment (CabNo,RecNo,Date,Amount,Cancel,NightFlag,Location,SpFlag) VALUES ('" + taxiNumber + "' ,'" + tbRecNo.Text + "','" + date + "','300','" + flag + "','" + nightFlag + "','" + location + "','" + spFlag + "') ";
                                    else
                                        command.CommandText = "INSERT INTO TestPayment (CabNo,RecNo,Date,Amount,Cancel,NightFlag,Location,SpFlag) VALUES ('" + taxiNumber + "' ,'" + tbRecNo.Text + "','" + date + "','" + amount + "','" + flag + "','" + nightFlag + "','" + location + "','" + spFlag + "') ";
                                    command.ExecuteNonQuery();
                                }
                            }

                            connection1.Close();

                            saveAppRental(tbRecNo.Text, tbTaxi, tbAprent, lbAppReason, dgv5, cUser, location, dateNow, enteredDateTime);

                            MessageBox.Show("Saved");
                            us.SystemLog(DateTime.Now, cUser, "Receipt No " + tbRecNo.Text + " Entered,Saved,Printed");
                            //nrcn.decideReciptUpdate(tbRecNo);
                            nrcn.updateReceiptNo(tbRecNo);//this is for base
                            //updateReceiptNoYard(tbRecNo);//this is for yard
                            numofdays = working;//to get out number of working days
                            if (working == 25)
                            {
                                MessageBox.Show("Please Give Free 5 Days");
                                tsbFree.PerformClick();
                                //Form11 f11 = new Form11();
                                //f11.Show();                                   

                            }
                        }
                    }
                    savePhoneNumber(tbTaxi.Text, tbPhoneNumber.Text);

                    if (chb2.Checked == true)
                        printReceiptForBrandedCars(tbTaxi, tbRecNo);
                    if (chb2.Checked == false)
                        printReceipt(tbTaxi, tbRecNo);
                    if (chb4.Checked == true)
                        printReceiptForBankDeposit(tbTaxi, tbRecNo);

                    // setSMSforPayment(tbPhoneNumber, tbTaxi, frmDate, tDate, tbAmount);
                    // setSMSforRemindExpiryDate(tbPhoneNumber, tbTaxi, tDate);
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
                //savePhoneNumber(tbTaxi.Text, tbPhoneNumber.Text);

                //if (chb2.Checked == true)
                //    printReceiptForBrandedCars(tbTaxi, tbRecNo);
                //else
                //    printReceipt(tbTaxi, tbRecNo);

                //setSMSforPayment(tbPhoneNumber, tbTaxi, frmDate, tDate, tbAmount);
                //setSMSforRemindExpiryDate(tbPhoneNumber, tbTaxi, tDate);
            }
            else
            {
                MessageBox.Show("Dont save");
            }
        }
        //public void SaveDetailsToYard(DataGridView dgv2, DataGridView dgv3, int workDays, TextBox tbTaxi, TextBox tbRecNo, TextBox tbAmount, TextBox tbWordAmount, TextBox tbPhoneNumber, CheckBox chb2)
        //{
        //    string taxiNumber = "K" + tbTaxi.Text;
        //    string flag = "";
        //    if (chb2.Checked == true)
        //        flag = "B";
        //    else
        //        flag = "0";

        //    MySqlConnection connection1 = new MySqlConnection(constr);
        //    connection1.Open();
        //    MySqlCommand command = connection1.CreateCommand();
        //    MySqlCommand command1 = connection1.CreateCommand();

        //    for (int i = 0; i <= dgv2.RowCount - 1; i++)
        //    {
        //        if (dgv2.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
        //        {
        //            string date = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dgv2.Rows[i].Cells[0].Value));
        //            int amount = Convert.ToInt32(dgv2.Rows[i].Cells[2].Value.ToString());

        //            if (chb2.Checked == true)// branded car free of charge
        //                command.CommandText = "INSERT INTO TestPayment (CabNo,RecNo,Date,Amount,Cancel) VALUES ('" + taxiNumber + "' ,'" + tbRecNo.Text + "','" + date + "','0','" + flag + "') ";
        //            else//normal cars
        //                command.CommandText = "INSERT INTO TestPayment (CabNo,RecNo,Date,Amount,Cancel) VALUES ('" + taxiNumber + "' ,'" + tbRecNo.Text + "','" + date + "','" + amount + "','" + flag + "') ";

        //            command.ExecuteNonQuery();
        //        }
        //    }

        //    connection1.Close();
        //    MessageBox.Show("Yard Updated!!");

        //}

        public void savePhoneNumber(string cabNo, string phoneNumber)
        {
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO TestDriverMobile (cabNo,MobileNo) VALUES('" + cabNo + "','" + phoneNumber + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void setSMSforPayment(TextBox tbMobileNo, TextBox tbCabno, string fromDate, string toDate, TextBox tbAmount)
        {
            string message = "Cab " + tbCabno.Text + " Thank you for Rental payment of Rs." + tbAmount.Text + " Valid for " + fromDate + " to " + toDate + " Budget Meter Taxi 2592592";
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO TestSMSsotre (cabNo,MobileNo,message,Flag) Values('" + tbCabno.Text + "','" + tbMobileNo.Text + "','" + message + "','0')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("SMS is Delivered to " + tbCabno.Text);
            connection.Close();
        }

        public void setSMSforRemindExpiryDate(TextBox tbMobileNo, TextBox tbCabno, string toDate)
        {
            string expireDate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(toDate));
            string message = "Cab " + tbCabno.Text + " Your Rental Expire on 26/06/2013 Please Make a payment on or before " + toDate + " Budget Meter Taxi 2592592";
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO TestExpireDate (MobileNo,expireDate,message,Flag) Values('" + tbMobileNo.Text + "','" + expireDate + "','" + message + "','0')";
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        public void displayOtherPayment(Label lbSimDepo, Label lbSticker, Label lbTshirt, Label lbRegstrtn)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Testcollection";
            using (var reader = cmd.ExecuteReader())
            {
                reader.Read();
                lbSimDepo.Text = reader[1].ToString();
                reader.Read();
                lbSticker.Text = reader[1].ToString();
                reader.Read();
                lbRegstrtn.Text = reader[1].ToString();
                reader.Read();
                lbTshirt.Text = reader[1].ToString();
                //reader.Read();
                //lbRegstrtn.Text = reader[1].ToString();
            }
            connection.Close();
        }

        public void calTotOtherPayment(TextBox tbTotAmount, TextBox tbSimDepo, TextBox tbStikcer, TextBox tbTshirt, TextBox tbAtoZ, TextBox tbLeasing, TextBox tbSimFine, TextBox tbFine, TextBox tbOtherAmount, TextBox tbregisration, TextBox tbLdeposit, TextBox tbDriverDepo)
        {
            try
            {
                tbTotAmount.Text = (Convert.ToDouble(tbSimDepo.Text) + Convert.ToDouble(tbStikcer.Text) + Convert.ToDouble(tbTshirt.Text) + Convert.ToDouble(tbAtoZ.Text) + Convert.ToDouble(tbLeasing.Text) + Convert.ToDouble(tbSimFine.Text) + Convert.ToDouble(tbFine.Text) + Convert.ToDouble(tbOtherAmount.Text) + Convert.ToDouble(tbregisration.Text) + Convert.ToDouble(tbLdeposit.Text) + Convert.ToDouble(tbDriverDepo.Text)).ToString();
            }
            catch (Exception) { }
        }

        public string saveOtherPayment(string recno, TextBox tbCabNo, TextBox tbPlateNo, TextBox tbName, TextBox tbNICDL, DateTimePicker dtDate, TextBox tbTotalAmount, TextBox tbregistration, TextBox tbSimDepo, TextBox tbSticker, TextBox tbTshirt, TextBox tbAtoZ, TextBox tbLeasing, TextBox tbLdeposit, TextBox tbSimFine, TextBox tbFine, TextBox tbotherAmount, TextBox tbRemark, GroupBox gb1, TextBox tbrecno, TextBox tbDriverDepo)
        {
            string location = "";
            location = get_Location();
            us = new User();
            string remark = "Registration-" + tbregistration.Text + "Sim Deposit-" + tbSimDepo.Text + ", " + "Stickering- " + tbSticker.Text + ", " + "T-Shirt- " + tbTshirt.Text;
            string date = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dtDate.Text));
            string datemin = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dtDate.Text));
            string user = us.getCurrentUser();
            string cab = "K" + tbCabNo.Text;

            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO TestOtherPayment (OtherRecNo,cabNo,PlateNo,Name,NICDL,Date,Amount,DriverDepo,Registration,SimDepo,sticker,Tshirt,AtoZ,Leasing,Ldeposit, SimFine,Fine,OtherAmount,Remark,Refund,user,RefundBy,RefundDate,location) VALUES('" + recno + "','" + tbCabNo.Text + "','" + tbPlateNo.Text + "','" + tbName.Text + "','" + tbNICDL.Text + "','" + date + "','" + tbTotalAmount.Text + "','" + tbDriverDepo.Text + "', '" + tbregistration.Text + "','" + tbSimDepo.Text + "','" + tbSticker.Text + "','" + tbTshirt.Text + "', '" + tbAtoZ.Text + "','" + tbLeasing.Text + "','" + tbLdeposit.Text + "',   '" + tbSimFine.Text + "','" + tbFine.Text + "','" + tbotherAmount.Text + "','" + tbRemark.Text + "','0','" + user + "','','" + datemin + "','" + location + "')";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Saved!");
            printOtherReceipt(recno);
            //clearOtherPayment(dtDate, tbCabNo, tbSimDepo, tbSticker, tbTshirt, tbTotalAmount, tbregistration,tbRemark,tbotherAmount,tbName,tbPlateNo,tbNICDL,tbrecno);
            gb1.Enabled = false;
            return recno;

        }

        public void clearOtherPayment(DateTimePicker dtPicker, TextBox tbCabNo, TextBox tbSimDepo, TextBox tbsticker, TextBox tbTshirt, TextBox tbTotAmount, TextBox tbRegistration, TextBox tbRemark, TextBox tbotherAmount, TextBox tbName, TextBox tbPlateNo, TextBox tbNICDL, TextBox tbrecno, TextBox tbAtoz, TextBox tbLleter, TextBox tbLdeposit, TextBox tbSimFine, TextBox tbfine, TextBox tbDriverDepo)
        {
            dtPicker.Value = DateTime.Now; tbCabNo.Text = ""; tbSimDepo.Text = "0"; tbsticker.Text = "0"; tbTshirt.Text = "0"; tbTotAmount.Text = "0"; tbRegistration.Text = "0";
            tbRemark.Text = ""; tbotherAmount.Text = "0"; tbName.Text = ""; tbPlateNo.Text = ""; tbNICDL.Text = ""; tbrecno.Text = ""; tbDriverDepo.Text = "0";
            tbLleter.Text = "0"; tbLdeposit.Text = "0"; tbSimFine.Text = "0"; tbfine.Text = "0";
        }

        public void printOtherReceipt(string recno)
        {
            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT OtherRecNo,cabNo,PlateNo,Name,NICDL,Date,Amount,DriverDepo,Registration,SimDepo,sticker,Tshirt,AtoZ,Leasing,Ldeposit,SimFine,Fine, OtherAmount,Remark,user FROM  TestOtherPayment WHERE OtherRecNo='" + recno + "'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "OtherRecPrint");


            connection.Close();

            CrystalReport15 rpt = new CrystalReport15();
            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";
            // rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;

            DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 1, 1);
            }


        }

        public void printReceipt(TextBox tbtaxi, TextBox tbrecNo)
        {


            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,  TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord, TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID  FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestPayment.CabNo='" + tbtaxi.Text + "'  ORDER BY TestPayment.Date DESC ";
            command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,TestPayment.Cancel,TestPayment.NightFlag, TestReciptHeader.ReciptDate,TestReciptHeader.nDays,TestReciptHeader.TotalAmount, TestReciptHeader.TotalAmountWord,TestReciptHeader.Fine, TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID, TestReciptHeader.monthFd,TestReciptHeader.FineRemark, TestReciptHeader.cancellRecNoForNew,TestReciptHeader.appRental,TestReciptHeader.AppRemark	FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestReciptHeader.RecNo='" + tbrecNo.Text + "' ORDER BY TestPayment.Date ASC ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "RecPrint");


            connection.Close();

            CrystalReport1 rpt = new CrystalReport1();

            //TextObject txtDeposit = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            //txtDeposit.Text = "(Bank Deposit)";

            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            // rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;
            rpt.PrintToPrinter(1, false, 1, 1);


        }

        public void printReceiptForBrandedCars(TextBox tbtaxi, TextBox tbrecNo)
        {


            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,  TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord, TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID  FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestPayment.CabNo='" + tbtaxi.Text + "'  ORDER BY TestPayment.Date DESC ";
            command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,TestPayment.Cancel,TestPayment.NightFlag, TestReciptHeader.nDays,TestReciptHeader.TotalAmount, TestReciptHeader.TotalAmountWord,TestReciptHeader.Fine, TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID, TestReciptHeader.monthFd,TestReciptHeader.FineRemark,TestReciptHeader.cancellRecNoForNew, TestReciptHeader.appRental,TestReciptHeader.AppRemark FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestReciptHeader.RecNo='" + tbrecNo.Text + "' ORDER BY TestPayment.Date ASC ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "RecPrint");


            connection.Close();

            CrystalReport22 rpt = new CrystalReport22();
            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            // rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;
            rpt.PrintToPrinter(1, false, 1, 1);


        }

        public void printReceiptForBankDeposit(TextBox tbtaxi, TextBox tbrecNo)
        {


            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,  TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord, TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID  FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestPayment.CabNo='" + tbtaxi.Text + "'  ORDER BY TestPayment.Date DESC ";
            command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,TestPayment.Cancel,TestPayment.NightFlag, TestReciptHeader.ReciptDate,TestReciptHeader.nDays,TestReciptHeader.TotalAmount,TestReciptHeader.TotalAmountWord,TestReciptHeader.Fine,TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID, TestReciptHeader.monthFd,TestReciptHeader.FineRemark,TestReciptHeader.cancellRecNoForNew,TestReciptHeader.appRental,TestReciptHeader.AppRemark FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestReciptHeader.RecNo='" + tbrecNo.Text + "' ORDER BY TestPayment.Date ASC ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "RecPrint");


            connection.Close();

            CrystalReport1 rpt = new CrystalReport1();

            TextObject txtDeposit = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            txtDeposit.Text = "(Bank Deposit)";

            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            // rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;
            rpt.PrintToPrinter(1, false, 1, 1);


        }

        //public void updateReceiptNo(TextBox tbRecNo) //this is for base (update Receipt number para)
        //{
        //    string s = tbRecNo.Text;
        //    string[] split = s.Split(new string[] { "BA" }, StringSplitOptions.RemoveEmptyEntries);
        //    int recno = Convert.ToInt32(split[0]);
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command = connection.CreateCommand();
        //    command.CommandText = "UPDATE TestPara SET ReceiptNo='" + recno + "' WHERE ID=0";
        //    command.ExecuteNonQuery();
        //    connection.Close();
        //}

        //public void updateReceiptNoYard(TextBox tbRecNo)//this for Yard
        //{
        //    string s = tbRecNo.Text;
        //    string[] split = s.Split(new string[] { "RE" }, StringSplitOptions.RemoveEmptyEntries);
        //    int recno = Convert.ToInt32(split[0]);
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command = connection.CreateCommand();
        //    command.CommandText = "UPDATE TestYardPara SET ReceiptNo='" + recno + "' WHERE ID=0";
        //    command.ExecuteNonQuery();
        //    connection.Close();
        //}

        public void findReceiptForCancell(String key, TextBox tbrecno, TextBox tbcabno, TextBox tbreceiptamt, TextBox tbreceiptdate, RadioButton rb1, RadioButton rb2, RadioButton rb3, RadioButton rb4,TextBox tbBranded)
        {

            if (rb1.Checked == true || rb2.Checked == true || rb3.Checked == true || rb4.Checked == true)
            {

                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                if (rb1.Checked == true)
                    command.CommandText = "SELECT RecNo,ReciptDate,ReciptAmount,CabNo,Flag FROM TestReciptHeader WHERE RecNo='" + key + "'";
                if (rb2.Checked == true)
                    command.CommandText = "SELECT CabNo,RefNo,Amount FROM TestFreePayment WHERE RecNo='" + key + "' OR RefNo='" + key + "'";
                if (rb3.Checked == true)
                    command.CommandText = "SELECT cabNo,RecNo,Amount,PayDate FROM TestVenRecHeader WHERE RecNo='" + key + "'";
                if (rb4.Checked == true)
                    command.CommandText = "SELECT cabNo,OtherRecNo,Amount,Date FROM TestOtherPayment WHERE OtherRecNo='" + key + "'";

                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (rb1.Checked == true)
                            {
                                tbrecno.Text = reader["RecNo"].ToString();
                                tbcabno.Text = reader["CabNo"].ToString();
                                tbreceiptamt.Text = reader["ReciptAmount"].ToString();
                                tbreceiptdate.Text = reader["ReciptDate"].ToString();
                                tbBranded.Text = reader["Flag"].ToString();
                            }
                            else if (rb2.Checked == true)
                            {
                                tbrecno.Text = reader["RefNo"].ToString();
                                tbcabno.Text = reader["CabNo"].ToString();
                                tbreceiptamt.Text = reader["Amount"].ToString();
                            }
                            else if (rb3.Checked == true)
                            {
                                tbrecno.Text = reader["RecNo"].ToString();
                                tbcabno.Text = reader["cabNo"].ToString();
                                tbreceiptamt.Text = reader["Amount"].ToString();
                                tbreceiptdate.Text = reader["PayDate"].ToString();
                            }
                            else if (rb4.Checked == true)
                            {
                                tbrecno.Text = reader["OtherRecNo"].ToString();
                                tbcabno.Text = reader["cabNo"].ToString();
                                tbreceiptamt.Text = reader["Amount"].ToString();
                                tbreceiptdate.Text = reader["Date"].ToString();
                            }

                        }

                    }
                    connection.Close();
                }
                catch (Exception) { connection.Close(); }
            }
            else
                MessageBox.Show("Please Select Receipt Type !!!!!!!");
        }

        public void cancellAllReceipt(TextBox tbrecno, TextBox tbcabno, TextBox tbamount, TextBox tbdate, TextBox tbreason, RadioButton rb1, RadioButton rb2, RadioButton rb3, RadioButton rb4,ComboBox cmbReason,TextBox tbBranded)
        {
            us=new User();
            string user=us.getCurrentUser();
            string remark=cmbReason.Text+" "+tbreason.Text;

            var cabno = tbcabno.Text;
            cabno = cabno.Substring(1);

            string cancelDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            if (rb1.Checked == true || rb2.Checked == true || rb3.Checked == true || rb4.Checked == true)
            {
                MySqlConnection connection = new MySqlConnection(constr);

                try
                {
                    connection.Open();
                    MySqlCommand command1 = connection.CreateCommand();
                    MySqlCommand command2 = connection.CreateCommand();
                    MySqlCommand command3 = connection.CreateCommand();
                    MySqlCommand command4 = connection.CreateCommand();
                    MySqlCommand command5 = connection.CreateCommand();
                    MySqlCommand command6 = connection.CreateCommand();
                    if (rb1.Checked == true)
                    {
                        command1.CommandText = "UPDATE `TestPayment` SET `Delete`='Y' WHERE `RecNo`='" + tbrecno.Text + "' AND `CabNo`='" + tbcabno.Text + "' ";
                        command2.CommandText = "UPDATE `TestReciptHeader` SET `Delete`='Y',canceledBy='" + user + "',canceledDate='" + cancelDate + "',remark='"+remark+"'    WHERE `RecNo`='" + tbrecno.Text + "' AND `CabNo`='" + tbcabno.Text + "' ";
                        command3.CommandText = "UPDATE `TestPhoneBillDetail` SET `Delete`='Y' WHERE `RecNo`='" + tbrecno.Text + "' AND `CabNo`='" + tbcabno.Text + "'";
                        command4.CommandText = "UPDATE `TestBankDeposit` SET `Delete`='Y' WHERE `recno`='" + tbrecno.Text + "' AND `CabNo`='" + tbcabno.Text + "'";
                        command5.CommandText = "UPDATE  `TestAppRental`  SET `Flag`='Y' WHERE `RecNo`='" + tbrecno.Text + "' AND `CabNo`='" + cabno + "'";
                        command6.CommandText = "UPDATE  `TestAppRentSum`  SET `Flag`='Y' WHERE `RecNo`='" + tbrecno.Text + "' AND `CabNo`='" + cabno + "'";

                        command1.ExecuteNonQuery();
                        command2.ExecuteNonQuery();
                        command3.ExecuteNonQuery();
                        command4.ExecuteNonQuery();
                        command5.ExecuteNonQuery();
                        command6.ExecuteNonQuery();
                    }
                    else if (rb2.Checked == true)
                    {
                        command1.CommandText = "UPDATE `TestFreePayment` SET `Cancel`='Y' WHERE (`RecNo`='" + tbrecno.Text + "' OR `RefNo`='" + tbrecno.Text + "') AND (`CabNo`='" + tbcabno.Text + "') ";
                        command1.ExecuteNonQuery();
                    }
                    else if (rb3.Checked == true)
                    {
                        command1.CommandText = "UPDATE `TestVenturaPayment` SET `Flag`='Y' WHERE `RecNo`='" + tbrecno.Text + "' AND `cabNo`='" + tbcabno.Text + "' ";
                        command2.CommandText = "UPDATE `TestVenRecHeader` SET `Flag`='Y' WHERE `RecNo`='" + tbrecno.Text + "' AND `cabNo`='" + tbcabno.Text + "' ";
                        command1.ExecuteNonQuery();
                        command2.ExecuteNonQuery();
                    }
                    else if (rb4.Checked == true)
                    {
                        command4.CommandText = "UPDATE `TestOtherPayment` SET `Delete`='Y' WHERE (`OtherRecNo`='" + tbrecno.Text + "') AND (`cabNo`='" + tbcabno.Text + "') ";
                        command4.ExecuteNonQuery();
                    }
                    connection.Close();
                    saveCancellationInfo(tbrecno, tbcabno, tbamount, tbdate, tbreason, rb1, rb2, rb3, rb4);
                    MessageBox.Show("Cancelled !!!");
                }
                catch (Exception) { connection.Close(); }

                //if (tbBranded.Text == "B")
                //    SecettedBrandedCarRecipPrint(dataGridView1);
            }
            else
                MessageBox.Show("Please Select Receipt Type !!!!");
        }

        public void saveCancellationInfo(TextBox tbrecno, TextBox tbcabno, TextBox tbamount, TextBox tbdate, TextBox tbreason, RadioButton rb1, RadioButton rb2, RadioButton rb3, RadioButton rb4)
        {
            string category = ""; string user = "";
            us = new User();
            user = us.getCurrentUser();

            if (rb1.Checked == true)
                category = "Daily Payment Receipt";
            if (rb2.Checked == true)
                category = "Free Days Receipt";
            if (rb3.Checked == true)
                category = "Ventura Receipt";
            if (rb4.Checked == true)
                category = "Other Payment Receipt";


            MySqlConnection connection = new MySqlConnection(constr);
            try
            {
                connection.Open();
                MySqlCommand command1 = connection.CreateCommand();
                command1.CommandText = "INSERT INTO TestCancellation (RecNo,ReciptAmount,CabNo,Category,Date,DateTime,Remark,User) VALUES ('" + tbrecno.Text + "','" + tbamount.Text + "','" + tbcabno.Text + "','" + category + "','" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "','" + String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now) + "','" + tbreason.Text + "','" + user + "')";
                command1.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception) { connection.Close(); }
        }

        public void findReceipt(TextBox tbrecNo, DataGridView dgv1)
        {
            if (tbrecNo.Text != "")
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataSet1 recfd = new DataSet1();
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command1 = connection.CreateCommand();
                command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.DateFrom,TestReciptHeader.Deposited, TestReciptHeader.Delete,TestReciptHeader.canceledBy,TestReciptHeader.canceledDate,TestReciptHeader.remark From TestReciptHeader where (TestReciptHeader.RecNo LIKE '" + tbrecNo.Text + "%' ) AND (TestReciptHeader.Flag !='B') ORDER BY TestReciptHeader.DateFrom DESC";
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

        public void setGridView(DataGridView dgv1)
        {
            for (int i = 0; i < dgv1.Rows.Count - 1; i++)
            {
                if (dgv1.Rows[i].Cells[2].Value.ToString() == "Y")
                    dgv1.Rows[i].Cells[2].Value = "BD";
                else
                    dgv1.Rows[i].Cells[2].Value = "C";
            }
        }

        public void findBrandedCarReceipt(TextBox tbrecNo, DataGridView dgv1)
        {
            if (tbrecNo.Text != "")
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataSet1 recfd = new DataSet1();
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command1 = connection.CreateCommand();
                command1.CommandText = "SELECT TestReciptHeader.RecNo ,TestReciptHeader.DateFrom, TestReciptHeader.Delete,TestReciptHeader.canceledBy,TestReciptHeader.canceledDate,TestReciptHeader.remark  From TestReciptHeader where (TestReciptHeader.RecNo LIKE '" + tbrecNo.Text + "%' ) AND (TestReciptHeader.Flag = 'B') ORDER BY TestReciptHeader.DateFrom DESC";
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

        public void findVenturaReceipt(TextBox tbRecno, DataGridView dgv1)
        {
            if (tbRecno.Text != "")
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataSet1 recfd = new DataSet1();
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command1 = connection.CreateCommand();
                command1.CommandText = "SELECT RecNo,Date,cabNo FROM TestVenturaPayment WHERE  RecNo LIKE '" + tbRecno.Text + "%' Order by Date";
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

        public string SecettedRecipPrint(DataGridView dgv1)
        {
            string recNo = dgv1.Rows[dgv1.CurrentRow.Index].Cells[0].Value.ToString();
            string type = dgv1.Rows[dgv1.CurrentRow.Index].Cells[2].Value.ToString();
            string cancel = dgv1.Rows[dgv1.CurrentRow.Index].Cells[3].Value.ToString();
            string cancelledby = ""; string canceldate = ""; string remRemark = "";    

             try{ cancelledby=dgv1.Rows[dgv1.CurrentRow.Index].Cells[4].Value.ToString();}catch (Exception) { }
             try { canceldate = dgv1.Rows[dgv1.CurrentRow.Index].Cells[5].Value.ToString(); }catch (Exception) { }
             try { remRemark = dgv1.Rows[dgv1.CurrentRow.Index].Cells[6].Value.ToString(); }catch (Exception) { }
             
            
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,TestPayment.Cancel,TestPayment.NightFlag,TestReciptHeader.ReciptDate,TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord, TestReciptHeader.Fine,TestReciptHeader.FineRemark, TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID,TestReciptHeader.monthFd,TestReciptHeader.cancellRecNoForNew ,TestReciptHeader.appRental,TestReciptHeader.AppRemark  FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestReciptHeader.RecNo='" + recNo + "' ORDER BY TestPayment.Date ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "RecPrint");
            connection.Close();

            CrystalReport1 rpt = new CrystalReport1();

            if (type == "Y")
            {
                TextObject txtDeposit = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];

                txtDeposit.Text = "(Bank Deposit)";
            }

            if (cancel == "Y")
            {
                TextObject txtCancell = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
                txtCancell.Text = "Cancelled";

                TextObject lbCancelledBy = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
                lbCancelledBy.Text = "Cancelled By-:";
                TextObject txtCancelledBy = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
                txtCancelledBy.Text=cancelledby;

                TextObject lbCancelledDate = (TextObject)rpt.ReportDefinition.ReportObjects["Text32"];
                lbCancelledDate.Text = "Date-:";
                TextObject txtCancelledDate = (TextObject)rpt.ReportDefinition.ReportObjects["Text33"];
                txtCancelledDate.Text = canceldate;

                TextObject lbRemark = (TextObject)rpt.ReportDefinition.ReportObjects["Text30"];
                lbRemark.Text = "Remark-:";
                TextObject txtRemark = (TextObject)rpt.ReportDefinition.ReportObjects["Text31"];
                txtRemark.Text=remRemark;
            }

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            //rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 1, 1);
            }


            return recNo;
        }

        public string SecettedBrandedCarRecipPrint(DataGridView dgv1)
        {            

            string recNo = dgv1.Rows[dgv1.CurrentRow.Index].Cells[0].Value.ToString();      
            string cancell = dgv1.Rows[dgv1.CurrentRow.Index].Cells[2].Value.ToString();
            string cancelledby = ""; string canceldate = ""; string remRemark = "";

            try { cancelledby = dgv1.Rows[dgv1.CurrentRow.Index].Cells[3].Value.ToString(); }
            catch (Exception) { }
            try { canceldate = dgv1.Rows[dgv1.CurrentRow.Index].Cells[4].Value.ToString(); }
            catch (Exception) { }
            try { remRemark = dgv1.Rows[dgv1.CurrentRow.Index].Cells[5].Value.ToString(); }
            catch (Exception) { }
             

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,TestPayment.Cancel,TestPayment.NightFlag,  TestReciptHeader.nDays,TestReciptHeader.TotalAmountWord, TestReciptHeader.TotBillRecv,TestReciptHeader.DateFrom, TestReciptHeader.DateTo,TestReciptHeader.UserID,TestReciptHeader.monthFd,TestReciptHeader.cancellRecNoForNew,TestReciptHeader.appRental,TestReciptHeader.AppRemark  FROM TestPayment  inner JOIN TestReciptHeader  on TestPayment.RecNo=TestReciptHeader.RecNo   where TestReciptHeader.RecNo='" + recNo + "' ORDER BY TestPayment.Date ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "RecPrint");
            connection.Close();

            CrystalReport22 rpt = new CrystalReport22();
            if (cancell == "Y")
            {
                TextObject txtCancell = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
                txtCancell.Text = "Cancelled";


                TextObject lbCancelledBy = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
                lbCancelledBy.Text = "Cancelled By-:";
                TextObject txtCancelledBy = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
                txtCancelledBy.Text = cancelledby;

                TextObject lbCancelledDate = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
                lbCancelledDate.Text = "Date-:";
                TextObject txtCancelledDate = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
                txtCancelledDate.Text = canceldate;

                TextObject lbRemark = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
                lbRemark.Text = "Remark-:";
                TextObject txtRemark = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];
                txtRemark.Text = remRemark;
            }
            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            //rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 1, 1);
            }


            return recNo;
        }

        public string SecettedVenturaRecipPrint(DataGridView dgv1)
        {
            string recNo = dgv1.Rows[dgv1.CurrentRow.Index].Cells[0].Value.ToString();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT cabNo,PlateNo,RecNo,Date,Amount,PayDatetime,User FROM TestVenturaPayment WHERE  RecNo='" + recNo + "' Order by Date";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VenRecPrint");


            connection.Close();

            CrystalReport24 rpt = new CrystalReport24();
            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            // rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;

            DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 1, 1);
            }




            return recNo;
        }

        public void CancelRecipt(string RecNo)
        {
            us = new User();
            DialogResult dr = MessageBox.Show("Are You Sure Want To Cancel this Receipt No " + RecNo + "\n If you want to Cancel, press YES " + "\n If not Press No", "Confirm Cancellation", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                MySqlConnection connection1 = new MySqlConnection(constr);
                connection1.Open();
                MySqlCommand command = connection1.CreateCommand();
                command.CommandText = "Update TestReciptHeader SET Flag='C' WHERE RecNo='" + RecNo + "'";
                command.ExecuteNonQuery();
                command.CommandText = "Update TestPayment SET Cancel='C' WHERE RecNo='" + RecNo + "'";
                command.ExecuteNonQuery();
                us.SystemLog(DateTime.Now, us.getCurrentUser(), "Receipt No " + RecNo + " Cancelled");
                MessageBox.Show("Cancelled");

            }

        }

        public void paymentCheckerAll(DataGridView dgv1, DateTimePicker dtp)
        {
            string dtpDate = String.Format("{0:yyyy-MM-dd}", dtp.Value);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,Date,RecNo FROM TestPayment WHERE Date='" + dtpDate + "' AND TestPayment.Delete !='Y' ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

            newadp1.Fill(ds);
            dt = ds.Tables[0];
            dgv1.DataSource = dt;
            connection.Close();
        }

        public void cabAvailability(DataGridView dgv1, DateTimePicker dtp, TextBox tbCabNo)
        {


            string cab = "K" + tbCabNo.Text;
            string dtpDate = String.Format("{0:yyyy-MM-dd}", dtp.Value);


            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CabNo FROM TestPayment WHERE (CabNo='" + cab + "') AND (TestPayment.Delete!='Y')";
            var reader = cmd.ExecuteReader();

            if (reader.Read() == true)
            {
                conn.Close();
                //get calling number
            }
            else
            {
                conn.Open();
                cmd.CommandText = "SELECT CabNo FROM TestPayment WHERE (CabNo='" + cab + "') AND (TestPayment.Delete!='Y')";
                var reader1 = cmd.ExecuteReader();

                if (reader1.Read() == true)
                {
                    conn.Close();
                    //get calling number
                }
            }


        }

        public void freeDaysCabs(DataGridView dgv3, DateTimePicker dtp)
        {
            string dtpDate = String.Format("{0:yyyy-MM-dd}", dtp.Value);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,Date,RecNo FROM TestFreePayment WHERE Date='" + dtpDate + "' AND Cancel!='Y'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

            newadp1.Fill(ds);
            dt = ds.Tables[0];
            dgv3.DataSource = dt;
            connection.Close();
        }

        public void paymentCheckerSelectedTaxi(DataGridView dgv1, DateTimePicker dtp, TextBox tbTaxi)
        {
            dgv1.DataSource = null;
            string dtpDate = String.Format("{0:yyyy-MM-dd}", dtp.Value);
            string taxino = "K" + tbTaxi.Text;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,Date,RecNo FROM TestPayment WHERE (Date='" + dtpDate + "' AND CabNo='" + taxino + "')AND TestPayment.Delete!='Y'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);

            newadp1.Fill(ds);
            dt = ds.Tables[0];
            dgv1.DataSource = dt;
            connection.Close();
        }

        public void FreeSelectedTaxiChecker(DataGridView dgv3, DateTimePicker dtp, TextBox tbTaxi)
        {
            dgv3.DataSource = null;
            string dtpDate = String.Format("{0:yyyy-MM-dd}", dtp.Value);
            string taxino = "K" + tbTaxi.Text;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,Date,RecNo FROM TestFreePayment WHERE (Date='" + dtpDate + "' AND CabNo='" + taxino + "') AND (Cancel!='Y') ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);

            newadp1.Fill(ds);
            dt = ds.Tables[0];
            dgv3.DataSource = dt;
            connection.Close();
        }

        public void numberOfPaidTaxi(DataGridView dgv1, TextBox tbpaidTaxi, TextBox tbIncome)
        {
            int taxiCounter = 0;
            for (int i = 0; i < dgv1.RowCount; i++)
            {
                if (dgv1.Rows[i].Cells[0].Value != null)
                {
                    taxiCounter++;
                }
            }
            tbpaidTaxi.Text = taxiCounter.ToString();
            tbIncome.Text = (taxiCounter * perDayCharge).ToString();
        }

        public void numberOfFreeTaxi(DataGridView dgv3, TextBox tbFreeTaxi)
        {
            int taxiCounter = 0;
            for (int i = 0; i < dgv3.RowCount; i++)
            {
                if (dgv3.Rows[i].Cells[0].Value != null)
                {
                    taxiCounter++;
                }
            }
            tbFreeTaxi.Text = taxiCounter.ToString();

        }

        public string checkLastSunday(DateTime lastDate, DataGridView dgv1)
        {
            DateTime workedDate; DateTime reverseDate; DateTime lastSunday;

            if (lastDate > DateTime.Now)
            {
                lastDate = lastDate;
            }
            else
            {
                lastDate = DateTime.Now;
            }

            for (int i = 1; i <= 7; i++)
            {
                reverseDate = lastDate.AddDays(-(i));

                if (reverseDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    lastSunday = reverseDate;
                    for (int j = 0; j < minimumDays; j++)//no need to chek all datagrid row, only back 10ys more than enouhf 
                    {
                        try
                        {
                            workedDate = Convert.ToDateTime(dgv1.Rows[j].Cells[1].Value.ToString());

                            if (workedDate.ToShortDateString() == lastSunday.ToShortDateString())
                            {
                                return "On Duty";
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }
            return "OFF";
        }

        public bool sundayOffWitrhLast(DataGridView dgv2, TextBox txtSunday)
        {
            DateTime sunday;
            for (int i = 0; i < dgv2.RowCount; i++)
            {
                sunday = Convert.ToDateTime(dgv2.Rows[i].Cells[0].Value.ToString());

                if (sunday.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (txtSunday.Text == "OFF")
                    {
                        return true;
                    }
                }
                //return false;
            }
            return false;
        }

        public bool findFirstSunday(DataGridView dgv, int index, TextBox tbsun)
        {

            bool sunOff = backsevendaySunday(dgv, index);



            for (int i = index; i <= (index + 7); i++)
            {
                DateTime sunday = Convert.ToDateTime(dgv.Rows[i].Cells[0].Value.ToString());
                if (sunday.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (sunOff == true)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return true;
        }

        public bool backsevendaySunday(DataGridView dgv, int index)
        {
            for (int i = (index - 1); i >= (index - 7); i--)
            {
                DateTime sunday = Convert.ToDateTime(dgv.Rows[i].Cells[0].Value.ToString());
                if (sunday.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (dgv.Rows[i].Cells[3].Value.Equals(((char)254).ToString()))
                    {

                        return true;
                    }
                    if (dgv.Rows[i].Cells[3].Value.Equals(((char)253).ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;

        }

        public DataTable getPaymentDatesForCheck(string taxino) //just for view        
        {
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();

                MySqlConnection connection1 = new MySqlConnection(constr);
                connection1.Open();
                MySqlCommand command = connection1.CreateCommand();
                // command.CommandText = "select ReciptNo,ReciptDate from ReciptHeader where CabNo='" + taxi + "' order by ReciptDate DESC";
                command.CommandText = "select CabNo,Date,RecNo from TestPayment where CabNo='" + taxino + "' AND TestPayment.Delete!='Y' order by Date DESC";
                MySqlDataAdapter newadp = new MySqlDataAdapter(command);
                newadp.Fill(ds);
                dt = ds.Tables[0];
                connection1.Close();

                if (ds.Tables[0] != null)
                {
                    return dt;
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string PaidForToday(DataGridView dgv, DateTime dt) //just for view
        {
            for (int i = 0; i < (dgv.RowCount - 1); i++)
            {
                if ((dt.ToShortDateString()) == ((Convert.ToDateTime(dgv.Rows[i].Cells[1].Value.ToString())).ToShortDateString()))
                {
                    dgv.Rows[i].Cells[1].Style.BackColor = Color.Red;
                    return "Paid For today";
                }
            }
            return "Not Paid For Tody";
        }

        public void setDataForMobitelbill(DataGridView dgv3, string mnthName, string yr)//when user untick a date, if autamatically tick a next month date, get the phone bill for next month 
        {
            string tempMonth;
            string tempYear;
            string tempAmount;

            if (dgv3.RowCount == 1)
            {
                if (dgv3.Rows[0].Cells[0].Value == null)
                {
                    dgv3.Rows[0].Cells[0].Value = mnthName;
                    dgv3.Rows[0].Cells[1].Value = yr;
                    dgv3.Rows[0].Cells[2].Value = phoneBil.ToString();
                }
                else
                {
                    tempMonth = dgv3.Rows[0].Cells[0].Value.ToString();
                    tempYear = dgv3.Rows[0].Cells[1].Value.ToString();
                    tempAmount = dgv3.Rows[0].Cells[2].Value.ToString();

                    dgv3.Rows.Add(1);

                    dgv3.Rows[0].Cells[0].Value = tempMonth;
                    dgv3.Rows[0].Cells[1].Value = tempYear;
                    dgv3.Rows[0].Cells[2].Value = tempAmount;

                    dgv3.Rows[1].Cells[0].Value = mnthName;
                    dgv3.Rows[1].Cells[1].Value = yr;
                    dgv3.Rows[1].Cells[2].Value = phoneBil.ToString();

                }
            }
            else
            {
                tempMonth = dgv3.Rows[dgv3.RowCount - 1].Cells[0].Value.ToString();
                tempYear = dgv3.Rows[dgv3.RowCount - 1].Cells[1].Value.ToString();
                tempAmount = dgv3.Rows[dgv3.RowCount - 1].Cells[2].Value.ToString();

                dgv3.Rows.Add(1);

                dgv3.Rows[dgv3.RowCount - 1].Cells[0].Value = mnthName;
                dgv3.Rows[dgv3.RowCount - 1].Cells[1].Value = yr;
                dgv3.Rows[dgv3.RowCount - 1].Cells[2].Value = phoneBil.ToString();

                dgv3.Rows[dgv3.RowCount - 2].Cells[0].Value = tempMonth;
                dgv3.Rows[dgv3.RowCount - 2].Cells[1].Value = tempYear;
                dgv3.Rows[dgv3.RowCount - 2].Cells[2].Value = tempAmount;

            }


        }

        //public int dateCheckForStartingDate(DataGridView dgv, DateTimePicker dtp,TextBox tblastPayDate) 
        //{

        //   // string dt=dgv.Rows[0].Cells[1].Value.ToString();
        //   //string dt1= String.Format("{0:yyyy-MM-dd}", dt);
        //    string dt=Convert.ToDateTime(dgv.Rows[0].Cells[1].Value.ToString()).ToShortDateString();
        //    for (int i = 0; i < (dgv.RowCount - 1); i++) 
        //    {
        //        if (dtp.Value.ToShortDateString() == (Convert.ToDateTime(dgv.Rows[i].Cells[1].Value.ToString()).ToShortDateString())) 
        //        {
        //            MessageBox.Show("Already Paid" );
        //            dtp.Value = Convert.ToDateTime(tblastPayDate.Text).AddDays(1);
        //            return 1;
        //        }
        //    }
        //    return 0;
        //}

        public void getLastDayForFreeDays(TextBox tbReciptNo, TextBox tbTaxiNo, DataGridView dgv1)  //For the Free Day Offering
        {
            DateTime lastDate = DateTime.MinValue;

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM TestReciptHeader WHERE RecNo='" + tbReciptNo.Text + "'AND TestReciptHeader.Delete !='Y'";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    lastDate = Convert.ToDateTime(reader["DateTo"].ToString());
                    tbTaxiNo.Text = reader["CabNo"].ToString();
                }
                connection.Close();

                feedFreeDays(lastDate.AddDays(1), dgv1);
            }
        }

        public void getCabNoForSpecialFreeDays(TextBox tbReciptNo, TextBox tbTaxiNo, TextBox tbLastDate)  //For the Free Day Offering
        {
            DateTime lastDate = DateTime.MinValue;

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM TestReciptHeader WHERE RecNo='" + tbReciptNo.Text + "' AND TestReciptHeader.Delete !='Y'";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tbTaxiNo.Text = reader["CabNo"].ToString();
                    tbLastDate.Text = Convert.ToDateTime(reader["DateTo"].ToString()).ToShortDateString();
                }
                connection.Close();
            }
        }

        public void feedFreeDays(DateTime startDate, DataGridView dgv1)
        {
            dgv1.Rows.Add(5);
            for (int i = 0; i < 5; i++)
            {
                dgv1.Rows[i].Cells[0].Value = startDate.AddDays(i).ToShortDateString();
                dgv1.Rows[i].Cells[1].Value = startDate.AddDays(i).ToString("dddd");
            }
        }

        public void saveFreeWorkingDays(DataGridView dgv1, TextBox tbtaxi, TextBox tbReciptNo, TextBox tbRefNo, ToolStripButton tsbtn, TextBox tbRemark,int option)
        {
            if (tbRemark.Text != "")
            {
                string location = "";
                nrcn = new NewReceiptNumber();
                string curentUser;
                us = new User();
                curentUser = us.getCurrentUser();

                if (dgv1.RowCount >= 4)
                {
                    MySqlConnection connection = new MySqlConnection(constr);
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    for (int i = 0; i < dgv1.Rows.Count; i++)
                    {
                        if (dgv1.Rows[i].Cells[0].Value != null)
                        {
                            DateTime dates = Convert.ToDateTime(dgv1.Rows[i].Cells[0].Value.ToString());
                            string freedates = String.Format("{0:yyyy-MM-dd}", dates);
                            string freeDay = dgv1.Rows[i].Cells[1].Value.ToString();
                            if (option == 1)
                                command.CommandText = "INSERT INTO TestFreePayment (CabNo,RecNo,RefNo,Remark,Date,Day,Amount,Cuser,Cancel) VALUES ('" + tbtaxi.Text + "' ,'" + tbReciptNo.Text + "','" + tbRefNo.Text + "', '" + tbRemark.Text + "','" + freedates + "','" + freeDay + "','0','" + curentUser + "','0') ";
                            if (option == 2)
                                command.CommandText = "INSERT INTO TestFreePayment (CabNo,RecNo,RefNo,Remark,Date,Day,Amount,Cuser,Cancel,SpFlag) VALUES ('" + tbtaxi.Text + "' ,'" + tbReciptNo.Text + "','" + tbRefNo.Text + "', '" + tbRemark.Text + "','" + freedates + "','" + freeDay + "','0','" + curentUser + "','0','Y') ";
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Saved");
                    tsbtn.Enabled = false;
                    connection.Close();
                    nrcn.updatePromoReceiptNo(tbRefNo);
                    us = new User();
                }
                else
                {
                    MessageBox.Show("Please Enter Receipt Number");
                }
            }
            else 
            {
                MessageBox.Show("Please Enter Remark !!!!!!!!");
            }
        }

        public void clearFreeDayForm(DataGridView dgv1, TextBox tbCabNo, TextBox tbRecNo, TextBox tbRefNo, ToolStripButton tsbtn, TextBox tbRemark) //form 11
        {
            tsbtn.Enabled = true;
            dgv1.Rows.Clear();
            tbCabNo.Text = "";
            tbRecNo.Text = "";
            tbRefNo.Text = "";
            tbRemark.Text = "";
        }

        public int addSpecialFreeDays(DataGridView dgv1, DateTimePicker dtp1)
        {
            for (int i = 0; i < 20; i++)
            {
                if (dgv1.Rows[i].Cells[0].Value == null)
                {
                    dgv1.Rows[i].Cells[0].Value = dtp1.Value.ToShortDateString();
                    dgv1.Rows[i].Cells[1].Value = dtp1.Value.DayOfWeek.ToString();
                    return 0;
                }
            }
            return 0;
        }
        //Server Migrations - 02/06/2015
        //public void getCallingNo(DataGridView dgv1, string date, TextBox tbSelectCab, TextBox tbSelectVoucher)
        //{
        //    dgv1.Rows.Clear();
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();


        //    DataSet1 recfd = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client, TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' ORDER BY TestVoucherRef.CabNo";
        //    //MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

        //    //newadp1.Fill(ds);
        //    //dt = ds.Tables[0];
        //    //dgv1.DataSource = dt;


        //    using (var reader = command1.ExecuteReader())
        //    {
        //        displayGrid(reader, dgv1);
        //    }

        //    connection.Close();

        //    gridFormat(dgv1);

        //}

        public void getCallingNo(DataGridView dgv1, string date, TextBox tbSelectCab, TextBox tbSelectVoucher, Panel p3, TextBox tb58)
        {
            dgv1.Rows.Clear();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();


            DataSet1 recfd = new DataSet1();
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT voucherRefNo,CabNo,voucherDate,client,paytype,paid,Organization FROM VoucherRef WHERE voucherDate='" + date + "' ORDER BY CabNo ";//WHERE voucherDate='" + date + "' ORDER BY CabNo
            //MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

            //newadp1.Fill(ds);
            //dt = ds.Tables[0];
            //dgv1.DataSource = dt;


            using (var reader = command1.ExecuteReader())
            {
                displayGrid(reader, dgv1, p3, tb58, tbSelectCab);
            }

            connection.Close();

            gridFormat(dgv1);

        }

        public void getCallingNoFromDespatchBooking(DataGridView dgv1, string date, TextBox tbSelectCab, TextBox tbSelectVoucher, Panel p3, TextBox tb58)
        {
            dgv1.Rows.Clear();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();


            DataSet1 recfd = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr7);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT RefNo as voucherRefNo,CabNo,  DATE(RecordInsertTime) as voucherDate , ClientName as client, PaymentType as paytype,Paid as paid,ClientAddress as Organization,Amount FROM DespatchBooking WHERE DATE(RecordInsertTime)='" + date + "' ORDER BY CabNo ";//WHERE voucherDate='" + date + "' ORDER BY CabNo
            //MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

            //newadp1.Fill(ds);
            //dt = ds.Tables[0];
            //dgv1.DataSource = dt;

          
            using (var reader = command1.ExecuteReader())
            {
                displayGridAPP(reader, dgv1, p3, tb58, tbSelectCab);
            }

            connection.Close();

            gridFormat(dgv1);

        }


        public void getCallingNoFromLogsheet(DataGridView dgv1, string date, TextBox tbSelectCab, TextBox tbSelectVoucher, Panel p3, TextBox tb58)
        {
            dgv1.Rows.Clear();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();


            DataSet1 recfd = new DataSet1();
            //MySqlConnection connection = new MySqlConnection(constr7);
            //MySqlConnection connection = new MySqlConnection(constr8);

            MySqlConnection connection = new MySqlConnection(constr9); //26 07 2019
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT BookingID as voucherRefNo,VehiclID as CabNo,  DATE(MeterOFFTime) as voucherDate , PaymentType as paytype,ClientName as client,ClientAddress as Organization,  Paid as paid,Amount,WaitingCost FROM viewLogSheet WHERE  DATE(MeterOFFTime)='" + date + "'  ORDER BY CabNo LIMIT 100";//WHERE voucherDate='" + date + "' ORDER BY CabNo
            //command1.CommandText = "SELECT 	booking_id as voucherRefNo,vehicl_id as CabNo,  DATE(meter_off_time) as voucherDate , payment_type as paytype,cus_name as client,meter_on_loation as Organization,  Paid as paid, 	total_amount as Amount,waiting_cost as WaitingCost FROM budget_bookings WHERE  DATE(meter_off_time)='" + date + "'  ORDER BY vehicl_id LIMIT 100";//WHERE voucherDate='" + date + "' ORDER BY CabNo

           // command1.CommandText = "SELECT 	refID as voucherRefNo,cabNo as CabNo,bookingType as bookingType,  DATE(endTime) as voucherDate , paymentMethod as paytype,customerFirstName as client,pickUpAddress as Organization, Paid as paid, totalFare as Amount,waitingFare as WaitingCost, organization as orgz,discountType as dtype,voucherNumber as voucherNo FROM bookings WHERE  DATE(endTime)='" + date + "'  ORDER BY cabNo LIMIT 100";
            command1.CommandText = "SELECT 	refID as voucherRefNo,cabNo as CabNo,bookingType as bookingType,  DATE(endTime) as voucherDate , paymentMethod as paytype,customerFirstName as client,pickUpAddress as Organization, Paid as paid, totalFare as Amount,waitingFare as WaitingCost, organization as orgz,discountType as dtype,voucherNumber as voucherNo,requiredCabNo FROM bookings WHERE  DATE(endTime)='" + date + "' LIMIT 100";
            //MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

            //newadp1.Fill(ds);
            //dt = ds.Tables[0];
            //dgv1.DataSource = dt;

            try
            {
                using (var reader = command1.ExecuteReader())
                {
                    displayGridAPPLogsheet(reader, dgv1, p3, tb58, tbSelectCab);
                }
            }
            catch (Exception ex) {MessageBox.Show( ex.Message.ToString()); }

            connection.Close();

            gridFormat(dgv1);

        }


        //public string displayGrid(MySqlDataReader reader, DataGridView dgv1)
        //{

        //    int i = 0; string cab = "";
        //    //try
        //    ////{
        //    while (reader.Read())
        //    {
        //        dgv1.Rows.Add();
        //        cab = reader["CabNo"].ToString();
        //        dgv1.Rows[i].Cells[0].Value = cab;
        //        dgv1.Rows[i].Cells[1].Value = (Convert.ToDateTime(reader["Date"])).ToShortDateString();
        //        try
        //        {
        //            dgv1.Rows[i].Cells[2].Value = reader["Cnumber"].ToString();
        //        }
        //        catch (IndexOutOfRangeException) { }
        //        dgv1.Rows[i].Cells[3].Value = reader["voucherRefNo"].ToString();
        //        dgv1.Rows[i].Cells[4].Value = reader["client"].ToString();

        //        if (reader["paytype"].ToString() == "CA")
        //            dgv1.Rows[i].Cells[5].Value = "Cash";
        //        if (reader["paytype"].ToString() == "CR")
        //        {
        //            dgv1.Rows[i].Cells[5].Value = "Credit";
        //            dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
        //        }

        //        if (reader["paid"].ToString() == "0")
        //            dgv1.Rows[i].Cells[6].Value = "No";
        //        if (reader["paid"].ToString() == "1")
        //        {
        //            dgv1.Rows[i].Cells[6].Value = "Paid";
        //            dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
        //        }
        //        i++;

        //    }
        //    return cab;
        //    //  }catch(MySqlException){}
        //}
        public string displayGrid(SqlDataReader reader, DataGridView dgv1, Panel p3, TextBox tb58, TextBox tbCab)
        {
            Voucher vr = new Voucher();
            int i = 0; string cab = ""; //bool select = true; 
            //try
            ////{


            while (reader.Read())
            {
                dgv1.Rows.Add();
                cab = reader["CabNo"].ToString();


                dgv1.Rows[i].Cells[0].Value = cab;
                dgv1.Rows[i].Cells[1].Value = (Convert.ToDateTime(reader["voucherDate"])).ToShortDateString();
                try
                {
                    dgv1.Rows[i].Cells[7].Value = reader["Organization"].ToString();
                }
                catch (IndexOutOfRangeException) { }
                dgv1.Rows[i].Cells[3].Value = reader["voucherRefNo"].ToString();
                dgv1.Rows[i].Cells[4].Value = reader["client"].ToString();

                if (reader["paytype"].ToString() == "CA")
                    dgv1.Rows[i].Cells[5].Value = "Cash";
                if (reader["paytype"].ToString() == "CR")
                {
                    dgv1.Rows[i].Cells[5].Value = "Credit";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }
                if (reader["paytype"].ToString() == "CV")
                {
                    dgv1.Rows[i].Cells[5].Value = "Voucher";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }
                if (reader["paytype"].ToString() == "CO")
                {
                    dgv1.Rows[i].Cells[5].Value = "Corperate";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                }
                //if (reader["paytype"].ToString() == "CV")
                //{
                //    dgv1.Rows[i].Cells[5].Value = "Voucher";
                //    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                //}
                //if (reader["paytype"].ToString() == "CO")
                //{
                //    dgv1.Rows[i].Cells[5].Value = "Corporate";
                //    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                //}

                if (reader["paid"].ToString() == "0")
                    dgv1.Rows[i].Cells[6].Value = "No";
                if (reader["paid"].ToString() == "1")
                {
                    dgv1.Rows[i].Cells[6].Value = "Paid";
                    dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }
                i++;
            }
            //}
            return cab;
            //  }catch(MySqlException){}
        }


        public string displayGridAPP(MySqlDataReader reader, DataGridView dgv1, Panel p3, TextBox tb58, TextBox tbCab)
        {
            Voucher vr = new Voucher();
            int i = 0; string cab = ""; //bool select = true; 
            //try
            ////{


            while (reader.Read())
            {
                dgv1.Rows.Add();
                cab = reader["CabNo"].ToString();


                dgv1.Rows[i].Cells[0].Value = cab;
                dgv1.Rows[i].Cells[1].Value = (Convert.ToDateTime(reader["voucherDate"])).ToShortDateString();
                dgv1.Rows[i].Cells[2].Value = reader["Amount"].ToString();
                try
                {
                    dgv1.Rows[i].Cells[7].Value = reader["Organization"].ToString();
                }
                catch (IndexOutOfRangeException) { }
                dgv1.Rows[i].Cells[3].Value = reader["voucherRefNo"].ToString();
                dgv1.Rows[i].Cells[4].Value = reader["client"].ToString();

                if (reader["paytype"].ToString() == "CA")
                    dgv1.Rows[i].Cells[5].Value = "Cash";
                if (reader["paytype"].ToString() == "CR")
                {
                    dgv1.Rows[i].Cells[5].Value = "Credit";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }
                if (reader["paytype"].ToString() == "CV")
                {
                    dgv1.Rows[i].Cells[5].Value = "Voucher";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }
                if (reader["paytype"].ToString() == "CO")
                {
                    dgv1.Rows[i].Cells[5].Value = "Corperate";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                }

                if (reader["paytype"].ToString() == "VW")
                {
                    dgv1.Rows[i].Cells[5].Value = "Wallet";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }


                if (reader["paid"].ToString() == "0")
                    dgv1.Rows[i].Cells[6].Value = "No";
                if (reader["paid"].ToString() == "1")
                {
                    dgv1.Rows[i].Cells[6].Value = "Paid";
                    dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }
                i++;
            }
            //}
            return cab;
            //  }catch(MySqlException){}
        }


        public string displayGridAPPLogsheet(MySqlDataReader reader, DataGridView dgv1, Panel p3, TextBox tb58, TextBox tbCab)
        {
            Voucher vr = new Voucher();
            int i = 0; string cab = ""; string refno = ""; //bool select = true; 
            double amount = 0.00; double waiting = 0.00;
            string payType = "";
            string bookingType="";
            string orgz = "";
            string dtype = "";
            string voucherNo = "";
            //try
            ////{


            while (reader.Read())
            {
                dgv1.Rows.Add();
                cab = reader["CabNo"].ToString();


                dgv1.Rows[i].Cells[0].Value = cab;               

                dgv1.Rows[i].Cells[1].Value = (Convert.ToDateTime(reader["voucherDate"])).ToShortDateString();

                //dgv1.Rows[i].Cells[2].Value = reader["Amount"].ToString();

                amount = Convert.ToDouble(reader["Amount"].ToString());
                waiting = 0; //Convert.ToDouble(reader["WaitingCost"].ToString());
                dgv1.Rows[i].Cells[2].Value = (amount + waiting).ToString();

                try
                {
                    dtype = reader["dtype"].ToString();
                    orgz=reader["Orgz"].ToString();
                    dgv1.Rows[i].Cells[7].Value = reader["Organization"].ToString();
                    voucherNo=reader["voucherNo"].ToString();

                }
                catch (IndexOutOfRangeException) { }



                //int ascii1 = Convert.ToInt32((reader["voucherRefNo"].ToString().Substring(0, 2)));
                //int ascii2 = Convert.ToInt32((reader["voucherRefNo"].ToString().Substring(2, 2)));
                //char character1 = (char)ascii1; char character2 = (char)ascii2; string character3 = reader["voucherRefNo"].ToString().Substring(4, 4);
                //refno = (character1.ToString() + character2.ToString() + character3.ToString()).ToString();
                dgv1.Rows[i].Cells[3].Value =reader["voucherRefNo"].ToString(); //refno;


                dgv1.Rows[i].Cells[4].Value = reader["client"].ToString();

                //New modification 02/08/2019
                payType=reader["paytype"].ToString();
                bookingType= reader["bookingType"].ToString();

                if (bookingType == "External")
                {
                    if (orgz == "SLTPublicationPvtLtd")
                    {
                        payType = "SLT";
                    }
                    else
                    {
                        payType = "Corporate";
                    }
                }
                if (dtype == "Call-up-area-discount" && orgz != "SLTPublicationPvtLtd" && payType!="Cash")
                {
                    payType = "Call-UP";
                }


                //if (reader["paytype"].ToString() == "CA")
                //if (reader["paytype"].ToString() == "Cash")

                if (payType == "Cash")
                    dgv1.Rows[i].Cells[5].Value = "Cash";

                //if (reader["paytype"].ToString() == "CR")
               // if (reader["paytype"].ToString() == "Credit-card")
                if (payType == "Credit-card")
                {
                    dgv1.Rows[i].Cells[5].Value = "Credit";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }
               // if (reader["paytype"].ToString() == "CV")
               // if (reader["paytype"].ToString() == "Voucher")
                if (payType == "Voucher")
                {
                    if (Regex.IsMatch(voucherNo, ".*?[a-zA-Z].*?"))
                    {
                        dgv1.Rows[i].Cells[5].Value = "Token";
                        dgv1.Rows[i].Cells[5].Style.BackColor = Color.Goldenrod;
                    }
                    else
                    {
                        dgv1.Rows[i].Cells[5].Value = "Voucher";
                        dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                    }
                }
                //if (reader["paytype"].ToString() == "CO")
                //if (reader["paytype"].ToString() == "Corporate")
                if (payType == "Corporate")
                {
                    dgv1.Rows[i].Cells[5].Value = "Corperate";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                }

                if (payType == "SLT")
                {
                    dgv1.Rows[i].Cells[5].Value = "SLT";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Blue;
                }

                //if (reader["paytype"].ToString() == "Touch")
                if (payType == "Touch")
                {
                    dgv1.Rows[i].Cells[5].Value = "Touch";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Indigo;
                }

                if (payType == "Call-UP")
                {
                    dgv1.Rows[i].Cells[5].Value = "Call-UP" ; 
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.DodgerBlue;
                }

                //if (reader["paytype"].ToString() == "VW")
                //{
                //    dgv1.Rows[i].Cells[5].Value = "Wallet";
                //    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                //}

                if (reader["paid"].ToString() == "0")
                    dgv1.Rows[i].Cells[6].Value = "No";
                if (reader["paid"].ToString() == "1")
                {
                    dgv1.Rows[i].Cells[6].Value = "Paid";
                    dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }

                if (reader["requiredCabNo"].ToString() == cab)
                {
                    
                        dgv1.Rows[i].Cells[3].Style.BackColor = Color.Red;
                    
                }

                i++;
            }
            //}
            return cab;
            //  }catch(MySqlException){}
        }


        public string displayGridFromJob(SqlDataReader reader, DataGridView dgv1, Panel p3, TextBox tb58)
        {

            int i = 0; string refno = ""; string cab = ""; string date = ""; string company = ""; string name = "";
            string paytype = "";

            while (reader.Read())
            {
                dgv1.Rows.Add();

                cab = ("K" + reader["CabNo"].ToString()).Replace(" ", "");
                date = (Convert.ToDateTime(reader["DispatchTime"])).ToShortDateString();
                int ascii1 = Convert.ToInt32((reader["JobID"].ToString().Substring(0, 2)));
                int ascii2 = Convert.ToInt32((reader["JobID"].ToString().Substring(2, 2)));
                char character1 = (char)ascii1; char character2 = (char)ascii2; string character3 = reader["JobID"].ToString().Substring(4, 4);
                refno = (character1.ToString() + character2.ToString() + character3.ToString()).ToString();
                name = reader["Name"].ToString();
                paytype = reader["CashCredit"].ToString();

                try
                {
                    company = (reader["Organization"].ToString()).Replace(" ", "");
                }
                catch (IndexOutOfRangeException) { }
                dgv1.Rows[i].Cells[0].Value = cab;
                dgv1.Rows[i].Cells[1].Value = date;
                dgv1.Rows[i].Cells[7].Value = company;
                dgv1.Rows[i].Cells[3].Value = refno; // reader["JobID"].ToString();
                dgv1.Rows[i].Cells[4].Value = name;

                if (paytype == "CA")
                    dgv1.Rows[i].Cells[5].Value = "Cash";
                if (paytype == "CR")
                {
                    dgv1.Rows[i].Cells[5].Value = "Credit";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }
                if (paytype == "CV")
                {
                    dgv1.Rows[i].Cells[5].Value = "Voucher";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }
                if (paytype == "CO")
                {
                    dgv1.Rows[i].Cells[5].Value = "Corperate";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                }

                if (paytype == "VW")
                {
                    dgv1.Rows[i].Cells[5].Value = "Wallet";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }

                string s = reader["Paid"].ToString();
                if (reader["Paid"].ToString() != "1")// destination is used to update the paid or not (becuse of unable to add new colum to job table)
                    dgv1.Rows[i].Cells[6].Value = "No";
                if (reader["Paid"].ToString() == "1")
                {
                    dgv1.Rows[i].Cells[6].Value = "Paid";
                    dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }
                i++;
            }

            try
            {
                // sendVoucherRefFromJob(refno, cab, String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(date)), name, paytype, "0", company);
            }
            catch (Exception) { }
            return cab;

        }


        public string displayGridFromLogsheet(MySqlDataReader reader, DataGridView dgv1, Panel p3, TextBox tb58)
        {

            int i = 0; string refno = ""; string cab = ""; string date = ""; string company = ""; string name = "";
            string paytype = ""; double amount = 0.00; double waiting = 0.00; string bookingType = ""; string dtype = "";
            string voucherNo = ""; 
   

            while (reader.Read())
            {
                dgv1.Rows.Add();

                cab = ("K" + reader["CabNo"].ToString()).Replace(" ", "");
                date = (Convert.ToDateTime(reader["DispatchTime"])).ToShortDateString();
               // int ascii1 = Convert.ToInt32((reader["voucherRefNo"].ToString().Substring(0, 2)));
                //int ascii2 = Convert.ToInt32((reader["voucherRefNo"].ToString().Substring(2, 2)));
                //char character1 = (char)ascii1; char character2 = (char)ascii2; string character3 = reader["voucherRefNo"].ToString().Substring(4, 4);
                refno =reader["voucherRefNo"].ToString(); //(character1.ToString() + character2.ToString() + character3.ToString()).ToString();
                name = reader["Name"].ToString();
                paytype = reader["CashCredit"].ToString();
                //amount = Convert.ToDouble(reader["Amount"].ToString());
                amount = Convert.ToDouble(reader["Amount"].ToString());
                waiting = 0; //Convert.ToDouble(reader["WaitingCost"].ToString());
                //dgv1.Rows[i].Cells[2].Value = (amount + waiting).ToString();
               
                string orgz = "";

                try
                {
                    dtype = reader["dtype"].ToString();
                    company =(reader["Organization"].ToString()).Replace(" ", "");
                    orgz = reader["Orgz"].ToString();
                    voucherNo = reader["voucherNo"].ToString();

                }
                catch (IndexOutOfRangeException) { }
                dgv1.Rows[i].Cells[0].Value = cab;
                dgv1.Rows[i].Cells[1].Value = date;
                dgv1.Rows[i].Cells[2].Value = (amount+waiting).ToString();
                dgv1.Rows[i].Cells[7].Value = company;
                dgv1.Rows[i].Cells[3].Value = refno; // reader["JobID"].ToString();
                dgv1.Rows[i].Cells[4].Value = name;

                bookingType=reader["bookingType"].ToString();

                //if ( bookingType== "External")
                //    paytype = "Corporate";
                if (bookingType == "External")
                {
                    if (orgz == "SLTPublicationPvtLtd")
                    {
                        paytype = "SLT";
                    }
                    else
                    {
                        paytype = "Corporate";
                    }
                }
                if (dtype == "Call-up-area-discount" && orgz != "SLTPublicationPvtLtd" && paytype!="Cash")
                {
                    paytype = "Call-UP";
                }

                //if (paytype == "CA")
                if (paytype == "Cash")
                    dgv1.Rows[i].Cells[5].Value = "Cash";

                //if (paytype == "CR")
                if (paytype == "Credit-card")
                {
                    dgv1.Rows[i].Cells[5].Value = "Credit";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }

               // if (paytype == "CV")
                if (paytype == "Voucher")
                {
                    if (Regex.IsMatch(voucherNo, ".*?[a-zA-Z].*?"))
                    {
                        dgv1.Rows[i].Cells[5].Value = "Token";
                        dgv1.Rows[i].Cells[5].Style.BackColor = Color.Goldenrod;
                    }
                    else
                    {
                        dgv1.Rows[i].Cells[5].Value = "Voucher";
                        dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                    }
                }

                //if (paytype == "CO")
                if (paytype == "Corporate")
                    
                {
                    dgv1.Rows[i].Cells[5].Value = "Corperate";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                }

                if (paytype == "SLT")
                {
                    dgv1.Rows[i].Cells[5].Value = "SLT";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Blue;
                }

                if (paytype == "Touch")
                {
                    dgv1.Rows[i].Cells[5].Value = "Touch";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Indigo;
                }
                if (paytype == "Call-UP")
                {
                    dgv1.Rows[i].Cells[5].Value = "Call-UP";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.DodgerBlue;
                }
                //if (paytype == "VW")
                //{
                //    dgv1.Rows[i].Cells[5].Value = "Wallet";
                //    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                //}
                string s = reader["Paid"].ToString();
                if (reader["Paid"].ToString() != "1")// destination is used to update the paid or not (becuse of unable to add new colum to job table)
                    dgv1.Rows[i].Cells[6].Value = "No";
                if (reader["Paid"].ToString() == "1")
                {
                    dgv1.Rows[i].Cells[6].Value = "Paid";
                    dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }

                if (reader["requiredCabNo"].ToString() == cab.Substring(1)) 
                {
                    
                    
                        dgv1.Rows[i].Cells[3].Style.BackColor = Color.Red;
                    
                } 
                i++;
            }

            try
            {
                // sendVoucherRefFromJob(refno, cab, String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(date)), name, paytype, "0", company);
            }
            catch (Exception) { }
            return cab;

        }

        public string displayGridFromJobAPP(MySqlDataReader reader, DataGridView dgv1, Panel p3, TextBox tb58)
        {

            int i = 0; string refno = ""; string cab = ""; string date = ""; string company = ""; string name = "";
            string paytype = ""; double amount = 0.00;

            while (reader.Read())
            {
                dgv1.Rows.Add();

                cab = ("K" + reader["CabNo"].ToString()).Replace(" ", "");
                date = (Convert.ToDateTime(reader["DispatchTime"])).ToShortDateString();
                //int ascii1 = Convert.ToInt32((reader["JobID"].ToString().Substring(0, 2)));
                //int ascii2 = Convert.ToInt32((reader["JobID"].ToString().Substring(2, 2)));
                //char character1 = (char)ascii1; char character2 = (char)ascii2; string character3 = reader["JobID"].ToString().Substring(4, 4);
                refno = reader["voucherRefNo"].ToString(); //(character1.ToString() + character2.ToString() + character3.ToString()).ToString();
                name = reader["Name"].ToString();
                paytype = reader["CashCredit"].ToString();
                amount = Convert.ToDouble(reader["Amount"].ToString());
                try
                {
                    company = (reader["Organization"].ToString()).Replace(" ", "");
                }
                catch (IndexOutOfRangeException) { }
                dgv1.Rows[i].Cells[0].Value = cab;
                dgv1.Rows[i].Cells[1].Value = date;
                dgv1.Rows[i].Cells[2].Value = amount;
                dgv1.Rows[i].Cells[7].Value = company;
                dgv1.Rows[i].Cells[3].Value = refno; // reader["JobID"].ToString();
                dgv1.Rows[i].Cells[4].Value = name;

                if (paytype == "CA")
                    dgv1.Rows[i].Cells[5].Value = "Cash";
                if (paytype == "CR")
                {
                    dgv1.Rows[i].Cells[5].Value = "Credit";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }
                if (paytype == "CV")
                {
                    dgv1.Rows[i].Cells[5].Value = "Voucher";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }
                if (paytype == "CO")
                {
                    dgv1.Rows[i].Cells[5].Value = "Corperate";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                }
                if (paytype == "VW")
                {
                    dgv1.Rows[i].Cells[5].Value = "Wallet";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }
                string s = reader["Paid"].ToString();
                if (reader["Paid"].ToString() != "1")// destination is used to update the paid or not (becuse of unable to add new colum to job table)
                    dgv1.Rows[i].Cells[6].Value = "No";
                if (reader["Paid"].ToString() == "1")
                {
                    dgv1.Rows[i].Cells[6].Value = "Paid";
                    dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                }
                i++;
            }

            try
            {
                // sendVoucherRefFromJob(refno, cab, String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(date)), name, paytype, "0", company);
            }
            catch (Exception) { }
            return cab;

        }

        public string displayGridFromCancelJob(SqlDataReader reader, DataGridView dgv1, Panel p3, TextBox tb58)
        {

            int i = 0; string refno = ""; string cab = ""; string date = ""; string company = ""; string name = "";
            string paytype = "";

            while (reader.Read())
            {
                dgv1.Rows.Add();

                cab = ("K" + reader["CabNo"].ToString()).Replace(" ", "");
                date = (Convert.ToDateTime(reader["DispatchTime"])).ToShortDateString();
                int ascii1 = Convert.ToInt32((reader["JobID"].ToString().Substring(0, 2)));
                int ascii2 = Convert.ToInt32((reader["JobID"].ToString().Substring(2, 2)));
                char character1 = (char)ascii1; char character2 = (char)ascii2; string character3 = reader["JobID"].ToString().Substring(4, 4);
                refno = (character1.ToString() + character2.ToString() + character3.ToString()).ToString();
                name = reader["Name"].ToString();
                paytype = reader["CashCredit"].ToString();

                try
                {
                    company = (reader["Organization"].ToString()).Replace(" ", "");
                }
                catch (IndexOutOfRangeException) { }
                dgv1.Rows[i].Cells[0].Value = cab;
                dgv1.Rows[i].Cells[1].Value = date;
                dgv1.Rows[i].Cells[7].Value = company;
                dgv1.Rows[i].Cells[3].Value = refno; // reader["JobID"].ToString();
                dgv1.Rows[i].Cells[4].Value = name;

                if (paytype == "CA")
                    dgv1.Rows[i].Cells[5].Value = "Cash";
                if (paytype == "CR")
                {
                    dgv1.Rows[i].Cells[5].Value = "Credit";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Yellow;
                }
                if (paytype == "CV")
                {
                    dgv1.Rows[i].Cells[5].Value = "Voucher";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }
                if (paytype == "CO")
                {
                    dgv1.Rows[i].Cells[5].Value = "Corperate";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Orange;
                }

                if (paytype == "VW")
                {
                    dgv1.Rows[i].Cells[5].Value = "Wallet";
                    dgv1.Rows[i].Cells[5].Style.BackColor = Color.Green;
                }

                //string s = reader["Organization"].ToString();
                //if (reader["Organization"].ToString() != "1")// destination is used to update the paid or not (becuse of unable to add new colum to job table)
                //    dgv1.Rows[i].Cells[6].Value = "No";
                //if (reader["Organization"].ToString() == "1")
                //{
                dgv1.Rows[i].Cells[6].Value = "Cancelled";
                dgv1.Rows[i].Cells[6].Style.BackColor = Color.Red;
                //}
                i++;
            }

            try
            {
                // sendVoucherRefFromJob(refno, cab, String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(date)), name, paytype, "0", company);
            }
            catch (Exception) { }
            return cab;

        }

        public void sendVoucherRefFromJob(string voucherRefNoo, string CabNo, string voucherDate, string client, string paytype, string paid, string Organization) //if refno not available in voucher ref table, get ref from job table and insert into voucherref table
        {
            try
            {
                SqlConnection connection = new SqlConnection(constr5);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO VoucherRef(voucherRefNo,CabNo,voucherDate,client,paytype,paid,Organization) VALUES('" + voucherRefNoo + "', '" + CabNo + "', '" + voucherDate + "', '" + client + "', '" + paytype + "', '" + paid + "','" + Organization + "')";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception) { }
        }

        public void gridFormat(DataGridView dgv)
        {
            dgv.Columns[0].Width = 40;
            dgv.Columns[1].Width = 70;
            dgv.Columns[2].Width = 65;
            dgv.Columns[3].Width = 80;
            dgv.Columns[4].Width = 100;
            dgv.Columns[5].Width = 65;
            dgv.Columns[6].Width = 40;
            dgv.Columns[7].Width = 160;
        }

        //public void getCallingNoForSelectedTaxi(DataGridView dgv5, TextBox tbCabNo, string date)
        //{
        //    string cab = "K" + tbCabNo.Text;

        //    dgv5.Rows.Clear();



        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();


        //    DataSet1 recfd = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";

        //    using (var reader = command1.ExecuteReader())
        //    {
        //        displayGrid(reader, dgv5);
        //    }
        //    connection.Close();

        //    gridFormat(dgv5);

        //}

        public void getCallingNoForSelectedTaxi(DataGridView dgv5, TextBox tbCabNo, string date, Panel p3, TextBox tb58)
        {
            string cab = "K" + tbCabNo.Text;

            dgv5.Rows.Clear();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();
            //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
            command1.CommandText = command1.CommandText = "SELECT CabNo,voucherDate,voucherRefNo,client,paytype,paid FROM VoucherRef WHERE voucherDate='" + date + "' AND CabNo='" + cab + "' ORDER BY CabNo";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                    displayGrid(reader, dgv5, p3, tb58, tbCabNo);
            }
            connection.Close();

            gridFormat(dgv5);

        }


        


        public bool bolockCab(string cabno)
        {

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT id FROM TestCabBlock WHERE (cab='" + cabno + "' AND flag='Y')";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    blockID = Convert.ToInt32(reader["id"].ToString());
                    connection.Close();
                    return true;
                }
                else
                {
                    connection.Close();
                    return false;
                }
            }

        }

        public void unblock(string remark, string cabno, Panel p3)
        {
            us = new User();
            string user = us.getCurrentUser();

            string vdate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "UPDATE TestCabBlock SET remarkunblocked='" + remark + "',unblockedby='" + user + "',unblockeddate='" + vdate + "',flag='N' where (cab='" + cabno + "' AND id='" + blockID + "')";
            command1.ExecuteNonQuery();
            connection.Close();

            MessageBox.Show("Unblocked....!!!!");
            p3.Visible = false;

        }

        //server migration 02-06-2015
        //public string getCabNoForSelectedVocher(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura)
        //{
        //    dgv5.Rows.Clear(); string cab = "";

        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    DataSet1 recfd = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    //command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo WHERE TestVoucherRef.Date='" + date + "' AND TestVoucherRef.voucherRefNo='" + tbVoucher.Text + "' ORDER BY TestVoucherRef.CabNo";

        //    command1.CommandText = "SELECT CabNo,Date,voucherRefNo,client,paytype,paid FROM TestVoucherRef WHERE Date='" + date + "' AND RIGHT(voucherRefNo,4)='" + tbVoucher.Text + "' ORDER BY CabNo";
        //    using (var reader = command1.ExecuteReader())
        //    {
        //        //while (reader.Read())
        //        //{
        //        //    string vocherRef = reader["voucherRefNo"].ToString();
        //        //}

        //        cab = displayGrid(reader, dgv5);
        //    }
        //    connection.Close();

        //    gridFormat(dgv5);
        //    return cab;
        //}

        //public string getCabNoForSelectedVocher(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura,Panel p3,TextBox tb58,TextBox tbCabno)
        //{
        //Voucher vr=new Voucher();
        //string cab = "";
        //if (tbCabno.Text != null && tbCabno.Text!="")
        //{
        //    dgv5.Rows.Clear();

        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    DataSet1 recfd = new DataSet1();
        //    SqlConnection connection = new SqlConnection(constr5);
        //    connection.Open();
        //    SqlCommand command1 = connection.CreateCommand();
        //    //command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo WHERE TestVoucherRef.Date='" + date + "' AND TestVoucherRef.voucherRefNo='" + tbVoucher.Text + "' ORDER BY TestVoucherRef.CabNo";         
        //    command1.CommandText = "SELECT CabNo,voucherDate,voucherRefNo,client,paytype,paid,Organization FROM VoucherRef WHERE voucherDate='" + date + "' AND RIGHT(voucherRefNo,4)='" + tbVoucher.Text + "' ORDER BY CabNo";
        //    using (var reader = command1.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //             displayGrid(reader, dgv5, p3, tb58,tbCabno);
        //           //bool result= vr.CheckCabValidity(cab, tbCabno);
        //        }
        //        else                       
        //getRefNoForSelectedTaxiFromJob(dgv5, tbVoucher, date, tbventura, p3, tb58, tbCabno);

        //    }

        //    connection.Close();

        //    gridFormat(dgv5);
        //}
        //else
        //    MessageBox.Show("Please Enter The Cab No");

        //return cab;
        //}

        public string getCabNoForSelectedRefFromJob(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        {
            dgv5.Rows.Clear(); string cab = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();
            //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
            command1.CommandText = command1.CommandText = "SELECT  CabNo,DispatchTime,JobID,Name,CashCredit,Organization,Paid FROM Job WHERE  (RIGHT(JobID,4)='" + tbVoucher.Text + "') AND (CAST(DispatchTime AS DATE)='" + date + "') AND (Flag='DE') ORDER BY JobID DESC";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    cab = displayGridFromJob(reader, dgv5, p3, tb58);

                    if (CheckCab(cab, tbCabNo) == true)
                        tbCabNo.Text = cab.Substring(1);
                }
                else
                {
                    getCabNoForSelectedRefFromCancelJob(dgv5, tbVoucher, date, tbventura, p3, tb58, tbCabNo);
                }
            }

            connection.Close();

            gridFormat(dgv5);
            return cab;
        }

        
        public string getCabNoForSelectedRefFromDespatchBooking(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        {
            dgv5.Rows.Clear(); string cab = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr7);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
            command1.CommandText = command1.CommandText = "SELECT 	RefNo as voucherRefNo, CabNo,RecordInsertTime as DispatchTime, RefNo , ClientName as Name,PaymentType as CashCredit,Amount, ClientAddress as Organization,Paid FROM DespatchBooking WHERE  (RefNo='" + tbVoucher.Text + "') AND ( DATE (RecordInsertTime)='" + date + "')  ORDER BY AutoID DESC";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    cab = displayGridFromJobAPP(reader, dgv5, p3, tb58);

                    if (CheckCab(cab, tbCabNo) == true)
                        tbCabNo.Text = cab.Substring(1);
                }
                else
                {
                    getCabNoForSelectedRefFromLogSheet(dgv5, tbVoucher, date, tbventura, p3, tb58, tbCabNo);
                }
            }

            connection.Close();

            gridFormat(dgv5);
            return cab;
           
        }

         public string getCabNoForSelectedRefFromLogSheet(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        {
            dgv5.Rows.Clear(); string cab = "";

            char char1 = Convert.ToChar(tbVoucher.Text.Substring(0, 1));
            char char2 = Convert.ToChar(tbVoucher.Text.Substring(1, 1));
            string str = tbVoucher.Text.Substring(2, 4);

            string ascii1 = ((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
            string asciiRefNo = tbVoucher.Text;  // ascii1 + ascii2 + str;



            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            //MySqlConnection connection = new MySqlConnection(constr7);
            //MySqlConnection connection = new MySqlConnection(constr8);
            MySqlConnection connection = new MySqlConnection(constr9);

            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
          // SELECT 	RefNo as voucherRefNo, CabNo,RecordInsertTime as DispatchTime, RefNo , ClientName as Name,PaymentType as CashCredit,Amount, ClientAddress as Organization,Paid FROM DespatchBooking WHERE  (RefNo='" + tbVoucher.Text + "') AND ( DATE (RecordInsertTime)='" + date + "')  ORDER BY AutoID DESC
           // command1.CommandText = command1.CommandText = "SELECT VehiclID as CabNo, MeterOFFTime as DispatchTime,BookingID as voucherRefNo,Amount,WaitingCost, PaymentType as CashCredit,ClientName as Name,ClientAddress as Organization, Paid FROM viewLogSheet WHERE  (BookingID = '" + asciiRefNo + "') AND (VehiclID='" + tbCabNo.Text + "')  ORDER BY BookingID DESC";
           // command1.CommandText = "SELECT vehicl_id as CabNo, meter_off_time as DispatchTime,booking_id as voucherRefNo,total_amount as Amount, waiting_cost as WaitingCost, payment_type as CashCredit,cus_name as Name,meter_on_loation as Organization, Paid FROM budget_bookings WHERE  (booking_id = '" + asciiRefNo + "') AND (vehicl_id='" + tbCabNo.Text + "')  ORDER BY booking_id DESC";

            command1.CommandText = "SELECT cabNo as CabNo,bookingType as bookingType, endTime as DispatchTime,refID as voucherRefNo,totalFare as Amount, waitingFare as WaitingCost, paymentMethod as CashCredit,customerFirstName as Name,pickUpAddress as Organization, Paid, organization as orgz,discountType as dtype,voucherNumber as voucherNo FROM bookings WHERE  (refID = '" + asciiRefNo + "') AND (cabNo='" + tbCabNo.Text + "')  ORDER BY refID DESC";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    cab = displayGridFromLogsheet(reader, dgv5, p3, tb58);

                    if (CheckCab(cab, tbCabNo) == true)
                        tbCabNo.Text = cab.Substring(1);

                }
            }

            connection.Close();

            gridFormat(dgv5);
            return cab;
        }

        
        public string getCabNoForSelectedRefFromCancelJob(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        {
            dgv5.Rows.Clear(); string cab = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();
            //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
            command1.CommandText = command1.CommandText = "SELECT TOP 1000 CabNo,DispatchTime,JobID,Name,CashCredit,Organization FROM CancelJob WHERE  (RIGHT(JobID,4)='" + tbVoucher.Text + "') AND (CAST(CancelFinishTime AS DATE)='" + date + "') AND (Flag='DE') ORDER BY JobID DESC";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    cab = displayGridFromCancelJob(reader, dgv5, p3, tb58);

                    if (CheckCab(cab, tbCabNo) == true)
                        tbCabNo.Text = cab.Substring(1);

                }
            }

            connection.Close();

            gridFormat(dgv5);
            return cab;
        }

        public bool CheckCab(string cab, TextBox tbCab)
        {
            string part1 = ""; string cabnumber = "";

            part1 = cab.Substring(0, 1);

            if (part1 == "K" || part1 == "k")
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
                MessageBox.Show("This Reference No is Allocated to More than One Cab, Please check Again!");
                return false;
                //tbCab.Text = "";

            }
        }

        public string getRefNoForSelectedCabFromJob(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        {
            dgv5.Rows.Clear(); string cab = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();
            SqlConnection connection = new SqlConnection(constr5);
            connection.Open();
            SqlCommand command1 = connection.CreateCommand();
            //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
            command1.CommandText = command1.CommandText = "SELECT TOP 1000 CabNo,DispatchTime,JobID,Name,CashCredit,Organization,Paid FROM Job WHERE  (CabNo='" + tbCabNo.Text + "') AND (CAST(DispatchTime AS DATE)='" + date + "') AND (Flag='DE') ORDER BY JobID DESC";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    cab = displayGridFromJob(reader, dgv5, p3, tb58);
                    //tbCabNo.Text = cab.Substring(1);
                }
            }

            connection.Close();

            gridFormat(dgv5);
            return cab;
        }


        public string getRefNoForSelectedCabFromNewDispatch(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        {
            dgv5.Rows.Clear(); string cab = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();

           
            MySqlConnection connection = new MySqlConnection(constr7);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT RefNo as voucherRefNo,CabNo,  DATE(RecordInsertTime) as voucherDate , ClientName as client, PaymentType as paytype,Paid as paid,ClientAddress as Organization FROM DespatchBooking WHERE DATE(RecordInsertTime)='" + date + "' ORDER BY CabNo ";//WHERE voucherDate='" + date + "' ORDER BY CabNo

            
            //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
            command1.CommandText = command1.CommandText = "SELECT  CabNo,DATE(RecordInsertTime) as DispatchTime,ClientName as Name,RefNo as voucherRefNo,  PaymentType as CashCredit,ClientAddress as Organization , Amount, paid FROM DespatchBooking WHERE  (CabNo='" + tbCabNo.Text + "') AND (DATE(RecordInsertTime)='" + date + "')  ORDER BY AutoID DESC";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    cab = displayGridFromJobAPP(reader, dgv5, p3, tb58);
                    //tbCabNo.Text = cab.Substring(1);
                }
            }

            connection.Close();

            gridFormat(dgv5);
            return cab;
        }

        public string getRefNoForSelectedCabFromLogsheet(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        {
            dgv5.Rows.Clear(); string cab = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataSet1 recfd = new DataSet1();


            //MySqlConnection connection = new MySqlConnection(constr7);
            //MySqlConnection connection = new MySqlConnection(constr8);
            MySqlConnection connection = new MySqlConnection(constr9);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT RefNo as voucherRefNo,CabNo,  DATE(RecordInsertTime) as voucherDate , ClientName as client, PaymentType as paytype,Paid as paid,ClientAddress as Organization FROM DespatchBooking WHERE DATE(RecordInsertTime)='" + date + "' ORDER BY CabNo ";//WHERE voucherDate='" + date + "' ORDER BY CabNo


            //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
           // command1.CommandText = command1.CommandText = command1.CommandText = command1.CommandText = "SELECT VehiclID as CabNo, MeterOFFTime as DispatchTime,BookingID as voucherRefNo,Amount,WaitingCost,  PaymentType as CashCredit,ClientName as Name,ClientAddress as Organization , Paid FROM viewLogSheet WHERE  (VehiclID='" + tbCabNo.Text + "') AND (DATE(MeterOFFTime)='" + date + "') ORDER BY BookingID DESC";
            //command1.CommandText = "SELECT 	vehicl_id as CabNo, meter_off_time as DispatchTime,booking_id as voucherRefNo,total_amount as Amount,waiting_cost as WaitingCost,  payment_type as CashCredit,cus_name as Name,meter_on_loation as Organization , Paid FROM budget_bookings WHERE  (vehicl_id='" + tbCabNo.Text + "') AND (DATE(meter_off_time)='" + date + "') ORDER BY booking_id DESC";

           // command1.CommandText = "SELECT 	cabNo as CabNo, endTime as DispatchTime,refID as voucherRefNo,totalFare as Amount,waitingFare as WaitingCost,  paymentMethod as CashCredit,customerFirstName as Name,pickUpAddress as Organization , Paid FROM bookings WHERE  (cabNo='" + tbCabNo.Text + "') AND (DATE(endTime)='" + date + "') ORDER BY refID DESC";

            command1.CommandText = "SELECT 	cabNo as CabNo,bookingType as bookingType, endTime as DispatchTime,refID as voucherRefNo,totalFare as Amount,waitingFare as WaitingCost,  paymentMethod as CashCredit,customerFirstName as Name,pickUpAddress as Organization , Paid ,organization as orgz,discountType as dtype,voucherNumber as voucherNo,requiredCabNo FROM bookings WHERE  (cabNo='" + tbCabNo.Text + "') AND (DATE(endTime)='" + date + "') ORDER BY refID DESC";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    cab = displayGridFromLogsheet(reader, dgv5, p3, tb58);
                    //tbCabNo.Text = cab.Substring(1);
                }
            }

            connection.Close();

            gridFormat(dgv5);
            return cab;
        }



        //public void getAllRefForAllCabsFromJob(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        //{
        //    dgv5.Rows.Clear(); string cab = "";
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    DataSet1 recfd = new DataSet1();
        //    SqlConnection connection = new SqlConnection(constr5);
        //    connection.Open();
        //    SqlCommand command1 = connection.CreateCommand();
        //    //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
        //    command1.CommandText = command1.CommandText = "SELECT TOP 25 CabNo,DispatchTime,JobID,Name,CashCredit,Organization FROM Job WHERE CAST(DispatchTime AS DATE)='" + date + "' AND (Flag='DE') ORDER BY JobID DESC";

        //    using (var reader = command1.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //            cab = displayGridFromJob(reader, dgv5, p3, tb58);
        //            /// tbCabNo.Text = cab.Substring(1);
        //        }
        //    }

        //    connection.Close();

        //    gridFormat(dgv5);
        //    //return cab;
        //}


        //public void getAllRefForAllCabsFromDespatchBooking(DataGridView dgv5, TextBox tbVoucher, string date, TextBox tbventura, Panel p3, TextBox tb58, TextBox tbCabNo)
        //{
        //    dgv5.Rows.Clear(); string cab = "";
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();

        //    DataSet1 recfd = new DataSet1();
        //    SqlConnection connection = new SqlConnection(constr5);
        //    connection.Open();
        //    SqlCommand command1 = connection.CreateCommand();
        //    //  command1.CommandText = "SELECT TestCallingNo.CabNo,TestCallingNo.Date,TestVoucherRef.voucherRefNo,TestCallingNo.Cnumber,TestVoucherRef.client,TestVoucherRef.paytype,TestVoucherRef.paid FROM TestCallingNo INNER JOIN TestVoucherRef ON TestCallingNo.CabNo=TestVoucherRef.CabNo AND TestCallingNo.Date=TestVoucherRef.Date WHERE TestCallingNo.Date='" + date + "' AND TestCallingNo.CabNo='" + cab + "' ORDER BY TestVoucherRef.CabNo";
        //    command1.CommandText = command1.CommandText = "SELECT TOP 25 CabNo,DispatchTime,JobID,Name,CashCredit,Organization FROM Job WHERE CAST(DispatchTime AS DATE)='" + date + "' AND (Flag='DE') ORDER BY JobID DESC";

        //    using (var reader = command1.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //            cab = displayGridFromJob(reader, dgv5, p3, tb58);
        //            /// tbCabNo.Text = cab.Substring(1);
        //        }
        //    }

        //    connection.Close();

        //    gridFormat(dgv5);
        //    //return cab;
        //}

        public bool checkFroDailyPayment(DataGridView dgv1, DateTimePicker dtpicker)
        {
            string cab = dgv1.Rows[0].Cells[0].Value.ToString();
            string date = String.Format("{0:yyyy-MM-dd}", dtpicker.Value);
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo FROM TestPayment WHERE (CabNo='" + cab + "') AND (Date='" + date + "') AND (TestPayment.Delete!='Y')";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    connection.Close();
                    return true;

                }
                else
                {
                    connection.Close();
                    return false;

                }
            }

        }

        public bool checkForNightCars(DataGridView dgv1, DateTimePicker dtpicker)
        {
            string cab = dgv1.Rows[0].Cells[0].Value.ToString();
            DateTime previousDate = dtpicker.Value.AddDays(-1);
            string date = String.Format("{0:yyyy-MM-dd}", previousDate);
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo FROM TestPayment WHERE (CabNo='" + cab + "') AND (Date='" + date + "') AND (NightFlag ='Y') AND (TestPayment.Delete!='Y')";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    connection.Close();
                    return true;

                }
                else
                {
                    connection.Close();
                    return false;
                }
            }

        }

        public bool checkForFreeDays(DataGridView dgv1, DateTimePicker dtpicker)
        {
            string cab = dgv1.Rows[0].Cells[0].Value.ToString();
            string date = String.Format("{0:yyyy-MM-dd}", dtpicker.Value);
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo FROM TestFreePayment WHERE (CabNo='" + cab + "' AND Date='" + date + "') AND (TestFreePayment.Cancel!='Y')";

            using (var reader = command1.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    connection.Close();
                    return true;
                }
                else
                {
                    connection.Close();
                    return false;
                }
            }

        }

        public void voucherDetailsView(DataGridView dgv5, object sender, DataGridViewCellEventArgs e, TextBox tbCab, DateTimePicker dtDate, TextBox tbRef, TextBox tbCalling, TextBox tbPayType, TextBox tbCompany, Panel panel1, DateTimePicker dtpicker, CheckBox cb1, TextBox tbcomm, Panel p3, TextBox tb58, Label lbGridRowIndex,Button btnAdd,DataGridView dgvSelected,TextBox tbAmount,TextBox tbAppAmount,CheckBox chb2,CheckBox chbSlt,TextBox tbComRate)
        {
            double amount=0.00;
            cb1.Checked = false;
            tbcomm.Text = "7.5"; tbcomm.Enabled = false;
            tbPayType.Text = "Credit";
            string type = ""; 
            string cab="";
            cab=dgv5.Rows[e.RowIndex].Cells[0].Value.ToString();
            tbCab.Text = cab;
            if (checkCabNoForExtraCom(cab) == true)
            {
                if (dgv5.Rows[e.RowIndex].Cells[3].Style.BackColor == Color.Red)
                {
                    MessageBox.Show("This cab will be charged 15% commission");
                    tbComRate.Text = "15";
                }

            }
            else 
            {
                tbComRate.Text = "7.5";
            }


            if (dgv5.Rows[e.RowIndex].Cells[2].Value != null)
            amount = Convert.ToDouble(dgv5.Rows[e.RowIndex].Cells[2].Value.ToString());
            tbAppAmount.Text = ((int)Math.Round(amount, 0)).ToString();
            tbAmount.Text= ((int)Math.Round(amount, 0)).ToString();
            string refno=dgv5.Rows[e.RowIndex].Cells[3].Value.ToString();
            type = dgv5.Rows[e.RowIndex].Cells[5].Value.ToString();

            string[] split = tbCab.Text.Split(new string[] { "K" }, StringSplitOptions.RemoveEmptyEntries);
            string cabno = split[0];
            if (bolockCab(cabno) == false)
            {


                //tbDate.Text = dgv5.Rows[e.RowIndex].Cells[1].Value.ToString();
                dtDate.Text = (dgv5.Rows[e.RowIndex].Cells[1].Value.ToString());
                bool paid = checkFroDailyPayment(dgv5, dtpicker);
                bool freeOk = checkForFreeDays(dgv5, dtpicker);
                bool nightok = checkForNightCars(dgv5, dtpicker);
                try
                {
                    //tbCalling.Text = dgv5.Rows[e.RowIndex].Cells[2].Value.ToString();
                }
                catch (NullReferenceException) { }
                tbRef.Text = dgv5.Rows[e.RowIndex].Cells[3].Value.ToString();
                if (paid == true || freeOk == true || nightok == true)
                {
                    if ("Paid" != dgv5.Rows[e.RowIndex].Cells[6].Value.ToString())
                    {
                        if (type != "SLT")
                        {
                            if ("No" == dgv5.Rows[e.RowIndex].Cells[6].Value.ToString())// to ignore selected voucher 28/09/2015
                            {
                                if (type == "Call-UP") {
                                    tbcomm.Text = "0"; // if call up, no 7.5 discount 

                                }

                                try
                                {
                                    tbCompany.Text = dgv5.Rows[e.RowIndex].Cells[4].Value.ToString();
                                }
                                catch (NullReferenceException) { }
                                try
                                {
                                    tbPayType.Text = dgv5.Rows[e.RowIndex].Cells[5].Value.ToString();
                                }
                                catch (NullReferenceException) { }
                                panel1.Visible = true;
                                btnAdd.Enabled = true;

                                lbGridRowIndex.Text = e.RowIndex.ToString();
                            }
                        }
                        else if (type == "SLT")
                        {

                            //DialogResult dr = MessageBox.Show("මෙය SLT වව්චර් එකකි, කරුණාකර වෙනම ලදු පතක් ලබා දෙන්න.This is a SLT Voucher, Please pay as a seperate receipt!, Are you sure want to continue ", "SLT Voucher", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                            //if (dr == DialogResult.Yes)
                            //{
                                chb2.Checked = true;
                                chbSlt.Checked = true;
                                tbRef.Text = dgv5.Rows[e.RowIndex].Cells[3].Value.ToString();
                                tbAmount.Text = "0";
                                tbAppAmount.Text = "0";
                                tbPayType.Text = "SLT";

                                lbGridRowIndex.Text = e.RowIndex.ToString();
                            //}

                        }
                    }
                    else if ("Cancelled" == dgv5.Rows[e.RowIndex].Cells[6].Value.ToString())
                    {
                        MessageBox.Show("This is a Cancelled Voucher...!! If You Wish to Pay for This, Please Pay as a Manual Payment ..!");
                    }
                    else if ("Selected" == dgv5.Rows[e.RowIndex].Cells[6].Value.ToString())
                    {
                        DialogResult dr = MessageBox.Show("Sorry!,You Have Piad For " + tbRef.Text + "", "Confirm", MessageBoxButtons.OK);

                    }
                    //else if (true == checkFromSelectedVouchers(dgvSelected, refno))
                    //{
                    //    DialogResult dr = MessageBox.Show("Sorry!,You Have Piad For " + tbRef.Text + "", "Confirm", MessageBoxButtons.OK); 
                    //}
                }
                else if (paid == false && freeOk == false)
                {
                    DialogResult dr = MessageBox.Show("Please Make a Payment for " + dtpicker.Value.ToString(), "Payment", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    if (dr == DialogResult.Yes)
                    {
                        Form20 f20 = new Form20();
                        f20.textBox1.Text = tbCab.Text;
                        f20.dateTimePicker1.Value = dtpicker.Value;
                        f20.textBox2.Text = perDayCharge.ToString();
                        f20.Show();
                    }
                }
            }
            else
            {

                DialogResult dr = MessageBox.Show("This Cab is Blocked. If you wish to unblock please press Yes if not press No", "Blocked", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.Yes)
                {
                    p3.Visible = true;
                   
                    tb58.Text = cabno;
                }
            }

            
        }

        //public bool checkFromSelectedVouchers(DataGridView dgvSelected,string refno) 
        //{
        //    for (int i = 0; i < dgvSelected.Rows.Count; i++)
        //    {
        //        if (dgvSelected.Rows[i].Cells[2].Value != null)
        //        {
        //            if (refno == dgvSelected.Rows[i].Cells[2].Value)
        //                return true;
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //    return false;
        //}

        public void SavePaymentFroOneDayFromVouchers(TextBox tbrecno, TextBox tbCabNo, TextBox tbAmount, DateTimePicker dtpicker)
        {
            DialogResult dr = MessageBox.Show("Are You Sure Want To Save", "Confirm", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                us = new User();
                nrcn = new NewReceiptNumber();
                //string recno = nrcn.decideReceiptNo(tbrecno);
                string recno = nrcn.getReciptNo();
                tbrecno.Text = recno;
                string user = us.getCurrentUser();
                string date = String.Format("{0:yyyy-MM-dd}", dtpicker.Value);
                string dateNow = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

                string location = "";
                location = get_Location();
                string enteredDateTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                

                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO TestReciptHeader(RecNo,ReciptDate,ReciptAmount,CabNo,DateFrom,DateTo,nDays,TotalAmountWord,TotBillRecv,Flag,UserID,monthFd,EnteredDateTime,Location) VALUES('" + recno + "','" + dateNow + "','" + tbAmount.Text + "','" + tbCabNo.Text + "','" + date + "','" + date + "','1','Three Hundred Rupees onlly','0','0','" + user + "','','"+enteredDateTime+"','"+location+"')";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO TestPayment(CabNo,RecNo,Date,Amount,Cancel) VALUES('" + tbCabNo.Text + "','" + recno + "','" + date + "','" + tbAmount.Text + "','N')";
                command.ExecuteNonQuery();
                connection.Close();

                //nrcn.decideReciptUpdate(tbrecno);
                nrcn.updateReceiptNo(tbrecno);
                MessageBox.Show("Saved");

                printReceipt(tbCabNo, tbrecno);
            }

        }

        public void panelClear(TextBox tbcabNo, TextBox tbCallNo, DateTimePicker dtDate, TextBox tbRefNo, TextBox tbCompany, TextBox tbPayType, TextBox tbAmount, TextBox tbVno,TextBox tbApamt)
        {


            tbRefNo.Clear(); tbcabNo.Clear(); dtDate.Value = DateTime.Now; tbRefNo.Clear(); tbCompany.Clear(); tbPayType.Clear(); tbAmount.Clear(); tbVno.Clear(); tbAmount.Text = "0";

          

        }
        //public void vouchersSaveWithRef(TextBox tbcabNo, TextBox tbCallNo, TextBox tbDate, TextBox tbRefNo, TextBox tbCompany, TextBox tbPayType, TextBox tbvoucherNo, TextBox tbVoucherAmount, TextBox tbCommRate, TextBox tbCommition, TextBox tbBallAmount, DataGridView dgv5, DateTimePicker dtdate, DataGridView dgv6, TextBox tbRefNum, TextBox tbBalAmount, TextBox tbNumOfVoucher, TextBox tbTotAmount, string selectedDate, TextBox tbSelectedCab, TextBox tbAllCabNo,Panel p3,TextBox tb58)
        //{

        //    us = new User();
        //    string user = us.getCurrentUser();

        //    string vloc = EnteredLocation();

        //    DialogResult dr = MessageBox.Show("Are You Sure Want To Add", "Confirm", MessageBoxButtons.YesNoCancel);


        //    if (dr == DialogResult.Yes)
        //    {
        //        addVouchersToList(dgv6, tbRefNum, tbcabNo, tbBalAmount, tbNumOfVoucher, tbTotAmount, tbAllCabNo, tbVoucherAmount, tbDate, tbCompany, tbPayType, tbvoucherNo,tbCommRate,tbCommition);
        //        string vdate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(tbDate.Text));

        //        MySqlConnection connection = new MySqlConnection(constr);
        //        connection.Open();
        //        MySqlCommand command = connection.CreateCommand();
        //        command.CommandText = "INSERT INTO TestRefVoucherPay (cabNo,Cnumber,VoucherDate,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location)  VALUES('" + tbcabNo.Text + "','" + tbCallNo.Text + "','" + vdate + "','" + tbRefNo.Text + "','" + tbCompany.Text + "','" + tbPayType.Text + "','" + tbvoucherNo.Text + "','" + tbVoucherAmount.Text + "','" + tbCommRate.Text + "','" + tbCommition.Text + "','" + tbBallAmount.Text + "','" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "', '" + String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now) + "','" + user + "','" + vloc + "')";
        //        command.ExecuteNonQuery();
        //        connection.Close();

        //        SqlConnection con = new SqlConnection(constr5);//call center DB table update
        //        con.Open();
        //        SqlCommand cmd = con.CreateCommand();
        //        cmd.CommandText = "UPDATE VoucherRef SET paid=1 WHERE (CabNo='" + tbcabNo.Text + "' AND voucherDate='" + vdate + "') AND voucherRefNo= '" + tbRefNo.Text + "'  ";
        //        cmd.ExecuteNonQuery();
        //        con.Close();

        //        getCallingNoForSelectedTaxi(dgv5, tbSelectedCab, selectedDate,p3,tb58);
        //    }

        //}

        //public bool addVouchersToList(DataGridView dgv6, TextBox tbRefno, TextBox tbcabno, TextBox tbBalAmount, TextBox tbNumOfVoucher, TextBox tbTotalAmount, TextBox tballCabno,TextBox tbVoucherAmount,TextBox tbVoucherDate,TextBox tbCompany,TextBox tbPayType,TextBox tbVoucherNo,TextBox tbComRate,TextBox tbComm)
        public bool addVouchersToList(TextBox tbcabNo, TextBox tbCallNo, DateTimePicker dtvdate, TextBox tbRefNo, TextBox tbCompany, TextBox tbPayType, TextBox tbvoucherNo, TextBox tbVoucherAmount, TextBox tbCommRate, TextBox tbCommition, TextBox tbBallAmount, DataGridView dgv5, DateTimePicker dtdate, DataGridView dgv6, TextBox tbRefNum, TextBox tbBalAmount, TextBox tbNumOfVoucher, TextBox tbTotAmount, string selectedDate, TextBox tbSelectedCab, TextBox tbAllCabNo, Panel p3, TextBox tb58, TextBox tbAddEarn, TextBox tbDeduction, TextBox tbNetAmount, Label lbGridRowIndex,TextBox tbPreDeduct,TextBox tbPreRefund,TextBox tbAppAmount, CheckBox chbRfYN)
        {
            int index = Convert.ToInt32(lbGridRowIndex.Text);
            Voucher vr = new Voucher();
            if (checkVoucherGrid(dgv6, tbRefNo,tbvoucherNo) == false)
            {
                DialogResult dr = MessageBox.Show("Are You Sure Want To Add", "Confirm", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    if (tbBalAmount.Text != "")
                    {
                        dgv5.Rows[index].Cells[6].Style.BackColor = Color.Red;//change the selected voucher color.
                        dgv5.Rows[index].Cells[6].Value = "Selected";
                        for (int i = 1; i < dgv6.Rows.Count; i++)
                        {
                            if (dgv6.Rows[i].Cells[0].Value == null)
                            {
                                dgv6.Rows[i].Cells[0].Value = tbcabNo.Text;
                                dgv6.Rows[i].Cells[1].Value = String.Format("{0:yyyy-MM-dd}", dtvdate.Value);
                                dgv6.Rows[i].Cells[2].Value = tbRefNo.Text;
                                dgv6.Rows[i].Cells[3].Value = tbCompany.Text;
                                dgv6.Rows[i].Cells[4].Value = tbPayType.Text;
                                dgv6.Rows[i].Cells[5].Value = tbvoucherNo.Text;
                                dgv6.Rows[i].Cells[6].Value = tbVoucherAmount.Text;
                                dgv6.Rows[i].Cells[7].Value = tbCommRate.Text;
                                dgv6.Rows[i].Cells[8].Value = tbCommition.Text;
                                dgv6.Rows[i].Cells[9].Value = tbBalAmount.Text;
                                dgv6.Rows[i].Cells[10].Value = tbAppAmount.Text;
                                if (chbRfYN.Checked == true) {
                                    dgv6.Rows[i].Cells[11].Value = "N";
                                }
                                else if (chbRfYN.Checked == false) 
                                {
                                    dgv6.Rows[i].Cells[11].Value = "Y";
                                }

                                if (tbCommRate.Text == "15") 
                                {
                                    dgv6.Rows[i].Cells[12].Value = "Y";
                                }
                                if(tbCommRate.Text != "15") 
                                {
                                    dgv6.Rows[i].Cells[12].Value = "N";
                                }
                                vr.calTotalVoucherAmount(dgv6, tbNumOfVoucher, tbTotAmount, tbAllCabNo, tbcabNo, tbAddEarn, tbDeduction, tbNetAmount,tbPreDeduct,tbPreRefund);
                                return true;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Voucher Amount!");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Sorry !!! Alredy added to the Payment Grid");
                return false;
            }
        }

        public bool checkVoucherGrid(DataGridView dgv6, TextBox tbRefNo,TextBox tbVoucherNo)
        {
            int  count = 0;

            if (tbRefNo.Text != "XXXXXX")
            {
              
                for (int i = 1; i < dgv6.Rows.Count; i++)
                {
                    if (dgv6.Rows[i].Cells[2].Value != null)
                    {
                        if (tbRefNo.Text == (dgv6.Rows[i].Cells[2].Value.ToString()))                        
                            count++;             
                    }
                }
                if (count > 0)
                    return true;
                else
                    return false;
            }
            else if (tbRefNo.Text == "XXXXXX")
            {
               
                for (int i = 1; i < dgv6.Rows.Count; i++)
                {
                    if (dgv6.Rows[i].Cells[5].Value != null)
                    {
                        if (tbVoucherNo.Text == (dgv6.Rows[i].Cells[5].Value.ToString()))
                         count++;
                    }
                }
                if (count > 0)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public bool findDuplicateVoucher(DataGridView dgv6) //find dulicate using array list
        {
            string refno="";

            System.Collections.ArrayList arrListIDs = new System.Collections.ArrayList();
            for (int i = 1; i < dgv6.Rows.Count; i++)
            {
                if (dgv6.Rows[i].Cells[2].Value != null)
                {
                    if (dgv6.Rows[i].Cells[2].Value.ToString() != "XXXXXX") //system voucher                   
                         refno =(dgv6.Rows[i].Cells[2].Value.ToString()).Replace(" ", String.Empty);
                    else
                        refno = (dgv6.Rows[i].Cells[5].Value.ToString()).Replace(" ", String.Empty); //manual voucher- no ref number


                        if (!arrListIDs.Contains(refno))
                        {
                            arrListIDs.Add(refno);

                        }
                        else
                        {
                            //MessageBox.Show("Duplicate Voucher Entries Detected !! Please Re-Enter Vouchers");
                            return true;
                        }
                   
                }
            }
            return false;

          }



        public void vouchersSaveWithoutRef(TextBox tbcabNo, TextBox tbCallNo, DateTimePicker dtVdate, TextBox tbCompany, TextBox tbvoucherNo, TextBox tbVoucherAmount, TextBox tbCommRate, TextBox tbCommition, TextBox tbBallAmount)
        {
            us = new User();
            string user = us.getCurrentUser();

            string vloc = EnteredLocation();

            DialogResult dr = MessageBox.Show("Are You Sure Want To Pay", "Confirm", MessageBoxButtons.YesNoCancel);

            if (dr == DialogResult.Yes)
            {
                string vdate = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dtVdate.Text));

                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO TestNoRefVoucherPay (cabNo,Cnumber,VoucherDate,company,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,user,Location) VALUES('" + tbcabNo.Text + "','" + tbCallNo.Text + "','" + vdate + "','" + tbCompany.Text + "','" + tbvoucherNo.Text + "','" + tbVoucherAmount.Text + "','" + tbCommRate.Text + "','" + tbCommition.Text + "','" + tbBallAmount.Text + "','" + String.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now) + "','" + user + "','" + vloc + "')";
                command.ExecuteNonQuery();
                //command.CommandText = "UPDATE TestVoucherRef SET paid=1 WHERE (CabNo='" + tbcabNo.Text + "' AND Date='" + vdate + "') AND voucherRefNo= '" + tbRefNo.Text + "'  ";
                //command.ExecuteNonQuery();
                connection.Close();
                //dgv5.DataSource = null;
                //getCallingNo(dgv5, String.Format("{0:yyyy-MM-dd}", dtdate.Value));
                MessageBox.Show("Paid!!");
            }
        }

        public void releaseVoucherPayment(DataGridView dgv6, TextBox tbNumOfVoucher, TextBox tbTotalAmount, Panel panel2, TextBox tbpanelAmount, TextBox tbNic, TextBox tbcabno, DateTimePicker dtdate, TextBox tbcab, TextBox tbDeduction, TextBox tbRefund, TextBox tbNetAmount, TextBox tbVoucherTot, TextBox tbRefno, CheckBox chb2, CheckBox chbIgnore, CheckBox chbSpecial,TextBox tbPhoneBill,TextBox tbPreRefund,TextBox tbPreDeduct,CheckBox chbFreund,CheckBox chbAppFine,TextBox tbAppfine)
        {
            //DialogResult dr = MessageBox.Show("Are You Sure Want To Pay", "Confirm", MessageBoxButtons.YesNoCancel);

            //if (dr == DialogResult.Yes)
            //{
            //MessageBox.Show("Total Amount is Rs "+tbNetAmount.Text+" /=");

            saveVoucherPaymentProof(tbcabno, dtdate, tbNic, tbTotalAmount, dgv6);
            panel2.Visible = true;
            tbpanelAmount.Text = tbNetAmount.Text;
            dgv6.Rows.Clear();
            dgv6.Rows.Add(25);
            dgv6.Rows[0].Cells[2].Value = "Ref No";
            dgv6.Rows[0].Cells[9].Value = "Amount";
            tbNumOfVoucher.Text = "0";
            tbcab.Text = ""; tbRefno.Text = ""; chb2.Checked = false;
            //tbTotalAmount.Text = "";
            tbNic.Clear();
            tbDeduction.Text = "0.00"; tbRefund.Text = "0.00"; tbNetAmount.Text = "0.00"; tbVoucherTot.Text = "0.00";
            chbIgnore.Checked = false; chbSpecial.Checked = false; tbPhoneBill.Text = "0.00";
            tbPreRefund.Text = "0.00"; tbPreDeduct.Text = "0.00"; chbFreund.Checked = false; chbAppFine.Checked = false; tbAppfine.Text = "0.00";
            //}
        }

        public void VoucherCommition(TextBox tbVamount, TextBox tbCommitionRate, TextBox tbCommition, TextBox tbBalance)
        {
            double voucherAmount = 0.00; double commitionRate = 0.00; double commition = 0.00; double balance = 0.00;
            try
            {
                voucherAmount = Convert.ToDouble(tbVamount.Text);
            }
            catch (Exception) { }
            try
            {
                commitionRate = Convert.ToDouble(tbCommitionRate.Text);
            }
            catch (Exception) { }

            if (commitionRate != 0.00)
                commition = (voucherAmount / 100) * commitionRate;
            if (commitionRate == 0.00)
                commition = 0.00;

            balance = voucherAmount - commition;

            tbCommition.Text = commition.ToString();
            tbBalance.Text = balance.ToString();
                        
        }

        public void displayValidNICForVoucher(TextBox tbcabNo, DataGridView dgv1)
        {
            if (dgv1.Rows.Count >= 1)
                dgv1.Rows.Clear();

            string cab = "k" + tbcabNo.Text;
            int i = 0;

            MySqlConnection connection = new MySqlConnection(constr2);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT NicNo,Name FROM VoucPerm WHERE CabNo='" + cab + "'";
            using (var reader = command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    dgv1.Rows.Add();
                    dgv1.Rows[i].Cells[0].Value = reader["NicNo"].ToString();
                    dgv1.Rows[i].Cells[1].Value = reader["Name"].ToString();
                    i++;
                }
            }
            connection.Close();
        }

        public void addValidNICForVoucher(TextBox tbCabNo, TextBox tbNIC, DataGridView dgv, TextBox tbName)
        {
            if (tbCabNo.Text != null)
            {
                DialogResult dr = MessageBox.Show("Are you Sure Want to save?", "Confirmation", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    string cab = "k" + tbCabNo.Text;
                    MySqlConnection conn = new MySqlConnection(constr2);
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO  VoucPerm(CabNo,NicNo,Name)VALUES('" + cab + "','" + tbNIC.Text + "','" + tbName.Text + "')";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    if (dgv.Rows.Count > 0)
                    {
                        dgv.Rows.Clear();
                    }
                    displayValidNICForVoucher(tbCabNo, dgv);
                }
                MessageBox.Show("Saved");
                tbNIC.Clear();
            }
            else
            {
                MessageBox.Show("Please Enter a Cab No");
            }
        }

        public void saveVoucherPaymentProof(TextBox tbCabno, DateTimePicker dtDate, TextBox tbNIC, TextBox tbAmount, DataGridView dgv)
        {
            us = new User();
            string user = us.getCurrentUser();
            if (tbNIC.Text == "")
                tbNIC.Text = "Unknown";

            string cab = "K" + tbCabno.Text;
            string date = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);
            string voucherRef = "";
            for (int i = 1; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[0].Value != null)
                    voucherRef = voucherRef + ", " + dgv.Rows[i].Cells[0].Value.ToString();
            }
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO testVoucherPayProof VALUES('" + cab + "','" + date + "','" + tbNIC.Text + "','" + tbAmount.Text + "','" + voucherRef + "','" + user + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void addNICfromGridToTextBox(DataGridView dgv, DataGridViewCellEventArgs e, TextBox tbNic, TextBox tbName)
        {
            try
            {
                tbNic.Text = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                tbName.Text = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
            catch (Exception) { }
        }

        public string EnteredLocation()
        {
            if (location == "Y")
                return "Yard - Wijerama";
            if (location == "H")
                return "Head Office - Colombo 04";
            else
                return "";
        }

        public string getLocation()
        {
            return location;
        }

        public void getLastPrintedTime(TextBox tbUser, DateTimePicker dtLastPrintTime, DateTimePicker dtTimeFrom)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT withrefDate from TestPrinter WHERE USER='" + tbUser.Text + "'";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dtLastPrintTime.Text = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", Convert.ToDateTime(reader["withrefDate"]));
                    dtTimeFrom.Text = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", Convert.ToDateTime(reader["withrefDate"]));
                }
            }

            connection.Close();
        }

        public void updateLastPrintedUserWithRef(string user)
        {
            string printedTime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE TestPrinter SET withrefDate='" + printedTime + "' WHERE user='" + user + "'";
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void FindPaidVoucher(TextBox tbRefNo, DateTimePicker dtVoucherDate, TextBox tbVoucherNo, TextBox tbCabNo, TextBox tbVoucherDate, TextBox tbVoucherAmount, TextBox tbPaidAmount, TextBox tbPaiDateTime, TextBox tbPaidBy, TextBox tbPaidLocation)
        {
            string vDate = String.Format("{0:yyyy-MM-dd}", dtVoucherDate.Value);
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            //command.CommandText = "SELECT * FROM TestRefVoucherPay  WHERE (VoucherDate='" + vDate + "' AND RIGHT(voucherRefNo,4)='" + tbRefNo.Text + "') OR VoucherNo='" + tbVoucherNo.Text + "'";
            command.CommandText = "SELECT * FROM TestRefVoucherPay  WHERE (VoucherDate='" + vDate + "' AND RIGHT(voucherRefNo,4)='" + tbRefNo.Text + "')";
            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    //dtOpDate.Value = Convert.ToDateTime(reader["OpDate"].ToString());

                    tbRefNo.Text = reader["voucherRefNo"].ToString();
                    tbVoucherNo.Text = reader["VoucherNo"].ToString();
                    tbCabNo.Text = reader["cabNo"].ToString();
                    tbVoucherDate.Text = reader["VoucherDate"].ToString();
                    tbVoucherAmount.Text = reader["VoucherAmount"].ToString();
                    tbPaidAmount.Text = reader["BalAmount"].ToString();
                    tbPaiDateTime.Text = reader["PayDateTime"].ToString();
                    tbPaidBy.Text = reader["user"].ToString();
                    tbPaidLocation.Text = reader["Location"].ToString();

                }

            }
            connection.Close();

        }

        public void FindPaidVoucherNo(TextBox tbRefNo, DateTimePicker dtVoucherDate, TextBox tbVoucherNo, TextBox tbCabNo, TextBox tbVoucherDate, TextBox tbVoucherAmount, TextBox tbPaidAmount, TextBox tbPaiDateTime, TextBox tbPaidBy, TextBox tbPaidLocation, RadioButton rb1, RadioButton rb2,Label lbRecNo,DataGridView dgv1,CheckBox chbAll)
        {
            string recno = "";
            if (rb1.Checked == true || rb2.Checked == true)
            {
                if ((tbRefNo.Text == "" || tbRefNo.Text == null))
                    tbRefNo.Text = "xxxxx";

                if (tbVoucherNo.Text == "" || tbVoucherNo == null)
                    tbVoucherNo.Text = "XXXXXXXX";

                string vDate = String.Format("{0:yyyy-MM-dd}", dtVoucherDate.Value);
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                if (rb1.Checked == true)//with ref no
                
                    command.CommandText = "SELECT * FROM TestRefVoucherPay  WHERE (VoucherNo='" + tbVoucherNo.Text + "' OR voucherRefNo='" + tbRefNo.Text + "') AND (cancel!='Y')";
                    
                

                else if (rb2.Checked == true)//without ref no                  
                    command.CommandText = "SELECT * FROM TestNoRefVoucherPay  WHERE (VoucherNo='" + tbVoucherNo.Text + "') AND (cancel!='Y')";

                try
                {
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            recno = reader["vrecno"].ToString();
                            lbRecNo.Text = recno;
                            if (rb1.Checked == true)
                                tbRefNo.Text = reader["voucherRefNo"].ToString();

                            tbVoucherNo.Text = reader["VoucherNo"].ToString();
                            tbCabNo.Text = reader["cabNo"].ToString();
                            tbVoucherDate.Text = reader["VoucherDate"].ToString();
                            dtVoucherDate.Text = reader["VoucherDate"].ToString();
                            tbVoucherAmount.Text = reader["VoucherAmount"].ToString();
                            tbPaidAmount.Text = reader["BalAmount"].ToString();

                            if (rb1.Checked == true)
                                tbPaiDateTime.Text = reader["PayDateTime"].ToString();

                            if (rb2.Checked == true)
                                tbPaiDateTime.Text = reader["PayDate"].ToString();

                            tbPaidBy.Text = reader["user"].ToString();
                            tbPaidLocation.Text = reader["Location"].ToString();

                        }

                    }

                    if (!string.IsNullOrEmpty(recno))
                    {
                        if (rb1.Checked == true)//with ref no
                        {

                            if (true == finddedcut(recno,chbAll) || true == findrefund(recno,chbAll))
                                getAllVoucherNumberFromWithRef(recno, dgv1);
                        }

                        else if (rb2.Checked == true)//without ref no
                        {
                            if (true == finddedcut(recno,chbAll) || true == findrefund(recno,chbAll))
                                getAllVoucherNumberFromWithoutRef(recno, dgv1);
                        }
                    }

                            connection.Close();
                }
                catch (Exception ex) { connection.Close(); MessageBox.Show(ex.Message); }
            }
            else { MessageBox.Show("Please Select Voucher Category !!!"); }

        }
        public bool finddedcut(string recNo,CheckBox chbAll) 
        {
            string newRec = "";
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VrecNo FROM TestNewDeductInfo WHERE VrecNo='"+recNo+"' ";

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["VrecNo"] != DBNull.Value)
                            newRec = reader["VrecNo"].ToString();
                    }

                }

            }
            connection.Close();
            if (newRec == recNo)
            {
                chbAll.Checked = true;
                return true;
            }
            else
            {
                chbAll.Checked = false;
                return false;
            }

        }

        public bool findrefund(string recNo,CheckBox chbAll)
        {
            string newRec = "";
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT VrecNo FROM TestNewRefundInfo WHERE VrecNo='" + recNo + "' ";

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["VrecNo"] != DBNull.Value)
                            newRec = reader["VrecNo"].ToString();
                    }

                }

            }
            connection.Close();
            if (newRec == recNo)
            {
                chbAll.Checked = true;
                return true;
            }
            else
            {
                chbAll.Checked = false;
                return false;
            }

        }

        public void getAllVoucherNumberFromWithRef(string Vrec,DataGridView dgv) 
        {
            int i = 0;
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT voucherRefNo FROM TestRefVoucherPay WHERE vrecno='"+Vrec+"'";
            using (var reader = command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader["voucherRefNo"] != DBNull.Value)
                    {
                        dgv.Rows.Add();
                        dgv.Rows[i].Cells[0].Value = reader["voucherRefNo"].ToString();

                        i++;
                    }
                }
            }
            connection.Close();
        }


        public void cancelAllVoucherWithDeductORrefund() 
        {

        }

        public void getAllVoucherNumberFromWithoutRef(string Vrec, DataGridView dgv)
        {
            int i = 0;
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT VoucherNo FROM TestNoRefVoucherPay WHERE vrecno='" + Vrec + "'";
            using (var reader = command1.ExecuteReader())
            {
                while (reader.Read())
                {

                    if (reader["VoucherNo"] != DBNull.Value)
                    {
                        dgv.Rows.Add();
                        dgv.Rows[i].Cells[0].Value = reader["VoucherNo"].ToString();

                        i++;
                    }
                }
            }
            connection.Close();
        }

        //public void FindPaidVouchersForCancel (TextBox tbRefNo, DateTimePicker dtVoucherDate, TextBox tbVoucherNo, TextBox tbCabNo, TextBox tbVoucherDate, TextBox tbVoucherAmount, TextBox tbPaidAmount, TextBox tbPaiDateTime, TextBox tbPaidBy, TextBox tbPaidLocation,RadioButton rb1,RadioButton rb2)
        //{
        //    if (rb1.Checked == true || rb2.Checked == true)
        //    {
        //        if ((tbRefNo.Text == "" || tbRefNo.Text == null))
        //            tbRefNo.Text = "xxxxx";

        //        if (tbVoucherNo.Text == "" || tbVoucherNo == null)
        //            tbVoucherNo.Text = "XXXXXXXX";

        //        string vDate = String.Format("{0:yyyy-MM-dd}", dtVoucherDate.Value);
        //        MySqlConnection connection = new MySqlConnection(constr);
        //        connection.Open();
        //        MySqlCommand command = connection.CreateCommand();

        //        if (rb1.Checked == true)//with ref no
        //                command.CommandText = "SELECT * FROM TestRefVoucherPay  WHERE VoucherNo='" + tbVoucherNo.Text + "' OR voucherRefNo='" + tbRefNo.Text + "'";          

        //        else if (rb2.Checked == true)//without ref no                     
        //                command.CommandText = "SELECT * FROM TestNoRefVoucherPay  WHERE VoucherNo='" + tbVoucherNo.Text + "'";

        //        try
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    if (rb1.Checked == true)
        //                        tbRefNo.Text = reader["voucherRefNo"].ToString();

        //                    tbVoucherNo.Text = reader["VoucherNo"].ToString();
        //                    tbCabNo.Text = reader["cabNo"].ToString();
        //                    tbVoucherDate.Text = reader["VoucherDate"].ToString();
        //                    tbVoucherAmount.Text = reader["VoucherAmount"].ToString();
        //                    tbPaidAmount.Text = reader["BalAmount"].ToString();

        //                    if (rb1.Checked == true)
        //                        tbPaiDateTime.Text = reader["PayDateTime"].ToString();

        //                    if (rb2.Checked == true)
        //                        tbPaiDateTime.Text = reader["PayDate"].ToString();

        //                    tbPaidBy.Text = reader["user"].ToString();
        //                    tbPaidLocation.Text = reader["Location"].ToString();
        //                }

        //            }
        //            connection.Close();
        //        }
        //        catch (Exception ex) { connection.Close(); MessageBox.Show(ex.Message); }
        //    }
        //    else
        //        MessageBox.Show("Please Select The Voucher Categery");

        //}

        public void VoucherCancel(string refno, string voucherno, string remark, string cabno, DateTime VoucherDate, string voucherAmount, string paidAmount, string paidBy, RadioButton rb1, RadioButton rb2,CheckBox chbCancellAll,Label lbRecNo)
        {
            us = new User();
            string cuser = us.getCurrentUser();

            DialogResult dr = MessageBox.Show("Are You Sure Want to Cance This Voucher ?????", "Warning !!!!!", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes && string.Equals(cuser, paidBy, StringComparison.OrdinalIgnoreCase))
            {


                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                MySqlCommand command1 = connection.CreateCommand();

                if (rb1.Checked == true)//with ref no
                {
                    if (chbCancellAll.Checked == true)
                    {
                        command.CommandText = "UPDATE TestRefVoucherPay SET cancel='Y' WHERE vrecno='" + lbRecNo.Text + "'";
                    }
                    else
                    {
                        command.CommandText = "UPDATE TestRefVoucherPay SET cancel='Y' WHERE voucherRefNo='" + refno + "'";
                    }
                    
                    updateReferenceNumberForCancellFromLogsheet(refno, chbCancellAll, lbRecNo);
                   
                }
                else if (rb2.Checked == true)//without ref no
                {

                    if (chbCancellAll.Checked == true)
                    {
                        command.CommandText = "UPDATE TestNoRefVoucherPay SET cancel='Y' WHERE 	vrecno='" + lbRecNo.Text + "'";
                    }
                    else
                    {
                        command.CommandText = "UPDATE TestNoRefVoucherPay SET cancel='Y' WHERE VoucherNo='" + voucherno + "'";
                    }
                }
                command.ExecuteNonQuery();
                connection.Close();

                updateVaoucherCancellation(refno, voucherno, cabno, VoucherDate, voucherAmount, paidAmount, remark, rb1, rb2);

                if (chbCancellAll.Checked == true)
                {
                    cancelVoucherDeduct(lbRecNo.Text, cabno);
                    cancelVoucherRefund(lbRecNo.Text, cabno);
                    cancelVoucherDeductRefundReceipt(lbRecNo.Text, cabno);
                }

                MessageBox.Show("Cancelled !!!");
            }
            else
            {
                MessageBox.Show("You Dont Have a Permission to Cancell this Voucher. the Voucher is Entered By " + paidBy + ". Please Contact " + paidBy);
            }
        }

        public void cancelVoucherDeduct(string refno, string cabno)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE TestNewDeductInfo SET Flag='1' WHERE VrecNo='" + refno + "'";
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void cancelVoucherRefund(string refno, string cabno)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE TestNewRefundInfo SET Flag='1' WHERE VrecNo='" + refno + "'";
            command.ExecuteNonQuery();
            connection.Close();
        }
        
        public void cancelVoucherDeductRefundReceipt(string refno, string cabno)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE TestNewDeductReceipt SET Flag='1' WHERE RecNo='" + refno + "'";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void updateReferenceNumberForCancellFromLogsheet(string refno,CheckBox chbCancelAll,Label lbRecNo)
        {
            char char1 = Convert.ToChar(refno.Substring(0, 1));
            char char2 = Convert.ToChar(refno.Substring(1, 1));
            string str = refno.Substring(2, 4);

            string ascii1 = ((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
            string asciiRefNo = refno; //ascii1 + ascii2 + str;

            try
            {
               // MySqlConnection connection = new MySqlConnection(constr7);
               // MySqlConnection connection = new MySqlConnection(constr8);
                MySqlConnection connection = new MySqlConnection(constr9);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                if(chbCancelAll.Checked==true)
                    //command.CommandText = "UPDATE budget_bookings SET Paid='0' WHERE PaidRef ='" + lbRecNo.Text + "'";
                    command.CommandText = "UPDATE bookings SET Paid='0' WHERE refID ='" + lbRecNo.Text + "'";
                else
                    //command.CommandText = "UPDATE budget_bookings SET Paid='0' WHERE booking_id ='" + asciiRefNo + "'";
                    command.CommandText = "UPDATE bookings SET Paid='0' WHERE refID ='" + asciiRefNo + "'";

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception) { }
        }

        public void updateReferenceNumberForCancell(string refno)
        {
            char char1 = Convert.ToChar(refno.Substring(0, 1));
            char char2 = Convert.ToChar(refno.Substring(1, 1));
            string str = refno.Substring(2, 4);

            string ascii1 = ((int)(char1)).ToString(); string ascii2 = ((int)(char2)).ToString();
            string asciiRefNo = ascii1 + ascii2 + str;

            try
            {
                SqlConnection connection = new SqlConnection(constr5);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE Job SET Paid='0' WHERE JobID ='" + asciiRefNo + "'";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception) { }
        }

        public void updateVaoucherCancellation(String refno, string voucherNo, String cabNo, DateTime voucherDate, string voucherAmount, string paidAmount, string remark, RadioButton rb1, RadioButton rb2)
        {
            us = new User();
            string currentUser = us.getCurrentUser();
            string Voucherdate = String.Format("{0:yyyy-MM-dd}", voucherDate);
            string date = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(DateTime.Now));
            string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);
            string type = "";
            if (rb1.Checked == true)
                type = "With Ref";
            if (rb2.Checked == true)
                type = "Without Ref";
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO TestVoucherCancellation (refno,voucherNo,cabNo,voucherDate,voucherAmount,paidAmount,type,remark,date,dateAndTime,user) VALUES('" + refno + "','" + voucherNo + "','" + cabNo + "','" + Voucherdate + "','" + voucherAmount + "','" + paidAmount + "','" + type + "',  '" + remark + "','" + date + "','" + datetime + "','" + currentUser + "')";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void AddNewCab(TextBox tbCabNo, TextBox tbPlateNo, DateTimePicker dtDate)
        {
            string date = String.Format("{0:yyyy-MM-dd}", dtDate.Value);
            string cab = "K" + tbCabNo.Text;
            try
            {
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO TestCabList(CabNo,PlateNo,OpenDate,CloseDate,flag)VALUES('" + cab + "','" + tbPlateNo.Text + "','" + date + "','0001-01-01','0')";
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Saved!!!!");
            }
            catch (Exception) { MessageBox.Show("Error!!!!!"); }

        }

        public void FindCabList(TextBox tbCabNo, TextBox tbPlateNo)
        {
            string cab = "K" + tbCabNo.Text;
            try
            {
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT CabNo,PlateNo FROM TestCabList WHERE (CabNo='" + cab + "' OR PlateNo='" + tbPlateNo.Text + "') AND flag='0'";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tbCabNo.Text = reader["CabNo"].ToString();
                            tbPlateNo.Text = reader["PlateNo"].ToString();
                        }
                    }
                }
            }
            catch (Exception) { MessageBox.Show("Error!!!!!"); }

        }

        public void withdrawCab(TextBox tbCabNo, TextBox tbPlateNo, DateTimePicker dtDate)
        {
            string date = String.Format("{0:yyyy-MM-dd}", dtDate.Value);
            string cab = tbCabNo.Text;
            try
            {
                DialogResult dr = MessageBox.Show("Are you Sure Want to Withdraw this Cab", "Withdraw", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    MySqlConnection connection = new MySqlConnection(constr);
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "UPDATE TestCabList SET flag='1',CloseDate='" + date + "' WHERE CabNo='" + cab + "' OR PlateNo='" + tbPlateNo.Text + "'";
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Withdrew!!!!");
                }

            }
            catch (Exception) { MessageBox.Show("Error!!!!!"); }
        }

        public void ClearCablist(TextBox tbCabNo, TextBox tbPlateNo, DateTimePicker dtDate)
        {
            tbCabNo.Text = "";
            tbPlateNo.Text = "";
            dtDate.Value = DateTime.Now;

        }

        public void selectSIMServiceProvider(TextBox tbmobileNumer, TextBox tbserviceprovider)
        {
            string sp = (tbmobileNumer.Text).Substring(0, 3);
            // string number=((tbmobileNumer.Text).Substring(3, 9));
            if (sp == "071" || sp == "070")
                tbserviceprovider.Text = "Mobitel";
            if (sp == "072")
                tbserviceprovider.Text = "Etisalat";
        }

        public void setInfoFromVouchers(string cabno, TextBox tbamount, TextBox tbventura)
        {

            Form23 f23 = new Form23();
            f23.Show();
            f23.textCabNo.Text = cabno;
            f23.textAmount.Text = tbamount.Text;
            tbventura.Visible = false;


        }

        public void clearManualVoucher(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                if (c is DataGridView)
                {
                    ((DataGridView)c).DataSource = null;
                }
                if (c.HasChildren)
                {
                    clearManualVoucher(c);
                }
            }
        }

        public void viewsimDeposit(string nicdl, string recno, TextBox tbNicdl, TextBox tbRecno, DateTimePicker dtpicker, TextBox tbName, TextBox tbCabno, TextBox tbPlateNo, TextBox tbDeposit)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            if (nicdl != "")
                command.CommandText = "SELECT * FROM TestOtherPayment WHERE NICDL='" + nicdl + "'AND Refund='0'";
            if (recno != "")
                command.CommandText = "SELECT * FROM TestOtherPayment WHERE OtherRecNo='" + recno + "' AND Refund='0'";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tbNicdl.Text = reader["NICDL"].ToString();
                    dtpicker.Value = Convert.ToDateTime(reader["Date"].ToString());
                    tbRecno.Text = reader["OtherRecNo"].ToString();
                    tbName.Text = reader["Name"].ToString();
                    tbCabno.Text = reader["cabNo"].ToString();
                    tbPlateNo.Text = reader["PlateNo"].ToString();
                    tbDeposit.Text = reader["SimDepo"].ToString();
                }
                connection.Close();
            }
        }

        public void refundSimDeposit(string recno)
        {
            string cuser = "";
            DialogResult dr = MessageBox.Show("Are You Sure Want To Refund?", "Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                us = new User();
                cuser = us.getCurrentUser();
                string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE TestOtherPayment SET Refund='1',RefundBy='" + cuser + "',RefundDate='" + date + "' WHERE OtherRecNo='" + recno + "'";
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Refunded!!!!");
            }

        }

        public int identifyNightWorkingDays(string nightdate, DataGridView dgv)
        {
            dgv.Rows.Add(1);
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[0].Value == null)
                {
                    dgv.Rows[i].Cells[0].Value = nightdate;
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    return 0;
                }

            }
            return 0;
        }

        public void refreshNightWorikngDays(DataGridView dgv2, DataGridView dgv4)
        {
            string date = "";
            for (int i = 0; i < dgv2.RowCount; i++)
            {
                if (dgv2.Rows[i].Cells[3].Value.Equals(((char)253).ToString()))
                {
                    date = dgv2.Rows[i - 1].Cells[0].Value.ToString();
                    dgv4.Rows.Clear();
                    identifyNightWorkingDays(date, dgv4);
                }
            }

        }

        public void SaveBankDepositInfo(TextBox tbCano, TextBox tbrecno, DateTimePicker dtbDepodate, TextBox tbRefno, TextBox tbAmount, TextBox tbName, TextBox tbNic, Button btn1, Panel pnl1)
        {
            us = new User();
            string user = us.getCurrentUser();

            string depdate = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", dtbDepodate.Value);

            try
            {
                MySqlConnection conn = new MySqlConnection(constr);
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO TestBankDeposit(CabNo,recno,DepositDate,RefNo,Amount,Name,NIC,Date,DateAndTime,User) VALUES('" + tbCano.Text + "','" + tbrecno.Text + "','" + depdate + "','" + tbRefno.Text + "','" + tbAmount.Text + "','" + tbName.Text + "','" + tbNic.Text + "','" + String.Format("{0:yyyy-MM-dd}", DateTime.Now) + "','" + String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now) + "','" + user + "')";
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Saved!");
                btn1.Enabled = false;
                pnl1.Visible = false;
            }
            catch { Exception ex; }

        }

        public void clear_all(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                if (c is MaskedTextBox)
                    ((MaskedTextBox)c).Clear();
                if (c.HasChildren)
                {
                    clear_all(c);
                }
            }
        }
        //public void find_leasingLetterDeposit(string lldrecno,TextBox tblldAmount,Panel p1,TextBox tbsticker) 
        //{
        //    MySqlConnection connection = new MySqlConnection(constr4);
        //    connection.Open();
        //    MySqlCommand command = connection.CreateCommand();

        //    command.CommandText = "SELECT RecNo,amount FROM PaymentInfo WHERE RecNo='"+lldrecno+"'";
        //    using (var reader = command.ExecuteReader())
        //    {
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                tblldAmount.Text = reader["amount"].ToString();
        //            }
        //            //tbsticker.Text = "2500";
        //            p1.Visible = false;
        //        }
        //        else 
        //        {
        //            MessageBox.Show("You Have Entered a Invalid Receipt Number, Please Try Again !!!");
        //        }
        //    }
        //}

        public void calculateAbsenceCharges(string lastPaid, string lastFree, DateTimePicker dtStart, TextBox tbFine, Label lbFineReason, TextBox tbAmount, TextBox tbTotAmount, CheckBox chbIgnore, CheckBox chbNewTaxi,TextBox tbAppRental)
        {
            if (chbNewTaxi.Checked == false)
            {
                if (chbIgnore.Checked == false)
                {
                    DateTime lastPaidDate = Convert.ToDateTime(lastPaid);//DateTime.ParseExact(lastPaid, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime lastFreeDate = Convert.ToDateTime(lastFree);  //DateTime.ParseExact(lastFree, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    DateTime startDate = dtStart.Value;
                    DateTime lastDate = DateTime.Now;
                    if (lastFreeDate > lastPaidDate)
                        lastDate = lastFreeDate;
                    else if (lastFreeDate < lastPaidDate)
                        lastDate = lastPaidDate;

                    int dateDiff = (startDate.AddDays(-1) - lastDate).Days;

                    if (dateDiff > 5 && dateDiff <= 10)
                    {
                        tbFine.Text = ((dateDiff - 5) * perDayCharge).ToString();
                        lbFineReason.Text = "For " + (dateDiff - 5) + " Day/s";
                        MessageBox.Show("You Have To Pay Additional Rs. " + tbFine.Text + " For Fine Charges");
                        tbTotAmount.Text = (Convert.ToInt32(tbFine.Text) + Convert.ToInt32(tbAmount.Text) + Convert.ToInt32(tbAppRental.Text)).ToString();
                    }
                    else if (dateDiff > 10)
                    {
                        tbFine.Text = "4500";
                        lbFineReason.Text = "For Exceeding 10 Days";
                        MessageBox.Show("You Have To Pay Additional Rs. " + tbFine.Text + " For Fine Charges");
                        tbTotAmount.Text = (Convert.ToInt32(tbFine.Text) + Convert.ToInt32(tbAmount.Text) + Convert.ToInt32(tbAppRental.Text)).ToString();
                    }
                    else
                        tbFine.Text = "0";
                }

            }
            else 
            {
                tbFine.Text = "0";
            }
        }

        public bool findBlockCabByCallCenter(TextBox tbCabNo)
        {
            string cabno = tbCabNo.Text; string date = ""; string boperater = "";

            MySqlConnection connection = new MySqlConnection(constr2);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            //command.CommandText = "SELECT CabNo,Date,Boperator FROM BlockBudget WHERE CabNo='" + cabno + "'";
            command.CommandText = "SELECT CabNo,Fbdate,FblockName FROM CarOwner WHERE (CabNo='" + cabno + "' AND Fblock='Y') AND wflag='AC'";

            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                date = reader["Fbdate"].ToString();

                reader.Read();
                boperater = reader["FblockName"].ToString();

                connection.Close();
                MessageBox.Show("This Cab Is Bloked by " + boperater + ", Blocked Date is " + date + ". Please Contact " + boperater);
                return true;

            }
            connection.Close();
            return false;
        }


        public void findNoAppPhone(TextBox tbCabNo,CheckBox chbAppFine,Panel p6) 
        {
            string imei = "";
            MySqlConnection connection = new MySqlConnection(constr2);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT imei_number FROM CarOwner WHERE CabNo='" + tbCabNo.Text + "' AND 	wflag ='AC'";
              var reader = command.ExecuteReader();
              if (reader.HasRows)
              {
                  reader.Read();
                  imei = reader["imei_number"].ToString();
              }
              connection.Close();

              if (imei.Length > 10)//  phone
                  chbAppFine.Checked = false;
              else // no phone
              {
                  chbAppFine.Checked = true;
                  p6.Visible = true;
              }
        }

        public bool findMobilePhoneLoanCab(TextBox tbCabNo,TextBox tbMobilePhoneAreas,CheckBox chbNobileLoan,Label lbLoanNumber,Label lbopenAreas)
        {
            string cabno = tbCabNo.Text;

            int loan = 0; int downPayment = 0; double areas = 0.00;

            MySqlConnection connection = new MySqlConnection(constr2);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT CabNo,Loan_Amount,Down_Payment,Areas,Loan_Number FROM mobile_loan WHERE (CabNo='" + cabno + "' AND  Settle_Flag='N')";


            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                loan = Convert.ToInt32(reader["Loan_Amount"].ToString());

                reader.Read();
                downPayment = Convert.ToInt32(reader["Down_Payment"].ToString());

                reader.Read();
                areas= Convert.ToDouble( reader["Areas"]);

                //reader.Read();
                //tbMobilePhoneAreas.Text = reader["Areas"].ToString();

                //reader.Read();
                //lbopenAreas.Text = reader["Areas"].ToString();

                reader.Read();
                lbLoanNumber.Text = reader["Loan_Number"].ToString();

                chbNobileLoan.Checked = true;

                connection.Close();
                
                findPhoneLoanAras( cabno,  areas,  loan, downPayment,  tbMobilePhoneAreas,lbopenAreas) ; 
       
                //MessageBox.Show("ජංගම දුරකථනය සඳහා ණය වාරික ගෙවීමට සිදුවේ !!");
                return true;

            }
            connection.Close();

            return false;

        }


        public void findPhoneLoanAras(string cabno, double areas, int loan, int downpayment, TextBox tbMobilePhoneAreas, Label lbopenAreas)         
        {
            int paid = 0; double areasFinal = 0.00;
            MySqlConnection connection = new MySqlConnection(constr2);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT sum(pay_amount)as Paid FROM mobile_pay WHERE (CabNo='" + cabno + "')";
            

            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                //if(reader.IsDBNull("Paid"))
                //if (! reader.IsDBNull)
                reader.Read();
                paid = Convert.ToInt32(reader["Paid"].ToString());

                areasFinal = loan - downpayment - paid;

                if (areasFinal > 0.00)
                {
                    tbMobilePhoneAreas.Text = areasFinal.ToString();
                    lbopenAreas.Text = areasFinal.ToString();
                    MessageBox.Show("ජංගම දුරකථනය සඳහා ණය වාරික ගෙවීමට සිදුවේ !!");
                }

                connection.Close();
            }
            else 
            {
                areasFinal = areas;
                if (areasFinal > 0.00)
                {
                    tbMobilePhoneAreas.Text = areasFinal.ToString();
                    lbopenAreas.Text = areasFinal.ToString();
                    MessageBox.Show("ජංගම දුරකථනය සඳහා ණය වාරික ගෙවීමට සිදුවේ !!");
                }
            }

            connection.Close();
        }

        public void calNewMobilePhoneAreas(TextBox tbAreas,TextBox tbMobileLoanAmount,Label lbopenAreas)
        {
            tbAreas.Text = (Convert.ToDouble(lbopenAreas.Text) - Convert.ToDouble(tbMobileLoanAmount.Text)).ToString();
        }

        public void fillLocationComboBox(ComboBox cmbLocation)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            MySqlConnection connection1 = new MySqlConnection(constr);
            connection1.Open();
            MySqlCommand command = connection1.CreateCommand();
            // command.CommandText = "select ReciptNo,ReciptDate from ReciptHeader where CabNo='" + taxi + "' order by ReciptDate DESC";
            command.CommandText = "SELECT `Name` FROM `TestLocationPara`";
            MySqlDataAdapter newadp = new MySqlDataAdapter(command);
            newadp.Fill(ds);
            dt = ds.Tables[0];
            cmbLocation.DataSource = ds.Tables[0];
            cmbLocation.ValueMember = "Name";
            cmbLocation.DisplayMember = "Name";
            connection1.Close();
        }


        public void getPendingPhonebills(DataGridView dgv10, TextBox tbcabNo, TextBox tbTotPhoneBill)
        {
            if (dgv10.Rows.Count >= 1)
                dgv10.Rows.Clear();

            string cab = "k" + tbcabNo.Text;
            int i = 0;
            int tot = 0;
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Year,Month,Pending FROM TestPhoneBillDetail WHERE (CabNo='" + cab + "' AND branded ='Y') AND (Amount=0)  AND (Pending>0) AND (TestPhoneBillDetail.Delete !='Y')";
            using (var reader = command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    tot = tot + Convert.ToInt32(reader["Pending"].ToString());
                    dgv10.Rows.Add();
                    dgv10.Rows[i].Cells[0].Value = reader["CabNo"].ToString();
                    dgv10.Rows[i].Cells[1].Value = reader["RecNo"].ToString();
                    dgv10.Rows[i].Cells[2].Value = reader["Year"].ToString();
                    dgv10.Rows[i].Cells[3].Value = reader["Month"].ToString();
                    dgv10.Rows[i].Cells[4].Value = reader["Pending"].ToString();
                    i++;
                }
            }
            connection.Close();
            tbTotPhoneBill.Text = tot.ToString();
        }


        public void updatePhoneBillFromVoucher(DataGridView dgv10,string VrecNo)
        {
            string cabno; string recno; int amount = 0;
            string vdate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            us = new User();
            string user = us.getCurrentUser();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            for (int i = 0; i <= dgv10.RowCount - 1; i++)
            {
                if (dgv10.Rows[i].Cells[0].Value != null)
                {
                    cabno = dgv10.Rows[i].Cells[0].Value.ToString();
                    recno = dgv10.Rows[i].Cells[1].Value.ToString();
                    amount = Convert.ToInt32(dgv10.Rows[i].Cells[4].Value.ToString());

                    command.CommandText = "UPDATE TestPhoneBillDetail SET Amount='" + amount + "',Pending='0', VpayDateTime='" + String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now) + "',VpayDate='" + vdate + "',RefNo='" + VrecNo + "',Vuser='" + user + "'  WHERE (RecNo='" + recno + "'  AND CabNo='" + cabno + "') AND (branded='Y') AND (TestPhoneBillDetail.Delete !='Y') ";
                    command.ExecuteNonQuery();

                }
            }
        }


        public void updateMobilePhoneLoan(TextBox tbCabNo, Label lbLoanNo, TextBox tbAreas, TextBox tbMobilePhoneLoan) 
        {

            if (Convert.ToDouble(tbMobilePhoneLoan.Text) > 0.00)
            {
                string date = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                string time = String.Format("{0: HH:mm:ss tt}", DateTime.Now);
                string dateTime = String.Format("{0:yyyy-MM-dd HH:mm:ss tt}", DateTime.Now);

                us = new User();
                string user = us.getCurrentUser();

                string flag = "N";
                if (Convert.ToDouble(tbAreas.Text) == 0.00 || Convert.ToDouble(tbAreas.Text) == 0)
                    flag = "Y";

                MySqlConnection connection = new MySqlConnection(constr2);
                connection.Open();
                MySqlCommand command1 = connection.CreateCommand();
                MySqlCommand command2 = connection.CreateCommand();

                command1.CommandText = "UPDATE mobile_loan SET Areas='" + tbAreas.Text + "',Settle_Flag='" + flag + "' WHERE (CabNo='" + tbCabNo.Text + "' AND  Loan_Number='" + lbLoanNo.Text + "') ";
                command1.ExecuteNonQuery();

                command2.CommandText = "Insert INTO mobile_pay (CabNo,pay_amount,Date,Time,LoanNo,Operator,enter_flag,dateTime) VALUES('" + tbCabNo.Text + "','" + tbMobilePhoneLoan.Text + "','" + date + "','" + time + "','" + lbLoanNo.Text + "','" + user + "','P','" + dateTime + "')";
                command2.ExecuteNonQuery();

                connection.Close();
            }
        }

        public bool checkCabNoForExtraCom(string cabNo) 
        {
             string cab=cabNo.Substring(1);

             MySqlConnection connection = new MySqlConnection(constr);
                 connection.Open();
                 MySqlCommand command = connection.CreateCommand();
                 //command.CommandText = "SELECT CabNo,Date,Boperator FROM BlockBudget WHERE CabNo='" + cabno + "'";
                 command.CommandText = "SELECT cabNo FROM TestExtraComCab WHERE (cabNo='" + cab + "' AND flag='N')";

                 var reader = command.ExecuteReader();
                 if (reader.HasRows)
                 {
                     

                     return true;
                 }
                 return false;
        }
        public void saveAdditionComCab (string cab)
        { 
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
           
            
            
                try
                {
                    command.CommandText = "INSERT INTO TestExtraComCab (cabNo,flag) VALUES ('"+cab+"','N')";
                    
                    command.ExecuteNonQuery();
               
                    connection.Close();
                    MessageBox.Show("Saved");
                }
                catch (MySqlException myex) { MessageBox.Show( ""+myex.Message); }
        }

        public void RemoveAdditionComCab(string cab)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();



            try
            {
                command.CommandText = "Update TestExtraComCab Set flag='Y' Where cabNo='" + cab + "'";

                command.ExecuteNonQuery();

                connection.Close();
                MessageBox.Show("Removed");
            }
            catch (MySqlException myex) { MessageBox.Show("" + myex.Message); }
        }
    }

}



