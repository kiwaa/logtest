using LogTest;
using NUnit.Framework;
using System.IO;

namespace LogComponent.Tests.IntegrationTests
{
    [TestFixture]
    public class FilleAppenderTests
    {
        private const string TestFileName = "test.log";

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(TestFileName))
                File.Delete(TestFileName);
        }

        [Test]
        public void Ctor_CreatesLogFile()
        {
            var fixture = new Fixture();
            var target = fixture.CreateSut();

            target.Dispose();

            Assert.AreEqual(true, File.Exists(TestFileName));
        }

        [Test]
        public void Write_WritesToFile()
        {
            var fixture = new Fixture();
            var target = fixture.CreateSut();

            target.Write("test");
            target.Dispose();

            var read = LogReaderHelper.Read(TestFileName);
            CollectionAssert.AreEqual(new[] { "test" }, read);
        }

        private class Fixture
        {
            public FileAppender CreateSut()
            {
                return new FileAppender(TestFileName);
            }
        }
    }
}
