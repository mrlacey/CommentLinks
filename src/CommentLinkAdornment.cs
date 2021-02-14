// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Button = System.Windows.Controls.Button;

namespace CommentLinks
{
    internal sealed class CommentLinkAdornment : Button
    {
        internal CommentLinkAdornment(CommentLinkTag tag)
        {
            this.Content = new TextBlock { Text = "➡" };
            this.BorderBrush = null;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(0);
            this.Background = new SolidColorBrush(Colors.GreenYellow);
            this.Cursor = Cursors.Hand;
            this.CmntLinkTag = tag;
        }

        public CommentLinkTag CmntLinkTag { get; private set; }

        internal int GetLineNumber(string pathToFile, string content)
        {
            int lineNumber = 0;
            foreach (var line in System.IO.File.ReadAllLines(pathToFile))
            {
                lineNumber++;
                if (line.Contains(content))
                {
                    return lineNumber;
                }
            }

            return 0;
        }

        internal void Update(CommentLinkTag dataTag)
        {
            this.CmntLinkTag = dataTag;
        }

#pragma warning disable VSTHRD100 // Avoid async void methods
        protected override async void OnClick()
#pragma warning restore VSTHRD100 // Avoid async void methods
        {
            base.OnClick();

            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                var projItem = ProjectHelpers.Dte2.Solution.FindProjectItem(this.CmntLinkTag.FileName);

                if (projItem != null)
                {
                    string filePath;

                    // If an item in a solution folder
                    if (projItem.Kind == "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}")
                    {
                        filePath = projItem.FileNames[1];
                    }
                    else
                    {
                        filePath = projItem.Properties.Item("FullPath").Value.ToString();
                    }


                    VsShellUtilities.OpenDocument(
                        new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)ProjectHelpers.Dte),
                        filePath,
                        Guid.Empty,
                        out _,
                        out _,
                        out IVsWindowFrame pWindowFrame,
                        out IVsTextView viewAdapter);

                    if (this.CmntLinkTag.LineNo > 0)
                    {
                        // Set the cursor at the beginning of the declaration.
                        if (viewAdapter.SetCaretPos(this.CmntLinkTag.LineNo - 1, 0) == VSConstants.S_OK)
                        {
                            // Make sure that the text is visible.
                            viewAdapter.CenterLines(this.CmntLinkTag.LineNo - 1, 1);
                        }
                        else
                        {
                            await StatusBarHelper.ShowMessageAsync($"'{this.CmntLinkTag.FileName}' contains fewer than '{this.CmntLinkTag.LineNo}' lines.");
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(this.CmntLinkTag.SearchTerm))
                    {
                        var lineNo = this.GetLineNumber(filePath, this.CmntLinkTag.SearchTerm);

                        if (lineNo > 0)
                        {
                            ErrorHandler.ThrowOnFailure(viewAdapter.SetCaretPos(lineNo - 1, 0));
                            viewAdapter.CenterLines(lineNo - 1, 1);
                        }
                        else
                        {
                            await StatusBarHelper.ShowMessageAsync($"Could not find '{this.CmntLinkTag.SearchTerm}' in '{this.CmntLinkTag.FileName}'.");
                        }
                    }
                }
                else
                {
                    await StatusBarHelper.ShowMessageAsync($"Unable to find file '{this.CmntLinkTag.FileName}'");
                }
            }
            catch (Exception exc)
            {
                await OutputPane.Instance.WriteAsync(exc);
            }
        }
    }
}
