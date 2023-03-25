using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kasutajaliides
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bot = new LokaalneAndmepyydja.CLAP();
            bot.chooseFile();
            string contents = "";
            bot.readFile(ref contents);
            var data = bot.parseContents(contents);

            string msg = data[1].Item1.ToString() + " " + data[1].Item2.ToString();

            MessageBox.Show(msg);
        }
    }
}
