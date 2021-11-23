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
using System.Reflection;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Arkanoid.xaml
    /// </summary>
    public partial class Arkanoid : Window
    {
        DispatcherTimer Timer;
        private HoldKey holdkey;
        private List<Rectangle> Bricks;
        private List<Ellipse> Balls;
        public Arkanoid()
        {
            InitializeComponent();
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            Timer.Tick += Timer_Tick;
            Timer.Tick += Timer_Tick1;
            Timer.Tick += Timer_Tick_Bricks;
            Bricks = new List<Rectangle>();
            Balls = new List<Ellipse>();
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            #region Движение каретки
            double dx = 0;
            if (holdkey == HoldKey.Left && Canvas.GetLeft(Ship) > 0) dx = -10;
            if (holdkey == HoldKey.Right && Canvas.GetLeft(Ship) < Field.Width - Ship.Width) dx = 10;
            if (dx != 0)
            {
                Canvas.SetLeft(Ship, Canvas.GetLeft(Ship) + dx);
            }
            #endregion

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            List<Ellipse> toRemove = new List<Ellipse>();
            foreach (var Ball in Balls)
            {
                #region Движение шарика                             
                BallData data = (BallData)Ball.Tag; //Unboxing data - это копия
                double ballX = Canvas.GetLeft(Ball);
                double ballY = Canvas.GetTop(Ball);

                if (ballY > Field.Height - Ball.Height)//ниже края поля - проигрыш
                {
                    //Timer.Stop();
                    //MessageBox.Show("Game Over!", "Looser",
                    //    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    toRemove.Add(Ball);
                }
                if (ballY + Ball.Height >= Canvas.GetTop(Ship)) //нижняя область отражения
                {
                    double shipX = Canvas.GetLeft(Ship);

                    //проверяем по горизонтали - или шарик над кареткой
                    if (ballX >= shipX - Ball.Width / 2//левый край 
                        && ballX <= shipX - Ball.Width / 2 + Ship.Width)//правый край
                    {
                        //ballY = Field.Height - Ball.Height;
                        data.Vy = -Math.Abs(data.Vy); //изменение копии - не меняет Tag

                    }
                }
                if (ballX >= Field.Width - Ball.Width)
                {
                    ballX = Field.Width - Ball.Width;
                    data.Vx = -data.Vx;

                }
                if (ballY <= 0)
                {
                    ballY = 0;
                    data.Vy = -data.Vy;
                }
                if (ballX <= 0)
                {
                    ballX = 0;
                    data.Vx = -data.Vx;
                }

                Canvas.SetTop(Ball, ballY + data.Vy);
                Canvas.SetLeft(Ball, ballX + data.Vx);
                //Ball.Tag = data; // новая упаковка - новый объект
                #endregion Движение шарика
            }
            foreach (var rem in toRemove)
            {
                Balls.Remove(rem);
                Field.Children.Remove(rem);
            }
            if (Balls.Count == 0)
            {
                Timer.Stop();
                MessageBox.Show("Game Over!", "Looser",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Timer_Tick_Bricks(object sender, EventArgs e)
        {
            List<Rectangle> toRemove = new List<Rectangle>();
            List<Ellipse> toDuplicate = new List<Ellipse>();

            foreach (var Ball in Balls)
            {
                BallData data = (BallData)Ball.Tag;
                double ballX = Canvas.GetLeft(Ball);
                double ballY = Canvas.GetTop(Ball);


                foreach (var Brick in Bricks)
                {
                    bool strike = false;
                    double brickX = Canvas.GetLeft(Brick);
                    double brickY = Canvas.GetTop(Brick);
                    //сверху
                    if (ballY + Ball.Height >= brickY
                        && ballY + Ball.Height <= brickY + Math.Abs(2 * data.Vy)
                          && ballX >= brickX - Ball.Width / 2//левый край
                            && ballX <= brickX - Ball.Width / 2 + Brick.Width)
                    {
                        data.Vy = -Math.Abs(data.Vy);
                        //toRemove.Add(Brick);
                        strike = true;
                    }
                    //снизу
                    else if (ballY <= brickY + Brick.Height
                         && ballY >= brickY + Ball.Height + Math.Abs(2 * data.Vy)
                            && ballX >= brickX - Ball.Width / 2
                              && ballX <= brickX - Ball.Width / 2 + Brick.Width)
                    {
                        data.Vy = Math.Abs(data.Vy);
                        //toRemove.Add(Brick);
                        strike = true;
                    }
                    //справа
                    else if (ballX <= brickX + Brick.Width
                        && ballX >= brickX + Ball.Width + Math.Abs(2 * data.Vx)
                          && ballY >= brickY - Ball.Height / 2//левый край 
                            && ballY <= brickY - Ball.Height / 2 + Brick.Height)
                    {
                        data.Vx = Math.Abs(data.Vx);
                        //toRemove.Add(Brick);
                        strike = true;
                    }
                    //слева
                    else if (ballX + Ball.Width >= brickX
                          && ballX + Ball.Width <= brickX + Math.Abs(2 * data.Vx)
                            && ballX >= brickY - Ball.Height / 2//левый край 
                             && ballY <= brickY - Ball.Height / 2 + Brick.Height)
                    {
                        data.Vx = -Math.Abs(data.Vx);
                        //toRemove.Add(Brick);
                        strike = true;


                    }
                    //точное попадание в угол
                    else if (Math.Abs(brickX + Brick.Width - ballX - Ball.Width / 2) < Ball.Width / 2
                        && Math.Abs(brickY + Brick.Height - ballY - Ball.Height / 2) < Ball.Height / 2

                        || Math.Abs(brickX - ballX - Ball.Width / 2) < Ball.Width / 2
                        && Math.Abs(brickY + Brick.Height - ballY - Ball.Height / 2) < Ball.Height / 2

                        || Math.Abs(brickX + Brick.Width - ballX - Ball.Width / 2) < Ball.Width / 2
                        && Math.Abs(brickY - ballY - Brick.Height / 2) < Ball.Height / 2

                        || Math.Abs(brickX - ballX - Ball.Width / 2) < Ball.Width / 2
                        && Math.Abs(brickY - ballY - Brick.Height / 2) < Ball.Height / 2)
                    {
                        data.Vx = -data.Vx;
                        data.Vy = -data.Vy;
                        //toRemove.Add(Brick);
                        strike = true;
                    }
                    if (strike)
                    {//кирпич на удаление, шарик на дубликат
                        if (!toRemove.Contains(Brick))
                            toRemove.Add(Brick);

                        if (!toDuplicate.Contains(Ball))
                            toDuplicate.Add(Ball);
                    }


                }
            }
            foreach (var rem in toRemove)
            {
                Bricks.Remove(rem);
                Field.Children.Remove(rem);
            }
            foreach (var dup in toDuplicate)
            {
                Ellipse newBall = new Ellipse
                {
                    Width = dup.Width,
                    Height = dup.Height,
                    Fill = new SolidColorBrush(
                        Color.FromRgb(150, 150, 250)),
                    Tag = new BallData
                    {
                        Vx = (dup.Tag as BallData).Vx,
                        Vy = -(dup.Tag as BallData).Vy
                    }
                };
                Balls.Add(newBall);
                Field.Children.Add(newBall);
                Canvas.SetLeft(newBall, Canvas.GetLeft(dup));
                Canvas.SetTop(newBall, Canvas.GetTop(dup));
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Продолжение конструктора - работа с элементами интерфейс
            Ball1.Tag = (object)new BallData
            {
                Vx = -5,
                Vy = 5

            };
            Balls.Add(Ball1);
            holdkey = HoldKey.None;
            for (int i = 0; i < 8; i++)
            {
                Rectangle rectangle = new Rectangle
                {
                    Width = 70,
                    Height = 20,
                    Fill = Brushes.DarkGoldenrod
                };
                Field.Children.Add(rectangle);
                Canvas.SetLeft(rectangle, 40 + 80 * i);
                Canvas.SetTop(rectangle, 20);
                Bricks.Add(rectangle);
            }
            Timer.Start();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                holdkey = HoldKey.Left;
            }
            if (e.Key == Key.Right)
            {
                holdkey = HoldKey.Right;
            }
            if (e.Key == Key.Escape)
            {
                if (Timer.IsEnabled) Timer.Stop();
                else Timer.Start();
            }
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && holdkey == HoldKey.Left)
            {
                holdkey = HoldKey.None;

            }
            if (e.Key == Key.Right && holdkey == HoldKey.Right)
            {
                holdkey = HoldKey.None;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Timer.Stop();
        }
    }

    class BallData  // информация о шарике
    {
        // public UIElement Image;
        public double Vx;
        public double Vy;
    }
}
// Задание: убирать кирпичи при попадании в них шарика

/*
 
        private void Timer_Tick_struct(object sender, EventArgs e)
        {
            #region Движение шарика
            BallData data = (BallData)Ball.Tag;  // Unboxing: data - это копия

            double ballX = Canvas.GetLeft(Ball);
            double ballY = Canvas.GetTop(Ball);

            if (ballY > Field.Height - Ball.Height)  // ниже края поля - проигрыш
            {
                Timer.Stop();
                MessageBox.Show("Game Over", "Looser",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            if (ballY + Ball.Height >= Canvas.GetTop(Ship))  // нижняя область отражения
            {
                double shipX = Canvas.GetLeft(Ship);
                // проверяем по горизонтали - или шарик над кареткой
                if (ballX >= shipX - Ball.Width / 2 // левый край каретки
                 && ballX <= shipX - Ball.Width / 2 + Ship.Width)  // правый край
                {
                    // ballY = Field.Height - Ball.Height;
                    data.Vy = -Math.Abs(data.Vy);  // изменение копии - не меняет Tag

                    // ((BallData)Ball.Tag).Vy *= -1;  // нельзя модифицировать результат распаковки
                    Ball.Tag             // Рефлексия - самоанализ
                        .GetType()       // Получаем данные о типе (Ball.Tag)
                        .GetField("Vy")  // Получаем данные о поле "Vy"
                        .SetValue(       // Меняем такое поле
                            Ball.Tag,    // в объекте Ball.Tag 
                            data.Vy);    // на значение data.Vy
                }
            }

            if (ballX >= Field.Width - Ball.Width)
            {
                ballX = Field.Width - Ball.Width;
                data.Vx = -data.Vx;
                Ball.Tag.GetType().GetField("Vx").SetValue(Ball.Tag, data.Vx);
            }
            if (ballX <= 0)
            {
                ballX = 0;
                data.Vx = -data.Vx;
                Ball.Tag.GetType().GetField("Vx").SetValue(Ball.Tag, data.Vx);
            }
            if (ballY <= 0)
            {
                ballY = 0;
                data.Vy = -data.Vy;
                Ball.Tag.GetType().GetField("Vy").SetValue(Ball.Tag, data.Vy);
            }

            Canvas.SetLeft(Ball, ballX + data.Vx);
            Canvas.SetTop(Ball, ballY + data.Vy);
            // Ball.Tag = data;  // новая упаковка - новый объект
            #endregion
        }

 */