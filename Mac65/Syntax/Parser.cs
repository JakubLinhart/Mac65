using System;
using System.CodeDom;
using System.Diagnostics;
using System.Linq;

namespace Mac65.Syntax
{
    public class Parser
    {
        private readonly ParserHandler handler;
        private Scanner scanner;

        public Parser(ParserHandler handler)
        {
            this.handler = handler;
        }

        public bool Parse(string source)
        {
            scanner = new Scanner(source);

            while (!scanner.IsEnd)
            {
                if (!ParseLine())
                    return false;
                SkipOne('\r');
                SkipOne('\n');
            }

            return true;
        }

        private bool ParseLine()
        {
            return Emit(SyntaxNodeKind.Line, () => LineNumber() && SourceStatement());
        }

        private bool SourceStatement()
        {
            LabelOrDirective();

            return scanner.IsEndOfLine ||
                   (Optional(Space()) && (Instruction() || Directive() || ChangeOriginDirective())) || Comment();
        }

        private bool Comment()
        {
            if (scanner.CurrentChar == '*' || scanner.CurrentChar == ';')
            {
                scanner.SkipRestOfLine();
                return true;
            }

            return false;
        }

        private bool LabelOrDirective()
        {
            return
                (Label() && Optional(Optional(Space()) && (AssignmentDirective() || Directive())))
                || Directive();
        }

        private bool ChangeOriginDirective()
        {
            if (scanner.Peek("*="))
            {
                return
                    Emit(SyntaxNodeKind.Directive,
                        () => AcceptTerminal(SyntaxNodeKind.DirectiveName, "*=") &&
                              Emit(SyntaxNodeKind.DirectiveOperands, Expression));
            }

            return false;
        }

        private bool AssignmentDirective()
        {
            if (scanner.Peek('='))
            {
                return
                    Emit(SyntaxNodeKind.Directive,
                        () =>
                            AcceptTerminal(SyntaxNodeKind.DirectiveName, "=") &&
                            Emit(SyntaxNodeKind.DirectiveOperands, Expression));
            }

            return false;
        }

        private bool Directive()
        {
            if (!scanner.Peek('.'))
                return false;

            return Macro() || ExpressionDirective() || ExpressionArrayDirective() || GenericDirective();
        }

        private bool ExpressionDirective()
        {
            if (scanner.Peek(".DS") || scanner.Peek(".IF"))
            {
                return Emit(SyntaxNodeKind.Directive,
                    () => AcceptTerminal(SyntaxNodeKind.DirectiveName, ".DS", ".IF") &&
                          Emit(SyntaxNodeKind.DirectiveOperands, Expression));
            }

            return false;
        }

        private bool ExpressionArrayDirective()
        {
            if (scanner.Peek(".BYTE") || scanner.Peek(".WORD") || scanner.Peek(".DBYTE"))
            {
                return Emit(SyntaxNodeKind.Directive,
                    () => AcceptTerminal(SyntaxNodeKind.DirectiveName, ".BYTE", ".WORD", ".DBYTE") &&
                          ExpressionArray());
            }

            return false;
        }

        private bool ExpressionArray()
        {
            if (!ExpressionArrayItem())
                return false;

            while (true)
            {
                Space();
                if (!scanner.Peek(','))
                    break;
                scanner.Skip();
                Space();

                if (!ExpressionArrayItem())
                    return false;
            }

            Comment();

            return true;
        }

        private bool ExpressionArrayItem()
        {
            return Literal() || Emit(SyntaxNodeKind.DirectiveOperands, Expression);
        }

        private bool Literal()
        {
            var span = scanner.AcceptAny(Token.Literal);
            if (span.HasValue)
            {
                EmitTerminal(SyntaxNodeKind.Literal, new TextSpan(span.Value.Start + 1, span.Value.End - 1, span.Value.Source));
                return true;
            }

            return false;
        }

        private bool GenericDirective()
        {
            return Emit(SyntaxNodeKind.Directive,
                () => Emit(SyntaxNodeKind.DirectiveName, () => SkipOne('.') && Symbol()));
        }

