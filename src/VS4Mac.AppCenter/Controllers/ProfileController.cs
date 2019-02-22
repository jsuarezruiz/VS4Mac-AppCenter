using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Models;
using VS4Mac.AppCenter.Services;
using VS4Mac.AppCenter.Views;

namespace VS4Mac.AppCenter.Controllers
{
	public class ProfileController : IController
	{
		readonly IProfileView _view;

		public ProfileController(IProfileView view)
		{
			_view = view;

			view.SetController(this);
		}

		public Profile GetProfile()
		{
			return AppCenterService.Instance.GetProfile();
		}

		public void Logout()
		{
			AppCenterService.Instance.Logout();
		}
	}
}