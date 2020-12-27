using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Forms;


namespace WindowsFormsApplication3
{
  public  class NewReceiptNumber
    {
      private string constr = ConfigurationManager.ConnectionStrings["WindowsFormsApplication3.Properties.Settings.CabPaymentConnectionString1"].ConnectionString;
      private string receiptCode="";
      Taxi t1;


      public string get_Location()
      {
          return ConfigurationManager.AppSettings["Location"];
      }

      public string getReciptNo()//Get receipt no for Daily Payment
      {
          string location = get_Location();
          string recCode = location + "/DP/";

          try
          {

              System.Data.DataSet ds = new System.Data.DataSet();
              System.Data.DataTable dt = new System.Data.DataTable();
              MySqlConnection connection = new MySqlConnection(constr);
              connection.Open();
              MySqlCommand command = connection.CreateCommand();
              command.CommandText = "SELECT ReceiptNo FROM TestLocationPara WHERE Location='"+location+"'";
              MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
              newadp.Fill(ds);
              dt = ds.Tables[0];
              connection.Close();

              int code = Convert.ToInt32(dt.Rows[0]["ReceiptNo"]);
              code = code + 1;
              if (code <= 9)
              {
                  return receiptCode = recCode+"000000" + code.ToString();
              }
              if (code >= 10 && code <= 99)
              {
                  return receiptCode = recCode + "00000" + code.ToString();
              }
              if (code >= 100 && code <= 999)
              {
                  return receiptCode = recCode + "0000" + code.ToString();
              }
              if (code >= 1000 && code <= 9999)
              {
                  return receiptCode = recCode + "000" + code.ToString();
              }
              if (code >= 10000 && code <= 99999)
              {
                  return receiptCode = recCode + "00" + code.ToString();
              }
              if (code >= 100000 && code <= 999999)
              {
                  return receiptCode = recCode + "0" + code.ToString();
              }
              if (code >= 1000000 && code <= 9999999)
              {
                  return receiptCode = recCode +  code.ToString();
              }
              return receiptCode = "----";
          }
          catch (Exception ex)
          {
              return receiptCode = "----";
          }
      }

      public void updateReceiptNo(TextBox tbRecNo) //Update Daily Receipt Number
      {
          string location = get_Location();
          string s = tbRecNo.Text;        

          int recno = Convert.ToInt32( s.Substring(Math.Max(0, s.Length - 7))); //);
          //string[] split = s.Split(new string[] { "BA" }, StringSplitOptions.RemoveEmptyEntries);
          //int recno = Convert.ToInt32(split[0]);
          MySqlConnection connection = new MySqlConnection(constr);
          connection.Open();
          MySqlCommand command = connection.CreateCommand();
          command.CommandText = "UPDATE TestLocationPara SET ReceiptNo='" + recno + "' WHERE Location='"+location+"'";
          command.ExecuteNonQuery();
          connection.Close();
      }

      public string generateOtherReceiptNo()// Get Other Receipt Number
      {

          string location = get_Location();
          string recCode = location + "/OR/";

          //string receiptCode = "";
          try
          {
              System.Data.DataSet ds = new System.Data.DataSet();
              System.Data.DataTable dt = new System.Data.DataTable();

              MySqlConnection connection = new MySqlConnection(constr);
              connection.Open();
              MySqlCommand command = connection.CreateCommand();
              command.CommandText = "SELECT OtherRecNo FROM TestLocationPara WHERE Location='" + location + "'";
              MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
              newadp.Fill(ds);
              dt = ds.Tables[0];
              int code = Convert.ToInt32(dt.Rows[0]["OtherRecNo"]);
              code = code + 1;
              updateOtherReciept(code);
              connection.Close();


              code = code + 1;
              if (code <= 9)
              {
                  return receiptCode = recCode + "000000" + code.ToString();
              }
              if (code >= 10 && code <= 99)
              {
                  return receiptCode = recCode + "00000" + code.ToString();
              }
              if (code >= 100 && code <= 999)
              {
                  return receiptCode = recCode + "0000" + code.ToString();
              }
              if (code >= 1000 && code <= 9999)
              {
                  return receiptCode = recCode + "000" + code.ToString();
              }
              if (code >= 10000 && code <= 99999)
              {
                  return receiptCode = recCode + "00" + code.ToString();
              }
              if (code >= 100000 && code <= 999999)
              {
                  return receiptCode = recCode + "0" + code.ToString();
              }
              if (code >= 1000000 && code <= 9999999)
              {
                  return receiptCode = recCode + code.ToString();
              }

              return receiptCode = "----";
          }
          catch (Exception ex) { return receiptCode = "----"; }
      } 

