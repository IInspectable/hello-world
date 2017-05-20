#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using JetBrains.Annotations;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    public sealed class TaskInitCodeModel: CodeModel {

        TaskInitCodeModel(string initName, TaskCodeModel taskCodeModel, ImmutableList<ParameterCodeModel> parameter) {

            TaskCodeModel        = taskCodeModel ?? throw new ArgumentNullException(nameof(taskCodeModel));
            Parameter            = parameter     ?? throw new ArgumentNullException(nameof(parameter));
            BeginMethodName      = $"{CodeGenFacts.BeginMethodPrefix}";
            BeginLogicMethodName = $"{CodeGenFacts.BeginMethodPrefix}{CodeGenFacts.LogicMethodSuffix}";           
            InitName             = initName ?? String.Empty;
        }
        
        public static TaskInitCodeModel FromInitNode(IInitNodeSymbol initNodeSymbol) {

            if (initNodeSymbol == null) {
                throw new ArgumentNullException(nameof(initNodeSymbol));
            }

            var taskCodeModel = TaskCodeModel.FromTaskDefinition(initNodeSymbol.ContainingTask);

            return FromInitNode(initNodeSymbol, taskCodeModel);
        }

        internal static TaskInitCodeModel FromInitNode(IInitNodeSymbol initNodeSymbol, TaskCodeModel taskCodeModel) {

            if (initNodeSymbol == null) {
                throw new ArgumentNullException(nameof(initNodeSymbol));
            }
            if (taskCodeModel == null) {
                throw new ArgumentNullException(nameof(taskCodeModel));
            }

            string GetParameterName(string name, ref int i) {
                return String.IsNullOrEmpty(name) ? $"p{i++}" : name;
            }

            var parameter = new List<ParameterCodeModel>();
            var paramterList = initNodeSymbol.Syntax.CodeParamsDeclaration?.ParameterList;
            if (paramterList != null) {
                // TODO parameterName Fallback überprüfen
                int i = 1;
                foreach (var parameterSyntax in paramterList) {
                    parameter.Add(new ParameterCodeModel(
                        parameterType: parameterSyntax.Type?.ToString(), 
                        parameterName: GetParameterName(parameterSyntax.Identifier.ToString(), ref i)));
                }
            }
            
            return new TaskInitCodeModel(initName     : initNodeSymbol.Name ?? String.Empty, 
                                         taskCodeModel: taskCodeModel, 
                                         parameter    : parameter.ToImmutableList());
        }

        [NotNull]
        public TaskCodeModel TaskCodeModel { get; }
        [NotNull]
        public string BeginLogicMethodName { get; }
        [NotNull]
        public string BeginMethodName { get; }
        [NotNull]
        public string InitName { get; }
        [NotNull]
        public ImmutableList<ParameterCodeModel> Parameter { get; }
    }
}