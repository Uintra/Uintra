using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Uintra20.Core.Navigation.Sql;
using Uintra20.Persistence;

namespace Uintra20.Core.Navigation
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

        public MyLink Get(MyLinkDTO model)
        {
            var links = _myLinksRepository.FindAll(link => link.UserId == model.UserId && link.ContentId == model.ContentId);
            return links.SingleOrDefault(l => IsQueryStringEqual(model.QueryString, l.QueryString));
        }

        public IEnumerable<MyLink> GetMany(IEnumerable<Guid> ids)
        {
            return _myLinksRepository.GetMany(ids);
        }

        public IEnumerable<MyLink> GetMany(Guid userId)
        {
            return _myLinksRepository.FindAll(myLink => myLink.UserId == userId);
        }

        public Guid Create(MyLinkDTO model)
        {
            var entity = new MyLink
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                ContentId = model.ContentId,
                QueryString = model.QueryString.Trim('?'),
                CreatedDate = DateTime.UtcNow,
                ActivityId = model.ActivityId
            };

            _myLinksRepository.Add(entity);
            return entity.Id;
        }

        public void Delete(Guid id)
        {
            _myLinksRepository.Delete(id);
        }

        public void DeleteByActivityId(Guid activityId)
        {
            _myLinksRepository.Delete(link => link.ActivityId == activityId);
        }

        public async Task<MyLink> GetAsync(Guid id)
        {
            return await _myLinksRepository.GetAsync(id);
        }

        public async Task<MyLink> GetAsync(MyLinkDTO model)
        {
            var links = await _myLinksRepository.FindAllAsync(link => link.UserId == model.UserId && link.ContentId == model.ContentId);
            return links.SingleOrDefault(l => IsQueryStringEqual(model.QueryString, l.QueryString));
        }

        public async Task<IEnumerable<MyLink>> GetManyAsync(IEnumerable<Guid> ids)
        {
            return await _myLinksRepository.GetManyAsync(ids);
        }

        public async Task<IEnumerable<MyLink>> GetManyAsync(Guid userId)
        {
            return await _myLinksRepository.FindAllAsync(myLink => myLink.UserId == userId);
        }

        public async Task<Guid> CreateAsync(MyLinkDTO model)
        {
            var entity = new MyLink
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                ContentId = model.ContentId,
                QueryString = model.QueryString.Trim('?'),
                CreatedDate = DateTime.UtcNow,
                ActivityId = model.ActivityId
            };

            await _myLinksRepository.AddAsync(entity);
            return entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _myLinksRepository.DeleteAsync(id);
        }

        public async Task DeleteByActivityIdAsync(Guid activityId)
        {
            await _myLinksRepository.DeleteAsync(link => link.ActivityId == activityId);
        }

        private static bool IsQueryStringEqual(string query, string queryToCompare)
        {
            var queryCollection = HttpUtility.ParseQueryString(query);
            var queryCollectionCompareTo = HttpUtility.ParseQueryString(queryToCompare);

            if (queryCollection.Count != queryCollectionCompareTo.Count)
            {
                return false;
            }

            foreach (string key in queryCollection)
            {
                var queryValue = queryCollection[key];

                var queryNameValue = queryCollectionCompareTo.Get(key);
                if (queryNameValue == null || queryNameValue != queryValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}