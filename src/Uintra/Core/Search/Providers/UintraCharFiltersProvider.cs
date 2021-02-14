using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Compent.Shared.Search.Elasticsearch.Providers;
using Nest;

namespace Uintra.Core.Search.Providers
{
    public class UintraCharFiltersProvider : CharFiltersProvider
    {
        public override AnalysisDescriptor Apply(AnalysisDescriptor analysis)
        {
            var result =  base.Apply(analysis);
            analysis.CharFilters(c => c
                .Mapping("mapping", f => f.Mappings(GetCharMapping()))
                //.PatternReplace("digits", f => f.Pattern("[^0-9]").Replacement(""))
                .UserDefined("digits", new PatternReplaceCharFilter {Pattern = "[^0-9]", Replacement = ""})
            );

            return analysis;
        }

        public override IEnumerable<string> GetCharMapping()
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