        private bool Macro()
        {
            if (!scanner.Peek(".MACRO"))
                return false;

            return Emit(SyntaxNodeKind.Macro, () => scanner.AcceptAny(".MACRO").HasValue && MacroName());
        }

        private bool MacroName()
        {
            return Emit(SyntaxNodeKind.MacroName, Symbol);
        }

        private bool Label()
        {
            if (!char.IsLetter(scanner.CurrentChar) && !scanner.Peek('?') && !scanner.Peek('@'))
                return false;

            return Emit(SyntaxNodeKind.Label, LabelName);
        }

        private bool LabelName()
        {
            scanner.SkipAll(IsLabelNameChar);

            return true;
        }

        private bool IsLabelNameChar(char ch)
        {
            return char.IsLetterOrDigit(ch) || ch == '?' || ch == '@' || ch == '.';
        }

        private bool LineNumber()
        {
            return Emit(SyntaxNodeKind.LineNumber, () => SkipAll(char.IsDigit) && SkipOne(IsSpace));
        }

        private bool Instruction()
        {
            return Emit(SyntaxNodeKind.Instruction,
                () => Mnemonic() && IsSpaceOrEndOfLine(scanner.CurrentChar) && Optional(Space) && Optional(Operand));
        }

        private bool Operand()
        {
            return ImpliedOperand() || ImmediateOperand() || AccumulatorOperand() || AbsoluteOperand() ||
                   IndirectOperand();
        }

        private bool IndirectOperand()
        {
            if (!scanner.Peek('('))
                return false;

            return Emit(SyntaxNodeKind.IndirectOperand, () =>
                SkipOne('(') &&
                Expression() &&
                (IndirectX() || IndirectY() || SkipOne(')')) &&
                scanner.IsEndOfLine);
        }

        private bool IndirectX()
        {
            if (!scanner.Peek(','))
                return false;

            return OperandIndexer('X') && SkipOne(')');
        }

        private bool IndirectY()
        {
            if (!scanner.Peek("),"))
                return false;

            return SkipOne(')') && OperandIndexer('Y');
        }

        private bool OperandIndexer(char indexer)
        {
            if (!scanner.Peek(','))
                return false;

            return SkipOne(',') && Emit(SyntaxNodeKind.OperandIndexer, () => SkipOne(indexer));
        }

        private bool AccumulatorOperand()
        {
            return scanner.CurrentChar == 'A' && Emit(SyntaxNodeKind.AccumulatorOperand, () => SkipOne('A')) &&
                   scanner.IsEndOfLine;
        }

        private bool ImmediateOperand()
        {
            return SkipOne('#') && Emit(SyntaxNodeKind.ImmediateOperand, Expression);
        }

        private bool ImpliedOperand()
        {
            if (scanner.IsEndOfLine)
            {
                EmitTerminal(SyntaxNodeKind.ImpliedOperand, scanner.PositionToSpan());
                return true;
            }

            return false;
        }

        private bool AbsoluteOperand()
        {
            return Emit(SyntaxNodeKind.AbsoluteOperand, () => Expression() && Optional(OperandIndexer));
        }

        private bool OperandIndexer()
        {
            if (!scanner.Peek(','))
                return false;

            return SkipOne(',') && Emit(SyntaxNodeKind.OperandIndexer, () => SkipOne('X') || SkipOne('Y'));
        }

        public bool ParseExpression(string source)
        {
            scanner = new Scanner(source);

            return Expression();
        }

        private bool Expression()
        {
            return Emit(SyntaxNodeKind.Expression, () =>
            {
                while (true)
                {
                    ExpressionUnaryOperator();
                    if (!(ExpressionIdentifier() || Constant() || ParenthesisedExpression()))
                        return false;

                    if (!ExpressionOperator())
                        break;
                    SkipAll(IsSpace);
                }

                return true;
            });
        }

        private bool Constant()
        {
            return AcceptTerminal(SyntaxNodeKind.Constant, Token.CharacterConstant, Token.DecimalNumber, Token.HexNumber);
        }

        private bool ExpressionOperator()
        {
            return AcceptTerminal(SyntaxNodeKind.Operator, Token.Operator);
        }

