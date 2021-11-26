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
    /// Interaction logic for DnD.xaml
    /// </summary>
    public partial class DnD : Window
    {
        private bool isDragged;
        private Point touchPoint;
        public DnD()
        {
            InitializeComponent();
            isDragged = false;
        }
        private void Mouse_Up(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    isDragged = false;
                    Field.ReleaseMouseCapture();//освобождение мыши
                    break;
            }
        }
        private void Mouse_Move(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Canvas.SetLeft(Subj, e.GetPosition(Field).X - touchPoint.X);
                Canvas.SetTop(Subj, e.GetPosition(Field).Y - touchPoint.Y);
                if (Canvas.GetLeft(Subj) + Subj.Width / 2 >= Canvas.GetLeft(Subj2))
                {
                    Canvas.SetLeft(Subj, e.GetPosition(Subj2).X);
                    Canvas.SetTop(Subj, e.GetPosition(Subj2).Y);
                    
                }
                else
                {
                    Field.ReleaseMouseCapture();
                }
            }

        }

        private void Subj_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    isDragged = true;
                    touchPoint = e.GetPosition(Subj);
                    Field.CaptureMouse(); //захват - события
                    //мыши будут попадать в это окно, даже если указатель из него выйдет
                    break;
                case MouseButton.Middle:
                    break;
                case MouseButton.Right:
                    break;
                case MouseButton.XButton1:
                    break;
                case MouseButton.XButton2:
                    break;
            }
        }


    }
}

/*
 * DnD (Drag n'Drop)
 * Технология визуального интерфейса, связывания с "перетаскиванием"
 * обьектов мышью
 * Для реализации необходимо:
 *  - по событию нажатия перейти в режим перетаскивания
 *   - по событию движения менять позицию обьекта
 *    - по событию отжатия - зафиксировать носую позицию
 *    
 *    Вариации:
 *    "фантомы" - копии обьектов, перетягивание вместо оригиналов
 *    " контейнеры" - если не попадаем в контейнер, перетягивание отменяется
 *    
 *    Особенности:
 *     - событие нажития получает сам обьект, тогда как движение и отжатие - все окно
 *     Иначе при резких движениях курсор уходит с обьекта и он теряет событие
 *      - для перетягивания за точку "взятия" необходимо запоминать
 *      координаты этой точки в событии нажатия, а корректировать
 *      в событии движения мыши
 *       - похожая на п.1 ситуация возможна с окном: при 
 *       выходе указателя мыши из окна "теряется" событие отжатия кнопки.
 *       Решается "захватом" указателя на время нажатия
 * */
