using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using VS4Mac.AppCenter.Controllers;
using VS4Mac.AppCenter.Views;

namespace VS4Mac.AppCenter.Commands
{
	public class ApplicationsCommand : CommandHandler
	{
		protected override void Run()
		{
			try
			{
				var applicationsDialog = new ApplicationsDialog();
				var samplesImporterController = new ApplicationsController(applicationsDialog);
				applicationsDialog.Run(Xwt.MessageDialog.RootWindow);
			}
			catch (Exception ex)
			{
				MessageService.ShowError(ex.Message);
			}
		}
	}
}