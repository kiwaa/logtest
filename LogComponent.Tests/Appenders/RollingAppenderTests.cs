using System;
using LogTest;
using NSubstitute;
using NUnit.Framework;

namespace LogComponent.Tests.Appenders
{
    [TestFixture]
    public class RollingAppenderTests
    {
        [Test]
        public void Write_Create1stAppender()
        {
            var fixture = new Fixture();
            var target = fixture.CreateSut();

            target.Write("test");

            fixture.Factory.Received(1).Create();
        }

        [Test]
        public void Write_Create2ndAppenderAfterMidnight()
        {
            var fixture = new Fixture();
            var target = fixture.CreateSut();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 08, 46, 44, 567));
            target.Write("test");
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 07, 00, 00, 01, 784));
            target.Write("test");

            fixture.Factory.Received(2).Create();
        }

        [Test]
        public void Write_Create3ndAppenderAfter2ndMidnight()
        {
            var fixture = new Fixture();
            var target = fixture.CreateSut();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 08, 46, 44, 567));
            target.Write("test");
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 07, 00, 00, 01, 784));
            target.Write("test");
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 08, 00, 00, 01, 784));
            target.Write("test");

            fixture.Factory.Received(3).Create();
        }

        [Test]
        public void Write_DoNotCreate3ndAppenderAfter1stMidnight()
        {
            var fixture = new Fixture();
            var target = fixture.CreateSut();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 08, 46, 44, 567));
            target.Write("test");
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 07, 00, 00, 01, 784));
            target.Write("test");
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 07, 14, 00, 01, 784));

            fixture.Factory.Received(2).Create();
        }

        [Test]
        public void Write_DisposeOldAppender()
        {
            var fixture = new Fixture();
            var appender = Substitute.For<IAppender>();
            fixture.Factory.Create().Returns(appender);
            var target = fixture.CreateSut();

            fixture.TimeSource.SetTime(new DateTime(2020, 10, 06, 08, 46, 44, 567));
            target.Write("test");
            fixture.TimeSource.SetTime(new DateTime(2020, 10, 07, 00, 00, 01, 784));
            target.Write("test");

            appender.Received(1).Dispose();
        }

        [Test]
        public void Dispose_NeverUsed()
        {
            var fixture = new Fixture();
            var target = fixture.CreateSut();

            target.Dispose();

            Assert.Pass(); // ok if not exception
        }

        [Test]
        public void Dispose_DisposeAppender()
        {
            var fixture = new Fixture();
            var appender = Substitute.For<IAppender>();
            fixture.Factory.Create().Returns(appender);
            var target = fixture.CreateSut();

            target.Write("test");
            target.Dispose();

            appender.Received(1).Dispose();
        }

        private class Fixture
        {
            public ManualTimeSource TimeSource { get; } = new ManualTimeSource();
            public IAppenderFactory Factory { get; } = NSubstitute.Substitute.For<IAppenderFactory>();
            public RollingAppender CreateSut()
            {
                return new RollingAppender(TimeSource, Factory);
            }
        }
    }
}
