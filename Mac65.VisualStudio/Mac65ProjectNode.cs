using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;
using Microsoft.VisualStudio.Shell;
using VSLangProj;

namespace Mac65.VisualStudio
{
    [Guid("18F12F97-418B-4115-B65C-4EEF6E94BBBB")]
    public class Mac65ProjectNode : ProjectNode
    {
        private readonly Package package;
        private VSProject vsProject;

        public Mac65ProjectNode(Package package)
        {
            this.package = package;
        }

        public override bool IsCodeFile(string fileName)
        {
            return fileName.ToLower().EndsWith(".m65");
        }

        protected override void ProcessReferences()
        {
        }

        public override Guid ProjectGuid => typeof (Mac65ProjectFactory).GUID;
        public override string ProjectType => "Mac65";

        protected override ConfigProvider CreateConfigProvider()
        {
            return new Mac65ConfigProvider(this);
        }

        public override object GetAutomationObject()
        {
            return new OAMac65Project(this);
        }

        public override FileNode CreateFileNode(ProjectElement item)
        {
            var node = new Mac65ProjectFileNode(this, item);

            node.OleServiceProvider.AddService(typeof(EnvDTE.Project), new OleServiceProvider.ServiceCreatorCallback(CreateServices), false);
            node.OleServiceProvider.AddService(typeof(ProjectItem), node.ServiceCreator, false);
            node.OleServiceProvider.AddService(typeof(VSProject), new OleServiceProvider.ServiceCreatorCallback(CreateServices), false);

            return node;
        }

        protected internal VSProject VSProject
        {
            get
            {
                if (vsProject == null)
                {
                    vsProject = new OAVSProject(this);
                }

                return vsProject;
            }
        }

        private object CreateServices(Type serviceType)
        {
            object service = null;
            if (typeof(VSProject) == serviceType)
            {
                service = VSProject;
            }
            else if (typeof(EnvDTE.Project) == serviceType)
            {
                service = GetAutomationObject();
            }
            return service;
        }


        protected override int QueryStatusOnNode(Guid cmdGroup, uint cmd, IntPtr pCmdText, ref QueryStatusResult result)
        {
            if ((VSConstants.VSStd2KCmdID)cmd == VSConstants.VSStd2KCmdID.ADDREFERENCE)
            {
                result |= QueryStatusResult.NOTSUPPORTED | QueryStatusResult.INVISIBLE;
                return VSConstants.S_OK;
            }

            return base.QueryStatusOnNode(cmdGroup, cmd, pCmdText, ref result);
        }

        protected override Guid[] GetConfigurationIndependentPropertyPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(GeneralPropertyPage).GUID;
            return result;
        }

        protected override Guid[] GetPriorityProjectDesignerPages()
        {
            Guid[] result = new Guid[1];
            result[0] = typeof(GeneralPropertyPage).GUID;
            return result;
        }
    }
}
