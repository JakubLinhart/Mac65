using System;
using System.Collections.Generic;
using System.Linq;
using Mac65.Syntax;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace Mac65.VisualStudio
{
    internal class Mac65Classifier : IClassifier
    {
        private readonly Dictionary<SyntaxNodeKind, IClassificationType> classificationTypes =
            new Dictionary<SyntaxNodeKind, IClassificationType>();

        internal Mac65Classifier(IClassificationTypeRegistryService registry)
        {
            classificationTypes[SyntaxNodeKind.Label] = registry.GetClassificationType("Mac65Label");
            classificationTypes[SyntaxNodeKind.ExpressionIdentifier] = registry.GetClassificationType("Mac65Label");
            classificationTypes[SyntaxNodeKind.LineNumber] = registry.GetClassificationType("Mac65LineNumber");
            classificationTypes[SyntaxNodeKind.Mnemonic] = registry.GetClassificationType("Mac65Mnemonic");
            classificationTypes[SyntaxNodeKind.Operator] = registry.GetClassificationType("Mac65Operator");
            classificationTypes[SyntaxNodeKind.UnaryOperator] = registry.GetClassificationType("Mac65Operator");
            classificationTypes[SyntaxNodeKind.Constant] = registry.GetClassificationType("Mac65Constant");
            classificationTypes[SyntaxNodeKind.Literal] = registry.GetClassificationType("Mac65Literal");
            classificationTypes[SyntaxNodeKind.DirectiveName] = registry.GetClassificationType("Mac65DirectveName");
        }

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var handler = new ClassificationParserHandler(span, classificationTypes);
            var parser = new Parser(handler);
            if (parser.Parse(span.GetText()))
            {
                return handler.Result;
            }

            return new List<ClassificationSpan>();
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
    }
}