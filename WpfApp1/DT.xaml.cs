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
            DateTime dateTime = DTpicker.SelectedDate.Value;
            DTtext.Text = "ToString " + dateTime.ToString()
                + "\nToBinary: " + dateTime.ToBinary()
                 + "\nToLocalTime: " + dateTime.ToLocalTime()
                 + "\nToLongDateString " + dateTime.ToLongDateString()
                 + "\nToLongTimeString " + dateTime.ToLongTimeString()
                 + "\nToShortDateString " + dateTime.ToShortDateString()
                 + "\nToShortTimeString " + dateTime.ToShortTimeString()
                 + "\nToUniversalTime " + dateTime.ToUniversalTime()
                 ;
            String iso8601 = String.Format(
                "{0}-{1}-{2} {3}:{4}:{5}.{6}",
                dateTime.Year,
                (dateTime.Month < 10 ? "0" : "") + dateTime.Month,
                (dateTime.Day < 10 ? "0" : "") + dateTime.Day,
                (dateTime.Hour < 10 ? "0" : "") + dateTime.Hour,
                (dateTime.Minute < 10 ? "0" : "") + dateTime.Minute,
                (dateTime.Second < 10 ? "0" : "") + dateTime.Second,
                dateTime.Millisecond
                );
            DTtext.Text += "\nISO-8601 " + iso8601;
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
