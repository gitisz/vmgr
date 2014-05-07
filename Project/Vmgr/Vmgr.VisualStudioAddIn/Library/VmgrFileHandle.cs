using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vmgr.VisualStudioAddin.Library
{
    public class VmgrFileHandle
    {
        private ProjectPaths _projectFolder = null;
        private string _vmgrFilename = null;
        private FileInfo _vmgrFileInfo = null;

        public ProjectPaths SelectedProject
        {
            get { return _projectFolder; }
            set { _projectFolder = value; }
        }

        public string VmgrFilename
        {
            get
            {
                if (_vmgrFilename == null)
                {
                    if (this.SelectedProject != null && this.SelectedProject.DirInfo != null)
                    {

                        string wspFilePath = this.SelectedProject.DirInfo.FullName + @"\" + this.SelectedProject.DirInfo.Name + ".vmgx";
                       
                        if (File.Exists(wspFilePath))
                        {
                            _vmgrFilename = wspFilePath;
                        }
                        else
                        {
                            if (Directory.Exists(this.SelectedProject.DirInfo.FullName))
                            {
                                DirectoryInfo projectDir = new DirectoryInfo(this.SelectedProject.DirInfo.FullName);
                                foreach (FileInfo file in projectDir.GetFiles("*.vmgx"))
                                {
                                    _vmgrFilename = file.FullName;
                                    break;
                                }
                            }
                        }

                    }
                }
                return _vmgrFilename;
            }
            set { _vmgrFilename = value; }
        }

        public FileInfo VmgrFileInfo
        {
            get
            {
                if (_vmgrFileInfo == null)
                {
                    if (this.VmgrFilename != null)
                    {
                        _vmgrFileInfo = new FileInfo(this.VmgrFilename);
                    }
                }
                return _vmgrFileInfo;
            }
            set { _vmgrFileInfo = value; }
        }

        public VmgrFileHandle(DTEHandler dteHandler)
        {
            this.SelectedProject = new ProjectPaths(dteHandler.SelectedProject);
        }

        public bool ProjectFilesHaveChanged()
        {
            return CheckForNewerFiles(this.SelectedProject.DirInfo);
        }

        private bool CheckForNewerFiles(DirectoryInfo parentInfo)
        {
            DateTime wspLastWriteTime = this.VmgrFileInfo.LastWriteTime;

            foreach (FileInfo file in parentInfo.GetFiles("*.cs"))
            {
                if (wspLastWriteTime < file.LastWriteTime)
                {
                    return true;
                }
            }

            foreach (FileInfo file in parentInfo.GetFiles("*.xml"))
            {
                if (wspLastWriteTime < file.LastWriteTime)
                {
                    return true;
                }
            }

            foreach (DirectoryInfo child in parentInfo.GetDirectories())
            {
                if (CheckForNewerFiles(child))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
