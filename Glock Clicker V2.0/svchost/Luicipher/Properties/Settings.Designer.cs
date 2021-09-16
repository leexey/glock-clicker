using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Luicipher.Properties
{
	// Token: 0x02000005 RID: 5
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000100D4 File Offset: 0x0000E2D4
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040000FB RID: 251
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
