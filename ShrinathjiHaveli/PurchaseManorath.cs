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
    public partial class PurchaseManorath : Form
    {
        DataGridViewRow row=null;
        public PurchaseManorath()
        {
            InitializeComponent();
        }

        private void PurchaseManorath_Load(object sender, EventArgs e)
        {
            new DB_Handler().autoSuggestion("select Name from BHET", comboBox1);
            new DB_Handler().autoSuggestion("select Name from Items", comboBox2);
            new DB_Handler().autoSuggestion("select Name from Items", comboBox3);
            comboBox1_KeyUp(sender, new KeyEventArgs(Keys.Tab));
            comboBox2_KeyUp(sender, new KeyEventArgs(Keys.Tab));
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            DB_Handler obj = new DB_Handler();
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                MessageBox.Show("Account Not Found");
                return;
            }
            int id = new DB_Handler().insert("INSERT INTO Manorath ([Date],[Name],[Count]) VALUES ('"+dateTimePicker1.Text+"','"+comboBox1.Text+"','"+textBox1.Text+"')");
            for (int i = 0, j = 3; i < comboBox2.Items.Count; i++, j++)
            {
                  OleDbDataReader dr = obj.select("select * from BHET where Name='" + comboBox1.Text + "'");
                  if (dr.Read())
                  {
                      if (dr[j].ToString() != "")
                      {
                          OleDbDataReader wt = obj.select("select SUM(Weight) from Manorath where Name ='" + comboBox2.Items[i].ToString() + "'");
                          OleDbDataReader dr1 = obj.select("select SUM(['" + comboBox2.Items[i].ToString() + "']) from Manorath");
                          OleDbDataReader op = obj.select("select OpeningStock,Unit from Items where Name='" + comboBox2.Items[i].ToString() + "'");
                          double cb=0;
                          if (wt.Read())
                          {
                              string a = wt[0].ToString();
                              if (dr1.Read())
                              {
                                  string b = dr1[0].ToString();
                                  if (op.Read())
                                  {
                                      if (a == "")
                                          a = "0";
                                      if (b == "")
                                          b = "0";
                                       cb = (Convert.ToDouble(a) + Convert.ToDouble(op[0].ToString())) - Convert.ToDouble(b);
                                  }
                              }
                          }
                          if (cb < Convert.ToDouble(dr[j].ToString()) * Convert.ToDouble(textBox1.Text) && op[1].ToString().ToLower()=="kg")
                          {
                              new DB_Handler().Update_Delete("delete from Manorath  where ID=" + id);
                              MessageBox.Show(comboBox2.Items[i].ToString() + " is out of stock");
                              return;
                          }
                          new DB_Handler().Update_Delete("update Manorath  set ['" + comboBox2.Items[i].ToString() + "']= ('" + Convert.ToDouble(dr[j].ToString()) * Convert.ToDouble(textBox1.Text) + "') where ID=" + id);
                      }
                  }
             }
            obj.con.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            double res = 0;
            if (comboBox2.Items.Contains(comboBox2.Text))
            {
                 if (!double.TryParse(textBox4.Text, out  res))
                 {
                      new DB_Handler().insert("insert into Manorath ([Date],[Name],[Weight]) Values ('" + dateTimePicker2.Text + "','" + comboBox2.Text + "','" + textBox2.Text + "')");
                 }
                 else
                      MessageBox.Show("invalid format");
             }
             else
                    MessageBox.Show("No items found");
        } 
       
        private void button3_Click(object sender, EventArgs e)
        {
            DB_Handler obj = new DB_Handler();
            if (comboBox3.Items.Contains(comboBox3.Text))
            {
                new DB_Handler().dataFatch("Select ID,Date,Name as Particular,Count,Weight as InWard,['" + comboBox3.Text + "'] as OutWard from Manorath where (Name='" + comboBox3.Text + "' or ['" + comboBox3.Text + "'] <> '0') and Date>='" + dateTimePicker4.Text + "' and Date<='" + dateTimePicker3.Text + "'", dataGridView1);
                dataGridView1.Columns["ID"].Visible = false;
                int c = dataGridView1.Rows.Count - 1;
                label13.Text = comboBox3.Text;
                OleDbDataReader dr1 =obj.select("select Unit from Items where Name='"+comboBox3.Text+"'");
                string Unit = "";
                if (dr1.Read())
                    Unit = dr1[0].ToString();
                double creditSum = 0, debitSum = 0;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (c == 0)
                    {
                        row.Cells["InWard"].Value = creditSum;
                        row.Cells["OutWard"].Value = debitSum;
                        row.Cells["InWard"].Value += DB_Handler.FormatWeight(creditSum,Unit);
                        row.Cells["OutWard"].Value += DB_Handler.FormatWeight(debitSum, Unit);
                        break;
                    }
                    if (row.Cells["InWard"].Value.ToString() != "" && row.Cells["InWard"].Value.ToString() != "0")
                    {
                        creditSum+=Convert.ToDouble(row.Cells["InWard"].Value.ToString());
                        row.Cells["InWard"].Value += DB_Handler.FormatWeight(Convert.ToDouble(row.Cells["InWard"].Value.ToString()), Unit);
                    }
                    if (row.Cells["OutWard"].Value.ToString() != "" && row.Cells["OutWard"].Value.ToString() != "0")
                    {
                        debitSum += Convert.ToDouble(row.Cells["OutWard"].Value.ToString());
                        row.Cells["OutWard"].Value += DB_Handler.FormatWeight(Convert.ToDouble(row.Cells["OutWard"].Value.ToString()), Unit);
                    }
                    c--;
                }
                
                OleDbDataReader wt = obj.select("select SUM(Weight) from Manorath where Name ='" + comboBox3.Text + "' and Date<'" + dateTimePicker4.Text + "'");
                OleDbDataReader dr = obj.select("select SUM(['" + comboBox3.Text.ToString() + "']) from Manorath where Date<'" + dateTimePicker4.Text + "'");
                OleDbDataReader op = obj.select("select OpeningStock from Items where Name='" + comboBox3.Text + "'");
                if (wt.Read())
                {
                    string a = wt[0].ToString();
                    if (dr.Read())
                    {
                        string b = dr[0].ToString();
                        if (op.Read())
                        {
                            if (a == "")
                                a = "0";
                            if (b == "")
                                b = "0";
                            textBox4.Text = ((Convert.ToDouble(a) - Convert.ToDouble(b)) + Convert.ToDouble(op[0].ToString())).ToString();
                            textBox4.Text += DB_Handler.FormatWeight(Convert.ToDouble(textBox4.Text),Unit);
                        }
                    }
                }
                wt = obj.select("select sum(Weight) from Manorath where Name = '" + comboBox3.Text + "' and Date<='" + dateTimePicker3.Text + "'");
                dr = obj.select("select sum(['" + comboBox3.Text.ToString() + "']) from Manorath where Date<='" + dateTimePicker3.Text + "'");
                op = obj.select("select OpeningStock from Items where Name='" + comboBox3.Text + "'");
                if (wt.Read())
                {
                    string a = wt[0].ToString();
                    if (dr.Read())
                    {
                        string b = dr[0].ToString();
                        if (op.Read())
                        {
                            if (a == "")
                                a = "0";
                            if (b == "")
                                b = "0";
                            double cb =((Convert.ToDouble(a) - Convert.ToDouble(b)) + Convert.ToDouble(op[0].ToString()));
                            textBox3.Text = cb.ToString();
                            textBox3.Text += DB_Handler.FormatWeight(cb,Unit);
                        }
                    }
                }
            }
            obj.con.Close();
        }

       

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["InWard"].Value.ToString() == "" || row.Cells["InWard"].Value.ToString() == "0")
                {
                    Id_Manorath.Text = row.Cells["ID"].Value.ToString();
                    comboBox1.Text = row.Cells["Particular"].Value.ToString();
                    textBox1.Text = row.Cells["Count"].Value.ToString();
                }
                else
                {
                    ID_Samagri.Text = row.Cells["ID"].Value.ToString();
                    comboBox2.Text = row.Cells["Particular"].Value.ToString();
                    string s = row.Cells["InWard"].Value.ToString();
                    textBox2.Text = s.Remove(s.Length-2,2);
                }
            }

        }
        private void reset()
        {
            DB_Handler obj = new DB_Handler();
            new DB_Handler().Update_Delete("update Manorath set Date=["+row.Cells["Date"].Value.ToString()+"],Particular=[" + row.Cells["Particular"].Value.ToString() + "],Count=[" + row.Cells["Count"].Value.ToString() + "] where ID="+Id_Manorath.Text);
            for (int i = 0, j = 3; i < comboBox2.Items.Count; i++, j++)
            {
                OleDbDataReader dr = obj.select("select * from BHET where Name='" + row.Cells["Particular"].Value.ToString() + "'");
                if (dr.Read())
                {
                    new DB_Handler().Update_Delete("update Manorath  set ['" + comboBox2.Items[i].ToString() + "']= ('" + Convert.ToDouble(dr[j].ToString()) * Convert.ToDouble(row.Cells["Count"].Value.ToString()) + "') where ID=" +Convert.ToInt32( Id_Manorath.Text));
                } 
            }
            obj.con.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            DB_Handler obj = new DB_Handler();
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                MessageBox.Show("Account Not Found");
                return;
            }
            new DB_Handler().Update_Delete("update Manorath set Date=["+dateTimePicker1.Text+"],Particular=[" + comboBox1.Text + "],Count=[" + textBox1.Text + "] where ID="+Id_Manorath.Text);
            for (int i = 0, j = 3; i < comboBox2.Items.Count; i++, j++)
            {
                OleDbDataReader dr = obj.select("select * from BHET where Name='" + comboBox1.Text + "'");
                if (dr.Read())
                {
                    if (dr[j].ToString() != "")
                    {
                        OleDbDataReader wt = obj.select("select SUM(Weight) from Manorath where Name ='" + comboBox2.Items[i].ToString() + "'");
                        OleDbDataReader dr1 = obj.select("select SUM(['" + comboBox2.Items[i].ToString() + "']) from Manorath");
                        OleDbDataReader op = obj.select("select OpeningStock,Unit from Items where Name='" + comboBox2.Items[i].ToString() + "'");
                        double cb = 0;
                        if (wt.Read())
                        {
                            string a = wt[0].ToString();
                            if (dr1.Read())
                            {
                                string b = dr1[0].ToString();
                                if (op.Read())
                                {
                                    if (a == "")
                                        a = "0";
                                    if (b == "")
                                        b = "0";
                                    cb = (Convert.ToDouble(a) + Convert.ToDouble(op[0].ToString())) - Convert.ToDouble(b);
                                }
                            }
                        }
                        if (cb < Convert.ToDouble(dr[j].ToString()) * Convert.ToDouble(textBox1.Text) && op[1].ToString().ToLower() == "kg")
                        {
                            reset();
                            MessageBox.Show(comboBox2.Items[i].ToString() + " is out of stock");
                            return;
                        }
                        new DB_Handler().Update_Delete("update Manorath  set ['" + comboBox2.Items[i].ToString() + "']= ('" + Convert.ToDouble(dr[j].ToString()) * Convert.ToDouble(textBox1.Text) + "') where ID=" + Id_Manorath.Text);
                    }
                }
            }
            obj.con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!comboBox1.Items.Contains(comboBox1.Text))
            {
                MessageBox.Show("No Account Found");
                return;
            }
            new DB_Handler().Update_Delete("delete from Manorath where ID=" + Id_Manorath.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            double res = 0;
            if (comboBox2.Items.Contains(comboBox2.Text))
            {
                if (!double.TryParse(textBox4.Text, out  res))
                {
                    new DB_Handler().Update_Delete("Update Manorath set Date='" + dateTimePicker2.Text + "',Name='" + comboBox2.Text + "',Weight='" + textBox2.Text + "' where ID=" + ID_Samagri);
                }
                else
                    MessageBox.Show("invalid format");
            }
            else
            {
                MessageBox.Show("No items found");
                return;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!comboBox2.Items.Contains(comboBox2.Text))
            {
                MessageBox.Show("No Account Found");
                return;
            }
            new DB_Handler().Update_Delete("delete from manorath where ID=" + ID_Samagri.Text);
        }
        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox1.Items.Contains(comboBox1.Text))
            {
                button1.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }
        }

        private void comboBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (comboBox2.Items.Contains(comboBox2.Text))
            {
                button2.Enabled = true;
                button6.Enabled = true;
                button7.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
            }
        }
    }
}
