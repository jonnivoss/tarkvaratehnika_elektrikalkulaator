using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using VecUCT = System.Collections.Generic.Dictionary<string, System.Tuple<double, double>>;

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
            { "",             Tuple.Create(0.0, 0.0) },
            { "Röster",       Tuple.Create(1200.0, 10.0) },
            { "Tolmuimeja",   Tuple.Create(2000.0, 30.0) },
            { "Televiisor",   Tuple.Create(90.0, 120.0) },
            { "Pesumasin",    Tuple.Create(900.0, 180.0) },
            { "Veekeetja",    Tuple.Create(3000.0, 10.0) },
            { "Elektripliit", Tuple.Create(3000.0, 60.0) },
            { "Köögikombain", Tuple.Create(300.0, 15.0) },
            { "Kohvimasin",   Tuple.Create(1500.0, 5.0) },
            { "Raadio",       Tuple.Create(50.0, 120.0) },
            { "Munakeetja",   Tuple.Create(1000.0, 6.0)  },
            { "Föön",         Tuple.Create(2000.0, 10.0) }
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
