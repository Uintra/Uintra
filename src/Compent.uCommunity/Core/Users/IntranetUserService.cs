using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.User;

namespace Compent.uCommunity.Core.Users
{
    public class IntranetUserService : IIntranetUserService<IntranetUserBase>
    {
        private readonly List<IntranetUserBase> users = new List<IntranetUserBase>
        {
            new IntranetUser { Id = new Guid("70921E96-8C41-449B-A245-4E92CAC52B6E"), Name = "Test user name 1" },
            new IntranetUser { Id = new Guid("F3CA93FC-B7AE-4CB3-A138-003E8725855E"), Name = "Test user name 2" }
        };

        public IEnumerable<string> GetFullNamesByIds(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Guid GetCurrentUserId()
        {
            return users.First().Id;
        }

        public IEnumerable<IntranetUserBase> GetAll()
        {
            return users;
        }

        public IntranetUserBase Get(int umbracoId)
        {
            throw new NotImplementedException();
        }

        public IntranetUserBase GetCurrentUser()
        {
            return users.First();
        }

        public IntranetUserBase GetActivityUser(int umbracoId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<IntranetUser> GetByIds(IEnumerable<Guid> ids)
        {
            return new List<IntranetUser>
            {
                new IntranetUser { Id = new Guid("70921E96-8C41-449B-A245-4E92CAC52B6E"), Name = "Test 1" },
                new IntranetUser { Id = new Guid("F3CA93FC-B7AE-4CB3-A138-003E8725855E"), Name = "Test 2" }
            };
        }

        public IEnumerable<Tuple<Guid, string>> GetManyNames(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IntranetUserBase> GetMany(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public void FillCreator(IHasCreator<IntranetUserBase> entity)
        {
            entity.Creator = entity.UmbracoCreatorId.HasValue ? users.First() : users.Last();
        }
    }
}