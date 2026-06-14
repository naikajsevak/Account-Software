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
    public partial class Alter : Form
    {
        public Alter()
        {
            InitializeComponent();
        }
        public void reset()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
       

        private void Manorath_Load(object sender, EventArgs e)
        {
            new DB_Handler().autoSuggestion("Select Name from BHET", comboBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {

           
                new DB_Handler().Update_Delete("UPDATE BHET SET Name='" + textBox1.Text + "',OpeningBalance='" + textBox2.Text + "'  WHERE Name='" + comboBox1.Text + "'");

                new DB_Handler().Update_Delete("UPDATE Rokad SET Particular='" + textBox1.Text + "' where Particular='" + comboBox1.Text + "'");
                new DB_Handler().Update_Delete("UPDATE Manorath SET Name='" + textBox1.Text + "' where Name='" + comboBox1.Text + "'");
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Skip the new row placeholder if present
                    if (row.IsNewRow) continue;


                    string item = row.Cells["Name"].Value.ToString();
                    string itemWeight = row.Cells["Weight"].Value.ToString();

                    new DB_Handler().Update_Delete("update BHET set '" + item + "'='" + itemWeight + "' where Name='" + textBox1.Text + "'");
                }
                comboBox1.Items.Remove(comboBox1.Text);
                comboBox1.Items.Add(textBox1.Text);
                reset();
                MessageBox.Show("Ledger Altered Successfull");
            
        }
        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            textBox1.Text = comboBox1.Text;
            new DB_Handler().dataFatch("Select Name From Items", dataGridView1);
            DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
            newColumn.Name = "Weight";
            newColumn.HeaderText = "Item Weight";
            dataGridView1.Columns.Add(newColumn);

            // Optionally, add some data to the new column for existing rows
            int c = dataGridView1.Rows.Count - 1,ind=3;
            DB_Handler obj = new DB_Handler();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (c == 0)
                    break;
                OleDbDataReader dr =obj.select("select * from BHET where Name='" + comboBox1.Text + "'");
                if(dr.Read())
                    row.Cells["Weight"].Value = dr[ind];
                ind++;
                c--;
            }
            obj.con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new DB_Handler().Update_Delete("Delete from Manorath where Name='" + comboBox1.Text + "'");
            new DB_Handler().Update_Delete("Delete from Rokad where Particular='" + comboBox1.Text + "'");
            new DB_Handler().Update_Delete("Delete from BHET where Name='" + comboBox1.Text + "'");
        }
    }
}
