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
        //private bool isDragged;
        private Point initialPoint;
        private Point touchPoint;
        private FrameworkElement draggedObject;
        private FrameworkElement phantomObject;
        public DnD()
        {
            InitializeComponent();
            //isDragged = false;
            draggedObject = null;
            phantomObject = null;
        }
        private void Mouse_Up(object sender, MouseButtonEventArgs e)
        {
            if (draggedObject != null)
            {
                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        if (Canvas.GetLeft(draggedObject) < Canvas.GetLeft(Subj1) || Canvas.GetLeft(draggedObject) > Canvas.GetLeft(Subj1) + Subj.Width ||
                            Canvas.GetTop(draggedObject) < Canvas.GetTop(Subj1) || Canvas.GetTop(draggedObject) < Canvas.GetTop(Subj1) - Subj1.Height)
                        {//возвращаев в исходную позицию
                            Canvas.SetLeft(draggedObject, initialPoint.X);
                            Canvas.SetTop(draggedObject, initialPoint.Y);
                        }
                        else 
                        {
                            Canvas.SetLeft(draggedObject, Canvas.GetLeft(Subj1) + Subj1.Width/2- Subj.Width/2);
                            Canvas.SetTop(draggedObject, Canvas.GetTop(Subj1) + Subj1.Height/2 - Subj.Height/2);
                        }
                        //draggedObject = null;
                        //if (touchPoint.X > Canvas.GetLeft(Subj1) || touchPoint.X >= Canvas.GetLeft(Subj1) + Subj1.Width)
                        //{
                        //    Canvas.SetLeft(draggedObject, Canvas.GetLeft(Subj1) + Subj1.Width / 2);
                        //    Canvas.SetTop(draggedObject, Canvas.GetLeft(Subj1) + Subj1.Width / 2);
                        //    Field.ReleaseMouseCapture();//освобождение мыши
                        //}
                        //else
                        //{
                        //    Field.ReleaseMouseCapture();
                        //}
                        draggedObject = null;
                        Field.ReleaseMouseCapture();
                        break;
                }
            }
            //**********PHANTOM********************//
            if (phantomObject != null)
            {
                switch(e.ChangedButton)
                {
                    case MouseButton.Left:
                        if (Canvas.GetLeft(phantomObject) < Canvas.GetLeft(Subj1) || Canvas.GetLeft(phantomObject) > Canvas.GetLeft(Subj1) + Subj.Width ||
                            Canvas.GetTop(phantomObject) < Canvas.GetTop(Subj1) || Canvas.GetTop(phantomObject) < Canvas.GetTop(Subj1)-Subj1.Height)
                        {//возвращаев в исходную позицию
                            Canvas.SetLeft(phantomObject, 224);
                            Canvas.SetTop(phantomObject, 200);
                        }
                        else
                        {
                            Canvas.SetLeft(phantomObject, Canvas.GetLeft(Subj1) + Subj1.Width / 2 - Subj.Width / 2);
                            Canvas.SetTop(phantomObject, Canvas.GetTop(Subj1) + Subj1.Height / 2 - Subj.Height / 2);
                        }
                        phantomObject = null;
                        Field.ReleaseMouseCapture();
                        break;
                }

                Field.Children.Remove(phantomObject);
                //phantomObject = null;
            }
         }
        private void Mouse_Move(object sender, MouseEventArgs e)
        {
            if (draggedObject != null)
            {
                Canvas.SetLeft(draggedObject, e.GetPosition(Field).X - touchPoint.X);
                Canvas.SetTop(draggedObject, e.GetPosition(Field).Y - touchPoint.Y);

            }
            //Для фантомного объекта
            if (phantomObject != null)
            {
                Canvas.SetLeft(phantomObject, e.GetPosition(Field).X - touchPoint.X);
                Canvas.SetTop(phantomObject, e.GetPosition(Field).Y - touchPoint.Y);

            }



        }
        private void Subj_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    draggedObject = sender as FrameworkElement;
                    if (draggedObject == null) return;

                    touchPoint = e.GetPosition(draggedObject);
                    initialPoint.X = Canvas.GetLeft(draggedObject);
                    initialPoint.Y = Canvas.GetTop(draggedObject);

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

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Вариация с фантомным объектом
            Ellipse proto = sender as Ellipse;
            phantomObject = new Ellipse
            {
                Width = proto.Width,
                Height = proto.Height,
                Stroke = Brushes.Tomato,
                //Fill = Brushes.Gray
            };
            Field.Children.Add(phantomObject);
            Canvas.SetLeft(phantomObject, Canvas.GetLeft(proto));
            Canvas.SetTop(phantomObject, Canvas.GetTop(proto));
            touchPoint = e.GetPosition(proto);
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
