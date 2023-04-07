using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace AndmeSalvestaja
{
    public enum ASSetting
    {
        tarbijaAndmed,
        suurendusLubatud,

        size
    };

    interface IAS
    {
        bool loadFile();
        bool saveFile();

        bool changeSetting(ASSetting setting, string value);
        string getSetting(ASSetting setting);

        void setMarketData(VecT data);
        VecT getMarketData();
    }
}
