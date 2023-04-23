using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestArvutaja
{
    [TestClass]
    public class UnitTestArvutajaFinalPrice
    {
        Arvutaja.CArvutaja objekt = new Arvutaja.CArvutaja();

        public bool IsClose(double a, double b, double precision)
        {
            double diff = Math.Abs(a * precision);
            return Math.Abs(a - b) <= diff;
        }

        [TestMethod]
        public void StockPrice()
        {
            double[] stockPrices =
            {
                5.5,
                5.5,
                6.4,
                123.2
            };
            Andmepyydja.PackageInfo[] packageData =
            {
                new Andmepyydja.PackageInfo("", "", 0.0, 4.3, 0,      0,   true, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 4.3, 234234, 0,   true, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 4.3, 0,      343, true, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 4.3, 123,    343, true, false),
            };
            DateTime[] timeData =
            {
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 15),
                new DateTime(2023, 3, 2),
                new DateTime(2023, 2, 24)
            };

            double[] expectedPrices =
            {
                11.76,
                11.76,
                12.84,
                153.0
            };

            Assert.AreEqual(expectedPrices.Length, stockPrices.Length);
            Assert.AreEqual(expectedPrices.Length, packageData.Length);
            Assert.AreEqual(expectedPrices.Length, timeData.Length);

            for (int i = 0; i < stockPrices.Length; ++i)
            {
                double actualPrice = objekt.finalPrice(stockPrices[i], packageData[i], timeData[i]);
                Assert.AreEqual(true, IsClose(expectedPrices[i], actualPrice, 0.00001));
            }
        }

        [TestMethod]
        public void Riigipyhad()
        {
            double[] stockPrices =
            {
                5.5,
                0.0,
                0.0,
                0.0
            };
            Andmepyydja.PackageInfo[] packageData =
            {
                new Andmepyydja.PackageInfo("", "", 0.0, 0.0, 4,     0.4, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 0.0, 2.344, 0.4, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 4.3, 4.2,   0.0, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 1.1, 123,   0.001, false, false),
            };
            DateTime[] timeData =
            {
                new DateTime(2023, 12, 24),
                new DateTime(2023, 12, 25),
                new DateTime(2024,  3, 31),
                new DateTime(2023,  2, 24)
            };

            double[] expectedPrices =
            {
                0.48,
                0.48,
                10.2,
                1.3212
            };

            Assert.AreEqual(expectedPrices.Length, stockPrices.Length);
            Assert.AreEqual(expectedPrices.Length, packageData.Length);
            Assert.AreEqual(expectedPrices.Length, timeData.Length);

            for (int i = 0; i < stockPrices.Length; ++i)
            {
                double actualPrice = objekt.finalPrice(stockPrices[i], packageData[i], timeData[i]);
                Assert.AreEqual(true, IsClose(expectedPrices[i], actualPrice, 0.00001));
            }
        }

        [TestMethod]
        public void P2evaTariif()
        {
            double[] stockPrices =
            {
                5.5,
                0.0,
                0.0,
                0.0
            };
            Andmepyydja.PackageInfo[] packageData =
            {
                new Andmepyydja.PackageInfo("", "", 0.0, 1.1, 4,     0.4, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 2.2, 2.344, 0.4, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 4.3, 4.2,   0.1, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 1.1, 123,   0.001, false, false),
            };
            DateTime[] timeData =
            {
                new DateTime(2023, 11, 24, 15, 43,  0),
                new DateTime(2023, 11, 25, 12,  0,  0),
                new DateTime(2023,  3, 31, 22, 59, 59),
                new DateTime(2023,  1, 24,  6, 59, 59)
            };

            double[] expectedPrices =
            {
                6.12,
                5.4528,
                10.2,
                1.3212
            };

            Assert.AreEqual(expectedPrices.Length, stockPrices.Length);
            Assert.AreEqual(expectedPrices.Length, packageData.Length);
            Assert.AreEqual(expectedPrices.Length, timeData.Length);

            for (int i = 0; i < stockPrices.Length; ++i)
            {
                double actualPrice = objekt.finalPrice(stockPrices[i], packageData[i], timeData[i]);
                Assert.AreEqual(true, IsClose(expectedPrices[i], actualPrice, 0.00001));
            }
        }

        [TestMethod]
        public void OoTariif()
        {
            double[] stockPrices =
            {
                5.5,
                0.0,
                0.0,
                0.0
            };
            Andmepyydja.PackageInfo[] packageData =
            {
                new Andmepyydja.PackageInfo("", "", 0.0, 0.0, 4,     0.4, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 0.0, 2.344, 5.0, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 4.3, 4.2,   4.0, false, false),
                new Andmepyydja.PackageInfo("", "", 0.0, 1.1, 123,   0.0, false, false),
            };
            DateTime[] timeData =
            {
                new DateTime(2023, 12, 24,  5, 43,  0),
                new DateTime(2023, 12, 25, 23,  0,  0),
                new DateTime(2024,  3, 31, 22, 59, 59),
                new DateTime(2023,  2, 24,  6, 59, 59)
            };

            double[] expectedPrices =
            {
                0.48,
                6.0,
                10.2,
                148.92
            };

            Assert.AreEqual(expectedPrices.Length, stockPrices.Length);
            Assert.AreEqual(expectedPrices.Length, packageData.Length);
            Assert.AreEqual(expectedPrices.Length, timeData.Length);

            for (int i = 0; i < stockPrices.Length; ++i)
            {
                double actualPrice = objekt.finalPrice(stockPrices[i], packageData[i], timeData[i]);
                Assert.AreEqual(true, IsClose(expectedPrices[i], actualPrice, 0.00001));
            }
        }
    }
}
