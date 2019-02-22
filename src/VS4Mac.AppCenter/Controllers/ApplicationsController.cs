using System;
using System.Collections.Generic;
using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Services;
using VS4Mac.AppCenter.Views;

namespace VS4Mac.AppCenter.Controllers
{
	public class ApplicationsController : IController
	{
		readonly IApplicationsView _view;

		public event EventHandler<EventArgs> ApplicationsChanged;

		public ApplicationsController(IApplicationsView view)
		{
			_view = view;

			view.SetController(this);

			AppCenterService.Instance.ApplicationsChanged += OnApplicationsChanged;
		}

		public Models.Application SelectedApplication { get; set; }

		public List<Models.Application> LoadApplications()
		{
			return AppCenterService.Instance.GetApplications();
		}

		public bool DeleteApplication(string ownerName, string name)
		{
			AppCenterService.Instance.DeleteApplication(ownerName, name);

			return true;
		}

		void OnApplicationsChanged(object sender, EventArgs e)
		{
			ApplicationsChanged?.Invoke(this, new EventArgs());
		}
	}
}