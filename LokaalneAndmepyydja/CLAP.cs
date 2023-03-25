using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public bool readFile(byte[] contents)
        {
            return true;
        }
    }
}
