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
        private Arvutaja.CArvutaja AR = new Arvutaja.CArvutaja();

        DateTime startTime, stopTime;
        bool showStock = true, isGraph = true;

        private void updateGraph()
        {
            // Uuenda graafikut
            timeRange.Clear();
            costRange.Clear();
            // Ära luba lõppkuupäeva alguskuupäevast väiksemaks panna
            //dateStopTime.MinDate = dateStartTime.Value;
            foreach (var item in userData)
            {
                if (item.Item1 >= startTime && item.Item1 <= stopTime)
                {
                    timeRange.Add(item.Item1);
                    costRange.Add(item.Item2);

                    /*string line = item.Item1.ToString() + ": " + item.Item2.ToString();

                    txtDebug.AppendText(line);
                    txtDebug.AppendText(Environment.NewLine);*/
                }
            }

            if (rbStockPrice.Checked)
            {
                double integraal = 0;
                int olek = AR.integreerija(userData, priceData, startTime, stopTime, ref integraal);
                switch (olek)
                {
                    case 0:
                        txtHind.Text = (integraal / 1000).ToString() + " €";
                        break;
                    case 1:
                        txtHind.Text = "Viga 1";
                        break;
                    case 2:
                        txtHind.Text = "Viga 2";
                        break;
                }
            }

            if (timeRange.Count > 0)
            {
                changeInterval(timeRange.Count);
                chartPrice.Series["Tarbimine"].Points.DataBindXY(timeRange, costRange);
            }


            priceTimeRange.Clear();
            priceCostRange.Clear();
            tablePrice.Rows.Clear();

            foreach (var item in priceData)
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
            chartPrice.Series["Elektrihind"].Points.DataBindXY(priceTimeRange, priceCostRange);
            chartPrice.Invalidate();
            tablePrice.Invalidate();
        }

        private void changeInterval(int count)
        {
            if (count <= 26) //kui andmeid on ühe päeva jagu
            {
                chartPrice.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "HH:mm";
                chartPrice.ChartAreas["ChartArea1"].AxisX.Interval = 2; // silt iga kahe tunni tagant
            }
            else if (count <= 50) // kui andmeid on kahe päeva jagu
            {
                chartPrice.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd/MM HH:mm";
                chartPrice.ChartAreas["ChartArea1"].AxisX.Interval = 4; // silt iga nelja tunni tagant
            }
            else if (count <= 74) // kui andmeid on kolme päeva jagu
            {
                chartPrice.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd/MM HH:mm";
                chartPrice.ChartAreas["ChartArea1"].AxisX.Interval = 12; // silt iga 12 tunni tagant
            }
            else // kui andmeid on 4+ päeva jagu
            {
                chartPrice.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Days;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd/MM/yy";
                chartPrice.ChartAreas["ChartArea1"].AxisX.Interval = 0; // sildi intervall määratakse automaatselt 
            }
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
            else
            {
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

                var timeRangeArr = timeRange.ToArray();

                dateStartTime.MinDate = timeRangeArr[0];
                dateStartTime.MaxDate = timeRangeArr[timeRangeArr.Length - 1];
                dateStartTime.Value = timeRangeArr[0];

                dateStopTime.MinDate = timeRangeArr[0];
                dateStopTime.MaxDate = timeRangeArr[timeRangeArr.Length - 1];
                dateStopTime.Value = timeRangeArr[timeRangeArr.Length - 1];
            }

            if ((this.startTime == default(DateTime)) || (this.stopTime == default(DateTime)))
            {
                //txtDebug.AppendText("Jõudsin nullini");
                //txtDebug.AppendText(Environment.NewLine);
                this.startTime = dateStartTime.Value + new TimeSpan(0, 0, 0);
                this.stopTime = dateStopTime.Value + new TimeSpan(23, 59, 59);
                txtDebug.AppendText("jeba\n\n");
            }

            priceTimeRange.Clear();
            priceCostRange.Clear();


            if (timeRange.Count == 0)
            {
                this.startTime = DateTime.Now.Date + new TimeSpan(0, 0, 0);
                this.stopTime = DateTime.Now.Date + new TimeSpan(23, 59, 59);
                // Ajaintervalli määramine kuvamisel
                changeInterval(timeRange.Count);
                txtDebug.AppendText("  kaas   ");
            }


            callAPI();
            updateGraph();
        }

        private void callAPI()
        {
            priceData = AP.HindAegInternet(startTime, stopTime);
            MessageBox.Show(priceData.Count.ToString());
            foreach (var item in priceData)
            {
                priceTimeRange.Add(item.Item1);
                priceCostRange.Add(item.Item2);
                tablePrice.Rows.Add(item.Item1, item.Item2);
            }

            changeInterval(priceData.Count);
            /*if (priceTimeRange.Count <= 50)
            {
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "HH:mm";
            }*/
            txtDebug.AppendText("kutsun api\n");
        }

        private void calcPrice()
        {
            double time, power, price;
            try
            {
                time = Double.Parse(txtAjakulu.Text);
                power = Double.Parse(txtVoimsus.Text);
                // Do some crazy price calculation
                if (rbStockPrice.Checked)
                {
                    // Sööstab arvutajasse, leiab valitud ajavahemikust optimaalseima ajapikkuse
                    Console.WriteLine("Time: " + time.ToString());
                    var beg = this.startTime;
                    var end = beg.Date + TimeSpan.FromHours(Math.Ceiling(time));
                    Console.WriteLine("Begin: " + beg.ToString() + "; end: " + end.ToString());

                    double bestIntegral = double.PositiveInfinity;
                    var bestDate = beg;

                    for (; end <= this.stopTime;)
                    {
                        VecT useData = new VecT();
                        // Generate usedata
                        for (DateTime date = beg, tempend = (beg + TimeSpan.FromHours(time)); date < tempend; date = date.AddHours(1))
                        {
                            var hrs = (tempend - date).TotalHours;
                            if (hrs > 1.0)
                            {
                                hrs = 1.0;
                            }
                            Console.WriteLine("asd: " + date.ToString() + "; " + (power * hrs).ToString());
                            useData.Add(Tuple.Create(date, power * hrs));
                        }

                        // Integreerib
                        double integral = 0.0;
                        if (AR.integreerija(useData, this.priceData, useData.First().Item1, useData.Last().Item1, ref integral) == 0)
                        {
                            /*Console.WriteLine("beg: " + beg.ToString() + "; end: " + end.ToString() + "; int: " + integral.ToString());
                            Console.Write("!!!All DATA");
                            for (int d = this.priceData.FindIndex(Tuple => Tuple.Item1 == useData.First().Item1), l = this.priceData.FindIndex(Tuple => Tuple.Item1 == useData.Last().Item1) + 1; d <= l; ++d)
                            {
                                var item = this.priceData[d];
                                Console.WriteLine("dat: " + item.Item1.ToString() + ": " + item.Item2.ToString());
                            }*/
                            if (integral < bestIntegral)
                            {
                                bestIntegral = integral;
                                bestDate = beg;
                            }
                        }

                        beg = beg.AddHours(1);
                        end = end.AddHours(1);
                    }

                    price = bestIntegral / 1000.0;
                    MessageBox.Show("Tarbimist alustada " + bestDate.ToString("dd.MM.yyyy HH:mm"));
                }
                else
                {
                    var mwh = Double.Parse(tbMonthlyPrice.Text);
                    price = time * power * mwh / 1000.0;
                }

                txtHind.Text = Math.Round(price, 2).ToString();
            }
            catch (Exception)
            {
                return;
            }
        }


        private void Kasutajaliides_Load(object sender, EventArgs e)
        {
            // Lisab tüüp-kasutusmallid
            chartPrice.MouseWheel += chartPrice_zooming;
            txtHind.Text = "-";
            // Proovib avada CSV
            AS.loadFile();

            var items = AS.getUseCases();
            cbKasutusmall.Items.Clear();
            foreach (var i in items)
            {
                cbKasutusmall.Items.Add(i.Key);
            }


            AP.setFile(AS.getSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed));
            openCSV();
        }

        private void txtAjakulu_KeyPress(object sender, KeyPressEventArgs e)
        {
            double parsedValue;
            if (!double.TryParse(txtAjakulu.Text + e.KeyChar, out parsedValue) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                MessageBox.Show("Palun sisestage ainult numbreid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void txtVoimsus_KeyPress(object sender, KeyPressEventArgs e)
        {
            double parsedValue;
            if (!double.TryParse(txtVoimsus.Text + e.KeyChar, out parsedValue) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                MessageBox.Show("Palun sisestage ainult numbreid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        private void dateStartTime_ValueChanged(object sender, EventArgs e)
        {
            txtDebug.AppendText("; date: " + sender.ToString());
            txtDebug.AppendText(Environment.NewLine);

            var d = dateStartTime.Value.Date + new TimeSpan(0, 0, 0);

            // Sea uus algusaeg
            if (dateStartTime.Value <= dateStopTime.Value)
            {
                this.startTime = d;
            }
            else
            {
                this.startTime = d;
                dateStopTime.Value = d.Date + new TimeSpan(23, 59, 59);
                this.stopTime = dateStopTime.Value;
            }
            callAPI();
            updateGraph();
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
            var d = dateStopTime.Value.Date + new TimeSpan(23, 59, 59);

            if (dateStopTime.Value >= dateStartTime.Value)
            {
                this.stopTime = d;
            }
            else
            {
                this.stopTime = d;
                dateStartTime.Value = d.Date + new TimeSpan(0, 0, 0);
                this.startTime = dateStartTime.Value;
            }
            callAPI();
            updateGraph();
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

        private void cbKasutusmall_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                var item = AS.getUseCases()[cbKasutusmall.SelectedItem.ToString()];
                txtVoimsus.Text = Math.Round(item.Item1 / 1000.0, 3).ToString();
                txtAjakulu.Text = Math.Round(item.Item2 / 60.0, 3).ToString();

                // Calculate price
                calcPrice();
            }
            catch (Exception)
            {
            }
        }

        private void txtAjakulu_TextChanged(object sender, EventArgs e)
        {
            calcPrice();
        }

        private void txtVoimsus_TextChanged(object sender, EventArgs e)
        {
            calcPrice();
        }

        private void tbMonthlyPrice_TextChanged(object sender, EventArgs e)
        {
            if (rbMonthlyCost.Checked)
            {
                calcPrice();
            }
        }

        private void tbMonthlyPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            double parsedValue;
            if (!double.TryParse(tbMonthlyPrice.Text + e.KeyChar, out parsedValue) && e.KeyChar != 8 && e.KeyChar != 46)
            {
                MessageBox.Show("Palun sisestage ainult numbreid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Handled = true;
                return;
            }
        }

        void chartPrice_zooming(object sender, MouseEventArgs e)
        {
            if (dateStartTime.Value.Day < dateStopTime.Value.Day)
            {
                var deltaDate = dateStopTime.Value - dateStartTime.Value;
                var distanceFromMin = dateStartTime.Value - dateStartTime.MinDate;
                var distanceFromMax = dateStopTime.MaxDate - dateStopTime.Value;
                double xCoordPercent = (e.Location.X - 88) / 464.0; // jagades graafiku laiusega saame pointeri normaliseeritud asukoha graafikul vahemikus [0;1]
                //MessageBox.Show(xCoordPercent.ToString
                int leftStep = 1, rightStep = 1, totalStep = 1; // sammud graafiku otste nihutamiseks (tundides)
                if (deltaDate.TotalDays < 2) totalStep = 12;
                else totalStep = 24;
                leftStep = Convert.ToInt16(xCoordPercent * totalStep);
                rightStep = totalStep - leftStep;

                if (xCoordPercent <= 1 && xCoordPercent >= 0)
                {
                    if (e.Delta > 0 && (dateStartTime.Value < dateStopTime.Value) && deltaDate.TotalDays >= 2)
                    {
                        dateStartTime.Value = dateStartTime.Value.AddHours(leftStep);
                        dateStopTime.Value = dateStopTime.Value.AddHours(-rightStep);
                        startTime = dateStartTime.Value;
                        stopTime = dateStopTime.Value;
                    }
                    else if (e.Delta < 0)
                    {
                        if (distanceFromMin.TotalDays >= 1)
                        {
                            dateStartTime.Value = dateStartTime.Value.AddHours(-leftStep);
                            startTime = dateStartTime.Value;
                        }
                        else
                        {
                            dateStartTime.Value = dateStartTime.MinDate;
                            startTime = dateStartTime.Value;
                        }
                        if (distanceFromMax.TotalDays >= 1)
                        {
                            dateStopTime.Value = dateStopTime.Value.AddHours(rightStep);
                            stopTime = dateStopTime.Value;
                        }
                        else
                        {
                            dateStopTime.Value = dateStopTime.MaxDate;
                            stopTime = dateStopTime.Value;
                        }
                    }
                }
            }
            updateGraph();
        }
    }
}