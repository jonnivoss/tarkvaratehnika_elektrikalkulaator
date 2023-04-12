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

        List<DateTime> timeRange = new List<DateTime>();
        List<double> costRange = new List<double>();

        List<DateTime> priceTimeRange = new List<DateTime>();
        List<double> priceCostRange = new List<double>();

        VecT userData = new VecT();
        string fileContents;

        VecT priceData = new VecT();

        private Andmepyydja.CAP AP = new Andmepyydja.CAP();
        private AndmeSalvestaja.CAS AS = new AndmeSalvestaja.CAS("settings.json");

        DateTime startTime, stopTime;
        bool showStock = true, isGraph = true;

        private void updateGraph()
        {
            // Uuenda graafikut
            timeRange.Clear();
            costRange.Clear();
            // Ära luba lõppkuupäeva alguskuupäevast väiksemaks panna
            dateStopTime.MinDate = dateStartTime.Value;
            foreach (var item in userData) {
                if(item.Item1 >= startTime && item.Item1 <= stopTime)
                {
                    timeRange.Add(item.Item1);
                    costRange.Add(item.Item2);

                    /*string line = item.Item1.ToString() + ": " + item.Item2.ToString();

                    txtDebug.AppendText(line);
                    txtDebug.AppendText(Environment.NewLine);*/
                }
            }

            priceTimeRange.Clear();
            priceCostRange.Clear();
            tablePrice.Rows.Clear();

            foreach(var item in priceData)
            {
                if (item.Item1 >= startTime && item.Item1 <= stopTime)
                {
                    priceTimeRange.Add(item.Item1);
                    priceCostRange.Add(item.Item2);
                    tablePrice.Rows.Add(item.Item1, item.Item2);

                    /*string line = "i: " + item.Item1.ToString() + ": " + item.Item2.ToString();

                    txtDebug.AppendText(line);
                    txtDebug.AppendText(Environment.NewLine);*/
                }
            }

            chartPrice.Series["Tarbimine"].Points.DataBindXY(timeRange, costRange);
            chartPrice.Series["Elektrihind"].Points.DataBindXY(priceTimeRange, priceCostRange);
            chartPrice.Invalidate();
            tablePrice.Invalidate();
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
                return;
            }
            userData = AP.parseContents(fileContents);

            timeRange.Clear();
            costRange.Clear();

            foreach (var item in userData)
            {
                timeRange.Add(item.Item1);
                costRange.Add(item.Item2);

                /*string line = item.Item1.ToString() + ": " + item.Item2.ToString();

                txtDebug.AppendText(line);
                txtDebug.AppendText(Environment.NewLine);*/
            }

            //chartPrice.Series["Tarbimine"].Points.DataBindXY(time, cost);
            //chartPrice.Invalidate();
            var timeRangeArr = timeRange.ToArray();

            dateStartTime.MinDate = timeRangeArr[0];
            dateStartTime.MaxDate = timeRangeArr[timeRangeArr.Length - 1];
            dateStartTime.Value = timeRangeArr[0];

            dateStopTime.MinDate = timeRangeArr[0];
            dateStopTime.MaxDate = timeRangeArr[timeRangeArr.Length - 1];           
            dateStopTime.Value = timeRangeArr[timeRangeArr.Length - 1];

            if ((this.startTime == default(DateTime)) || (this.stopTime == default(DateTime)))
            {
                //txtDebug.AppendText("Jõudsin nullini");
                //txtDebug.AppendText(Environment.NewLine);
                this.startTime = timeRangeArr[0];
                this.stopTime  = timeRangeArr[timeRangeArr.Length - 1];
            }

            priceTimeRange.Clear();
            priceCostRange.Clear();

            priceData = AP.HindAegInternet(timeRange.First(), timeRange.Last());
            //MessageBox.Show(priceData.Count.ToString());
            foreach (var item in priceData)
            {
                priceTimeRange.Add(item.Item1);
                priceCostRange.Add(item.Item2);
                tablePrice.Rows.Add(item.Item1, item.Item2);
            }

            txtDebug.AppendText("Jõudsin graafini");
            txtDebug.AppendText(Environment.NewLine);
            updateGraph();
        }
        

        private void Kasutajaliides_Load(object sender, EventArgs e)
        {
            // Proovib avada CSV
            if (!AS.loadFile())
            {
                return;
            }
            AP.setFile(AS.getSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed));
            openCSV();
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
            txtDebug.AppendText("; date: " + sender.ToString());
            txtDebug.AppendText(Environment.NewLine);
            
            // Sea uus algusaeg
            if (dateStartTime.Value <= dateStopTime.Value)
            {
                this.startTime = dateStartTime.Value;
                updateGraph();
            }
            else
            {
                MessageBox.Show("Alguskuupäev peab olema väiksem kui lõppkuupäev!");
            }
        }
        private void dateStartTime_DropDown(object sender, EventArgs e)
        {
            dateStartTime.ValueChanged += dateStartTime_ValueChanged;
        }
        private void dateStartTime_CloseUp(object sender, EventArgs e)
        {
            dateStartTime.ValueChanged -= dateStartTime_ValueChanged;
        }

        private void dateStopTime_ValueChanged(object sender, EventArgs e)
        {
            txtDebug.AppendText("date2: " + sender.ToString());
            txtDebug.AppendText(Environment.NewLine);
            // Sea uus lõppaeg
            if (dateStopTime.Value >= dateStartTime.Value)
            {
                this.stopTime = dateStopTime.Value;
                updateGraph();
            }
            else
            {
                MessageBox.Show("Lõppkuupäev peab olema suurem kui alguskuupäev");
            }
        }
        private void dateStopTime_DropDown(object sender, EventArgs e)
        {
            dateStopTime.ValueChanged += dateStopTime_ValueChanged;
        }
        private void dateStopTime_CloseUp(object sender, EventArgs e)
        {
            dateStopTime.ValueChanged -= dateStopTime_ValueChanged;
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

        private void btnElektriHind_Click(object sender, EventArgs e)
        {
            updateGraph();
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
