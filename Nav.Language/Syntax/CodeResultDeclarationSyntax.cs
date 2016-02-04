﻿using System;
using JetBrains.Annotations;

namespace Pharmatechnik.Nav.Language {

    [Serializable]
    [SampleSyntax("[result Type p]")]
    public partial class CodeResultDeclarationSyntax : CodeSyntax {
        readonly ParameterSyntax _result;

        internal CodeResultDeclarationSyntax(TextExtent extent, ParameterSyntax result) : base(extent) {
            AddChildNode(_result = result);
        }

        public SyntaxToken ResultKeyword {
            get { return ChildTokens().FirstOrMissing(SyntaxTokenType.ResultKeyword); }
        }

        [CanBeNull]
        public ParameterSyntax Result {
            get { return _result; }
        }   
    }
}