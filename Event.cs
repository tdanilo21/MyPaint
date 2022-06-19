using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace MyPaint
{
    abstract class Event
    {
        protected Pen pen;
        protected bool mouseDown;
        protected Point a;
        public Bitmap prev, image;
        public Event(Bitmap bmp)
        {
            pen = new Pen(Color.Black);
            mouseDown = false;
            prev = new Bitmap(bmp);
            image = new Bitmap(bmp);
        }

        public Event(Event e)
        {
            pen = e.pen;
            image = new Bitmap(e.image);
            mouseDown = false;
            prev = new Bitmap(e.image);
        }
        public abstract void MouseMove(Point p);
        public abstract void Reset();
        public void ChangeColor(Color c) { pen.Color = c; }
        public void ChangeWidth(int w) { pen.Width = w; }
    }

    class Drawing : Event
    {
        System.Drawing.Drawing2D.GraphicsPath path;
        public Drawing(Bitmap bmp) : base(bmp)
        {
            path = new System.Drawing.Drawing2D.GraphicsPath();
        }

        public Drawing(Event e) : base(e)
        {
            path = new System.Drawing.Drawing2D.GraphicsPath();
        }

        public override void MouseMove(Point p)
        {
            if (mouseDown) ExtendPath(p);
            mouseDown = true;
            a = p;
        }

        private void ExtendPath(Point p)
        {
            path.AddLine(a, p);
            image = new Bitmap(prev);
            using (Graphics g = Graphics.FromImage(image))
                g.DrawPath(pen, path);
        }

        public override void Reset()
        {
            prev = new Bitmap(image);
            path.Reset();
            mouseDown = false;
        }
    }

    abstract class DrawShape : Event
    {
        public DrawShape(Bitmap bmp) : base(bmp) { }
        public DrawShape(Event e) : base(e) { }

        protected abstract void ExtendShape(Point p);

        public override void MouseMove(Point p)
        {
            if (mouseDown) ExtendShape(p);
            else a = p;
            mouseDown = true;
        }
        public override void Reset()
        {
            prev = new Bitmap(image);
            mouseDown = false;
        }
    }

    class MyRectangle : DrawShape
    {
        public MyRectangle(Bitmap bmp) : base(bmp) { }
        public MyRectangle(Event e) : base(e) { }

        protected override void ExtendShape(Point p)
        {
            Point b = new Point(Math.Min(a.X, p.X), Math.Min(a.Y, p.Y));
            Point c = new Point(Math.Max(a.X, p.X), Math.Max(a.Y, p.Y));
            image = new Bitmap(prev);
            using (Graphics g = Graphics.FromImage(image))
                g.DrawRectangle(pen, b.X, b.Y, c.X - b.X, c.Y - b.Y);
        }
    }

    class MyEllipse : DrawShape
    {
        public MyEllipse(Bitmap bmp) : base(bmp) { }
        public MyEllipse(Event e) : base(e) { }

        protected override void ExtendShape(Point p)
        {
            image = new Bitmap(prev);
            using (Graphics g = Graphics.FromImage(image))
                g.DrawEllipse(pen, a.X, a.Y, p.X - a.X, p.Y - a.Y);
        }
    }
    class MyTriangle : DrawShape
    {
        public MyTriangle(Bitmap bmp) : base(bmp) { }
        public MyTriangle(Event e) : base(e) { }

        protected override void ExtendShape(Point p)
        {
            Point[] points = { new Point((a.X + p.X) / 2, a.Y), new Point(a.X, p.Y), new Point(p.X, p.Y) };
            image = new Bitmap(prev);
            using (Graphics g = Graphics.FromImage(image))
                g.DrawPolygon(pen, points);
        }
    }

    class Arrow : DrawShape
    {
        public Arrow(Bitmap bmp) : base(bmp) { }
        public Arrow(Event e) : base(e) { }

        protected override void ExtendShape(Point p)
        {
            Point[] points = { new Point((a.X + p.X) / 2, a.Y), new Point(a.X, (a.Y + p.Y) / 2), new Point((3 * a.X + p.X) / 4, (a.Y + p.Y) / 2),
                               new Point((3 * a.X + p.X) / 4, p.Y), new Point((a.X + 3 * p.X) / 4, p.Y), new Point((a.X + 3 * p.X) / 4, (a.Y + p.Y) / 2),
                               new Point(p.X, (a.Y + p.Y) / 2) };
            image = new Bitmap(prev);
            using (Graphics g = Graphics.FromImage(image))
                g.DrawPolygon(pen, points);
        }
    }
    class Star : DrawShape
    {
        public Star(Bitmap bmp) : base(bmp) { }
        public Star(Event e) : base(e) { }

        protected override void ExtendShape(Point p)
        {
            Point[] points = { new Point((a.X + p.X) / 2, a.Y), new Point((2 * a.X + p.X) / 3, (2 * a.Y + p.Y) / 3), new Point(a.X, (a.Y + p.Y) / 2),
                               new Point((2 * a.X + p.X) / 3, (a.Y + 2 * p.Y) / 3), new Point((a.X + p.X) / 2, p.Y), new Point((a.X + 2 * p.X) / 3, (a.Y + 2 * p.Y) / 3),
                               new Point(p.X, (a.Y + p.Y) / 2), new Point((a.X + 2 * p.X) / 3, (2 * a.Y + p.Y) / 3) };
            image = new Bitmap(prev);
            using (Graphics g = Graphics.FromImage(image))
                g.DrawPolygon(pen, points);
        }
    }
}
