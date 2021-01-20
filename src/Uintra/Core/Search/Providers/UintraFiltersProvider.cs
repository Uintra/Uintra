using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Shared.Search.Elasticsearch.Providers;
using Nest;

namespace Uintra.Core.Search.Providers
{
    public class UintraFiltersProvider : FiltersProvider
    {
        public override AnalysisDescriptor Apply(AnalysisDescriptor analysis)
        {
            analysis.TokenFilters(f => f
                .NGram("digits_ngram", t => t.MinGram(3).MaxGram(8))
                .Length("length_limit", t => t.Min(1).Max(20))
                .EdgeNGram("custom_edge_ngram", t => t.MinGram(2).MaxGram(30))
                .PatternReplace("whitespace_remove", t => t.Pattern(" ").Replacement(string.Empty))
            );

            return analysis;
        }
    }
}