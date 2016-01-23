using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65
{
    public class AtasciiEncoding : Encoding
    {
        public static readonly AtasciiEncoding Instance = new AtasciiEncoding();

        public override int GetByteCount(char[] chars, int index, int count)
        {
            return ASCII.GetByteCount(chars, index, count);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            return ASCII.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return ASCII.GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return ASCII.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

        public override int GetMaxByteCount(int charCount)
        {
            return ASCII.GetMaxByteCount(charCount);
        }

        public override int GetMaxCharCount(int byteCount)
        {
            return ASCII.GetMaxCharCount(byteCount);
        }
    }
}
