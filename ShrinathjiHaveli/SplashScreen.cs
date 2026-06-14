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
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            
        }
        private void SplashScreen_Load(object sender, EventArgs e)
        {
        }
        int c = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (c == 0)
            {
                c = 1;
            }
            else
            {
                this.Hide();
                new FrmLogin().ShowDialog();
                timer1.Stop();
            }
        }
    }
}
