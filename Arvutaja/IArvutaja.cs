using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace Arvutaja
{
    public interface IArvutaja
    {
        int integral(VecT andmed1, VecT andmed2, System.DateTime alumine, System.DateTime ylemine, ref double integraal);

        VecT generateUsageData(
             System.DateTime start,
             double usageLength,
             double power
        );

        int smallestIntegral(
            VecT priceData,
            double power,
            double usageLength,
            System.DateTime start,
            System.DateTime stop,
            ref double outSmallestIntegral,
            ref System.DateTime outOptimalDate
        );

        int average(
            VecT andmed,
            System.DateTime alumine,
            System.DateTime ylemine,
            ref double avg
        );

        double finalPrice(double stockPrice, AndmePyydja.PackageInfo package, DateTime time);

        bool isDailyRate(DateTime time);
    }

}
