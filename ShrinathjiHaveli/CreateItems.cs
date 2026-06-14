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
    public partial class CreateItems : Form
    {
       
        public CreateItems()
        {
            InitializeComponent();
          
        }
        void reset()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox3.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "" && textBox3.Text.Trim()!="" && textBox4.Text.Trim() != "")
            {
                string s;
                if (textBox2.Text == "")
                    s = "0.000";
                else
                    s = textBox2.Text;
                try
                {
                    string q = "INSERT INTO Items ([Name],[Unit],[Rate],[OpeningStock]) VALUES ('" + textBox1.Text + "','" + textBox4.Text + "','"+textBox3.Text+"','"+s+"')";
                    new DB_Handler().insert(q);
                    new DB_Handler().Update_Delete("ALTER TABLE BHET ADD COLUMN '"+textBox1.Text+"' varchar(255)");
                    new DB_Handler().Update_Delete("ALTER TABLE Manorath ADD COLUMN '" + textBox1.Text + "' varchar(255)");
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show("Duplicate Item");
                }
               
            }
            else
            {
                MessageBox.Show("All Field are mandentory");
      
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
        }

       
    }
}
