#region Using Directives

using System;
using JetBrains.Annotations;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

using Pharmatechnik.Nav.Language.CodeFixes;
using Pharmatechnik.Nav.Language.Extension.Common;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.CodeFixes {

    class CodeFixActionsParameter {
        
        public CodeFixActionsParameter(SnapshotPoint caretPoint, ITextView textView, CodeGenerationUnitAndSnapshot codeGenerationUnitAndSnapshot) {
            TextView = textView ?? throw new ArgumentNullException(nameof(textView));
            CodeGenerationUnitAndSnapshot = codeGenerationUnitAndSnapshot ?? throw new ArgumentNullException(nameof(codeGenerationUnitAndSnapshot));

            CodeFixContext = new CodeFixContext(
                position          : caretPoint,
                codeGenerationUnit: CodeGenerationUnitAndSnapshot.CodeGenerationUnit,
                editorSettings    : TextView.GetEditorSettings());
        }
        
        [NotNull]
        public CodeGenerationUnitAndSnapshot CodeGenerationUnitAndSnapshot { get; }
        
        [NotNull]
        public ITextView TextView { get; }

        [NotNull]
        public ITextBuffer TextBuffer => CodeGenerationUnitAndSnapshot.Snapshot.TextBuffer;

        [NotNull]
        public CodeFixContext CodeFixContext { get; }
    }
}