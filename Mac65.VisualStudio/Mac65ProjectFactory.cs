using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;
using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Mac65.VisualStudio
{
    [Guid("BD9B900B-FDDD-4578-AD6A-9652C5FF55D5")]
    public class Mac65ProjectFactory : ProjectFactory
    {
        private readonly Package package;

        public Mac65ProjectFactory(Package package) : base(package)
        {
            this.package = package;
        }

        protected override ProjectNode CreateProject()
        {
            var project = new Mac65ProjectNode(this.package);
            project.SetSite((IServiceProvider)((System.IServiceProvider)this.package).GetService(typeof(IServiceProvider)));
            return project;
        }
    }
}