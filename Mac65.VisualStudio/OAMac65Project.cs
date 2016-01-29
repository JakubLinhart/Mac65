using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project.Automation;

namespace Mac65.VisualStudio
{
    [ComVisible(true)]
    public class OAMac65Project : OAProject
    {
        public OAMac65Project(Mac65ProjectNode project) : base(project)
        {
        }
    }
}