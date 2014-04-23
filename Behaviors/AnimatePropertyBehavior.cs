using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Behaviors
{
    /// <summary>Поведение, анимирующее изменение указанного свойства-зависимости</summary>
    public class AnimatePropertyBehavior : Behavior<UIElement>
    {
        /// <summary>Функции, запускающие анимацию для определённого типа свойства</summary>
        private static readonly Dictionary<Type, AnimationFactoryMethod> AnimationFactories =
            new Dictionary<Type, AnimationFactoryMethod>
            {
                { typeof (Double), AnimateDouble }
            };

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof (Duration), typeof (AnimatePropertyBehavior),
                                        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(500))));

        /// <summary>Предыдущее значение свойства</summary>
        private object _previousValue;

        /// <summary>Дескриптор свойства-зависимости</summary>
        private PropertyDescriptor _propertyDescriptor;

        public AnimatePropertyBehavior() { }

        public AnimatePropertyBehavior(DependencyProperty Property) : this() { this.Property = Property; }

        /// <summary>Продолжительность анимации</summary>
        public Duration AnimationDuration
        {
            get { return (Duration)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>Анимируемое свойство-зависимость</summary>
        public DependencyProperty Property { get; set; }

        /// <summary>Получает текущее, не анимированное значение свойства-зависимости</summary>
        /// <returns></returns>
        private object GetPropertyValue()
        {
            return AssociatedObject.GetAnimationBaseValue(Property);
        }

        protected override void OnAttached()
        {
            _propertyDescriptor = DependencyPropertyDescriptor.FromProperty(Property, AssociatedType);
            _propertyDescriptor.AddValueChanged(AssociatedObject, Handler);
            _previousValue = GetPropertyValue();

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            _propertyDescriptor.RemoveValueChanged(AssociatedObject, Handler);
            base.OnDetaching();
        }

        /// <summary>Обработчик изменения отслеживаемого свойства</summary>
        private void Handler(object Sender, EventArgs EventArgs)
        {
            if (Equals(_previousValue, GetPropertyValue())) return;
            if (AnimationFactories.ContainsKey(Property.PropertyType))
            {
                AnimationTimeline animation = AnimationFactories[Property.PropertyType](_previousValue,
                                                                                        GetPropertyValue(),
                                                                                        AnimationDuration);
                AssociatedObject.BeginAnimation(Property, animation);
            }
            _previousValue = GetPropertyValue();
        }

        /// <summary>Аниматор для Double-свойств</summary>
        private static AnimationTimeline AnimateDouble(object OldValue, object NewValue, Duration AnimationDuration)
        {
            return new DoubleAnimation((double)OldValue, (double)NewValue, AnimationDuration)
                   {
                       FillBehavior = FillBehavior.Stop
                   };
        }

        /// <summary>Метод, запускающий анимацию указанного свойства</summary>
        /// <param name="OldValue">Старое значение свойства (с которого начинается анимация)</param>
        /// <param name="NewValue">Новое значение свойства (которым должна закончиться анимация)</param>
        /// <param name="AnimationDuration">Продолжительность анимации</param>
        private delegate AnimationTimeline AnimationFactoryMethod(
            Object OldValue, Object NewValue, Duration AnimationDuration);
    }
}
