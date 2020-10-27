﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uintra.Features.Groups.Sql;

namespace Uintra.Features.Groups.Services
{
    public interface IGroupDocumentsService
    {
        IEnumerable<GroupDocument> GetByGroup(Guid groupId);
        Task<IList<GroupDocument>> GetByGroupAsync(Guid groupId);
        Guid Create(GroupDocument document);
        Task<Guid> CreateAsync(GroupDocument document);
        void Create(IEnumerable<GroupDocument> documents);
        Task CreateAsync(IEnumerable<GroupDocument> documents);
        GroupDocument Get(Guid documentId);
        Task<GroupDocument> GetAsync(Guid documentId);
        void Delete(GroupDocument document);
        Task DeleteAsync(GroupDocument document);
        bool CanUpload(Guid groupId);
        Task<bool> CanUploadAsync(Guid groupId);
    }
}
