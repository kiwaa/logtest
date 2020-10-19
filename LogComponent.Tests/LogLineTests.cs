using LogTest;
using NUnit.Framework;
using System;

namespace LogComponent.Tests
{
    [TestFixture]
    public class LogLineTests
    {
        [Test]
        public void Format_EmptyLogLine()
        {
            var formatter = new LogLineFormatter();
            var line = new LogLine(formatter);

            var actual = line.Format();

            Assert.AreEqual("0001-01-01 00:00:00:000\t\t" + Environment.NewLine, actual);
        }

        [Test]
        public void Format_NoText()
        {
            var formatter = new LogLineFormatter();
            var line = new LogLine(formatter)
            {
                Timestamp = new DateTime(2020, 06, 10, 08, 45, 44, 567)
            };

            var actual = line.Format();

            Assert.AreEqual("2020-06-10 08:45:44:567\t\t" + Environment.NewLine, actual);
        }

        [Test]
        public void Format_Full()
        {
            var formatter = new LogLineFormatter();
            var line = new LogLine(formatter)
            {
                Timestamp = new DateTime(2020, 06, 10, 08, 45, 44, 567),
                Text = "test"
            };

            var actual = line.Format();

            Assert.AreEqual("2020-06-10 08:45:44:567\ttest. \t" + Environment.NewLine, actual);
        }
    }
}
