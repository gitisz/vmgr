using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr.VisualStudioAddin.Library;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Vmgr.VisualStudioAddin
{
    public class VmgrBuilderHandle
    {
        public const string VMGRTOOLSKEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Dominion\Vmgr\Packager";
        public const string VMGRTOOLSPATH = "VmgrToolsPath";

        private DTEHandler _DTEInstance = null;
        private string _vmgrToolsPath = null;
        private bool _running = false;

        public DTEHandler DTEInstance
        {
            get { return _DTEInstance; }
            set { _DTEInstance = value; }
        }

        public string VmgrToolsPath
        {
            get
            {
                if (_vmgrToolsPath == null)
                {
                    _vmgrToolsPath = Registry.GetValue(VMGRTOOLSKEY, VMGRTOOLSPATH, "") as string;
                }
                return _vmgrToolsPath;
            }
            set { _vmgrToolsPath = value; }
        }

        public bool Running
        {
            get { return _running; }
            set { _running = value; }
        }

        public VmgrBuilderHandle()
        {
        }

        public VmgrBuilderHandle(DTEHandler handler)
        {
            this.DTEInstance = handler;
        }

        public void BuildAsync(ProjectPaths projectPaths)
        {
            //string output = projectPaths.OutputPath;
            string[] param = new string[] { projectPaths.FullPath, "-TraceLevel information" };
            Log("Building V-Manager Package file!");

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync(param);
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] param = e.Argument as string[];
            Run(param[0], param[1]);
        }

        public void RunVmgrBuilder(string path, string arguments)
        {
            string[] param = new string[] { path, arguments };
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync(param);
        }

        public void Run(string path, string arguments)
        {
            try
            {
                Running = true;

                if (this.DTEInstance != null)
                {
                    this.DTEInstance.StartNewBuildWindow();
                }

                // Set up process info.
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = VmgrToolsPath + @"Vmgr.Packager.exe";
                psi.Arguments = arguments;
                psi.WorkingDirectory = path;
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

                if (!fStarted)
                    throw new Exception("Unable to start Vmgr.Packager.exe process.");

                while (!p.HasExited)
                {
                    string text = p.StandardOutput.ReadLine();
                   
                    if (!String.IsNullOrEmpty(text))
                    {
                        if (this.DTEInstance != null)
                        {
                            this.DTEInstance.WriteBuildWindow(text);
                        }
                    }
                    
                    System.Threading.Thread.Sleep(100);
                }

                if (this.DTEInstance != null)
                {
                    this.DTEInstance.WriteBuildWindow(p.StandardOutput.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                Running = false;
            }
        }

        private void Log(string message)
        {
            if (this.DTEInstance != null)
            {
                this.DTEInstance.WriteBuildAndStatusBar(message);
            }
        }
    }
}
