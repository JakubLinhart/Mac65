using Microsoft.VisualStudio.Project;

namespace Mac65.VisualStudio
{
    public class Mac65ConfigProvider : ConfigProvider
    {
        private readonly Mac65ProjectNode manager;

        public Mac65ConfigProvider(Mac65ProjectNode manager) : base(manager)
        {
            this.manager = manager;
        }

        protected override ProjectConfig CreateProjectConfiguration(string configName)
        {
            return new Mac65ProjectConfig(manager, configName);
        }
    }
}