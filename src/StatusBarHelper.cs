// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CommentLinks
{
    public static class StatusBarHelper
    {
        public static async Task ShowMessageAsync(string message)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                ProjectHelpers.Dte.StatusBar.Text = message;
            }
            catch (Exception exc)
            {
                await OutputPane.Instance.WriteAsync(exc);
            }
        }
    }
}
