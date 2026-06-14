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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string q = "SELECT * from Admin";
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter UserName","Information",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                OleDbDataReader ds = new DB_Handler().select(q);
                
                if (ds.Read())
                {
                    if (textBox1.Text == (ds[1].ToString()) && textBox2.Text == (ds[2].ToString()))
                    {
                        new HomeScreen().ShowDialog();
                        this.Hide();
                    }
                    else
                        MessageBox.Show("Invalid user");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new reset().Show();
        }
    }
}
