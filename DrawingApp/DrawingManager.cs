using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DrawingApp.Shapes;
using System.Drawing;
using System.Transactions;
using System.Reflection;
using System.Diagnostics.Eventing.Reader;
using DrawingApp.Methods;

namespace DrawingApp
{
    enum Mode
    {
        None = -1,
        Circle = 0,
        Line = 1,
        Rectangle = 2,
        Polygon = 3,
        Polyline = 4,
    }

    internal class DrawingManager
    {
        private List<Shape> _shapes = new List<Shape>();
        private DrawingAppForm _form;
        public Mode Mode;
        List<Vector3> _shapePoints = new List<Vector3>();
        Shape? _tempShape = null;
        private const int Tolerance = 5;
        private bool _isCtrlPressed = false;
        private Point _lastMousePosition;
        private bool _isDragging = false;
        private List<Shape> _copiedShapes = new List<Shape>();
        public DrawingManager(DrawingAppForm form)
        {
            this._form = form;
            this.Mode = Mode.None;
        }

        internal void MouseMove(Point location)
        {
        if (_isDragging)
            {
                int dx = location.X - _lastMousePosition.X;
                int dy = location.Y - _lastMousePosition.Y;

                foreach (Shape shape in _shapes.Where(s => s.IsSelected))
                {
                    shape.Move(dx, dy); 
                }

                _lastMousePosition = location;
                _form.Canvas.Refresh();
            }

            else
            {
                if (Mode == Mode.Line && _shapePoints.Count == 1)
                {
                    _tempShape = new Shapes.Line(_shapePoints[0], new Vector3(location.X, location.Y));
                    _form.Canvas.Refresh();
                }
                else if (Mode == Mode.Circle && _shapePoints.Count == 1)
                {
                    Vector3 center = _shapePoints[0];
                    _tempShape = new Shapes.Circle(_shapePoints[0], new Vector3(location.X, location.Y));
                    _form.Canvas.Refresh();
                }
                else if (Mode == Mode.Polygon)
                {
                    if (_shapePoints.Count > 0)
                    {
                        var updatedPoints = new List<Vector3>(_shapePoints);
                        updatedPoints.Add(new Vector3(location.X, location.Y));
                        _tempShape = new Shapes.Polygon(updatedPoints);
                        ((Polygon)_tempShape).IsClosed = false;
                        _form.Canvas.Refresh();
                    }
                }
                else if (Mode == Mode.Polyline)
                {
                    if (_shapePoints.Count > 0)
                    {
                        var updatedPoints = new List<Vector3>(_shapePoints);
                        updatedPoints.Add(new Vector3(location.X, location.Y));
                        _tempShape = new Shapes.Polyline(updatedPoints);
                        _form.Canvas.Refresh();
                    }
                }
            }
        }
       
        internal void MouseUp(MouseEventArgs e)
        {
            Point location = e.Location;
            Vector3 point = new Vector3(location.X, location.Y);
            _isDragging = false;

            if (e.Button == MouseButtons.Right)
            {
                if (Mode == Mode.Polygon)
                {
                    if (_shapePoints.Count > 2)
                    {
                        _shapes.Add(new Shapes.Polygon(_shapePoints.ToList()));

                    }
                }
                else if (Mode == Mode.Polyline)
                {
                    if (_shapePoints.Count >= 2)
                    {
                        _shapes.Add(new Shapes.Polyline(_shapePoints.ToList()));
                    }
                }
                ClearTemporary();
                _form.Canvas.Refresh();
                return;
            }
            if (Mode == Mode.None) 
            {
                if (!_isCtrlPressed) 
                {
                    UnselectAll();
                }
                foreach (Shape shape in _shapes)
                {
                  //shape.IsSelected = false;
                  if (shape.HitTest(point))
                  {
                        shape.IsSelected = true;
                        break;
                  }
                }
                _form.Canvas.Refresh(); 
            }
            else
            {
                _shapePoints.Add(new Vector3(location.X, location.Y));
                if (Mode == Mode.Circle)
                {
                    if (_shapePoints.Count == 2)
                    {

                        _shapes.Add(new Circle(_shapePoints[0], _shapePoints[1]));
                        ClearTemporary();
                    }
                }
                else if (Mode == Mode.Line)
                {
                    if (_shapePoints.Count == 2)
                    {
                        _shapes.Add(new Shapes.Line(_shapePoints[0], _shapePoints[1]));
                        ClearTemporary();
                    }
                }
                else if (Mode == Mode.Polygon)
                {
                    if (_shapePoints.Count > 2 && MCommon.IsPointsEqual(_shapePoints[0], new Vector3(location.X, location.Y), Tolerance))
                    {
                        _shapePoints.RemoveAt(_shapePoints.Count - 1);
                        _shapes.Add(new Shapes.Polygon(_shapePoints.ToList()));
                        ClearTemporary();
                    }
                }
                
                _form.Canvas.Refresh();
            }
        }
        private void UnselectAll()
        {
            foreach (Shape shape in _shapes)
            {
                shape.IsSelected = false;
            } 
        }

        public void ClearTemporary()
        {
            _tempShape = null;
            _shapePoints.Clear();
        }
        
        internal void Draw(Graphics graphics)
        {
          if (_tempShape != null)
          {
            _tempShape.Draw(graphics);
          }
          foreach (Shape shape in _shapes)
          {
            shape.Draw(graphics);
          }
        }

        internal void KeyDown(Keys keyCode)
        {
          if (keyCode == Keys.ControlKey)
          {
            _isCtrlPressed = true; 
          }
            if (_isCtrlPressed && keyCode == Keys.C)
            {
                CopySelectedShapes();
            }
            else if (_isCtrlPressed && keyCode == Keys.V)
            {
                PasteCopiedShapes();
            }
          switch (keyCode)
          {
            case Keys.C:
                 SetMode(Mode.Circle);
                 break;
            case Keys.L:
                 SetMode(Mode.Line);
                 break;
            case Keys.P:
                 SetMode(Mode.Polygon);
                 break;
            case Keys.O:
                 SetMode(Mode.Polyline);
                 break;
            case Keys.Escape:
                 SetModeToNone();
                 break;
            case Keys.Delete:
                 DeleteSelectedShapes();
                 break;
          }
        }
        internal void KeyUp(Keys keyCode)
        {
          if (keyCode == Keys.ControlKey)
          {
            _isCtrlPressed = false;
          }
        }
        internal void SetMode(Mode newMode)
        {
          this.Mode = newMode;
          _shapePoints.Clear();
        }
        internal void SetModeToNone()
        {
          SetMode(Mode.None);
          ClearTemporary();
          _form.Canvas.Refresh();
        }

        internal void MouseDown(MouseEventArgs e)
        {
          if (e.Button == MouseButtons.Left)
          {
            if (Mode == Mode.None)
            {
              _isDragging = true;
              _lastMousePosition = e.Location;
            }
          }
        }
        internal void DeleteSelectedShapes() 
        {
           _shapes.RemoveAll(s => s.IsSelected);
           _form.Canvas.Refresh();
        }

        internal void CopySelectedShapes()
        {
            _copiedShapes.Clear();
            foreach (var shape in _shapes.Where(s => s.IsSelected))
            {
              _copiedShapes.Add(shape.Clone());
            }

            _form.Canvas.Refresh(); 
        }
        internal void PasteCopiedShapes()
        {
            foreach (var shape in _copiedShapes)
            {
                var pastedShape = shape.Clone();
                pastedShape.Move(10, 10); 
                _shapes.Add(pastedShape); 
            }
            _form.Canvas.Refresh();
        }
    }
}

