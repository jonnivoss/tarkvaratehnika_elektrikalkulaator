using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Net.Http;
using System.Net;


//https://dashboard.elering.ee/assets/api-doc.html#/balance-controller/getAllUsingGET

using DatePriceT = System.Tuple<System.DateTime, double>;
using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using ParseCSVDataLineT = System.Tuple<string, char>;

using PackageT = System.Collections.Generic.List<Andmepyydja.PackageInfo>;

namespace Andmepyydja
{
    public class PackageInfo
    {
        public string providerName, packageName;
        public double monthlyPrice, sellerMarginal, basePrice, dayPrice, nightPrice;
        public bool isDayNight, isStockPackage, isGreenPackage;
    }

    public class CAP : Andmepyydja.IAP
    {
        private OpenFileDialog ofdUserData = new OpenFileDialog(), ofdPackage = new OpenFileDialog();
        private string fileNameUserData = "";
        private string fileNamePackage = "";

        public bool chooseFile()
        {
            DialogResult result = ofdUserData.ShowDialog();
            if (result != DialogResult.OK)
            {
                return false;
            }

            fileNameUserData = ofdUserData.FileName;

            return true;
        }
        public bool chooseFilePackages()
        {
            DialogResult result = ofdPackage.ShowDialog();
            if (result != DialogResult.OK)
            {
                return false;
            }

            fileNamePackage = ofdPackage.FileName;

            return true;
        }

        public bool readUserDataFile(ref string contents)
        {
            if (this.fileNameUserData == "")
            {
                // Failinime pole valitud
                return false;
            }
            try
            {
                // Proovib failist lugeda
                contents = File.ReadAllText(this.fileNameUserData, Encoding.UTF8);
                // Kui fail oli tühi, siis see on automaatselt *fail*
                return (contents != "");
            }
            catch (Exception)
            {
                // Faili lugemine ebaõnnestus
                return false;
            }
        }
        public bool readPackageFile(ref string contents)
        {
            if (this.fileNamePackage == "")
            {
                // Failinime pole valitud
                return false;
            }
            try
            {
                // Proovib failist lugeda
                contents = File.ReadAllText(this.fileNamePackage, Encoding.UTF8);
                // Kui fail oli tühi, siis see on automaatselt *fail*
                return (contents != "");
            }
            catch (Exception)
            {
                // Faili lugemine ebaõnnestus
                return false;
            }
        }

        private static DatePriceT parseCSVUserDataLine(ParseCSVDataLineT arguments)
        {
            // ParseCSVDataLineT.Item1 on rida
            // ParseCSVDataLineT.Item2 on delimiter

            // Lõhub rea tokeniteks
            var rida = arguments.Item1.Split(arguments.Item2);
            if ((rida.Length < 4) || (rida[2].Length == 0))
            {
                return null;
            }
            double num = Convert.ToDouble(rida[2]);

            DateTime d;
            try
            {
                d = DateTime.ParseExact(
                    rida[0],
                    "dd.MM.yyyy HH:mm",
                    CultureInfo.InvariantCulture
                );
            }
            catch (Exception)
            {
                // Kuupäeva parsimine ebaõnnestus
                return null;
            }
            return new DatePriceT(d, num);
        }

        public string getUserDataFileName()
        {
            return this.fileNameUserData;
        }
        public void setUserDataFileName(string filename)
        {
            this.fileNameUserData = filename;
        }

        private static VecT parseCSVUserData(
            string contents,
            string tableBeginToken,
            char delimiter
        )
        {
            VecT v = new VecT();

            List<string> splitContents;
            try
            {
                // Discardib kõik andmed, mis asuvad enne tabeli headerit
                contents = contents.Substring(contents.IndexOf(tableBeginToken));
                // Teeb stringi ridade massiiviks
                splitContents = contents.Split('\n').ToList();
                // Eemaldab tabeli headeri
                splitContents.RemoveAt(0);
            }
            catch (Exception)
            {
                return v;
            }

            foreach (string line in splitContents)
            {
                if (line.Length == 0)
                {
                    continue;
                }

                var item = parseCSVUserDataLine(Tuple.Create(line, delimiter));
                if (item == null)
                {
                    continue;
                }

                v.Add(item);
            }

            return v;
        }

        public VecT parseUserData(string contents)
        {
            return CAP.parseCSVUserData(
                contents,
                "Algus",
                ';'
            );
        }


        public string getPackageFileName()
        {
            return this.fileNamePackage;
        }
        public void setPackageFileName(string fileName)
        {
            this.fileNamePackage = fileName;
        }

