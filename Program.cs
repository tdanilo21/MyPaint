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
        Event state;
        LinkedList<Bitmap> prev, next;

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
            form = new Form1(MouseMove, MouseUp, ColorChanged, WidthChanged, EventChanged, Undo, Redo);
            state = new Drawing(form.GetInitialScreenBitmap());

            prev = new LinkedList<Bitmap>();
            prev.AddFirst(new Bitmap(state.image));

            next = new LinkedList<Bitmap>();
        }

        #region Event Handlers
        private void ColorChanged(Color c)
        {
            state.ChangeColor(c);
        }

        private void WidthChanged(int w)
        {
            state.ChangeWidth(w);
        }

        private void MouseMove(Point p)
        {
            state.MouseMove(p);
            form.ShowScreen(state.image);
        }

        private void MouseUp()
        {
            state.Reset();
            prev.AddFirst(new Bitmap(state.image));
            next.Clear();
            if (prev.Count() > 100) prev.RemoveLast();
        }

        private void EventChanged(int i)
        {
            if (i == 0) state = new Drawing(state);
            else if (i == 1) state = new MyRectangle(state);
            else if (i == 2) state = new MyEllipse(state);
            else if (i == 3) state = new MyTriangle(state);
            else if (i == 4) state = new Arrow(state);
            else state = new Star(state);
        }

        private void Undo()
        {
            if (prev.Count() == 1) return;
            next.AddFirst(prev.First());
            prev.RemoveFirst();
            state.image = new Bitmap(prev.First());
            state.prev = new Bitmap(prev.First());
            form.ShowScreen(state.image);
        }

        private void Redo()
        {
            if (next.Count() == 0) return;
            prev.AddFirst(next.First());
            next.RemoveFirst();
            state.image = new Bitmap(prev.First());
            state.prev = new Bitmap(prev.First());
            form.ShowScreen(state.image);
        }
        #endregion
    }
}