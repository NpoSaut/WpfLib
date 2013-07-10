using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfTools
{
    public class ProgressReporter
    {
        public Dispatcher SyncronizeToDispatcher { get; private set; }

        private String _Message;
        public event EventHandler MessageChanged;
        /// <summary>
        /// Сообщение
        /// </summary>
        public String Message
        {
            get { return _Message; }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    if (MessageChanged != null) MessageChanged(this, new EventArgs());
                }
            }
        }

        private double _Progress;
        public event EventHandler ProgressChanged;
        /// <summary>
        /// Прогресс
        /// </summary>
        public double Progress
        {
            get { return _Progress; }
            set
            {
                if (_Progress != value)
                {
                    //App.Current.Dispatcher.Invoke((Action)(delegate()
                    (SyncronizeToDispatcher ?? Dispatcher.CurrentDispatcher).Invoke((Action)(delegate()
                    {
                        _Progress = value;
                        if (ProgressChanged != null) ProgressChanged(this, new EventArgs());
                    }), System.Windows.Threading.DispatcherPriority.Send);
                }
            }
        }

        public ProgressReporter(Dispatcher SyncronizeToDispatcher)
        {
            this.SyncronizeToDispatcher = SyncronizeToDispatcher;
        }
    }

    public class ProgressEnumerator<T> : IEnumerator<T>
    {
        private IList<T> OnList;
        private ProgressBar pr;
        public DispatcherPriority Priority { get; set; }
        public String Message { get; set; }

        private double prewValue = -1;
        private int _i = -1;
        public int i
        {
            get { return _i; }
            set
            {
                var prc = OnList.Count > 0 ?
                    Math.Min(Math.Round((double)(value + 1) / OnList.Count, 2), 1.0) : 
                    1.0;
                if (prc != prewValue)
                {
                    prewValue = prc;
                    //pr.Value = prc;
                    pr.Dispatcher.Invoke((Action)delegate()
                    {
                        pr.Value = prc;
                        if (Message != null) pr.ToolTip = String.Format(Message, prc);
                    });

                    // Небольшая заглушка - ждём, чтобы событие отображения результата успело сработать
                    pr.Dispatcher.Invoke((Action)(() => { /* заглушка */ }), Priority);
                }
                _i = value;
            }
        }

        public ProgressEnumerator(IList<T> OnList, ProgressBar Progress, DispatcherPriority Priority, String Message)
        {
            this.pr = Progress;
            this.OnList = OnList;
            this.Priority = Priority;
            
            if (Message.Contains("{")) this.Message = Message;
            else pr.ToolTip = Message;
        }

        T IEnumerator<T>.Current
        {
            get { return OnList[i]; }
        }

        object System.Collections.IEnumerator.Current
        {
            get { return OnList[i]; }
        }

        public void Reset()
        {
            i = -1;
        }

        public bool MoveNext()
        {
            //App.Current.Dispatcher.Invoke((Action)(() => i++), System.Windows.Threading.DispatcherPriority.Loaded);
            //i++;
            return ++i < OnList.Count;
        }

        public void Dispose()
        {
        }
    }

    public class ProgressEnumerable<T> : IEnumerable<T>
    {
        private ProgressBar pr;
        private IList<T> OnList;
        public DispatcherPriority Priority { get; set; }
        public String Message { get; set; }

        public ProgressEnumerable(IList<T> OnList, ProgressBar Progress, DispatcherPriority Priority, String Message)
        {
            this.pr = Progress;
            this.OnList = OnList;
            this.Priority = Priority;
            this.Message = Message;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new ProgressEnumerator<T>(OnList, pr, Priority, Message);
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ProgressEnumerator<T>(OnList, pr, Priority, Message);
        }
    }

    public static class ProgressEnumerator
    {
        public static IEnumerable<T> AsProgressEnumerable<T>(this IList<T> Source, ProgressBar Progress)
        {
            return AsProgressEnumerable(Source, Progress, "");
        }
        public static IEnumerable<T> AsProgressEnumerable<T>(this IList<T> Source, ProgressBar Progress, String Message)
        {
            return AsProgressEnumerable(Source, Progress, Message, DispatcherPriority.Loaded);
        }
        public static IEnumerable<T> AsProgressEnumerable<T>(this IList<T> Source, ProgressBar Progress, String Message, DispatcherPriority Priority)
        {
            return new ProgressEnumerable<T>(Source, Progress, Priority, Message);
        }
    }
}
