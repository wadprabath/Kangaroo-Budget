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
//using CrystalDecisions.CrystalReports.Engine.TextObject;
//using CrystalDecisions.CrystalReports.Engine.Sections;
using CrystalDecisions.CrystalReports.Engine;
namespace WindowsFormsApplication3
{
    class ReportsPrint
    {
        private string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        private string constr2 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.Calling_numberConnectionString1"].ConnectionString;
        private string constr3 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.AmilaConnectionString"].ConnectionString;
        private string constr7 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.budgetConnectionString"].ConnectionString;                                        
        User us;
        Taxi t1;
        string printLocation = "H";

        public string get_Location()
        {
            return ConfigurationManager.AppSettings["Location"];
        }
        public string get_LocationName()
        {
            return ConfigurationManager.AppSettings["LocName"];
        }

        public void PayementForGivenDate(DateTime givenDate)
        {
            string date = String.Format("{0:yyyy-MM-dd}", givenDate);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT TestPayment.Date ,TestPayment.RecNo, TestPayment.Amount, TestPayment.CabNo,TestPayment.Cancel,TestPayment.Delete from  TestPayment WHERE (TestPayment.Date='" + date + "')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenDatePrint");
            connection.Close();

            CrystalReport3 rpt = new CrystalReport3();
            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void MobitelBillForGivenMonth(string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (branded !='Y')  ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthMobitelBill");
            connection.Close();

            CrystalReport4 rpt = new CrystalReport4();
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            txtLocation.Text = "All";
            
            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }


        public void AppRentlForGivenMonth(string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,MontName,Year,Amount,Date FROM TestAppRental WHERE (MontName='" + month + "' and Year='" + Year + "') AND (Flag !='Y') ORDER BY Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthAppRental");
            connection.Close();

            CryAppRentalForMonth rpt = new CryAppRentalForMonth();
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            txtLocation.Text = "All";

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void BrandedCarMobitelBillForGivenMonth(string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate,VpayDate,RefNo FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (branded ='Y' AND Amount>0)  ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "BrandedPhoneBill");
            connection.Close();

            CryPhoneBilBranded rpt = new CryPhoneBilBranded();
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            txtLocation.Text = "All";

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }


        public void MobitelBillLocationWise(DateTime from, DateTime to, string location, string fullocation, string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            string fromDate = String.Format("{0:yyyy-MM-dd}",from);
            string toDate = String.Format("{0:yyyy-MM-dd}",to);

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (PayDate BETWEEN '" + fromDate + "' AND '" + toDate + "')  AND (TestPhoneBillDetail.Delete !='Y') AND (TestPhoneBillDetail.location='" + location + "') AND (branded !='Y') ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthMobitelBill");
            connection.Close();

            CrystalReport4 rpt = new CrystalReport4();
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];

            txtLocation.Text = fullocation;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }


        public void BrandedMobitelBillLocationWise(DateTime from, DateTime to, string location, string fullocation, string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (PayDate BETWEEN '" + fromDate + "' AND '" + toDate + "')  AND (TestPhoneBillDetail.Delete !='Y') AND (TestPhoneBillDetail.location='" + location + "') AND (branded ='Y' AND Amount >0) ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthMobitelBill");
            connection.Close();

            CryPhoneBilBranded rpt = new CryPhoneBilBranded();
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];

            txtLocation.Text = fullocation;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }



        public void MobitelBillLocationWiseMonthWise(DateTime from, DateTime to, string location, string fullocation, string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (TestPhoneBillDetail.location='" + location + "') AND (branded !='Y') ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthMobitelBill");
            connection.Close();

            CrystalReport4 rpt = new CrystalReport4();
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            txtLocation.Text = fullocation;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }
        public void BrandedMobitelBillLocationWiseMonthWise(DateTime from, DateTime to, string location, string fullocation, string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (TestPhoneBillDetail.location='" + location + "') AND (branded ='Y' AND Amount >0) ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthMobitelBill");
            connection.Close();

            CryPhoneBilBranded rpt = new CryPhoneBilBranded();
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            txtLocation.Text = fullocation;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void MobitelBillForGivenMonthYard(string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (RecNo LIKE 'RE%') AND (branded !='Y') ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthMobitelBill");
            connection.Close();

            CrystalReport4 rpt = new CrystalReport4();

            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            txtLocation.Text ="Yard";
            
            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void MobitelBillForGivenMonthHeadOffice(string month, string Year)
        {
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (RecNo LIKE 'BA%') AND (branded !='Y') ORDER BY RecNo";
            //command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (RecNo LIKE 'BA%') ORDER BY RecNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "GivenMonthMobitelBill");
            connection.Close();

            CrystalReport4 rpt = new CrystalReport4();

            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            txtLocation.Text = "Head Office";

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        //public void MobitelBillForGivenMonthYard(string month, string Year)
        //{
        //    Form3 f3 = new Form3();
        //    f3.Show();

        //    DataSet1 recds = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    command1.CommandText = "SELECT CabNo,RecNo,Month,Year,Amount,PayDate FROM TestPhoneBillDetail WHERE (Month='" + month + "' and Year='" + Year + "') AND (TestPhoneBillDetail.Delete !='Y') AND (RecNo LIKE 'RE%')";
        //    MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
        //    newadp1.Fill(recds, "GivenMonthMobitelBill");
        //    connection.Close();

        //    CrystalReport4 rpt = new CrystalReport4();

        //    TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
        //    txtLocation.Text = "Yard";

        //    rpt.SetDataSource(recds);
        //    f3.crystalReportViewer1.ReportSource = rpt;

        //}

        public void BrandedMonthlyIncomeTaxi(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
  
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            command1.CommandText = "SELECT  TestPayment.CabNo ,count(Date)as noOfDays,sum(TestPayment.Amount)as Total from TestPayment Where (Date between '" + fromDate + "' and  '" + toDate + "') AND (Cancel='B') AND (TestPayment.Delete !='Y') group by TestPayment.CabNo";
            
           
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
           
            newadp1.Fill(recds, "BrandedMonthlyIncome");
       
            connection.Close();          
           //CrystalReport5 rpt = new CrystalReport5();
            CryBrandedMonthlyIncome rpt = new CryBrandedMonthlyIncome();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

           txtFrom.Text = fromDate;
           txtTo.Text = toDate;

           rpt.SetDataSource(recds);
            
           f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void BrandedPaymentUserWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();
            us=new User();
            string currentUser=us.getCurrentUser();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            command1.CommandText = "SELECT RecNo,ReciptDate,ReciptAmount,CabNo,DateFrom,DateTo,TotBillRecv,Flag,UserID From TestReciptHeader Where (ReciptDate between '" + fromDate + "' and  '" + toDate + "') And (Flag='B') AND (UserID='" + currentUser + "') AND (TestReciptHeader.Delete !='Y')";


            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);

            newadp1.Fill(recds, "BrandedPayment");

            connection.Close();
            //CrystalReport5 rpt = new CrystalReport5();
            CryBrandedIncomeRecUser rpt = new CryBrandedIncomeRecUser();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtWise = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtUser.Text=currentUser;
            txtWise.Text = "User Wise";

            rpt.SetDataSource(recds);

            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void BrandedPaymentAllUsers(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();
           

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            command1.CommandText = "SELECT RecNo,ReciptDate,ReciptAmount,CabNo,DateFrom,DateTo,TotBillRecv,Flag,UserID From TestReciptHeader Where (ReciptDate between '" + fromDate + "' and  '" + toDate + "') And (Flag='B') AND (TestReciptHeader.Delete !='Y') ";


            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);

            newadp1.Fill(recds, "BrandedPayment");

            connection.Close();
            //CrystalReport5 rpt = new CrystalReport5();
            CryBrandedIncomeRecUser rpt = new CryBrandedIncomeRecUser();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtWise = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtUser.Text = "All Users";
            txtWise.Text = "";

            rpt.SetDataSource(recds);

            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void NightCars(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();


            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,RecNo,Date From TestPayment  Where (Date between '" + fromDate + "' and  '" + toDate + "') And (NightFlag='Y') AND (TestPayment.Delete !='Y')Order By Date";


            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);

            newadp1.Fill(recds, "NightCars");

            connection.Close();
            //CrystalReport5 rpt = new CrystalReport5();
            CryNightCars rpt = new CryNightCars();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            

            rpt.SetDataSource(recds);

            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void NormalMonthlyIncomeTaxi(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            command1.CommandText = "SELECT  TestPayment.CabNo ,count(Date)as noOfDays,sum(TestPayment.Amount)as Total,TestPayment.Delete from TestPayment Where (Date between '" + fromDate + "' and  '" + toDate + "') AND (Cancel !='B') AND (TestPayment.Delete!='Y')AND (SpFlag !='Y') group by TestPayment.CabNo";


            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);

            newadp1.Fill(recds, "NormalMonthlyIncome");

            connection.Close();
            //CrystalReport5 rpt = new CrystalReport5();
            CryNormalMonthlyIncome rpt = new CryNormalMonthlyIncome();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);

            f3.crystalReportViewer1.ReportSource = rpt;

        }


        public void MagnetlMonthlyIncomeTaxi(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            command1.CommandText = "SELECT  TestPayment.CabNo ,count(Date)as noOfDays,sum(TestPayment.Amount)as Total,TestPayment.Delete from TestPayment Where (Date between '" + fromDate + "' and  '" + toDate + "') AND (Cancel !='B') AND (TestPayment.Delete!='Y') AND (SpFlag='Y') group by TestPayment.CabNo";


            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);

            newadp1.Fill(recds, "NormalMonthlyIncome");

            connection.Close();
            //CrystalReport5 rpt = new CrystalReport5();
            CryMagnetMonthlyIncome rpt = new CryMagnetMonthlyIncome();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);

