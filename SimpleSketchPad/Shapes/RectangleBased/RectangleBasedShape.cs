using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleSketchPad
{
    // Base class for shapes that are painted based on rectangles (like rectnagles and ellipses)
    abstract class RectangleBasedShape : Shape
    {
        public int x0;
        public int y0;
        public int x1;
        public int y1;
        public bool IsSpec; // If this is true we should actually draw a sqaure using the points as a reference

        // Current point may not be to the bottom right of origin so take care of this in ctor to have a nice rectangle saved for painting
        public RectangleBasedShape(int x0, int y0, int x_current, int y_current, Pen pen, bool isSpec)
        {
            if (x0 > x_current)
            {
                // Current is to the left of origin so they need to be flipped (note 0 is to the left)
                this.x0 = x_current;
                this.x1 = x0;
            }
            else
            {
                // Current is to the right of origin like normal
                this.x0 = x0;
                this.x1 = x_current;
            }

            if (y0 > y_current)
            {
                // Current is above origin so they need to be flipped (note 0 is high on the y-axis)
                this.y0 = y_current;
                this.y1 = y0;
            }
            else
            {
                // Current is below origin like normal
                this.y0 = y0;
                this.y1 = y_current;
            }

            Pen = pen;
            this.Pen.Width = 2;
            IsSpec = isSpec;
        }

        // These functions must be implemented by 
        /*
        public override void Paint(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override void Move(int x_translation, int y_translation)
        {
            throw new NotImplementedException();
        }

        public override void Paint(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override Point GetPosition()
        {
            throw new NotImplementedException();
        }
        */
    }
}
