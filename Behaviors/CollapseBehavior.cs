using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Behaviors
{
    /// <summary>
    /// Разворачивает элемент при заданном условии
    /// </summary>
    public class CollapseBehavior : TriggerHideBehavior<FrameworkElement>
    {
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof (Duration), typeof (CollapseBehavior), new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(300))));

        /// <summary>
        /// Продолжительность анимации
        /// </summary>
        public Duration AnimationDuration
        {
            get { return (Duration)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        protected IEasingFunction ShowEasingFunction
        {
            get { return new ElasticEase { Oscillations = 1, Springiness = 1.5 }; }
        }

        protected IEasingFunction HideEasingFunction
        {
            get { return new PowerEase(); }
        }

        private ScaleTransform _transform;

        protected override void Hide()
        {
            AssociatedObject.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Collapsed);
        }

        protected override void Show()
        {
            AssociatedObject.ClearValue(UIElement.VisibilityProperty);
        }

        protected override void AnimateHide()
        {
            var opacityAnimation = new DoubleAnimation(1, 0, AnimationDuration);
            opacityAnimation.Completed += (Sender, Args) => Hide();
            var scaleAnimation = new DoubleAnimation(0, AnimationDuration) { EasingFunction = HideEasingFunction };

            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            _transform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
        }

        protected override void AnimateShow()
        {
            Show();
            var scaleAnimation = new DoubleAnimation(0, 1, AnimationDuration) { EasingFunction = ShowEasingFunction };
            var opacityAnimation = new DoubleAnimation { Duration = AnimationDuration};
            
            _transform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);
            AssociatedObject.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }

        protected override void OnAttached()
        {
            _transform = new ScaleTransform();

            AssociatedObject.LayoutTransform = _transform;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LayoutTransform = null;
            base.OnDetaching();
        }
    }
}