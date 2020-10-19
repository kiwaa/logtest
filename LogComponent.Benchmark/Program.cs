using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComponent.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
//| Method                         | Mean    | Error    | StdDev   | Gen 0       | Gen 1       | Gen 2     | Allocated |
//| Write1milAndFlush_SingleThread | 5.083 s | 0.0479 s | 0.0448 s | 191000.0000 | 181000.0000 | 7000.0000 | 816.63 MB |
//|  Write1milAndFlush_MultiThread | 5.434 s | 0.1035 s | 0.1151 s | 193000.0000 | 188000.0000 | 3000.0000 | 808.86 MB |
            var summary = BenchmarkRunner.Run<AsyncLogBenchmark>();

            Console.ReadLine();

        }
    }
}
