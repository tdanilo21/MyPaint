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
        public Event()
        {
            pen = new Pen(Color.Black);
            mouseDown = false;
        }

        public Event(Event e)
        {
            pen = e.pen;
            mouseDown = false;
        }
        public Color Color
        {
            get { return pen.Color; }
            set { pen.Color = value; }
        }
        public int PenWidth
        {
            set
            {
                if (value <= 0) throw new Exception("Width of a pen cannot be less than or equal to zero.");
                pen.Width = value;
            }
        }
        public abstract void Update(IGraphics<MyGraphics> g, Point p);
        public virtual void Reset() { mouseDown = false; }
    }

    abstract class HandDrawing : Event
    {
        protected List<Point> path;
        public HandDrawing() : base()
        {
            path = new List<Point>();
        }

        public HandDrawing(Event e) : base(e)
        {
            path = new List<Point>();
        }
        protected abstract void Show(IGraphics<MyGraphics> g);
        public override void Update(IGraphics<MyGraphics> g, Point p)
        {
            if (mouseDown)
            {
                path.Add(p);
                Show(g);
            }
            mouseDown = true;
            a = p;
        }
        public override void Reset()
        {
            base.Reset();
            path.Clear();
        }
    }

    class Draw : HandDrawing
    {
        public Draw() : base() { }
        public Draw(Event e) : base(e) { }
        protected override void Show(IGraphics<MyGraphics> g)
        {
            g.DrawPath(pen, path.ToArray());
        }
    }
    class Erase : HandDrawing
    {
        public Erase() : base() { }
        public Erase(Event e) : base(e) { }
        protected override void Show(IGraphics<MyGraphics> g)
        {
            Color c = pen.Color;
            pen.Color = Color.FromArgb(240, 240, 240);
            g.DrawPath(pen, path.ToArray());
            pen.Color = c;
        }
    }

    abstract class DrawShape : Event
    {
        public DrawShape() : base() { }
        public DrawShape(Event e) : base(e) { }

        protected abstract void ExtendShape(IGraphics<MyGraphics> g, Point p);

        public override void Update(IGraphics<MyGraphics> g, Point p)
        {
            if (mouseDown) ExtendShape(g, p);
            else a = p;
            mouseDown = true;
        }
    }

    class StraightLine : DrawShape
    {
        public StraightLine() : base() { }
        public StraightLine(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            g.DrawLine(pen, a, p);
        }
    }

    class MyRectangle : DrawShape
    {
        public MyRectangle() : base() { }
        public MyRectangle(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            Point b = new Point(Math.Min(a.X, p.X), Math.Min(a.Y, p.Y));
            Point c = new Point(Math.Max(a.X, p.X), Math.Max(a.Y, p.Y));
            g.DrawRectangle(pen, b.X, b.Y, c.X - b.X, c.Y - b.Y);
        }
    }

    class MyEllipse : DrawShape
    {
        public MyEllipse() : base() { }
        public MyEllipse(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            g.DrawEllipse(pen, a.X, a.Y, p.X - a.X, p.Y - a.Y);
        }
    }
    class MyTriangle : DrawShape
    {
        public MyTriangle() : base() { }
        public MyTriangle(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            Point[] points = { new Point((a.X + p.X) / 2, a.Y), new Point(a.X, p.Y), new Point(p.X, p.Y) };
            g.DrawPolygon(pen, points);
        }
    }

    class VerticalArrow : DrawShape
    {
        public VerticalArrow() : base() { }
        public VerticalArrow(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            Point[] points = { new Point((a.X + p.X) / 2, a.Y), new Point(a.X, (a.Y + p.Y) / 2), new Point((3 * a.X + p.X) / 4, (a.Y + p.Y) / 2),
                               new Point((3 * a.X + p.X) / 4, p.Y), new Point((a.X + 3 * p.X) / 4, p.Y), new Point((a.X + 3 * p.X) / 4, (a.Y + p.Y) / 2),
                               new Point(p.X, (a.Y + p.Y) / 2) };
            g.DrawPolygon(pen, points);
        }
    }
    class HorizontalArrow : DrawShape
    {
        public HorizontalArrow() : base() { }
        public HorizontalArrow(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            Point[] points = { new Point(p.X, (a.Y + p.Y) / 2), new Point((a.X + p.X) / 2, a.Y), new Point((a.X + p.X) / 2, (3 * a.Y + p.Y) / 4),
                               new Point(a.X, (3 * a.Y + p.Y) / 4),  new Point(a.X, (a.Y + 3 * p.Y) / 4), new Point((a.X + p.X) / 2, (a.Y + 3 * p.Y) / 4),
                               new Point((a.X + p.X) / 2, p.Y) };
            g.DrawPolygon(pen, points);
        }
    }
    class Star : DrawShape
    {
        public Star() : base() { }
        public Star(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            Point[] points = { new Point((a.X + p.X) / 2, a.Y), new Point((2 * a.X + p.X) / 3, (2 * a.Y + p.Y) / 3), new Point(a.X, (a.Y + p.Y) / 2),
                               new Point((2 * a.X + p.X) / 3, (a.Y + 2 * p.Y) / 3), new Point((a.X + p.X) / 2, p.Y), new Point((a.X + 2 * p.X) / 3, (a.Y + 2 * p.Y) / 3),
                               new Point(p.X, (a.Y + p.Y) / 2), new Point((a.X + 2 * p.X) / 3, (2 * a.Y + p.Y) / 3) };
            g.DrawPolygon(pen, points);
        }
    }
    class Heart : DrawShape
    {
        public Heart() : base() { }
        public Heart(Event e) : base(e) { }

        protected override void ExtendShape(IGraphics<MyGraphics> g, Point p)
        {
            Point b = new Point(Math.Min(a.X, p.X), Math.Min(a.Y, p.Y));
            Point c = new Point(Math.Max(a.X, p.X), Math.Max(a.Y, p.Y));
            g.DrawBezier(pen, (b.X + c.X) / 2, (13 * b.Y + 7 * c.Y) / 20, (5 * c.X - b.X) / 4, b.Y, c.X, (b.Y + 3 * c.Y) / 4, (b.X + c.X) / 2, c.Y);
            g.DrawBezier(pen, (b.X + c.X) / 2, (13 * b.Y + 7 * c.Y) / 20, (5 * b.X - c.X) / 4, b.Y, b.X, (b.Y + 3 * c.Y) / 4, (b.X + c.X) / 2, c.Y);
        }
    }
}