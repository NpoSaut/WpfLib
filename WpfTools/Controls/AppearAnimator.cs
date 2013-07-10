using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfTools.Controls
{
    public static class AppearAnimator
    {
        #region Свойства анимации
        
        public static int GetAppearenceOrder(DependencyObject obj)
        {
            return (int)obj.GetValue(AppearenceOrderProperty);
        }
        public static void SetAppearenceOrder(DependencyObject obj, int value)
        {
            obj.SetValue(AppearenceOrderProperty, value);
        }
        public static readonly DependencyProperty AppearenceOrderProperty = DependencyProperty.RegisterAttached("AppearenceOrder", typeof(int), typeof(AppearAnimator), new UIPropertyMetadata(0));



        public static AlignmentX GetSlideDirectionX(DependencyObject obj)
        {
            return (AlignmentX)obj.GetValue(SlideDirectionXProperty);
        }
        public static void SetSlideDirectionX(DependencyObject obj, AlignmentX value)
        {
            obj.SetValue(SlideDirectionXProperty, value);
        }
        public static readonly DependencyProperty SlideDirectionXProperty = DependencyProperty.RegisterAttached("SlideDirectionX", typeof(AlignmentX), typeof(AppearAnimator), new UIPropertyMetadata(AlignmentX.Left));

        

        #endregion


        private static Double GetSignedShiftForDirection(AlignmentX Direction, Double ShiftLength)
        {
            switch (Direction)
            {
                case AlignmentX.Left: return +ShiftLength;
                case AlignmentX.Right: return -ShiftLength;
                default: return 0;
            }
        }
        private static Double GetSignedShiftForDirection(AlignmentY Direction, Double ShiftLength)
        {
            switch (Direction)
            {
                case AlignmentY.Top: return +ShiftLength;
                case AlignmentY.Bottom: return -ShiftLength;
                default: return 0;
            }
        }

        public static void Animate(FrameworkElement el)
        {
            int order = GetAppearenceOrder(el);
            AlignmentX DirectionX = GetSlideDirectionX(el);
            double SlideLength = 15;
            TimeSpan SlideDuration = TimeSpan.FromMilliseconds(600);
            TimeSpan OpacityDuration = TimeSpan.FromMilliseconds(250);
            TimeSpan OrderDelay = TimeSpan.FromMilliseconds(100);

            if (!(el.RenderTransform is TranslateTransform)) el.RenderTransform = new TranslateTransform();
            var rt = el.RenderTransform as TranslateTransform;

            Storyboard sb = new Storyboard();
            
            var ao0 = new DoubleAnimation()
                {
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(0)
                };
            Storyboard.SetTarget(ao0, el);
            Storyboard.SetTargetProperty(ao0, new PropertyPath(FrameworkElement.OpacityProperty));
            sb.Children.Add(ao0);

            var ao1 = new DoubleAnimation()
                {
                    BeginTime = TimeSpan.FromMilliseconds(order * OrderDelay.TotalMilliseconds),
                    Duration = OpacityDuration
                };
            Storyboard.SetTarget(ao1, el);
            Storyboard.SetTargetProperty(ao1, new PropertyPath(FrameworkElement.OpacityProperty));
            sb.Children.Add(ao1);

            if (DirectionX != AlignmentX.Center)
            {
                var at = new DoubleAnimation()
                    {
                        From = GetSignedShiftForDirection(DirectionX, SlideLength),
                        Duration = SlideDuration,
                        BeginTime = TimeSpan.FromMilliseconds(order * OrderDelay.TotalMilliseconds),
                        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 3 }
                    };
                Storyboard.SetTarget(at, el);
                Storyboard.SetTargetProperty(at, new PropertyPath("RenderTransform.X"));
                sb.Children.Add(at);
            }

            sb.Begin();

        }
    }
}
