using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleSketchPad
{
    class Ellipse : RectangleBasedShape
    {
        // When making an Ellipse just pass the arguments up to the ctor for RectangleBasedShape
        public Ellipse(int x0, int y0, int x_current, int y_current, Pen pen, bool isSpec) : base(x0, y0, x_current, y_current, pen, isSpec) { }

        public override void Paint(Graphics g)
        {
            if (IsSpec)
            {
                // Draw a circle by enforcing letting the height be whatever the width of the bounding rectangle would be
                g.DrawEllipse(Pen, x0, y0, x1 - x0, x1 - x0);
            }
            else
            {
                // Draw the true ellipse as defined by the points
                g.DrawEllipse(Pen, x0, y0, x1 - x0, y1 - y0);
            }  
        }

        // To move a Ellipse adjust both its points based on the translation
        public override void Move(int x_translation, int y_translation)
        {
            x0 += x_translation;
            y0 += y_translation;
            x1 += x_translation;
            y1 += y_translation;
        }

        public override Point GetPosition()
        {
            return new Point(x0, y0);
        }

        public override Shape Copy()
        {
            return new Ellipse(x0, y0, x1, y1, Pen, IsSpec);
        }
    }
}
