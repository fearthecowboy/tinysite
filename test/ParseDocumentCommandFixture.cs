﻿using System.IO;
using TinySite.Commands;
using Xunit;

namespace RobMensching.TinySite.Test
{
    public class ParseDocumentCommandFixture
    {
        [Fact]
        public void CanParseNoMetadata()
        {
            var path = Path.GetFullPath(@"data\documents\nometa.txt");
            string expected = "This is text.\r\n   It has no metadata.";

            var command = new ParseDocumentCommand();
            command.DocumentPath = path;
            command.ExecuteAsync().Wait();

            Assert.Null(command.Date);
            Assert.False(command.Draft);
            Assert.Empty(command.Metadata);
            Assert.Equal(expected, command.Content);
        }

        [Fact]
        public void CanParseDraft()
        {
            var path = Path.GetFullPath(@"data\documents\draft.txt");
            string expected = "This is a draft.\r\n   It has metadata.";

            var command = new ParseDocumentCommand();
            command.DocumentPath = path;
            command.ExecuteAsync().Wait();

            Assert.Null(command.Date);
            Assert.True(command.Draft);
            Assert.Empty(command.Metadata);
            Assert.Equal(expected, command.Content);
        }
    }
}
