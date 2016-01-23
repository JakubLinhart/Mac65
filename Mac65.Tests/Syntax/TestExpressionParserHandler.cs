using System.Collections.Generic;
using Mac65.Syntax;

namespace Mac65.Tests.Syntax
{
    public class TestExpressionParserHandler : ParserHandler
    {
        private readonly List<string> result = new List<string>();

        public string[] Result
        {
            get { return result.ToArray(); }
        }

        public override void EndNode(ParserContext context)
        {
            switch (context.NodeKind)
            {
                case SyntaxNodeKind.UnaryOperator:
                    result.Add(FormatResult("unaryop", context.Span));
                    break;
                case SyntaxNodeKind.Operator:
                    result.Add(FormatResult("op", context.Span));
                    break;
                case SyntaxNodeKind.Constant:
                    result.Add(FormatResult("num", context.Span));
                    break;
                case SyntaxNodeKind.ExpressionIdentifier:
                    result.Add(FormatResult("identifier", context.Span));
                    break;
                case SyntaxNodeKind.ExpressionLeftParenthesis:
                    result.Add("(");
                    break;
                case SyntaxNodeKind.ExpressionRightParenthesis:
                    result.Add(")");
                    break;

            }
        }

        private string FormatResult(string label, string value)
        {
            return string.Format("{0}<{1}>", label, value);
        }

        private string FormatResult(string label, TextSpan span)
        {
            return FormatResult(label, span.Text.Trim());
        }
    }
}