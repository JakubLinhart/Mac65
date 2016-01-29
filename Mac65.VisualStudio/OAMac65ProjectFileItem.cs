using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;

namespace Mac65.VisualStudio
{
    [ComVisible(true)]
    [Guid("F3F6DC0E-05EE-4C0D-97BA-4E28AA971805")]

    internal class OAMac65ProjectFileItem : OAFileItem
    {
        public OAMac65ProjectFileItem(OAProject project, FileNode node) : base(project, node)
        {
        }
    }
}