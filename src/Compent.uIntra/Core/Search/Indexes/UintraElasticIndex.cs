
using System;
using System.Collections.Generic;
using System.Linq;
using Compent.uIntra.Core.Search.Entities;
using Nest;
using uIntra.Core.Extensions;
using uIntra.Search;

namespace Compent.uIntra.Core.Search.Indexes
{
    public class UintraElasticIndex : ElasticIndex
    {
            private readonly SearchScoreModel scores;

            public UintraElasticIndex(IElasticSearchRepository elasticSearchRepository, ISearchScoreProvider searchScoreProvider) : base(elasticSearchRepository)
            {
                scores = searchScoreProvider.GetScores();
            }

            protected override QueryContainer[] GetQueryContainers(string query)
            {
                var containers = base.GetQueryContainers(query).ToList();
                containers.Add(GetActivityTagNames(query));
                containers.Add(GetContentTagNames(query));
                containers.Add(GetUserTagNames(query));
                containers.Add(GetTagsDescriptor(query));
                containers.AddRange(GetUserDescriptor(query));
                return containers.ToArray();
            }

            protected override QueryContainer GetBaseDescriptor(string query)
            {
                var desc = new QueryContainerDescriptor<SearchableBase>().Match(m => m
                     .Query(query)
                     .Boost(scores.TitleScore)
                     .Analyzer(ElasticHelpers.Replace)
                     .Field(f => f.Title));
                return desc;
            }

            protected override QueryContainer GetActivityDescriptor(string query)
            {
                var desc = new QueryContainerDescriptor<SearchableUintraActivity>().Match(m => m
                    .Query(query)
                    .Analyzer(ElasticHelpers.Replace)
                    .Field(f => f.Description));
                return desc;
            }

            public QueryContainer GetTagsDescriptor(string query)
            {
                var desc = new QueryContainerDescriptor<SearchableTag>().Match(m => m
                   .Query(query)
                   .Analyzer(ElasticHelpers.Replace)
                   .Field(f => f.Title));
                return desc;
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
                            document.userTagNames = highlightedField.ToEnumerableOfOne().ToDynamic();
                            break;
                        case "phone":
                            document.phone = highlightedField;
                            break;
                        case "email":
                            document.email = highlightedField;
                            break;
                    }
                }
            }

            private QueryContainer GetUserTagNames(string query)
            {
                var desc = new QueryContainerDescriptor<SearchableUser>().Match(m => m
                   .Query(query)
                   .Analyzer(ElasticHelpers.Replace)
                   .Field(f => f.UserTagNames));
                return desc;
            }

            private QueryContainer GetActivityTagNames(string query)
            {
                var desc = new QueryContainerDescriptor<SearchableUintraActivity>().Match(m => m
                   .Query(query)
                   .Analyzer(ElasticHelpers.Replace)
                   .Field(f => f.UserTagNames));
                return desc;
            }

            private QueryContainer GetContentTagNames(string query)
            {
                var desc = new QueryContainerDescriptor<SearchableUintraContent>().Match(m => m
                   .Query(query)
                   .Analyzer(ElasticHelpers.Replace)
                   .Field(f => f.UserTagNames));
                return desc;
            }
        }
}