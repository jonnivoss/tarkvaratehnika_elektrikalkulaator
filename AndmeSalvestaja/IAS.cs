using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
using VecUCT = System.Collections.Generic.Dictionary<string, System.Tuple<double, double>>;

namespace AndmeSalvestaja
{
    public enum ASSetting
    {
        tarbijaAndmed,
        paketiAndmed,
        suurendusLubatud,

        size
    };

    interface ISerializable
    {
        Dictionary<ASSetting, string> setMap { get; set; }
        VecT marketData { get; set; }
        VecUCT useCases { get; }
    }

    public interface IAS
    {
        bool loadFile();
        bool saveFile();

        bool changeSetting(ASSetting setting, string value);
        string getSetting(ASSetting setting);

        void setMarketData(VecT data);
        VecT getMarketData();

        VecUCT getUseCases();
    }
}
