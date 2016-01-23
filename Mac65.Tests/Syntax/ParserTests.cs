using Mac65.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mac65.Tests.Syntax
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Can_parse_line_with_mnemonic()
        {
            var result = Parse("150  DEY");

            AssertResult(result, "line<150>", "instruction<mnemonic<DEY>, implied>");
        }

        [TestMethod]
        public void Can_parse_line_with_label()
        {
            var result = Parse("150 LABEL");

            AssertResult(result, "line<150>", "label<LABEL>");
        }

        [TestMethod]
        public void Can_parse_label_starting_with_at_sign()
        {
            var result = Parse("150 @LABEL");

            AssertResult(result, "line<150>", "label<@LABEL>");
        }

        [TestMethod]
        public void Can_parse_label_starting_with_questionmark()
        {
            var result = Parse("150 ?LABEL");

            AssertResult(result, "line<150>", "label<?LABEL>");
        }

        [TestMethod]
        public void Can_parse_label_containing_number()
        {
            var result = Parse("150 LABEL1");

            AssertResult(result, "line<150>", "label<LABEL1>");
        }

        [TestMethod]
        public void Can_parse_label_containing_dot()
        {
            var result = Parse("150 LABEL.");

            AssertResult(result, "line<150>", "label<LABEL.>");
        }

        [TestMethod]
        public void Can_parse_label_containing_questionmark()
        {
            var result = Parse("150 LABEL?");

            AssertResult(result, "line<150>", "label<LABEL?>");
        }

        [TestMethod]
        public void Can_parse_label_containing_at_sign()
        {
            var result = Parse("150 L@BEL");

            AssertResult(result, "line<150>", "label<L@BEL>");
        }

        [TestMethod]
        [Ignore]
        public void Refuses_parse_label_starting_with_dot()
        {
            var result = Parse("10 .LABEL");
        }

        [TestMethod]
        [Ignore]
        public void Refuses_parse_label_starting_with_number()
        {
            var result = Parse("10 1LABEL");
        }

        [TestMethod]
        public void Can_parse_line_with_label_and_mnemonic()
        {
            var result = Parse("150 LABEL DEY");

            AssertResult(result, "line<150>", "label<LABEL>", "instruction<mnemonic<DEY>, implied>");
        }

        [TestMethod]
        public void Can_parse_implied_instruction_at_non_last_line()
        {
            var result = Parse(@"10  DEY
20 LABEL");

            AssertResult(result, "line<10>", "instruction<mnemonic<DEY>, implied>", "line<20>", "label<LABEL>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_decimal_absolute_operand()
        {
            var result = Parse("150  JMP 1234");

            AssertResult(result, "line<150>", "instruction<mnemonic<JMP>, absolute<1234>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_hex_absolute_operand()
        {
            var result = Parse("150  JMP $1234");

            AssertResult(result, "line<150>", "instruction<mnemonic<JMP>, absolute<$1234>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_absoluteX_operand()
        {
            var result = Parse("10  LDA $4400,X");

            AssertResult(result, "line<10>", "instruction<mnemonic<LDA>, absolute<$4400,X>, indexer<X>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_absoluteY_operand()
        {
            var result = Parse("10  LDA $4400,Y");

            AssertResult(result, "line<10>", "instruction<mnemonic<LDA>, absolute<$4400,Y>, indexer<Y>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_indirect_operand()
        {
            var result = Parse("10  JMP ($1234)");

            AssertResult(result, "line<10>", "instruction<mnemonic<JMP>, indirect<($1234)>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_indirectX_operand()
        {
            var result = Parse("10  LDA ($12,X)");

            AssertResult(result, "line<10>", "instruction<mnemonic<LDA>, indirect<($12,X)>, indexer<X>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_indirectY_operand()
        {
            var result = Parse("10  LDA ($12),Y");

            AssertResult(result, "line<10>", "instruction<mnemonic<LDA>, indirect<($12),Y>, indexer<Y>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_label_absolute_operand()
        {
            var result = Parse("150  JMP LABEL");

            AssertResult(result, "line<150>", "instruction<mnemonic<JMP>, absolute<LABEL>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_decimal_immediate_operand()
        {
            var result = Parse("150  LDA #1");

            AssertResult(result, "line<150>", "instruction<mnemonic<LDA>, immediate<1>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_hex_immediate_operand()
        {
            var result = Parse("150  LDA #$1");

            AssertResult(result, "line<150>", "instruction<mnemonic<LDA>, immediate<$1>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_label_immediate_operand()
        {
            var result = Parse("150  LDA #LABEL");

            AssertResult(result, "line<150>", "instruction<mnemonic<LDA>, immediate<LABEL>>");
        }

        [TestMethod]
        public void Can_parse_instruction_with_accumulator_operand()
        {
            var result = Parse("150  ASL A");

            AssertResult(result, "line<150>", "instruction<mnemonic<ASL>, accumulator>");
        }

        [TestMethod]
        public void Can_parse_multiple_lines()
        {
            var result = Parse(@"10  ASL A
20  LDA #1");

            AssertResult(result, "line<10>", "instruction<mnemonic<ASL>, accumulator>", "line<20>", "instruction<mnemonic<LDA>, immediate<1>>");
        }

        [TestMethod]
        public void Can_parse_macro()
        {
            var result = Parse(@"10 .MACRO TEST
20  NOP
30 .ENDM");

            AssertResult(result,
                "line<10>", "macro<TEST>",
                "line<20>", "instruction<mnemonic<NOP>, implied>",
                "line<30>", "directive<.ENDM>");
        }

        [TestMethod]
        public void Can_parse_macro_with_directives_on_mnemonic_position()
        {
            var result = Parse(@"10  .MACRO TEST
20  NOP
30  .ENDM");

            AssertResult(result,
                "line<10>", "macro<TEST>",
                "line<20>", "instruction<mnemonic<NOP>, implied>",
                "line<30>", "directive<.ENDM>");
        }
        
        [TestMethod]
        public void Can_parse_IF_directive()
        {
            var result = Parse(@"10 .IF 1
20  NOP
30 .ENDIF");

            AssertResult(result,
                "line<10>", "directive<.IF, 1>",
                "line<20>", "instruction<mnemonic<NOP>, implied>",
                "line<30>", "directive<.ENDIF>");
        }

        [TestMethod]
        public void Can_parse_IF_directive_with_empty_body()
        {
            var result = Parse(@"10 .IF 1
20 .ENDIF");

            AssertResult(result,
                "line<10>", "directive<.IF, 1>",
                "line<20>", "directive<.ENDIF>");
        }

        [TestMethod]
        public void Can_parse_IF_directive_with_ELSE()
        {
            var result = Parse(@"10 .IF 1
20  NOP
30 .ELSE
40  NOP
50 .ENDIF");

            AssertResult(result,
                "line<10>", "directive<.IF, 1>",
                "line<20>", "instruction<mnemonic<NOP>, implied>",
                "line<30>", "directive<.ELSE>",
                "line<40>", "instruction<mnemonic<NOP>, implied>",
                "line<50>", "directive<.ENDIF>");
        }

        [TestMethod]
        [Ignore]
        public void Refuses_parse_IF_directive_with_two_ELSE()
        {
            var result = Parse(@"10 .IF 1
20  NOP
30 .ELSE
40 .ELSE
50  NOP
60 .ENDIF");
            Assert.Inconclusive();
        }

        [Ignore]
        [TestMethod]
        public void Refuses_parse_IF_directive_without_condition()
        {
            var result = Parse(@"10 .IF
20 .ENDIF");
        }

        [Ignore]
        [TestMethod]
        public void Refuses_parse_IF_directive_without_ENDIF()
        {
            var result = Parse(@"10 .IF");
        }

        [TestMethod]
        public void Can_parse_add_with_numbers()
        {
            var result = ParseExpression("10+20");
            AssertResult(result, "num<10>", "op<+>", "num<20>");
        }

        [TestMethod]
        public void Can_parse_add_with_number_and_identifier()
        {
            var result = ParseExpression("10+LABEL");
            AssertResult(result, "num<10>", "op<+>", "identifier<LABEL>");
        }

        [TestMethod]
        public void Can_parse_sum_with_identifier_and_number()
        {
            var result = ParseExpression("LABEL+20");
            AssertResult(result, "identifier<LABEL>", "op<+>", "num<20>");
        }

        [TestMethod]
        public void Can_parse_sub_with_identifier_and_number()
        {
            var result = ParseExpression("10-20");
            AssertResult(result, "num<10>", "op<->", "num<20>");
        }

        [TestMethod]
        public void Can_parse_multiplication_with_identifier_and_number()
        {
            var result = ParseExpression("LABEL*20");
            AssertResult(result, "identifier<LABEL>", "op<*>", "num<20>");
        }

        [TestMethod]
        public void Can_parse_multiplication_with_identifier_with_digit_at_sign_and_qustionmark_in_name()
        {
            var result = ParseExpression("L?@1*20");
            AssertResult(result, "identifier<L?@1>", "op<*>", "num<20>");
        }

        [TestMethod]
        public void Can_parse_division_with_number_and_identifier()
        {
            var result = ParseExpression("10/LABEL");
            AssertResult(result, "num<10>", "op</>", "identifier<LABEL>");
        }

        [TestMethod]
        public void Can_parse_three_adds()
        {
            var result = ParseExpression("10+20+30");
            AssertResult(result, "num<10>", "op<+>", "num<20>", "op<+>", "num<30>");
        }

        [TestMethod]
        public void Can_parse_three_multiplications()
        {
            var result = ParseExpression("10*20*30");
            AssertResult(result, "num<10>", "op<*>", "num<20>", "op<*>", "num<30>");
        }

        [TestMethod]
        public void Can_parse_two_adds_and_two_multiplications()
        {
            var result = ParseExpression("10+20*30+40*50");
            AssertResult(result, "num<10>", "op<+>", "num<20>", "op<*>", "num<30>", "op<+>", "num<40>", "op<*>", "num<50>");
        }

        [TestMethod]
        public void Can_parse_expression_with_parentheses()
        {
            var result = ParseExpression("[10+20]*[30+40]");
            AssertResult(result, "(", "num<10>", "op<+>", "num<20>", ")", "op<*>", "(", "num<30>", "op<+>", "num<40>", ")");
        }

        [TestMethod]
        public void Can_parse_expression_with_bitwise_operators()
        {
            var result = ParseExpression("10&20!30^40");
            AssertResult(result, "num<10>", "op<&>", "num<20>", "op<!>", "num<30>", "op<^>", "num<40>");
        }

        [TestMethod]
        public void Can_parse_expression_with_high_byte_operator()
        {
            var result = ParseExpression(">10");
            AssertResult(result, "unaryop<>>", "num<10>");
        }

        [TestMethod]
        public void Can_parse_expression_with_low_byte_operator()
        {
            var result = ParseExpression("<10");
            AssertResult(result, "unaryop<<>", "num<10>");
        }

        [TestMethod]
        public void Can_parse_expression_with_high_byte_operator_applied_on_label()
        {
            var result = ParseExpression("<LABEL");
            AssertResult(result, "unaryop<<>", "identifier<LABEL>");
        }

        [TestMethod]
        public void Can_parse_expression_with_low_byte_operator_applied_on_label()
        {
            var result = ParseExpression(">LABEL");
            AssertResult(result, "unaryop<>>", "identifier<LABEL>");
        }

        [TestMethod]
        public void Can_parse_high_byte_operator_applied_on_parenthesised_expression()
        {
            var result = ParseExpression(">[1+2]");
            AssertResult(result, "unaryop<>>", "(", "num<1>", "op<+>", "num<2>", ")");
        }

        [TestMethod]
        public void Can_parse_low_byte_operator_applied_on_parenthesised_expression()
        {
            var result = ParseExpression("<[1+2]");
            AssertResult(result, "unaryop<<>", "(", "num<1>", "op<+>", "num<2>", ")");
        }

        [TestMethod]
        public void Can_parse_unary_minus_operator_in_expression_with_add_operator()
        {
            var result = ParseExpression("-1+-2");
            AssertResult(result, "unaryop<->", "num<1>", "op<+>", "unaryop<->", "num<2>");
        }

        [TestMethod]
        public void Can_parse_application_of_unary_minus_operator_on_parenthesised_expression()
        {
            var result = ParseExpression("-[1+2]");
            AssertResult(result, "unaryop<->", "(", "num<1>", "op<+>", "num<2>", ")");
        }

        [TestMethod]
        public void Can_parse_expression_with_comparison_operators()
        {
            var result = ParseExpression("1>2<3<>4=5<=6>=7");

            AssertResult(result, "num<1>", "op<>>", "num<2>", "op<<>", "num<3>", "op<<>>", "num<4>", "op<=>",
                "num<5>", "op<<=>", "num<6>", "op<>=>", "num<7>");
        }

        [TestMethod]
        public void Can_parse_expression_with_logical_operators()
        {
            var result = ParseExpression("1.AND 2.OR .NOT 3");
            AssertResult(result, "num<1>", "op<.AND>", "num<2>", "op<.OR>", "unaryop<.NOT>", "num<3>");
        }

        [TestMethod]
        public void Can_parse_expression_with_REF_operator()
        {
            var result = ParseExpression(".REF LABEL");
            AssertResult(result, "unaryop<.REF>", "identifier<LABEL>");
        }

        [TestMethod]
        public void Can_parse_expression_with_DEF_operator()
        {
            var result = ParseExpression(".DEF LABEL");
            AssertResult(result, "unaryop<.DEF>", "identifier<LABEL>");
        }

        [TestMethod]
        public void Can_parse_expression_with_identifier_and_operator_followed_by_space()
        {
            var result = ParseExpression("L1 + L2");
            AssertResult(result, "identifier<L1>", "op<+>", "identifier<L2>");
        }

        [TestMethod]
        public void Can_parse_expression_with_number_and_operator_followed_by_space()
        {
            var result = ParseExpression("$12 + 45");
            AssertResult(result, "num<$12>", "op<+>", "num<45>");
        }

        [TestMethod]
        public void Can_parse_expression_with_character_constant()
        {
            var result = ParseExpression("'1 + '2");
            AssertResult(result, "num<'1>", "op<+>", "num<'2>");
        }

        [TestMethod]
        [Ignore]
        [Description("Braces are not in ATASCII table, ignoring for now")]
        public void Refuse_parse_non_atascii_character_constant()
        {
            var result = ParseExpression("'{");
            Assert.Fail();
        }

        [TestMethod]
        public void Can_parse_single_number_in_parentheses()
        {
            var result = ParseExpression("[1]");
            AssertResult(result, "(", "num<1>", ")");
        }

        [TestMethod]
        public void Can_parse_assignment_directive()
        {
            var result = Parse("10 L1 = 1");
            AssertResult(result, "line<10>", "label<L1>", "directive<=, 1>");
        }

        [TestMethod]
        public void Can_parse_assignment_directive_without_spaces()
        {
            var result = Parse("10 L1=1");
            AssertResult(result, "line<10>", "label<L1>", "directive<=, 1>");
        }

        [TestMethod]
        [Ignore]
        public void Refuse_parse_assignment_directive_without_operand()
        {
            var result = Parse("10 L1 =");
            Assert.Fail();
        }

        [TestMethod]
        public void Can_parse_change_current_origin_directive_without_label()
        {
            var result = Parse("10  *= 1");
            AssertResult(result, "line<10>", "directive<*=, 1>");
        }

        [TestMethod]
        public void Can_parse_change_current_origin_directive_with_label()
        {
            var result = Parse("10 L1 *= 1");
            AssertResult(result, "line<10>", "label<L1>", "directive<*=, 1>");
        }

        [Ignore]
        [TestMethod]
        public void Refuse_parse_current_origin_directive_on_label_position()
        {
            var result = Parse("10 *= 1");
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Can_parse_DS_directive_on_mnemonic_position()
        {
            var result = Parse("10  .DS 1");
            AssertResult(result, "line<10>", "directive<.DS, 1>");
        }

        [TestMethod]
        public void Can_parse_DS_directive_on_label_position()
        {
            var result = Parse("10 .DS 1");
            AssertResult(result, "line<10>", "directive<.DS, 1>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_single_number_on_label_position()
        {
            var result = Parse("10 .BYTE 1");

            AssertResult(result, "line<10>", "directive<.BYTE, 1>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_single_number_on_mnemonic_position()
        {
            var result = Parse("10  .BYTE 1");

            AssertResult(result, "line<10>", "directive<.BYTE, 1>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_two_numbers()
        {
            var result = Parse("10  .BYTE 1, 2");

            AssertResult(result, "line<10>", "directive<.BYTE, 1, 2>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_two_numbers_without_space_after_comma()
        {
            var result = Parse("10  .BYTE 1,2");

            AssertResult(result, "line<10>", "directive<.BYTE, 1, 2>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_string()
        {
            var result = Parse("10  .BYTE \"ABC\"");

            AssertResult(result, "line<10>", "directive<.BYTE, ABC>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_two_strings()
        {
            var result = Parse("10  .BYTE \"ABC\",\"ABC\"");

            AssertResult(result, "line<10>", "directive<.BYTE, ABC, ABC>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_string_and_number()
        {
            var result = Parse("10  .BYTE \"ABC\",1");

            AssertResult(result, "line<10>", "directive<.BYTE, ABC, 1>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_number_and_string()
        {
            var result = Parse("10  .BYTE 1,\"ABC\"");

            AssertResult(result, "line<10>", "directive<.BYTE, 1, ABC>");
        }

        [TestMethod]
        public void Can_parse_BYTE_directive_with_expression()
        {
            var result = Parse("10  .BYTE 3*[2+2]");

            AssertResult(result, "line<10>", "directive<.BYTE, 3*[2+2]>");
        }

        [TestMethod]
        public void Can_parse_WORD_directive_with_two_numbers_without_space_after_comma()
        {
            var result = Parse("10  .WORD 1,2");

            AssertResult(result, "line<10>", "directive<.WORD, 1, 2>");
        }

        [TestMethod]
        public void Can_parse_WORD_directive_with_expression()
        {
            var result = Parse("10  .WORD 3*[2+2]");

            AssertResult(result, "line<10>", "directive<.WORD, 3*[2+2]>");
        }

        [TestMethod]
        [Ignore]
        public void Refuses_parse_WORD_directive_with_string()
        {
            var result = Parse("10  .WORD 1,\"ABC\"");

            Assert.Inconclusive();
        }

        [TestMethod]
        public void Can_parse_DBYTE_directive_with_two_numbers_without_space_after_comma()
        {
            var result = Parse("10  .DBYTE 1,2");

            AssertResult(result, "line<10>", "directive<.DBYTE, 1, 2>");
        }

        [TestMethod]
        [Ignore]
        public void Can_parse_END_direcive()
        {
            var result = Parse("10 .END");
        }

        [TestMethod]
        public void Can_parse_empty_line_with_comment()
        {
            var result = Parse(@"10 ;comment
20 *comment");

            AssertResult(result, "line<10>", "line<20>");
        }

        [TestMethod]
        public void Can_parse_label_followed_by_comment()
        {
            var result = Parse(@"10 L1 ;comment");

            AssertResult(result, "line<10>", "label<L1>");
        }

        [TestMethod]
        public void Can_parse_label_followed_by_comment_wihtout_space()
        {
            var result = Parse(@"10 L1;comment");

            AssertResult(result, "line<10>", "label<L1>");
        }

        [TestMethod]
        public void Can_parse_label_followed_by_comment_after_many_spaces()
        {
            var result = Parse(@"10 L1       ;comment");

            AssertResult(result, "line<10>", "label<L1>");
        }

        [TestMethod]
        public void Can_parse_implied_instruction_followed_by_comment_wihtout_space()
        {
            var result = Parse(@"10  NOP;comment");

            AssertResult(result, "line<10>", "instruction<mnemonic<NOP>, implied>");
        }

        [TestMethod]
        public void Can_parse_directive_on_label_position_followed_by_comment_wihtout_space()
        {
            var result = Parse(@"10 .BYTE 3*[2+2];comment");

            AssertResult(result, "line<10>", "directive<.BYTE, 3*[2+2]>");
        }

        [TestMethod]
        public void Can_parse_directive_followed_by_comment_wihtout_space()
        {
            var result = Parse(@"10  .BYTE 3*[2+2];comment");

            AssertResult(result, "line<10>", "directive<.BYTE, 3*[2+2]>");
        }

        private string[] ParseExpression(string source)
        {
            var handler = new TestExpressionParserHandler();
            bool success = new Parser(handler).ParseExpression(source);
            Assert.IsTrue(success, "Cannot parse expression " + source);

            return handler.Result;
        }

        private string[] Parse(string source)
        {
            var handler = new TestParserHandler();
            bool success = new Parser(handler).Parse(source);
            Assert.IsTrue(success, "Cannot parse: " + source);

            return handler.Result;
        }

        private void AssertResult(string[] actual, params string[] expected)
        {
            var i = 0;
            string message = string.Empty;

            if (actual.Length == expected.Length)
            {
                foreach (var str in actual)
                {
                    if (str != expected[i])
                    {
                        message = string.Format("Mismatch on {0} position. Expected:\n{1}\n but actual is\n{2}\n.", i, expected[i], str);
                    }
                    i++;
                }
            }
            else
            {
                message = string.Format("Actual result length {0} doesn't match expected result length {1}", actual.Length, expected.Length);
            }

            if (!string.IsNullOrEmpty(message))
            {
                Assert.Fail("{0}\n{1} expected but actual result is {2}", message, string.Join(",", expected),
                    string.Join(",", actual));
            }
        }
    }
}