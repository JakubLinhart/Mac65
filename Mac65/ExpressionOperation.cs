using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mac65
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct ExpressionOperation
    {
        [FieldOffset(0)] public readonly ExpressionOperationKind Kind;
        [FieldOffset(sizeof (int))] private readonly int Number;

        public ExpressionOperation(ExpressionOperationKind kind, int number)
        {
            if (kind != ExpressionOperationKind.Number)
                throw new InvalidOperationException(
                    "Number can be specified only for ExpressionOperationKind.Number");

            Kind = kind;
            Number = number;
        }

        public ExpressionOperation(ExpressionOperationKind kind)
        {
            if (kind == ExpressionOperationKind.Number)
                throw new InvalidOperationException(
                    "Number has to be specified only for ExpressionOperationKind.Number or Identifier");

            Kind = kind;
            Number = 0;
        }

        public int Precedence
        {
            get
            {
                switch (Kind)
                {
                    case ExpressionOperationKind.Expression:
                        return 1;
                    case ExpressionOperationKind.LogicalAnd:
                    case ExpressionOperationKind.LogicalOr:
                        return 5;
                    case ExpressionOperationKind.LowByte:
                    case ExpressionOperationKind.HighByte:
                        return 10;
                    case ExpressionOperationKind.Equal:
                    case ExpressionOperationKind.NotEqual:
                    case ExpressionOperationKind.GreaterThan:
                    case ExpressionOperationKind.SmallerThan:
                    case ExpressionOperationKind.GreaterThanOrEqual:
                    case ExpressionOperationKind.SmallerThanOrEqual:
                        return 15;
                    case ExpressionOperationKind.BitwiseAnd:
                    case ExpressionOperationKind.BitwiseOr:
                    case ExpressionOperationKind.BitwiseXor:
                        return 25;
                    case ExpressionOperationKind.Add:
                    case ExpressionOperationKind.Sub:
                        return 50;
                    case ExpressionOperationKind.Div:
                    case ExpressionOperationKind.Mult:
                        return 100;
                    case ExpressionOperationKind.LogicalNot:
                        return 150;
                    case ExpressionOperationKind.UnaryMinus:
                        return 200;
                    case ExpressionOperationKind.Ref:
                    case ExpressionOperationKind.Def:
                        return 666;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public void Evaluate(Stack<int> evaluationStack)
        {
            if (Kind == ExpressionOperationKind.Number)
            {
                evaluationStack.Push(Number);
            }
            else
            {
                var y = evaluationStack.Pop();

                switch (Kind)
                {
                    case ExpressionOperationKind.HighByte:
                        evaluationStack.Push((y & 0xFF00) >> 8);
                        return;
                    case ExpressionOperationKind.LowByte:
                        evaluationStack.Push(y & 0xFF);
                        return;
                    case ExpressionOperationKind.UnaryMinus:
                        evaluationStack.Push(-y);
                        return;
                    case ExpressionOperationKind.LogicalNot:
                        evaluationStack.Push((y == 0) ? 1 : 0);
                        return;
                    default:
                        var x = evaluationStack.Pop();

                        switch (Kind)
                        {
                            case ExpressionOperationKind.Add:
                                evaluationStack.Push(x + y);
                                return;
                            case ExpressionOperationKind.Sub:
                                evaluationStack.Push(x - y);
                                return;
                            case ExpressionOperationKind.Mult:
                                evaluationStack.Push(x*y);
                                return;
                            case ExpressionOperationKind.Div:
                                evaluationStack.Push(x/y);
                                return;
                            case ExpressionOperationKind.BitwiseAnd:
                                evaluationStack.Push(x & y);
                                return;
                            case ExpressionOperationKind.BitwiseOr:
                                evaluationStack.Push(x | y);
                                return;
                            case ExpressionOperationKind.BitwiseXor:
                                evaluationStack.Push(x ^ y);
                                return;
                            case ExpressionOperationKind.LogicalAnd:
                                evaluationStack.Push((x == y && x != 0) ? 1 : 0);
                                break;
                            case ExpressionOperationKind.LogicalOr:
                                evaluationStack.Push((x != 0 || y != 0) ? 1 : 0);
                                break;
                            case ExpressionOperationKind.Equal:
                                evaluationStack.Push((x == y) ? 1 : 0);
                                return;
                            case ExpressionOperationKind.NotEqual:
                                evaluationStack.Push((x != y) ? 1 : 0);
                                return;
                            case ExpressionOperationKind.GreaterThan:
                                evaluationStack.Push((x > y) ? 1 : 0);
                                return;
                            case ExpressionOperationKind.GreaterThanOrEqual:
                                evaluationStack.Push((x >= y) ? 1 : 0);
                                return;
                            case ExpressionOperationKind.SmallerThan:
                                evaluationStack.Push((x < y) ? 1 : 0);
                                return;
                            case ExpressionOperationKind.SmallerThanOrEqual:
                                evaluationStack.Push((x <= y) ? 1 : 0);
                                return;
                        }
                        break;
                }
            }
        }
    }
}