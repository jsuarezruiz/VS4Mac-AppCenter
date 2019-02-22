using VS4Mac.AppCenter.Controllers;
using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Views.Base;
using Xwt;

namespace VS4Mac.AppCenter.Views
{
	public interface IApplicationsDetailsView : IView
	{

	}

	public class ApplicationDetailsDialog : Dialog, IApplicationsDetailsView
	{
		const string DefaultPlatform = "Xamarin";
		const string DefaultOs = "Android";

		VBox _mainBox;
		HBox _nameBox;
		Label _nameLabel;
		TextEntry _nameEntry;
		HBox _displayNameBox;
		Label _displayNameLabel;
		TextEntry _displayNameEntry;
		HBox _platformBox;
		Label _platformLabel;
		ComboBox _platformCombo;
		HBox _osBox;
		Label _osLabel;
		ComboBox _osCombo;
		HBox _descriptionBox;
		Label _descriptionLabel;
		TextEntry _descriptionEntry;
		HBox _ownerNameBox;
		Label _ownerNameLabel;
		TextEntry _ownerNameEntry;
		HBox _ownerEmailBox;
		Label _ownerEmailLabel;
		TextEntry _ownerEmailEntry;
		HBox _buttonBox;
		Button _cancelButton;
		Button _okButton;
		ApplicationDetailsController _controller;

		public ApplicationDetailsDialog()
		{
			Init();
			BuildGui();
			AttachEvents();
		}

		void Init()
		{
			_mainBox = new VBox();
			_nameBox = new HBox();

			_nameLabel = new Label("Name")
			{
				HorizontalPlacement = WidgetPlacement.End,
				WidthRequest = 80
			};

			_nameEntry = new TextEntry();

			_displayNameBox = new HBox();

			_displayNameLabel = new Label("Display Name")
			{
				HorizontalPlacement = WidgetPlacement.End,
				WidthRequest = 80
			};

			_displayNameEntry = new TextEntry();

			_platformBox = new HBox();

			_platformLabel = new Label("Platform")
			{
				HorizontalPlacement = WidgetPlacement.End,
				WidthRequest = 80
			};

			_platformCombo = new ComboBox();

			_osBox = new HBox();

			_osLabel = new Label("Os")
			{
				HorizontalPlacement = WidgetPlacement.End,
				WidthRequest = 80
			};

			_osCombo = new ComboBox();

			_descriptionBox = new HBox();

			_descriptionLabel = new Label("Description")
			{
				HorizontalPlacement = WidgetPlacement.End,
				WidthRequest = 80
			};

			_descriptionEntry = new TextEntry();

			_ownerNameBox = new HBox();

			_ownerNameLabel = new Label("Owner Name")
			{
				HorizontalPlacement = WidgetPlacement.End,
				WidthRequest = 80
			};

			_ownerNameEntry = new TextEntry();

			_ownerEmailBox = new HBox();

			_ownerEmailLabel = new Label("Owner Email")
			{
				HorizontalPlacement = WidgetPlacement.End,
				WidthRequest = 80
			};

			_ownerEmailEntry = new TextEntry();

			_buttonBox = new HBox();
			_cancelButton = new Button("Cancel");

			_okButton = new Button("Ok")
			{
				BackgroundColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionBackgroundColor,
				LabelColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionTextColor
			};
		}

		void BuildGui()
		{
			Title = "App Center Application Details";

			Height = 300;
			Width = 450;

			_nameBox.PackStart(_nameLabel);
			_nameBox.PackEnd(_nameEntry, true);

			_displayNameBox.PackStart(_displayNameLabel);
			_displayNameBox.PackEnd(_displayNameEntry, true);

			_platformBox.PackStart(_platformLabel);
			_platformBox.PackEnd(_platformCombo, true);

			_osBox.PackStart(_osLabel);
			_osBox.PackEnd(_osCombo, true);

			_descriptionBox.PackStart(_descriptionLabel);
			_descriptionBox.PackEnd(_descriptionEntry, true);

			_ownerNameBox.PackStart(_ownerNameLabel);
			_ownerNameBox.PackEnd(_ownerNameEntry, true);

			_ownerEmailBox.PackStart(_ownerEmailLabel);
			_ownerEmailBox.PackEnd(_ownerEmailEntry, true);

			_buttonBox.PackStart(_cancelButton);
			_buttonBox.PackEnd(_okButton);

			_mainBox.PackStart(_nameBox);
			_mainBox.PackStart(_displayNameBox);
			_mainBox.PackStart(_platformBox);
			_mainBox.PackStart(_osBox);
			_mainBox.PackStart(_descriptionBox);
			_mainBox.PackStart(_ownerNameBox);
			_mainBox.PackStart(_ownerEmailBox);
			_mainBox.PackEnd(_buttonBox);

			Content = _mainBox;
			Resizable = false;
		}

		void AttachEvents()
		{
			_cancelButton.Clicked += OnCancelButtonClicked;
			_okButton.Clicked += OnOkButtonClicked;
		}

		protected override void Dispose(bool disposing)
		{
			_cancelButton.Clicked -= OnCancelButtonClicked;
			_okButton.Clicked -= OnOkButtonClicked;
		}

		public void SetController(IController controller)
		{
			_controller = (ApplicationDetailsController)controller;

			FillApplicationDetails();
		}

		internal void FillApplicationDetails()
		{
			var application = _controller.LoadApplicationDetails();
			var platforms = _controller.LoadPlatforms();
			var os = _controller.LoadOS();

			foreach (var platform in platforms)
				_platformCombo.Items.Add(platform);

			_platformCombo.SelectedItem = DefaultPlatform;

			foreach (var s in os)
				_osCombo.Items.Add(s);

			_osCombo.SelectedItem = DefaultOs;

			if (application != null)
			{
				_nameEntry.Text = application.Name;
				_displayNameEntry.Text = application.DisplayName;
				_platformCombo.SelectedItem = application.Platform;
				_osCombo.SelectedItem = application.OS;
				_descriptionEntry.Text = application.Description;
				_ownerNameEntry.Text = application.OwnerName;
				_ownerEmailEntry.Text = application.OwnerEmail;

				_platformCombo.Sensitive = false;
				_osCombo.Sensitive = false;
				_ownerNameEntry.Sensitive = false;
				_ownerEmailEntry.Sensitive = false;
			}
			else
			{
				Height = 150;
				_nameBox.Visible = false;
				_descriptionBox.Visible = false;
				_ownerNameBox.Visible = false;
				_ownerEmailBox.Visible = false;
			}
		}

		void OnCancelButtonClicked(object sender, System.EventArgs e)
		{
			Respond(Command.Cancel);
			Close();
		}

		void OnOkButtonClicked(object sender, System.EventArgs e)
		{
			if(_controller.CurrentApplication == null)
			{
				_controller.CreateApplication(
					_displayNameEntry.Text, 
					_osCombo.SelectedItem.ToString(), 
					_platformCombo.SelectedItem.ToString());
			}
			else
			{
				_controller.UpdateApplication(
					_ownerNameEntry.Text,
					_nameEntry.Text,
					_displayNameEntry.Text,
					_descriptionEntry.Text);
			}

			Respond(Command.Ok);
			Close();
		}
	}
}