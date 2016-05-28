using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace SimpleSketchPad
{
    // Sketch Pad States
    enum PadStates { ReadyToDraw, DrawingShape, Selecting, Moving };

    // Selected Shapes Enum
    enum SelectedShapeType { Rectangle, Ellipse, Line, Polygon, FreeHand };

    // Shape Selector Cb Selected Index Enum
    enum ShapeSelectorCbSelectedIndex { Rectangle, Ellipse, Line, Polygon, FreeHand };

    // Misc Constants
    enum Constants { OriginMarkerRadius = 10 };
    
    public partial class MainWindow : Form
    {
        private Graphics PanelGraphics;
        private SelectedShapeType SelectedShapeType = SelectedShapeType.FreeHand;
        private Color SelectedColor = Color.Black;
        private PadStates State = PadStates.ReadyToDraw;
        private bool IsSpec = false; // Flag that is true if shift is being held down and false otherwise. Used to draw special versions of shapes
        private bool IsCtrlKeyHeld = false;
        private long ID = 1; // Incrementing ID assigned to each Shape upon addition to the Panel

        private Shape CurrentShape = null;
        private Ellipse OriginMarker = null;
        private Line CurrentPolygonLine = null;
        private List<Shape> SelectedShapes;
        private List<Shape> Clipboard;

        private const string DrawingModeDisplay = "Current Mode: Drawing";
        private const string SelectingModeDisplay = "Current Mode: Selecting";
        private const string DrawingPolygonDisplay = "Click to mark vertices. Press ENTER or ESC to finish drawing polygon.";
        private const string DrawingOtherShapeDisplay = "Click and drag to draw shape.";
        private const string SelectingModeHelpDisplay = "Use the list to select some objects. Click and drag to move the selected object(s).";

        // Misc Coordinate variables for drawing different shapes (represent the origin in different contexts)
        private int x0 = 0;
        private int y0 = 0;

        // Reference to old mouse position when moving shapes
        private Point OldMouseLocation;

        private delegate void TextUpdateDelegate(ListViewItem.ListViewSubItem item, Point p);
        
        public MainWindow()
        {
            InitializeComponent();

            PanelGraphics = Panel.CreateGraphics();
            SelectedShapes = new List<Shape>(15);
            Clipboard = new List<Shape>(15);
            OldMouseLocation = new Point();

            this.KeyDown += new KeyEventHandler(OnMainWindowKeyDown);
            this.KeyUp += new KeyEventHandler(OnMainWindowKeyUp);

            Panel.MouseDown += new MouseEventHandler(OnPanelMouseDown);
            Panel.MouseUp += new MouseEventHandler(OnPanelMouseUp);
            Panel.MouseMove += new MouseEventHandler(OnPanelMouseMove);
            Panel.Painted += new EventHandler<PaintedEventArgs>(OnPanelPained);

            ShapeSelectorCb.SelectedIndexChanged += new EventHandler(OnShapeSelectorCbIndexChanged);
            ColorSelectorBtn.Click += new EventHandler(OnColorSelectorBtnClick);
            ShapesLv.SelectedIndexChanged += new EventHandler(OnShapesLvSelectedIndexChanged);

            CutBtn.Click += new EventHandler(OnCutBtnClick);
            CopyBtn.Click += new EventHandler(OnCopyBtnClick);
            PasteBtn.Click += new EventHandler(OnPasteBtnClick);

            ToggleModeBtn.Click += new EventHandler(OnToggleModeBtnClick);
            ShowHelpCb.CheckedChanged += new EventHandler(OnShowHelpCbCheckChanged);

            ShapeSelectorCb.SelectedIndex = (int)ShapeSelectorCbSelectedIndex.FreeHand;
        }

        // Copy can be done using a key stroke or a button click which both call this function
        private void OnCopy()
        {
            // Put copies of all the selected shapes in the clipboard
            Clipboard.Clear();
            foreach (Shape shape in SelectedShapes)
            {
                Clipboard.Add(shape.Copy());
            }
        }

        // Copy can be done using a key stroke or a button click which both call this function
        private void OnCut()
        {
            // Put all the selected shapes in the clipboard and remove them from the Shapes list
            Clipboard.Clear();
            Shape[] selectedCopy = new Shape[SelectedShapes.Count]; // Need to copy so we dont have a crash as SelectedShapes is changing
            SelectedShapes.CopyTo(selectedCopy);
            foreach (Shape shape in selectedCopy)
            {
                Clipboard.Add(shape);
                int index = Panel.Shapes.IndexOf(shape);
                Panel.Shapes.Remove(shape);
                ShapesLv.Items.RemoveAt(index);
            }
            Panel.Invalidate();
        }

        // Copy can be done using a key stroke or a button click which both call this function
        private void OnPaste()
        {
            // Take all the shapes from the clipboard and draw deep copies of them
            foreach (Shape shape in Clipboard)
            {
                string shapeType;
                if (shape is Rectangle)
                    shapeType = "Rectangle";
                else if (shape is Ellipse)
                    shapeType = "Ellipse";
                else if (shape is Line)
                    shapeType = "Line";
                else
                    shapeType = "Line Collection";
                    
                shape.Move(-50, -50); // Shift the pasted image up and to the left
                Shape newShape = shape.Copy();
                newShape.ID = this.ID++;
                Panel.Shapes.Add(newShape);

                ListViewItem item = new ListViewItem(Convert.ToString(newShape.ID));
                item.SubItems.Add(shapeType);
                item.SubItems.Add(newShape.Pen.Color.ToString().Split('[')[1].Split(']')[0]);
                item.SubItems.Add(newShape.GetPosition().X + "," + newShape.GetPosition().Y);
                ShapesLv.Items.Add(item);
            }
            Panel.Invalidate();
        }

        private Color GetOriginMarkerColor()
        {
            // By default the marker should be orange but if the drawing is orangle the marker should be blue
            if (SelectedColor == Color.Orange)
            {
                return Color.Blue;
            }
            else
            {
                return Color.Orange;
            }
        }

        // Shows the appropriate help text if the cb is checked and we are in a drawing state
        private void ShowHelpText()
        {
            if (ShowHelpCb.Checked)
            {
                if (State == PadStates.ReadyToDraw || State == PadStates.DrawingShape)
                {
                    if (SelectedShapeType == SelectedShapeType.Polygon)
                    {
                        DrawingHelpLbl.Text = DrawingPolygonDisplay;
                    }
                    else
                    {
                        DrawingHelpLbl.Text = DrawingOtherShapeDisplay;
                    }
                }
                else
                {
                    // We are in the selecting state
                    DrawingHelpLbl.Text = SelectingModeHelpDisplay;
                }   
            }
            else
            {
                DrawingHelpLbl.Text = "";
            }
        }

        // Toggles the operating mode between drawing and selecting
        private void ToggleMode()
        {
            if (State == PadStates.DrawingShape)
            {
                return; // Can't toggle modes while drawing
            }
            else if (State == PadStates.ReadyToDraw)
            {
                State = PadStates.Selecting;
                ModeLbl.Text = SelectingModeDisplay;
                ShowHelpText();
            }
            else if (State == PadStates.Selecting)
            {
                State = PadStates.ReadyToDraw;
                ModeLbl.Text = DrawingModeDisplay;
                ShowHelpText();
            }
        }


        // ----- Event Handlers -----
        private void OnMainWindowKeyDown(object sender, KeyEventArgs e)
        {
            // Set the IsSpec property to true if the shift button is being held
            if (e.KeyCode == Keys.ShiftKey)
                IsSpec = true;
            else if (e.KeyCode == Keys.ControlKey)
                IsCtrlKeyHeld = true;
            else if (e.KeyCode == Keys.C && IsCtrlKeyHeld)
                OnCopy();
            else if (e.KeyCode == Keys.X && IsCtrlKeyHeld)
                OnCut();
            else if (e.KeyCode == Keys.V && IsCtrlKeyHeld)
                OnPaste();
        }

        private void OnMainWindowKeyUp(object sender, KeyEventArgs e)
        {
            // Set the IsSpec property to false if the shift buttin is not being held any more
            if (e.KeyCode == Keys.ShiftKey)
            {
                IsSpec = false;
            }
            else if (State == PadStates.DrawingShape && SelectedShapeType == SelectedShapeType.Polygon &&
                    (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter))
            {
                // When drawing a polygon, the user can finish by pressing the enter or esc keys as oppse to clicking up like with other shapes
                if (CurrentPolygonLine != null)
                    Panel.Shapes.Remove(CurrentPolygonLine);
                if (OriginMarker != null)
                    Panel.Shapes.Remove(OriginMarker);

                // Add new polygon to list
                ListViewItem item = new ListViewItem(Convert.ToString(CurrentShape.ID));
                item.SubItems.Add("Polygon");
                item.SubItems.Add(SelectedColor.ToString().Split('[')[1].Split(']')[0]);
                item.SubItems.Add(((LineCompositeShape)CurrentShape).Origin.X + "," + ((LineCompositeShape)CurrentShape).Origin.Y);
                ShapesLv.Items.Add(item);

                // Go back to ready to draw 
                CurrentShape = null;
                CurrentPolygonLine = null;
                OriginMarker = null;
                State = PadStates.ReadyToDraw;
                Panel.Invalidate(); // Clear the temp line drawn for the polygon since it is now null
            }
            else if (e.KeyCode == Keys.M)
            {
                // M is a hot key for toggling the mode
                ToggleMode();
            }
            else if (e.KeyCode == Keys.ControlKey)
            {
                IsCtrlKeyHeld = false;
            }
        }

        private void OnPanelMouseDown(object sender, MouseEventArgs e)
        {
            switch (State)
            {
                case PadStates.ReadyToDraw:
                    // Set the initial points and let mouse move handle actually drawing the shape
                    x0 = e.X;
                    y0 = e.Y;
                    State = PadStates.DrawingShape;
                    break;
                case PadStates.Selecting:
                    if (SelectedShapes.Count != 0)
                    {
                        State = PadStates.Moving;
                        OldMouseLocation = e.Location;
                    }
                    break;
            }
        }

        private void OnPanelMouseUp(object sender, MouseEventArgs e)
        {
            switch (State)
            {
                case PadStates.DrawingShape:
                    if (SelectedShapeType == SelectedShapeType.Polygon)
                    {
                        // Mouse up is when we add the current line to the polygon and set the new origin points
                        if (CurrentShape == null)
                        {
                            if (CurrentPolygonLine == null)
                            {
                                // There is no line yet (it is still being made, this can happen when the user clicks down and up quickly without moving the mouse)
                                // This allows user to start a polygon with a drag or a click.
                                return;
                            }
                            // There is no polygon yet so create it and add it to the panel
                            CurrentShape = new LineCompositeShape(CurrentPolygonLine.p0, CurrentPolygonLine.p1, new Pen(SelectedColor));
                            CurrentShape.ID = ID++;
                            Panel.Shapes.Remove(CurrentPolygonLine);
                            Panel.Shapes.Add(CurrentShape);
                            x0 = CurrentPolygonLine.p1.X;
                            y0 = CurrentPolygonLine.p1.Y;
                            Panel.Invalidate();
                        }
                        else
                        {
                            // There is already a polygon in the panel so just add the current line to it
                            ((LineCompositeShape)CurrentShape).AddLine(CurrentPolygonLine.p1);
                            Panel.Shapes.Remove(CurrentPolygonLine);
                            x0 = CurrentPolygonLine.p1.X;
                            y0 = CurrentPolygonLine.p1.Y;
                            Panel.Invalidate();
                        }
                    }
                    else
                    {
                        // The shape is not a polygon so it is done being drawn and is already in Shapes so just add it to the ListView of Shapes
                        ListViewItem item = new ListViewItem(Convert.ToString(CurrentShape.ID));
                        switch (SelectedShapeType)
                        {
                            case SelectedShapeType.Rectangle:
                                item.SubItems.Add("Rectangle");
                                item.SubItems.Add(SelectedColor.ToString().Split('[')[1].Split(']')[0]);
                                item.SubItems.Add(((RectangleBasedShape)CurrentShape).x0 + "," + ((RectangleBasedShape)CurrentShape).y0);
                                ShapesLv.Items.Add(item);
                                break;
                            case SelectedShapeType.Ellipse:
                                item.SubItems.Add("Ellipse");
                                item.SubItems.Add(SelectedColor.ToString().Split('[')[1].Split(']')[0]);
                                item.SubItems.Add(((RectangleBasedShape)CurrentShape).x0 + "," + ((RectangleBasedShape)CurrentShape).y0);
                                ShapesLv.Items.Add(item);
                                break;
                            case SelectedShapeType.Line:
                                item.SubItems.Add("Line");
                                item.SubItems.Add(SelectedColor.ToString().Split('[')[1].Split(']')[0]);
                                item.SubItems.Add(((Line)CurrentShape).p0.X + "," + ((Line)CurrentShape).p0.Y);
                                ShapesLv.Items.Add(item);
                                break;
                            case SelectedShapeType.FreeHand:
                                item.SubItems.Add("Free-Hand");
                                item.SubItems.Add(SelectedColor.ToString().Split('[')[1].Split(']')[0]);
                                item.SubItems.Add(((LineCompositeShape)CurrentShape).Origin.X + "," + ((LineCompositeShape)CurrentShape).Origin.Y);
                                ShapesLv.Items.Add(item);
                                break;
                        }
                        
                        //Set the current shape to null and go back to ready.
                        CurrentShape = null;
                        State = PadStates.ReadyToDraw;
                    }
                    break;
                case PadStates.Moving:
                    State = PadStates.Selecting;
                    break;
            }
        }

        private void OnPanelMouseMove(object sender, MouseEventArgs e)
        {
            switch (State)
            {
                case PadStates.DrawingShape:
                    switch (SelectedShapeType)
                    {
                        case SelectedShapeType.Rectangle:
                            if (CurrentShape == null)
                            {
                                // The Rectangle is not being drawn yet so create it using the origin and save to CurrentShape. Then push it onto the list of shapes and update Panel
                                CurrentShape = new Rectangle(x0, y0, e.X, e.Y, new Pen(SelectedColor), IsSpec);
                                CurrentShape.ID = ID++;
                                Panel.Shapes.Add(CurrentShape);
                                Panel.Invalidate();
                            }
                            else
                            {
                                // CurrentShape is already the shape we are drawing so remove it from shapes and replace with the new rectangle and update panel.
                                long oldId = CurrentShape.ID;
                                Panel.Shapes.Remove(CurrentShape);
                                CurrentShape = new Rectangle(x0, y0, e.X, e.Y, new Pen(SelectedColor), IsSpec);
                                CurrentShape.ID = oldId;
                                Panel.Shapes.Add(CurrentShape);
                                Panel.Invalidate();
                            }
                            break;
                        case SelectedShapeType.Ellipse:
                            if (CurrentShape == null)
                            {
                                // The Ellipse is not being drawn yet so create it using the origin and save to CurrentShape. Then push it onto the list of shapes and update Panel
                                CurrentShape = new Ellipse(x0, y0, e.X, e.Y, new Pen(SelectedColor), IsSpec);
                                CurrentShape.ID = ID++;
                                Panel.Shapes.Add(CurrentShape);
                                Panel.Invalidate();
                            }
                            else
                            {
                                // CurrentShape is already the shape we are drawing so remove it from shapes and replace with the new ellipse and update panel.
                                long oldId = CurrentShape.ID;
                                Panel.Shapes.Remove(CurrentShape);
                                CurrentShape = new Ellipse(x0, y0, e.X, e.Y, new Pen(SelectedColor), IsSpec);
                                CurrentShape.ID = oldId;
                                Panel.Shapes.Add(CurrentShape);
                                Panel.Invalidate();
                            }
                            break;
                        case SelectedShapeType.Line:
                            if (CurrentShape == null)
                            {
                                // The Line is not being drawn yet so create it using the origin and save to CurrentShape. Then push it onto the list of shapes and update Panel
                                CurrentShape = new Line(new Point(x0, y0), new Point(e.X, e.Y), new Pen(SelectedColor));
                                CurrentShape.ID = ID++;
                                Panel.Shapes.Add(CurrentShape);
                                Panel.Invalidate();
                            }
                            else
                            {
                                // CurrentShape is already the shape we are drawing so remove it from shapes and replace with the new line and update panel.
                                long oldId = CurrentShape.ID;
                                Panel.Shapes.Remove(CurrentShape);
                                CurrentShape = new Line(new Point(x0, y0), new Point(e.X, e.Y), new Pen(SelectedColor));
                                CurrentShape.ID = oldId;
                                Panel.Shapes.Add(CurrentShape);
                                Panel.Invalidate();
                            }
                            break;
                        case SelectedShapeType.Polygon:
                            // During Mouse Move making a polygon is like making a line
                            if (CurrentPolygonLine == null)
                            {
                                // This is the first line in the polygon so create the shape itself and the origin marker
                                CurrentPolygonLine = new Line(new Point(x0, y0), new Point(e.X, e.Y), new Pen(SelectedColor));
                                OriginMarker = new Ellipse(x0 - (int)Constants.OriginMarkerRadius, y0 - (int)Constants.OriginMarkerRadius,
                                    e.X + (int)Constants.OriginMarkerRadius, e.Y + (int)Constants.OriginMarkerRadius, new Pen(GetOriginMarkerColor()), true);
                                Panel.Shapes.Add(CurrentPolygonLine);
                                Panel.Shapes.Add(OriginMarker);
                                Panel.Invalidate();
                            }
                            else
                            {
                                // The Polygon already exists so just make a new line
                                Panel.Shapes.Remove(CurrentPolygonLine);
                                if ((e.X > OriginMarker.x0 && e.X < OriginMarker.x1) && (e.Y > OriginMarker.y0 && e.Y < OriginMarker.y1) && CurrentShape != null)
                                {
                                    // The mouse is currently inside the origin of the polygon so snap the end of the line to the polygon's origin
                                    CurrentPolygonLine = new Line(new Point(x0, y0), ((LineCompositeShape)CurrentShape).Origin, new Pen(SelectedColor));
                                }
                                else
                                {
                                    // The mouse is not in the origin so make the line normally
                                    CurrentPolygonLine = new Line(new Point(x0, y0), new Point(e.X, e.Y), new Pen(SelectedColor));
                                }   
                                Panel.Shapes.Add(CurrentPolygonLine);
                                Panel.Invalidate();
                            }
                            break;
                        case SelectedShapeType.FreeHand:
                            if (CurrentShape == null)
                            {
                                // This is the first small line in the Skectch so create the shape itself and print the first line to the screen
                                CurrentShape = new LineCompositeShape(new Point(x0, y0), new Point(e.X, e.Y), new Pen(SelectedColor));
                                CurrentShape.ID = ID++;
                                Panel.Shapes.Add(CurrentShape);
                                Panel.Invalidate();
                            }
                            else
                            {
                                // The Freehand sketch already exists and the Panel has a reference to it so just add the new line to it
                                ((LineCompositeShape)CurrentShape).AddLine(new Point(e.X, e.Y));
                                Panel.Invalidate();
                            }
                            break;
                    }
                    break;
                case PadStates.Moving:
                    // Move all selected shapes based on old and current mouse positions and update panel
                    foreach (Shape shape in SelectedShapes)
                    {
                        shape.Move(e.X - OldMouseLocation.X, e.Y - OldMouseLocation.Y);
                    }
                    OldMouseLocation.X = e.X;
                    OldMouseLocation.Y = e.Y;
                    Panel.Invalidate();
                    break;
            }
        }

        private void OnPanelPained(object sender, PaintedEventArgs e)
        {
            // Update the shape with the new position by ID in the ShapesLv
            foreach (ListViewItem item in ShapesLv.Items)
            {
                if (item.Text == Convert.ToString(e.ID))
                {
                    item.SubItems[3].Text = e.Position.X + "," + e.Position.Y;
                }
            }
        }

        private void OnShapeSelectorCbIndexChanged(object sender, EventArgs e)
        {
            // Change the selected shape property based on the new value of the combo box. Assume user wants to draw a shape now.
            if (ShapeSelectorCb.SelectedIndex == (int)ShapeSelectorCbSelectedIndex.Rectangle)
            {
                SelectedShapeType = SelectedShapeType.Rectangle;
                State = PadStates.ReadyToDraw;
                ModeLbl.Text = DrawingModeDisplay;
                ShowHelpText();
            }
            else if (ShapeSelectorCb.SelectedIndex == (int)ShapeSelectorCbSelectedIndex.Ellipse)
            {
                SelectedShapeType = SelectedShapeType.Ellipse;
                State = PadStates.ReadyToDraw;
                ModeLbl.Text = DrawingModeDisplay;
                ShowHelpText();
            }
            else if (ShapeSelectorCb.SelectedIndex == (int)ShapeSelectorCbSelectedIndex.Line)
            {
                SelectedShapeType = SelectedShapeType.Line;
                State = PadStates.ReadyToDraw;
                ModeLbl.Text = DrawingModeDisplay;
                ShowHelpText();
            }
            else if (ShapeSelectorCb.SelectedIndex == (int)ShapeSelectorCbSelectedIndex.Polygon)
            {
                SelectedShapeType = SelectedShapeType.Polygon;
                State = PadStates.ReadyToDraw;
                ModeLbl.Text = DrawingModeDisplay;
                ShowHelpText();
            }
            else if (ShapeSelectorCb.SelectedIndex == (int)ShapeSelectorCbSelectedIndex.FreeHand)
            {
                SelectedShapeType = SelectedShapeType.FreeHand;
                State = PadStates.ReadyToDraw;
                ModeLbl.Text = DrawingModeDisplay;
                ShowHelpText();
            }
        }

        private void OnCutBtnClick(object sender, EventArgs e)
        {
            // Call private helper function
            OnCut();
            Panel.Focus();
        }

        private void OnCopyBtnClick(object sender, EventArgs e)
        {
            // Call private helper function
            OnCopy();
            Panel.Focus();
        }

        private void OnPasteBtnClick(object sender, EventArgs e)
        {
            // Call private helper function
            OnPaste();
            Panel.Focus();
        }
        
        private void OnShapesLvSelectedIndexChanged(object sender, EventArgs e)
        {
            // If we're in the ready to draw state and user makes a selection we should move to the selecting state so they can make more selections or move a shape
            if (State == PadStates.ReadyToDraw)
            {
                ToggleMode();
            }
            
            // At any given time the state of the Shapes list is the same as the Items in ShapesLv so build map Shapes directly to selected items
            SelectedShapes.Clear();
            foreach(int i in ShapesLv.SelectedIndices)
            {
                SelectedShapes.Add(Panel.Shapes[i]);
            }
        }

        private void OnColorSelectorBtnClick(object sender, EventArgs e)
        {
            // Get a Color from a dialog and set it as the Selected color property (if one is selected)
            ColorDialog dialog = new ColorDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
                SelectedColor = dialog.Color;

            Panel.Select(); // Take focus away from button
        }

        private void OnToggleModeBtnClick(object sender, EventArgs e)
        {
            ToggleMode();
            Panel.Select(); // Take focus away from button
        }

        private void OnShowHelpCbCheckChanged(object sender, EventArgs e)
        {
            ShowHelpText();
        }
    }
}