      public void updateOtherReciept(int code) // Update Other Receipt 
      {
          string location = get_Location();

          MySqlConnection connection = new MySqlConnection(constr);
          connection.Open();
          MySqlCommand command = connection.CreateCommand();
          command.CommandText = "UPDATE TestLocationPara SET OtherRecNo='" + code + "' WHERE Location='"+location+"'";
          command.ExecuteNonQuery();
          connection.Close();
      }    

      public void clearAll(DataGridView dgv1, DataGridView dgv2, DataGridView dgv3, DateTimePicker dt,TextBox tb2,TextBox tb3,TextBox tb4,TextBox tb5,TextBox tb6,TextBox tb7,TextBox tb8,TextBox tb9, TextBox tb10,TextBox tb11, TextBox tb12 ,TextBox tb13,TextBox tb14,TextBox tb15,CheckBox chb1,TextBox tb17,TextBox tb18,CheckBox chb2,CheckBox chb3,TextBox tb24,TextBox tb25)
         
        {
            dgv1.DataSource=null;
            dgv2.Rows.Clear();
            dgv3.Rows.Clear();

            dt.Value = DateTime.Now;

            tb2.Enabled = true;
            tb6.Enabled = true;
            tb2.Text = "";
            tb3.Text = "0";
            tb4.Text = "0";
            tb5.Text = "0";
            tb6.Text = "0";
            tb7.Text = "0";
            tb8.Text = "";
            tb9.Text = "0";
            tb10.Text = "0";
            tb11.Text = "0";
            tb12.Text = "0";
            tb13.Text = "0";
            tb14.Text = "0";
            tb15.Text = "";
            tb17.Text = "";
            tb18.Text = "";
            tb24.Text = "0";
            tb25.Text = "0";
            chb1.Checked = false;
            chb2.Checked = false;
            chb3.Checked = false;
        }

      public string getPromotionRecNo()// Get Promotion receipt
      {
          string location = get_Location();
          string recCode = location + "/FD/";

          try
          {

              System.Data.DataSet ds = new System.Data.DataSet();
              System.Data.DataTable dt = new System.Data.DataTable();
              MySqlConnection connection = new MySqlConnection(constr);
              connection.Open();
              MySqlCommand command = connection.CreateCommand();
              command.CommandText = "SELECT PromoRecNo FROM TestLocationPara WHERE Location='"+location+"'";
              MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
              newadp.Fill(ds);
              dt = ds.Tables[0];
              connection.Close();

              int code = Convert.ToInt32(dt.Rows[0]["PromoRecNo"]);
              code = code + 1;
              if (code <= 9)
              {
                  return receiptCode = recCode + "000000" + code.ToString();
              }
              if (code >= 10 && code <= 99)
              {
                  return receiptCode = recCode + "00000" + code.ToString();
              }
              if (code >= 100 && code <= 999)
              {
                  return receiptCode = recCode + "0000" + code.ToString();
              }
              if (code >= 1000 && code <= 9999)
              {
                  return receiptCode = recCode + "000" + code.ToString();
              }
              if (code >= 10000 && code <= 99999)
              {
                  return receiptCode = recCode + "00" + code.ToString();
              }
              if (code >= 100000 && code <= 999999)
              {
                  return receiptCode = recCode + "0" + code.ToString();
              }
              if (code >= 1000000 && code <= 9999999)
              {
                  return receiptCode = recCode + code.ToString();
              }
              return receiptCode = "----";
          }
          catch (Exception ex)
          {
              return receiptCode = "----";
          }
      }

