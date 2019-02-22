using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VS4Mac.AppCenter.Helpers;
using VS4Mac.AppCenter.Models;

namespace VS4Mac.AppCenter.Services
{
	public class AppCenterService
	{
		private static AppCenterService _instance;

		public event EventHandler<EventArgs> ApplicationsChanged;

		public static AppCenterService Instance
		{
			get
			{
				if (_instance == null)
					_instance = new AppCenterService();

				return _instance;
			}
		}

		const string Tool = "appcenter";
		const string AnalyticsCommand = "analytics"; 
		const string AppsCommand = "apps";
		const string ProfileCommand = "profile";

		public List<string> GetOS()
		{
			return new List<string>
			{
				"iOS",
				"Android",
				"Windows",
				"macOS"
			};
		}

		public List<string> GetPlatforms()
		{
			return new List<string>
			{
				"Objective-C / Swift",
				"React Native",
				"Cordova",
				"Xamarin",
				"Unity"
			};
		}

		public List<Application> GetApplications()
		{
			List<Application> applications = new List<Application>();

			var command = CreateCommand(AppsCommand, "list");
			var result = BashHelper.ExecuteBashCommand(command);
			string[] lines = Regex.Split(result.Output, "\n");

			foreach (var line in lines)
				if(!string.IsNullOrEmpty(line))
					applications.Add(Application.ParseFromString(line));

			return applications;
		}

		public Application GetApplicationDetails(string applicationOwner, string applicationName)
		{
			Application application = new Application();

			var command = CreateCommand(AppsCommand, $"show --app \"{applicationOwner}/{applicationName}\"");
			var result = BashHelper.ExecuteBashCommand(command);
			string[] lines = Regex.Split(result.Output, "\n");

			application = Application.ParseFromString(lines);

			return application;
		}

		public Application CreateApplication(string displayName, string os, string platform)
		{
			Application application = new Application();

			var command = CreateCommand(AppsCommand, $"create -d \"{displayName}\"  -o \"{os}\"  -p \"{platform}\"");
			var result = BashHelper.ExecuteBashCommand(command);
			string[] lines = Regex.Split(result.Output, "\n");

			application = Application.ParseFromString(lines);

			ApplicationsChanged?.Invoke(this, new EventArgs());

			return application;
		}

		public Application UpdateApplication(string ownerName, string name, string displayName, string description)
		{
			Application application = new Application();

			var command = CreateCommand(AppsCommand, $"update --app \"{ownerName}/{name}\"  -n \"{name}\" -d \"{displayName}\" -description \"{description}\"");
			var result = BashHelper.ExecuteBashCommand(command);
			string[] lines = Regex.Split(result.Output, "\n");

			application = Application.ParseFromString(lines);

			ApplicationsChanged?.Invoke(this, new EventArgs());

			return application;
		}

		public void DeleteApplication(string ownerName, string name)
		{
			var command = CreateCommand(AppsCommand, $"delete --app \"{ownerName}/{name}\"");
			BashHelper.ExecuteBashCommandWithConfirmation(command);
			ApplicationsChanged?.Invoke(this, new EventArgs());
		}

		public AudienceAnalytics GetAudienceAnalytics(string ownerName, string name, DateTime startDate, DateTime endDate)
		{
			var command = CreateCommand(AnalyticsCommand, $"audience --app \"{ownerName}/{name}\"  -s \"{endDate.ToShortDateString()}\"  -e \"{startDate.ToShortDateString()}\"");
			var result = BashHelper.ExecuteBashCommand(command);
			string[] lines = Regex.Split(result.Output, "\n");

			return AudienceAnalytics.ParseFromString(lines);
		}

		public SessionAnalytics GetSessionAnalytics(string ownerName, string name, DateTime startDate, DateTime endDate)
		{
			var command = CreateCommand(AnalyticsCommand, $"sessions --app \"{ownerName}/{name}\"  -s \"{endDate.ToShortDateString()}\"  -e \"{startDate.ToShortDateString()}\"");
			var result = BashHelper.ExecuteBashCommand(command);
			string[] lines = Regex.Split(result.Output, "\n");

			return SessionAnalytics.ParseFromString(lines);
		}

		public Profile GetProfile()
		{
			var command = CreateCommand(ProfileCommand, $"list");
			var result = BashHelper.ExecuteBashCommand(command);
			string[] lines = Regex.Split(result.Output, "\n");

			return Profile.ParseFromString(lines);
		}

		public void Logout()
		{
			var command = CreateCommand(ProfileCommand, $"logout");
			BashHelper.ExecuteBashCommand(command);
		}

		internal string CreateCommand(string command, string parameters)
		{
			return $"{Tool} {command} {parameters}";
		}
	}
}