using System.Linq;
using VS4Mac.AppCenter.Helpers;

namespace VS4Mac.AppCenter.Models
{
	public class Application
	{
		public string AppSecret { get; set; }
		public string Description { get; set; }
		public string DisplayName { get; set; }
		public string Name { get; set; }
		public string OS { get; set; }
		public string Platform { get; set; }
		public string OwnerId { get; set; }
		public string OwnerDisplayName { get; set; }
		public string OwnerEmail { get; set; }
		public string OwnerName { get; set; }
		public string AzureSubscriptionId { get; set; }

		public static Application ParseFromString(string value)
		{
			ErrorHelper.CheckError(value);

			var values = value.Split('/');

			return new Application
			{
				OwnerName = values[0],
				Name = values[1]
			};
		}

		public static Application ParseFromString(string[] lines)
		{
			Application application = new Application();

			if (!lines.Any())
				return application;

			ErrorHelper.CheckError(lines[0]);

			foreach (var line in lines)
			{
				if (!string.IsNullOrEmpty(line))
				{
					var values = line.Split(':');

					var key = values[0];
					var value = values[1].TrimStart();

					switch (key)
					{
						case "App Secret":
							application.AppSecret = value;
							break;
						case "Description":
							application.Description = value;
							break;
						case "Display Name":
							application.DisplayName = value;
							break;
						case "Name":
							application.Name = value;
							break;
						case "OS":
							application.OS = value;
							break;
						case "Platform":
							application.Platform = value;
							break;
						case "Owner ID":
							application.OwnerId = value;
							break;
						case "Owner Display Name":
							application.OwnerDisplayName = value;
							break;
						case "Owner Email":
							application.OwnerEmail = value;
							break;
						case "Owner Name":
							application.OwnerName = value;
							break;
						case "Azure Subscription ID":
							application.AzureSubscriptionId = value;
							break;
					}

				}
			}

			return application;
		}
	}
}
