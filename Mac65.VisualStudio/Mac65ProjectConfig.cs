using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;

namespace Mac65.VisualStudio
{
    public class Mac65ProjectConfig : ProjectConfig
    {
        public Mac65ProjectConfig(Mac65ProjectNode manager, string configName) : base(manager, configName)
        {
        }

        public override int QueryDebugLaunch(uint flags, out int fCanLaunch)
        {
            fCanLaunch = 1;
            return VSConstants.S_OK;
        }

        public override int DebugLaunch(uint grfLaunch)
        {
            throw new NotImplementedException();
        }
    }
}