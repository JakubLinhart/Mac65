using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65
{
    public class MemoryChunk : IEnumerable<byte>
    {
        public ushort StartAddress { get; private set; }
        private readonly byte[] instructionBuffer;

        public MemoryChunk(ushort startAddress, IEnumerable<byte> instructionBuffer)
        {
            StartAddress = startAddress;
            this.instructionBuffer = instructionBuffer.ToArray();
            Length = this.instructionBuffer.Length;
        }

        public byte this[ushort index]
        {
            get { return instructionBuffer[index]; }
        }

        public int Length { get; private set; }

        public IEnumerator<byte> GetEnumerator()
        {
            return ( (IEnumerable<byte>)instructionBuffer ).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
