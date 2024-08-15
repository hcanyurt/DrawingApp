using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;
using DrawingApp.Methods;

namespace DrawingApp.Shapes
{
    public class Polygon : Shape
    {
        private List<Vector3> _vertex;
        public bool IsClosed { get; set; }
        public Polygon(List<Vector3> shapePoints)
        : base()
        {
          _vertex = new List<Vector3>(shapePoints);
          IsClosed = true;
        }
        public List<Vector3> Vertexes {  get { return _vertex; } }
        public override void Draw(Graphics g)
        {
            Pen pen = GetPen();
            for (int i = 0; i < _vertex.Count - 1; i++)
            {
               g.DrawLine(pen, _vertex[i].ToPointF, _vertex[i + 1].ToPointF);
            }
            if (IsClosed && _vertex.Count > 2)
            {
                g.DrawLine(pen, _vertex[_vertex.Count - 1].ToPointF, _vertex[0].ToPointF);
            } 
            pen.Dispose(); 
        }
       
        public override bool HitTest(Vector3 point)
        {
            double dist = MGeometry.DistanceFromPointToPolygon(point, this);
            if (dist < SelectionTolerance)
                return true;

            return false;
        }
        public override void Move(int dx, int dy)
        {
            for (int i = 0; i < _vertex.Count; i++)
            {
                _vertex[i] = new Vector3(_vertex[i].X + dx, _vertex[i].Y + dy, _vertex[i].Z);
            }
        }
        public override  Shape Clone()
        {
            List<Vector3> clonedVertices = new List<Vector3>();
            foreach (var vertex in _vertex)
            {
                clonedVertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
            }

            return new Polygon(clonedVertices)
            {
                IsSelected = this.IsSelected,
                SelectionTolerance = this.SelectionTolerance,
                IsClosed = this.IsClosed,
            };
        }

    }
}
