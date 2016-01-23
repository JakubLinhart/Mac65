using System;
using System.Collections.Generic;

namespace Mac65
{
    public class Macro
    {
        private readonly string name;
        private readonly List<Instruction> instructions = new List<Instruction>();

        public Macro(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }

        public void AddInstruction(Instruction instruction)
        {
            instructions.Add(instruction);
        }

        public void Apply(Action<Instruction> onInstructionResolvedAction)
        {
            foreach (var instruction in instructions)
            {
                onInstructionResolvedAction(instruction);
            }
        }
    }
}