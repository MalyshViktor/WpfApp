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
    /// Interaction logic for DT.xaml
    /// </summary>
    public partial class DT : Window
    {
        public DT()
        {
            InitializeComponent();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            DTtext.Text = DTpicker.SelectedDate.Value.ToString();
        }
    }
}

/* Дата / время
 * Есть множество форматов представления даты/ время
 *  - SQL
 *  - Email/Web
 *  - Internet
    + локализация (национальные стандарты)
    Натболее общее представление даты/времени в программировании - 
    TIMESTAMP - количество секунд или миллисекунд, прошедших с
    определенного момента (старт первой Unix - машины)
 */
