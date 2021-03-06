﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using EnvDTE80;
using EnvDTE;
using Vmgr.VisualStudioAddin.Library.Menu;
using System.Diagnostics;

namespace Vmgr.VisualStudioAddin.Library
{
    public class DTEHandler
    {
        [DllImport("ole32.dll", EntryPoint = "GetRunningObjectTable")]
        static extern uint GetRunningObjectTable(uint res, out UCOMIRunningObjectTable ROT);
        
        [DllImport("ole32.dll", EntryPoint = "CreateBindCtx")]
        static extern uint CreateBindCtx(uint res, out UCOMIBindCtx ctx);

        private DTE2 _application = null;
        private AddIn _addInInstance;
        private OutputWindow _applicationOutputWindow = null;
        private VSMenuHandler _menu = null;
        private OutputWindowPane _buildWindow = null;
        private bool _runningCommand = false;

        public DTE2 Application
        {
            get { return _application; }
            set { _application = value; }
        }

        public AddIn AddInInstance
        {
            get { return _addInInstance; }
            set { _addInInstance = value; }
        }

        public OutputWindow ApplicationOutputWindow
        {
            get
            {
                if (_applicationOutputWindow == null)
                {
                    _applicationOutputWindow = this.Application.ToolWindows.OutputWindow;
                }
                return _applicationOutputWindow;
            }
            set { _applicationOutputWindow = value; }
        }

        public VSMenuHandler Menu
        {
            get { return _menu; }
            set { _menu = value; }
        }

        public OutputWindowPane BuildWindow
        {
            get
            {
                if (_buildWindow == null)
                {
                    // Add-in code.
                    // Create a reference to the Output window.
                    // Create a tool window reference for the Output window
                    // and window pane.
                    string buildName = CommandBarStrings.GetName("Build", this.Application);
                    _buildWindow = ApplicationOutputWindow.OutputWindowPanes.Item(buildName);
                }
                return _buildWindow;
            }
            set { _buildWindow = value; }
        }

        public SelectedItem FirstSelectedItem
        {
            get
            {
                SelectedItem item = null;
                if (Application.SelectedItems.Count > 0)
                {
                    item = Application.SelectedItems.Item(1);
                }
                //UIHierarchy UIH = Application.ToolWindows.SolutionExplorer;
                //return  (UIHierarchyItem)((System.Array)UIH.SelectedItems).GetValue(0);
                return item;
            }
        }

        public Project SelectedProject
        {
            get
            {
                return this.GetProject(FirstSelectedItem);
            }
        }

        public bool RunningCommand
        {
            get { return _runningCommand; }
            set { _runningCommand = value; }
        }

        public DTEHandler(DTE2 application, AddIn addInInst)
        {
            this.Application = application;
            this.AddInInstance = addInInst;
            this.Menu = new VSMenuHandler(this.Application, this.AddInInstance);
        }

        /// <summary>
        /// Get all Projects from Solution
        /// </summary>
        /// <returns></returns>
        public List<Project> GetAllProjectOfSolution()
        {
            List<Project> prj = new List<Project>();
            if (_application == null || _application.Solution == null ||
            _application.Solution.Projects == null)
                return prj;
            foreach (Project p in _application.Solution.Projects)
                prj.Add(p);

            return prj;
        }


        public bool IsWSPBuilderProject()
        {
            bool found = true;
            try
            {
            }
            catch (Exception ex)
            {
                // If something went wrong, the folder wasn't found.
                Debug.Write(ex.ToString());

            }

            return found;
        }

        /// <summary>
        /// Clears the build window and then activates it
        /// </summary>
        public void StartNewBuildWindow()
        {
            this.BuildWindow.Clear();
            this.BuildWindow.Activate();
            this.ApplicationOutputWindow.Parent.Activate();
        }

        public void WriteBuildWindow(string text)
        {

            if (this.Application != null)
            {
                this.BuildWindow.OutputString(text + "\r\n");
            }
        }


        public void StatusBar(string message)
        {
            if (this.Application != null)
            {
                this.Application.StatusBar.Text = message;
            }
        }

        public void WriteBuildAndStatusBar(string message)
        {
            this.WriteBuildWindow(message);
            this.StatusBar(message);
        }

        public Project GetProject(SelectedItem item)
        {
            Project result = null;
            if (item != null)
            {
                if (item.ProjectItem != null)
                {
                    result = item.ProjectItem.ContainingProject;
                }
                else
                {
                    result = item.Project;
                }
            }
            return result;
        }
    }
}
