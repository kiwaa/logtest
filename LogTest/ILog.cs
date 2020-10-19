using System;

namespace LogTest
{
    public interface ILog : IDisposable
    {
        /// <summary>
        /// The call will not return until all logs have been written to Log.
        /// </summary>
        void Flush();

        /// <summary>
        /// Stop the logging. If any outstadning logs theses will not be written to Log
        /// </summary>
        void Stop();

        /// <summary>
        /// Write a message to the Log.
        /// </summary>
        /// <param name="text">The text to written to the log</param>
        void Write(string text);
    }
}
