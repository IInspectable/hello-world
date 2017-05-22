﻿#region Using Directives

using System;
using System.IO;
using System.Linq;

using Pharmatechnik.Nav.Language;
using Pharmatechnik.Nav.Language.CodeGen;
using Pharmatechnik.Nav.Language.BuildTasks;

using Pharmatechnik.Nav.Utilities.IO;

#endregion

namespace Nav.Client {

    sealed class Program : IDisposable {

        static void Main(string[] args) {

            var cl = CommandLine.Parse(args);

            if (cl == null) {
                PressAnyKeyToContinue();
                return;
            }

            using (var p = new Program()) {
                p.Run(cl);
            }

            //PressAnyKeyToContinue();
        }

        static void PressAnyKeyToContinue() {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public void Dispose() {
        }

        void Run(CommandLine cl) {

            var syntaxProviderFactory = cl.UseSyntaxCache ? SyntaxProviderFactory.Cached : SyntaxProviderFactory.Default;
    
            var options  = new GenerationOptions(force: cl.Force, generateToClasses:cl.GenerateToClasses);
            var logger   = new ConsoleLogger(cl.Verbose);
            var pipeline = new NavCodeGeneratorPipeline(options, logger, syntaxProviderFactory);

            var navFiles  = Directory.EnumerateFiles(cl.Directory, "*.nav", SearchOption.AllDirectories);
            var fileSpecs = navFiles.Select(file => new FileSpec(identity: PathHelper.GetRelativePath(cl.Directory, file), fileName: file));

            pipeline.Run(fileSpecs);
        }        
    }
}