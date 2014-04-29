using System.Windows;

namespace Behaviors.DragDrop
{
    public class FileDropTargetBehavior : CommandParameterDropTargetBehavior
    {
        protected override object GetCommandParameter(DragEventArgs DragEventArgs)
        {
            return DragEventArgs.Data.GetData(DataFormats.FileDrop);
        }

        protected override DragDropEffects GetDragDropEffects(object Parameter) { return DragDropEffects.Copy; }
    }
}