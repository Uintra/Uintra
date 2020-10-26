﻿using System;
using System.Collections.Generic;
using System.Linq;
using Compent.LinkPreview.HttpClient.Extensions;
using Nest;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Paging;
using Uintra.Core.Search.Repository;
using Uintra.Core.Search.Sorting;
using Uintra.Features.Search.Member;
using Uintra.Features.Search.Queries;

namespace Uintra.Core.Search.Indexes
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
						documents.Add(SerializationExtensions.Deserialize<SearchableActivity>(document.ToString()));
						break;
					case (int)UintraSearchableTypeEnum.Content:
						documents.Add(SerializationExtensions.Deserialize<SearchableContent>(document.ToString()));
						break;
					case (int)UintraSearchableTypeEnum.Document:
						documents.Add(SerializationExtensions.Deserialize<SearchableDocument>(document.ToString()));
						break;
					case (int)UintraSearchableTypeEnum.Tag:
						documents.Add(SerializationExtensions.Deserialize<SearchableTag>(document.ToString()));
						break;
					case (int)UintraSearchableTypeEnum.Member:
						documents.Add( SerializationExtensions.Deserialize<SearchableMember>(document.ToString()));
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