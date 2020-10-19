using LogTest;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogComponent.Tests
{
    public class InMemoryAppender : IAppender
    {
        public List<string> Lines { get; private set; }

        public InMemoryAppender()
        {
            Lines = new List<string>();
        }

        public void Write(string value)
        {
            Lines.Add(value);
        }

        public void Dispose()
        {
        }


    }
}
