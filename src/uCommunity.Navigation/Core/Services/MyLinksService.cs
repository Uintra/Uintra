using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Navigation.Core
{
    public class MyLinksService : IMyLinksService
    {
        private readonly ISqlRepository<MyLink> _myLinksRepository;

        public MyLinksService(ISqlRepository<MyLink> myLinksRepository)
        {
            _myLinksRepository = myLinksRepository;
        }

        public MyLink Get(Guid id)
        {
            return _myLinksRepository.Get(id);
        }

        public IEnumerable<MyLink> GetMany(IEnumerable<Guid> ids)
        {
            return _myLinksRepository.GetMany(ids.Cast<object>());
        }

        public IEnumerable<MyLink> GetMany(Guid userId)
        {
            return _myLinksRepository.FindAll(myLink => myLink.UserId == userId);
        }

        public void Create(Guid userId, int contentId)
        {
            var entity = new MyLink
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ContentId = contentId,
                CreatedDate = DateTime.Now
            };

            _myLinksRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            _myLinksRepository.DeleteById(id);
        }

        public bool Exists(Guid userId, int contentId)
        {
            return _myLinksRepository.Exists(link => link.UserId == userId && link.ContentId == contentId);
        }
    }
}