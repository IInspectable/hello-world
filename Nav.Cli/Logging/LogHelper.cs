#region Using Directives

using JetBrains.Annotations;
using Pharmatechnik.Nav.Language.Generator;

#endregion

namespace Pharmatechnik.Nav.Language.Logging {

    static class LogHelper {

        public static string GetFileIdentity(Diagnostic diag, [CanBeNull] FileSpec fileSpec) {
            if (diag?.Location.FilePath?.ToLower() == fileSpec?.FilePath.ToLower()) {
                return fileSpec?.Identity ?? diag?.Location.FilePath;
            }
            return diag?.Location.FilePath;
        }
    }
}