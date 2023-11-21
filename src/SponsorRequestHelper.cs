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
                ShowPromptForSponsorship();
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

        private static void ShowPromptForSponsorship()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            OutputPane.Instance.WriteLine("Sorry to interrupt. I know your time is busy, presumably that's why you installed this extension (Comment Links).");
            OutputPane.Instance.WriteLine("I'm happy that the extensions I've created have been able to help you and many others");
            OutputPane.Instance.WriteLine("but I also need to make a living, and limited paid work over the last few years has been a challenge. :(");
            OutputPane.Instance.WriteLine(string.Empty);
            OutputPane.Instance.WriteLine("Show your support by making a one-off or recurring donation at https://github.com/sponsors/mrlacey");
            OutputPane.Instance.WriteLine(string.Empty);
            OutputPane.Instance.WriteLine("If you become a sponsor, I'll tell you how to hide this message too. ;)");
            OutputPane.Instance.WriteLine(string.Empty);
            OutputPane.Instance.Activate();
        }
    }
}
