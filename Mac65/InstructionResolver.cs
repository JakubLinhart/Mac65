using System;
using System.Collections.Generic;
using Mac65.Syntax;

namespace Mac65
{
    public class InstructionResolver : ParserHandler
    {
        private readonly List<MemoryChunk> chunks = new List<MemoryChunk>();
        private readonly ExpressionResolver expressionResolver;
        private readonly MacroStore macroStore;
        private readonly LabelStore labelStore;
        private readonly List<byte> opcodes = new List<byte>();
        private readonly Queue<int> directiveOperands = new Queue<int>();
        private ushort currentChunkStartAddress;
        private Macro currentMacro;
        private string currentMacroName;
        private string directiveName;
        private IndexerKind? indexer;
        private string mnemonic;
        private OperandKind operandKind;
        private int operandValue;
        private string labelName;
        private int lineNumber;
        private int skippingIfsCount;
        private readonly Stack<int> ifConditionValues = new Stack<int>();

        public InstructionResolver(LabelStore labelStore, MacroStore macroStore)
        {
            this.macroStore = macroStore;
            this.labelStore = labelStore;
            expressionResolver = new ExpressionResolver(labelStore);
        }

        public override void StartNode(ParserContext context)
        {
            expressionResolver.StartNode(context);

            switch (context.NodeKind)
            {
                case SyntaxNodeKind.Line:
                    lineNumber++;
                    indexer = null;
                    directiveName = null;
                    labelName = null;
                    mnemonic = null;
                    break;
                case SyntaxNodeKind.Instruction:
                    if (labelName != null)
                    {
                        AddLabel(new Label(labelName, currentChunkStartAddress + opcodes.Count));
                        labelName = null;
                    }
                    break;
            }
        }

        public override void EndNode(ParserContext context)
        {
            expressionResolver.EndNode(context);

            switch (context.NodeKind)
            {
                case SyntaxNodeKind.Label:
                    labelName = context.Span.Text;
                    break;

                case SyntaxNodeKind.MacroName:
                    currentMacroName = context.Span.Text;
                    break;

                case SyntaxNodeKind.Macro:
                    currentMacro = new Macro(currentMacroName);
                    break;

                case SyntaxNodeKind.DirectiveName:
                    directiveName = context.Span.Text;
                    if (labelName != null && directiveName != "=")
                    {
                        AddLabel(new Label(labelName, currentChunkStartAddress + opcodes.Count));
                        labelName = null;
                    }
                    break;

                case SyntaxNodeKind.Directive:
                    ProcessDriective();
                    break;

                case SyntaxNodeKind.Instruction:
                    ProcessInstruction();
                    break;

                case SyntaxNodeKind.ImpliedOperand:
                    operandKind = OperandKind.Implied;
                    break;

                case SyntaxNodeKind.AbsoluteOperand:
                    ProcessAbsoluteOperand();
                    break;

                case SyntaxNodeKind.ImmediateOperand:
                    operandValue = (byte) expressionResolver.Evaluate();
                    operandKind = OperandKind.Immediate;
                    break;

                case SyntaxNodeKind.IndirectOperand:
                    ProcessIndirectOperand();
                    break;

                case SyntaxNodeKind.OperandIndexer:
                    ProcessOperandIndexer(context.Span.Text);
                    break;

                case SyntaxNodeKind.Mnemonic:
                    mnemonic = context.Span.Text;
                    break;

                case SyntaxNodeKind.DirectiveOperands:
                    directiveOperands.Enqueue(expressionResolver.Evaluate());
                    break;

                case SyntaxNodeKind.Literal:
                    foreach (var b in AtasciiEncoding.Instance.GetBytes(context.Span.Text))
                    {
                        directiveOperands.Enqueue(b);
                    }
                    break;

                case SyntaxNodeKind.Line:
                    if (labelName != null)
                    {
                        AddLabel(new Label(labelName, currentChunkStartAddress + opcodes.Count));
                        labelName = null;
                    }
                    break;

            }
        }

        private void AddLabel(Label label)
        {
            labelStore.AddLabel(label);
            labelStore.AddReferencedLabel(labelName, lineNumber);
        }

        private void ProcessIndirectOperand()
        {
            operandValue = expressionResolver.Evaluate();
            if (indexer.HasValue)
            {
                switch (indexer.Value)
                {
                    case IndexerKind.X:
                        operandKind = OperandKind.IndirectX;
                        break;
                    case IndexerKind.Y:
                        operandKind = OperandKind.IndirectY;
                        break;
                }
            }
            else
            {
                operandKind = OperandKind.Indirect;
            }
        }

        private void ProcessAbsoluteOperand()
        {
            operandValue = expressionResolver.Evaluate();
            if (indexer.HasValue)
            {
                switch (indexer.Value)
                {
                    case IndexerKind.X:
                        operandKind = OperandKind.AbsoluteX;
                        break;
                    case IndexerKind.Y:
                        operandKind = OperandKind.AbsoluteY;
                        break;
                }
            }
            else
            {
                operandKind = OperandKind.Absolute;
            }
        }

