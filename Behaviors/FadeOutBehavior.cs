using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Behaviors
{
    /// <summary>Выполняет эффект "уплывания" и "наплывания" объекта при выполнении условия его отображения</summary>
    public class FadeOutBehavior : TriggerHideBehavior<FrameworkElement>
    {
        public Double DeltaX { get; set; }
        public Double DeltaY { get; set; }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof (Duration), typeof (FadeOutBehavior),
                                        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(500))));

        public Duration AnimationDuration
        {
            get { return (Duration)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        protected IEasingFunction ShowEasingFunction
        {
            get { return new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 1, Springiness = 2.5 }; }
        }

        protected IEasingFunction HideEasingFunction
        {
            get { return new PowerEase(); }
        }

        private readonly TranslateTransform _transform = new TranslateTransform();

        protected override void Hide()
        {
            if (AssociatedObject != null)
                AssociatedObject.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
        }

        protected override void Show()
        {
            if (AssociatedObject != null)
                AssociatedObject.ClearValue(UIElement.VisibilityProperty);
        }

        protected override void AnimateHide()
        {
            var opacityAnimation = new DoubleAnimation(0, AnimationDuration);
            opacityAnimation.Completed += (Sender, Args) => Hide();
            var translateXAnimation = new DoubleAnimation(DeltaX, AnimationDuration) { EasingFunction = HideEasingFunction };
            var translateYAnimation = new DoubleAnimation(DeltaY, AnimationDuration) { EasingFunction = HideEasingFunction };

            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            _transform.BeginAnimation(TranslateTransform.XProperty, translateXAnimation);
            _transform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
        }

        protected override void AnimateShow()
        {
            Show();
            var opacityAnimation = new DoubleAnimation { From = 0, Duration = AnimationDuration };
            var translateXAnimation = new DoubleAnimation { From = DeltaX, Duration = AnimationDuration, EasingFunction = ShowEasingFunction };
            var translateYAnimation = new DoubleAnimation { From = DeltaY, Duration = AnimationDuration, EasingFunction = ShowEasingFunction };

            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            _transform.BeginAnimation(TranslateTransform.XProperty, translateXAnimation);
            _transform.BeginAnimation(TranslateTransform.YProperty, translateYAnimation);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.RenderTransform = _transform;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.RenderTransform = null;
        }
    }
}