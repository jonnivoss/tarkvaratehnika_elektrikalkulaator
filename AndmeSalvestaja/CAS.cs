using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

namespace AndmeSalvestaja
{
    public class Serializable
    {
        public Dictionary<ASSetting, string> setMap = new Dictionary<ASSetting, string>
        {
            { ASSetting.tarbijaAndmed,    "" },
            { ASSetting.suurendusLubatud, "0" }
        };
        public VecT marketData = new VecT { };
    }

    public class CAS : AndmeSalvestaja.IAS
    {
        private string path = "";
        private Serializable data;

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
                this.data = JsonConvert.DeserializeObject<Serializable>(contents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool saveFile()
        {
            try
            {
                var contents = JsonConvert.SerializeObject(this.data, Formatting.Indented);
                File.WriteAllText(this.path, contents);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool changeSetting(ASSetting setting, string value)
        {
            if ((int)setting >= (int)ASSetting.size)
            {
                return false;
            }

            try
            {
                this.data.setMap[setting] = value;
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

            return this.data.setMap[setting];
        }

        public void setMarketData(VecT data)
        {
            this.data.marketData = data;
        }
        public VecT getMarketData()
        {
            return this.data.marketData;
        }
    }
}
