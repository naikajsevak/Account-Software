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
    public partial class reset : Form
    {
        OleDbConnection con;
        public reset()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string q = "SELECT * from Admin";

            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter Old Password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Enter New password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DB_Handler obj = new DB_Handler();
                OleDbDataReader ds = obj.select(q);

                if (ds.Read())
                {
                    if (textBox1.Text == (ds[2].ToString()))
                        new DB_Handler().Update_Delete("update Admin set password='"+textBox2.Text+"' where password"+textBox1.Text);
                    else
                        MessageBox.Show("Invalid Old Password");
                }
                obj.con.Close();
            }
        }
    }
}
