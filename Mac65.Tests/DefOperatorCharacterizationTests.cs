using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mac65.Tests
{
    [TestClass]
    public class DefOperatorCharacterizationTests
    {
        [TestMethod]
        public void When_label_backward_defined_then_DEF_evaluates_to_1()
        {
            var image = Generate(@"10 L1
20  LDA #.DEF L1");

            Mac65Assert.OpCodes(image, 0xA9, 0x01);
        }

        [TestMethod]
        public void When_label_forward_defined_then_DEF_evaluates_to_1()
        {
            var image = Generate(@"10  LDA #.DEF L1
20 L1");

            Mac65Assert.OpCodes(image, 0xA9, 0x01);
        }

        [TestMethod]
        public void When_label_defined_on_the_same_line_then_DEF_evaluates_to_1()
        {
            var image = Generate(@"10 L1 LDA #.DEF L1");

            Mac65Assert.OpCodes(image, 0xA9, 0x01);
        }

        [TestMethod]
        public void When_label_not_defined_then_DEF_evaluates_to_0()
        {
            var image = Generate(@"10  LDA #.DEF L1");

            Mac65Assert.OpCodes(image, 0xA9, 0x00);
        }

        private MemoryImage Generate(string source)
        {
            var assembler = new Assembler(source);

            return assembler.Build();
        }
    }
}
