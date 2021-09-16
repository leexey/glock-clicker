using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000006 RID: 6
	public class checkBoxes : ContainerControl
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000090 RID: 144 RVA: 0x0001010C File Offset: 0x0000E30C
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00010124 File Offset: 0x0000E324
		[Category("Colors")]
		public Color BaseColor
		{
			get
			{
				return this._BaseColor;
			}
			set
			{
				this._BaseColor = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00010130 File Offset: 0x0000E330
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00010148 File Offset: 0x0000E348
		public bool ShowText
		{
			get
			{
				return this._ShowText;
			}
			set
			{
				this._ShowText = value;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00010154 File Offset: 0x0000E354
		public checkBoxes()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			this.BackColor = Color.Transparent;
			base.Size = new Size(240, 180);
			this.Font = new Font("Segoe ui", 10f);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000101DC File Offset: 0x0000E3DC
		protected override void OnPaint(PaintEventArgs e)
		{
			this.UpdateColors();
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			this.W = base.Width - 1;
			this.H = base.Height - 1;
			GraphicsPath path = new GraphicsPath();
			new GraphicsPath();
			new GraphicsPath();
			Rectangle rectangle = new Rectangle(2, 2, this.W - 4, this.H - 4);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics.Clear(this.BackColor);
			path = Helpers.RoundRec(rectangle, 3);
			graphics.FillPath(new SolidBrush(this._BaseColor), path);
			bool showText = this.ShowText;
			base.OnPaint(e);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000102CC File Offset: 0x0000E4CC
		private void UpdateColors()
		{
			FlatColors colors = Helpers.GetColors(this);
			this._TextColor = colors.Flat;
		}

		// Token: 0x040000FC RID: 252
		private int W;

		// Token: 0x040000FD RID: 253
		private int H;

		// Token: 0x040000FE RID: 254
		private bool _ShowText = true;

		// Token: 0x040000FF RID: 255
		private Color _BaseColor = Color.FromArgb(60, 70, 73);

		// Token: 0x04000100 RID: 256
		private Color _TextColor = Helpers.FlatColor;
	}
}
