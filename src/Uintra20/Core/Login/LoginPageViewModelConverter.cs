using System;
using UBaseline.Core.Node;
using Uintra20.Features.Information;

namespace Uintra20.Core.Login
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