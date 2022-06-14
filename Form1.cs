using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPaint
{
    public partial class Form1 : Form
    {
        Graphics g;
        Toolbox toolbox;
        Func<Point, int> MouseDownHandler;
        Func<int> MouseUpHandler;
        const int toolboxHeight = 100;
        bool mouse_down = false;
        public Form1(Func<Color, int> ColorChanged, Func<Point, int> MouseDown, Func<int> MouseUp)
        {
            InitializeComponent();
            g = CreateGraphics();
            ToolboxProps t = new ToolboxProps(this, new Point(0, 0), ClientRectangle.Width, toolboxHeight, ColorChanged);
            toolbox = new Toolbox(t);
            MouseDownHandler = MouseDown;
            MouseUpHandler = MouseUp;
        }
        public Graphics GetGraphics() { return g; }

        public Bitmap GetScreen()
        {
            Bitmap screen = new Bitmap(ClientRectangle.Width, ClientRectangle.Height - toolboxHeight);
            DrawToBitmap(screen, new Rectangle(new Point(0, 0), screen.Size));
            return screen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolbox.Load();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            toolbox.Paint(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y > toolboxHeight)
            {
                mouse_down = true;
                MouseDownHandler(new Point(e.X, e.Y));
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_down)
                MouseDownHandler(new Point(e.X, e.Y));
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
            MouseUpHandler();
        }
    }
}
