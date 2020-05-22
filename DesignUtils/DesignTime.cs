using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesignUtils
{
    public static class DesignTime
    {
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
            "Background", typeof(Brush), typeof(DesignTime),
            new PropertyMetadata(default(Brush), SetBackgroundCallback));

        private static bool? _inDesignMode;

        private static bool InDesignMode
        {
            get
            {
                if (_inDesignMode == null)
                {
                    DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
                    _inDesignMode = (bool) DependencyPropertyDescriptor
                                          .FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;

                    if (!_inDesignMode.GetValueOrDefault(false) && Process.GetCurrentProcess()
                                                                          .ProcessName
                                                                          .StartsWith(
                                                                               "devenv", StringComparison.Ordinal))
                        _inDesignMode = true;
                }

                return _inDesignMode.GetValueOrDefault(false);
            }
        }

        private static void SetBackgroundCallback(
            DependencyObject O, DependencyPropertyChangedEventArgs PropertyChangedEventArgs)
        {
            if (!InDesignMode) return;
            O.SetValue(Control.BackgroundProperty, PropertyChangedEventArgs.NewValue);
        }

        public static void SetBackground(DependencyObject element, Brush value)
        {
            element.SetValue(BackgroundProperty, value);
        }

        public static Brush GetBackground(DependencyObject element)
        {
            return (Brush) element.GetValue(BackgroundProperty);
        }
    }
}