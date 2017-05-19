#region Using Directives

using System;
using System.Linq;
using System.Diagnostics;

using NUnit.Framework;
using Pharmatechnik.Nav.Language;

#endregion

namespace Nav.Language.Tests {

    [TestFixture]
    public class PerformanceTests {

        [Test]
        //[Ignore("Schl�gt auf lahmen Buildserven zu oft fehl.")]
        public void TestPerformance() {
            
            string s = Resources.LargeNav;
            SyntaxTree.ParseText(s);

            var sw = Stopwatch.StartNew();
            var syntaxTree = SyntaxTree.ParseText(s);
            var t2 = sw.Elapsed;
            var lastToken = syntaxTree.Tokens.Last();
            Assert.That(lastToken.End, Is.EqualTo(s.Length));
            Assert.That(t2.TotalMilliseconds, Is.LessThan(200));
            Console.WriteLine( t2.TotalMilliseconds);
        }
    }
}