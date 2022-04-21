using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IDZ2
{
    public class Tree
    {
        Circle _node;
        List<Tree> _childNodes = new List<Tree>();
        int _circlesForRing;
        int _iterationNumber;
        bool _isFilled;

        PointF _nodeCenter;
        float HOffset = 15;
        float VOffset = 25;

        public Tree(Circle node, int circlesForRing, int iterationNumber = 1, bool isFilled = false)
        {
            _node = node;
            _circlesForRing = circlesForRing;
            _iterationNumber = iterationNumber;
            _isFilled = isFilled;
        }

        private Color GetColor(int num)
        {
            switch (num)
            {
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Green;
                case 4:
                    return Color.Purple;
                case 5:
                    return Color.DarkOrange;
                case 6:
                    return Color.Brown;
                default:
                    return Color.Red;
            }
        }

        private void makeIteration(int num)
        {
            float R1 = _node.R;
            float R2 = R1;
            float R3, R4;
            float angle;
            float xprev, yprev;
            PointF ringCircleCenter;
            Color ringColor, circleColor;
            ringColor = GetColor((num * 2) % 6);
            circleColor = GetColor((num*2) % 6 + 1);
            while (true)
            {
                R2 -= (float)0.001;
                R3 = (R1 - R2)/2;
                R4 = R1 - R3;
                angle = (float)(2 * Math.Asin(R3 / R4));
                if (_circlesForRing * R4 * angle >= 2 * Math.PI * R4) break;
            }

            _childNodes.Add(new Tree(new Circle(_node.Center, R2, circleColor), _circlesForRing, _iterationNumber + 1));
            ringCircleCenter = new PointF(_node.Center.X, _node.Center.Y + R2 + R3);
            for (int i = 0; i < _circlesForRing; i++)
            {
                _childNodes.Add(new Tree(new Circle(ringCircleCenter, R3, ringColor), _circlesForRing, _iterationNumber + 1));
                xprev = ringCircleCenter.X;
                yprev = ringCircleCenter.Y;
                ringCircleCenter.X = (float)((xprev - _node.Center.X) * Math.Cos(angle) - (yprev - _node.Center.Y) * Math.Sin(angle)) + _node.Center.X;
                ringCircleCenter.Y = (float)((xprev - _node.Center.X) * Math.Sin(angle) + (yprev - _node.Center.Y) * Math.Cos(angle)) + _node.Center.Y;
            }

            _isFilled = true;
        }

        public void makeIterationForAllChild(int num)
        {
            if (_childNodes.Count == 0)
            {
                makeIteration(num);
            }

            else if (_childNodes.TrueForAll(childNode => !childNode._isFilled))
            {
                _childNodes.ForEach(childNode => childNode.makeIteration(num));
            }

            else
            {
                _childNodes.ForEach(childNode => childNode.makeIterationForAllChild(num));
            }
        }

        public void DrawFractal(Graphics graphics, float cX, float cY)
        {
            _node.Draw(graphics, cX, cY);
            _childNodes.ForEach(childNode => childNode.DrawFractal(graphics, cX, cY));
        }

        private SizeF GetNodeSize(Graphics graphics, Font font)
        {
            return graphics.MeasureString(_iterationNumber.ToString(), font) + new SizeF(5, 8);
        }

        private void LocateNodes(Graphics graphics, Font font, ref float xmin, ref float ymin)
        {
            SizeF my_size = GetNodeSize(graphics, font);

            float x = xmin;
            float biggest_ymin = ymin + my_size.Height;
            float subtree_ymin = ymin + my_size.Height + VOffset;

            _childNodes.ForEach(childNode =>
            {
                float child_ymin = subtree_ymin;
                childNode.LocateNodes(graphics, font, ref x, ref child_ymin);

                if (biggest_ymin < child_ymin) biggest_ymin = child_ymin;

                x += HOffset;
            });

            if (_childNodes.Count > 0) x -= HOffset;

            float subtree_width = x - xmin;
            if (my_size.Width > subtree_width)
            {
                x = xmin + (my_size.Width - subtree_width) / 2;

                _childNodes.ForEach(childNode =>
                {
                    childNode.LocateNodes(graphics, font, ref x, ref subtree_ymin);

                    x += HOffset;
                });

                subtree_width = my_size.Width;
            }

            _nodeCenter = new PointF(
                xmin + subtree_width / 2,
                ymin + my_size.Height / 2);

            xmin += subtree_width;
            ymin = biggest_ymin;
        }

        private void DrawNode(Graphics graphics, float x, float y, Pen pen, Brush text_brush, Font font)
        {
            SizeF my_size = GetNodeSize(graphics, font);
            StringFormat string_format = new StringFormat();
            Brush bg_brush = new SolidBrush(_node.Color);
            RectangleF rect = new RectangleF(
                x - my_size.Width / 2,
                y - my_size.Height / 2,
                my_size.Width, my_size.Height);
            graphics.FillEllipse(bg_brush, rect);
            graphics.DrawEllipse(pen, rect);

            string_format.Alignment = StringAlignment.Center;
            string_format.LineAlignment = StringAlignment.Center;
            graphics.DrawString(_iterationNumber.ToString(), font, text_brush, x, y, string_format);
        }

        private void DrawNodes(Graphics graphics, Pen pen, Brush text_brush, Font font)
        {
            DrawNode(graphics, _nodeCenter.X, _nodeCenter.Y, pen, text_brush, font);
            _childNodes.ForEach(childNode => childNode.DrawNodes(graphics, pen, text_brush, font));
        }

        private void DrawNodeLinks(Graphics graphics, Pen pen)
        {
            _childNodes.ForEach(childNode =>
            {
                graphics.DrawLine(pen, _nodeCenter, childNode._nodeCenter);
                childNode.DrawNodeLinks(graphics, pen);
            });
        }

        public void DrawTree(Graphics graphics, Pen pen, Brush text_brush, Font font)
        {
            float xmin = 0, ymin = 150;
            LocateNodes(graphics, font, ref xmin, ref ymin);

            DrawNodeLinks(graphics, pen);
            DrawNodes(graphics, pen, text_brush, font);
        }
    }
}