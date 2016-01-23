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
    public class AutocompletionParserHandlerTests
    {
        [TestMethod]
        public void When_no_mnemonic_Then_suggests_first_according_alphabet_order()
        {
            var suggestion = Suggest("10  ", 3);

            Assert.AreEqual("ADC", suggestion);
        }

        [TestMethod]
        public void When_one_letter_in_mnemonic_Then_suggests_first_according_alphabet_order()
        {
            var suggestion = Suggest("10  B", 4);

            Assert.AreEqual("BCC", suggestion);
        }

        [TestMethod]
        public void When_two_letter_in_mnemonic_Then_suggests_first_according_alphabet_order()
        {
            var suggestion = Suggest("10  BE", 5);

            Assert.AreEqual("BEQ", suggestion);
        }

        [TestMethod]
        public void When_three_letter_in_mnemonic_Then_suggests_the_mnemonic()
        {
            var suggestion = Suggest("10  INY", 6);

            Assert.AreEqual("INY", suggestion);
        }

        [TestMethod]
        public void When_longer_than_allowed_mnemonic_Then_suggests_according_first_three_letters()
        {
            var suggestion = Suggest("10  INYa", 7);

            Assert.AreEqual("INY", suggestion);
        }

        [TestMethod]
        public void When_on_line_number_Then_no_suggestion()
        {
            var suggestion = Suggest("10 ", 0);

            Assert.IsNull(suggestion);
        }

        private string Suggest(string source, int cursorPosition)
        {
            var handler = new AutocompletionParserHandler(cursorPosition);
            var parser = new Parser(handler);

            parser.Parse(source);

            return handler.GetSuggestion();
        }
    }
}
