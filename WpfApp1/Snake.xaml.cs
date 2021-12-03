using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Snake.xaml
    /// </summary>
    public partial class Snake : Window
    {
        private List<Segment> Python;
        private Random random;
        private DispatcherTimer timer;
        MoveDirection moveDirection;

        public Snake()
        {
            InitializeComponent();
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(300) };
            timer.Tick += TimerTick;
            Python = new List<Segment>();
            random = new Random();
            for (int i = 0; i < 5; i++)
            {
                Python.Add(new Segment
                {
                    Figure = new Ellipse
                    {
                        Width = 20,
                        Height = 20,
                        Fill = new SolidColorBrush(
                            Color.FromRgb(
                                (byte)random.Next(100,250),
                                (byte)random.Next(100, 250),
                                (byte)random.Next(100, 250)))
                    },
                    X = 400 + i * 20,
                    Y = 100
                });
            }
        }
        //1. (Простое решение) Хвостовой сегмент перенести вперед
        private void TimerTickBW(object sender, EventArgs e)
        {
            
            Segment tail = Python.First();// ссылка на 1-й элемент
            Segment head = Python.Last();
            Python.Remove(tail);
            switch (moveDirection)
            {
                case MoveDirection.Left:
                    tail.X = head.X - tail.Figure.Height;
                    tail.Y = head.Y;
                    break;
                case MoveDirection.Right:
                    tail.X = head.X + tail.Figure.Height;
                    tail.Y = head.Y;
                    break;
                case MoveDirection.Up:
                    tail.Y = head.Y - tail.Figure.Width;
                    tail.X = head.X;
                    break;
                case MoveDirection.Down:
                    tail.Y = head.Y + tail.Figure.Width;
                    tail.X = head.X;
                    break;


            }
            Python.Add(tail);
            tail.Show(Field);
        }

        private void TimerTick(object sender, EventArgs e)
        {

            for (int i = Python.Count - 1; i >= 1; i--)
            {
                Segment seg = Python[i];
                Segment seg_1 = Python[i - 1];
                seg.Y = seg_1.Y;
                seg.X = seg_1.X;
            }

            switch (moveDirection)
            {
                case MoveDirection.Left:
                    Python[0].X -= 20;
                    break;
                case MoveDirection.Right:
                    Python[0].X += 20;
                    break;
                case MoveDirection.Up:
                    Python[0].Y -= 20;
                    break;
                case MoveDirection.Down:
                    Python[0].Y += 20;
                    break;
            }
            ShowPython();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowPython();
            moveDirection = MoveDirection.Left;
            timer.Start();
        }

        private void ShowPython()
        {
            foreach (var segment in Python)
            {
                segment.Show(Field);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:   if(moveDirection !=MoveDirection.Right) moveDirection = MoveDirection.Left; break;
                case Key.Right:  if(moveDirection != MoveDirection.Left) moveDirection = MoveDirection.Right; break;
                case Key.Up:     if(moveDirection != MoveDirection.Down) moveDirection = MoveDirection.Up; break;
                case Key.Down:   if(moveDirection != MoveDirection.Up) moveDirection = MoveDirection.Down; break;

            }
        }

        public enum MoveDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        public class Segment
        {
            //Композиция - сильная связь с объектом FrameworkElement


            public FrameworkElement Figure { get; set; }

            public double X { get; set; }
            public double Y { get; set; }
            public void Show(Canvas field)
            {
                if (!field.Children.Contains(Figure))
                {
                    field.Children.Add(Figure);
                }
                Canvas.SetLeft(Figure, X);
                Canvas.SetTop(Figure, Y);

            }

        }

    }
}
