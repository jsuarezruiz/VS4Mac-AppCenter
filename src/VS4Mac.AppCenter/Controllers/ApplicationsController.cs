using System;
using System.Collections.Generic;
using System.Linq;
using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Extensions;
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

		public List<Models.Application> Applications { get; set; }

		public Models.Application SelectedApplication { get; set; }

		public List<Models.Application> LoadApplications()
		{
			var applications = AppCenterService.Instance.GetApplications();
			Applications = applications;
			return applications;
		}

		public List<Models.Application> FilterApplications(string filter)
		{
			if (string.IsNullOrEmpty(filter))
				return Applications;

			return Applications
				.Where(app => app.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || app.OwnerName.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
				.ToList();
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