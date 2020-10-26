using System;
using System.Linq;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Entities.Mappers;
using Uintra.Core.Search.Indexers.Diagnostics;
using Uintra.Core.Search.Indexers.Diagnostics.Models;
using Uintra.Core.Search.Indexes;
using Uintra.Features.Search.Web;

namespace Uintra.Core.Search.Indexers
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
