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
            _readyForDrop = CanAcceptDrop(DragEventArgs);
            DragEventArgs.Effects = _readyForDrop ? GetDragDropEffects(DragEventArgs) : DragDropEffects.None;
        }

        protected override void OnDrop(DragEventArgs DragEventArgs)
        {
            var parameter = GetCommandParameter(DragEventArgs);
            if (DropCommand.CanExecute(parameter)) DropCommand.Execute(parameter);
        }

        protected override bool CanAcceptDrop(DragEventArgs DragEventArgs)
        {
            return DropCommand.CanExecute(GetCommandParameter(DragEventArgs));
        }

        protected abstract object GetCommandParameter(DragEventArgs DragEventArgs);
        protected abstract DragDropEffects GetDragDropEffects(object parameter);
    }
}