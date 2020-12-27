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
    public partial class FrmVoucherRecFind : Form
    {
        public FrmVoucherRecFind()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            vr.findVoucherReceipt(textBox1, dataGridView1);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Voucher vr = new Voucher();
                vr.findVoucherReceipt(textBox1, dataGridView1);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString();
            //textBox4.Text = dataGridView1.CurrentRow.Index.ToString();
            Voucher vr = new Voucher();
            vr.printSelectedVoucherReceipt(textBox3.Text, textBox2, textBox3);
           // vr.printSelectedVoucherReceipt(dataGridView1.Rowtos[dataGridView1.vr.printSelectedVoucherReceipt(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString());.Index].Cells[1].Value.ToString(), textBox2, textBox3);
           // vr.printSelectedVoucherReceipt(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Voucher vr = new Voucher();
            vr.printSelectedVoucherReceipt(textBox3.Text,textBox2,textBox3);
        }   
        
    }
}
