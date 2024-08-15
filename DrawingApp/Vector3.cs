using DrawingApp.Methods;
using DrawingApp.Shapes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp
{
  public class Vector3
  {
    private double x;
    private double y;
    private double z;

    public double X
    {
      get { return x; }
      set { x = value; }
    }
    public double Y
    {
      get { return y; }
      set { y = value; }
    }
    public double Z
    {
      get { return z; }
      set { z = value; }
    }

    public Vector3(double x, double y)
    {
      this.X = x;
      this.Y = y;
      this.Z = 0.0;
    }

    public Vector3(double x, double y, double z)
      : this(x, y)
    {
      this.Z = z;
    }

    public Vector3(System.Drawing.PointF ptf)
      :this(ptf.X,ptf.Y)
    {
    }

    public Vector3(System.Drawing.Point pt)
      : this(pt.X, pt.Y)
    {
    }

    public System.Drawing.PointF ToPointF
    {
      get { return new PointF((float)x, (float)y); }
    }

    public static Vector3 Zero
    {
      get { return new Vector3(0.0, 0.0, 0.0); }
    }

    public Vector2 ToVector2
    {
      get { return new Vector2(X, Y); }
    }

    public static Vector3 operator +(Vector3 v,double d)
    {
      return new Vector3(v.X + d, v.Y + d, v.Z + d);
    }

    public static Vector3 operator -(Vector3 v, double d)
    {
      return new Vector3(v.X - d, v.Y - d, v.Z - d);
    }
    public static Vector3 operator +(Vector3 v1, Vector3 v2)
    {
      return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }
    public static Vector3 operator -(Vector3 v1, Vector3 v2)
    {
      return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }
    public static Vector3 operator *(Vector3 v, double d)
    {
      return new Vector3(v.X * d, v.Y * d, v.Z * d);
    }

    public static Vector3 operator /(Vector3 v, double d)
    {
      return new Vector3(v.X / d, v.Y / d, v.Z / d);
    }

    public static bool operator ==(Vector3 v1, Vector3 v2)
    {
      return (v1.x == v2.x && v1.y == v2.y);
    }

    public static bool operator !=(Vector3 v1, Vector3 v2)
    {
      return !(v1 == v2);
    }

    public double DistanceFrom(Vector3 pt)
    {
      return Math.Sqrt((this.X - pt.X) * (this.X - pt.X) + (this.Y - pt.Y) * (this.Y - pt.Y) + (this.Z - pt.Z) * (this.Z - pt.Z));
    }

    public static double Length(Vector3 pt1, Vector3 pt2)
    {
      return Math.Sqrt((pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y) + (pt1.Z - pt2.Z) * (pt1.Z - pt2.Z));
    }


    public double Angle(Vector3 p2)
    {

      double angle = Math.Atan2((p2.Y - this.Y), (p2.X - this.X)) * 180 / Math.PI;

      if (angle < 0.0)
        angle += 360;

      return angle;
    }

    public void Normalize()
    {
      double l = Length(this, Vector3.Zero);
      if (l < 1e-8)
      { x = 0; y = 0; }
      else
      { x /= l; y /= l; }
    }

    public Vector3 GetNormalized()
    {
      double l = Length(this, Vector3.Zero);
      if (l < 1e-8)
        return new Vector3(0, 0);
      else
        return new Vector3(x / l, y / l);
    }

    public Vector3 CopyOrMove(Vector3 from, Vector3 to)
    {
      double dx = to.X - from.X;
      double dy = to.Y - from.Y;
      double dz = to.Z - from.Z;

      return new Vector3(this.X + dx,this.Y + dy,this.Z + dz);
    }

    public Vector3 Clone()
    {
      return new Vector3(this.X, this.Y, this.Z);
    }

    public void Scale(double scale)
    {
      this.x *= scale;
      this.y *= scale;
      this.z *= scale;
    }
    public void Translate(Vector3 translate)
    {
      this.x += translate.x;
      this.y += translate.y;
      this.z += translate.z;
    }

    public override bool Equals(object obj)
    {
      return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public void Mirror(Line axis)
    {
      double dist = 2*MGeometry.DistanceFromPointToLine(this, axis, out Vector3 closest, true);
      Vector3 normal = (closest - this).GetNormalized();
      Vector3 result = Vector3.Zero;
      result = this + normal * dist;
      this.x = result.x;
      this.y = result.y;
      this.z = result.z;
    }

    public void Rotate(Vector3 origin, double angle)
    {
      angle = angle * MCommon.DegToRad;
      Vector3 rotated =  new Vector3(
          origin.X + (this.X - origin.X) * Math.Cos(angle) - (this.Y - origin.y) * Math.Sin(angle),
          origin.y + (this.x - origin.x) * Math.Sin(angle) + (this.y - origin.y) * Math.Cos(angle)
      );
      this.x = rotated.x;
      this.y = rotated.y;
      this.z = rotated.z;
    }

  }
}
