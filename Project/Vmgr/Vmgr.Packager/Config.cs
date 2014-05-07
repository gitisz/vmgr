using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using System.Configuration;

namespace Vmgr.Packager
{
    [Category("Configuration")]
    public sealed class Config
    {
        private bool? _buildVmgr = null;
        private string _packagePath = string.Empty;
        private string _outputPath = string.Empty;
        private string _includefiles = string.Empty;
        private string _excludefiles = string.Empty;
        private bool? _buildSolution = null;
        private string _projectPath = string.Empty;
        private bool? _uploadPackage = null;
        private List<string> _includefiletypes = null;
        private List<string> _excludefiletypes = null;
        private List<string> _excludepaths = null;
        private ArgumentParameters _arguments = null;
        private StringDictionary _includeFilesDictionary = null;
        private static Config _current = new Config();
        private StringDictionary _excludeFilesDictionary = null;
        private string DEFAULT_OUTPUTPATH = "";
        private string DEFAULT_SOLUTIONPATH = "";
        private BuildModeType DEFAULT_BUILDMODE = BuildModeType.Default;
        private string DEFAULT_PROJECTDLLPATHPART = @"\bin\release";
        public const string BUILDVMGR = "BuildVmgr";
        public const string OUTPUTPATH = "OutputPath";
        public const string INCLUDEFILES = "Includefiles";
        public const string EXCLUDEFILES = "Excludefiles";
        public const string INCLUDEFILETYPES = "Includefiletypes";
        public const string EXCLUDEFILETYPES = "Excludefiletypes";
        public const string EXCLUDEPATHS = "Excludepaths";
        public const string BUILDSOLUTION = "BuildSolution";
        public const string PROJECTPATH = "ProjectPath";
        public const string UPLOADPACKAGE = "UploadPackage";

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Config()
        {
        }

        public Config()
        {
            //Pre-set some arguments acording to the default build location of the WSP
            PreInitializeArguments();

            // Parse the App.config
            ParseAppConfig();

            // Parse the commandline.
            Arguments.Parse(Environment.CommandLine, " ");
        }

