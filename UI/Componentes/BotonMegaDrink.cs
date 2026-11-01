using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UI.Componentes
{
    public class BotonMegaDrink : Button
    {
        private int borderRadius = 10;
        private Color colorNormal;
        private Color colorHover;

        public int BorderRadius
        {
            get { return borderRadius; }
            set { borderRadius = value; this.Invalidate(); }
        }

        public BotonMegaDrink()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = TemaMegaDrink.AzulCeleste;
            this.ForeColor = TemaMegaDrink.TextoClaro;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.Cursor = Cursors.Hand;
            this.Resize += (s, e) => this.Invalidate();

            colorNormal = this.BackColor;
            colorHover = Color.FromArgb(colorNormal.R + 20 > 255 ? 255 : colorNormal.R + 20,
                                        colorNormal.G + 20 > 255 ? 255 : colorNormal.G + 20,
                                        colorNormal.B + 20 > 255 ? 255 : colorNormal.B + 20);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            colorNormal = this.BackColor;
            this.BackColor = colorHover;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.BackColor = colorNormal;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rectSurface = this.ClientRectangle;
            int smoothSize = 2;

            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
            using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
            {
                this.Region = new Region(pathSurface);
                pevent.Graphics.DrawPath(penSurface, pathSurface);
            }
        }

        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}