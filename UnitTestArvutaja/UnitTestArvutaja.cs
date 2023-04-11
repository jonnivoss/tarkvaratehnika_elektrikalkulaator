using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

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
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10f),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16f),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08f),
                System.Tuple.Create(new System.DateTime(2023,03,10,13,0,0), 4.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,14,0,0), 5.32f),
                System.Tuple.Create(new System.DateTime(2023,03,10,15,0,0), 8.01f)
            };
            float integraal = 0.0f;
            System.DateTime alumineRaja = andmed[0].Item1;
            System.DateTime ylemineRaja = andmed[7].Item1;
            int reaalne1 = objekt.integreerija(andmed, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 0;
            float reaalne2 = integraal;
            float oodatud2 = 38.03f;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void V6rdsedRajad()
        {
            VecT andmed = new VecT
            {
                System.Tuple.Create(System.DateTime.Now, 8.01f),
                System.Tuple.Create(System.DateTime.Now, 6.51f)
            };
            float integraal = 0.0f;
            System.DateTime alumineRaja = andmed[0].Item1;
            System.DateTime ylemineRaja = andmed[0].Item1;
            int reaalne1 = objekt.integreerija(andmed, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 2;
            float reaalne2 = integraal;
            float oodatud2 = 0.0f;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void TyhjadAndmed()
        {
            VecT andmed = new VecT
            {
            };
            float integraal = 0.0f;
            System.DateTime alumineRaja = new System.DateTime(2022, 03, 10, 8, 15, 0);
            System.DateTime ylemineRaja = new System.DateTime(2022, 03, 10, 9, 15, 0);
            int reaalne1 = objekt.integreerija(andmed, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 1;
            float reaalne2 = integraal;
            float oodatud2 = 0.0f;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void AlumineRajaV2ljaspoolPiire()
        {
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10f),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16f),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08f),
            };
            float integraal = 0.0f;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 7, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 11, 0, 0);
            int reaalne1 = objekt.integreerija(andmed, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 1;
            float reaalne2 = integraal;
            float oodatud2 = 0.0f;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void YlemineRajaV2ljaspoolPiire()
        {
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10f),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16f),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08f),
            };
            float integraal = 0.0f;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 13, 0, 0);
            int reaalne1 = objekt.integreerija(andmed, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 1;
            float reaalne2 = integraal;
            float oodatud2 = 0.0f;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }

        [TestMethod]
        public void RajadeSuurusVahetuses()
        {
            VecT andmed = new VecT
            {
                System.Tuple.Create(new System.DateTime(2023,03,10,8,0,0), 4.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,9,0,0), 5.10f),
                System.Tuple.Create(new System.DateTime(2023,03,10,10,0,0), 3.16f),
                System.Tuple.Create(new System.DateTime(2023,03,10,11,0,0), 6.12f),
                System.Tuple.Create(new System.DateTime(2023,03,10,12,0,0), 2.08f),
            };
            float integraal = 0.0f;
            System.DateTime alumineRaja = new System.DateTime(2023, 03, 10, 12, 0, 0);
            System.DateTime ylemineRaja = new System.DateTime(2023, 03, 10, 8, 0, 0);
            int reaalne1 = objekt.integreerija(andmed, alumineRaja, ylemineRaja, ref integraal);
            int oodatud1 = 3;
            float reaalne2 = integraal;
            float oodatud2 = 0.0f;
            Assert.AreEqual(oodatud1, reaalne1);
            Assert.AreEqual(oodatud2, reaalne2);
        }
    }
}
