using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Mac65.VisualStudio
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Mac65Label")]
    [Name("Mac65Label")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mac65Label : ClassificationFormatDefinition
    {
        public Mac65Label()
        {
            this.DisplayName = "Mac65 - Label";
            this.ForegroundColor = Colors.BlueViolet;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Mac65LineNumber")]
    [Name("Mac65LineNumber")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mac65LineNumber : ClassificationFormatDefinition
    {
        public Mac65LineNumber()
        {
            this.DisplayName = "Mac65 - LineNumber";
            this.ForegroundColor = Colors.Crimson;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Mac65Mnemonic")]
    [Name("Mac65Mnemonic")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mac65Mnemonic : ClassificationFormatDefinition
    {
        public Mac65Mnemonic()
        {
            this.DisplayName = "Mac65 - Mnemonic";
            this.ForegroundColor = Colors.Blue;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Mac65Operator")]
    [Name("Mac65Operator")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mac65Operator : ClassificationFormatDefinition
    {
        public Mac65Operator()
        {
            this.DisplayName = "Mac65 - Operator";
            this.ForegroundColor = Colors.LightGreen;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Mac65Constant")]
    [Name("Mac65Constant")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mac65Constant : ClassificationFormatDefinition
    {
        public Mac65Constant()
        {
            this.DisplayName = "Mac65 - Constant";
            this.ForegroundColor = Colors.PaleVioletRed;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Mac65Literal")]
    [Name("Mac65Literal")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mac65Literal : ClassificationFormatDefinition
    {
        public Mac65Literal()
        {
            this.DisplayName = "Mac65 - Literal";
            this.ForegroundColor = Colors.DarkRed;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "Mac65DirectveName")]
    [Name("Mac65DirectveName")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class Mac65DirectveName : ClassificationFormatDefinition
    {
        public Mac65DirectveName()
        {
            this.DisplayName = "Mac65 - DirectiveName";
            this.ForegroundColor = Colors.Blue;
        }
    }
}
