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

        public int lokaalsedAndmed(VecT andmed)
        {

            return 0;
        }
        public int internetiAndmed(VecT andmed)
        {

            return 0;
        }

        // KAHE FUNKTSIOONI KORRUTISE INTEGRAATOR
        // andmed1: esimene funktsioon
        // andmed2: teine funktsioon
        /*public int integreerija(VecT andmed1, VecT andmed2, System.DateTime alumine, System.DateTime ylemine, ref double integraal)
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
    }
}