using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EnvDTE;
using Vmgr.VisualStudioAddin.Library;

namespace Vmgr.VisualStudioAddin
{
    public class ProjectPaths
    {
        private Project _selectedProject = null;
        private string _fullPath = null;
        private DirectoryInfo _dirInfo = null;
        private string _outputPath = null;

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set { _selectedProject = value; }
        }

        public string FullPath
        {
            get
            {
                if (_fullPath == null)
                {
                    if (SelectedProject != null)
                    {
                        _fullPath = SelectedProject.Properties.Item("FullPath").Value as string;
                    }
                }
                return _fullPath;
            }
            set { _fullPath = value; }
        }

        public DirectoryInfo DirInfo
        {
            get
            {
                if (_dirInfo == null)
                {
                    if (Directory.Exists(this.FullPath))
                    {
                        _dirInfo = new DirectoryInfo(this.FullPath);
                    }
                }
                return _dirInfo;
            }
            set { _dirInfo = value; }
        }

        public string OutputPathDLL
        {
            get
            {
                if (_outputPath == null)
                {
                    _outputPath = Utility.CombinePaths(this.FullPath, this.SelectedProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value as string);
                }
                return _outputPath;
            }
        }

        public string OutputFilename
        {
            get
            {
                string result = "";
                if (this.SelectedProject != null)
                {
                    result = this.SelectedProject.Properties.Item("OutputFileName").Value.ToString();
                }
                return result;
            }
        }

        public string OutputFilePath
        {
            get
            {
                return Path.Combine(this.OutputPathDLL, this.OutputFilename);
            }
        }

        public ProjectPaths(Project project)
        {
            this.SelectedProject = project;
        }
    }
}
