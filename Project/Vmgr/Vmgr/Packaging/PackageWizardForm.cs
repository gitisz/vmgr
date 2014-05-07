using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vmgr.Packaging
{
    public partial class PackageWizardForm : Form
    {
        private string packageName = string.Empty;
        private string packageDescription = string.Empty;

        public PackageWizardForm()
        {
            InitializeComponent();
        }

        internal string GetPackageName()
        {
            return this.packageName;
        }

        internal string GetPackageDescription()
        {
            return this.packageDescription;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxPackageName.Text))
            {
                this.packageName = this.textBoxPackageName.Text;
                this.packageDescription = this.textBoxPackageDescription.Text;

                this.Dispose();
            }
        }
    }
}
