using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using Uintra20.Features.Search.Entities;
using Uintra20.Features.Search.Member;
using Uintra20.Features.Search.Paging;
using Uintra20.Features.Search.Queries;
using Uintra20.Features.Search.Sorting;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Search.Indexes
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
			//containers.Add(GetTagNames<SearchableUintraContent>(query));
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
					case (int)UintraSearchableTypeEnum.Socials:
						documents.Add(document.ToString().Deserialize<SearchableUintraActivity>());
						break;
					//case (int)UintraSearchableTypeEnum.Content:
					//	documents.Add(document.ToString().Deserialize<SearchableUintraContent>());
					//	break;
					case (int)UintraSearchableTypeEnum.Document:
						documents.Add(document.ToString().Deserialize<SearchableDocument>());
						break;
					case (int)UintraSearchableTypeEnum.Tag:
						documents.Add(document.ToString().Deserialize<SearchableTag>());
						break;
					case (int)UintraSearchableTypeEnum.Member:
						documents.Add(document.ToString().Deserialize<SearchableMember>());
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
						document.userTagNames = new List<string>(){ highlightedField }.ToDynamic();
						break;
					case "email":
						document.email = highlightedField;
						break;
				}
			}
		}

		protected override QueryContainer[] GetSearchPostFilters(SearchTextQuery query)
		{
			if (query.SearchableTypeIds.Count() == 1 && query.SearchableTypeIds.First() == (int)UintraSearchableTypeEnum.Member)
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