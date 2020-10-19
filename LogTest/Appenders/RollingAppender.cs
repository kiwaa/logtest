namespace LogTest
{
    using System;
    public sealed class RollingAppender : IAppender
    {
        private readonly TimeSource _timeSource;
        private readonly IAppenderFactory _factory;
        private IAppender _appender;
        private DateTime _curDate;

        public RollingAppender(TimeSource timeSource, IAppenderFactory factory)
        {
            _timeSource = timeSource ?? throw new ArgumentNullException(nameof(timeSource));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public void Write(string value)
        {
            var now = _timeSource.Time;
            if (_appender == null || (now - _curDate).Days != 0)
            {
                _curDate = now.Date;
                _appender?.Dispose();
                _appender = _factory.Create();
            }

            _appender.Write(value);
        }

        public void Dispose()
        {
            _appender?.Dispose(); 
            GC.SuppressFinalize(this);
        }
    }
}
