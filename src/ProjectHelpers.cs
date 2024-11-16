// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace CommentLinks
{
	public static class ProjectHelpers
	{
		static ProjectHelpers()
		{
			// Rely on caller being on UI thread as shouldn't do it here in the constructor.
#pragma warning disable VSTHRD010 // Invoke single-threaded types on Main thread
			Dte = (DTE)Package.GetGlobalService(typeof(DTE));
#pragma warning restore VSTHRD010 // Invoke single-threaded types on Main thread
			Dte2 = (DTE2)Package.GetGlobalService(typeof(DTE));
		}

		public static DTE Dte { get; }

		public static DTE2 Dte2 { get; }
	}
}
