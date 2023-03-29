using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndmeSalvestaja
{
    public enum ASSetting
    {
        test0,
        test1,

        size
    };

    interface IAS
    {
        bool loadFile();
        bool saveFile();

        bool changeSetting(ASSetting setting, string value);
        string getSetting(ASSetting setting);
    }
}
