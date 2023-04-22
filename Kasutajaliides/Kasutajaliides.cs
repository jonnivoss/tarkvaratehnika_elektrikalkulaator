using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using PackageT = System.Collections.Generic.List<Andmepyydja.PackageInfo>;

namespace Kasutajaliides
{

    public partial class Kasutajaliides : Form
    {
        public Kasutajaliides()
        {
            InitializeComponent();
        }

        //Point clickPoint = new Point();
        ToolTip toolTip = new ToolTip();

        // kasutajaliidese tekstifondid
        Font Normal = new Font("Impact", 12);
        Font Bigger = new Font("Impact", 16);
        // 
        List<DateTime> timeRange = new List<DateTime>();
        List<double> costRange = new List<double>();

        List<DateTime> priceTimeRange = new List<DateTime>();
        List<double> priceCostRange = new List<double>();

        VecT userData = new VecT();
        string fileContents, packageFileContents;

        VecT priceData = new VecT();
        PackageT packageData = new PackageT();

        private Andmepyydja.CAP AP = new Andmepyydja.CAP();
        private AndmeSalvestaja.CAS AS = new AndmeSalvestaja.CAS("settings.json");
        private Arvutaja.CArvutaja AR = new Arvutaja.CArvutaja();

        DateTime startTime, stopTime;
        DateTime endOfDayDate = DateTime.Now.Date.AddHours(24); // vastab tänase päeva lõpule (24:00)
        bool showStock = true, showUsage = true;
        bool state = true;
        bool state2 = true; // DARK MODE BUTTON TOGGLE

        // akna elementide mõõtmete vaikeväärtused
        Rectangle originalWindowSize;
        Rectangle originalChartPriceSize;
        Rectangle originalTablePriceSize;
        Rectangle originalTablePackagesSize;
        Rectangle originalButtonChangeSize;
        Rectangle originalLabelKasutusmall;
        Rectangle originalComboBoxKasutusmall;
        Rectangle originalLabelAeg;
        Rectangle originalTextAjakulu;
        Rectangle originalLabelTund;
        Rectangle originalLabelV6imsus;
        Rectangle originalTextV6imsus;
        Rectangle originalLabelkW;
        Rectangle originalLabelHind;
        Rectangle originalTextHind;
        Rectangle originalLabelAndresEek;
        Rectangle originalTextDebug;
        Rectangle originalButtonAvaCsv;
        Rectangle originalCheckBoxShowPrice;
        Rectangle originalCheckBoxShowUsage;
        Rectangle originalCheckBoxShowTable;
        Rectangle originalLabelBeginning;
        Rectangle originalDateStartTimePicker;
        Rectangle originalLabelEnd;
        Rectangle originalDateStopTimePicker;
        Rectangle originalGroupBoxGroupTypePrice;
        Rectangle originalLabelTarbimisAeg;
        Rectangle originalTextTarbimisAeg;
        Rectangle originalLabelCostNow;
        Rectangle originalTextCostNow;
        Rectangle originalLabelSKwh2;
        Rectangle originalButtonDarkMode;
        Rectangle originalBtnOpenPackages;

        private void updateGraph()
        {
            // Uuenda graafikut
            timeRange.Clear();
            costRange.Clear();
            foreach (var item in userData)
            {
                if (item.Item1 >= startTime && item.Item1 <= stopTime)
                {
                    timeRange.Add(item.Item1);
                    costRange.Add(item.Item2);
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

            DateTime ajutineDate = new DateTime();
            double ajutinePrice = 0.0;

            foreach (var item in priceData)
            {
                if (item.Item1 >= startTime && item.Item1 <= stopTime)
                {
                    priceTimeRange.Add(item.Item1);
                    priceCostRange.Add(item.Item2 / 10.0);
                    tablePrice.Rows.Add(item.Item1, item.Item2 / 10.0);

                    ajutineDate = item.Item1.AddHours(1);
                    ajutinePrice = item.Item2 / 10.0;

                    /*string line = "i: " + item.Item1.ToString() + ": " + item.Item2.ToString();

                    txtDebug.AppendText(line);
                    txtDebug.AppendText(Environment.NewLine);*/
                }
            }
            priceTimeRange.Add(ajutineDate);
            priceCostRange.Add(ajutinePrice);

            chartPrice.Series["Elektrihind"].Points.DataBindXY(priceTimeRange, priceCostRange);
            
            chartPrice.Series["Elektrihind"].Enabled = showStock;
            chartPrice.Series["Tarbimine"].Enabled = showUsage;
            chartPrice.Invalidate();
            tablePrice.Invalidate();
        }

        //arvutab ristküllikuid
        //https://stackoverflow.com/questions/9647666/finding-the-value-of-the-points-in-a-chart
        //arvutab charti pindala
        RectangleF ChartAreaClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF CAR = CA.Position.ToRectangleF();
            float pw = chart.ClientSize.Width / 100f;
            float ph = chart.ClientSize.Height / 100f;
            return new RectangleF(pw * CAR.X, ph * CAR.Y, pw * CAR.Width, ph * CAR.Height);
        }
        //sama asi
        RectangleF InnerPlotPositionClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF IPP = CA.InnerPlotPosition.ToRectangleF();
            RectangleF CArp = ChartAreaClientRectangle(chart, CA);

            float pw = CArp.Width / 100f;
            float ph = CArp.Height / 100f;

            return new RectangleF(CArp.X + pw * IPP.X, CArp.Y + ph * IPP.Y,
                                    pw * IPP.Width, ph * IPP.Height);
        }

