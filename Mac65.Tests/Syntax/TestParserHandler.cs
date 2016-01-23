using System.Collections.Generic;
using System.Text;
using Mac65.Syntax;

namespace Mac65.Tests.Syntax
{
    internal class TestParserHandler : ParserHandler
    {
        private readonly StringBuilder instruction = new StringBuilder(128);
        private readonly List<string> result = new List<string>();
        private string indexer;
        private string macroName;
        private string operand;
        private readonly StringBuilder directive = new StringBuilder(256);

        public string[] Result
        {
            get { return result.ToArray(); }
        }

        public override void StartNode(ParserContext context)
        {
            switch (context.NodeKind)
            {
                case SyntaxNodeKind.Line:
                    instruction.Clear();
                    directive.Clear();
                    indexer = null;
                    break;
                case SyntaxNodeKind.Directive:
                    directive.Append("directive<");
                    break;
            }
        }

        public override void EndNode(ParserContext context)
        {
            switch (context.NodeKind)
            {
                case SyntaxNodeKind.LineNumber:
                    EndLineNumber(context);
                    break;
                case SyntaxNodeKind.Mnemonic:
                    EndMnemonic(context);
                    break;
                case SyntaxNodeKind.Instruction:
                    EndInstruction(context);
                    break;
                case SyntaxNodeKind.Label:
                    EndLabel(context);
                    break;
                case SyntaxNodeKind.AbsoluteOperand:
                    operand = FormatResult("absolute", context.Span);
                    break;
                case SyntaxNodeKind.ImpliedOperand:
                    operand = "implied";
                    break;
                case SyntaxNodeKind.AccumulatorOperand:
                    operand = "accumulator";
                    break;
                case SyntaxNodeKind.IndirectOperand:
                    operand = FormatResult("indirect", context.Span);
                    break;
                case SyntaxNodeKind.ImmediateOperand:
                    operand = FormatResult("immediate", context.Span);
                    break;
                case SyntaxNodeKind.OperandIndexer:
                    indexer = FormatResult("indexer", context.Span);
                    break;
                case SyntaxNodeKind.Macro:
                    result.Add(FormatResult("macro", macroName));
                    break;
                case SyntaxNodeKind.DirectiveOperands:
                    directive.AppendFormat(", {0}", context.Span.Text);
                    break;
                case SyntaxNodeKind.Literal:
                    directive.AppendFormat(", {0}", context.Span.Text);
                    break;
                case SyntaxNodeKind.MacroName:
                    macroName = context.Span.Text;
                    break;
                case SyntaxNodeKind.Directive:
                    directive.Append(">");
                    result.Add(directive.ToString());
                    break;
                case SyntaxNodeKind.DirectiveName:
                    directive.Append(context.Span.Text);
                    break;
            }
        }

        private void EndLineNumber(ParserContext context)
        {
            result.Add(FormatResult("line", context.Span));
        }

        private void EndMnemonic(ParserContext context)
        {
            instruction.Append(FormatResult("mnemonic", context.Span));
        }

        private void EndInstruction(ParserContext context)
        {
            result.Add(indexer == null
                ? string.Format("instruction<{0}, {1}>", instruction, operand)
                : string.Format("instruction<{0}, {1}, {2}>", instruction, operand, indexer));
        }

        private void EndLabel(ParserContext context)
        {
            result.Add(FormatResult("label", context.Span));
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