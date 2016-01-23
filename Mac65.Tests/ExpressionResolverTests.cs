using System.Collections.Generic;
using Mac65.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mac65.Tests
{
    [TestClass]
    public class ExpressionResolverTests
    {
        [TestMethod]
        public void Can_evaluate_add()
        {
            Assert.AreEqual(30, EvaluateExpression("10+20"));
        }

        [TestMethod]
        public void Can_evaluate_subtract_decimal_with_hex_number()
        {
            Assert.AreEqual(4, EvaluateExpression("6-$2"));
        }

        [TestMethod]
        public void Can_evaluate_mult()
        {
            Assert.AreEqual(12, EvaluateExpression("6*2"));
        }

        [TestMethod]
        public void Can_evaluate_div()
        {
            Assert.AreEqual(3, EvaluateExpression("6/2"));
        }

        [TestMethod]
        public void Can_add_and_sub_multiple_numbers()
        {
            Assert.AreEqual(10, EvaluateExpression("4+2-3+6-7+8"));
        }

        [TestMethod]
        public void Can_evaluate_higher_mult_over_add_priority()
        {
            Assert.AreEqual(11, EvaluateExpression("1+2*3+4"));
            Assert.AreEqual(14, EvaluateExpression("1*2+3*4"));
            Assert.AreEqual(10, EvaluateExpression("1*2*3+4"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_parenthesis()
        {
            Assert.AreEqual(14, EvaluateExpression("2*[3+4]"));
            Assert.AreEqual(9, EvaluateExpression("[1+2]*3"));
            Assert.AreEqual(21, EvaluateExpression("[1+2]*[3+4]"));
            Assert.AreEqual(27, EvaluateExpression("[1+2]*[1+2]*[1+2]"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_nested_parenthesis()
        {
            Assert.AreEqual(24, EvaluateExpression("2*[2*[2*[1+2]]]"));
            Assert.AreEqual(24, EvaluateExpression("[[[1+2]*2]*2]*2"));
            Assert.AreEqual(0, EvaluateExpression("2+[2*[2-[1+2]]]"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_bitwise_operators()
        {
            Assert.AreEqual(0, EvaluateExpression("$FF00&$0FF"));
            Assert.AreEqual(0x0B, EvaluateExpression("$03!$0A"));
            Assert.AreEqual(0x0120, EvaluateExpression("$003F^$011F"));
        }

        [TestMethod]
        public void Bitwise_operator_has_lower_precedence_than_add_operators()
        {
            Assert.AreEqual(0, EvaluateExpression("$FF00&$0FE+1"));
            Assert.AreEqual(0, EvaluateExpression("1+$0FE&$FF00"));
        }

        [TestMethod]
        public void Bitwise_operator_has_lower_precedence_than_mult_operator()
        {
            Assert.AreEqual(1, EvaluateExpression("1!$0FF*0"));
            Assert.AreEqual(1, EvaluateExpression("0*1!1"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_high_byte_operator()
        {
            Assert.AreEqual(0xFF, EvaluateExpression(">$FF00"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_low_byte_operator()
        {
            Assert.AreEqual(0xFB, EvaluateExpression("<$FAFB"));
        }

        [TestMethod]
        public void HighByte_operator_has_lower_precedence_than_add_operator()
        {
            Assert.AreEqual(0, EvaluateExpression(">1+1"));
        }

        [TestMethod]
        public void HighByte_operator_has_lower_precedence_than_mult_operator()
        {
            Assert.AreEqual(1, EvaluateExpression(">$FF*2"));
        }

        [TestMethod]
        public void Can_apply_low_byte_operator_on_parenthesised_subexpression()
        {
            Assert.AreEqual(3, EvaluateExpression("<[1+2]"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_unary_minus_in_expression_with_add_operator()
        {
            Assert.AreEqual(-3, EvaluateExpression("-1+-2"));
        }

        [TestMethod]
        public void Can_apply_unary_minus_operator_on_parenthesised_subexpression()
        {
            Assert.AreEqual(-3, EvaluateExpression("-[1+2]"));
        }

        [TestMethod]
        public void Can_evaluate_comparison_operators()
        {
            Assert.AreEqual(1, EvaluateExpression("1=1"));
            Assert.AreEqual(0, EvaluateExpression("1=0"));
            Assert.AreEqual(1, EvaluateExpression("1>0"));
            Assert.AreEqual(0, EvaluateExpression("0>1"));
            Assert.AreEqual(0, EvaluateExpression("1>1"));
            Assert.AreEqual(1, EvaluateExpression("0<1"));
            Assert.AreEqual(0, EvaluateExpression("1<0"));
            Assert.AreEqual(0, EvaluateExpression("1<1"));
            Assert.AreEqual(1, EvaluateExpression("1<=1"));
            Assert.AreEqual(0, EvaluateExpression("1<=0"));
            Assert.AreEqual(1, EvaluateExpression("0<=1"));
            Assert.AreEqual(1, EvaluateExpression("1>=1"));
            Assert.AreEqual(1, EvaluateExpression("1>=0"));
            Assert.AreEqual(0, EvaluateExpression("0>=1"));
            Assert.AreEqual(0, EvaluateExpression("0<>0"));
            Assert.AreEqual(1, EvaluateExpression("0<>1"));
        }

        [TestMethod]
        public void Comparison_operators_have_lower_precedence_than_mult_add_and_bitwise_operators()
        {
            Assert.AreEqual(1, EvaluateExpression("4=2+2"));
            Assert.AreEqual(1, EvaluateExpression("4=2*2"));
            Assert.AreEqual(1, EvaluateExpression("4=4&4"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_logical_operators()
        {
            Assert.AreEqual(1, EvaluateExpression("1.AND 1"));
            Assert.AreEqual(0, EvaluateExpression("1.AND 0"));
            Assert.AreEqual(0, EvaluateExpression("0.AND 1"));
            Assert.AreEqual(0, EvaluateExpression("0.AND 0"));
            Assert.AreEqual(1, EvaluateExpression("1.OR 0"));
            Assert.AreEqual(1, EvaluateExpression("0.OR 1"));
            Assert.AreEqual(1, EvaluateExpression("1.OR 1"));
            Assert.AreEqual(0, EvaluateExpression("0.OR 0"));
            Assert.AreEqual(0, EvaluateExpression(".NOT 1"));
            Assert.AreEqual(1, EvaluateExpression(".NOT 0"));
        }

        [TestMethod]
        public void Logical_AND_logical_OR_operators_have_lowest_precedence()
        {
            Assert.AreEqual(0, EvaluateExpression("1.AND 1+1"));
            Assert.AreEqual(1, EvaluateExpression("2=2.OR 0"));
        }

        [TestMethod]
        public void Logical_NOT_has_higher_precedence_than_mult_and_add_operators()
        {
            Assert.AreEqual(1, EvaluateExpression(".NOT 0*1"));
            Assert.AreEqual(2, EvaluateExpression("1+.NOT 0"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_DEF_operator()
        {
            Assert.AreEqual(0, EvaluateExpression(".DEF L1"));
            Assert.AreEqual(1, EvaluateExpression(".DEF L1", new[] { new KeyValuePair<string, int>("L1", 1) }));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_REF_operator_when_label_is_not_referenced()
        {
            Assert.AreEqual(0, EvaluateExpression(".REF L1"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_REF_operator_when_label_is_referenced()
        {
            var store = new LabelStore();
            store.AddReferencedLabel("L1", -1);
            Assert.AreEqual(1, EvaluateExpression(".REF L1", store));
        }

        [TestMethod]
        public void REF_operator_has_higher_priority_than_mult()
        {
            var store = new LabelStore();
            store.AddReferencedLabel("L1", -1);

            Assert.AreEqual(2, EvaluateExpression(".REF L1 * 2", store));
            Assert.AreEqual(2, EvaluateExpression(".REF L1 + 1", store));
            Assert.AreEqual(1, EvaluateExpression(".REF L1 = 1", store));
            Assert.AreEqual(1, EvaluateExpression(".REF L1 .AND 1", store));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_label()
        {
            Assert.AreEqual(3, EvaluateExpression("2+LABEL", new[] { new KeyValuePair<string, int>("LABEL", 1), }));
        }

        [TestMethod]
        public void Can_evaluate_character_constant()
        {
            Assert.AreEqual(0x41, EvaluateExpression("'A"));
            Assert.AreEqual(0x61, EvaluateExpression("'a"));
            Assert.AreEqual(0x31, EvaluateExpression("'1"));
            Assert.AreEqual(0x27, EvaluateExpression("''"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_character_constant()
        {
            Assert.AreEqual(0x63, EvaluateExpression("'1 + '2"));
        }

        [TestMethod]
        public void Can_evaluate_expression_with_single_number_inside_parentheses()
        {
            Assert.AreEqual(1, EvaluateExpression("[1]"));
        }

        private int EvaluateExpression(string source, LabelStore labelStore)
        {
            var resolver = new ExpressionResolver(labelStore);
            var parser = new Parser(resolver);

            bool success = parser.ParseExpression(source);
            if (!success)
            {
                Assert.Inconclusive("Cannot parse expression " + source);
            }

            return resolver.Evaluate();
        }

        private int EvaluateExpression(string source, KeyValuePair<string, int>[] labels = null)
        {
            var labelStore = new LabelStore();

            if (labels != null)
            {
                foreach (var pair in labels)
                {
                    labelStore.AddLabel(new Label(pair.Key, pair.Value));
                }
            }

            return EvaluateExpression(source, labelStore);
        }
    }
}
