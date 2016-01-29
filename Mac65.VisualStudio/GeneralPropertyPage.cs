using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;

namespace Mac65.VisualStudio
{
    [ComVisible(true)]
    [Guid("62854DB7-5FAC-4191-9182-6AD5F8677D3E")]
    public class GeneralPropertyPage : SettingsPage
    {
        public GeneralPropertyPage()
        {
            Name = "General";
        }

        [Description("Mac65 Test Description")]
        [DisplayName(@"Mac65 Test")]
        [Category("Mac65")]
        public string Test { get; set; }

        protected override void BindProperties()
        {
            Test = ProjectMgr.GetProjectProperty("Test", false);
        }

        protected override int ApplyChanges()
        {
            ProjectMgr.SetProjectProperty("Test", Test);

            IsDirty = false;
            return VSConstants.S_OK;
        }
    }
}
