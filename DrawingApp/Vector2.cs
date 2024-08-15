using DrawingApp.Methods;
using DrawingApp.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp
{
  public class Vector2
  {
		private double _x;
		private double _y;
		public Vector2(double x, double y)
		{
			this._x = x;
			this._y = y;
		}

		public static Vector2 Zero
		{
			get { return new Vector2(0.0, 0.0);}
		}
		public double Y
		{
			get { return _y; }
			set { _y = value; }
		}

		public double X
		{
			get { return _x; }
			set { _x = value; }
		}

		public System.Drawing.PointF ToPointF
		{
			get { return new System.Drawing.PointF((float)this._x, (float)this._y); }
		}

    public Vector3 ToVector3
    {
      get { return new Vector3(this._x, this._y); }
    }
    public Vector2 CopyOrMove(Vector2 from, Vector2 to)
    {
      double dx = to.X - from.X;
      double dy = to.Y - from.Y;

      return new Vector2(this.X + dx, this.Y + dy);
    }

    public Vector2 Clone()
    {
      return new Vector2(this.X, this.Y);
    }

    public double DistancFrom(Vector2 v2)
		{
			double dx = v2.X - X;
			double dy = v2.Y - Y;
			return Math.Sqrt(dx * dx + dy * dy);
		}

    public void Mirror(Line axis)
    {
      double dist = 2 * MGeometry.DistanceFromPointToLine(this.ToVector3, axis, out Vector3 closest, true);
      Vector3 normal = (closest - this.ToVector3).GetNormalized();
      Vector3 result = Vector3.Zero;
      result = this.ToVector3 + normal * dist;
      this.X = result.X;
      this.Y = result.Y;
    }

    public void Rotate(Vector3 origin, double angle)
    {
      Vector3 rotated = new Vector3(this.X, this.Y);
      rotated.Rotate(origin, angle);
      this.X = rotated.X;
      this.Y = rotated.Y;
    }
  }
}
