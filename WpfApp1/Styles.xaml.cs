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
    /// Interaction logic for Styles.xaml
    /// </summary>
    public partial class Styles : Window
    {
        public Styles()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            uniGrid.Children.Add(
                new Label
                {
                    Content = "Label 9"
                }
            );
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            uniGrid.Children.Add(
                new Label
                {
                    Content = "Label Big",
                    Style = FindResource("BigCentered") as Style
                }
            );
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            uniGrid.Children.Add(
                new Label
                 {
                   Content = "Mixed",
                    Style = FindResource("New") as Style
                 }
            );
        }
    }
}
