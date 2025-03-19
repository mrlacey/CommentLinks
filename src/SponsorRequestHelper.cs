// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace CommentLinks
{
	public class SponsorRequestHelper
	{
		public static async Task CheckIfNeedToShowAsync()
		{
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

			if (await SponsorDetector.IsSponsorAsync())
			{
				if (new Random().Next(1, 10) == 2)
				{
					ShowThanksForSponsorshipMessage();
				}
			}
			else
			{
				await ShowPromptForSponsorshipAsync();
			}
		}

		private static void ShowThanksForSponsorshipMessage()
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			OutputPane.Instance.WriteLine("Thank you for your sponsorship. It really helps.");
			OutputPane.Instance.WriteLine("If you have ideas for new features or suggestions for new features");
			OutputPane.Instance.WriteLine("please raise an issue at https://github.com/mrlacey/CommentLinks/issues");
			OutputPane.Instance.WriteLine(string.Empty);
		}

		private static async Task ShowPromptForSponsorshipAsync()
		{
			await OutputPane.Instance.WriteAsync("********************************************************************************************************");
			await OutputPane.Instance.WriteAsync("This is a free extension that is made possible thanks to the kind and generous donations of:");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("Daniel, James, Mike, Bill, unicorns39283, Martin, Richard, Alan, Howard, Mike, Dave, Joe, ");
			await OutputPane.Instance.WriteAsync("Alvin, Anders, Melvyn, Nik, Kevin, Richard, Orien, Shmueli, Gabriel, Martin, Neil, Daniel, ");
			await OutputPane.Instance.WriteAsync("Victor, Uno, Paula, Tom, Nick, Niki, chasingcode, luatnt, holeow, logarrhythmic, kokolorix, ");
			await OutputPane.Instance.WriteAsync("Guiorgy, Jessé, pharmacyhalo, MXM-7, atexinspect, João, hals1010, WTD-leachA, andermikael, ");
			await OutputPane.Instance.WriteAsync("spudwa, Cleroth, relentless-dev-purchases & 20+ more");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("Join them to show you appreciation and ensure future maintenance and development by becoming a sponsor.");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("Go to https://github.com/sponsors/mrlacey");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("Any amount, as either a one-off or on a monthly basis, is appreciated more than you can imagine.");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("I'll also tell you how to hide this message too.  ;)");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("If you can't afford to support financially, you can always");
			await OutputPane.Instance.WriteAsync("leave a positive review at https://marketplace.visualstudio.com/items?itemName=MattLaceyLtd.CommentLinks&ssr=false#review-details");
			await OutputPane.Instance.WriteAsync("");
			await OutputPane.Instance.WriteAsync("********************************************************************************************************");
		}
	}
}
