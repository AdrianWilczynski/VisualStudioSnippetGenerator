using System;
using Microsoft.AspNetCore.Components;

namespace VisualStudioSnippetGenerator.Pages
{
    public abstract class SyncableComponentBase : ComponentBase
    {
        public abstract void Sync();

        protected void SetThenSync<T>(T value, ref T backingField)
        {
            backingField = value;
            Sync();
        }

        protected void WithSync(Action action)
        {
            action();
            Sync();
        }
    }
}