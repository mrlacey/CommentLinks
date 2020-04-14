// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Web;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CommentLinks.Commands
{
    public sealed class LinkToSelectionCommand : CommentLinkCommandBase
    {
        public const int CommandId = 0x0300;

        private LinkToSelectionCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static LinkToSelectionCommand Instance { get; private set; }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new LinkToSelectionCommand(package, commandService);
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        private async void Execute(object sender, EventArgs e)
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            try
            {
                var doc = ProjectHelpers.Dte.ActiveDocument;

                var filePath = doc.FullName;

                var selectedText = (doc.Selection as EnvDTE.TextSelection).Text;

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    Clipboard.SetText($"Link:{SimpleSpaceEncoding(Path.GetFileName(filePath))}:{SimpleSpaceEncoding(selectedText)}");
                    await StatusBarHelper.ShowMessageAsync("Link to selection copied to clipboard.");
                }
                else
                {
                    await StatusBarHelper.ShowMessageAsync("File path for document not available.");
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }
        }
    }
}
