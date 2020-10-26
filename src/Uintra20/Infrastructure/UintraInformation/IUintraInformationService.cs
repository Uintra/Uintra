using System;

namespace Uintra20.Infrastructure.UintraInformation
{
    public interface IUintraInformationService
    {
        Uri DocumentationLink { get; }
        Version Version { get; }
    }
}