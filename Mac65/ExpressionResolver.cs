using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Mac65.Syntax;

namespace Mac65
{
    public class ExpressionResolver : ParserHandler
    {
        private readonly LabelStore labelStore;
        private readonly Stack<ExpressionOperation> operatorStack = new Stack<ExpressionOperation>();
        private readonly Queue<ExpressionOperation> outputQueue = new Queue<ExpressionOperation>();
        private int lineNumber;

        public ExpressionResolver(LabelStore labelStore)
        {
            this.labelStore = labelStore;
        }

        public int Evaluate()
        {
            var evaluationStack = new Stack<int>(outputQueue.Count);

            while (outputQueue.Count > 0)
            {
                outputQueue.Dequeue().Evaluate(evaluationStack);
            }

            return evaluationStack.Pop();
        }

        private ExpressionOperationKind ConvertUnaryToOperationKind(TextSpan span)
        {
            switch (span.Text)
            {
                case "<":
                    return ExpressionOperationKind.LowByte;
                case ">":
                    return ExpressionOperationKind.HighByte;
                case "-":
                    return ExpressionOperationKind.UnaryMinus;
                case ".NOT":
                    return ExpressionOperationKind.LogicalNot;
                case ".DEF":
                    return ExpressionOperationKind.Def;
                case ".REF":
                    return ExpressionOperationKind.Ref;
                default:
                    throw new InvalidOperationException();
            }
        }

        private ExpressionOperationKind ConvertToOperationKind(TextSpan span)
        {
            switch (span.Text)
            {
                case "=":
                    return ExpressionOperationKind.Equal;
                case "<>":
                    return ExpressionOperationKind.NotEqual;
                case "<":
                    return ExpressionOperationKind.SmallerThan;
                case ">":
                    return ExpressionOperationKind.GreaterThan;
                case "<=":
                    return ExpressionOperationKind.SmallerThanOrEqual;
                case ">=":
                    return ExpressionOperationKind.GreaterThanOrEqual;
                case "+":
                    return ExpressionOperationKind.Add;
                case "-":
                    return ExpressionOperationKind.Sub;
                case "*":
                    return ExpressionOperationKind.Mult;
                case "/":
                    return ExpressionOperationKind.Div;
                case "&":
                    return ExpressionOperationKind.BitwiseAnd;
                case "!":
                    return ExpressionOperationKind.BitwiseOr;
                case "^":
                    return ExpressionOperationKind.BitwiseXor;
                case ".AND":
                    return ExpressionOperationKind.LogicalAnd;
                case ".OR":
                    return ExpressionOperationKind.LogicalOr;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override void StartNode(ParserContext context)
        {
            switch (context.NodeKind)
            {
                case SyntaxNodeKind.Expression:
                    operatorStack.Push(new ExpressionOperation(ExpressionOperationKind.Expression));
                    break;
                case SyntaxNodeKind.Line:
                    lineNumber++;
                    break;
            }
        }

        public override void EndNode(ParserContext context)
        {
            switch (context.NodeKind)
            {
                case SyntaxNodeKind.Expression:
                    while (operatorStack.Count > 0)
                    {
                        var op = operatorStack.Pop();
                        if (op.Kind == ExpressionOperationKind.Expression)
                            break;
                        outputQueue.Enqueue(op);
                    }
                    break;
                case SyntaxNodeKind.UnaryOperator:
                    var unaryOp = new ExpressionOperation(ConvertUnaryToOperationKind(context.Span));
                    EnqueOperation(unaryOp);
                    break;
                case SyntaxNodeKind.Operator:
                    var op1 = new ExpressionOperation(ConvertToOperationKind(context.Span));
                    EnqueOperation(op1);
                    break;
                case SyntaxNodeKind.Constant:
                    outputQueue.Enqueue(
                        new ExpressionOperation(ExpressionOperationKind.Number, ToNumber(context.Span.Text)));
                    break;
                case SyntaxNodeKind.ExpressionIdentifier:
                    Label label;
                    var labelName = context.Span.Text;
                    var found = labelStore.TryGetLabel(labelName, out label);

                    var topOperatorKind = operatorStack.Peek().Kind;

                    switch (topOperatorKind)
                    {
                        case ExpressionOperationKind.Def:
                            labelStore.AddReferencedLabel(labelName, lineNumber);
                            operatorStack.Pop();
                            outputQueue.Enqueue(new ExpressionOperation(ExpressionOperationKind.Number, found ? 1 : 0));
                            break;
                        case ExpressionOperationKind.Ref:
                            operatorStack.Pop();
                            ISet<int> referencingLines;
                            
                            bool anyReference = labelStore.TryGetReferencingLines(labelName, out referencingLines);
                            int refOperatorResult = (anyReference &&
                                                (referencingLines.Count != 1 || !referencingLines.Contains(lineNumber)))
                                ? 1
                                : 0;
                            outputQueue.Enqueue(new ExpressionOperation(ExpressionOperationKind.Number, refOperatorResult));
                            break;
                        default:
                            labelStore.AddReferencedLabel(labelName, lineNumber);
                            outputQueue.Enqueue(
                                found
                                    ? new ExpressionOperation(ExpressionOperationKind.Number, label.Value)
                                    : new ExpressionOperation(ExpressionOperationKind.Number, 0));
                            break;
                    }
                    break;
            }
        }

        private void EnqueOperation(ExpressionOperation operation)
        {
            while (operatorStack.Count > 0 && operation.Precedence <= operatorStack.Peek().Precedence)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }
            operatorStack.Push(operation);
        }

        private int ToNumber(string text)
        {
            if (text.StartsWith("$"))
            {
                return int.Parse(text.Substring(1, text.Length - 1), NumberStyles.HexNumber);
            }
            else if (text[0] == '\'')
            {
                if (text.Length > 0)
                    return AtasciiEncoding.Instance.GetBytes(text)[1];
            }

            return int.Parse(text);
        }
    }
}