        private VerticalLineAnnotation vert;

        private void chartPrice_MouseMove(object sender, MouseEventArgs e)
        {
            /*// Cast the sender object to a Chart control
            Chart chart = (Chart)sender;

            // Call HitTest method to get the HitTestResult
            HitTestResult result = chart.HitTest(e.X, e.Y);

            // Check if the result is a data point
            if (result.ChartElementType == ChartElementType.DataPoint && result.Series.Name != "Tarbimine")
            {
                // Get the y-value of the data point
                double yValue = result.Series.Points[result.PointIndex].YValues[0];
                
                DateTime ahhha = dateStartTime.Value.AddHours(result.PointIndex);
                // Set the tooltip of the data point to the y-value
                string clockString = "kell: ";
                clockString += ahhha.ToString("HH");
                ahhha = ahhha.AddHours(1);
                clockString += "-";
                clockString += ahhha.ToString("HH");
                toolTip.SetToolTip(chart, "hind: " + yValue.ToString()+" senti " +clockString);
            }
            else 
            {
                toolTip.Hide(chart);
            }*/

            Chart chart = (Chart)sender;
            //kui tt tühi tee uus
            if (toolTip == null) toolTip = new ToolTip();

            ChartArea ca = chart.ChartAreas[0];
            //kui hiir on graafiku sees ss hakka asju tegema
            if (InnerPlotPositionClientRectangle(chart, ca).Contains(e.Location))
            {

                Axis ax = ca.AxisX;
                Axis ay = ca.AxisY;
                //leia x kordinaat kus hiir on
                double x = ax.PixelPositionToValue(e.X);
                double y = 0;
                //leia punkt millele x vastab ja salvesta selle y kordinaat
                DateTime s = DateTime.FromOADate(x);
                foreach (var item in priceData)
                {
                    if (item.Item1.Hour == s.Hour && item.Item1.Date == s.Date)
                    {
                        y = item.Item2 / 10.0;
                        break;
                    }
                }
                //tt tekst
                toolTip.SetToolTip(chart, "hind: " + y.ToString("0.000") + "\n" + s.ToString("kell HH:00") + "\n" + s.ToString("dd/MM/yy"));
                //kui juba joon olemas ss kustuta ära et uus joonistada
                if (vert != null)
                {
                    chart.Annotations.Remove(vert);
                }
                //new line
                vert = new VerticalLineAnnotation();
                //joonistub x axise kohal
                vert.AxisX = chart.ChartAreas[0].AxisX;
                vert.X = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                //läbib tervet charti
                vert.IsInfinitive = true;
                vert.ClipToChartArea = ca.Name;
                //joone nimi
                vert.Name = "Elektrihind";
                //joone stiil värv ja laius
                vert.LineColor = Color.Green;
                vert.LineWidth = 1;
                vert.LineDashStyle = ChartDashStyle.Solid;
                //joonista joon
                chart.Annotations.Add(vert);
            }
            else
            {
                //kustuta tt ja joon
                chart.Annotations.Remove(vert);
                toolTip.Hide(chart);
            }
        }

