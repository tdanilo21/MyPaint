using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp1
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        static Form1 form;
        static IGraphics g;

        [STAThread]
        static void Run()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            Application.Run(form);
        }
        static void Main()
        {
            Run();
            g = new MyGraphics(form.GetGraphics());
            g.DrawLine(new Pen(Color.Black), new Point(10, 10), new Point(20, 20));
        }
    }
}
