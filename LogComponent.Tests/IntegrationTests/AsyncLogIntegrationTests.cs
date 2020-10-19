using LogComponent.Tests.IntegrationTests;
using LogTest;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogComponent.Tests
{
    [TestFixture]
    public class AsyncLogIntegrationTests
    {
        private const string OutputDir = "C:/LogTest";

        [SetUp]
        public void TearDown()
        {
            if (Directory.Exists(OutputDir))
            {
                var files = Directory.GetFiles(OutputDir);
                foreach (var file in files)
                    File.Delete(file);
                Directory.Delete(OutputDir);
            }
        }

        [Test]
        public void Write0to15_StopWithFlush()
        {
            var fixture = new Fixture();
            ILog logger = fixture.CreateSut();

            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(50);
            }

            logger.Flush();
            logger.Stop();
            logger.Dispose();

            var files = Directory.GetFiles(OutputDir);
            Assert.AreEqual(1, files.Length);
            var log = LogReaderHelper.Read(files[0]);
            var results = log.Skip(1).Select(x => x.Substring(x.IndexOf('\t') + 1)).ToArray();
            var expected = Enumerable.Range(0, 15).Select(x => $"Number with Flush: {x}. \t").ToArray();
            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void Write50to0_StopWithoutFlush()
        {
            var fixture = new Fixture();
            ILog logger2 = fixture.CreateSut();

            for (int i = 50; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i.ToString());
                Thread.Sleep(20);
            }

            logger2.Stop();
            logger2.Dispose();

            var files = Directory.GetFiles(OutputDir);
            Assert.AreEqual(1, files.Length);
            var log = LogReaderHelper.Read(files[0]);
            var results = log.Skip(1).Select(x => x.Substring(x.IndexOf('\t') + 1)).ToArray();
            Assert.That(() => results.Length > 0 && results.Length <= 50);
        }

        [Test]
        public void MidnightRollover_StopWithFlush()
        {
            var timeSource = new ManualTimeSource();
            timeSource.SetTime(DateTime.UtcNow);
            var fixture = new Fixture();
            ILog logger = fixture.CreateSut(timeSource);

            for (int i = 0; i < 5; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(20);
            }

            timeSource.SetTime(timeSource.Time.AddDays(1));

            for (int i = 5; i < 10; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(20);
            }

            logger.Stop();
            logger.Dispose();

            var files = Directory.GetFiles(OutputDir);
            Assert.AreEqual(2, files.Length);
            var log = LogReaderHelper.Read(files[0]);
            var results = log.Skip(1).Select(x => x.Substring(x.IndexOf('\t') + 1)).ToArray();
            var expected = Enumerable.Range(0, 5).Select(x => $"Number with Flush: {x}. \t").ToArray();
            CollectionAssert.AreEqual(expected, results);

            var log2 = LogReaderHelper.Read(files[1]);
            var results2 = log2.Skip(1).Select(x => x.Substring(x.IndexOf('\t') + 1)).ToArray();
            var expected2 = Enumerable.Range(5, 5).Select(x => $"Number with Flush: {x}. \t").ToArray();
            CollectionAssert.AreEqual(expected2, results2);

        }

        [Test]
        public void WriteMultiThread_StopWithFlush()
        {
            var fixture = new Fixture();
            ILog logger2 = fixture.CreateSut();

            var taskNum = 10;
            var tasks = new Task[taskNum];
            for (int t = 0; t < taskNum; t++)
            {
                tasks[t] = Task.Run(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        logger2.Write($"Number with Flush (Thread {Thread.CurrentThread.ManagedThreadId}): {i}");
                        Thread.Sleep(20);
                    }
                });
            }
            Task.WhenAll(tasks).Wait();

            logger2.Flush();
            logger2.Stop();
            logger2.Dispose();

            var files = Directory.GetFiles(OutputDir);
            Assert.AreEqual(1, files.Length);
            var log = LogReaderHelper.Read(files[0]);
            var results = log.Skip(1).Select(x => x.Substring(x.IndexOf('\t') + 1)).ToArray();
            Assert.That(() => results.Length == 1000);
        }

        //[Test]
        //public void TwoLoggers_WroteSomething()
        //{
        //    var timeSource = new ManualTimeSource();
        //    timeSource.SetTime(new DateTime(2020, 10, 06, 8, 26, 22, 567));

        //    var fixture = new Fixture();
        //    var logger = fixture.CreateSut(timeSource);
        //    var logger2 = fixture.CreateSut(timeSource);

        //    logger.Write("test");
        //    logger2.Write("test");

        //    logger.StopWithFlush();
        //    logger2.StopWithFlush();
        //    logger.Dispose();
        //    logger2.Dispose();

        //    Thread.Sleep(100);
        //    Assert.Pass();
        //}

        private class Fixture
        {
            public AsyncLog CreateSut()
            {
                return new AsyncLog();
            }
            public AsyncLog CreateSut(TimeSource source)
            {
                var factory = new FileAppenderFactory(source)
                {
                    LogDirectory = "C:/LogTest",
                    Header = "Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t"
                };
                return new AsyncLog(source, new RollingAppender(source, factory), new LogLineFormatter());
            }
        }
    }
}
