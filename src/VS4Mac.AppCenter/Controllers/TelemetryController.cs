using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using VS4Mac.AppCenter.Services;
using VS4Mac.AppCenter.Views;

namespace VS4Mac.AppCenter.Controllers
{
	public enum DateRange
	{
		Day,
		Week,
		Month
	}

	public class TelemetryController : Base.IController
	{
		readonly ITelemetryView _view;

		public TelemetryController(ITelemetryView view)
		{
			_view = view;

			DateRange = DateRange.Week;

			view.SetController(this);
		}

		public Models.Application SelectedApplication { get; set; }

		public DateRange DateRange { get; set; }

		public List<Models.Application> LoadApplications()
		{
			return AppCenterService.Instance.GetApplications();
		}

		public Models.AudienceAnalytics LoadAudienceAnalytics(string ownerName, string name)
		{
			Models.AudienceAnalytics audienceAnalytics = new Models.AudienceAnalytics();

			switch(DateRange)
			{
				case DateRange.Day:
					audienceAnalytics = AppCenterService.Instance.GetAudienceAnalytics(ownerName, name, DateTime.Now, DateTime.Now.AddDays(-1));
					break;
				case DateRange.Week:
					audienceAnalytics = AppCenterService.Instance.GetAudienceAnalytics(ownerName, name, DateTime.Now, DateTime.Now.AddDays(-7));
					break;
				case DateRange.Month:
					audienceAnalytics = AppCenterService.Instance.GetAudienceAnalytics(ownerName, name, DateTime.Now, DateTime.Now.AddMonths(-1));
					break;
			}

			return audienceAnalytics;
		}

		public Models.SessionAnalytics LoadSessionAnalytics(string ownerName, string name)
		{
			Models.SessionAnalytics sessionAnalytics = new Models.SessionAnalytics();

			switch (DateRange)
			{
				case DateRange.Day:
					sessionAnalytics = AppCenterService.Instance.GetSessionAnalytics(ownerName, name, DateTime.Now, DateTime.Now.AddDays(-1));
					break;
				case DateRange.Week:
					sessionAnalytics = AppCenterService.Instance.GetSessionAnalytics(ownerName, name, DateTime.Now, DateTime.Now.AddDays(-7));
					break;
				case DateRange.Month:
					sessionAnalytics = AppCenterService.Instance.GetSessionAnalytics(ownerName, name, DateTime.Now, DateTime.Now.AddMonths(-1));
					break;
			}

			return sessionAnalytics;
		}

		public PlotModel CreatePiePlotModel(Models.AudienceAnalytics audienceAnalytics, Models.AudienceAnalyticsType audienceAnalyticsType)
		{
			var model = new PlotModel
			{
				LegendPosition = LegendPosition.BottomCenter,
				PlotAreaBorderColor = OxyColors.Transparent
			};

			var series = new PieSeries
			{
				StrokeThickness = 2.0,
				InsideLabelPosition = 0.8,
 				InnerDiameter = 0.4,
				AngleSpan = 360,
				StartAngle = 0
			};

			switch (audienceAnalyticsType)
			{
				case Models.AudienceAnalyticsType.Countries:
					model.DefaultColors = new List<OxyColor>
					{
						OxyColor.Parse("#266489"),
						OxyColor.Parse("#68B9C0"),
						OxyColor.Parse("#90D585")
					};

					foreach (var country in audienceAnalytics.Countries)
					{
						series.Slices.Add(new PieSlice(country.Country, country.Count) { IsExploded = true });
					}
					break;
				case Models.AudienceAnalyticsType.Devices:
					model.DefaultColors = new List<OxyColor>
					{
						OxyColor.Parse("#97A69D"),
						OxyColor.Parse("#A65B69"),
						OxyColor.Parse("#DABFAF")
					};

					foreach (var device in audienceAnalytics.Devices)
					{
						series.Slices.Add(new PieSlice(device.Device, device.Count) { IsExploded = true });
					}
					break;
				case Models.AudienceAnalyticsType.Languages:
					model.DefaultColors = new List<OxyColor>
					{
						OxyColor.Parse("#8F97A4"),
						OxyColor.Parse("#DAC096"),
						OxyColor.Parse("#76846E")
					};

					foreach (var language in audienceAnalytics.Languages)
					{
						series.Slices.Add(new PieSlice(language.Language, language.Count) { IsExploded = true });
					}
					break;
			}

			model.Series.Add(series);

			return model;
		}

		public PlotModel CreateBarPlotModel(Models.AudienceAnalytics audienceAnalytics)
		{
			var model = new PlotModel
			{
				LegendPlacement = LegendPlacement.Outside,
				LegendPosition = LegendPosition.BottomCenter,
				LegendOrientation = LegendOrientation.Horizontal,
				LegendBorderThickness = 0
			};

			model.DefaultColors = new List<OxyColor>
			{
				OxyColor.Parse("#97A69D"),
				OxyColor.Parse("#A65B69"),
				OxyColor.Parse("#DABFAF")
			};

			var dailySeries = new BarSeries { Title = "Daily", StrokeColor = OxyColor.Parse("#006594") };
	
			foreach (var user in audienceAnalytics.ActiveUsers)
			{
				dailySeries.Items.Add(new BarItem(user.Daily));
			}

			var weeklySeries = new BarSeries { Title = "Weekly", StrokeColor = OxyColor.Parse("#c262af") };

			foreach (var user in audienceAnalytics.ActiveUsers)
			{
				weeklySeries.Items.Add(new BarItem(user.Weekly));
			}

			var monthlySeries = new BarSeries { Title = "Monthly", StrokeColor = OxyColor.Parse("#ffa600") };

			foreach (var user in audienceAnalytics.ActiveUsers)
			{
				monthlySeries.Items.Add(new BarItem(user.Monthly));
			}

			model.Series.Add(dailySeries);
			model.Series.Add(weeklySeries);
			model.Series.Add(monthlySeries);

			var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };

			categoryAxis.IsZoomEnabled = false;
			categoryAxis.IsPanEnabled = false;

			foreach (var user in audienceAnalytics.ActiveUsers)
			{
				var time = "01:00:00";
				var date = user.Date.Substring(0, user.Date.IndexOf(time, StringComparison.InvariantCultureIgnoreCase));
				categoryAxis.Labels.Add(date);
			}

			model.Axes.Add(categoryAxis);

			return model;
		}
	}
}