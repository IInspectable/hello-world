﻿#region Using Directives

using NUnit.Framework;
using Pharmatechnik.Nav.Language;

#endregion

namespace Nav.Language.Tests {


    [TestFixture]
    public class SyntaxNodeTests {

        // TODO Weitere Tests für Trivias
        [Test]
        public void GetLeadingTriviaExtentTests() {

            string source = 
@"//Foo
    task A;
    // Comment
    task B; //Comment
task C;
";
            var ndb= Syntax.ParseNodeDeclarationBlock(source);

            var taskA=ndb.NodeDeclarations[0];

            Assert.That(taskA.GetLeadingTriviaExtent(), Is.EqualTo(new TextExtent(0, length: 11)));
            Assert.That(taskA.GetTrailingTriviaExtent(), Is.EqualTo(new TextExtent(18, length: 2))); // NewLine!!

            var taskB = ndb.NodeDeclarations[1];

            Assert.That(taskB.GetLeadingTriviaExtent(), Is.EqualTo(new TextExtent(20, length: 4+2+14)));
            Assert.That(taskB.GetTrailingTriviaExtent(), Is.EqualTo(new TextExtent(47, length: 12))); // NewLine!!

            var taskC = ndb.NodeDeclarations[2];

            Assert.That(taskC.GetLeadingTriviaExtent(),  Is.EqualTo(new TextExtent(59, length:0)));
            Assert.That(taskC.GetTrailingTriviaExtent(), Is.EqualTo(new TextExtent(66, length: 2))); // NewLine!!
        }

        // TODO Weitere Tests für Trivias
        [Test]
        public void GetLeadingTriviaExtentTests2() {

            string source =@" task A;    /* Comment*/    task B; /*Comment*/task C;
";
            var ndb = Syntax.ParseNodeDeclarationBlock(source);
            //
            var taskA = ndb.NodeDeclarations[0];
            
            Assert.That(taskA.GetLeadingTriviaExtent(), Is.EqualTo(new TextExtent(0, length: 1)));
            Assert.That(taskA.GetTrailingTriviaExtent(), Is.EqualTo(new TextExtent(8, length: 20))); 
            //
            var taskB = ndb.NodeDeclarations[1];
            //
            Assert.That(taskB.GetLeadingTriviaExtent(), Is.EqualTo(new TextExtent(28, length: 0)));
            Assert.That(taskB.GetTrailingTriviaExtent(), Is.EqualTo(new TextExtent(35, length: 12))); 
            //
            var taskC = ndb.NodeDeclarations[2];
            //
            //Assert.That(taskC.GetLeadingTriviaExtent(), Is.EqualTo(new TextExtent(59, length: 0)));
            //Assert.That(taskC.GetTrailingTriviaExtent(), Is.EqualTo(new TextExtent(66, length: 2))); // NewLine!!
        }
    }
}