using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mac65.Syntax;

namespace Mac65
{
    public class Assembler
    {
        private readonly string source;

        public Assembler(string source)
        {
            this.source = source;
        }

        public MemoryImage Build()
        {
            var labelStore = new LabelStore();
            var macroStore = new MacroStore();

            FirstPass(labelStore, macroStore);
            return SecondPass(labelStore, macroStore);
        }

        private void FirstPass(LabelStore labelStore, MacroStore macroStore)
        {
            var instructionResolver = new InstructionResolver(labelStore, macroStore);
            var parser = new Parser(instructionResolver);

            parser.Parse(source);
        }

        private MemoryImage SecondPass(LabelStore labelStore, MacroStore macroStore)
        {
            labelStore.ClearReferencedLabels();
            var instructionResolver = new InstructionResolver(labelStore, macroStore);
            var parser = new Parser(instructionResolver);

            parser.Parse(source);

            return instructionResolver.CreateMemoryImage();
        }
    }
}
