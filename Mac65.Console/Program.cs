using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembler = new Assembler(File.ReadAllText(args[0]));
            var memoryImage = assembler.Build();
            XexFile.Write(memoryImage, args[1]);
        }
    }
}
