using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IDZ2
{
    public partial class Form2 : Form
    {
        private Graphics graphics;
        private Tree tree;
        private Pen pen = Pens.Black;
        private Brush text_brush = Brushes.White;
        private Font font = new Font("Helvetica", 12);

        public Form2()
        {
            InitializeComponent();
            graphics = pictureBox1.CreateGraphics();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            graphics = pictureBox1.CreateGraphics();
        }

        public void GetTree(Tree treee)
        {
            tree = treee;
        }

        public void DrawTreeOnForm()
        {
            graphics.Clear(pictureBox1.BackColor);
            tree.DrawTree(graphics, pen, text_brush, font);
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            graphics.Clear(pictureBox1.BackColor);
            tree.DrawTree(graphics, pen, text_brush, font);
        }
    }
}