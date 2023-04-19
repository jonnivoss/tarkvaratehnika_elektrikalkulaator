using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace Arvutaja
{
    interface IArvutaja
    {
        int integreerija(VecT andmed1, VecT andmed2, System.DateTime alumine, System.DateTime ylemine, ref double integraal);
        int average(
            VecT andmed,
            System.DateTime alumine,
            System.DateTime ylemine,
            ref double avg
        );
    }
}
