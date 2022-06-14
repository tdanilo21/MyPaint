using System;
using System.Collections.Generic;
using System.Text;
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
        Func<Color, int> ColorChanged;
        public ColorPalete(Form1 f, Point p, Func<Color, int> callback)
        {
            form = f; a = p;
            ColorChanged = callback;
            form.ColorLabel.Location = new Point(a.X + 5, a.Y + 55);
            form.ColorLabel.Size = new Size(40, 15);
            form.ColorLabel.BackColor = Color.White;
            form.ColorLabel.Text = "Color";
            form.ColorLabel.TextAlign = ContentAlignment.MiddleCenter;
            form.Controls.Add(form.ColorLabel);
            for (int i = 0; i < form.ColorButtons.Length; i++)
            {
                form.ColorButtons[i] = new Button();
                form.ColorButtons[i].Location = new Point(a.X + 55 + 35 * i, a.Y + 25);
                form.ColorButtons[i].Size = new Size(30, 30);
                form.ColorButtons[i].BackColor = cols[i];
                form.ColorButtons[i].Click += new EventHandler(ColorButtonClicked);
                form.Controls.Add(form.ColorButtons[i]);
            }
        }

        public void Load() { color = Color.Black; ColorChanged(color); }

        public void Paint(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.White), a.X, a.Y, 50, 80);
            g.DrawRectangle(new Pen(Color.Black), a.X, a.Y, 50, 80);
            g.FillRectangle(new SolidBrush(color), a.X + 5, a.Y + 5, 40, 40);
        }

        private void ColorButtonClicked(object sender, EventArgs e)
        {
            if (!(sender is Button)) return;
            Button b = (Button)sender;
            color = b.BackColor;
            form.Refresh();
            ColorChanged(color);
        }
    }

    struct ToolboxProps
    {
        public Form1 form;
        public Point p;
        public int w, h;
        public Func<Color, int> ColorChanged;

        public ToolboxProps(Form1 form, Point p, int w, int h, Func<Color, int> ColorChanged)
        {
            this.form = form;
            this.p = p;
            this.w = w; this.h = h;
            this.ColorChanged = ColorChanged;
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
            palete = new ColorPalete(props.form, new Point(a.X + 500, a.Y + 10), props.ColorChanged);
        }

        public void Load() { palete.Load(); }

        public void Paint(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.LightBlue), a.X, a.Y, width, height);
            palete.Paint(g);
        }
    }
}
