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
    public partial class Form2 : Form
    {
        public bool paid = false;
        public int amount = 0;
        public DateTime tempdate;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int shortAmount = Convert.ToInt32(label3.Text);
            int paidAmount = Convert.ToInt32(textBox1.Text);
            if(shortAmount==paidAmount)
            {
                //Recipt r2=new Recipt();
                //r2.newmonthbilwithgrid();
               
            }
        }
    }
}
