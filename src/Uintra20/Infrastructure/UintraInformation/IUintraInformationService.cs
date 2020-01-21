using System;

namespace Uintra20.Infrastructure
{
    public interface IUintraInformationService
    {
        Uri DocumentationLink { get; }
        Version Version { get; }
    }
}