using DrawingApp.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.AxHost;
using System.Reflection;
using DrawingApp;

namespace DrawingApp.Methods
{
	public class MGeometry
	{
		public static double LineAngle(Vector3 p1, Vector3 p2)
		{
			double angle = Math.Atan2((p2.Y - p1.Y), (p2.X - p1.X)) * 180 / Math.PI;

			if (angle < 0.0)
				angle += 360;

			return angle;
		}

		public static double LineAngle(Line line)
		{
			return LineAngle(line.Start, line.End);
		}

		public static Vector3 LineLineIntersection(Shapes.Line l1, Shapes.Line l2, bool extended = false)
		{
			Vector3 result;
			Vector3 p1 = l1.Start;
			Vector3 p2 = l1.End;
			Vector3 p3 = l2.Start;
			Vector3 p4 = l2.End;

			double dx12 = p2.X - p1.X;
			double dy12 = p2.Y - p1.Y;
			double dx34 = p4.X - p3.X;
			double dy34 = p4.Y - p3.Y;

			double denominator = dy12 * dx34 - dx12 * dy34;
			double k1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;

			if (double.IsInfinity(k1))
				return new Vector3(double.NaN, double.NaN);

			result = new Vector3(p1.X + dx12 * k1, p1.Y + dy12 * k1);

			if (extended)
				return result;
			else
			{
				if (IsPointOnLine(l1, result) && IsPointOnLine(l2, result))
					return result;
				else
					return new Vector3(double.NaN, double.NaN);
			}
		}

		//public static List<Vector3> LineEllipseIntersection(Line line, Ellipse ellipse)
		//{
		//	double x1 = line.Start.X;
		//	double y1 = line.Start.Y;
		//	double x2 = line.End.X;
		//	double y2 = line.End.Y;

		//	double xc = ellipse.Center.X;
		//	double yc = ellipse.Center.Y;

		//	double a = ellipse.Major;
		//	double b = ellipse.Minor;

		//	double angle = ellipse.Rotation*MCommon.DegToRad;

		//	double X, Y;
		//	List<Vector3> result = new List<Vector3>();

		//	double x21 = x2 - x1;
		//	double y21 = y2 - y1;

		//	x1 -= xc;
		//	y1 -= yc;

		//	double sin = Math.Sin(angle);
		//	double cos = Math.Cos(angle);

		//	double A1 = b * b * cos * cos + a * a * sin * sin;
		//	double B1 = a * a * cos * cos + b * b * sin * sin;
		//	double C1 = 2 * (b * b - a * a) * sin * cos;

		//	double A = A1 * x21 * x21 + B1 * y21 * y21 + C1 * x21 * y21;
		//	double B = 2 * A1 * x1 * x21 + 2 * B1 * y1 * y21 + C1 * x1 * y21 + C1 * x21 * y1;
		//	double C = A1 * x1 * x1 + B1 * y1 * y1 + C1 * x1 * y1 - a * a * b * b;

		//	double Delta = B * B - 4 * A * C;

		//	List<double> t_value = new List<double>();

		//	if(Delta == 0)
		//		t_value.Add(-B/2/A);
		//	else if (Delta > 0)
		//	{ 
		//		t_value.Add((-B + Math.Sqrt(Delta)) / 2 / A);
		//		t_value.Add((-B - Math.Sqrt(Delta)) / 2 / A);
		//	}

		//	foreach(double t in t_value)
		//	{
		//		if (t>=0.0 && t<=1.0)
		//		{
		//			X = x1 + x21 * t + xc;
		//			Y = y1 + y21 * t + yc;
		//			result.Add(new Vector3(X, Y));
		//		}
		//	}

		//	return result;
		//}

		public static List<Vector3> LineCircleIntersection(Line line, Circle circle)
		{
			List<Vector3> result = new List<Vector3>();
			double x1 = line.Start.X;
			double y1 = line.Start.Y;
			double x2 = line.End.X;
			double y2 = line.End.Y;

			double xc = circle.Center.X;
			double yc = circle.Center.Y;
			double r = circle.Radius;

			double dx = x2 - x1;
			double dy = y2 - y1;

			x1 -= xc;
			y1 -= yc;
			double A = dx * dx + dy * dy;
			double B = 2 * (x1 * dx + y1 * dy);
			double C = x1 * x1 + y1 * y1 - r * r;
			double Delta = B * B - 4 * A * C;
			List<double> t_values = new List<double>();

			if (Delta == 0)
				t_values.Add(-B / 2 / A);
			else if (Delta > 0)
			{
				t_values.Add((-B + Math.Sqrt(Delta)) / 2 / A);
				t_values.Add((-B - Math.Sqrt(Delta)) / 2 / A);
			}

			foreach (double t in t_values)
			{
				if (t >= 0.0 && t <= 1.0)
				{
					double X = x1 + dx * t + xc;
					double Y = y1 + dy * t + yc;
					result.Add(new Vector3(X, Y));
				}
			}

			return result;

		}

