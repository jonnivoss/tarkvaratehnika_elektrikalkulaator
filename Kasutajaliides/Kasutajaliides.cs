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
            if (AP.chooseFile() && AP.readFile(ref fileContents))
            {
                data = AP.parseContents(fileContents);

                time.Clear();
                cost.Clear();

                foreach (var item in data)
                {
                    time.Add(item.Item1);
                    cost.Add(item.Item2);

                    string line = item.Item1.ToString() + ": " + item.Item2.ToString();

                    txtDebug.AppendText(line);
                    txtDebug.AppendText(Environment.NewLine);
                }
                chartElektrihind.Series["Tarbimine"].Points.DataBindXY(time, cost);
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

        private void dateStartTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateStopTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbShowPrice_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbShowTabel_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbStockPrice_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
