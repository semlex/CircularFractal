using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace IDZ2
{
    public class Circle
    {
        private PointF _center;
        private float _R;
        private Color _color;
        
        public Circle (PointF center, float R, Color color)
        {
            _center = center;
            _R = R;
            _color = color;
        }

        public PointF Center
        {
            get { return _center; }
        }

        public float R
        {
            get { return _R; }
        }

        public Color Color
        {
            get { return _color; }
        }
        
        public void Draw(Graphics graphics, float cX, float cY)
        {
            Pen pen = new Pen(new SolidBrush(_color));
            float x = (float)(cX + _center.X - _R);
            float y = (float)(cY + _center.Y - _R);
            float radius = (float)(_R * 2);

            graphics.DrawEllipse(pen, x, y, radius, radius);
        }
    }
}
