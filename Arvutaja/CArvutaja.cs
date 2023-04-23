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
        /*public int integreerija(VecT andmed1, VecT andmed2, System.DateTime alumine, System.DateTime ylemine, ref double integraal)
        {
            if (alumine > ylemine)
            {
                // VIGA! RAJAD ON SUURUSE POOLEST VAHETUSES!
                return 2;
            }
            // alumisele ja ülemisele rajale vastavate indekside määramine
            int alumineIndeks = andmed1.FindIndex(Tuple => Tuple.Item1 == alumine);
            int ylemineIndeks = andmed1.FindIndex(Tuple => Tuple.Item1 == ylemine);
            int alumineIndeks2 = andmed2.FindIndex(Tuple => Tuple.Item1 == alumine);
            int ylemineIndeks2 = andmed2.FindIndex(Tuple => Tuple.Item1 == ylemine);
            if (alumineIndeks >= 0 && ylemineIndeks >= 0 && alumineIndeks2 >= 0 && ylemineIndeks2 >= 0)
            {
                alumineRaja = andmed1[alumineIndeks].Item1;
                ylemineRaja = andmed1[ylemineIndeks].Item1;
            }
            else
            {
                // VIGA! VÄHEMALT ÜKS SOOVITUD INTEGREERIMISRAJADEST PUUDUB ANDMETE HULGAS!
                return 1;
            }
            // INTEGREERIMINE
            // NB! Eeldatud on, et ajasamm dt = 1h
            int indeks = alumineIndeks;
            System.DateTime raja = andmed1[indeks].Item1;
            integraal = 0.0; // NB! viidana antud muutuja, omandab pärast integraaali väärtuse
            if (alumine == ylemine)
            {
                // rajad on võrdsed, integraal on null
                integraal = 0.0;
                return 0;
            }
            while (indeks <= ylemineIndeks)
            {
                integraal += andmed1[indeks].Item2 * andmed2[indeks].Item2;
                indeks++;
            }
            return 0;
        }*/
        public int integreerija(VecT andmed1, VecT andmed2, System.DateTime alumine, System.DateTime ylemine, ref double integraal)
        {
            if (alumine > ylemine)
            {
                // VIGA! RAJAD ON SUURUSE POOLEST VAHETUSES!
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

        public int diferentseerija()
        {
            return 0;
        }

        public int average(
            VecT andmed,
            System.DateTime alumine,
            System.DateTime ylemine,
            ref double avg
        )
        {
            if (alumine > ylemine)
            {
                // Rajad vahetuses
                return 1;
            }

            int begIdx = andmed.FindIndex(Tuple => Tuple.Item1 == alumine);
            int endIdx = andmed.FindIndex(Tuple => Tuple.Item1 == ylemine);
            if (!((begIdx >= 0) && (endIdx >= 0) && (endIdx >= begIdx)))
            {
                // Kuupäevasid andmetes ei leidunud
                return 2;
            }

            avg = 0.0;
            int items = 0;
            for (int idx = begIdx; idx <= endIdx; ++idx)
            {
                avg += andmed[idx].Item2;
                ++items;
            }
            if (items == 0)
            {
                return 3;
            }

            avg /= (double)items;

            return 0;
        }

        private bool isDailyRate(DateTime time)
        {
            // riigipühad.ee
            DateTime[] riigiPyhad =
            {
                new DateTime(0, 4, 7),  // suur reede 2023
                new DateTime(0, 4, 9),  // ülestõusmispühade 1. püha 2023
                new DateTime(0, 5, 28), // nelipühade 1. püha 2023
                new DateTime(0, 1, 1),
                new DateTime(0, 2, 24),
                new DateTime(0, 5, 1),
                new DateTime(0, 6, 23),
                new DateTime(0, 6, 24),
                new DateTime(0, 8, 20),
                new DateTime(0, 12, 24),
                new DateTime(0, 12, 25),
                new DateTime(0, 12, 26)
            };

            // Add suur reede
            // Add ülestõusmispühade 1. püha
            // Add nelipühade 1. püha
            // Mitte-deterministlikud riigipühad :/
            switch (time.Year)
            {
                case 2024:
                    riigiPyhad[0] = new DateTime(0, 3, 29);
                    riigiPyhad[1] = new DateTime(0, 3, 31);
                    riigiPyhad[2] = new DateTime(0, 5, 19);
                    break;
                case 2025:
                    riigiPyhad[0] = new DateTime(0, 4, 18);
                    riigiPyhad[1] = new DateTime(0, 4, 20);
                    riigiPyhad[2] = new DateTime(0, 6, 8);
                    break;
                case 2026:
                    riigiPyhad[0] = new DateTime(0, 4, 3);
                    riigiPyhad[1] = new DateTime(0, 4, 5);
                    riigiPyhad[2] = new DateTime(0, 5, 24);
                    break;
                case 2027:
                    riigiPyhad[0] = new DateTime(0, 3, 26);
                    riigiPyhad[1] = new DateTime(0, 3, 28);
                    riigiPyhad[2] = new DateTime(0, 5, 16);
                    break;
            }

            var clock = time.TimeOfDay;
            time = new DateTime(0, time.Month, time.Day);

            // Riigipühadel on öötariif
            if (riigiPyhad.Contains(time))
            {
                return false;
            }
            else
            {
                // Kella check, 7-22 on päevatariif
                if ((clock.Hours >= 7) && (clock.Hours <= 22))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public double finalPrice(double stockPrice, Andmepyydja.PackageInfo package, DateTime time)
        {
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

            return price * 1.20;
        }

    }
}