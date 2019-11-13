using System;

namespace Uintra20.Core.Subscribe
{
    public interface ISubscribeSettings
    {
        Guid Id { get; }

        bool CanSubscribe { get; set; }

        string SubscribeNotes { get; set; }
    }
}
