using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mac65.Tests
{
    public static class Mac65Assert
    {
        public static void OpCodes(MemoryImage memoryImage, params byte[] expected)
        {
            OpCodes(memoryImage.Chunks.Single(), new MemoryChunk(0, expected));
        }

        public static void OpCodes(MemoryChunk actualChunk, MemoryChunk expectedChunk)
        {
            string message = null;

            if (expectedChunk.Length != actualChunk.Length)
            {
                message = string.Format("expected and actual image lenght doesn't match, expected {0}, actual is {1}",
                    expectedChunk.Length, actualChunk.Length);
            }

            if (string.IsNullOrEmpty(message))
            {
                ushort i = 0;

                foreach (var expectedByte in expectedChunk)
                {
                    if (expectedByte != actualChunk[i])
                    {
                        message = string.Format("Mismatch on position {0}. Expected {1}, actual is {2}", i, expectedByte,
                            actualChunk[i]);
                        break;
                    }
                    i++;
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                var expectedText = expectedChunk.Select(x => x.ToString("X2")).Aggregate((l, r) => l + ", " + r);
                var actualText = actualChunk.Select(x => x.ToString("X2")).Aggregate((l, r) => l + ", " + r);
                Assert.Fail("{0}\nexpected:\n{1}\nactual:\n{2}", message, expectedText, actualText);
            }
        }

        internal static void OpCodes(MemoryImage actualImage, params MemoryChunk[] expectedChunks)
        {
            string message = null;

            if (actualImage.Chunks.Count() != expectedChunks.Length)
            {
                Assert.Fail("expected and actual chunk count doesn't match, expected {0}, actual is {1}",
                    expectedChunks.Length, actualImage.Chunks.Count());
            }

            var i = 0;
            foreach (var actualChunk in actualImage.Chunks)
            {
                var expectedChunk = expectedChunks[i];

                if (actualChunk.StartAddress != expectedChunk.StartAddress)
                {
                    Assert.Fail("expected chunk start address is {0} but actual is {1}", expectedChunk.StartAddress.ToString("X4"),
                        actualChunk.StartAddress.ToString("X4"));
                }

                OpCodes(actualChunk, expectedChunk);
                i++;
            }
        }
    }
}