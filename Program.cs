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
        public Point prev;

        public State()
        {
            pen = new Pen(Color.Black);
            MouseDown = false;
        }
    }
    class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>

        Form1 form;
        Graphics g;
        State state;
        Bitmap screen;

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
            state = new State();
            screen = form.GetInitialScreenBitmap();
            g = Graphics.FromImage(screen);
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
                g.DrawLine(state.pen, state.prev, p);
                form.ShowScreen(screen);
            }
            state.MouseDown = true;
            state.prev = p;
        }

        private void MouseUp()
        {
            state.MouseDown = false;
        }
    }
}
