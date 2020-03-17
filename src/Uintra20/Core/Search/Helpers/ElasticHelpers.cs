using System.Collections.Generic;
using Nest;

namespace Uintra20.Core.Search.Helpers
{
    public static class ElasticHelpers
    {
        public const string Replace = "Replace";
        public const string Digits = "Digits";
        public const string Key = "Key";
        public const string Web = "Web";
        public const string Lowercase = "Lowercase";
        public const string ReplaceNgram = "ReplaceNgram";
        public const string Phone = "Phone";
        public const string Tag = "Tag";
        public const int MaxAggregationSize = 500;

        public static class Normalizer
        {
            public const string Sort = "sort_normalizer";

            public static AnalysisDescriptor Initialize(AnalysisDescriptor analysis) =>
                analysis.Normalizers(n => n
                    .Custom(Sort, descriptor => descriptor.Filters("lowercase")));
        }

        public static AnalysisDescriptor SetAnalysis(AnalysisDescriptor analysis)
        {
            analysis = analysis
                .CharFilters(c => c
                    .Mapping("mapping", f => f.Mappings(GetCharMapping()))
                    .PatternReplace("digits", f => f.Pattern("[^0-9]").Replacement(""))
                )
                .TokenFilters(f => f
                    .NGram("digits_ngram", t => t.MinGram(3).MaxGram(8))
                    .Length("length_limit", t => t.Min(1).Max(20))
                    .EdgeNGram("custom_edge_ngram", t => t.MinGram(2).MaxGram(30))
                    .PatternReplace("whitespace_remove", t => t.Pattern(" ").Replacement(string.Empty))
                )
                .Tokenizers(t => t
                    .NGram("phoneNgram", d => d.MinGram(2).MaxGram(10).TokenChars(TokenChar.Digit))
                    .NGram("ngram", d => d.MinGram(2).MaxGram(30))
                    .Pattern("comma", p => p.Pattern(",")))
                .Analyzers(a => a
                    .UserDefined(Phone, new CustomAnalyzer { Tokenizer = "phoneNgram", CharFilter = new[] { "digits" }, Filter = new[] { "whitespace_remove", "lowercase" } })
                    .UserDefined(Tag, new CustomAnalyzer { Tokenizer = "comma", Filter = new[] { "whitespace_remove", "lowercase" }, CharFilter = new[] { "html_strip" } })
                    .UserDefined(Replace, new CustomAnalyzer { Tokenizer = "whitespace", Filter = new[] { "lowercase", "scandinavian_folding", "unique" }, CharFilter = new[] { "mapping" } })
                    .UserDefined(ReplaceNgram, new CustomAnalyzer { Tokenizer = "whitespace", Filter = new[] { "lowercase", "scandinavian_folding", "custom_edge_ngram", "unique" }, CharFilter = new[] { "mapping", "html_strip" } })
                    .UserDefined(Key, new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase", "scandinavian_folding" } })
                    .UserDefined(Digits, new CustomAnalyzer { Tokenizer = "keyword", CharFilter = new[] { "digits" }, Filter = new[] { "digits_ngram", "unique", "length_limit" } })
                    .UserDefined(Lowercase, new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase" } })
                );
            return Normalizer.Initialize(analysis);
        }



        public static IEnumerable<string> GetCharMapping()
        {
            return new[]
            {
                "ph=>f",
                "ch=>k",
                "nn=>n",
                "ll=>l",
                "kk=>k",
                "gg=>g",
                "mm=>m",
                "pp=>p",
                "ss=>s",
                "ff=>f",
                "dd=>d",
                "dt=>d",
                "tt=>t",
                "ks=>x",

                "Ph=>f",
                "Ch=>k",
                "Nn=>n",
                "Ll=>l",
                "Kk=>k",
                "Gg=>g",
                "Mm=>m",
                "Pp=>p",
                "Ss=>s",
                "Ff=>f",
                "Dd=>d",
                "Dt=>d",
                "Tt=>t",
                "Ks=>x",

                "pH=>f",
                "cH=>k",
                "nN=>n",
                "lL=>l",
                "kK=>k",
                "gG=>g",
                "mM=>m",
                "pP=>p",
                "sS=>s",
                "fF=>f",
                "dD=>d",
                "dT=>d",
                "tT=>t",
                "kS=>x",

                "PH=>f",
                "CH=>k",
                "NN=>n",
                "LL=>l",
                "KK=>k",
                "GG=>g",
                "MM=>m",
                "PP=>p",
                "SS=>s",
                "FF=>f",
                "DD=>d",
                "DT=>d",
                "TT=>t",
                "KS=>x"
            };
        }
    }
}