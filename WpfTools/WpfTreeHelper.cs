using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace WpfTools
{
    public static class WpfTreeHelper
    {
        /// <summary>
        /// Глубоко перечисляет всех визуальных потомков выбранного элемента
        /// </summary>
        /// <param name="root">Элемент для поиска</param>
        /// <param name="deep">Глубоко ли?</param>
        /// <returns>Послойное глубокое перечисление визуальных потомков</returns>
        public static IEnumerable<DependencyObject> EnumerateVisualChilds(this DependencyObject root, Boolean deep = true)
        {
            var childs = Enumerable.Range(0, VisualTreeHelper.GetChildrenCount(root)).Select(i => VisualTreeHelper.GetChild(root, i));
            if (!deep) return childs;
            else return childs.Concat(childs.SelectMany(c => EnumerateVisualChilds(c)));
        }

        /// <summary>
        /// Перечисляет всех визуальных предков выбранного элемента
        /// </summary>
        /// <param name="root">Элемент для поиска</param>
        /// <returns>Перечисление визуальных предков</returns>
        public static IEnumerable<DependencyObject> EnumerateVisualParents(this DependencyObject root)
        {
            var p = VisualTreeHelper.GetParent(root);
            if (p != null)
                return Enumerable.Repeat(p, 1)
                                 .Concat(p.EnumerateVisualParents());
            else return Enumerable.Empty<DependencyObject>();
        }

        /// <summary>
        /// Находит визуального потомка указанного типа
        /// </summary>
        /// <typeparam name="ChildType">Тип потомка для поиска</typeparam>
        /// <param name="Root">Элемент для поиска</param>
        /// <param name="DigLevel">Уровень поиска (какого по счёту потомка возвращать)</param>
        /// <returns>Визуальный потомок указанного типа или null</returns>
        public static ChildType FindVisualChild<ChildType>(this DependencyObject Root, int DigLevel = 1)
        {
            return Root.EnumerateVisualChilds().OfType<ChildType>().Skip(DigLevel - 1).FirstOrDefault();
        }
        /// <summary>
        /// Находит визуального предка указанного типа
        /// </summary>
        /// <typeparam name="ParentType">Тип предка для поиска</typeparam>
        /// <param name="Root">Элемент для поиска</param>
        /// <param name="DigLevel">Уровень поиска (какого по счёту потомка возвращать)</param>
        /// <returns>Визуальный предок указанного типа или null</returns>
        public static ParentType FindVisualParent<ParentType>(this DependencyObject Root, int DigLevel = 1)
        {
            return Root.EnumerateVisualParents().OfType<ParentType>().Skip(DigLevel - 1).FirstOrDefault();
        }
    }
}
