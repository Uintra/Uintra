using System;

namespace Uintra.Features.Information
{
	public interface IInformationService
	{
		Uri DocumentationLink { get; }
		Version Version { get; }
	}
}