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
        Toolbox toolbox;
        const int toolboxHeight = 100;
        bool mouse_down = false;
        Action<Point> MouseMoveHandler;
        Action MouseUpHandler, UndoHandler, RedoHandler;
        public Form1(Action<Point> MouseMove, Action MouseUp, Action<Color> ColorChanged, Action<int> WidthChanged, Action<int> ShapeChanged, Action Undo, Action Redo)
        {
            InitializeComponent();
            ToolboxProps props = new ToolboxProps(this, new Point(0, 0), ClientRectangle.Width,
                                                    toolboxHeight, ColorChanged, WidthChanged, ShapeChanged);
            toolbox = new Toolbox(props);

            screen.Image = new Bitmap(ClientRectangle.Width, ClientRectangle.Height - toolboxHeight);
            screen.Location = new Point(0, toolboxHeight);
            screen.Size = new Size(ClientRectangle.Width, ClientRectangle.Height - toolboxHeight);
            
            MouseMoveHandler = MouseMove;
            MouseUpHandler = MouseUp;
            UndoHandler = Undo;
            RedoHandler = Redo;
        }

        public Bitmap GetInitialScreenBitmap()
        {
            return new Bitmap(screen.Image);
        }

        public void ShowScreen(Bitmap img)
        {
            screen.Image = img;
            Refresh();
        }

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            toolbox.Load();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            toolbox.Paint(e.Graphics);
        }
        private void Screen_MouseDown(object sender, MouseEventArgs e)
        {
            screen.Focus();
            mouse_down = true;
            MouseMoveHandler(new Point(e.X, e.Y));
        }
        private void Screen_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_down) MouseMoveHandler(new Point(e.X, e.Y));
        }
        private void Screen_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down = false;
            MouseUpHandler();
        }
        private void Screen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.Z) UndoHandler();
                else if (e.KeyCode == Keys.Y) RedoHandler();
            }
        }
        #endregion
    }
}