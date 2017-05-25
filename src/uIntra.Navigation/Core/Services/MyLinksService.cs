using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uCommunity.Navigation.Core.Models;
using uIntra.Core.Persistence.Sql;

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

        public void Create(MyLinkDTO model)
        {
            var entity = new MyLink
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                ContentId = model.ContentId,
                QueryString = model.QueryString.Trim('?'),
                CreatedDate = DateTime.Now,
            };

            _myLinksRepository.Add(entity);
        }

        public void Delete(MyLinkDTO model)
        {
            var link = Get(model);
            _myLinksRepository.DeleteById(link.Id);
        }

        public bool Exists(MyLinkDTO model)
        {
            var link = Get(model);
            return link != null;
        }

        private MyLink Get(MyLinkDTO model)
        {
            var links = _myLinksRepository.FindAll(link => link.UserId == model.UserId && link.ContentId == model.ContentId);
            return links.SingleOrDefault(l => IsQueryStringEqual(model.QueryString, l.QueryString));
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