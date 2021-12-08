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
        public static Random random;
        private List<int> foodIndexes;
        private List<Segment> Python;
        private DispatcherTimer timer;
        MoveDirection moveDirection;
        private Food fruit;

        public Snake()
        {
            InitializeComponent();
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
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
                                (byte)random.Next(100, 250),
                                (byte)random.Next(100, 250),
                                (byte)random.Next(100, 250)))
                    },
                    X = 400 + i * 20,
                    Y = 100
                });
            }
            fruit = new Food();
            foodIndexes = new List<int>();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Segment head = Python[0];
            #region Движение змейки
            //for (int i = Python.Count - 1; i >= 1; i--)
            //{
            //    Segment seg = Python[i];
            //    Segment seg_1 = Python[i - 1];
            //    seg.Y = seg_1.Y;
            //    seg.X = seg_1.X;
            //}
            //int foodIndex = -1;
            //коррекция - еда "мигрирует" в хвост
            #region С одной едой
            /*
            int foodIndex = -1;
            for (int i = Python.Count - 1; i >= 1; i--)
            {
                //проходим все элементы, проверяем на еду
                if (Python[i] as Food != null)
                {
                    foodIndex = i;
                }
            }
            if (foodIndex != -1)
            {
                //foodInside
                Segment food = Python[foodIndex];
                if (foodIndex == Python.Count - 1)
                {//еда в самом хвосте - меняем ее на сегмент
                    Python[foodIndex] = new Segment()
                    {
                        X = food.X,
                        Y = food.Y,
                        Figure = new Ellipse
                        {
                            Width = food.Figure.Width,
                            Height = food.Figure.Height,
                            Fill = (food.Figure as Rectangle).Fill
                        }
                    };

                }
                else
                {//еда в середине - сдвигаем ее на 1 позицию
                    Python.Remove(food);
                    Python.Insert(foodIndex + 1, food);
                }
                */
            #endregion
            foodIndexes.Clear();

            for (int i = 1; i < Python.Count; i++)
            {
                if ((Python[i] as Food) != null)
                {
                    foodIndexes.Add(i);
                }
            }
            foreach (int foodIndex in foodIndexes)
            {
                //foodInside
                Segment food = Python[foodIndex];
                if (foodIndex == Python.Count - 1)
                {//еда в самом хвосте - меняем ее на сегмент
                    Python[foodIndex] = new Segment
                    {
                        X = food.X,
                        Y = food.Y,
                        Figure = new Ellipse
                        {
                            Width = food.Figure.Width,
                            Height = food.Figure.Height,
                            Fill = (food.Figure as Rectangle).Fill
                        }
                    };
                    //убираем с поля
                    Field.Children.Remove(food.Figure);
                }
                else
                {//еда в середине - сдвигаем ее на 1 позицию
                    Python.Remove(food);
                    Python.Insert(foodIndex + 1, food);
                }

            }

            for (int i = Python.Count - 1; i >= 1; i--)
            {
                Segment seg = Python[i];
                Segment seg_1 = Python[i - 1];
                seg.Y = seg_1.Y;
                seg.X = seg_1.X;
            }

            switch (moveDirection)
            {
                case MoveDirection.Left:    Python[0].X -= 20;
                    break;
                case MoveDirection.Right:   Python[0].X += 20;
                    break;
                case MoveDirection.Up:      Python[0].Y -= 20;
                    break;
                case MoveDirection.Down:    Python[0].Y += 20;
                    break;
            }
            //голова после шага попала в ячейку с едой:
            if (head.X == fruit.X && head.Y == fruit.Y)
            {
                //делаем еще один шаг в этом же направлении
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
                //вставляем еду в змейку
                Python.Insert(1, fruit); // 1 - индекс вставки
                //генерируем новую еду
                fruit = new Food();
                //проверяем чтобы еда не попала на змею
                bool collision;
                do
                {
                    collision = false;
                    fruit.X = random.Next(1, 40) * 20;
                    foreach (Segment seg in Python)
                    {
                        if (Math.Abs(seg.X - fruit.X) <= seg.Figure.Width && 
                            seg.Y - fruit.Y <= seg.Figure.Height)
                        {
                            collision = true;
                        }
                    }
                } while (collision);
                //отображаем ее
                fruit.Show(Field);
            }
            #endregion

            #region Выход за поле
            if (Python[0].X < 0 || Python[0].X >= Field.Width - Field.Width % head.Figure.Width || Python[0].Y <= Field.Height % head.Figure.Height || Python[0].Y >= Field.Height - Field.Height % head.Figure.Height)
            {
                MessageBox.Show("Игра окончена!");
                timer.Stop();
                Field.Children.Clear();
                return;
            }
            //if (moveDirection == MoveDirection.Right && Python[0].X >= Field.Width - Field.Width % head.Figure.Width) Python[0].X = 20;
            //if (moveDirection == MoveDirection.Up && Python[0].Y < 0) Python[0].Y = Field.Height - Field.Height % head.Figure.Height;
            //if (moveDirection == MoveDirection.Down && Python[0].Y >= Field.Height - Field.Height%head.Figure.Height) Python[0].Y = 20;
            #endregion
            ShowPython();
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowPython();
            moveDirection = MoveDirection.Left;
            fruit.Show(Field);
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

        public class Food : Segment
        {
            public Food()
            {

                Figure = new Rectangle
                {
                    Width = 20,
                    Height = 20,
                    Fill = new SolidColorBrush(
                        Color.FromRgb(
                            (byte)Snake.random.Next(100, 250),
                            (byte)Snake.random.Next(100, 250),
                            (byte)Snake.random.Next(100, 250)))
                };
                X = Snake.random.Next(1, 40) * 20;
                Y = Snake.random.Next(1, 20) * 20; ;
            }
        }

    }


}


/*
***** 0
*****0
****0*
***0**


0*****
******
    */