using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using OxyPlot.GtkSharp;
using VS4Mac.AppCenter.Controllers;
using VS4Mac.AppCenter.Controllers.Base;
using VS4Mac.AppCenter.Extensions;
using VS4Mac.AppCenter.Views.Base;
using Xwt;
using Xwt.Drawing;

namespace VS4Mac.AppCenter.Views
{
	public interface ITelemetryView : IView
	{

	}

	public class TelemetryView : AbstractXwtViewContent, ITelemetryView
	{
		VBox _mainBox;
		HBox _headerBox;
		Label _titleLabel;
		Label _appNameLabel;
		HBox _rangeBox;
		ToggleButton _lastDayToggle;
		ToggleButton _lastWeekToggle;
		ToggleButton _lastMonthToggle;
		ComboBox _appsCombo;
		Separator _separator;
		VBox _contentBox;
		MDSpinner _contentSpinner;
		ScrollView _contentScroll;
		VBox _contentScrollBox;
		HBox _firstBox;
		RoundedFrameBox _devicesFrameBox;
		VBox _devicesBox;
		Label _devicesTitleLabel;
		PlotView _devicesPlotView;
		RoundedFrameBox _countriesFrameBox;
		VBox _countriesBox;
		Label _countriesTitleLabel;
		PlotView _countriesPlotView;
		RoundedFrameBox _languagesFrameBox;
		VBox _languagesBox;
		Label _languagesTitleLabel;
		PlotView _languagesPlotView;
		HBox _secondBox;
		RoundedFrameBox _usersFrameBox;
		VBox _usersBox;
		Label _usersTitleLabel;
		PlotView _usersPlotView;
		HBox _thirdBox;
		RoundedFrameBox _durationsFrameBox;
		VBox _durationsBox;
		Label _durationsTitleLabel;
		ListView _durationsView;
		DataField<string> _descriptionField;
		DataField<int> _countField;
		ListStore _durationsStore;
		RoundedFrameBox _statisticsFrameBox;
		VBox _statisticsBox;
		Label _statisticsTitleLabel;
		ListView _statisticsView;
		DataField<string> _statisticsDescriptionField;
		DataField<double> _statisticsCountField;
		DataField<string> _statisticsChangeField;
		ListStore _statisticsStore;

		TelemetryController _controller;

		public TelemetryView()
		{
			Init();
			BuildGui();
			AttachEvents();
		}

		public override Widget Widget => _mainBox;

		public override bool IsViewOnly
		{
			get
			{
				return true;
			}
		}

		public override bool IsFile
		{
			get
			{
				return false;
			}
		}

