using Compent.Shared.Search.Elasticsearch.Providers;
using Nest;

namespace Uintra.Core.Search.Providers
{
    public class UintraAnalyzerProvider : AnalyzerProvider
    {
        public const string Replace = "Replace";
        public const string Digits = "Digits";
        public const string Key = "Key";
        public const string Web = "Web";
        public const string Lowercase = "Lowercase";
        public const string ReplaceNgram = "ReplaceNgram";
        public const string Phone = "Phone";
        public const string Tag = "Tag";

        public UintraAnalyzerProvider(ICharFiltersProvider charFiltersProvider) : base(charFiltersProvider)
        {
        }

        public override AnalysisDescriptor Apply(AnalysisDescriptor analysis)
        {
            analysis.Analyzers(a => a
                .UserDefined(Phone, new CustomAnalyzer { Tokenizer = "phoneNgram", CharFilter = new[] { "digits" }, Filter = new[] { "whitespace_remove", "lowercase" } })
                .UserDefined(Tag, new CustomAnalyzer { Tokenizer = "comma", Filter = new[] { "whitespace_remove", "lowercase" }, CharFilter = new[] { "html_strip" } })
                .UserDefined(Replace, new CustomAnalyzer { Tokenizer = "whitespace", Filter = new[] { "lowercase", "scandinavian_folding", "unique" }, CharFilter = new[] { "mapping" } })
                .UserDefined(ReplaceNgram, new CustomAnalyzer { Tokenizer = "whitespace", Filter = new[] { "lowercase", "scandinavian_folding", "custom_edge_ngram", "unique" }, CharFilter = new[] { "mapping", "html_strip" } })
                .UserDefined(Key, new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase", "scandinavian_folding" } })
                .UserDefined(Digits, new CustomAnalyzer { Tokenizer = "keyword", CharFilter = new[] { "digits" }, Filter = new[] { "digits_ngram", "unique", "length_limit" } })
                .UserDefined(Lowercase, new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase" } })
            );

            return analysis;
        }
    }
}