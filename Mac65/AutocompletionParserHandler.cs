using System;
using System.Linq;
using Mac65.Syntax;

namespace Mac65
{
    public class AutocompletionParserHandler : ParserHandler
    {
        private readonly int cursorPosition;

        private readonly string[] instructions =
        {
            "ADC", "AND", "ASL", "BCC", "BCS", "BEQ", "BIT", "BMI", "BNE", "BPL",
            "BRK", "BVC", "BVS", "CLC", "CLD", "CLI", "CLV", "CMP", "CPX", "CPY",
            "DEC", "DEX", "DEY", "EOR", "INC", "INX", "INY", "JMP", "JSR", "LDA",
            "LDX", "LDY", "LSR", "NOP", "ORA", "PHA", "PHP", "PLA", "PLP", "ROL",
            "ROR", "RTI", "RTS", "SBC", "SEC", "SED", "SEI", "STA", "STX", "STY",
            "TAX", "TAY", "TSX", "TXA", "TXS", "TYA"
        };

        private string suggestion;

        public AutocompletionParserHandler(int cursorPosition)
        {
            this.cursorPosition = cursorPosition;
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
            }
        }

        private void EndLineNumber(ParserContext context)
        {
            if (context.Span.End <= cursorPosition)
                suggestion = instructions.First();
        }

        private void EndMnemonic(ParserContext context)
        {
            if (context.Span.End >= cursorPosition)
            {
                if (context.Span.Length == 0)
                    suggestion = instructions.First();

                var enteredMnemonic = context.Span.Text.Substring(0,
                    (context.Span.Length >= 3) ? 3 : context.Span.Length);

                suggestion = instructions.FirstOrDefault(i => i.StartsWith(enteredMnemonic));
            }
        }

        public string GetSuggestion()
        {
            return suggestion;
        }
    }
}