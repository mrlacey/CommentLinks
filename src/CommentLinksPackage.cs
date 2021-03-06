﻿// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Threading;
using CommentLinks.Commands;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CommentLinks
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "2.5.1")] // Info on this package for Help/About
    [Guid(CommentLinksPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(OptionsGrid), "Comment Links", "General", 0, 0, true)]
    [ProvideProfileAttribute(typeof(OptionsGrid), "Comment Links", "General", 0, 0, isToolsOptionPage: true, DescriptionResourceID = 108)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class CommentLinksPackage : AsyncPackage
    {
        public const string PackageGuidString = "e1724685-50af-49aa-9d96-ff26a69cc1c9";

        public OptionsGrid Options
        {
            get
            {
                return (OptionsGrid)this.GetDialogPage(typeof(OptionsGrid));
            }
        }

        public static CommentLinksPackage Instance { get; private set; }

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            Instance = this;

            await LinkToFileCommand.InitializeAsync(this);
            await LinkToLineCommand.InitializeAsync(this);
            await LinkToSelectionCommand.InitializeAsync(this);
        }
    }
}
