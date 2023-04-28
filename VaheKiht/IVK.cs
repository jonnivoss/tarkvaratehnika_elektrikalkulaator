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
        void addLastPoints();

        List<DateTime> getUserDataTimeRange();
        List<double> getUserDataUsageRange();
        VecT getUserDataRange();

        List<DateTime> getPriceTimeRange();
        List<double> getPriceCostRange();
        VecT getPriceRange();
        double getAveragePrice();
    }
}
