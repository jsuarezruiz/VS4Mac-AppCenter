using System.Linq;
using VS4Mac.AppCenter.Helpers;

namespace VS4Mac.AppCenter.Models
{
	public class Profile
	{
		public string Username { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }

		public static Profile ParseFromString(string[] lines)
		{
			Profile profile = new Profile();

			if (!lines.Any())
				return profile;

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
						case "Username":
							profile.Username = value;
							break;
						case "Display Name":
							profile.DisplayName = value;
							break;
						case "Email":
							profile.Email = value;
							break;
					}
				}
			}

			return profile;
		}
	}
}