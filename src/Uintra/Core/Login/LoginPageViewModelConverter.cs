using UBaseline.Core.Node;
using Uintra.Features.Information;

namespace Uintra.Core.Login
{
	public class LoginPageViewModelConverter: INodeViewModelConverter<LoginPageModel, LoginPageViewModel>
	{
		private readonly IInformationService _informationService;

		public LoginPageViewModelConverter(IInformationService informationService)
		{
			_informationService = informationService;
		}
		public void Map(LoginPageModel node, LoginPageViewModel viewModel)
		{
			viewModel.CurrentIntranetVersion = _informationService.Version;
		}
	}
}