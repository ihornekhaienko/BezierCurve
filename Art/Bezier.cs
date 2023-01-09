using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Art
{
    public class Bezier : BezierBase
    {
        public Bezier(Point P0, Point P1, Point P2)
        {
            order = "second";
            this.P0 = P0;
            this.P1 = P1;
            this.P2 = P2;
         
            this.Build2();
            this.Tangent2();
            this.Node2();
        }

        public Bezier(Point P0, Point P1, Point P2, Point P3)
        {
            order = "third";
            this.P0 = P0;
            this.P1 = P1;
            this.P2 = P2;
            this.P3 = P3;

            this.Build3();
            this.Tangent3();
            this.Node3();
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

        public Polyline Build3()
        {
            bezier = new Polyline
            {
                Stroke = Brushes.Navy,
                StrokeThickness = 2
            };

            for (double t = 0; t <= 1; t += 0.001)
            {
                double x = Bezier3(P0.X, P1.X, P2.X, P3.X, t);
                double y = Bezier3(P0.Y, P1.Y, P2.Y, P3.Y, t);

                bezier.Points.Add(new Point(x, y));
            }

            return bezier;
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

        private void MoveP1(double x, double y)
        {
            P1 = new Point(x, y);
        }
        private void MoveP2(double x, double y)
        {
            P2 = new Point(x, y);
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
            tangent.Points.Add(P3);

            return tangent;
        }

        public void Node3()
        {
            var node1 = new PointUI(P1.X, P1.Y, this);
            node1.OnMove += MoveP1;
            var node2 = new PointUI(P2.X, P2.Y, this);
            node2.OnMove += MoveP2;

            Nodes.Add(node1);
            Nodes.Add(node2);
        }

        public override void Rebuild()
        {
            if (order == "second")
            {
                Build2();
                Tangent2();
            }
            else
            {
                Build3();
                Tangent3();
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
            else
            {
                Build3();
                Tangent3();
                Nodes.Clear();
                Node3();
            }
        }
    }
}
