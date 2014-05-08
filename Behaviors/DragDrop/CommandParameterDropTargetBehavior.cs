using System.Diagnostics;
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

        public static readonly DependencyProperty DropPreviewerProperty =
            DependencyProperty.Register("DropPreviewer", typeof (IDropPreviewable), typeof (CommandParameterDropTargetBehavior), new PropertyMetadata(default(IDropPreviewable)));

        public IDropPreviewable DropPreviewer
        {
            get { return (IDropPreviewable)GetValue(DropPreviewerProperty); }
            set { SetValue(DropPreviewerProperty, value); }
        }

        protected override void OnDragEnter(DragEventArgs DragEventArgs)
        {
            base.OnDragEnter(DragEventArgs);
            var readyForDrop = CanAcceptDrop(DragEventArgs);
            DragEventArgs.Effects = readyForDrop ? GetDragDropEffects(DragEventArgs) : DragDropEffects.None;
        }

        protected override void OnDragOver(DragEventArgs DragEventArgs)
        {
            DragEventArgs.Effects = GetDragDropEffects(GetCommandParameter(DragEventArgs));
            base.OnDragOver(DragEventArgs);
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

        private bool _inDropPreview;
        protected override void OnPreviewDrop(DragEventArgs DragEventArgs)
        {
            base.OnPreviewDrop(DragEventArgs);
            if (DropPreviewer != null && CanAcceptDrop(DragEventArgs) && !_inDropPreview)
                DropPreviewer.PreviewDrop(GetCommandParameter(DragEventArgs));
            _inDropPreview = true;
        }

        protected override void OnDiscardPreviewDrop(DragEventArgs DragEventArgs)
        {
            base.OnDiscardPreviewDrop(DragEventArgs);
            if (DropPreviewer != null && _inDropPreview)
                DropPreviewer.DiscardPreviewDrop(GetCommandParameter(DragEventArgs));
            _inDropPreview = false;
        }
    }
}