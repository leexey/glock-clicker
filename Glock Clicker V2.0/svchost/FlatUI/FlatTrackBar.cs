using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x0200000A RID: 10
	[DefaultEvent("Scroll")]
	public class FlatTrackBar : Control
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00010A5C File Offset: 0x0000EC5C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			base.OnMouseDown(e);
			this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
			bool flag = e.Button == MouseButtons.Left || e.Button == MouseButtons.Right;
			if (flag)
			{
				this.Val = Convert.ToInt32((float)(this._Value - this.minvalue) / (float)(this._Maximum - this.minvalue) * (float)(base.Width - 10));
				this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
				this.Track = new Rectangle(this.Val, 0, 10, 40);
				this.Bool = this.Track.Contains(e.Location);
			}
			bool flag2 = this._Value > this._Maximum;
			if (flag2)
			{
				this._Value = this._Maximum;
			}
			bool flag3 = this._Value < this.minvalue;
			if (flag3)
			{
				this._Value = this.minvalue;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00010BA4 File Offset: 0x0000EDA4
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			base.OnMouseDown(e);
			bool flag = this.Bool && e.X > -10 && e.X < base.Width + 10;
			if (flag)
			{
				this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
			}
			bool flag2 = e.Button == MouseButtons.Left || e.Button == MouseButtons.Right;
			if (flag2)
			{
				this.Val = Convert.ToInt32((float)(this._Value - this.minvalue) / (float)(this._Maximum - this.minvalue) * (float)(base.Width - 10));
				this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
				this.Track = new Rectangle(this.Val, 0, 10, 40);
			}
			bool flag3 = this._Value > this._Maximum;
			if (flag3)
			{
				this._Value = this._Maximum;
			}
			bool flag4 = this._Value < this.minvalue;
			if (flag4)
			{
				this._Value = this.minvalue;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00010CFC File Offset: 0x0000EEFC
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.Bool = false;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00010D10 File Offset: 0x0000EF10
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x00010D28 File Offset: 0x0000EF28
		[Category("Colors")]
		public Color TrackColor
		{
			get
			{
				return this._TrackColor;
			}
			set
			{
				this._TrackColor = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00010D34 File Offset: 0x0000EF34
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00010D4C File Offset: 0x0000EF4C
		[Category("Colors")]
		public Color HatchColor
		{
			get
			{
				return this._HatchColor;
			}
			set
			{
				this._HatchColor = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00010D58 File Offset: 0x0000EF58
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00010D6F File Offset: 0x0000EF6F
		[Category("Colors")]
		public Color ColorScheme1
		{
			get
			{
				return FlatTrackBar.scheme1;
			}
			set
			{
				FlatTrackBar.scheme1 = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00010D78 File Offset: 0x0000EF78
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x00010D8F File Offset: 0x0000EF8F
		[Category("Colors")]
		public Color ColorScheme2
		{
			get
			{
				return FlatTrackBar.scheme2;
			}
			set
			{
				FlatTrackBar.scheme2 = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00010D98 File Offset: 0x0000EF98
		// (set) Token: 0x060000BA RID: 186 RVA: 0x00010DB0 File Offset: 0x0000EFB0
		[Category("Misc")]
		public int Minimum
		{
			get
			{
				return this.minvalue;
			}
			set
			{
				this.minvalue = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00010DBC File Offset: 0x0000EFBC
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00010DD4 File Offset: 0x0000EFD4
		[Category("Misc")]
		public bool Full
		{
			get
			{
				return this.filled;
			}
			set
			{
				this.filled = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00010DE0 File Offset: 0x0000EFE0
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00010DF8 File Offset: 0x0000EFF8
		[Category("Misc")]
		public bool Decimal
		{
			get
			{
				return this.floatText;
			}
			set
			{
				this.floatText = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00010E04 File Offset: 0x0000F004
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00010E1C File Offset: 0x0000F01C
		[Category("Misc")]
		public double FloatValue
		{
			get
			{
				return this.FloatVal;
			}
			set
			{
				this.FloatVal = value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000C1 RID: 193 RVA: 0x00010E28 File Offset: 0x0000F028
		// (remove) Token: 0x060000C2 RID: 194 RVA: 0x00010E60 File Offset: 0x0000F060
		public event FlatTrackBar.ScrollEventHandler Scroll;

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00010E98 File Offset: 0x0000F098
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00010EB0 File Offset: 0x0000F0B0
		public int Maximum
		{
			get
			{
				return this._Maximum;
			}
			set
			{
				this._Maximum = value;
				bool flag = value < this._Value;
				if (flag)
				{
					this._Value = value;
				}
				bool flag2 = value < this.minvalue;
				if (flag2)
				{
					this.minvalue = value;
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00010EF8 File Offset: 0x0000F0F8
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00010F10 File Offset: 0x0000F110
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				bool flag = value == this._Value;
				if (!flag)
				{
					bool flag2 = value <= this.Maximum && value >= this.minvalue;
					if (flag2)
					{
						this._Value = value;
						base.Invalidate();
						bool flag3 = this.Scroll != null;
						if (flag3)
						{
							this.Scroll(this);
						}
					}
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00010F78 File Offset: 0x0000F178
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00010F90 File Offset: 0x0000F190
		public bool ShowValue
		{
			get
			{
				return this._ShowValue;
			}
			set
			{
				this._ShowValue = value;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00010F9C File Offset: 0x0000F19C
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			bool flag = e.KeyCode != Keys.Subtract;
			if (flag)
			{
				bool flag2 = e.KeyCode == Keys.Add;
				if (flag2)
				{
					bool flag3 = this.Value == this._Maximum;
					if (!flag3)
					{
						int value = this.Value;
						this.Value = value + 1;
					}
				}
			}
			else
			{
				bool flag4 = this.Value == 0;
				if (!flag4)
				{
					int value = this.Value;
					this.Value = value - 1;
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0001101F File Offset: 0x0000F21F
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			base.Invalidate();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00011031 File Offset: 0x0000F231
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			base.Height = 25;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00011048 File Offset: 0x0000F248
		public FlatTrackBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(38, 38, 38);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000110F8 File Offset: 0x0000F2F8
		protected override void OnPaint(PaintEventArgs e)
		{
			new Pen(Color.FromArgb(FlatTrackBar.scheme2.ToArgb()), 1f);
			Bitmap bitmap = new Bitmap(base.Width + 30, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			this.W = base.Width;
			this.H = base.Height - 1;
			Rectangle rectangle = new Rectangle(2, 2, this.W - 2, this.H - 3);
			GraphicsPath graphicsPath = new GraphicsPath();
			new GraphicsPath();
			Graphics graphics2 = graphics;
			graphics2.SmoothingMode = SmoothingMode.HighQuality;
			graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics2.Clear(this.BackColor);
			graphicsPath = Helpers.RoundRec(rectangle, 3);
			graphics2.FillPath(new SolidBrush(this.BaseColor), graphicsPath);
			this.Val = Convert.ToInt32((float)(this._Value - this.minvalue) / (float)(this._Maximum - this.minvalue) * (float)(this.W - 8));
			bool flag = this._Value == this.minvalue;
			if (flag)
			{
				this.Track = new Rectangle(this.Val + 4, 4, 6, this.H - 7);
			}
			else
			{
				bool flag2 = this._Value == this.minvalue + 1;
				if (flag2)
				{
					this.Track = new Rectangle(this.Val + 2, 4, 6, this.H - 7);
				}
				else
				{
					this.Track = new Rectangle(this.Val, 4, 6, this.H - 7);
				}
			}
			graphicsPath.AddRectangle(rectangle);
			graphics2.SetClip(graphicsPath);
			graphics2.FillRectangle(new SolidBrush(this._TrackColor), new Rectangle(0, 1, this.Track.X + this.Track.Width, 9));
			graphics2.ResetClip();
			new HatchBrush(HatchStyle.Plaid, this.HatchColor, this._TrackColor);
			new HatchBrush(HatchStyle.Plaid, this.BaseColor, this.BaseColor);
			GraphicsPath path = new GraphicsPath();
			Rectangle rectangle2 = default(Rectangle);
			bool flag3 = this.filled;
			if (flag3)
			{
				rectangle2 = new Rectangle(4, 4, this.Track.X + this.Track.Width - 5, this.H - 7);
			}
			else
			{
				rectangle2 = new Rectangle(4, 2, this.Track.X + this.Track.Width - 5, 0);
			}
			path = Helpers.RoundRec(rectangle2, 2);
			graphics2.FillPath(new SolidBrush(FlatTrackBar.scheme1), path);
			GraphicsPath path2 = new GraphicsPath();
			path2 = Helpers.RoundRec(this.Track, 2);
			graphics2.FillPath(new SolidBrush(FlatTrackBar.scheme1), path2);
			graphics2.FillRectangle(this.HBB2, new Rectangle(0, 0, 3, 3));
			bool showValue = this.ShowValue;
			if (showValue)
			{
				bool flag4 = this.floatText;
				if (flag4)
				{
					double floatVal = ((double)this.Minimum + (double)this.Value) / 100.0;
					this.FloatVal = floatVal;
					string str = floatVal.ToString();
					graphics2.DrawString(str + this.Text, new Font("Arial", 10f), Brushes.White, new Rectangle(0, 0, this.W, this.H + 2), new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					});
				}
				else
				{
					graphics2.DrawString(this.Value.ToString() + this.Text, new Font("Arial", 10f), Brushes.White, new Rectangle(0, 0, this.W, this.H + 2), new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					});
				}
			}
			base.OnPaint(e);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x04000111 RID: 273
		private int W;

		// Token: 0x04000112 RID: 274
		private int H;

		// Token: 0x04000113 RID: 275
		private int Val;

		// Token: 0x04000114 RID: 276
		private bool Bool;

		// Token: 0x04000115 RID: 277
		private bool filled;

		// Token: 0x04000116 RID: 278
		private bool floatText;

		// Token: 0x04000117 RID: 279
		private double FloatVal;

		// Token: 0x04000118 RID: 280
		private Rectangle Track;

		// Token: 0x04000119 RID: 281
		private int minvalue;

		// Token: 0x0400011A RID: 282
		private int _Maximum = 10;

		// Token: 0x0400011B RID: 283
		private int _Value;

		// Token: 0x0400011C RID: 284
		private bool _ShowValue;

		// Token: 0x0400011D RID: 285
		private HatchBrush HBB2 = new HatchBrush(HatchStyle.Plaid, Color.FromArgb(25, 25, 25), Color.FromArgb(25, 25, 25));

		// Token: 0x0400011E RID: 286
		public static Color scheme1 = Color.FromArgb(130, 96, 189);

		// Token: 0x0400011F RID: 287
		public static Color scheme2 = Color.FromArgb(130, 96, 189);

		// Token: 0x04000120 RID: 288
		public static Pen outline_color = new Pen(Color.FromArgb(FlatTrackBar.scheme1.ToArgb()), 1f);

		// Token: 0x04000121 RID: 289
		private Color SliderColor = Color.FromArgb(FlatTrackBar.scheme1.ToArgb());

		// Token: 0x04000122 RID: 290
		private Color BaseColor = Color.FromArgb(60, 60, 60);

		// Token: 0x04000123 RID: 291
		private Color _TrackColor = Color.FromArgb(100, 100, 100);

		// Token: 0x04000124 RID: 292
		private Color _HatchColor = Color.FromArgb(100, 100, 100);

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x060000D8 RID: 216
		public delegate void ScrollEventHandler(object sender);
	}
}
