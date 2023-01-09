using System.Linq;
using System.Windows;
using System.Windows.Shapes;

namespace Art
{
    public static class Offsetter
    {
        public static double dX { get; set; }
        public static double dY { get; set; }

        static Offsetter()
        {
            Clear();
        }

        public static void Clear()
        {
            dX = 0;
            dY = 0;
        }

        public static void Offset(Polyline curve, double dX, double dY)
        {
            for (int i = 0; i < curve.Points.Count(); i++)
            {
                curve.Points[i] = new Point(curve.Points[i].X + dX, curve.Points[i].Y - dY);
            }
        }

        public static Point Offset(Point point, double dX, double dY)
        {
            return new Point(point.X + dX, point.Y - dY);
        }

        public static void Offset(BezierBase bezier, double dX, double dY)
        {
            bezier.P0 = Offset(bezier.P0, dX, dY);
            bezier.P1 = Offset(bezier.P1, dX, dY);
            bezier.P2 = Offset(bezier.P2, dX, dY);
            if (bezier.order == "third")
                bezier.P3 = Offset(bezier.P3, dX, dY);
        }
    }
}
