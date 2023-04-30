﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace UnitTestArvutaja
{
    [TestClass]
    public class UnitTestArvutajaIsDailyRate
    {
        Arvutaja.IArvutaja objekt = new Arvutaja.CArvutaja();

        [TestMethod]
        public void TyypilisedAndmed()
        {
            DateTime[] dates =
            {
                new DateTime(2023, 2, 24, 0, 0, 0),
                new DateTime(2023, 2, 24, 15, 0, 0),
                new DateTime(2023, 2, 24, 15, 23, 55),
            };
            bool[] expectedResults =
            {
                false,
                false,
                false
            };

            List<bool> realResults = new List<bool>();

            Assert.AreEqual(dates.Length, expectedResults.Length);

            for (int i = 0; i < dates.Length; ++i)
            {
                realResults.Add(objekt.isDailyRate(dates[i]));
            }

            CollectionAssert.AreEqual(expectedResults, realResults.ToArray());
        }
    }
}
