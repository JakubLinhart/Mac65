using System;
using System.Collections.Generic;

namespace Mac65
{
    public partial class Instruction
    {
        static Instruction()
        {
            AddInstruction(new Instruction("ADC", OperandKind.Immediate, 0x69, 2));
            AddInstruction(new Instruction("ADC", OperandKind.ZeroPage, 0x65, 2));
            AddInstruction(new Instruction("ADC", OperandKind.ZeroPageX, 0x75, 2));
            AddInstruction(new Instruction("ADC", OperandKind.Absolute, 0x6D, 3));
            AddInstruction(new Instruction("ADC", OperandKind.AbsoluteX, 0x7D, 3));
            AddInstruction(new Instruction("ADC", OperandKind.AbsoluteY, 0x79, 3));
            AddInstruction(new Instruction("ADC", OperandKind.IndirectX, 0x61, 2));
            AddInstruction(new Instruction("ADC", OperandKind.IndirectY, 0x71, 2));

            AddInstruction(new Instruction("AND", OperandKind.Immediate, 0x29, 2));
            AddInstruction(new Instruction("AND", OperandKind.ZeroPage, 0x25, 2));
            AddInstruction(new Instruction("AND", OperandKind.ZeroPageX, 0x35, 2));
            AddInstruction(new Instruction("AND", OperandKind.Absolute, 0x2D, 3));
            AddInstruction(new Instruction("AND", OperandKind.AbsoluteX, 0x3D, 3));
            AddInstruction(new Instruction("AND", OperandKind.AbsoluteY, 0x39, 3));
            AddInstruction(new Instruction("AND", OperandKind.IndirectX, 0x21, 2));
            AddInstruction(new Instruction("AND", OperandKind.IndirectY, 0x31, 2));

            AddInstruction(new Instruction("ASL", OperandKind.Accumulator, 0x0A, 1));
            AddInstruction(new Instruction("ASL", OperandKind.ZeroPage, 0x06, 2));
            AddInstruction(new Instruction("ASL", OperandKind.ZeroPageX, 0x16, 2));
            AddInstruction(new Instruction("ASL", OperandKind.Absolute, 0x0E, 3));
            AddInstruction(new Instruction("ASL", OperandKind.AbsoluteX, 0x1E, 3));

            AddInstruction(new Instruction("BIT", OperandKind.ZeroPage, 0x24, 2));
            AddInstruction(new Instruction("BIT", OperandKind.Absolute, 0x2C, 3));

            AddInstruction(new Instruction("BPL", OperandKind.Relative, 0x10, 2));
            AddInstruction(new Instruction("BMI", OperandKind.Relative, 0x30, 2));
            AddInstruction(new Instruction("BVC", OperandKind.Relative, 0x50, 2));
            AddInstruction(new Instruction("BVS", OperandKind.Relative, 0x70, 2));
            AddInstruction(new Instruction("BCC", OperandKind.Relative, 0x90, 2));
            AddInstruction(new Instruction("BCS", OperandKind.Relative, 0xB0, 2));
            AddInstruction(new Instruction("BNE", OperandKind.Relative, 0xD0, 2));
            AddInstruction(new Instruction("BEQ", OperandKind.Relative, 0xF0, 2));

            AddInstruction(new Instruction("BRK", OperandKind.Implied, 0x00, 1));

            AddInstruction(new Instruction("CMP", OperandKind.Immediate, 0xC9, 2));
            AddInstruction(new Instruction("CMP", OperandKind.ZeroPage, 0xC5, 2));
            AddInstruction(new Instruction("CMP", OperandKind.ZeroPageX, 0xD5, 2));
            AddInstruction(new Instruction("CMP", OperandKind.Absolute, 0xCD, 3));
            AddInstruction(new Instruction("CMP", OperandKind.AbsoluteX, 0xDD, 3));
            AddInstruction(new Instruction("CMP", OperandKind.AbsoluteY, 0xD9, 3));
            AddInstruction(new Instruction("CMP", OperandKind.IndirectX, 0xC1, 2));
            AddInstruction(new Instruction("CMP", OperandKind.IndirectY, 0xD1, 2));

            AddInstruction(new Instruction("CPX", OperandKind.Immediate, 0xE0, 2));
            AddInstruction(new Instruction("CPX", OperandKind.ZeroPage, 0xE4, 2));
            AddInstruction(new Instruction("CPX", OperandKind.Absolute, 0xEC, 3));

            AddInstruction(new Instruction("CPY", OperandKind.Immediate, 0xC0, 2));
            AddInstruction(new Instruction("CPY", OperandKind.ZeroPage, 0xC4, 2));
            AddInstruction(new Instruction("CPY", OperandKind.Absolute, 0xCC, 3));

            AddInstruction(new Instruction("DEC", OperandKind.ZeroPage, 0xC6, 2));
            AddInstruction(new Instruction("DEC", OperandKind.ZeroPageX, 0xD6, 2));
            AddInstruction(new Instruction("DEC", OperandKind.Absolute, 0xCE, 3));
            AddInstruction(new Instruction("DEC", OperandKind.AbsoluteX, 0xDE, 3));

            AddInstruction(new Instruction("EOR", OperandKind.Immediate, 0x49, 2));
            AddInstruction(new Instruction("EOR", OperandKind.ZeroPage, 0x45, 2));
            AddInstruction(new Instruction("EOR", OperandKind.ZeroPageX, 0x55, 2));
            AddInstruction(new Instruction("EOR", OperandKind.Absolute, 0x4D, 3));
            AddInstruction(new Instruction("EOR", OperandKind.AbsoluteX, 0x5D, 3));
            AddInstruction(new Instruction("EOR", OperandKind.AbsoluteY, 0x59, 3));
            AddInstruction(new Instruction("EOR", OperandKind.IndirectX, 0x41, 2));
            AddInstruction(new Instruction("EOR", OperandKind.IndirectY, 0x51, 2));

            AddInstruction(new Instruction("CLC", OperandKind.Implied, 0x18, 1));
            AddInstruction(new Instruction("SEC", OperandKind.Implied, 0x38, 1));
            AddInstruction(new Instruction("CLI", OperandKind.Implied, 0x58, 1));
            AddInstruction(new Instruction("SEI", OperandKind.Implied, 0x78, 1));
            AddInstruction(new Instruction("CLV", OperandKind.Implied, 0xB8, 1));
            AddInstruction(new Instruction("CLD", OperandKind.Implied, 0xD8, 1));
            AddInstruction(new Instruction("SED", OperandKind.Implied, 0xF8, 1));

            AddInstruction(new Instruction("INC", OperandKind.ZeroPage, 0xE6, 2));
            AddInstruction(new Instruction("INC", OperandKind.ZeroPageX, 0xF6, 2));
            AddInstruction(new Instruction("INC", OperandKind.Absolute, 0xEE, 3));
            AddInstruction(new Instruction("INC", OperandKind.AbsoluteX, 0xFE, 3));

            AddInstruction(new Instruction("JMP", OperandKind.Absolute, 0x4C, 3));
            AddInstruction(new Instruction("JMP", OperandKind.Indirect, 0x6C, 3));

            AddInstruction(new Instruction("JSR", OperandKind.Absolute, 0x20, 3));

            AddInstruction(new Instruction("LDA", OperandKind.Immediate, 0xA9, 2));
            AddInstruction(new Instruction("LDA", OperandKind.ZeroPage, 0xA5, 2));
            AddInstruction(new Instruction("LDA", OperandKind.ZeroPageX, 0xB5, 2));
            AddInstruction(new Instruction("LDA", OperandKind.Absolute, 0xAD, 3));
            AddInstruction(new Instruction("LDA", OperandKind.AbsoluteX, 0xBD, 3));
            AddInstruction(new Instruction("LDA", OperandKind.AbsoluteY, 0xB9, 3));
            AddInstruction(new Instruction("LDA", OperandKind.IndirectX, 0xA1, 2));
            AddInstruction(new Instruction("LDA", OperandKind.IndirectY, 0xB1, 2));

            AddInstruction(new Instruction("LDX", OperandKind.Immediate, 0xA2, 2));
            AddInstruction(new Instruction("LDX", OperandKind.ZeroPage, 0xA6, 2));
            AddInstruction(new Instruction("LDX", OperandKind.ZeroPageY, 0xB6, 2));
            AddInstruction(new Instruction("LDX", OperandKind.Absolute, 0xAE, 3));
            AddInstruction(new Instruction("LDX", OperandKind.AbsoluteY, 0xBE, 3));

            AddInstruction(new Instruction("LDY", OperandKind.Immediate, 0xA0, 2));
            AddInstruction(new Instruction("LDY", OperandKind.ZeroPage, 0xA4, 2));
            AddInstruction(new Instruction("LDY", OperandKind.ZeroPageX, 0xB4, 2));
            AddInstruction(new Instruction("LDY", OperandKind.Absolute, 0xAC, 3));
            AddInstruction(new Instruction("LDY", OperandKind.AbsoluteX, 0xBC, 3));

            AddInstruction(new Instruction("LSR", OperandKind.Accumulator, 0x4A, 1));
            AddInstruction(new Instruction("LSR", OperandKind.ZeroPage, 0x46, 2));
            AddInstruction(new Instruction("LSR", OperandKind.ZeroPageX, 0x56, 2));
            AddInstruction(new Instruction("LSR", OperandKind.Absolute, 0x4E, 3));
            AddInstruction(new Instruction("LSR", OperandKind.AbsoluteX, 0x5E, 3));

            AddInstruction(new Instruction("NOP", OperandKind.Implied, 0xEA, 1));

            AddInstruction(new Instruction("ORA", OperandKind.Immediate, 0x09, 2));
            AddInstruction(new Instruction("ORA", OperandKind.ZeroPage, 0x05, 2));
            AddInstruction(new Instruction("ORA", OperandKind.ZeroPageX, 0x15, 2));
            AddInstruction(new Instruction("ORA", OperandKind.Absolute, 0x0D, 3));
            AddInstruction(new Instruction("ORA", OperandKind.AbsoluteX, 0x1D, 3));
            AddInstruction(new Instruction("ORA", OperandKind.AbsoluteY, 0x19, 3));
            AddInstruction(new Instruction("ORA", OperandKind.IndirectX, 0x01, 2));
            AddInstruction(new Instruction("ORA", OperandKind.IndirectY, 0x11, 2));

            AddInstruction(new Instruction("TAX", OperandKind.Implied, 0xAA, 1));
            AddInstruction(new Instruction("TXA", OperandKind.Implied, 0x8A, 1));
            AddInstruction(new Instruction("DEX", OperandKind.Implied, 0xCA, 1));
            AddInstruction(new Instruction("INX", OperandKind.Implied, 0xE8, 1));
            AddInstruction(new Instruction("TAY", OperandKind.Implied, 0xA8, 1));
            AddInstruction(new Instruction("TYA", OperandKind.Implied, 0x98, 1));
            AddInstruction(new Instruction("DEY", OperandKind.Implied, 0x88, 1));
            AddInstruction(new Instruction("INY", OperandKind.Implied, 0xCA, 1));

            AddInstruction(new Instruction("ROL", OperandKind.Accumulator, 0x2A, 1));
            AddInstruction(new Instruction("ROL", OperandKind.ZeroPage, 0x26, 2));
            AddInstruction(new Instruction("ROL", OperandKind.ZeroPageX, 0x36, 2));
            AddInstruction(new Instruction("ROL", OperandKind.Absolute, 0x2E, 3));
            AddInstruction(new Instruction("ROL", OperandKind.AbsoluteX, 0x3E, 3));

            AddInstruction(new Instruction("ROR", OperandKind.Accumulator, 0x6A, 1));
            AddInstruction(new Instruction("ROR", OperandKind.ZeroPage, 0x66, 2));
            AddInstruction(new Instruction("ROR", OperandKind.ZeroPageX, 0x76, 2));
            AddInstruction(new Instruction("ROR", OperandKind.Absolute, 0x6E, 3));
            AddInstruction(new Instruction("ROR", OperandKind.AbsoluteX, 0x7E, 3));

            AddInstruction(new Instruction("RTI", OperandKind.Implied, 0x40, 1));
            AddInstruction(new Instruction("RTS", OperandKind.Implied, 0x60, 1));

            AddInstruction(new Instruction("SBC", OperandKind.Immediate, 0xE9, 2));
            AddInstruction(new Instruction("SBC", OperandKind.ZeroPage, 0xE5, 2));
            AddInstruction(new Instruction("SBC", OperandKind.ZeroPageX, 0xF5, 2));
            AddInstruction(new Instruction("SBC", OperandKind.Absolute, 0xED, 3));
            AddInstruction(new Instruction("SBC", OperandKind.AbsoluteX, 0xFD, 3));
            AddInstruction(new Instruction("SBC", OperandKind.AbsoluteY, 0xF9, 3));
            AddInstruction(new Instruction("SBC", OperandKind.IndirectX, 0xE1, 2));
            AddInstruction(new Instruction("SBC", OperandKind.IndirectY, 0xF1, 2));

            AddInstruction(new Instruction("STA", OperandKind.ZeroPage, 0x85, 2));
            AddInstruction(new Instruction("STA", OperandKind.ZeroPageX, 0x95, 2));
            AddInstruction(new Instruction("STA", OperandKind.Absolute, 0x8D, 3));
            AddInstruction(new Instruction("STA", OperandKind.AbsoluteX, 0x9D, 3));
            AddInstruction(new Instruction("STA", OperandKind.AbsoluteY, 0x99, 3));
            AddInstruction(new Instruction("STA", OperandKind.IndirectX, 0x81, 2));
            AddInstruction(new Instruction("STA", OperandKind.IndirectY, 0x91, 2));

            AddInstruction(new Instruction("TXS", OperandKind.Implied, 0x9A, 1));
            AddInstruction(new Instruction("TSX", OperandKind.Implied, 0xBA, 1));
            AddInstruction(new Instruction("PHA", OperandKind.Implied, 0x48, 1));
            AddInstruction(new Instruction("PLA", OperandKind.Implied, 0x68, 1));
            AddInstruction(new Instruction("PHP", OperandKind.Implied, 0x08, 1));
            AddInstruction(new Instruction("PLP", OperandKind.Implied, 0x28, 1));

            AddInstruction(new Instruction("STX", OperandKind.ZeroPage, 0x86, 2));
            AddInstruction(new Instruction("STX", OperandKind.ZeroPageY, 0x96, 2));
            AddInstruction(new Instruction("STX", OperandKind.Absolute, 0x8E, 3));

            AddInstruction(new Instruction("STY", OperandKind.ZeroPage, 0x84, 2));
            AddInstruction(new Instruction("STY", OperandKind.ZeroPageY, 0x94, 2));
            AddInstruction(new Instruction("STY", OperandKind.Absolute, 0x8C, 3));
        }
    }
}