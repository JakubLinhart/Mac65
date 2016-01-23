using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65
{
    public enum OperandKind
    {
        Implied,
        Immediate,
        ZeroPage,
        ZeroPageX,
        ZeroPageY,
        Accumulator,
        Absolute,
        AbsoluteX,
        AbsoluteY,
        Indirect,
        IndirectX,
        IndirectY,
        Relative,
    }
}
