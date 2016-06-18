#region Using Directives

using Microsoft.VisualStudio.Imaging.Interop;
using Pharmatechnik.Nav.Language.CodeAnalysis.Annotation;
using Pharmatechnik.Nav.Language.Extension.GoToLocation;
using Pharmatechnik.Nav.Language.Extension.GoToLocation.Provider;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.CSharp.GoTo {

    class IntraTextGoToTag : GoToTag {

        public IntraTextGoToTag(NavTaskAnnotation navTaskAnnotation): base(new NavTaskAnnotationLocationInfoProvider(navTaskAnnotation)) {
            TaskAnnotation = navTaskAnnotation;
        }

        public IntraTextGoToTag(ILocationInfoProvider provider, NavTaskAnnotation navTaskAnnotation) : base(provider) {
            TaskAnnotation = navTaskAnnotation;
        }

        public NavTaskAnnotation TaskAnnotation { get; }

        public ImageMoniker ImageMoniker {
            get {
                // TODO Evtl. Visitor um Annotations bauen...
                if (TaskAnnotation is NavInitCallAnnotation) {
                    return GoToImageMonikers.GoToInitCallDeclaration;
                }
                return TaskAnnotation is NavTriggerAnnotation ? GoToImageMonikers.GoToTriggerDefinition : GoToImageMonikers.GoToTaskDefinition;
            }
        }

        public object ToolTip {
            get {
                // TODO Tooltip Texte zentralisieren
                // TODO Evtl. Visitor um Annotations bauen...
                if (TaskAnnotation is NavTriggerAnnotation) {
                    return "Go To Trigger Definition";
                } else if(TaskAnnotation is NavInitAnnotation) {
                    return "Go To Init Definition";
                } else if (TaskAnnotation is NavExitAnnotation) {
                    return "Go To Exit Transition Definition";
                } else if(TaskAnnotation is NavInitCallAnnotation) {
                    return "Go To Begin Logic";
                }
                return "Go To Task Definition"; }
        }
    }
}