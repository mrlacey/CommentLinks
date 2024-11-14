// Copyright (c) Matt Lacey Ltd. All rights reserved.
// Licensed under the MIT license.

using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace CommentLinks
{
	public class OptionsGrid : DialogPage
	{
		[Category("General")]
		[DisplayName("Trigger word")]
		[Description("The word used to use to indicate a link. Alphabetic characters only. Re-open documents to see the change.")]
		public string TriggerWord { get; set; } = "link";

		[Category("General")]
		[DisplayName("Accept command risk")]
		[Description("Running commands can have consequences. By using this functionality you accept the risk of any unexpected or unintended consequences.")]
		public bool AcceptRiskOfRunningCommands { get; set; } = false;

		[Category("Display")]
		[DisplayName("Link button character")]
		[Description("The unicode character to display on the button next to a link. A single character is recommended. Will default to '➡' if blank.")]
		public string ButtonIcon { get; set; } = "➡";

		[Category("Display")]
		[DisplayName("Link button background color")]
		[Description("The color to use for the link button background. Can be a named color or a hex code (e.g. #FF336699). Default is 'GreenYellow'.")]
		public string ButtonBackground { get; set; } = "GreenYellow";

		[Category("Generated Links")]
		[DisplayName("Include relative path to file")]
		[Description("If set to true, links generated with the context menu will include the relative path to the file within a project.")]
		public bool IncludePathToFile { get; set; } = false;

		[Category("Generated Links")]
		[DisplayName("Link text case")]
		[Description("How the link indicator will be cased in links generated from the context menu.")]
		public CaseOption LinkCasing { get; set; } = CaseOption.lowercase;

		[Category("Generated Links")]
		[DisplayName("Use absolute paths")]
		[Description("Include the full path to a file in links generated from the context menu. This takes priority over including the relative path.")]
		public bool UseAbsolutePaths { get; set; } = false;

		internal void EnsureValidTriggerWord()
		{
			var temp = new System.Text.StringBuilder();

			foreach (var chr in this.TriggerWord)
			{
				if (char.IsLetter(chr))
				{
					temp.Append(chr);
				}
			}

			if (temp.Length < 1)
			{
				this.TriggerWord = "link";
			}
			else
			{
				this.TriggerWord = temp.ToString();
			}
		}

		private void ValidateOptions()
		{
			this.EnsureValidTriggerWord();

			this.ButtonIcon = this.ButtonIcon.Trim();

			if (string.IsNullOrEmpty(this.ButtonIcon))
			{
				this.ButtonIcon = "➡";
			}

			// TODO: Add validation to ensure a valid button background value
			this.ButtonBackground = this.ButtonBackground.Trim();

			if (string.IsNullOrEmpty(this.ButtonBackground))
			{
				this.ButtonBackground = "GreenYellow";
			}
		}

		protected override void OnApply(PageApplyEventArgs e)
		{
			ValidateOptions();
			base.OnApply(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			ValidateOptions();
			base.OnClosed(e);
		}
	}
}
