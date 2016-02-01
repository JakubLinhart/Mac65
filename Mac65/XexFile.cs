using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65
{
    public static class XexFile
    {
        public static void Write(MemoryImage image, string fileName)
        {
            using (var writeStream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Write(image, writeStream);
            }
        }

        public static void Write(MemoryImage image, FileStream writeStream)
        {
            foreach (var chunk in image.Chunks)
            {
                Write(chunk, writeStream);
            }
        }

        private static void Write(MemoryChunk chunk, FileStream writeStream)
        {
            writeStream.WriteByte(0xFF);
            writeStream.WriteByte(0xFF);
            writeStream.WriteByte((byte)(chunk.StartAddress & 0xFF));
            writeStream.WriteByte((byte)((chunk.StartAddress & 0xFF00) >> 8));
            writeStream.WriteByte((byte)(chunk.Length & 0xFF));
            writeStream.WriteByte((byte)((chunk.Length & 0xFF00) >> 8));
            writeStream.Write(chunk.Buffer, 0, chunk.Length);
        }
    }
}
