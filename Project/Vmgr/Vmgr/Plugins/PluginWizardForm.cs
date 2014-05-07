using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vmgr.Plugins
{
    public partial class PluginWizardForm : Form
    {
        private string pluginName = string.Empty;
        private string pluginDescription = string.Empty;

        public PluginWizardForm()
        {
            InitializeComponent();
        }

        internal string GetPluginName()
        {
            return this.pluginName;
        }

        internal string GetPluginDescription()
        {
            return this.pluginDescription;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxPluginName.Text))
            {
                this.pluginName = this.textBoxPluginName.Text;
                this.pluginDescription = this.textBoxPluginDescription.Text;

                this.Dispose();
            }
        }
    }
}
