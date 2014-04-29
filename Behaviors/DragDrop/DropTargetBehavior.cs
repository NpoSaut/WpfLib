using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Behaviors.DragDrop
{
    public abstract class DropTargetBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AllowDrop = true;
            AssociatedObject.Drop += AssociatedObjectOnDrop;
            AssociatedObject.DragEnter += AssociatedObjectOnDragEnter;
            AssociatedObject.GiveFeedback += AssociatedObjectOnGiveFeedback;
        }

        private void AssociatedObjectOnGiveFeedback(object Sender, GiveFeedbackEventArgs FeedbackEventArgs) { OnGiveFeedback(FeedbackEventArgs); }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AllowDrop = false;
            AssociatedObject.Drop -= AssociatedObjectOnDrop;
        }

        private void AssociatedObjectOnDragEnter(object Sender, DragEventArgs e)
        {
            OnDragEnter(e);
        }

        private void AssociatedObjectOnDrop(object Sender, DragEventArgs e)
        {
            OnDrop(e);
        }

        protected abstract void OnDrop(DragEventArgs DragEventArgs);
        protected abstract void OnDragEnter(DragEventArgs DragEventArgs);
        protected abstract void OnGiveFeedback(GiveFeedbackEventArgs FeedbackEventArgs);
    }
}
