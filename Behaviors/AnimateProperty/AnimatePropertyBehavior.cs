using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace Behaviors.AnimateProperty
{
    /// <summary>Поведение, анимирующее изменение указанного свойства-зависимости</summary>
    public class AnimatePropertyBehavior : Behavior<DependencyObject>
    {
        /// <summary>Функции, запускающие анимацию для определённого типа свойства</summary>
        private static readonly Dictionary<Type, AnimationFactoryMethod> AnimationFactories =
            new Dictionary<Type, AnimationFactoryMethod>
            {
                { typeof (Double), GetDoubleAnimation }
            };

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof (Duration), typeof (AnimatePropertyBehavior),
                                        new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(1500))));

        /// <summary>Предыдущее значение свойства</summary>
        private object _previousValue;

        /// <summary>Дескриптор свойства-зависимости</summary>
        private PropertyDescriptor _propertyDescriptor;

        private AnimationClock lastAnimationClock;

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

        /// <summary>Событие возникает при завершении анимации изменения свойства</summary>
        public event EventHandler<PropertyAnimationCompleatedEventArgs> AnimationCompleated;

        /// <summary>Получает текущее, не анимированное значение свойства-зависимости</summary>
        /// <returns></returns>
        private object GetPropertyValue()
        {
            return ((IAnimatable)AssociatedObject).GetAnimationBaseValue(Property);
        }

        protected override void OnAttached()
        {
            if (!(AssociatedObject is IAnimatable))
            {
                throw new ArgumentException(
                    string.Format("Невозможно применить поведение {0} к элементу {1}, т.е. он не реализует интерфейс {2}",
                                  typeof (AnimatePropertyBehavior).Name,
                                  AssociatedObject,
                                  typeof (IAnimatable).Name));
            }

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
                object oldValue = _previousValue;
                object newValue = GetPropertyValue();

                if (lastAnimationClock != null)
                {
                    oldValue = lastAnimationClock.GetCurrentValue(oldValue, newValue);
                    Debug.Assert(lastAnimationClock.Controller != null, "lastAnimationClock.Controller != null");
                    lastAnimationClock.Completed -= AnimationOnCompleted;
                    lastAnimationClock.Controller.Remove();
                    lastAnimationClock = null;
                }

                AnimationTimeline animation = GetAnimation(oldValue, newValue);
                lastAnimationClock = animation.CreateClock();

                lastAnimationClock.Completed += AnimationOnCompleted;

                ((IAnimatable)AssociatedObject).ApplyAnimationClock(Property, lastAnimationClock, HandoffBehavior.Compose);
            }
            _previousValue = GetPropertyValue();
        }

        /// <summary>Обработчик события Compleated внутренней анимации</summary>
        private void AnimationOnCompleted(object Sender, EventArgs Args)
        {
            lastAnimationClock = null;
            OnAnimationCompleated(new PropertyAnimationCompleatedEventArgs());
        }

        /// <summary>Вызывает событие AnimationCompleated у поведения</summary>
        protected virtual void OnAnimationCompleated(PropertyAnimationCompleatedEventArgs e)
        {
            lastAnimationClock = null;
            EventHandler<PropertyAnimationCompleatedEventArgs> handler = AnimationCompleated;
            if (handler != null) handler(this, e);
        }

        /// <summary>Получает и настраивает подходящую анимацию</summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        private AnimationTimeline GetAnimation(object OldValue, object NewValue)
        {
            AnimationTimeline animation = AnimationFactories[Property.PropertyType](OldValue, NewValue, AnimationDuration);
            animation.FillBehavior = FillBehavior.Stop;
            //animation.Completed += AnimationOnCompleted;
            return animation;
        }

        /// <summary>Аниматор для Double-свойств</summary>
        private static AnimationTimeline GetDoubleAnimation(object OldValue, object NewValue, Duration AnimationDuration)
        {
            return new DoubleAnimation((double)OldValue, (double)NewValue, AnimationDuration);
        }

        /// <summary>Метод, запускающий анимацию указанного свойства</summary>
        /// <param name="OldValue">Старое значение свойства (с которого начинается анимация)</param>
        /// <param name="NewValue">Новое значение свойства (которым должна закончиться анимация)</param>
        /// <param name="AnimationDuration">Продолжительность анимации</param>
        private delegate AnimationTimeline AnimationFactoryMethod(Object OldValue, Object NewValue, Duration AnimationDuration);
    }

    public class PropertyAnimationCompleatedEventArgs : EventArgs { }
}
