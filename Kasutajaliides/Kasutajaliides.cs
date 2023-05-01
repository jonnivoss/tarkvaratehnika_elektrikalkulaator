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

        // kasutajaliidese värvid
        // VÄRVID (RGB hex.)
        Color chalkWhite = ColorTranslator.FromHtml("#BBBBBB");
        Color midGrey = ColorTranslator.FromHtml("#202020");
        Color darkGrey = ColorTranslator.FromHtml("#090909");
        Color xtraDarkGrey = ColorTranslator.FromHtml("#050505");
        Color subtleGreen = ColorTranslator.FromHtml("#ddffdd");
        Color xtraDarkGreen = ColorTranslator.FromHtml("#054505");
        Color semiDarkRed = ColorTranslator.FromHtml("#bb2200");

        // Lubatud tähed, mida peale numbrite kirjutada tohib numbrikasti
        int[] numberBoxValidChars =
        {
             8, // Backspace
             1, // Ctrl+A
             3, // Ctrl+C
            22, // Ctrl+V
            13, // Enter
        };

        VecT userData = new VecT();
        VecT priceData = new VecT();
        PackageT packageInfo = new PackageT();

        private AndmePyydja.IAP AP = new AndmePyydja.CAP();
        private AndmeSalvestaja.IAS AS = new AndmeSalvestaja.CAS("settings.json");
        private Arvutaja.IArvutaja AR = new Arvutaja.CArvutaja();
        private CSVExporterDNF.IExporter CSV = new CSVExporterDNF.CExporter();
        // IDGAFAKT
        // a "friendly" reminder, mi deer amigo
        private VaheKiht.IVK VK = new VaheKiht.CVK();

        DateTime endOfDayDate = DateTime.Now.Date.AddHours(48); // vastab järgmise päeva lõpule (24:00)
        DateTime zoomStartTime, zoomStopTime;
        bool showStock = true, showUsage = true;
        bool state = true;
        bool isNotDarkMode = true; // DARK MODE BUTTON TOGGLE
        bool state3 = false; // graafiku vajutamisega suurendamise jaoks
        bool isPackageSelected = false;
        bool ret = false;

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

        Rectangle originalGroupExport;
        Rectangle originalLabelExportDelimiter;
        Rectangle originalTextExportDelimiter;
        Rectangle originalLabelExportQualifier;
        Rectangle originalTextExportQualifier;
        Rectangle originalCheckBoxExportAppend;
        Rectangle originalButtonExportSave;
        Rectangle originalButtonExportOpen;
        Rectangle originalLabelExportPath;
        Rectangle originalTextExportPath;


        private void updateGraph()
        {
            // Uuenda graafikut
            //Console.WriteLine("updateGraph!!!!");

            var start = dateStartTime.Value;
            var stop  = dateStopTime.Value;

            if (showUsage && !VK.createUserDataRange(this.userData, start, stop))
            {
                MessageBox.Show("kankel!");
                return;
            }
            if (showStock && !VK.createStockRange(this.priceData, start, stop))
            {
                MessageBox.Show("kankel!");
                return;
            }
            // Lisab lõppu ühe punkti juurde, et saada "ilusat" joont
            VK.addLastPoints();


            chartPrice.Series["Tarbimine"].Enabled = showUsage && (VK.userDataTimeRange.Count > 0);
            chartPrice.Series["Elektrihind"].Enabled = showStock;

            if (this.showUsage)
            {
                chartPrice.Series["Tarbimine"].Points.DataBindXY(VK.userDataTimeRange, VK.userDataUsageRange);
            }
            if (this.showStock)
            {
                chartPrice.Series["Elektrihind"].Points.DataBindXY(VK.priceTimeRange, VK.priceCostRange);
            }


            tablePrice.Rows.Clear();
            for (int i = 0; i < VK.priceTimeRange.Count; ++i)
            {
                tablePrice.Rows.Add(VK.priceTimeRange[i], VK.priceCostRange[i]);
            }

            if (showStock)
            {
                for (int i = 0; i < VK.priceCostRange.Count; i++) // Käib valitud ajaintervalli hinnad läbi
                {
                    if (i != VK.priceCostRange.Count - 1) // Kui ei ole tegemist viimase hinnaga
                    {
                        if (VK.priceCostRange[i] < VK.averagePrice) // Väiksem kui keskmine hind ==> roheline
                        {
                            chartPrice.Series["Elektrihind"].Points[i + 1].Color = Color.Green;
                        }
                        else // Suurem kui keskmine hind ==> punane
                        {
                            chartPrice.Series["Elektrihind"].Points[i + 1].Color = Color.Red;
                        }
                    }
                    else // Kui on tegemist viimase hinnaga
                    {
                        if (VK.priceCostRange[i] < VK.averagePrice) // Väiksem kui keskmine hind ==> roheline
                        {
                            chartPrice.Series["Elektrihind"].Points[i].Color = Color.Green;
                        }
                        else // Suurem kui keskmine hind ==> punane
                        {
                            chartPrice.Series["Elektrihind"].Points[i].Color = Color.Red;
                        }
                    }
                }
            }


            // Keskmise hinna joon
            chartPrice.Annotations.Remove(averagePriceLine);
            averagePriceLine.AxisY = chartPrice.ChartAreas["ChartArea1"].AxisY2;
            averagePriceLine.IsSizeAlwaysRelative = false;
            averagePriceLine.AnchorY = VK.averagePrice;
            averagePriceLine.IsInfinitive = true;
            averagePriceLine.ClipToChartArea = chartPrice.ChartAreas["ChartArea1"].Name;
            averagePriceLine.LineColor = Color.BlueViolet;
            averagePriceLine.LineWidth = 1;
            averagePriceLine.Name = "priceLine";
            chartPrice.Annotations.Add(averagePriceLine);

            string line = "Keskmine hind: " + VK.averagePrice.ToString();
            string line2 = "Max: " + chartPrice.ChartAreas["ChartArea1"].AxisY2.Maximum.ToString();
            txtDebug.AppendText(Environment.NewLine);
            txtDebug.AppendText(line);
            txtDebug.AppendText(line2);


            // pakettide graafikud
            if (isPackageSelected)
            {
                /*for (int i = 0; i < tablePackages.SelectedRows.Count; ++i)
                {
                    string packageName = tablePackages.SelectedRows[i].Index.ToString() + ": " + tablePackages.SelectedRows[i].Cells[2].Value.ToString();
                    //var packageCost = new List<double>();
                    /*foreach (var item in VK.priceTimeRange)
                    {
                        packageCost.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[3].Value));
                    }*/
                //chartPrice.Series[packageName].Points.DataBindXY(VK.priceTimeRange, packageCost);
                //}


                for (int i = 0; i < tablePackages.SelectedRows.Count; ++i)
                {
                    string packageName = tablePackages.SelectedRows[i].Index.ToString() + ": " + tablePackages.SelectedRows[i].Cells[2].Value.ToString();
                    var costPerKwh = new List<double>();

                    if (tablePackages.SelectedRows[i].Cells[7].Value.ToString() == "Jah")
                    {
                        foreach (var item in VK.priceRange)
                        {
                            costPerKwh.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[4].Value) + item.Item2);
                        }
                    }
                    else // kui ei ole tegemist börsipaketiga
                    {
                        // Kui on ainult päevane hind
                        if (tablePackages.SelectedRows[i].Cells[6].Value.ToString() == "-")
                        {
                            for (int j = 0; j < VK.priceTimeRange.Count; ++j)
                            {
                                costPerKwh.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[5].Value) + Convert.ToDouble(tablePackages.SelectedRows[i].Cells[4].Value));
                            }
                        }
                        else
                        {
                            for (int j = 0; j < VK.priceTimeRange.Count; ++j)
                            {
                                if (AR.isDailyRate(VK.priceTimeRange[j]))
                                {
                                    costPerKwh.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[5].Value) + Convert.ToDouble(tablePackages.SelectedRows[i].Cells[4].Value));
                                }
                                else
                                {
                                    costPerKwh.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[6].Value) + Convert.ToDouble(tablePackages.SelectedRows[i].Cells[4].Value));
                                }
                            }
                        }
                    }
                    chartPrice.Series[packageName].Points.DataBindXY(VK.priceTimeRange, costPerKwh);

                    // pakettide graafikud hind-tarbimine
                    if (showUsage && ret)
                    {
                        string packageNameUsage = packageName + " tarbimisel";


                        var packageUsageCost = new List<double>();
                        if (VK.userDataTimeRange.Count == 0)
                        {
                            try
                            {
                                chartPrice.Series[packageNameUsage].Enabled = false;
                            }
                            catch (Exception)
                            {

                            }
                            continue;
                        }

                        // kui tegemist on börsipaketiga
                        if (tablePackages.SelectedRows[i].Cells[7].Value.ToString() == "Jah")
                        {
                            var stockCost = VK.createRange(this.priceData, VK.userDataTimeRange.First(), VK.userDataTimeRange.Last());
                            List<double> stockCostWithMarginals = new List<double>();
                            foreach (var item in stockCost)
                            {
                                double newPrice = item.Item2 + Convert.ToDouble(tablePackages.SelectedRows[i].Cells[4].Value);

                                stockCostWithMarginals.Add(newPrice);
                            }

                            var stockUsageCost = VK.userDataUsageRange.Zip(stockCostWithMarginals, (u, c) => new { Usage = u, Cost = c });
                            foreach (var uc in stockUsageCost)
                            {
                                packageUsageCost.Add(uc.Usage * uc.Cost);
                            }
                        }
                        else // kui ei ole tegemist börsipaketiga
                        {
                            // Kui on ainult päevane hind
                            if (tablePackages.SelectedRows[i].Cells[6].Value.ToString() == "-")
                            {
                                foreach (var item in VK.userDataUsageRange)
                                {
                                    packageUsageCost.Add(item * costPerKwh.First());
                                }
                            }
                            else
                            {
                                for (int j = 0; j < VK.userDataUsageRange.Count; ++j)
                                {
                                    packageUsageCost.Add(VK.userDataUsageRange[j] * costPerKwh[j]);
                                }
                            }
                        }
                        try
                        {
                            chartPrice.Series[packageNameUsage].Points.DataBindXY(VK.userDataTimeRange, packageUsageCost);
                            chartPrice.Series[packageNameUsage].Enabled = true;
                            txtDebug.AppendText("Alustan tabelisse lisamist");
                            tablePrice.Rows.Clear();
                            for (int c = 0; c < VK.userDataTimeRange.Count; ++c)
                            {
                                tablePrice.Rows.Add(VK.priceTimeRange[c], VK.priceCostRange[c], packageUsageCost[c]);
                                //tablePrice.Rows.Add(costPerKwh[i], packageUsageCost[i]);
                            }
                            txtDebug.AppendText("Tabelisse lisatud");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            this.updatePakettideTarbimishind();

            chartPrice.Invalidate();
            tablePrice.Invalidate();
            // Skaala reguleerimine:
            changeInterval(Convert.ToInt32((stop - start).TotalHours));
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
            Brush b = (isNotDarkMode)? Brushes.Black : Brushes.White;
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
                for (int i = 0; i < VK.priceTimeRange.Count; ++i)
                {
                    if (VK.priceTimeRange[i].Hour == s.Hour && VK.priceTimeRange[i].Date == s.Date)
                    {
                        y = VK.priceCostRange[i];
                        this.updatePakettideHinnad(VK.priceTimeRange[i]);
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
                //this.updatePakettideHinnad(default(DateTime));
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
            // TrimEnd eemaldab lõpust üleliigsed tühikud
            chartPrice.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = ("HH:mm ".Substring(6 - 6*Convert.ToInt32(count <= 24)) + "dd/MM HH:mm ".Substring(12 - 12*Convert.ToInt32(count <= 72 && count > 24)) + "dd/MM/yy ".Substring(9 - 9*Convert.ToInt32(count > 72))).TrimEnd();
            chartPrice.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
        }

        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            if (AP.chooseFileUserData())//ei
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed, AP.userDataFileName);
                this.openCSVUserData();
            }
        }
        private bool openCSVUserData()
        {
            ret = true;
            string fileContents = "";
            if (!AP.readUserDataFile(ref fileContents))
            {
                ret = false;
            }
            else
            {
                this.userData = AP.parseUserData(fileContents);
            }

            if ((this.dateStartTime.Value == default(DateTime)) || (this.dateStopTime.Value == default(DateTime)))
            {

                this.dateStartTime.Value += new TimeSpan(0, 0, 0);
                this.dateStopTime.Value += new TimeSpan(23, 00, 00);
                //dateStopTime.Value += new TimeSpan(24, 00, 00);
                txtDebug.AppendText("jeba\n\n");
            }

            if (this.userData.Count == 0)
            {
                this.dateStartTime.Value = DateTime.Now.Date + new TimeSpan(0, 0, 0);
                this.dateStopTime.Value = DateTime.Now.Date + new TimeSpan(23, 00, 00);
                //dateStopTime.Value += new TimeSpan(24, 00, 00);
                txtDebug.AppendText("  kaas   ");
                this.priceData = AP.HindAegInternet(dateStartTime.Value, dateStopTime.Value);
            }
            else  // määra otspunktide vaikeväärtuseks tarbimisandmete algus- ja lõppaeg
            {
                dateStartTime.Value = this.userData.First().Item1;
                dateStopTime.Value = this.userData.Last().Item1;
                this.priceData = AP.HindAegInternet(this.userData.First().Item1.AddDays(-30), endOfDayDate);
            }
            

            updateGraph();
            return ret;
        }

        private void calcPrice()
        {
            double time = 0.0, power = 0.0, price;
            DateTime bestDate = default(DateTime);

            try
            {
                time  = Double.Parse(txtAjakulu.Text);
                power = Double.Parse(txtVoimsus.Text);
                // Do some crazy price calculation
                int smRet = 0;
                if (rbStockPrice.Checked)
                {
                    // Sööstab arvutajasse, leiab valitud ajavahemikust optimaalseima ajapikkuse
                    Console.WriteLine("Time: " + time.ToString());

                    var now = DateTime.Now.Date + new TimeSpan(DateTime.Now.Hour, 0, 0);

                    var beg = (this.dateStartTime.Value < now) ? now : this.dateStartTime.Value;
                    //var end = beg + TimeSpan.FromHours(Math.Ceiling(time));

                    //Console.WriteLine("Begin: " + beg.ToString() + "; end: " + end.ToString());


                    double bestIntegral;

                    smRet = AR.smallestIntegral(
                        this.VK.priceRange,
                        power,
                        time,
                        beg,
                        dateStopTime.Value,
                        out bestIntegral,
                        out bestDate
                    );

                    price = bestIntegral / 100.0;
                    if ((this.dateStopTime.Value < DateTime.Now) || (smRet != 0))
                    {
                        txtTarbimisAeg.Text = "-";
                    }
                    else
                    {
                        txtTarbimisAeg.Text = bestDate.ToString("dd.MM.yyyy HH:mm");
                    }
                }
                else
                {
                    bestDate = dateStartTime.Value;
                    var skwh = Double.Parse(tbMonthlyPrice.Text);
                    // Teisendab sentidest eurodesse
                    price = time * power * skwh / 100.0;
                }
                if (smRet != 0)
                {
                    // arvutab tarbimishinna keskmise hinnaga
                    price = time * power * VK.averagePrice / 100.0;
                }

                // Arvutab korrektse lõpphinna, lisab käibemaksu
                price *= 1.2;

                // Ümardab 3 komakohani ja ümardab üles
                txtHind.Text = Math.Round(price + 0.0005, 3).ToString();
            }
            catch (Exception)
            {
            }

            this.updatePakettideMallid(bestDate, time, power);
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
            this.updateCostNow();
            
            // Proovib avada CSV
            AS.loadFile();

            var useCases = AS.getUseCases();
            cbKasutusmall.Items.Clear();
            foreach (var i in useCases)
            {
                cbKasutusmall.Items.Add(i.Key);
            }

            AP.userDataFileName = AS.getSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed);
            AP.packageFileName = AS.getSetting(AndmeSalvestaja.ASSetting.paketiAndmed);
            //priceData = AP.HindAegInternet(DateTime.Now.Date.AddDays(-60), DateTime.Now);
            //MessageBox.Show(priceTimeRange.Last().ToString());
            bool isUserData = openCSVUserData();
            bool isPackage = openCSVPackage();
            if (!isUserData || !isPackage)
            {
                MessageBox.Show(
                    "CSV Lugemine ebaõnnestus!",
                    this.Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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

            originalGroupExport = new Rectangle(groupExport.Location.X, groupExport.Location.Y, groupExport.Size.Width, groupExport.Size.Height);
            originalLabelExportDelimiter = new Rectangle(lblExportDelimiter.Location.X, lblExportDelimiter.Location.Y, lblExportDelimiter.Size.Width, lblExportDelimiter.Size.Height);
            originalTextExportDelimiter = new Rectangle(txtExportDelimiter.Location.X, txtExportDelimiter.Location.Y, txtExportDelimiter.Size.Width, txtExportDelimiter.Size.Height);
            originalLabelExportQualifier = new Rectangle(lblExportQualifier.Location.X, lblExportQualifier.Location.Y, lblExportQualifier.Size.Width, lblExportQualifier.Size.Height);
            originalTextExportQualifier = new Rectangle(txtExportQualifier.Location.X, txtExportQualifier.Location.Y, txtExportQualifier.Size.Width, txtExportQualifier.Size.Height);
            originalCheckBoxExportAppend = new Rectangle(cbExportAppend.Location.X, cbExportAppend.Location.Y, cbExportAppend.Size.Width, cbExportAppend.Size.Height);
            originalButtonExportSave = new Rectangle(btnExportSave.Location.X, btnExportSave.Location.Y, btnExportSave.Size.Width, btnExportSave.Size.Height);
            originalButtonExportOpen = new Rectangle(btnExportOpen.Location.X, btnExportOpen.Location.Y, btnExportOpen.Size.Width, btnExportOpen.Size.Height);
            originalLabelExportPath = new Rectangle(lblExportPath.Location.X, lblExportPath.Location.Y, lblExportPath.Size.Width, lblExportPath.Size.Height);
            originalTextExportPath = new Rectangle(txtExportPath.Location.X, txtExportPath.Location.Y, txtExportPath.Size.Width, txtExportPath.Size.Height);
        }


        private DateTime lastUpdated;
        private void updateCostNow()
        {
            var time = DateTime.Now;
            time = time.Date + new TimeSpan(time.Hour, 0, 0);

            // Kontrollib, kas uuendamine on põhjendatud
            if (lastUpdated == time)
            {
                return;
            }

            VecT costNowData = AP.HindAegInternet(time, time.AddHours(1));
            double costNow = Double.NegativeInfinity;
            foreach (var item in costNowData)
            {
                if (item.Item1.Date == DateTime.Now.Date && item.Item1.Hour == DateTime.Now.Hour)
                {
                    costNow = item.Item2;
                    break;
                }
            }
            if (costNow == Double.NegativeInfinity)
            {
                txtCostNow.Text = "Error!";
            }
            else
            {
                txtCostNow.Text = costNow.ToString("0.000");
                lastUpdated = time;
            }
        }

        int handleNumberBoxKeyPress(string text, KeyPressEventArgs e, double maxLimit = Double.PositiveInfinity)
        {
            double parsedValue;
            bool isParsed = double.TryParse(text + e.KeyChar, out parsedValue);
            if ((isParsed && (parsedValue > maxLimit)) || (!isParsed && !numberBoxValidChars.Contains(e.KeyChar)))
            {
                e.Handled = true;
                if (!isParsed)
                {
                    MessageBox.Show("Palun sisestage ainult numbreid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 2;
                }
                return 1;
            }
            return 0;
        }

        private void txtAjakulu_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.handleNumberBoxKeyPress(txtAjakulu.Text, e, 1e6);
        }

        private void txtVoimsus_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.handleNumberBoxKeyPress(txtVoimsus.Text, e, 1e4);
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
                btnChangeSize.Font = Bigger;
                btnDarkMode.Font = Bigger;
                 
                groupExport.Font = new Font("Impact", 12);
                lblExportDelimiter.Font = Bigger;
                txtExportDelimiter.Font = Bigger;
                lblExportQualifier.Font = Bigger;
                txtExportQualifier.Font = Bigger;
                cbExportAppend.Font = Bigger;
                btnExportSave.Font = Bigger;
                btnExportOpen.Font = Bigger;
                lblExportPath.Font = Bigger;
                txtExportPath.Font = Bigger;

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
                btnChangeSize.Font = Normal;
                btnDarkMode.Font = Normal;

                groupExport.Font = new Font("Impact", 9);
                lblExportDelimiter.Font = Normal;
                txtExportDelimiter.Font = Normal;
                lblExportQualifier.Font = Normal;
                txtExportQualifier.Font = Normal;
                cbExportAppend.Font = Normal;
                btnExportSave.Font = Normal;
                btnExportOpen.Font = Normal;
                lblExportPath.Font = Normal;
                txtExportPath.Font = Normal;
                state = true;
            }
        }

        private void cbShowUsage_CheckedChanged(object sender, EventArgs e)
        {
            var state = cbShowUsage.Checked;
            if (state)
            {
                this.showUsage = true;
            }
            else
            {
                this.showUsage = false;
            }
            updateGraph();
        }

        private void tbMonthlyPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.handleNumberBoxKeyPress(tbMonthlyPrice.Text, e, 1e4);
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
            isNotDarkMode = !isNotDarkMode;

            if (!isNotDarkMode)
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

                groupExport.ForeColor = chalkWhite;
                lblExportDelimiter.ForeColor = chalkWhite;
                txtExportDelimiter.ForeColor = chalkWhite;
                txtExportDelimiter.BackColor = midGrey;
                lblExportQualifier.ForeColor = chalkWhite;
                txtExportQualifier.ForeColor = chalkWhite;
                txtExportQualifier.BackColor = midGrey;
                cbExportAppend.ForeColor = chalkWhite;
                btnExportSave.ForeColor = chalkWhite;
                btnExportSave.BackColor = midGrey;
                btnExportOpen.ForeColor = chalkWhite;
                btnExportOpen.BackColor = midGrey;
                lblExportPath.ForeColor = chalkWhite;
                txtExportPath.ForeColor = chalkWhite;
                txtExportPath.BackColor = midGrey;

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

                groupExport.ForeColor = Color.Black;
                lblExportDelimiter.ForeColor = Color.Black;
                txtExportDelimiter.ForeColor = Color.Black;
                txtExportDelimiter.BackColor = Color.White;
                lblExportQualifier.ForeColor = Color.Black;
                txtExportQualifier.ForeColor = Color.Black;
                txtExportQualifier.BackColor = Color.White;
                cbExportAppend.ForeColor = Color.Black;
                btnExportSave.ForeColor = Color.Black;
                btnExportSave.BackColor = SystemColors.Control;
                btnExportOpen.ForeColor = Color.Black;
                btnExportOpen.BackColor = SystemColors.Control;
                lblExportPath.ForeColor = Color.Black;
                txtExportPath.ForeColor = Color.Black;
                txtExportPath.BackColor = Color.White;

                btnOpenPackages.BackColor = SystemColors.Control;
                btnOpenPackages.ForeColor = Color.Black;

                toolTip.BackColor = SystemColors.Control;
                toolTip.ForeColor = Color.Black;
                chartPrice.Series["Tarbimine"].Color = ColorTranslator.FromHtml("#0BD0D1");
                chartPrice.ChartAreas["ChartArea1"].AxisX.TitleForeColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY.TitleForeColor = Color.Black;
                chartPrice.ChartAreas["ChartArea1"].AxisY2.TitleForeColor = Color.Black;
            }

            this.drawGreenPacketColumn(ref tablePackages, 8);
            this.updatePakettideVarvid();
        }

        


        private void btnOpenPackages_Click(object sender, EventArgs e)
        {
            if (AP.chooseFilePackages())
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.paketiAndmed, AP.packageFileName);
                this.openCSVPackage();
            }
        }

        private bool openCSVPackage()
        {
            bool ret = true;
            string packageFileContents = "";
            if (!AP.readPackageFile(ref packageFileContents))
            {
                ret = false;
            }
            else
            {
                this.packageInfo = AP.parsePackage(packageFileContents);

                tablePackages.Rows.Clear();
                int i = 0;
                foreach (var item in this.packageInfo)
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
                        item.isGreenPackage ? "Jah" : "Ei",
                        "-",
                        "-",
                        "-"
                    );
                    ++i;
                }
                this.updatePakettideTarbimishind();

                this.drawGreenPacketColumn(ref tablePackages, 8);
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

            resizeGuiElement(originalGroupExport, groupExport);
            resizeGuiElement(originalLabelExportDelimiter, lblExportDelimiter);
            resizeGuiElement(originalTextExportDelimiter, txtExportDelimiter);
            resizeGuiElement(originalLabelExportQualifier, lblExportQualifier);
            resizeGuiElement(originalTextExportQualifier, txtExportQualifier);
            resizeGuiElement(originalCheckBoxExportAppend, cbExportAppend);
            resizeGuiElement(originalButtonExportSave, btnExportSave);
            resizeGuiElement(originalButtonExportOpen, btnExportOpen);
            resizeGuiElement(originalLabelExportPath, lblExportPath);
            resizeGuiElement(originalTextExportPath, txtExportPath);

            Refresh(); // vajalik et ei tekiks "render glitche" (nt. ComboBox ei suurene korraks jms.)
        }

        private void tablePackages_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show("It works!");
            isPackageSelected = tablePackages.SelectedRows.Count != 0;


            List<Series> removables = new List<Series>();
            List<DataGridViewColumn> removableColumns = new List<DataGridViewColumn>();
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
                    string seriesNameUsage = tablePackages.SelectedRows[j].Index.ToString() + ": " + tablePackages.SelectedRows[j].Cells[2].Value.ToString() + " tarbimisel";
                    if (seriesName == series.Name || seriesNameUsage == series.Name)
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

            foreach (DataGridViewColumn column in tablePrice.Columns)
            {
                if (column.Equals(Aeg) || column.Equals(Hind))
                {
                    continue;
                }

                bool removeItem = true;
                for (int j = 0; j < tablePackages.SelectedRows.Count; ++j)
                {
                    string seriesName = tablePackages.SelectedRows[j].Index.ToString() + ": " + tablePackages.SelectedRows[j].Cells[2].Value.ToString();
                    //string seriesNameUsage = tablePackages.SelectedRows[j].Index.ToString() + ": " + tablePackages.SelectedRows[j].Cells[2].Value.ToString() + " tarbimisel";
                    if (column.Equals(seriesName))
                    {
                        removeItem = false;
                        break;
                    }
                }
                if (removeItem)
                {
                    removableColumns.Add(column);
                }
            }

            foreach (var removable in removables)
            {
                chartPrice.Series.Remove(removable);
            }
            removables.Clear();

            foreach(DataGridViewColumn column in removableColumns)
            {
                tablePrice.Columns.Remove(column);
            }

            // Lisab uued, mis on valitud
            for (int i = 0; i < tablePackages.SelectedRows.Count; ++i)
            {
                string packageName = tablePackages.SelectedRows[i].Index.ToString() + ": " + tablePackages.SelectedRows[i].Cells[2].Value.ToString();
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

                var packageCost = new List<double>();
                foreach (var item in VK.priceTimeRange)
                {
                    packageCost.Add(Convert.ToDouble(tablePackages.SelectedRows[i].Cells[3].Value));
                }
                chartPrice.Series[packageName].Points.DataBindXY(VK.priceTimeRange, packageCost);
            }

            

            // Kui tarbimine on valitud, siis lisab paketi graafiku selle põhjal
            if (showUsage && ret)
            {
                for (int i = 0; i < tablePackages.SelectedRows.Count; ++i)
                {
                    string packageNameUsage = tablePackages.SelectedRows[i].Index.ToString() + ": " + tablePackages.SelectedRows[i].Cells[2].Value.ToString() + " tarbimisel";
                    if (chartPrice.Series.FindByName(packageNameUsage) != null)
                    {
                        continue;
                    }
                    
                    if (VK.userDataTimeRange.Count == 0)
                    {
                        Console.WriteLine("Ei saa graafikut lisada!");
                        continue;
                    }
                    
                    Random r = new Random();

                    chartPrice.Series.Add(packageNameUsage);
                    chartPrice.Series[packageNameUsage].ChartArea = "ChartArea1";
                    chartPrice.Series[packageNameUsage].YAxisType = AxisType.Secondary;
                    chartPrice.Series[packageNameUsage].Color = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                    chartPrice.Series[packageNameUsage].Legend = "Legend1";
                    chartPrice.Series[packageNameUsage].ChartType = SeriesChartType.Line;

                    var newColumn = new DataGridViewTextBoxColumn();
                    newColumn.HeaderText = packageNameUsage;
                    newColumn.Name = packageNameUsage;

                    tablePrice.Columns.Add(newColumn);
                }

                
            }

            updateGraph();
        }

        private void tablePackages_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var index = e.RowIndex;

            // Unselect the line
            tablePackages.Rows[index].Selected = false;
            // Update graph based on selected items
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

        private void tmrCostNow_Tick(object sender, EventArgs e)
        {
            this.updateCostNow();
            //txtDebug.AppendText("Kontrollisin hinda...");
            //txtDebug.AppendText(Environment.NewLine);
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

        private void btnExportSave_Click(object sender, EventArgs e)
        {
            if (txtExportPath.Text.Length == 0)
            {
                btnExportOpen_Click(sender, e);
            }
            if (txtExportPath.Text.Length == 0)
            {
                MessageBox.Show("Faili pole valitud!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string errorStr = "";
            if (txtExportDelimiter.Text == "")
            {
                errorStr += "Eksporditava CSV eraldajat pole valitud!\n";
            }
            if (txtExportQualifier.Text == "")
            {
                errorStr += "Eksporditava CSV sõne kvalifikaatorit pole valitud!";
            }
            // Eemaldab üleliigse newline'i
            errorStr.Trim('\n');

            if (errorStr.Length > 0)
            {
                MessageBox.Show(errorStr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Proovib kirjutada faili tabeli andmed
            var isAppend = cbExportAppend.Checked;

            List<string[]> exportStrings = new List<string[]>();
            // Tabeli header
            exportStrings.Add(new string[] { "Aeg", "Börsihind (s/kWh)" });
            foreach (var item in VK.priceRange)
            {
                exportStrings.Add(new string[] { item.Item1.ToString(), item.Item2.ToString("0.000") });
            }

            // Teeb 2-dimensionaalse array, mis hoiab exportString.Count * 2 stringi
            var exportData = Array.CreateInstance(typeof(string), exportStrings.Count, 2);
            // Täidab selle array
            for (int i = 0; i < exportStrings.Count; ++i)
            {
                exportData.SetValue(exportStrings[i][0], i, 0);
                exportData.SetValue(exportStrings[i][1], i, 1);
            }

            CSV.delimiter = txtExportDelimiter.Text;
            CSV.textQualifier = txtExportQualifier.Text;
            int numLines = (int)CSV.saveDataToCsv(ref exportData, isAppend);

            if (numLines != exportStrings.Count)
            {
                MessageBox.Show("Faili kirjutamine nurjus! Kirjutati " + numLines.ToString() + " rida.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportOpen_Click(object sender, EventArgs e)
        {
            txtExportPath.Text = (string)CSV.setFileToSave();
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
                priceData = AP.HindAegInternet(dateStartTime.Value.Date.AddDays(-60), endOfDayDate); // kutsub API ainult vajadusel välja ja loeb seejuures korraga rohkem andmeid!
            }
            updateGraph();
            calcPrice();
        }

        void drawGreenPacketColumn(ref DataGridView table, int greenPacketRow)
        {
            for (int i = 0; i < table.Rows.Count; ++i)
            {
                // Kui reas on vähem celle, siis väldib out-of-bounds viga
                if (table.Rows[i].Cells.Count <= greenPacketRow)
                {
                    break;
                }

                if (table.Rows[i].Cells[greenPacketRow].Value.ToString() == "Jah")
                {
                    // Värvib vastavalt värviskeemile rohepaketid roheliseks
                    table.Rows[i].Cells[greenPacketRow].Style.BackColor = isNotDarkMode ? subtleGreen : xtraDarkGreen;
                }
                else
                {
                    // tundub, et lihtsaim viis stiili resettimiseks
                    table.Rows[i].Cells[greenPacketRow].Style = null;
                }
            }
        }
        void resetRowColor(ref DataGridView table, int rowIdx, int skipRow)
        {
            for (int i = 0; i < table.Rows[rowIdx].Cells.Count; ++i)
            {
                // Jätab rohepaketi tulba vahele
                if (i == skipRow)
                {
                    continue;
                }
                table.Rows[rowIdx].Cells[i].Style = null;
            }
        }
        void setRowColor(ref DataGridView table, int rowIdx, Color foreColor, Color backColor, int skipRow)
        {
            for (int i = 0; i < table.Rows[rowIdx].Cells.Count; ++i)
            {
                // Jätab rohepaketi tulba vahele
                if (i == skipRow)
                {
                    continue;
                }
                // Seab uue värvi ainult siis kui värv tõesti muutuks, väldib mõttetuid uuesti joonistamisi
                if (table.Rows[rowIdx].Cells[i].Style.ForeColor != foreColor)
                {
                    table.Rows[rowIdx].Cells[i].Style.ForeColor = foreColor;
                }
                if (table.Rows[rowIdx].Cells[i].Style.BackColor != backColor)
                {
                    table.Rows[rowIdx].Cells[i].Style.BackColor = backColor;
                }
            }
        }

        private void updatePakettideVarvid()
        {
            // Leiab minimaalse ja maksimaalse hetkehinnaga müüja
            Tuple<int, double> minRida = Tuple.Create(0, Double.PositiveInfinity), maxRida = Tuple.Create(0, Double.NegativeInfinity);
            for (int i = 0; i < tablePackages.Rows.Count; ++i)
            {
                var price = Double.Parse(tablePackages.Rows[i].Cells[9].Value.ToString());

                if (price < minRida.Item2)
                {
                    minRida = Tuple.Create(i, price);
                }
                if (price > maxRida.Item1)
                {
                    maxRida = Tuple.Create(i, price);
                }
            }
            // Teeb kõik minimaalse ja maksimaalse hinnaga müüjad vastavalt roheliseks/punaseks,
            // Käib kogu tabeli läbi, sest võib eksisteerida mitu sama hinnaga müüjat
            for (int i = 0; i < tablePackages.Rows.Count; ++i)
            {
                if (Math.Abs(Double.Parse(tablePackages.Rows[i].Cells[9].Value.ToString()) - minRida.Item2) < 0.0005)
                {
                    this.setRowColor(ref tablePackages, i, isNotDarkMode ? chalkWhite : Color.Black, isNotDarkMode ? Color.DarkGreen : Color.LightGreen, 8);
                    //Console.WriteLine("Pakett " + i.ToString() + " on odavaim!");
                }
                else if (Math.Abs(Double.Parse(tablePackages.Rows[i].Cells[9].Value.ToString()) - maxRida.Item2) < 0.0005)
                {
                    this.setRowColor(ref tablePackages, i, isNotDarkMode ? Color.White : Color.Black, isNotDarkMode ? semiDarkRed : Color.Red, 8);
                    //Console.WriteLine("Pakett " + i.ToString() + " on kalleim!");
                }
                else
                {

                    this.resetRowColor(ref tablePackages, i, 8);
                }
            }
        }

        private void updatePakettideHinnad(DateTime time)
        {
            // Kui tahetakse hetkehinna näitu eemaldada
            if (time == default(DateTime))
            {
                for (int i = 0; i < tablePackages.Rows.Count; ++i)
                {
                    tablePackages.Rows[i].Cells[9].Value = "-";
                    this.resetRowColor(ref tablePackages, i, 8);
                }
            }
            else
            {
                for (int i = 0; i < tablePackages.Rows.Count; ++i)
                {
                    // Leitakse valitud ajahetkele vastav börsihind
                    double stockPrice = VK.priceRange.Find(Tuple => Tuple.Item1 == time).Item2;
                    // Arvutatakse paketti arvestades tarbija lõpphind
                    double price = AR.finalPrice(stockPrice, this.packageInfo[i], time);

                    tablePackages.Rows[i].Cells[9].Value = price.ToString("0.000");
                }
                // Uuendatakse tabeli odavaimate/kalleimate hindade värve
                this.updatePakettideVarvid();
            }
        }
        private void updatePakettideMallid(DateTime startTime, double usageLength, double power)
        {
            // Kui tahetakse tarbimismalli maksumust kõrvaldada
            if (startTime == default(DateTime))
            {
                for (int i = 0; i < tablePackages.Rows.Count; ++i)
                {
                    tablePackages.Rows[i].Cells[11].Value = "-";
                }
            }
            else if ((usageLength == 0.0) || (usageLength >= 1e6))
            {
                for (int i = 0; i < tablePackages.Rows.Count; ++i)
                {
                    tablePackages.Rows[i].Cells[11].Value = "Error!";
                }
            }
            else
            {
                // stopp-aeg on 1 tunni võrra väiksem, sest VK.createRange võtab viimase andmepunkti lõpuajaga kaasa-arvatud
                System.DateTime stopTime = startTime + TimeSpan.FromHours(Math.Ceiling(usageLength) - 1);
                // stopp-argument on aeg, kust algab viimane tund
                var stockRange = VK.createRange(this.priceData, startTime, stopTime);
                var usageRange = AR.generateUsageData(startTime, usageLength, power);

                /*if (stockRange.Count != usageRange.Count)
                {
                    // Midagi on katki :/
                    Console.WriteLine("Ei ole võrdsed!!: " + stockRange.Count.ToString() + " ja " + usageRange.Count.ToString());
                }
                else
                {
                    for (int i = 0; i < stockRange.Count; ++i)
                    {
                        Console.WriteLine(stockRange[i].Item2.ToString() + ": " + usageRange[i].Item2.ToString() + " kW");
                    }
                }*/

                for (int i = 0; i < tablePackages.Rows.Count; ++i)
                {
                    // Genereerib hinna-andmed
                    VecT stockRangeWithMargins = new VecT();
                    foreach (var item in stockRange)
                    {
                        // Arvutab vastavalt paketile lõpphinna
                        double endPrice = AR.finalPrice(item.Item2, this.packageInfo[i], item.Item1);

                        stockRangeWithMargins.Add(Tuple.Create(item.Item1, endPrice));
                    }

                    // Leiab integraali
                    double integral;
                    int iRet = AR.integral(
                        usageRange,
                        stockRangeWithMargins,
                        startTime,
                        stopTime,
                        out integral
                    );

                    if (iRet != 0)
                    {
                        tablePackages.Rows[i].Cells[11].Value = "Error!";
                        continue;
                    }

                    // Ümardab 3 komakohani üles, sendid ==> eurodeks
                    tablePackages.Rows[i].Cells[11].Value = Math.Round(integral / 100.0 + 0.0005, 3).ToString();
                }
            }
        }
        private void updatePakettideTarbimishind()
        {
            if (VK.userDataTimeRange.Count == 0)
            {
                return;
            }
            var stockCost = VK.createRange(this.priceData, VK.userDataTimeRange.First(), VK.userDataTimeRange.Last()); // Börsihinna väärtuste loomine
            var start = dateStartTime.Value;
            var stop = dateStopTime.Value;
            try
            {
                for (int c = 0; c < tablePackages.Rows.Count; ++c) // Loop läbi tabeli kõikide ridade
                {
                    List<double> packageUsageCost = new List<double>();
                    double cost = 0.0;
                    var usageCost = VK.userDataUsageRange.Zip(stockCost, (u, s) => new { Usage = u, Stock = s }); // Kahest listist pannakse kokku üks
                    foreach (var us in usageCost)
                    {
                        if (us.Stock.Item1 < start)
                        {
                            continue;
                        }
                        else if (us.Stock.Item1 > stop)
                        {
                            break;
                        }
                        packageUsageCost.Add(us.Usage * AR.finalPrice(us.Stock.Item2, packageInfo[c], us.Stock.Item1)); // Arvutab iga tunni hinna ja lisb listi
                    }
                    foreach (var item in packageUsageCost)
                    {
                        cost += item; // Liidab tunnihinnad 
                    }
                    tablePackages.Rows[c].Cells[10].Value = ((cost + Convert.ToDouble(tablePackages.Rows[c].Cells[3].Value)) / 100).ToString("0.00"); // Lisab saadud hinna pakettide tabelisse 
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
