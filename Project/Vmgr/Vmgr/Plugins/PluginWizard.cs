using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using System.Diagnostics;

namespace Vmgr.Plugins
{
    public class PluginWizard : IWizard
    {
        private PluginWizardForm pluginWizardForm;
        private string pluginName = string.Empty;
        private string pluginDescription = string.Empty;
        
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
                pluginWizardForm = new PluginWizardForm();
                pluginWizardForm.ShowDialog();

                pluginName = pluginWizardForm.GetPluginName();
                pluginDescription = pluginWizardForm.GetPluginDescription();

                // Add custom parameters.
                replacementsDictionary.Add("$pluginname$", pluginName);
                replacementsDictionary.Add("$plugindesc$", pluginDescription);
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
