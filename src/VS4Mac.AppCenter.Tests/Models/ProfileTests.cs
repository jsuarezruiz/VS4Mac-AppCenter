using System.Text.RegularExpressions;
using NUnit.Framework;
using VS4Mac.AppCenter.Models;

namespace VS4Mac.AppCenter.Tests.Models
{
	[TestFixture]
	public class ProfileTests
	{
		[Test]
		public void ProfileParseFromStringTest()
		{
			string bashProfile = "Username:user\nDisplay Name:User\nEmail:user@mail.com";
			string[] lines = Regex.Split(bashProfile, "\n");
			var profile = Profile.ParseFromString(lines);

			Assert.IsNotNull(profile);
		}
	}
}
