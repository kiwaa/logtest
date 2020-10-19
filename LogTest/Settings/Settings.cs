namespace LogTest
{
    public sealed class Settings
    {
        public static IAppenderFactory CreateFileAppenderFactory(TimeSource timeSource)
        {
            return new FileAppenderFactory(timeSource)
            {
                LogDirectory = "C:/LogTest",
                Header = "Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t"
            };
        }
    }
}
