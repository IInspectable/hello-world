#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    public sealed class CodeModelResult {

        public CodeModelResult(
            ITaskDefinitionSymbol taskDefinition, 
            IPathProvider pathProvider, 
            IBeginWfsCodeModel beginWfsCodeModel,
            IWfsCodeModel wfsCodeModel, 
            WfsBaseCodeModel wfsBaseCodeModel,
            [CanBeNull]IEnumerable<TOCodeModel> toCodeModels) {

            TaskDefinition     = taskDefinition    ?? throw new ArgumentNullException(nameof(taskDefinition));
            PathProvider       = pathProvider      ?? throw new ArgumentNullException(nameof(pathProvider));
            IBeginWfsCodeModel = beginWfsCodeModel ?? throw new ArgumentNullException(nameof(beginWfsCodeModel));
            IWfsCodeModel      = wfsCodeModel      ?? throw new ArgumentNullException(nameof(wfsCodeModel));
            WfsBaseCodeModel   = wfsBaseCodeModel  ?? throw new ArgumentNullException(nameof(wfsCodeModel));
            TOCodeModels       = (toCodeModels     ?? Enumerable.Empty<TOCodeModel>()).ToImmutableList();
        }

        // ReSharper disable InconsistentNaming
        [NotNull]
        public ITaskDefinitionSymbol TaskDefinition { get; }
        [NotNull]
        public IPathProvider PathProvider { get; }
        [NotNull]
        public IBeginWfsCodeModel IBeginWfsCodeModel { get; }
        [NotNull]
        public IWfsCodeModel IWfsCodeModel { get; }
        [NotNull]
        public WfsBaseCodeModel WfsBaseCodeModel { get; }
        [NotNull]
        public ImmutableList<TOCodeModel> TOCodeModels { get; }
        // ReSharper restore InconsistentNaming
    }
}