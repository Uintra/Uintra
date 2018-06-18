using System;

namespace Uintra.Subscribe
{
    public interface ISubscribeSettings
    {
        Guid Id { get; }

        bool CanSubscribe { get; set; }

        string SubscribeNotes { get; set; }
    }
}