using LogTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComponent.Tests
{
    public class ManualTimeSource : TimeSource
    {
        private DateTime _time;

        public override DateTime Time => _time;

        public void SetTime(DateTime time)
        {
            _time = time;
        }
    }
}