      public void updatePromoReceiptNo(TextBox tbRecNo) //Update Promotion Receipt
      {

          string location = get_Location();
          string s = tbRecNo.Text;

          int recno = Convert.ToInt32(s.Substring(Math.Max(0, s.Length - 7))); //);

          //string s = tbRecNo.Text;
          //string[] split = s.Split(new string[] { "FRBA" }, StringSplitOptions.RemoveEmptyEntries);
          //int recno = Convert.ToInt32(split[0]);
          MySqlConnection connection = new MySqlConnection(constr);
          connection.Open();
          MySqlCommand command = connection.CreateCommand();
          command.CommandText = "UPDATE TestLocationPara SET PromoRecNo='" + recno + "' WHERE Location='"+location+"'";
          command.ExecuteNonQuery();
          connection.Close();
      }
      
      public string generateVenturaRecNo()//Get ventura Receipt Number
      {
          string location = get_Location();
          string recCode = location + "/VN/";

          //string receiptCode = "";
          try
          {

              System.Data.DataSet ds = new System.Data.DataSet();
              System.Data.DataTable dt = new System.Data.DataTable();
              MySqlConnection connection = new MySqlConnection(constr);
              connection.Open();
              MySqlCommand command = connection.CreateCommand();
              command.CommandText = "SELECT VenRecNo FROM TestLocationPara WHERE Location='" + location +"'";
              MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
              newadp.Fill(ds);
              dt = ds.Tables[0];
              connection.Close();

              int code = Convert.ToInt32(dt.Rows[0]["VenRecNo"]);
              code = code + 1;
              if (code <= 9)
              {
                  return receiptCode = recCode + "000000" + code.ToString();
              }
              if (code >= 10 && code <= 99)
              {
                  return receiptCode = recCode + "00000" + code.ToString();
              }
              if (code >= 100 && code <= 999)
              {
                  return receiptCode = recCode + "0000" + code.ToString();
              }
              if (code >= 1000 && code <= 9999)
              {
                  return receiptCode = recCode + "000" + code.ToString();
              }
              if (code >= 10000 && code <= 99999)
              {
                  return receiptCode = recCode + "00" + code.ToString();
              }
              if (code >= 100000 && code <= 999999)
              {
                  return receiptCode = recCode + "0" + code.ToString();
              }
              if (code >= 1000000 && code <= 9999999)
              {
                  return receiptCode = recCode + code.ToString();
              }
              return receiptCode = "----";
          }
          catch (Exception ex)
          {
              return receiptCode = "----";
          }

      }

      public void updateVenturaRecNo(string receiptno) // Update Ventura Receipt
      {
          string location = get_Location();
          string s = receiptno;

          int recno = Convert.ToInt32(s.Substring(Math.Max(0, s.Length - 7))); //);
          //string[] split = s.Split(new string[] { "V" }, StringSplitOptions.RemoveEmptyEntries);
          //int recno = Convert.ToInt32(split[0]);
          MySqlConnection connection = new MySqlConnection(constr);
          connection.Open();
          MySqlCommand command = connection.CreateCommand();
          command.CommandText = "UPDATE TestLocationPara SET VenRecNo='" + recno + "' WHERE Location='"+location+"'";
          command.ExecuteNonQuery();
          connection.Close();
      }

