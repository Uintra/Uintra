using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Search.Entities;
using Nest;
using Uintra.Core.Extensions;
using Uintra.Search;
using static LanguageExt.Prelude;

namespace Compent.Uintra.Core.Search.Indexes
{
    public class UintraElasticIndex : ElasticIndex
    {
        private readonly SearchScoreModel scores;

        public UintraElasticIndex(
            IElasticSearchRepository elasticSearchRepository, 
            ISearchScoreProvider searchScoreProvider,
            IEnumerable<IElasticEntityMapper> mappers) : base(elasticSearchRepository, mappers)
        {
            scores = searchScoreProvider.GetScores();
        }

        public QueryContainer GetTagsDescriptor(string query)
        {
            var desc = new QueryContainerDescriptor<SearchableTag>().Match(m => m
                .Query(query)
                .Field(f => f.Title));
            return desc;
        }

        protected override QueryContainer[] GetQueryContainers(string query)
        {
            var containers = base.GetQueryContainers(query).ToList();

            containers.Add(GetTagNames<SearchableUintraContent>(query));
            containers.Add(GetTagNames<SearchableUintraActivity>(query));
            containers.Add(GetTagNames<SearchableUser>(query));
            containers.Add(GetTagsDescriptor(query));
            containers.AddRange(GetUserDescriptor(query));
            return containers.ToArray();
        }

        public QueryContainer[] GetUserDescriptor(string query)
        {
            var desc = new List<QueryContainer>
            {
                new QueryContainerDescriptor<SearchableUser>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.FullName)
                    .Boost(scores.UserNameScore)),
                new QueryContainerDescriptor<SearchableUser>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Lowercase)
                    .Field(f => f.Email)
                    .Boost(scores.UserEmailScore))
            };

            return desc.ToArray();
        }

        protected override List<T> CollectDocuments<T>(ISearchResponse<dynamic> response)
        {
            var documents = new List<T>();
            foreach (var document in response.Documents)
            {
                switch ((int)document.type)
                {
                    case (int)UintraSearchableTypeEnum.Events:
                    case (int)UintraSearchableTypeEnum.News:
                    case (int)UintraSearchableTypeEnum.Bulletins:
                        documents.Add(SerializationExtensions.Deserialize<SearchableUintraActivity>(document.ToString()));
                        break;
                    case (int)UintraSearchableTypeEnum.Content:
                        documents.Add(SerializationExtensions.Deserialize<SearchableUintraContent>(document.ToString()));
                        break;
                    case (int)UintraSearchableTypeEnum.Document:
                        documents.Add(SerializationExtensions.Deserialize<SearchableDocument>(document.ToString()));
                        break;
                    case (int)UintraSearchableTypeEnum.Tag:
                        documents.Add(SerializationExtensions.Deserialize<SearchableTag>(document.ToString()));
                        break;
                    case (int)UintraSearchableTypeEnum.User:
                        documents.Add(SerializationExtensions.Deserialize<SearchableUser>(document.ToString()));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Could not resolve type of searchable entity {(SearchableTypeEnum)document.type}");
                }
            }

            return documents;
        }

        protected override void HighlightAdditional(dynamic document, Dictionary<string, HighlightHit> fields, List<string> panelContent)
        {
            foreach (var field in fields.Values)
            {
                var highlightedField = field.Highlights.FirstOrDefault();
                switch (field.Field)
                {
                    case "title":
                        document.title = highlightedField;
                        break;
                    case "fullName":
                        document.fullName = highlightedField;
                        break;
                    case "userTagNames":
                        document.tagsHighlighted = true;
                        document.userTagNames = List(highlightedField).ToDynamic();
                        break;
                    case "email":
                        document.email = highlightedField;
                        break;
                }
            }
        }

        protected override QueryContainer[] GetSearchPostFilters(SearchTextQuery query)
        {
            if (query.SearchableTypeIds.Count() == 1 && query.SearchableTypeIds.First() == (int)UintraSearchableTypeEnum.User)
                return base.GetSearchPostFilters(query)
                .Append(new QueryContainerDescriptor<SearchableUser>().Terms(t => t.Field(f => f.Inactive).Terms(false)))
                .ToArray();
            return base.GetSearchPostFilters(query);
        }



        protected static QueryContainer GetTagNames<T>(string query) where T : class, ISearchibleTaggedActivity
        {
            var desc = new QueryContainerDescriptor<T>().Match(m => m
                .Query(query)
                .Field(f => f.UserTagNames));
            return desc;
        }


    }
}