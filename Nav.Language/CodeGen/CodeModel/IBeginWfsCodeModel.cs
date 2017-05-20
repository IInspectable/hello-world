﻿#region Using Directives

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;

using JetBrains.Annotations;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    // TODO Gemeinsames TaskBased CodeModel Einführen. Gemeinsame Eigenschaften : TaskName, SyntaxFileName?
    // ReSharper disable once InconsistentNaming
    public sealed class IBeginWfsCodeModel : FileGenerationCodeModel {

        IBeginWfsCodeModel(string relativeSyntaxFileName, TaskCodeModel taskCodeModel, ImmutableList<string> usingNamespaces, string taskName, string baseInterfaceName, ImmutableList<TaskInitCodeModel> taskInits, ImmutableList<string> codeDeclarations, string filePath) 
            :base(taskCodeModel, relativeSyntaxFileName, filePath) {

            UsingNamespaces   = usingNamespaces   ?? throw new ArgumentNullException(nameof(usingNamespaces));
            TaskName          = taskName          ?? throw new ArgumentNullException(nameof(usingNamespaces));
            BaseInterfaceName = baseInterfaceName ?? throw new ArgumentNullException(nameof(usingNamespaces));
            TaskInits         = taskInits         ?? throw new ArgumentNullException(nameof(usingNamespaces));
            CodeDeclarations  = codeDeclarations  ?? throw new ArgumentNullException(nameof(codeDeclarations));
        }

        public static IBeginWfsCodeModel FromTaskDefinition(ITaskDefinitionSymbol taskDefinition, IPathProvider pathProvider) {

            if (taskDefinition == null) {
                throw new ArgumentNullException(nameof(taskDefinition));
            }
            if (pathProvider == null) {
                throw new ArgumentNullException(nameof(pathProvider));
            }

            var taskDefinitionSyntax = taskDefinition.Syntax;
            var taskCodeModel = TaskCodeModel.FromTaskDefinition(taskDefinition);

            // UsingNamespaces
            var namespaces = new List<string>();
            namespaces.Add(taskCodeModel.IwflNamespace);
            namespaces.Add(CodeGenFacts.NavigationEngineIwflNamespace);
            namespaces.Add(CodeGenFacts.NavigationEngineWflNamespace);
            namespaces.AddRange(taskDefinition.CodeGenerationUnit.GetCodeUsingNamespaces());

            var codeDeclarations = new List<string>();
            if (taskDefinitionSyntax.CodeDeclaration != null) {
                codeDeclarations.AddRange(taskDefinitionSyntax.CodeDeclaration.GetGetStringLiterals().Select(sl => sl.ToString().Trim('"')));
            }

            // Inits
            var taskInits = new List<TaskInitCodeModel>();
            foreach (var initNode in taskDefinition.NodeDeclarations.OfType<IInitNodeSymbol>()) {
                var taskInit = TaskInitCodeModel.FromInitNode(initNode, taskCodeModel);
                taskInits.Add(taskInit);
            }
            var relativeSyntaxFileName = pathProvider.GetRelativePath(pathProvider.IBeginWfsFileName, pathProvider.SyntaxFileName);
            
            return new IBeginWfsCodeModel(
                relativeSyntaxFileName: relativeSyntaxFileName,
                taskCodeModel         : taskCodeModel,
                usingNamespaces       : namespaces.ToSortedNamespaces(),
                taskName              : taskDefinition.Name ?? string.Empty,
                baseInterfaceName     : taskDefinitionSyntax.CodeBaseDeclaration?.IBeginWfsBaseType?.ToString() ?? CodeGenFacts.DefaultIBeginWfsBaseType,
                taskInits             : taskInits.ToImmutableList(),
                codeDeclarations      : codeDeclarations.ToImmutableList(),
                filePath              : pathProvider.IBeginWfsFileName);
        }
    
        [NotNull]
        public ImmutableList<string> UsingNamespaces { get; }

        [NotNull]
        public string Namespace => Task.WflNamespace;
        [NotNull]
        public string TaskName { get; }
        [NotNull]
        public string BaseInterfaceName { get; }
        [NotNull]
        public ImmutableList<TaskInitCodeModel> TaskInits { get; }
        [NotNull]
        public ImmutableList<string> CodeDeclarations { get; }
    }
}