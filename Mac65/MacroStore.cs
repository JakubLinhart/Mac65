using System.Collections.Generic;

namespace Mac65
{
    public class MacroStore
    {
        private readonly Dictionary<string, Macro> macros = new Dictionary<string, Macro>(); 

        public Macro GetMacro(string name)
        {
            return macros[name];
        }

        public bool TryGetMacro(string name, out Macro macro)
        {
            return macros.TryGetValue(name, out macro);
        }

        public void Add(Macro macro)
        {
            macros[macro.Name] = macro;
        }
    }
}