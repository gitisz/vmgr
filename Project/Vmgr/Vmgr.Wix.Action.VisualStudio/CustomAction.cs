using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32;

namespace Vmgr.Wix.Action.VisualStudio
{
    public class CustomActions
    {
        private const string ITEMTEMPLATES_NAME = "ItemTemplates";
        private const string PROJECTTEMPLATES_NAME = "ProjectTemplates";

        private static string INSTALLOCATION = string.Empty;

        [CustomAction]
        public static ActionResult OnInstallVisualStudioAction(Session session)
        {
            session.Log("Beginning OnInstallVisualStudioAction...");

#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif

            CustomActions.INSTALLOCATION = session.CustomActionData["INSTALLOCATION"] + "VisualStudio\\";

            session.Message(InstallMessage.Progress, new Record { FormatString = "Deploying Visual Studio 2010 V-Manager templates and add-in..." });

            if (!CustomActions.Install(session, "10.0"))
                return ActionResult.Failure;

            session.Message(InstallMessage.Progress, new Record { FormatString = "Deploying Visual Studio 2012 V-Manager templates and add-in..." });

            if (!CustomActions.Install(session, "11.0"))
                return ActionResult.Failure;

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult OnUnInstallVisualStudioAction(Session session)
        {
            session.Log("Beginning OnUnInstallVisualStudioAction...");

#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif

            CustomActions.INSTALLOCATION = session.CustomActionData["INSTALLOCATION"] + "VisualStudio\\";

            if (!CustomActions.UnInstall(session))
                return ActionResult.Failure;

            return ActionResult.Success;
        }

        private static void CallInstallTemplates(string vsinstalldir)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = vsinstalldir + "devenv.exe";
            psi.Arguments = "/InstallVsTemplates";
            psi.WorkingDirectory = vsinstalldir;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // Create the process.
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            // Associate process info with the process.
            p.StartInfo = psi;

            // Run the process.
            bool fStarted = p.Start();

            if (fStarted)
            {
                p.WaitForExit();
            }
            else
            {
                throw new ApplicationException("Unable to start Devenv.exe process.");
            }
        }

        private static void CopyTemplates(string vsversion, string templateTargetPath, string templateSourcePath, string templateName)
        {
            string targetCSharpPath = String.Format(templateTargetPath, "CSharp");

            // Item templates
            Directory.CreateDirectory(targetCSharpPath);

            string[] templatefilelist = Directory.GetFiles(templateSourcePath, "*.zip");

            List<string> templateinstalledlist = new List<string>();

            foreach (string filename in templatefilelist)
            {
                FileInfo fi = new FileInfo(filename);

                string filePath = targetCSharpPath + @"\" + fi.Name;
                File.Copy(filename, filePath, true);
                templateinstalledlist.Add(filePath);
            }
        }

        private static string GetInstallDir(string vsversion)
        {
            string vsinstalldir = null;

            if (Environment.Is64BitOperatingSystem)
            {
                vsinstalldir = (string)Registry.GetValue(
                   string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Microsoft\\VisualStudio\\{0}\\", vsversion),
                    "InstallDir",
                    null);
            }
            else
            {
                vsinstalldir = (string)Registry.GetValue(
                    string.Format("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\VisualStudio\\{0}\\", vsversion),
                    "InstallDir",
                    null
                    );
            }

            return vsinstalldir;
        }

        private static bool Install(Session session, string vsversion)
        {
            try
            {
                string vsinstalldir = CustomActions.GetInstallDir(vsversion);

                if (!String.IsNullOrEmpty(vsinstalldir))
                {
                    string sourcePath = Path.GetDirectoryName(CustomActions.INSTALLOCATION);

                    // Copy item templates
                    string templateTargetPath = vsinstalldir + @"ItemTemplates\{0}\Vmgr"; //CSharp
                    string templateSourcePath = sourcePath + @"\" + ITEMTEMPLATES_NAME;
                    string templateName = ITEMTEMPLATES_NAME;

                    CustomActions.CopyTemplates(vsversion, templateTargetPath, templateSourcePath, templateName);

                    // Copy project templates
                    templateTargetPath = vsinstalldir + @"ProjectTemplates\{0}\Vmgr";
                    templateSourcePath = sourcePath + @"\" + PROJECTTEMPLATES_NAME;
                    templateName = PROJECTTEMPLATES_NAME;

                    CustomActions.CopyTemplates(vsversion, templateTargetPath, templateSourcePath, templateName);

                    // Update Visual Studio
                    CustomActions.CallInstallTemplates(vsinstalldir);
                }
            }
            catch (Exception ex)
            {
                session.Log("Failed to install Visual Studio Addon. Version: {0}. Exception: {1}", vsversion, ex);

                return false;
            }

            return true;
        }

        private static void RemoveVisualStudioFiles(Session session)
        {
            DirectoryInfo visualStudioDirectory = null;

            if (Directory.Exists(CustomActions.INSTALLOCATION))
            {
                visualStudioDirectory = new DirectoryInfo(CustomActions.INSTALLOCATION);

                foreach (FileInfo file in visualStudioDirectory.GetFiles())
                    file.Delete();

                //foreach (DirectoryInfo dir in visualStudioDirectory.GetDirectories())
                //    dir.Delete(true);
            }

            DirectoryInfo visualStudioAddinDirectory = new DirectoryInfo("%ALLUSERSPROFILE%\\Application Data\\microsoft\\MSEnvShared\\Addins");
            DirectoryInfo visualStudio10ItemTemplatesDirectory = new DirectoryInfo(CustomActions.GetInstallDir("10.0") + @"ItemTemplates\CSharp\Vmgr");
            DirectoryInfo visualStudio10ProjectTemplatesDirectory = new DirectoryInfo(CustomActions.GetInstallDir("10.0") + @"ProjectTemplates\CSharp\Vmgr");
            DirectoryInfo visualStudio11ItemTemplatesDirectory = new DirectoryInfo(CustomActions.GetInstallDir("11.0") + @"ItemTemplates\CSharp\Vmgr");
            DirectoryInfo visualStudio11ProjectTemplatesDirectory = new DirectoryInfo(CustomActions.GetInstallDir("11.0") + @"ProjectTemplates\CSharp\Vmgr");

            if (visualStudioAddinDirectory.Exists)
                foreach (FileInfo file in visualStudioAddinDirectory.GetFiles("Vmgr.*"))
                    file.Delete();

            if (visualStudio10ItemTemplatesDirectory.Exists)
                foreach (FileInfo file in visualStudio10ItemTemplatesDirectory.GetFiles())
                    file.Delete();

            if (visualStudio10ProjectTemplatesDirectory.Exists)
                foreach (FileInfo file in visualStudio10ProjectTemplatesDirectory.GetFiles())
                    file.Delete();

            if (visualStudio11ItemTemplatesDirectory.Exists)
                foreach (FileInfo file in visualStudio11ItemTemplatesDirectory.GetFiles())
                    file.Delete();

            if (visualStudio11ProjectTemplatesDirectory.Exists)
                foreach (FileInfo file in visualStudio11ProjectTemplatesDirectory.GetFiles())
                    file.Delete();

        }

        private static bool UnInstall(Session session)
        {
            try
            {
                CustomActions.RemoveVisualStudioFiles(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to remove Vmgr.VisualStudio files.  Exception: {0}", ex);

                return false;
            }

            return true;

        }
    }
}
