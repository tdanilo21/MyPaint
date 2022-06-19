using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MyPaint
{
    class ColorPalette
    {
        Color[] cols = {Color.Black, Color.Gray, Color.Brown, Color.Red, Color.Orange,
                        Color.Yellow, Color.Green, Color.LightBlue, Color.Blue, Color.Purple};
        Color color;
        Form1 form;
        Point a;
        Action<Color> ColorChangeHandler;
        Action<int> WidthChangeHandler, PenButtonClickHandler;
        public ColorPalette(Form1 f, Point p, Action<Color> ColorChanged, Action<int> WidthChanged, Action<int> _PenButtonClicked)
        {
            form = f; a = p;
            ColorChangeHandler = ColorChanged;
            WidthChangeHandler = WidthChanged;
            PenButtonClickHandler = _PenButtonClicked;

            string exeFile = (new Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);
            string fullDirPath = exeDir.Substring(0, exeDir.Length - 20) + "\\Images";
            //
            // pen_button
            //
            Button pen_button = new Button();
            pen_button.Location = new Point(a.X, a.Y + 5);
            pen_button.Size = new Size(30, 30);
            pen_button.Image = Image.FromFile(fullDirPath + "\\pen.png");
            pen_button.TabIndex = 0;
            pen_button.Click += new EventHandler(PenButtonClicked);
            form.Controls.Add(pen_button);
            //
            // eraser_button
            //
            Button eraser_button = new Button();
            eraser_button.Location = new Point(a.X, a.Y + 45);
            eraser_button.Size = new Size(30, 30);
            eraser_button.Image = Image.FromFile(fullDirPath + "\\eraser.png");
            eraser_button.TabIndex = 1;
            eraser_button.Click += new EventHandler(EraserButtonClicked);
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
                width_buttons[i].Image = Image.FromFile(fullDirPath + "\\width" + (i + 1).ToString() + ".png");
                width_buttons[i].TabIndex = i + 2;
            }
            width_buttons[0].Click += new EventHandler(Width1ButtonClicked);
            width_buttons[1].Click += new EventHandler(Width2ButtonClicked);
            width_buttons[2].Click += new EventHandler(Width3ButtonClicked);
            width_buttons[3].Click += new EventHandler(Width4ButtonClicked);
            for (int i = 0; i < width_buttons.Length; i++)
                form.Controls.Add(width_buttons[i]);
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
            //
            // color_dialog
            //
            Button color_dialog_button = new Button();
            color_dialog_button.Location = new Point(a.X + 525, a.Y + 5);
            color_dialog_button.Size = new Size(50, 40);
            color_dialog_button.Image = Image.FromFile(fullDirPath + "\\ColorDialog.png");
            color_dialog_button.Click += new EventHandler(ColorDialogButtonClicked);
            form.Controls.Add(color_dialog_button);
            Label color_dialog_label = new Label();
            color_dialog_label.Location = new Point(a.X + 525, a.Y + 45);
            color_dialog_label.Size = new Size(50, 30);
            color_dialog_label.BackColor = Color.White;
            color_dialog_label.Text = "More Colors";
            color_dialog_label.TextAlign = ContentAlignment.MiddleCenter;
            form.Controls.Add(color_dialog_label);
        }

        #region Even Handlers
        public void Load() { color = Color.Black; ColorChangeHandler(color); }

        public void Paint(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.Black);
            g.FillRectangle(brush, a.X + 520, a.Y, 60, 80);
            g.DrawRectangle(pen, a.X + 520, a.Y, 60, 80);

            g.FillRectangle(brush, a.X + 115, a.Y, 50, 80);
            g.DrawRectangle(pen, a.X + 115, a.Y, 50, 80);
            brush.Color = color;
            g.FillRectangle(brush, a.X + 120, a.Y + 5, 40, 40);
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
            PenButtonClickHandler(0);
            ColorChangeHandler(color);
        }

        private void EraserButtonClicked(object sender, EventArgs e)
        {
            PenButtonClickHandler(0);
            ColorChangeHandler(Color.FromArgb(240, 240, 240));
        }

        private void Width1ButtonClicked(object sender, EventArgs e) { WidthChangeHandler(1); }
        private void Width2ButtonClicked(object sender, EventArgs e) { WidthChangeHandler(2); }
        private void Width3ButtonClicked(object sender, EventArgs e) { WidthChangeHandler(4); }
        private void Width4ButtonClicked(object sender, EventArgs e) { WidthChangeHandler(8); }

        private void ColorDialogButtonClicked(object sender, EventArgs e)
        {
            ColorDialog color_dialog = new ColorDialog();
            color_dialog.ShowHelp = true;
            color_dialog.Color = color;
            if (color_dialog.ShowDialog() == DialogResult.OK)
            {
                color = color_dialog.Color;
                ColorChangeHandler(color_dialog.Color);
                form.Refresh();
            }
        }
        #endregion
    }

    class ShapePalette
    {
        Action<int> ShapeChangeHandler;

        public ShapePalette(Form1 f, Point p, Action<int> ShapeChanged)
        {
            ShapeChangeHandler = ShapeChanged;
            Button[] shape_buttons = new Button[5];
            for (int i = 0; i < shape_buttons.Length; i++)
            {
                shape_buttons[i] = new Button();
                shape_buttons[i].Location = new Point(p.X + 55 * i, p.Y);
                shape_buttons[i].Size = new Size(50, 50);
                shape_buttons[i].TabIndex = i;
            }
            string exeFile = (new Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);
            string fullDirPath = exeDir.Substring(0, exeDir.Length - 20) + "\\Images";
            //
            // Rectangle button
            //
            shape_buttons[0].Image = Image.FromFile(fullDirPath + "\\Rectangle.png");
            shape_buttons[0].Click += new EventHandler(RectangleButtonClicked);
            f.Controls.Add(shape_buttons[0]);
            //
            // Ellispe button
            //
            shape_buttons[1].Image = Image.FromFile(fullDirPath + "\\Ellipse.png");
            shape_buttons[1].Click += new EventHandler(EllipseButtonClicked);
            f.Controls.Add(shape_buttons[1]);
            //
            // Triangle button
            //
            shape_buttons[2].Image = Image.FromFile(fullDirPath + "\\Triangle.png");
            shape_buttons[2].Click += new EventHandler(TriangleButtonClicked);
            f.Controls.Add(shape_buttons[2]);
            //
            // Arrow button
            //
            shape_buttons[3].Image = Image.FromFile(fullDirPath + "\\Arrow.png");
            shape_buttons[3].Click += new EventHandler(ArrowButtonClicked);
            f.Controls.Add(shape_buttons[3]);
            //
            // Star button
            //
            shape_buttons[4].Image = Image.FromFile(fullDirPath + "\\Star.png");
            shape_buttons[4].Click += new EventHandler(StarButtonClicked);
            f.Controls.Add(shape_buttons[4]);
        }
        private void RectangleButtonClicked(object sender, EventArgs e) { ShapeChangeHandler(1); }
        private void EllipseButtonClicked(object sender, EventArgs e) { ShapeChangeHandler(2); }
        private void TriangleButtonClicked(object sender, EventArgs e) { ShapeChangeHandler(3); }
        private void ArrowButtonClicked(object sender, EventArgs e) { ShapeChangeHandler(4); }
        private void StarButtonClicked(object sender, EventArgs e) { ShapeChangeHandler(5); }
    }

    struct ToolboxProps
    {
        public Form1 form;
        public Point p;
        public int w, h;
        public Action<Color> ColorChanged;
        public Action<int> WidthChanged, ShapeChanged;

        public ToolboxProps(Form1 form, Point p, int w, int h, Action<Color> ColorChanged, Action<int> WidthChanged, Action<int> ShapeChanged)
        {
            this.form = form;
            this.p = p;
            this.w = w; this.h = h;
            this.ColorChanged = ColorChanged;
            this.WidthChanged = WidthChanged;
            this.ShapeChanged = ShapeChanged;
        }
    }
    class Toolbox
    {
        Point a;
        int width, height;

        ColorPalette color_palete;
        public Toolbox(ToolboxProps props)
        {
            a = props.p; width = props.w; height = props.h;
            color_palete = new ColorPalette(props.form, new Point(a.X + 300, a.Y + 10), props.ColorChanged, props.WidthChanged, props.ShapeChanged);
            new ShapePalette(props.form, new Point(a.X + 25, a.Y + 20), props.ShapeChanged);
        }

        public void Load() { color_palete.Load(); }

        public void Paint(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(105, 250, 240)), a.X, a.Y, width, height);
            color_palete.Paint(g);
        }
    }
}