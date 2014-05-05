using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Behaviors.DragDrop
{
    public abstract class DropTargetBehavior : Behavior<FrameworkElement>
    {
        #region Присоединение / Отсоединение

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.PreviewDrop += AssociatedObjectOnDrop;
            AssociatedObject.DragEnter += AssociatedObjectOnDragEnter;
            AssociatedObject.DragLeave += AssociatedObjectOnDragLeave;
            AssociatedObject.PreviewGiveFeedback += AssociatedObjectOnGiveFeedback;

            _leaveTimer = new Timer(100);
            _leaveTimer.Elapsed += LeaveTimerOnElapsed;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AllowDrop = false;
            AssociatedObject.Drop -= AssociatedObjectOnDrop;
        }

        #endregion

        #region Обработчики встроеных событий Drag/Drop

        private void AssociatedObjectOnDrop(object Sender, DragEventArgs e)
        {
            OnDiscardPreviewDrop(e);
            if (CanAcceptDrop(e)) OnDrop(e);
            else e.Effects = DragDropEffects.None;
        }
        private void AssociatedObjectOnGiveFeedback(object Sender, GiveFeedbackEventArgs e) { OnGiveFeedback(e); }

        private IEnumerable<DependencyObject> EnumerateVisualParents(DependencyObject element)
        {
            if (element == null) yield break;
            DependencyObject parent;
            do
            {
                parent = VisualTreeHelper.GetParent(element);
                yield return parent;
            } while (parent != null);
        }

        private void AssociatedObjectOnDragEnter(object Sender, DragEventArgs e)
        {
            _leaveTimer.Stop();
            OnPreviewDrop(e);
            OnDragEnter(e);
        }

        private void AssociatedObjectOnDragLeave(object Sender, DragEventArgs e)
        {
            _leaveEventArgs = e;
            _leaveTimer.Start();
        }

        private Timer _leaveTimer;
        private DragEventArgs _leaveEventArgs;
        private void LeaveTimerOnElapsed(object Sender, ElapsedEventArgs Args)
        {
            _leaveTimer.Stop();
            Dispatcher.BeginInvoke((Action<DragEventArgs>)OnDragDeferredLeave, _leaveEventArgs);
        }

        private void OnDragDeferredLeave(DragEventArgs e)
        {
            OnDiscardPreviewDrop(e);
            OnDragLeave(e);
        }

        #endregion

        #region Абстракции событий Drag/Drop

        protected virtual bool CanAcceptDrop(DragEventArgs DragEventArgs) { return true; }
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

        public static readonly DependencyProperty NegativeFeedbackStyleProperty =
            DependencyProperty.Register("NegativeFeedbackStyle", typeof (Style), typeof (DropTargetBehavior), new PropertyMetadata(default(Style)));

        public Style NegativeFeedbackStyle
        {
            get { return (Style)GetValue(NegativeFeedbackStyleProperty); }
            set { SetValue(NegativeFeedbackStyleProperty, value); }
        }

        protected virtual void OnPreviewDrop(DragEventArgs DragEventArgs)
        {
            var canAcceptDrop = CanAcceptDrop(DragEventArgs);
            var feedbackStyle = canAcceptDrop ? FeedbackStyle : (NegativeFeedbackStyle ?? FeedbackStyle);
            (FeedbackElement ?? AssociatedObject).SetCurrentValue(FrameworkElement.StyleProperty, feedbackStyle);
        }

        protected virtual void OnDiscardPreviewDrop(DragEventArgs DragEventArgs)
        {
            (FeedbackElement ?? AssociatedObject).ClearValue(FrameworkElement.StyleProperty);
        }
    }
}
