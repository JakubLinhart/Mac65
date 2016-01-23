using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mac65
{
    public class MemoryImage : IEnumerable<byte>
    {
        private readonly List<MemoryChunk> chunks;

        public MemoryImage(IEnumerable<MemoryChunk> chunks)
        {
            this.chunks = chunks.ToList();

            Length = this.chunks.Sum(ch => ch.Length);
        }

        public byte this[ushort address]
        {
            get
            {
                var chunk = chunks.Last(ch => ch.StartAddress <= address && ch.StartAddress + ch.Length > address);

                if (address < chunk.StartAddress)
                {
                    throw new InvalidOperationException("Invalid chunk found");
                }

                var index = (ushort)(address - chunk.StartAddress);

                return chunk[index];
            }
        }

        public IEnumerable<MemoryChunk> Chunks { get { return chunks; } }

        public int Length { get; private set; }

        public IEnumerator<byte> GetEnumerator()
        {
            return chunks.SelectMany(chunk => chunk).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}