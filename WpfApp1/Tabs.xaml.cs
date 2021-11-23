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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Tabs.xaml
    /// </summary>
    public partial class Tabs : Window
    {
        private bool MouseHold;

        public Tabs()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {

            MouseX.Text = e.GetPosition(this).X.ToString();
            MouseY.Text = e.GetPosition(this).Y.ToString();
            if (MouseHold)
            {
                var p = new Ellipse
                {
                    Fill = Brushes.RosyBrown,
                    Width = 5,
                    Height = 5
                };
                canvas.Children.Add(p);
                Canvas.SetLeft(p, e.GetPosition(canvas).X);
                Canvas.SetTop(p, e.GetPosition(canvas).Y);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseHold = true;
            var p = new Ellipse
            {
                Fill = (e.ChangedButton == MouseButton.Left)
                ? Brushes.Red
                : Brushes.Green,
                Width = 10,
                Height = 10
            };
            canvas.Children.Add(p);
            Canvas.SetLeft(p, e.GetPosition(canvas).X);
            Canvas.SetTop(p, e.GetPosition(canvas).Y);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseHold = false;
            if (e.ChangedButton == MouseButton.Middle)
            {
                canvas.Children.Clear();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var r = new Rectangle
            {
                Fill = Brushes.Chocolate,
                Width = 15,
                Height = 15
            };
            canvas.Children.Add(r);
        }


        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                variant1.Text += e.Key;
            }
            else e.Handled = true;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            variant1.Text += e.Key;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            variant2.Text = "Focused";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            variant2.Text = "";
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //e.Handled = true;

            if (e.Delta > 0)
            {
                ++MouseX.FontSize;
                ++MouseY.FontSize;
            }
            else
            {
                --MouseX.FontSize;
                --MouseY.FontSize;
            }

        }
    }
}
