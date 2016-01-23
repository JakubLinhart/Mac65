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
    public class LabelResolvingTests
    {
        [TestMethod]
        public void Can_resolve_label_on_first_line()
        {
            var labels = ResolveLabels(@"10 LABEL");

            AssertLabel(labels, new Label("LABEL", 0));
        }

        [TestMethod]
        public void Can_resolve_label_after_implied_instruction()
        {
            var labels = ResolveLabels(@"10  NOP
20 LABEL");

            AssertLabel(labels, new Label("LABEL", 1));
        }

        [TestMethod]
        public void Can_resolve_label_at_the_same_line_as_implied_instruction()
        {
            var labels = ResolveLabels(@"10 L1 NOP");

            AssertLabel(labels, new Label("L1", 0));
        }

        [TestMethod]
        public void Can_resolve_label_after_immediate_instruction()
        {
            var labels = ResolveLabels(@"10  LDA #12
20 LABEL");

            AssertLabel(labels, new Label("LABEL", 2));
        }

        [TestMethod]
        public void Can_resolve_label_after_absolute_instruction()
        {
            var labels = ResolveLabels(@"10  JMP $1234
20 LABEL");

            AssertLabel(labels, new Label("LABEL", 3));
        }

        [TestMethod]
        public void Can_resolve_label_after_three_instructions()
        {
            var labels = ResolveLabels(@"10  JMP $1234
20  NOP
30  LDA #12
40 LABEL");

            AssertLabel(labels, new Label("LABEL", 6));
        }

        [TestMethod]
        public void Can_resolve_label_defined_by_assignment_directive()
        {
            var labels = ResolveLabels(@"10 L1 = 1");
            AssertLabel(labels, new Label("L1", 1));
        }

        [TestMethod]
        public void Can_resolve_two_labels_defined_by_assignment_directive()
        {
            var labels = ResolveLabels(@"10 L1 = 1
20 L2 = 2");
            AssertLabel(labels, new Label("L1", 1), new Label("L2", 2));
        }

        [TestMethod]
        public void Can_resolve_label_after_origin_change()
        {
            var labels = ResolveLabels(@"10  *= $1000
20 L1");

            AssertLabel(labels, new Label("L1", 0x1000));
        }

        [TestMethod]
        public void Can_resolve_label_after_BYTE_directive()
        {
            var labels = ResolveLabels(@"10 .BYTE 1
20 L1");

            AssertLabel(labels, new Label("L1", 1));
        }

        [TestMethod]
        public void Can_resolve_label_after_BYTE_directive_with_literal()
        {
            var labels = ResolveLabels(@"10 .BYTE ""ABC""
20 L1");

            AssertLabel(labels, new Label("L1", 3));
        }

        [TestMethod]
        public void Can_resolve_labels_overflowing_16bits()
        {
            AssertLabel(ResolveLabels(@"10 L1 = $FFFF + 1"), new Label("L1", 0));
            AssertLabel(ResolveLabels(@"10 L1 = 0 - 1"), new Label("L1", 0xFFFF));
        }

        [TestMethod]
        public void Can_resolve_label_after_IF_directive_when_condition_is_1()
        {
            var labels = ResolveLabels(@"10 .IF 1
20  NOP
30 L1 .ENDIF");

            AssertLabel(labels, new Label("L1", 1));
        }

        [TestMethod]
        public void Can_resolve_label_after_IF_directive_when_condition_is_0()
        {
            var labels = ResolveLabels(@"10 .IF 0
20  NOP
30 L1 .ENDIF");

            AssertLabel(labels, new Label("L1", 0));
        }

        [TestMethod]
        public void Can_resolve_label_inside_IF_directive()
        {
            var labels = ResolveLabels(@"10 .IF 1
20 L1 NOP
30 .ENDIF");

            AssertLabel(labels, new Label("L1", 0));
        }

        [TestMethod]
        public void Can_resolve_label_inside_ELSE_directive()
        {
            var labels = ResolveLabels(@"10 .IF 0
20  NOP
30 .ELSE
40 L2 NOP 
50 .ENDIF");

            AssertLabel(labels, new Label("L2", 0));
        }

        [TestMethod]
        public void Can_resolve_label_on_same_line_with_DS_directive()
        {
            var labels = ResolveLabels("10 L1 .DS 2");

            AssertLabel(labels, new Label("L1", 0));
        }

        private void AssertLabel(Label[] actualLabels, params Label[] expectedLabels)
        {
            string errorMessage = null;

            if (actualLabels.Length != expectedLabels.Length)
            {
                errorMessage = string.Format("Expected number of labels is {0} but actual number of labels is {1}",
                    expectedLabels.Length,
                    actualLabels.Length);
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                int i = 0;
                foreach (var actualLabel in actualLabels)
                {
                    if (!actualLabel.Equals(expectedLabels[i]))
                    {
                        errorMessage = "Actual labels doesn't match expected labels.";
                        break;
                    }
                    i++;
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                string actualLabelsText = string.Join(", ", actualLabels.Select(x => string.Format("{0} ({1})", x.Name, x.Value)));
                string expectedLabelsText = string.Join(", ", expectedLabels.Select(x => string.Format("{0} ({1})", x.Name, x.Value)));
                Assert.Fail("{0}.\nExpected labels: {1}\nActual labels: {2}", errorMessage, expectedLabelsText, actualLabelsText);
            }
        }

        private Label[] ResolveLabels(string source, LabelStore labelStore)
        {
            var resolver = new InstructionResolver(labelStore, new MacroStore());
            var parser = new Parser(resolver);

            parser.Parse(source);

            return labelStore.ToArray();
        }

        private Label[] ResolveLabels(string source)
        {
            var labelStore = new LabelStore();

            return ResolveLabels(source, labelStore);
        }
    }
}
