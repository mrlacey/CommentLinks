// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CommentLinks
{
    public class OutputPane
    {
        private static Guid dsPaneGuid = new Guid("61839752-500E-4B8C-AA1E-0F62BD71C741");

        private static OutputPane instance;

        private readonly IVsOutputWindowPane pane;

        private OutputPane()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (ServiceProvider.GlobalProvider.GetService(typeof(SVsOutputWindow)) is IVsOutputWindow outWindow
                && (ErrorHandler.Failed(outWindow.GetPane(ref dsPaneGuid, out this.pane)) || this.pane == null))
            {
                if (ErrorHandler.Failed(outWindow.CreatePane(ref dsPaneGuid, "Comment Links", 1, 0)))
                {
                    System.Diagnostics.Debug.WriteLine("Failed to create output pane.");
                    return;
                }

                if (ErrorHandler.Failed(outWindow.GetPane(ref dsPaneGuid, out this.pane)) || (this.pane == null))
                {
                    System.Diagnostics.Debug.WriteLine("Failed to get output pane.");
                }
            }
        }

        public static OutputPane Instance => instance ?? (instance = new OutputPane());

        public async Task ActivateAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(CancellationToken.None);

            this.pane?.Activate();
        }

        public async Task WriteAsync(Exception exception, [CallerMemberName] string caller = "*unknown method*")
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(CancellationToken.None);

            await this.WriteAsync(string.Empty);
            await this.WriteAsync(DateTimeOffset.Now.ToString());
            await this.WriteAsync($"Error in {caller}");
            await this.WriteAsync(exception.Message);
            await this.WriteAsync(exception.Source);
            await this.WriteAsync(exception.StackTrace);
        }

        public async Task WriteAsync(string message)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(CancellationToken.None);

            this.pane?.OutputStringThreadSafe($"{message}{Environment.NewLine}");
        }
    }
}
