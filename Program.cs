using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MyPaint
{
    class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        Form1 form;
        IGraphics g;
        Pen pen;
        bool mouse_down = false;
        Point prev;

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
        private int ColorChanged(Color c)
        {
            pen.Color = c;
            return 0;
        }

        private int MouseDown(Point p)
        {
            if (mouse_down)
            {
                g.DrawLine(pen, prev, p);
                ScreenChange();
            }
            mouse_down = true;
            prev = p;
            return 0;
        }

        private int MouseUp()
        {
            mouse_down = false;
            return 0;
        }

        private void ScreenChange()
        {
            Bitmap img = form.GetScreen();
        }

        private void Initialize()
        {
            pen = new Pen(Color.Black);
            form = new Form1(ColorChanged, MouseDown, MouseUp);
            g = new MyGraphics(form.CreateGraphics());
        }
    }
}
