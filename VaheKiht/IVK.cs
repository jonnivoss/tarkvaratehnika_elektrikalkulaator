using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace VaheKiht
{
    public interface IVK
    {
        bool createUserDataRange(VecT data, DateTime start, DateTime stop);
        bool createStockRange(VecT data, DateTime start, DateTime stop);

        List<DateTime> getUserDataTimeRange();
        List<double> getUserDataUsageRange();

        List<DateTime> getStockTimeRange();
        List<double> getStockPriceRange();


    }
}