		//public static List<Vector3> LineArcIntersection(Line line, CircularArc arc)
		//{
		//	List<Vector3> result = new List<Vector3>();
		//	List<Vector3> list = LineCircleIntersection(line,new Circle(arc.Center,arc.Radius));

		//	foreach(Vector3 v in list)
		//	{
		//		if (IsPointOnArc(arc, v))
		//			result.Add(v);
		//	}

		//	return result;
		//}

		//private static bool IsPointOnArc(CircularArc arc, Vector3 v)
		//{
		//	Line line = new Line(arc.Center, v);

		//	double angle = LineAngle(line);
		//	double start = arc.StartAngle;
		//	double end = arc.EndAngle;

		//	if ((start < end) && (start <= angle) && (angle <= end))
		//		return true;
		//	else if ((start > end) && (angle >= start) && (angle <= 360.0))
		//		return true;
		//	else if ((start > end) && (angle >= 0) && (angle <= end))
		//		return true;
		//	else
		//		return false;
		//}

		public static bool IsPointOnLine(Shapes.Line line, Vector3 point)
		{
			return MCommon.IsEqual(line.Length, point.DistanceFrom(line.Start) + point.DistanceFrom(line.End), 0.001);
		}

		public static Vector3 GetNormal(Shapes.Line l)
		{
			double magnitude = l.Length;
			Vector3 v = l.End - l.Start;
			return v / magnitude;
		}

		public static double GetCounterClockwiseAngle(Vector3 origin, Vector3 point)
		{
			double angle = Math.Atan2(point.Y - origin.Y, point.X - origin.X);

			if (angle < 0)
			{
				angle += 2 * Math.PI;
			}

			double degrees = angle * (180 / Math.PI);

			return degrees;
		}


		public static double DistanceFromPointToLine(Vector3 point, Line line, out Vector3 closest, bool IsExtended = false)
		{
			double x1 = line.Start.X;
			double y1 = line.Start.Y;
			double x2 = line.End.X;
			double y2 = line.End.Y;

			double x = point.X;
			double y = point.Y;

			double dx = x2 - x1;
			double dy = y2 - y1;

			if (dx == 0 && dy == 0)
			{
				closest = line.Start;
				dx = x - x1;
				dy = y - y1;
				return Math.Sqrt(dx * dx + dy * dy);
			}

			double k = ((x - x1) * dx + (y - y1) * dy) / (dx * dx + dy * dy);

			closest = new Vector3(x1 + k * dx, y1 + k * dy);
			dx = x - closest.X;
			dy = y - closest.Y;

			if (!IsExtended)
			{
				if (k < 0)
				{
					closest = new Vector3(x1, y1);
					dx = x - x1;
					dy = y - y1;
				}
				else if (k > 1)
				{
					closest = new Vector3(x2, y2);
					dx = x - x2;
					dy = y - y2;
				}
			}
			return Math.Sqrt(dx * dx + dy * dy);
		}
		public static double DistanceFromPointToLine(Vector3 point, Line line, bool IsExtended = false)
		{
			return DistanceFromPointToLine(point, line, out Vector3 closest, IsExtended);
		}

		//public static double DistanceFromPointToEllipse(Vector3 point, Ellipse ellipse, out Vector3 PointOnEllipse)
		//{
		//	Line centerToPoint = new Line(ellipse.Center, point);
		//	Vector3 normal = centerToPoint.NormalVector;
		//	Vector3 extendedPt = normal * 100000;
		//	centerToPoint.End = extendedPt;
		//	List<Vector3> intersection = new List<Vector3>();
		//	intersection = LineEllipseIntersection(centerToPoint, ellipse);
		//	if (intersection.Count > 0)
		//		PointOnEllipse = intersection[0];
		//	else
		//		PointOnEllipse = Vector3.Zero;
		//	return PointOnEllipse.DistanceFrom(point);
		//}
		//public static double DistanceFromPointToEllipse(Vector3 point, Ellipse ellipse)
		//{
		//	return DistanceFromPointToEllipse(point, ellipse, out Vector3 pointOnEllipse);
		//}

		public static double DistanceFromPointToCircle(Vector3 point, Circle circle, out Vector3 PointOnCircle)
		{
			double d = point.DistanceFrom(circle.Center) - circle.Radius;
			double angle = LineAngle(point, circle.Center);

			double x = d * Math.Cos(angle * MCommon.DegToRad) + point.X;
			double y = d * Math.Sin(angle * MCommon.DegToRad) + point.Y;

			PointOnCircle = new Vector3(x, y);
			return point.DistanceFrom(PointOnCircle);
		}
		public static double DistanceFromPointToCircle(Vector3 point, Circle circle)
		{
			return DistanceFromPointToCircle(point, circle, out Vector3 pointOnCircle);
		}


		//public static double DistanceFromPointToCircularArc(Vector3 point, CircularArc arc, out Vector3 PointOnArc)
		//{
		//	double d = point.DistanceFrom(arc.Center) - arc.Radius;
		//	double angle = LineAngle(point, arc.Center);

		//	double x = d * Math.Cos(angle * MCommon.DegToRad) + point.X;
		//	double y = d * Math.Sin(angle * MCommon.DegToRad) + point.Y;

