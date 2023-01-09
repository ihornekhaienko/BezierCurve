using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Art
{
    public class BezierSmooth : BezierBase
    {
        public BezierBase Left { get; set; }
        public BezierBase Right { get; set; }

        public BezierSmooth(Point P0, Point P1, Point P2)
        {
            order = "second";
            this.P0 = P0;
            this.P1 = P1;
            this.P2 = P2;

            this.Build2();
            this.Tangent2();
            this.Node2();
        }

        public Polyline Build2()
        {
            bezier = new Polyline
            {
                StrokeThickness = 2,
                Stroke = Brushes.Navy
            };

            for (double t = 0; t <= 1; t += 0.001)
            {
                double x = Bezier2(P0.X, P1.X, P2.X, t);
                double y = Bezier2(P0.Y, P1.Y, P2.Y, t);

                bezier.Points.Add(new Point(x, y));
            }

            return bezier;
        }

        public static double Bezier2(double p0, double p1, double p2, double t)
        {
            return (Math.Pow(1 - t, 2) * p0) + (2 * t * (1 - t) * p1) + (t * t * p2);
        }

        private static double Bezier3(double p0, double p1, double p2, double p3, double t)
        {
            return (Math.Pow(1 - t, 3) * p0)
                + (3 * t * Math.Pow(1 - t, 2) * p1)
                    + (3 * t * t * (1 - t) * p2)
                        + (Math.Pow(t, 3) * p3);
        }

        public Polyline Tangent2()
        {
            tangent = new Polyline
            {
                Stroke = Brushes.Gray,
                StrokeThickness = 1.5
            };

            tangent.Points.Add(P0);
            tangent.Points.Add(P1);
            tangent.Points.Add(P2);

            return tangent;
        }

        public void Node2()
        {
            var node = new PointUI(P1.X, P1.Y, this);
            node.OnMove += MoveP1;
            Nodes.Add(node);
        }

        public void MoveP1(double x, double y)
        {
            var tmp = P1;
            P1 = new Point(x, y);

            if (Left is not null and BezierSmooth left)
            {
                double l = VectorLength(left.P1, P0) / VectorLength(P0, tmp);
                SmoothifyLeft(left, l);
            }

            if (Right is not null and BezierSmooth right)
            {
                double l = VectorLength(tmp, P0) / VectorLength(P0, right.P1);
                SmoothifyRight(right, l);
            }
        }

        private void SmoothifyLeft(BezierSmooth bezier, double l)
        {
            double x = (bezier.P1.X + l * P1.X) / (1 + l);
            double y = (bezier.P1.Y + l * P1.Y) / (1 + l);

            P0 = new Point(x, y);
            bezier.P2 = P0;

            bezier.RebuildWithNodes();
        }

        private void SmoothifyRight(BezierSmooth bezier, double l)
        {
            double x = (bezier.P1.X + l * P1.X) / (1 + l);
            double y = (bezier.P1.Y + l * P1.Y) / (1 + l);

            P2 = new Point(x, y);
            bezier.P0 = P2;

            bezier.RebuildWithNodes();
        }

        private double VectorLength(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private bool OnOneLine(Point p1, Point p2, Point p3)
        {
            return Math.Abs((p3.X - p1.X) / (p2.X - p1.X) - (p3.Y - p1.Y) / (p2.Y - p1.Y)) > 0.09;
        }
        public Polyline Tangent3()
        {
            tangent = new Polyline
            {
                Stroke = Brushes.Gray,
                StrokeThickness = 1.5
            };

            tangent.Points.Add(P0);
            tangent.Points.Add(P1);
            tangent.Points.Add(P2);

            return tangent;
        }

        public override void Rebuild()
        {
            if (order == "second")
            {
                Build2();
                Tangent2();
            }
        }

        public override void RebuildWithNodes()
        {
            if (order == "second")
            {
                Build2();
                Tangent2();
                Nodes.Clear();
                Node2();
            }
        }
    }
}
