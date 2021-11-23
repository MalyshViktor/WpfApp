using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Interaction logic for Minesweeper.xaml
    /// </summary>
    public partial class Minesweeper : Window
    {
        private Random random;

        public Minesweeper()
        {
            InitializeComponent();
            random = new Random();
            // SystemSounds.Exclamation.Play();
            for (int y = 0; y < App.SizeY; y++)
                for (int x = 0; x < App.SizeX; x++)
                {
                    var mineLabel = new MineLabel
                    {
                        X = x,
                        Y = y,
                        FontSize = 30,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    mineLabel.MouseDown += MineLabel_MouseDown;
                    this.RegisterName(
                        "label_" + x + "_" + y, 
                        mineLabel);
                    Field.Children.Add(mineLabel);
                }
            RestartGame();
        }

        private void MineLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mineLabel = sender as MineLabel;
            if (mineLabel == null) return;
            // Если клик на мину - The end
            if (mineLabel.IsMine)
            {
                if(MessageBoxResult.Yes ==
                    MessageBox.Show(
                        "Еще раз?", 
                        "Игра провалена",
                        MessageBoxButton.YesNo))
                {
                    //MessageBox.Show("Это в платной версии");
                    RestartGame();
                    return;
                }
                else
                {
                    this.Close();
                }
            }

            // Определяем соседей:
            int x = mineLabel.X, y = mineLabel.Y;
            // Массив предполагаемых имен:
            String[] names = { 
                "label_" + (x-1) + "_" + (y-1), 
                "label_" + (x-1) + "_" + (y), 
                "label_" + (x-1) + "_" + (y+1), 
                "label_" + (x)   + "_" + (y-1), 
                "label_" + (x)   + "_" + (y+1), 
                "label_" + (x+1) + "_" + (y-1), 
                "label_" + (x+1) + "_" + (y),
                "label_" + (x+1) + "_" + (y+1) };
            // Счетчик мин
            int mines = 0;
            // Цикл по именам
            foreach(String name in names)
            {
                // Ищем по имени (ccылку на Label):
                MineLabel label = this.FindName(name) as MineLabel;
                if(label != null)  // такое имя найдено
                {
                    if (label.IsMine)  // Проверяем или это мина
                    {
                        mines++;  // увеличиваем счетчик мин
                    }
                }
            }
            mineLabel.Content = mines.ToString();

            // mineLabel.Foreground = Brush. mines;
            // MessageBox.Show("Click " + mineLabel.X + " " + mineLabel.Y);
        }

        private void RestartGame()
        {
            foreach(var child in Field.Children)
            {
                MineLabel label = child as MineLabel;
                if(label != null)
                {
                    bool isMine = random.Next(5) == 0;
                    label.IsMine = isMine;
                    label.Content = isMine ? "\x2622" : "\x224B";
                    label.labelState = LabelState.Unvisited;
                }
            }
        }
    }

    class MineLabel : Label
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsMine { get; set; }

        public LabelState labelState;
    }

    enum LabelState
    {
        Unvisited,
        Marked,
        Open
    }
}
