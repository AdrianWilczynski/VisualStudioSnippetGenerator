using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudioSnippetGenerator.Utilities
{
    public class ObservableCollection<T> : IEnumerable<T>
    {
        private List<T> _list = new List<T>();

        public void Add(T element)
        {
            _list.Add(element);
            OnChanged?.Invoke(new ObservableObjectChangedArgs(this));
        }

        public void Remove(T element)
        {
            _list.Remove(element);
            OnChanged?.Invoke(new ObservableObjectChangedArgs(this));
        }

        public void Move(int currentIndex, int newIndex)
        {
            var element = _list[currentIndex];
            _list.RemoveAt(currentIndex);
            _list.Insert(newIndex, element);
            OnChanged?.Invoke(new ObservableObjectChangedArgs(this));
        }

        public void Replace(IEnumerable<T> elements)
        {
            _list = elements.ToList();
            OnChanged?.Invoke(new ObservableObjectChangedArgs(this));
        }

        public int Count => _list.Count;

        public event Action<ObservableObjectChangedArgs>? OnChanged;

        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}