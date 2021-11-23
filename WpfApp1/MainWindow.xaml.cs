using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Product> products = new List<Product>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Read file
            /* Операции ввода/вывода, в случае неудачи,
             * выбрасывают исключения (throw exception)
             * Если исключение не ловится (catch), то
             * оно разрушает программу (ОС получает
             * "недопустимую операцию"). Для того чтобы
             * обработать исключения, потенциально опасные
             * операциии (в рез-те которых возможен выброс
             * исключения) берут в блок try-catch
             */
            String txt = "";
            try
            {  // блок с кодом, в котором возможны исключения
                using (var reader =
                     new StreamReader("dataE.txt"))
                {
                    txt = reader.ReadToEnd();
                }
            }
            catch (IOException ex)
            {   // блок выполняется, если будет исключение
                MessageBox.Show(ex.Message);
                txt = "* * *";
                return;
            }   // блоков catch может быть несколько для разных типов исключений
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                txt = "* * * *";
            }
            finally
            {  // блок выполнится в любом случае (даже если есть return)
                text1.Text = txt;
            }

        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            // Write file
            /* Файлы (открытие файлов) относятся к
               к неуправляемым ресурсам. Платформа
               не закрывает файлы автоматически.
               Наша программа должна позаботиться о
               закрытии файлов.
               Для обобщения работы с неупр. ресурсами
               существует блок using(){}
               То, что создается в этом блоке, разрушается
               (вызывается метод Dispose() ) после окончания
               блока
             */
            using (var writer =
                new StreamWriter("data.txt"))
            {
                writer.Write(text1.Text);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            text1.Text += " *";
        }

        /**
         * Добавить
         */
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            #region создать объект с данными из полей ввода
            Product product = null;
            try
            {
                product = new Product
                {
                    SN = Convert.ToInt32(SN.Text),
                    Name = Name.Text,
                    Price = Convert.ToDecimal(Price.Text),
                    Discount = Convert.ToDecimal(Discount.Text)
                };
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Неправильное число");
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Слишком большое число");
            }
            if (product == null) return;
            #endregion

            // добавляем объект в коллекцию
            products.Add(product);
            // выводим в текстовое поле
            text1.Text += "\n" + product.Name + " " + product.Price;

        }

        /**
         * Сохранить
         */
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            #region сериализуем коллекцию
            // using System.Xml.Serialization;

            // Создаем сериализатор. Ему нужет тип, с которым работает
            XmlSerializer serializer =
                new XmlSerializer(products.GetType());

            // создаем файл, в который будем сохранять результаты сериализации
            using(var writer = new StreamWriter("products.xml"))
            {
                // сериализуем объект product
                serializer.Serialize(writer, products);
                MessageBox.Show("Сохранено");
            }
            #endregion
        }

        /**
         * Считать
         */
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var reader = new FileStream("products.xml", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(
                        typeof(List<Product>)
                    );
                    products = (List<Product>)
                        serializer.Deserialize(reader);
                }
                ShowProducts();
            }
            catch
            {
                MessageBox.Show("Ошибка чтения файла");
            }
        }

        private void ShowProducts( 
            ProductComparers sorter = ProductComparers.BySN )
        {
            switch (sorter)
            {
                case ProductComparers.BySN:
                    products.Sort();
                    break;
                case ProductComparers.ByPrice:
                    products.Sort(new Product.PriceComparer());
                    break;
                case ProductComparers.ByName:
                    products.Sort(new Product.NameComparer());
                    break;
                case ProductComparers.ByDiscountAsc:
                    break;
                case ProductComparers.ByDiscountDesc:
                    break;
            }

            text1.Text = "";
            foreach(Product p in products)
            {
                text1.Text += p + "\n";
            }
        }
    }

    public class Product : IComparable<Product>
    {
        public Int32 SN { get; set; }
        public String Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        public int CompareTo(Product other)
        {
            // return SN.CompareTo(other.SN);
            if (SN < other.SN) return -1;
            if (SN > other.SN) return 1;
            return 0;
        }

        public override string ToString()
        {
            return String.Format("{0, -7} {1, -10} {2} (-{3}%)", SN, Name, Price, Discount);
        }

        public class PriceComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                return x.Price.CompareTo(y.Price);
            }
        }
        public class NameComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }
    }

    public enum ProductComparers
    {
        BySN,
        ByPrice,
        ByName,
        ByDiscountAsc,
        ByDiscountDesc
    }
}
/*  Сериализация
 *  Serial - последовательный
 *  Представление объектов в виде последовательности (строки)
 *  Используется для сохранения объектов либо их передачи по
 *  последовательному каналу
 */