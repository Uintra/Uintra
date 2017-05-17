using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<MyLink> GetMany(IEnumerable<Guid> ids)
        {
            return _myLinksRepository.FindAll(myLink => ids.Contains(myLink.Id));
        }

        public IEnumerable<MyLink> GetUserLinks(Guid userId)
        {
            return _myLinksRepository.FindAll(myLink => myLink.UserId == userId);
        }

        public bool AddRemove(Guid userId, string name, string url)
        {
            var userLinks = GetUserLinks(userId);
            var existingLink = userLinks.FirstOrDefault(x => x.Url == url);

            if (existingLink == null)
            {
                return AddLink(userId, name, url);
            }
            else
            {
                return RemoveLink(existingLink);
            }
        }

        public bool Sort(Dictionary<Guid, int> sortOrders)
        {
            var existingLinks = GetMany(sortOrders.Select(x => x.Key));

            foreach (var link in existingLinks)
            {
                link.SortOrder = sortOrders[link.Id];
            }

            try
            {
                _myLinksRepository.Update(existingLinks);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool AddLink(Guid userId, string name, string url)
        {
            if (url.IsNullOrEmpty())
            {
                return false;
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool RemoveLink(MyLink myLink)
        {
            try
            {
                _myLinksRepository.Delete(myLink);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}