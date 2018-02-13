using System.Collections.Generic;
using Nest;

namespace Uintra.Search
{
    public static class ElasticHelpers
    {
        public const string Replace = "Replace";
        public const string Digits = "Digits";
        public const string PostalCode = "PostalCode";
        public const string Key = "Key";
        public const string Web = "Web";
        public const string NotAnalyzed = "NotAnalyzed";
        public const string Lowercase = "Lowercase";
        public const string ReplaceNgram = "ReplaceNgram";
        public const string EmailDomainName = "EmailDomainName";
        public const int MaxAggregationSize = 500;


        public static AnalysisDescriptor SetAnalysis(AnalysisDescriptor analysis)
        {
            return analysis
                .CharFilters(c => c
                    .Mapping("mapping", f => f.Mappings(GetCharMapping()))
                    .PatternReplace("digits", f => f.Pattern("[^0-9]").Replacement(""))
                )
                .TokenFilters(f => f
                    .NGram("digits_ngram", t => t.MinGram(3).MaxGram(8))
                    .Length("length_limit", t => t.Min(1).Max(20))
                    .EdgeNGram("custom_edge_ngram", t => t.MinGram(1).MaxGram(50))
                )
                .Tokenizers(t => t
                    .NGram("ngram", d => d.MinGram(2).MaxGram(30)))
                .Analyzers(a => a
                    .UserDefined(Replace, new CustomAnalyzer { Tokenizer = "whitespace", Filter = new[] { "lowercase", "scandinavian_folding", "unique" }, CharFilter = new[] { "mapping" } })
                    .UserDefined(ReplaceNgram, new CustomAnalyzer { Tokenizer = "whitespace", Filter = new[] { "lowercase", "scandinavian_folding", "custom_edge_ngram", "unique" }, CharFilter = new[] { "mapping", "html_strip" } })
                    .UserDefined(Key, new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase", "scandinavian_folding" } })
                    .UserDefined(Digits, new CustomAnalyzer { Tokenizer = "keyword", CharFilter = new[] { "digits" }, Filter = new[] { "digits_ngram", "unique", "length_limit" } })
                    .UserDefined(Lowercase, new CustomAnalyzer { Tokenizer = "keyword", Filter = new[] { "lowercase" } })
                );
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