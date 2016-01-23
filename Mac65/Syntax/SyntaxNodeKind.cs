using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65.Syntax
{
    public enum SyntaxNodeKind
    {
        LineNumber,
        Label,
        Instruction,
        Mnemonic,
        ImmediateOperand,
        AccumulatorOperand,
        ImpliedOperand,
        AbsoluteOperand,
        IndirectOperand,
        OperandIndexer,
        Line,
        Macro,
        MacroName,
        Directive,
        DirectiveName,
        Operator,
        UnaryOperator,
        Constant,
        ExpressionIdentifier,
        ExpressionLeftParenthesis,
        ExpressionRightParenthesis,
        Expression,
        DirectiveOperands,
        Literal
    }
}
