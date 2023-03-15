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
        protected ClassTest.CClassTest component = new ClassTest.CClassTest();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int arv = component.arvuta(1, 2);
            MessageBox.Show("Hello world " + arv.ToString(), "title", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