        private void changeInterval(int count)
        {
            if (count <= 24) //kui andmeid on ühe päeva jagu
            {
                chartPrice.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "HH:mm";
                chartPrice.ChartAreas["ChartArea1"].AxisX.Interval = 2; // silt iga kahe tunni tagant
            }
            else if (count <= 48) // kui andmeid on kahe päeva jagu
            {
                chartPrice.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Hours;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dd/MM HH:mm";
                chartPrice.ChartAreas["ChartArea1"].AxisX.Interval = 4; // silt iga nelja tunni tagant
            }
            else if (count <= 72) // kui andmeid on kolme päeva jagu
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
            chartPrice.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
        }

        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            if (AP.chooseFile())
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed, AP.getUserDataFileName());
                this.openCSV();
            }
        }
        private void openCSV()
        {
            if (!AP.readUserDataFile(ref fileContents))
            {
                MessageBox.Show("Lugemine ebaõnnestus!");
            }
            else
            {
                userData = AP.parseUserData(fileContents);

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

                //dateStartTime.MinDate = timeRangeArr[0];
                //dateStartTime.MaxDate = timeRangeArr[timeRangeArr.Length - 1];
                //dateStartTime.Value = timeRangeArr[0];

                //dateStopTime.MinDate = timeRangeArr[0];
                //dateStopTime.MaxDate = timeRangeArr[timeRangeArr.Length - 1];
                //dateStopTime.Value = timeRangeArr[timeRangeArr.Length - 1];
            }

            if ((this.startTime == default(DateTime)) || (this.stopTime == default(DateTime)))
            {
                //txtDebug.AppendText("Jõudsin nullini");
                //txtDebug.AppendText(Environment.NewLine);
                this.startTime = dateStartTime.Value + new TimeSpan(0, 0, 0);
                this.stopTime  = dateStopTime.Value + new TimeSpan(23, 59, 59);
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
            //MessageBox.Show(priceData.Last().Item1.ToString());
            callAPI(startTime.Date.AddDays(-60), endOfDayDate); // kutsub API ainult vajadusel välja ja loeb seejuures korraga rohkem andmeid!
            updateGraph();
        }

        // API caller muudetud üldisemaks, saab kutsuda suvaliste ajaparameetritega (peale startTime ja stopTime)
        private void callAPI(DateTime start, DateTime stop)
        {
            priceData = AP.HindAegInternet(start, stop);
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
                    txtTarbimisAeg.Text = bestDate.ToString("dd.MM.yyyy HH:mm");
                    //MessageBox.Show("Tarbimist alustada " + bestDate.ToString("dd.MM.yyyy HH:mm"));
                }
                else
                {
                    var skwh = Double.Parse(tbMonthlyPrice.Text);
                    price = time * power * skwh / 100.0;
                }

                //see on pask
                //descusting
                /*if (rbStockPrice.Checked)
                {
                    double integraal = 0;
                    int olek = AR.integreerija(userData, priceData, startTime, stopTime, ref integraal);
                    switch (olek)
                    {
                        case 0:
                            txtHind.Text = (integraal / 1000).ToString() + " €";
                            break;
                        case 1:
                            txtHind.Text = "";
                            break;
                        case 3:
                            txtHind.Text = "-";
                            break;
                    }
                }*/
                txtHind.Text = Math.Round(price, 2).ToString();
            }
            catch (Exception)
            {
                return;
            }
        }


        private void Kasutajaliides_Load(object sender, EventArgs e)
        {
            // "MOUSE SCROLL" suurendus. EI KUSTUTA KURAT, kui vaja siis commenti välja!
            this.chartPrice.MouseWheel += new MouseEventHandler(chartPrice_zooming);

            this.BackColor = SystemColors.Control;

            this.MinimumSize = new Size(1083, 713);
            // Lisab tüüp-kasutusmallid
            txtHind.Text = "-";
            tablePrice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tablePackages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // LAEN KOHE SISSE KAHE KUU JAGU ANDMEID ET EI PEAKS IGA KORD APId VÄLJA KUTSUMA KUI KUUPÄEV VEIDI MUUTUB!
            VecT costNowData = AP.HindAegInternet(DateTime.Now.Date.AddDays(-60), DateTime.Now);
            List<double> costNow = new List<double>();
            foreach (var item in costNowData)
            {
                costNow.Add(item.Item2);
            }
            txtCostNow.Text = (costNow[0]/10).ToString();
            
            // Proovib avada CSV
            AS.loadFile();

            var items = AS.getUseCases();
            cbKasutusmall.Items.Clear();
            foreach (var i in items)
            {
                cbKasutusmall.Items.Add(i.Key);
            }


            AP.setUserDataFileName(AS.getSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed));
            AP.setPackageFileName(AS.getSetting(AndmeSalvestaja.ASSetting.paketiAndmed));
            //callAPI(DateTime.Now.Date.AddDays(-60), DateTime.Now);
            //MessageBox.Show(priceTimeRange.Last().ToString());
            openCSV();
            openCSVPackage();

            // akna elementide mõõtmete vaikeväärtuste määramine
            originalWindowSize = new Rectangle(this.Location.X, this.Location.Y, this.Size.Width, this.Size.Height);
            originalChartPriceSize = new Rectangle(chartPrice.Location.X, chartPrice.Location.Y, chartPrice.Size.Width, chartPrice.Size.Height);
            originalTablePriceSize = new Rectangle(tablePrice.Location.X, tablePrice.Location.Y, tablePrice.Size.Width, tablePrice.Size.Height);
            originalTablePackagesSize = new Rectangle(tablePackages.Location.X, tablePackages.Location.Y, tablePackages.Size.Width, tablePackages.Size.Height);
            originalButtonChangeSize = new Rectangle(btnChangeSize.Location.X, btnChangeSize.Location.Y, btnChangeSize.Size.Width, btnChangeSize.Size.Height);
            originalLabelKasutusmall = new Rectangle(lblKasutusmall.Location.X, lblKasutusmall.Location.Y, lblKasutusmall.Size.Width, lblKasutusmall.Size.Height);
            originalComboBoxKasutusmall = new Rectangle(cbKasutusmall.Location.X, cbKasutusmall.Location.Y, cbKasutusmall.Size.Width, cbKasutusmall.Size.Height);
            originalLabelAeg = new Rectangle(lblAeg.Location.X, lblAeg.Location.Y, lblAeg.Size.Width, lblAeg.Size.Height);
            originalTextAjakulu = new Rectangle(txtAjakulu.Location.X, txtAjakulu.Location.Y, txtAjakulu.Size.Width, txtAjakulu.Size.Height);
            originalLabelTund = new Rectangle(lblTund.Location.X, lblTund.Location.Y, lblTund.Size.Width, lblTund.Size.Height);
            originalLabelV6imsus = new Rectangle(lblVoimsus.Location.X, lblVoimsus.Location.Y, lblVoimsus.Size.Width, lblVoimsus.Size.Height);
            originalTextV6imsus = new Rectangle(txtVoimsus.Location.X, txtVoimsus.Location.Y, txtVoimsus.Size.Width, txtVoimsus.Size.Height);
            originalLabelkW = new Rectangle(lblkW.Location.X, lblkW.Location.Y, lblkW.Size.Width, lblkW.Size.Height);
            originalLabelHind = new Rectangle(lblHind.Location.X, lblHind.Location.Y, lblHind.Size.Width, lblHind.Size.Height);
            originalTextHind = new Rectangle(txtHind.Location.X, txtHind.Location.Y, txtHind.Size.Width, txtHind.Size.Height);
            originalLabelAndresEek = new Rectangle(lblAndresEek.Location.X, lblAndresEek.Location.Y, lblAndresEek.Size.Width, lblAndresEek.Size.Height);
            originalTextDebug = new Rectangle(txtDebug.Location.X, txtDebug.Location.Y, txtDebug.Size.Width, txtDebug.Size.Height);
            originalButtonAvaCsv = new Rectangle(btnAvaCSV.Location.X, btnAvaCSV.Location.Y, btnAvaCSV.Size.Width, btnAvaCSV.Size.Height);
            originalCheckBoxShowPrice = new Rectangle(cbShowPrice.Location.X, cbShowPrice.Location.Y, cbShowPrice.Size.Width, cbShowPrice.Size.Height);
            originalCheckBoxShowUsage = new Rectangle(cbShowUsage.Location.X, cbShowUsage.Location.Y, cbShowUsage.Size.Width, cbShowUsage.Size.Height);
            originalCheckBoxShowTable = new Rectangle(cbShowTabel.Location.X, cbShowTabel.Location.Y, cbShowTabel.Size.Width, cbShowTabel.Size.Height);
            originalLabelBeginning = new Rectangle(lblBeginning.Location.X, lblBeginning.Location.Y, lblBeginning.Size.Width, lblBeginning.Size.Height);
            originalDateStartTimePicker = new Rectangle(dateStartTime.Location.X, dateStartTime.Location.Y, dateStartTime.Size.Width, dateStartTime.Size.Height);
            originalLabelEnd = new Rectangle(lblEnd.Location.X, lblEnd.Location.Y, lblEnd.Size.Width, lblEnd.Size.Height);
            originalDateStopTimePicker = new Rectangle(dateStopTime.Location.X, dateStopTime.Location.Y, dateStopTime.Size.Width, dateStopTime.Size.Height);
            originalGroupBoxGroupTypePrice = new Rectangle(groupPriceType.Location.X, groupPriceType.Location.Y, groupPriceType.Size.Width, groupPriceType.Size.Height);
            originalLabelTarbimisAeg = new Rectangle(lblTarbimisAeg.Location.X, lblTarbimisAeg.Location.Y, lblTarbimisAeg.Size.Width, lblTarbimisAeg.Size.Height);
            originalTextTarbimisAeg = new Rectangle(txtTarbimisAeg.Location.X, txtTarbimisAeg.Location.Y, txtTarbimisAeg.Size.Width, txtTarbimisAeg.Size.Height);
            originalLabelCostNow = new Rectangle(lblCostNow.Location.X, lblCostNow.Location.Y, lblCostNow.Size.Width, lblCostNow.Size.Height);
            originalTextCostNow = new Rectangle(txtCostNow.Location.X, txtCostNow.Location.Y, txtCostNow.Size.Width, txtCostNow.Size.Height);
            originalLabelSKwh2 = new Rectangle(lblSKwh2.Location.X, lblSKwh2.Location.Y, lblSKwh2.Size.Width, lblSKwh2.Size.Height);
            originalButtonDarkMode = new Rectangle(btnDarkMode.Location.X, btnDarkMode.Location.Y, btnDarkMode.Size.Width, btnDarkMode.Size.Height);
            originalBtnOpenPackages = new Rectangle(btnOpenPackages.Location.X, btnOpenPackages.Location.Y, btnOpenPackages.Size.Width, btnOpenPackages.Size.Height);
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
            if (startTime < priceData.First().Item1 || stopTime > priceData.Last().Item1)
            {
                callAPI(startTime.Date.AddDays(-60), endOfDayDate); // kutsub API ainult vajadusel välja ja loeb seejuures korraga rohkem andmeid!
            }
            calcPrice();
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
            if (startTime < priceData.First().Item1 || stopTime > priceData.Last().Item1)
            {
                callAPI(startTime.Date.AddDays(-60), endOfDayDate); // kutsub API ainult vajadusel välja ja loeb seejuures korraga rohkem andmeid!
            }
            calcPrice();
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
            }
            else
            {
                // Kuva graafik
                tablePrice.Visible = false;
                chartPrice.Visible = true;
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
            calcPrice();
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
        private void btnNormalSize_Click(object sender, EventArgs e)
        {
            if (state)
            {
                lblKasutusmall.Font = Bigger;
                lblAeg.Font = Bigger;
                lblTund.Font = Bigger;
                lblHind.Font = Bigger;
                lblAndresEek.Font = Bigger;
                lblVoimsus.Font = Bigger;
                lblkW.Font = Bigger;
                lblRate.Font = Bigger;
                lblSKwh2.Font = Bigger;
                lblCostNow.Font = Bigger;
                txtCostNow.Font = Bigger;
                cbShowPrice.Font = Bigger;
                cbShowTabel.Font = Bigger;
                cbShowUsage.Font = Bigger;
                rbMonthlyCost.Font = Bigger;
                rbStockPrice.Font = Bigger;
                groupPriceType.Font = new Font("Impact", 12);
                lblBeginning.Font = Bigger;
                lblEnd.Font = Bigger;
                cbKasutusmall.Font = Bigger;
                txtAjakulu.Font = Bigger;
                txtVoimsus.Font = Bigger;
                txtHind.Font = Bigger;
                btnAvaCSV.Font = Bigger;
                dateStartTime.Font = Bigger;
                dateStopTime.Font = Bigger;
                btnChangeSize.Font = Bigger;
                tbMonthlyPrice.Font = Bigger;
                lblRate.Font = Bigger;
                lblTarbimisAeg.Font = Bigger;
                txtTarbimisAeg.Font = Bigger;
                btnChangeSize.Text = "-";
                chartPrice.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("Comic Sans MS", 12);
                chartPrice.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("Comic Sans MS", 12);
                chartPrice.ChartAreas["ChartArea1"].AxisY2.TitleFont = new Font("Comic Sans MS", 12);
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("Comic Sans MS", 10);
                chartPrice.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new Font("Comic Sans MS", 10);
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LabelStyle.Font = new Font("Comic Sans MS", 10);
                chartPrice.Legends["Legend1"].Font = new Font("Comic Sans MS", 10);
                tablePrice.ColumnHeadersDefaultCellStyle.Font = Bigger;
                tablePrice.RowsDefaultCellStyle.Font = Bigger;
                tablePackages.ColumnHeadersDefaultCellStyle.Font = Bigger;
                tablePackages.RowsDefaultCellStyle.Font = Bigger;
                btnOpenPackages.Font = Bigger;
                state = false;
            }
            else
            {
                lblKasutusmall.Font = Normal;
                lblAeg.Font = Normal;
                lblTund.Font = Normal;
                lblHind.Font = Normal;
                lblAndresEek.Font = Normal;
                lblVoimsus.Font = Normal;
                lblkW.Font = Normal;
                lblRate.Font = Normal;
                lblSKwh2.Font = Normal;
                lblCostNow.Font = Normal;
                txtCostNow.Font = Normal;
                cbShowPrice.Font = Normal;
                cbShowTabel.Font = Normal;
                cbShowUsage.Font = Normal;
                rbMonthlyCost.Font = Normal;
                rbStockPrice.Font = Normal;
                groupPriceType.Font = new Font("Impact", 9);
                lblBeginning.Font = Normal;
                lblEnd.Font = Normal;
                cbKasutusmall.Font = Normal;
                txtAjakulu.Font = Normal;
                txtVoimsus.Font = Normal;
                txtHind.Font = Normal;
                btnAvaCSV.Font = Normal;
                dateStartTime.Font = Normal;
                dateStopTime.Font = Normal;
                btnChangeSize.Font = Normal;
                tbMonthlyPrice.Font = Normal;
                lblRate.Font = Normal;
                lblTarbimisAeg.Font = Normal;
                txtTarbimisAeg.Font = Normal;
                btnChangeSize.Text = "+";
                chartPrice.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("Comic Sans MS", 10);
                chartPrice.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("Comic Sans MS", 10);
                chartPrice.ChartAreas["ChartArea1"].AxisY2.TitleFont = new Font("Comic Sans MS", 10);
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("Comic Sans MS", 8.25f);
                chartPrice.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new Font("Comic Sans MS", 8.25f);
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LabelStyle.Font = new Font("Comic Sans MS", 8.25f);
                chartPrice.Legends["Legend1"].Font = new Font("Comic Sans MS", 8.25f);
                tablePrice.ColumnHeadersDefaultCellStyle.Font = Normal;
                tablePrice.RowsDefaultCellStyle.Font = Normal;
                tablePackages.ColumnHeadersDefaultCellStyle.Font = Normal;
                tablePackages.RowsDefaultCellStyle.Font = Normal;
                btnOpenPackages.Font = Normal;
                state = true;
            }
        }

        private void cbShowUsage_CheckedChanged(object sender, EventArgs e)
        {
            var state = cbShowUsage.Checked;
            if (state)
            {
                this.showUsage = true;
                updateGraph();
            }
            else
            {
                this.showUsage = false;
                updateGraph();
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

        private void resizeGuiElement(Rectangle nelinurk, Control element)
        {
            // kui suure osa moodustavad uued mõõtmed ja koordinaadid esialgsetest
            float xSuhe = (float)this.Width / (float)originalWindowSize.Width;
            float ySuhe = (float)this.Height / (float)originalWindowSize.Height;
            int uusXkoordinaat = (int)(nelinurk.Location.X * xSuhe);
            int uusYkoordinaat = (int)(nelinurk.Location.Y * ySuhe);
            int uusXlaius = (int)(nelinurk.Width * xSuhe);
            int uusYk6rgus = (int)(nelinurk.Height * ySuhe);
            element.Location = new Point(uusXkoordinaat, uusYkoordinaat);
            element.Size = new Size(uusXlaius, uusYk6rgus);
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            state2 = !state2;
            // VÄRVID (RGB hex.)
            var chalkWhite = ColorTranslator.FromHtml("#BBBBBB");
            var midGrey = ColorTranslator.FromHtml("#202020");
            var darkGrey = ColorTranslator.FromHtml("#090909");
            var xtraDarkGrey = ColorTranslator.FromHtml("#050505");
            if (!state2)
            {
                btnDarkMode.Text = "L";
                // värvide varieerimine "DARK MODE" jaoks
                // üldvärvid
                this.BackColor = xtraDarkGrey;
                this.ForeColor = chalkWhite;

                chartPrice.BackColor = darkGrey;
                // täpsustused
                chartPrice.ChartAreas["ChartArea1"].BackColor = xtraDarkGrey;

                chartPrice.ChartAreas["ChartArea1"].AxisX.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.ForeColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY.LabelStyle.ForeColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LabelStyle.ForeColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.MajorGrid.LineColor = chalkWhite;
                
                tablePrice.BackgroundColor = xtraDarkGrey;
                tablePrice.ForeColor = chalkWhite;
                tablePrice.DefaultCellStyle.BackColor = xtraDarkGrey;

                tablePackages.BackgroundColor = xtraDarkGrey;
                tablePackages.ForeColor = chalkWhite;
                tablePackages.DefaultCellStyle.BackColor = xtraDarkGrey;

                cbKasutusmall.BackColor = midGrey;
                cbKasutusmall.ForeColor = chalkWhite;
                txtAjakulu.BackColor = midGrey;
                txtAjakulu.ForeColor = chalkWhite;
                txtVoimsus.BackColor = midGrey;
                txtVoimsus.ForeColor = chalkWhite;
                txtHind.BackColor = midGrey;
                txtHind.ForeColor = chalkWhite;
                txtTarbimisAeg.BackColor = midGrey;
                txtTarbimisAeg.ForeColor = chalkWhite;
                txtDebug.BackColor = midGrey;
                txtDebug.ForeColor = chalkWhite;
                btnAvaCSV.BackColor = midGrey;
                btnAvaCSV.ForeColor = chalkWhite;
                txtCostNow.BackColor = midGrey;
                txtCostNow.ForeColor = chalkWhite;
                btnChangeSize.BackColor = midGrey;
                btnChangeSize.ForeColor = chalkWhite;
                btnDarkMode.BackColor = midGrey;
                btnDarkMode.ForeColor = chalkWhite;
                tbMonthlyPrice.BackColor = midGrey;
                tbMonthlyPrice.ForeColor = chalkWhite;
                groupPriceType.ForeColor = chalkWhite;

                btnOpenPackages.ForeColor = chalkWhite;
                btnOpenPackages.BackColor = midGrey;

                chartPrice.ChartAreas["ChartArea1"].BorderColor = chalkWhite;
                //Bigger = new Font("Impact", 16);
            }
            else
            {
                btnDarkMode.Text = "D";
                // värvide varieerimine "LIGHT MODE" jaoks
                this.BackColor = SystemColors.Control;
                this.ForeColor = Color.Black;
                chartPrice.BackColor = Color.White;
                chartPrice.ChartAreas["ChartArea1"].BackColor = SystemColors.Control;
                // täpsustused
                chartPrice.ChartAreas["ChartArea1"].BackColor = Color.White;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LineColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.ForeColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY.LineColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY.LabelStyle.ForeColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LineColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LabelStyle.ForeColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.MajorGrid.LineColor = Color.Black;

                tablePrice.BackgroundColor = SystemColors.ControlDark;
                tablePrice.ForeColor = Color.Black;
                tablePrice.DefaultCellStyle.BackColor = Color.White;

                tablePackages.BackgroundColor = SystemColors.ControlDark;
                tablePackages.ForeColor = Color.Black;
                tablePackages.DefaultCellStyle.BackColor = Color.White;

                cbKasutusmall.BackColor = Color.White;
                cbKasutusmall.ForeColor = Color.Black;
                txtAjakulu.BackColor = Color.White;
                txtAjakulu.ForeColor = Color.Black;
                txtVoimsus.BackColor = Color.White;
                txtVoimsus.ForeColor = Color.Black;
                txtHind.BackColor = SystemColors.Control;
                txtHind.ForeColor = Color.Black;
                txtTarbimisAeg.BackColor = SystemColors.Control;
                txtTarbimisAeg.ForeColor = Color.Black;
                txtDebug.BackColor = Color.White;
                txtDebug.ForeColor = Color.Black;
                btnAvaCSV.BackColor = SystemColors.Control;
                btnAvaCSV.ForeColor = Color.Black;
                txtCostNow.BackColor = SystemColors.Control;
                txtCostNow.ForeColor = Color.Black;
                btnChangeSize.BackColor = SystemColors.Control;
                btnChangeSize.ForeColor = Color.Black;
                btnDarkMode.BackColor = SystemColors.Control;
                btnDarkMode.ForeColor = Color.Black;
                tbMonthlyPrice.BackColor = SystemColors.Control;
                tbMonthlyPrice.ForeColor = Color.Black;
                groupPriceType.ForeColor = Color.Black;

                btnOpenPackages.BackColor = SystemColors.Control;
                btnOpenPackages.ForeColor = Color.Black;
            }
        }

        


        private void btnOpenPackages_Click(object sender, EventArgs e)
        {
            if (AP.chooseFilePackages())
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.paketiAndmed, AP.getPackageFileName());
                this.openCSVPackage();
            }
        }

        private void openCSVPackage()
        {
            if (!AP.readPackageFile(ref packageFileContents))
            {
                MessageBox.Show("Lugemine ebaõnnestus!");
            }
            else
            {
                packageData = AP.parsePackage(packageFileContents);

                tablePackages.Rows.Clear();
                foreach (var item in packageData)
                {
                    tablePackages.Rows.Add(
                        item.providerName,
                        item.packageName,
                        item.monthlyPrice.ToString("0.00"),
                        item.sellerMarginal.ToString("0.000"),
                        item.isStockPackage ? "-" : item.basePrice.ToString("0.000"),
                        (!item.isDayNight || item.isStockPackage) ? "-" : item.nightPrice.ToString("0.000"),
                        item.isStockPackage ? "Jah" : "Ei",
                        item.isGreenPackage ? "Jah" : "Ei"
                    );
                }
            }
        }

        private void Kasutajaliides_Resize(object sender, EventArgs e)
        {
            resizeGuiElement(originalChartPriceSize, chartPrice);
            resizeGuiElement(originalTablePriceSize, tablePrice);
            resizeGuiElement(originalTablePackagesSize, tablePackages);
            resizeGuiElement(originalButtonChangeSize, btnChangeSize);
            resizeGuiElement(originalLabelKasutusmall, lblKasutusmall);
            resizeGuiElement(originalComboBoxKasutusmall, cbKasutusmall);
            resizeGuiElement(originalLabelAeg, lblAeg);
            resizeGuiElement(originalTextAjakulu, txtAjakulu);
            resizeGuiElement(originalLabelTund, lblTund);
            resizeGuiElement(originalLabelV6imsus, lblVoimsus);
            resizeGuiElement(originalTextV6imsus, txtVoimsus);
            resizeGuiElement(originalLabelkW, lblkW);
            resizeGuiElement(originalLabelHind, lblHind);
            resizeGuiElement(originalTextHind, txtHind);
            resizeGuiElement(originalLabelAndresEek, lblAndresEek);
            resizeGuiElement(originalTextDebug, txtDebug);
            resizeGuiElement(originalButtonAvaCsv, btnAvaCSV);
            resizeGuiElement(originalCheckBoxShowPrice, cbShowPrice);
            resizeGuiElement(originalCheckBoxShowUsage, cbShowUsage);
            resizeGuiElement(originalCheckBoxShowTable, cbShowTabel);
            resizeGuiElement(originalLabelBeginning, lblBeginning);
            resizeGuiElement(originalDateStartTimePicker, dateStartTime);
            resizeGuiElement(originalLabelEnd, lblEnd);
            resizeGuiElement(originalDateStopTimePicker, dateStopTime);
            resizeGuiElement(originalGroupBoxGroupTypePrice, groupPriceType);
            resizeGuiElement(originalLabelTarbimisAeg, lblTarbimisAeg);
            resizeGuiElement(originalTextTarbimisAeg, txtTarbimisAeg);
            resizeGuiElement(originalLabelCostNow, lblCostNow);
            resizeGuiElement(originalTextCostNow, txtCostNow);
            resizeGuiElement(originalLabelSKwh2, lblSKwh2);
            resizeGuiElement(originalButtonDarkMode, btnDarkMode);
            resizeGuiElement(originalBtnOpenPackages, btnOpenPackages);
            Refresh(); // vajalik et ei tekiks "render glitche" (nt. ComboBox ei suurene korraks jms.)
        }

        // https://stackoverflow.com/questions/47463926/how-to-get-pixel-position-from-datetime-value-on-x-axis 
        // https://stackoverflow.com/questions/11955866/retrieving-datetime-x-axis-value-from-chart-control 
        void chartPrice_zooming(object sender, MouseEventArgs e)
        {
            dateStartTime.ValueChanged += dateStartTime_ValueChanged; // lülita DateTimePickeri muutuse handler sisse
            // rulliga kerimisel hiire asukoht X-telje suhtes; Andmetüübiks DateTime!
            var mousePositionDate = DateTime.FromOADate(chartPrice.ChartAreas["ChartArea1"].AxisX.PixelPositionToValue(e.Location.X));
            if (startTime < stopTime)
            {
                //MessageBox.Show(startTime.ToString() + " " + stopTime.ToString());
                var deltaDate = stopTime - startTime;
                var distanceFromToday = DateTime.Now - stopTime;
                //double xCoordPercent = (e.Location.X - 88) / 464.0; // jagades graafiku laiusega saame pointeri normaliseeritud asukoha graafikul vahemikus [0;1]
                double xCoordPercent = (mousePositionDate - startTime).TotalMilliseconds / (stopTime - startTime).TotalMilliseconds; // jagades graafiku laiusega saame pointeri normaliseeritud asukoha graafikul vahemikus [0;1]
                //MessageBox.Show(xCoordPercent.ToString());
                int leftStep = 1, rightStep = 1, totalStep = 1; // sammud graafiku otste nihutamiseks (tundides)
                if (deltaDate.TotalDays < 2)
                {
                    totalStep = 12;
                }
                else if (deltaDate.TotalDays < 10)
                {
                    totalStep = 24;
                }
                else if (deltaDate.TotalDays < 30)
                {
                    totalStep = 96;
                }
                else if(deltaDate.TotalDays < 60)
                {
                    totalStep = 192;
                }
                else
                {
                    totalStep = 384;
                }
                totalStep = 24;
                leftStep = Convert.ToInt16(xCoordPercent * totalStep);
                rightStep = totalStep - leftStep;
                //MessageBox.Show((mousePositionDate - startTime).TotalMilliseconds.ToString());
                if (xCoordPercent <= 1 && xCoordPercent >= 0)
                {
                    if (e.Delta > 0 && (startTime < stopTime) && deltaDate.TotalDays >= 2)
                    {
                        dateStartTime.Value = dateStartTime.Value.AddHours(leftStep);
                        dateStopTime.Value = dateStopTime.Value.AddHours(-rightStep);
                        startTime = startTime.AddHours(leftStep);
                        stopTime = stopTime.AddHours(-rightStep);
                    }
                    else if (e.Delta < 0)
                    {
                        if (distanceFromToday.TotalHours <= 2)
                        {
                            dateStopTime.Value = endOfDayDate; // ümardab alla täistunniks
                            stopTime = endOfDayDate;
                            dateStartTime.Value = dateStartTime.Value.AddHours(-totalStep);
                            startTime = startTime.AddHours(-totalStep);
                        }
                        else
                        {
                            dateStopTime.Value = dateStopTime.Value.AddHours(rightStep);
                            stopTime = stopTime.AddHours(rightStep);
                            dateStartTime.Value = dateStartTime.Value.AddHours(-leftStep);
                            startTime = startTime.AddHours(-leftStep);
                        }
                    }
                }
            }
            updateGraph();
            dateStartTime.ValueChanged -= dateStartTime_ValueChanged; // lülita DateTimePickeri muutuse handler välja
        }
    }
}