      public string getVoucherRecNo()
      {
          string location = get_Location();
          string recCode = location + "/VC/";
          //string receiptCode = "";

          try
          {

              System.Data.DataSet ds = new System.Data.DataSet();
              System.Data.DataTable dt = new System.Data.DataTable();
              MySqlConnection connection = new MySqlConnection(constr);
              connection.Open();
              MySqlCommand command = connection.CreateCommand();
              command.CommandText = "SELECT VoucherRecNo FROM TestLocationPara WHERE Location='"+location+"'";
              MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
              newadp.Fill(ds);
              dt = ds.Tables[0];
              connection.Close();

              int code = Convert.ToInt32(dt.Rows[0]["VoucherRecNo"]);
              code = code + 1;
             
              if (code <= 9)
              {
                  return receiptCode = recCode + "000000" + code.ToString();
              }
              if (code >= 10 && code <= 99)
              {
                  return receiptCode = recCode + "00000" + code.ToString();
              }
              if (code >= 100 && code <= 999)
              {
                  return receiptCode = recCode + "0000" + code.ToString();
              }
              if (code >= 1000 && code <= 9999)
              {
                  return receiptCode = recCode + "000" + code.ToString();
              }
              if (code >= 10000 && code <= 99999)
              {
                  return receiptCode = recCode + "00" + code.ToString();
              }
              if (code >= 100000 && code <= 999999)
              {
                  return receiptCode = recCode + "0" + code.ToString();
              }
              if (code >= 1000000 && code <= 9999999)
              {
                  return receiptCode = recCode + code.ToString();
              }
              return receiptCode = "----";
          }
          catch (Exception ex)
          {
              return receiptCode = "----";
          }
      }

      public void updateVoucherRecNo(string vrecno)
      {

          string location = get_Location();
          string s = vrecno;

          int recno = Convert.ToInt32(s.Substring(Math.Max(0, s.Length - 7))); //);
          //string[] split = vrecno.Split(new string[] { "VR" }, StringSplitOptions.RemoveEmptyEntries);
          //int recno = Convert.ToInt32(split[0]);
          MySqlConnection connection = new MySqlConnection(constr);
          connection.Open();
          MySqlCommand command = connection.CreateCommand();
          command.CommandText = "UPDATE TestLocationPara SET VoucherRecNo='" + recno + "' WHERE Location='"+location+"'";
          command.ExecuteNonQuery();
          connection.Close();
      }


      public string getAppFineRecNo()
      {
          string location = get_Location();
          string recCode = location + "/AR/";
          //string receiptCode = "";

          try
          {

              System.Data.DataSet ds = new System.Data.DataSet();
              System.Data.DataTable dt = new System.Data.DataTable();
              MySqlConnection connection = new MySqlConnection(constr);
              connection.Open();
              MySqlCommand command = connection.CreateCommand();
              command.CommandText = "SELECT AppRefundRec FROM TestLocationPara WHERE Location='" + location + "'";
              MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
              newadp.Fill(ds);
              dt = ds.Tables[0];
              connection.Close();

              int code = Convert.ToInt32(dt.Rows[0]["AppRefundRec"]);
              code = code + 1;

              if (code <= 9)
              {
                  return receiptCode = recCode + "000000" + code.ToString();
              }
              if (code >= 10 && code <= 99)
              {
                  return receiptCode = recCode + "00000" + code.ToString();
              }
              if (code >= 100 && code <= 999)
              {
                  return receiptCode = recCode + "0000" + code.ToString();
              }
              if (code >= 1000 && code <= 9999)
              {
                  return receiptCode = recCode + "000" + code.ToString();
              }
              if (code >= 10000 && code <= 99999)
              {
                  return receiptCode = recCode + "00" + code.ToString();
              }
              if (code >= 100000 && code <= 999999)
              {
                  return receiptCode = recCode + "0" + code.ToString();
              }
              if (code >= 1000000 && code <= 9999999)
              {
                  return receiptCode = recCode + code.ToString();
              }
              return receiptCode = "----";
          }
          catch (Exception ex)
          {
              return receiptCode = "----";
          }
      }

      public void updateAppFineRecNo(string aprecno)
      {

          string location = get_Location();
          string s = aprecno;

          int recno = Convert.ToInt32(s.Substring(Math.Max(0, s.Length - 7))); //);
          //string[] split = vrecno.Split(new string[] { "VR" }, StringSplitOptions.RemoveEmptyEntries);
          //int recno = Convert.ToInt32(split[0]);
          MySqlConnection connection = new MySqlConnection(constr);
          connection.Open();
          MySqlCommand command = connection.CreateCommand();
          command.CommandText = "UPDATE TestLocationPara SET AppRefundRec='" + recno + "' WHERE Location='" + location + "'";
          command.ExecuteNonQuery();
          connection.Close();
      }

