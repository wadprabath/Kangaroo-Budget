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
    public partial class Form1 : Form
    {
        string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
        MySqlConnection connection;
        MySqlDataAdapter adapter;
        DataTable DTItems;

        private void frmMySqlSample_Load(object sender, EventArgs e)
        {
            //Initialize mysql connection
            connection = new MySqlConnection(constr);

            //Get all items in datatable
            DTItems = GetAllItems();

            //Fill grid with items
            dataGridView1.DataSource = DTItems;
        }

      public   DataTable GetAllItems()
        {
            try
            {
                //prepare query to get all records from items table
                string query = "select * from MobitelBill";
                //prepare adapter to run query
                adapter = new MySqlDataAdapter(query, connection);
                DataSet DS = new DataSet();
                //get query results in dataset
                adapter.Fill(DS);
                return DS.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;

        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialize mysql connection
            connection = new MySqlConnection(constr);

            //Get all items in datatable
            DTItems = GetAllItems();

            //Fill grid with items
            dataGridView1.DataSource = DTItems;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
