using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDZ2
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private Form2 form2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            graphics = pictureBox1.CreateGraphics();
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            comboBox1.SelectedIndex = 2;
            comboBox2.SelectedIndex = 1;

            form2 = new Form2(); 
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graphics.Clear(pictureBox1.BackColor);
            int circlesForRing = Convert.ToInt32(comboBox1.Text);
            int countOfIterations = Convert.ToInt32(comboBox2.Text);
            int width = 1200, height = 900, squareSide;
            int firstRow, secondRow;
            int cX = 0, cY = 0;
            firstRow = countOfIterations / 2 + countOfIterations % 2;
            secondRow = countOfIterations / 2;
            if (secondRow != 0) height /= 2;
            width /= firstRow;
            squareSide = Math.Min(width, height);
            PointF center = new PointF(squareSide / 2, squareSide / 2);
            Tree tree = new Tree(new Circle(center, squareSide / 2 - 10, Color.Black), circlesForRing);
            for (int i = 1; i < countOfIterations; i++)
            {
                tree.DrawFractal(graphics, cX, cY);
                if (i < firstRow)
                {
                    cX = i * squareSide;
                }
                else
                {
                    cY = squareSide;
                    cX = (i % firstRow) * squareSide;
                }
                tree.makeIterationForAllChild(i);
            }
            tree.DrawFractal(graphics, cX, cY);

            form2.GetTree(tree);
            form2.DrawTreeOnForm();
        }
    }
}