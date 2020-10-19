namespace LogTest
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public class AsyncLog : ILog
    {
        private readonly Thread _runThread;
        private readonly TimeSource _timeSource;
        private readonly IAppender _appender;
        private readonly IFormatter _formatter;

        // shared data
        private readonly BlockingCollection<LogLine> _lines;
        private readonly ManualResetEventSlim _flushEvent;
        private volatile bool _exit;
        
        public AsyncLog() : this(new TimeSource())
        {
        }

        public AsyncLog(TimeSource timeSource) : this(timeSource, new RollingAppender(timeSource, Settings.CreateFileAppenderFactory(timeSource)), new LogLineFormatter())
        {
        }

        public AsyncLog(TimeSource timeSource, IAppender appender, IFormatter formatter)
        {
            _timeSource = timeSource ?? throw new ArgumentNullException(nameof(timeSource));
            _appender = appender ?? throw new ArgumentNullException(nameof(appender));
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));

            _lines = new BlockingCollection<LogLine>();
            _flushEvent = new ManualResetEventSlim();

            _runThread = new Thread(this.MainLoop);
            _runThread.Start();
        }

        private void MainLoop()
        {
            try
            {
                MainLoopSafe();
            }
            catch
            {
                // error
            }
        }

        private void MainLoopSafe()
        {
            while (!_exit)
            {
                if (_lines.Count == 0)
                    _flushEvent.Set();

                LogLine logLine = _lines.Take();
                var formatted = logLine.Format();
                _appender.Write(formatted);
            }

            _flushEvent.Set();
        }

        public void Write(string text)
        {
            if (_exit)
                return;

            var line = new LogLine(_formatter)
            {
                Text = text,
                Timestamp = _timeSource.Time
            };

            _lines.Add(line);
            _flushEvent.Reset();
        }

        public void Flush()
        {
            _flushEvent.Wait();
        }

        public void Stop()
        {
            _exit = true;
            _lines.CompleteAdding();
            Dispose();
        }
        public void Dispose()
        {
            if (!_exit)
                Stop();
            _appender.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}