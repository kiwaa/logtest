namespace LogTest
{
    using System;
    using System.Text;

    /// <summary>
    /// This is the object that the diff. loggers (filelogger, consolelogger etc.) will operate on. The LineText() method will be called to get the text (formatted) to log
    /// </summary>
    public sealed class LogLine
    {
        #region Private Fields
        private readonly IFormatter _formatter;

        #endregion

        #region Constructors

        public LogLine(IFormatter formatter)
        {
            _formatter = formatter ?? throw new ArgumentNullException();

            this.Text = "";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Formats logline for log
        /// </summary>
        public string Format()
        {
            return _formatter.Format(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The text to be display in logline
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The Timestamp is initialized when the log is added. Th
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        #endregion
    }
}