using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Behaviors
{
    /// <summary>Тип условия отображения элемента</summary>
    public enum ShowingCondition
    {
        /// <summary>При равенстве значений</summary>
        OnEquals,

        /// <summary>При различии значений</summary>
        OnUnequal
    }

    /// <summary>Базовое поведение отображения элемента только при выполнении условия</summary>
    public abstract class TriggerHideBehavior<TElement> : Behavior<TElement>
        where TElement : UIElement
    {
        public static readonly DependencyProperty TriggerBindingProperty =
            DependencyProperty.Register("TriggerBinding", typeof (Object), typeof (TriggerHideBehavior<TElement>),
                                        new PropertyMetadata(default(Object), TriggerBindingPropertyChangedCallback));

        private static readonly DependencyPropertyKey IsShownKey =
            DependencyProperty.RegisterReadOnly("IsShown", typeof (Boolean), typeof (TriggerHideBehavior<TElement>),
                                                new PropertyMetadata(true, IsShownPropertyChangedCallback));

        // ReSharper disable once StaticFieldInGenericType
        public static readonly DependencyProperty IsShownProperty = IsShownKey.DependencyProperty;
        private bool _initialized;

        public TriggerHideBehavior()
        {
            TriggerValue = true;
            TriggerCondition = ShowingCondition.OnEquals;
        }

        /// <summary>Биндинг для проверки условия отображения</summary>
        public Object TriggerBinding
        {
            get { return GetValue(TriggerBindingProperty); }
            set { SetValue(TriggerBindingProperty, value); }
        }

        /// <summary>Значение для проверки условия отображения</summary>
        public Object TriggerValue { get; set; }

        /// <summary>Условие отображения элемента</summary>
        public ShowingCondition TriggerCondition { get; set; }

        /// <summary>Показывает, отображается ли элемент в данный момент</summary>
        public Boolean IsShown
        {
            get { return (Boolean)GetValue(IsShownProperty); }
            protected set { SetValue(IsShownKey, value); }
        }

        private static void TriggerBindingPropertyChangedCallback(DependencyObject o,
                                                                  DependencyPropertyChangedEventArgs e)
        {
            var behavior = (TriggerHideBehavior<TElement>)o;
            behavior.Refresh();
        }

        private static void IsShownPropertyChangedCallback(DependencyObject o,
                                                           DependencyPropertyChangedEventArgs e)
        {
            var behavior = (TriggerHideBehavior<TElement>)o;
            if (behavior._initialized)
            {
                if (Equals(e.NewValue, true)) behavior.AnimateShow();
                if (Equals(e.NewValue, false)) behavior.AnimateHide();
            }
            else
            {
                if (Equals(e.NewValue, true)) behavior.Show();
                if (Equals(e.NewValue, false)) behavior.Hide();
            }
        }

        private void Refresh()
        {
            IsShown = (Equals(TriggerBinding, TriggerValue)) ^ (TriggerCondition != ShowingCondition.OnEquals);
        }

        /// <summary>Показывает элемент</summary>
        protected abstract void Show();

        /// <summary>Скрывает элемент</summary>
        protected abstract void Hide();

        /// <summary>Начинает анимацию отображения элемента</summary>
        protected virtual void AnimateShow()
        {
            Show();
        }

        /// <summary>Начинает анимацию сокрытия элемента</summary>
        protected virtual void AnimateHide()
        {
            Hide();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            Refresh();
            _initialized = true;
        }
    }
}
