using Nest;
using Uintra20.Core.Search.Helpers;
using Uintra20.Features.Links.Models;

namespace Uintra20.Core.Search.Entities.Mappings
{
    public class SearchableBaseMap<T> : PropertiesDescriptor<T> where T : SearchableBase
    {
        public SearchableBaseMap()
        {
            Text(t => t.Name(n => n.Title).Analyzer(ElasticHelpers.ReplaceNgram));
            Number(t => t.Name(n => n.Type).Type(NumberType.Integer));
            Keyword(t => t.Name(n => n.Id));
            Nested<UintraLinkModel>(nst =>
                nst.Name(n => n.Url)
                    .Properties(p =>
                        p.Keyword(k => k.Name(n => n.OriginalUrl))));

            Nested<UintraLinkModel>(nst =>
                nst.Name(n => n.Url)
                    .Properties(p =>
                        p.Keyword(k => k.Name(n => n.BaseUrl))));

            Nested<UintraLinkModel>(nst =>
                nst.Name(n => n.Url)
                    .Properties(p =>
                        p.Nested<UintraLinkParamModel>(
                            nnst => 
                                nnst.Name(n => n.Params)
                                .Properties(pp => 
                                    pp.Keyword(kk => 
                                        kk.Name(nn => nn.Name))))));

            Nested<UintraLinkModel>(nst =>
                nst.Name(n => n.Url)
                    .Properties(p =>
                        p.Nested<UintraLinkParamModel>(
                            nnst =>
                                nnst.Name(n => n.Params)
                                    .Properties(pp =>
                                        pp.Keyword(kk =>
                                            kk.Name(nn => nn.Data))))));
        }
    }
}