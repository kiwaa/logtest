namespace LogTest
{
    using System.Text;

    public sealed class LogLineFormatter : IFormatter
    {
        public string TimestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss:fff";
        public string Separator { get; set; } = "\t";
        public string EndOfData { get; set; } = ". ";

        public string Format(LogLine line)
        {
            StringBuilder sb = new StringBuilder();
            AppendTimestamp(sb, line);
            AppendSeparator(sb);
            AppendLineText(sb, line);
            AppendSeparator(sb);
            sb.AppendLine();
            return sb.ToString();
        }

        private void AppendSeparator(StringBuilder sb)
        {
            sb.Append(Separator);
        }

        private void AppendTimestamp(StringBuilder sb, LogLine line)
        {
            sb.Append(line.Timestamp.ToString(TimestampFormat));
        }

        /// <summary>
        /// Append a formatted line
        /// </summary>
        /// <returns></returns>
        private void AppendLineText(StringBuilder sb, LogLine line)
        {
            if (line.Text.Length > 0)
            {
                sb.Append(line.Text);
                sb.Append(EndOfData);
            }
        }
    }
}
