using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using VS4Mac.AppCenter.Controllers;
using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Views.Base;
using Xwt;

namespace VS4Mac.AppCenter.Views
{
	public interface IApplicationsView : IView
	{

	}

	public class ApplicationsDialog : Xwt.Dialog, IApplicationsView
	{
		Label _titleLabel;
		ListView _listView;
		ListStore _listStore;
		DataField<string> _nameField;
		DataField<string> _ownerField;
		DataField<Models.Application> _appField;
		HBox _buttonBox;
		Button _closeButton;
		Button _addButton;
		Button _editButton;
		Button _removeButton;
		MDSpinner _appsSpinner;

		ApplicationsController _controller;

		internal ApplicationsDialog()
		{
			Init();
			BuildGui();
			AttachEvents();
		}

		void Init()
		{
			_titleLabel = new Label("Applications")
			{
				Margin = new WidgetSpacing(0, 6, 0, 6)
			};

			_listView = new ListView
			{
				MinHeight = 150,
				WidthRequest = 400
			};

			_nameField = new DataField<string>();
			_ownerField = new DataField<string>();
			_appField = new DataField<Models.Application>();

			_listStore = new ListStore(_nameField, _ownerField, _appField);

			_listView.Columns.Add(new ListViewColumn("Name", new TextCellView(_nameField)));
			_listView.Columns.Add(new ListViewColumn("Owner", new TextCellView(_ownerField)));

			_listView.DataSource = _listStore;

			_buttonBox = new HBox();

			_addButton = new Button("Add")
			{
				BackgroundColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionBackgroundColor,
				LabelColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionTextColor
			};

			_editButton = new Button("Edit");
			_removeButton = new Button("Remove");
			_closeButton = new Button("Close");

			_appsSpinner = new MDSpinner(Gtk.IconSize.Dialog)
			{
				HeightRequest = 24,
				WidthRequest = 24,
				Animate = true,
				HorizontalPlacement = WidgetPlacement.Center,
				VerticalPlacement = WidgetPlacement.Center
			};
		}

		void BuildGui()
		{
			Title = "App Center Applications";

			Height = 200;
			Width = 450;

			VBox content = new VBox();

			content.PackStart(_titleLabel);

			VBox mainBox = new VBox
			{
				HeightRequest = 150
			};

			mainBox.PackStart(_listView);
			mainBox.PackStart(_appsSpinner, true, WidgetPlacement.Center);

			content.PackStart(mainBox);

			_buttonBox.PackStart(_closeButton);
			_buttonBox.PackEnd(_addButton);
			_buttonBox.PackEnd(_editButton);
			_buttonBox.PackEnd(_removeButton);

			content.PackStart(_buttonBox);

			Content = content;
			Resizable = false;
		}

		void AttachEvents()
		{
			_listView.SelectionChanged += OnListViewSelectionChanged; 
			_addButton.Clicked += OnAdd;
			_editButton.Clicked += OnEdit;
			_removeButton.Clicked += OnRemove;
			_closeButton.Clicked += OnClose;
		}

		protected override void Dispose(bool disposing)
		{
			_listView.SelectionChanged -= OnListViewSelectionChanged;
			_addButton.Clicked -= OnAdd;
			_editButton.Clicked -= OnEdit;
			_removeButton.Clicked -= OnRemove;
			_closeButton.Clicked -= OnClose;

			if (_controller != null)
				_controller.ApplicationsChanged -= _OnApplicationsChanged;

			base.Dispose(disposing);
		}

		public void SetController(IController controller)
		{
			_controller = (ApplicationsController)controller;

			_controller.ApplicationsChanged += _OnApplicationsChanged;

			FillData();
		}

		void _OnApplicationsChanged(object sender, EventArgs e)
		{
			RefreshData();
		}

		internal void Loading(bool isLoading)
		{
			_appsSpinner.Visible = isLoading;
			_titleLabel.Sensitive = !isLoading;
			_listView.Visible = !isLoading;
			_addButton.Sensitive = !isLoading;
			_editButton.Sensitive = !isLoading;
			_removeButton.Sensitive = !isLoading;
			_closeButton.Sensitive = !isLoading;
		}

		internal void FillData()
		{
			Loading(true);

			_listStore.Clear();

			Task.Run(async () =>
			{
				var apps = new List<Models.Application>();

				try
				{
					apps = _controller.LoadApplications();
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Load applications failed.", ex);
					MessageService.ShowError("An error ocurred loading the applications.",ex);
				}

				await Runtime.RunInMainThread(() =>
				{
					if (apps.Any())
					{
						foreach (var app in apps)
						{
							var row = _listStore.AddRow();
							_listStore.SetValue(row, _nameField, app.Name);
							_listStore.SetValue(row, _ownerField, app.OwnerName);
							_listStore.SetValue(row, _appField, app);
						}

						if(apps.Any())
							_listView.SelectRow(0);
					}

					Loading(false);
				});
			});
		}

		internal void RefreshData()
		{
			FillData();
		}

		void OpenApplicationDetails(Models.Application application = null)
		{
			var applicationDetailsDialog = new ApplicationDetailsDialog();
			var applicationDetailsController = new ApplicationDetailsController(applicationDetailsDialog, application);
			applicationDetailsDialog.Run(MessageDialog.RootWindow);
		}

		void OnListViewSelectionChanged(object sender, EventArgs e)
		{
			var row = _listView.SelectedRow;

			if (row == -1)
			{
				return;
			}

			var selectedApp = _listStore.GetValue(row, _appField);

			if (selectedApp is Models.Application application)
			{
				_controller.SelectedApplication = application;
			}
		}

		void OnAdd(object sender, EventArgs args)
		{
			OpenApplicationDetails();
		}

		void OnEdit(object sender, EventArgs args)
		{
			OpenApplicationDetails(_controller.SelectedApplication);
		}

		void OnRemove(object sender, EventArgs args)
		{
			if (_controller.SelectedApplication != null)
			{
				if (MessageService.Confirm("Are you sure to delete the application?", AlertButton.Delete))
				{
					_controller.DeleteApplication(_controller.SelectedApplication.OwnerName, _controller.SelectedApplication.Name);
				}
			}
		}

		void OnClose(object sender, EventArgs args)
		{
			Respond(Command.Close);
			Close();
		}
	}
}