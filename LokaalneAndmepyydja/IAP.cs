using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime;

using DatePriceT = System.Tuple<System.DateTime, float>;
using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

using PackageT = System.Collections.Generic.List<AndmePyydja.IPackageInfo>;

namespace AndmePyydja
{
    public interface IPackageInfo
    {
        string providerName { get; set; }
        string packageName { get; set; }
        double monthlyPrice { get; set; }
        double sellerMargins { get; set; }
        double basePrice { get; set; }
        double dayPrice { get; set; }
        double nightPrice { get; set; }
        bool isDayNight { get; set; }
        bool isStockPackage { get; set; }
        bool isGreenPackage { get; set; }

        string ToString();
        bool Equals(object obj);
    }

    public interface IAP
    {
        bool chooseFileUserData();
        bool chooseFilePackages();

        bool readUserDataFile(ref string contents);
        bool readPackageFile(ref string contents);
        bool writePackageFile(string contents);


        string userDataFileName { get; set; }
        VecT parseUserData(string contents);


        string packageFileName { get; set; }
        PackageT parsePackage(string contents);

        string createPackageCSV(PackageT pack);

        //bett

        DateTime UnixToDateTime(string a);

        VecT HindAegInternet(DateTime algus, DateTime lopp);
    }
}
