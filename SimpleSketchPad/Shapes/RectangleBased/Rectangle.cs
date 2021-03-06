﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleSketchPad
{
    class Rectangle : RectangleBasedShape
    {
        // When making a rectangle just pass the arguments up to the ctor for RectangleBasedShape
        public Rectangle(int x0, int y0, int x_current, int y_current, Pen pen, bool isSpec) : base(x0, y0, x_current, y_current, pen, isSpec) { }

        public override void Paint(Graphics g)
        {
            if (IsSpec)
            {
                // Draw a square by enforcing letting the height be whatever the width of the rectangle would be
                g.DrawRectangle(Pen, x0, y0, x1 - x0, x1 - x0);
            }
            else
            {
                // Draw the true rectangle as defined by the points
                g.DrawRectangle(Pen, x0, y0, x1 - x0, y1 - y0); 
            }       
        }

        // To move a Rectangle adjust both its points based on the translation
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
            return new Rectangle(x0, y0, x1, y1, Pen, IsSpec);
        }
    }
}
