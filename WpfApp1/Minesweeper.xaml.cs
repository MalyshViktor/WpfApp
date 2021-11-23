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
            {
                for (int x = 0; x < App.SizeX; x++)
                {
                    //bool isMine = random.Next(5) ==0;
                    var mineLabel = new MineLabel
                    {
                        X = x,
                        Y = y,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    mineLabel.MouseDown += MineLabel_MouseDown;
                    Field.Children.Add(mineLabel);

                }
        }
            Restart();
        }

        private void Restart()
        {
            foreach (var child in Field.Children)
            {
                MineLabel label = child as MineLabel;
                if (label != null)
                {
                    bool isMine = random.Next(7) == 0;
                    label.IsMine = isMine;
                    label.Content = isMine ? "\x2622" : "\x224b";
                    label.labelState = LabelState.Unvisited;
                    label.Foreground = Brushes.Black;
                    label.Background = Brushes.Khaki;
                }

            }
        }


        private void MineLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mineLabel = sender as MineLabel;
            if (e.RightButton == MouseButtonState.Pressed && mineLabel.labelState != LabelState.Open)
            {
                mineLabel.labelState = LabelState.Marked;
                mineLabel.Content = "@";
                mineLabel.Foreground = Brushes.Black;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
            if (mineLabel == null) return;
            if (mineLabel.IsMine)
            {
                        MessageBoxButton.YesNo))
                {
                    return;
                }
                else
                {
                    this.Close();
                }
            }
                "label_" + (x-1) + "_" + (y), 
                "label_" + (x-1) + "_" + (y+1), 
                "label_" + (x+1) + "_" + (y-1), 
            int mines = 0;
            {
                MineLabel label = this.FindName(name) as MineLabel;
                {
                    {
                    }
                }
            }

            mineLabel.Content = mines.ToString();
            }


            // mineLabel.Foreground = Brush. mines;
            // MessageBox.Show("Click " + mineLabel.X + " " + mineLabel.Y);
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