		//	Vector3 result = new Vector3(x, y);

		//	double r_angle = LineAngle(arc.Center, result);

		//	double end = arc.EndAngle;



		//	if((r_angle >= arc.StartAngle && r_angle <= end) || (end < arc.StartAngle && (r_angle <= end || r_angle >= arc.StartAngle)))
		//	{
		//		PointOnArc = result;
		//	}
		//	else
		//	{
		//		double dist1 = arc.StartPoint.DistanceFrom(point);
		//		double dist2 = arc.EndPoint.DistanceFrom(point);

		//		if(dist1 < dist2)
		//		{
		//			d = dist1;
		//			PointOnArc = arc.StartPoint;
		//		}
		//		else
		//		{
		//			d = dist2;
		//			PointOnArc = arc.EndPoint;
		//		}
		//	}

		//	return Math.Abs(d);
		//}
		//public static double DistanceFromPointToArc(Vector3 point, CircularArc arc)
		//{
		//	return DistanceFromPointToCircularArc(point, arc, out Vector3 pointOnArc);
		//}


		public static double DistanceFromPointToPolygon(Vector3 point, Polygon ply, out Vector3 PointOnPolygon)
		{

			double minDistance = double.MaxValue;
			Vector3 closest;
			PointOnPolygon = Vector3.Zero;
			double dist;
			for (int i = 0; i < ply.Vertexes.Count; i++)
			{

				int j = (i + 1) % ply.Vertexes.Count;
				Line line = new Line(ply.Vertexes[i], ply.Vertexes[j]);
				dist = DistanceFromPointToLine(point, line, out closest);


				if (dist < minDistance)
				{
					minDistance = dist;
					PointOnPolygon = closest;
				}
			}


			return minDistance;
		}
		public static double DistanceFromPointToPolygon(Vector3 point, Polygon ply)
		{
			return DistanceFromPointToPolygon(point, ply, out Vector3 pointOnPolygon);
		}

		private static double CopySign(double a, double b)
		{
			return a * Math.Sign(b);
		}

		public static bool IsPointInRectangle(Vector3 point, Polygon rectangle)
		{


			double minx = double.MaxValue;
			double maxx = double.MinValue;
			double miny = double.MaxValue;
			double maxy = double.MinValue;

			foreach (Vector3 v in rectangle.Vertexes)
			{
				if (v.X < minx)
					minx = v.X;
				if (v.X > maxx)
					maxx = v.X;
				if (v.Y < miny)
					miny = v.Y;
				if (v.Y > maxy)
					maxy = v.Y;
			}
			double width = maxx - minx;
			double height = maxy - miny;

			Vector3 bl = new Vector3(minx, miny);
			Vector3 br = new Vector3(minx + width, miny);
			Vector3 tl = new Vector3(minx, miny + height);
			Vector3 tr = new Vector3(maxx, maxy);

			double d1 = MGeometry.DistanceFromPointToLine(point, new Line(tl, tr));
			double d2 = MGeometry.DistanceFromPointToLine(point, new Line(bl, br));
			double d3 = MGeometry.DistanceFromPointToLine(point, new Line(bl, tl));
			double d4 = MGeometry.DistanceFromPointToLine(point, new Line(br, tr));

			if (MCommon.IsEqual(d1 + d2, height, 0.01))
			{
				if (MCommon.IsEqual(d3 + d4, width, 0.01))
				{
					return true;
				}
			}

			return false;
		}

		public static Tuple<double, double> GetMaximumXandYOfAShape(Shape shape)
		{
			//if (shape.Type == ShapeType.CircularArc)
			//{
			//	CircularArc a = (CircularArc)shape;
			//	MGeometry.DistanceFromPointToCircularArc(new Vector3(10000000, 0), a, out Vector3 point);
			//	double x = point.X;
			//	MGeometry.DistanceFromPointToCircularArc(new Vector3(0, 10000000), a, out point);
			//	double y = point.Y;
			//	return new Tuple<double,double>(x, y);
			//}
			if ((Circle)shape != null)
			{
				Circle c = (Circle)shape;
				double x = c.Center.X + c.Radius;
				double y = c.Center.Y + c.Radius;
				return new Tuple<double, double>(x, y);
			}
			else if ((Line)shape != null)
			{
				Line l = (Line)shape;
				double x = Math.Max(l.Start.X, l.End.X);
				double y = Math.Max(l.Start.Y, l.End.Y);
				return new Tuple<double, double>(x, y);
			}
			/*else if(shape.Type == ShapeType.Ellipse)
			{
				Ellipse e = (Ellipse)shape;
				double x = e.Center.X + e.Major;
				double y = e.Center.Y + e.Minor;
				return new Tuple<double, double>(x, y);
			}*/
			else if ((Polygon)shape != null)
			{
				Polygon poly = (Polygon)shape;
				double x = double.MinValue;
				double y = double.MinValue;
				foreach (var v in poly.Vertexes)
				{
					if (v.X > x)
						x = v.X;
					if (v.Y > y)
						y = v.Y;
				}
				return new Tuple<double, double>(x, y);
			}
			return new Tuple<double, double>(double.NaN, double.NaN);
		}


