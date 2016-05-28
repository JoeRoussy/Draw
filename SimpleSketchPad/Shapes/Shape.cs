using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleSketchPad
{
    // Main interface that all graphical objects must implement
    abstract class Shape
    {
        public long ID;
        public Pen Pen;
        
        // Paint this object using g.
        public abstract void Paint(Graphics g);

        // Translate the position of this shape based on some coordinates
        public abstract void Move(int x_translation, int y_translation);

        // Get the position of the shape
        public abstract Point GetPosition();

        // Perform a deep copy of the Shape
        public abstract Shape Copy();
    }
}
