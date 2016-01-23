namespace Mac65.Syntax
{
    public struct ParserContext
    {
        public SyntaxNodeKind NodeKind { get; private set; }
        public TextSpan Span { get; private set; }

        public ParserContext(SyntaxNodeKind nodeKind, TextSpan span) : this()
        {
            NodeKind = nodeKind;
            Span = span;
        }
    }
}