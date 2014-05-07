using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Deployment.WindowsInstaller;

namespace Vmgr.Wix.Action.Packager
{
    public class CustomActions
    {
        private static string INSTALLOCATION = string.Empty;

        [CustomAction]
        public static ActionResult OnInstallVmgrPackagerAction(Session session)
        {
            session.Log("Beginning OnInstallVmgrPackagerAction...");

#if DEBUG
            //System.Diagnostics.Debugger.Launch();
#endif

            CustomActions.INSTALLOCATION = session.CustomActionData["INSTALLOCATION"] + "Packager\\";

            if (!CustomActions.Install(session))
                return ActionResult.Failure;

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult OnUnInstallVmgrPackagerAction(Session session)
        {
            session.Log("Beginning OnUnInstallVmgrPackagerAction...");

#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif

            CustomActions.INSTALLOCATION = session.CustomActionData["INSTALLOCATION"] + "Packager\\";

            if (!CustomActions.UnInstall(session))
                return ActionResult.Failure;

            return ActionResult.Success;
        }

        private static void ChangeConnectionString(Session session)
        {
            string connectionString = string.Empty;
            string configurationFile = CustomActions.INSTALLOCATION + "Vmgr.Packager.exe.config";

            if (!string.IsNullOrEmpty(session.CustomActionData["SQL_CONNECTION_STRING"]))
            {
                connectionString = session.CustomActionData["SQL_CONNECTION_STRING"];
            }

            //  Write connection string to config file.
            XDocument document = XDocument.Load(configurationFile);
            XElement appSetttings = document
                .Elements("configuration")
                .Elements("appSettings")
                .FirstOrDefault()
                ;

            XElement vmgrConnectionStringSetting = appSetttings.Descendants()
                .Where(a => a.Attribute("key").Value == "VmgrConnectionString")
                .First()
                ;

            vmgrConnectionStringSetting.Attribute("value").Value = connectionString;

            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(configurationFile, xws))
            {
                document.WriteTo(xw);
            }
        }

        private static bool Install(Session session)
        {
            session.Message(InstallMessage.Progress, new Record { FormatString = "Changing V-Manager Packager configuration connection string..." });

            try
            {
                CustomActions.ChangeConnectionString(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to change VmgrConnectionString.  Exception: {0}", ex);

                return false;
            }

            return true;
        }

        private static void RemovePackagerFiles(Session session)
        {
            DirectoryInfo serviceDirectory = null;

            if (Directory.Exists(CustomActions.INSTALLOCATION))
            {
                serviceDirectory = new DirectoryInfo(CustomActions.INSTALLOCATION);

                foreach (FileInfo file in serviceDirectory.GetFiles())
                    file.Delete();

                foreach (DirectoryInfo dir in serviceDirectory.GetDirectories())
                    dir.Delete(true);
            }
        }

        private static bool UnInstall(Session session)
        {
            try
            {
                CustomActions.RemovePackagerFiles(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to remove Vmgr.Packager files.  Exception: {0}", ex);

                return false;
            }

            return true;

        }
    }
}
