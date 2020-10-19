using LogTest;
using NUnit.Framework;
using System;

namespace LogComponent.Tests
{
    [TestFixture]
    public class AsyncLogTests
    {
        // those are not ideal unit tests because AsyncLog has thread inside

        [Test]
        public void WriteWithFlush_WroteSomething()
        {
            var fixture = new Fixture();
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 8, 26, 22, 567));
            var logger = fixture.CreateSut();

            logger.Write("test");

            logger.Flush();
            logger.Stop();

            var actual = fixture.Appender.Lines;
            CollectionAssert.AreEqual(new[] { "2020-10-06 08:26:22:567\ttest. \t" + Environment.NewLine }, actual);
        }

        [Test]
        public void WriteWithFlush_TakeActualTime()
        {
            var fixture = new Fixture();
            var logger = fixture.CreateSut();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 8, 26, 22, 567));
            logger.Write("test");
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 10, 8, 26, 22, 567));
            logger.Write("test");

            logger.Flush();
            logger.Stop();
            
            var actual = fixture.Appender.Lines;
            CollectionAssert.AreEqual(new[] 
            { 
                "2020-10-06 08:26:22:567\ttest. \t" + Environment.NewLine,
                "2020-10-10 08:26:22:567\ttest. \t" + Environment.NewLine 
            }, actual);
        }

        [Test]
        public void WriteAfterFlush_Ok()
        {
            var fixture = new Fixture();
            var logger = fixture.CreateSut();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 8, 26, 22, 567));
            logger.Write("test");
            logger.Flush();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 10, 8, 26, 22, 567));
            logger.Write("test");
            logger.Flush();

            logger.Stop();

            var actual = fixture.Appender.Lines;
            CollectionAssert.AreEqual(new[]
            {
                "2020-10-06 08:26:22:567\ttest. \t" + Environment.NewLine,
                "2020-10-10 08:26:22:567\ttest. \t" + Environment.NewLine
            }, actual);
        }

        [Test]
        public void WriteAfterStopWithFlush_Nothing()
        {
            var fixture = new Fixture();
            var logger = fixture.CreateSut();

            logger.Flush();
            logger.Stop();
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 8, 26, 22, 567));
            logger.Write("test");

            logger.Dispose();

            var actual = fixture.Appender.Lines;
            CollectionAssert.AreEqual(new string[] { }, actual);
        }

        [Test]
        public void WriteAfterDispose_Nothing()
        {
            var fixture = new Fixture();
            var logger = fixture.CreateSut();

            logger.Dispose();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 8, 26, 22, 567));
            logger.Write("test");

            var actual = fixture.Appender.Lines;
            CollectionAssert.AreEqual(new string[] { }, actual);
        }

        [Test]
        public void WriteWithoutFlush_WroteSomething()
        {
            var fixture = new Fixture();
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 8, 26, 22, 567));
            var logger = fixture.CreateSut();

            for (int i = 0; i < 100; i++)
                logger.Write("test");

            logger.Flush();
            logger.Stop();

            var actual = fixture.Appender.Lines;
            Assert.True(actual.Count > 1);
            Assert.AreEqual("2020-10-06 08:26:22:567\ttest. \t" + Environment.NewLine, actual[0]);
        }

        private class Fixture
        {
            public ManualTimeSource TimeSource = new ManualTimeSource();
            public InMemoryAppender Appender = new InMemoryAppender();
            public LogLineFormatter Formatter = new LogLineFormatter();

            public ILog CreateSut()
            {
                return new AsyncLog(TimeSource, Appender, Formatter);
            }
        }
    }
}
