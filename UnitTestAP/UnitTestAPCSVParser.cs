using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using System.Diagnostics;

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
            var ap = new AndmePyydja.CAP();
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

        [DataTestMethod]
        [Timeout(1000)]
        public void Test_AP_parsePackages()
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
                    "01.03.2023 02:00; 01.03.2023 03:00; 0,084; 44,45; \n\n\n\n",

                "asdhkjafsadfsdf;\n" +
                "Pakkuja; Nimi; Kuutasu(€); Marginaal(s / kWh); Baashind(s / kWh); Ööhind(s / kWh); BörsiPakett; Roheline\n" +
                "Alexela AS; Pingevaba; 1,99; 0,7; 10,2; 0; false; false\n" +
                "Elektrum Eesti OÜ; Kaljukindel Klõps kindlustusega; 1,85; 0; 13,62; 0; false; false\n" +
                "KGB Elektrivõrgud OÜ; Universaalteenus; 0; 1,46; 18,49; 0; false; false\n" +
                "Elektrum Eesti OÜ; Börsi Klõps; 0; 0,65; 0; 0; true; false\n" +
                "AsS Eesti Gaas; Muutuv; 0; 0,684; 0; 0; true; false\n" +
                "KGB Elektrivõrgud OÜ; Not - FiNe, INFP vibes; 0; 0,75; 0; 0; true; false\nasdasd;asdasd;\n\n\n\n"
            };
            var ap = new AndmePyydja.CAP();
            var vecs = new List<AndmePyydja.CPackageInfo>[]
            {
                new List<AndmePyydja.CPackageInfo> { },
                new List<AndmePyydja.CPackageInfo> { },
                new List<AndmePyydja.CPackageInfo> { },
                new List<AndmePyydja.CPackageInfo> { },

                new List<AndmePyydja.CPackageInfo>
                {
                    new AndmePyydja.CPackageInfo("Alexela AS", "Pingevaba", 1.99, 0.7, 10.2, 0, false, false),
                    new AndmePyydja.CPackageInfo("Elektrum Eesti OÜ", "Kaljukindel Klõps kindlustusega", 1.85, 0, 13.62, 0, false, false),
                    new AndmePyydja.CPackageInfo("KGB Elektrivõrgud OÜ", "Universaalteenus", 0, 1.46, 18.49, 0, false, false),
                    new AndmePyydja.CPackageInfo("Elektrum Eesti OÜ", "Börsi Klõps", 0, 0.65, 0, 0, true, false),
                    new AndmePyydja.CPackageInfo("AsS Eesti Gaas", "Muutuv", 0, 0.684, 0, 0, true, false),
                    new AndmePyydja.CPackageInfo("KGB Elektrivõrgud OÜ", "Not - FiNe, INFP vibes", 0, 0.75, 0, 0, true, false)
                }
            };

            Assert.AreEqual(contarr.Length, vecs.Length);

            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

            for (int i = 0; i < contarr.Length; ++i)
            {
                var vec2 = ap.parsePackage(contarr[i]);
                CollectionAssert.AreEqual(vecs[i], vec2);
            }
        }

        [DataTestMethod]
        [Timeout(1000)]
        public void Test_AP_createPackageCSVs()
        {
            // make content array
            string[] contarr =
            {
                "Pakkuja; Nimi; Kuutasu (€); Marginaal (s/kWh); Baashind (s/kWh); Ööhind (s/kWh); BörsiPakett; Roheline\n",

                "Pakkuja; Nimi; Kuutasu (€); Marginaal (s/kWh); Baashind (s/kWh); Ööhind (s/kWh); BörsiPakett; Roheline\n" +
                "Alexela AS; Pingevaba; 1,99; 0,7; 10,2; 0; False; False\n" +
                "Elektrum Eesti OÜ; Kaljukindel Klõps kindlustusega; 1,85; 0; 13,62; 0; False; False\n" +
                "KGB Elektrivõrgud OÜ; Universaalteenus; 0; 1,46; 18,49; 0; False; False\n" +
                "Elektrum Eesti OÜ; Börsi Klõps; 0; 0,65; 0; 0; True; False\n" +
                "AsS Eesti Gaas; Muutuv; 0; 0,684; 0; 0; True; False\n" +
                "KGB Elektrivõrgud OÜ; Not - FiNe, INFP vibes; 0; 0,75; 0; 0; True; False\n",

                "Pakkuja; Nimi; Kuutasu (€); Marginaal (s/kWh); Baashind (s/kWh); Ööhind (s/kWh); BörsiPakett; Roheline\n" +
                "Elektrišokker; Suured käärid; 10,99; 0; 13,62; 25,428; False; False\n"
            };
            var ap = new AndmePyydja.CAP();
            var vecs = new List<AndmePyydja.IPackageInfo>[]
            {
                new List<AndmePyydja.IPackageInfo> { },

                new List<AndmePyydja.IPackageInfo>
                {
                    new AndmePyydja.CPackageInfo("Alexela AS", "Pingevaba", 1.99, 0.7, 10.2, 0, false, false),
                    new AndmePyydja.CPackageInfo("Elektrum Eesti OÜ", "Kaljukindel Klõps kindlustusega", 1.85, 0, 13.62, 0, false, false),
                    new AndmePyydja.CPackageInfo("KGB Elektrivõrgud OÜ", "Universaalteenus", 0, 1.46, 18.49, 0, false, false),
                    new AndmePyydja.CPackageInfo("Elektrum Eesti OÜ", "Börsi Klõps", 0, 0.65, 0, 0, true, false),
                    new AndmePyydja.CPackageInfo("AsS Eesti Gaas", "Muutuv", 0, 0.684, 0, 0, true, false),
                    new AndmePyydja.CPackageInfo("KGB Elektrivõrgud OÜ", "Not - FiNe, INFP vibes", 0, 0.75, 0, 0, true, false)
                },

                new List<AndmePyydja.IPackageInfo>
                {
                    new AndmePyydja.CPackageInfo("Elektrišokker", "Suured käärid", 10.99, 0, 13.62, 25.428, false, false),
                }
            };

            Assert.AreEqual(contarr.Length, vecs.Length);

            for (int i = 0; i < contarr.Length; ++i)
            {
                var contents = ap.createPackageCSV(vecs[i]);
                Assert.AreEqual(contarr[i], contents);
            }
        }
    }
}
