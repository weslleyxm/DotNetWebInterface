using System.Collections.Concurrent;

namespace DotNetWebInterface.Server
{
    /// <summary>
    /// Manages a collection of IDisposable objects and provides methods to add and dispose them
    /// </summary>
    internal class DisposablesManager
    {
        private readonly ConcurrentBag<IDisposable> _disposables = new();

        /// <summary>
        /// Adds an IDisposable object to the collection
        /// </summary>
        /// <param name="disposable">The IDisposable object to add</param>
        internal void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        /// <summary>
        /// Disposes all IDisposable objects in the collection
        /// </summary>
        internal void DisposeAll()
        { 
            while (_disposables.TryTake(out var disposable))
            {
                disposable.Dispose();
            }

        }
    }
}