		//internal static void TranslateObjects(List<Shape> shapes, Vector3 translation)
		//{
		//	foreach (var s in shapes)
		//	{
		//		s.Translate(translation);
		//	}
		//}


		//internal static Shapes.Rectangle FindBoundingBox(List<Shape> shapes)
		//{
		//	//Find maximum x and y values of all shapes
		//	double maxx = double.MinValue;
		//	double maxy = double.MinValue;

		//	var res = new Tuple<double, double>(maxx, maxy);
		//	foreach (Shape s in shapes)
		//	{
		//		Shapes.Rectangle bb = s.GetBoundingBox();
		//		res = new Tuple<double, double>(bb.TopRight.X,bb.TopRight.Y);
		//		if (res.Item1 != double.NaN && res.Item2 != double.NaN)
		//		{
		//			if (res.Item1 > maxx)
		//				maxx = res.Item1;

		//			if (res.Item2 > maxy)
		//				maxy = res.Item2;
		//		}
		//	}

		//	//Find minimum x and y values of all shapes
		//	double minx = double.MaxValue;
		//	double miny = double.MaxValue;
		//	res = new Tuple<double, double>(minx, miny);
		//	foreach (Shape s in shapes)
		//	{
		//		Shapes.Rectangle bb = s.GetBoundingBox();
		//		res = new Tuple<double, double>(bb.BottomLeft.X, bb.BottomLeft.Y);
		//		if (res.Item1 != double.NaN && res.Item2 != double.NaN)
		//		{
		//			if (res.Item1 < minx)
		//				minx = res.Item1;

		//			if (res.Item2 < miny)
		//				miny = res.Item2;
		//		}
		//	}

		//	return new Shapes.Rectangle(new Vector3(minx, miny), new Vector3(maxx, maxy));
		//}

		internal static bool IsLinesCoincident(Line line1, Line line2)
		{
			return MCommon.IsPointsEqual(line1.Start, line2.Start, 0.001) && MCommon.IsPointsEqual(line1.End, line2.End, 0.001)
						 || MCommon.IsPointsEqual(line1.Start, line2.End, 0.001) && MCommon.IsPointsEqual(line1.End, line2.Start, 0.001);
		}
		/* internal static double DistanceFromPointToEllipticArc(Vector3 point, EllipticArc arc)
             {
                 return DistanceFromPointToEllipticArc(point, arc, out Vector3 v);
         }
         internal static double DistanceFromPointToEllipticArc(Vector3 point, EllipticArc arc, out Vector3 PointOnArc)
         {
                 Ellipse ellipse = new Ellipse(arc.Center, arc.Major, arc.Minor);
                 Line ctopoint = new Line(arc.Center,point);
                 double r_angle = MGeometry.LineAngle(ctopoint);
           double d = DistanceFromPointToEllipse(point, ellipse, out Vector3 result);

           if ((r_angle >= arc.StartAngle && r_angle <= arc.EndAngle) || (arc.EndAngle < arc.StartAngle && (r_angle <= arc.EndAngle || r_angle >= arc.StartAngle)))
           {
             PointOnArc = result;
           }
           else
           {
             double dist1 = arc.StartPoint.DistanceFrom(point);
             double dist2 = arc.EndPoint.DistanceFrom(point);

             if (dist1 < dist2)
             {
               d = dist1;
               PointOnArc = arc.StartPoint;
             }
             else
             {
               d = dist2;
               PointOnArc = arc.EndPoint;
             }
           }

           return Math.Abs(d);
         }
       }

         public class MShape
         {
             public static Shapes.Ellipse GetEllipse(Vector3 center, Vector3 p1, Vector3 p2)
             {
                 double major = center.DistanceFrom(p1);
                 double minor = center.DistanceFrom(p2);
                 double angle = MGeometry.LineAngle(center, p1);
                 return new Shapes.Ellipse(center, major, minor, angle);
             }

         public static Shapes.Ellipse GetEllipseWithRectangle(Shapes.Rectangle box)
         {
                 Vector3 center = box.GetCenter();
           double major = box.Width/2;
           double minor = box.Height/2;
                 if (major < 0.0001 || minor < 0.0001)
                     return null;

           return new Shapes.Ellipse(center, major, minor);
       }
    }*/

		public static Shapes.Circle GetCircleWith3Points(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			double x1 = (p1.X + p2.X) / 2;
			double y1 = (p1.Y + p2.Y) / 2;
			double dx1 = p2.X - p1.X;
			double dy1 = p2.Y - p1.Y;

			double x2 = (p2.X + p3.X) / 2;
			double y2 = (p2.Y + p3.Y) / 2;
			double dx2 = p3.X - p2.X;
			double dy2 = p3.Y - p2.Y;

			Line line1 = new Line(new Vector3(x1, y1), new Vector3(x1 - dy1, y1 + dx1));
			Line line2 = new Line(new Vector3(x2, y2), new Vector3(x2 - dy2, y2 + dx2));

			Vector3 center = MGeometry.LineLineIntersection(line1, line2, true);

			double dx = center.X - p1.X;
			double dy = center.Y - p1.Y;

			double radius = Math.Sqrt(dx * dx + dy * dy);
			if (double.IsNaN(center.X) || double.IsNaN(center.Y) || double.IsNaN(radius))
				return null;
			return new Circle(center, radius);
		}

