using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Behaviors.AnimateProperty;

namespace Behaviors.TriggerHide
{
    /// <summary>Разворачивает элемент при заданном условии</summary>
    public class CollapseBehavior : TriggerHideBehavior<FrameworkElement>
    {
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof (Duration), typeof (CollapseBehavior),
                                        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(300))));

        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof (IEasingFunction), typeof (CollapseBehavior),
                                        new PropertyMetadata(default(IEasingFunction)));

        private ScaleTransform _transform;

        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }

        /// <summary>Продолжительность анимации</summary>
        public Duration AnimationDuration
        {
            get { return (Duration)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

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

            // Прозрачность
            Dispatcher.BeginInvoke((Action<Behavior>)Interaction.GetBehaviors(AssociatedObject).Add,
                                   new AnimatePropertyBehavior(UIElement.OpacityProperty)
                                   {
                                       AnimationDuration =
                                           new Duration(new TimeSpan((int)(AnimationDuration.TimeSpan.Ticks * 0.7)))
                                   });

            // Сжатие
            Dispatcher.BeginInvoke((Action<Behavior>)Interaction.GetBehaviors(_transform).Add,
                                   new AnimatePropertyBehavior(ScaleTransform.ScaleYProperty)
                                   {
                                       AnimationDuration = AnimationDuration,
                                       EasingFunction = EasingFunction
                                   });

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LayoutTransform = null;
            base.OnDetaching();
        }
    }
}
