using System;

namespace uIntra.Subscribe
{
    public interface ISubscribeSettings
    {
        Guid Id { get; set; }

        bool CanSubscribe { get; set; }

        string SubscribeNotes { get; set; }
    }
}