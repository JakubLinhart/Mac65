using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mac65
{
    public class MemoryChunk : IEnumerable<byte>
    {
        public MemoryChunk(ushort startAddress, IEnumerable<byte> buffer)
        {
            StartAddress = startAddress;
            Buffer = buffer.ToArray();

            if (Buffer.Length > ushort.MaxValue)
            {
                throw new InvalidOperationException("Chunk buffer to large: " + Buffer.Length);
            }
            Length = (ushort) Buffer.Length;
        }

        public ushort StartAddress { get; private set; }

        public byte this[ushort index]
        {
            get { return Buffer[index]; }
        }

        public byte[] Buffer { get; }

        public ushort Length { get; private set; }

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>) Buffer).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}