using System.Configuration;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Extensions.Bcl;
using Nest;
using UBaseline.Shared.ArticleContinuedPanel;
using UBaseline.Shared.ArticleStartPanel;
using UBaseline.Shared.FAQPanel;
using Uintra.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Entities.Mappers;
using Uintra.Core.Search.Entities.Mappings;
using Uintra.Core.Search.Helpers;
using Uintra.Core.Search.Indexers;
using Uintra.Core.Search.Indexers.Diagnostics;
using Uintra.Core.Search.Indexes;
using Uintra.Core.Search.Paging;
using Uintra.Core.Search.Providers;
using Uintra.Core.Search.Repository;
using Uintra.Core.Search.Sorting;
using Uintra.Features.Events;
using Uintra.Features.News;
using Uintra.Features.Search.Configuration;
using Uintra.Features.Search.Converters.Panel;
using Uintra.Features.Search.Member;
using Uintra.Features.Search.Web;
using Uintra.Features.Social;
using Uintra.Features.Tagging.UserTags.Models;

namespace Uintra.Core.Search.IoC
{
    public class SearchInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            var assembly = typeof(SearchInjectModule).Assembly;
            services.AddScopedToCollection<IIndexer, NewsService>();
            services.AddScopedToCollection<IIndexer, ContentIndexer>();
            services.AddScopedToCollection<IIndexer, EventsService>();
            services.AddScopedToCollection<IIndexer, SocialService<Features.Social.Entities.Social>>();
            services.AddScopedToCollection<IIndexer, DocumentIndexer>();
            services.AddScopedToCollection<IIndexer, MembersIndexer<SearchableMember>>();
            services.AddScopedToCollection<IIndexer, UserTagsSearchIndexer>();
            services.AddScopedToCollection<IDocumentIndexer, DocumentIndexer>();
            services.AddScoped<IContentIndexer, ContentIndexer>();
            

            services.AddScoped(typeof(ISearchableMemberMapper<SearchableMember>), typeof(SearchableMemberMapper<SearchableMember>));
            services.AddScoped<IElasticSearchRepository, ElasticSearchRepository>();
            services.AddScoped(typeof(IElasticSearchRepository<>), typeof(ElasticSearchRepository<>));

            services.AddSingleton<ISearchApplicationSettings, SearchApplicationSettings>();

            services.AddSingleton(typeof(PropertiesDescriptor<SearchableActivity>), typeof(SearchableActivityMap));
            services.AddSingleton(typeof(PropertiesDescriptor<SearchableUintraActivity>), typeof(SearchableUintraActivityMap));
            services.AddSingleton(typeof(PropertiesDescriptor<SearchableContent>), typeof(SearchableContentMap));
            services.AddSingleton(typeof(PropertiesDescriptor<SearchableDocument>), typeof(SearchableDocumentMap));
            services.AddSingleton(typeof(PropertiesDescriptor<SearchableTag>), typeof(SearchableTagMap));
            services.AddSingleton(typeof(PropertiesDescriptor<SearchableMember>), typeof(SearchableUserMap));

            services.AddScoped<IElasticActivityIndex, ElasticActivityIndex>();
            services.AddScoped<IElasticUintraActivityIndex, ElasticUintraActivityIndex>();
            services.AddScoped<IElasticContentIndex, ElasticContentIndex>();
            services.AddScoped<IElasticDocumentIndex, ElasticDocumentIndex>();
            services.AddScoped<IElasticTagIndex, ElasticTagIndex>();

            services.AddScoped<IActivityUserTagIndex, ActivityUserTagIndex>();
            services.AddScoped<IUserTagsSearchIndexer, UserTagsSearchIndexer>();

            services.AddScoped(typeof(IElasticMemberIndex<SearchableMember>),typeof(ElasticMemberIndex<SearchableMember>));

            services.AddScoped<IElasticEntityMapper, ElasticActivityIndex>();
            services.AddScoped<IElasticEntityMapper, ElasticContentIndex>();
            services.AddScoped<IElasticEntityMapper, ElasticDocumentIndex>();
            services.AddScoped<IElasticEntityMapper, ElasticTagIndex>();
            services.AddScoped<IElasticEntityMapper, ElasticMemberIndex<SearchableMember>>();

            services.AddScoped<IElasticIndex, UintraElasticIndex>();
            services.AddScoped<IMemberSearchDescriptorBuilder, MemberSearchDescriptorBuilder>();
            services.AddScoped(typeof(ISearchSortingHelper<>),typeof(BaseSearchSortingHelper<>));
            services.AddScoped(typeof(ISearchPagingHelper<>), typeof(BaseSearchPagingHelper<>));
            services.AddScoped<ISearchScoreProvider, SearchScoreProvider>();
            services.AddSingleton<ISearchableTypeProvider>(d => new SearchableTypeProvider(typeof(UintraSearchableTypeEnum)));
            services.AddScoped<ISearchUmbracoHelper, SearchUmbracoHelper>();
            services.AddScoped<IIndexerDiagnosticService, IndexerDiagnosticService>();
            
            services.AddScoped<ISearchContentPanelConverterProvider, SearchContentPanelConverterProvider>();
            services.AddScoped<SearchDocumentPanelConverter<ArticleStartPanelViewModel>, ArticleStartPanelSearchConverter>();
            services.AddScoped<SearchDocumentPanelConverter<ArticleContinuedPanelViewModel>, ArticleContinuedPanelSearchConverter>();
            services.AddScoped<SearchDocumentPanelConverter<UserTagsPanelViewModel>, UserTagsPanelSearchConverter>();
            RegisterHelper.ConnectImplementationsToTypesClosing(services, typeof(ISearchDocumentPanelConverter<>), assembly.ToEnumerable(), false);
            
            

            return services;
        }
    }
}