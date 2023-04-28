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
        VecT userRange = new VecT();

        List<DateTime> priceTimeRange = new List<DateTime>();
        List<double> priceCostRange = new List<double>();
        VecT priceRange = new VecT();

        double averagePrice = 0.0;

        public bool createUserDataRange(VecT data, DateTime start, DateTime stop)
        {
            if (start > stop)
            {
                return false;
            }

            userTimeRange.Clear();
            userUsageRange.Clear();
            userRange.Clear();

            try
            {
                foreach (var item in data)
                {
                    if (item.Item1 >= start && item.Item1 <= stop)
                    {
                        userRange.Add(item);
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
            priceRange.Clear();
            averagePrice = 0.0;

            try
            {
                foreach (var item in data)
                {
                    if (item.Item1 >= start && item.Item1 <= stop)
                    {
                        priceRange.Add(item);
                        priceTimeRange.Add(item.Item1);
                        priceCostRange.Add(item.Item2);
                        // Keskmise hinna arvutamiseks hindade kokku liitmine
                        averagePrice += item.Item2; // s/kWh
                    }
                }
                // Jagab kokkuliidetud hinnad hindade arvuga ==> keskmine hind
                averagePrice /= priceCostRange.Count;

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
                this.userRange.Add(Tuple.Create(this.userTimeRange.Last(), this.userUsageRange.Last()));
            }

            if (this.priceTimeRange.Count > 0)
            {
                this.priceTimeRange.Add(this.priceTimeRange.Last().AddHours(1));
                this.priceCostRange.Add(this.priceCostRange.Last());
                this.priceRange.Add(Tuple.Create(this.priceTimeRange.Last(), this.priceCostRange.Last()));
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
        public VecT getUserDataRange()
        {
            return this.userRange;
        }

        public List<DateTime> getPriceTimeRange()
        {
            return this.priceTimeRange;
        }
        public List<double> getPriceCostRange()
        {
            return this.priceCostRange;
        }
        public VecT getPriceRange()
        {
            return this.priceRange;
        }
        public double getAveragePrice()
        {
            return this.averagePrice;
        }
    }
}
