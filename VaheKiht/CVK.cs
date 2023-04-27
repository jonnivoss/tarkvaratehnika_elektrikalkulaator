using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;


namespace VaheKiht
{
    public class CVK : VaheKiht.IVK
    {
        public bool createUserDataRange(VecT data, DateTime start, DateTime stop)
        {
            return false;
        }
        public bool createStockRange(VecT data, DateTime start, DateTime stop)
        {
            return false;
        }

        public List<DateTime> getUserDataTimeRange()
        {
            return null;
        }
        public List<double> getUserDataUsageRange()
        {
            return null;
        }

        public List<DateTime> getStockTimeRange()
        {
            return null;
        }
        public List<double> getStockPriceRange()
        {
            return null;
        }
    }
}
