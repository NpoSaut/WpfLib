using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Behaviors.AnimateProperty;

namespace Behaviors.TriggerHide
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
            //AssociatedObject.SetCurrentValue(UIElement.OpacityProperty, 0.0);
            AssociatedObject.Opacity = 0;
            _transform.ScaleY = 0;
        }

        protected override void Show()
        {
            //AssociatedObject.ClearValue(UIElement.OpacityProperty);
            AssociatedObject.Opacity = 1;
            _transform.ScaleY = 1;
        }

        protected override void OnAttached()
        {
            _transform = new ScaleTransform();
            AssociatedObject.LayoutTransform = _transform;

            Dispatcher.BeginInvoke((Action<Behavior>)Interaction.GetBehaviors(AssociatedObject).Add,
                                   new AnimatePropertyBehavior(UIElement.OpacityProperty)
                                   {
                                       AnimationDuration =
                                           new Duration(
                                           new TimeSpan((int)(AnimationDuration.TimeSpan.Ticks * 0.7)))
                                   });

            var scaleYAnimateBehavior = new AnimatePropertyBehavior(ScaleTransform.ScaleYProperty) { AnimationDuration = AnimationDuration };
            Dispatcher.BeginInvoke((Action<Behavior>)Interaction.GetBehaviors(_transform).Add, scaleYAnimateBehavior);

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LayoutTransform = null;
            base.OnDetaching();
        }
    }
}