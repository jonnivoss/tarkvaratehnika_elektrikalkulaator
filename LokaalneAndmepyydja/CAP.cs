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

using DatePriceT = System.Tuple<System.DateTime, float>;
using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

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

            contents = contents.Substring(contents.IndexOf("Algus"));
            var splitcontent = contents.Split('\n').ToList();
            splitcontent.RemoveAt(0);

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
                float num = (float)Convert.ToDouble(rida[2]);

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

        static void aia(string a)
        {

            string[] nameParts = a.Split('{', '[', '}', ']', ',');

            for (int i = 3; i < nameParts.Length; i++)
            {
                if (String.Equals(nameParts[i], "\"fi\":"))
                {
                    break;
                }
                if (String.IsNullOrEmpty(nameParts[i]))
                {
                    continue;
                }
                Console.WriteLine(nameParts[i]);
            }

        }

        static async Task Main(string[] args)
        {
            string urla = "https://dashboard.elering.ee/api/nps/price?";
            // Console.WriteLine("mine putsi \n");

            using (var httpClient = new HttpClient())
            {
                string starTime = "2023-03-25T10";
                string endTime = "2023-03-25T20";
                string url = urla + "start=" + starTime + "%3A00%3A00.999Z&end=" + endTime + "%3A00%3A00.999Z";

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

                var responseString = await httpClient.GetStringAsync(url);
                aia(responseString);

            }
            Console.ReadKey();

        }
    }
}
