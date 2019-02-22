using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using VS4Mac.AppCenter.Controllers;
using VS4Mac.AppCenter.Views;

namespace VS4Mac.AppCenter.Commands
{
	public class AnalyticsCommand : CommandHandler
	{
		protected override void Run()
		{
			var telemetryView = new TelemetryView();
			var telemetryController = new TelemetryController(telemetryView);
			IdeApp.Workbench.OpenDocument(telemetryView, true);
		}
	}
}