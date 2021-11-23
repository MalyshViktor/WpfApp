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
    /// Interaction logic for FracCalc.xaml
    /// </summary>
    public partial class FracCalc : Window
    {
        public FracCalc()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            numerator1.Text = "";
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (numerator1.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введите числитель 1");
                return;
            }
            // Первая дробь
            Fraction frac1;
            try
            {
                frac1 = new Fraction
                {
                    Numerator = Convert.ToInt32(numerator1.Text),
                    Denominator = Convert.ToInt32(denominator1.Text)
                };
            }
            catch
            {
                MessageBox.Show("Неправильная запись дроби 1");
                return;
            }
            // Вторая дробь
            Fraction frac2;
            try
            {
                frac2 = new Fraction
                {
                    Numerator = Convert.ToInt32(numerator2.Text),
                    Denominator = Convert.ToInt32(denominator2.Text)
                };
            }
            catch
            {
                MessageBox.Show("Неправильная запись дроби 2");
                return;
            }

            // Результат действия c дробями
            Fraction res = null;
            if(rbMul.IsChecked.Value) res = frac1 * frac2;
            
            if(res == null)
            {   // Не выбрана операция
                MessageBox.Show("Выберите операцию");
                return;
            }

            // Отображение
            numeratorRes.Content = res.Numerator.ToString();
            denominatorRes.Content = res.Denominator.ToString();
        }

       
    }

    public class Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        // Перегрузка операторов
        // Отличия от С++:
        //  описываются как методы, но принимают 2 параметра
        //  задаются для класса, то есть в статическом виде

        public static Fraction operator *(Fraction f1, Fraction f2)
        {
            return new Fraction
            {
                Numerator   = f1.Numerator   * f2.Numerator,
                Denominator = f1.Denominator * f2.Denominator
            };
        }
    }
}
