// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Button = System.Windows.Controls.Button;

namespace CommentLinks
{
    internal sealed class CommentLinkAdornment : Button
    {
        private readonly int currentLineNumber;

        internal CommentLinkAdornment(CommentLinkTag tag, int currentLineNumber)
        {
            this.Content = new TextBlock { Text = "➡" };
            this.BorderBrush = null;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(0);
            this.Background = new SolidColorBrush(Colors.GreenYellow);
            this.Cursor = Cursors.Hand;
            this.CmntLinkTag = tag;
            this.currentLineNumber = currentLineNumber;
        }

        public CommentLinkTag CmntLinkTag { get; private set; }

        internal int GetLineNumberOfMatch(string pathToFile, string content, int lineToSkip)
        {
            return this.GetLineNumber(pathToFile, content, startLine: 0, searchDown: true, lineToSkip);
        }

        internal int GetLineNumberAboveCurrent(string pathToFile, string content, int lineToSkip)
        {
            return this.GetLineNumber(pathToFile, content, startLine: this.currentLineNumber - 1, searchDown: false, lineToSkip);
        }

        internal int GetLineNumberBelowCurrent(string pathToFile, string content, int lineToSkip)
        {
            return this.GetLineNumber(pathToFile, content, startLine: this.currentLineNumber + 1, searchDown: true, lineToSkip);
        }

        internal int GetLineNumber(string pathToFile, string content, int startLine, bool searchDown, int skipLine)
        {
            var lines = File.ReadAllLines(pathToFile);

            if (searchDown)
            {
                if (startLine < lines.Length)
                {
                    for (int i = startLine; i < lines.Length; i++)
                    {
                        if (i == skipLine)
                        {
                            continue;
                        }

                        if (lines[i].Contains(content))
                        {
                            return i + 1;
                        }
                    }
                }
            }
            else
            {
                for (int i = startLine; i >= 0; i--)
                {
                    if (lines[i].Contains(content))
                    {
                        return i + 1;
                    }
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
                async Task<IVsTextView> OpenFileAsync(string fileToOpen)
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    VsShellUtilities.OpenDocument(
                        new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)ProjectHelpers.Dte),
                        fileToOpen,
                        Guid.Empty,
                        out _,
                        out _,
                        out _,
                        out IVsTextView textViewAdapter);

                    return textViewAdapter;
                }

                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                if (this.CmntLinkTag.IsRunCommand)
                {
                    if (string.IsNullOrWhiteSpace(this.CmntLinkTag.FileName))
                    {
                        await StatusBarHelper.ShowMessageAsync($"No command to run.");
                    }
                    else
                    {
                        var spaceIndex = this.CmntLinkTag.FileName.IndexOfAny(new[] { ' ', '\t' });

                        var args = string.Empty;
                        var cmd = this.CmntLinkTag.FileName;

                        if (spaceIndex > 0)
                        {
                            cmd = this.CmntLinkTag.FileName.Substring(0, spaceIndex);
                            args = this.CmntLinkTag.FileName.Substring(spaceIndex);
                        }

                        System.Diagnostics.Process.Start(cmd, args);
                    }

                    return;
                }

