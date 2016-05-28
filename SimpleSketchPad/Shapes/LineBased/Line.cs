using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleSketchPad
{
    class Line : Shape
    {
        public Point p0;
        public Point p1;

        public Line(Point p0, Point p1, Pen pen)
        {
            this.p0 = p0;
            this.p1 = p1;
            Pen = pen;
            Pen.Width = 2;
        }

        public override void Paint(Graphics g)
        {
            g.DrawLine(Pen, p0, p1);
        }

        // To move a line adjust both its points based on the translation
        public override void Move(int x_translation, int y_translation)
        {
            p0.X += x_translation;
            p0.Y += y_translation;
            p1.X += x_translation;
            p1.Y += y_translation;
        }

        public override Point GetPosition()
        {
            return p0;
        }

        public override Shape Copy()
        {
            return new Line(p0, p1, Pen);
        }
    }
}
