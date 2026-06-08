using System;

namespace Core
{
    public class Disposable : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Disposable()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                DisposeCore();
            }

            _disposed = true;
        }

        protected virtual void DisposeCore()
        {
        }
    }
}