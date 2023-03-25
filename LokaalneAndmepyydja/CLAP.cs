using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

using DatePriceT = System.Tuple<System.DateTime, float>;
using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

namespace LokaalneAndmepyydja
{
    public class CLAP : LokaalneAndmepyydja.ILAP
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
            contents = File.ReadAllText(this.fname, Encoding.UTF8);

            return (contents != "");
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
                var item = new Tuple<DateTime, float>(d, num);
                v.Add(item);
            }

            return v;
        }
    }
}
