using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace AndmeSalvestaja
{
    public class CAS : AndmeSalvestaja.IAS
    {
        private string path = "";
        private Dictionary<ASSetting, string> setMap = new Dictionary<ASSetting, string>
        {
            { ASSetting.test0, "" },
            { ASSetting.test1, "" }
        };

        public CAS(string savepath)
        {
            this.path = savepath;
        }

        public bool loadFile()
        {
            if (this.path.Length == 0)
            {
                return false;
            }

            try
            {
                var contents = File.ReadAllText(this.path);
                this.setMap = JsonConvert.DeserializeObject<Dictionary<ASSetting, string>>(contents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool saveFile()
        {
            return true;
        }

        public bool changeSetting(ASSetting setting, string value)
        {
            if ((int)setting >= (int)ASSetting.size)
            {
                return false;
            }

            try
            {
                this.setMap[setting] = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string getSetting(ASSetting setting)
        {
            if ((int)setting >= (int)ASSetting.size)
            {
                return null;
            }

            return this.setMap[setting];
        }
    }
}
