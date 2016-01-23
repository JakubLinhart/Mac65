using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Mac65.VisualStudio
{
    internal static class Mac65ClassificationTypeDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Mac65Label")]
        internal static ClassificationTypeDefinition Label = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Mac65LineNumber")]
        internal static ClassificationTypeDefinition LineNumber = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Mac65Mnemonic")]
        internal static ClassificationTypeDefinition Mnemonic = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Mac65Operator")]
        internal static ClassificationTypeDefinition Operator = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Mac65Constant")]
        internal static ClassificationTypeDefinition Constant = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Mac65Literal")]
        internal static ClassificationTypeDefinition Literal = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Mac65DirectveName")]
        internal static ClassificationTypeDefinition DirectveName = null;
    }
}
