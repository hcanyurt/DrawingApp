using DrawingApp.Methods;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp.Shapes
{
  public class Circle : Shape
  {
    private Vector3 _center;
    private double _radius;

    public Circle()
      : base()
    {
      _radius = 10;
    }

    public Circle(Vector3 loc)
      : this()
    {
      _center = new Vector3(loc.X, loc.Y);
    }
    
    public Vector3 Center { get { return _center; } }
    public double Radius { get { return _radius; } }

    public Circle(Vector3 center, double radius) : this(center)
    {
       _radius = radius;
    }
    public Circle(Vector3 center, Vector3 secondPt) : this(center)
    {
     int radius = (int)Math.Sqrt(Math.Pow(secondPt.X - center.X, 2) + Math.Pow(secondPt.Y - center.Y, 2));
     _radius = radius;
    }
    public double Diameter { get { return _radius * 2; } }

    public override void Draw(Graphics g)
    {
      using (Pen pen = GetPen())
      {
        PointF rectTopLeft = new PointF((float)(_center.X - _radius), (float)(_center.Y - _radius));
        SizeF rectSize = new SizeF((float)Diameter, (float)Diameter);
        RectangleF rect = new RectangleF(rectTopLeft, rectSize);
        g.DrawEllipse(pen, rect);
      }
    }
    public override bool HitTest(Vector3 point)
    {
       double dist = MGeometry.DistanceFromPointToCircle(point, this);
       if (dist < SelectionTolerance)
        return true;

       return false;
    }
    public override void Move(int dx, int dy)
    {       
      _center = new Vector3(_center.X + dx, _center.Y + dy, _center.Z);
    }
    public override Shape Clone()
    {
      return new Circle(_center, _radius)
      {
       IsSelected = this.IsSelected,
       SelectionTolerance = this.SelectionTolerance,
      };
    }
  }

}
