using System;

namespace Mac65.Syntax
{
    public struct TextSpan
    {
        private readonly int end;
        private readonly string source;
        private readonly int start;

        public TextSpan(int start, int end, string source)
        {
            if (start < 0)
                throw new ArgumentException("start cannot be negative", "start");
            if (end < 0)
                throw new ArgumentException("end cannot be negative", "end");
            if (start > end)
                throw new InvalidOperationException("Start is greater than end.");
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("source cannot be null", source);

            this.start = start;
            this.end = end;
            this.source = source;
        }

        public int Start
        {
            get { return start; }
        }

        public int End
        {
            get { return end; }
        }

        public string Source
        {
            get { return source; }
        }

        public string Text
        {
            get { return source.Substring(Start, Length); }
        }

        public int Length { get { return End - Start; } }
    }
}