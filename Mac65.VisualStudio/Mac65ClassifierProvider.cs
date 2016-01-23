using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Mac65.VisualStudio
{
    [Export(typeof(IClassifierProvider))]
    [ContentType("Mac65")]
    internal class Mac65ClassifierProvider : IClassifierProvider
    {
        [Export]
        [FileExtension(".m65")]
        [ContentType("Mac65")]
        internal static FileExtensionToContentTypeDefinition Mac65FileType = null;

        [Export]
        [Name("Mac65")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition Mac65ContentType = null;


        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null;

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new Mac65Classifier(ClassificationRegistry));
        }
    }
}