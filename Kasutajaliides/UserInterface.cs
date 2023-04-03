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
    
    public partial class UserInterface : Form
    {
        private Andmepyydja.CAP AP = new Andmepyydja.CAP();
        private void btnAvaCSV_Click(object sender, EventArgs e)
        {
            AP.chooseFile();

        }
        public UserInterface() 
        {
            InitializeComponent();
        }
    }
}
