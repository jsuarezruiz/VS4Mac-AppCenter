using System;
using System.Collections.Generic;
using System.Linq;
using VS4Mac.AppCenter.Helpers;

namespace VS4Mac.AppCenter.Models
{
	public enum SessionAnalyticsType
	{
		Durations,
		Statistics
	}

	public class SessionAnalytics
	{
		public List<SessionDurationsAnalytics> Durations { get; set; }
		public List<SessionStatisticsAnalytics> Statistics { get; set; }

		public static SessionAnalytics ParseFromString(string[] lines)
		{
			SessionAnalytics sessionAnalytics = new SessionAnalytics
			{
				Durations = new List<SessionDurationsAnalytics>(),
				Statistics = new List<SessionStatisticsAnalytics>()
			};

			if (!lines.Any())
				return sessionAnalytics;

			ErrorHelper.CheckError(lines[0]);

			bool allowAdding = false;
			SessionAnalyticsType sessionAnalyticsType = SessionAnalyticsType.Durations;

			foreach (var line in lines)
			{
				if (line.Equals("Session Durations"))
				{
					sessionAnalyticsType = SessionAnalyticsType.Durations;
				}

				if (line.Equals("Session Statistics"))
				{
					sessionAnalyticsType = SessionAnalyticsType.Statistics;
				}

				allowAdding |= line.StartsWith("├", StringComparison.InvariantCultureIgnoreCase);
				allowAdding &= !line.StartsWith("└", StringComparison.InvariantCultureIgnoreCase);

				if (allowAdding)
				{
					var values = line.Split(new string[] { "│" }, StringSplitOptions.RemoveEmptyEntries);

					switch (sessionAnalyticsType)
					{
						case SessionAnalyticsType.Durations:
							if (values.Length > 1)
							{
								var durationsAnalytics = new SessionDurationsAnalytics
								{
									Description = values[0],
									Count = Convert.ToInt32(values[1])
								};

								sessionAnalytics.Durations.Add(durationsAnalytics);
							}
							break;
						case SessionAnalyticsType.Statistics:
							if (values.Length > 1)
							{
								var statisticsAnalytics = new SessionStatisticsAnalytics
								{
									Description = values[0],
									Count = Convert.ToDouble(values[1]),
									Change = values[2]
								};

								sessionAnalytics.Statistics.Add(statisticsAnalytics);
							}
							break;
					}
				}
			}

			return sessionAnalytics;
		}
	}

	public class SessionDurationsAnalytics
	{
		public string Description { get; set; }
		public int Count { get; set; }
	}

	public class SessionStatisticsAnalytics
	{
		public string Description { get; set; }
		public double Count { get; set; }
		public string Change { get; set; }
	}
}
