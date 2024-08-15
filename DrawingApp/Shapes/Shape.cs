using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp.Shapes
{
  public abstract class Shape
  {
    private float _thickness;
    private Color _color;
    private bool _isSelected;
    public double SelectionTolerance = 5;
   
    public Shape() 
    {
      _thickness = 1.0f;
      _color = Color.Black;
      _isSelected = false;
     
    }

    public float Thickness { get { return _thickness; } }
    public Color Color { get { return _color; } }
    public bool IsSelected
    {
      get { return _isSelected; }
      set { _isSelected = value; }
    }

     public object Type { get; internal set; }
    public Pen GetPen()
    {
       return new Pen(IsSelected ? Color.Red : _color, _thickness);
    }
    
    public virtual void Draw(Graphics g)
    {
    }
    public abstract bool HitTest(Vector3 point);
    public virtual void Move(int dx, int dy)
    {
    }
    public abstract Shape Clone();

    }
}
