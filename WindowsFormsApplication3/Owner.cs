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
    class Owner
    {      
        private static int code=0;
        private string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;

        public string getNewTaxi()
        {            
            string taxiNum = "";
            try
            {

                System.Data.DataSet ds = new System.Data.DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select TaxiNo from TestPara";
                
                using (var reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                      code = Convert.ToInt32(reader["TaxiNo"].ToString());
                    }
                }
                connection.Close();
            
                code = code + 1;
                if (code <= 9)
                {
                    return taxiNum = "K90" + code.ToString();
                }
                if (code >= 10 && code <= 99)
                {
                    return taxiNum = "K9" + code.ToString();
                }
                if (code >= 100 && code <= 999)
                {
                    return taxiNum = "K" + (900+code).ToString();
                }
               
                return taxiNum = "----";
            }
            catch (Exception ex)
            {
                return taxiNum = "----";
            }
        }

        public void hideDisableInfo(GroupBox gb1, GroupBox gb2)
        {
            gb1.Enabled = false;
            gb2.Visible = false;
        }

        public void enableOwnerInfo(GroupBox gb1)
        {
            gb1.Enabled = true;
        }

        public void viewDriverInfo(GroupBox gb2,ComboBox cbDrvtype) 
        {
            if (cbDrvtype.Text == "Driver")
            {
                gb2.Visible = true;
            }
            else if (cbDrvtype.Text == "Owner")
            {
                gb2.Visible = false;
            }
        }

        public void ClearTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();                   
                }
                if (c is DateTimePicker)
                {
                    ((DateTimePicker)c).Value = DateTime.Now;
                }
                if (c.HasChildren)
                {
                    ClearTextBoxes(c);
                }
            }
        }      

        public void save(bool status, TextBox txtTaxiNo,DateTimePicker dtOpDate,TextBox txtVhNo,TextBox txtYrManu,TextBox txtModel,TextBox txtOwName,TextBox txtOwAddrs,TextBox txtOwNIC, TextBox txtOwTpNo,TextBox txtOwMob,ComboBox txtDrvType,TextBox texComMobNo,TextBox txtDlNo,DateTimePicker dtLicExpDate,DateTimePicker dtInsExpDate,TextBox txtPayType,TextBox txtRate, TextBox txtDrvName,TextBox txtDrvAdrs,TextBox txtDrvTempAdrs,TextBox txtDrvNIC,TextBox txtDrvTpNo,TextBox txtDrvMobNo) 
        {
            string taxiNo=""; string opDate=""; string vhNo; string yrManu="";string model=""; string owName =""; string owadrs=""; string owNIC="";
            int owTpNo=0; int owMobNo=0; string DrType="";int comMobNo = 0; string DlNo=""; string licExpDate="";string insExpDate=""; 
            string payType=""; int rate =0;string drvName="" ;string drvAdrs="";string drvTempAdrs=""; string drvNIC="";int drvTpNo=0; int drvMobNo=0;           

            taxiNo=txtTaxiNo.Text;opDate=dtOpDate.Value.ToShortDateString(); vhNo=txtVhNo.Text; yrManu=txtYrManu.Text;
            model=txtModel.Text; owName=txtOwName.Text; owadrs=txtOwAddrs.Text; owNIC=txtOwNIC.Text;
            try { owTpNo = Convert.ToInt32(txtOwTpNo.Text); }catch (Exception) { }; try { owMobNo = Convert.ToInt32(txtOwMob.Text); }
            catch (Exception) { }; DrType = txtDrvType.Text; try { comMobNo = Convert.ToInt32(texComMobNo.Text); } catch(Exception){}; DlNo = txtDlNo.Text; licExpDate = dtLicExpDate.Value.ToShortDateString(); 
            insExpDate=dtInsExpDate.Value.ToShortDateString(); payType=txtPayType.Text; try{rate=Convert.ToInt32(txtRate.Text);}catch(Exception){}; 

           try{
                drvName= txtDrvName.Text ;drvAdrs=txtDrvAdrs.Text;drvTempAdrs=txtDrvTempAdrs.Text; drvNIC=txtDrvNIC.Text;
                drvTpNo = Convert.ToInt32(txtDrvTpNo.Text);  drvMobNo = Convert.ToInt32(txtDrvMobNo.Text);
           }
           catch (Exception) {};

            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            MySqlCommand command1 = connection.CreateCommand();
            if (status == false)
            {
                try
                {
                    command.CommandText = "INSERT INTO TestMaster VALUES ('" + taxiNo + "','" + opDate + "','" + vhNo + "','" + yrManu + "','" + model + "','" + owName + "','" + owadrs + "','" + owNIC + "','" + owTpNo + "','" + owMobNo + "','" + DrType + "','" + comMobNo + "','" + DlNo + "','" + licExpDate + "','" + insExpDate + "','" + payType + "','" + rate + "','" + drvName + "','" + drvAdrs + "','" + drvTempAdrs + "','" + drvNIC + "','" + drvTpNo + "','" + drvMobNo + "',0)";
                    command1.CommandText = "UPDATE TestPara SET TaxiNo='"+code+"' WHERE ID=1";                      
                    command.ExecuteNonQuery();
                    command1.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Saved");
                }
                catch (MySqlException myex) { MessageBox.Show( ""+myex.Message); }
            }
            else if(status == true) 
            {
                try
                {
                    command.CommandText = "UPDATE TestMaster SET OpDate= '" + opDate + "',VhNo ='" + vhNo + "',YrManu='" + yrManu + "',Model='" + model + "',OwName='" + owName + "',OwAdrs='" + owadrs + "',OwNIC='" + owNIC + "',OWTpNo='" + owTpNo + "',OwMobNo='" + owMobNo + "',DriverType='" + DrType + "',ComMobNo='" + comMobNo + "',DlNo='" + DlNo + "',LicExp='" + licExpDate + "',InsExp='" + insExpDate + "',Paytype='" + payType + "',Rate='" + rate + "',DrvName ='" + drvName + "',DrvAdrs='" + drvAdrs + "',DrvTemAdrs='" + drvTempAdrs + "',DrvNIC='" + drvNIC + "',DrvTpNo='" + drvTpNo + "',DrvMobNo='" + drvMobNo + "',Cancel=0  WHERE TaxiNo='" + taxiNo + "'";
                    command.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Upated");
                }catch(MySqlException myex) { MessageBox.Show( ""+myex.Message); }
            }

        }

        public void findDriver(string taxiNo, DateTimePicker dtOpDate, TextBox txtVhNo, TextBox txtYrManu, TextBox txtModel, TextBox txtOwName, TextBox txtOwAddrs, TextBox txtOwNIC, TextBox txtOwTpNo, TextBox txtOwMob, ComboBox txtDrvType, TextBox texComMobNo, TextBox txtDlNo, DateTimePicker dtLicExpDate, DateTimePicker dtInsExpDate, TextBox txtPayType, TextBox txtRate, TextBox txtDrvName, TextBox txtDrvAdrs, TextBox txtDrvTempAdrs, TextBox txtDrvNIC, TextBox txtDrvTpNo, TextBox txtDrvMobNo)
        {
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();

            command.CommandText = "SELECT * FROM TestMaster WHERE TaxiNo='" + taxiNo + "' and Cancel=0";           
            using (var reader = command.ExecuteReader())
            {
                
                    while (reader.Read())
                    {
                        dtOpDate.Value = Convert.ToDateTime(reader["OpDate"].ToString());

                        txtVhNo.Text = reader["VhNo"].ToString();
                        txtYrManu.Text = reader["YrManu"].ToString();
                        txtModel.Text = reader["Model"].ToString();
                        txtOwName.Text = reader["OwName"].ToString();
                        txtOwAddrs.Text = reader["OwAdrs"].ToString();
                        txtOwNIC.Text = reader["OwNIC"].ToString();
                        txtOwTpNo.Text = reader["OWTpNo"].ToString();
                        txtOwMob.Text = reader["OwMobNo"].ToString();
                        txtDrvType.Text = reader["DriverType"].ToString();
                        texComMobNo.Text = reader["ComMobNo"].ToString();
                        txtDlNo.Text = reader["DlNo"].ToString();
                        dtLicExpDate.Value = Convert.ToDateTime(reader["LicExp"].ToString());
                        dtInsExpDate.Value = Convert.ToDateTime(reader["InsExp"].ToString());
                        txtPayType.Text = reader["Paytype"].ToString();
                        txtRate.Text = reader["Rate"].ToString();
                        txtDrvName.Text = reader["DrvName"].ToString();
                        txtDrvAdrs.Text = reader["DrvAdrs"].ToString();
                        txtDrvTempAdrs.Text = reader["DrvTemAdrs"].ToString();
                        txtDrvNIC.Text = reader["DrvNIC"].ToString();
                        txtDrvTpNo.Text = reader["DrvTpNo"].ToString();
                        txtDrvMobNo.Text = reader["DrvMobNo"].ToString();
                    }
                
            }
            connection.Close();

        }

        public int CheckTaxiAvailability(TextBox tbTaxiNo) 
        {
            string taxi = "K" + tbTaxiNo.Text;
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT TaxiNo FROM TestMaster WHERE TaxiNo='" +taxi+ "'";
            using (var reader = command.ExecuteReader())
            {         
                if(reader.Read())
                 {
                    return 0;
                 }
                 else
                 {
                    MessageBox.Show("New Taxi.. Please Add Taxi Details");
                    return -1;
                 }
            }               
        }

        public void deleteTaxi(TextBox tbTaxi) 
        {
            DialogResult dr = MessageBox.Show("Are You Sure Want To Delete This Taxi","Delete",MessageBoxButtons.YesNoCancel);
            if (dr == DialogResult.Yes)
            {
                MySqlConnection connection = new MySqlConnection(constr);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE TestMaster SET Cancel=1 WHERE TaxiNo='" + tbTaxi.Text + "'";
                command.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Deleted");
            }
        }
}

}