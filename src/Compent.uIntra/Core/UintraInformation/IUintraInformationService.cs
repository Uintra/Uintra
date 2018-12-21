using System;

namespace Uintra.Core
{
    public interface IUintraInformationService
    {
        Uri DocumentationLink { get; }
        Version Version { get; }
    }
}