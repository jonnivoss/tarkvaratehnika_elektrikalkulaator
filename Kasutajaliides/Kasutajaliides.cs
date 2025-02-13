﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using PackageT = System.Collections.Generic.List<AndmePyydja.IPackageInfo>;

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
            24, // Ctrl+X
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
        private DateTime lastUpdated;

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

        Rectangle originalRadioStockPrice;
        Rectangle originalRadioMonthlyCost;
        Rectangle originalTextMonthlyPrice;
        Rectangle originalLabelRate;

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

        Rectangle originalGroupTrendFinder;
        Rectangle originalLabelTunnid;
        Rectangle originalTextPerioodTundides;
        Rectangle originalButtonLeiaTrend;
        Rectangle originalCheckBoxTundVoiPaev;

        // GRAAFIKU UUENDAMISE FUNKTSIOON
        /* Funktsioon uuendab börsiandmete ja tarbijaandmete graafikuid kui võimalik.
         * Joonistab börsiandmete graafikule keskmise hinna joone ning värvib graafiku
         * vastavalt keskmisest suurema ja väiksema hinnaga osades punaseks/roheliseks.
         * Kui on valitud pakett/paketid, siis joonistab paketiandmete põhjal nii paketihinna
         * graafiku kui ka simuleeritud tarbijaandmete hinnakulu graafiku. Uuendab ka graafikule
         * vastavas hinnatabelis olevat informatsiooni.
         * 
         * PARAMEETRID:
         * -
         * 
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void updateGraph()
        {
            var start = dateStartTime.Value;
            var stop  = dateStopTime.Value;
            // KUI KANKEL TRIGERIB, SIIS OSTA OP-MÄLU JUURDE...
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

            /*string line = "Keskmine hind: " + VK.averagePrice.ToString();
            string line2 = "Max: " + chartPrice.ChartAreas["ChartArea1"].AxisY2.Maximum.ToString();
            txtDebug.AppendText(Environment.NewLine);
            txtDebug.AppendText(line);
            txtDebug.AppendText(line2);*/


            // pakettide graafikud
            if (isPackageSelected)
            {
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

                            var stockUsageCost = VK.userDataRange.Zip(stockCostWithMarginals, (u, c) => new { Usage = u, Cost = c });
                            foreach (var uc in stockUsageCost)
                            {
                                //Console.WriteLine("suc: " + uc.Usage.Item2.ToString() + "; " + uc.Cost.ToString());
                                packageUsageCost.Add(uc.Usage.Item2 * uc.Cost);
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

                            var realCosts = packageUsageCost;
                            if (realCosts.Count > 1)
                            {
                                realCosts.RemoveAt(realCosts.Count - 1);
                            }

                            // Paneb tarbimise simulatsiooniandmed tabelisse, kui andmed puuduvad, paneb "-"

                            if (VK.priceTimeRange.Count > 1)
                            {
                                for (int c = 0, iCost = 0; c < (VK.priceTimeRange.Count - 1); ++c)
                                {
                                    string cost = "-";
                                    if (((iCost != 0) || (VK.priceTimeRange[c] >= VK.userDataRangeStart)) && (iCost < realCosts.Count))
                                    {
                                        cost = realCosts[iCost].ToString("0.000");
                                        ++iCost;
                                    }
                                    tablePrice.Rows.Add(VK.priceTimeRange[c], VK.priceCostRange[c], cost);
                                }
                            }
                            txtDebug.AppendText("Tabelisse lisatud");
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            else
            {
                // Lisab tavalise tabeli jaotuse, viimane punkt massiivis on fiktiivne, seda ei lisata
                if (VK.priceTimeRange.Count > 1)
                {
                    tablePrice.Rows.Clear();
                    for (int i = 0; i < (VK.priceTimeRange.Count - 1); ++i)
                    {
                        tablePrice.Rows.Add(VK.priceTimeRange[i], VK.priceCostRange[i]);
                    }
                }
            }

            this.updatePakettideTarbimishind();

            chartPrice.Invalidate();
            tablePrice.Invalidate();
            // Skaala reguleerimine:
            changeInterval(Convert.ToInt32((stop - start).TotalHours));
        }

        // GRAAFIKU KOORDINAATIDE TEISENDAJA KLIENTRAKENDUSE KOORDINAATIDEKS
        /* Funktsioon võtab argumendiks graafiku ala ning leiab graafiku
         * koordinaatidest klientrakendusele vastavad koordinaadid.
         * 
         * PARAMEETRID:
         *      chart: graafik, mille koordinaate on vaja leida (Chart)
         *      CA: graafiku ala, mis sisaldab hiire positsiooni koordinaate (ChartArea)
         *      
         * TAGASTUSVÄÄRTUSED:
         *      RectangleF: klientrakendusele vastavad graafiku koordinaadid
         */
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

        // TOOLTIPI HÜPIKU KUVAMINE
        /* Funktsioon kuvab korrektse suuruse ja fondiga tooltipi graafiku kohal kui
         * hiirega graafiku peale minna. Hüpiku suurus arvutatakse välja selle põhjal
         * kui palju teksti on vaja kuvada.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: hüpiku sündmuse argumendid (PopupEventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        void toolTip_Popup(object sender, PopupEventArgs e)
        {
            Font f = (state) ? Normal : Bigger;
            e.ToolTipSize = TextRenderer.MeasureText(tooltipText, f);
        }

        // TOOLTIPI HÜPIKU JOONISTAJA
        /* Funktsioon joonistab graafiku kohal kuvatava hüpik-tooltipi. Kuvatakse
         * börsihind, kellaaeg ning kuupäev.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: joonistamissündmustele vastavad argumendid (DrawToolTipEventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            //font ja värv
            Font f  = (state)?  Normal       : Bigger;
            Brush b = (isNotDarkMode)? Brushes.Black : Brushes.White;
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(tooltipText, f, b, new Point(2, 2));
        }

        // GRAAFIKUL HIIRELIIGUTUSTELE REAGEERIJA
        /* Funktsioon reageerib hiireliigutustele graafikul, uuendab graafikule joonistatavat
         * vertikaalset joont, mis tähistab valitud ajavahemiku. Samuti laseb joonistada
         * korrektse aeg-hind infoga tooltip-tüüpi hüpiku. Lisaks uuendab pakettide tabelis
         * olevaid pakettide tarbija lõpphindasid.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: hiiresündmustele vastavad parameetrid (MouseEventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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
                DateTime tooltipDate = default(DateTime);
                double tooltipPrice = 0;
                
                //leia punkt millele x vastab ja salvesta selle y kordinaat
                DateTime s = DateTime.FromOADate(x);
                for (int i = 0; i < VK.priceTimeRange.Count; ++i)
                {
                    if (VK.priceTimeRange[i].Hour == s.Hour && VK.priceTimeRange[i].Date == s.Date)
                    {
                        tooltipPrice = VK.priceCostRange[i];
                        tooltipDate  = VK.priceTimeRange[i];
                        this.updatePakettideHinnad(tooltipDate);
                        break;
                    }
                }
                bool foundUsage = false;
                double tooltipUsage = 0.0;
                for (int i = 0; i < VK.userDataTimeRange.Count; ++i)
                {
                    if (VK.userDataTimeRange[i].Hour == s.Hour && VK.userDataTimeRange[i].Date == s.Date)
                    {
                        tooltipUsage = VK.userDataUsageRange[i];
                        foundUsage = true;
                        break;
                    }
                }
                
                //tt tekst
                tooltipText = "börsihind: ";
                tooltipText += tooltipPrice.ToString("0.000") + " s/kWh";
                if (foundUsage)
                {
                    tooltipText += "\ntarbimine: " + tooltipUsage.ToString() + " kWh";
                }

                if (tooltipDate != default(DateTime))
                {
                    // Leitakse valitud ajahetkele vastav börsihind
                    double stockPrice = VK.priceRange.Find(Tuple => Tuple.Item1 == tooltipDate).Item2;
                    for (int i = 0; i < tablePackages.SelectedRows.Count; ++i)
                    {
                        var idx = tablePackages.SelectedRows[i].Index;
                        // Arvutatakse paketti arvestades tarbija lõpphind
                        double price = AR.finalPrice(stockPrice, this.packageInfo[idx], tooltipDate);

                        // Lisab hinna tooltipi
                        tooltipText += "\n" + idx.ToString() + " - " + tablePackages.SelectedRows[i].Cells[2].Value.ToString() + ": ";
                        tooltipText += price.ToString("0.000") + " s/kWh";
                    }
                }

                // Lisab valitud kella-aja kohta
                tooltipText += "\n" + tooltipDate.ToString("kell HH:00") + "\n" + tooltipDate.ToString("dd/MM/yy");

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

        // ZOOMIMISINTERVALLI ARVUTAJA
        /* Funktsioon arvutab graafiku zoomimisel korrektse ajaintervalli, mida
         * x-teljel kuvada. Kuvab x-teljel korrektse formaadiga ning korrektse arvu jaotisi.
         * Muudab x-teljele kantavate jaotiste tihedust.
         * 
         * PARAMEETRID (SISEND):
         *      count: graafikul kuvatavate punktide arv (int)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // TARBIJAANDMETE CSV AVAMISNUPU REAGEERIJA
        /* Funktsioon reageerib tarbijaandmeid valida võimaldava nupu vajutustele.
         * Kasutajal lastakse valida tarbijaandmeid sisaldav CSV fail ning need suunatakse
         * töötlemisele. Tarbijaandmete faili asukoht salvestatakse AndmeSalvestaja kaudu sätetesse.
         * Töödeldud tarbijaandmed kuvatakse graafikule.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            if (AP.chooseFileUserData())
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.tarbijaAndmed, AP.userDataFileName);
                this.openCSVUserData();
            }
        }

        // CSV TARBIJAANDMETE TÖÖTLEJA
        /* Funktsioon avab tarbijaandmete CSV faili ning töötleb selle sisu.
         * Küsib internetist tarbijaandmete ajale vastavaid börsiandmeid.
         * Pärast sisu töötlemist uuendab graafikut.
         * 
         * PARAMEETRID:
         * -
         *      
         * TAGASTUSVÄÄRTUSED:
         *      false: tarbijaandmete faili lugemine ebaõnnestus
         *      true: tarbijaandmete lugemine oli edukas
         */
        private bool openCSVUserData()
        {
            ret = true;
            string fileContents;
            if (!AP.readUserDataFile(out fileContents))
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

        // TARBIMISMALLI MAKSUMUSE ARVUTAJA
        /* Funktsioon arvutab sisestatud tarbimismalli andmete põhjal ning valitud ajavahemiku
         * põhjal optimaalse tarbimise alustamisaja ning hinnangulise maksumuse börsiandmete põhjal.
         * Samuti laseb funktsioon arvutada tarbimismalli hinnangulise maksumuse iga elektripaketi kohta.
         * Tarbimismalli maksumused pakettide kohta kuvatakse paketiandmete tabelisse.
         * 
         * PARAMEETRID:
         * -
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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
                        // Kui parim tarbimisaeg on praegu, siis kuvab "Praegu"
                        DateTime d = DateTime.Now;
                        d = d.Date + TimeSpan.FromHours(d.Hour);

                        txtTarbimisAeg.Text = (d == bestDate) ? "Präägu" : bestDate.ToString("dd.MM.yyyy HH:mm");
                    }
                }
                else
                {
                    bestDate = dateStartTime.Value;
                    var skwh = Double.Parse(txtMonthlyPrice.Text);
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

        // KASUTAJALIIDESE LAADUR/INITSIALISEERIJA
        /* Funktsioon initsialiseerib kasutajaliidesele vajalikud elemendid programmi
         * esmasel käitamisel. Nende hulka kuuluvad: graafiku hoverimise tooltip,
         * graafiku reageerimine hiirerullikule, akna miinimumsuurus, graafik, hinnatabel,
         * sätete laadimine, CSV andmete laadimine.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

            this.MinimumSize = new Size(1083, 1002);
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

            isNotDarkMode = AS.getSetting(AndmeSalvestaja.ASSetting.tumeTaust) == "1";
            btnDarkMode_Click(btnDarkMode, e);

            state = AS.getSetting(AndmeSalvestaja.ASSetting.suurendusLubatud) == "1";
            btnNormalSize_Click(btnChangeSize, e);

            


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

            {
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
                originalRadioStockPrice = new Rectangle(rbStockPrice.Location.X, rbStockPrice.Location.Y, rbStockPrice.Size.Width, rbStockPrice.Size.Height);
                originalRadioMonthlyCost = new Rectangle(rbMonthlyCost.Location.X, rbMonthlyCost.Location.Y, rbMonthlyCost.Size.Width, rbMonthlyCost.Size.Height);
                originalTextMonthlyPrice = new Rectangle(txtMonthlyPrice.Location.X, txtMonthlyPrice.Location.Y, txtMonthlyPrice.Size.Width, txtMonthlyPrice.Size.Height);
                originalLabelRate = new Rectangle(lblRate.Location.X, lblRate.Location.Y, lblRate.Size.Width, lblRate.Size.Height);
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
                originalGroupTrendFinder = new Rectangle(groupTrendFinder.Location.X, groupTrendFinder.Location.Y, groupTrendFinder.Size.Width, groupTrendFinder.Size.Height);
                originalLabelTunnid = new Rectangle(lblTunnid.Location.X, lblTunnid.Location.Y, lblTunnid.Size.Width, lblTunnid.Size.Height);
                originalTextPerioodTundides = new Rectangle(txtPerioodTundides.Location.X, txtPerioodTundides.Location.Y, txtPerioodTundides.Size.Width, txtPerioodTundides.Size.Height);
                originalButtonLeiaTrend = new Rectangle(btnLeiaTrend.Location.X, btnLeiaTrend.Location.Y, btnLeiaTrend.Size.Width, btnLeiaTrend.Size.Height);
                originalCheckBoxTundVoiPaev = new Rectangle(cbTundVoiPaev.Location.X, cbTundVoiPaev.Location.Y, cbTundVoiPaev.Size.Width, cbTundVoiPaev.Size.Height);
            }
        }

        // HETKE BÖRSIHINNA UUENDAJA
        /* Funktsioon uuendab kuvatavat hetkelist börsihinda. Uuendamine on
         * vajaduspõhine, kontrollitakse kellaaja järgi, kas on mõtet hinda uuendada.
         * Uuendamiseks kutsutakse välja Elering API, küsitakse andmeid praeguse tunni kohta.
         * 
         * PARAMEETRID:
         * -
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // NUMBRITE JAOKS MÕELDUD TEKSTIKASTI KLAHVIVAJUTUSE REAGEERIJA
        /* Funktsioon reageerib klahvivajutustele, mis tehakse numbritele mõeldud
         * tekstikastis. Funktsioon väldib tekstikasti mitte-numbrite sisestamist.
         * 
         * PARAMEETRID (SISEND):
         *      text: hetkel tekstikastis olev tekst (string)
         *      e: klahvivajutuse sündmustele vastavad parameetrid (KeyPressEventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         *      false: tekstikasti sisestati keelatud sümboleid
         *      true: tekstikasti sisestati korrektne sümbol
         */
        bool handleNumberBoxKeyPress(string text, KeyPressEventArgs e)
        {
            double parsedValue;
            bool isParsed = double.TryParse(text + e.KeyChar, out parsedValue);
            if (!isParsed && !numberBoxValidChars.Contains(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Palun sisestage ainult numbreid!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        // AJAKULU TEKSTIKASTI KLAHVIVAJUTUSE SÜNDMUS
        /* Funktsioon reageerib sündmusele kui vajutati mõnele klahvile tarbimismalli ajakulu
         * tekstikastis. Lubab kirjutada tekstikasti ainult numbreid.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: klahvivajutuse sündmustele vastavad parameetrid (KeyPressEventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void txtAjakulu_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.handleNumberBoxKeyPress(txtAjakulu.Text, e);
        }

        // VÕIMSUSE TEKSTIKASTI KLAHVIVAJUTUSE SÜNDMUS
        /* Funktsioon reageerib sündmusele kui vajutati mõnele klahvile tarbimismalli võimsuse
         * tekstikastis. Lubab kirjutada tekstikasti ainult numbreid.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: klahvivajutuse sündmustele vastavad parameetrid (KeyPressEventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void txtVoimsus_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.handleNumberBoxKeyPress(txtVoimsus.Text, e);
        }

        // ALGUSAJA MUUTUSELE REAGEERIJA
        /* Funktsioon reageerib sündmusele, kui keegi muutis analüüsi algusaega.
         * Kui algusaeg on suurem kui lõpuaeg, siis uuendatakse ka lõpuaega.
         * Arvutatakse tarbimismalli põhjal uus optimaalne tarbimisaeg ning hind.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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
        }

        // ALGUSAJA VALIJA AVAMISE REAGEERIJA
        /* Funktsioon reageerib sellele, kui kasutaja avab algusaja valimise kalendri.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void dateStartTime_DropDown(object sender, EventArgs e)
        {
            dateStartTime.ValueChanged += dateStartTime_ValueChanged;
        }

        // ALGUSAJA VALIJA SULGEMISE REAGEERIJA
        /* Funktsioon reageerib sellele, kui kasutaja sulgeb algusaja valimise kalendri.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void dateStartTime_CloseUp(object sender, EventArgs e)
        {
            dateStartTime.ValueChanged -= dateStartTime_ValueChanged;
        }

        // LÕPUAJA MUUTUSELE REAGEERIJA
        /* Funktsioon reageerib sündmusele, kui keegi muutis analüüsi lõpuaega.
         * Kui lõpuaeg on väiksem kui algusaeg, siis uuendatakse ka algusaega.
         * Arvutatakse tarbimismalli põhjal uus optimaalne tarbimisaeg ning hind.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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
        }

        // LÕPUAJA VALIJA AVAMISE REAGEERIJA
        /* Funktsioon reageerib sellele, kui kasutaja avab lõpuaja valimise kalendri.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void dateStopTime_DropDown(object sender, EventArgs e)
        {
            dateStopTime.ValueChanged += dateStopTime_ValueChanged;
        }

        // LÕPUAJA VALIJA SULGEMISE REAGEERIJA
        /* Funktsioon reageerib sellele, kui kasutaja sulgeb lõpuaja valimise kalendri.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void dateStopTime_CloseUp(object sender, EventArgs e)
        {
            dateStopTime.ValueChanged -= dateStopTime_ValueChanged;
        }

        // HINNAKUVAMISE LINNUKESELE REAGEERIJA
        /* Funktsioon reageerib sündmusele, kui kasutaja muudab börsihinna kuvamist
         * tähistava linnukese-kasti olekut. Uuendab graafikut.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // TABELIKUVAMISE LINNUKESELE REAGEERIJA
        /* Funktsioon reageerib sündmusele, kui kasutaja muudab tabeli kuvamist
         * tähistava linnukese-kasti olekut.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // KASUTAJALIIDESE SULGUR/DESTRUKTOR
        /* Funktsioon salvestab enne rakenduse lõplikku sulgemist sätted sätetefaili.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void Kasutajaliides_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Salvestab sätted
            AS.saveFile();
        }

        // BÖRSIHINDA VALIVA RAADIONUPU REAGEERIJA
        /* Funktsioon reageerib börsihinda valiva raadionupu olekumuutustele.
         * Selle põhjal valitakse, kas tarbimismalli simuleerimisel kastutatakse börsihinda
         * või kasutaja poolt sisestatud fikshinda. Uuendatakse automaatselt tarbimismalli hinnaarvutusi.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void rbStockPrice_CheckedChanged(object sender, EventArgs e)
        {
            var state = rbStockPrice.Checked;
            if (state)
            {
                txtMonthlyPrice.Enabled = false;
            }
            else
            {
                txtMonthlyPrice.Enabled = true;
            }
            calcPrice();
        }

        // KASUTUSMALLI VALIVA RIPPMENÜÜ REAGEERIJA
        /* Funktsioon reageerib sündmusele, kui kasutaja muudab valitud tüüpkasutusmalli.
         * Uuendatakse automaatselt hinnaarvutusi.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void cbKasutusmall_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                var userCase = AS.getUseCases()[cbKasutusmall.SelectedItem.ToString()];
                // Vatid kilovattideks
                txtVoimsus.Text = Math.Round(userCase.Item1 / 1000.0, 3).ToString();
                // Minutid tundideks
                txtAjakulu.Text = Math.Round(userCase.Item2 / 60.0, 3).ToString();

                // Calculate price
                calcPrice();
            }
            catch (Exception)
            {
            }
        }

        // NUMBRITE JAOKS MÕELDUD TEKSTIKASTI MUUDU KÄITLEJA
        /* Funktsioon võtab parameetriteks tekstikasti ning maksimaalse väärtuse
         * mida sinna sisestada võib. Kui väärtus ületab seatud piiri, siis surutakse väärtus
         * väärtus tagasi maksimumväärtusele.
         * 
         * PARAMEETRID:
         *      box: vaatluse all olev tekstikast (ref TextBox)
         *      maxLimit: maksimaalne aktsepteeritav arvväärtus, mida tekstikast tunnistab, vaikimisi piirangut pole (double)
         *
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        void handleNumberBoxChanged(ref TextBox box, double maxLimit = Double.PositiveInfinity)
        {
            string str = box.Text;

            double parsedValue;
            var isParsed = double.TryParse(str, out parsedValue);
            if (isParsed && parsedValue > maxLimit)
            {
                // Jätab praeguse kursori asukoha meelde
                var curpos = box.SelectionStart;
                box.Text = maxLimit.ToString();
                // Läheb sinna tagasi, kus ta oli, tavaliselt pärast teksti muutmist surub Windows
                // kursori teksti algusesse
                box.Select(Math.Min(curpos, box.Text.Length), 0);
            }
            else if (!isParsed)
            {
                // Esmalt jätab alles ainult numbrid/komad/punktid
                char[] nums =
                {
                    '0',
                    '1',
                    '2',
                    '3',
                    '4',
                    '5',
                    '6',
                    '7',
                    '8',
                    '9',
                    '.',
                    ','
                };

                string tempString = "";
                foreach (var ch in str)
                {
                    if (nums.Contains(ch))
                    {
                        tempString += ch;
                    }
                }
                str = tempString;
                // Eemaldab lõpust tähti senikaua, kuni on parsitav
                while ((str.Length > 0) && !double.TryParse(str, out parsedValue))
                {
                    str = str.Remove(str.Length - 1);
                }
                // Jätab praeguse kursori asukoha meelde
                var curpos = box.SelectionStart;
                box.Text = str;
                // Läheb sinna tagasi, kus ta oli, tavaliselt pärast teksti muutmist surub Windows
                // kursori teksti algusesse
                box.Select(Math.Min(curpos, box.Text.Length), 0);
            }
        }

        // AJAKULU LAHTRI VÄÄRTUSE MUUTMINE
        /* Funktsioon kutsutakse välja juhul kui muudetakse sisendit tekstikastis txtAjakulu.
         * Funktsioon ise kutsub välja funktsioonid handleNumberBoxChanged() ja calcPrice().
         * Esimeses töödeldakse sisend double väärtuseks ning kontrollitakse, et kasutaja  
         * sisestatud väärtus ei oleks üleliigselt suur. Seejuures võetakse arvesse funktsiooni 
         * teist argumenti. Teise funktsiooniga arvutatakse sisendi põhjal välja elektrihind.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void txtAjakulu_TextChanged(object sender, EventArgs e)
        {
            this.handleNumberBoxChanged(ref txtAjakulu, 1e6);
            calcPrice();
        }

        // VÕIMSUSE LAHTRI VÄÄRTUSE MUUTMINE
        /* Funktsioon kutsutakse välja juhul kui muudetakse sisendit tekstikastis txtVoimsus.
         * Funktsioon ise kutsub välja funktsioonid handleNumberBoxChanged() ja calcPrice().
         * Esimeses töödeldakse sisend double väärtuseks ning kontrollitakse, et kasutaja  
         * sisestatud väärtus ei oleks üleliigselt suur. Seejuures võetakse arvesse funktsiooni 
         * teist argumenti. Teise funktsiooniga arvutatakse sisendi põhjal välja elektrihind.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void txtVoimsus_TextChanged(object sender, EventArgs e)
        {
            this.handleNumberBoxChanged(ref txtVoimsus, 1e4);
            calcPrice();
        }

        // KUUTASU LAHTRI VÄÄRTUSE MUUTMINE
        /* Funktsioon kutsutakse välja juhul kui muudetakse sisendit tekstikastis txtMonthlyPrice.
         * Funktsioon ise kutsub välja funktsioonid handleNumberBoxChanged() ja calcPrice().
         * Esimeses töödeldakse sisend double väärtuseks ning kontrollitakse, et kasutaja  
         * sisestatud väärtus ei oleks üleliigselt suur. Seejuures võetakse arvesse funktsiooni
         * teist argumenti. Funktsiooni calcPrice() välja kutsumiseks kontrollitakse kõigepealt, 
         * et raadionupp rbMonthlyCost oleks valitud. Vastasel juhul funktsiooni välja ei 
         * kutsuta.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void txtMonthlyPrice_TextChanged(object sender, EventArgs e)
        {
            this.handleNumberBoxChanged(ref txtMonthlyPrice, 1e4); // kontrollib sisestatud tähemärke
            if (rbMonthlyCost.Checked) // kui kuutasu lahter on valitud
            {
                calcPrice(); // arvutakse hind
            }
        }

        // KASUTAJALIIDESE ELEMENTIDE SUURENDAMIS-/VÄHENDAMISNUPU VAJUTAMINE 
        /* Funktsiooni btnNormalSize_Click() töö käigus kontrollitakse ning muudetakse globaalset 
         * Boolean tüüpi muutujat state. Muutuja state väärtus on "true" või "false" vastavalt 
         * elementide suurusele: "true" kui elemdid on suured ning "false" kui need on tavalise
         * suurusega. Enamusel elementidel on kasutatud globaalseid väärtuseid Bigger ja Normal.
         * Vaikimisi väärtused (font, fondi suurus): 
         * Bigger = "Impact", 16
         * Normal = "Impact", 12
         * Soovi korral on võimalik väärtuseid muuta koodi alguses.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void btnNormalSize_Click(object sender, EventArgs e)
        {
            AS.changeSetting(AndmeSalvestaja.ASSetting.suurendusLubatud, state ? "1" : "0");

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
                txtMonthlyPrice.Font = Bigger;
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

                groupTrendFinder.Font = new Font("Impact", 12);
                lblTunnid.Font = Bigger;
                txtPerioodTundides.Font = Bigger;
                btnLeiaTrend.Font = Bigger;
                cbTundVoiPaev.Font = Bigger;

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
                txtMonthlyPrice.Font = Normal;
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

                groupTrendFinder.Font = new Font("Impact", 9);
                lblTunnid.Font = Normal;
                txtPerioodTundides.Font = Normal;
                btnLeiaTrend.Font = Normal;
                cbTundVoiPaev.Font = Normal;

                state = true;
            }
        }

        // TARBIMISANDMETE GRAAFIKUL NÄITAMISE LUBAMINE/KEELAMINE
        /* Funktsioon muudab vastavalt kasti cbShowUsage olekule (lokaalne muutuja state) 
         * tarbimisandmete kuvamist. Kui tarbimisandmete fail on rakenduses valitud ning kast 
         * on aktiivne, kuvatakse graafikul tarbimisandmete joon. Peale igat olekumuutust 
         * kutsutakse välja funktsioon updateGraph(), mis uuendab eelnimetatud joone nähtavust 
         * vastavalt showUsage muutuja väärtusele.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void cbShowUsage_CheckedChanged(object sender, EventArgs e)
        {
            var state = cbShowUsage.Checked; // kas tarbimisandmed on graafikul kuvamiseks lubatud või mitte
            if (state) // kontrollib muutujat
            {
                this.showUsage = true; // lubab graafikul kuvamise
            }
            else
            {
                this.showUsage = false; // keelab graafikul kuvamise
            }
            updateGraph(); // uuendab graafikut
        }

        // KUUTASU TEKSTIKASTI SISESTAMINE
        /* Funktsioon kontrollib peale igat sisestatud tähemärki, et see oleks õiges formaadis.
         * Kohe, kui sisetatakse midagi või vajutatakse mingit klahvi, kutsutakse välja funktsioon
         * handleNumberBoxKeyPress(), mis teostab kontrolli. 
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: nupuvajutuse sündmustele vastavad argumendid (KeyPressEventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -   
         */
        private void txtMonthlyPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.handleNumberBoxKeyPress(txtMonthlyPrice.Text, e);
        }

        // KASUTAJALIIDESE ELEMENTIDE SUURUSE MUUTMINE
        /* Funktsiooni kirjeldus siia!
         * 
         * PARAMEETRID (SISEND):
         *      a
         *      b
         *      c
         *      
         * PARAMEETRID (VÄLJUND):
         *      d
         *      e
         *      f
         *      
         * TAGASTUSVÄÄRTUSED:
         * 
         */
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

        // DARKMODE NUPULE VAJUTAMINE
        /* Funktsioon kasutab oma töös globaalset Boolean tüüpi muutujat isNotDarkMode. Kui nupuvajutus 
         * toimub, siis inverteeritakse muutuja väärtus ning vastavalt saadud väärtusele muudetakse
         * kõik kasutajaliidese elemendid heledaks või tumedaks. Kui muutuja väärtuseks on "false",
         * muudetakse elemendid tumedaks ning väärtusega "true" muudetakse heledaks.
         * Samuti kutsutakse välja funktsioonid drawGreenPacketColumn(), mis värvib roheliseks
         * rohepakettide tulba ja updatePakettideVarvid(), mis uuendab pakettide ridade värve.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            isNotDarkMode = !isNotDarkMode;

            AS.changeSetting(AndmeSalvestaja.ASSetting.tumeTaust, isNotDarkMode ? "0" : "1");

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
                txtMonthlyPrice.BackColor = midGrey;
                txtMonthlyPrice.ForeColor = chalkWhite;
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

                groupTrendFinder.ForeColor = chalkWhite;
                lblTunnid.ForeColor = chalkWhite;
                txtPerioodTundides.ForeColor = chalkWhite;
                txtPerioodTundides.BackColor = midGrey;
                btnLeiaTrend.ForeColor = chalkWhite;
                btnLeiaTrend.BackColor = midGrey;
                cbTundVoiPaev.ForeColor = chalkWhite;

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

                // Kuidas muuta nupud õiget värvi tagasi? -> UseVisualStyleBackColor property
                // https://stackoverflow.com/questions/10569200/how-to-reset-to-default-button-backcolor

                cbKasutusmall.BackColor = Color.White;
                cbKasutusmall.ForeColor = Color.Black;
                txtAjakulu.BackColor = SystemColors.Window;
                txtAjakulu.ForeColor = Color.Black;
                txtVoimsus.BackColor = SystemColors.Window;
                txtVoimsus.ForeColor = Color.Black;
                txtHind.BackColor = SystemColors.Window;
                txtHind.ForeColor = Color.Black;
                txtTarbimisAeg.BackColor = SystemColors.Window;
                txtTarbimisAeg.ForeColor = Color.Black;
                txtDebug.BackColor = SystemColors.Window;
                txtDebug.ForeColor = Color.Black;
                btnAvaCSV.BackColor = SystemColors.Control;
                btnAvaCSV.UseVisualStyleBackColor = true;
                btnAvaCSV.ForeColor = Color.Black;
                txtCostNow.BackColor = SystemColors.Window;
                txtCostNow.ForeColor = Color.Black;
                btnChangeSize.BackColor = SystemColors.Control;
                btnChangeSize.UseVisualStyleBackColor = true;
                btnChangeSize.ForeColor = Color.Black;
                btnDarkMode.BackColor = SystemColors.Control;
                btnDarkMode.UseVisualStyleBackColor = true;
                btnDarkMode.ForeColor = Color.Black;
                txtMonthlyPrice.BackColor = SystemColors.Window;
                txtMonthlyPrice.ForeColor = Color.Black;
                groupPriceType.ForeColor = Color.Black;

                groupExport.ForeColor = Color.Black;
                lblExportDelimiter.ForeColor = Color.Black;
                txtExportDelimiter.ForeColor = Color.Black;
                txtExportDelimiter.BackColor = SystemColors.Window;
                lblExportQualifier.ForeColor = Color.Black;
                txtExportQualifier.ForeColor = Color.Black;
                txtExportQualifier.BackColor = SystemColors.Window;
                cbExportAppend.ForeColor = Color.Black;
                btnExportSave.ForeColor = Color.Black;
                btnExportSave.BackColor = SystemColors.Control;
                btnExportSave.UseVisualStyleBackColor = true;
                btnExportOpen.ForeColor = Color.Black;
                btnExportOpen.BackColor = SystemColors.Control;
                btnExportOpen.UseVisualStyleBackColor = true;
                lblExportPath.ForeColor = Color.Black;
                txtExportPath.ForeColor = Color.Black;
                txtExportPath.BackColor = SystemColors.Window;

                groupTrendFinder.ForeColor = Color.Black;
                lblTunnid.ForeColor = Color.Black;
                txtPerioodTundides.ForeColor = Color.Black;
                txtPerioodTundides.BackColor = SystemColors.Window;
                btnLeiaTrend.ForeColor = Color.Black;
                btnLeiaTrend.BackColor = SystemColors.Control;
                btnLeiaTrend.UseVisualStyleBackColor = true;
                cbTundVoiPaev.ForeColor = Color.Black;

                btnOpenPackages.BackColor = SystemColors.Control;
                btnOpenPackages.UseVisualStyleBackColor = true;
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



        // PAKETTIDE CSV AVAMISE NUPUVAJUTUS
        /* Nupuvajutusel kutsutakse välja funktsioon chooseFilePackages() ning vastavalt selle tulemusele
         * (kas õnnestus fail leida või mitte) muudetakse sätetefailis pakettide faili path funktsiooniga
         * changeSetting(). Seejärel avatakse fail funktsiooniga openCSVPackage().
         * 
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void btnOpenPackages_Click(object sender, EventArgs e)
        {
            if (AP.chooseFilePackages()) // kui faili valimine õnnestub
            {
                AS.changeSetting(AndmeSalvestaja.ASSetting.paketiAndmed, AP.packageFileName); // muudetakse sätetefaili
                this.openCSVPackage(); // avab pakettide CSV faili
            }
        }

        // PAKETTIDE CSV FAILI AVAMINE
        /* Funktsioon loeb sisse faili sisu kasutades funktsiooni readPackageFile(). Kui lugemine ebaõnnestus 
         * muudetakse lokaalne muutuja ret vääraks. Kui lugemine õnnestub, parsitakse saadud andmed funktsiooniga 
         * parsePackage() ning lisatakse pakettide tabelisse. Kutustakse välja ka funktsioon updatePakettideTarbimishind(), 
         * mis arvutab ja uuendab tarbimisandmete põhjal hinnad, ning funktsioon drawGreenPacketColumn().
         * 
         * TAGASTUSVÄÄRTUSED:
         *  ret: boolean väärtus vastavalt faili lugemise õnnestumisele/ebaõnnestumisele
         */
        private bool openCSVPackage()
        {
            bool ret = true; // lokaalne muutuja tagastamiseks
            string packageFileContents; // faili sisu
            if (!AP.readPackageFile(out packageFileContents)) // kui faili ei õnnestunud lugeda
            {
                ret = false;
            }
            else // kui faili õnnestus lugeda 
            {
                this.packageInfo = AP.parsePackage(packageFileContents); // saadud sisu parsitakse

                tablePackages.Rows.Clear(); // tabeli sisu eemaldamine
                int i = 0;
                foreach (var item in this.packageInfo) // käiakse läbi saadud andmed
                {
                    tablePackages.Rows.Add( // andmed lisatakse tabelisse
                        i,
                        item.providerName,
                        item.packageName,
                        item.monthlyPrice.ToString("0.00"),
                        item.sellerMargins.ToString("0.000"),
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
            return ret; // tagastab saadud vääratuse
        }

        // KASUTAJALIIDESE ELEMENTIDE SUURUSE MUUTMINE
        /* Funktsioon kutsub kõikide kasutajaliidese elementide peal välja funktsiooni resizeGuiElemnt() ning seeläbi 
         * muudab elementide suurust. 
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

            resizeGuiElement(originalRadioStockPrice, rbStockPrice);
            resizeGuiElement(originalRadioMonthlyCost, rbMonthlyCost);
            resizeGuiElement(originalTextMonthlyPrice, txtMonthlyPrice);
            resizeGuiElement(originalLabelRate, lblRate);

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

            resizeGuiElement(originalGroupTrendFinder, groupTrendFinder);
            resizeGuiElement(originalLabelTunnid, lblTunnid);
            resizeGuiElement(originalTextPerioodTundides, txtPerioodTundides);
            resizeGuiElement(originalButtonLeiaTrend, btnLeiaTrend);
            resizeGuiElement(originalCheckBoxTundVoiPaev, cbTundVoiPaev);

            Refresh(); // vajalik et ei tekiks "render glitche" (nt. ComboBox ei suurene korraks jms.)
        }

        // PAKETTIDE TABELI REA PÄISE VALIMINE
        /* Kui valitakse pakettide tableist mingi pakett, siis see lisatakse joonena graafikule ning samuti
         * hindade tabelisse. Funktsioon määrab ära, millised paketid tuleb eemaldada nii tabelist kui ka 
         * graafikult, kui need pole enam valitud.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: DataGridViewCell hiire sündmustele vastavad argumendid (DataGridViewCellMouseEventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void tablePackages_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            isPackageSelected = tablePackages.SelectedRows.Count != 0; // kui pakette on valitud

            List<Series> removables = new List<Series>();
            List<DataGridViewColumn> removableColumns = new List<DataGridViewColumn>();
            // Eemaldab vanad pakettide jooned, mida enam pole valitud
            foreach (var series in chartPrice.Series)
            {
                if (series.Name == "Elektrihind" || series.Name == "Tarbimine") // Et ei eemaldaks börsihinna ja tarbimise jooni
                {
                    continue;
                }

                bool removeItem = true;
                for (int j = 0; j < tablePackages.SelectedRows.Count; ++j) // käib läbi valitud paketid
                {
                    string seriesName = tablePackages.SelectedRows[j].Index.ToString() + ": " + tablePackages.SelectedRows[j].Cells[2].Value.ToString();
                    string seriesNameUsage = tablePackages.SelectedRows[j].Index.ToString() + ": " + tablePackages.SelectedRows[j].Cells[2].Value.ToString() + " tarbimisel";
                    if (seriesName == series.Name || seriesNameUsage == series.Name) // kui paketi nimi on sama, mis joone nimi
                    {
                        removeItem = false; // võib eemaldada
                        break;
                    }
                }

                if (removeItem)
                {
                    removables.Add(series); // lisab joone eemaldatavate hulka
                }
            }

            // Eemaldab hinna tabelist paketi mida enam vaja ei ole
            foreach (DataGridViewColumn column in tablePrice.Columns) // Käib läbi tulbad
            {
                if (column.Equals(Aeg) || column.Equals(Hind)) // Kellaaeg ja börsihind peavad jääma
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

                Random r = new Random(); // Suvaliste värvide saamiseks

                chartPrice.Series.Add(packageName); // Joone lisamine
                chartPrice.Series[packageName].ChartArea = "ChartArea1"; // Määrab diagrammi ala
                chartPrice.Series[packageName].YAxisType = AxisType.Secondary; // Määrab telje tüübi 
                chartPrice.Series[packageName].Color     = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256)); // Suvaline värv
                chartPrice.Series[packageName].Legend    = "Legend1"; // Määrab legendi
                chartPrice.Series[packageName].ChartType = SeriesChartType.Line; // Määrab diagrammi tüübi

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
                    
                    if (VK.userDataTimeRange.Count == 0) // Kui pole tarbimisandmeid, siis lisamine ebaõnnestub
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

                    var newColumn = new DataGridViewTextBoxColumn(); // Lisab tabelisse uue tulba
                    newColumn.HeaderText = packageNameUsage + " (s)";          // Määrab päise nimeks paketi nime 
                    newColumn.Name = packageNameUsage;                 // Paketi nimi

                    tablePrice.Columns.Add(newColumn);                 // Lisab paketi tabelisse
                }

                
            }

            updateGraph(); // Uuendab graafikut
        }

        // TOPELTKLÕPS PAKETTIDE TABELI RIDADE PÄISES
        /* Kui pakettide ridade päises tehakse topeltklõps, salvestatakse vastava rea indeks ning vastav rida tehakse
         * mitteaktiivseks. Seejärel kutsutakse välja funktsioon tablePackages_RowHeaderMouseDoubleClick() ning
         * pakett eemaldatakse hindade tabelist ja graafikult.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: DataGridViewCell hiire sündmustele vastavad argumendid (DataGridViewCellMouseEventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void tablePackages_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var index = e.RowIndex;

            // Unselect the line
            tablePackages.Rows[index].Selected = false;
            // Update graph based on selected items
            tablePackages_RowHeaderMouseClick(sender, e);
        }

        // GRAAFIKU HIIRERULLIGA SUURENDAMISE FUNKTSIOON
        /* Funktsioon kasutab chartPrice lahtris hiire asukohta, mille põhjal leitakse pointeri asukoht ajaskaalal
         * (x-teljel). Graafiku alg- ja lõpuajast kui rajadest lähtudes implementeeritakse graafiku suurendamine. Kui hiir
         * paikneb sektsioonis chartPrice ja selle x-koordinaat jääb ajaskaalal algus- ja lõpuaja vahele, siis arvutatakse
         * hiire asukoha põhjal graafiku skaleerimise saavutamiseks uus algus- ja lõppaeg. Seejärel kutsutakse välja funktsioon
         * priceChart_zoom(), mis ajad uuendab ja graafiku värskendab.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt mille kohta sündmused kehtivad
         *      e: hiire sündmustele vastavad argumendid (MouseEventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // TIMER HINNA KONTROLLIMISEKS
        /* Funktsioon kutsub välja funktsiooni updateCostNow(), mis uuendab praegust börsihinda.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void tmrCostNow_Tick(object sender, EventArgs e)
        {
            this.updateCostNow();
            //txtDebug.AppendText("Kontrollisin hinda...");
            //txtDebug.AppendText(Environment.NewLine);
        }

        // SIIA PANE FUNKTSIOONI NIMI VÕI KIRJELDAV
        /* Funktsiooni kirjeldus siia!
         * 
         * PARAMEETRID (SISEND):
         *      a
         *      b
         *      c
         *      
         * PARAMEETRID (VÄLJUND):
         *      d
         *      e
         *      f
         *      
         * TAGASTUSVÄÄRTUSED:
         * 
         */
        void priceChart_MouseDown(object sender, MouseEventArgs e)
        {
            state3 = true;
            if (e.Button == MouseButtons.Left) // vasak hiireklahv ALLA
            {
                zoomStartTime = DateTime.FromOADate(chartPrice.ChartAreas["ChartArea1"].AxisX.PixelPositionToValue(e.Location.X));
                zoomStartTime = zoomStartTime.AddMilliseconds(-(zoomStartTime.Millisecond + 1000*zoomStartTime.Second + 60000*zoomStartTime.Minute));
            }
        }

        // EXPORT SALVESTAMISNUPU VAJUTAMINE
        /* Kui vajutatakse salvestusnupu peale siis salvestatakse valitud faili programmis olevad andmed. Kui faili 
         * pole valitud, annab programm sellest teada ning laseb seda teha. 
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

            // Eksisteerib kolmas paketiandmete tulp
            var hasPackage = tablePrice.Columns.Count == 3;

            List<string[]> exportStrings = new List<string[]>();
            // Tabeli header
            exportStrings.Add(new string[] { "Aeg", "Börsihind (s/kWh)", "Paketikulu (s)" });
            for (int i = 0; i < tablePrice.Rows.Count; ++i)
            {
                var row = tablePrice.Rows[i];
                var packagePrice = hasPackage ? row.Cells[2].Value.ToString() : "-";
                exportStrings.Add(new string[]{ row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(), packagePrice });
            }

            // Teeb 2-dimensionaalse array, mis hoiab exportString.Count * 3 stringi
            var exportData = Array.CreateInstance(typeof(string), exportStrings.Count, 3);
            // Täidab selle array
            for (int i = 0; i < exportStrings.Count; ++i)
            {
                exportData.SetValue(exportStrings[i][0], i, 0);
                exportData.SetValue(exportStrings[i][1], i, 1);
                exportData.SetValue(exportStrings[i][2], i, 2);
            }

            CSV.delimiter = txtExportDelimiter.Text;
            CSV.textQualifier = txtExportQualifier.Text;
            int numLines = (int)CSV.saveDataToCsv(ref exportData, isAppend);

            if (numLines != exportStrings.Count)
            {
                MessageBox.Show("Faili kirjutamine nurjus! Kirjutati " + numLines.ToString() + " rida.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // EXPORT FAILI VALIMISNUPU VAJUTUS
        /* Salvestab andmete eksportimiseks faili path'i.
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad
         *      e: sündmustele vastavad argumendid (EventArgs tüüpi)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void btnExportOpen_Click(object sender, EventArgs e)
        {
            txtExportPath.Text = (string)CSV.setFileToSave();
        }

        // SIIA PANE FUNKTSIOONI NIMI VÕI KIRJELDAV
        /* Funktsiooni kirjeldus siia!
         * 
         * PARAMEETRID (SISEND):
         *      a
         *      b
         *      c
         *      
         * PARAMEETRID (VÄLJUND):
         *      d
         *      e
         *      f
         *      
         * TAGASTUSVÄÄRTUSED:
         * 
         */
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

        // GRAAFIKUL ZOOMIMINE
        /* Funktsioon muudab graafikul olevaid algus- ja lõppkuupäeva vastavalt ette antud parameetritele.
         * Kui kõik andmeid pole olemas, siis funktsioon küsib API-lt uusi andmeid. Graafikut uuendatakse 
         * (updateGraph()) ning hinnad arvutatakse uuesti (calcPrice()).
         * 
         * PARAMEETRID (SISEND):
         *      start: alguskuupäev
         *      stop: lõppkuupäev
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // ROHEPAKETTIDE ROHELISEKS VÄRVIMINE
        /* Funktsioon värvib tabelis etteantud indeksiga rea rohepaketi tulba roheliseks, et eristada rohepakette.
         * 
         * PARAMEETRID (SISEND):
         *      table: tabel, mille tulpa värvitakse
         *      greenPacketRow: rohepaketi rea indeks
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // REA VÄRVI TAASTAMINE
        /* Funktsioon taastab rea esialgse värvi, jättes seejuures puutumata rohepaketi tulba.
         * 
         * PARAMEETRID (SISEND):
         *      table: tabel, mille ridu taastatakse
         *      rowIDX: rea indeks
         *      skipRow: rohepaketi rea indeks
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // REA VÄRVIMINE
        /* Funktsiooni kirjeldus siia!
         * 
         * PARAMEETRID (SISEND):
         *      table: tabel, mille ridu värvitakse
         *      rowIdx: rea indeks
         *      foreColor: teksti värv 
         *      backColor: tausta värv
         *      skipRow: rohepaketi rea indeks
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // SUURIMA JA VÄIKSEMA HETKEHINNAGA PAKETTIDE VÄRVIMINE
        /* Funktsioon käib läbi pakettide tabeli ning värvib punaseks kõrgeima hinnaga paketi rea ja roheliseks
         * madalaima hinnaga paketi rea. Funktsioon kasutab seejuures funktsiooni setRowColor() ning värvide 
         * taastamiseks funktsiooni resetRowColor().
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void updatePakettideVarvid()
        {
            // Leiab minimaalse ja maksimaalse hetkehinnaga müüja
            Tuple<int, double> minRida = Tuple.Create(0, Double.PositiveInfinity), maxRida = Tuple.Create(0, Double.NegativeInfinity);
            for (int i = 0; i < tablePackages.Rows.Count; ++i)
            {
                double price;
                if (!Double.TryParse(tablePackages.Rows[i].Cells[9].Value.ToString(), out price))
                {
                    break;
                }

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
                double value;
                if (!Double.TryParse(tablePackages.Rows[i].Cells[9].Value.ToString(), out value))
                {
                    Console.WriteLine("EL Kinder Bueno");
                    break;
                }
                if (Math.Abs(value - minRida.Item2) < 0.0005)
                {
                    this.setRowColor(ref tablePackages, i, isNotDarkMode ? chalkWhite : Color.Black, isNotDarkMode ? Color.DarkGreen : Color.LightGreen, 8);
                    //Console.WriteLine("Pakett " + i.ToString() + " on odavaim!");
                }
                else if (Math.Abs(value - maxRida.Item2) < 0.0005)
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

        // PAKETTIDE HINDADE UUENDAMINE
        /* Funktsiooniga saab uuendada pakettide tabelis olevaid hindasid. Hetkehindasid on võimalik eemaldada 
         * ning lõpphindu uuesti arvutada. Juhul kui hetkehinnad arvutatakse uuesti, taastatakse rea värv ning 
         * kui lõpphind arvutatakse uuesti, siis uuendatakse ridade värve.
         * 
         * PARAMEETRID (SISEND):
         *      time: Soovitud ajahetk, mida pakettide tabelisse kuvada (DateTime)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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
                // Leitakse valitud ajahetkele vastav börsihind
                double stockPrice = VK.priceRange.Find(Tuple => Tuple.Item1 == time).Item2;
                for (int i = 0; i < tablePackages.Rows.Count; ++i)
                {
                    // Arvutatakse paketti arvestades tarbija lõpphind
                    double price = AR.finalPrice(stockPrice, this.packageInfo[i], time);

                    tablePackages.Rows[i].Cells[9].Value = price.ToString("0.000");
                }
                // Uuendatakse tabeli odavaimate/kalleimate hindade värve
                this.updatePakettideVarvid();
            }
        }

        // TUNNI VÕI PÄEVA PERIOODI VALIMINE
        /* Funktsioon muudab txtPerioodTundides välja kas aktiivseks või mitte aktiivseks
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void cbTundVoiPaev_CheckedChanged(object sender, EventArgs e)
        {
                txtPerioodTundides.Enabled = !cbTundVoiPaev.Checked;
        }

        // TUNDIDE ARVU PIIRAMINE TXTPERIOODTUNDIDES
        /* Funktsioon paneb maksimaalse piiri numbrite kirjutamisele sisse
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void txtPerioodTundides_TextChanged(object sender, EventArgs e)
        {
            this.handleNumberBoxChanged(ref txtPerioodTundides, 24);
        }

        // BÖRSIHINNA TRENDI LEIDMINE
        /* leiab valitud ajavahemikus kõige odavama ja kallima tundide vahemiku või päeva
         * 
         * PARAMEETRID (SISEND):
         *      sender: objekt, mille kohta sündmused kehtivad (object)
         *      e: sündmustele vastavad parameetrid (EventArgs)
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void btnLeiaTrend_Click(object sender, EventArgs e)
        {
            //Uus ajutine VecT kuhu salvestada börsi hinda
            VecT ajutineVecT = new VecT();
            //Jada double arve kuhu salvestatakse hinnad
            double[] keskmisteHindadeBin;
            //Kui checkbox cbTundVoiPaev on valitud siis
            if (cbTundVoiPaev.Checked == true)
            {
                //Tee jadale 7 kasti iga päeva jaoks üks
                keskmisteHindadeBin = new double[7];
                //Nulli nende väärtused
                for(int i = 0; i < 7; i++) 
                {
                    keskmisteHindadeBin[i] = 0;
                }
                //Kui alguse ja lõpu vahel on vähem kui 7 päeva vahet siis liigutatakse algust tahapoole
                if ((dateStopTime.Value - dateStartTime.Value).TotalDays < 7.0)
                {
                    dateStartTime.Value = dateStopTime.Value.Date.AddDays(-6) + new TimeSpan(0, 0, 0);                    
                }
                //Kui algus ja lõpp aja vahel on mitte 7 jaguv arv päevi
                //siis liigutatakse algust edasi nii palju et oleks 7 jaguv arv päevi vahet
                if (((dateStopTime.Value.Date - dateStartTime.Value.Date).TotalDays+1) % 7 != 0) 
                {
                    dateStartTime.Value = dateStartTime.Value.AddDays(((dateStopTime.Value.Date - dateStartTime.Value.Date).TotalDays + 1) % 7);
                }
                //Uuendab graafikut
                priceChart_zoom(dateStartTime.Value, dateStopTime.Value);
                //Küsib selle ajaperioodi börsi andmed
                ajutineVecT = AP.HindAegInternet(dateStartTime.Value, dateStopTime.Value);
                //Kontrollib kas sai internetist andmeid 
                if (ajutineVecT.Count != 0)
                {
                    //Liidab iga päeva tunni elektri hinna selle päeva "binni"
                    foreach (var item in ajutineVecT)
                    {
                        keskmisteHindadeBin[(int)item.Item1.DayOfWeek] += item.Item2;
                    }
                    //Ajutised muutujad
                    double vaiksem = keskmisteHindadeBin[0] /= (ajutineVecT.Count / 7);
                    double suurim = keskmisteHindadeBin[0] /= (ajutineVecT.Count / 7);
                    int dayOfWeekValueSuurim = 0;
                    int dayOfWeekValueVaikseim = 0;
                    //Leiab kalleima ja odavaima päeva
                    for (int i = 1; i < 7; i++)
                    {
                        keskmisteHindadeBin[i] /= (ajutineVecT.Count / 7);
                        if (keskmisteHindadeBin[i] < vaiksem)
                        {
                            dayOfWeekValueVaikseim = i;
                            vaiksem = keskmisteHindadeBin[i];
                        }
                        if (keskmisteHindadeBin[i] > suurim)
                        {
                            dayOfWeekValueSuurim = i;
                            suurim = keskmisteHindadeBin[i];
                        }
                    }
                    //Leitud kalleima ja odavaima päeva indeksiga trükitakse messageboxiga välja need eesti keeles
                    CultureInfo cultureInfo = new CultureInfo("et-EE"); // Estonian culture
                    string dayNameVaikseim = cultureInfo.DateTimeFormat.GetDayName((DayOfWeek)dayOfWeekValueVaikseim);
                    string dayNameSuurim = cultureInfo.DateTimeFormat.GetDayName((DayOfWeek)dayOfWeekValueSuurim);

                    MessageBox.Show("Tavaliselt odavaim tarbimis päev on: " + dayNameVaikseim + "," + Environment.NewLine + "Tavaliselt kalleim tarbimis päev on: " + dayNameSuurim);
                }
            }
            else 
            {
                //Tee jadale 24 kasti iga tunni jaoks üks
                keskmisteHindadeBin = new double[24];
                //Nulli nende väärtused
                for (int i = 0; i < 24; i++)
                {
                    keskmisteHindadeBin[i] = 0;
                }
                //Kui alguse ja lõpu vahel on vähem kui 24 tundi vahet siis liigutatakse algus ja lõpp 24 tunnisele vahele
                if ((dateStopTime.Value - dateStartTime.Value).TotalHours < 24.0)
                {
                    dateStartTime.Value += new TimeSpan(0, 0, 0);
                    dateStopTime.Value += new TimeSpan(23, 0, 0);
                }
                //Uuenda graafikut
                priceChart_zoom(dateStartTime.Value, dateStopTime.Value);
                //Küsib selle ajaperioodi börsi andmed
                ajutineVecT = AP.HindAegInternet(dateStartTime.Value, dateStopTime.Value);
                //Kontrollib kas sai internetist andmeid
                if (ajutineVecT.Count != 0)
                {
                    //Liidab iga tunni andmed tema "binni"
                    foreach (var item in ajutineVecT)
                    {
                        keskmisteHindadeBin[(int)item.Item1.Hour] += item.Item2;
                    }
                    //ajutised muutujad
                    ajutineVecT.Clear();
                    ajutineVecT = new VecT();
                    DateTime ajutineDate = new DateTime(2000, 1, 1, 0, 0, 0);
                    DateTime ajutineDateAlgus = new DateTime(2000, 1, 1, 0, 0, 0);
                    DateTime algusAeg = new DateTime(2000, 1, 1, 0, 0, 0);
                    //Täidab ajutise VecT "binni" ehk keskmisteHindadeBin andmetega
                    for (int i = 0; i < 24; i++)
                    {
                        ajutineVecT.Add(Tuple.Create(ajutineDate, keskmisteHindadeBin[i]));
                        ajutineDate = ajutineDate.AddHours(1);
                    }
                    //Vaatab kas on kirjutatud ajaperiood mida otsitakse
                    double periood = 0.0;
                    if (!string.IsNullOrEmpty(txtPerioodTundides.Text))
                    {
                        periood = Double.Parse(txtPerioodTundides.Text);
                    }
                    //Kui ei ole siis default on 1 tund
                    else
                    {
                        periood = 1.0;
                    }

                    //Küsib väikseima hinnaga integrali
                    double ajutineOutDouble = 0;
                    int testSmallest = AR.smallestIntegral(ajutineVecT, 1.0, periood, ajutineDateAlgus, ajutineDate, out ajutineOutDouble, out algusAeg);
                    //Kuvab selle tekstina MessageBoxis
                    if (testSmallest == 0)
                    {
                        MessageBox.Show("Tavaliselt odavaim tarbimis aeg on: " + algusAeg.ToString("HH") + " - " + algusAeg.AddHours(Convert.ToInt32(periood)).ToString("HH"));
                    }
                    //küsib suurima hinnaga integrali
                    int testLargest = AR.largestIntegral(ajutineVecT, 1.0, periood, ajutineDateAlgus, ajutineDate, out ajutineOutDouble, out algusAeg);
                    //Kuvab selle tekstina MessageBoxis
                    if (testLargest == 0)
                    {
                        MessageBox.Show("Tavaliselt kalleim tarbimis aeg on: " + algusAeg.ToString("HH") + " - " + algusAeg.AddHours(Convert.ToInt32(periood)).ToString("HH"));
                    }
                }
            }
        }



        // PAKETTIDE TABELIS MALLI HINNA UUENDAMINE
        /* Tablisse lisatakse/eemaldatakse pakettide lõpphinnad vastavalt valitud kasutusmallile.
         * 
         * PARAMEETRID (SISEND):
         *      startTime: tarbimise algusaeg
         *      usageLength: tarbimise kestus
         *      power: tarbimisel kasutatav võimsus
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
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

        // PAKETTIDE TARBIMISHINNA UUENDAMINE
        /* Kui kasutaja on üles laadinud ning valinud huvi pakkuva ajaperioodi, kuvatakse tabelisse kõikide pakettide
         * tarbimishinnad valitud ajaperioodil.
         *      
         * TAGASTUSVÄÄRTUSED:
         * -
         */
        private void updatePakettideTarbimishind()
        {
            if (VK.userDataTimeRange.Count == 0) // Kui tarbimisandmed puuduvad
            {
                return; // Katkestab funktsiooni töö
            }
            var stockCost = VK.createRange(this.priceData, VK.userDataRangeStart, VK.userDataRangeStop); // Börsihinna väärtuste loomine
            var start = dateStartTime.Value;
            var stop  = dateStopTime.Value;
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
                        //Console.WriteLine(us.Stock.Item1.ToString() + ": " + us.Usage.ToString() + "; " + us.Stock.Item2.ToString());
                        packageUsageCost.Add(us.Usage * AR.finalPrice(us.Stock.Item2, packageInfo[c], us.Stock.Item1)); // Arvutab iga tunni hinna ja lisab listi
                    }
                    foreach (var item in packageUsageCost)
                    {
                        cost += item; // Liidab tunnihinnad 
                    }
                    tablePackages.Rows[c].Cells[10].Value = (cost / 100).ToString("0.00"); // Lisab saadud hinna pakettide tabelisse 
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
