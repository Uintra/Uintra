using System;
using System.Collections.Generic;
using System.Linq;
using Compent.Uintra.Core.Search.Entities;
using Nest;
using Uintra.Core.Extensions;
using Uintra.Search;
using Uintra.Search.Member;
using Uintra.Search.Paging;
using Uintra.Search.Sorting;
using static LanguageExt.Prelude;

namespace Compent.Uintra.Core.Search.Indexes
{
	public class UintraElasticIndex : ElasticIndex
	{
		private readonly IMemberSearchDescriptorBuilder _memberSearchDescriptorBuilder;

		public UintraElasticIndex(
			IElasticSearchRepository elasticSearchRepository,
			IEnumerable<IElasticEntityMapper> mappers,
			ISearchSortingHelper<dynamic> searchSorting,
			ISearchPagingHelper<dynamic> searchPagingHelper,
			IMemberSearchDescriptorBuilder memberSearchDescriptorBuilder) : base(elasticSearchRepository, mappers, searchSorting, searchPagingHelper)
		{
			_memberSearchDescriptorBuilder = memberSearchDescriptorBuilder;
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
			containers.Add(GetTagNames<SearchableMember>(query));
			containers.Add(GetTagsDescriptor(query));
			containers.AddRange(_memberSearchDescriptorBuilder.GetMemberDescriptors(query));
			return containers.ToArray();
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
						documents.Add(SerializationExtensions.Deserialize<SearchableMember>(document.ToString()));
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
				.Append(new QueryContainerDescriptor<SearchableMember>().Terms(t => t.Field(f => f.Inactive).Terms(false)))
				.ToArray();
			return base.GetSearchPostFilters(query);
		}



		protected static QueryContainer GetTagNames<T>(string query) where T : class, ISearchableTaggedActivity
		{
			var desc = new QueryContainerDescriptor<T>().Match(m => m
				.Query(query)
				.Field(f => f.UserTagNames));
			return desc;
		}


	}
}