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
    public partial class AlterItems : Form
    {
        public AlterItems()
        {
            InitializeComponent();
        }

        private void comboBox2_KeyUp(object sender, KeyEventArgs e)
        {
            DB_Handler obj = new DB_Handler();
            if (comboBox2.Text.Trim() != "")
            {
                OleDbDataReader dr = obj.select("select * from Items where Name='" + comboBox2.Text + "'");
                if (dr.Read())
                {
                    comboBox1.Text = dr["Unit"].ToString();
                    textBox3.Text = dr["Rate"].ToString();
                    textBox2.Text = dr["OpeningStock"].ToString();
                }
            }
            obj.con.Close();
        }

        private void AlterItems_Load(object sender, EventArgs e)
        {
            new DB_Handler().autoSuggestion("select Name from Items", comboBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!comboBox2.Items.Contains(comboBox2.Text))
            {
                MessageBox.Show("No Item Found");
                return;
            }
            if(textBox1.Text.Trim()!="")
                new DB_Handler().Update_Delete("update Items set Name='"+textBox1.Text+"',Unit='"+comboBox1.Text+"',Rate='"+textBox3.Text+"',OpeningStock='"+textBox2.Text+"' where Name='"+comboBox2.Text+"'");
            else
                new DB_Handler().Update_Delete("update Items set Name='" + comboBox2.Text + "',Unit='" + comboBox1.Text + "',Rate='" + textBox3.Text + "',OpeningStock='" + textBox2.Text + "'  where Name='" + comboBox2.Text + "'");
            new DB_Handler().autoSuggestion("select Name from Items", comboBox2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!comboBox2.Items.Contains(comboBox2.Text))
            {
                MessageBox.Show("No Item Found");
                return;
            }
            new DB_Handler().Update_Delete("Delete from Items where Name='" + comboBox2.Text + "'");
            new DB_Handler().Update_Delete("Delete from Manorath where Name='" + comboBox2.Text + "'");
            new DB_Handler().Update_Delete("ALTER TABLE Manorath DROP COLUMN ['" + comboBox2.Text + "']");
            new DB_Handler().Update_Delete("ALTER TABLE BHET DROP COLUMN ['"+comboBox2.Text+"']");
            new DB_Handler().autoSuggestion("select Name from Items", comboBox2);
        }
    }
}
