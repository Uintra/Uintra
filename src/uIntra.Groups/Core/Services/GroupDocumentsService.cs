using System;
using System.Linq;
using System.Collections.Generic;
using Uintra.Core.Persistence;
using Uintra.Groups.Sql;

namespace Uintra.Groups
{
    public class GroupDocumentsService : IGroupDocumentsService
    {
        private readonly ISqlRepository<GroupDocument> _repository;

        public GroupDocumentsService(ISqlRepository<GroupDocument> repository)
        {
            _repository = repository;
        }

        public GroupDocument Get(Guid documentId)
        {
            return _repository.Get(documentId);
        }

        public IEnumerable<GroupDocument> GetByGroup(Guid groupId)
        {
            var documents = _repository.FindAll(s => s.GroupId == groupId);
            return documents;
        }

        public Guid Create(GroupDocument document)
        {
            document.Id = Guid.NewGuid();
            _repository.Add(document);
            return document.Id;
        }

        public void Create(IEnumerable<GroupDocument> documents)
        {
            _repository.Add(documents.Select(i=> 
            {
                i.Id = Guid.NewGuid();
                return i;
            }));
        }

        public void Delete(GroupDocument document)
        {
            _repository.Delete(document);
        }
    }
}