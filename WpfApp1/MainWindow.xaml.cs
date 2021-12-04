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
        {/*Read File
         Операции вода/вывода, в случае неудачи,
         выбрасывает исключения(throw exception)
         Если исключние не отлавливается (catch), то оно 
         разрушает программу(ОС получает недопустимую операцию)
         для того чтобы обработать исключения, потенциально опасные операции
         (в результате которых возможен выброс исключения) берут в блок try-catch
         */
            String txt = "";
            try
            {
                using (var reader = new StreamReader("data.txt"))
                {
                    txt = reader.ReadToEnd();
                }
            }
            catch (IOException ex)
            {//блок выполняется, если будет исключение
                MessageBox.Show(ex.Message);
                txt = "* * *";
            }//блоков catch может быть несколько для разных типов исключений
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
                txt = "* * * * ";
            }
            finally
            {//блок выполнится в любом случае(даже если есть return)
                text1.Text = txt;
            }
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {//Write file
         //Файлы(открытие файлов) относятся к неуправляемым ресурсам
         //Платформа не закрывает файлы автоматически/Наша программа должна позаботиться о закрытии файлов.
         //для обобщения работ с неупр. ресурсами существует блок using(){}
         //То, что создается в это блоке, разрушается(вызывается метод Dispose()) после окончания блока

            using (StreamWriter writer = new StreamWriter("data.txt", true))
            {
                writer.WriteLine(text1.Text);
            }
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {//Click
            text1.Text += "* ";
        }




        private void Button_Click_1(object sender, RoutedEventArgs e)
        {


            #region
            //Создать обьект с данными из полей ввода
            Product product = null;
            try
            {
                product = new Product
                {
                    SN = Convert.ToInt32(SN.Text),
                    Name = Name.Text,
                    Price = Convert.ToDecimal(Price.Text),
                    Discount = Convert.ToDecimal(Discount.Text),

                };
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Неправильное число!");
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Слишком большое число");
            }
            if (product == null) return;

            #endregion
            products.Add(product);
            text1.Text += "\n" + product.Name + " " + product.Price + "-" + product.Discount + "%";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {


            //using System.Xml.Serialization;
            //Создаем сериализацию, Емиу нужен тип, с которым работает
            XmlSerializer serializer = new XmlSerializer(products.GetType());
            //создаем файл, в который будем сохранять результаты сериализации
            using (var writer = new StreamWriter("products.xml"))
            {
                //сериализуем обьект
                serializer.Serialize(writer, products);
                MessageBox.Show("Сохранено!");
            }

        }
        /*
         * Считать
         * */
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader reader = new StreamReader("products.xml"))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Product>));
                    products = (List<Product>)serializer.Deserialize(reader);
                }
                ShowProducts();
            }
            catch
            {
                MessageBox.Show("Ошибка чтения файла!");
            }
        }

        private void ShowProducts(ProductComparers sorter = ProductComparers.BySN)
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
            //products.Sort(new Product.PriceComparer());
            text1.Text = "";
            foreach (Product p in products)
            {
                text1.Text += p + "\n";
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ShowProducts(ProductComparers.BySN);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ShowProducts(ProductComparers.ByName);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ShowProducts(ProductComparers.ByPrice);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            ShowProducts(ProductComparers.ByDiscountAsc);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            ShowProducts(ProductComparers.ByDiscountDesc);
        }
    }

    public class Product : IComparable<Product>
    {
        public int SN { get; set; }
        public String Name { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        public int CompareTo(Product other)
        {
            return Name.CompareTo(other.Name);
            //if (Price < other.Price) return -1;
            //if (Price > other.Price) return 1;

        }

        public override string ToString()
        {
            return String.Format("{0, -7} {1, -10} {2, -10} (-{3}%)", SN, Name, Price, Discount);
        }

        public class PriceComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                //return x.Price.CompareTo(y.Price);
                return x.Price.CompareTo(y.Price);
            }
        }
        public class NameComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                //return x.Price.CompareTo(y.Price);
                return x.Name.CompareTo(y.Name);
            }
        }
        public class SNComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                //return x.Price.CompareTo(y.Price);
                return x.SN.CompareTo(y.SN);
            }
        }
        public class DiscountComparer : IComparer<Product>
        {
            public int Compare(Product x, Product y)
            {
                //return x.Price.CompareTo(y.Price);
                return x.Discount.CompareTo(y.Discount);
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