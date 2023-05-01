using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace Arvutaja
{
    /*
     * KLASS ARVUTAJA (Class Arvutaja -> CArvutaja)
     * Tegu on klassiga, mis defineerib projekti "Tarkvaratehnika-elektrikalkulaator"
     * jaoks põhilised matemaatilised meetodid nagu integreerija (integrator) ja 
     * diferentseerija (differentiator).
     */
    public class CArvutaja : Arvutaja.IArvutaja
    {
        // ### ATRIBUUDID ###
        protected System.DateTime alumineRaja;
        protected System.DateTime ylemineRaja;
        protected System.DateTime samm;

        // #### MEETODID ####

        // KAHE FUNKTSIOONI KORRUTISE INTEGRAATOR
        // andmed1: esimene funktsioon
        // andmed2: teine funktsioon
        public int integral(VecT andmed1, VecT andmed2, System.DateTime alumine, System.DateTime ylemine, out double integraal)
        {
            if (alumine > ylemine)
            {
                // VIGA! RAJAD ON SUURUSE POOLEST VAHETUSES!
                integraal = 0.0;
                return 3;
            }
            // alumisele ja ülemisele rajale vastavate indekside määramine
            int alumineIndeks = andmed1.FindIndex(Tuple => Tuple.Item1 == alumine);
            int ylemineIndeks = andmed1.FindIndex(Tuple => Tuple.Item1 == ylemine);
            int alumineIndeks2 = andmed2.FindIndex(Tuple => Tuple.Item1 == alumine);
            int ylemineIndeks2 = andmed2.FindIndex(Tuple => Tuple.Item1 == ylemine);
            if (!(alumineIndeks >= 0 && ylemineIndeks >= 0 && alumineIndeks2 >= 0 && ylemineIndeks2 >= 0))
            {
                // VIGA! VÄHEMALT ÜKS SOOVITUD INTEGREERIMISRAJADEST PUUDUB ANDMETE HULGAS!
                integraal = 0.0;
                return 1;
            }
            // INTEGREERIMINE
            // NB! Eeldatud on, et ajasamm dt = 1h
            int indeks1 = alumineIndeks, indeks2 = alumineIndeks2;
            integraal = 0.0; // NB! viidana antud muutuja, omandab pärast integraaali väärtuse
            while ((indeks1 <= ylemineIndeks) && (indeks2 <= ylemineIndeks2))
            {
                integraal += andmed1[indeks1].Item2 * andmed2[indeks2].Item2;
                indeks1++;
                indeks2++;
            }
            return 0;
        }

        public VecT generateUsageData(
             System.DateTime start,
             double usageLength,
             double power
        )
        {
            VecT usageData = new VecT();
            // Genereerimisel arvestab seda, et kui on komaga arv tunde, siis kohtleb
            // seda olukorda nagu oleks täisarv tunde, aga viimases tunnis on keskmine võimsus siis väiksem
            for (DateTime date = start, tempend = (start + TimeSpan.FromHours(usageLength)); date < tempend; date = date.AddHours(1))
            {
                // Maksimaalne samm on 1 tund
                var hrs = Math.Min((tempend - date).TotalHours, 1.0);
                //Console.WriteLine("asd: " + date.ToString() + "; " + (power * hrs).ToString());
                usageData.Add(Tuple.Create(date, power * hrs));
            }
            return usageData;
        }

        public int smallestIntegral(
            VecT priceData,
            double power,
            double usageLength,
            System.DateTime start,
            System.DateTime stop,
            out double outSmallestIntegral,
            out System.DateTime outOptimalDate
        )
        {
            if (start > stop)
            {
                outSmallestIntegral = 0.0;
                outOptimalDate = default(System.DateTime);
                return 1;
            }
            
            double bestIntegral = double.PositiveInfinity;
            DateTime bestDate = start;
            DateTime usageEnd = start + TimeSpan.FromHours(Math.Ceiling(usageLength));

            // Proovib sobitada kasutusmalli hinnaandmete graafikule ja seda nii, et
            // kumbki dataset ei läheks üksteise piiridest välja
            while (usageEnd <= stop)
            {
                VecT tempUsageData = this.generateUsageData(start, usageLength, power);

                // Integreerib
                if (tempUsageData.Count == 0)
                {
                    outSmallestIntegral = 0.0;
                    outOptimalDate = default(System.DateTime);
                    return 2;
                }

                double integral;
                if (this.integral(tempUsageData, priceData, tempUsageData.First().Item1, tempUsageData.Last().Item1, out integral) == 0)
                {
                    if (integral < bestIntegral)
                    {
                        bestIntegral = integral;
                        bestDate = start;
                    }
                }

                start    = start.AddHours(1);
                usageEnd = usageEnd.AddHours(1);
            }

            // while loop tõenäoliselt ei käivitunudki või oli integreerimisega probleem :/
            if (bestIntegral == double.PositiveInfinity)
            {
                outSmallestIntegral = 0.0;
                outOptimalDate = default(System.DateTime);
                return 2;
            }

            outSmallestIntegral = bestIntegral;
            outOptimalDate      = bestDate;

            return 0;
        }

        public int diferentseerija()
        {
            return 0;
        }

        public int average(
            VecT andmed,
            System.DateTime alumine,
            System.DateTime ylemine,
            out double avg
        )
        {
            if (alumine > ylemine)
            {
                // Rajad vahetuses
                avg = 0.0;
                return 1;
            }

            int begIdx = andmed.FindIndex(Tuple => Tuple.Item1 == alumine);
            int endIdx = andmed.FindIndex(Tuple => Tuple.Item1 == ylemine);
            if (!((begIdx >= 0) && (endIdx >= 0) && (endIdx >= begIdx)))
            {
                // Kuupäevasid andmetes ei leidunud
                avg = 0.0;
                return 2;
            }

            avg = 0.0;
            int items = 0;
            for (int idx = begIdx; idx <= endIdx; ++idx)
            {
                avg += andmed[idx].Item2;
                // hoiab for loopis liidetavate arvu meeles
                ++items;
            }
            // andmeid polnudki :(
            if (items == 0)
            {
                avg = 0.0;
                return 3;
            }

            avg /= (double)items;

            return 0;
        }

        public bool isDailyRate(DateTime time)
        {
            // Quantize time to date and hours
            var clock = time.TimeOfDay;
            time = time.Date + TimeSpan.FromHours(time.Hour);

            // https://xn--riigiphad-v9a.ee/
            DateTime[] riigiPyhad =
            {
                new DateTime(time.Year,  4,  7),  // suur reede 2023
                new DateTime(time.Year,  4,  9),  // ülestõusmispühade 1. püha 2023
                new DateTime(time.Year,  5, 28), // nelipühade 1. püha 2023
                new DateTime(time.Year,  1,  1),
                new DateTime(time.Year,  2, 24),
                new DateTime(time.Year,  5,  1),
                new DateTime(time.Year,  6, 23),
                new DateTime(time.Year,  6, 24),
                new DateTime(time.Year,  8, 20),
                new DateTime(time.Year, 12, 24),
                new DateTime(time.Year, 12, 25),
                new DateTime(time.Year, 12, 26)
            };

            // Add suur reede
            // Add ülestõusmispühade 1. püha
            // Add nelipühade 1. püha
            // Mitte-deterministlikud riigipühad :/
            switch (time.Year)
            {
                case 2024:
                    riigiPyhad[0] = new DateTime(2024, 3, 29);
                    riigiPyhad[1] = new DateTime(2024, 3, 31);
                    riigiPyhad[2] = new DateTime(2024, 5, 19);
                    break;
                case 2025:
                    riigiPyhad[0] = new DateTime(2025, 4, 18);
                    riigiPyhad[1] = new DateTime(2025, 4, 20);
                    riigiPyhad[2] = new DateTime(2025, 6,  8);
                    break;
                case 2026:
                    riigiPyhad[0] = new DateTime(2026, 4,  3);
                    riigiPyhad[1] = new DateTime(2026, 4,  5);
                    riigiPyhad[2] = new DateTime(2026, 5, 24);
                    break;
                case 2027:
                    riigiPyhad[0] = new DateTime(2027, 3, 26);
                    riigiPyhad[1] = new DateTime(2027, 3, 28);
                    riigiPyhad[2] = new DateTime(2027, 5, 16);
                    break;
            }


            // Riigipühadel on öötariif
            // https://raha.geenius.ee/rubriik/uudis/eesti-energia-muudab-oma-tuuptingimusi/
            if (riigiPyhad.Contains(time.Date))
            {
                return false;
            }
            else
            {
                // Kella check, 7-22 on päevatariif
                if ((clock >= new TimeSpan(7, 0, 0)) && (clock <= new TimeSpan(22, 0, 0)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public double finalPrice(double stockPrice, AndmePyydja.PackageInfo package, DateTime time)
        {
            // Käibemaks
            const double tax = 0.2;

            double price;
            if (package.isStockPackage)
            {
                price = stockPrice + package.sellerMarginal;
            }
            else if (!package.isDayNight)
            {
                price = package.basePrice + package.sellerMarginal;
            }
            else
            {
                if (isDailyRate(time))
                {
                    price = package.dayPrice + package.sellerMarginal;
                }
                else
                {
                    price = package.nightPrice + package.sellerMarginal;
                }
            }

            return price * (1.0 + tax);
        }

    }
}