﻿#region Using Directives

using System;
using System.Collections.Immutable;

using Pharmatechnik.Nav.Language.Text;

#endregion

namespace Pharmatechnik.Nav.Language.FindReferences {

    public class ReferenceEntry {

        public ReferenceEntry(DefinitionEntry definition, Location location, ImmutableArray<ClassifiedText> displayParts) {
            Definition   = definition ?? throw new ArgumentNullException(nameof(definition));
            Location     = location   ?? throw new ArgumentNullException(nameof(location));
            DisplayParts = displayParts;

        }

        public DefinitionEntry                Definition   { get; }
        public ImmutableArray<ClassifiedText> DisplayParts { get; }
        public Location                       Location     { get; }

        public string Text => DisplayParts.JoinText();

    }

}