        private bool ExpressionUnaryOperator()
        {
            return AcceptTerminal(SyntaxNodeKind.UnaryOperator, Token.UnaryOperator);
        }

        private bool ParenthesisedExpression()
        {
            if (!scanner.Peek('['))
                return false;

            return Emit(SyntaxNodeKind.ExpressionLeftParenthesis, () => SkipOne('[')) &&
                Expression() &&
                Emit(SyntaxNodeKind.ExpressionRightParenthesis, () => SkipOne(']'));
        }

        private bool ExpressionIdentifier()
        {
            return AcceptTerminal(SyntaxNodeKind.ExpressionIdentifier, Token.Identifier);
        }

        private bool AcceptTerminal(SyntaxNodeKind syntaxKind, params string[] acceptableStrings)
        {
            var span = scanner.AcceptAny(acceptableStrings);

            if (span.HasValue)
            {
                EmitTerminal(syntaxKind, span.Value);
                return true;
            }

            return false;
        }

        private bool AcceptTerminal(SyntaxNodeKind syntaxKind, params Token[] acceptableTokens)
        {
            var span = scanner.AcceptAny(acceptableTokens);

            if (span.HasValue)
            {
                EmitTerminal(syntaxKind, span.Value);
                return true;
            }

            return false;
        }

        private bool Mnemonic()
        {
            return Emit(SyntaxNodeKind.Mnemonic, Symbol) && IsSpaceOrEndOfLine(scanner.CurrentChar);
        }

        private bool Symbol()
        {
            if (!char.IsLetter(scanner.CurrentChar))
                return false;

            scanner.SkipAll(char.IsLetter);

            return true;
        }

        private bool Space()
        {
            return SkipAll(IsSpace);
        }

        [DebuggerStepThrough]
        private bool Optional(bool value)
        {
            return true;
        }
        
        [DebuggerStepThrough]
        private bool Optional(Func<bool> action)
        {
            action();

            return true;
        }

        private bool SkipAll(Func<char, bool> action)
        {
            if (!action(scanner.CurrentChar))
                return false;

            scanner.SkipAll(action);

            return true;
        }

        private bool SkipOne(char characterToSkip)
        {
            return SkipOne(ch => characterToSkip == ch);
        }

        private bool SkipOne(Func<char, bool> action)
        {
            if (!action(scanner.CurrentChar))
                return false;

            scanner.Skip();

            return true;
        }

        [DebuggerStepThrough]
        private bool Emit(SyntaxNodeKind nodeKind, Func<bool> consumeAction)
        {
            CallHandler(new ParserContext(nodeKind, scanner.PositionToSpan()), (h, c) => h.StartNode(c));

            scanner.StartSpan();
            if (consumeAction != null && !consumeAction())
            {
                return false;
            }

            var span = scanner.EndSpan();

            CallHandler(new ParserContext(nodeKind, span), (h, c) => h.EndNode(c));

            return true;
        }

        private void EmitTerminal(SyntaxNodeKind nodeKind, TextSpan span)
        {
            handler.EndNode(new ParserContext(nodeKind, span));
        }

        private bool IsSpace(char ch)
        {
            return (ch == ' ' || ch == '\t');
        }

        private bool IsSpaceOrEndOfLine(char ch)
        {
            return Comment() || IsSpaceOrEnd(ch) || ch == '\r';
        }

        private bool IsSpaceOrEnd(char ch)
        {
            return scanner.IsEnd || IsSpace(ch);
        }

        private void CallHandler(ParserContext context, Action<ParserHandler, ParserContext> handlerMethod)
        {
            if (handler != null)
            {
                handlerMethod(handler, context);
            }
        }

        [DebuggerStepThrough]
        private bool Accept(SyntaxNodeKind nodeKind, string acceptedString)
        {
            if (scanner.Peek(acceptedString))
            {
                return Emit(nodeKind, () => scanner.Skip(acceptedString.Length));
            }

            return false;
        }

        [DebuggerStepThrough]
        private bool AcceptAny(SyntaxNodeKind nodeKind, params string[] acceptedStrings)
        {
            return acceptedStrings.Any(s => Accept(nodeKind, s));
        }
    }
}