		/*public static CircularArc GetCircularArcWith3Points(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			double start, end;
			CircularArc result = new CircularArc();

			Circle c = GetCircleWith3Points(p1, p2, p3);
			if (ReferenceEquals(c, null))
				return null;

			if (c.Radius > 0)
			{
				if(DeterminePointOfLine(new Line(p1,p3),p2) < 0)
				{
					start = MGeometry.LineAngle(new Line(c.Center, p3));
					end = MGeometry.LineAngle(new Line(c.Center, p1));
				}
				else
				{
					start = MGeometry.LineAngle(new Line(c.Center, p1));
					end = MGeometry.LineAngle(new Line(c.Center, p3));
				}
				if (MCommon.IsEqual(start, end,0.0001))
					return null;
				result = new CircularArc(c.Center, c.Radius, start, end);
			}

			return result;
		}
    public static CircularArc GetCircularArcWithStartCenterEnd(Vector3 center, Vector3 start, Vector3 end)
		{
      double startA = MGeometry.LineAngle(center, start);
      double endA = MGeometry.LineAngle(center, end);
			if (startA > endA)
			{
				double tmp = startA;
				startA = endA;
				endA = startA;
			}
      double radius = center.DistanceFrom(start);
			return new CircularArc(center, radius, startA, endA);
    }

		/*public static EllipticArc GetEllipticArcWithBoxAndAngles(ComosysSymbolEditor.Shapes.Rectangle box, double startAngle, double endAngle)
		{
			//check shape
			if (box.Width < 0.0001 || box.Height < 0.0001 || Math.Abs(startAngle-endAngle) < 0.001)
				return null;
				
			
			EllipticArc arc = new EllipticArc(box.GetCenter(),box.Width/2,box.Height/2,startAngle,endAngle);
			return arc;
		}
    public static EllipticArc GetEllipticArcWithBoxAndStartEndPoints(ComosysSymbolEditor.Shapes.Rectangle box, Vector3 start, Vector3 end, bool mirror = false)
		{
			Vector3 center = box.GetCenter();
			double stangle = center.Angle(start);
      double endangle = center.Angle(end);
			if (mirror)
			{
				double tmp = stangle;
				stangle = endangle;
				endangle = tmp;
			}
			return GetEllipticArcWithBoxAndAngles(box, stangle, endangle);
    }*/

		private static double DeterminePointOfLine(Line line, Vector3 v)
		{
			return (v.X - line.Start.X) * (line.End.Y - line.Start.Y) - (v.Y - line.Start.Y) * (line.End.X - line.Start.X);
		}

		//public static Polygon PointToRect(Vector3 firstCorner, Vector3 secondCorner, out int direction)
		//{
		//	double x = Math.Min(firstCorner.X, secondCorner.X);
		//	double y = Math.Min(firstCorner.Y, secondCorner.Y);
		//	double width = Math.Abs(firstCorner.X - secondCorner.X);
		//	double height = Math.Abs(firstCorner.Y - secondCorner.Y);

		//	double dx = secondCorner.X - firstCorner.X;

		//	List<PolygonVertex> vertexes = new List<PolygonVertex>();
		//	vertexes.Add(new PolygonVertex(x, y));
		//	vertexes.Add(new PolygonVertex(x + width, y));
		//	vertexes.Add(new PolygonVertex(x + width, y + height));
		//	vertexes.Add(new PolygonVertex(x, y + height));

		//	if (dx > 0)
		//		direction = 1;
		//	else if (dx < 0)
		//		direction = 2;
		//	else
		//		direction = -1;

		//	Polygon rect = new Polygon(vertexes, true);
		//	if (!MShape.CheckPolygon(rect,true))
		//		return null;

		//	return rect;
		//}

		//public static Polygon GetPolygon(List<Vector3> pts, bool checkFinalShape)
		//{
		//	List<PolygonVertex> vertexes = new List<PolygonVertex>();
		//	foreach (Vector3 v in pts)
		//	{
		//		vertexes.Add(new PolygonVertex(v.X, v.Y));
		//	}
		//	Polygon ply = new Polygon(vertexes, true);

		//	if (!MShape.CheckPolygon(ply, checkFinalShape))
		//		return null;

		//	return ply;
		//}

		//internal static bool CheckPolygon(Polygon ply, bool checkFinalShape)
		//{
		//	List<Line> lines = ply.GetLines();
		//	if (!checkFinalShape)
		//		lines.Remove(lines.Last());

		//	for (int i = 0; i < lines.Count() - 1; i++)
		//	{
		//		for (int j = i + 1; j < lines.Count(); j++)
		//		{
		//			if (MGeometry.IsLinesCoincident(lines[i], lines[j]))
		//				return false;

