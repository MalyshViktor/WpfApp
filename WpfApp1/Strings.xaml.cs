using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Strings.xaml
    /// </summary>
    public partial class Strings : Window
    {
        delegate String StringProcessor(String str);
        // typedef int Processor(int);

        StringProcessor reverser, shuffler ;
        StringProcessor[] processors = new StringProcessor[2];

        String wordsReverse(String str)
        {
            String[] words = str.Split(' ');
            String res = "";
            foreach (String word in words)
            {
                res = word + " " + res;
            }
            return res;
        }

        String wordsShuffler(String str)
        {
            String[] words = str.Split(' ');
            String res = "";
            // сколько слов? Готовим массив с числами 0,1,2...
            int N = words.Length;
            int[] nums = new int[N];
            for (int i = 0; i < N; i++) nums[i] = i;
            // перемешиваем массив - генерируем два случайных
            // индекса и меняем значения в массиве
            var random = new Random();
            int n1, n2;
            for (int i = 0; i < N; i++)
            {
                n1 = random.Next(N);
                do { n2 = random.Next(N); } while (n1 == n2);
                // поменять местами nums[n1] и nums[n2]
                int tmp = nums[n1];
                nums[n1] = nums[n2];
                nums[n2] = tmp;
            }

            for (int i = 0; i < N; i++) res += words[nums[i]] + " ";
            return res;
        }

        public Strings()
        {
            InitializeComponent();
            reverser = wordsReverse;
            shuffler = wordsShuffler;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            labelResult.Text = reverser(textBlock.Text);
        }

        private void Digits_Click(object sender, RoutedEventArgs e)
        {
            /* Регулярные выражения (Regular Expressions, Regex)
             * Специальные средства для работы со строками:
             * - проверка по шаблону / поиск шаблона
             * - разбиение по шаблону-разделителю
             * - поиск-замена
             * Особенности: 
             * - использование спец-символов
             *  \d (digit) - любая цифра
             *  \D (non-digit) - любая не-цифра
             *  \s (space)
             *  \S (non-space)
             *  \w (word-sym) - то, что может быть в слове
             *  \W (non-word)
             *  .  любой символ ; \. - таки "."
             * - квантификаторы (quantifiers - указатели количества)
             *  +   1 и более
             *  *   0 и более
             *  ?   0-1
             *  {3} ровно 3
             *  {3,5}  от 3 до 5
             * - символы-якоря
             *  ^ начало
             *  $ конец
             * - множества (один из)
             * [123] - 1 или 2 или 3
             * [1 2 3] - 1 или 2 или 3 или пробел (! аккуратно!)
             * [1,2,3] - 1 или 2 или 3 или запятая (! аккуратно!)
             * [0-9] диапазон
             * [a-z] [a-zA-Z] [0-9a-f] [а-пюя]
             * 
             * [^123] - любой символ, кроме 1,2,3
             */
            Regex digits = new Regex(@"^\s+[123]{3}\s+$");
            if (digits.IsMatch(textBlock2.Text))
            {
                result2.Text = "Yes";
            }
            else
            {
                result2.Text = "No";
            }
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            Regex digits = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (digits.IsMatch(textBlock2.Text))
            {
                result2.Text = "Yes";
            }
            else
            {
                result2.Text = "No";
            }
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            labelResult.Text = shuffler(textBlock.Text);
        }
    }
}
