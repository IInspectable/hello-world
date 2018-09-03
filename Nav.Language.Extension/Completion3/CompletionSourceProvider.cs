﻿#region Using Directives

using System.Collections.Generic;
using System.ComponentModel.Composition;

using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

#endregion

namespace Pharmatechnik.Nav.Language.Extension.Completion3 {

    [Export(typeof(IAsyncCompletionSourceProvider))]
    [Name(CompletionProviderNames.NavCompletionSourceProvider)]
    [ContentType(NavLanguageContentDefinitions.ContentType)]
    class CompletionSourceProvider: IAsyncCompletionSourceProvider {

        readonly IDictionary<ITextView, IAsyncCompletionSource> _cache = new Dictionary<ITextView, IAsyncCompletionSource>();

        public IAsyncCompletionSource GetOrCreate(ITextView textView) {
            
            if (_cache.TryGetValue(textView, out var completionSource)) {
                return completionSource;
            }

            var source = new CompletionSource();                  // opportunity to pass in MEF parts
            textView.Closed += (o, e) => _cache.Remove(textView); // clean up memory as files are closed
            _cache.Add(textView, source);
            
            return source;
        }

    }

}