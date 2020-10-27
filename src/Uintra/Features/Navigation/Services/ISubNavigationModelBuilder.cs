using Uintra.Features.Navigation.Models;

namespace Uintra.Features.Navigation.Services
{
	public interface ISubNavigationModelBuilder
	{
		SubNavigationMenuItemModel GetMenu();
	}
}
