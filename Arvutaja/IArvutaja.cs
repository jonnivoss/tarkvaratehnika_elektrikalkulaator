using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arvutaja
{
    using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;
    interface IArvutaja
    {
        int integreerija(VecT andmed1, VecT andmed2, System.DateTime alumine, System.DateTime ylemine, ref double integraal);
    }

}
