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
            //    DateTime dateTime = DTpicker.SelectedDate.Value;
            //    DTtext.Text = "ToString " + dateTime.ToString()
            //        + "\nToBinary: " + dateTime.ToBinary()
            //         + "\nToLocalTime: " + dateTime.ToLocalTime()
            //         + "\nToLongDateString " + dateTime.ToLongDateString()
            //         + "\nToLongTimeString " + dateTime.ToLongTimeString()
            //         + "\nToShortDateString " + dateTime.ToShortDateString()
            //         + "\nToShortTimeString " + dateTime.ToShortTimeString()
            //         + "\nToUniversalTime " + dateTime.ToUniversalTime()
            //
            // ;
            try
            {
                DTtext.Text = "ToString " + DTcalendar.SelectedDate.Value.ToString()
                    + "\nNow: " + DateTime.Now.ToString("dd mm yyyy hh mm ss zzz");
               
            }
            catch
            {
                DTtext.Text = "Select Date";
            }


        }

            private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DTpicker.SelectedDate.Value;
            String iso8601 = String.Format(
                 "{0}-{1}-{2} {3}:{4}:{5}",
                  dateTime.Year,
                 (dateTime.Month < 10 ? "0" : "") + dateTime.Month,
                 (dateTime.Day < 10 ? "0" : "") + dateTime.Day,
                 (dateTime.Hour < 10 ? "0" : "") + dateTime.Hour,
                 (dateTime.Minute < 10 ? "0" : "") + dateTime.Minute,
                 (dateTime.Second < 10 ? "0" : "") + dateTime.Second
                 );
            DTtext.Text += "\nISO-8601 " + iso8601;
        }

        private void NewButton1_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DTpicker.SelectedDate.Value;
            String rfc2822 = dateTime.ToString("R");
            DTtext.Text += "\nRFC-2822 " + rfc2822;
        }

        private void NewButton2_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DTpicker.SelectedDate.Value;
            String rfc3339 = //dateTime.ToString("o");
            String.Format(
            "{0}-{1}-{2}T{3}:{4}:{5} +{6}",
            dateTime.Year,
            dateTime.Month,
            (dateTime.Day < 10 ? "0" : "") + dateTime.Day,
            (dateTime.Hour < 10 ? "0" : "") + dateTime.Hour,
            (dateTime.Minute < 10 ? "0" : "") + dateTime.Minute,
            (dateTime.Second < 10 ? "0" : "") + dateTime.Second,
            dateTime.ToString("ffffK")
            );
            DTtext.Text += "\nRFC-3339 " + rfc3339;

        }

        private void parseDate_Click(object sender, RoutedEventArgs e)
        {
            String DTstr = tbDate.Text;
            try
            {
                var dt = DateTime.Parse(DTstr);
                DTtext.Text = dt.ToString();
            }
            catch 
            {
                DTtext.Text = "--";
            }
            try
            {
                DTtext.Text += "\n" + Convert.ToDateTime(DTstr).ToString();
            }
            catch
            {
                DTtext.Text = "\n--";
            }


        }
    }
    
}

/* Дата / время
 * Есть множество форматов представления даты/ время
 *  - SQL
 *  - Email/Web
 *  - Internet
    + локализация (национальные стандарты)
    Наиболее общее представление даты/времени в программировании - 
    TIMESTAMP - количество секунд или миллисекунд, прошедших с
    определенного момента (старт первой Unix - машины)
 */