		void Init()
		{
			_mainBox = new VBox();

			_headerBox = new HBox
			{
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_appNameLabel = new Label
			{
				Font = Font.SystemFont.WithSize(24)
			};

			_titleLabel = new Label("Dashboard")
			{
				Font = Font.SystemFont.WithSize(24)
			};

			_rangeBox = new HBox
			{
				HorizontalPlacement = WidgetPlacement.Center
			};

			_lastDayToggle = new ToggleButton("Today");

			_lastWeekToggle = new ToggleButton("Last Week")
			{
				Active = true
			};

			_lastMonthToggle = new ToggleButton("Last Month");

			_appsCombo = new ComboBox
			{
				VerticalPlacement = WidgetPlacement.Center,
				WidthRequest = 250
			};

			_separator = new HSeparator();

			_contentBox = new VBox();

			_contentSpinner = new MDSpinner
			{
				Animate = true,
				Visible = false
			};

			_contentScroll = new ScrollView
			{
				BorderVisible = false
			};

			_contentScrollBox = new VBox();

			_firstBox = new HBox();

			_devicesFrameBox = new RoundedFrameBox
			{
				BackgroundColor = Styles.BaseBackgroundColor,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_devicesBox = new VBox
			{
				HeightRequest = 300
			};

			_devicesTitleLabel = new Label("Devices")
			{
				Font = Font.SystemFont.WithSize(14),
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_devicesPlotView = new PlotView
			{
				BorderWidth = 0,
				HeightRequest = 200,
				WidthRequest = 200,
				Visible = true
			};

			_devicesPlotView.ModifyBg(Gtk.StateType.Normal, Styles.BaseBackgroundColor.ToGdkColor());

			_countriesFrameBox = new RoundedFrameBox
			{
				BackgroundColor = Styles.BaseBackgroundColor,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_countriesBox = new VBox
			{
				HeightRequest = 300
			};

			_countriesTitleLabel = new Label("Countries")
			{
				Font = Font.SystemFont.WithSize(14),
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_countriesPlotView = new PlotView
			{
				BorderWidth = 0,
				HeightRequest = 200,
				WidthRequest = 200,
				Visible = true
			};

			_countriesPlotView.ModifyBg(Gtk.StateType.Normal, Styles.BaseBackgroundColor.ToGdkColor());

			_languagesFrameBox = new RoundedFrameBox
			{
				BackgroundColor = Styles.BaseBackgroundColor,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_languagesBox = new VBox
			{
				HeightRequest = 300
			};

			_languagesTitleLabel = new Label("Languages")
			{
				Font = Font.SystemFont.WithSize(14),
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_languagesPlotView = new PlotView
			{
				BorderWidth = 0,
				HeightRequest = 200,
				WidthRequest = 200,
				Visible = true
			};

			_languagesPlotView.ModifyBg(Gtk.StateType.Normal, Styles.BaseBackgroundColor.ToGdkColor());

			_secondBox = new HBox();

			_usersFrameBox = new RoundedFrameBox
			{
				BackgroundColor = Styles.BaseBackgroundColor,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_usersBox = new VBox
			{
				HeightRequest = 600
			};

			_usersTitleLabel = new Label("Active Users")
			{
				Font = Font.SystemFont.WithSize(14),
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_usersPlotView = new PlotView
			{
				Visible = true
			};

			_usersPlotView.ModifyBg(Gtk.StateType.Normal, Styles.BaseBackgroundColor.ToGdkColor());

			_thirdBox = new HBox();

			_durationsFrameBox = new RoundedFrameBox
			{
				BackgroundColor = Styles.BaseBackgroundColor,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_durationsBox = new VBox();

			_durationsTitleLabel = new Label("Session Durations")
			{
				Font = Font.SystemFont.WithSize(14),
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_durationsView = new ListView
			{
				SelectionMode = SelectionMode.None,
				HeightRequest = 200,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_descriptionField = new DataField<string>();
			_countField = new DataField<int>();

			_durationsStore = new ListStore(_descriptionField, _countField);

			_durationsView.Columns.Add(new ListViewColumn("", new TextCellView(_descriptionField) { Editable = false }));
			_durationsView.Columns.Add(new ListViewColumn("Count", new TextCellView(_countField) { Editable = false }));

			_durationsView.DataSource = _durationsStore;

			_statisticsFrameBox = new RoundedFrameBox
			{
				BackgroundColor = Styles.BaseBackgroundColor,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_statisticsBox = new VBox();

			_statisticsTitleLabel = new Label("Session Statistics")
			{
				Font = Font.SystemFont.WithSize(14),
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_statisticsView = new ListView
			{
				SelectionMode = SelectionMode.None,
				HeightRequest = 100,
				Margin = new WidgetSpacing(6, 6, 6, 6)
			};

			_statisticsDescriptionField = new DataField<string>();
			_statisticsCountField = new DataField<double>();
			_statisticsChangeField = new DataField<string>();

			_statisticsStore = new ListStore(_statisticsDescriptionField, _statisticsCountField, _statisticsChangeField);

			_statisticsView.Columns.Add(new ListViewColumn("", new TextCellView(_statisticsDescriptionField) { Editable = false }));
			_statisticsView.Columns.Add(new ListViewColumn("Count", new TextCellView(_statisticsCountField) { Editable = false }));
			_statisticsView.Columns.Add(new ListViewColumn("Change", new TextCellView(_statisticsChangeField) { Editable = false }));

			_statisticsView.DataSource = _statisticsStore;
		}

		void BuildGui()
		{
			ContentName = "Analytics";

			_headerBox.PackStart(_appNameLabel);
			_headerBox.PackStart(_titleLabel);
			_headerBox.PackEnd(_appsCombo);

			_rangeBox.PackStart(_lastDayToggle);
			_rangeBox.PackStart(_lastWeekToggle);
			_rangeBox.PackStart(_lastMonthToggle);

			_devicesBox.PackStart(_devicesTitleLabel);
			var xwtDevicesPlotView = Toolkit.CurrentEngine.WrapWidget(_devicesPlotView);
			_devicesBox.PackStart(xwtDevicesPlotView, true);

			_countriesBox.PackStart(_countriesTitleLabel);
			var xwtCountriesPlotView = Toolkit.CurrentEngine.WrapWidget(_countriesPlotView);
			_countriesBox.PackStart(xwtCountriesPlotView, true);

			_languagesBox.PackStart(_languagesTitleLabel);
			var xwtLanguagesPlotView = Toolkit.CurrentEngine.WrapWidget(_languagesPlotView);
			_languagesBox.PackStart(xwtLanguagesPlotView, true);

			_usersBox.PackStart(_usersTitleLabel);
			var xwtUsersPlotView = Toolkit.CurrentEngine.WrapWidget(_usersPlotView);
			_usersBox.PackStart(xwtUsersPlotView, true);

			_devicesFrameBox.Content = _devicesBox;
			_countriesFrameBox.Content = _countriesBox;
			_languagesFrameBox.Content = _languagesBox;
			_usersFrameBox.Content = _usersBox;

			_firstBox.PackStart(_devicesFrameBox, true);
			_firstBox.PackStart(_countriesFrameBox, true);
			_firstBox.PackStart(_languagesFrameBox, true);

			_secondBox.PackStart(_usersFrameBox, true);

			_durationsBox.PackStart(_durationsTitleLabel);
			_durationsBox.PackStart(_durationsView);

			_statisticsBox.PackStart(_statisticsTitleLabel);
			_statisticsBox.PackStart(_statisticsView);

			_durationsFrameBox.Content = _durationsBox;
			_statisticsFrameBox.Content = _statisticsBox;

			_thirdBox.PackStart(_durationsFrameBox, true);
			_thirdBox.PackStart(_statisticsFrameBox, true);

			_contentScrollBox.PackStart(_firstBox);
			_contentScrollBox.PackStart(_secondBox);
			_contentScrollBox.PackStart(_thirdBox);

			_contentScroll.Content = _contentScrollBox;

			_contentBox.PackStart(_contentScroll, true);
			_contentBox.PackStart(_contentSpinner, true, WidgetPlacement.Center);

			_mainBox.PackStart(_headerBox);
			_mainBox.PackStart(_rangeBox);
			_mainBox.PackStart(_separator);
			_mainBox.PackEnd(_contentBox, true);
		}

		void AttachEvents()
		{
			_appsCombo.SelectionChanged += AppsComboSelectionChanged;
			_lastDayToggle.Clicked += OnLastDayToggleToggled;
			_lastWeekToggle.Clicked += OnLastWeekToggleToggled;
			_lastMonthToggle.Clicked += OnLastMonthToggleToggled;
		}

		public void SetController(IController controller)
		{
			_controller = (TelemetryController)controller;

			FillApplications();
		}

		internal void FillApplications()
		{
			Task.Run(async () =>
			{
				// Load the applications
				var applications = new List<Models.Application>();

				try
				{
					applications = _controller.LoadApplications();
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Load applications failed.", ex);
					MessageService.ShowError("An error ocurred loading the applications.", ex);
				}

				await Runtime.RunInMainThread(() =>
				{
					_appsCombo.Items.Clear();

					foreach (var application in applications)
						_appsCombo.Items.Add(application, application.Name);

					_appsCombo.SelectedIndex = 0;
				});
			});
		}

		internal void IsLoading(bool isLoading)
		{
			_appsCombo.Sensitive = !isLoading;
			_lastDayToggle.Sensitive = !isLoading;
			_lastWeekToggle.Sensitive = !isLoading;
			_lastMonthToggle.Sensitive = !isLoading;
			_contentSpinner.Visible = isLoading;
			_contentScroll.Visible = !isLoading;
		}

		internal void FillAnalytics()
		{
			if (_controller.SelectedApplication == null)
				return;

			IsLoading(true);

			_appNameLabel.Text = _controller.SelectedApplication.Name.UppercaseFirst();

			Task.Run(async () =>
			{
				var audienceAnalytics = _controller.LoadAudienceAnalytics(_controller.SelectedApplication.OwnerName, _controller.SelectedApplication.Name);
				var sessionAnalytics = _controller.LoadSessionAnalytics(_controller.SelectedApplication.OwnerName, _controller.SelectedApplication.Name);

				await Runtime.RunInMainThread(() =>
				{
					_devicesPlotView.Model = _controller.CreatePiePlotModel(audienceAnalytics, Models.AudienceAnalyticsType.Devices);
					_countriesPlotView.Model = _controller.CreatePiePlotModel(audienceAnalytics, Models.AudienceAnalyticsType.Countries);
					_languagesPlotView.Model = _controller.CreatePiePlotModel(audienceAnalytics, Models.AudienceAnalyticsType.Languages);
					_usersPlotView.Model = _controller.CreateBarPlotModel(audienceAnalytics);

					_devicesPlotView.SetSizeRequest(200, 200);
					_devicesPlotView.ShowAll();

					_countriesPlotView.SetSizeRequest(200, 200);
					_countriesPlotView.ShowAll();

					_devicesPlotView.SetSizeRequest(200, 200);
					_devicesPlotView.ShowAll();

					_usersPlotView.ShowAll();

					_durationsStore.Clear();

					foreach (var item in sessionAnalytics.Durations)
					{
						var row = _durationsStore.AddRow();
						_durationsStore.SetValue(row, _descriptionField, item.Description);
						_durationsStore.SetValue(row, _countField, item.Count);
					}

					_statisticsStore.Clear();

					foreach (var item in sessionAnalytics.Statistics)
					{
						var row = _statisticsStore.AddRow();
						_statisticsStore.SetValue(row, _statisticsDescriptionField, item.Description);
						_statisticsStore.SetValue(row, _statisticsCountField, item.Count);
						_statisticsStore.SetValue(row, _statisticsChangeField, item.Change);
					}

					IsLoading(false);
				});
			});
		}

		internal void RefreshData()
		{
			FillAnalytics();
		}

		void AppsComboSelectionChanged(object sender, System.EventArgs e)
		{
			_controller.SelectedApplication = _appsCombo.SelectedItem as Models.Application;

			RefreshData();
		}

		void OnLastDayToggleToggled(object sender, System.EventArgs e)
		{
			_controller.DateRange = DateRange.Day;
			_usersBox.HeightRequest = 300;
			UpdateRangeButtons();
			RefreshData();
		}

		void OnLastWeekToggleToggled(object sender, System.EventArgs e)
		{
			_controller.DateRange = DateRange.Week;
			_usersBox.HeightRequest = 600;
			UpdateRangeButtons();
			RefreshData();
		}

		void OnLastMonthToggleToggled(object sender, System.EventArgs e)
		{
			_controller.DateRange = DateRange.Month;
			_usersBox.HeightRequest = 1800;
			UpdateRangeButtons();
			RefreshData();
		}

		void UpdateRangeButtons()
		{
			_lastDayToggle.Active = false;
			_lastWeekToggle.Active = false;
			_lastMonthToggle.Active = false;

			switch (_controller.DateRange)
			{
				case DateRange.Day:
					_lastDayToggle.Active = true;
					break;
				case DateRange.Week:
					_lastWeekToggle.Active = true;
					break;
				case DateRange.Month:
					_lastMonthToggle.Active = true;
					break;
			}
		}
	}
}
