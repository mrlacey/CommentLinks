// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace CommentLinks.Commands
{
    public class CommentLinkCommandBase
    {
        public static readonly Guid CommandSet = new Guid("8b7f9d00-fcc1-4b0f-a951-3d63273c87de");

#pragma warning disable SA1401 // Fields should be private
        protected AsyncPackage package;
#pragma warning restore SA1401 // Fields should be private

        protected IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        protected static async Task<IWpfTextView> GetTextViewAsync(IAsyncServiceProvider serviceProvider)
        {
            if (!(await serviceProvider.GetServiceAsync(typeof(SVsTextManager)) is IVsTextManager textManager))
            {
                return null;
            }

            textManager.GetActiveView(1, null, out IVsTextView textView);

            if (textView == null)
            {
                return null;
            }
            else
            {
                var provider = await GetEditorAdaptersFactoryServiceAsync(serviceProvider);
                return provider.GetWpfTextView(textView);
            }
        }

        protected static async Task<IVsEditorAdaptersFactoryService> GetEditorAdaptersFactoryServiceAsync(IAsyncServiceProvider serviceProvider)
        {
            if (await serviceProvider.GetServiceAsync(typeof(SComponentModel)) is IComponentModel componentModel)
            {
                return componentModel.GetService<IVsEditorAdaptersFactoryService>();
            }
            else
            {
                return null;
            }
        }
    }
}
