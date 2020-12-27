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
    public partial class Form9 : Form
    {
        Owner ow;
        private bool status=false;
       
        public Form9()
        {
            InitializeComponent();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            ow = new Owner();
            ow.hideDisableInfo(groupBox1, groupBox2);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            status = false;
            ow = new Owner();
            ow.enableOwnerInfo(groupBox1);           
            ow.ClearTextBoxes(this);

            txtTaxiNo.Text=ow.getNewTaxi();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ow = new Owner();
            ow.save(status,txtTaxiNo, dtOpDate, txtVhiNo, txtManuYear, txtModel, txtOwName, txtOwAddrs, txtOwNIC, TxtOwTpNo, txtOwMobNo, cmbDriver, txtOwComMob, txtDlNo, dtLicExpDate, dtInsExpDate, txtPaytype, TxtRate, txtDrvName, txtDrvAddrs, txtDrvTempAdrs, txtDrvNIC, txtDrvTpNo,txtDrvMobNo);
        }

        private void cmbDriver_SelectedValueChanged(object sender, EventArgs e)
        {
            ow = new Owner();
            ow.viewDriverInfo(groupBox2, cmbDriver);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ow = new Owner();
            ow.findDriver(txtTaxiNo.Text, dtOpDate, txtVhiNo, txtManuYear, txtModel, txtOwName, txtOwAddrs, txtOwNIC, TxtOwTpNo, txtOwMobNo, cmbDriver, txtOwComMob, txtDlNo, dtLicExpDate, dtInsExpDate, txtPaytype, TxtRate, txtDrvName, txtDrvAddrs, txtDrvTempAdrs, txtDrvNIC, txtDrvTpNo, txtDrvMobNo);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            status = true;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            ow = new Owner();
            ow.save(true, txtTaxiNo, dtOpDate, txtVhiNo, txtManuYear, txtModel, txtOwName, txtOwAddrs, txtOwNIC, TxtOwTpNo, txtOwMobNo, cmbDriver, txtOwComMob, txtDlNo, dtLicExpDate, dtInsExpDate, txtPaytype, TxtRate, txtDrvName, txtDrvAddrs, txtDrvTempAdrs, txtDrvNIC, txtDrvTpNo, txtDrvMobNo);
        }

        private void txtTaxiNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                ow = new Owner();
                ow.findDriver(txtTaxiNo.Text, dtOpDate, txtVhiNo, txtManuYear, txtModel, txtOwName, txtOwAddrs, txtOwNIC, TxtOwTpNo, txtOwMobNo, cmbDriver, txtOwComMob, txtDlNo, dtLicExpDate, dtInsExpDate, txtPaytype, TxtRate, txtDrvName, txtDrvAddrs, txtDrvTempAdrs, txtDrvNIC, txtDrvTpNo, txtDrvMobNo);
                dtOpDate.Focus();
            }
        }

        private void dtOpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtVhiNo.Focus();
            }
        }

        private void txtVhiNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtManuYear.Focus();
            }
        }

        private void txtManuYear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtModel.Focus();
            }
        }

        private void txtModel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtOwName.Focus();
            }
        }

        private void txtOwName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtOwAddrs.Focus();
            }
        }

        private void txtOwAddrs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtOwNIC.Focus();
            }
        }

        private void txtOwNIC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                TxtOwTpNo.Focus();
            }
        }

        private void TxtOwTpNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtOwMobNo.Focus();
            }
        }

        private void txtOwMobNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                cmbDriver.Focus();
            }
        }

        private void cmbDriver_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                if (cmbDriver.Text == "Driver")
                {
                    txtDrvName.Focus();
                }
                else
                {
                    txtOwComMob.Focus();
                }
            }
        }

        private void txtOwComMob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDlNo.Focus();
            }
        }

        private void txtDlNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                dtLicExpDate.Focus();
            }
        }

        private void dtLicExpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                dtInsExpDate.Focus();
            }
        }

        private void dtInsExpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtPaytype.Focus();
            }
        }

        private void txtPaytype_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                TxtRate.Focus();
            }
        }

        private void txtDrvName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDrvAddrs.Focus();
            }
        }

        private void txtDrvAddrs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDrvTempAdrs.Focus();
            }
        }

        private void txtDrvTempAdrs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDrvNIC.Focus();
            }
        }

        private void txtDrvNIC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtDrvTpNo.Focus();
            }
        }

        private void txtDrvTpNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
               txtDrvMobNo.Focus();
            }
        }

        private void txtDrvMobNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                txtOwComMob.Focus();
            }
        }

        private void TxtRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
               
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
             ow = new Owner();
             ow.deleteTaxi(txtTaxiNo);
            
        }

        private void toolStripButton5_Click_1(object sender, EventArgs e)
        {
            ow=new Owner();
            ow.CheckTaxiAvailability(txtTaxiNo);
        }
       
               
    }
}
