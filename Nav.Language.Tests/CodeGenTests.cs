﻿#region Using Directives

using System.Linq;

using Pharmatechnik.Nav.Language;
using Pharmatechnik.Nav.Language.CodeGen;
using Pharmatechnik.Nav.Language.CodeGen.Templates;

using NUnit.Framework;

#endregion

namespace Nav.Language.Tests {
    [TestFixture]
    public class CodeGenTests {

        [Test]
        public void TestResources() {
            var n = Resources.IBeginWfsTemplate;
            Assert.That(n, Is.Not.Empty);
        }

        [Test]
        public void SimpleCodegenTest() {

            string navCode = @"[namespaceprefix NS]

[using Pharmatechnik.Apotheke.XTplus.Framework.Core.WFL]
[using Pharmatechnik.Apotheke.XTplus.Framework.Core.IWFL] 


task TaskA [base StandardWFS : ILegacyMessageBoxWFS]
                  [result MessageBoxResult]
{
    init I1 [params string message];
    init I2 [params string message, MessageBoxImage messageBoxImage];
        
    view MessageBoxOK;
    
    exit Ok;

    I1 --> MessageBoxOK;  
    I2 --> MessageBoxOK; 
    
    MessageBoxOK --> Ok on Ok;
    MessageBoxOK --> Ok on OnFoo;
}";
            var codeGenerationUnitSyntax= Syntax.ParseCodeGenerationUnit(navCode, @"c:\TaskA.nav");
            var codeGenerationUnit = CodeGenerationUnit.FromCodeGenerationUnitSyntax(codeGenerationUnitSyntax);
            var generator = new CodeGenerator(CodeGenerationOptions.Default);

            var results = generator.Generate(codeGenerationUnit).ToList();

            Assert.That(results.Count, Is.EqualTo(1));

            Assert.That(results[0].IBeginWfsCode, Is.Not.Empty);
            Assert.That(results[0].IWfsCode     , Is.Not.Empty);
            Assert.That(results[0].WfsBaseCode  , Is.Not.Empty);
            Assert.That(results[0].WfsCode      , Is.Not.Empty);
        }
    }
}