        private void ProcessDriective()
        {
            switch (directiveName)
            {
                case ".BYTE":
                    while (directiveOperands.Count > 0)
                    {
                        var directiveOperand = directiveOperands.Dequeue();
                        AddOpCode((byte) (directiveOperand & 0xFF));
                    }
                    break;
                case ".WORD":
                    while (directiveOperands.Count > 0)
                    {
                        var directiveOperand = directiveOperands.Dequeue();
                        AddOpCode((byte)( directiveOperand & 0xFF ));
                        AddOpCode((byte)( ( directiveOperand & 0xFF00 ) >> 8 ));
                    }
                    break;
                case ".DBYTE":
                    while (directiveOperands.Count > 0)
                    {
                        var directiveOperand = directiveOperands.Dequeue();
                        AddOpCode((byte)( ( directiveOperand & 0xFF00 ) >> 8 ));
                        AddOpCode((byte)( directiveOperand & 0xFF ));
                    }
                    break;
                case ".IF":
                    var conditionValue = directiveOperands.Dequeue();
                    ifConditionValues.Push(conditionValue);
                    if (conditionValue == 0)
                    {
                        skippingIfsCount++;
                    }

                    break;
                case ".ELSE":
                    var conditionValue2 = ifConditionValues.Pop();
                    if (conditionValue2 == 0)
                    {
                        ifConditionValues.Push(1);
                        skippingIfsCount--;
                    }
                    else
                    {
                        ifConditionValues.Push(0);
                        skippingIfsCount++;
                    }
                    break;
                case ".ENDIF":
                    if (ifConditionValues.Pop() == 0)
                        skippingIfsCount--;
                    if (skippingIfsCount < 0)
                        throw new InvalidOperationException("unpaired .ENDIF");
                    break;
                case ".ENDM":
                    macroStore.Add(currentMacro);
                    currentMacro = null;
                    currentMacroName = null;
                    break;
                case ".DS":
                    var dsLength = directiveOperands.Dequeue();
                    SetCurrentOrigin(opcodes.Count + currentChunkStartAddress + dsLength);
                    break;
                case "*=":
                    SetCurrentOrigin(directiveOperands.Dequeue());
                    break;
                case "=":
                    AddLabel(new Label(labelName, directiveOperands.Dequeue()));
                    labelName = null;
                    break;
            }

            if (directiveOperands.Count > 0)
            {
                throw new InvalidOperationException("Unprocessed directive operands");
            }
        }

        private void AddOpCode(byte opCode)
        {
            if (skippingIfsCount == 0)
                opcodes.Add(opCode);
        }

        private void SetCurrentOrigin(int newOrigin)
        {
            if (opcodes.Count > 0)
            {
                chunks.Add(new MemoryChunk(currentChunkStartAddress, opcodes));
                opcodes.Clear();
            }

            currentChunkStartAddress = (ushort)(newOrigin & 0xFFFF);
        }

        private void ProcessInstruction()
        {
            Instruction instruction;
            if (Instruction.TryFind(mnemonic, operandKind, out instruction))
            {
                if (currentMacro != null)
                {
                    currentMacro.AddInstruction(instruction);
                }
                else
                {
                    OnInstructionResolved(instruction);
                }
            }
            else
            {
                ApplyMacro(mnemonic);
            }
        }

        private void ProcessOperandIndexer(string indexerString)
        {
            switch (indexerString)
            {
                case "X":
                    indexer = IndexerKind.X;
                    break;
                case "Y":
                    indexer = IndexerKind.Y;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected indexer '" + indexerString + "'");
            }
        }

        private void ApplyMacro(string name)
        {
            Macro macro;

            if (macroStore.TryGetMacro(name, out macro))
            {
                macro.Apply(OnInstructionResolved);
            }
        }

        private void OnInstructionResolved(Instruction instruction)
        {
            instruction = GetZeroPageAlternative(instruction) ?? instruction;

            var operandLo = (byte) (operandValue & 0xFF);
            var operandHi = (byte) ((operandValue & 0xFF00) >> 8);
            AddOpCode(instruction.OpCode);
            switch (instruction.Length)
            {
                case 1:
                    break;
                case 2:
                    AddOpCode(operandLo);
                    break;
                case 3:
                    AddOpCode(operandLo);
                    AddOpCode(operandHi);
                    break;
            }
        }

        private Instruction GetZeroPageAlternative(Instruction instruction)
        {
            Instruction zeroPageAlternative = null;

            if ((operandValue & 0xFF00) == 0)
            {
                switch (instruction.OperandKind)
                {
                    case OperandKind.Absolute:
                        Instruction.TryFind(instruction.Mnemonic, OperandKind.ZeroPage, out zeroPageAlternative);
                        break;
                    case OperandKind.AbsoluteX:
                        Instruction.TryFind(instruction.Mnemonic, OperandKind.ZeroPageX, out zeroPageAlternative);
                        break;
                    case OperandKind.AbsoluteY:
                        Instruction.TryFind(instruction.Mnemonic, OperandKind.ZeroPageY, out zeroPageAlternative);
                        break;
                }
            }

            return zeroPageAlternative;
        }

        public MemoryImage CreateMemoryImage()
        {
            chunks.Add(new MemoryChunk(currentChunkStartAddress, opcodes));

            return new MemoryImage(chunks);
        }
    }
}