// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using CommentLinks.Commands;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CommentLinks
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)] // Info on this package for Help/About
	[Guid(PackageGuids.guidCommentLinksPackageString)]
	[ProvideOptionPage(typeof(OptionsGrid), Vsix.Name, "General", 0, 0, true)]
	[ProvideProfileAttribute(typeof(OptionsGrid), Vsix.Name, "General", 0, 0, isToolsOptionPage: true, DescriptionResourceID = 108)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	public sealed class CommentLinksPackage : AsyncPackage
	{
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

			await SponsorRequestHelper.CheckIfNeedToShowAsync();

			TrackBasicUsageAnalytics();
		}

		private static void TrackBasicUsageAnalytics()
		{
#if !DEBUG
			try
			{
				if (string.IsNullOrWhiteSpace(AnalyticsConfig.TelemetryConnectionString))
				{
					return;
				}

				var config = new TelemetryConfiguration
				{
					ConnectionString = AnalyticsConfig.TelemetryConnectionString,
				};

				var client = new TelemetryClient(config);

				var properties = new Dictionary<string, string>
				{
					{ "VsixVersion", Vsix.Version },
					{ "VsVersion", Microsoft.VisualStudio.Telemetry.TelemetryService.DefaultSession?.GetSharedProperty("VS.Core.ExeVersion") },
				};

				client.TrackEvent(Vsix.Name, properties);
			}
			catch (Exception exc)
			{
				System.Diagnostics.Debug.WriteLine(exc);
				OutputPane.Instance.WriteLine("Error tracking usage analytics: " + exc.Message);
			}
#endif
		}
	}
}
