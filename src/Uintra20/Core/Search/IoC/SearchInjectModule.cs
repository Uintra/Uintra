using System.Configuration;
using Compent.Shared.DependencyInjection.Contract;
using Compent.Shared.Extensions.Bcl;
using Nest;
using Uintra20.Core.Search.Converters.SearchDocumentPanelConverter;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Entities.Mappers;
using Uintra20.Core.Search.Entities.Mappings;
using Uintra20.Core.Search.Helpers;
using Uintra20.Core.Search.Indexers;
using Uintra20.Core.Search.Indexes;
using Uintra20.Core.Search.Paging;
using Uintra20.Core.Search.Providers;
using Uintra20.Core.Search.Repository;
using Uintra20.Core.Search.Sorting;
using Uintra20.Features.News;
using Uintra20.Features.Search.Configuration;
using Uintra20.Features.Search.Member;
using Uintra20.Features.Search.Web;
using Uintra20.Features.Social;

namespace Uintra20.Core.Search.IoC
{
    public class SearchInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            var assembly = typeof(SearchInjectModule).Assembly;
            services.AddScopedToCollection<IIndexer, NewsService>();
            services.AddScopedToCollection<IIndexer, ContentIndexer>();
            //services.AddScopedToCollection<IIndexer, EventsService>();
            services.AddScopedToCollection<IIndexer, SocialService<Features.Social.Entities.Social>>();
            services.AddScopedToCollection<IIndexer, DocumentIndexer>();
            services.AddScopedToCollection<IIndexer, MembersIndexer<SearchableMember>>();
            services.AddScopedToCollection<IDocumentIndexer, DocumentIndexer>();

            services.AddScoped(typeof(ISearchableMemberMapper<SearchableMember>), typeof(SearchableMemberMapper<SearchableMember>));
            services.AddScoped<IElasticSearchRepository, ElasticSearchRepository>();
            services.AddScoped(typeof(IElasticSearchRepository<>), typeof(ElasticSearchRepository<>));

            services.AddSingleton<IElasticConfigurationSection>(a => ConfigurationManager.GetSection("elasticConfiguration") as ElasticConfigurationSection);
            
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
            
            services.AddScoped<ISearchContentPanelConverterProvider, SearchContentPanelConverterProvider>();
            RegisterHelper.ConnectImplementationsToTypesClosing(services, typeof(ISearchDocumentPanelConverter<>), assembly.ToEnumerable(), false);
            
            

            return services;
        }
    }
}