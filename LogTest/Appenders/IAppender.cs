using System;

namespace LogTest
{
    public interface IAppender : IDisposable
    {
        void Write(string value);
    }
}
