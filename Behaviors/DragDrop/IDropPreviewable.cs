namespace Behaviors.DragDrop
{
    public interface IDropPreviewable
    {
        void PreviewDrop(object parameter);
        void DiscardPreviewDrop(object parameter);
    }
}