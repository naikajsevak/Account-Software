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
    public partial class Rokad : Form
    {
        DataGridViewRow row;
        private int currentRowIndex = 0;
        public Rokad()
        {
            InitializeComponent();
        }
        void changeColor() 
        {
            if (comboBox2.Text == "Payment")
            {
                panel1.BackColor = Color.FromArgb(192, 255, 192);
                label8.Text = "Payment Voucher";
                if (CreditAmount.Text != "0.00" && CreditAmount.Text != "0")
                    DebitAmount.Text = CreditAmount.Text;
                CreditAmount.Text = "0.00";
                DebitAmount.Enabled = true;
                label.Enabled = true;
                CreditAmount.Enabled = false;
                label7.Enabled = false;
            }
            else
            {
                panel1.BackColor = Color.FromArgb(255, 255, 160);
                label8.Text = "Reciept Voucher";
                if (DebitAmount.Text != "0.00" && DebitAmount.Text != "0")
                    CreditAmount.Text = DebitAmount.Text;
                DebitAmount.Text = "0.00";
                DebitAmount.Enabled = false;
                label.Enabled = false;
                CreditAmount.Enabled = true;
                label7.Enabled = true;
            }
        }
      
        private void Rokad_Load(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
           
            comboBox2.Text = "Reciept";
            new DB_Handler().autoSuggestion("Select Name from BHET", comboBox1);
            new DB_Handler().autoSuggestion("Select Name from BHET", comboBox3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new DB_Handler().dataFatch("select * from Rokad where Date >='" + dateTimePicker1.Text + "' and Date<='" + dateTimePicker2.Text + "'", dataGridView1);
            dataGridView1.Columns["ID"].Visible = false;
            
            OleDbDataReader dr = new DB_Handler().select("select SUM(Credit) - Sum(Debit) From Rokad where Date<'"+dateTimePicker1.Text+"'");
            if (dr.Read())
                textBox1.Text = dr[0].ToString();
            dr = new DB_Handler().select("select SUM(Credit) - Sum(Debit) From Rokad where Date<='" + dateTimePicker2.Text + "'");
            if (dr.Read())
                textBox2.Text = dr[0].ToString();
        }       
        private void button2_Click(object sender, EventArgs e)
        {
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                MessageBox.Show("No Account Found");
                return;
            }
            new DB_Handler().Update_Delete("UPDATE Rokad SET Date='"+dateTimePicker3.Text+"',Particular='"+comboBox1.Text+"',Credit='"+CreditAmount.Text+"',Debit='"+DebitAmount.Text+"',Narration='"+Narration_Reach_Text_Box.Text+"' WHERE ID=" +  int.Parse(Id.Text));
            button4.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;  
            button1_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                MessageBox.Show("No Account Found");
                return;
            }
            new DB_Handler().Update_Delete("DELETE FROM Rokad WHERE ID="+int.Parse(Id.Text));
            button4.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button1_Click(sender, e);
        }

      

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            changeColor();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                MessageBox.Show("No Account Found");
                return;
            }
            List<string> st = new List<string>();
            st.Add(CreditAmount.Text);
            st.Add(DebitAmount.Text);
            if (!DB_Handler.inputValidate(st))
            {
                    MessageBox.Show("Input is not in correct format");
                    return;
            }
            new DB_Handler().insert("INSERT INTO Rokad ([Date],[Particular],[Credit],[Debit],[Narration]) VALUES ('" + dateTimePicker3.Text + "','" + comboBox1.Text + "','" + CreditAmount.Text + "','" + DebitAmount.Text + "','"+Narration_Reach_Text_Box.Text+"')");
            CreditAmount.Text = "0.00";
            DebitAmount.Text = "0.00";
            comboBox1.Text = "";
            Narration_Reach_Text_Box.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                row = dataGridView1.Rows[e.RowIndex];

                Id.Text = row.Cells["Id"].Value.ToString();
                dateTimePicker3.Text = row.Cells["Date"].Value.ToString();
                comboBox1.Text = row.Cells["Particular"].Value.ToString();
                CreditAmount.Text = row.Cells["Credit"].Value.ToString();
                DebitAmount.Text = row.Cells["Debit"].Value.ToString();
                Narration_Reach_Text_Box.Text = row.Cells["Narration"].Value.ToString();
                if (CreditAmount.Text == "0" || CreditAmount.Text == "0.00")
                    comboBox2.Text = "Payment";
                else
                    comboBox2.Text = "Reciept";

                button4.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                changeColor();
            }
        }

        private void comboBox3_KeyUp(object sender, KeyEventArgs e)
        {
            DB_Handler obj = new DB_Handler();
            obj.dataFatch("select * from Rokad where ID like '%" + comboBox3.Text + "%' or Date like '%" + comboBox3.Text + "%' or Particular like '%" + comboBox3.Text + "%' or Credit like '%" + comboBox3.Text + "%' or Debit like '%" + comboBox3.Text + "%' or Narration like '%"+comboBox3.Text+"%'", dataGridView1);
            dataGridView1.Columns["ID"].Visible = false;
            if (dataGridView1.Rows.Count > 1)
            {
                dataGridView1.Sort(dataGridView1.Columns["Date"], ListSortDirection.Ascending);
                DataGridViewRow firstRow = dataGridView1.Rows[0];
                int lastRowIndex = dataGridView1.Rows.Count - 1;
                DataGridViewRow lastRow = dataGridView1.Rows[lastRowIndex];
                dateTimePicker1.Text = firstRow.Cells["Date"].Value.ToString();
                dateTimePicker2.Text = lastRow.Cells["Date"].Value.ToString();
                OleDbDataReader dr = new DB_Handler().select("select SUM(Credit) - Sum(Debit) From Rokad where Date<='" + dataGridView1.Rows[0].Cells["Date"].Value.ToString() + "'");
                if (dr.Read())
                    textBox1.Text = dr[0].ToString();
                dr = new DB_Handler().select("select SUM(Credit) - Sum(Debit) From Rokad where Date<='" + dataGridView1.Rows[dataGridView1.Rows.Count - 2].Cells["Date"].Value.ToString() + "'");
                if (dr.Read())
                    textBox2.Text = dr[0].ToString();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            RectangleF printArea = e.PageSettings.PrintableArea;

            // Header information with enhanced styling
            e.Graphics.DrawString("Shrinathji Bhandar Porbandar", new Font("Arial", 24, FontStyle.Bold), Brushes.DarkBlue, new RectangleF(280, printArea.Top, printArea.Width, 50));

            // Decorative separator
            e.Graphics.DrawLine(Pens.Black, new PointF(printArea.Left, 110), new PointF(printArea.Right, 110));

            e.Graphics.DrawString("CaseBook ", new Font("Arial", 18, FontStyle.Bold), Brushes.Black, new PointF(400, 120));
            e.Graphics.DrawString("Date: " + dateTimePicker1.Text + "", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new PointF(printArea.Left + 10, 120));
            e.Graphics.DrawString("Date: " + dateTimePicker2.Text + "", new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new PointF(printArea.Right - 190, 120));

            int x = (int)printArea.Left;
            int y = 160; // Start y-coordinate below the header
            int inc = 40;

            // Draw the header for the items table with enhanced styling
            //e.Graphics.FillRectangle(Brushes.Brown, new Rectangle(x, y, (int)printArea.Width, inc));
            e.Graphics.DrawString("Opening Balance:-" + textBox1.Text + "", new Font("Arial", 18, FontStyle.Bold), Brushes.Black, new Rectangle(x, y, 350, inc), new StringFormat() { Alignment = StringAlignment.Center });

            y += inc;
            e.Graphics.DrawLine(Pens.MidnightBlue, new PointF(printArea.Left, y), new PointF(printArea.Right, y));

            // Variable to track the height available for drawing rows
            float pageHeight = e.MarginBounds.Height;
            float rowHeight = inc;
            float remainingHeight = pageHeight - y;

            // Loop through the rows and print them
            for (int i = currentRowIndex; i < dataGridView1.Rows.Count; i++)
            {
                if (remainingHeight < rowHeight)
                {
                    // No more space on this page, indicate there are more pages to print
                    e.HasMorePages = true;
                    currentRowIndex = i; // Save the current row index for the next page
                    return;
                }

                DataGridViewRow row = dataGridView1.Rows[i];
                y += inc;
                e.Graphics.DrawString(row.Cells["Date"].Value.ToString(), new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Rectangle(x, y, 250, inc), new StringFormat() { Alignment = StringAlignment.Center });
                e.Graphics.DrawString(row.Cells["Particular"].Value.ToString(), new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Rectangle(x + 300, y, 150, inc), new StringFormat() { Alignment = StringAlignment.Center });
                e.Graphics.DrawString(row.Cells["Credit"].Value.ToString(), new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Rectangle(x + 500, y, 150, inc), new StringFormat() { Alignment = StringAlignment.Center });
                e.Graphics.DrawString(row.Cells["Debit"].Value.ToString(), new Font("Arial", 14, FontStyle.Regular), Brushes.Black, new Rectangle(x + 650, y, 200, inc), new StringFormat() { Alignment = StringAlignment.Center });

                y += inc;
                e.Graphics.DrawLine(Pens.MidnightBlue, new PointF(printArea.Left, y), new PointF(printArea.Right, y));

                remainingHeight -= (rowHeight);
            }

            // Footer with enhanced styling
            y += inc;
            e.Graphics.DrawString("Closing Balance : " + textBox2.Text, new Font("Arial", 18, FontStyle.Bold), Brushes.Black, new PointF(x + 320, y));

            // Indicate that there are no more pages to print
            e.HasMorePages = false;
            currentRowIndex = 0; // Reset the row index for the next print job
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }  
    }
}
