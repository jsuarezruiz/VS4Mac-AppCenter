using System;
using System.Collections.Generic;
using System.Linq;
using VS4Mac.AppCenter.Helpers;

namespace VS4Mac.AppCenter.Models
{
	public enum AudienceAnalyticsType
	{
		Devices,
		Countries,
		Languages,
		ActiveUsers
	}

	public class AudienceAnalytics
	{
		public List<DeviceAnalytics> Devices { get; set; }
		public List<CountryAnalytics> Countries { get; set; }
		public List<LanguageAnalytics> Languages { get; set; }
		public List<ActiveUsersAnalytics> ActiveUsers { get; set; }

		public static AudienceAnalytics ParseFromString(string[] lines)
		{
			AudienceAnalytics audienceAnalytics = new AudienceAnalytics
			{
				Devices = new List<DeviceAnalytics>(),
				Countries = new List<CountryAnalytics>(),
				Languages = new List<LanguageAnalytics>(),
				ActiveUsers = new List<ActiveUsersAnalytics>()
			};

			if (!lines.Any())
				return audienceAnalytics;

			ErrorHelper.CheckError(lines[0]);

			bool allowAdding = false;
			AudienceAnalyticsType audienceAnalyticsType = AudienceAnalyticsType.Devices;

			foreach (var line in lines)
			{
				if (line.Equals("Devices"))
				{
					audienceAnalyticsType = AudienceAnalyticsType.Devices;
				}

				if (line.Equals("Countries"))
				{
					audienceAnalyticsType = AudienceAnalyticsType.Countries;
				}

				if (line.Equals("Languages"))
				{
					audienceAnalyticsType = AudienceAnalyticsType.Languages;
				}

				if (line.Equals("Active Users"))
				{
					audienceAnalyticsType = AudienceAnalyticsType.ActiveUsers;
				}

				allowAdding |= line.StartsWith("├", StringComparison.InvariantCultureIgnoreCase);
				allowAdding &= !line.StartsWith("└", StringComparison.InvariantCultureIgnoreCase);

				if(allowAdding)
				{
					var values = line.Split(new string[] { "│" }, StringSplitOptions.RemoveEmptyEntries);

					switch (audienceAnalyticsType)
					{
						case AudienceAnalyticsType.Devices:
							if (values.Length > 1)
							{
								var deviceAnalytics = new DeviceAnalytics
								{
									Device = values[0],
									Count = Convert.ToInt32(values[1]),
									Change = values[2]
								};

								audienceAnalytics.Devices.Add(deviceAnalytics);
							}
							break;
						case AudienceAnalyticsType.Countries:
							if (values.Length > 1)
							{
								var countryAnalytics = new CountryAnalytics
								{
									Country = values[0],
									Count = Convert.ToInt32(values[1]),
									Change = values[2]
								};

								audienceAnalytics.Countries.Add(countryAnalytics);
							}
							break;
						case AudienceAnalyticsType.Languages:
							if (values.Length > 1)
							{
								var languageAnalytics = new LanguageAnalytics
								{
									Language = values[0],
									Count = Convert.ToInt32(values[1]),
									Change = values[2]
								};

								audienceAnalytics.Languages.Add(languageAnalytics);
							}
							break;
						case AudienceAnalyticsType.ActiveUsers:
							if (values.Length > 1)
							{
								var activeUsersAnalytics = new ActiveUsersAnalytics
								{
									Date = values[0],
									Monthly = Convert.ToInt32(values[1]),
									Weekly = Convert.ToInt32(values[2]),
									Daily = Convert.ToInt32(values[3])
								};

								audienceAnalytics.ActiveUsers.Add(activeUsersAnalytics);
							}
							break;
					}
				}
			}

			return audienceAnalytics;
		}
	}

	public class DeviceAnalytics
	{
		public string Device { get; set; }
		public int Count { get; set; }
		public string Change { get; set; }
	}

	public class CountryAnalytics
	{
		public string Country { get; set; }
		public int Count { get; set; }
		public string Change { get; set; }
	}

	public class LanguageAnalytics
	{
		public string Language { get; set; }
		public int Count { get; set; }
		public string Change { get; set; }
	}

	public class ActiveUsersAnalytics
	{
		public string Date { get; set; }
		public int Monthly { get; set; }
		public int Weekly { get; set; }
		public int Daily { get; set; }
	}
}
