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
    public partial class AccountsDetails : Form
    {

        public AccountsDetails()
        {
            InitializeComponent();
        }


        private void AccountsDetails_Load(object sender, EventArgs e)
        {
            new DB_Handler().autoSuggestion("Select Name from BHET", comboBox1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Account_Name.Text = comboBox1.Text;
            new DB_Handler().dataFatch("select Date,Credit,Debit from Rokad where Particular='" + comboBox1.Text + "' and Date Between '"+dateTimePicker1.Text+"' and '"+dateTimePicker2.Text+"'", dataGridView1);
            OleDbDataReader dr = new DB_Handler().select("select OpeningBalance from BHET where Name='"+Account_Name.Text+"'");
            string s="";
            if (dr.Read())
                s = dr[0].ToString();
            dr = new DB_Handler().select("select SUM(Credit) - Sum(Debit) From Rokad where Particular='"+comboBox1.Text+"' and Date<'" + dateTimePicker1.Text + "'");
            if (dr.Read())
            {
                if (dr[0].ToString() != "" && s!="")
                    op_bal.Text = (Convert.ToDouble(dr[0].ToString()) + Convert.ToDouble(s)) + "";
                else
                    op_bal.Text = s!=""?s:dr[0].ToString()!=""?dr[0].ToString():"0.00";
            }
            dr = new DB_Handler().select("select SUM(Credit) - Sum(Debit) From Rokad where Particular='" + comboBox1.Text + "' and (Date>='" + dateTimePicker1.Text + "' and Date<='" + dateTimePicker2.Text + "')");
            if (dr.Read())
            {
                if (dr[0].ToString() != "" && s!="")
                    cb_bal.Text = (Math.Abs(Convert.ToDouble(dr[0].ToString())) + Convert.ToDouble(s)) + "";
                else
                    cb_bal.Text = dr[0].ToString()!=""?dr[0].ToString():s;
            }
        }
    }
}
