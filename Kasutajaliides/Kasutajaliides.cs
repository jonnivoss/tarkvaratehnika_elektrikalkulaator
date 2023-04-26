using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using PackageT = System.Collections.Generic.List<AndmePyydja.PackageInfo>;

namespace Kasutajaliides
{

    public partial class Kasutajaliides : Form
    {
        public Kasutajaliides()
        {
            InitializeComponent();
        }

        //tooltip seotud muutujad
        ToolTip toolTip = new ToolTip();
        string tooltipText;
        private int lastX;
        private int lastY;
        private VerticalLineAnnotation mouseTimeHover;

        // kasutajaliidese tekstifondid
        Font Normal = new Font("Impact", 12);
        Font Bigger = new Font("Impact", 16);
        // 
        List<DateTime> timeRange = new List<DateTime>();
        List<double> costRange = new List<double>();

        List<DateTime> priceTimeRange = new List<DateTime>();
        List<double> priceCostRange = new List<double>();

        List<double> packageCost = new List<double>(); // paketi hinna punktide jaoks

        VecT userData = new VecT();
        string fileContents, packageFileContents;

        VecT priceData = new VecT();
        PackageT packageData = new PackageT();

        private AndmePyydja.IAP AP = new AndmePyydja.CAP();
        private AndmeSalvestaja.IAS AS = new AndmeSalvestaja.CAS("settings.json");
        private Arvutaja.IArvutaja AR = new Arvutaja.CArvutaja();
        private CSVExporterDNF.IExporter CSV = new CSVExporterDNF.CExporter();

        DateTime endOfDayDate = DateTime.Now.Date.AddHours(48); // vastab järgmise päeva lõpule (24:00)
        DateTime zoomStartTime, zoomStopTime;
        bool showStock = true, showUsage = true;
        bool state = true;
        bool state2 = true; // DARK MODE BUTTON TOGGLE
        bool state3 = false; // graafiku vajutamisega suurendamise jaoks
        bool packageState = false;

        double averagePrice;

        Series packageSeries = new Series();
        string packageName;

        // Keskmise hinna joon graafikul
        HorizontalLineAnnotation averagePriceLine = new HorizontalLineAnnotation();

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
            // Tarbimise andmed
            timeRange.Clear();
            costRange.Clear();
            foreach (var item in userData)
            {
                if (item.Item1 >= dateStartTime.Value && item.Item1 <= dateStopTime.Value)
                {
                    timeRange.Add(item.Item1);
                    costRange.Add(item.Item2);
                }
            }

            priceTimeRange.Clear();
            priceCostRange.Clear();
            tablePrice.Rows.Clear();

            DateTime ajutineDate = new DateTime();
            double ajutinePrice = 0.0;

            averagePrice = 0.0;

            // Börsihinna andmed
            foreach (var item in priceData)
            {
                if (item.Item1 >= dateStartTime.Value && item.Item1 <= dateStopTime.Value)
                {
                    priceTimeRange.Add(item.Item1);
                    priceCostRange.Add(item.Item2 / 10.0);
                    tablePrice.Rows.Add(item.Item1, item.Item2 / 10.0);

                    // Keskmise hinna arvutamiseks hindade kokku liitmine
                    averagePrice += item.Item2 / 10.0; // s/kWh

                    ajutineDate = item.Item1.AddHours(1);
                    ajutinePrice = item.Item2 / 10.0;
                }
            }

            // Jagab kokkuliidetud hinnad hindade arvuga ==> keskmine hind
            averagePrice /= priceCostRange.Count;

            // Keskmise hinna joon
            chartPrice.Annotations.Remove(averagePriceLine);
            averagePriceLine.AxisY = chartPrice.ChartAreas["ChartArea1"].AxisY2;
            averagePriceLine.IsSizeAlwaysRelative = false;
            averagePriceLine.AnchorY = averagePrice;
            averagePriceLine.IsInfinitive = true;
            averagePriceLine.ClipToChartArea = chartPrice.ChartAreas["ChartArea1"].Name;
            averagePriceLine.LineColor = Color.BlueViolet;
            averagePriceLine.LineWidth = 1;
            averagePriceLine.Name = "priceLine";
            chartPrice.Annotations.Add(averagePriceLine);

