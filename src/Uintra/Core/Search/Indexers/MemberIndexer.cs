using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compent.Shared.Search.Contract;
using UBaseline.Search.Core;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Core.Search.Entities;
using Uintra.Core.Search.Entities.Mappers;

namespace Uintra.Core.Search.Indexers
{
    public class MemberIndexer : ISearchDocumentIndexer
    {
        public Type Type { get; } = typeof(SearchableMember);

        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly ISearchableMemberMapper<SearchableMember> _searchableMemberMapper;
        private readonly IIndexContext<SearchableMember> _indexContext;
        private readonly ISearchRepository<SearchableMember> _searchRepository;

        public MemberIndexer(
            IIndexContext<SearchableMember> indexContext, 
            ISearchRepository<SearchableMember> searchRepository,
            ISearchableMemberMapper<SearchableMember> searchableMemberMapper, 
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _indexContext = indexContext;
            _searchRepository = searchRepository;
            _searchableMemberMapper = searchableMemberMapper;
            _intranetMemberService = intranetMemberService;
        }


        public async Task<bool> RebuildIndex()
        {
            try
            {
                var actualUsers = _intranetMemberService.GetAll().Where(u => !u.Inactive);
                var searchableUsers = actualUsers.Select(_searchableMemberMapper.Map).ToList();
                await _indexContext.RecreateIndex();
                await _searchRepository.IndexAsync(searchableUsers);

                return true;

                //return _indexerDiagnosticService.GetSuccessResult(typeof(MembersIndexer<T>).Name, searchableUsers);
            }
            catch (Exception e)
            {
                return false;

                //return _indexerDiagnosticService.GetFailedResult(e.Message + e.StackTrace, typeof(MembersIndexer<T>).Name);
            }
        }

        public Task<bool> Delete(IEnumerable<string> nodeIds)
        {
            return Task.FromResult(true);
        }

    }
}