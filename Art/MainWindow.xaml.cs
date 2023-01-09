using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Art
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public double X1;
        public double X2;
        public double Y1;
        public double Y2;

        public double oX;
        public double oY;

        public Dictionary<Ellipse, PointUI> nodes = new Dictionary<Ellipse, PointUI>();
        public List<Polyline> curves = new List<Polyline>();
        public List<Polyline> tangents = new List<Polyline>();
        public List<BezierBase> beziers = new List<BezierBase>();
        public List<List<PointUI>> points = new List<List<PointUI>>();

        public List<List<Polyline>> curvesPlane = new List<List<Polyline>>();
        public List<List<Polyline>> tangentsPlane = new List<List<Polyline>>();
        public List<Dictionary<Ellipse, PointUI>> pointsPlane = new List<Dictionary<Ellipse, PointUI>>();

        public List<List<Polyline>> curvesShark = new List<List<Polyline>>();
        public List<List<Polyline>> tangentsShark = new List<List<Polyline>>();
        public List<Dictionary<Ellipse, PointUI>> pointsShark = new List<Dictionary<Ellipse, PointUI>>();

        public List<Polyline> gridList = new List<Polyline>();
        public List<Label> gridLabels = new List<Label>();

        public string current = "Plane";
        delegate void draw(Dictionary<string, Point> points);
        public MainWindow()
        {
            InitializeComponent();
        }

        #region INIT
        private void SetDefaults()
        {
            Offsetter.Clear();
            dxTB.Text = "0";
            dyTB.Text = "0";

            nodes.Clear();
            curves.Clear();
            beziers.Clear();

            Scaler.Clear();
            scaleTB.Text = "1";

            Rotator.Clear();
            xTB.Text = "0";
            yTB.Text = "0";
            angleTB.Text = "0";
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            X1 = 10;
            X2 = grid.ActualWidth - 10;
            Y1 = 10;
            Y2 = grid.ActualHeight - 10;
            oX = X2 / 2;
            oY = Y2 / 2;

            Misc.X1 = X1;
            Misc.X2 = X2;
            Misc.Y1 = Y1;
            Misc.Y2 = Y2;
            Misc.oX = oX;
            Misc.oY = oY;

            Drawer.grid = grid;
        }

        private void defaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaults();
        }
        #endregion

        #region MISC
        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key is (>= Key.D0 and <= Key.D9) or (>= Key.NumPad0 and <= Key.NumPad9) or Key.Back or Key.Left or Key.Right or Key.OemComma or Key.OemMinus)
            {
                return;
            }
            e.Handled = true;
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.Z))
            {
                if (grid.Children.Count > 0)
                    grid.Children.Remove(grid.Children[grid.Children.Count - 1]);
            }
            if (Keyboard.IsKeyDown(Key.RightAlt))
            {
                if ((bool)drawRB.IsChecked)
                {
                    drawRB.IsChecked = false;
                    dragRB.IsChecked = true;
                }
                else
                {
                    drawRB.IsChecked = true;
                    dragRB.IsChecked = false;
                }
            }
            if (Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if ((bool)secondRB.IsChecked)
                {
                    secondRB.IsChecked = false;
                    thirdRB.IsChecked = true;
                }
                else
                {
                    secondRB.IsChecked = true;
                    thirdRB.IsChecked = false;
                }
            }
        }
        #endregion

        #region DRAWING
        Point start = new Point(-1,-1);
        Point end;

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((bool)dragRB.IsChecked)
            {
                return;
            }

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if ((bool)newRB.IsChecked || (start.X == -1 && start.Y == -1))
                {
                    start = e.GetPosition(grid);
                }
                else
                {
                    start = end;
                }
            }
        }

        private void grid_MouseMove(object sender, MouseEventArgs e)
        {
            if ((bool)dragRB.IsChecked)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                end = e.GetPosition(grid);
            }
        }

        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((bool)dragRB.IsChecked)
            {
                return;
            }

            if ((bool)secondRB.IsChecked)
            {
                if ((bool)smoothCB.IsChecked)
                {
                    DrawSmoothBezier2();
                }
                else
                {
                    DrawBezier2();
                }
            }
            else
            {
                DrawBezier3();
            }
        }

        private void DrawSmoothBezier2()
        {
            Point middle = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
            BezierSmooth bezier = new BezierSmooth(start, middle, end);
            Drawer.Draw(bezier.bezier);
            Drawer.Draw(bezier.tangent);
            foreach (var node in bezier.Nodes)
            {
                node.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                node.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                node.Point.MouseMove += OnMouseMove;

                nodes[node.Point] = node;
                Drawer.Draw(node.Point);
            }
            foreach (var node in nodes)
            {
                if (bezier.Left != null && bezier.Right != null)
                {
                    break;
                }
                if (node.Value.Bezier is BezierSmooth smooth)
                {
                    if (Misc.ComparePoints(bezier.P0, smooth.P1) || Misc.ComparePoints(bezier.P2, smooth.P1))
                    {
                        break;
                    }

                    if (Misc.ComparePoints(bezier.P0, smooth.P2))
                    {
                        if (smooth.Right == null)
                        {
                            smooth.Right = bezier;
                            bezier.Left = smooth;
                            //smooth.MoveP1(smooth.P1.X, smooth.P1.Y);
                        }
                    }

                    if (Misc.ComparePoints(bezier.P2, smooth.P0))
                    {
                        if (smooth.Left == null)
                        {
                            smooth.Left = bezier;
                            bezier.Right = smooth;
                            //smooth.MoveP1(smooth.P1.X, smooth.P1.Y);
                        }
                    }
                }
            }

            if (bezier.Left != null)
            {
                MoveSmooth(bezier, bezier.P1.X, bezier.P2.Y);
            }

            if (bezier.Right != null)
            {
                MoveSmooth(bezier, bezier.P1.X, bezier.P2.Y);
            }
        }

        private void DrawBezier2()
        {
            Point middle = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
            Bezier bezier = new Bezier(start, middle, end);
            Drawer.Draw(bezier.bezier);
            Drawer.Draw(bezier.tangent);

            foreach (var node in bezier.Nodes)
            {
                node.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                node.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                node.Point.MouseMove += OnMouseMove;

                nodes[node.Point] = node;
                Drawer.Draw(node.Point);
            }
        }

        private void DrawBezier3()
        {
            Point middle1 = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);
            Point middle2 = new Point((start.X + end.X) / 2, (start.Y + end.Y) / 2);

            //Point middle2 = new Point(2 * (start.X + end.X) / 3, 2 * (start.Y + end.Y) / 3);

            Bezier bezier = new Bezier(start, middle1, middle2, end);
            Drawer.Draw(bezier.bezier);
            Drawer.Draw(bezier.tangent);

            foreach (var node in bezier.Nodes)
            {
                node.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                node.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                node.Point.MouseMove += OnMouseMove;

                nodes[node.Point] = node;
                Drawer.Draw(node.Point);
            }
        }
        #endregion

        #region DRAGGING
        TranslateTransform originTT;
        Point clickPosition;
        bool isDragging;

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OnDoubleClick(sender as Ellipse);
                return;
            }
            if ((bool)drawRB.IsChecked)
            {
                return;
            }
            isDragging = true;
            var draggable = sender as Ellipse;
            originTT = draggable.RenderTransform as TranslateTransform ?? new TranslateTransform();
            clickPosition = e.GetPosition(grid);
            draggable.CaptureMouse();
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((bool)drawRB.IsChecked)
            {
                return;
            }
            isDragging = false;
            var draggable = sender as Ellipse;
            draggable.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if ((bool)drawRB.IsChecked)
            {
                return;
            }
            var draggable = sender as Ellipse;
            if (isDragging && draggable != null)
            {
                Point currentPosition = e.GetPosition(grid);
                var transform = draggable.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X = originTT.X + (currentPosition.X - clickPosition.X);
                transform.Y = originTT.Y + (currentPosition.Y - clickPosition.Y);
                draggable.RenderTransform = new TranslateTransform(transform.X, transform.Y);
                Drawer.Erase(nodes[draggable].Bezier.bezier);
                Drawer.Erase(nodes[draggable].Bezier.tangent);

                if (nodes[draggable].Bezier is Bezier)
                {
                    Move(nodes[draggable], currentPosition.X, currentPosition.Y);
                }
                else
                {
                    MoveSmooth(nodes[draggable], currentPosition.X, currentPosition.Y);
                }

                Drawer.Draw(nodes[draggable].Bezier.bezier);
                Drawer.Draw(nodes[draggable].Bezier.tangent);
            }
        }
        private void Move(PointUI point, double x, double y)
        {
            point.Move(x, y);
            point.Bezier.Rebuild();
        }

        private void MoveSmooth(PointUI point, double x, double y)
        {
            var bezier = point.Bezier as BezierSmooth;
            if (bezier.Left is not null and BezierSmooth leftBefore)
            {
                foreach (var i in leftBefore.Nodes)
                {
                    nodes.Remove(i.Point);
                    Drawer.Erase(i.Point);
                }
                Drawer.Erase(leftBefore.bezier);
                Drawer.Erase(leftBefore.tangent);
            }
            if (bezier.Right is not null and BezierSmooth rightBefore)
            {
                foreach (var i in rightBefore.Nodes)
                {
                    nodes.Remove(i.Point);
                    Drawer.Erase(i.Point);
                }
                Drawer.Erase(rightBefore.bezier);
                Drawer.Erase(rightBefore.tangent);
            }

            point.Move(x, y);
            bezier.Rebuild();

            if (bezier.Left is not null and BezierSmooth leftAfter)
            {
                Drawer.Draw(leftAfter.bezier);
                Drawer.Draw(leftAfter.tangent);
                foreach (var i in leftAfter.Nodes)
                {
                    i.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    i.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                    i.Point.MouseMove += OnMouseMove;

                    nodes[i.Point] = i;
                    Drawer.Draw(i.Point);
                }
            }
            if (bezier.Right is not null and BezierSmooth rightAfter)
            {
                Drawer.Draw(rightAfter.bezier);
                Drawer.Draw(rightAfter.tangent);
                foreach (var i in rightAfter.Nodes)
                {
                    i.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    i.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                    i.Point.MouseMove += OnMouseMove;

                    nodes[i.Point] = i;
                    Drawer.Draw(i.Point);
                }
            }
        }

        private void MoveSmooth(BezierSmooth bezier, double x, double y)
        {
            Drawer.Erase(bezier.bezier);
            Drawer.Erase(bezier.tangent);
            foreach (var i in bezier.Nodes)
            {
                nodes.Remove(i.Point);
                Drawer.Erase(i.Point);
            }

            if (bezier.Left is not null and BezierSmooth leftBefore)
            {
                foreach (var i in leftBefore.Nodes)
                {
                    nodes.Remove(i.Point);
                    Drawer.Erase(i.Point);
                }
                Drawer.Erase(leftBefore.bezier);
                Drawer.Erase(leftBefore.tangent);
            }
            if (bezier.Right is not null and BezierSmooth rightBefore)
            {
                foreach (var i in rightBefore.Nodes)
                {
                    nodes.Remove(i.Point);
                    Drawer.Erase(i.Point);
                }
                Drawer.Erase(rightBefore.bezier);
                Drawer.Erase(rightBefore.tangent);
            }

            bezier.MoveP1(x, y);
            bezier.RebuildWithNodes();

            if (bezier.Left != null && bezier.Left is BezierSmooth leftAfter)
            {
                Drawer.Draw(leftAfter.bezier);
                Drawer.Draw(leftAfter.tangent);
                foreach (var i in leftAfter.Nodes)
                {
                    i.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    i.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                    i.Point.MouseMove += OnMouseMove;

                    nodes[i.Point] = i;
                    Drawer.Draw(i.Point);
                }
            }
            if (bezier.Right != null && bezier.Right is BezierSmooth rightAfter)
            {
                Drawer.Draw(rightAfter.bezier);
                Drawer.Draw(rightAfter.tangent);
                foreach (var i in rightAfter.Nodes)
                {
                    i.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    i.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                    i.Point.MouseMove += OnMouseMove;

                    nodes[i.Point] = i;
                    Drawer.Draw(i.Point);
                }
            }

            Drawer.Draw(bezier.bezier);
            Drawer.Draw(bezier.tangent);
            foreach (var i in bezier.Nodes)
            {
                i.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                i.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                i.Point.MouseMove += OnMouseMove;
                nodes[i.Point] = i;
                Drawer.Draw(i.Point);
            }
        }
        #endregion

        #region DOUBLE CLICK
        private async void OnDoubleClick(Ellipse sender)
        {
            var bezier = nodes[sender].Bezier;
            string path = @"C:\projects\7\Леденящий душу пиздец\4\Art\Art\log.txt";
            string message;
            if (bezier.order == "second")
            {
                message = $"P0({bezier.P0.X} {bezier.P0.Y})\nP1({bezier.P1.X} {bezier.P1.Y})\nP2({bezier.P2.X} {bezier.P2.Y})";
            }
            else
            {
                message = $"P0({bezier.P0.X} {bezier.P0.Y})\nP1({bezier.P1.X} {bezier.P1.Y})\nP2({bezier.P2.X} {bezier.P2.Y})\nP3({bezier.P3.X} {bezier.P3.Y})";
            }
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                await sw.WriteLineAsync(message);
            }
            MessageBox.Show(message);
        }
        #endregion

        #region BUILD
        private void Grid()
        {
            Grid grid = new Grid();

            grid.Build();

            gridList = grid.gridList;
            gridLabels = grid.gridLabels;

            Drawer.Clear();

            Drawer.Draw(gridList);
            Drawer.Draw(gridLabels);
        }

        private void Setup()
        {
            if (current == "Plane")
            {
                Plane.GeneratePoints();
                Plane.Build();
                beziers = Plane.Beziers;
            }
            else
            {
                Shark.GeneratePoints();
                Shark.Build();
                beziers = Shark.Beziers;
            }

            curves = beziers.Select(b => b.bezier).ToList();
            tangents = beziers.Select(b => b.tangent).ToList();
            points = beziers.Select(b => b.Nodes).ToList();
            nodes = new Dictionary<Ellipse, PointUI>();
            foreach (var i in points)
            {
                foreach (var j in i)
                {
                    j.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    j.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                    j.Point.MouseMove += OnMouseMove;

                    nodes[j.Point] = j;
                }
            }
        }

        private void Draw()
        {
            Grid();

            Drawer.Draw(curves);
            Drawer.Draw(tangents);
            Drawer.Draw(nodes);
        }

        public void Build()
        {
            Setup();
            Draw();
        }

        private void gridButton_Click(object sender, RoutedEventArgs e)
        {
            Grid();
        }

        private void planeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                current = "Plane";
                Build();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void sharkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                current = "Shark";
                Build();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        #endregion

        #region IMAGE
        private void loadPicture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double xFactor = !string.IsNullOrWhiteSpace(xImageTB.Text) ? Convert.ToDouble(xImageTB.Text) : 1;
                double yFactor = !string.IsNullOrWhiteSpace(yImageTB.Text) ? Convert.ToDouble(yImageTB.Text) : 1;

                string path = OpenImage();
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(path));
                image.Width = 592 * xFactor;
                image.Height = 421 * yFactor;
                image.Margin = new Thickness(oX - image.Width / 2, oY - image.Height / 2, 0, 0);
                grid.Children.Add(image);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private string OpenImage()
        {
            string path = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    path = openFileDialog.FileName;
                }
                catch
                {
                    MessageBox.Show("Failed to open image");
                }
            }
            return path;
        }

        private void erasePicture_Click(object sender, RoutedEventArgs e)
        {
            Drawer.EraseImages();
        }
        #endregion

        #region ROTATION
        private void RotateAnime(double angle)
        {
            double degree = 1;
            if (angle < 0)
            {
                degree *= -1;
                angle *= -1;
            }

            for (int i = 0; i < angle; i++)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => RotateAnimeInner(Misc.DegreesToRadians(degree)))).Wait();
            }
        }

        private void RotateAnimeInner(double angle)
        {
            //MessageBox.Show(angle.ToString());
            foreach (var bezier in beziers)
            {
                Rotate(bezier, angle);
            }
        }

        private void Rotate(BezierBase bezier, double angle)
        {
            Drawer.Erase(bezier.bezier);
            curves.Remove(bezier.bezier);
            Drawer.Erase(bezier.tangent);
            tangents.Remove(bezier.tangent);
            foreach (var i in bezier.Nodes)
            {
                var node = nodes.Where(n => n.Value == i).SingleOrDefault();
                Drawer.Erase(node.Key);
                nodes.Remove(node.Key);
            }

            Rotator.Rotate(bezier, angle, bezier.dX, bezier.dY);
            bezier.RebuildWithNodes();

            curves.Add(bezier.bezier);
            Drawer.Draw(bezier.bezier);
            tangents.Add(bezier.tangent);
            Drawer.Draw(bezier.tangent);
            foreach (var j in bezier.Nodes)
            {
                j.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                j.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                j.Point.MouseMove += OnMouseMove;

                nodes[j.Point] = j;
                Drawer.Draw(j.Point);
            }
        }
        private void angleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            angleTB.Text = angleSlider.Value.ToString();
        }

        private void angleTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(angleTB.Text) && angleTB.Text != "-")
                {
                    double angle = Convert.ToDouble(angleTB.Text);
                    if (angle is >= 0 and <= 360)
                    {
                        if (angleSlider != null)
                            angleSlider.Value = angle;
                    }
                }
            }
            catch
            {

            }
        }

        private void rotateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(xTB.Text) && !string.IsNullOrWhiteSpace(yTB.Text) && !string.IsNullOrWhiteSpace(angleTB.Text))
                {
                    Rotator.rX += Convert.ToDouble(xTB.Text);
                    Rotator.rY += Convert.ToDouble(yTB.Text);
                    Rotator.angle += Misc.DegreesToRadians(Convert.ToDouble(angleTB.Text));
                }

                RotateAnime(Convert.ToDouble(angleTB.Text));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #endregion

        #region OFFSET

        private void OffsetAnime(double dx, double dy)
        {
            double pX = 1;
            double pY = 1;
            if (dx < 0)
            {
                pX *= -1;
                dx *= -1;
            }
            if (dy < 0)
            {
                pY *= -1;
                dy *= -1;
            }
            for (double i = 0, j = 0; i < dx || j < dy; i += 0.05, j += 0.05)
            {
                if (i >= dx)
                {
                    pX = 0;
                }
                if (j >= dy)
                {
                    pY = 0;
                }

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => OffsetAnimeInner(pX, pY))).Wait();
            }
        }

        private void OffsetAnimeInner(double dx, double dy)
        {
            foreach (var bezier in beziers)
            {
                bezier.dX += dx;
                bezier.dY += dy;

                Offset(bezier, dx, dy);
            }
        }

        private void Offset(BezierBase bezier, double dx, double dy)
        {
            Drawer.Erase(bezier.bezier);
            curves.Remove(bezier.bezier);
            Drawer.Erase(bezier.tangent);
            tangents.Remove(bezier.tangent);
            foreach (var i in bezier.Nodes)
            {
                var node = nodes.Where(n => n.Value == i).SingleOrDefault();
                Drawer.Erase(node.Key);
                nodes.Remove(node.Key);
            }

            Offsetter.Offset(bezier, dx, dy);
            bezier.RebuildWithNodes();

            curves.Add(bezier.bezier);
            Drawer.Draw(bezier.bezier);
            tangents.Add(bezier.tangent);
            Drawer.Draw(bezier.tangent);
            foreach (var j in bezier.Nodes)
            {
                j.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                j.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                j.Point.MouseMove += OnMouseMove;

                nodes[j.Point] = j;
                Drawer.Draw(j.Point);
            }
        }

        private void offsetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(dxTB.Text) && !string.IsNullOrWhiteSpace(dyTB.Text))
                {
                    Offsetter.dX = Misc.CellsToPixels(Convert.ToDouble(dxTB.Text)) * 2;
                    Offsetter.dY = Misc.CellsToPixels(Convert.ToDouble(dyTB.Text)) * 2;
                }

                OffsetAnime(Convert.ToDouble(dxTB.Text), Convert.ToDouble(dyTB.Text));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #endregion

        #region SCALE
        private void scaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scaleTB.Text = scaleSlider.Value.ToString();
        }

        private void scaleTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(scaleTB.Text))
                {
                    double scale = Convert.ToDouble(scaleTB.Text);
                    if (scale is >= 0.1 and <= 2)
                    {
                        scaleSlider.Value = scale;
                    }
                }
            }
            catch
            {

            }
        }

        private void scaleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(scaleTB.Text))
                {
                    if (string.IsNullOrWhiteSpace(scaleTB.Text))
                    {
                        return;
                    }
                    double factor = Convert.ToDouble(scaleTB.Text);
                    if (factor is < 0.1 or > 2)
                    {
                        throw new Exception("Scale should be in the range from 0.1 to 2");
                    }

                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #endregion

        #region ANIME
        private void Anime()
        {
            try
            {
                Shark.GeneratePoints();
                Plane.GeneratePoints();
                var source = current == "Plane" ? Plane.Points : Shark.Points ;
                var destination = current == "Plane" ?  Shark.Points : Plane.Points;
                draw drawer = current == "Plane" ? DrawShark : DrawPlane;

                bool flag = true;

                while (flag)
                {
                    flag = false;
                    for (int i = 0; i < source.Count; i++)
                    {
                        var point = source.ElementAt(i);
                        double dx = 0;
                        double dy = 0;
                   
                        if (point.Value.X != destination[point.Key].X)
                        {
                            flag = true;
                            dx = point.Value.X < destination[point.Key].X ? 1 : -1;
                        }

                        if (point.Value.Y != destination[point.Key].Y)
                        {
                            flag = true;
                            dy = point.Value.Y < destination[point.Key].Y ? -1 : 1;
                        }

                        source[point.Key] = new Point(point.Value.X + dx, point.Value.Y - dy);
                    }
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => drawer(source))).Wait();
                }
                current = current == "Plane" ? "Shark" : "Plane";
                Build();
                MessageBox.Show("finished");
            }
            catch
            {

            }
        }

        private void DrawPlane(Dictionary<string, Point> dict)
        {
            Drawer.Erase(curves);
            Drawer.Erase(tangents);
            Drawer.Erase(nodes);

            Plane.Build(dict);
            beziers = Plane.Beziers;
            curves = beziers.Select(b => b.bezier).ToList();
            tangents = beziers.Select(b => b.tangent).ToList();
            points = beziers.Select(b => b.Nodes).ToList();
            nodes = new Dictionary<Ellipse, PointUI>();
            foreach (var i in points)
            {
                foreach (var j in i)
                {
                    j.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    j.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                    j.Point.MouseMove += OnMouseMove;

                    nodes[j.Point] = j;
                }
            }

            Drawer.Draw(curves);
            Drawer.Draw(tangents);
            Drawer.Draw(nodes);

            curvesPlane.Add(curves);
            tangentsPlane.Add(tangents);
            pointsPlane.Add(nodes);
        }

        private void DrawShark(Dictionary<string, Point> dict)
        {
            Drawer.Erase(curves);
            Drawer.Erase(tangents);
            Drawer.Erase(nodes);

            Shark.Build(dict);
            beziers = Shark.Beziers;
            curves = beziers.Select(b => b.bezier).ToList();
            tangents = beziers.Select(b => b.tangent).ToList();
            points = beziers.Select(b => b.Nodes).ToList();
            nodes = new Dictionary<Ellipse, PointUI>();
            foreach (var i in points)
            {
                foreach (var j in i)
                {
                    j.Point.MouseLeftButtonDown += OnMouseLeftButtonDown;
                    j.Point.MouseLeftButtonUp += OnMouseLeftButtonUp;
                    j.Point.MouseMove += OnMouseMove;

                    nodes[j.Point] = j;
                }
            }

            Drawer.Draw(curves);
            Drawer.Draw(tangents);
            Drawer.Draw(nodes);

            curvesShark.Add(curves);
            tangentsShark.Add(tangents);
            pointsShark.Add(nodes);
        }

        private void animeButton_Click(object sender, RoutedEventArgs e)
        {
            if (current == "Plane")
            {
                if (curvesShark.Count == 0)
                {
                    Anime();
                    return;
                }
            }
            else
            {
                if (curvesPlane.Count == 0)
                {
                    Anime();
                    return;
                }
            }

            Cheat();
        }

        private void Cheat()
        {
            if (current == "Plane")
            {
                for (int i = 0; i < curvesShark.Count; i++)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => DrawCheat(curvesShark[i], tangentsShark[i], pointsShark[i]))).Wait();
                }
            }
            else
            {
                for (int i = 0; i < curvesPlane.Count; i++)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => DrawCheat(curvesPlane[i], tangentsPlane[i], pointsPlane[i]))).Wait();
                }
            }

            current = current == "Plane" ? "Shark" : "Plane";
            Build();
        }

        private void DrawCheat(List<Polyline> _curves, List<Polyline> _tangents, Dictionary<Ellipse, PointUI> _points)
        {
            Drawer.Erase(curves);
            Drawer.Erase(tangents);
            Drawer.Erase(nodes);

            curves = _curves;
            tangents = _tangents;
            nodes = _points;

            Drawer.Draw(curves);
            Drawer.Draw(tangents);
            Drawer.Draw(nodes);
        }
        #endregion
    }
}