        public static Config Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
            }
        }

        public ArgumentParameters Arguments
        {
            get
            {
                if (_arguments == null)
                {
                    _arguments = new ArgumentParameters();
                }
                return _arguments;
            }
        }

        [DisplayName("-BuildVmgr [True|False] (Default is true.)")]
        [Description("If set to true then a V-Manager package will be generated.")]
        public bool BuildVmgr
        {
            get
            {
                if (_buildVmgr == null)
                {
                    _buildVmgr = GetBool(BUILDVMGR, true);
                }
                return (bool)_buildVmgr;
            }
            set
            {
                _buildVmgr = value;
            }
        }

        [DisplayName("-PackagePath [filepath] (Default value is current package path)")]
        [Description("Defines the path to Package.xml.")]
        public string PackagePath
        {
            get
            {
                if (String.IsNullOrEmpty(_packagePath))
                {
                    _packagePath = Config.Current.PackagePath + @"\Package.xml";

                }
                return _packagePath;
            }
            set { _packagePath = value; }
        }

        [DisplayName("-Outputpath [Path] (Default is the current directory)")]
        [Description("Specifies where the Vmgx file is saved.")]
        public string OutputPath
        {
            get
            {
                if (string.IsNullOrEmpty(_outputPath))
                {
                    _outputPath = GetString(OUTPUTPATH, string.Empty);
                    if (string.IsNullOrEmpty(_outputPath))
                    {
                        _outputPath = DEFAULT_OUTPUTPATH;
                    }
                    _outputPath = FileProvider.GetCleanDirectoryPath(_outputPath);
                }
                return _outputPath;
            }
            set
            {
                _outputPath = value;
            }
        }

        [DisplayName("-Includefiles [filename]")]
        [Description("The VmgrBuilder will only include files in the package, if they are specified in the file provided.\r\nTo create a file list first use the -CreateVmgrFileList argument.")]
        public string Includefiles
        {
            get
            {
                if (string.IsNullOrEmpty(_includefiles))
                {
                    _includefiles = GetString(INCLUDEFILES, "");
                }
                return _includefiles;
            }
            set { _includefiles = value; }
        }

        [DisplayName("-Excludefiles [filename]")]
        [Description("The VmgrBuilder will exclude files from the package, if they are specified in the file provided.\r\nTo create a file list first use the -CreateVmgrFileList argument.")]
        public string Excludefiles
        {
            get
            {
                if (string.IsNullOrEmpty(_excludefiles))
                {
                    _excludefiles = GetString(EXCLUDEFILES, "");
                }
                return _excludefiles;
            }
            set { _excludefiles = value; }
        }

        [DisplayName("-Includefiletypes [*|xml,gif,jpg, ...] (Default is asterisk '*' meaning every file type. Use comma as a separator.)")]
        [Description("Specifies which file types to include in the Package.xml and Vmgx file.\t\rIf the file type if not defined in the Includefiletypes or in the Excludefiletypes arguments then it is always included.\t\rThe Includefiletypes always overrules the Exludefiletypes argument.\t\rDefine more than one file type by using comma.")]
        public List<string> Includefiletypes
        {
            get
            {
                if (_includefiletypes == null)
                {
                    string typeString = GetString(INCLUDEFILETYPES, "*");

                    string[] types = typeString.Split(',');

                    _includefiletypes = new List<string>(types);
                }
                return _includefiletypes;
            }
            set
            {
                _includefiletypes = value;
            }
        }

        [DisplayName("-Excludefiletypes [\"cs,scc, ...\"] (Default is 'cs,scc'. Use comma as a separator.)")]
        [Description("Specifies which file types to exclude in the Package.xml and Vmgx file.\t\rDefine more than one file type by using comma.\t\rIt is possible to use asterisk '*' to exclude every file type, except those defined in the Includefiletypes argument.")]
        public List<string> Excludefiletypes
        {
            get
            {
                if (_excludefiletypes == null)
                {
                    string typeString = GetString(EXCLUDEFILETYPES, "cs,scc");

                    string[] types = typeString.Split(',');

                    _excludefiletypes = new List<string>(types);
                }

                return _excludefiletypes;
            }
            set
            {
                _excludefiletypes = value;
            }
        }

        [DisplayName("-Excludepaths [\"path1;path2\"] (Default is empty. Use semicolon as a separator.)")]
        [Description("Specifies which paths to exclude and not include in the building process.")]
        public List<string> Excludepaths
        {
            get
            {
                if (_excludepaths == null)
                {
                    string typeString = GetString(EXCLUDEPATHS, null);
                    List<string> list = new List<string>();

                    if (typeString != null)
                    {
                        string[] paths = typeString.Split(';');
                        foreach (string path in paths)
                        {

                            DirectoryInfo dir = new DirectoryInfo(path);
                            list.Add(dir.FullName);

                        }
                    }

                    _excludepaths = list;
                }

                return _excludepaths;
            }
            set
            {
                _excludepaths = value;
            }
        }

        [DisplayName("-BuildSolution [True|False] (Default is false)")]
        [Description("Specifies that multiple projects have to be build into a single Vmgx file. All the projects have to be the subfolders of the solution folder specified with -SolutionPath")]
        public bool BuildSolution
        {
            get
            {
                if (_buildSolution == null)
                {
                    _buildSolution = GetBool(BUILDSOLUTION, false);
                }
                return (bool)_buildSolution;
            }
            set
            {
                _buildSolution = value;
            }
        }

        [DisplayName("-ProjectPath [Path] (Default is the current SolutionPath directory)")]
        [Description("Specifies where the project is located.")]
        public string ProjectPath
        {
            get
            {
                if (string.IsNullOrEmpty(_projectPath))
                {
                    // Try to get the path from the configuration
                    _projectPath = GetString(PROJECTPATH, "");

                    if (string.IsNullOrEmpty(_projectPath))
                    {
                        // Use the Default solution path 
                        _projectPath = DEFAULT_SOLUTIONPATH;
                    }

                    // Ensure that the path exists now and remove tailing backslash
                    _projectPath = FileProvider.GetCleanDirectoryPath(_projectPath);

                }
                return _projectPath;
            }
            set
            {
                _projectPath = value;
            }

        }

        [DisplayName("-UploadPackage [True|False] (Default is false.)")]
        [Description("If set to true then a V-Manager package will be uploaded locally.")]
        public bool UploadPackage
        {
            get
            {
                if (_uploadPackage == null)
                {
                    _uploadPackage = GetBool(UPLOADPACKAGE, false);
                }
                return (bool)_uploadPackage;
            }
            set
            {
                _uploadPackage = value;
            }
        }

        /// <summary>
        /// A list of files that are allowed into the WSP package.
        /// </summary>
        public StringDictionary IncludeFilesDictionary
        {
            get
            {
                if (_includeFilesDictionary == null)
                {
                    _includeFilesDictionary = LoadVmgrFileList(this.Includefiles);
                }
                return _includeFilesDictionary;
            }
            set { _includeFilesDictionary = value; }
        }


        /// <summary>
        /// A list of files that are not allowed into the WSP package.
        /// </summary>
        public StringDictionary ExcludeFilesDictionary
        {
            get
            {
                if (_excludeFilesDictionary == null)
                {
                    _excludeFilesDictionary = LoadVmgrFileList(this.Excludefiles);
                }
                return _excludeFilesDictionary;
            }
            set { _excludeFilesDictionary = value; }
        }

        /// <summary>
        /// Gets a value for the specified key. A default value is defined in case that the argument has not been defined.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string key, string defaultValue)
        {
            string value = null;

            if (Arguments.ContainsKey(key))
            {
                value = Arguments[key];
            }

            if (String.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Gets a value for the specified key. A default value is defined in case that the argument has not been defined.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBool(string key, bool defaultValue)
        {
            string value = this.GetString(key, defaultValue.ToString());

            bool result = defaultValue;

            bool success = bool.TryParse(value, out result);

            if (!success)
            {
                throw new ApplicationException("Invalid value '" + value + "' for key '" + key + "'. The value has to be 'true' or 'false'.");
            }

            return result;
        }

        /// <summary>
        /// Gets a value for the specified key. A default value is defined in case that the argument has not been defined.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Guid GetGuid(string key, Guid defaultValue)
        {
            string value = this.GetString(key, defaultValue.ToString());

            Guid result = Guid.Empty;
            try
            {
                result = new Guid(value);
            }
            catch
            {
                throw new ApplicationException("Invalid value '" + value + "' for key '" + key + "'. The value has to be a valid GUID.");
            }
            return result;
        }

        /// <summary>
        /// Loads the file list and gets the Full path if the filename is relative.
        /// </summary>
        /// <param name="filename">The txt file containing the files paths</param>
        /// <returns>StringDictionary of file paths</returns>
        private StringDictionary LoadVmgrFileList(string filename)
        {
            StringDictionary filelist = new StringDictionary();

            if (!String.IsNullOrEmpty(filename))
            {
                // Load the string lines into the Dictionary
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (sr.Peek() >= 0)
                    {
                        string filepath = sr.ReadLine();
                        // Check if the path is absolute
                        if (!Path.IsPathRooted(filepath))
                        {
                            filepath = Path.GetFullPath(filepath);
                        }

                        filelist.Add(filepath, string.Empty);
                    }
                    sr.Close();
                }
            }
            return filelist;
        }

        /// <summary>
        /// //Pre-set some arguments acording to the default build location of the WSP
        /// </summary>
        public void PreInitializeArguments()
        {
            string path = Environment.CurrentDirectory;

            // Start with CurrentDirectory
            DEFAULT_OUTPUTPATH = path;

            if (path.EndsWith(@"\bin\release", StringComparison.InvariantCultureIgnoreCase))
            {
                DEFAULT_BUILDMODE = BuildModeType.Release;
                DEFAULT_PROJECTDLLPATHPART = @"\bin\release";
                DEFAULT_OUTPUTPATH = path; // Use the release folder
                path = path.Substring(0, path.Length - @"\bin\release".Length);
            }
            else if (path.EndsWith(@"\bin\debug", StringComparison.InvariantCultureIgnoreCase))
            {
                DEFAULT_BUILDMODE = BuildModeType.Debug;
                DEFAULT_PROJECTDLLPATHPART = @"\bin\debug";
                DEFAULT_OUTPUTPATH = path; // Use the debug folder
                path = path.Substring(0, path.Length - @"\bin\debug".Length);
            }

            DEFAULT_SOLUTIONPATH = path;
        }


        /// <summary>
        /// Parse all the arguments from the app.config
        /// </summary>
        private void ParseAppConfig()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (string key in appSettings.Keys)
            {
                string value = appSettings[key];
              
                if (Arguments.ContainsKey(key))
                {
                    Arguments[key] = value;
                }
                else
                {
                    Arguments.Add(key, value);
                }
            }
        }
    }
}
