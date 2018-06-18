using System;

namespace Uintra.Core.Context
{
    [Flags]
    public enum ContextType
    {
        Empty = 0,
        News = 2,
        Events = 4,
        Bulletins = 8,
        Activity = News | Events | Bulletins,
        Comment = 16,
        CentralFeed = 32,
        GroupFeed = 64,
        Feed = CentralFeed | GroupFeed,
        PagePromotion = 128,
        ContentPage = 256,
        Any = int.MaxValue
    }
}