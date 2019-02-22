﻿using MonoDevelop.Components.Commands;

namespace VS4Mac.AppCenter.Commands
{
	public class BuildCommand : CommandHandler
	{
		protected override void Update(CommandInfo info)
		{
			info.Enabled = false;
			base.Update(info);
		}
	}
}
