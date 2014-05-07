using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using System.Diagnostics;

namespace Vmgr.Packaging
{
    public class PackageWizard : IWizard
    {
        private PackageWizardForm packageWizardForm;
        private string packageName = string.Empty;
        private string packageDescription = string.Empty;
        
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject
            , Dictionary<string, string> replacementsDictionary
            , WizardRunKind runKind
            , object[] customParams)
        {
            try
            {
                // Display a form to the user. The form collects 
                // input for the custom message.
                packageWizardForm = new PackageWizardForm();
                packageWizardForm.ShowDialog();

                packageName = packageWizardForm.GetPackageName();
                packageDescription = packageWizardForm.GetPackageDescription();

                // Add custom parameters.
                replacementsDictionary.Add("$packagename$", packageName);
                replacementsDictionary.Add("$packagedesc$", packageDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
