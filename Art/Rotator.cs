using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Art
{
    public static class Rotator
    {
        public static double rX { get; set; }
        public static double rY { get; set; }
        public static double angle { get; set; }

        static Rotator()
        {
            Clear();
        }

        public static void Clear()
        {
            rX = 0;
            rY = 0;
            angle = 0;
        }

        public static Point Rotate(Point point, double angle, double dX = 0, double dY = 0)
        {
            double x = Misc.ShizaToX(point.X) - rX - dX;
            double y = Misc.ShizaToY(point.Y) - rY - dY;

            double u = x * Math.Cos(angle) - y * Math.Sin(angle);
            double v = x * Math.Sin(angle) + y * Math.Cos(angle);

            u += rX + dX;
            v += rY + dY;

            return new Point(Misc.XToShiza(u), Misc.YToShiza(v));
        }

        public static void Rotate(BezierBase bezier, double angle, double dX = 0, double dY = 0)
        {
            bezier.P0 = Rotate(bezier.P0, angle, dX, dY);
            bezier.P1 = Rotate(bezier.P1, angle, dX, dY);
            bezier.P2 = Rotate(bezier.P2, angle, dX, dY);
            if (bezier.order == "third")
                bezier.P3 = Rotate(bezier.P3, angle, dX, dY);
        }
    }
}
