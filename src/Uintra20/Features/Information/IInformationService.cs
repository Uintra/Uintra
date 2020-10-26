using System;

namespace Uintra20.Features.Information
{
	public interface IInformationService
	{
		Uri DocumentationLink { get; }
		Version Version { get; }
	}
}