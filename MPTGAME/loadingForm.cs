using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameHost
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        public void SetStatusLength(int length)
        {
            progressBar1.Invoke(new Action(() => progressBar1.Maximum = length));
        }

        public void Tick()
        {
            progressBar1.Invoke(new Action(() => progressBar1.Value += 1));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
