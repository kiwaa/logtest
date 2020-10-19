using System;
using System.IO;

namespace LogTest
{
    public sealed class FileAppender : IAppender
    {
        private StreamWriter _writer;

        public string Header { get; }

        public FileAppender(string file) : this(file, null)
        {
        }

        public FileAppender(string file, string header)
        {
            Header = header;

            CreateDirectoryIfNeeded(file);
            _writer = CreateWriter(file);

            WriteHeader();
        }

        private void CreateDirectoryIfNeeded(string file)
        {
            var dir = Path.GetDirectoryName(file);
            if (string.IsNullOrEmpty(dir))
                dir = Directory.GetCurrentDirectory();
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        private StreamWriter CreateWriter(string file)
        {
            var sw = File.AppendText(file);
            sw.AutoFlush = true;
            return sw;
        }

        private void WriteHeader()
        {
            if (Header != null)
                _writer.WriteLine(Header);
        }

        public void Write(string value)
        {
            _writer.Write(value);
        }

        public void Dispose()
        {
            _writer.Dispose(); 
            GC.SuppressFinalize(this);
        }

    }
}
