using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShrinathjiHaveli
{
    public partial class HomeScreen : Form
    {
        public HomeScreen()
        {

            InitializeComponent();
        }

      

       
        private void button1_Click(object sender, EventArgs e)
        {
            new CreateLedger().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Alter().ShowDialog();
        }

       
        private void button4_Click(object sender, EventArgs e)
        {
            new Rokad().ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            new AccountsDetails().ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Stocks().ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
