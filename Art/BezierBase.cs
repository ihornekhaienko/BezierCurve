using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Art
{
    public abstract class BezierBase
    {
        public Polyline bezier { get; set; }
        public Polyline tangent { get; set; }
        public Point P0 { get; set; }
        public Point P1 { get; set; }
        public Point P2 { get; set; }
        public Point P3 { get; set; }
        public List<PointUI> Nodes { get; set; } = new List<PointUI>();
        public string order;
        public double dX = 0;
        public double dY = 0;

        public abstract void Rebuild();
        public abstract void RebuildWithNodes();
    }
}
