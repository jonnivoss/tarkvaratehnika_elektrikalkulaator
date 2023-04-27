using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace UnitTestArvutaja
{
    [TestClass]
    public class UnitTestArvutajaSmallestIntegral
    {
        Arvutaja.CArvutaja objekt = new Arvutaja.CArvutaja();

        public bool IsClose(double a, double b, double precision)
        {
            double diff = Math.Abs(a * precision);
            return Math.Abs(a - b) <= diff;
        }

        [TestMethod]
        public void TyypilisedAndmed()
        {
            var d = System.DateTime.Now;
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
                System.Tuple.Create(new System.DateTime(2023,03,10,13,0,0), 1.08),
                System.Tuple.Create(new System.DateTime(2023,03,10,14,0,0), 0.08),
                System.Tuple.Create(new System.DateTime(2023,03,10,15,0,0), 1.08),
                System.Tuple.Create(new System.DateTime(2023,03,10,16,0,0), 30.08),
            };
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 9, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 17, 0, 0);

            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;

            int reaalne1 = objekt.smallestIntegral(andmed, 1.0, 4.1, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 0;
            double reaalne2 = integraal;
            double oodatud2 = 7.328;
            DateTime reaalne3 = optimalDate;
            DateTime oodatud3 = new System.DateTime(2023, 03, 10, 12, 0, 0);

            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(true, IsClose(oodatud2, reaalne2, 0.00001));
            Assert.AreEqual(oodatud3, reaalne3);
        }

        [TestMethod]
        public void YksTund()
        {
            var d = System.DateTime.Now;
            VecT andmed = new VecT
            {
                System.Tuple.Create(d, 8.01),
                System.Tuple.Create(d.AddHours(1), 6.51)
            };
            System.DateTime alumineRaja = d;
            System.DateTime ylemineRaja = d.AddHours(2);

            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;

            int reaalne1 = objekt.smallestIntegral(andmed, 1.0, 1.1, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 0;
            double reaalne2 = integraal;
            double oodatud2 = 8.661;
            DateTime reaalne3 = optimalDate;
            DateTime oodatud3 = d;

            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(true, IsClose(oodatud2, reaalne2, 0.00001));
            Assert.AreEqual(oodatud3, reaalne3);
        }

        [TestMethod]
        public void V6rdsedRajad()
        {
            var d = System.DateTime.Now;
            VecT andmed = new VecT
            {
                System.Tuple.Create(d, 8.01),
                System.Tuple.Create(d, 6.51)
            };
            System.DateTime alumineRaja = d;
            System.DateTime ylemineRaja = d;

            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;

            int reaalne1 = objekt.smallestIntegral(andmed, 1.0, 1.0, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 2;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void TyhjadAndmed()
        {
            VecT andmed = new VecT
            {
            };
            System.DateTime alumineRaja = new System.DateTime(2022, 03, 10, 8, 15, 0);
            System.DateTime ylemineRaja = new System.DateTime(2022, 03, 10, 9, 15, 0);
            
            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;
            
            int reaalne1 = objekt.smallestIntegral(andmed, 0.0, 0.0, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 2;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void AlumineRajaV2ljaspoolPiire()
        {
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 7, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 11, 0, 0);

            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;

            int reaalne1 = objekt.smallestIntegral(andmed, 0.0, 0.0, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 2;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void YlemineRajaV2ljaspoolPiire()
        {
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 13, 0, 0);

            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;

            int reaalne1 = objekt.smallestIntegral(andmed, 0.0, 0.0, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 2;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void RajadVahetuses()
        {
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08),
            };
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 12, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);

            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;

            int reaalne1 = objekt.smallestIntegral(andmed, 0.0, 0.0, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 1;
            double reaalne2 = integraal;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void TarbiminePikemKuiRajad()
        {
            var d = System.DateTime.Now;
            VecT andmed = new VecT
            {
                System.Tuple.Create(d, 8.01),
                System.Tuple.Create(d.AddHours(1), 6.51)
            };
            System.DateTime alumineRaja = d;
            System.DateTime ylemineRaja = d.AddHours(2);

            double integraal = 0.0;
            System.DateTime optimalDate = alumineRaja;

            int reaalne1 = objekt.smallestIntegral(andmed, 1.0, 2.1, alumineRaja, ylemineRaja, ref integraal, ref optimalDate);
            int oodatud1 = 2;
            double reaalne2 = integraal;
            double oodatud2 = 0;

            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }
    }
}
