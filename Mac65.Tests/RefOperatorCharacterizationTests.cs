using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mac65.Tests
{
    [TestClass]
    public class RefOperatorCharacterizationTests
    {
        [TestMethod]
        public void When_label_backward_declared_then_REF_evaluates_to_1()
        {
            var image = Generate(@"10 L1
20  LDA #.REF L1");

            Mac65Assert.OpCodes(image, 0xA9, 0x01);
        }

        [TestMethod]
        public void When_label_forward_declared_then_REF_evaluates_to_0()
        {
            var image = Generate(@"10  LDA #.REF L1
20 L1");

            Mac65Assert.OpCodes(image, 0xA9, 0x00);
        }

        [TestMethod]
        public void When_label_referenced_in_same_expression_as_REF_then_REF_evaluates_to_0()
        {
            var image = Generate(@"10  LDA #.REF L1 .OR .DEF L1
20  LDA #.DEF L2 .OR .REF L2");

            Mac65Assert.OpCodes(image, 0xA9, 0x00, 0xA9, 0x00);
        }

        [Ignore]
        [Description("Very strange case, just ignore it until the underlining implementation is more clear.")]
        [TestMethod]
        public void When_label_referenced_by_REF_operator_in_another_expression_then_both_REF_evaluates_to_1()
        {
            var image = Generate(@"10  LDA #.REF L2
20  LDA #.REF L2");

            Mac65Assert.OpCodes(image, 0xA9, 0x01, 0xA9, 0x01);
        }

        [TestMethod]
        public void When_label_referenced_in_expression_on_previous_line_then_REF_evaluates_to_1()
        {
            var image = Generate(@"10  LDA #.DEF L2
20  LDA #.REF L2");

            Mac65Assert.OpCodes(image, 0xA9, 0x00, 0xA9, 0x01);
        }

        [TestMethod]
        public void When_label_referenced_in_expression_on_next_line_then_REF_evaluates_to_0()
        {
            var image = Generate(@"10  LDA #.REF L2
20  LDA #.DEF L2");

            Mac65Assert.OpCodes(image, 0xA9, 0x00, 0xA9, 0x00);
        }

        private MemoryImage Generate(string source)
        {
            var assembler = new Assembler(source);

            return assembler.Build();
        }
    }
}