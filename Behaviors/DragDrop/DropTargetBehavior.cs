using System.Windows;
using System.Windows.Interactivity;

namespace Behaviors.DragDrop
{
    public abstract class DropTargetBehavior : Behavior<FrameworkElement>
    {
        #region Присоединение / Отсоединение

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.Drop += AssociatedObjectOnDrop;
            AssociatedObject.DragEnter += AssociatedObjectOnDragEnter;
            AssociatedObject.GiveFeedback += AssociatedObjectOnGiveFeedback;
            AssociatedObject.DragLeave += AssociatedObjectOnDragLeave;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AllowDrop = false;
            AssociatedObject.Drop -= AssociatedObjectOnDrop;
        }

        #endregion

        #region Обработчики встроеных событий Drag/Drop

        private void AssociatedObjectOnDrop(object Sender, DragEventArgs e) { OnDrop(e); }
        private void AssociatedObjectOnGiveFeedback(object Sender, GiveFeedbackEventArgs e) { OnGiveFeedback(e); }

        private void AssociatedObjectOnDragEnter(object Sender, DragEventArgs e)
        {
            (FeedbackElement ?? AssociatedObject).SetCurrentValue(FrameworkElement.StyleProperty, FeedbackStyle);
            OnDragEnter(e);
        }

        private void AssociatedObjectOnDragLeave(object Sender, DragEventArgs e)
        {
            (FeedbackElement ?? AssociatedObject).ClearValue(FrameworkElement.StyleProperty);
            OnDragLeave(e);
        }

        #endregion

        #region Абстракции событий Drag/Drop

        protected abstract void OnDrop(DragEventArgs DragEventArgs);
        protected virtual void OnDragEnter(DragEventArgs DragEventArgs) { }
        protected virtual void OnDragLeave(DragEventArgs DragEventArgs) { }
        protected virtual void OnGiveFeedback(GiveFeedbackEventArgs FeedbackEventArgs) { }

        #endregion

        public static readonly DependencyProperty FeedbackElementProperty =
            DependencyProperty.Register("FeedbackElement", typeof (FrameworkElement), typeof (DropTargetBehavior),
                                        new PropertyMetadata(default(FrameworkElement)));

        public static readonly DependencyProperty FeedbackStyleProperty =
            DependencyProperty.Register("FeedbackStyle", typeof (Style), typeof (DropTargetBehavior),
                                        new PropertyMetadata(default(Style)));

        /// <summary>
        ///     Элемент, к которому будут применяться эффекты при перетаскивании. По-умолчанию - элемент, ассоциированный с
        ///     поведением
        /// </summary>
        public FrameworkElement FeedbackElement
        {
            get { return (FrameworkElement)GetValue(FeedbackElementProperty); }
            set { SetValue(FeedbackElementProperty, value); }
        }

        /// <summary>Стиль, который будет применяться к элементу при перетаскивании над ним</summary>
        public Style FeedbackStyle
        {
            get { return (Style)GetValue(FeedbackStyleProperty); }
            set { SetValue(FeedbackStyleProperty, value); }
        }
    }
}
