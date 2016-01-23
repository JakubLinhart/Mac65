using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65.Syntax
{
    public abstract class ParserHandler
    {
        public virtual void StartNode(ParserContext context) { }
        public virtual void EndNode(ParserContext context) { }
    }
}
