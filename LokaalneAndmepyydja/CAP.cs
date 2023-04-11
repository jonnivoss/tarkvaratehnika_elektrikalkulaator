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

namespace Andmepyydja
{
    public class CAP : Andmepyydja.IAP
    {
        private OpenFileDialog ofd = new OpenFileDialog();
        private string fname = "";

        public bool chooseFile()
        {
            DialogResult result = ofd.ShowDialog();
            if (result != DialogResult.OK)
            {
                return false;
            }

            fname = ofd.FileName;

            return true;
        }

        public bool readFile(ref string contents)
        {
            if (this.fname == "")
            {
                return false;
            }
            try
            {
                contents = File.ReadAllText(this.fname, Encoding.UTF8);
                return (contents != "");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public VecT parseContents(string contents)
        {
            VecT v = new VecT();

            System.Collections.Generic.List<string> splitcontent;
            try
            {
                contents = contents.Substring(contents.IndexOf("Algus"));
                splitcontent = contents.Split('\n').ToList();
                splitcontent.RemoveAt(0);
            }
            catch (Exception)
            {
                return v;
            }

            foreach (string i in splitcontent)
            {
                if (i.Length == 0)
                {
                    continue;
                }
                var rida = i.Split(';');
                if ((rida.Length < 4) || (rida[2].Length == 0))
                {
                    continue;
                }
                double num = Convert.ToDouble(rida[2]);

                DateTime d;
                try
                {
                    d = DateTime.ParseExact(rida[0], "dd.MM.yyyy hh:mm", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    // Carry on, ;)
                    continue;
                }
                var item = new DatePriceT(d, num);
                v.Add(item);
            }

            return v;
        }



        //siit algab neti otsimine


        //muuda unix standard time DateTimeiks
        public DateTime UnixToDateTime(string a)
        {
            long unixTime = long.Parse(a);
            DateTimeOffset systemTime = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            DateTime yez = systemTime.UtcDateTime;
            return yez;
        }

        //max lopp aeg on järgmise päev 21:00
        public VecT HindAegInternet(DateTime algus, DateTime lopp)
        {
            string urla = "https://dashboard.elering.ee/api/nps/price?";
            algus = DateTime.Now;
            VecT nett = new VecT();

            using (var httpClient = new HttpClient())
            {
                string url = urla + "start=" + algus.ToString("yyyy-MM-ddTHH") + "%3A00%3A00.999Z&end=" + lopp.ToString("yyyy-MM-ddTHH") + "%3A00%3A00.999Z";

                //siin saadab Apile requesti ja salvestab selle vastuse stringi
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
                
                var responseStringTask = httpClient.GetStringAsync(url);
                var responseString = responseStringTask.Result;

                //siin jagatakse üks suur api vastuse sõne sõnede jadaks
                string[] vastuseSõned = responseString.Split('{', '[', '}', ']', ',');
                
                for (int i = 4; i < vastuseSõned.Length; i += 2)
                {
                    int eli = i - 1;
                    if (String.Equals(vastuseSõned[i], "\"fi\":") || String.Equals(vastuseSõned[eli], "\"fi\":"))
                    {
                        break;
                    }
                    if (String.IsNullOrEmpty(vastuseSõned[i]))
                    {
                        continue;
                    }

                    //siin eraldatakse numbrid sõnadest
                    string ajaString = vastuseSõned[eli].Substring(vastuseSõned[eli].IndexOf(":") + 1);
                    string hinnaString = vastuseSõned[i].Substring(vastuseSõned[i].IndexOf(":") + 1);

                    //siin muudetakse string numbriteks
                    DateTime aeg = UnixToDateTime(ajaString);
                    double hind = double.Parse(hinnaString, CultureInfo.InvariantCulture.NumberFormat);

                    //ühendab hinna ja aja ning lisab selle VecT-i
                    DatePriceT ime = Tuple.Create(aeg, hind);
                    nett.Add(ime);
                }   
            }
            return nett;
        }
    }
}
