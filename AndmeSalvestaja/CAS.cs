using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using VecUCT = System.Collections.Generic.List<System.Tuple<string, double>>;

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
        public VecUCT useCases = new VecUCT
        {
            Tuple.Create("",             0.0),
            Tuple.Create("Röster",       800.0),
            Tuple.Create("Tolmuimeja",   2000.0),
            Tuple.Create("Televiisor",   90.0),
            Tuple.Create("Pesumasin",    900.0),
            Tuple.Create("Veekeetja",    3000.0),
            Tuple.Create("Elektripliit", 3000.0),
            Tuple.Create("Köögikombain", 300.0),
            Tuple.Create("Kohvimasin",   1500.0),
            Tuple.Create("Raadio",       50.0),
            Tuple.Create("Munakeetja",   350.0),
            Tuple.Create("Föön",         2000.0)
        };
    }

    public class CAS : AndmeSalvestaja.IAS
    {
        private string path = "";
        private Serializable data = new Serializable();

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

        public VecUCT getUseCases()
        {
            return this.data.useCases;
        }
    }
}