		//			Vector3 intpt = MGeometry.LineLineIntersection(lines[i], lines[j]);
		//			if (MCommon.IsPointsEqual(lines[i].End, intpt, 0.001) && MCommon.IsPointsEqual(lines[j].Start, intpt, 0.001))
		//				continue;
		//			if (!double.IsNaN(intpt.X) && !double.IsNaN(intpt.Y))
		//			{
		//				if (!(MCommon.IsEqual(intpt.X, ply.Vertexes[0].X, 0.001) &&
		//							MCommon.IsEqual(intpt.Y, ply.Vertexes[0].Y, 0.001)))
		//				{
		//					return false;
		//				}
		//			}
		//		}
		//	}
		//	return true;
		//}

		internal static Vector3 GetCenterOfCircleWith3Points(Vector3 P1, Vector3 P2, Vector3 P3)
		{
			double s = 0.5 * ((P2.X - P3.X) * (P1.X - P3.X) - (P2.Y - P3.Y) * (P3.Y - P1.Y));
			double sUnder = (P1.X - P2.X) * (P3.Y - P1.Y) - (P2.Y - P1.Y) * (P1.X - P3.X);

			if (MCommon.IsEqual(sUnder, 0.0, 0.000001))
				return new Vector3(double.NaN, double.NaN); // insufficient data to calculate center

			s /= sUnder;
			Vector3 Center = Vector3.Zero;
			Center.X = 0.5 * (P1.X + P2.X) + s * (P2.Y - P1.Y); // center X coordinate
			Center.Y = 0.5 * (P1.Y + P2.Y) + s * (P1.X - P2.X); // center Y coordinate

			return Center;
		}
	}

	public class MCommon
	{
		public static bool IsEqual(double d1, double d2, double Tol)
		{
			return (Math.Abs(d1 - d2) < Tol);
		}

		public static double DegToRad = Math.PI / 180;

		public static bool IsPointsEqual(Vector3 pt1, Vector3 pt2, double Tol)
		{
			return IsEqual(pt1.X, pt2.X, Tol) && IsEqual(pt1.Y, pt2.Y, Tol);
		}

		//public static string GetSaveLoadValue(Enum Value)
		//{
		//  Type Type = Value.GetType();

		//  FieldInfo fieldInfo = Type.GetField(Value.ToString());

		//  SaveLoadAttribute Attribute = fieldInfo.GetCustomAttribute(
		//      typeof(SaveLoadAttribute)
		//  ) as SaveLoadAttribute;

		//  return Attribute.SaveLoadValue;
		//}

		//public static T GetEnumValueFromSaveLoadValue<T>(string saveLoadValue) where T : Enum
		//{
		//  Type type = typeof(T);

		//  foreach (var field in type.GetFields())
		//  {
		//    var attribute = field.GetCustomAttribute<SaveLoadAttribute>();

		//    if (attribute != null && attribute.SaveLoadValue == saveLoadValue)
		//    {
		//      return (T)field.GetValue(null);
		//    }
		//  }
		//  throw new ArgumentException($"No enum value found for SaveLoadValue: {saveLoadValue}");
		//}

		internal static bool IsPointStartOrEndOfLine(Line l, Vector3 pt2)
		{
			return (IsPointsEqual(l.Start, pt2, 0.0001) || IsPointsEqual(l.End, pt2, 0.0001));
		}
		internal static bool IsPointStartOfLine(Line l, Vector3 pt2)
		{
			return (IsPointsEqual(l.Start, pt2, 0.0001));
		}
		internal static bool IsPointEndOfLine(Line l, Vector3 pt2)
		{
			return (IsPointsEqual(l.End, pt2, 0.0001));
		}
	}
}
    /*internal static bool IsPointStartOrEndOfArc(CircularArc a, Vector3 pt2)
    {
      return (IsPointsEqual(a.StartPoint, pt2, 0.0001) || IsPointsEqual(a.EndPoint, pt2, 0.0001));
    }
    internal static bool IsPointStartOfArc(CircularArc a, Vector3 pt2)
    {
      return (IsPointsEqual(a.StartPoint, pt2, 0.0001));
    }
    internal static bool IsPointEndOfArc(CircularArc a, Vector3 pt2)
    {
      return (IsPointsEqual(a.EndPoint, pt2, 0.0001));
    }*/

//    internal static Vector3 GetClosestPointToSelectedShapes(List<Shape> shps, Vector3 currentPt, double scaleFactor)
//		{
//			Vector3 pt = null;
//			double distance = 0.0;
//			foreach (var shp in shps)
//			/*{
//        if (shp.Type == ShapeType.Ellipse)
//        {
//          Ellipse ell = (Ellipse)shp;
//          distance = MGeometry.DistanceFromPointToEllipse(currentPt, ell, out pt);
//        }*/
//        if (shp.Type == ShapeType.Polygon)
//        {
//          Polygon ply = (Polygon)shp;
//          distance = MGeometry.DistanceFromPointToPolygon(currentPt, ply, out pt);
//        }
//        else if (shp.Type == ShapeType.Line)
//        {
//          Line line = (Line)shp;
//          distance = MGeometry.DistanceFromPointToLine(currentPt, line, out pt);
//        }
//        else if (shp.Type == ShapeType.Circle)
//        {
//          Circle circle = (Circle)shp;
//          distance = MGeometry.DistanceFromPointToCircle(currentPt, circle, out pt);
//        }
//        /*else if (shp.Type == ShapeType.CircularArc)
//        {
//          CircularArc arc = (CircularArc)shp;
//          distance = MGeometry.DistanceFromPointToCircularArc(currentPt, arc, out pt);
//        }
//        else if (shp.Type == ShapeType.EllipticArc)
//        {
//          EllipticArc arc = (EllipticArc)shp;
//          distance = MGeometry.DistanceFromPointToEllipticArc(currentPt, arc, out pt);
//        }*/
//      }

