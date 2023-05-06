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

        DateTime userDataRangeStart { get; }
        DateTime userDataRangeStop { get; }

        List<DateTime> priceTimeRange { get; }
        List<double> priceCostRange { get; }
        VecT priceRange { get; }

        DateTime priceRangeStart { get; }
        DateTime priceRangeStop { get; }

        double averagePrice { get; }


        VecT createRange(VecT inData, DateTime start, DateTime stop);

        bool createUserDataRange(VecT inData, DateTime start, DateTime stop);
        bool createStockRange(VecT inData, DateTime start, DateTime stop);
        void addLastPoints();
    }
}
