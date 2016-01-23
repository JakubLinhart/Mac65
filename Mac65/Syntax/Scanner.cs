using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Mac65.Syntax
{
    public class Scanner
    {
        private readonly string source;
        private readonly Stack<int> spanStarts = new Stack<int>(1024);

        public Scanner(string source)
        {
            this.source = source;
            Position = 0;
        }

        public char CurrentChar
        {
            get { return Position >= source.Length ? (char) 0 : source[Position]; }
        }

        public bool IsEnd
        {
            get { return source.Length <= Position; }
        }

        public int Position { get; set; }


        public bool SkipAll(Func<char, bool> predicate)
        {
            while (Position < source.Length && predicate(CurrentChar))
            {
                Position++;
            }

            return true;
        }

        public void StartSpan()
        {
            spanStarts.Push(Position);
        }

        public TextSpan EndSpan()
        {
            var start = spanStarts.Pop();

            return new TextSpan(start, Position, source);
        }

        public bool Skip(int count)
        {
            while (count > 0)
            {
                if (!Skip())
                    return false;

                count--;
            }

            return true;
        }

        public void SkipWhiteSpace()
        {
            while (!IsEnd && (CurrentChar == ' ' || CurrentChar == '\t'))
            {
                if (CurrentChar == '*' || CurrentChar == ';')
                {
                    SkipRestOfLine();
                }
                Position++;
            }
        }

        public void SkipRestOfLine()
        {
            while (!IsEnd && !IsEndOfLine)
            {
                Position++;
            }
        }

        internal TextSpan? AcceptAny(params string[] acceptableStrings)
        {
            SkipWhiteSpace();

            int start = Position;
            if (AcceptAnyImpl(acceptableStrings))
            {
                return new TextSpan(start, Position, source);
            }

            Position = start;

            return null;
        }

        public TextSpan? AcceptAny(params Token[] tokens)
        {
            SkipWhiteSpace();

            int start = Position;
            if (tokens.Any(token => AcceptImpl(token)))
            {
                return new TextSpan(start, Position, source);
            }

            Position = start;

            return null;
        }

        private bool AcceptImpl(Token token)
        {
            switch (token)
            {
                case Token.CharacterConstant:
                    return CharacterConstant();
                case Token.DecimalNumber:
                    return DecimalNumber();
                case Token.HexNumber:
                    return HexNumber();
                case Token.Operator:
                    return Operator();
                case Token.UnaryOperator:
                    return UnaryOperator();
                case Token.Identifier:
                    return Identifier();
                case Token.Literal:
                    return Literal();
                default:
                    throw new InvalidOperationException("Unknown token: " + token.ToString());
            }
        }

        private bool Identifier()
        {
            if (!IsLabelNameFirstChar(CurrentChar))
                return false;

            return SkipAll(IsLabelNameChar);
        }

        private bool Literal()
        {
            if (!Peek('"'))
                return false;

            Skip();
            while (!IsEnd && CurrentChar != '"')
                Skip();

            return SkipOne('"');
        }

        private bool IsLabelNameFirstChar(char ch)
        {
            return ch == '?' || ch == '@' || ch == '.' || char.IsLetter(ch);
        }

        private bool IsLabelNameChar(char ch)
        {
            return char.IsDigit(ch) || IsLabelNameFirstChar(ch);
        }

        private bool UnaryOperator()
        {
            switch (CurrentChar)
            {
                case '<':
                case '>':
                case '-':
                    Skip();
                    return true;
                case '.':
                    return AcceptAnyImpl(".NOT", ".REF", ".DEF");
                default:
                    return false;
            }
        }

        private bool CharacterConstant()
        {
            if (!Peek('\''))
                return false;

            Skip(2);

            return true;
        }

        private bool DecimalNumber()
        {
            if (!char.IsDigit(CurrentChar))
                return false;

            SkipAll(char.IsDigit);

            return true;
        }

        private bool HexNumber()
        {
            if (!Peek('$'))
                return false;

            return SkipOne('$') && SkipAll(IsHexDigit);
        }

        private bool Operator()
        {
            switch (CurrentChar)
            {
                case '>':
                    Skip();
                    if (CurrentChar == '=')
                        Skip();
                    return true;
                case '<':
                    Skip();
                    if (CurrentChar == '=' || CurrentChar == '>')
                        Skip();

                    return true;
                case '=':
                case '+':
                case '-':
                case '*':
                case '/':
                case '&':
                case '^':
                case '!':
                    Skip();
                    return true;
                case '.':
                    if (AcceptAnyImpl(".AND", ".OR"))
                        return true;
                    break;
            }

            return false;
        }

        private bool AcceptImpl(string acceptedString)
        {
            if (Peek(acceptedString))
            {
                Skip(acceptedString.Length);
                return true;
            }

            return false;
        }

        private bool AcceptAnyImpl(params string[] acceptedStrings)
        {
            return acceptedStrings.Any(x => AcceptImpl(x));
        }

        private bool SkipOne(char characterToSkip)
        {
            return SkipOne(ch => characterToSkip == ch);
        }

        private bool SkipOne(Func<char, bool> action)
        {
            if (!action(CurrentChar))
                return false;

            Skip();

            return true;
        }

        private bool IsHexDigit(char ch)
        {
            if (char.IsDigit(ch))
                return true;

            return ( char.ToUpper(ch) >= 'A' && char.ToUpper(ch) <= 'F' );
        }

        public bool Skip()
        {
            if (IsEnd)
                return false;

            Position++;

            return true;
        }

        public TextSpan PositionToSpan()
        {
            SkipWhiteSpace();

            return new TextSpan(Position, Position, source);
        }

        public bool IsEndOfLine { get { return IsEnd || CurrentChar == '\r' || CurrentChar == '\n'; } }

        internal bool Peek(char ch)
        {
            return CurrentChar == ch;
        }

        internal bool Peek(string str)
        {
            int offset = Position;

            foreach (var ch in str)
            {
                if (offset >= source.Length || source[offset] != ch)
                {
                    return false;
                }

                offset++;
            }

            return true;
        }
    }
}