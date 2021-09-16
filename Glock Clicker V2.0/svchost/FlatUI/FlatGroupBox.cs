using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000009 RID: 9
	public class FlatGroupBox : ContainerControl
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00010878 File Offset: 0x0000EA78
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00010890 File Offset: 0x0000EA90
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0001089C File Offset: 0x0000EA9C
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000108B4 File Offset: 0x0000EAB4
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

		// Token: 0x060000AB RID: 171 RVA: 0x000108C0 File Offset: 0x0000EAC0
		public FlatGroupBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			this.BackColor = Color.Transparent;
			base.Size = new Size(240, 180);
			this.Font = new Font("Segoe ui", 10f);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00010948 File Offset: 0x0000EB48
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
			path = Helpers.RoundRec(rectangle, 7);
			graphics.FillPath(new SolidBrush(this._BaseColor), path);
			bool showText = this.ShowText;
			base.OnPaint(e);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00010A38 File Offset: 0x0000EC38
		private void UpdateColors()
		{
			FlatColors colors = Helpers.GetColors(this);
			this._TextColor = colors.Flat;
		}

		// Token: 0x0400010B RID: 267
		private int W;

		// Token: 0x0400010C RID: 268
		private int H;

		// Token: 0x0400010D RID: 269
		private bool _ShowText = true;

		// Token: 0x0400010E RID: 270
		private Color _BaseColor = Color.FromArgb(60, 70, 73);

		// Token: 0x0400010F RID: 271
		private Color _TextColor = Helpers.FlatColor;
	}
}
