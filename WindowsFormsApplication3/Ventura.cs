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

namespace WindowsFormsApplication3
{
    class Ventura
    {
        ReportsPrint rprnt;
        NewReceiptNumber nrecn;
        User us;
        private string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        private int DailyPayment = 1750; private int MonthlyPayment = 52500;

        public string get_Location()
        {
            return ConfigurationManager.AppSettings["Location"];
        }

        public void getVenturaCabDetails(string cabno,TextBox tbplateno, TextBox tbownername)
        {
            string CabNo = "";
            if (cabno.Length == 3)
                CabNo = cabno;
            if (cabno.Length == 4)
                CabNo = cabno.Substring(1, 3);

            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM TestVenturaCab WHERE CabNo='" + CabNo + "'";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tbplateno.Text = reader["PlateNo"].ToString();
                        tbownername.Text = reader["Name"].ToString();
                    }
                }
                conn.Close();
        }
        
        public string getLastVenturaPaidDate(string cabno) 
        {
            string CabNo = "";
            if (cabno.Length == 3)
                CabNo = cabno;
            if (cabno.Length == 4)
                CabNo = cabno.Substring(1, 3);
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT date FROM TestVenturaPayment WHERE CabNo='" + CabNo + "' ORDER BY date DESC LIMIT 1";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return reader["Date"].ToString();                  
                }
            }
            return "";
        }
        
        //public void fillGridForPayment(DataGridView dgv1,int days,string cabno) 
        //{
        //     DateTime lastPaidDate; string date = "";

        //     if (days > 0)
        //     {
        //         dgv1.Rows.Add(days);
        //         date = getLastVenturaPaidDate(cabno);
        //         if (date != "")
        //         {
        //             lastPaidDate = Convert.ToDateTime(date).AddDays(1);// add(1)= if paid upto 10th should start from 11th
        //         }
        //         else
        //         {
        //             lastPaidDate = DateTime.Now;
        //         }
        //         for (int i = 0; i < days; i++)
        //         {
        //             dgv1.Rows[i].Cells[0].Value = String.Format("{0:yyyy-MM-dd}", lastPaidDate.AddDays(i));
        //             dgv1.Rows[i].Cells[1].Value = DailyPayment.ToString();
        //         }
        //     }
        //}

        //public int calTotalDays(TextBox tbAmount ,TextBox tbTotalAmount,TextBox tbBalance,TextBox tbDays,TextBox tbCabno,DataGridView dgv1) 
        //{
        //    int amount = 0; int totalAmount = 0; int balance = 0; int tempdays = 0; int days = 0; DateTime lastDate=DateTime.MinValue;
        //    amount = Convert.ToInt32(tbAmount.Text);
        //    try
        //    {
        //         lastDate = Convert.ToDateTime(getLastVenturaPaidDate(tbCabno.Text)).AddDays(1);
        //    }
        //    catch (FormatException) { }
        //    DateTime today = DateTime.Today;
        //    DateTime endOfMonthDate = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        //    int i = DateTime.Compare(lastDate, endOfMonthDate);  // lstdate < enddate -(<0) , lstdate = enddate -(=0),lstdate > enddate -(>0) 

        //    if (amount >= DailyPayment)
        //    {
        //        balance = amount % DailyPayment;
        //        tempdays = (amount - balance) / DailyPayment;
        //        int r = DateTime.Compare(lastDate.AddDays(tempdays), endOfMonthDate);

        //        if (r <= 0)
        //        {
        //            days = tempdays;
        //            totalAmount = (amount - balance);
        //            tbBalance.Text = balance.ToString();
        //            tbTotalAmount.Text = totalAmount.ToString();
        //            tbDays.Text = days.ToString();
        //        }
        //        else if (r > 0) 
        //        {
        //            TimeSpan diff = lastDate.AddDays(tempdays - 1) - endOfMonthDate;// in here last date mean (lastdate+1) so (tempdays-1)
        //            int diffDays = diff.Days;
        //            days = tempdays- diffDays;
        //            //balance = amount - (balance + (tempdays * DailyPayment));
        //            balance = amount - ((DailyPayment) * days);
        //            totalAmount = amount - balance;
        //            tbBalance.Text = balance.ToString();
        //            tbTotalAmount.Text = totalAmount.ToString(); 
        //            tbDays.Text = days.ToString();
        //        }              

        //        fillGridForPayment(dgv1,days,tbCabno.Text);
        //        return days;
        //    }
        //    else 
        //    {
        //        fillGridForPayment(dgv1, days, tbCabno.Text);
        //        return days;
        //    }
        //}
        
        public void datagridFillForPayment(string cabno, DataGridView dgv1,  TextBox tbAmount,TextBox tbTotAmount,TextBox tbBalance,TextBox tbNumofDays) 
        {
            double amount = 0.00; double balance = 0.00; int days = 0; int i = 0;
            DateTime lastPaidDate; string date = "";

            date = getLastVenturaPaidDate(cabno);
            if (date != "")
            {
                lastPaidDate = Convert.ToDateTime(date).AddDays(1);// add(1)= if paid upto 10th should start from 11th
            }
            else
            {
                lastPaidDate = DateTime.Now;
            }

            try{amount = Convert.ToDouble(tbAmount.Text);}catch(Exception){}

            if (amount >= DailyPayment)
            {
                balance = amount % DailyPayment;
                days = Convert.ToInt32((amount - balance) / DailyPayment);

               

                if (days > 0)
                {
                    dgv1.Rows.Add(days + 1);//balance =1 day so days+1
                   
                    for (i = 0; i < days; i++)
                    {
                        dgv1.Rows[i].Cells[0].Value = String.Format("{0:yyyy-MM-dd}", lastPaidDate.AddDays(i));
                        dgv1.Rows[i].Cells[1].Value = DailyPayment.ToString();
                    }
                    if (balance > 0)
                    {
                        dgv1.Rows[i].Cells[0].Value = String.Format("{0:yyyy-MM-dd}", lastPaidDate.AddDays(i));
                        dgv1.Rows[i].Cells[1].Value = balance.ToString();
                    }
                }
                tbTotAmount.Text = tbAmount.Text;
                tbBalance.Text = "0";
                if (balance > 0)
                    tbNumofDays.Text = (days + 1).ToString();
                else
                    tbNumofDays.Text = (days).ToString();
            }
            else if (amount<DailyPayment)
            {
                days = 1;
                dgv1.Rows.Add(days + 1);
                dgv1.Rows[i].Cells[0].Value = String.Format("{0:yyyy-MM-dd}", lastPaidDate.AddDays(i));
                dgv1.Rows[i].Cells[1].Value = amount.ToString();
                tbTotAmount.Text = tbAmount.Text;
                tbBalance.Text = "0";
                tbNumofDays.Text = (days).ToString();
            }
        }

        public void CheckVenturaCab(string cabno,TextBox tbVentura) 
        {
            //string CabNo="";
            //if (cabno.Length == 3)
            //    CabNo = cabno;
            //if (cabno.Length == 4)
            //    CabNo = cabno.Substring(1,3);
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM TestVenturaCab WHERE CabNo='" + cabno + "'";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) 
                {
                    tbVentura.Visible = true;
                }
              
            }
            conn.Close();
        }

      
       

        public void saveVenturaPayment(TextBox tbCabno,TextBox tbPlateNo,TextBox tbOwner,TextBox tbTotalAmount,TextBox tbDays, DataGridView dgv1,TextBox tbrecno) 
        {
            string location = "";
            location = get_Location();
            nrecn = new NewReceiptNumber();
            us = new User();
            string cuser = us.getCurrentUser();
            string recno = ""; string date = ""; string payDate=""; string payDateTime = "";
            int year = 0; int month = 0;

            recno =  nrecn.generateVenturaRecNo();

            payDate = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            payDateTime=  String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            for (int i = 0; i < (dgv1.Rows.Count-1); i++)
            {
                if (dgv1.Rows[i].Cells[0].Value != null)
                {
                    date=String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dgv1.Rows[i].Cells[0].Value.ToString()));
                    command.CommandText = "INSERT INTO TestVenturaPayment (cabNo,PlateNo,RecNo,Date,Amount,PayDate,Month,Year,PayDatetime,User,Flag,location ) VALUES ('" + tbCabno.Text + "','" + tbPlateNo.Text + "','" + recno + "','" + date + "','" + dgv1.Rows[i].Cells[1].Value.ToString() + "','" + payDate + "','" + month + "','" + year + "','" + payDateTime + "','" + cuser + "','0','"+location+"')";
                   command.ExecuteNonQuery();
                }
            }

            saveVenturaRecHeader(recno, tbCabno, tbPlateNo, tbOwner, tbTotalAmount, tbDays, dgv1, payDate, payDateTime, cuser, tbrecno);
            connection.Close();
        }

        public void saveVenturaRecHeader(string recno,TextBox tbCabno, TextBox tbPlateNo, TextBox tbOwner, TextBox tbTotalAmount, TextBox tbDays, DataGridView dgv1,string paydate,string paydatetime, string user,TextBox tbrecno)
        {
            nrecn = new NewReceiptNumber();

            rprnt = new ReportsPrint();
            string dateFrom = ""; string DateTo = "";     
          
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            dateFrom = String.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(dgv1.Rows[0].Cells[0].Value));
            DateTo = String.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(dgv1.Rows[dgv1.Rows.Count - 2].Cells[0].Value));
            command.CommandText = "INSERT INTO TestVenRecHeader (cabNo,PlateNo,Owner,RecNo,Amount,DateFrom,DateTo,NumOfDays,PayDate,PayDateTime,User,Flag) VALUES ('" + tbCabno.Text + "','" + tbPlateNo.Text + "','" + tbOwner.Text + "','" + recno + "','" + tbTotalAmount.Text + "','" + dateFrom + "','" + DateTo + "','" + tbDays.Text + "','" + paydate + "','" + paydatetime + "','" + user + "','0')";
            command.ExecuteNonQuery(); 
            connection.Close();
            areasUpdater(tbCabno.Text,Convert.ToDouble(tbTotalAmount.Text));
            nrecn.updateVenturaRecNo(recno);
            tbrecno.Text = recno;
            rprnt.printVenturaReceipt(tbCabno, recno);
            MessageBox.Show("Saved");
           
        }
       
        public void areasUpdater(string cabno,double PaidAmount) 
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            double areas = 0.0;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CabNo,Amount FROM TestVenturaAreas WHERE Month='" + month + "' AND Year='" + year + "'";

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    areas = Convert.ToDouble(reader["Amount"].ToString()) - PaidAmount;

                    conn.Close();
                    conn.Open();
                    cmd.CommandText = "UPDATE TestVenturaAreas SET Amount='" + areas + "' WHERE (Month='" + month + "' AND Year='" + year + "') AND  CabNo='" + cabno + "' ";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    areas = MonthlyPayment - PaidAmount;

                    conn.Close();
                    conn.Open();
                    cmd.CommandText = "INSERT INTO TestVenturaAreas(CabNo,Month,Year,Amount) VALUES ('" + cabno + "','" + month + "','" + year + "','" + areas + "') ";
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }

        public void viewAreas(string cabno,DataGridView dgv,TextBox tbtotAreas)
        {
            try
            {
                string CabNo = "";
                if (cabno.Length == 3)
                    CabNo = "K" + cabno;
                if (cabno.Length == 4)
                    CabNo = cabno.Substring(1, 3);
                if (dgv.Rows.Count > 0)
                    dgv.Rows.Clear();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                int totareas = 0;
                MySqlConnection connection1 = new MySqlConnection(constr);
                connection1.Open();
                MySqlCommand command = connection1.CreateCommand();
                command.CommandText = "SELECT Month,Amount FROM TestVenturaAreas WHERE CabNo='" + CabNo + "' ORDER BY Month ASC";
                MySqlDataAdapter newadp = new MySqlDataAdapter(command);
                newadp.Fill(ds);
                dt = ds.Tables[0];
                connection1.Close();

                if (ds.Tables[0] != null)
                {
                    dgv.DataSource = dt;
                }

                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    if (dgv.Rows[i].Cells[0].Value != "")
                    {
                        totareas = totareas + Convert.ToInt32(dgv.Rows[i].Cells[1].Value.ToString());
                    }
                }
                tbtotAreas.Text = totareas.ToString();
            }
            catch (Exception) { }
        }

        public void clearAll(Control control) 
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                if (c is DataGridView)
                {
                    ((DataGridView)c).DataSource=null;
                }
                if (c.HasChildren)
                {
                    clearAll(c);
                }
            }
        }


    }
}
