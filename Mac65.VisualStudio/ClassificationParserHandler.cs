using System.Collections.Generic;
using Mac65.Syntax;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace Mac65.VisualStudio
{
    public class ClassificationParserHandler : ParserHandler
    {
        private readonly List<ClassificationSpan> classifications = new List<ClassificationSpan>();
        private readonly Dictionary<SyntaxNodeKind, IClassificationType> classificationTypes;
        private readonly SnapshotSpan regionOfInterestSpan;

        public ClassificationParserHandler(SnapshotSpan regionOfInterestSpan,
            Dictionary<SyntaxNodeKind, IClassificationType> classificationTypes)
        {
            this.regionOfInterestSpan = regionOfInterestSpan;
            this.classificationTypes = classificationTypes;
        }

        public IList<ClassificationSpan> Result
        {
            get { return classifications; }
        }

        public override void EndNode(ParserContext context)
        {
            if (context.Span.Start + regionOfInterestSpan.Start.Position <=  regionOfInterestSpan.End.Position)
            {
                IClassificationType type;
                if (classificationTypes.TryGetValue(context.NodeKind, out type))
                {
                    classifications.Add(new ClassificationSpan(
                        new SnapshotSpan(regionOfInterestSpan.Snapshot, context.Span.Start + regionOfInterestSpan.Start, context.Span.Length), type));
                }
            }
        }
    }
}