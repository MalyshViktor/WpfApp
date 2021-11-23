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
        private bool mouseHold;

        public Tabs()
        {
            InitializeComponent();
            mouseHold = false;
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX.Text = e.GetPosition(this).X.ToString();
            mouseY.Text = e.GetPosition(this).Y.ToString();
            if (mouseHold)
            {
                var p = new Ellipse
                {
                    Fill = Brushes.LimeGreen,
                    Width = 5,
                    Height = 5
                };
                canvas.Children.Add(p);
                Canvas.SetLeft(p, e.GetPosition(canvas).X);
                Canvas.SetTop(p, e.GetPosition(canvas).Y);
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseHold = true;

            var p = new Ellipse {
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
            mouseHold = false;
            if(e.ChangedButton == MouseButton.Middle)
            {
                canvas.Children.Clear();
            }
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var r = new Rectangle {
                Fill = Brushes.Chocolate,
                Width = 15,
                Height = 15
            };
            canvas.Children.Add(r);
            Canvas.SetLeft(r, e.GetPosition(canvas).X);
            Canvas.SetTop(r, e.GetPosition(canvas).Y);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
               // variant1.Text += e.Key;
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

        private void StackPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // MessageBox.Show(e.Delta.ToString());
            if(e.Delta > 0)
            {
                mouseX.FontSize++;
            }
        }
    }
}
