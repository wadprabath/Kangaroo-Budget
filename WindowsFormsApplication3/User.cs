using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.Types;

namespace WindowsFormsApplication3
{
  
    class User
    {
        public static string currentUser="";
        public static string branchCode = "";
        public static string branchName = "";

        private string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;

        private string constr2 = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.AmilaConnectionString"].ConnectionString;
        
        public bool rv = false;

        public void IsValidatedUser(string username, string password,Form f4)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(constr);
                MySqlConnection connection2 = new MySqlConnection(constr2);
                connection.Open();
                connection2.Open();
                MySqlCommand command = connection.CreateCommand();
                MySqlCommand command2 = connection2.CreateCommand();
                //command.CommandText = "if exists (select * from TestUser where UserName='" + username + "' and  Password='" + password + "' )select 1;else select 0; ";
                //command.CommandText = "SELECT * FROM TestUser WHERE UserName='" + username + "' AND  Password='" + password + "' ";
                // rv = Convert.ToBoolean(command.ExecuteScalar());

                command2.CommandText = "SELECT UserAccess.Uname as userName, branch_code.id as branchId ,branch_code.branch_name as branchName FROM `UserAccess` INNER JOIN branch_code on UserAccess.branch_code=branch_code.id WHERE UserAccess.Uname='" + username + "' AND UserAccess.Psword='" + password + "' ";               
                //string s = (string)(command.ExecuteScalar());
             
                //connection.Close();
                 using (var reader = command2.ExecuteReader())
                {
                if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            currentUser = reader["userName"].ToString();
                            branchCode = reader["branchId"].ToString();
                            branchName = reader["branchName"].ToString();
                            
                        }
                    currentUser = username;
                    //connection.Open(); 
                    command.CommandText = "UPDATE TestPara SET CUser='" + username + "' WHERE ID=0";
                    command.ExecuteNonQuery();
                    connection.Close();
                    Form10 f10 = new Form10();
                    f10.Show();
                    //Form5 f5 = new Form5();
                    //f5.Show(); 

                     f4.Hide();
                     connection.Close();
                     loginTime(DateTime.Now, currentUser);
                    SystemLog(DateTime.Now, currentUser, "Loged");
                }
                else
                {
                    MessageBox.Show("Invalid User Name or Password, Please Try Again");
                    SystemLog(DateTime.Now,username, "Loging Faild");
                }
                 }
                 connection2.Close();
            }
            catch (Exception ex)
            {
                // Log errors
                throw;
                MessageBox.Show("Pleace check the Database Conection");
            }
           
        }

        public string getCurrentUser()
        {
            return currentUser;
        }

        public string getBranchCode() 
        {
            return branchCode;
        }
        public string getBranchName() 
        {
            return branchName;
        }

        public void SystemLog(DateTime dt,string userName,String action) 
        {
            string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt);
           
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO TestSystemLog VALUES ('"+datetime+"','"+userName+"','"+action+"')";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void loginTime(DateTime dt, string userName)
        {
            string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt);

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE TestUser  SET LoginTime='" + datetime + "',status='0' WHERE (UserName='" + userName + "') AND (status ='1')";
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void logoutTime(DateTime dt, string userName,DateTimePicker dtLogOut,DateTimePicker dtLogin,Button btn11)
        {
            DialogResult dr = MessageBox.Show("Are You Sure Want To Log Out", "Log Out", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                dtLogOut.Value = DateTime.Now;
                string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt);
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE TestUser  SET LogoutTime='" + datetime + "',status='1' WHERE UserName='" + userName + "'";
                command.ExecuteNonQuery();
                connection.Close();
                btn11.Enabled = true;
                getLoginTime(userName, dtLogin);
            }
        }

        public void getLoginTime(string userName, DateTimePicker dtLogin)
        {
              
                //string datetime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", dt);
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT LoginTime FROM TestUser WHERE UserName='"+userName+"'";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        string date = reader["LoginTime"].ToString();
                        dtLogin.Value = Convert.ToDateTime(date);
                    }                  
                    
                }
                connection.Close();
        }

        public void SystemLogDisplay(RichTextBox rtb) 
        {          

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM TestSystemLog";
            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {           
                    string date = reader["dateAndTime"].ToString();
                    string user = reader["userName"].ToString();
                    string action = reader["Action"].ToString();
                    string fullText = user + "\t\t" + action + "        "+date+"\n";
                    rtb.AppendText(fullText);               
                }
            }
        }

        public void conectionChecker() 
        {            
                MySqlConnection connection = new MySqlConnection(constr);
                try
                {
                    connection.Open();
                }
                catch (MySqlException ex) { MessageBox.Show("" + ex.Message); Application.Exit(); }
                connection.Close();            
        }
    }
}
