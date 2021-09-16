using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Luicipher
{
	// Token: 0x02000003 RID: 3
	internal static class Program
	{
		// Token: 0x0600006D RID: 109 RVA: 0x0000FA15 File Offset: 0x0000DC15
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000FA30 File Offset: 0x0000DC30
		public static void selfDelete()
		{
			Process.Start(new ProcessStartInfo
			{
				Arguments = "/C choice /C Y /N /D Y /T & Del \"" + Program.path + "\" & exit",
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				FileName = "cmd.exe",
				Verb = "runas"
			});
			Environment.Exit(0);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000FA94 File Offset: 0x0000DC94
		public static void deletePrefetch()
		{
			FileInfo[] files = new DirectoryInfo("C:\\Windows\\Prefetch\\").GetFiles("*.pf");
			string fileName = Path.GetFileName(Program.path);
			foreach (FileInfo fileInfo in files)
			{
				bool flag = fileInfo.FullName.ToLower().Contains(fileName.ToLower());
				if (flag)
				{
					try
					{
						File.Delete(fileInfo.FullName);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000FB20 File Offset: 0x0000DD20
		private static int KillProcess(string processName)
		{
			int num = 0;
			Process[] processesByName = Process.GetProcessesByName(processName);
			for (int i = 0; i < processesByName.Length; i++)
			{
				processesByName[i].Kill();
				num++;
			}
			return num;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000FB60 File Offset: 0x0000DD60
		public static void appendHWID()
		{
			Process.Start(new ProcessStartInfo
			{
				Arguments = string.Concat(new string[]
				{
					"/C choice /C Y /N /D Y /T & find /c \"",
					Program.HWID(),
					"\" \"",
					Program.path,
					"\"  || ( echo ◙",
					Program.HWID(),
					" >> \"",
					Program.path,
					"\" )"
				}),
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				FileName = "cmd.exe",
				Verb = "runas"
			});
			Environment.Exit(0);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000FC08 File Offset: 0x0000DE08
		private static string HWID()
		{
			string name = "SOFTWARE\\Microsoft\\Cryptography";
			string name2 = "MachineGuid";
			string result;
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
				{
					result = Program.hash(registryKey2.GetValue(name2).ToString());
				}
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000FC94 File Offset: 0x0000DE94
		private static string hash(string toEncrypt)
		{
			string result;
			using (SHA256 sha = SHA256.Create())
			{
				byte[] array = sha.ComputeHash(Encoding.UTF8.GetBytes(toEncrypt));
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x040000F8 RID: 248
		public static string path = Assembly.GetExecutingAssembly().Location;
	}
}
