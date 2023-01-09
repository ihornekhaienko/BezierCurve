using System.Windows;

namespace Art
{
    public class MyPoint
    {
        public Point P { get; set; }
        public Type Type { get; set; }

        public MyPoint(Point p)
        {
            P = p;
        }

        public MyPoint(double x, double y)
        {
            P = new Point(x, y);
        }

        public MyPoint(Point p, Type type)
        {
            P = p;
            Type = type;
        }

        public MyPoint(double x, double y, Type type)
        {
            P = new Point(x, y);
            Type = type;
        }
    }

    public enum Type
    {
        Node,
        NodeSmooth,
        Raper
    }
}
