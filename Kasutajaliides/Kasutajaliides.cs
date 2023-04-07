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

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

namespace Kasutajaliides
{
    
    public partial class Kasutajaliides : Form
    {
        List<DateTime> aeg = new List<DateTime>();
        List<double> maksumus = new List<double>();

        VecT andmed = new VecT();

        private Andmepyydja.CAP AP = new Andmepyydja.CAP();
        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            AP.chooseFile();

        }
        public Kasutajaliides() 
        {
            InitializeComponent();
            

            foreach(var item in )
            {

            }

        }
    }
}
