using System;

namespace Uintra.Features.Subscribe
{
    public interface ISubscribeSettings
    {
        Guid Id { get; }

        bool CanSubscribe { get; set; }

        string SubscribeNotes { get; set; }
    }
}
