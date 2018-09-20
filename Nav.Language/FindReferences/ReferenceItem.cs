﻿#region Using Directives

using System;
using System.Collections.Immutable;

using Pharmatechnik.Nav.Language.Text;

#endregion

namespace Pharmatechnik.Nav.Language.FindReferences {

    public class ReferenceItem {

        public ReferenceItem(DefinitionItem definition,
                             Location location,
                             ImmutableArray<ClassifiedText> textParts,
                             TextExtent textHighlightExtent,
                             ImmutableArray<ClassifiedText> previewParts,
                             TextExtent previewHighlightExtent) {

            Definition             = definition ?? throw new ArgumentNullException(nameof(definition));
            Location               = location   ?? throw new ArgumentNullException(nameof(location));
            TextParts              = textParts;
            TextHighlightExtent    = textHighlightExtent;
            PreviewParts           = previewParts;
            PreviewHighlightExtent = previewHighlightExtent;

        }

        public DefinitionItem                 Definition             { get; }
        public Location                       Location               { get; }
        public ImmutableArray<ClassifiedText> TextParts              { get; }
        public TextExtent                     TextHighlightExtent    { get; }
        public ImmutableArray<ClassifiedText> PreviewParts           { get; }
        public TextExtent                     PreviewHighlightExtent { get; }

        public string Text        => TextParts.JoinText();
        public string Preview     => PreviewParts.JoinText();
        public string ProjectName => Definition.SearchRoot;

    }

}