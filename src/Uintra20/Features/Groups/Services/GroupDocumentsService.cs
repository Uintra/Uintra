using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uintra20.Core.Member.Entities;
using Uintra20.Core.Member.Services;
using Uintra20.Features.Groups.Sql;
using Uintra20.Infrastructure.Extensions;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Groups.Services
{
    public class GroupDocumentsService : IGroupDocumentsService
    {
        private readonly ISqlRepository<GroupDocument> _repository;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;

        public GroupDocumentsService(
            ISqlRepository<GroupDocument> repository,
            IGroupMemberService groupMemberService,
            IIntranetMemberService<IntranetMember> intranetMemberService)
        {
            _repository = repository;
            _groupMemberService = groupMemberService;
            _intranetMemberService = intranetMemberService;
        }

        public GroupDocument Get(Guid documentId)
        {
            return _repository.Get(documentId);
        }

        public Task<GroupDocument> GetAsync(Guid documentId)
        {
            return _repository.GetAsync(documentId);
        }

        public IEnumerable<GroupDocument> GetByGroup(Guid groupId)
        {
            var documents = _repository.FindAll(s => s.GroupId == groupId);
            return documents;
        }

        public Task<IList<GroupDocument>> GetByGroupAsync(Guid groupId)
        {
            var documents = _repository.FindAllAsync(s => s.GroupId == groupId);
            return documents;
        }

        public Guid Create(GroupDocument document)
        {
            document.Id = Guid.NewGuid();
            _repository.Add(document);
            return document.Id;
        }

        public async Task<Guid> CreateAsync(GroupDocument document)
        {
            document.Id = Guid.NewGuid();
            await _repository.AddAsync(document);
            return document.Id;
        }

        public void Create(IEnumerable<GroupDocument> documents)
        {
            _repository.Add(documents.Select(i =>
            {
                i.Id = Guid.NewGuid();
                return i;
            }));
        }

        public Task CreateAsync(IEnumerable<GroupDocument> documents)
        {
            return _repository.AddAsync(documents.Select(i =>
            {
                i.Id = Guid.NewGuid();
                return i;
            }));
        }

        public void Delete(GroupDocument document)
        {
            _repository.Delete(document);
        }

        public Task DeleteAsync(GroupDocument document)
        {
            return _repository.DeleteAsync(document);
        }

        public bool CanUpload(Guid groupId)
        {
            return _groupMemberService.IsGroupMember(groupId, _intranetMemberService.GetCurrentMemberId());
        }

        public async Task<bool> CanUploadAsync(Guid groupId)
        {
            return await _groupMemberService.IsGroupMemberAsync(groupId, await _intranetMemberService.GetCurrentMemberIdAsync());
        }
    }
}