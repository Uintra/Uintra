using System;
using System.Collections.Generic;
using uCommunity.Core.App_Plugins.Core.Activity;
using uCommunity.Likes.App_Plugins.Likes;
using uCommunity.Likes.App_Plugins.Likes.Models;

namespace Compent.uCommunity.App_Plugins.Likes
{
    public class LikeableTestModel : ILikeable
    {
        public Guid Id => new Guid("28d672e1-d727-45da-aa03-96839efc0854");
        public IntranetActivityTypeEnum Type => IntranetActivityTypeEnum.Events;

        public IEnumerable<LikeModel> Likes
        {
            get { return new List<LikeModel>(); }
            set { Likes = value; }
        }
    }
}