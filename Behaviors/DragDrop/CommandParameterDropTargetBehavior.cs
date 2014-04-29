using System.Windows;
using System.Windows.Input;

namespace Behaviors.DragDrop
{
    public abstract class CommandParameterDropTargetBehavior : DropTargetBehavior
    {
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register("DropCommand", typeof(ICommand), typeof(DropTargetBehavior),
                                        new PropertyMetadata(default(ICommand)));

        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        private bool _readyForDrop = false;

        protected override void OnDragEnter(DragEventArgs DragEventArgs)
        {
            _readyForDrop = DropCommand.CanExecute(GetCommandParameter(DragEventArgs));
            if (_readyForDrop)
            {
                DragEventArgs.Effects = GetDragDropEffects(DragEventArgs);
            }
            else
            {
                DragEventArgs.Effects = DragDropEffects.None;
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs FeedbackEventArgs)
        {
            /*if (_readyForDrop)
            {
                FeedbackEventArgs.Effects = GetDragDropEffects(FeedbackEventArgs);
            }
            else
            {
                FeedbackEventArgs.Effects = DragDropEffects.None;
            }*/
        }



        protected override void OnDrop(DragEventArgs DragEventArgs)
        {
            var parameter = GetCommandParameter(DragEventArgs);
            if (DropCommand.CanExecute(parameter)) DropCommand.Execute(parameter);
        }

        protected abstract object GetCommandParameter(DragEventArgs DragEventArgs);
        protected abstract DragDropEffects GetDragDropEffects(object parameter);
    }
}