namespace FluentExtractor.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class TrackingList<T> : IEnumerable<T>
    {
        private readonly List<T> _innerCollection = new List<T>();

        public event Action<T> ItemAdded;

        public void Add(T item)
        {
            _innerCollection.Add(item);

            ItemAdded?.Invoke(item);
        }

        public IDisposable OnItemAdded(Action<T> onItemAdded)
        {
            ItemAdded += onItemAdded;
            return new EventDisposable(this, onItemAdded);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class EventDisposable : IDisposable
        {
            private readonly TrackingList<T> parent;
            private readonly Action<T> handler;

            public EventDisposable(TrackingList<T> parent, Action<T> handler)
            {
                this.parent = parent;
                this.handler = handler;
            }

            public void Dispose()
            {
                parent.ItemAdded -= handler;
            }
        }
    }
}
