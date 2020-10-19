using BenchmarkDotNet.Attributes;
using LogTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogComponent.Benchmark
{
    [MemoryDiagnoser]
    public class AsyncLogBenchmark
    {
        private const string OutputDir = "C:/LogTest";
        private AsyncLog _log;

        [IterationSetup]
        public void Prepare()
        {
            _log = new AsyncLog();
        }

        [IterationCleanup]
        public void TearDown()
        {
            _log.Dispose();
            _log = null;
            var files = Directory.GetFiles(OutputDir);
            foreach (var file in files)
                File.Delete(file);
            Directory.Delete(OutputDir);
        }

        [Benchmark]
        public void Write1milAndFlush_SingleThread()
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                _log.Write("Number with Flush: " + i.ToString());
            }

            _log.Flush();
            _log.Stop();
        }

        [Benchmark]
        public void Write1milAndFlush_MultiThread()
        {
            var taskNum = 10;
            var tasks = new Task[taskNum];
            for (int t = 0; t < taskNum; t++)
            {
                tasks[t] = Task.Run(() =>
                {
                    for (int i = 0; i < 100_000; i++)
                    {
                        _log.Write($"Number with Flush: " + i.ToString());
                    }
                });
            }
            Task.WhenAll(tasks).Wait();


            _log.Flush();
            _log.Stop();
        }
    }

}
