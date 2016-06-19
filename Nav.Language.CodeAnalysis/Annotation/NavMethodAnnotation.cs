#region Using Directives

using System;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace Pharmatechnik.Nav.Language.CodeAnalysis.Annotation {

    public abstract class NavMethodAnnotation: NavTaskAnnotation {

        protected NavMethodAnnotation(NavTaskAnnotation taskAnnotation,
                                      MethodDeclarationSyntax methodDeclarationSyntax) : base(taskAnnotation) {

            if (methodDeclarationSyntax == null) {
                throw new ArgumentNullException(nameof(methodDeclarationSyntax));
            }
            MethodDeclarationSyntax = methodDeclarationSyntax;
        }

        [NotNull]
        public MethodDeclarationSyntax MethodDeclarationSyntax { get; }
    }
}