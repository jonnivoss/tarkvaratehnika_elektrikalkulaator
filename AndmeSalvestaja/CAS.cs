using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndmeSalvestaja
{
    public class CAS : AndmeSalvestaja.IAS
    {
        private string path = "";
        private string[] setMap = new string[(int)ASSetting.size];

        public CAS(string savepath)
        {
            this.path = savepath;
        }

        public bool loadFile()
        {
            return true;
        }
        public bool saveFile()
        {
            return true;
        }

        public bool changeSetting(ASSetting setting, string value)
        {
            int iset = (int)setting;
            if (iset >= (int)ASSetting.size)
            {
                return false;
            }

            try
            {
                this.setMap[iset] = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string getSetting(ASSetting setting)
        {
            int iset = (int)setting;
            if (iset >= (int)ASSetting.size)
            {
                return null;
            }

            return this.setMap[iset];
        }
    }
}
