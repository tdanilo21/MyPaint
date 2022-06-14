using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinFormsApp1
{
    interface IGraphics
    {
        public abstract void DrawLine(Pen p, Point a, Point b);
        public abstract void DrawPolygon(Pen p, Point[] points);
        public abstract void FillPolygon(SolidBrush sb, Point[] points);
        public abstract void DrawEllipse(Pen p, Point a, Point b);
        public abstract void FillEllipse(SolidBrush sb, Point a, Point b);
    }
    class MyGraphics : IGraphics
    {
        Graphics g;
        public MyGraphics(Graphics g) { this.g = g; }
        public void DrawLine(Pen p, Point a, Point b) { g.DrawLine(p, a, b); }
        public void DrawPolygon(Pen p, Point[] points) { g.DrawPolygon(p, points); }
        public void FillPolygon(SolidBrush sb, Point[] points) { g.FillPolygon(sb, points); }
        public void DrawEllipse(Pen p, Point a, Point b) { g.DrawEllipse(p, a.X, a.Y, b.X - a.X, b.Y - a.Y); }
        public void FillEllipse(SolidBrush sb, Point a, Point b) { g.FillEllipse(sb, a.X, a.Y, b.X - a.X, b.Y - a.Y); }
    }
}