        private static PackageInfo parseCSVPackageDataLine(ParseCSVDataLineT arguments)
        {
            // ParseCSVDataLineT.Item1 on rida
            // ParseCSVDataLineT.Item2 on delimiter

            // Lõhub rea tokeniteks
            var rida = arguments.Item1.Split(arguments.Item2);
            if (rida.Length < 8)
            {
                return null;
            }

            var info = new PackageInfo();
            try
            {
                info.providerName = rida[0];
                info.packageName = rida[1];
                info.monthlyPrice = Convert.ToDouble(rida[2]);
                info.sellerMarginal = Convert.ToDouble(rida[3]);
                info.basePrice = Convert.ToDouble(rida[4]);
                info.dayPrice = info.basePrice;
                info.nightPrice = Convert.ToDouble(rida[5]);
                info.isStockPackage = Convert.ToBoolean(rida[6]);
                info.isGreenPackage = Convert.ToBoolean(rida[7]);

                info.isDayNight = info.nightPrice == 0.0 ? true : false;
            }
            catch (Exception)
            {
            }

            return info;
        }

        private static PackageT parseCSVPackageData(
            string contents,
            string tableBeginToken,
            char delimiter
        )
        {
            PackageT v = new PackageT();

            List<string> splitContents;
            try
            {
                // Discardib kõik andmed, mis asuvad enne tabeli headerit
                contents = contents.Substring(contents.IndexOf(tableBeginToken));
                // Teeb stringi ridade massiiviks
                splitContents = contents.Split('\n').ToList();
                // Eemaldab tabeli headeri
                splitContents.RemoveAt(0);
            }
            catch (Exception)
            {
                return v;
            }

            foreach (string line in splitContents)
            {
                if (line.Length == 0)
                {
                    continue;
                }

                var item = parseCSVPackageDataLine(Tuple.Create(line, delimiter));
                if (item == null)
                {
                    continue;
                }

                v.Add(item);
            }

            return v;
        }
        public PackageT parsePackage(string contents)
        {
            return CAP.parseCSVPackageData(
                contents,
                "Pakkuja",
                ';'
            );
        }

        //siit algab neti otsimine


        //muuda unix standard time DateTimeiks
        public DateTime UnixToDateTime(string a)
        {
           
            long unixTime = long.Parse(a);
            DateTimeOffset systemTime = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            DateTime yez = systemTime.UtcDateTime;
            return yez.AddHours(2);
        }

        //tagastab vect DatePrice 
        public VecT HindAegInternet(DateTime algus1, DateTime lopp1)
        {
            string url = "https://dashboard.elering.ee/api/nps/price?";
            VecT nett = new VecT();
            DateTime algus = algus1.AddHours(-2);
            DateTime lopp = lopp1.AddHours(-2);

            using (var httpClient = new HttpClient())
            {
                // Construct the URL with parameters
                string start = algus.ToString("yyyy-MM-ddTHH") + "%3A00%3A00.999Z";
                string end = lopp.ToString("yyyy-MM-ddTHH") + "%3A00%3A00.999Z";
                string requestUrl = $"{url}start={start}&end={end}";

                // Send request and get response
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                var responseString = httpClient.GetStringAsync(requestUrl).Result;

                // Split the response string into array of substrings
                string[] responseStrings = responseString.Split('{', '[', '}', ']', ',');

                // Loop through the substrings and extract date-time and price values
                for (int i = 4; i < responseStrings.Length; i += 2)
                {
                    int eli = i - 1;
                    if (String.Equals(responseStrings[i], "\"fi\":") || String.Equals(responseStrings[eli], "\"fi\":"))
                    {
                        break;
                    }
                    if (String.IsNullOrEmpty(responseStrings[i]))
                    {
                        continue;
                    }

                    // Extract date-time and price strings
                    string ajaString = responseStrings[eli].Substring(responseStrings[eli].IndexOf(":") + 1);
                    string hinnaString = responseStrings[i].Substring(responseStrings[i].IndexOf(":") + 1);

                    // Parse strings to DateTime and double values
                    DateTime aeg = UnixToDateTime(ajaString);
                    double hind = double.Parse(hinnaString, CultureInfo.InvariantCulture.NumberFormat);

                    // Create DatePriceT tuple and add it to VecT
                    DatePriceT ajutine = Tuple.Create(aeg, hind);
                    nett.Add(ajutine);
                }
            }
            return nett;
        }
    }
}
