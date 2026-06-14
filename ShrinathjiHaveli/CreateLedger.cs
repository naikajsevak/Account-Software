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
    public partial class CreateLedger : Form
    {
        public CreateLedger()
        {
            InitializeComponent();
        }
        void reset()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string s;
            if (textBox2.Text == "")
                s = "0.00";
            else
                s = textBox2.Text;

            if (textBox1.Text.Trim() != "")
            {
                    string q = "INSERT INTO BHET ([Name],[OpeningBalance]) VALUES ('" + textBox1.Text + "','" + s + "')";
                    int id=new DB_Handler().insert(q);
                    if (id != -1)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Skip the new row placeholder if present
                            if (row.IsNewRow) continue;


                            string item = row.Cells["Name"].Value.ToString();
                            string itemWeight = row.Cells["Weight"].Value.ToString();

                            new DB_Handler().Update_Delete("update BHET set '" + item + "'='" + itemWeight + "' where Name='" + textBox1.Text + "'");

                        }
                        MessageBox.Show("Ledger created Successfully");
                    }
                    else
                        MessageBox.Show("Duplicate Account");
            }
            else
            {
                MessageBox.Show("All Field are mandentory");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reset();
        }
        private void load()
        {
            new DB_Handler().dataFatch("Select Name From Items", dataGridView1);
            DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
            newColumn.Name = "Weight";
            newColumn.HeaderText = "Item Weight";
            dataGridView1.Columns.Add(newColumn);

            // Optionally, add some data to the new column for existing rows
            int c = dataGridView1.Rows.Count - 1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (c == 0)
                    break;
                row.Cells["Weight"].Value = "0.000";
                c--;
            }
        }
        private void CreateLedger_Load(object sender, EventArgs e)
        {
            load();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Check if Ctrl+S is pressed
            if (keyData == (Keys.Control | Keys.C))
            {
                new CreateItems().Show();
                load();
                //return true; // Indicate that the key press was handled
            }
            return base.ProcessCmdKey(ref msg, keyData); // Call the base class method
        }
    }
}
