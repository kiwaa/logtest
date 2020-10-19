using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComponent.Tests.IntegrationTests
{
    internal static class LogReaderHelper
    {
        public static IEnumerable<string> Read(string file)
        {
            return File.ReadAllLines(file);
        }
    }
}
