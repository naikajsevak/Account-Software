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
    public partial class Stocks : Form
    {
        public Stocks()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new CreateItems().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new PurchaseManorath().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new AlterItems().ShowDialog();
        }

        
    }
}
