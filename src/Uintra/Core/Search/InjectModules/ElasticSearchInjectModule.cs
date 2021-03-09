using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Search.Contract;
using Compent.Shared.Search.Elasticsearch;
using Compent.Shared.Search.Elasticsearch.Providers;
using Compent.Shared.Search.Elasticsearch.SearchHighlighting;
using UBaseline.Search.Core;
using UBaseline.Shared.PanelContainer;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Entities.Mappers;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Indexers;
using Uintra.Core.Search.Indexes;
using Uintra.Core.Search.Paging;
using Uintra.Core.Search.Providers;
using Uintra.Core.Search.Queries;
using Uintra.Core.Search.Queries.DeleteByType;
using Uintra.Core.Search.Repository;
using Uintra.Core.Search.Sorting;
using Uintra.Features.Events;
using Uintra.Features.News;
using Uintra.Features.Search.Member;
using Uintra.Features.Search.Web;
using Uintra.Features.Social;
using Uintra.Features.Social.Entities;
using ISearchableTypeProvider = Uintra.Core.Search.Providers.ISearchableTypeProvider;

namespace Uintra.Core.Search
{
    public class ElasticSearchInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddSingleton(typeof(IUintraSearchRepository<>), typeof(UintraSearchRepository<>));
            services.AddSingleton<IUintraSearchRepository, UintraSearchRepository>();
            services.AddSingleton<ISearchHighlightingHelper, UintraSearchHighlightingHelper>();

            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.TextPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.FAQPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.ContactPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.ImagePanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.DocumentLibraryPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.ArticleContinuedPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.ArticleStartPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.HeroPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.MailchimpPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.NewsPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.QuotePanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.SvgPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.UserTagsPanelSearchConverter>();
            services.AddScopedToCollection<Converters.SearchDocumentPanelConverter.ISearchDocumentPanelConverter, Uintra.Features.Search.Converters.Panel.VideoPanelSearchConverter>();

            
            //services.AddScopedToCollection<ISearchDocumentIndexer, NewsService>();
            //services.AddScopedToCollection<ISearchDocumentIndexer, ContentIndexer>();
            //services.AddScopedToCollection<ISearchDocumentIndexer, EventsService>();
            //services.AddScopedToCollection<ISearchDocumentIndexer, SocialService<Features.Social.Entities.Social>>();
            //services.AddScopedToCollection<ISearchDocumentIndexer, DocumentIndexer>();
            //services.AddScopedToCollection<ISearchDocumentIndexer, MemberIndexer>();
            //services.AddScopedToCollection<ISearchDocumentIndexer, UserTagIndexer>();

            services.AddScopedToCollection<ISearchDocumentIndexer, NewsService>();
            services.AddScopedToCollection<ISearchDocumentIndexer, EventsService>();
            services.AddScopedToCollection<ISearchDocumentIndexer, SocialService<Social>>();
            services.AddScopedToCollection<ISearchDocumentIndexer, MemberIndexer>();

            services.AddScoped<ISearchSpecificationFactory<SearchDocument, Queries.SearchByTextQuery>, SearchByTextSpecificationFactory>();
            services.AddScoped<ISearchSpecificationFactory<SearchableMember, SearchByMemberQuery>, SearchByMemberSpecificationFactory>();
            services.AddScoped<IDeleteSpecificationFactory<SearchableActivity, DeleteSearchableActivityByTypeQuery>, DeleteSearchableActivityByTypeSpecificationFactory>();

            services.AddScoped<IDocumentIndexer, DocumentIndexer>();
            services.AddScoped<IContentIndexer, ContentIndexer>();
            services.AddScoped<IUserTagIndexer, UserTagIndexer>();
            services.AddScoped<IActivityUserTagSearchRepository, ActivityUserTagSearchRepository>();
            services.AddScoped<IAnalyzerProvider, UintraAnalyzerProvider>();
            services.AddScoped<ICharFiltersProvider, UintraCharFiltersProvider>();
            services.AddScoped<IFiltersProvider, UintraFiltersProvider>();
            services.AddScoped<ITokenizerProvider, UintraTokenizerProvider>();

            services.AddScoped<ISearchUmbracoHelper, SearchUmbracoHelper>();
            services.AddSingleton<ISearchableTypeProvider>(d => new Providers.SearchableTypeProvider(typeof(UintraSearchableTypeEnum)));

            services.AddScoped<ISearchContentPanelConverterProvider, SearchContentPanelConverterProvider>();
            services.AddScoped<IMemberSearchDescriptorBuilder, MemberSearchDescriptorBuilder>();
            services.AddScoped<ISearchScoreProvider, SearchScoreProvider>();
            services.AddScoped(typeof(ISearchPagingHelper<>), typeof(BaseSearchPagingHelper<>));
            services.AddScoped(typeof(ISearchableMemberMapper<SearchableMember>), typeof(SearchableMemberMapper<SearchableMember>));

            services.AddScoped(typeof(ISearchSortingHelper<>), typeof(BaseSearchSortingHelper<>));


            services.AddSingleton(typeof(IIndexContext<>), typeof(UintraIndexContext<>));


            return services;
        }
    }
}