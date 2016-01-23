using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mac65.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mac65.Tests
{
    [TestClass]
    public class AssemblerTests
    {
        [TestMethod]
        public void Can_generate_instruction_with_implied_operand()
        {
            MemoryImage image = Generate("10  NOP");

            Mac65Assert.OpCodes(image, 0xEA);
        }

        [TestMethod]
        public void Can_generate_instruction_with_immediate_operand()
        {
            MemoryImage image = Generate("10  LDA #15");

            Mac65Assert.OpCodes(image, 0xA9, 0x0F);
        }

        [TestMethod]
        public void Can_generate_instruction_with_absolute_operand()
        {
            MemoryImage image = Generate("10  LDA $1234");

            Mac65Assert.OpCodes(image, 0xAD, 0x34, 0x12);
        }

        [TestMethod]
        public void Can_generate_instruction_with_zeropage_operand()
        {
            MemoryImage image = Generate("10  LDA $12");

            Mac65Assert.OpCodes(image, 0xA5, 0x12);
        }

        [TestMethod]
        public void Can_generate_instruction_with_absoluteX_operand()
        {
            var image = Generate("10  LDA $1234,X");

            Mac65Assert.OpCodes(image, 0xBD, 0x34, 0x12);
        }

        [TestMethod]
        public void Can_generate_instruction_with_zeropageX_operand()
        {
            MemoryImage image = Generate("10  LDA $65,X");

            Mac65Assert.OpCodes(image, 0xB5, 0x65);
        }

        [TestMethod]
        public void Can_generate_instruction_with_zeropageY_operand()
        {
            MemoryImage image = Generate("10  LDX $65,Y");

            Mac65Assert.OpCodes(image, 0xB6, 0x65);
        }

        [TestMethod]
        public void Can_generate_instruction_with_absoluteY_operand()
        {
            var image = Generate("10  LDA $1234,Y");

            Mac65Assert.OpCodes(image, 0xB9, 0x34, 0x12);
        }

        [TestMethod]
        public void Can_distinguish_between_different_addressing_modes()
        {
            var image = Generate(@"10  LDA $1234
20  LDA $4321,X
30  LDA $9876,Y
40  LDA ($54,X)
50  LDA ($45),Y");

            Mac65Assert.OpCodes(image, 
                0xAD, 0x34, 0x12, 
                0xBD, 0x21, 0x43,
                0XB9, 0x76, 0x98,
                0xA1, 0x54,
                0xB1, 0x45);
        }

        [TestMethod]
        public void Can_generate_instruction_with_indirect_operand()
        {
            var image = Generate("10  JMP ($1234)");

            Mac65Assert.OpCodes(image, 0x6C, 0x34, 0x12);
        }

        [TestMethod]
        public void Can_gernerate_instruction_with_indirectX_operand()
        {
            var image = Generate("10  LDA ($12,X)");

            Mac65Assert.OpCodes(image, 0xA1, 0x12);
        }

        [TestMethod]
        public void Can_gernerate_instruction_with_indirectY_operand()
        {
            var image = Generate("10  LDA ($21),Y");

            Mac65Assert.OpCodes(image, 0xB1, 0x21);
        }

        [TestMethod]
        public void Can_generate_instruction_referencing_backward_label()
        {
            MemoryImage image = Generate(@"10 LABEL
20  JMP LABEL");

            Mac65Assert.OpCodes(image, 0x4C, 0x00, 0x00);
        }

        [TestMethod]
        [Ignore]
        public void Refuses_generate_forward_label_with_different_value_in_second_pass()
        {
            MemoryImage image = Generate(@"10  LDA L1
20 L1");

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Can_generate_instruction_referencing_forward_label()
        {
            MemoryImage image = Generate(@"10  JMP LABEL
20 LABEL");

            Mac65Assert.OpCodes(image, 0x4C, 0x03, 0x00);
        }

        [TestMethod]
        public void Can_generate_instruction_for_backward_macro()
        {
            MemoryImage image = Generate(@"10 .MACRO TEST
20  NOP
25  NOP
30 .ENDM
40  TEST
45  NOP
50  TEST");

            Mac65Assert.OpCodes(image, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA);
        }

        [TestMethod]
        public void Can_generate_instruction_for_forward_macro()
        {
            MemoryImage image = Generate(@"10  TEST
12  NOP
15  TEST
20 .MACRO TEST
30  NOP
30  NOP
40 .ENDM");

            Mac65Assert.OpCodes(image, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA);
        }

        [TestMethod]
        public void Can_generate_instruction_for_empty_macro()
        {
            MemoryImage image = Generate(@"10 .MACRO TEST
20 .ENDM");

            Mac65Assert.OpCodes(image, new byte[] { });
        }

        [TestMethod]
        public void Can_generate_instruction_for_expression_in_operarnd()
        {
            var image = Generate(@"10  LDA #1+1");

            Mac65Assert.OpCodes(image, 0xA9, 0x02);
        }

        [TestMethod]
        public void Can_set_current_origin()
        {
            var image = Generate(@"10  *= $1000
20  LDA #1");

            Mac65Assert.OpCodes(image, new MemoryChunk(0x1000, new byte[] { 0xA9, 0x01 }));
        }

        [TestMethod]
        public void Can_change_current_origin()
        {
            var image = Generate(@"10  LDA #1
20  *= $1000
30  LDA #1");
          
            Mac65Assert.OpCodes(image, new MemoryChunk(0, new byte[] { 0xA9, 0x01 }),
                new MemoryChunk(0x1000, new byte[] { 0xA9, 0x01 }));
        }

        [TestMethod]
        public void Can_change_current_origin_with_DS_directive()
        {
            var image = Generate(@"10  *= $1000
20  .DS $10
30  LDA #1");

            Mac65Assert.OpCodes(image, new MemoryChunk(0x1010, new byte[] { 0xA9, 0x01 }));
        }

        [TestMethod]
        public void Can_change_current_origin_with_DS_directive_with_negative_operand()
        {
            var image = Generate(@"10  *= $1010
20  .DS -$10
30  LDA #1");

            Mac65Assert.OpCodes(image, new MemoryChunk(0x1000, new byte[] { 0xA9, 0x01 }));
        }

        [TestMethod]
        public void Can_change_current_origin_to_negatvie_values()
        {
            var image = Generate(@"10  *= 0
20  .DS -1
30  LDA #1");

            Mac65Assert.OpCodes(image, new MemoryChunk(0xFFFF, new byte[] { 0xA9, 0x01 }));
        }

        [TestMethod]
        public void Can_generate_BYTE()
        {
            Mac65Assert.OpCodes(Generate("10 .BYTE 1"), 1);
            Mac65Assert.OpCodes(Generate("10 .BYTE -1"), 0xFF);
            Mac65Assert.OpCodes(Generate("10 .BYTE 1,2"), 1, 2);
            Mac65Assert.OpCodes(Generate("10 .BYTE 1,2,3"), 1, 2, 3);
            Mac65Assert.OpCodes(Generate("10 .BYTE [1+2]*3"), 9);
            Mac65Assert.OpCodes(Generate("10 .BYTE \"ABC\""), 0x41, 0x42, 0x43);
            Mac65Assert.OpCodes(Generate("10 .BYTE \"ABC\",\"ABC\""), 0x41, 0x42, 0x43, 0x41, 0x42, 0x43);
        }

        [TestMethod]
        public void Can_generate_WORD()
        {
            Mac65Assert.OpCodes(Generate("10 .WORD 1"), 1, 0);
            Mac65Assert.OpCodes(Generate("10 .WORD -1"), 0xFF, 0xFF);
            Mac65Assert.OpCodes(Generate("10 .WORD 1,2"), 1, 0, 2, 0);
            Mac65Assert.OpCodes(Generate("10 .WORD 1,2,3"), 1, 0, 2, 0, 3, 0);
            Mac65Assert.OpCodes(Generate("10 .WORD [1+2]*3"), 9, 0);
        }

        [TestMethod]
        public void Can_generate_DBYTE()
        {
            Mac65Assert.OpCodes(Generate("10 .DBYTE 1"), 0, 1);
            Mac65Assert.OpCodes(Generate("10 .DBYTE -1"), 0xFF, 0xFF);
            Mac65Assert.OpCodes(Generate("10 .DBYTE 1,2"), 0, 1, 0, 2);
            Mac65Assert.OpCodes(Generate("10 .DBYTE 1,2,3"), 0, 1, 0, 2, 0, 3);
            Mac65Assert.OpCodes(Generate("10 .DBYTE [1+2]*3"), 0, 9);
        }

        [TestMethod]
        [Ignore]
        public void Refuse_generate_BYTE_with_operand_higher_than_255()
        {
            Generate("10 .BYTE 256");
        }

        [TestMethod]
        public void Can_generate_IF_directive_when_condition_is_1()
        {
            var result = Generate(@"10 .IF 1
20  NOP
30 .ENDIF");

            Mac65Assert.OpCodes(result, 0xEA);
        }

        [TestMethod]
        public void Can_generate_IF_directive_when_condition_is_100()
        {
            var result = Generate(@"10 .IF 100
20  NOP
30 .ENDIF");

            Mac65Assert.OpCodes(result, 0xEA);
        }

        [TestMethod]
        public void Can_generate_IF_directive_when_condition_is_0()
        {
            var result = Generate(@"10 .IF 0
20  NOP
30 .ENDIF");

            Mac65Assert.OpCodes(result, new byte[] { });
        }

        [TestMethod]
        public void Can_generate_instruction_before_and_after_IF_directive_when_condition_is_0()
        {
            var result = Generate(@"10  LDA #1
20 .IF 0
30  LDA #2
40 .ENDIF
50  LDA #3");

            Mac65Assert.OpCodes(result, 0xA9, 0x01, 0xA9, 0x03);
        }

        [TestMethod]
        public void Can_generate_instruction_before_and_after_IF_directive_when_condition_is_1()
        {
            var result = Generate(@"10  LDA #1
20 .IF 0
30  LDA #2
40 .ENDIF
50  LDA #3");

            Mac65Assert.OpCodes(result, 0xA9, 0x01, 0xA9, 0x03);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_nested_IF_directives_when_all_conditions_are_1()
        {
            var result = Generate(@"10 L1 = 1
20 L2 = 1
30 .IF L1
40  LDA #1
50 .IF L2
60  LDA #2
70 .ENDIF
80  LDA #3
90 .ENDIF
95  LDA #4");

            Mac65Assert.OpCodes(result, 0xA9, 0x01, 0xA9, 0x02, 0xA9, 0x03, 0xA9, 0x04);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_nested_IF_directives_when_inner_condition_is_0_and_outer_condition_is_1()
        {
            var result = Generate(@"10 L1 = 1
20 L2 = 0
30 .IF L1
40  LDA #1
50 .IF L2
60  LDA #2
70 .ENDIF
80  LDA #3
90 .ENDIF
95  LDA #4");

            Mac65Assert.OpCodes(result, 0xA9, 0x01, 0xA9, 0x03, 0xA9, 0x04);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_nested_IF_directives_when_inner_condition_is_1_and_outer_condition_is_0()
        {
            var result = Generate(@"10 L1 = 0
20 L2 = 1
30 .IF L1
40  LDA #1
50 .IF L2
60  LDA #2
70 .ENDIF
80  LDA #3
90 .ENDIF
95  LDA #4");

            Mac65Assert.OpCodes(result, 0xA9, 0x04);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_nested_IF_directives_when_all_conditions_are_0()
        {
            var result = Generate(@"10 L1 = 0
20 L2 = 0
30 .IF L1
40  LDA #1
50 .IF L2
60  LDA #2
70 .ENDIF
80  LDA #3
90 .ENDIF
95  LDA #4");

            Mac65Assert.OpCodes(result, 0xA9, 0x04);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_ELSE_directive_when_condition_is_1()
        {
            var result = Generate(@"10 .IF 1
20  LDA #1
30 .ELSE
40  LDA #2
50 .ENDIF
");

            Mac65Assert.OpCodes(result, 0xA9, 0x01);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_IF_nested_inside_ELSE_directive()
        {
            var result = Generate(@"10 .IF 0
20  LDA #1
30 .ELSE
40 .IF 1
50  LDA #2
60 .ENDIF
40  LDA #3
50 .ENDIF
");

            Mac65Assert.OpCodes(result, 0xA9, 0x02, 0xA9, 0x03);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_ELSE_directive_when_condition_is_0()
        {
            var result = Generate(@"10 .IF 0
20  LDA #1
30 .ELSE
40  LDA #2
50 .ENDIF
");

            Mac65Assert.OpCodes(result, 0xA9, 0x02);
        }

        [TestMethod]
        public void Can_generate_instructions_inside_nested_ELSE_directives_when_all_conditions_are_0()
        {
            var result = Generate(@"10 L1 = 0
20 L2 = 0
30 .IF L1
40  LDA #1
50 .IF L2
60  LDA #2
62 .ELSE
64  LDA #3
70 .ENDIF
80  LDA #4
82 .ELSE
84  LDA #5
90 .ENDIF
95  LDA #6");

            Mac65Assert.OpCodes(result, 0xA9, 0x05, 0xA9, 0x06);
        }

        private MemoryImage Generate(string source)
        {
            var assembler = new Assembler(source);

            return assembler.Build();
        }
    }
}
