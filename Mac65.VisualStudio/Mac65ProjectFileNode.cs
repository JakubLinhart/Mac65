using System;
using EnvDTE;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Project.Automation;

namespace Mac65.VisualStudio
{
    public class Mac65ProjectFileNode : FileNode
    {
        private OAMac65ProjectFileItem automationObject;

        public Mac65ProjectFileNode(ProjectNode root, ProjectElement element) : base(root, element)
        {
        }

        public override object GetAutomationObject()
        {
            if (automationObject == null)
            {
                automationObject = new OAMac65ProjectFileItem(this.ProjectMgr.GetAutomationObject() as OAProject, this);
            }

            return automationObject;
        }

        internal OleServiceProvider.ServiceCreatorCallback ServiceCreator => CreateServices;

        private object CreateServices(Type serviceType)
        {
            object service = null;
            if (typeof (ProjectItem) == serviceType)
            {
                service = GetAutomationObject();
            }
            return service;
        }
    }
}