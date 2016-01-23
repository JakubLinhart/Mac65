using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65.Syntax
{
    public enum Token
    {
        DecimalNumber,
        HexNumber,
        CharacterConstant,
        Operator,
        UnaryOperator,
        Identifier,
        Literal
    }
}
