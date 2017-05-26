﻿#region Using Directives

using System;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    abstract class CallCodeModel: CodeModel {
        
        protected CallCodeModel(string name, EdgeMode edgeMode) {
            Name     = name ?? String.Empty;
            EdgeMode = edgeMode;
        }

        public EdgeMode EdgeMode { get; }
        public string Name { get; }
        public string PascalCaseName => Name.ToPascalcase();
        public abstract string TemplateName { get; }
    }

    sealed class ExitCallCodeModel : CallCodeModel {

        public ExitCallCodeModel(string name, EdgeMode edgeMode) : base(name, edgeMode) {
        }

        public override string TemplateName => "goToExit";
    }

    sealed class EndCallCodeModel : CallCodeModel {

        public EndCallCodeModel(string name, EdgeMode edgeMode) : base(name, edgeMode) {
        }

        public override string TemplateName => "goToEnd";
    }

    sealed class TaskCallCodeModel : CallCodeModel {

        public TaskCallCodeModel(string name, EdgeMode edgeMode, ParameterCodeModel taskResult) : base(name, edgeMode) {
            TaskResult = taskResult ?? throw new ArgumentNullException(nameof(taskResult));
        }

        public ParameterCodeModel TaskResult { get; }

        public override string TemplateName {
            get {
                switch (EdgeMode) {
                    case EdgeMode.Modal:
                        return "openModalTask";
                    case EdgeMode.NonModal:
                        return "startNonModalTask";
                    case EdgeMode.Goto:
                        return "gotoTask";
                    default:
                        return "";
                }
            }
        }
    }

    sealed class GuiCallCodeModel : CallCodeModel {

        public GuiCallCodeModel(string name, EdgeMode edgeMode) : base(name, edgeMode) {
        }

        public override string TemplateName {
            get {
                switch (EdgeMode) {
                    case EdgeMode.Modal:
                        return "openModalGUI";
                    case EdgeMode.NonModal:
                        return "startNonModalGUI";  
                    case EdgeMode.Goto:
                        return "gotoGUI";
                    default:
                        return "";
                }
            }
        }
    }
}