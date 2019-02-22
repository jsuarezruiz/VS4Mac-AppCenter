using System.Collections.Generic;
using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Services;
using VS4Mac.AppCenter.Views;

namespace VS4Mac.AppCenter.Controllers
{
	public class ApplicationDetailsController : IController
	{
		readonly IApplicationsDetailsView _view;

		public ApplicationDetailsController(IApplicationsDetailsView view, Models.Application application = null)
		{
			_view = view;

			CurrentApplication = application;

			view.SetController(this);
		}

		public Models.Application CurrentApplication { get; set; }

		public List<string> LoadOS()
		{
			return AppCenterService.Instance.GetOS();
		}

		public List<string> LoadPlatforms()
		{
			return AppCenterService.Instance.GetPlatforms();
		}

		public Models.Application LoadApplicationDetails()
		{
			if(CurrentApplication != null)			
				return AppCenterService.Instance.GetApplicationDetails(CurrentApplication.OwnerName, CurrentApplication.Name);

			return null;
		}

		public Models.Application CreateApplication(string displayName, string os, string platform)
		{
			if(CurrentApplication == null)
			{
				return AppCenterService.Instance.CreateApplication(displayName, os, platform);
			}

			return null;
		}

		public Models.Application UpdateApplication(string ownerName, string name, string displayName, string description)
		{
			if (CurrentApplication != null)
			{
				return AppCenterService.Instance.UpdateApplication(ownerName, name, displayName, description);
			}

			return null;
		}
	}
}