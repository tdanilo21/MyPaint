using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyPaint
{
    class State
    {
        public Pen pen;
        public bool MouseDown;
        public Point a;
        public System.Drawing.Drawing2D.GraphicsPath path;
        public Bitmap prev, screen;

        public State(Bitmap bmp)
        {
            pen = new Pen(Color.Black);
            MouseDown = false;
            path = new System.Drawing.Drawing2D.GraphicsPath();
            prev = new Bitmap(bmp);
            screen = new Bitmap(bmp);
        }

        public void ExtendPath(Point p)
        {
            path.AddLine(a, p);
            screen = new Bitmap(prev);
            using (Graphics g = Graphics.FromImage(screen))
                g.DrawPath(pen, path);
        }

        public void FinishPath()
        {
            prev = new Bitmap(screen);
            path.Reset();
            MouseDown = false;
        }
    }
    class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>

        Form1 form;
        State state;
        LinkedList<Bitmap> prev;

        [STAThread]
        static void Main() { new Program().Run(); }
        private void Run()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Initialize();
            Application.Run(form);
        }

        private void Initialize()
        {
            form = new Form1(MouseMove, MouseUp, ColorChanged, WidthChanged);
            state = new State(form.GetInitialScreenBitmap());
            prev = new LinkedList<Bitmap>();
            prev.AddFirst(new Bitmap(state.screen));
        }

        private void ColorChanged(Color c)
        {
            state.pen.Color = c;
        }

        private void WidthChanged(int w)
        {
            state.pen.Width = w;
        }

        private void MouseMove(Point p)
        {
            if (state.MouseDown)
            {
                state.ExtendPath(p);
                form.ShowScreen(state.screen);
            }
            state.MouseDown = true;
            state.a = p;
        }

        private void MouseUp()
        {
            state.FinishPath();
            prev.AddFirst(new Bitmap(state.screen));
            if (prev.Count() > 20) prev.RemoveLast();
        }

        private void Undo()
        {

        }
    }
}