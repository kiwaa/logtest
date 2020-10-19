using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest
{
    public interface IFormatter
    {
        string Format(LogLine logLine);
    }
}
