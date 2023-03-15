using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTest
{
    public class CClassTest : IClassTest
    {
        public int arvuta(int arv1, int arv2)
        {
            return arv1 + arv2;
        }
    }
}
