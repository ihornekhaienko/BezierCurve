using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Art
{
    public class PointUI
    {
        public delegate void MoveDelegate(double x, double y);
        public event MoveDelegate OnMove;
        public Ellipse Point { get; private set; }
        public BezierBase Bezier { get; set; }
        public double cX { get; set; }
        public double cY { get; set; }

        public PointUI(double x, double y)
        {
            cX = x;
            cY = y;
            Create();
        }

        public PointUI(double x, double y, BezierBase bezier)
        {
            cX = x;
            cY = y;
            this.Bezier = bezier;

            Create();
        }

        public Ellipse Create()
        {
            Point = new Ellipse
            {
                Fill = Brushes.Crimson,
                Width = 7,
                Height = 7
            };

            double left = cX - (Point.Width / 2);
            double top = cY - (Point.Height / 2);

            Point.Margin = new Thickness(left, top, 0, 0);

            return Point;
        }

        public void Move(double x, double y)
        {
            cX = x;
            cY = y;

            OnMove(x, y);
        }
    }
}
