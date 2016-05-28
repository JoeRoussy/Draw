using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleSketchPad
{
    // EventArgs for event emited by a Shape when it is painted which encapsulates its position 
    class PaintedEventArgs : EventArgs
    {
        public Point Position;
        public long ID;

        public PaintedEventArgs(Point p, long id)
        {
            Position = p;
            ID = id;
        }
    }
}
