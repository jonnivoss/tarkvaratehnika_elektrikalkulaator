using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Kasutajaliides
{
    
    public partial class Kasutajaliides : Form
    {
        private Andmepyydja.CAP AP = new Andmepyydja.CAP();
        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            AP.chooseFile();

        }
        public Kasutajaliides() 
        {
            InitializeComponent();
        }
    }
}
