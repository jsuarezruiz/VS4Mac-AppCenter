using System;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using VS4Mac.AppCenter.Controllers;
using VS4Mac.AppCenter.Views;

namespace VS4Mac.AppCenter.Commands
{
	public class ProfileCommand : CommandHandler
	{
		protected override void Run()
		{
			try
			{
				var profileDialog = new ProfileDialog();
				var profileController = new ProfileController(profileDialog);
				profileDialog.Run(Xwt.MessageDialog.RootWindow);
			}
			catch (Exception ex)
			{
				MessageService.ShowError(ex.Message);
			}
		}
	}
}
