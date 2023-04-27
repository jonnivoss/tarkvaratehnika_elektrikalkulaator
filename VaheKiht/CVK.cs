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
        List<DateTime> userTimeRange = new List<DateTime>();
        List<double> userUsageRange = new List<double>();

        List<DateTime> priceTimeRange = new List<DateTime>();
        List<double> priceCostRange = new List<double>();

        public bool createUserDataRange(VecT data, DateTime start, DateTime stop)
        {
            if (start > stop)
            {
                return false;
            }

            userTimeRange.Clear();
            userUsageRange.Clear();

            try
            {
                foreach (var item in data)
                {
                    if (item.Item1 >= start && item.Item1 <= stop)
                    {
                        userTimeRange.Add(item.Item1);
                        userUsageRange.Add(item.Item2);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                // trust issues with RAM :/
                return false;
            }
        }
        public bool createStockRange(VecT data, DateTime start, DateTime stop)
        {
            if (start > stop)
            {
                return false;
            }

            priceTimeRange.Clear();
            priceCostRange.Clear();

            try
            {
                foreach (var item in data)
                {
                    if (item.Item1 >= start && item.Item1 <= stop)
                    {
                        priceTimeRange.Add(item.Item1);
                        priceCostRange.Add(item.Item2);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                // trust issues with RAM :/
                return false;
            }
        }

        public void addLastPoints()
        {
            if (this.userTimeRange.Count > 0)
            {
                this.userTimeRange.Add(this.userTimeRange.Last().AddHours(1));
                this.userUsageRange.Add(this.userUsageRange.Last());
            }

            if (this.priceTimeRange.Count > 0)
            {
                this.priceTimeRange.Add(this.priceTimeRange.Last().AddHours(1));
                this.priceCostRange.Add(this.priceCostRange.Last());
            }
        }

        public List<DateTime> getUserDataTimeRange()
        {
            return this.userTimeRange;
        }
        public List<double> getUserDataUsageRange()
        {
            return this.userUsageRange;
        }

        public List<DateTime> getPriceTimeRange()
        {
            return this.priceTimeRange;
        }
        public List<double> getPriceCostRange()
        {
            return this.priceCostRange;
        }
    }
}