                if (this.CmntLinkTag.FileName.StartsWith("..."))
                {
                    var relPath = this.CmntLinkTag.FileName.TrimStart(new[] { '.', '\\', '/' });
                    var posDir = Path.GetDirectoryName(ProjectHelpers.Dte.ActiveDocument.FullName);

                    var keepTrying = true;

                    while (keepTrying)
                    {
                        var posFilePath = Path.Combine(posDir, relPath);

                        if (File.Exists(posFilePath))
                        {
                            this.CmntLinkTag = this.CmntLinkTag.UpdateFileName(posFilePath);
                            keepTrying = false;
                        }
                        else
                        {
                            try
                            {
                                posDir = Path.GetDirectoryName(posDir);

                                if (string.IsNullOrWhiteSpace(posDir))
                                {
                                    await StatusBarHelper.ShowMessageAsync($"Unable to find file '{this.CmntLinkTag.FileName}'");
                                    keepTrying = false;
                                    return;
                                }
                            }
                            catch (Exception)
                            {
                                await StatusBarHelper.ShowMessageAsync($"Unable to find file '{this.CmntLinkTag.FileName}'");
                                keepTrying = false;
                                return;
                            }
                        }
                    }
                }
                else if (this.CmntLinkTag.FileName.StartsWith("..")
                 || this.CmntLinkTag.FileName.StartsWith("./")
                 || this.CmntLinkTag.FileName.StartsWith(".\\"))
                {
                    if (ProjectHelpers.Dte.ActiveDocument.FullName != null)
                    {
                        var newPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(ProjectHelpers.Dte.ActiveDocument.FullName), this.CmntLinkTag.FileName));
                        this.CmntLinkTag = this.CmntLinkTag.UpdateFileName(newPath);
                    }
                }

                NavigationType navType = NavigationType.Default;
                int skipLine = -1;

                if (File.Exists(this.CmntLinkTag.FileName))
                {
                    var va = await OpenFileAsync(this.CmntLinkTag.FileName);
                    await this.NavigateWithinFileAsync(va, this.CmntLinkTag.FileName, this.CmntLinkTag.LineNo, this.CmntLinkTag.SearchTerm, navigationType: navType, lineToSkip: skipLine);

                    return;
                }

                if (Uri.IsWellFormedUriString(this.CmntLinkTag.FileName, UriKind.Absolute))
                {
                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo(this.CmntLinkTag.FileName)
                        { UseShellExecute = true });

                    return;
                }

                string filePath = string.Empty;
                IVsTextView viewAdapter = null;
                bool sameFile = false;

                if (this.CmntLinkTag.FileName.Equals("^"))
                {
                    sameFile = true;
                    navType = NavigationType.Up;
                }
                else if (this.CmntLinkTag.FileName.Equals("v"))
                {
                    sameFile = true;
                    navType = NavigationType.Down;
                }

                if (!sameFile)
                {
                    var projItem = ProjectHelpers.Dte2.Solution.FindProjectItem(this.CmntLinkTag.FileName);

                    if (projItem != null)
                    {
                        // If an item in a solution folder
                        if (projItem.Kind == "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}")
                        {
                            filePath = projItem.FileNames[1];
                        }
                        else if (projItem.Kind == "{66A2671F-8FB5-11D2-AA7E-00C04F688DDE}")
                        {
                            // A miscellaneous file (possibly something open but not in the solution)
                            filePath = this.CmntLinkTag.FileName;
                        }
                        else
                        {
                            filePath = projItem.Properties?.Item("FullPath")?.Value?.ToString();
                        }
                    }
                }

                var activeDocPath = ProjectHelpers.Dte.ActiveDocument.FullName;

                if (activeDocPath == filePath || System.IO.Path.GetFileName(activeDocPath) == filePath)
                {
                    skipLine = this.currentLineNumber;
                    sameFile = true;
                }

                if (sameFile)
                {
                    viewAdapter = this.GetActiveTextView();
                    filePath = activeDocPath;
                    sameFile = true;
                }
                else if (!string.IsNullOrEmpty(filePath))
                {
                    viewAdapter = await OpenFileAsync(filePath);
                    sameFile = filePath == activeDocPath;
                }

                if (viewAdapter != null)
                {
                    await this.NavigateWithinFileAsync(viewAdapter, filePath, this.CmntLinkTag.LineNo, this.CmntLinkTag.SearchTerm, navType, skipLine);

                    return;
                }

                await StatusBarHelper.ShowMessageAsync($"Unable to find file '{this.CmntLinkTag.FileName}'");
            }
            catch (Exception exc)
            {
                await OutputPane.Instance.WriteAsync(exc);
            }
        }

        private async Task NavigateWithinFileAsync(IVsTextView textViewAdapter, string fileToNavigate, int lineNo, string searchText, NavigationType navigationType, int lineToSkip)
        {
            if (textViewAdapter == null)
            {
                return;
            }

            if (lineNo > 0)
            {
                // Set the cursor at the beginning of the declaration.
                if (textViewAdapter.SetCaretPos(lineNo - 1, 0) == VSConstants.S_OK)
                {
                    // Make sure that the text is visible.
                    textViewAdapter.CenterLines(lineNo - 1, 1);
                }
                else
                {
                    await StatusBarHelper.ShowMessageAsync($"'{fileToNavigate}' contains fewer than '{lineNo}' lines.");
                }
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                if (navigationType == NavigationType.Default)
                {
                    var foundLineNo = this.GetLineNumberOfMatch(fileToNavigate, searchText, lineToSkip);

                    if (foundLineNo > 0)
                    {
                        ErrorHandler.ThrowOnFailure(textViewAdapter.SetCaretPos(foundLineNo - 1, 0));
                        textViewAdapter.CenterLines(foundLineNo - 1, 1);
                    }
                    else
                    {
                        await StatusBarHelper.ShowMessageAsync($"Could not find '{searchText}' in '{fileToNavigate}'.");
                    }
                }
                else if (navigationType == NavigationType.Up)
                {
                    var foundLineNo = this.GetLineNumberAboveCurrent(fileToNavigate, searchText, lineToSkip);

                    if (foundLineNo > 0)
                    {
                        ErrorHandler.ThrowOnFailure(textViewAdapter.SetCaretPos(foundLineNo - 1, 0));
                        textViewAdapter.CenterLines(foundLineNo - 1, 1);
                    }
                    else
                    {
                        await StatusBarHelper.ShowMessageAsync($"Could not find '{searchText}' above the link.");
                    }
                }
                else if (navigationType == NavigationType.Down)
                {
                    var foundLineNo = this.GetLineNumberBelowCurrent(fileToNavigate, searchText, lineToSkip);

                    if (foundLineNo > 0)
                    {
                        ErrorHandler.ThrowOnFailure(textViewAdapter.SetCaretPos(foundLineNo - 1, 0));
                        textViewAdapter.CenterLines(foundLineNo - 1, 1);
                    }
                    else
                    {
                        await StatusBarHelper.ShowMessageAsync($"Could not find '{searchText}' below the link.");
                    }
                }
            }
        }

        // From https://docs.microsoft.com/en-us/visualstudio/extensibility/walkthrough-creating-a-view-adornment-commands-and-settings-column-guides?view=vs-2019
        /// <summary>
        /// Find the active text view (if any) in the active document.
        /// </summary>
        /// <returns>The IVsTextView of the active view, or null if there is no active
        /// document or the
        /// active view in the active document is not a text view.</returns>
        private IVsTextView GetActiveTextView()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IVsMonitorSelection selection =
                ServiceProvider.GlobalProvider.GetService(typeof(IVsMonitorSelection)) as IVsMonitorSelection;
            Assumes.Present(selection);
            ErrorHandler.ThrowOnFailure(
                selection.GetCurrentElementValue(
                    (uint)VSConstants.VSSELELEMID.SEID_DocumentFrame, out object frameObj));

            if (!(frameObj is IVsWindowFrame frame))
            {
                return null;
            }

            return GetActiveView(frame);
        }

        private static IVsTextView GetActiveView(IVsWindowFrame windowFrame)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (windowFrame == null)
            {
                throw new ArgumentException("windowFrame");
            }

            ErrorHandler.ThrowOnFailure(
                windowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out object pvar));

            IVsTextView textView = pvar as IVsTextView;
            if (textView == null)
            {
                if (pvar is IVsCodeWindow codeWin)
                {
                    ErrorHandler.ThrowOnFailure(codeWin.GetLastActiveView(out textView));
                }
            }

            return textView;
        }
    }
}
