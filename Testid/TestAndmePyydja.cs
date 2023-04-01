using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using PairT = System.Tuple<System.DateTime, float>;

namespace Testid
{
    [TestClass]
    public class TestAndmePyydja
    {
        [DataTestMethod]
        [Timeout(1000)]
        [DataRow("asd", new PairT[] () )]
        public void Test_AP_parseContents(string contents, PairT[] vec)
        {
            var ap = new Andmepyydja.CAP();

            PairT[] vec2 = ap.parseContents(contents).ToArray();
            CollectionAssert.AreEqual(vec, vec2);
        }
    }
}