//			if (pt is null || distance > 10/scaleFactor)
//				return currentPt;
			
//			return pt;
//		}

//}

  /*public class MSave
	{
		public static string GetFillName(ComosysSymbolEditor.Objects.FillType fill)
		{
			switch (fill) 
			{
				case ComosysSymbolEditor.Objects.FillType.Solid:
					return "SOLID";
				case ComosysSymbolEditor.Objects.FillType.Clear:
					return "CLEAR";
				case ComosysSymbolEditor.Objects.FillType.Horizontal:
					return "HORIZONTAL";
				case ComosysSymbolEditor.Objects.FillType.Vertical:
					return "VERTICAL";
				case ComosysSymbolEditor.Objects.FillType.Cross:
					return "CROSS";
				case ComosysSymbolEditor.Objects.FillType.Concrete:
					return "CONCRETE";
				case ComosysSymbolEditor.Objects.FillType.Pixel:
					return "PIXEL";
				case ComosysSymbolEditor.Objects.FillType.Brick:
					return "Brick";
				case ComosysSymbolEditor.Objects.FillType.ConcreteLine:
					return "CONCRETELINE";
			}
			return "CLEAR";

		public static string GetColor(Shape s)
		{
			if (s.UseParentColor)
				return "USEPARENTCOLOR";

			Color c = s.Color;
			string hexcolor = $"#{c.R:X2}{c.G:X2}{c.B:X2}";

			return hexcolor;
		}*/

	//}

	//public class MLoad
	//{
	//	public static System.Drawing.Color GetColor(string hexColor, bool isOldVersion)
	//	{
	//		if (hexColor == "USEPARENTCOLOR")
	//			return Color.Empty;

	//		if (hexColor.StartsWith("#"))
	//		{
	//			hexColor = hexColor.Substring(1);
	//		}

	//		if (hexColor.Length != 6)
	//		{
	//			// Handle invalid hex color format
	//			return Color.Black;
	//		}

	//		int r = int.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
	//		int g = int.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
	//		int b = int.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

	//		if (isOldVersion && r == 255 && g == 255 && b == 255)
	//			return Color.Black;

	//		return Color.FromArgb(r, g, b);
	//	}

	//	public static Shape GetShape(string shapename)
	//	{
	//		switch (shapename.ToUpperInvariant())
	//		{
	//			case "CIRCLE":
	//				return new Circle();
	//			/*case "CIRCULARARC":
	//				return new CircularArc();
	//			case "ELLIPTICARC":
	//				return new EllipticArc();
	//			case "ELLIPSE":
	//				return new Ellipse();*/
	//			case "LINE":
	//				return new Line();
	//			case "#POLYGON":
	//				return new Polygon();
	//			//case "TEXT":
	//				//return new Text();
	//			default:
	//				break;
	//		}
				
	//		return null;
	//	}

	//	internal static void FitObjectsToFrame(List<Shape> shapes, int frameHalfSize)
	//	{
			
	//		Shapes.Rectangle box = MGeometry.FindBoundingBox(shapes);
			
	//		//Find scale factor and scale all objects
	//		double max = Math.Max(box.Width, box.Height);

	//		double scalef = 1;
	//		if (max > frameHalfSize * 2) //local size of canvas in the application
	//		{
	//			scalef = frameHalfSize * 2 / max;
	//		}

	//		if (scalef !=  1)
	//		{
	//			foreach (var s in shapes)
	//			{
	//				s.Scale(scalef);
	//			}
	//		}

	//		foreach (var s in shapes)
	//		{
	//			s.Translate(new Vector3(-frameHalfSize, -frameHalfSize));
	//		}
	//		box = MGeometry.FindBoundingBox(shapes);

	//		//Translate object to center

	//		Vector3 ctr = new Vector3(box.BottomLeft.X + box.Width/2, box.BottomLeft.Y + box.Height/2);

	//		foreach (var s in shapes)
	//		{
	//			s.Translate(new Vector3(-ctr.X, -ctr.Y));
	//		}

	//	}

 //   internal static void LoadLineStrip(List<Shape> shps, string objectParameters, double scale)
 //   {
 //     string[] coors = objectParameters.Split(' ');
	//		for (int i = 0; i < coors.Length - 1; i++)
	//		{
	//			string op = coors[i] + " " + coors[i + 1];
	//			Line l = new Line();
	//			l.LoadObject(ref op, scale);
	//			shps.Add(l);
 //     }
 //   }

 //   internal static void LoadLineLoop(List<Shape> shps, string objectParameters, double scale)
 //   {
 //     string[] coors = objectParameters.Split(' ');
	//		int j = 0;
 //     for (int i = 0; i < coors.Length; i++)
 //     {
	//			j = (i+1)% coors.Length;
 //       string op = coors[i] + " " + coors[j];
 //       Line l = new Line();
 //       l.LoadObject(ref op, scale);
 //       shps.Add(l);
 //     }
 //   }

 //   internal static void LoadOldPolygon(List<Shape> shps, string objectParameters, int scale)
 //   {
 //     string[] tokens = objectParameters.Split(' ');

 //     if (tokens.Length > 1)
 //     {
 //       List<PolygonVertex> vertices = new List<PolygonVertex>();

 //       for (int i = 0; i < tokens.Length; i++)
 //       {
 //         if (tokens[i] == "")
 //           continue;
 //         PolygonVertex v = new PolygonVertex();
 //         v.Type = VertexType.Line;
 //         v.Position.X = double.Parse(tokens[i].Split(',')[0], CultureInfo.InvariantCulture) * scale;
 //         v.Position.Y = double.Parse(tokens[i].Split(',')[1], CultureInfo.InvariantCulture) * scale;
 //         vertices.Add(v);
 //       }
	//			shps.Add(new Polygon(vertices, true));
 //     }
 //   }
 // }

