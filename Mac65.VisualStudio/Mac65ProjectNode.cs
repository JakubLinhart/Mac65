using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;

namespace Mac65.VisualStudio
{
    [Guid("18F12F97-418B-4115-B65C-4EEF6E94BBBB")]
    public class Mac65ProjectNode : ProjectNode
    {
        private readonly Package package;

        public Mac65ProjectNode(Package package)
        {
            this.package = package;
        }

        public override bool IsCodeFile(string fileName)
        {
            return fileName.ToLower().EndsWith(".m65");
        }

        public override Guid ProjectGuid => typeof (Mac65ProjectFactory).GUID;
        public override string ProjectType => "Mac65";
    }
}
