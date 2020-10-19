namespace LogTest
{
    using System;
    using System.IO;
    public sealed class FileAppenderFactory : IAppenderFactory
    {
        public TimeSource TimeSource { get; }
        public string LogDirectory { get; set; }
        public string Header { get; set; }
        public string FileNameTimeFormat { get; set; } = "yyyyMMdd HHmmss fff";

        public FileAppenderFactory(TimeSource timeSource)
        {
            TimeSource = timeSource ?? throw new ArgumentNullException(nameof(timeSource));
            LogDirectory = Directory.GetCurrentDirectory();
        }

        public IAppender Create()
        {
            var file = CreateFileName();
            return new FileAppender(file, Header);
        }

        private string CreateFileName()
        {
            return Path.Combine(LogDirectory, "Log" + TimeSource.Time.ToString(FileNameTimeFormat) + ".log");
        }
    }
}
