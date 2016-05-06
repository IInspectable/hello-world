﻿#region Using Directives

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.Underlining {

    [Export(typeof(ITaggerProvider))]
    [ContentType(NavLanguageContentDefinitions.ContentType)]
    [TagType(typeof(UnderlineTag))]
    sealed class UnderlineTaggerProvider : ITaggerProvider {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag {
            return UnderlineTagger.GetOrCreateSingelton<T>(buffer);
        }
    }
}