            f3.crystalReportViewer1.ReportSource = rpt;

        }


        public void NormalPerDayTaxiIncome(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT  TestPayment.Date , count(TestPayment.CabNo)as noOfCabs , sum(TestPayment.Amount)as Total from TestPayment Where (TestPayment.Date between  '" + fromDate + "' and  '" + toDate + "') and (TestPayment.Amount > 0) AND (Cancel!='B') AND (TestPayment.Delete !='Y') AND (SpFlag != 'Y')  group by TestPayment.Date";
            //command1.CommandText = "SELECT  TestPayment.Date , count(TestPayment.CabNo)as noOfCabs , sum(TestPayment.Amount)as Total from TestPayment Where (TestPayment.Date between  '" + fromDate + "' and  '" + toDate + "')   group by TestPayment.Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "PerDayIncome");
            connection.Close();

            CrystalReport6 rpt = new CrystalReport6();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];
            TextObject txtCarType = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtAmount = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtCarType.Text = "Without Branded and Magnet Cars";
            txtAmount.Text = "LKR 300.00 Per Day";

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }


        public void MagnetPerDayTaxiIncome(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT  TestPayment.Date , count(TestPayment.CabNo)as noOfCabs , sum(TestPayment.Amount)as Total from TestPayment Where (TestPayment.Date between  '" + fromDate + "' and  '" + toDate + "') and (TestPayment.Amount > 0) AND (Cancel!='B') AND (TestPayment.Delete !='Y') AND (SpFlag = 'Y')  group by TestPayment.Date";
            //command1.CommandText = "SELECT  TestPayment.Date , count(TestPayment.CabNo)as noOfCabs , sum(TestPayment.Amount)as Total from TestPayment Where (TestPayment.Date between  '" + fromDate + "' and  '" + toDate + "')   group by TestPayment.Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "PerDayIncome");
            connection.Close();

            CryMagnetMonthly rpt = new CryMagnetMonthly();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];
            TextObject txtCarType = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtAmount = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtCarType.Text = "Magnet Cars Without Branded Cars";
            txtAmount.Text = "LKR 350.00 Per Day";

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }



        public void BrandedPerDayTaxiIncome(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT  TestPayment.Date , count(TestPayment.CabNo)as noOfCabs , sum(TestPayment.Amount)as Total from TestPayment Where (TestPayment.Date between  '" + fromDate + "' and  '" + toDate + "') and (TestPayment.Amount > 0) AND (Cancel='B')  group by TestPayment.Date";
            //command1.CommandText = "SELECT  TestPayment.Date , count(TestPayment.CabNo)as noOfCabs , sum(TestPayment.Amount)as Total from TestPayment Where (TestPayment.Date between  '" + fromDate + "' and  '" + toDate + "')   group by TestPayment.Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "PerDayIncome");
            connection.Close();

            CrystalReport6 rpt = new CrystalReport6();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];
            TextObject txtCarType = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtCarType.Text = "Branded Car";
            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void DailyPaymentWithPhoneBill(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "'";
            command1.CommandText = "SELECT * from TestReciptHeader WHERE ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            //newadp1.Fill(recds, "AllSumarry");
            newadp1.Fill(recds, "AllSumarryNew");  
            connection.Close();

            CrystalReport7 rpt = new CrystalReport7();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text23"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];

            txtUser.Text = "All";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void DailyPaymentWithPhoneBillUserWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us=new User();
            string currentUser=us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            MySqlCommand command3 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT * from TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (UserID='" + currentUser + "') and (SpFlag !='Y')";
            command2.CommandText = "SELECT `Month` , sum( `Amount` ) AS Amount FROM `TestPhoneBillDetail` WHERE (`PayDate` BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (`user` = '" + currentUser + "') AND (`Delete`!='Y') AND (SpFlag !='Y')  GROUP BY `Month` ORDER BY Amount ASC";
            command3.CommandText = "SELECT monthname(`Date`) as month,sum( `Amount` ) as amount,count( `Date` ) AS Date FROM `dailyPayment` WHERE (`ReciptDate` BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (`UserID` = '" + currentUser + "') AND (SpFlag !='Y') GROUP BY month( `Date` ) ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            MySqlDataAdapter newadp2 = new MySqlDataAdapter(command2);//to retrive data (we can use data reader) 
            MySqlDataAdapter newadp3 = new MySqlDataAdapter(command3);
            newadp1.Fill(recds, "AllSumarryNew");
            newadp2.Fill(recds, "MobitelUser");
            newadp3.Fill(recds, "PaymentMonth"); 

            DataTable dtmonth = recds.Tables["MobitelUser"];
            DataTable dtPayment = recds.Tables["PaymentMonth"];
          
            string bill = "";
            for (int i = 0; i < dtmonth.Rows.Count; i++)
            {
              
               bill += ","+dtmonth.Rows[i]["Month"].ToString();
               bill += " = " + dtmonth.Rows[i]["Amount"].ToString()+".00";
              
            }

            string payment = "";
            for (int i = 0; i < dtPayment.Rows.Count; i++)
            {

                payment += "  ,  " + dtPayment.Rows[i]["month"].ToString();
                payment += " ( For " + dtPayment.Rows[i]["Date"].ToString() + " Days)  = " + dtPayment.Rows[i]["amount"].ToString() + ".00";
            }


            connection.Close();

            CrystalReport7 rpt = new CrystalReport7();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text23"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject billMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text129"];
            TextObject payMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text132"];

            txtLocation.Text = get_LocationName();
            txtUser.Text = currentUser;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            billMonth.Text = bill;
            payMonth.Text = payment;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }



        public void DailyMagnetCarPaymentWithPhoneBillUserWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            MySqlCommand command3 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT * from TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (UserID='" + currentUser + "') and (SpFlag ='Y')";
            command2.CommandText = "SELECT `Month` , sum( `Amount` ) AS Amount FROM `TestPhoneBillDetail` WHERE (`PayDate` BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (`user` = '" + currentUser + "') AND (`Delete`!='Y') AND (SpFlag ='Y')  GROUP BY `Month` ORDER BY Amount ASC";
            command3.CommandText = "SELECT monthname(`Date`) as month,sum( `Amount` ) as amount,count( `Date` ) AS Date FROM `dailyPayment` WHERE (`ReciptDate` BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (`UserID` = '" + currentUser + "') AND (SpFlag ='Y') GROUP BY month( `Date` ) ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            MySqlDataAdapter newadp2 = new MySqlDataAdapter(command2);//to retrive data (we can use data reader) 
            MySqlDataAdapter newadp3 = new MySqlDataAdapter(command3);
            newadp1.Fill(recds, "AllSumarryNew");
            newadp2.Fill(recds, "MobitelUser");
            newadp3.Fill(recds, "PaymentMonth");

            DataTable dtmonth = recds.Tables["MobitelUser"];
            DataTable dtPayment = recds.Tables["PaymentMonth"];

            string bill = "";
            for (int i = 0; i < dtmonth.Rows.Count; i++)
            {

                bill += "," + dtmonth.Rows[i]["Month"].ToString();
                bill += " = " + dtmonth.Rows[i]["Amount"].ToString() + ".00";

            }

            string payment = "";
            for (int i = 0; i < dtPayment.Rows.Count; i++)
            {

                payment += "  ,  " + dtPayment.Rows[i]["month"].ToString();
                payment += " ( For " + dtPayment.Rows[i]["Date"].ToString() + " Days)  = " + dtPayment.Rows[i]["amount"].ToString() + ".00";
            }


            connection.Close();

            CryDailyRecSumary rpt = new CryDailyRecSumary();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text23"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject billMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text129"];
            TextObject payMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text132"];

            txtLocation.Text = get_LocationName();
            txtUser.Text = currentUser;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            billMonth.Text = bill;
            payMonth.Text = payment;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }



        public void DailyPaymentWithPhoneBillLocationWise(DateTime from, DateTime to,string location,string fullLocation)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();
        
            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection.CreateCommand();
            MySqlCommand command3 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT * from TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (Location='" + location + "')";
            command2.CommandText = "SELECT `Month` , sum( `Amount` ) AS Amount FROM `TestPhoneBillDetail` WHERE (`PayDate` BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (`location` = '" + location + "')  GROUP BY `Month` ORDER BY Amount ASC";
            command3.CommandText = "SELECT monthname(`Date`) as month,sum( `Amount` ) as amount FROM `dailyPayment` WHERE (`ReciptDate` BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (`Location` = '" + location + "') GROUP BY month( `Date` ) ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            MySqlDataAdapter newadp2 = new MySqlDataAdapter(command2);//to retrive data (we can use data reader) 
            MySqlDataAdapter newadp3 = new MySqlDataAdapter(command3);
            newadp1.Fill(recds, "AllSumarryNew");
            newadp2.Fill(recds, "MobitelUser");
            newadp3.Fill(recds, "PaymentMonth");

            DataTable dtmonth = recds.Tables["MobitelUser"];
            DataTable dtPayment = recds.Tables["PaymentMonth"];

            string bill = "";
            for (int i = 0; i < dtmonth.Rows.Count; i++)
            {

                bill += "," + dtmonth.Rows[i]["Month"].ToString();
                bill += " = " + dtmonth.Rows[i]["Amount"].ToString() + ".00";
            }

            string payment = "";
            for (int i = 0; i < dtPayment.Rows.Count; i++)
            {

                payment += "," + dtPayment.Rows[i]["month"].ToString();
                payment += " = " + dtPayment.Rows[i]["amount"].ToString() + ".00";
            }


            connection.Close();

            CrystalReport7 rpt = new CrystalReport7();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text23"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject billMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text129"];
            TextObject payMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text132"];

            txtUser.Text = "All";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            billMonth.Text = bill;
            txtLocation.Text = fullLocation;
            payMonth.Text = payment;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        
        public void DailyPaymentWithPhoneBillUserWiseTimeRange(DateTime timeFrom, DateTime timeTo)
        {
            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT * from TestReciptHeader WHERE (EnteredDateTime BETWEEN '" + fromTime + "' and '" + toTime + "') and (UserID='" + currentUser + "')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            //newadp1.Fill(recds, "AllSumarry");
            newadp1.Fill(recds, "AllSumarryNew");
            connection.Close();

            CrystalReport7 rpt = new CrystalReport7();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text23"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];

            txtUser.Text = currentUser;
            txtFrom.Text = fromTime;
            txtTo.Text = toTime;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }
        
        public void FineUserWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT CabNo,RecNo,ReciptDate,Fine,FineRemark,UserID FROM TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (UserID='" + currentUser + "') and(TestReciptHeader.Delete !='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            //newadp1.Fill(recds, "AllSumarry");
            newadp1.Fill(recds, "FineAll");
            connection.Close();

            CryFineAll rpt = new CryFineAll();
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

            txtType.Text = "User Wise";
            txtUser.Text = currentUser;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void AppRentalUserWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT CabNo,RecNo,ReciptDate,appRental,AppRemark,UserID FROM TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (UserID='" + currentUser + "') and(TestReciptHeader.Delete !='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            //newadp1.Fill(recds, "AllSumarry");
            newadp1.Fill(recds, "AppRentalAll");
            connection.Close();

            CryAppRentalAll rpt = new CryAppRentalAll();
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

            txtType.Text = "User Wise";
            txtUser.Text = currentUser;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }



        public void FineAllUser(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT CabNo,RecNo,ReciptDate,Fine,FineRemark,UserID FROM TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (TestReciptHeader.Delete !='Y') and (Fine>0) ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            //newadp1.Fill(recds, "AllSumarry");
            newadp1.Fill(recds, "FineAll");
            connection.Close();

            CryFineAll rpt = new CryFineAll();
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

            txtType.Text = "All User";
            txtUser.Text = "All User";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void AppRentalAllUser(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT CabNo,RecNo,ReciptDate,appRental,AppRemark,UserID FROM TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (TestReciptHeader.Delete !='Y') and (appRental>0) ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            //newadp1.Fill(recds, "AllSumarry");
            newadp1.Fill(recds, "AppRentalAll");
            connection.Close();

            CryAppRentalAll rpt = new CryAppRentalAll();
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

            txtType.Text = "All User";
            txtUser.Text = "All User";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }





        public void FineAllUserLocationWise(DateTime from, DateTime to,string location,string fullocation)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptDate,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.DateFrom,TestReciptHeader.DateTo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' and TestReciptHeader.UserID='"+currentUser+"'";
            command1.CommandText = "SELECT CabNo,RecNo,ReciptDate,Fine,FineRemark,UserID FROM TestReciptHeader WHERE (ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "') and (TestReciptHeader.Delete !='Y') and (Fine>0) and (TestReciptHeader.location='"+location+"') ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            //newadp1.Fill(recds, "AllSumarry");
            newadp1.Fill(recds, "FineAll");
            connection.Close();

            CryFineAll rpt = new CryFineAll();
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];

            txtType.Text = "All User";
            txtUser.Text = "All User";
            txtLocation.Text = fullocation;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printCancellationInfo(DateTime from, DateTime to) 
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();
            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT * FROM TestCancellation WHERE Date BETWEEN '"+fromDate+"' AND '"+toDate+"' ORDER By Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "Cancell");
            connection.Close();

            CryCancellDetails rpt = new CryCancellDetails();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtUser.Text = "All";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printCancellationInfoUSerWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();
            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT * FROM TestCancellation WHERE (Date BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (User='"+currentUser+"') ORDER By Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "Cancell");
            connection.Close();

            CryCancellDetails rpt = new CryCancellDetails();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtUser.Text = currentUser;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printVoucherCancellationInfoUSerWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            us = new User();
            string currentUser = us.getCurrentUser();

            Form3 f3 = new Form3();
            f3.Show();
            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT * FROM TestVoucherCancellation WHERE (date BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (user='" + currentUser + "') ORDER By date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherCancell");
            connection.Close();

            CryVoucherCancell rpt = new CryVoucherCancell();           
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtUser.Text = currentUser;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printVoucherCancellationInfo(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);
            

            Form3 f3 = new Form3();
            f3.Show();
            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT * FROM TestVoucherCancellation WHERE (date BETWEEN '" + fromDate + "' AND '" + toDate + "')  ORDER By date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherCancell");
            connection.Close();

            CryVoucherCancell rpt = new CryVoucherCancell();
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtUser.Text = "All";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;


            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void DateWisePaymentWithPhoneBill(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT ReciptDate, count(CabNo)as CabCount, SUM(ReciptAmount) as TotAmount,SUM(TotBillRecv) as TotBill FROM `TestReciptHeader`WHERE ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' GROUP BY ReciptDate";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "AllSumDate");
            connection.Close();

            CrystalReport8 rpt = new CrystalReport8();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void SumaryPaymentWithPhoneBill(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT  TestReciptHeader.CabNo, sum(TestReciptHeader.ReciptAmount)as TotalAmount ,sum(TestReciptHeader.TotBillRecv)as TotalBill,TestReciptHeader.monthFd from TestReciptHeader WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' group by TestReciptHeader.cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "AllSumDetails");
            connection.Close();

            CrystalReport9 rpt = new CrystalReport9();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text19"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printFreePromotion(TextBox tbRecNo,int option)
        {

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            if(option==1)
                command1.CommandText = "SELECT CabNo,RecNo,RefNo,Date,Day,Cuser FROM TestFreePayment WHERE (RecNo='" + tbRecNo.Text + "') AND (SpFlag !='Y')  ORDER BY Date ASC";
            if(option==2)
                command1.CommandText = "SELECT CabNo,RecNo,RefNo,Date,Day,Cuser FROM TestFreePayment WHERE (RecNo='" + tbRecNo.Text + "') AND (SpFlag ='Y') ORDER BY Date ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "FreeDays");

            connection.Close();

            CrystalReport10 rpt = new CrystalReport10();
            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";
            // rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;
            rpt.PrintToPrinter(1, false, 1, 1);
        }

        public void SumaryPaymentWithPhoneB(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT  TestReciptHeader.CabNo, sum(TestReciptHeader.ReciptAmount)as TotalAmount ,sum(TestReciptHeader.TotBillRecv)as TotalBill,TestReciptHeader.monthFd from TestReciptHeader WHERE  TestReciptHeader.ReciptDate BETWEEN '" + fromDate + "' and '" + toDate + "' group by TestReciptHeader.cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "AllSumDetails");
            connection.Close();

            CrystalReport9 rpt = new CrystalReport9();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text19"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printFreeDayDateWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT Count(CabNo) as CabCount,RecNo,RefNo,Date FROM TestFreePayment WHERE (Date  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (TestFreePayment.Cancel !='Y') GROUP BY Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "FreeDayDateWise");
            connection.Close();

            CrystalReport11 rpt = new CrystalReport11();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printBrandedFreeDays(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT Date,count(CabNo) as cabcount FROM TestPayment WHERE (Date  BETWEEN '" + fromDate + "' and '" + toDate + "') and (Cancel='B') and (TestPayment.Delete !='Y') group by Date";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "BrandedFreeDays");
            connection.Close();

            CrystalReport23 rpt = new CrystalReport23();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printFreeDayCabWise(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT Date,CabNo,RecNo,RefNo,Remark,Cuser FROM TestFreePayment WHERE  (Date  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (TestFreePayment.Cancel !='Y') ORDER BY Date ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "FreeDays");
            connection.Close();

            CrystalReport12 rpt = new CrystalReport12();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printSpecialFreeDays(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();           
            command1.CommandText = "SELECT `CabNo`,`RefNo`,count(`Date`) as Dates,`Remark`,`Cuser`  FROM `TestFreePayment` WHERE (`SpFlag`='Y' ) AND  (Date  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (`Cancel`='0')Group by `CabNo`,`RefNo`";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "SpecialFreeDays");
            connection.Close();

            CrySpecialFreeDays rpt = new CrySpecialFreeDays();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printBrandedCars(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,Count(Date)as Date FROM TestPayment WHERE (Date BETWEEN '" + fromDate + "' AND '" + toDate + "') AND (Cancel='B') AND (TestPayment.Delete !='Y') GROUP BY CabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "BrandedCars");
            connection.Close();

            CrystalReport29 rpt = new CrystalReport29();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }
        
        public void printWithRefVoucherUserWise(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (user='" + user + "')  ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13 rpt = new CrystalReport13();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = user;
            systemLocation.Text = vloc;

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printWithoutRefVoucherUserWise(DateTime from, DateTime to)
        {

            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,company,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,user,Location FROM TestNoRefVoucherPay WHERE  (PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (user='" + user + "')  AND (cancel !='Y') ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithoutRef");
            connection.Close();

           // CrystalReport14 rpt = new CrystalReport14();
            CrystalReport14Group rpt = new CrystalReport14Group();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];

            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = user;
            systemLocation.Text = vloc;

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printWithoutRefVoucherUserWiseMonthGroup(DateTime from, DateTime to)
        {

            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,company,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,user,Location,cancel FROM TestNoRefVoucherPay WHERE  (PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (user='" + user + "')  ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithoutRef");
            connection.Close();

            // CrystalReport14 rpt = new CrystalReport14();
            CrystalReport14Group rpt = new CrystalReport14Group();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];

            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = user;
            systemLocation.Text = vloc;

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printWithRefVoucherLocationWise(DateTime from, DateTime to, string cbLocation)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (Location='" + cbLocation + "')   ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13 rpt = new CrystalReport13();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = "All Users";
            systemLocation.Text = cbLocation;

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printWithRefVoucherYardAndOffice(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE ( VoucherDate  BETWEEN '" + fromDate + "' and '" + toDate + "')    ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13 rpt = new CrystalReport13();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = "All Users";
            systemLocation.Text = "Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printVoucherPaymentSummaryWithref(DateTime from, DateTime to)
        {
            //us = new User();
            //string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT VoucherDate,sum(VoucherAmount) as VoucherAmount,sum(commition)as commition ,sum(BalAmount)as BalAmount FROM TestRefVoucherPay WHERE  (VoucherDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel !='Y') Group BY VoucherDate";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherSum");
            connection.Close();

            CrystalReport21 rpt = new CrystalReport21();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtType.Text = "With Ref";
            //systemUser.Text = "All Users";
            //systemLocation.Text = "Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


           

        }

        public void printVoucherPaymentSummaryWithoutref(DateTime from, DateTime to)
        {
            //us = new User();
            //string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT VoucherDate,sum(VoucherAmount) as VoucherAmount,sum(commition)as commition ,sum(BalAmount)as BalAmount FROM TestNoRefVoucherPay WHERE  (VoucherDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel !='Y') Group BY VoucherDate";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherSum");
            connection.Close();

            CrystalReport21 rpt = new CrystalReport21();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtType.Text = "Without Ref";
            //systemUser.Text = "All Users";
            //systemLocation.Text = "Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


        }

        public void printAllVoucherPaymentSummary(DateTime from, DateTime to)
        {
            //us = new User();
            //string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT VoucherDate,sum(VoucherAmount) as VoucherAmount,sum(commition)as commition ,sum(BalAmount)as BalAmount FROM TestViewAllVoucherPay WHERE  (VoucherDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel !='Y') Group BY VoucherDate";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherSum");
            connection.Close();

            CrystalReport21 rpt = new CrystalReport21();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtType = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtType.Text = "All Voucher Payment";
            //systemUser.Text = "All Users";
            //systemLocation.Text = "Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


        }

        public void printWithoutRefVoucherLocationWise(DateTime from, DateTime to, string cbLocation)
        {

            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,company,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,user,Location FROM TestNoRefVoucherPay WHERE  (PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND  (Location='" + cbLocation + "')  AND (cancel !='Y') ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithoutRef");
            connection.Close();

            CrystalReport14 rpt = new CrystalReport14();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];

            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = "ALL Users";
            systemLocation.Text = cbLocation;

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printWithoutRefVoucherLocationWiseMonthGroup(DateTime from, DateTime to, string cbLocation)
        {

            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,company,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,user,Location,cancel FROM TestNoRefVoucherPay WHERE  (PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND  (Location='" + cbLocation + "')  ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithoutRef");
            connection.Close();

            CrystalReport14Group rpt = new CrystalReport14Group();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];

            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = "ALL Users";
            systemLocation.Text = cbLocation;

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printWithoutRefVoucherYardAndOffice(DateTime from, DateTime to)
        {

            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,company,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,user,Location FROM TestNoRefVoucherPay WHERE  PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "' ORDER BY PayDate ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithoutRef");
            connection.Close();

            CrystalReport14 rpt = new CrystalReport14();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];

            TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            systemUser.Text = "ALL Users";
            systemLocation.Text = "Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}

        }

        public void printWithRefVoucherUserWiseTimeRange(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND user='" + user + "' ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13 rpt = new CrystalReport13();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];



            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }

        public void printWithRefVoucherUserWiseTimeRangeMonthGroup(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype !='Wallet') AND (paytype !='Credit')AND (paytype !='Corperate') AND (paytype !='Touch')AND (paytype !='SLT')AND (paytype !='Call-UP')AND (paytype !='Token') AND (exCom ='N') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Without Credit Card and Corperate - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }


        public void printWithRefVoucherUserWiseTimeRangeMonthGroupCreditCard(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype ='Credit') AND (exCom ='N') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Credit Card Only - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }

        public void printWithRefVoucherUserWiseTimeRangeMonthGroupCorperate(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype ='Corperate') AND (exCom ='N') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Corperate Only - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }

        public void printWithRefVoucherUserWiseTimeRangeMonthGroupAddCom(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (exCom ='Y') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Additional Commission - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }
        public void printWithRefVoucherUserWiseTimeRangeMonthGroupTouch(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype ='Touch') AND (exCom ='N')ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Touch Only - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }

        public void printWithRefVoucherUserWiseTimeRangeMonthGroupSLT(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype ='SLT') AND (exCom ='N') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "SLT Only - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }

        public void printWithRefVoucherUserWiseTimeRangeMonthGroupCallUP(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype ='Call-UP') AND (exCom ='N') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Call-UP Only - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }

        public void printWithRefVoucherUserWiseTimeRangeMonthGroupToken(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,AppAmount,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype ='Token') AND (exCom ='N') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Token Only - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }


        public void printWithRefVoucherUserWiseTimeRangeMonthGroupWallet(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName(); //t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND (user='" + user + "') AND (paytype ='Wallet') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CryVoucherRefPayGroupWallet rpt = new CryVoucherRefPayGroupWallet();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Wallet Only - With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }



        public void printWithRefVoucherUserWiseMonthGroupLocationWise(DateTime DateFrom, DateTime DateTo,string location, string fullocation)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            //string vloc = t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromDate = String.Format("{0:yyyy-MM-dd}", DateFrom);
            string toDate = String.Format("{0:yyyy-MM-dd}", DateTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestRefVoucherPay WHERE  (PayDate  BETWEEN '" + fromDate + "' and '" + toDate + "') AND (Location='" + location + "') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromDate;
            txtTimeTo.Text = toDate;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = fullocation;
            txtTopic.Text = "With Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                //t1.updateLastPrintedUserWithRef(user);
            }

        }

        public void printWithoutRefVoucherUserWiseTimeRangeMonthGroup(DateTime timeFrom, DateTime timeTo)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            string vloc = get_LocationName();//t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeFrom);
            string toTime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", timeTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestNoRefVoucherPay WHERE  (PayDateTime  BETWEEN '" + fromTime + "' and '" + toTime + "') AND user='" + user + "' ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromTime;
            txtTimeTo.Text = toTime;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = vloc;
            txtTopic.Text = "Without Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }


        public void printWithoutRefVoucherLocationWise(DateTime DateFrom, DateTime DateTo,string Location, string fullocation)
        {
            us = new User();
            string user = us.getCurrentUser();

            t1 = new Taxi();
            //string vloc = t1.EnteredLocation();

            t1 = new Taxi();

            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            string fromDate = String.Format("{0:yyyy-MM-dd}", DateFrom);
            string toDate = String.Format("{0:yyyy-MM-dd}", DateTo);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,Cnumber,VoucherDate,month(VoucherDate) as Month,voucherRefNo,company,paytype,VoucherNo,VoucherAmount,CommRate,commition,BalAmount,PayDate,PayDateTime,user,Location,cancel FROM TestNoRefVoucherPay WHERE  (PayDate  BETWEEN '" + DateFrom + "' and '" + DateTo + "') AND (Location='" + Location + "') ORDER BY PayDateTime ASC";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherWithRef");
            connection.Close();

            CrystalReport13Group rpt = new CrystalReport13Group();

            TextObject txtTimeFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text22"];
            TextObject txtTimeTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtSystemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtSystemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text28"];


            txtTimeFrom.Text = fromDate;
            txtTimeTo.Text = toDate;
            txtSystemUser.Text = user;
            txtSystemLocation.Text = fullocation;
            txtTopic.Text = "Without Ref No";

            rpt.SetDataSource(recds);
            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";

            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Are you Sure Want to Print This Report", "Print", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 0, 0);
                t1.updateLastPrintedUserWithRef(user);
            }

        }
        
        public void printOtherPayment(DateTime from, DateTime to)
        {

            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT * FROM TestOtherPayment WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (TestOtherPayment.Delete!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "OtherRecPrint");
            connection.Close();

            CrystalReport16 rpt = new CrystalReport16();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text17"];
            TextObject User = (TextObject)rpt.ReportDefinition.ReportObjects["Text27"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            User.Text = "All";
            txtLocation.Text = get_LocationName();
            //systemUser.Text = "ALL Users";
            //systemLocation.Text ="Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;

        }


        public void printOtherPaymentLocationWise(DateTime from, DateTime to,string location, string fullocation)
        {

            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT * FROM TestOtherPayment WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (TestOtherPayment.Delete!='Y') AND (TestOtherPayment.location='" + location + "')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "OtherRecPrint");
            connection.Close();

            CrystalReport16 rpt = new CrystalReport16();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text17"];
            TextObject User = (TextObject)rpt.ReportDefinition.ReportObjects["Text27"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            User.Text = "All";
            txtLocation.Text = fullocation;
            //systemUser.Text = "ALL Users";
            //systemLocation.Text ="Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;

        }




        public void printOtherPaymentCancellaion(DateTime from, DateTime to)
        {

            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT * FROM TestOtherPayment WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (TestOtherPayment.Delete ='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "OtherRecPrint");
            connection.Close();

            CrystalReport16 rpt = new CrystalReport16();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text17"];
            TextObject User = (TextObject)rpt.ReportDefinition.ReportObjects["Text27"];
            TextObject flag = (TextObject)rpt.ReportDefinition.ReportObjects["Text26"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            User.Text = "All";
            flag.Text = "Cancellation Report";
            //systemUser.Text = "ALL Users";
            //systemLocation.Text ="Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void printOtherPaymentUserWise(DateTime from, DateTime to)
        {

            us = new User();
            string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT * FROM TestOtherPayment WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "')AND (user='" + user + "') AND (TestOtherPayment.Delete!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "OtherRecPrint");
            connection.Close();

            CrystalReport16 rpt = new CrystalReport16();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text17"];
            TextObject User = (TextObject)rpt.ReportDefinition.ReportObjects["Text27"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            User.Text = user;
            //systemUser.Text = "ALL Users";
            //systemLocation.Text ="Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void printVoucherPaymentProof(DateTime from,DateTime to) 
        {
            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT * FROM testVoucherPayProof WHERE date BETWEEN '"+fromDate+"' AND '"+toDate+"'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherPayProof");
            connection.Close();

            CrystalReport17 rpt = new CrystalReport17();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


        }

        public void printVoucherHire(DateTime from, DateTime to)
        {
            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
           // command1.CommandText = "SELECT * FROM testVoucherPayProof WHERE date BETWEEN '" + fromDate + "' AND '" + toDate + "'";
            command1.CommandText = "SELECT cabNo, count(voucherRefNo) as TotHire,sum(VoucherAmount)as VoucherAmount ,sum(commition)as commition,sum(BalAmount) as BalAmount FROM TestRefVoucherPay WHERE (VoucherDate BETWEEN '" + fromDate + "' AND '" + toDate + "')  AND (cancel !='Y') GROUP BY cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherHire");
            connection.Close();

            CrystalReport18 rpt = new CrystalReport18();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;


        }
             
        public void printVoucherHireFillGrid(DateTime from, DateTime to,DataGridView dgv1)
        {
           
            dgv1.DataSource = null;
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

                    
            MySqlConnection connection = new MySqlConnection(constr);
            MySqlConnection connection2 = new MySqlConnection(constr2);
            connection.Open();
            connection2.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection2.CreateCommand();
            command1.CommandText = "SELECT cabNo, count(voucherRefNo) as TotHire,sum(VoucherAmount)as VoucherAmount ,sum(commition)as commition,sum(BalAmount) as BalAmount FROM allVoucherPayment WHERE (VoucherDate BETWEEN '" + fromDate + "' AND '" + toDate + "')  AND (cancel !='Y')  GROUP BY cabNo";
            //command1.CommandText = "SELECT cabNo, count(voucherRefNo) as TotHire,sum(VoucherAmount)as VoucherAmount ,sum(commition)as commition,sum(BalAmount) as BalAmount FROM TestRefVoucherPay WHERE (VoucherDate BETWEEN '" + fromDate + "' AND '" + toDate + "')  AND (cancel !='Y')  GROUP BY cabNo";
            command2.CommandText = "SELECT count(Date) as workedDate , CabNo as callcab FROM CallingBdNo WHERE Date BETWEEN '" + fromDate + "'AND '" + toDate + "' GROUP BY CabNo ORDER BY CabNo";

     
            int j = 0;
            using (var reader2 = command2.ExecuteReader())
            {

                while (reader2.Read())
                {
                        dgv1.Rows.Add();
                        dgv1.Rows[j].Cells[5].Value = reader2["callcab"].ToString();
                        dgv1.Rows[j].Cells[6].Value = reader2["workedDate"].ToString();                      
                        j++;                    
                }
            }
       
            using (var reader = command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    string cabno = reader["cabNo"].ToString();

                    int r;
                    for (r=0; r < j; r++)
                    {
                        if (("K" + dgv1.Rows[r].Cells[5].Value.ToString()) == cabno)
                        {
                            dgv1.Rows[r].Cells[0].Value = reader["cabNo"].ToString();
                            dgv1.Rows[r].Cells[1].Value = reader["TotHire"].ToString();
                            dgv1.Rows[r].Cells[2].Value = reader["VoucherAmount"].ToString();
                            dgv1.Rows[r].Cells[3].Value = reader["commition"].ToString();
                            dgv1.Rows[r].Cells[4].Value = reader["BalAmount"].ToString();
                        }
                        
                    }
                    r = 0;                  

                }
            }

                   connection.Close();
                   connection2.Close();

            //*******************Fill DataTable Using  DataGridView*****************************

                   DataSet1 ds = new DataSet1();
                   DataTable dt = new DataTable();
                   dt = ds.VoucherHireWorked;
                   DataRow workRow;

                   for (int n = 0; n < dgv1.RowCount; n++)
                   {
                       if (dgv1.Rows[n].Cells[0].Value != null)
                       {
                           workRow = ds.VoucherHireWorked.NewRow();
                           workRow[0] = dgv1.Rows[n].Cells[0].Value.ToString();
                           workRow[1] = dgv1.Rows[n].Cells[1].Value.ToString();
                           workRow[2] = dgv1.Rows[n].Cells[2].Value.ToString();
                           workRow[3] = dgv1.Rows[n].Cells[3].Value.ToString();
                           workRow[4] = dgv1.Rows[n].Cells[4].Value.ToString();
                           workRow[5] = dgv1.Rows[n].Cells[6].Value.ToString();
                           dt.Rows.Add(workRow);
                       }
                   }

            //***************************************************
                   Form3 f3 = new Form3();
                   CryVoucherHireWorkedDate rpt = new CryVoucherHireWorkedDate();

                   TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
                   TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
                   txtFrom.Text = fromDate;
                   txtTo.Text = toDate;

                   rpt.SetDataSource(ds);
                   f3.crystalReportViewer1.ReportSource = rpt;
                   f3.Show();

            }
        
        //public void countHireDateWithVouchers(string fromDate, string toDate)
        //{
        //    frmGridReport frmgrid = new frmGridReport();
        //    //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
        //    //string toDate = String.Format("{0:yyyy-MM-dd}", to);


        //    //System.Data.DataSet ds = new System.Data.DataSet();
        //    //System.Data.DataTable dt = new System.Data.DataTable();

        //    DataSet1 recds = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr2);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    command1.CommandText = "SELECT count(Date) as workedDate , CabNo as callcab FROM CallingBdNo WHERE Date BETWEEN '" + fromDate + "'AND '" + toDate + "' GROUP BY CabNo ORDER BY CabNo";
        //    //MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  

        //    int i = 0;
        //    using (var reader = command1.ExecuteReader())
        //    {

        //        while (reader.Read())
        //        {

        //            //frmgrid.dataGridView1.Rows.Add();
        //            frmgrid.dataGridView1.Rows[i].Cells[5].Value = reader["callcab"].ToString();
        //            frmgrid.dataGridView1.Rows[i].Cells[6].Value = reader["workedDate"].ToString();
        //            i++;
        //        }
        //    }



        //    //newadp1.Fill(ds);
        //    //dt = ds.Tables[0];
        //    connection.Close();

        //    //frmGridReport frmgrid = new frmGridReport();
        //    //frmgrid.dataGridView1.DataSource = dt;
        //    //frmgrid.Show();
            

        //}

        public void printVoucherHireCabWise(DateTime from, DateTime to,string cabno)
        {
            cabno = "K" + cabno;
            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
           // command1.CommandText = "SELECT * FROM testVoucherPayProof WHERE date BETWEEN '" + fromDate + "' AND '" + toDate + "'";
            command1.CommandText = "SELECT cabNo, count(voucherRefNo) as TotHire,SUM(VoucherAmount) as VoucherAmount ,SUM(commition) as commition,SUM(BalAmount) as BalAmount FROM TestRefVoucherPay WHERE (cabNo='" + cabno + "') AND (VoucherDate  BETWEEN '" + fromDate + "' AND '" + toDate + "')  AND (cancel !='Y') GROUP BY cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherHire");
            connection.Close();

            CrystalReport18 rpt = new CrystalReport18();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void VoucherPaymentSummary(DateTime from, DateTime to) //nandika Reports
        {
            us = new User();
            string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            // command1.CommandText = "SELECT * FROM testVoucherPayProof WHERE date BETWEEN '" + fromDate + "' AND '" + toDate + "'";
            command1.CommandText = "SELECT cabNo,VoucherDate,voucherRefNo,VoucherNo,VoucherAmount,BalAmount,PayDate,user  FROM TestRefVoucherPay WHERE (user='" + user + "') AND (PayDate  BETWEEN '" + fromDate + "' AND '" + toDate + "')  AND (cancel !='Y') order BY cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherPaySummary");
            connection.Close();

            CrystalReport19 rpt = new CrystalReport19();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        //public void printOtherPaymentUserWise(DateTime from, DateTime to)
        //{

        //    us = new User();
        //    string cuser = us.getCurrentUser();

        //    //t1 = new Taxi();
        //    //string vloc = t1.VoucherEnteredLocation();


        //    string fromDate = String.Format("{0:yyyy-MM-dd}", from);
        //    string toDate = String.Format("{0:yyyy-MM-dd}", to);

        //    Form3 f3 = new Form3();
        //    f3.Show();

        //    DataSet1 recds = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
        //    command1.CommandText = "SELECT * FROM TestOtherPayment WHERE (Date BETWEEN '" + fromDate + "' AND '" + toDate + "') AND user='" + cuser + "'";
        //    MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
        //    newadp1.Fill(recds, "OtherRecPrint");
        //    connection.Close();

        //    CrystalReport16 rpt = new CrystalReport16();
        //    TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
        //    TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text17"];
        //    TextObject User = (TextObject)rpt.ReportDefinition.ReportObjects["Text27"];
        //    //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
        //    //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

        //    txtFrom.Text = fromDate;
        //    txtTo.Text = toDate;
        //    User.Text = cuser;
        //    //systemUser.Text = "ALL Users";
        //    //systemLocation.Text ="Yard And Head Office";

        //    rpt.SetDataSource(recds);
        //    //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
        //    //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
        //    // rpt.PrintOptions.PrinterName = "Epson LX-300+";
        //    f3.crystalReportViewer1.ReportSource = rpt;

        //}

        public void findOtherReceipt(TextBox tbrecNo, DataGridView dgv1)
        {
            if (tbrecNo.Text != "")
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataSet1 recfd = new DataSet1();
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command1 = connection.CreateCommand();
                command1.CommandText = "SELECT OtherRecNo,Date FROM TestOtherPayment WHERE OtherRecNo LIKE '" + tbrecNo.Text + "%'";
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

        public string SecettedOtherRecipPrint(DataGridView dgv1)
        {
            string recNo = dgv1.Rows[dgv1.CurrentRow.Index].Cells[0].Value.ToString();

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT OtherRecNo,cabNo,PlateNo,Name,NICDL,Date,Amount,DriverDepo,Registration,SimDepo,sticker,Tshirt,AtoZ,Leasing,Ldeposit,SimFine,Fine, OtherAmount,Remark,user FROM  TestOtherPayment WHERE OtherRecNo='" + recNo + "'";                                  
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "OtherRecPrint");
            connection.Close();
            CrystalReport15 rpt = new CrystalReport15();
            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";
             //rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            //f3.crystalReportViewer1.ReportSource = rpt;
            //rpt.PrintToPrinter(1, false, 1, 1);
            
            f3.crystalReportViewer1.ReportSource = rpt;
            DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                rpt.PrintToPrinter(1, false, 1, 1);
            }


            return recNo;
        }

        public void printActiveCabList(DateTimePicker dtFrom,DateTimePicker dtTo)
        {
            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", dtFrom.Value);
            string toDate = String.Format("{0:yyyy-MM-dd}", dtTo.Value);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT CabNo,PlateNo,OpenDate,CloseDate FROM TestCabList  WHERE  (OpenDate BETWEEN '" + fromDate + "' AND '" + toDate + "') AND flag='0' ORDER BY cabNo";
            command1.CommandText = "SELECT CabNo,PlateNo,OpenDate,CloseDate FROM TestCabList  WHERE flag='0'  ORDER BY cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "CabList");
            connection.Close();

            CrystalReport20 rpt = new CrystalReport20();
            TextObject CablistType = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            TextObject datefrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject dateto = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
           
            CablistType.Text ="Active Cab List";
            datefrom.Text = fromDate;
            dateto.Text = toDate;
            rpt.SetDataSource(recds);
            
            f3.crystalReportViewer1.ReportSource = rpt;


        }

        public void printWithdrawnCabList(DateTimePicker dtFrom, DateTimePicker dtTo)
        {
            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", dtFrom.Value);
            string toDate = String.Format("{0:yyyy-MM-dd}", dtTo.Value);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT CabNo,PlateNo,OpenDate,CloseDate FROM TestCabList  WHERE  (OpenDate BETWEEN '" + fromDate + "' AND '" + toDate + "') AND flag='0' ORDER BY cabNo";
            command1.CommandText = "SELECT CabNo,PlateNo,OpenDate,CloseDate FROM TestCabList  WHERE (flag='1') AND CloseDate BETWEEN '"+fromDate+"' AND '"+toDate+"'  ORDER BY cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "CabList");
            connection.Close();

            CrystalReport20 rpt = new CrystalReport20();
            TextObject CablistType = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            TextObject datefrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject dateto = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];

            CablistType.Text = "Withdrew Cab List";
            datefrom.Text = fromDate;
            dateto.Text = toDate;
            rpt.SetDataSource(recds);

            f3.crystalReportViewer1.ReportSource = rpt;


        }

        public void printVenturaReceipt(TextBox tbcabno, string recno)
        {


            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,PlateNo,RecNo,Date,Amount,PayDatetime,User FROM TestVenturaPayment WHERE cabNo='" + tbcabno.Text + "' AND RecNo='" + recno + "' Order by Date";
            //command1.CommandText = "SELECT cabNo,PlateNo,RecNo,Date,Amount,PayDatetime,User FROM TestVenturaPayment WHERE cabNo='K712' AND RecNo='V000004' Order by Date";
          
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
            

        }

        public void PrintVenturaIncomeUserWise(DateTime from, DateTime to)
        {
            us = new User();
            string cuser = us.getCurrentUser();
                     
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,PlateNo,Owner,RecNo,NumOfDays,Amount,User FROM TestVenRecHeader WHERE (PayDate BETWEEN '" + fromDate + "' AND '" + toDate + "') AND User='" + cuser + "'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VenSmryUser");
            connection.Close();

            CrystalReport25 rpt = new CrystalReport25();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text17"];
                      
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
                      
            rpt.SetDataSource(recds);           
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void PrintVenturaIncomeDateWise(DateTime from, DateTime to)//not completed
        {
            us = new User();
            string cuser = us.getCurrentUser();

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,PlateNo,Owner,RecNo,NumOfDays,Amount,User FROM TestVenRecHeader WHERE (PayDate BETWEEN '" + fromDate + "' AND '" + toDate + "') ";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VenSmryUser");
            connection.Close();

            CrystalReport25 rpt = new CrystalReport25();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text17"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void PrintVenturaIncomeCabWise(DateTime from, DateTime to)
        {
            us = new User();
            string cuser = us.getCurrentUser();

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT cabNo,PlateNo,Sum(Amount)as Amount FROM TestVenturaPayment WHERE (PayDate BETWEEN '" + fromDate + "' AND '" + toDate + "') Group by cabNo";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VenSmryCab");
            connection.Close();

            CrystalReport27 rpt = new CrystalReport27();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void selectPrintOption(RadioButton rb1,RadioButton rb2,RadioButton rb3,DateTimePicker dtfrom,DateTimePicker dtto,TextBox tbcabno,TextBox tbuser) 
        {
            if (rb1.Checked == true && (rb2.Checked == false && rb3.Checked == false)) 
            {
                PrintVenturaIncomeUserWise(dtfrom.Value,dtto.Value);
            }
            if (rb2.Checked == true && (rb1.Checked == false && rb3.Checked == false))
            {
                
            }
            if (rb3.Checked == true && (rb1.Checked == false && rb2.Checked == false))
            {
               PrintVenturaIncomeCabWise(dtfrom.Value,dtto.Value);
            }
        }

        public void printSimDepoRefund(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT * from TestOtherPayment WHERE (RefundDate BETWEEN '" + fromDate + "' and '" + toDate + "') AND (Refund='1')AND (TestOtherPayment.Delete!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);           
            newadp1.Fill(recds, "SimDepoRefund");
            connection.Close();

            CrystalReport28 rpt = new CrystalReport28();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text36"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text38"];

           
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void selectVoucherReportPrint(string user, string cabno, string location, DateTimePicker dtfromdate, DateTimePicker dttodate, RadioButton rb1, RadioButton rb2, RadioButton rb3, RadioButton rb4, RadioButton rb5, RadioButton rb6, RadioButton rb7, RadioButton rb8, RadioButton rb9, RadioButton rb10, RadioButton rb11, RadioButton rb12, RadioButton rb13, RadioButton rb14, RadioButton rb15, RadioButton rb16, RadioButton rb17, RadioButton rb18, RadioButton rb19, RadioButton rb20, RadioButton rb21, RadioButton rb22, RadioButton rb23,RadioButton rb24,RadioButton rb25,RadioButton rb26,RadioButton rb27, DataGridView dgv1,ComboBox cmbMonth,ComboBox cmbYear,RadioButton rb28 ,RadioButton rb29,RadioButton rb30) 
        {
            if (rb1.Checked == true)
                printVoucherPaymentSummaryWithref(dtfromdate.Value, dttodate.Value);
            else if (rb15.Checked == true)
                printVoucherPaymentSummaryWithoutref(dtfromdate.Value, dttodate.Value);
            else if (rb16.Checked == true)
                printAllVoucherPaymentSummary(dtfromdate.Value, dttodate.Value);
            else if (rb2.Checked == true)
                printWithRefVoucherUserWise(dtfromdate.Value, dttodate.Value);
            else if (rb3.Checked == true)
                printWithRefVoucherLocationWise(dtfromdate.Value, dttodate.Value, location);
            else if (rb4.Checked == true)
                printWithRefVoucherYardAndOffice(dtfromdate.Value, dttodate.Value);
            else if (rb5.Checked == true)
                printWithoutRefVoucherUserWiseMonthGroup(dtfromdate.Value, dttodate.Value);
            else if (rb6.Checked == true)
                printWithoutRefVoucherLocationWiseMonthGroup(dtfromdate.Value, dttodate.Value, location);
            else if (rb7.Checked == true)
                printVoucherHire(dtfromdate.Value, dttodate.Value);
            else if (rb8.Checked == true)
                printVoucherHireCabWise(dtfromdate.Value, dttodate.Value, cabno);
            else if (rb9.Checked == true)
                printVoucherPaymentProof(dtfromdate.Value, dttodate.Value);
            else if (rb10.Checked == true)
                VoucherPaymentSummary(dtfromdate.Value, dttodate.Value);
            else if (rb11.Checked == true || rb12.Checked == true || rb13.Checked == true || rb14.Checked == true)
                printVoucherSummary(dtfromdate.Value, dttodate.Value, user, rb11, rb12, rb13, rb14);
            else if (rb17.Checked == true)
                printVoucherHireFillGrid(dtfromdate.Value, dttodate.Value, dgv1);
            else if (rb18.Checked == true)
                printVoucherDeductRefundSummary(dtfromdate.Value, dttodate.Value);
            else if (rb19.Checked == true)
                printVoucherDeductRefundAllUser(dtfromdate.Value, dttodate.Value);
            else if (rb20.Checked == true)
                printVoucherDeductRefundFlow(dtfromdate.Value, dttodate.Value, cmbMonth, cmbYear);
            else if (rb21.Checked == true)
                printVoucherDeductRefundIndividualCabs(dtfromdate.Value, dttodate.Value, cabno);

            else if (rb22.Checked == true)
                printVoucherDeductRefundAllUserVoucherMonth(cmbMonth, cmbYear);
            else if (rb24.Checked == true)
                printVoucherNotRefundedSummary(dtfromdate.Value, dttodate.Value);
            else if (rb25.Checked == true)
                printVoucherPartiallyrefundedSummary(dtfromdate.Value, dttodate.Value);
            else if (rb26.Checked == true)
                printVoucherDeductRefundUserViceIndividual(dtfromdate.Value, dttodate.Value, cmbMonth, cmbYear);
            else if (rb27.Checked == true)
                printVoucherDeductRefundUserViceIndividualCalender(dtfromdate.Value, dttodate.Value);
            else if (rb28.Checked == true)
                AppPhoneFineAllUser(dtfromdate.Value, dttodate.Value);
            else if (rb29.Checked == true)
                AppPhoneFineAllCabSummary(dtfromdate.Value, dttodate.Value);
            else if (rb30.Checked == true)
                AppPhoneFineRefundSummary(dtfromdate.Value, dttodate.Value);
                
        }

        public void printBankDepositedPayment(DateTime from, DateTime to)
        {
            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT TestReciptHeader.RecNo,TestReciptHeader.ReciptAmount,TestReciptHeader.CabNo,TestReciptHeader.nDays,TestReciptHeader.TotBillRecv,TestPhoneBillDetail.Month,TestPhoneBillDetail.Year,TestPhoneBillDetail.Amount from TestReciptHeader right join TestPhoneBillDetail on TestReciptHeader.RecNo=TestPhoneBillDetail.RecNo WHERE  TestReciptHeader.ReciptDate BETWEEN '"+fromDate+"' and '"+toDate+"'";
            command1.CommandText = "SELECT CabNo,recno,DepositDate,Amount,Name,User FROM TestBankDeposit WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (`Delete` !='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "BankDeposit");
            connection.Close();

            CryBankDeposit rpt = new CryBankDeposit();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];
            //TextObject User = (TextObject)rpt.ReportDefinition.ReportObjects["Text27"];
            //TextObject systemUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text2"];
            //TextObject systemLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];

            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
           // User.Text = "All";
            //systemUser.Text = "ALL Users";
            //systemLocation.Text ="Yard And Head Office";

            rpt.SetDataSource(recds);
            //rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            //rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            // rpt.PrintOptions.PrinterName = "Epson LX-300+";
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        public void printVoucherSummary(DateTime from, DateTime to, string user,  RadioButton rb1,RadioButton rb2,RadioButton rb3,RadioButton rb4)
        {

            //us = new User();
            //string user = us.getCurrentUser();

            //t1 = new Taxi();
            //string vloc = t1.VoucherEnteredLocation();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            
            if(rb1.Checked==true)
                command1.CommandText = "SELECT cabNo,SUM(VoucherAmount) AS VoucherAmount,SUM(commition) AS commition, SUM(BalAmount) AS BalAmount   FROM TestRefVoucherPay WHERE PayDate BETWEEN '" + fromDate + "' and '" + toDate + "'  GROUP BY cabNo";

            else if(rb2.Checked==true)               
                 command1.CommandText = "SELECT cabNo,SUM(VoucherAmount) AS VoucherAmount,SUM(commition) AS commition, SUM(BalAmount) AS BalAmount   FROM TestNoRefVoucherPay WHERE PayDate BETWEEN '" + fromDate + "' and '" + toDate + "'  GROUP BY cabNo";

            else if (rb3.Checked == true)
                command1.CommandText = "SELECT cabNo,SUM(VoucherAmount) AS VoucherAmount,SUM(commition) AS commition, SUM(BalAmount) AS BalAmount   FROM TestRefVoucherPay WHERE (PayDate BETWEEN '" + fromDate + "' and '" + toDate + "') AND user='" + user + "' GROUP BY cabNo";

            else if (rb4.Checked == true)
                command1.CommandText = "SELECT cabNo,SUM(VoucherAmount) AS VoucherAmount,SUM(commition) AS commition, SUM(BalAmount) AS BalAmount   FROM TestNoRefVoucherPay WHERE (PayDate BETWEEN '" + fromDate + "' and '" + toDate + "') AND user='" + user + "' GROUP BY cabNo";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "VoucherSummary");
            connection.Close();

            CryVoucherSum rpt = new CryVoucherSum();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject User = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject heading = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
          
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;

            if(rb1.Checked==true || rb2.Checked==true)
                User.Text = "All";

            else if (rb3.Checked == true || rb4.Checked == true)
                User.Text = user;

            if (rb1.Checked == true || rb3.Checked == true)
                heading.Text = "With Ref No";

           else if (rb2.Checked == true || rb4.Checked == true)
                heading.Text = "Without Ref No";
           

            rpt.SetDataSource(recds);            
            f3.crystalReportViewer1.ReportSource = rpt;

        }

        //public void printVoucherDeductionUserVice(DateTime from, DateTime to)
        //{
        //    us = new User();
        //    string user = us.getCurrentUser();


        //    string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
        //    string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);



        //    Form3 f3 = new Form3();
        //    f3.Show();

        //    DataSet1 recds = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,totEarning,addEarning,month,year FROM TestVoucherDeduct WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "')AND (cancel!='Y') AND (user='" + user + "')";
        //    MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
        //    newadp1.Fill(recds, "Deduction");
        //    connection.Close();

        //    CryDeduction rpt = new CryDeduction();
        //    TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
        //    TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];


        //    txtFrom.Text = fromDate;
        //    txtTo.Text = toDate;
        //    txtuser.Text = user;

        //    rpt.SetDataSource(recds);
        //    f3.crystalReportViewer1.ReportSource = rpt;
        //}

        //public void printVoucherRefundUserVice(DateTime from, DateTime to)
        //{
        //    us = new User();
        //    string user = us.getCurrentUser();


        //    string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
        //    string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);



        //    Form3 f3 = new Form3();
        //    f3.Show();

        //    DataSet1 recds = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    command1.CommandText = "SELECT vrecno,cabNo,reAmount,totDays,totEarning, FROM TestVoucherDeduct WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "')AND (cancel!='Y') AND (user='" + user + "')";
        //    MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
        //    newadp1.Fill(recds, "Deduction");
        //    connection.Close();

        //    CryDeduction rpt = new CryDeduction();
        //    TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
        //    TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];


        //    txtFrom.Text = fromDate;
        //    txtTo.Text = toDate;
        //    txtuser.Text = user;

        //    rpt.SetDataSource(recds);
        //    f3.crystalReportViewer1.ReportSource = rpt;
        //}

        public void printVoucherDeductRefundUserVice(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "')AND (cancel!='Y') AND (user='" + user + "')";
            
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReUser rpt = new CryDeReUser();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtuser.Text = user;
            txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printNewVoucherDeductRefundUserVice(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "')AND (cancel!='Y') AND (user='" + user + "')";
            command1.CommandText = "SELECT RecNo,CabNo,CurrentDeduct,CurrentRefund,	NumOfDays,CreditLimit,Target,TotEarning,DateTime,User,Flag FROM TestNewDeductReceipt WHERE (DateTime BETWEEN '" + fromDate + "' and '" + toDate + "')AND (Flag!='1') AND (User='" + user + "')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "NewDeductRefund");
            connection.Close();

            CryNewDeReUser rpt = new CryNewDeReUser();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtuser.Text = user;
            txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printVoucherDeductRefundLocationWise(DateTime from, DateTime to,string location,string fullocation)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);



            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "')AND (cancel!='Y') AND (location='" + location + "')";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReUser rpt = new CryDeReUser();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtuser.Text = user;
            txtLocation.Text = fullocation;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printVoucherDeductRefundAllUser(DateTime from, DateTime to)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y')  ORDER BY date ASC";
           // command1.CommandText = "SELECT vrecno,SUBSTRING(cabNo, PATINDEX('%[0-9]%', cabNo) as cabNo,deAmount,reAmount,totDays,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReAllUser rpt = new CryDeReAllUser();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text1"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            //TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];

            txtTopic.Text = "Voucher Payment Deduction Summary - User Wise";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

         public void printNewDeductionREfundAllCabMonthly(DateTime from, DateTime to,ComboBox cmbMonth,ComboBox cmbYear)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            string toDate = String.Format("{0:yyyy-MM-dd }", to);
            string month =cmbMonth.SelectedItem.ToString();
            string year = cmbYear.SelectedItem.ToString(); 

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT `CabNo`,CreditLimit, max( `NumOfDays` ) as totDays , max( `TotEarning` ) as totEarn , sum( `CurrentDeduct` ) as totDeduct , sum( `CurrentRefund` ) as totRefund FROM `TestNewDeductReceipt` WHERE (`Date` BETWEEN '" + fromDate + "' and '" + toDate + "') AND (`Month`='" + month + "' AND `Year`='" + year + "') GROUP BY `CabNo` order by totDays desc";
           // command1.CommandText = "SELECT vrecno,SUBSTRING(cabNo, PATINDEX('%[0-9]%', cabNo) as cabNo,deAmount,reAmount,totDays,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "NewDeductRefundAllCab");
            connection.Close();

            CryNewDeductionRefundAllCabMonth rpt = new CryNewDeductionRefundAllCabMonth();
            TextObject txtMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtYear = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            //TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];

            //txtTopic.Text = "Voucher Payment Deduction Summary - User Wise";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtMonth.Text =month;
            txtYear.Text = year;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printVoucherDeductRefundAllUserVoucherMonth(ComboBox cmbMonth,ComboBox cmbYear)
        {
            //string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (month='" + cmbMonth.Text + "' AND year='" + cmbYear.Text + "') AND (cancel!='Y')  ORDER BY date ASC";
            // command1.CommandText = "SELECT vrecno,SUBSTRING(cabNo, PATINDEX('%[0-9]%', cabNo) as cabNo,deAmount,reAmount,totDays,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReAllUser rpt = new CryDeReAllUser();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text1"];
            TextObject txtMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txtYear = (TextObject)rpt.ReportDefinition.ReportObjects["Text21"];
            //TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];

            txtTopic.Text = "Voucher Payment Deduction Summary - User Wise - Voucher Month";
            txtMonth.Text = cmbMonth.Text;
            txtYear.Text = cmbYear.Text;
            //txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }
        
        public void printVoucherDeductRefundFlow(DateTime from, DateTime to,ComboBox cmbMonth,ComboBox cmbYear)
        {

            
            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (month='" + cmbMonth.Text + "') AND (year='" + cmbYear.Text + "') AND (cancel!='Y')  ORDER BY cabNo,date";
            // command1.CommandText = "SELECT vrecno,SUBSTRING(cabNo, PATINDEX('%[0-9]%', cabNo) as cabNo,deAmount,reAmount,totDays,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReAllUser rpt = new CryDeReAllUser();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text1"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            //TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];

            txtTopic.Text = "Voucher Payment Deduction Information - Cab Wise ";
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printVoucherDeductRefundIndividualCabs(DateTime from, DateTime to,string cabno)
        {

            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y') AND (cabNo='" + cabno + "') ORDER BY cabNo,date";
            // command1.CommandText = "SELECT vrecno,SUBSTRING(cabNo, PATINDEX('%[0-9]%', cabNo) as cabNo,deAmount,reAmount,totDays,target,totEarning,date,user,cancel FROM TestDeReInfo WHERE (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y')";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReCab rpt = new CryDeReCab();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtCab = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            //TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];

            //txtTopic.Text = "Voucher Payment Deduction Information - Cab Wise ";
            txtCab.Text = cabno;
            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printVoucherDeductRefundSummary(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();
           
            double preDeduc = 0.00;
            double preRefund = 0.00;
            double preDeRe = 0.00;
            double preTot = 0.00;

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);            

            int month=from.Month;
            int year=from.Year;
            double avgcabs = 0; int avgdays = 0;
            

            

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            MySqlConnection connection2 = new MySqlConnection(constr2);
            connection.Open();
            connection2.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection2.CreateCommand();

            command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE (month='" + month + "' AND year='" + year + "') AND (cancel!='Y')  ORDER BY totDays";
          // command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount=0) )  ORDER BY totDays";
            //command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount != 0) ) AND ((deAmount-reAmount) > 0) ORDER BY totDays";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefundSum");

            command2.CommandText = "SELECT  (count( `CabNo` ) / Count( DISTINCT `Date` )) as avgCabs, Count( DISTINCT `Date`) as avgDays FROM `CallingBdNo` WHERE `Date` BETWEEN '"+fromDate+"' AND '"+toDate+"'";            

             using (var reader2 = command2.ExecuteReader())
            {
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        avgcabs = Convert.ToDouble(reader2["avgCabs"].ToString());
                        avgdays = Convert.ToInt32(reader2["avgDays"].ToString()); 
                    }
                }
             }

             DataTable dtDeRe = recds.Tables["DeductRefundSum"];
             int recount = 0; int decount = 0; int derecount = 0; 

             for (int i = 0; i < dtDeRe.Rows.Count; i++)
             {
                 double de = Convert.ToDouble(dtDeRe.Rows[i]["deAmount"]);
                 double re = Convert.ToDouble(dtDeRe.Rows[i]["reAmount"]);

                 if(re==0.00)  //deducted bu not refunded
                    decount++;
                 if((re != 0) && ((de - re) != 0.00))//partiall refunded
                    derecount++;
                 if((de - re) == 0)//totally refunded
                    recount++;             

                 //if ((de-re) != 0.00)

             }

            connection.Close();

            connection2.Close();

            CryDeReSummary rpt = new CryDeReSummary();
            //CryDeReCustom rpt = new CryDeReCustom();
          

            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            TextObject txt1 = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            TextObject txt2 = (TextObject)rpt.ReportDefinition.ReportObjects["Text19"];
            TextObject txt3 = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
            TextObject txt4 = (TextObject)rpt.ReportDefinition.ReportObjects["Text30"];

            TextObject txtAvgCabs = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            TextObject txtAvgDays = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];

            TextObject txtDateFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text26"];
            TextObject txtDateTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text32"];



            preDeduc = Math.Round(((decount / Math.Round(avgcabs)) * 100),2);
            preRefund=Math.Round((recount/Math.Round(avgcabs)*100),2);
            preDeRe = Math.Round((derecount / Math.Round(avgcabs) * 100), 2);
            preTot = Math.Round(((derecount+recount+decount) / Math.Round(avgcabs) * 100), 2);

            txtAvgCabs.Text = (Math.Round(avgcabs)).ToString() ;
            txtAvgDays.Text = "For "+avgdays.ToString()+" Days";

            txt1.Text = "( "+preDeduc.ToString()+"%"+" )";
            txt2.Text = "( " + preRefund.ToString() + "%" + " )";
            txt3.Text = "( " + preDeRe.ToString() + "%" + " )";
            txt4.Text = "( " + preTot.ToString() + "%" + " )";

            txtFrom.Text = from.ToLongDateString().Split(' ').ElementAt(1);
            txtTo.Text = from.Year.ToString();
            txtDateFrom.Text = fromDate.ToString();
            txtDateTo.Text = toDate.ToString();
            //txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }
        
        //public void printVoucherDeductRefundIndividualUserSummary(DateTime from, DateTime to)
        //{
        //    us = new User();
        //    string user = us.getCurrentUser();

        //    double preDeduc = 0.00;
        //    double preRefund = 0.00;
        //    double preDeRe = 0.00;
        //    double preTot = 0.00;

        //    string fromDate = String.Format("{0:yyyy-MM-dd}", from);
        //    string toDate = String.Format("{0:yyyy-MM-dd}", to);

        //    int month = from.Month;
        //    int year = from.Year;
        //    double avgcabs = 0; int avgdays = 0;




        //    Form3 f3 = new Form3();
        //    f3.Show();

        //    DataSet1 recds = new DataSet1();

        //    MySqlConnection connection = new MySqlConnection(constr);
        //    MySqlConnection connection2 = new MySqlConnection(constr2);
        //    connection.Open();
        //    connection2.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    MySqlCommand command2 = connection2.CreateCommand();

        //    command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (user='"+user+"') ORDER BY totDays";
        //    // command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount=0) )  ORDER BY totDays";
        //    //command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount != 0) ) AND ((deAmount-reAmount) > 0) ORDER BY totDays";
        //    MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
        //    newadp1.Fill(recds, "DeductRefundSum");

        //    command2.CommandText = "SELECT  (count( `CabNo` ) / Count( DISTINCT `Date` )) as avgCabs, Count( DISTINCT `Date`) as avgDays FROM `CallingBdNo` WHERE `Date` BETWEEN '" + fromDate + "' AND '" + toDate + "'";

        //    using (var reader2 = command2.ExecuteReader())
        //    {
        //        if (reader2.HasRows)
        //        {
        //            while (reader2.Read())
        //            {
        //                avgcabs = Convert.ToDouble(reader2["avgCabs"].ToString());
        //                avgdays = Convert.ToInt32(reader2["avgDays"].ToString());
        //            }
        //        }
        //    }

        //    DataTable dtDeRe = recds.Tables["DeductRefundSum"];
        //    int recount = 0; int decount = 0; int derecount = 0;

        //    for (int i = 0; i < dtDeRe.Rows.Count; i++)
        //    {
        //        double de = Convert.ToDouble(dtDeRe.Rows[i]["deAmount"]);
        //        double re = Convert.ToDouble(dtDeRe.Rows[i]["reAmount"]);

        //        if (re == 0.00)  //deducted bu not refunded
        //            decount++;
        //        if ((re != 0) && ((de - re) != 0.00))//partiall refunded
        //            derecount++;
        //        if ((de - re) == 0)//totally refunded
        //            recount++;

        //        //if ((de-re) != 0.00)

        //    }

        //    connection.Close();

        //    connection2.Close();

        //    CryDeReSummaryIndividualUser rpt = new CryDeReSummaryIndividualUser();
        //    //CryDeReCustom rpt = new CryDeReCustom();


        //    //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

        //    TextObject txt1 = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
        //    TextObject txt2 = (TextObject)rpt.ReportDefinition.ReportObjects["Text19"];
        //    TextObject txt3 = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
        //    TextObject txt4 = (TextObject)rpt.ReportDefinition.ReportObjects["Text30"];

        //    TextObject txtPrintDate = (TextObject)rpt.ReportDefinition.ReportObjects["Text33"];
        //    TextObject txtUSer = (TextObject)rpt.ReportDefinition.ReportObjects["Text37"];

        //    TextObject txtAvgCabs = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
        //    TextObject txtAvgDays = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
        //    TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];

        //    TextObject txtDateFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text26"];
        //    TextObject txtDateTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text32"];



        //    preDeduc = Math.Round(((decount / Math.Round(avgcabs)) * 100), 2);
        //    preRefund = Math.Round((recount / Math.Round(avgcabs) * 100), 2);
        //    preDeRe = Math.Round((derecount / Math.Round(avgcabs) * 100), 2);
        //    preTot = Math.Round(((derecount + recount + decount) / Math.Round(avgcabs) * 100), 2);

        //    txtAvgCabs.Text = (Math.Round(avgcabs)).ToString();
        //    txtAvgDays.Text = "For " + avgdays.ToString() + " Days";

        //    txt1.Text = "( " + preDeduc.ToString() + "%" + " )";
        //    txt2.Text = "( " + preRefund.ToString() + "%" + " )";
        //    txt3.Text = "( " + preDeRe.ToString() + "%" + " )";
        //    txt4.Text = "( " + preTot.ToString() + "%" + " )";

        //    txtFrom.Text = from.ToLongDateString().Split(' ').ElementAt(1);
        //    txtTo.Text = from.Year.ToString();
        //    txtDateFrom.Text = fromDate.ToString();
        //    txtDateTo.Text = toDate.ToString();
        //    txtUSer.Text = user;
        //    txtPrintDate.Text = DateTime.Now.ToString();
        //    //txtuser.Text = user;

        //    rpt.SetDataSource(recds);
        //    f3.crystalReportViewer1.ReportSource = rpt;
        //}
        
        public void printVoucherDeductRefundUserViceIndividual(DateTime from, DateTime to, ComboBox cmbMonth, ComboBox cmbYear)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

            int month = Convert.ToInt32(cmbMonth.Text);
            int year = Convert.ToInt32(cmbYear.Text);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo  WHERE (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (user='" + user + "')";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReUserIndividual rpt = new CryDeReUserIndividual();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtYear = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];


            txtMonth.Text = month.ToString();
            txtYear.Text = year.ToString();
            txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        //public void printVoucherDeductRefundUserViceIndividual(DateTime from, DateTime to, ComboBox cmbMonth, ComboBox cmbYear)
        //{
        //    us = new User();
        //    string user = us.getCurrentUser();


        //    string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
        //    string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

        //    int month = Convert.ToInt32(cmbMonth.Text);
        //    int year = Convert.ToInt32(cmbYear.Text);


        //    Form3 f3 = new Form3();
        //    f3.Show();

        //    DataSet1 recds = new DataSet1();
        //    MySqlConnection connection = new MySqlConnection(constr);
        //    connection.Open();
        //    MySqlCommand command1 = connection.CreateCommand();
        //    command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo  WHERE (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (user='" + user + "')";

        //    MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
        //    newadp1.Fill(recds, "DeductRefund");
        //    connection.Close();

        //    CryDeReUserIndividual rpt = new CryDeReUserIndividual();
        //    //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
        //    TextObject txtYear = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
        //    TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];


        //    txtMonth.Text = month.ToString();
        //    txtYear.Text = year.ToString();
        //    txtuser.Text = user;

        //    rpt.SetDataSource(recds);
        //    f3.crystalReportViewer1.ReportSource = rpt;
        //}

         public void printVoucherDeductRefundUserViceIndividualCalender(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            //string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

            string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);


          

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            //command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo  WHERE  (date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y') AND (user='" + user + "')";
            command1.CommandText = "SELECT vrecno,cabNo,deAmount,reAmount,totDays,perDayLimit,target,totEarning,date,user,cancel FROM TestDeReInfo  WHERE  (DATE(date) BETWEEN '" + fromDate + "' and '" + toDate + "') AND (cancel!='Y') AND (user='" + user + "')";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefund");
            connection.Close();

            CryDeReUserIndividualCalender rpt = new CryDeReUserIndividualCalender();
            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];
            TextObject txtuser = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }
        
        public void printVoucherNotRefundedSummary(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();

            double preDeduc = 0.00;
            double preRefund = 0.00;
            double preDeRe = 0.00;
            double preTot = 0.00;

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            int month = from.Month;
            int year = from.Year;
            double avgcabs = 0; int avgdays = 0;




            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            MySqlConnection connection2 = new MySqlConnection(constr2);
            connection.Open();
            connection2.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection2.CreateCommand();

            //command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE (month='" + month + "' AND year='" + year + "') AND (cancel!='Y')  ORDER BY cabNo";
             command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount=0) )  ORDER BY totDays";
           // command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount != 0) ) AND ((deAmount-reAmount) > 0) ORDER BY totDays";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefundSum");

            command2.CommandText = "SELECT  (count( `CabNo` ) / Count( DISTINCT `Date` )) as avgCabs, Count( DISTINCT `Date`) as avgDays FROM `CallingBdNo` WHERE `Date` BETWEEN '" + fromDate + "' AND '" + toDate + "'";

            using (var reader2 = command2.ExecuteReader())
            {
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        avgcabs = Convert.ToDouble(reader2["avgCabs"].ToString());
                        avgdays = Convert.ToInt32(reader2["avgDays"].ToString());
                    }
                }
            }

            DataTable dtDeRe = recds.Tables["DeductRefundSum"];
            int recount = 0; int decount = 0; int derecount = 0;

            for (int i = 0; i < dtDeRe.Rows.Count; i++)
            {
                double de = Convert.ToDouble(dtDeRe.Rows[i]["deAmount"]);
                double re = Convert.ToDouble(dtDeRe.Rows[i]["reAmount"]);

                if (re == 0.00)  //deducted bu not refunded
                    decount++;
                if ((re != 0) && ((de - re) != 0.00))//partiall refunded
                    derecount++;
                if ((de - re) == 0)//totally refunded
                    recount++;

                //if ((de-re) != 0.00)

            }

            connection.Close();

            connection2.Close();

           // CryDeReSummary rpt = new CryDeReSummary();
            CryDeReCustom rpt = new CryDeReCustom();
            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text1"];
            txtTopic.Text = "Deducted Cabs (Not Refunded)";

            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            //TextObject txt1 = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
           //// TextObject txt2 = (TextObject)rpt.ReportDefinition.ReportObjects["Text19"];
           // TextObject txt3 = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
            //TextObject txt4 = (TextObject)rpt.ReportDefinition.ReportObjects["Text30"];

            //TextObject txtAvgCabs = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            //TextObject txtAvgDays = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];

            TextObject txtDateFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text26"];
            TextObject txtDateTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text32"];



            preDeduc = Math.Round(((decount / Math.Round(avgcabs)) * 100), 2);
            preRefund = Math.Round((recount / Math.Round(avgcabs) * 100), 2);
            preDeRe = Math.Round((derecount / Math.Round(avgcabs) * 100), 2);
            preTot = Math.Round(((derecount + recount + decount) / Math.Round(avgcabs) * 100), 2);

            //txtAvgCabs.Text = (Math.Round(avgcabs)).ToString();
            //txtAvgDays.Text = "For " + avgdays.ToString() + " Days";

                //txt1.Text = "( " + preDeduc.ToString() + "%" + " )";
                //txt2.Text = "( " + preRefund.ToString() + "%" + " )";
                //txt3.Text = "( " + preDeRe.ToString() + "%" + " )";
                //txt4.Text = "( " + preTot.ToString() + "%" + " )";

            txtFrom.Text = from.ToLongDateString().Split(' ').ElementAt(1);
            txtTo.Text = from.Year.ToString();
            txtDateFrom.Text = fromDate.ToString();
            txtDateTo.Text = toDate.ToString();
            //txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printVoucherPartiallyrefundedSummary(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();

            double preDeduc = 0.00;
            double preRefund = 0.00;
            double preDeRe = 0.00;
            double preTot = 0.00;

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", to);

            int month = from.Month;
            int year = from.Year;
            double avgcabs = 0; int avgdays = 0;




            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();

            MySqlConnection connection = new MySqlConnection(constr);
            MySqlConnection connection2 = new MySqlConnection(constr2);
            connection.Open();
            connection2.Open();
            MySqlCommand command1 = connection.CreateCommand();
            MySqlCommand command2 = connection2.CreateCommand();

            //command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE (month='" + month + "' AND year='" + year + "') AND (cancel!='Y')  ORDER BY cabNo";
            // command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount=0) )  ORDER BY totDays";
            command1.CommandText = "SELECT cabNo,deAmount,reAmount,totDays,perDayLimit,totEarning,addEarning,month,year,cancel FROM TestVoucherDeduct WHERE( (month='" + month + "' AND year='" + year + "') AND (cancel!='Y') AND (reAmount != 0) ) AND ((deAmount-reAmount) > 0) ORDER BY totDays";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "DeductRefundSum");

            command2.CommandText = "SELECT  (count( `CabNo` ) / Count( DISTINCT `Date` )) as avgCabs, Count( DISTINCT `Date`) as avgDays FROM `CallingBdNo` WHERE `Date` BETWEEN '" + fromDate + "' AND '" + toDate + "'";

            using (var reader2 = command2.ExecuteReader())
            {
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        avgcabs = Convert.ToDouble(reader2["avgCabs"].ToString());
                        avgdays = Convert.ToInt32(reader2["avgDays"].ToString());
                    }
                }
            }

            DataTable dtDeRe = recds.Tables["DeductRefundSum"];
            int recount = 0; int decount = 0; int derecount = 0;

            for (int i = 0; i < dtDeRe.Rows.Count; i++)
            {
                double de = Convert.ToDouble(dtDeRe.Rows[i]["deAmount"]);
                double re = Convert.ToDouble(dtDeRe.Rows[i]["reAmount"]);

                if (re == 0.00)  //deducted bu not refunded
                    decount++;
                if ((re != 0) && ((de - re) != 0.00))//partiall refunded
                    derecount++;
                if ((de - re) == 0)//totally refunded
                    recount++;

                //if ((de-re) != 0.00)

            }

            connection.Close();

            connection2.Close();

           // CryDeReSummary rpt = new CryDeReSummary();
            
            CryDeReCustom rpt = new CryDeReCustom();

            TextObject txtTopic = (TextObject)rpt.ReportDefinition.ReportObjects["Text1"];
            txtTopic.Text = "Partially Rerfunded Cabs";


            //TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            //TextObject txt1 = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];
            //TextObject txt2 = (TextObject)rpt.ReportDefinition.ReportObjects["Text19"];
            //TextObject txt3 = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
            //TextObject txt4 = (TextObject)rpt.ReportDefinition.ReportObjects["Text30"];

            //TextObject txtAvgCabs = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            //TextObject txtAvgDays = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text7"];

            TextObject txtDateFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text26"];
            TextObject txtDateTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text32"];



            preDeduc = Math.Round(((decount / Math.Round(avgcabs)) * 100), 2);
            //preRefund = Math.Round((recount / Math.Round(avgcabs) * 100), 2);
            //preDeRe = Math.Round((derecount / Math.Round(avgcabs) * 100), 2);
            preTot = Math.Round(((derecount + recount + decount) / Math.Round(avgcabs) * 100), 2);

            //txtAvgCabs.Text = (Math.Round(avgcabs)).ToString();
            //txtAvgDays.Text = "For " + avgdays.ToString() + " Days";

            //txt1.Text = "( " + preDeduc.ToString() + "%" + " )";
            //txt2.Text = "( " + preRefund.ToString() + "%" + " )";
            //txt3.Text = "( " + preDeRe.ToString() + "%" + " )";
            //txt4.Text = "( " + preTot.ToString() + "%" + " )";

            txtFrom.Text = from.ToLongDateString().Split(' ').ElementAt(1);
            txtTo.Text = from.Year.ToString();
            txtDateFrom.Text = fromDate.ToString();
            txtDateTo.Text = toDate.ToString();
            //txtuser.Text = user;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void lastPaymentDates()
        {
            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT `CabNo` , max( `Date` ) AS lDate FROM TestPayment WHERE `Delete` = 'N' GROUP BY `CabNo` ORDER BY lDate";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "LastPaymentDates");
            connection.Close();

            CryLastPayment rpt = new CryLastPayment();
            //TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            //TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];

            //txtFrom.Text = fromDate;
            //txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void lastPaymentDatesGivenRange(DateTime from)
        {
            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            //string toDate = String.Format("{0:yyyy-MM-dd}", to);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText ="SELECT `CabNo` , max( `Date` ) AS lDate FROM `TestPayment` GROUP BY `CabNo` HAVING max( `Date` ) >= '"+fromDate+"' ORDER BY lDate";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "LastPaymentDates");
            connection.Close();

            CryLastPayment rpt = new CryLastPayment();
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            //TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];

            txtFrom.Text = fromDate;
            //txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void cabAnalyser(DateTime from, DataGridView dgv1, DataGridView dgv2,DataGridView dgv3, DataGridView dgv4, Label lb1,Label lb2, Label lb3 ,int option) 
        {

            DateTime firstOfNextMonth = new DateTime(from.Year, from.Month, 1).AddMonths(1);
            DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);

            DateTime firstOfOtherNextMonth = new DateTime(firstOfNextMonth.Year, firstOfNextMonth.Month, 1).AddMonths(1);
            DateTime lastOfNextMonth = firstOfOtherNextMonth.AddDays(-1);

            string fromDate = String.Format("{0:yyyy-MM-dd}", from); //01-04-2016
            string toDate = String.Format("{0:yyyy-MM-dd}", lastOfThisMonth);//30-04-2016


            string nextFromDate = String.Format("{0:yyyy-MM-dd}", firstOfNextMonth); //01-05-2016
            string nextToDate = String.Format("{0:yyyy-MM-dd}", lastOfNextMonth);//31-05-2016

            getGivenMonthCabs(from, dgv1, dgv2,lb1,lb2);
            FindNewCabs(dgv1, dgv2,dgv3);
            FindAbsentCabs(dgv1, dgv2, dgv4);
            
            if(option==1)
            {

                CryCabanalysing rpt=new CryCabanalysing();
                printAnalisingData(dgv1,dgv2,dgv3,dgv4,fromDate,toDate,nextFromDate,nextToDate,option,rpt );
            }
           if(option==2)
            {

                CryNotWorkingCabanalysing rpt=new CryNotWorkingCabanalysing();
                printAnalisingData(dgv1, dgv2, dgv3, dgv4, fromDate, toDate, nextFromDate, nextToDate, option, rpt);
            }

            //printAnalisingData(dgv1,dgv2,dgv3,dgv4,fromDate,toDate,nextFromDate,nextToDate,option,);

          


        }

        public void getGivenMonthCabs(DateTime from,DataGridView dgv1,DataGridView dgv2,Label lb1,Label lb2) 
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();


            DateTime firstOfNextMonth = new DateTime(from.Year, from.Month, 1).AddMonths(1);
            DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", lastOfThisMonth);

            lb1.Text = "Date From " + fromDate + " To" + toDate;

            MySqlConnection connection1 = new MySqlConnection(constr);
            connection1.Open();
            MySqlCommand command = connection1.CreateCommand();
            // command.CommandText = "select ReciptNo,ReciptDate from ReciptHeader where CabNo='" + taxi + "' order by ReciptDate DESC";
            command.CommandText = "SELECT  DISTINCT `CabNo` FROM `TestPayment`WHERE (`Date`BETWEEN '" + fromDate + "'AND '" + toDate + "') AND (`Delete` = 'N') order by `CabNo` ";
            MySqlDataAdapter newadp = new MySqlDataAdapter(command);
            newadp.Fill(ds);
            dt = ds.Tables[0];
            dgv1.DataSource = dt;
            connection1.Close();

            getNextMonthCabs(firstOfNextMonth,dgv2,lb2); // send the next month first date
        }
        public void getNextMonthCabs(DateTime from,DataGridView dgv2,Label lb2) 
        {


            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();


            DateTime firstOfNextMonth = new DateTime(from.Year, from.Month, 1).AddMonths(1);
            DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);

            string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string toDate = String.Format("{0:yyyy-MM-dd}", lastOfThisMonth);

            lb2.Text = "Date From " + fromDate + " To" + toDate;

            MySqlConnection connection1 = new MySqlConnection(constr);
            connection1.Open();
            MySqlCommand command = connection1.CreateCommand();
            // command.CommandText = "select ReciptNo,ReciptDate from ReciptHeader where CabNo='" + taxi + "' order by ReciptDate DESC";
            command.CommandText = "SELECT  DISTINCT `CabNo` FROM `TestPayment`WHERE (`Date`BETWEEN '" + fromDate + "'AND '" + toDate + "')AND(`Delete` = 'N')  order by `CabNo`";
            MySqlDataAdapter newadp = new MySqlDataAdapter(command);
            newadp.Fill(ds);
            dt = ds.Tables[0];
            dgv2.DataSource = dt;
            connection1.Close();
        }

        public void FindNewCabs(DataGridView dgv1,DataGridView dgv2,DataGridView dgv3) 
        {
            int a = 0;
            int dgv1Rows = dgv1.Rows.Count;
            int dgv2Row = dgv2.Rows.Count;
            for (int i = 0; i < dgv2.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgv1.Rows.Count - 1; j++)
                {
                    if (dgv2.Rows[i].Cells[0].Value.ToString() == dgv1.Rows[j].Cells[0].Value.ToString())
                    {
                        dgv2.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            }

            for (int i = 0; i < dgv2.Rows.Count - 1; i++)
            {
                if (dgv2.Rows[i].DefaultCellStyle.BackColor != Color.Yellow)
                {
                    dgv3.Rows.Add();
                    dgv3.Rows[a].Cells[0].Value = dgv2.Rows[i].Cells[0].Value.ToString();
                    a++;
                }
            }
        }

        public void FindAbsentCabs(DataGridView dgv1, DataGridView dgv2, DataGridView dgv4) 
        {
            int a = 0;
            int dgv1Rows = dgv1.Rows.Count;
            int dgv2Row = dgv2.Rows.Count;
            for (int i = 0; i < dgv1.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dgv2.Rows.Count - 1; j++)
                {
                    if (dgv1.Rows[i].Cells[0].Value.ToString() == dgv2.Rows[j].Cells[0].Value.ToString())
                    {
                        dgv1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }

            for (int i = 0; i < dgv1.Rows.Count - 1; i++)
            {
                if (dgv1.Rows[i].DefaultCellStyle.BackColor != Color.Red)
                {
                    dgv4.Rows.Add();
                    dgv4.Rows[a].Cells[0].Value = dgv1.Rows[i].Cells[0].Value.ToString();
                    a++;
                }
            }
        }


        public void printAnalisingData(DataGridView dgv1,DataGridView dgv2,DataGridView dgv3,DataGridView dgv4,string lastFrom, string lastTo, string nextFrom,string nextTo,int option,ReportDocument rpt) 
        {
            DataSet1 ds = new DataSet1();
            DataTable dt = new DataTable();
            dt = ds.LastWorked;
            DataRow workRow;
           
          

            for (int n = 0; n < dgv2.RowCount; n++)
            {
                if (dgv2.Rows[n].Cells[0].Value != null)
                {                   
                    workRow = ds.LastWorked.NewRow();

                    if (dgv1.RowCount - 1 > n)
                    {
                        workRow[0] = dgv1.Rows[n].Cells[0].Value.ToString();
                        workRow[4] = (n+1).ToString();
                    }

                    workRow[1] = dgv2.Rows[n].Cells[0].Value.ToString();
                    workRow[5] = (n + 1).ToString();

                    if (dgv3.RowCount - 1 > n)
                    {
                        workRow[2] = dgv3.Rows[n].Cells[0].Value.ToString();
                        workRow[6] = (n + 1).ToString();
                    }
                    if (dgv4.RowCount - 1 > n)
                    {
                        workRow[3] = dgv4.Rows[n].Cells[0].Value.ToString();
                        workRow[7] = (n + 1).ToString();
                    }
                    //workRow[4] = dgv1.Rows[n].Cells[4].Value.ToString();
                    //workRow[5] = dgv1.Rows[n].Cells[6].Value.ToString();
                    dt.Rows.Add(workRow);
                }
            }


            //for (int n = 0; n < dgv2.RowCount; n++)
            //{
            //    if (dgv2.Rows[n].Cells[0].Value != null)
            //    {
            //        workRow = ds.LastWorked.NewRow();

            //        workRow[1] = dgv2.Rows[n].Cells[0].Value.ToString();
            //        //workRow[1] = dgv1.Rows[n].Cells[1].Value.ToString();
            //        //workRow[2] = dgv1.Rows[n].Cells[2].Value.ToString();
            //        //workRow[3] = dgv1.Rows[n].Cells[3].Value.ToString();
            //        //workRow[4] = dgv1.Rows[n].Cells[4].Value.ToString();
            //        //workRow[5] = dgv1.Rows[n].Cells[6].Value.ToString();
            //        dt.Rows.Add(workRow);
            //    }
            //}


            //***************************************************
            //CryCabanalysing rpt;
            //CryNotWorkingCabanalysing= rpt;
            Form3 f3 = new Form3();
            //if(option==1)
            //CryCabanalysing rpt = new CryCabanalysing();

            TextObject LastMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject NextMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];

            TextObject NewCab = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
            TextObject AbsentCab = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];

            TextObject top1 = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
            TextObject top2 = (TextObject)rpt.ReportDefinition.ReportObjects["Text14"];
            TextObject top3 = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject top4 = (TextObject)rpt.ReportDefinition.ReportObjects["Text16"];

            LastMonth.Text = "From " + lastFrom + " To " + lastTo;
            NextMonth.Text = "From " + nextFrom + " To " + nextTo;

            NewCab.Text = "From " + nextFrom + " To " + nextTo;
            AbsentCab.Text = "From " + nextFrom + " To " + nextTo;

            top1.Text = "From " + lastFrom + " To " + lastTo;
            top2.Text = "From " + nextFrom + " To " + nextTo;
            top3.Text = "From " + nextFrom + " To " + nextTo;
            top4.Text = "From " + nextFrom + " To " + nextTo;

            rpt.SetDataSource(ds);
            f3.crystalReportViewer1.ReportSource = rpt;
            f3.Show();


        }

        public void notWorkingNotPaid(DateTime from,DateTime ldate, TextBox tbBackDays)
        {
            int backDays = Convert.ToInt32(tbBackDays.Text);
            DateTime backDate = from.AddDays(-backDays);

            string backdate = String.Format("{0:yyyy-MM-dd}", backDate);
            //string fromDate = String.Format("{0:yyyy-MM-dd}", from);
            string lastDate = String.Format("{0:yyyy-MM-dd}", ldate);

            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT `CabNo` , max( `Date` ) AS lDate FROM `TestPayment` GROUP BY `CabNo` HAVING max( `Date` ) <= '" + backdate + "' AND max( `Date` ) >= '"+lastDate+"' ORDER BY lDate";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "LastPaymentDates"); 
            connection.Close();

            CryNotPaid rpt = new CryNotPaid();
            //TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            //TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];

            //txtFrom.Text = fromDate;
            //txtTo.Text = toDate;

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printVoucherPhoneBillRecoveryDaily(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT RecNo,CabNo,Month,Year,Amount,VpayDateTime, VpayDate, RefNo,Vuser FROM TestPhoneBillDetail WHERE (VpayDateTime BETWEEN '" + fromDate + "' and '" + toDate + "')AND (TestPhoneBillDetail.Delete !='Y') AND (Vuser='" + user + "')";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "BrandedPhoneBilFromVoucher");
            connection.Close();

            cryBrandPhoneBill rpt = new cryBrandPhoneBill();
           
            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];            
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text20"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;            
            txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printVoucherPhoneBillRecoverySummary(DateTime from, DateTime to)
        {
            //us = new User();
            //string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            string toDate = String.Format("{0:yyyy-MM-dd }", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT RecNo,CabNo,Month,Year,Amount,VpayDateTime, VpayDate, RefNo,	Vuser  FROM TestPhoneBillDetail WHERE (VpayDate BETWEEN '" + fromDate + "' and '" + toDate + "')AND (TestPhoneBillDetail.Delete !='Y') ";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "BrandedPhoneBilFromVoucher");
            connection.Close();

            cryBrandPhoneBillUser rpt = new cryBrandPhoneBillUser();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
           


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void printPendingPhoneBill()
        {
            //us = new User();
            //string user = us.getCurrentUser();


            //string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            //string toDate = String.Format("{0:yyyy-MM-dd }", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT RecNo,CabNo,Month,Year,Pending,PayDate,user FROM TestPhoneBillDetail WHERE (Pending > 0 AND  Amount=0) AND (branded='Y') AND   (TestPhoneBillDetail.Delete !='Y') ";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "BrandedPendingPhoneBill");
            connection.Close();

            CryPendingPhoneBill rpt = new CryPendingPhoneBill();

            //TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            //TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];



          


            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printMobileLoanRecoveryDaily(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr3);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,pay_amount,LoanNo,Operator,dateTime FROM mobile_pay WHERE (dateTime BETWEEN '" + fromDate + "' and '" + toDate + "')AND (Operator='" + user + "')";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "MobileLoan");
            connection.Close();

            CryMobilePhoneLoanRecoveryUserWise rpt = new CryMobilePhoneLoanRecoveryUserWise();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printAppPhoneFineChargesDaily(DateTime from, DateTime to) 
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,recNo,Amount,Date,DateTime,USer,Flag,Refund,RefundBy FROM TestAppFine  WHERE (DateTime BETWEEN '" + fromDate + "' and '" + toDate + "') AND (USer='" + user + "') AND (Amount > 0)";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "AppFine");
            connection.Close();

            CryAppPhoneFine rpt = new CryAppPhoneFine();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            //TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printAppPhoneFineRefundDaily(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", from);
            string toDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT `refundRecNo`,`CabNo`, sum(`Amount`) as amount,RefundBy,Refund FROM `TestAppFine` WHERE (`RefundBy` ='" + user + "') AND (RefunddateTime BETWEEN '" + fromDate + "' and '" + toDate + "')  group by `CabNo`";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "AppFineRefund");
            connection.Close();

            CryDailyAppFineRefund rpt = new CryDailyAppFineRefund();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            //TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }



        public void AppPhoneFineAllUser(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            string toDate = String.Format("{0:yyyy-MM-dd }", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,recNo,Amount,Date,DateTime,USer,Flag,Refund,RefundBy FROM TestAppFine  WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (Amount > 0)";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "AppFine");
            connection.Close();

            CryAppPhoneFineAllUser rpt = new CryAppPhoneFineAllUser();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            //TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }

        public void AppPhoneFineAllCabSummary(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            string toDate = String.Format("{0:yyyy-MM-dd }", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,sum(Amount) as Amount,Flag FROM TestAppFine  WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (Amount > 0) group by CabNo";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "AppFine");
            connection.Close();

            CryAppFineSummary rpt = new CryAppFineSummary();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text5"];
            //TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void AppPhoneFineRefundSummary(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            string toDate = String.Format("{0:yyyy-MM-dd }", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT refundRecNo,CabNo,sum(Amount) as amount,Refund,RefundBy,	RefunddateTime FROM TestAppFine  WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "') AND (Flag = 1) group by CabNo";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "AppFineRefund");
            connection.Close();

            CryAppFineRefundUserWise rpt = new CryAppFineRefundUserWise();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text11"];
            //TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            //txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printMobileLoanRecoverySummary(DateTime from, DateTime to)
        {
            us = new User();
            string user = us.getCurrentUser();


            string fromDate = String.Format("{0:yyyy-MM-dd }", from);
            string toDate = String.Format("{0:yyyy-MM-dd }", to);


            Form3 f3 = new Form3();
            f3.Show();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr3);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();
            command1.CommandText = "SELECT CabNo,pay_amount,LoanNo,Operator,Date FROM mobile_pay WHERE (Date BETWEEN '" + fromDate + "' and '" + toDate + "')";

            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);
            newadp1.Fill(recds, "MobileLoan");
            connection.Close();

            CryMobilePhoneLoanRecoverySumary rpt = new CryMobilePhoneLoanRecoverySumary();

            TextObject txtFrom = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];
            TextObject txtTo = (TextObject)rpt.ReportDefinition.ReportObjects["Text13"];
            TextObject txtLocation = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];


            txtFrom.Text = fromDate;
            txtTo.Text = toDate;
            txtLocation.Text = get_LocationName();

            rpt.SetDataSource(recds);
            f3.crystalReportViewer1.ReportSource = rpt;
        }


        public void printAppFineRefundReceipt(string recno)
        {
            Form3 f3 = new Form3();
            f3.Show();
            us = new User();

            DataSet1 recds = new DataSet1();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command1 = connection.CreateCommand();

            command1.CommandText = "SELECT `refundRecNo`,`CabNo`,sum(`Amount`)as amount,`RefunddateTime`,`RefundBy`,`Refund` FROM `TestAppFine` WHERE `refundRecNo`='"+recno+"'";
            MySqlDataAdapter newadp1 = new MySqlDataAdapter(command1);//to retrive data (we can use data reader)  
            newadp1.Fill(recds, "AppFineRefund");


            connection.Close();

            CryAppFineRefundReceipt rpt = new CryAppFineRefundReceipt();
            rpt.SetDataSource(recds);

            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize;
            //rpt.PrintOptions.PrinterName = "Epson LX-300+";
            // rpt.PrintOptions.PrinterName = "Epson LX-300+ (Copy 2)";
            f3.crystalReportViewer1.ReportSource = rpt;

            //DialogResult dr = MessageBox.Show("Do you want to print this Receipt", "Print", MessageBoxButtons.YesNoCancel);
            //if (dr == DialogResult.Yes)
            //{
            //    rpt.PrintToPrinter(1, false, 1, 1);
            //}


        }

        public void printNewDeductionReceipt(DataGridView dgv1, TextBox tbCreditLimt,TextBox tbNumOfDays,TextBox tbTotIncome,TextBox tbTraget,TextBox tbAddIncome,TextBox tbYear,TextBox tbMonth,TextBox tbPreDeduct,TextBox tbPreRefund,TextBox tbCurrentDeduct,TextBox tbCurrentRefund,TextBox tbRecNo,Label lbCabno,Label lbDate,Label lbUser) 
        {
           
            Form3 f3 = new Form3();
            f3.Show();
            int i = 0;
            CryDeductReceipt rpt = new CryDeductReceipt();

            DataSet1 ds = new DataSet1();
            DataTable dt = new DataTable();
            dt = ds.Calculation;
            DataRow workRow;

            for (int n = 0; n < dgv1.RowCount; n++)
            {
                if (dgv1.Rows[n].Cells[0].Value != null)
                {
                    
                    workRow = ds.Calculation.NewRow();
                    workRow[0] = dgv1.Rows[n].Cells[0].Value.ToString();
                    workRow[1] = dgv1.Rows[n].Cells[1].Value.ToString();
                    workRow[2] = dgv1.Rows[n].Cells[2].Value.ToString();
                    workRow[3] = dgv1.Rows[n].Cells[3].Value.ToString();
                    workRow[4] = dgv1.Rows[n].Cells[4].Value.ToString();                    
                    dt.Rows.Add(workRow);
                    
                }
            }


            TextObject txtCreditLimt = (TextObject)rpt.ReportDefinition.ReportObjects["Text6"];
            TextObject txtNumOfDays = (TextObject)rpt.ReportDefinition.ReportObjects["Text10"];
            TextObject txtTotIncome = (TextObject)rpt.ReportDefinition.ReportObjects["Text12"];
            TextObject txtTotDays = (TextObject)rpt.ReportDefinition.ReportObjects["Text15"];
            TextObject txtTarget = (TextObject)rpt.ReportDefinition.ReportObjects["Text18"];
            TextObject txtAddIncome = (TextObject)rpt.ReportDefinition.ReportObjects["Text21"];
            TextObject txtYear = (TextObject)rpt.ReportDefinition.ReportObjects["Text24"];
            TextObject txtMonth = (TextObject)rpt.ReportDefinition.ReportObjects["Text25"];
            TextObject txtPreDeduct = (TextObject)rpt.ReportDefinition.ReportObjects["Text29"];
            TextObject txtPreRefund = (TextObject)rpt.ReportDefinition.ReportObjects["Text30"];
            TextObject txtCurentDeduct = (TextObject)rpt.ReportDefinition.ReportObjects["Text33"];
            TextObject txtCurrentRefund = (TextObject)rpt.ReportDefinition.ReportObjects["Text34"];
            TextObject txtRefNo = (TextObject)rpt.ReportDefinition.ReportObjects["Text37"];
            TextObject txtCabNo = (TextObject)rpt.ReportDefinition.ReportObjects["Text41"];
            TextObject txtDate = (TextObject)rpt.ReportDefinition.ReportObjects["Text39"];
            TextObject txtUser = (TextObject)rpt.ReportDefinition.ReportObjects["Text35"];


            txtCreditLimt.Text = tbCreditLimt.Text;
            txtNumOfDays.Text = tbNumOfDays.Text;
            txtTotIncome.Text = tbTotIncome.Text;
            txtTotDays.Text = tbNumOfDays.Text;
            txtTarget.Text = tbTraget.Text;
            txtAddIncome.Text = tbAddIncome.Text;
            txtYear.Text = tbYear.Text;
            txtMonth.Text = tbMonth.Text;
            txtPreDeduct.Text = tbPreDeduct.Text;
            txtPreRefund.Text = tbPreRefund.Text;
            txtCurentDeduct.Text = tbCurrentDeduct.Text;
            txtCurrentRefund.Text = tbCurrentRefund.Text;
            txtRefNo.Text = tbRecNo.Text;
            txtCabNo.Text = lbCabno.Text;
            txtDate.Text = lbDate.Text;
            txtUser.Text = lbUser.Text;

            rpt.SetDataSource(ds);
            f3.crystalReportViewer1.ReportSource = rpt;
            rpt.PrintToPrinter(1, false, 1, 1);

        }

    }



     
    
}
