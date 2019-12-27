using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VisualStudioSnippetGenerator.Utilities
{
    public abstract class ObservableObject
    {
        protected void SetProperty<T>(ref T backingField, T value,
            Action? onChanged = null, [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;

            var previousValue = backingField;
            backingField = value;

            onChanged?.Invoke();
            OnChanged?.Invoke(new ObservableObjectChangedArgs(this, value, previousValue, propertyName));
        }

        public event Action<ObservableObjectChangedArgs>? OnChanged;
    }
}