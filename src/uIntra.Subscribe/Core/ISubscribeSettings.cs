using System;

namespace uIntra.Subscribe
{
    public interface ISubscribeSettings
    {
        Guid Id { get; }

        bool CanSubscribe { get; set; }

        string SubscribeNotes { get; set; }
    }
}