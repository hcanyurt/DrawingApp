using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using DrawingApp.Methods;

namespace DrawingApp.Shapes
{
    public class Polyline : Shape
    {
        private List<Vector3> _vertex;
        public Polyline(List<Vector3> shapePoints)
        : base()
        {
            _vertex = new List<Vector3>(shapePoints);
        }
        public List<Line> Segments
        {
            get
            {
                List<Line> segments = new List<Line>();
                for (int i = 0; i < _vertex.Count - 1; i++)
                {
                    segments.Add(new Line(_vertex[i], _vertex[i + 1]));
                }
                return segments;
            }
        }
        public override void Draw(Graphics g)
        {
            Pen pen = GetPen();
            if (_vertex.Count > 1)
            {
                for (int i = 0; i < _vertex.Count - 1; i++)
                {
                    g.DrawLine(pen, (_vertex[i].ToPointF), _vertex[i + 1].ToPointF);
                }
            }
            pen.Dispose();
        }

        public override bool HitTest(Vector3 point)
        {
            double minDist = double.MaxValue;

            foreach (var segment in Segments)
            {
                double dist = MGeometry.DistanceFromPointToLine(point, segment);
                if (dist < minDist)
                {
                    minDist = dist;
                }
            }
            return minDist < SelectionTolerance;
        }
        public override void Move(int dx, int dy)
        {
            for (int i = 0; i < _vertex.Count; i++)
            {
                _vertex[i] = new Vector3(_vertex[i].X + dx, _vertex[i].Y + dy, _vertex[i].Z);
            }
        }
        public override Shape Clone()
        {
            List<Vector3> clonedVertices = new List<Vector3>();
            foreach (var vertex in _vertex)
            {
                clonedVertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
            }

            return new Polyline(clonedVertices)
            {
                IsSelected = this.IsSelected,
                SelectionTolerance = this.SelectionTolerance,  
            };
        }
    }
}