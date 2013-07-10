using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;

namespace WpfTools
{
    public interface IViewModelProjection<T>
    {
        T Link { get; }
    }
        
    /// <summary>
    /// Класс для генерации оболочечной ReadOnlyObservableCollection из имеющейся ObservableCollection при помощи заданного преобразования
    /// </summary>
    /// <typeparam name="Torig"></typeparam>
    /// <typeparam name="Tproj"></typeparam>
    class ObservableShellConstructor<Torig, Tproj>
        where Tproj : IViewModelProjection<Torig>
    {
        public Func<Torig, Tproj> ProjectingExpression { get; private set; }
        public ReadOnlyObservableCollection<Torig> Originals { get; private set; }
        private ObservableCollection<Tproj> _Projections { get; set; }
        public ReadOnlyObservableCollection<Tproj> Projections { get; private set; }

        public ObservableShellConstructor(ReadOnlyObservableCollection<Torig> Originals, Func<Torig, Tproj> ProjectingExpression)
        {
            this.ProjectingExpression = ProjectingExpression;
            this.Originals = Originals;
            this._Projections = new ObservableCollection<Tproj>(Originals.Select(o => this.ProjectingExpression(o)));
            ((INotifyCollectionChanged)Originals).CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Original_CollectionChanged);
            this.Projections = new ReadOnlyObservableCollection<Tproj>(this._Projections);
        }

        public Tproj GetProjectionFor(Torig Item)
        {
            return _Projections.Single(pi => pi.Link.Equals(Item));
        }

        void Original_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (var ne in e.NewItems.OfType<Torig>()) _Projections.Add(ProjectingExpression(ne));
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (var re in e.OldItems.OfType<Torig>().Select(oi => GetProjectionFor(oi)).ToList()) _Projections.Remove(re);
                    break;
                default: throw new NotImplementedException(String.Format("Не написан код ля данной операции ({0})", e.Action));
            }
        }
    }



    public static class ObservableCollectionHelper
    {
        public static ReadOnlyObservableCollection<T2> ProjectToReadOnlyObservableCollection<T1, T2>(this ReadOnlyObservableCollection<T1> From, Func<T1, T2> ConvertingExpression)
            where T2 : IViewModelProjection<T1>
        {
            return (new ObservableShellConstructor<T1, T2>(From, ConvertingExpression)).Projections;
        }
    }
}
