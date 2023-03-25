using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LokaalneAndmepyydja
{
    interface ILAP
    {
        bool chooseFile();

        bool readFile(ref string contents);
    }
}
