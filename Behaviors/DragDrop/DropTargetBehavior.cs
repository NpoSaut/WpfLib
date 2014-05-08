using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Behaviors.DragDrop
{
    public enum DropStateKind
    {
        Free,
        Preview,
        NegativePreview
    }

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
            AssociatedObject.DragOver += AssociatedObjectOnDragOver;

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

        private DragEventArgs _leaveEventArgs;
        private Timer _leaveTimer;

        private void AssociatedObjectOnDrop(object Sender, DragEventArgs e)
        {
            OnDiscardPreviewDrop(e);
            if (CanAcceptDrop(e)) OnDrop(e);
            else e.Effects = DragDropEffects.None;
        }

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

        private void AssociatedObjectOnDragOver(object Sender, DragEventArgs e)
        {
            if (!CanAcceptDrop(e)) e.Effects = DragDropEffects.None;
            else OnDragOver(e);
            e.Handled = true;
        }

        private void AssociatedObjectOnDragEnter(object Sender, DragEventArgs e)
        {
            _leaveTimer.Stop();
            CurrentDragEventArgs = e;
            OnPreviewDrop(e);
            OnDragEnter(e);
        }

        private void OnDragDeferredLeave(DragEventArgs e)
        {
            OnDiscardPreviewDrop(e);
            OnDragLeave(e);
            CurrentDragEventArgs = null;
        }

        private void AssociatedObjectOnDragLeave(object Sender, DragEventArgs e)
        {
            _leaveEventArgs = e;
            _leaveTimer.Start();
        }

        private void LeaveTimerOnElapsed(object Sender, ElapsedEventArgs Args)
        {
            _leaveTimer.Stop();
            Dispatcher.BeginInvoke((Action<DragEventArgs>)OnDragDeferredLeave, _leaveEventArgs);
        }

        #endregion

        #region Абстракции событий Drag/Drop

        protected virtual bool CanAcceptDrop(DragEventArgs DragEventArgs) { return true; }
        protected abstract void OnDrop(DragEventArgs DragEventArgs);
        protected virtual void OnDragEnter(DragEventArgs DragEventArgs) { }
        protected virtual void OnDragLeave(DragEventArgs DragEventArgs) { }
        protected virtual void OnDragOver(DragEventArgs DragEventArgs) { }

        #endregion

        #region Присоединённое свойство DropState

        public static readonly DependencyProperty DropStateProperty =
            DependencyProperty.RegisterAttached("DropState", typeof (DropStateKind), typeof (DropTargetBehavior),
                                                new PropertyMetadata(DropStateKind.Free));

        public static void SetDropState(DependencyObject element, DropStateKind value) { element.SetValue(DropStateProperty, value); }

        public static DropStateKind GetDropState(DependencyObject element) { return (DropStateKind)element.GetValue(DropStateProperty); }

        #endregion

        protected DragEventArgs CurrentDragEventArgs { get; private set; }

        protected virtual void OnPreviewDrop(DragEventArgs DragEventArgs)
        {
            bool canAcceptDrop = CanAcceptDrop(DragEventArgs);
            SetDropState(AssociatedObject, canAcceptDrop ? DropStateKind.Preview : DropStateKind.NegativePreview);
        }

        protected virtual void OnDiscardPreviewDrop(DragEventArgs DragEventArgs) { SetDropState(AssociatedObject, DropStateKind.Free); }
    }
}
