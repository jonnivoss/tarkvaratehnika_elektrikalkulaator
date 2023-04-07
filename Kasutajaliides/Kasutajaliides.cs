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

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace Kasutajaliides
{
    
    public partial class Kasutajaliides : Form
    {
        List<DateTime> time = new List<DateTime>();
        List<double> cost = new List<double>();

        VecT data = new VecT();
        string fileContents;

        private Andmepyydja.CAP AP = new Andmepyydja.CAP();
        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            AP.chooseFile();
            if (AP.chooseFile())
            {
                AP.readFile(ref fileContents);
                data = AP.parseContents(fileContents);

                foreach (var item in data)
                {
                    time.Add(item.Item1);
                    cost.Add(item.Item2);
                }
                chartElektrihind.Series["Elektrihind"].Points.DataBindXY(time, data);
                chartElektrihind.Invalidate();
            }

            
        }
        public Kasutajaliides() 
        {
            InitializeComponent();
            
        }

        private void Kasutajaliides_Load(object sender, EventArgs e)
        {
            chartElektrihind.Series["Elektrihind"].Points.DataBindXY(time, data);
        }

        private void txtAjakulu_KeyPress(object sender, KeyPressEventArgs e)
        {
            double parsedValue;
            if (!double.TryParse(txtAjakulu.Text + e.KeyChar, out parsedValue) && e.KeyChar !=8 && e.KeyChar != 46)
            {
                MessageBox.Show("Palun sisestage ainult numbreid!");
                e.Handled = true;
                return;
            }
        }

        private void txtVoimsus_KeyPress(object sender, KeyPressEventArgs e)
        {
            double parsedValue;
            if (!double.TryParse(txtVoimsus.Text + e.KeyChar, out parsedValue) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                MessageBox.Show("Palun sisestage ainult numbreid!");
                e.Handled = true;
                return;
            }
        }
    }
}