//	public class MDebug
//	{
//		++public static void WriteVerticesOfAllPolygons(List<Shape> shapes)
//		{
//#if DEBUG
//			foreach (var s in shapes)
//			{
//				if (s.Type == ShapeType.Polygon)
//				{
//					Polygon p = (Polygon)s;
//					Console.WriteLine($"Count: {p.Vertexes.Count}");
//					if (p != null)
//					{
//						foreach (var v in p.Vertexes)
//						{
//							if (v.Type == VertexType.Line)
//								Console.WriteLine($"Type: Line");
//							else
//								Console.WriteLine($"Type: Arc");

//							Console.WriteLine($"X: {v.X} Y: {v.Y}");
//						}
//					}
//				}
//			}
//#endif
//		}

//    +++internal static void DrawClosestPointToObjects(List<Shape> currentShapeList,Vector3 currentPt, Graphics g)
//    {
//#if DEBUG
//			Pen myPen = new Pen(Color.Black, 1);
//			foreach (Shape shp in currentShapeList)
//      {
//        if (shp.Type == ShapeType.Ellipse)
//        {
//          Ellipse ell = (Ellipse)shp;
//          double distance = MGeometry.DistanceFromPointToEllipse(currentPt, ell, out Vector3 pt);
//          g.DrawCircle(myPen, new Circle(pt, 1));
//        }
//        else if (shp.Type == ShapeType.Polygon)
//        {
//          Polygon ply = (Polygon)shp;
//          double distance = MGeometry.DistanceFromPointToPolygon(currentPt, ply, out Vector3 pt);
//          g.DrawCircle(myPen, new Circle(pt, 1));
//        }
//        else if (shp.Type == ShapeType.Line)
//        {
//          Line line = (Line)shp;
//          double distance = MGeometry.DistanceFromPointToLine(currentPt, line, out Vector3 pt);
//          g.DrawCircle(myPen, new Circle(pt, 1));
//        }
//        else if (shp.Type == ShapeType.Circle)
//        {
//          Circle circle = (Circle)shp;
//          double distance = MGeometry.DistanceFromPointToCircle(currentPt, circle, out Vector3 pt);
//          g.DrawCircle(myPen, new Circle(pt, 1));
//        }
//       /* else if (shp.Type == ShapeType.CircularArc)
//        {
//          CircularArc arc = (CircularArc)shp;
//          double distance = MGeometry.DistanceFromPointToCircularArc(currentPt, arc, out Vector3 pt);
//          g.DrawCircle(myPen, new Circle(pt, 1));
//        }
//        else if (shp.Type == ShapeType.EllipticArc)
//        {
//          EllipticArc arc = (EllipticArc)shp;
//          double distance = MGeometry.DistanceFromPointToEllipticArc(currentPt, arc, out Vector3 pt);
//          g.DrawCircle(myPen, new Circle(pt, 1));
//        }*/
//      }
//			myPen.Dispose();
//#endif
//		}

//    public static void DrawBoundingBoxOfShapes(List<Shape> shapes, Graphics g)
//    {
//#if DEBUG
//      Pen myPen = new Pen(Color.Orange, 1);

//      foreach (var s in shapes)
//      {
//				Shapes.Rectangle box = s.GetBoundingBox();
//				box.Color = Color.FromArgb(100, 255, 140, 0);
//        g.DrawRectangle(myPen, box);
//      }
//			myPen.Dispose();
//#endif
//    }

//    public static string WritePoint(Vector3 point)
//    {
//			return $"{point.X},{point.Y}";
//    }
//  }

//}
