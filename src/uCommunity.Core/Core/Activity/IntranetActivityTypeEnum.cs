using ServiceStack.DataAnnotations;

namespace uCommunity.Core.Activity
{
    [EnumAsInt]
    public enum IntranetActivityTypeEnum
    {
        News = 1,
        Ideas = 2,
        Events = 3
    }
}