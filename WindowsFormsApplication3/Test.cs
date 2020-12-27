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
    public partial class Test : Form
    {

        string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        //MySqlConnection connection;
        //MySqlDataAdapter adapter;
        //DataTable DTItems;

        public Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataRetrive(); 

           // //Initialize mysql connection
           // connection = new MySqlConnection(constr);

           // //Get all items in datatable
           // DTItems = GetAllItems();
           //// DTItems.;
           // //Fill grid with items
           // dataGridView1.DataSource = DTItems;
        }

        //public DataTable GetAllItems()
        //{
        ////    try
        ////    {
        //        //prepare query to get all records from items table
        //        //string query = "select * from MobitelBill where CabNo='000' and nMonth=6 and PAY=0";

        //        //string query = "select * from MobitelBill where CabNo='000' and nMonth='6' and PAY='0'";
        //        string query = "if exists(select * from MobitelBill where CabNo='000' and nMonth='6' and PAY='0') select 1;	else select 0;"; 

        //        if (query == "0")
        //        {
        //            MessageBox.Show("Null");
        //        }
        //        //prepare adapter to run query
        //        adapter = new MySqlDataAdapter(query, connection);
        //        DataSet DS = new DataSet();
        //        //get query results in dataset
        //        adapter.Fill(DS);
        //        return DS.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    return null;
        //}


        public void dataRetrive() 
        {
            DataSet ds = new DataSet();
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            //MySqlDataReader Reader;
            // command.CommandText = "select * from MobitelBill";select (1) else select (0)

            command.CommandText = "select count(*) from MobitelBill where CabNo='000'";    
        
            int rv = Convert.ToInt32(command.ExecuteScalar());
            //MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
            //newadp.Fill(ds);
            //connection.Close();
            //dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }
    }
}
