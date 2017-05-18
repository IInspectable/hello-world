﻿#region Using Directives

using System;

using Antlr4.StringTemplate;

using Pharmatechnik.Nav.Language.CodeGen.Templates;

#endregion

namespace Pharmatechnik.Nav.Language.CodeGen {

    public class CodeGenerator: Generator {

        const string TemplateName         = "Begin";
        const string ModelAttributeName   = "model";
        const string ContextAttributeName = "context";

        public CodeGenerator(GenerationOptions options): base(options) {
        }

        public CodeGenerationResult Generate(CodeModelResult codeModelResult) {

            if (codeModelResult == null) {
                throw new ArgumentNullException(nameof(codeModelResult));
            }

            var context = new CodeGeneratorContext(this);

            var codeGenerationResult = new CodeGenerationResult(
                taskDefinition: codeModelResult.TaskDefinition                          ,
                iBeginWfsCode : GenerateIBeginWfsCode(codeModelResult.IBeginWfsCodeModel, context),
                iWfsCode      : GenerateIWfsCode(codeModelResult.IWfsCodeModel          , context),
                wfsBaseCode   : GenerateWfsBaseCode(codeModelResult.WfsBaseCodeModel    , context),
                wfsCode       : GenerateWfsCode(codeModelResult.WfsBaseCodeModel        , context));

            return codeGenerationResult;
        }
        
        static string GenerateIBeginWfsCode(IBeginWfsCodeModel model, CodeGeneratorContext context) {

            var group = LoadTemplateGroup(Resources.IBeginWfsTemplate);            
            var st    = group.GetInstanceOf(TemplateName);

            st.Add(ModelAttributeName  , model);
            st.Add(ContextAttributeName, context);

            var result = st.Render();

            return result;
        }

        static string GenerateIWfsCode(IWfsCodeModel model, CodeGeneratorContext context) {

            var group = LoadTemplateGroup(Resources.IWfsTemplate);            
            var st    = group.GetInstanceOf(TemplateName);

            st.Add(ModelAttributeName  , model);
            st.Add(ContextAttributeName, context);

            var result = st.Render();

            return result;
        }

        static string GenerateWfsBaseCode(WfsBaseCodeModel model, CodeGeneratorContext context) {

            var group = LoadTemplateGroup(Resources.WfsBaseTemplate);
            var st    = group.GetInstanceOf(TemplateName);

            st.Add(ModelAttributeName  , model);
            st.Add(ContextAttributeName, context);

            var result = st.Render();

            return result;
        }

        static string GenerateWfsCode(WfsBaseCodeModel model, CodeGeneratorContext context) {

            var group = LoadTemplateGroup(Resources.WFSOneShotTemplate);
            var st    = group.GetInstanceOf(TemplateName);

            st.Add(ModelAttributeName, model);
            st.Add(ContextAttributeName, context);

            var result = st.Render();

            return result;
        }

        static TemplateGroup LoadTemplateGroup(string resourceName) {
            var group = new TemplateGroupString(resourceName);
            group.ImportTemplates(new TemplateGroupString(Resources.CommonTemplate));
            return group;
        }
    }
}