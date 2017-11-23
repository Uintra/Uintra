using System.Collections.Generic;

namespace uIntra.Notification.Base
{
    public interface INotifierDataValue
    {
        string Url { get; set; }
        Dictionary<string, string> Tokens { get; set; }
    }
}
