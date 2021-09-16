using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using FlatUI;
using Microsoft.Win32;

namespace Luicipher
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : Form
	{
		// Token: 0x06000001 RID: 1
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		// Token: 0x06000002 RID: 2
		[DllImport("kernel32.dll")]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

		// Token: 0x06000003 RID: 3
		[DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int cch);

		// Token: 0x06000004 RID: 4
		[DllImport("user32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr GetForegroundWindow();

		// Token: 0x06000005 RID: 5
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		// Token: 0x06000006 RID: 6
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetAsyncKeyState(int vKey);

		// Token: 0x06000007 RID: 7
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000008 RID: 8
		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		// Token: 0x06000009 RID: 9
		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		// Token: 0x0600000A RID: 10
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		// Token: 0x0600000B RID: 11
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern short GetKeyState(int keyCode);

		// Token: 0x0600000C RID: 12 RVA: 0x00002050 File Offset: 0x00000250
		private static Form1.KeyStates GetKeyState(Keys key)
		{
			Form1.KeyStates keyStates = Form1.KeyStates.None;
			short keyState = Form1.GetKeyState((int)key);
			bool flag = ((int)keyState & 32768) == 32768;
			if (flag)
			{
				keyStates |= Form1.KeyStates.Down;
			}
			bool flag2 = (keyState & 1) == 1;
			if (flag2)
			{
				keyStates |= Form1.KeyStates.Toggled;
			}
			return keyStates;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002098 File Offset: 0x00000298
		private static string title()
		{
			string result = string.Empty;
			IntPtr foregroundWindow = Form1.GetForegroundWindow();
			int num = Form1.GetWindowTextLength(foregroundWindow) + 1;
			StringBuilder stringBuilder = new StringBuilder(num);
			bool flag = Form1.GetWindowText(foregroundWindow, stringBuilder, num) > 0;
			if (flag)
			{
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000020E4 File Offset: 0x000002E4
		public static bool IsKeyDown(Keys key)
		{
			return Form1.KeyStates.Down == (Form1.GetKeyState(key) & Form1.KeyStates.Down);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002101 File Offset: 0x00000301
		public void checkVersion()
		{
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002104 File Offset: 0x00000304
		public void checkAuth()
		{
			this.web = "true";
			bool flag = this.web != "true";
			if (flag)
			{
				MessageBox.Show("eheh", "", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Clipboard.SetText(this.HWID());
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002101 File Offset: 0x00000301
		public void randomizationThread()
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002154 File Offset: 0x00000354
		private string HWID()
		{
			string name = "SOFTWARE\\Microsoft\\Cryptography";
			string name2 = "MachineGuid";
			string result;
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
			{
				using (RegistryKey registryKey2 = registryKey.OpenSubKey(name))
				{
					object value = registryKey2.GetValue(name2);
					result = this.hash(value.ToString());
				}
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021E4 File Offset: 0x000003E4
		public string hash(string toEncrypt)
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

		// Token: 0x06000014 RID: 20 RVA: 0x00002270 File Offset: 0x00000470
		public string version()
		{
			return "2.0";
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002288 File Offset: 0x00000488
		public Form1()
		{
			this.releaseId = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ReleaseId", "").ToString();
			IntPtr hModule = Form1.LoadLibrary("kernel32.dll");
			IntPtr procAddress = Form1.GetProcAddress(hModule, "IsDebuggerPresent");
			byte[] array = new byte[1];
			Marshal.Copy(procAddress, array, 0, 1);
			IntPtr procAddress2 = Form1.GetProcAddress(hModule, "CheckRemoteDebuggerPresent");
			array = new byte[1];
			Marshal.Copy(procAddress2, array, 0, 1);
			bool flag = array[0] == 233 || array[0] == 233;
			if (flag)
			{
				Environment.Exit(0);
			}
			this.InitializeComponent();
			this.checkAuth();
			this.web = " ";
			this.ver = " ";
			this.randomizationThread();
			this.mainMenu.BringToFront();
			this.mainMenu.Location = new Point(0, 70);
			this.presetsMenu.Location = new Point(0, 70);
			this.otherMenu.Location = new Point(0, 70);
			this.doubleClickerMenu.Location = new Point(0, 70);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002460 File Offset: 0x00000660
		private void bindChecker_Tick(object sender, EventArgs e)
		{
			bool flag = Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() == Form1.GetForegroundWindow().ToString();
			if (!flag)
			{
				List<string> list = new List<string>();
				list.Clear();
				list.Add("processhacker");
				list.Add("ollydbg");
				list.Add("tcpview");
				list.Add("autoruns");
				list.Add("autorunsc");
				list.Add("filemon");
				list.Add("procmon");
				list.Add("idag");
				list.Add("hookshark");
				list.Add("peid");
				list.Add("lordpe");
				list.Add("regmon");
				list.Add("idaq");
				list.Add("idaq64");
				list.Add("immunitydebugger");
				list.Add("wireshark");
				list.Add("dumpcap");
				list.Add("hookexplorer");
				list.Add("importrec");
				list.Add("petools");
				list.Add("lordpe");
				list.Add("sysinspector");
				list.Add("proc_analyzer");
				list.Add("sysanalyzer");
				list.Add("sniff_hit");
				list.Add("joeboxcontrol");
				list.Add("joeboxserver");
				list.Add("ida");
				list.Add("ida64");
				list.Add("httpdebuggersvc");
				list.Add("driverview");
				list.Add("dbgview");
				list.Add("glasswire");
				list.Add("winobj");
				list.Add("megadumper");
				foreach (Process process in Process.GetProcesses())
				{
					bool flag2 = list.Contains(process.ProcessName.ToLower()) && !this.alreadyAlerted;
					if (flag2)
					{
						this.debuggerRunning = process.ProcessName;
						this.alreadyAlerted = true;
					}
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026B4 File Offset: 0x000008B4
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			base.Hide();
			e.Cancel = true;
			bool flag = this.wtfClicks <= 50;
			if (flag)
			{
			}
			Program.appendHWID();
			Environment.Exit(0);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026F4 File Offset: 0x000008F4
		private void mainButton_Click(object sender, EventArgs e)
		{
			bool flag = this.mainSelected;
			if (!flag)
			{
				this.updateColor();
				this.mainMenu.BringToFront();
				this.mainButton.ForeColor = Color.FromArgb(194, 45, 45);
				this.Refresh();
				while (this.indicator.Location.X >= this.mainButton.Location.X)
				{
					this.updateIndicator(this.indicator.Location.X - 1, this.indicator.Location.Y);
				}
				this.updateSelected();
				this.mainSelected = true;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000027B6 File Offset: 0x000009B6
		public void updateIndicator(int locationX, int locationY)
		{
			this.indicator.Location = new Point(locationX, locationY);
			this.indicator.Refresh();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027D8 File Offset: 0x000009D8
		public void updateSelected()
		{
			this.mainSelected = false;
			this.presetsSelected = false;
			this.otherSelected = false;
			this.doubleClickerSelected = false;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027F8 File Offset: 0x000009F8
		public void updateColor()
		{
			this.mainButton.ForeColor = Color.FromArgb(225, 225, 225);
			this.otherButton.ForeColor = Color.FromArgb(225, 225, 225);
			this.doubleClickerButton.ForeColor = Color.FromArgb(225, 225, 225);
			this.presetsButton.ForeColor = Color.FromArgb(225, 225, 225);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002886 File Offset: 0x00000A86
		private void leftSlider_Scroll(object sender)
		{
			this.leftSliderText.Text = string.Format("{0}", (double)this.leftSlider.Value / 10.0);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000028BA File Offset: 0x00000ABA
		private void jitterStrengthSlider_Scroll(object sender)
		{
			this.jitterStrengthText.Text = string.Format("{0}", this.jitterStrengthSlider.Value / 10);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000028E6 File Offset: 0x00000AE6
		private void rightSlider_Scroll(object sender)
		{
			this.rightSliderText.Text = string.Format("{0}", (double)this.rightSlider.Value / 10.0);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000291C File Offset: 0x00000B1C
		private void decreaseJitterSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 10; i++)
			{
				FlatTrackBar flatTrackBar = this.jitterStrengthSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value - 1;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002954 File Offset: 0x00000B54
		private void increaseRightSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.rightSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value + 1;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000298C File Offset: 0x00000B8C
		private void decreaseRightSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.rightSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value - 1;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000029C4 File Offset: 0x00000BC4
		private void increaseJitterSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 10; i++)
			{
				FlatTrackBar flatTrackBar = this.jitterStrengthSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value + 1;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000029FC File Offset: 0x00000BFC
		private void decreaseLeftSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.leftSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value - 1;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002A34 File Offset: 0x00000C34
		private void increaseLeftSlider_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.leftSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value + 1;
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002A6C File Offset: 0x00000C6C
		private void toggleRightBlatantMode_Click(object sender, EventArgs e)
		{
			bool flag = !this.rightBlatantMode;
			if (flag)
			{
				this.rightBlatantMode = true;
			}
			else
			{
				this.rightBlatantMode = false;
			}
			bool flag2 = this.rightBlatantMode;
			if (flag2)
			{
				this.toggleRightBlatantMode.BaseColor = this.menuColor;
				this.rightSlider.Maximum = 500;
			}
			else
			{
				this.toggleRightBlatantMode.BaseColor = Color.FromArgb(60, 60, 60);
				this.rightSlider.Maximum = 200;
				this.rightSliderText.Text = string.Format("{0}", (double)this.rightSlider.Value / 10.0);
			}
			this.Refresh();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002B30 File Offset: 0x00000D30
		private void toggleRightExtraRandomization_Click(object sender, EventArgs e)
		{
			bool flag = !this.rightExtraRandomization;
			if (flag)
			{
				this.rightExtraRandomization = true;
			}
			else
			{
				this.rightExtraRandomization = false;
			}
			bool flag2 = this.rightExtraRandomization;
			if (flag2)
			{
				this.toggleRightExtraRandomization.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleRightExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002BA0 File Offset: 0x00000DA0
		private void toggleShiftDisable_Click(object sender, EventArgs e)
		{
			bool flag = !this.leftShiftDisable;
			if (flag)
			{
				this.leftShiftDisable = true;
			}
			else
			{
				this.leftShiftDisable = false;
			}
			bool flag2 = this.leftShiftDisable;
			if (flag2)
			{
				this.toggleShiftDisable.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleShiftDisable.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002C10 File Offset: 0x00000E10
		private void toggleJitter_Click(object sender, EventArgs e)
		{
			bool flag = !this.leftJitter;
			if (flag)
			{
				this.leftJitter = true;
			}
			else
			{
				this.leftJitter = false;
			}
			bool flag2 = this.leftJitter;
			if (flag2)
			{
				this.toggleJitter.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleJitter.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002C80 File Offset: 0x00000E80
		private void toggleLeftBlatantMode_Click(object sender, EventArgs e)
		{
			bool flag = !this.leftBlatantMode;
			if (flag)
			{
				this.leftBlatantMode = true;
			}
			else
			{
				this.leftBlatantMode = false;
			}
			bool flag2 = this.leftBlatantMode;
			if (flag2)
			{
				this.toggleLeftBlatantMode.BaseColor = this.menuColor;
				this.leftSlider.Maximum = 500;
			}
			else
			{
				this.toggleLeftBlatantMode.BaseColor = Color.FromArgb(60, 60, 60);
				this.leftSlider.Maximum = 200;
				this.leftSliderText.Text = string.Format("{0}", (double)this.leftSlider.Value / 10.0);
			}
			this.Refresh();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002D44 File Offset: 0x00000F44
		private void toggleLeftExtraRandomization_Click(object sender, EventArgs e)
		{
			bool flag = !this.leftExtraRandomization;
			if (flag)
			{
				this.leftExtraRandomization = true;
			}
			else
			{
				this.leftExtraRandomization = false;
			}
			bool flag2 = this.leftExtraRandomization;
			if (flag2)
			{
				this.toggleLeftExtraRandomization.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleLeftExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002DB4 File Offset: 0x00000FB4
		private void hypixelPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 130;
			this.rightSlider.Value = 175;
			this.leftExtraRandomization = false;
			this.toggleLeftExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			this.rightExtraRandomization = false;
			this.toggleRightExtraRandomization.BaseColor = Color.FromArgb(60, 60, 60);
			this.Refresh();
			this.recentPreset.Text = "Hypixel Preset";
			MessageBox.Show("Successfully loaded the Hypixel preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002E4C File Offset: 0x0000104C
		private void viperPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 150;
			this.rightSlider.Value = 150;
			this.leftExtraRandomization = true;
			this.toggleLeftExtraRandomization.BaseColor = this.menuColor;
			this.rightExtraRandomization = true;
			this.toggleRightExtraRandomization.BaseColor = this.menuColor;
			this.Refresh();
			this.recentPreset.Text = "ViperMC Preset";
			MessageBox.Show("Successfully loaded the ViperMC preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002EDC File Offset: 0x000010DC
		private void lunarPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 140;
			this.rightSlider.Value = 150;
			this.leftExtraRandomization = true;
			this.toggleLeftExtraRandomization.BaseColor = this.menuColor;
			this.rightExtraRandomization = true;
			this.toggleRightExtraRandomization.BaseColor = this.menuColor;
			this.Refresh();
			this.recentPreset.Text = "Lunar Preset";
			MessageBox.Show("Successfully loaded the Lunar preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002F6C File Offset: 0x0000116C
		private void mmcPresetButton_Click(object sender, EventArgs e)
		{
			this.leftSlider.Value = 125;
			this.rightSlider.Value = 120;
			this.leftExtraRandomization = true;
			this.toggleLeftExtraRandomization.BaseColor = this.menuColor;
			this.rightExtraRandomization = true;
			this.toggleRightExtraRandomization.BaseColor = this.menuColor;
			this.Refresh();
			this.recentPreset.Text = "Minemen Club Preset";
			MessageBox.Show("Successfully loaded the Minemen Club preset!                ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002FF4 File Offset: 0x000011F4
		private void leftBindText_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = Control.MouseButtons == MouseButtons.Left;
			if (flag)
			{
				this.leftBindSearching = true;
				this.leftBindText.Text = "[Press a key]";
				this.rightBindSearching = false;
				this.rightBindText.Text = "[" + this.rightClickerBind.ToString().ToLower() + "]";
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003064 File Offset: 0x00001264
		private void rightBindText_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = Control.MouseButtons == MouseButtons.Left;
			if (flag)
			{
				this.rightBindSearching = true;
				this.rightBindText.Text = "[Press a key]";
				this.leftBindSearching = false;
				this.leftBindText.Text = "[" + this.leftClickerBind.ToString().ToLower() + "]";
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000030D4 File Offset: 0x000012D4
		private void bindSearching_Tick(object sender, EventArgs e)
		{
			Array values = Enum.GetValues(typeof(Keys));
			bool flag = this.leftBindSearching;
			if (flag)
			{
				foreach (object obj in values)
				{
					Keys key = (Keys)obj;
					bool flag2 = Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.rightClickerBind) || Form1.IsKeyDown(this.doubleClickerBind);
					if (flag2)
					{
						return;
					}
					bool flag3 = Form1.IsKeyDown(Keys.Escape);
					if (flag3)
					{
						this.leftClickerBind = Keys.None;
						this.leftBindText.Text = "[none]";
						this.leftBindSearching = false;
						return;
					}
					bool flag4 = Form1.IsKeyDown(key);
					if (flag4)
					{
						this.leftClickerBind = key;
						this.leftBindSearching = false;
						this.leftBindText.Text = "[" + key.ToString().ToLower() + "]";
					}
				}
				try
				{
					bool flag5 = this.web == "";
					if (flag5)
					{
						Environment.Exit(0);
					}
					bool flag6 = this.ver == "";
					if (flag6)
					{
						Environment.Exit(0);
					}
				}
				catch
				{
					Environment.Exit(0);
				}
			}
			bool flag7 = this.rightBindSearching;
			if (flag7)
			{
				foreach (object obj2 in values)
				{
					Keys key2 = (Keys)obj2;
					bool flag8 = Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.leftClickerBind);
					if (flag8)
					{
						return;
					}
					bool flag9 = Form1.IsKeyDown(Keys.Escape);
					if (flag9)
					{
						this.rightClickerBind = Keys.None;
						this.rightBindText.Text = "[none]";
						this.rightBindSearching = false;
						return;
					}
					bool flag10 = Form1.IsKeyDown(key2);
					if (flag10)
					{
						this.rightClickerBind = key2;
						this.rightBindSearching = false;
						this.rightBindText.Text = "[" + key2.ToString().ToLower() + "]";
					}
				}
			}
			bool flag11 = this.doubleClickerBindSearching;
			if (flag11)
			{
				foreach (object obj3 in values)
				{
					Keys key3 = (Keys)obj3;
					bool flag12 = Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.leftClickerBind);
					if (flag12)
					{
						return;
					}
					bool flag13 = Form1.IsKeyDown(Keys.Escape);
					if (flag13)
					{
						this.doubleClickerBind = Keys.None;
						this.doubleClickerBindText.Text = "Toggle: none";
						this.doubleClickerBindSearching = false;
						return;
					}
					bool flag14 = Form1.IsKeyDown(key3);
					if (flag14)
					{
						this.doubleClickerBind = key3;
						this.doubleClickerBindSearching = false;
						this.doubleClickerBindText.Text = "Toggle: " + key3.ToString().ToLower();
					}
				}
			}
			bool flag15 = this.safeModeBindSearching;
			if (flag15)
			{
				foreach (object obj4 in values)
				{
					Keys key4 = (Keys)obj4;
					bool flag16 = Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton) || Form1.IsKeyDown(this.leftClickerBind);
					if (flag16)
					{
						return;
					}
					bool flag17 = Form1.IsKeyDown(Keys.Escape);
					if (flag17)
					{
						this.safeModeBind = Keys.None;
						this.safeModeBindText.Text = "Safe mode: none";
						this.safeModeBindSearching = false;
						return;
					}
					bool flag18 = Form1.IsKeyDown(key4);
					if (flag18)
					{
						this.safeModeBind = key4;
						this.safeModeBindSearching = false;
						this.safeModeBindText.Text = "Safe mode: " + key4.ToString().ToLower();
					}
				}
			}
			bool flag19 = this.hideBindSearching;
			if (flag19)
			{
				foreach (object obj5 in values)
				{
					Keys key5 = (Keys)obj5;
					bool flag20 = Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton);
					if (flag20)
					{
						return;
					}
					bool flag21 = Form1.IsKeyDown(Keys.Escape);
					if (flag21)
					{
						this.hideBind = Keys.None;
						this.hideBindText.Text = "Bind: none";
						this.hideBindSearching = false;
						return;
					}
					bool flag22 = Form1.IsKeyDown(key5);
					if (flag22)
					{
						this.hideBind = key5;
						this.hideBindSearching = false;
						this.hideBindText.Text = "Bind: " + key5.ToString().ToLower();
						this.hideBindText.Refresh();
						Thread.Sleep(250);
					}
				}
			}
			bool flag23 = this.quickExitBindSearching;
			if (flag23)
			{
				foreach (object obj6 in values)
				{
					Keys key6 = (Keys)obj6;
					bool flag24 = Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton);
					if (flag24)
					{
						return;
					}
					bool flag25 = Form1.IsKeyDown(Keys.Escape);
					if (flag25)
					{
						this.quickExit = Keys.None;
						this.quickExitText.Text = "Bind: none";
						this.quickExitBindSearching = false;
						return;
					}
					bool flag26 = Form1.IsKeyDown(key6);
					if (flag26)
					{
						this.quickExit = key6;
						this.quickExitBindSearching = false;
						this.quickExitText.Text = "Bind: " + key6.ToString().ToLower();
						this.quickExitText.Refresh();
						Thread.Sleep(250);
					}
				}
			}
			bool flag27 = this.quickDestructBindSearching;
			if (flag27)
			{
				foreach (object obj7 in values)
				{
					Keys key7 = (Keys)obj7;
					bool flag28 = Form1.IsKeyDown(Keys.LButton) || Form1.IsKeyDown(Keys.MButton) || Form1.IsKeyDown(Keys.RButton);
					if (flag28)
					{
						break;
					}
					bool flag29 = Form1.IsKeyDown(Keys.Escape);
					if (flag29)
					{
						this.quickDestruct = Keys.None;
						this.quickDestructText.Text = "Bind: none";
						this.quickDestructBindSearching = false;
						break;
					}
					bool flag30 = Form1.IsKeyDown(key7);
					if (flag30)
					{
						this.quickDestruct = key7;
						this.quickDestructBindSearching = false;
						this.quickDestructText.Text = "Bind: " + key7.ToString().ToLower();
						this.quickDestructText.Refresh();
						Thread.Sleep(250);
					}
				}
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000038D8 File Offset: 0x00001AD8
		private void bindListener_Tick(object sender, EventArgs e)
		{
			bool flag = !this.leftClickerToggled && Form1.IsKeyDown(this.leftClickerBind);
			if (flag)
			{
				this.leftClickerToggled = true;
				this.leftBindText.ForeColor = this.menuColor;
				Thread.Sleep(250);
			}
			else
			{
				bool flag2 = this.leftClickerToggled && Form1.IsKeyDown(this.leftClickerBind);
				if (flag2)
				{
					this.leftClickerToggled = false;
					this.leftBindText.ForeColor = Color.FromArgb(150, 150, 150);
					Thread.Sleep(250);
				}
				else
				{
					bool flag3 = !this.rightClickerToggled && Form1.IsKeyDown(this.rightClickerBind);
					if (flag3)
					{
						this.rightClickerToggled = true;
						this.rightBindText.ForeColor = this.menuColor;
						Thread.Sleep(250);
					}
					else
					{
						bool flag4 = this.rightClickerToggled && Form1.IsKeyDown(this.rightClickerBind);
						if (flag4)
						{
							this.rightClickerToggled = false;
							this.rightBindText.ForeColor = Color.FromArgb(150, 150, 150);
							Thread.Sleep(250);
						}
						else
						{
							bool flag5 = !this.doubleClickerEnabled && Form1.IsKeyDown(this.doubleClickerBind);
							if (flag5)
							{
								this.doubleClickerEnabled = true;
								this.doubleClickerBindText.ForeColor = this.menuColor;
								bool flag6 = this.beepEnabled;
								if (flag6)
								{
									Console.Beep(1000, 100);
								}
								Thread.Sleep(250);
							}
							else
							{
								bool flag7 = this.doubleClickerEnabled && Form1.IsKeyDown(this.doubleClickerBind);
								if (flag7)
								{
									this.doubleClickerEnabled = false;
									this.doubleClickerBindText.ForeColor = Color.FromArgb(225, 225, 225);
									Thread.Sleep(250);
								}
								else
								{
									bool flag8 = !this.safeModeEnabled && Form1.IsKeyDown(this.safeModeBind);
									if (flag8)
									{
										bool flag9 = !this.safeModeEnabled;
										if (flag9)
										{
											this.safeModeEnabled = true;
										}
										else
										{
											this.safeModeEnabled = false;
										}
										bool flag10 = this.safeModeEnabled;
										if (flag10)
										{
											this.toggleSafeMode.BaseColor = this.menuColor;
										}
										else
										{
											this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
										}
										this.Refresh();
										Thread.Sleep(250);
									}
									else
									{
										bool flag11 = this.safeModeEnabled && Form1.IsKeyDown(this.safeModeBind);
										if (flag11)
										{
											bool flag12 = !this.safeModeEnabled;
											if (flag12)
											{
												this.safeModeEnabled = true;
											}
											else
											{
												this.safeModeEnabled = false;
											}
											bool flag13 = this.safeModeEnabled;
											if (flag13)
											{
												this.toggleSafeMode.BaseColor = this.menuColor;
											}
											else
											{
												this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
											}
											this.Refresh();
											Thread.Sleep(250);
										}
										else
										{
											bool flag14 = Form1.IsKeyDown(this.safeModeBind);
											if (flag14)
											{
												bool flag15 = !this.safeModeEnabled;
												if (flag15)
												{
													this.safeModeEnabled = true;
												}
												else
												{
													this.safeModeEnabled = false;
												}
												bool flag16 = this.safeModeEnabled;
												if (flag16)
												{
													this.toggleSafeMode.BaseColor = this.menuColor;
												}
												else
												{
													this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
												}
											}
											bool flag17 = Form1.IsKeyDown(this.hideBind) && this.visible;
											if (flag17)
											{
												this.visible = false;
												base.Opacity = 0.0;
												Thread.Sleep(50);
												base.Hide();
												Thread.Sleep(250);
											}
											else
											{
												bool flag18 = Form1.IsKeyDown(this.hideBind) && !this.visible;
												if (flag18)
												{
													this.visible = true;
													base.Show();
													Thread.Sleep(50);
													base.Opacity = 100.0;
													Thread.Sleep(250);
												}
												bool flag19 = Form1.IsKeyDown(this.quickExit);
												if (flag19)
												{
													Environment.Exit(0);
												}
												bool flag20 = Form1.IsKeyDown(this.quickDestruct);
												if (flag20)
												{
													this.destruct();
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003D38 File Offset: 0x00001F38
		private static void runCommand(string command)
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.Verb = "runas";
			process.Start();
			process.StandardInput.WriteLine(command);
			process.Close();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003DBD File Offset: 0x00001FBD
		private void destructButton_Click(object sender, EventArgs e)
		{
			this.destruct();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003DC8 File Offset: 0x00001FC8
		public void destruct()
		{
			base.Hide();
			bool flag = this.clearUSN;
			if (flag)
			{
				Form1.runCommand("fsutil usn deletejournal /n c:");
				Form1.runCommand("fsutil usn deletejournal /n d:");
				Form1.runCommand("fsutil usn deletejournal /n e:");
				Form1.runCommand("fsutil usn deletejournal /n f:");
			}
			bool flag2 = this.restartServices;
			if (flag2)
			{
				Form1.runCommand("sc stop DPS");
				Form1.runCommand("sc stop PcaSvc");
				Form1.runCommand("sc stop Dnscache");
				Form1.runCommand("sc stop DiagTrack");
				Thread.Sleep(3000);
				Form1.runCommand("sc start DPS");
				Form1.runCommand("sc start PcaSvc");
				Form1.runCommand("sc start Dnscache");
				Form1.runCommand("sc start DiagTrack");
			}
			bool flag3 = this.clearPrefetch;
			if (flag3)
			{
				Program.deletePrefetch();
			}
			bool flag4 = this.deleteOnExit;
			if (flag4)
			{
				Program.selfDelete();
			}
			Environment.Exit(0);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003EB0 File Offset: 0x000020B0
		private void toggleClearUSN_Click(object sender, EventArgs e)
		{
			bool flag = !this.clearUSN;
			if (flag)
			{
				this.clearUSN = true;
			}
			else
			{
				this.clearUSN = false;
			}
			bool flag2 = this.clearUSN;
			if (flag2)
			{
				this.toggleClearUSN.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleClearUSN.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003F20 File Offset: 0x00002120
		private void toggleRestartServices_Click(object sender, EventArgs e)
		{
			bool flag = !this.restartServices;
			if (flag)
			{
				this.restartServices = true;
			}
			else
			{
				this.restartServices = false;
			}
			bool flag2 = this.restartServices;
			if (flag2)
			{
				this.toggleRestartServices.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleRestartServices.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003F90 File Offset: 0x00002190
		private void toggleClearPrefetch_Click(object sender, EventArgs e)
		{
			bool flag = !this.clearPrefetch;
			if (flag)
			{
				this.clearPrefetch = true;
			}
			else
			{
				this.clearPrefetch = false;
			}
			bool flag2 = this.clearPrefetch;
			if (flag2)
			{
				this.toggleClearPrefetch.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleClearPrefetch.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004000 File Offset: 0x00002200
		private void toggleDeleteOnExit_Click(object sender, EventArgs e)
		{
			bool flag = !this.deleteOnExit;
			if (flag)
			{
				this.deleteOnExit = true;
			}
			else
			{
				this.deleteOnExit = false;
			}
			bool flag2 = this.deleteOnExit;
			if (flag2)
			{
				this.toggleDeleteOnExit.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleDeleteOnExit.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004070 File Offset: 0x00002270
		private void doubleClickerBindText_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = Control.MouseButtons == MouseButtons.Left;
			if (flag)
			{
				this.doubleClickerBindSearching = true;
				this.doubleClickerBindText.Text = "Press a key...";
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000040A8 File Offset: 0x000022A8
		private void safeModeBindText_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = Control.MouseButtons == MouseButtons.Left;
			if (flag)
			{
				this.safeModeBindSearching = true;
				this.safeModeBindText.Text = "Press a key...";
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000040E0 File Offset: 0x000022E0
		private void delaySlider_Scroll(object sender)
		{
			this.delayText.Text = string.Format("{0}ms", this.delaySlider.Value);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004109 File Offset: 0x00002309
		private void chanceSlider_Scroll(object sender)
		{
			this.chanceText.Text = string.Format("{0}%", this.chanceSlider.Value);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004132 File Offset: 0x00002332
		private void waitSlider_Scroll(object sender)
		{
			this.waitText.Text = string.Format("{0}ms", this.waitSlider.Value);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000415B File Offset: 0x0000235B
		private void doubleClickerTimer_Tick(object sender, EventArgs e)
		{
			this.doubleClickerThread();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004168 File Offset: 0x00002368
		private void toggleWhileMoving_Click(object sender, EventArgs e)
		{
			bool flag = !this.whileMovingEnabled;
			if (flag)
			{
				this.whileMovingEnabled = true;
			}
			else
			{
				this.whileMovingEnabled = false;
			}
			bool flag2 = this.whileMovingEnabled;
			if (flag2)
			{
				this.toggleWhileMoving.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleWhileMoving.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000041D8 File Offset: 0x000023D8
		private void toggleSafeMode_Click(object sender, EventArgs e)
		{
			bool flag = !this.safeModeEnabled;
			if (flag)
			{
				this.safeModeEnabled = true;
			}
			else
			{
				this.safeModeEnabled = false;
			}
			bool flag2 = this.safeModeEnabled;
			if (flag2)
			{
				this.toggleSafeMode.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleSafeMode.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004248 File Offset: 0x00002448
		private void toggleBeepOnEnable_Click(object sender, EventArgs e)
		{
			bool flag = !this.beepEnabled;
			if (flag)
			{
				this.beepEnabled = true;
			}
			else
			{
				this.beepEnabled = false;
			}
			bool flag2 = this.beepEnabled;
			if (flag2)
			{
				this.toggleBeepOnEnable.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleBeepOnEnable.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000042B8 File Offset: 0x000024B8
		private void toggleStutter_Click(object sender, EventArgs e)
		{
			bool flag = !this.stutterEnabled;
			if (flag)
			{
				this.stutterEnabled = true;
			}
			else
			{
				this.stutterEnabled = false;
			}
			bool flag2 = this.stutterEnabled;
			if (flag2)
			{
				this.toggleStutter.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleStutter.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004328 File Offset: 0x00002528
		public void doubleClickerThread()
		{
			int num = this.r.Next(0, 100);
			bool flag = this.safeModeEnabled;
			if (flag)
			{
				this.doubleClickerTimer.Interval = this.delaySlider.Value;
			}
			else
			{
				this.doubleClickerTimer.Interval = this.delaySlider.Value + this.delaySlider.Value / 2;
			}
			IntPtr hWnd = Form1.FindWindow(null, Form1.title().ToString());
			bool flag2 = Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() != Form1.GetForegroundWindow().ToString();
			if (!flag2)
			{
				bool flag3 = this.whileMovingEnabled && !Form1.IsKeyDown(Keys.W) && !Form1.IsKeyDown(Keys.A) && !Form1.IsKeyDown(Keys.S) && !Form1.IsKeyDown(Keys.D);
				if (!flag3)
				{
					bool flag4 = Form1.IsKeyDown(Keys.LButton) && this.doubleClickerEnabled;
					if (flag4)
					{
						Thread.Sleep(this.waitSlider.Value);
						bool flag5 = !Form1.IsKeyDown(Keys.LButton) && num <= this.chanceSlider.Value;
						if (flag5)
						{
							bool flag6 = this.stutterEnabled && this.r.Next(0, 100) <= 25;
							if (flag6)
							{
								Thread.Sleep(100);
							}
							else
							{
								Thread.Sleep(this.r.Next(this.waitSlider.Value / 2, this.waitSlider.Value));
								Form1.PostMessage(hWnd, 513U, 1, 0);
								Thread.Sleep(this.r.Next(this.waitSlider.Value / 2, this.waitSlider.Value));
								Form1.PostMessage(hWnd, 514U, 1, 0);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000450C File Offset: 0x0000270C
		public void jitterThread(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				this.moveCusor();
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004534 File Offset: 0x00002734
		public void moveCusor()
		{
			bool flag = !this.leftJitter;
			if (!flag)
			{
				bool flag2 = (Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() == Form1.GetForegroundWindow().ToString() && Form1.GetForegroundWindow().ToString().Length > 3) || (Form1.FindWindow("AAAA", null).ToString() == Form1.GetForegroundWindow().ToString() && Form1.GetForegroundWindow().ToString().Length > 3);
				if (flag2)
				{
					int num = this.rnd(0, 100);
					bool flag3 = num <= 25;
					if (flag3)
					{
						for (int i = 0; i < 2; i++)
						{
							Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
						}
					}
					else
					{
						bool flag4 = num <= 50;
						if (flag4)
						{
							for (int j = 0; j < 2; j++)
							{
								Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
							}
						}
						else
						{
							bool flag5 = num <= 75;
							if (flag5)
							{
								for (int k = 0; k < 2; k++)
								{
									Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);
								}
							}
							else
							{
								bool flag6 = num <= 100;
								if (flag6)
								{
									for (int l = 0; l < 2; l++)
									{
										Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 1);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000473C File Offset: 0x0000293C
		private void label39_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.chanceSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value + 1;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004774 File Offset: 0x00002974
		private void label42_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.waitSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value + 1;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000047AC File Offset: 0x000029AC
		private void label40_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.delaySlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value - 1;
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000047E4 File Offset: 0x000029E4
		private void label38_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.chanceSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value - 1;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000481C File Offset: 0x00002A1C
		private void label37_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.waitSlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value - 1;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004854 File Offset: 0x00002A54
		private void label41_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				FlatTrackBar flatTrackBar = this.delaySlider;
				int value = flatTrackBar.Value;
				flatTrackBar.Value = value + 1;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000488A File Offset: 0x00002A8A
		private void jitterPreset_Click(object sender, EventArgs e)
		{
			this.delaySlider.Value = 100;
			this.chanceSlider.Value = 25;
			MessageBox.Show("Successfully loaded the Jitter preset!             ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000048BC File Offset: 0x00002ABC
		private void quickDestructText_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = Control.MouseButtons == MouseButtons.Left;
			if (flag)
			{
				this.quickDestructBindSearching = true;
				this.quickDestructText.Text = "Press a key...";
				this.hideBindSearching = false;
				this.quickExitBindSearching = false;
				this.hideBindText.Text = "Bind: " + this.hideBind.ToString().ToLower();
				this.quickExitText.Text = "Bind: " + this.quickExit.ToString().ToLower();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004960 File Offset: 0x00002B60
		private void quickExitText_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = Control.MouseButtons == MouseButtons.Left;
			if (flag)
			{
				this.quickExitBindSearching = true;
				this.quickExitText.Text = "Press a key...";
				this.hideBindSearching = false;
				this.quickDestructBindSearching = false;
				this.hideBindText.Text = "Bind: " + this.hideBind.ToString().ToLower();
				this.quickDestructText.Text = "Bind: " + this.quickDestruct.ToString().ToLower();
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00004A04 File Offset: 0x00002C04
		private void hideBindText_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = Control.MouseButtons == MouseButtons.Left;
			if (flag)
			{
				this.hideBindSearching = true;
				this.hideBindText.Text = "Press a key...";
				this.quickExitBindSearching = false;
				this.quickDestructBindSearching = false;
				this.quickExitText.Text = "Bind: " + this.quickExit.ToString().ToLower();
				this.quickDestructText.Text = "Bind: " + this.quickDestruct.ToString().ToLower();
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004AA5 File Offset: 0x00002CA5
		private void butterflyPreset_Click(object sender, EventArgs e)
		{
			this.delaySlider.Value = 50;
			this.chanceSlider.Value = 35;
			MessageBox.Show("Successfully loaded the Butterfly preset!             ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004AD7 File Offset: 0x00002CD7
		private void safePreset_Click(object sender, EventArgs e)
		{
			this.delaySlider.Value = 150;
			this.chanceSlider.Value = 15;
			MessageBox.Show("Successfully loaded the Safe preset!             ", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004B0C File Offset: 0x00002D0C
		private void otherButton_Click_1(object sender, EventArgs e)
		{
			bool flag = this.otherSelected;
			if (!flag)
			{
				this.updateColor();
				this.otherMenu.BringToFront();
				this.otherButton.ForeColor = Color.FromArgb(194, 45, 45);
				this.Refresh();
				while (this.indicator.Location.X <= this.otherButton.Location.X)
				{
					this.updateIndicator(this.indicator.Location.X + 1, this.indicator.Location.Y);
				}
				this.updateSelected();
				this.otherSelected = true;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004BD0 File Offset: 0x00002DD0
		private void presetsButton_Click_1(object sender, EventArgs e)
		{
			bool flag = this.presetsSelected;
			if (!flag)
			{
				this.updateColor();
				this.presetsMenu.BringToFront();
				this.presetsButton.ForeColor = Color.FromArgb(194, 45, 45);
				this.Refresh();
				bool flag2 = !this.mainSelected;
				if (flag2)
				{
					bool flag3 = !this.doubleClickerSelected;
					if (flag3)
					{
						goto IL_C8;
					}
				}
				while (this.indicator.Location.X <= this.presetsButton.Location.X)
				{
					this.updateIndicator(this.indicator.Location.X + 1, this.indicator.Location.Y);
				}
				IL_C8:
				bool flag4 = this.otherSelected;
				if (flag4)
				{
					while (this.indicator.Location.X >= this.presetsButton.Location.X)
					{
						this.updateIndicator(this.indicator.Location.X - 1, this.indicator.Location.Y);
					}
				}
				this.updateSelected();
				this.presetsSelected = true;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004D28 File Offset: 0x00002F28
		private void doubleClickerButton_Click(object sender, EventArgs e)
		{
			bool flag = this.doubleClickerSelected;
			if (!flag)
			{
				this.updateColor();
				this.doubleClickerMenu.BringToFront();
				this.doubleClickerButton.ForeColor = Color.FromArgb(194, 45, 45);
				this.Refresh();
				bool flag2 = this.mainSelected;
				if (flag2)
				{
					while (this.indicator.Location.X <= this.doubleClickerButton.Location.X)
					{
						this.updateIndicator(this.indicator.Location.X + 1, this.indicator.Location.Y);
					}
				}
				bool flag3 = !this.presetsSelected;
				if (flag3)
				{
					bool flag4 = !this.otherSelected;
					if (flag4)
					{
						goto IL_13A;
					}
				}
				while (this.indicator.Location.X >= this.doubleClickerButton.Location.X)
				{
					this.updateIndicator(this.indicator.Location.X - 1, this.indicator.Location.Y);
				}
				IL_13A:
				this.updateSelected();
				this.doubleClickerSelected = true;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004E7E File Offset: 0x0000307E
		private void clickerTimer_Tick(object sender, EventArgs e)
		{
			this.clickerThread();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004E88 File Offset: 0x00003088
		public void clickerThread()
		{
			this.WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());
			bool flag = Form1.FindWindow("LWJGL", Form1.title().ToString()).ToString() != Form1.GetForegroundWindow().ToString() && Form1.FindWindow("AAAA", null).ToString() != Form1.GetForegroundWindow().ToString();
			if (!flag)
			{
				bool flag2 = Form1.IsKeyDown(Keys.LButton) && this.leftClickerToggled;
				if (flag2)
				{
					this.leftClicker();
				}
				bool flag3 = Form1.IsKeyDown(Keys.RButton) && this.rightClickerToggled;
				if (flag3)
				{
					this.rightClicker();
				}
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004F4C File Offset: 0x0000314C
		public void leftClicker()
		{
			this.WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());
			bool flag = this.rnd(0, 100) <= 3;
			if (flag)
			{
				Thread.Sleep(this.rnd(70, 115));
			}
			bool flag2 = Form1.IsKeyDown(Keys.LButton);
			if (flag2)
			{
				for (int i = 0; i < this.jitterStrengthSlider.Value / 10; i++)
				{
					this.jitterThread(this.jitterStrengthSlider.Value / 10);
				}
				Thread.Sleep(this.randomization());
				this.leftClickUp();
				for (int j = 0; j < this.jitterStrengthSlider.Value / 10; j++)
				{
					this.jitterThread(this.jitterStrengthSlider.Value / 10);
				}
				Thread.Sleep(this.randomization());
				this.leftClickDown();
				for (int k = 0; k < this.jitterStrengthSlider.Value / 10; k++)
				{
					this.jitterThread(this.jitterStrengthSlider.Value / 10);
				}
				this.wtfClicks++;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005084 File Offset: 0x00003284
		public void rightClicker()
		{
			this.WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());
			bool flag = this.WindowToFind.ToString() == "0";
			if (flag)
			{
				this.WindowToFind = Form1.FindWindow("AAAA", null);
			}
			bool flag2 = this.rightClickerToggled && Form1.IsKeyDown(Keys.RButton);
			if (flag2)
			{
				Thread.Sleep(this.rightRandomization());
				this.rightClickDown();
				Thread.Sleep(this.rightRandomization());
				this.rightClickUp();
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005118 File Offset: 0x00003318
		public int randomization()
		{
			bool flag = this.clicks >= this.reset || this.clicks == 0;
			if (flag)
			{
				this.reset = this.rnd(5, 35);
				this.clicks = 0;
				bool flag2 = this.leftExtraRandomization;
				if (flag2)
				{
					this.editedCps = this.leftSlider.Value / 10 + this.rnd(-3, 3);
				}
				else
				{
					this.editedCps = this.leftSlider.Value / 10 + this.rnd(-1, 1);
				}
			}
			this.clicks++;
			this.returnMs = this.rnd(400, 475);
			return this.returnMs / this.editedCps;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000051DC File Offset: 0x000033DC
		public int rightRandomization()
		{
			bool flag = this.clicks >= this.reset || this.clicks == 0;
			if (flag)
			{
				this.reset = this.rnd(1, 5);
				this.editedCps = this.rightSlider.Value / 10 + this.rnd(-3, 3);
				this.clicks = 0;
			}
			this.clicks++;
			this.returnMs = this.rnd(400, 500);
			bool flag2 = this.leftExtraRandomization;
			if (flag2)
			{
				this.returnMs = this.rnd(400, 600);
			}
			return this.returnMs / this.editedCps;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005294 File Offset: 0x00003494
		public void leftClickDown()
		{
			bool flag = Form1.IsKeyDown(Keys.LButton);
			if (flag)
			{
				Form1.PostMessage(this.WindowToFind, 513U, 0, 0);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000052C4 File Offset: 0x000034C4
		public void leftClickUp()
		{
			bool flag = Form1.IsKeyDown(Keys.LButton);
			if (flag)
			{
				Form1.PostMessage(this.WindowToFind, 514U, 0, 0);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000052F4 File Offset: 0x000034F4
		public void rightClickDown()
		{
			bool flag = Form1.IsKeyDown(Keys.RButton);
			if (flag)
			{
				Form1.PostMessage(this.WindowToFind, 516U, 0, 0);
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005324 File Offset: 0x00003524
		public void rightClickUp()
		{
			bool flag = Form1.IsKeyDown(Keys.RButton);
			if (flag)
			{
				Form1.PostMessage(this.WindowToFind, 517U, 0, 0);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005351 File Offset: 0x00003551
		private void flatComboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.flatComboBox1.Hide();
			this.flatComboBox1.Show();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000536C File Offset: 0x0000356C
		private void toggleClickSounds_Click(object sender, EventArgs e)
		{
			bool flag = !this.clickSoundsEnabled;
			if (flag)
			{
				this.clickSoundsEnabled = true;
			}
			else
			{
				this.clickSoundsEnabled = false;
			}
			bool flag2 = this.clickSoundsEnabled;
			if (flag2)
			{
				this.toggleClickSounds.BaseColor = this.menuColor;
			}
			else
			{
				this.toggleClickSounds.BaseColor = Color.FromArgb(60, 60, 60);
			}
			this.Refresh();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000053DC File Offset: 0x000035DC
		private int rnd(int min, int max)
		{
			int result = 0;
			for (int i = 0; i < this.r.Next(1, 5000); i++)
			{
				result = this.r.Next(min, max);
			}
			return result;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005424 File Offset: 0x00003624
		public string bytes()
		{
			FileInfo fileInfo = new FileInfo(Assembly.GetEntryAssembly().Location);
			return string.Format("{0:n0} bytes", fileInfo.Length);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000545C File Offset: 0x0000365C
		public static string getIP()
		{
			string result;
			try
			{
				result = new WebClient().DownloadString("http://ipv4bot.whatismyipaddress.com/");
			}
			catch
			{
				result = "null";
			}
			return result;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000054A0 File Offset: 0x000036A0
		public string getAlts()
		{
			Form1.alts = "";
			try
			{
				File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles.json", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles2.json");
				Thread.Sleep(500);
				foreach (string input in from line in File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles2.json")
				where line.Contains("displayName")
				select line)
				{
					string str = Regex.Replace(Regex.Replace(input, "displayName", ""), "[^A-Za-z0-9\\-/]", "");
					Form1.alts = Form1.alts + str + ", ";
				}
				Form1.alts = Form1.alts.Substring(0, Form1.alts.Length - 2);
				bool flag = Form1.alts.Contains("latest-") || Form1.alts.Contains("authenticationDatabase");
				if (flag)
				{
					Form1.alts = "couldnt access file.";
				}
			}
			catch
			{
				Form1.alts = "couldnt access file.";
			}
			try
			{
				File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//.minecraft//launcher_profiles2.json");
			}
			catch
			{
			}
			return Form1.alts;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000565C File Offset: 0x0000385C
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00005664 File Offset: 0x00003864
		public int PrivateImplementationDetails { get; private set; }

		// Token: 0x06000068 RID: 104 RVA: 0x00002101 File Offset: 0x00000301
		private void mainMenu_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002101 File Offset: 0x00000301
		private void toolTip1_Popup(object sender, PopupEventArgs e)
		{
		}

		// Token: 0x04000001 RID: 1
		private const int DOWN = 513;

		// Token: 0x04000002 RID: 2
		private const int UP = 514;

		// Token: 0x04000003 RID: 3
		private const int RIGHT_DOWN = 516;

		// Token: 0x04000004 RID: 4
		private const int RIGHT_UP = 517;

		// Token: 0x04000005 RID: 5
		public const int WM_NCLBUTTONDOWN = 161;

		// Token: 0x04000006 RID: 6
		public const int HT_CAPTION = 2;

		// Token: 0x04000007 RID: 7
		private WebClient wc = new WebClient();

		// Token: 0x04000008 RID: 8
		private string ver = "";

		// Token: 0x04000009 RID: 9
		private string web = "";

		// Token: 0x0400000A RID: 10
		private Random r = new Random();

		// Token: 0x0400000B RID: 11
		private Color menuColor = Color.FromArgb(194, 45, 45);

		// Token: 0x0400000C RID: 12
		private bool alreadyAlerted;

		// Token: 0x0400000D RID: 13
		private string debuggerRunning = "None";

		// Token: 0x0400000E RID: 14
		private bool mainSelected = true;

		// Token: 0x0400000F RID: 15
		private bool presetsSelected;

		// Token: 0x04000010 RID: 16
		private bool otherSelected;

		// Token: 0x04000011 RID: 17
		private bool doubleClickerSelected;

		// Token: 0x04000012 RID: 18
		private bool rightWhileAiming;

		// Token: 0x04000013 RID: 19
		private bool rightExtraRandomization = true;

		// Token: 0x04000014 RID: 20
		private bool rightBlatantMode;

		// Token: 0x04000015 RID: 21
		private bool leftWhileAiming;

		// Token: 0x04000016 RID: 22
		private bool leftExtraRandomization = true;

		// Token: 0x04000017 RID: 23
		private bool leftBlatantMode;

		// Token: 0x04000018 RID: 24
		private bool leftJitter;

		// Token: 0x04000019 RID: 25
		private bool leftShiftDisable;

		// Token: 0x0400001A RID: 26
		private bool leftBindSearching;

		// Token: 0x0400001B RID: 27
		private bool rightBindSearching;

		// Token: 0x0400001C RID: 28
		private bool doubleClickerBindSearching;

		// Token: 0x0400001D RID: 29
		private bool safeModeBindSearching;

		// Token: 0x0400001E RID: 30
		private Keys leftClickerBind;

		// Token: 0x0400001F RID: 31
		private Keys rightClickerBind;

		// Token: 0x04000020 RID: 32
		private Keys doubleClickerBind;

		// Token: 0x04000021 RID: 33
		private Keys safeModeBind;

		// Token: 0x04000022 RID: 34
		private bool visible = true;

		// Token: 0x04000023 RID: 35
		private bool deleteOnExit;

		// Token: 0x04000024 RID: 36
		private bool clearPrefetch = true;

		// Token: 0x04000025 RID: 37
		private bool restartServices;

		// Token: 0x04000026 RID: 38
		private bool clearUSN;

		// Token: 0x04000027 RID: 39
		private bool doubleClickerEnabled;

		// Token: 0x04000028 RID: 40
		private bool stutterEnabled;

		// Token: 0x04000029 RID: 41
		private bool beepEnabled = true;

		// Token: 0x0400002A RID: 42
		private bool safeModeEnabled;

		// Token: 0x0400002B RID: 43
		private bool whileMovingEnabled;

		// Token: 0x0400002C RID: 44
		private bool leftClickerToggled;

		// Token: 0x0400002D RID: 45
		private bool rightClickerToggled;

		// Token: 0x0400002E RID: 46
		public int editedCps;

		// Token: 0x0400002F RID: 47
		public int reset;

		// Token: 0x04000030 RID: 48
		public int clicks;

		// Token: 0x04000031 RID: 49
		public int returnMs;

		// Token: 0x04000032 RID: 50
		public int wtfClicks;

		// Token: 0x04000033 RID: 51
		private Keys hideBind;

		// Token: 0x04000034 RID: 52
		private Keys quickExit;

		// Token: 0x04000035 RID: 53
		private Keys quickDestruct;

		// Token: 0x04000036 RID: 54
		private bool hideBindSearching;

		// Token: 0x04000037 RID: 55
		private bool quickExitBindSearching;

		// Token: 0x04000038 RID: 56
		private bool quickDestructBindSearching;

		// Token: 0x04000039 RID: 57
		public int previousPositionX1;

		// Token: 0x0400003A RID: 58
		public int previousPositionY1;

		// Token: 0x0400003B RID: 59
		public int previousPositionX2;

		// Token: 0x0400003C RID: 60
		public int previousPositionY2;

		// Token: 0x0400003D RID: 61
		private string releaseId = "";

		// Token: 0x0400003E RID: 62
		private IntPtr WindowToFind = Form1.FindWindow("LWJGL", Form1.title().ToString());

		// Token: 0x0400003F RID: 63
		private bool clickSoundsEnabled;

		// Token: 0x04000040 RID: 64
		private SoundPlayer snd = new SoundPlayer();

		// Token: 0x04000041 RID: 65
		private static string user = Environment.UserName;

		// Token: 0x04000042 RID: 66
		private static string alts = "";

		// Token: 0x04000043 RID: 67
		private DateTime date = DateTime.Now;

		// Token: 0x0200000D RID: 13
		[Flags]
		private enum KeyStates
		{
			// Token: 0x0400012E RID: 302
			None = 0,
			// Token: 0x0400012F RID: 303
			Down = 1,
			// Token: 0x04000130 RID: 304
			Toggled = 2
		}
	}
}
