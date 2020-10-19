using System;

namespace LogTest
{
    public class TimeSource
    {
        public virtual DateTime Time => DateTime.Now;
    }
}
