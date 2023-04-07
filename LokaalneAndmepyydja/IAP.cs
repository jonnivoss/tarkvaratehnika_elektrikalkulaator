using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime;

using DatePriceT = System.Tuple<System.DateTime, float>;
using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace Andmepyydja
{
    interface IAP
    {
        bool chooseFile();

        bool readFile(ref string contents);

        
        VecT parseContents(string contents);
    }
}
