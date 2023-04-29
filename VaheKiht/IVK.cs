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
        List<DateTime> userDataTimeRange { get; }
        List<double> userDataUsageRange { get; }
        VecT userDataRange { get; }

        List<DateTime> priceTimeRange { get; }
        List<double> priceCostRange { get; }
        VecT priceRange { get; }
        double averagePrice { get; }


        bool createUserDataRange(VecT data, DateTime start, DateTime stop);
        bool createStockRange(VecT data, DateTime start, DateTime stop);
        void addLastPoints();
    }
}
