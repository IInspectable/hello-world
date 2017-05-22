﻿#region Using Directives

using System;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    public sealed class TaskCodeModel: CodeModel {
        
        TaskCodeModel(string originalTaskName, string baseNamespace, string wfsBaseBaseClassName) {
            OriginalTaskName    = originalTaskName     ?? String.Empty;
            BaseNamespace       = baseNamespace        ?? String.Empty;
            WfsBaseBaseTypeName = wfsBaseBaseClassName ?? String.Empty;
        }

        public static TaskCodeModel FromTaskDefinition(ITaskDefinitionSymbol taskDefinition) {

            if (taskDefinition == null) {
                throw new ArgumentNullException(nameof(taskDefinition));
            }

            var taskName = taskDefinition.Name;
            var baseNamespace = (taskDefinition.Syntax.SyntaxTree.GetRoot() as CodeGenerationUnitSyntax)?.CodeNamespace?.Namespace?.ToString() ?? String.Empty;
            var wfsBaseBaseClassName = taskDefinition.Syntax.CodeBaseDeclaration?.WfsBaseType?.ToString() ?? CodeGenFacts.DefaultWfsBaseClass;

            return new TaskCodeModel(
                originalTaskName    : taskName,
                baseNamespace       : baseNamespace,
                wfsBaseBaseClassName: wfsBaseBaseClassName);
        }

        string BaseNamespace { get; }

        public string OriginalTaskName    { get; }        
        public string WfsBaseBaseTypeName { get; }
        public string TaskName                  => OriginalTaskName.ToPascalcase();
        public string WflNamespace              => $"{BaseNamespace}.{CodeGenFacts.WflNamespaceSuffix}";
        public string IwflNamespace             => $"{BaseNamespace}.{CodeGenFacts.IwflNamespaceSuffix}";        
        public string WfsBaseTypeName           => $"{TaskName}{CodeGenFacts.WfsBaseClassSuffix}";
        public string WfsTypeName               => $"{TaskName}{CodeGenFacts.WfsClassSuffix}";
        public string FullyQualifiedWfsName     => $"{WflNamespace}.{WfsTypeName}";
        public string FullyQualifiedWfsBaseName => $"{WflNamespace}.{WfsBaseTypeName}";
    }
}