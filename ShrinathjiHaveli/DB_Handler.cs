using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace ShrinathjiHaveli
{
    class DB_Handler
    {
       
        public OleDbConnection con;
     
        public DB_Handler()
        {
            con = new OleDbConnection();
            con.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\teduguda\OneDrive\Documents\Shrinathji.accdb";
            con.Open();
        }
        public static bool inputValidate(List<string> listOfString)
        {
            for (int i = 0; i < listOfString.Count; i++)
            {
                string input = listOfString[i];
                for (int j = 0; j < input.Length; j++)
                {
                    if ((input[j] < '0' || input[j] > '9') && input[j] != '.')
                        return false;
                }
            }
            return true;
        }
        public void autoSuggestion(string q, ComboBox comboBox1)
        {
            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            DataTable NameTable=null;
            try
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(q, con);
                NameTable = new DataTable();
                adapter.Fill(NameTable);

                // Populate ComboBox with data from the database
                comboBox1.Items.AddRange(NameTable.AsEnumerable().Select(row => row["Name"].ToString()).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message);
            }
            con.Close();
        }
        public static string FormatWeight(double weightInGrams, string unit)
        {
            if (unit.ToLower() == "kg")
            {
                weightInGrams *= 1000;
                if (weightInGrams < 1000)
                    return "g";
                else if (weightInGrams < 1000000)
                    return "kg";
                else
                    return "t";
            }
            return unit;
        }
        public void dataFatch(string q,DataGridView dgv)
        {
            try
            {
                OleDbDataAdapter da = new OleDbDataAdapter(q,con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgv.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            con.Close();
        }
        public OleDbDataReader select(string q)
        {
            OleDbDataReader dr=null;
            try
            {
                OleDbCommand cmd = new OleDbCommand(q, con);
                cmd.CommandType = CommandType.Text;
                dr = cmd.ExecuteReader(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return dr;
        }
        public void Update_Delete(string q)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand(q, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
            con.Close();
        }
       
        public int insert(string q)
        {
            int lastInsertedId = -1;
            try
            {
                OleDbCommand cmd = new OleDbCommand(q, con);
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT @@IDENTITY";
                lastInsertedId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
               
            }
            con.Close();
            return lastInsertedId;
        }
    }
}
