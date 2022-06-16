using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyPaint
{
    class ColorPalete
    {

        Color[] cols = {Color.Black, Color.Gray, Color.Brown, Color.Red, Color.Orange,
                        Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple};
        Color color;
        Form1 form;
        Point a;
        Action<Color> ColorChangeHandler;
        Action<int> WidthChangeHandler;
        public ColorPalete(Form1 f, Point p, Action<Color> ColorChanged, Action<int> WidthChanged)
        {
            form = f; a = p;
            ColorChangeHandler = ColorChanged;
            WidthChangeHandler = WidthChanged;
            //
            // pen_button
            //
            Button pen_button = new Button();
            pen_button.Location = new Point(a.X, a.Y + 5);
            pen_button.Size = new Size(30, 30);
            pen_button.Text = "P";
            pen_button.TabIndex = 0;
            pen_button.Click += new EventHandler(PenButtonClicked);
            form.Controls.Add(pen_button);
            //
            // eraser_button
            //
            Button eraser_button = new Button();
            eraser_button.Location = new Point(a.X, a.Y + 45);
            eraser_button.Size = new Size(30, 30);
            eraser_button.Text = "E";
            eraser_button.TabIndex = 1;
            eraser_button.Click += new EventHandler(PenButtonClicked);
            form.Controls.Add(eraser_button);
            //
            // width_buttons
            //
            Button[] width_buttons = new Button[4];
            for (int i = 0; i < width_buttons.Length; i++)
            {
                width_buttons[i] = new Button();
                width_buttons[i].Location = new Point(a.X + 35, a.Y + 4 + i * 19);
                width_buttons[i].Size = new Size(70, 15);
                width_buttons[i].Text = Math.Pow(2, i).ToString();
                width_buttons[i].TabIndex = i + 2;
                width_buttons[i].Click += new EventHandler(WidthButtonClicked);
                form.Controls.Add(width_buttons[i]);
            }
            //
            // color_label
            //
            Label color_label = new Label();
            color_label.Location = new Point(a.X + 120, a.Y + 55);
            color_label.Size = new Size(40, 15);
            color_label.BackColor = Color.White;
            color_label.Text = "Color";
            color_label.TextAlign = ContentAlignment.MiddleCenter;
            form.Controls.Add(color_label);
            //
            // color_buttons
            //
            Button[] color_buttons = new Button[10];
            for (int i = 0; i < color_buttons.Length; i++)
            {
                color_buttons[i] = new Button();
                color_buttons[i].Location = new Point(a.X + 170 + 35 * i, a.Y + 25);
                color_buttons[i].Size = new Size(30, 30);
                color_buttons[i].BackColor = cols[i];
                color_buttons[i].TabIndex = i + 6;
                color_buttons[i].Click += new EventHandler(ColorButtonClicked);
                form.Controls.Add(color_buttons[i]);
            }
        }

        public void Load() { color = Color.Black; ColorChangeHandler(color); }

        public void Paint(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.White), a.X + 115, a.Y, 50, 80);
            g.DrawRectangle(new Pen(Color.Black), a.X + 115, a.Y, 50, 80);
            g.FillRectangle(new SolidBrush(color), a.X + 120, a.Y + 5, 40, 40);
        }

        private void ColorButtonClicked(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            Button b = (Button)sender;
            color = b.BackColor;
            ColorChangeHandler(color);
            form.Refresh();
        }

        private void PenButtonClicked(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            Button b = (Button)sender;
            if (b.Text == "P") ColorChangeHandler(color);
            else ColorChangeHandler(Color.FromArgb(240, 240, 240));
        }

        private void WidthButtonClicked(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            Button b = (Button)sender;
            WidthChangeHandler(int.Parse(b.Text));
        }
    }

    struct ToolboxProps
    {
        public Form1 form;
        public Point p;
        public int w, h;
        public Action<Color> ColorChanged;
        public Action<int> WidthChanged;

        public ToolboxProps(Form1 form, Point p, int w, int h, Action<Color> ColorChanged, Action<int> WidthChanged)
        {
            this.form = form;
            this.p = p;
            this.w = w; this.h = h;
            this.ColorChanged = ColorChanged;
            this.WidthChanged = WidthChanged;
        }
    }
    class Toolbox
    {
        Point a;
        int width, height;

        ColorPalete palete;
        public Toolbox(ToolboxProps props)
        {
            a = props.p; width = props.w; height = props.h;
            palete = new ColorPalete(props.form, new Point(a.X + 500, a.Y + 10), props.ColorChanged, props.WidthChanged);
        }

        public void Load() { palete.Load(); }

        public void Paint(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.LightBlue), a.X, a.Y, width, height);
            palete.Paint(g);
        }
    }
}
