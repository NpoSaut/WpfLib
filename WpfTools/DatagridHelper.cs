using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfTools
{
    public static class DatagridHelper
    {
        public static void InvokeFocusOnCurrentCell(this DataGrid dg)
        {
            dg.Dispatcher.Invoke((Action<DataGrid>)(g => g.FocusOnCurrentCell()), System.Windows.Threading.DispatcherPriority.Loaded, dg);
        }
        public static void BeginInvokeFocusOnCurrentCell(this DataGrid dg)
        {
            dg.Dispatcher.BeginInvoke((Action<DataGrid>)(g => g.FocusOnCurrentCell()), System.Windows.Threading.DispatcherPriority.Loaded, dg);
        }
        public static void FocusOnCurrentCell(this DataGrid dg)
        {
            if (dg.SelectedItem != null)
            {
                dg.ScrollIntoView(dg.SelectedItem);
                var c = dg.ItemContainerGenerator.ContainerFromItem(dg.SelectedItem) as DataGridRow;
                if (c != null)
                {
                    c.Focus();
                    c.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
            }
        }
    }
}
