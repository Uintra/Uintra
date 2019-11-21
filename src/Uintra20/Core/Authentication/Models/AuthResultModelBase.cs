namespace Uintra20.Core.Authentication.Models
{
	public class AuthResultModelBase
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public string RedirectUrl { get; set; }
	}
}