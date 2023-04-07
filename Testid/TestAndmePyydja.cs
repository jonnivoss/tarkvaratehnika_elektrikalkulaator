using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using VecT = System.Collections.Generic.List<System.Tuple<System.DateTime, float>>;

namespace Testid
{
    [TestClass]
    public class TestAndmePyydja
    {
        [DataTestMethod]
        [Timeout(1000)]
        public void Test_AP_parseContents()
        {
            // make content array
            string[] contarr =
            {
                ""
            };
            var ap = new Andmepyydja.CAP();
            VecT[] vecs =
            {
                new VecT { }
            };

            Assert.AreEqual(contarr.Length, vecs.Length);

            for (int i = 0; i < contarr.Length; ++i)
            {
                var vec2 = ap.parseContents(contarr[i]);
                CollectionAssert.AreEqual(vecs[i], vec2);
            }
        }
    }
}
