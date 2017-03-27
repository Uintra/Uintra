using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.User;

namespace Compent.uCommunity.Core.Users
{
    public class IntranetUserService : IIntranetUserService
    {
        private readonly List<IntranetUser> users = new List<IntranetUser>
        {
            new IntranetUser { Id = new Guid("70921E96-8C41-449B-A245-4E92CAC52B6E"), DisplayedName = "Test user name 1" },
            new IntranetUser { Id = new Guid("F3CA93FC-B7AE-4CB3-A138-003E8725855E"), DisplayedName = "Test user name 2" }
        };

        public IEnumerable<string> GetFullNamesByIds(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Guid GetCurrentUserId()
        {
            return users.First().Id;
        }

        public IEnumerable<IIntranetUser> GetAll()
        {
            return users;
        }

        public IIntranetUser Get(int umbracoId)
        {
            throw new NotImplementedException();
        }

        public IIntranetUser GetCurrentUser()
        {
            return users.First();
        }

        public IIntranetUser GetActivityUser(int umbracoId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IntranetUser> GetByIds(IEnumerable<Guid> ids)
        {
            return new List<IntranetUser>
            {
                new IntranetUser { Id = new Guid("70921E96-8C41-449B-A245-4E92CAC52B6E"), DisplayedName = "Test 1" },
                new IntranetUser { Id = new Guid("F3CA93FC-B7AE-4CB3-A138-003E8725855E"), DisplayedName = "Test 2" }
            };
        }

        public IEnumerable<Tuple<Guid, string>> GetManyNames(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IIntranetUser> GetMany(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public void FillCreator(IHaveCreator entity)
        {
            entity.Creator = entity.UmbracoCreatorId.HasValue ? users.First() : users.Last();
        }

        public IIntranetUser Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IIntranetUser> GetMany(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
    }
}