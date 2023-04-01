using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

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
        public int integreerija(VecT andmed, System.DateTime alumine, System.DateTime ylemine, ref float integraal)
        {
            // alumisele ja ülemisele rajale vastavate indekside määramine
            int alumineIndeks = andmed.FindIndex(Tuple => Tuple.Item1 == alumine);
            int ylemineIndeks = andmed.FindIndex(Tuple => Tuple.Item1 == ylemine);
            if (alumineIndeks >= 0 && ylemineIndeks >= 0)
            {
                alumineRaja = andmed[alumineIndeks].Item1;
                ylemineRaja = andmed[ylemineIndeks].Item1;
            }
            else
            {
                // VIGA! VÄHEMALT ÜKS SOOVITUD INTEGREERIMISRAJADEST PUUDUB ANDMETE HULGAS!
                return 1;
            }
            // INTEGREERIMINE
            // NB! Eeldatud on, et ajasamm dt = 1h
            int indeks = alumineIndeks;
            System.DateTime raja = andmed[indeks].Item1;
            integraal = 0.0f; // NB! viidana antud muutuja, omandab pärast integraaali väärtuse
            if (alumine == ylemine)
            {
                // rajad on võrdsed, integraal on null
                return 2;
            }
            while (indeks <= ylemineIndeks)
            {
                integraal += andmed[indeks].Item2;
                indeks++;
            }
            return 0;
        }

        public int diferentseerija()
        {
            return 0;
        }
    }
}