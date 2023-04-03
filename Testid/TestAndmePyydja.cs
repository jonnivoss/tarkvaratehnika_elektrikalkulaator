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
        [DataRow("asd;asd;asd;\n01.01.1111 11:11;30.05.2020 10:00;123\n", new PairT[] () )]
        public void Test_AP_parseContents(string contents, PairT[] vec)
        {
            var ap = new Andmepyydja.CAP();

            PairT[] vec2 = ap.parseContents(contents).ToArray();
            CollectionAssert.AreEqual(vec, vec2);
        }
    }
}
