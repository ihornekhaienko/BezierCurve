using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Art
{
    public class Grid
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double oX { get; set; }
        public double oY { get; set; }

        public List<Polyline> gridList = new List<Polyline>();
        public List<Label> gridLabels = new List<Label>();

        public Grid()
        {
            X1 = Misc.X1;
            X2 = Misc.X2;
            Y1 = Misc.Y1;
            Y2 = Misc.Y2;
            oX = Misc.oX;
            oY = Misc.oY;
        }

        public void Build()
        {
            MakeGrid();
            BuildGridLabels();
        }

        public void MakeGrid()
        {
            gridList.Clear();
            Polyline xAxis = new Polyline();
            xAxis.Points.Add(new Point(X1 - 10, oY));
            xAxis.Points.Add(new Point(X2, oY));
            xAxis.Stroke = Brushes.Black;
            xAxis.StrokeThickness = 2;
            gridList.Add(xAxis);

            Polyline yAxis = new Polyline();
            yAxis.Points.Add(new Point(oX, Y1));
            yAxis.Points.Add(new Point(oX, Y2));
            yAxis.Stroke = Brushes.Black;
            yAxis.StrokeThickness = 2;
            gridList.Add(yAxis);

            for (double i = X1; i < X2; i += 20)
            {
                Polyline line = new Polyline();
                line.Points.Add(new Point(i, Y1));
                line.Points.Add(new Point(i, Y2));
                line.Stroke = Brushes.Gray;
                gridList.Add(line);
            }

            for (double i = Y1 - 14; i < Y2; i += 20)
            {
                if (i >= Y1)
                {
                    Polyline line = new Polyline();
                    line.Points.Add(new Point(X1 - 10, i));
                    line.Points.Add(new Point(X2, i));
                    line.Stroke = Brushes.Gray;
                    gridList.Add(line);
                }
            }

            Polyline xArr = new Polyline();
            xArr.Points.Add(new Point(X2 - 5, oY - 5));
            xArr.Points.Add(new Point(X2, oY));
            xArr.Points.Add(new Point(X2 - 5, oY + 5));
            xArr.Stroke = Brushes.Black;
            xArr.StrokeThickness = 2;
            gridList.Add(xArr);

            Polyline yArr = new Polyline();
            yArr.Points.Add(new Point(oX - 5, Y1 + 5));
            yArr.Points.Add(new Point(oX, Y1));
            yArr.Points.Add(new Point(oX + 5, Y1 + 5));
            yArr.Stroke = Brushes.Black;
            yArr.StrokeThickness = 2;
            gridList.Add(yArr);
        }

        public void BuildGridLabels()
        {
            gridLabels.Clear();
            var xArr = gridList[gridList.Count - 2];
            Label xLabel = new Label();
            xLabel.Content = "x";
            xLabel.FontSize = 18;
            xLabel.Margin = new Thickness(xArr.Points[2].X - 30, xArr.Points[2].Y - 10, 0, 0);
            gridLabels.Add(xLabel);

            var yArr = gridList[gridList.Count - 1];
            Label yLabel = new Label();
            yLabel.Content = "y";
            yLabel.FontSize = 18;
            yLabel.Margin = new Thickness(yArr.Points[2].X + 10, yArr.Points[2].Y - 10, 0, 0);
            gridLabels.Add(yLabel);
        }
    }
}
