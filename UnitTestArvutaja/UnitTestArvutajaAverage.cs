using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace UnitTestArvutaja
{
    [TestClass]
    public class UnitTestArvutajaAverage
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
            double avg = 0.0;
            System.DateTime alumineRaja = andmed1[0].Item1;
            System.DateTime ylemineRaja = andmed1[7].Item1;
            int reaalne1 = objekt.average(andmed1, alumineRaja, ylemineRaja, ref avg);
            int oodatud1 = 0;
            double reaalne2 = avg;
            double oodatud2 = 4.75375;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(true, IsClose(oodatud2, reaalne2, 0.00001));
        }


        [TestMethod]
        public void TyypilisedAndmedPiiratudRajad()
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
            double avg = 0.0;
            System.DateTime alumineRaja = andmed1[1].Item1;
            System.DateTime ylemineRaja = andmed1[5].Item1;
            int reaalne1 = objekt.average(andmed1, alumineRaja, ylemineRaja, ref avg);
            int oodatud1 = 0;
            double reaalne2 = avg;
            double oodatud2 = 4.116;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(true, IsClose(oodatud2, reaalne2, 0.00001));
        }

        [TestMethod]
        public void V6rdsedRajad()
        {
            var d = System.DateTime.Now;
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(d, 8.01),
                System.Tuple.Create(d, 6.51)
            };
            double avg = 0.0;
            System.DateTime alumineRaja = d;
            System.DateTime ylemineRaja = d;
            int reaalne1 = objekt.average(andmed1, alumineRaja, ylemineRaja, ref avg);
            int oodatud1 = 0;
            double reaalne2 = avg;
            double oodatud2 = 8.01;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(true, IsClose(oodatud2, reaalne2, 0.00001));
        }

        [TestMethod]
        public void TyhjadAndmed()
        {
            VecT andmed1 = new VecT {};
            double avg = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2022, 03, 10, 8, 15, 0);
            System.DateTime ylemineRaja = new System.DateTime(2022, 03, 10, 9, 15, 0);
            int reaalne1 = objekt.average(andmed1, alumineRaja, ylemineRaja, ref avg);
            int oodatud1 = 2;
            double reaalne2 = avg;
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
            double avg = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 7, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 11, 0, 0);
            int reaalne1 = objekt.average(andmed1, alumineRaja, ylemineRaja, ref avg);
            int oodatud1 = 2;
            double reaalne2 = avg;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void YlemineRajaV2ljasPoolPiire()
        {
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08)
            };
            double avg = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 13, 0, 0);
            int reaalne1 = objekt.average(andmed1, alumineRaja, ylemineRaja, ref avg);
            int oodatud1 = 2;
            double reaalne2 = avg;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void RajadVahetuses()
        {
            VecT andmed1 = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08)
            };
            double avg = 0.0;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 12, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);
            int reaalne1 = objekt.average(andmed1, alumineRaja, ylemineRaja, ref avg);
            int oodatud1 = 1;
            double reaalne2 = avg;
            double oodatud2 = 0.0;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }
    }
}
