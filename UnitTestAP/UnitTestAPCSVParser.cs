using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using DatePriceT = System.Tuple<System.DateTime, double>;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, double>>;

namespace UnitTestAP
{
    [TestClass]
    public class UnitTestAPCSVParser
    {
        [DataTestMethod]
        [Timeout(1000)]
        public void Test_AP_parseUserData()
        {
            // make content array
            string[] contarr =
            {
                "",
                "asdasdasdasdas;asdasd;asdasd;asdasd;asd;",
                "Algus;Lõpp;Kogus (kWh);Börsihind (EUR / MWh); \n" +
                    "01.03.2023 00:00; 01.03.2023 01:00; 0,152; 60,07; \n" +
                    "01.03.2023 01:00; 01.03.2023 02:00; 0,104; 60,40; \n" +
                    "01.03.2023 02:00; 01.03.2023 03:00; 0,084; 44,45; ",
                "asdasdma;asd,,asd,gds,fjgkjadhkjbhxkjcnknbvnd,dv,ad,vsdfjgjhdskfjghkj\n" +
                    "Algus;Lõpp;Kogus (kWh);Börsihind (EUR / MWh); \n" +
                    "01.03.2023 00:00; 01.03.2023 01:00; 0,152; 60,07; \n" +
                    "01.03.2023 01:00; 01.03.2023 02:00; 0,104; 60,40; \n" +
                    "01.03.2023 02:00; 01.03.2023 03:00; 0,084; 44,45; \n\n\n\n"
            };
            var ap = new Andmepyydja.CAP();
            var vecs = new VecT[]
            {
                new VecT { },
                new VecT { },
                new VecT
                {
                    Tuple.Create( new DateTime(2023, 3, 1, 0, 0, 0), 0.152 ),
                    Tuple.Create( new DateTime(2023, 3, 1, 1, 0, 0), 0.104 ),
                    Tuple.Create( new DateTime(2023, 3, 1, 2, 0, 0), 0.084 )
                },
                new VecT
                {
                    Tuple.Create( new DateTime(2023, 3, 1, 0, 0, 0), 0.152 ),
                    Tuple.Create( new DateTime(2023, 3, 1, 1, 0, 0), 0.104 ),
                    Tuple.Create( new DateTime(2023, 3, 1, 2, 0, 0), 0.084 )
                }
            };

            Assert.AreEqual(contarr.Length, vecs.Length);

            for (int i = 0; i < contarr.Length; ++i)
            {
                var vec2 = ap.parseUserData(contarr[i]);
                CollectionAssert.AreEqual(vecs[i], vec2);
            }
        }
    }
}
