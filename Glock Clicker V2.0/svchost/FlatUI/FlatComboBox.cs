using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000008 RID: 8
	public class FlatComboBox : ComboBox
	{
		// Token: 0x06000098 RID: 152 RVA: 0x00010301 File Offset: 0x0000E501
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.State = MouseState.Down;
			base.Invalidate();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0001031A File Offset: 0x0000E51A
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.State = MouseState.Over;
			base.Invalidate();
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00010333 File Offset: 0x0000E533
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.State = MouseState.Over;
			base.Invalidate();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0001034C File Offset: 0x0000E54C
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.State = MouseState.None;
			base.Invalidate();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00010368 File Offset: 0x0000E568
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.x = e.Location.X;
			this.y = e.Location.Y;
			base.Invalidate();
			bool flag = e.X < base.Width - 41;
			if (flag)
			{
				this.Cursor = Cursors.IBeam;
			}
			else
			{
				this.Cursor = Cursors.Hand;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000103E0 File Offset: 0x0000E5E0
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			base.Invalidate();
			bool flag = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
			if (flag)
			{
				base.Invalidate();
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00010415 File Offset: 0x0000E615
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			base.Invalidate();
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00010428 File Offset: 0x0000E628
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00010440 File Offset: 0x0000E640
		public Color HoverColor
		{
			get
			{
				return this._HoverColor;
			}
			set
			{
				this._HoverColor = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0001044C File Offset: 0x0000E64C
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00010464 File Offset: 0x0000E664
		private int StartIndex
		{
			get
			{
				return this._StartIndex;
			}
			set
			{
				this._StartIndex = value;
				try
				{
					base.SelectedIndex = value;
				}
				catch
				{
				}
				base.Invalidate();
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000104A4 File Offset: 0x0000E6A4
		public void DrawItem_(object sender, DrawItemEventArgs e)
		{
			bool flag = e.Index < 0;
			if (!flag)
			{
				e.DrawBackground();
				e.DrawFocusRectangle();
				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
				e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
				e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
				e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				bool flag2 = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
				if (flag2)
				{
					e.Graphics.FillRectangle(new SolidBrush(this._HoverColor), e.Bounds);
				}
				else
				{
					e.Graphics.FillRectangle(new SolidBrush(this._BaseColor), e.Bounds);
				}
				e.Graphics.DrawString(base.GetItemText(base.Items[e.Index]), new Font("Segoe UI", 8f), Brushes.White, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height));
				e.Graphics.Dispose();
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000105DF File Offset: 0x0000E7DF
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			base.Height = 18;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000105F4 File Offset: 0x0000E7F4
		public FlatComboBox()
		{
			base.DrawItem += this.DrawItem_;
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			base.DrawMode = DrawMode.OwnerDrawFixed;
			this.BackColor = Color.FromArgb(45, 45, 48);
			this.ForeColor = Color.White;
			base.DropDownStyle = ComboBoxStyle.DropDownList;
			this.Cursor = Cursors.Hand;
			this.StartIndex = 0;
			base.ItemHeight = 18;
			this.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000106CC File Offset: 0x0000E8CC
		protected override void OnPaint(PaintEventArgs e)
		{
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			this.W = base.Width;
			this.H = base.Height;
			Rectangle rect = new Rectangle(0, 0, this.W, this.H);
			Rectangle rect2 = new Rectangle(Convert.ToInt32(this.W - 40), 0, this.W, this.H);
			GraphicsPath graphicsPath = new GraphicsPath();
			new GraphicsPath();
			graphics.Clear(Color.FromArgb(45, 45, 48));
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics.FillRectangle(new SolidBrush(this._BGColor), rect);
			graphicsPath.Reset();
			graphicsPath.AddRectangle(rect2);
			graphics.SetClip(graphicsPath);
			graphics.FillRectangle(new SolidBrush(this._BaseColor), rect2);
			graphics.ResetClip();
			graphics.DrawLine(Pens.White, this.W - 10, 6, this.W - 30, 6);
			graphics.DrawLine(Pens.White, this.W - 10, 12, this.W - 30, 12);
			graphics.DrawLine(Pens.White, this.W - 10, 18, this.W - 30, 18);
			graphics.DrawString(this.Text, this.Font, Brushes.White, new Point(4, 4), Helpers.NearSF);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x04000102 RID: 258
		private int W;

		// Token: 0x04000103 RID: 259
		private int H;

		// Token: 0x04000104 RID: 260
		private int _StartIndex;

		// Token: 0x04000105 RID: 261
		private int x;

		// Token: 0x04000106 RID: 262
		private int y;

		// Token: 0x04000107 RID: 263
		private MouseState State;

		// Token: 0x04000108 RID: 264
		private Color _BaseColor = Color.FromArgb(25, 27, 29);

		// Token: 0x04000109 RID: 265
		private Color _BGColor = Color.FromArgb(60, 60, 60);

		// Token: 0x0400010A RID: 266
		private Color _HoverColor = Color.FromArgb(35, 168, 109);
	}
}
