using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace UnitTestArvutaja
{
    [TestClass]
    public class UnitTestArvutaja
    {
        // klassi Arvutaja põhjal uue objekti loomine (new instance of the class Arvutaja)
        Arvutaja.CArvutaja objekt = new Arvutaja.CArvutaja();

        // ANDMETE LUGEMISE TESTID

        // INTEGREERIMISE TESTID
        [TestMethod]
        public void TyypilisedAndmed()
        {
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
                System.Tuple.Create(new System.DateTime(2023,03,10,13,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,14,0,0), 5.32),
                System.Tuple.Create(new System.DateTime(2023,03,10,15,0,0), 8.01)
            };
            VecT andmed2 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
                System.Tuple.Create(new System.DateTime(2023,03,10,13,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,14,0,0), 5.32),
                System.Tuple.Create(new System.DateTime(2023,03,10,15,0,0), 8.01)
            };
            double integraal = 0.0;
            System.DateTime alumineRaja = andmed1[0].Item1;
            System.DateTime ylemineRaja = andmed1[7].Item1;
            int reaalne1 = objekt.integreerija(andmed1, andmed2, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 0;
            double reaalne2 = integraal;
            double oodatud2 = 204.1877;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void V6rdsedRajad()
        {
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(System.DateTime.Now, 8.01),
                System.Tuple.Create(System.DateTime.Now, 6.51)
            };
            VecT andmed2 = new VecT
            {
                System.Tuple.Create(System.DateTime.Now, 8.01),
                System.Tuple.Create(System.DateTime.Now, 6.51)
            };
            double integraal = 5.0;
            System.DateTime alumineRaja = andmed1[0].Item1;
            System.DateTime ylemineRaja = andmed1[0].Item1;
            int reaalne1 = objekt.integreerija(andmed1, andmed2, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 0;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void TyhjadAndmed()
        {
            VecT andmed1 = new VecT
            {
            };
            VecT andmed2 = new VecT
            {
            };
            double integraal = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2022, 03, 10, 8, 15, 0);
            System.DateTime ylemineRaja = new System.DateTime(2022, 03, 10, 9, 15, 0);
            int reaalne1 = objekt.integreerija(andmed1, andmed2, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 1;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void AlumineRajaV2ljaspoolPiire()
        {
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            VecT andmed2 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            double integraal = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 7, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 11, 0, 0);
            int reaalne1 = objekt.integreerija(andmed1, andmed2, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 1;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void YlemineRajaV2ljaspoolPiire()
        {
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            VecT andmed2 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            double integraal = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 13, 0, 0);
            int reaalne1 = objekt.integreerija(andmed1, andmed2, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 1;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void RajadeSuurusVahetuses()
        {
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            VecT andmed2 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            double integraal = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 12, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);
            int reaalne1 = objekt.integreerija(andmed1, andmed2, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 3;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }
    }
}
