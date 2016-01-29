using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;

namespace Mac65.VisualStudio
{
    [Guid(PackageGuid)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\12.0")]
    [ProvideService(typeof (Mac65Language), ServiceName = Mac65Language.LanguageName)]
    [ProvideLanguageService(typeof (Mac65Language), Mac65Language.LanguageName, 100, CodeSense = true,
        DefaultToInsertSpaces = false, EnableCommenting = true, MatchBraces = true, MatchBracesAtCaret = true,
        ShowCompletion = true, ShowMatchingBrace = true, QuickInfo = true, AutoOutlining = true)]
    [ProvideLanguageExtension(typeof (Mac65Language), Mac65Language.FileExtension)]
    [ProvideProjectFactory(typeof (Mac65ProjectFactory), "Mac65", "Mac65 Project Files (*.m65proj);*.m65proj", "m65proj",
        "m65proj", @"Templates\Projects", LanguageVsTemplate = "Mac65", NewProjectRequireNewFolderVsTemplate = false)]
    [ProvideProjectItem(typeof(Mac65ProjectFactory), "Mac65 Items", @"Templates\ProjectItems\Mac65", 500)]
    [ProvideObject(typeof(GeneralPropertyPageAdapter))]
    public sealed class Mac65Package : ProjectPackage
    {
        public const string PackageGuid = "8173272D-8BE3-4B85-9254-A5DCBC202312";

        public override string ProductUserContext => "Mac65Proj";

        protected override void Initialize()
        {
            base.Initialize();

            RegisterProjectFactory(new Mac65ProjectFactory(this));
        }
    }
}