            string line = "Keskmine hind: " + averagePrice.ToString();
            string line2 = "Max: " + chartPrice.ChartAreas["ChartArea1"].AxisY2.Maximum.ToString();
            txtDebug.AppendText(Environment.NewLine);
            txtDebug.AppendText(line);
            txtDebug.AppendText(line2);

            priceTimeRange.Add(ajutineDate);
            priceCostRange.Add(ajutinePrice);

            chartPrice.Series["Elektrihind"].Points.DataBindXY(priceTimeRange, priceCostRange);
            if (packageState)
            {
                for (int i = 0; i < tablePackages.SelectedRows.Count; ++i)
                {
                    packageName = tablePackages.SelectedRows[i].Index.ToString() + ": " + tablePackages.SelectedRows[i].Cells[2].Value.ToString();
                    packageCost.Clear();
                    foreach (var item in priceTimeRange)
                    {
                        packageCost.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[3].Value));
                    }
                    chartPrice.Series[packageName].Points.DataBindXY(priceTimeRange, packageCost);
                    //packageState = false;
                }
            }
            

            for (int i = 0; i < priceCostRange.Count; i++) // Käib valitud ajaintervalli hinnad läbi
            {
                if(i != priceCostRange.Count - 1) // Kui ei ole tegemist viimase hinnaga
                {
                    if (priceCostRange[i] < averagePrice) // Väiksem kui keskmine hind ==> roheline
                    {
                        chartPrice.Series["Elektrihind"].Points[i+1].Color = Color.Green;
                    }
                    else // Suurem kui keskmine hind ==> punane
                    {
                        chartPrice.Series["Elektrihind"].Points[i+1].Color = Color.Red;
                    }
                }
                else // Kui on tegemist viimase hinnaga
                {
                    if (priceCostRange[i] < averagePrice) // Väiksem kui keskmine hind ==> roheline
                    {
                        chartPrice.Series["Elektrihind"].Points[i].Color = Color.Green;
                    }
                    else // Suurem kui keskmine hind ==> punane
                    {
                        chartPrice.Series["Elektrihind"].Points[i].Color = Color.Red;
                    }
                }
                
            }

            chartPrice.Series["Elektrihind"].Enabled = showStock;
            chartPrice.Series["Tarbimine"].Enabled = showUsage && (timeRange.Count > 0);
            chartPrice.Invalidate();
            tablePrice.Invalidate();
            // Skaala reguleerimine:
            changeInterval(Convert.ToInt32((dateStopTime.Value - dateStartTime.Value).TotalHours));
            chartPrice.Series["Tarbimine"].Points.DataBindXY(timeRange, costRange);
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

        void toolTip_Popup(object sender, PopupEventArgs e)
        {
            Font f = (state) ? Normal : Bigger;
            e.ToolTipSize = TextRenderer.MeasureText(tooltipText, f);
        }

        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            //font ja värv
            Font f  = (state)?  Normal       : Bigger;
            Brush b = (state2)? Brushes.Black : Brushes.White;
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(tooltipText, f, b, new Point(2, 2));
        }

        private void chartPrice_MouseMove(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)sender;

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
                tooltipText = "hind: ";
                tooltipText += y.ToString("0.000") + "\n" + s.ToString("kell HH:00") + "\n" + s.ToString("dd/MM/yy");

                if (e.X != this.lastX || e.Y != this.lastY)
                {
                    toolTip.SetToolTip(chart, " ");

                    this.lastX = e.X;
                    this.lastY = e.Y;
                }
                
                //kui juba joon olemas ss kustuta ära et uus joonistada
                if (mouseTimeHover != null)
                {
                    chart.Annotations.Remove(mouseTimeHover);
                }
                //new line
                mouseTimeHover = new VerticalLineAnnotation();
                
                //joonistub x axise kohal
                mouseTimeHover.AxisX = chart.ChartAreas[0].AxisX;
                mouseTimeHover.X = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                
                //läbib tervet charti
                mouseTimeHover.IsInfinitive = true;
                mouseTimeHover.ClipToChartArea = ca.Name;
                
                //joone nimi
                mouseTimeHover.Name = "Elektrihind";
                
                //joone stiil värv ja laius
                mouseTimeHover.LineColor = Color.Green;
                mouseTimeHover.LineWidth = 1;
                mouseTimeHover.LineDashStyle = ChartDashStyle.Solid;

                //joonista joon
                chart.Annotations.Add(mouseTimeHover);
            }
            else
            {
                //kustuta tt ja joon
                chart.Annotations.Remove(mouseTimeHover);
                toolTip.Hide(chart);
            }
        }

        // Muudab graafiku X-teljele kantavate jaotiste tihedust (time scale density)
        private void changeInterval(int count)
        {
            // DateTimeInterval: 6 = Hours; 5 = Days. ERGO'S ARROGANT OPINION - SHORT IS GOOOOOOOOD! :P (KVAASI-HARGNEMISTETA VERSIOON)
            chartPrice.ChartAreas["ChartArea1"].AxisX.IntervalType = (DateTimeIntervalType)(6*(Convert.ToInt32(count <= 72)) + 5*(Convert.ToInt32(count > 72)));
            chartPrice.ChartAreas["ChartArea1"].AxisX.Interval = 0 + (Convert.ToInt32(count <= 12)) + 2*(Convert.ToInt32(count <= 24 && count > 12)) + 4*(Convert.ToInt32(count <= 48 && count > 24)) + 12*(Convert.ToInt32(count <= 72 && count > 48));
            //MessageBox.Show(count.ToString());
            //MessageBox.Show(("HH:mm ".Substring(5 - 5 * Convert.ToInt32(count <= 24)) + "dd/MM HH:mm ".Substring(11 - 11 * Convert.ToInt32(count <= 72 && count > 24)) + "dd/MM/yy ".Substring(9 - 9 * Convert.ToInt32(count > 72))).Replace(" ", "").ToString());
            chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = ("HH:mm ".Substring(5 - 5*Convert.ToInt32(count <= 24)) + "dd/MM HH:mm ".Substring(11 - 11*Convert.ToInt32(count <= 72 && count > 24)) + "dd/MM/yy ".Substring(9 - 9*Convert.ToInt32(count > 72))).Replace(" ", "");
            chartPrice.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
        }

        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            if (AP.chooseFileUserData())//ei
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed, AP.getUserDataFileName());
                this.openCSVUserData();
            }
        }
        private bool openCSVUserData()
        {
            bool ret = true;
            if (!AP.readUserDataFile(ref fileContents))
            {
                ret = false;
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
                }

                var timeRangeArr = timeRange.ToArray();
            }

            if ((this.dateStartTime.Value == default(DateTime)) || (this.dateStopTime.Value == default(DateTime)))
            {

                this.dateStartTime.Value += new TimeSpan(0, 0, 0);
                this.dateStopTime.Value += new TimeSpan(23, 00, 00);
                //dateStopTime.Value += new TimeSpan(24, 00, 00);
                txtDebug.AppendText("jeba\n\n");
            }

            priceTimeRange.Clear();
            priceCostRange.Clear();

            if (timeRange.Count == 0)
            {
                this.dateStartTime.Value = DateTime.Now.Date + new TimeSpan(0, 0, 0);
                this.dateStopTime.Value = DateTime.Now.Date + new TimeSpan(23, 00, 00);
                //dateStopTime.Value += new TimeSpan(24, 00, 00);
                txtDebug.AppendText("  kaas   ");
                callAPI(dateStartTime.Value, dateStopTime.Value);
            }
            else  // määra otspunktide vaikeväärtuseks tarbimisandmete algus- ja lõppaeg
            {
                dateStartTime.Value = timeRange.First();
                dateStopTime.Value = timeRange.Last();
                callAPI(timeRange.First().AddDays(-30), endOfDayDate);
            }
            updateGraph();
            return ret;
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
                    var beg = this.dateStartTime.Value;
                    var end = beg.Date + TimeSpan.FromHours(Math.Ceiling(time));
                    Console.WriteLine("Begin: " + beg.ToString() + "; end: " + end.ToString());

                    double bestIntegral = double.PositiveInfinity;
                    var bestDate = beg;

                    for (; end <= this.dateStopTime.Value;)
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
                }
                else
                {
                    var skwh = Double.Parse(tbMonthlyPrice.Text);
                    price = time * power * skwh / 100.0;
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
            // "MOUSE SCROLL" suurendus. EI KUSTUTA KURAT, kui vaja siis commenti välja!
            this.chartPrice.MouseWheel += new MouseEventHandler(chartPrice_zooming);
            // tarbimisandmete graafiku värv
            chartPrice.Series["Tarbimine"].Color = ColorTranslator.FromHtml("#0BD0D1");
            chartPrice.Series["Elektrihind"].Color = Color.Black;

            toolTip.OwnerDraw = true;
            toolTip.Draw += new DrawToolTipEventHandler(toolTip_Draw);
            toolTip.Popup += new PopupEventHandler(toolTip_Popup);
            toolTip.BackColor = SystemColors.Control;
            toolTip.ForeColor = Color.Black;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            toolTip.SetToolTip(chartPrice, null);

            this.BackColor = SystemColors.Control;

            this.MinimumSize = new Size(1083, 905);
            // Lisab tüüp-kasutusmallid
            txtHind.Text = "-";
            tablePrice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tablePackages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            VecT costNowData = AP.HindAegInternet(DateTime.Now, DateTime.Now);
            double costNow = 0.0;
            foreach (var item in costNowData)
            {
                if (item.Item1.Date == DateTime.Now.Date && item.Item1.Hour == DateTime.Now.Hour) {
                    costNow = item.Item2 / 10.0;
                    break;
                }
                
            }
            txtCostNow.Text = costNow.ToString("0.000");
            
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
            bool isUserData = openCSVUserData();
            bool isPackage = openCSVPackage();
            if (!isUserData || !isPackage)
            {
                MessageBox.Show("Lugemine ebaõnnestus!");
            }

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
                this.dateStartTime.Value = d;
            }
            else
            {
                this.dateStartTime.Value = d;
                dateStopTime.Value = d.Date + new TimeSpan(23, 59, 59);
                this.dateStopTime.Value = dateStopTime.Value;
            }
            priceChart_zoom(dateStartTime.Value, dateStopTime.Value); // uuendab ja suurendab graafikut!
            calcPrice();
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
                this.dateStopTime.Value = d;
            }
            else
            {
                this.dateStopTime.Value = d;
                dateStartTime.Value = d.Date + new TimeSpan(0, 0, 0);
                this.dateStartTime.Value = dateStartTime.Value;
            }
            priceChart_zoom(dateStartTime.Value, dateStopTime.Value); // uuendab ja suurendab graafikut!
            calcPrice();
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
                chartPrice.ChartAreas["ChartArea1"].AxisX.TitleForeColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY.LabelStyle.ForeColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY.TitleForeColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.LabelStyle.ForeColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.MajorGrid.LineColor = chalkWhite;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.TitleForeColor = chalkWhite;

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
                chartPrice.Series["Tarbimine"].Color = ColorTranslator.FromHtml("#0BD0D1");
                //Bigger = new Font("Impact", 16);

                toolTip.BackColor = midGrey;
                toolTip.ForeColor = chalkWhite;
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

                toolTip.BackColor = SystemColors.Control;
                toolTip.ForeColor = Color.Black;
                chartPrice.Series["Tarbimine"].Color = ColorTranslator.FromHtml("#0BD0D1");
                chartPrice.ChartAreas["ChartArea1"].AxisX.TitleForeColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.TitleForeColor = Color.Black;
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

        private bool openCSVPackage()
        {
            bool ret = true;
            if (!AP.readPackageFile(ref packageFileContents))
            {
                ret = false;
            }
            else
            {
                packageData = AP.parsePackage(packageFileContents);

                tablePackages.Rows.Clear();
                int i = 0;
                foreach (var item in packageData)
                {
                    tablePackages.Rows.Add(
                        i,
                        item.providerName,
                        item.packageName,
                        item.monthlyPrice.ToString("0.00"),
                        item.sellerMarginal.ToString("0.000"),
                        item.isStockPackage ? "-" : item.basePrice.ToString("0.000"),
                        (!item.isDayNight || item.isStockPackage) ? "-" : item.nightPrice.ToString("0.000"),
                        item.isStockPackage ? "Jah" : "Ei",
                        item.isGreenPackage ? "Jah" : "Ei"
                    );
                    ++i;
                }
            }
            return ret;
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

        private void tablePackages_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show("It works!");
            packageCost.Clear();
        
            packageState = tablePackages.SelectedRows.Count != 0;


            List<Series> removables = new List<Series>();
            // Eemaldab vanad, mida enam pole valitud
            foreach (var series in chartPrice.Series)
            {
                if (series.Name == "Elektrihind" || series.Name == "Tarbimine")
                {
                    continue;
                }

                bool removeItem = true;
                for (int j = 0; j < tablePackages.SelectedRows.Count; ++j)
                {
                    string seriesName = tablePackages.SelectedRows[j].Index.ToString() + ": " + tablePackages.SelectedRows[j].Cells[2].Value.ToString();
                    if (seriesName == series.Name)
                    {
                        removeItem = false;
                        break;
                    }
                }

                if (removeItem)
                {
                    removables.Add(series);
                }
            }
            foreach (var removable in removables)
            {
                chartPrice.Series.Remove(removable);
            }
            removables.Clear();


            // Lisab uued, mis on valitud
            for (int i = 0; i < tablePackages.SelectedRows.Count; ++i)
            {
                packageName = tablePackages.SelectedRows[i].Index.ToString() + ": " + tablePackages.SelectedRows[i].Cells[2].Value.ToString();
                if (chartPrice.Series.FindByName(packageName) != null)
                {
                    continue;
                }

                Random r = new Random();

                chartPrice.Series.Add(packageName);
                chartPrice.Series[packageName].ChartArea = "ChartArea1";
                chartPrice.Series[packageName].YAxisType = AxisType.Secondary;
                chartPrice.Series[packageName].Color     = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                chartPrice.Series[packageName].Legend    = "Legend1";
                chartPrice.Series[packageName].ChartType = SeriesChartType.Line;

                packageCost.Clear();
                foreach (var item in priceTimeRange)
                {
                    packageCost.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[3].Value));
                }
                chartPrice.Series[packageName].Points.DataBindXY(priceTimeRange, packageCost);
            }
            updateGraph();
        }

        private void tablePackages_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var index = e.RowIndex;

            // Unselect the line
            tablePackages.Rows[index].Selected = false;

            tablePackages_RowHeaderMouseClick(sender, e);
        }

        // https://stackoverflow.com/questions/47463926/how-to-get-pixel-position-from-datetime-value-on-x-axis 
        // https://stackoverflow.com/questions/11955866/retrieving-datetime-x-axis-value-from-chart-control 
        void chartPrice_zooming(object sender, MouseEventArgs e)
        {
            if (dateStartTime.Value < dateStopTime.Value)
            {
                // rulliga kerimisel hiire asukoht X-telje suhtes; Andmetüübiks DateTime!
                var mousePositionDate = DateTime.FromOADate(chartPrice.ChartAreas["ChartArea1"].AxisX.PixelPositionToValue(e.Location.X));
                var deltaDate = dateStopTime.Value - dateStartTime.Value;
                var distanceFromToday = DateTime.Now - dateStopTime.Value;
                double xCoordPercent = (mousePositionDate - dateStartTime.Value).TotalMilliseconds / (deltaDate).TotalMilliseconds; // jagades graafiku laiusega saame pointeri normaliseeritud asukoha graafikul vahemikus [0;1]
                int leftStep = 1, rightStep = 1, totalStep = 1; // sammud graafiku otste nihutamiseks (tundides)
                totalStep = 12*Convert.ToInt32(deltaDate.TotalDays <= 2) + 48*Convert.ToInt32(deltaDate.TotalDays <= 10 && deltaDate.TotalDays > 2) + 196*Convert.ToInt32(deltaDate.TotalDays <= 30 && deltaDate.TotalDays > 10) + 384*Convert.ToInt32(deltaDate.TotalDays > 30);
                leftStep = Convert.ToInt32(xCoordPercent * totalStep);
                rightStep = totalStep - leftStep;
                //MessageBox.Show((mousePositionDate - startTime).TotalMilliseconds.ToString());
                if (xCoordPercent <= 1 && xCoordPercent >= 0)
                {
                    if (e.Delta > 0 && (dateStartTime.Value < dateStopTime.Value) && deltaDate.TotalHours >= 12)
                    {
                        priceChart_zoom(dateStartTime.Value.AddHours(leftStep), dateStopTime.Value.AddHours(-rightStep));
                    }
                    else if (e.Delta < 0)
                    {
                        if (distanceFromToday.TotalHours <= 2)
                        {
                            priceChart_zoom(dateStartTime.Value.AddHours(-totalStep), endOfDayDate);
                        }
                        else
                        {
                            priceChart_zoom(dateStartTime.Value.AddHours(-leftStep), dateStopTime.Value.AddHours(rightStep));
                        }
                    }
                }
            }
        }

        void priceChart_MouseDown(object sender, MouseEventArgs e)
        {
            state3 = true;
            if (e.Button == MouseButtons.Left) // vasak hiireklahv ALLA
            {
                zoomStartTime = DateTime.FromOADate(chartPrice.ChartAreas["ChartArea1"].AxisX.PixelPositionToValue(e.Location.X));
                zoomStartTime = zoomStartTime.AddMilliseconds(-(zoomStartTime.Millisecond + 1000*zoomStartTime.Second + 60000*zoomStartTime.Minute));
            }
        }
        void priceChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (state3) // vasak hiireklahv ALLA
            {
                try
                {
                    zoomStopTime = DateTime.FromOADate(chartPrice.ChartAreas["ChartArea1"].AxisX.PixelPositionToValue(e.Location.X));
                }
                catch
                {
                    // You know nothing, Jon Snow...
                }
                if (zoomStopTime - zoomStartTime >= TimeSpan.FromHours(1) && zoomStartTime >= dateStartTime.Value && zoomStopTime < dateStopTime.Value)
                {
                    priceChart_zoom(zoomStartTime, zoomStopTime);
                }
            }
            state3 = false;
        }
        void priceChart_zoom(DateTime start, DateTime stop)
        {
            dateStartTime.Value = start;
            dateStopTime.Value = stop;
            if (dateStartTime.Value < priceData.First().Item1 || dateStopTime.Value > priceData.Last().Item1)
            {
                callAPI(dateStartTime.Value.Date.AddDays(-60), endOfDayDate); // kutsub API ainult vajadusel välja ja loeb seejuures korraga rohkem andmeid!
            }
            updateGraph();
            calcPrice();
        }

    }
}
