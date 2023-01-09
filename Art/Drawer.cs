using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Art
{
    public static class Drawer
    {
        public static Canvas grid { get; set; }

        public static void Draw(Polyline polyline)
        {
            grid.Children.Add(polyline);
        }

        public static void Draw(Label label)
        {
            grid.Children.Add(label);
        }

        public static void Draw(Ellipse ellipse)
        {
            grid.Children.Add(ellipse);
        }

        public static void Draw(List<Polyline> polylines)
        {
            foreach (var p in polylines)
            {
                Draw(p);
            }
        }

        public static void Draw(List<Label> labels)
        {
            foreach (var l in labels)
            {
                Draw(l);
            }
        }

        public static void Draw(Dictionary<Ellipse, PointUI> nodes)
        {
            foreach (var node in nodes.Keys)
            {
                Draw(node);
            }
        }

        public static void EraseImages()
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (grid.Children[i] is Image image)
                {
                    grid.Children.Remove(image);
                }
            }
        }

        public static void Erase(List<Polyline> polylines)
        {
            foreach (var p in polylines)
            {
                Erase(p);
            }
        }

        public static void Erase(Dictionary<Ellipse, PointUI> nodes)
        {
            foreach (var node in nodes.Keys)
            {
                Erase(node);
            }
        }

        public static void Erase(Polyline polyline)
        {
            grid.Children.Remove(polyline);
        }

        public static void Erase(Ellipse ellipse)
        {
            grid.Children.Remove(ellipse);
        }

        public static void Clear()
        {
            grid.Children.Clear();
        }
    }
}
