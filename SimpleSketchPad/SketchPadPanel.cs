using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SimpleSketchPad
{ 
    // Extension of the Panel class with styles thats that reduce flickering on paint
    class SketchPadPanel : Panel
    {
        public List<Shape> Shapes;

        public event EventHandler<PaintedEventArgs> Painted;
        
        public SketchPadPanel()
        {
            this.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            // Initialize the list with a decent amount of shapes so inserting is efficient most of the time (in case list is not implemented as a linked list)
            Shapes = new List<Shape>(15);
        }

        // When this object needs to be redrawn, paint all the Shapes if there are any
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Shapes.Count == 0)
                return;

            //Shapes.ForEach(s => s.Paint(e.Graphics));
            foreach (Shape shape in Shapes)
            {
                shape.Paint(e.Graphics);
            }

            // Now iterate through the shapes and raise painted events (we do this to streamline the paint procedure)
            foreach (Shape shape in Shapes)
            {
                // Raised Painted event and paint the image
                if (Painted != null)
                {
                    Painted(this, new PaintedEventArgs(shape.GetPosition(), shape.ID));
                }
            }
        }
    }
}
