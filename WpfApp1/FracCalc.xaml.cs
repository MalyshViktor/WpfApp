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
            integer1.Text = "";
            numerator1.Text = "";
            denominator1.Text = "";
            integer2.Text = "";
            numerator2.Text = "";
            denominator2.Text = "";

        }
        private int getNod(int f1, int f2)
        {
            if (f2 == 0)
                return f1;
            return getNod(f2, f1 % f2);
        }

        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (integer1.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введите целую часть 1");
                return;
            }
            if (integer2.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введите целую часть 2");
                return;
            }

            if (numerator1.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введите числитель 1");
                return;
            }
            if (denominator1.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введите знаменатель 1");
                return;
            }
            if (numerator2.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введите числитель 2");
                return;
            }
            if (denominator2.Text.Equals(String.Empty))
            {
                MessageBox.Show("Введите знаменатель 2");
                return;
            }
            //Первая дробь
            Fraction frac1;
            //Проверка на правильность ввода данных
            try
            {
                frac1 = new Fraction
                {
                    Numerator = Convert.ToInt32(numerator1.Text),
                    Denominator = Convert.ToInt32(denominator1.Text),
                    Integer = Convert.ToInt32(integer1.Text)
                };
            }
            catch
            {
                MessageBox.Show("Неправильная запись дроби 1");
                return;
            }
            //Вторая дробь
            Fraction frac2;
            //Проверка на правильность ввода данных
            try
            {
                frac2 = new Fraction
                {
                    Numerator = Convert.ToInt32(numerator2.Text),
                    Denominator = Convert.ToInt32(denominator2.Text),
                    Integer = Convert.ToInt32(integer2.Text)
                };
            }
            catch
            {
                MessageBox.Show("Неправильная запись дроби 2");
                return;
            }

            //Умножение дробей, результат
            Fraction res = null;
            if (rbMul.IsChecked.Value)
            {
                res = frac1 * frac2;
            }
            //Деление дробей, результат
            if (rbDiv.IsChecked.Value)
            {
                res = frac1 / frac2;
            }

            //Сложение дробей, результат
            if (rbPlus.IsChecked.Value)
            {
                res = frac1 + frac2;
            }

            //Вычитание дробей, результат
            if (rbMinus.IsChecked.Value)
            {
                res = frac1 - frac2;

            }

            //Сокращение дробей
            //Находим наименьший общий делитель
            int NOD;
            int resNum = res.Numerator;
            int resDenom = res.Denominator;

            NOD = getNod(resNum, resDenom);

            res.Numerator /= NOD;
            res.Denominator /= NOD;

            while (res.Numerator >= res.Denominator)
            {
                res.Numerator -= res.Denominator;
                res.Integer++;
            }
            
            if (res == null)
            {
                //Не выбрана операция
                MessageBox.Show("Выберите операцию");
                return;
            }
            //Отображение

            if (res.Integer == 0 && res.Numerator == 0 && res.Denominator == 1)
            {
                integerRes.Content = res.Integer.ToString();
                res.Numerator = (int)Visibility.Hidden;
                res.Denominator = (int)Visibility.Hidden;
            }
            else if (res.Integer != 0 && res.Numerator == 0 && res.Denominator == 1)
            {
                integerRes.Content = res.Integer.ToString();
                res.Numerator = (int)Visibility.Hidden;
                res.Denominator = (int)Visibility.Hidden;
            }
            else
            {
            numeratorRes.Content = res.Numerator.ToString();
                denomeratorRes.Content = res.Denominator.ToString();
                integerRes.Content = res.Integer.ToString();
        }

        }
       
    }

    public class Fraction
    {
        public int Numerator { get; set; }
        public int Denominator { get; set; }
        public int Integer { get; set; }

        //Перегрузка операторов
        //Отличия от С++: 
        //Описываются как методы, но принимают два параметра
        //задаются для класса, т.е. в статическом виде
        public static Fraction operator *(Fraction f1, Fraction f2)
        {
            return new Fraction
            {
                Numerator = (f1.Numerator + f1.Denominator * f1.Integer) * (f2.Numerator + f2.Denominator * f2.Integer),
                Denominator = f1.Denominator * f2.Denominator,
            };
        }
        public static Fraction operator /(Fraction f1, Fraction f2)
        {
            return new Fraction
            {
                Numerator = (f1.Numerator + f1.Denominator * f1.Integer) * f2.Denominator,
                Denominator = f1.Denominator * (f2.Numerator + f2.Denominator * f2.Integer),

            };
        }

        public static Fraction operator +(Fraction f1, Fraction f2)
        {
            return new Fraction
            {
                Numerator = f1.Numerator * f2.Denominator + f1.Denominator * f2.Numerator,
                Denominator = f1.Denominator * f2.Denominator,
                Integer = f1.Integer + f2.Integer
            };
        }

        public static Fraction operator -(Fraction f1, Fraction f2)
        {
            return new Fraction
            {
                Numerator = f1.Numerator * f2.Denominator - f2.Numerator * f1.Denominator,
                Denominator = f1.Denominator * f2.Denominator,
                Integer = f1.Integer - f2.Integer
            };
        }
    }
}
