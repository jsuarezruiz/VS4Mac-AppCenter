using System;
using System.Threading.Tasks;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using VS4Mac.AppCenter.Controllers;
using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Views.Base;
using Xwt;
using Xwt.Drawing;

namespace VS4Mac.AppCenter.Views
{
	public interface IProfileView : IView
	{

	}

	public class ProfileDialog : Dialog, IProfileView
	{
		VBox _mainBox;
		VBox _contentBox;
		Label _displayNameLabel;
		Label _emailLabel;
		Label _nameLabel;
		HBox _buttonBox;
		Button _cancelButton;
		Button _signOutButton;

		ProfileController _controller;

		public ProfileDialog()
		{
			Init();
			BuildGui();
			AttachEvents();
		}

		void Init()
		{
			_mainBox = new VBox();

			_contentBox = new VBox
			{
				Margin = new WidgetSpacing(12, 24, 12, 24)
			};

			_displayNameLabel = new Label
			{
				Font = Font.SystemFont.WithSize(20),
				HorizontalPlacement = WidgetPlacement.Center
			};

			_emailLabel = new Label
			{
				Font = Font.SystemFont.WithSize(12),
				HorizontalPlacement = WidgetPlacement.Center
			};

			_nameLabel = new Label
			{
				Font = Font.SystemFont.WithSize(12),
				HorizontalPlacement = WidgetPlacement.Center
			};

			_buttonBox = new HBox();
			_cancelButton = new Button("Cancel");

			_signOutButton = new Button("Sign Out")
			{
				BackgroundColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionBackgroundColor,
				LabelColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionTextColor
			};
		}

		void BuildGui()
		{
			Title = "Profile";

			Height = 200;
			Width = 300;

			_contentBox.PackStart(_displayNameLabel);
			_contentBox.PackStart(_emailLabel);
			_contentBox.PackStart(_nameLabel);

			_buttonBox.PackStart(_cancelButton);
			_buttonBox.PackEnd(_signOutButton);

			_mainBox.PackStart(_contentBox);
			_mainBox.PackEnd(_buttonBox);

			Content = _mainBox;
			Resizable = false;
		}

		void AttachEvents()
		{
			_cancelButton.Clicked += OnCancelButtonClicked;
			_signOutButton.Clicked += OnSignOutButtonClicked;
		}

		protected override void Dispose(bool disposing)
		{
			_cancelButton.Clicked -= OnCancelButtonClicked;
			_signOutButton.Clicked -= OnSignOutButtonClicked;
		}

		public void SetController(IController controller)
		{
			_controller = (ProfileController)controller;

			FillData();
		}

		internal void Loading(bool isLoading)
		{
			_signOutButton.Sensitive = !isLoading;
		}

		internal void FillData()
		{
			Loading(true);

			Task.Run(async () =>
			{
				var profile = new Models.Profile();
			
				try
				{
					profile = _controller.GetProfile();
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Load profile failed.", ex);
					MessageService.ShowError("An error ocurred loading the profile.", ex);
				}

				await Runtime.RunInMainThread(() =>
				{
					_displayNameLabel.Text = profile.DisplayName;
					_emailLabel.Text = profile.Email;
					_nameLabel.Text = profile.Username;

					Loading(false);
				});
			});
		}

		void OnCancelButtonClicked(object sender, System.EventArgs e)
		{
			Respond(Command.Close);
			Close();
		}

		void OnSignOutButtonClicked(object sender, System.EventArgs e)
		{
			_controller.Logout();
			Respond(Command.Ok);
			Close();
		}
	}
}
