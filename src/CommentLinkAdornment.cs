// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CommentLinks
{
    internal sealed class CommentLinkAdornment : Button
    {
        private System.Windows.Forms.Cursor previousCursor;

        internal CommentLinkAdornment(CommentLinkTag tag)
        {
            this.Content = new TextBlock { Text = "➡" };
            this.BorderBrush = null;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(0);
            this.Background = new SolidColorBrush(Colors.GreenYellow);
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

            return lineNumber;
        }

        internal void Update(CommentLinkTag dataTag)
        {
            this.CmntLinkTag = dataTag;
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.previousCursor = System.Windows.Forms.Cursor.Current;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            System.Windows.Forms.Cursor.Current = this.previousCursor;
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
                    var filePath = projItem.Properties.Item("FullPath").Value.ToString();

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
                        ErrorHandler.ThrowOnFailure(viewAdapter.SetCaretPos(this.CmntLinkTag.LineNo - 1, 0));

                        // Make sure that the text is visible.
                        viewAdapter.CenterLines(this.CmntLinkTag.LineNo - 1, 1);
                    }
                    else if (!string.IsNullOrWhiteSpace(this.CmntLinkTag.SearchTerm))
                    {
                        var lineNo = this.GetLineNumber(filePath, this.CmntLinkTag.SearchTerm);

                        if (lineNo > 0)
                        {
                            ErrorHandler.ThrowOnFailure(viewAdapter.SetCaretPos(lineNo - 1, 0));
                            viewAdapter.CenterLines(lineNo - 1, 1);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }
        }
    }
}
