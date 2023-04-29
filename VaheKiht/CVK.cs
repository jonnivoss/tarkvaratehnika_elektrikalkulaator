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
        public List<DateTime> userDataTimeRange { get; } = new List<DateTime>();
        public List<double> userDataUsageRange { get; } = new List<double>();
        public VecT userDataRange { get; } = new VecT();

        public List<DateTime> priceTimeRange { get; } = new List<DateTime>();
        public List<double> priceCostRange { get; } = new List<double>();
        public VecT priceRange { get; } = new VecT();

        public double averagePrice { get; } = 0.0;

        public bool createUserDataRange(VecT data, DateTime start, DateTime stop)
        {
            if (start > stop)
            {
                return false;
            }

            userDataTimeRange.Clear();
            userDataUsageRange.Clear();
            userDataRange.Clear();

            try
            {
                foreach (var item in data)
                {
                    if (item.Item1 >= start && item.Item1 <= stop)
                    {
                        userDataRange.Add(item);
                        userDataTimeRange.Add(item.Item1);
                        userDataUsageRange.Add(item.Item2);
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
            if (this.userDataTimeRange.Count > 0)
            {
                this.userDataTimeRange.Add(this.userDataTimeRange.Last().AddHours(1));
                this.userDataUsageRange.Add(this.userDataUsageRange.Last());
                this.userDataRange.Add(Tuple.Create(this.userDataTimeRange.Last(), this.userDataUsageRange.Last()));
            }

            if (this.priceTimeRange.Count > 0)
            {
                this.priceTimeRange.Add(this.priceTimeRange.Last().AddHours(1));
                this.priceCostRange.Add(this.priceCostRange.Last());
                this.priceRange.Add(Tuple.Create(this.priceTimeRange.Last(), this.priceCostRange.Last()));
            }
        }
    }
}
