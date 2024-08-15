using DrawingApp.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using DrawingApp.Methods;

namespace DrawingApp.Shapes
{
    public class Line : Shape
    {
        private Vector3 _stPoint;
        private Vector3 _endPoint;
       
        public Line()
            : base()
        {
            
        }
        
        public Line(Vector3 stPt, Vector3 endPt)
            : this()
        {
           _stPoint = stPt;
           _endPoint = endPt;
        }
        public Vector3 Start { get { return _stPoint; } }
        public Vector3 End { get { return _endPoint; } }
        public double Length { get { return Math.Sqrt(Math.Pow(Start.X - End.X,2) + Math.Pow(Start.Y - End.Y, 2)); } }

        public override void Draw(Graphics g)
        {
            Pen pen = GetPen();
            g.DrawLine(pen, _stPoint.ToPointF, _endPoint.ToPointF);
            pen.Dispose();
        }

        public override bool HitTest(Vector3 point)
        {
            double dist = MGeometry.DistanceFromPointToLine(point,this);
            if (dist < SelectionTolerance)
                return true;

            return false;
        }
        public override void Move(int dx, int dy)
        {
            _stPoint = new Vector3(_stPoint.X + dx, _stPoint.Y + dy, _stPoint.Z);
            _endPoint = new Vector3(_endPoint.X + dx, _endPoint.Y + dy, _endPoint.Z);
        }
        public override Shape Clone()
        {
            return new Line(_stPoint,_endPoint)
            {
                IsSelected = this.IsSelected,
                SelectionTolerance = this.SelectionTolerance,
            };
        }

    }


}



    
     
  
