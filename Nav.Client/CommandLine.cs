﻿#region Using Directives

using System;
using Fclp;

#endregion

namespace Nav.Client {

    sealed class CommandLine {

        public string Directory { get; set; }
        public bool Force { get; set; }
        public bool GenerateToClasses { get; set; }
        public bool UseSyntaxCache { get; set; }
        public bool Verbose { get; set; }

        public static CommandLine Parse(string[] commandline) {

            var clp = new FluentCommandLineParser<CommandLine>();
            clp.Setup(i => i.Directory).As('d', nameof(Directory)).Required();
            clp.Setup(i => i.Force).As('f', nameof(Force)).SetDefault(false);
            clp.Setup(i => i.GenerateToClasses).As('g', nameof(GenerateToClasses)).SetDefault(true);
            clp.Setup(i => i.UseSyntaxCache).As('c', nameof(UseSyntaxCache)).SetDefault(false);
            clp.Setup(i => i.Verbose).As('v', nameof(Verbose)).SetDefault(false);

            var result = clp.Parse(commandline);
            if (result.HasErrors) {
                Console.Error.WriteLine($"Unable to parse command line:\n{result.ErrorText}");
                return null;
            }

            CommandLine cla = clp.Object;

            return cla;
        }
    }
}