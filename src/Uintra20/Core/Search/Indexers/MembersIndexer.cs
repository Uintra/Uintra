using System;
using System.Linq;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Core.Search.Entities;
using Uintra20.Core.Search.Entities.Mappers;
using Uintra20.Core.Search.Indexers.Diagnostics;
using Uintra20.Core.Search.Indexers.Diagnostics.Models;
using Uintra20.Core.Search.Indexes;
using Uintra20.Features.Search.Web;

namespace Uintra20.Core.Search.Indexers
{
	public class MembersIndexer<T> : IIndexer where T : SearchableMember
	{
		private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
		private readonly IElasticMemberIndex<T> _elasticMemberIndex;
		private readonly ISearchableMemberMapper<T> _searchableMemberMapper;
        private readonly IIndexerDiagnosticService _indexerDiagnosticService;

		public MembersIndexer(
			IIntranetMemberService<IntranetMember> intranetMemberService,
			IElasticMemberIndex<T> elasticMemberIndex,
			ISearchableMemberMapper<T> searchableMemberMapper, 
            IIndexerDiagnosticService indexerDiagnosticService)
		{
			_intranetMemberService = intranetMemberService;
			_elasticMemberIndex = elasticMemberIndex;
			_searchableMemberMapper = searchableMemberMapper;
            _indexerDiagnosticService = indexerDiagnosticService;
        }

		public virtual IndexedModelResult FillIndex()
		{
            try
            {
                var actualUsers = _intranetMemberService.GetAll().Where(u => !u.Inactive);
                var searchableUsers = actualUsers.Select(_searchableMemberMapper.Map).ToList();
                _elasticMemberIndex.Index(searchableUsers);

                return _indexerDiagnosticService.GetSuccessResult(typeof(MembersIndexer<T>).Name, searchableUsers);
            }
            catch (Exception e)
            {
                return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(MembersIndexer<T>).Name);
            }
		}
	}
}
