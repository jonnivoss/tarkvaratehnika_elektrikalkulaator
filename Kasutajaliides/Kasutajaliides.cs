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
        public Kasutajaliides()
        {
            InitializeComponent();
        }

        List<DateTime> time = new List<DateTime>();
        List<double> cost = new List<double>();

        List<DateTime> timeRange = new List<DateTime>();
        List<double> costRange = new List<double>();

        VecT data = new VecT();
        string fileContents;

        private Andmepyydja.CAP AP = new Andmepyydja.CAP();
        private AndmeSalvestaja.CAS AS = new AndmeSalvestaja.CAS("settings.json");

        DateTime startTime, stopTime;
        bool showStock = true, isGraph = true;

        private void updateGraph()
        {
            // Uuenda graafikut
            timeRange.Clear();
            costRange.Clear();

            foreach(var item in data) {
                if(item.Item1 >= startTime && item.Item1 <= stopTime)
                {
                    timeRange.Add(item.Item1);
                    costRange.Add(item.Item2);

                    string line = item.Item1.ToString() + ": " + item.Item2.ToString();

                    txtDebug.AppendText(line);
                    txtDebug.AppendText(Environment.NewLine);
                }
            }
            chartPrice.Series["Tarbimine"].Points.DataBindXY(timeRange, costRange);
            chartPrice.Invalidate();
        }


        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            if (AP.chooseFile())
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed, AP.getFile());
                this.openCSV();
            }
        }
        private void openCSV()
        {
            if (!AP.readFile(ref fileContents))
            {
                MessageBox.Show("Lugemine ebaõnnestus!");
            }
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
            chartPrice.Series["Tarbimine"].Points.DataBindXY(time, cost);
            chartPrice.Invalidate();
        }
        

        private void Kasutajaliides_Load(object sender, EventArgs e)
        {
            chartPrice.Series["Elektrihind"].Points.DataBindXY(time, data);

            // Proovib avada CSV
            if (!AS.loadFile())
            {
                return;
            }
            AP.setFile(AS.getSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed));
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
            // Sea uus algusaeg
            this.startTime = dateStartTime.Value;
            updateGraph();
        }

        private void dateStopTime_ValueChanged(object sender, EventArgs e)
        {
            // Sea uus lõppaeg
            this.stopTime = dateStopTime.Value;
            updateGraph();
        }

        private void cbShowPrice_CheckedChanged(object sender, EventArgs e)
        {
            var state = cbShowPrice.Checked;
            if (state)
            {
                // Kuva börsihinda
                showStock = true;
                updateGraph();
            }
            else
            {
                // Kuva fikshinda
                showStock = false;
                updateGraph();
            }
        }

        private void cbShowTabel_CheckedChanged(object sender, EventArgs e)
        {
            var state = cbShowTabel.Checked;
            if (state)
            {
                // Kuva tabel
                chartPrice.Visible = false;
                tablePrice.Visible = true;
                isGraph = false;
            }
            else
            {
                // Kuva graafik
                tablePrice.Visible = false;
                chartPrice.Visible = true;
                isGraph = true;
            }
            updateGraph();
        }

        private void Kasutajaliides_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Salvestab sätted
            AS.saveFile();
        }

        private void rbStockPrice_CheckedChanged(object sender, EventArgs e)
        {
            var state = rbStockPrice.Checked;
            if (state)
            {
                tbMonthlyPrice.Enabled = false;
            }
            else
            {
                tbMonthlyPrice.Enabled = true;
            }
        }

        private void tbMonthlyPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            double parsedValue;
            if (!double.TryParse(tbMonthlyPrice.Text + e.KeyChar, out parsedValue) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                MessageBox.Show("Palun sisestage ainult numbreid!");
                e.Handled = true;
                return;
            }
        }
    }
}
