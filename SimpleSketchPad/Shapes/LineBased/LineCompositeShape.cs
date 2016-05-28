using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleSketchPad
{
    /* 
     * This class represents the structure of a Polygon and Free-Hand Sketch since they are both a collection of lines.
     * The difference between a Polygon and a Free-Hand Sketch are when the lines are commited to the shape which is handled
     * by the MainWindow class in the mouse event handlers. However, their data structre and paint method are the same so we
     * don't actually need two classes for both of them.
     */ 
    class LineCompositeShape : Shape
    {
        public List<Line> Lines;
        public Point Origin; // Gives a reference to the origin point for this shape for convenience
        public Point LastLineEnd; // Refernece to the end of the last line which is the start of any subsequent lines

        public LineCompositeShape(Point origin, Point p1, Pen pen)
        {
            Lines = new List<Line>(15); // Start off with a decent amount of lines
            Origin = origin;
            Pen = pen;
            Pen.Width = 2;

            Lines.Add(new Line(origin, p1, pen)); // Add the first line
            LastLineEnd = p1;
        }

        // Ctor used for copying
        public LineCompositeShape(Point origin, List<Line> lines, Pen pen)
        {
            Origin = origin;
            Lines = lines;
            Pen = pen;
        }

        public void AddLine(Point newEndPoint)
        {
            // Connect the new line to the end of the last line and then set the new line as the last line
            Lines.Add(new Line(LastLineEnd, newEndPoint, Pen));
            LastLineEnd = newEndPoint;
        }

        // When a LineCompositeShape is painted we need to paint all the lines that make it up
        public override void Paint(Graphics g)
        {
            Lines.ForEach(l => l.Paint(g));
        }

        // To Move a LineCompositeShape we need to move each line in it
        public override void Move(int x_translation, int y_translation)
        {
            Origin.X += x_translation;
            Origin.Y += y_translation;
            Lines.ForEach(l => l.Move(x_translation, y_translation));
        }

        public override Point GetPosition()
        {
            return Origin;
        }

        public override Shape Copy()
        {
            // Need to make copies of all the lines
            List<Line> newLines = new List<Line>();
            foreach (Line line in Lines)
            {
                newLines.Add(new Line(line.p0, line.p1, line.Pen));
            }

            return new LineCompositeShape(Origin, newLines, Pen);
        }
    }
}
