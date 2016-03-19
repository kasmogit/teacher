using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teacher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Size size = this.Size;
                this.DataBindings.Add(new System.Windows.Forms.Binding("ClientSize", global::Teacher.Properties.Settings.Default, "MySize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
                this.Size = size;
                global::Teacher.Properties.Settings.Default.Save();
            }
        }
    }
}
