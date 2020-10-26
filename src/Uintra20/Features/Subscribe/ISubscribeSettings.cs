using System;

namespace Uintra20.Features.Subscribe
{
    public interface ISubscribeSettings
    {
        Guid Id { get; }

        bool CanSubscribe { get; set; }

        string SubscribeNotes { get; set; }
    }
}
