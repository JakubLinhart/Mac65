using System;
using System.Collections.Generic;

namespace Mac65
{
    public partial class Instruction
    {
        public string Mnemonic { get; private set; }
        public OperandKind OperandKind { get; private set; }
        public byte OpCode { get; private set; }

        private static readonly Dictionary<string, Instruction> SupportedInstructions =
            new Dictionary<string, Instruction>(768);

        private Instruction(string mnemonic, OperandKind operandKind, byte opCode, byte length)
        {
            Mnemonic = mnemonic;
            OperandKind = operandKind;
            OpCode = opCode;
            Length = length;
        }

        private static void AddInstruction(Instruction instruction)
        {
            SupportedInstructions.Add(GetKey(instruction.Mnemonic, instruction.OperandKind), instruction);
        }

        private static string GetKey(string mnemonic, OperandKind operandKind)
        {
            return mnemonic + operandKind;
        }

        public static Instruction Find(string mnemonic, OperandKind operandKind)
        {
            return SupportedInstructions[GetKey(mnemonic, operandKind)];
        }

        public byte Length
        {
            get;
            private set;
        }

        public static bool TryFind(string mnemonic, OperandKind operandKind, out Instruction instruction)
        {
            return SupportedInstructions.TryGetValue(GetKey(mnemonic, operandKind), out instruction);
        }
    }
}