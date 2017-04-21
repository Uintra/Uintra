using System;
using System.Collections.Generic;
using uCommunity.Core.Extentions;
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

        public IEnumerable<MyLink> GetMany(Guid userId)
        {
            return _myLinksRepository.FindAll(myLink => myLink.UserId == userId);
        }

        public MyLink Create(Guid userId, string name, string url)
        {
            if (url.IsNullOrEmpty())
            {
                return null;
            }

            var entity = new MyLink
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name.IsNotNullOrEmpty() ? name : url,
                Url = url
            };

            entity.CreatedDate = entity.ModifyDate = DateTime.Now.ToUniversalTime();

            try
            {
                _myLinksRepository.Add(entity);
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Delete(Guid id)
        {
            var myLink = _myLinksRepository.Get(id);

            _myLinksRepository.Delete(myLink);
        }
    }
}