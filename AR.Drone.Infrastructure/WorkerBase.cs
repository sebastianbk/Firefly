﻿using System;
using System.Diagnostics;
using System.Threading;

namespace AR.Drone.Infrastructure
{
    public abstract class WorkerBase : DisposableBase
    {
        private CancellationTokenSource _cancellationTokenSource;

        public bool IsAlive
        {
            get { return _cancellationTokenSource != null; }
        }

        public event Action<Object, Exception> UnhandledException;

        public void Start()
        {
            if (_cancellationTokenSource != null)
                return;
            lock (this)
            {
                if (_cancellationTokenSource != null)
                    return;

                _cancellationTokenSource = new CancellationTokenSource();

                var thread = new Thread(RunLoop) {Name = GetType().Name};
                thread.Start();
            }
        }

        public void Stop()
        {
            if (_cancellationTokenSource == null)
                return;
            lock (this)
            {
                if (_cancellationTokenSource == null)
                    return;

                _cancellationTokenSource.Cancel();
            }
        }

        public void Join()
        {
            while (IsAlive) Thread.Sleep(1);
        }

        private void RunLoop()
        {
            try
            {
                CancellationToken token = _cancellationTokenSource.Token;
                Loop(token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                OnException(exception);
            }
            finally
            {
                lock (this)
                {
                    CancellationTokenSource cancellationTokenSource = _cancellationTokenSource;
                    _cancellationTokenSource = null;
                    cancellationTokenSource.Dispose();
                }
            }
        }

        protected abstract void Loop(CancellationToken token);

        protected virtual void OnException(Exception exception)
        {
            Trace.TraceError("{0} - Exception: {1}", GetType(), exception.Message);
            Trace.TraceError(exception.StackTrace);

            OnUnhandledException(exception);
        }

        protected virtual void OnUnhandledException(Exception exception)
        {
            if (UnhandledException != null)
                UnhandledException(this, exception);
        }

        protected override void DisposeOverride()
        {
            Stop();
        }
    }
}