        //public string decideReceiptNo(TextBox tbRecno)
        //{
        //    t1 = new Taxi();
        //    string location = t1.getLocation();

        //    if (location == "H")
        //    {
        //        tbRecno.Text = getReciptNo();
        //        return tbRecno.Text;
        //    }
        //    if (location == "Y")
        //    {
        //        tbRecno.Text = getReciptNoYard();
        //        return tbRecno.Text;
        //    }
        //    return tbRecno.Text;
        //}

      //public string getReciptNo()//this is for Base
      //{
      //    try
      //    {

      //        System.Data.DataSet ds = new System.Data.DataSet();
      //        System.Data.DataTable dt = new System.Data.DataTable();
      //        MySqlConnection connection = new MySqlConnection(constr);
      //        connection.Open();
      //        MySqlCommand command = connection.CreateCommand();
      //        command.CommandText = "select ReceiptNo from TestPara";
      //        MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
      //        newadp.Fill(ds);
      //        dt = ds.Tables[0];
      //        connection.Close();

      //        int code = Convert.ToInt32(dt.Rows[0]["ReceiptNo"]);
      //        code = code + 1;
      //        if (code <= 9)
      //        {
      //            return receiptCode = "BA00000" + code.ToString();
      //        }
      //        if (code >= 10 && code <= 99)
      //        {
      //            return receiptCode = "BA0000" + code.ToString();
      //        }
      //        if (code >= 100 && code <= 999)
      //        {
      //            return receiptCode = "BA000" + code.ToString();
      //        }
      //        if (code >= 1000 && code <= 9999)
      //        {
      //            return receiptCode = "BA00" + code.ToString();
      //        }
      //        if (code >= 10000 && code <= 99999)
      //        {
      //            return receiptCode = "BA0" + code.ToString();
      //        }
      //        return receiptCode = "----";
      //    }
      //    catch (Exception ex)
      //    {
      //        return receiptCode = "----";
      //    }
      //}

        //public string getReciptNoYard()//this is for Yard
        //{
        //    try
        //    {

        //        System.Data.DataSet ds = new System.Data.DataSet();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        MySqlConnection connection = new MySqlConnection(constr);
        //        connection.Open();
        //        MySqlCommand command = connection.CreateCommand();
        //        command.CommandText = "select ReceiptNo from TestYardPara";
        //        MySqlDataAdapter newadp = new MySqlDataAdapter(command);//to retrive data (we can use data reader)
        //        newadp.Fill(ds);
        //        dt = ds.Tables[0];
        //        connection.Close();

        //        int code = Convert.ToInt32(dt.Rows[0]["ReceiptNo"]);
        //        code = code + 1;
        //        if (code <= 9)
        //        {
        //            return receiptCode = "RE00000" + code.ToString();
        //        }
        //        if (code >= 10 && code <= 99)
        //        {
        //            return receiptCode = "RE0000" + code.ToString();
        //        }
        //        if (code >= 100 && code <= 999)
        //        {
        //            return receiptCode = "RE000" + code.ToString();
        //        }
        //        if (code >= 1000 && code <= 9999)
        //        {
        //            return receiptCode = "RE00" + code.ToString();
        //        }
        //        if (code >= 10000 && code <= 99999)
        //        {
        //            return receiptCode = "RE0" + code.ToString();
        //        }
        //        return receiptCode = "----";
        //    }
        //    catch (Exception ex)
        //    {
        //        return receiptCode = "----";
        //    }
        //}

      //public void decideReciptUpdate(TextBox tbrecno)
      //{
      //    t1 = new Taxi();
      //    string location = t1.getLocation();

      //    if (location == "H")
      //        updateReceiptNo(tbrecno);
      //    if (location == "Y")
      //        updateReceiptNoYard(tbrecno);
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

       
    }
}
