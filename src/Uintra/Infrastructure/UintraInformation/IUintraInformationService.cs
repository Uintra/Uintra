using System;

namespace Uintra.Infrastructure.UintraInformation
{
    public interface IUintraInformationService
    {
        Uri DocumentationLink { get; }
        Version Version { get; }
    }
}