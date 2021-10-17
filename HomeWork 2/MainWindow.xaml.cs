using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeWork_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double scale = 0.35;
        const int depth = 4;
        double rotate = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void BtnDrawStar_Click(object sender, RoutedEventArgs e)
        {
            image.Children.Clear();
            double xmid = image.Width / 2;
            double ymid = image.Height / 2;

            DrawStar(1, xmid, ymid, 100);
        }

        private void DrawStar(int level, double x, double y, double r)
        {
            const double offset = -Math.PI / 2;
            const double angle = 4 * Math.PI / 5;
            Polyline star = new Polyline();
            star.Stroke = Brushes.DarkRed;
            image.Children.Add(star);


            for (int i = 0; i <= 5; i++)
            {
                star.Points.Add(new Point((int)(x + r * Math.Cos(offset + i * angle)),
                    (int)(y + r * Math.Sin(offset + i * angle))));

                star.RenderTransform = new RotateTransform(rotate, x, y);

                if (level < depth)
                {
                    rotate += 72;
                    DrawStar(level + 1, x + r * Math.Cos(offset + i * angle),
                    y + r * Math.Sin(offset + i * angle), r * scale);
                }
            }
        }

        private static double _angle1 = 55 * (Math.PI / 180);

        private static double Angle1
        {
            set => _angle1 = value * (Math.PI / 180);
        }

        private static double _angle2 = 35 * (Math.PI / 180);

        private static double Angle2
        {
            set => _angle2 = value * (Math.PI / 180);
        }

        private static double _ratio = 0.7;

        private static int LengthRatio
        {
            set => _ratio = value / 100d;
        }
        private double FirstIterationLineLength = 100;
        private void BtnDrawTree_Click(object sender, RoutedEventArgs e)
        {
            image.Children.Clear();
            double xmid = image.Width;
            double ymid = image.Height;

            Draw(xmid / 2, ymid / 2, 1);
        }

        public void Draw(double x, double y, uint level)
        {
            Line line = new Line
            {
                X1 = x,
                X2 = x,
                Y1 = y,
                Y2 = y - FirstIterationLineLength,
                Stroke = Brushes.DarkRed
            };
            image.Children.Add(line);

            if (level < 12)
            {
                Draw(line, level + 1, Math.PI / 2);
            }
        }

        private void Draw(Line previousLine, uint level, double prevAngle)
        {
            double leftAngle = prevAngle + _angle1;
            double rightAngle = prevAngle - _angle2;
            Line line1 = new Line
            {
                X1 = previousLine.X2,
                X2 = previousLine.X2 + Math.Cos(leftAngle) * FirstIterationLineLength * Math.Pow(_ratio, level - 1),
                Y1 = previousLine.Y2,
                Y2 = previousLine.Y2 - Math.Sin(leftAngle) * FirstIterationLineLength * Math.Pow(_ratio, level - 1),
                Stroke = Brushes.DarkRed
            };
            Line line2 = new Line
            {
                X1 = previousLine.X2,
                X2 = previousLine.X2 + Math.Cos(rightAngle) * FirstIterationLineLength * Math.Pow(_ratio, level - 1),
                Y1 = previousLine.Y2,
                Y2 = previousLine.Y2 - Math.Sin(rightAngle) * FirstIterationLineLength * Math.Pow(_ratio, level - 1),
                Stroke = Brushes.DarkRed
            };
            image.Children.Add(line1);
            image.Children.Add(line2);
            if (level >= 10)
                return;
            Draw(line1, level + 1, leftAngle);
            Draw(line2, level + 1, rightAngle);
        }

        private double x = 0;
        private double y = 0;

        private void BtnTown_Click(object sender, RoutedEventArgs e)
        {
            Town Town1 = new Town() { x = 50 };
            Town Town2 = new Town() { x = 300 };
            Town Town3 = new Town() { x = 550 };
            Disk disk1 = new Disk() { size = 1 };
            Disk disk2 = new Disk() { size = 2 };
            Disk disk3 = new Disk() { size = 3 };

            Town1.disk.Add(disk1);
            Town1.disk.Add(disk2);
            Town1.disk.Add(disk3);

            image.Children.Clear();
            x = image.Width;
            y = image.Height;

            DrawTown(Town1);
            DrawTown(Town2);
            DrawTown(Town3);
            for (int i = 0; i < Town1.disk.Count; i++)
            {
                DrawDisks(Town1, Town1.disk[i]);
            }


           // MoveDisks(Town1, Town2, Town3, 3);


            Town1.CountDisk -= 3; //кастыль
            Town1.disk.Clear();
            var images1 = image.Children.OfType<Image>().ToList(); //Все элементы типа Image в нашем подопытном canvas1

            DeleteDisks();

            for (int i = 0; i < Town1.disk.Count; i++)
            {
                DrawDisks(Town1, Town1.disk[i]);
            }
            for (int i = 0; i < Town2.disk.Count; i++)
            {
                DrawDisks(Town2, Town2.disk[i]);
            }
            for (int i = 0; i < Town3.disk.Count; i++)
            {
                DrawDisks(Town3, Town3.disk[i]);
            }

        }
        
        void DeleteDisks()
        {
            var images1 = image.Children.OfType<Image>().ToList(); //Все элементы типа Image в нашем подопытном canvas1
            foreach (var image2 in images1)
            {
                if (image2.Name == "disk") //Соответствие на имя.
                    image.Children.Remove(image2); //Удаляем
            }
        }

        //start - откуда кладем, end - куда кладем, temp - промежуточный диск, disks - кол-во дисков

        void MoveDisks(Town start, Town temp, Town end, int disks)
        {
            if (disks > 1)
            {
                MoveDisks(start, end, temp, disks - 1);
            }

            end.disk.Add(start.disk[start.disk.Count - 1]);
            start.disk.RemoveAt(start.disk.Count - 1);

            if (disks > 1)
            {
                MoveDisks(temp, start, end, disks - 1);
            }
        }

        private void DrawTown(Town town)
        {
            Polygon basis = new Polygon();
            basis.Points.Add(new Point(town.x, y - 25));
            basis.Points.Add(new Point(town.x + town.lengthTown, y - 25));
            basis.Points.Add(new Point(town.x + town.lengthTown, y - 45));
            basis.Points.Add(new Point(town.x, y - 45));
            basis.Visibility = Visibility.Visible;
            basis.Fill = Brushes.Gray;
            image.Children.Add(basis);

            Polygon height = new Polygon();
            height.Points.Add(new Point(town.x + 80, y - 45));
            height.Points.Add(new Point(town.x + town.lengthTown -80, y - 45));
            height.Points.Add(new Point(town.x + town.lengthTown -80, y - 185));
            height.Points.Add(new Point(town.x + 80, y - 185));
            height.Visibility = Visibility.Visible;
            height.Fill = Brushes.Gray;
            image.Children.Add(height);
        }

        private void DrawDisks(Town town, Disk disk)
        {
            town.CountDisk++;
            Polygon disks = new Polygon();
            disks.Points.Add(new Point(town.x + 10 * disk.size, y - 25 - 20 * town.CountDisk));
            disks.Points.Add(new Point(town.x + town.lengthTown - 10 * disk.size, y - 25 - 20 * town.CountDisk));
            disks.Points.Add(new Point(town.x + town.lengthTown - 10 * disk.size, y - 45 - 20 * town.CountDisk));
            disks.Points.Add(new Point(town.x + 10 * disk.size, y - 45 - 20 * town.CountDisk));
            disks.Visibility = Visibility.Visible;
            disks.Fill = Brushes.DarkBlue;
            disks.Stroke = Brushes.DarkGreen;
            disks.Name = "disk";
            image.Children.Add(disks);
        }
    }
}

