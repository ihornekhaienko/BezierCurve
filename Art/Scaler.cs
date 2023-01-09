using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;

namespace Art
{
    public static class Scaler
    {
        public static double factor { get; set; }
        private static List<ScaleLine> list;

        static Scaler()
        {
            Clear();
        }

        public static void Clear()
        {
            list = new List<ScaleLine>();
            factor = 1;
        }

        public static void Scale(Polyline line, double factor, double dX, double dY)
        {
            ScaleLine scaleLine;
            if (list.Any(sl => sl.current == line))
            {
                scaleLine = list.Where(sl => sl.current == line).FirstOrDefault();
            }
            else
            {
                scaleLine = new ScaleLine
                {
                    original = Copy(line),
                    current = line
                };
                list.Add(scaleLine);
            }

            for (int i = 0; i < scaleLine.current.Points.Count; i++)
            {
                double x0 = scaleLine.original.Points[i].X - Misc.oX - dX;
                double y0 = Misc.oY - scaleLine.original.Points[i].Y - dY;

                x0 *= factor;
                y0 *= factor;

                double x = Misc.oX + dX + x0;
                double y = Misc.oY - dY - y0;

                scaleLine.current.Points[i] = new Point(x, y);
            }
        }

        public static void Scale(Polyline line, double dX, double dY)
        {
            Scale(line, factor, dX, dY);
        }

        private static Polyline Copy(Polyline p1)
        {
            Polyline p2 = new Polyline();

            foreach (var p in p1.Points)
            {
                p2.Points.Add(new Point(p.X, p.Y));
            }

            return p2;
        }


    }

    class ScaleLine
    {
        public Polyline original { get; set; }
        public Polyline current { get; set; }
    }
}
