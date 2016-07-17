using System;
using JetBrains.Annotations;

namespace Pharmatechnik.Nav.Language {
    [Serializable]
    [SampleSyntax("init Identifier [abstractmethod] [params T1 param1, T2<T3, T4<T5>> param2, T6[][] param3] do Instruction;")]
    public partial class InitNodeDeclarationSyntax : ConnectionPointNodeSyntax {

        readonly CodeAbstractMethodDeclarationSyntax _codeAbstractMethodDeclaration;
        readonly CodeParamsDeclarationSyntax         _codeParamsDeclaration;
        readonly DoClauseSyntax                      _doClause;

        internal InitNodeDeclarationSyntax(TextExtent extent,
                CodeAbstractMethodDeclarationSyntax codeAbstractMethodDeclaration, 
                CodeParamsDeclarationSyntax codeParamsDeclaration,
                DoClauseSyntax doClause)
                : base(extent) {

            AddChildNode(_codeAbstractMethodDeclaration = codeAbstractMethodDeclaration);
            AddChildNode(_codeParamsDeclaration         = codeParamsDeclaration);
            AddChildNode(_doClause                      = doClause);
        }

        public SyntaxToken InitKeyword {
            get { return ChildTokens().FirstOrMissing(SyntaxTokenType.InitKeyword); }
        }

        public SyntaxToken Identifier {
            get { return ChildTokens().FirstOrMissing(SyntaxTokenType.Identifier); }
        }

        [CanBeNull]
        public CodeParamsDeclarationSyntax CodeParamsDeclaration {
            get { return _codeParamsDeclaration; }
        }

        [CanBeNull]
        public CodeAbstractMethodDeclarationSyntax CodeAbstractMethodDeclaration {
            get { return _codeAbstractMethodDeclaration; }
        }

        [CanBeNull]
        public DoClauseSyntax DoClause {
            get { return _doClause; }
        }
    }
}