using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Runtime.InteropServices;

namespace Mac65.VisualStudio
{
    [Guid("0AC7D805-543E-454A-B41D-2B02DE67E0FF")]
    public class Mac65Language : LanguageService
    {
        public const string FileExtension = ".m65";
        public const string LanguageName = "Mac65";

        private LanguagePreferences preferences = null;

        public override LanguagePreferences GetLanguagePreferences()
        {
            if (preferences == null)
            {
                preferences = new LanguagePreferences(this.Site, typeof(Mac65Language).GUID, this.Name);

                if (preferences != null)
                {
                    preferences.Init();

                    preferences.EnableCodeSense = true;
                    preferences.EnableMatchBraces = true;
                    preferences.EnableCommenting = true;
                    preferences.EnableShowMatchingBrace = true;
                    preferences.EnableMatchBracesAtCaret = true;
                    preferences.HighlightMatchingBraceFlags = _HighlightMatchingBraceFlags.HMB_USERECTANGLEBRACES;
                    preferences.LineNumbers = true;
                    preferences.MaxErrorMessages = 100;
                    preferences.AutoOutlining = false;
                    preferences.MaxRegionTime = 2000;
                    preferences.ShowNavigationBar = true;

                    preferences.AutoListMembers = true;
                    preferences.EnableQuickInfo = true;
                    preferences.ParameterInformation = true;
                }
            }

            return preferences;
        }

        public override IScanner GetScanner(IVsTextLines buffer)
        {
            throw new System.NotImplementedException();
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            throw new System.NotImplementedException();
        }

        public override string GetFormatFilterList()
        {
            return "M65 File (*.m65) *.m65";
        }

        public override string Name => "Mac65";

        public override void Dispose()
        {
            try
            {
                if (preferences != null)
                {
                    preferences.Dispose();
                    preferences = null;
                }
            }
            finally
            {
                base.Dispose();
            }
        }
    }
}