﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime;

using DatePriceT = System.Tuple<System.DateTime, float>;
using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

namespace Andmepyydja
{
    interface IAP
    {
        bool chooseFile();

        bool readFile(ref string contents);

        
        VecT parseContents(string contents);


        //bett

        DateTime abua(string a);
        void aia(string a);
        DatePriceT iseOled(DateTime algus, DateTime lopp);
    }
}
