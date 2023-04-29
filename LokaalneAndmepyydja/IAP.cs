﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime;

using DatePriceT = System.Tuple<System.DateTime, float>;
using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

using PackageT = System.Collections.Generic.List<AndmePyydja.PackageInfo>;

namespace AndmePyydja
{
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
