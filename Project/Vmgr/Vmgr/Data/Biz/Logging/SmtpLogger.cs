using System;
using System.Reflection;
using System.Security.Principal;
using System.Xml;
using log4net;
using log4net.Config;
using Vmgr.Configuration;
using log4net.Repository;
using log4net.Appender;

namespace Vmgr.Data.Biz.Logging
{
    public class SmtpLogger : Exception
    {
        #region PRIVATE PROPERTIES

        private static SmtpLogger _logs;
        private string _serverName { get; set; }

        #endregion

        #region PROTECTED PROPERTIES

        protected LogType logType { get; set; }
        protected string message { get; set; }
        protected string applicationName { get; set; }
        protected Exception innerException { get; set; }
        protected string serverName
        {
            get
            {
                if (this._serverName == null)
                {
                    this._serverName = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                }
                return this._serverName;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        public static SmtpLogger Logs
        {
            get
            {
                if (_logs == null)
                {
                    _logs = new SmtpLogger();
                }
                return _logs;
            }
        }

        #endregion

        #region CTOR

        public SmtpLogger()
        {
            Impersonation.Impersonate(delegate
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Settings.GetSetting(Settings.Setting.VmgrLogger, true));
                XmlConfigurator.Configure(doc.DocumentElement);
            }
            )
            ;
        }

        public SmtpLogger(string message, LogType logType)
            : this(null, logType, message, "", true)
        {
        }

        public SmtpLogger(string message, LogType logType, string applicationName)
            : this(null, logType, message, applicationName, true)
        {
        }

        public SmtpLogger(Exception inner, LogType logType)
            : this(inner, logType, "", "", true)
        {
        }

        public SmtpLogger(Exception inner, LogType logType, string applicationName)
            : this(inner, logType, "", applicationName, true)
        {
        }

        public SmtpLogger(Exception inner, LogType logType, string message, string applicationName, bool persist
            )
            : base(message, inner)
        {
            Impersonation.Impersonate(delegate
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Settings.GetSetting(Settings.Setting.VmgrLogger, true));
                XmlConfigurator.Configure(doc.DocumentElement);
            }
            )
            ;

            this.logType = logType;
            this.message = message;
            this.applicationName = applicationName;

            if (persist)
                this.Save();
        }

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        #endregion

        /// <summary>
        /// Saves a record of the exception to the database.
        /// </summary>
        public ILog Save()
        {
            Impersonation.Impersonate(delegate
            {
                try
                {
                    ILog log = LogManager.GetLogger("VmgrSmtpLogger");

                    MDC.Set("user", WindowsIdentity.GetCurrent().Name);
                    MDC.Set("threadId", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    MDC.Set("applicationName", this.applicationName);
                    MDC.Set("server", this.serverName);

                    switch (this.logType)
                    {
                        case LogType.Debug:
                            log.Debug(this.message, innerException);
                            break;
                        case LogType.Error:
                            log.Error(this.message, innerException);
                            break;
                        case LogType.Warn:
                            log.Warn(this.message, innerException);
                            break;
                        case LogType.Info:
                            log.Info(this.message, innerException);
                            break;
                        case LogType.Fatal:
                            log.Fatal(this.message, innerException);
                            break;
                        default:
                            break;
                    }
                }
                finally
                {
                    ILoggerRepository rep = LogManager.GetRepository();

                    foreach (IAppender appender in rep.GetAppenders())
                    {
                        var buffered = appender as BufferingAppenderSkeleton;

                        if (buffered != null)
                        {
                            buffered.Flush();
                        }
                    }
                }
            }
            );

            return null;
        }

        public SmtpLogger Log(string message, LogType logType)
        {
            return Log(null, logType, message, Logger.GetExecutingAssemblyName(), true);
        }

        public SmtpLogger Log(string message, LogType logType, string applicationName)
        {
            return Log(null, logType, message, applicationName, true);
        }

        public SmtpLogger Log(string message, Exception inner, LogType logType)
        {
            return Log(inner, logType, message, Logger.GetExecutingAssemblyName(), true);
        }

        public SmtpLogger Log(string message, Exception inner, LogType logType, string applicationName)
        {
            return Log(inner, logType, message, applicationName, true);
        }

        public SmtpLogger Log(Exception inner, LogType logType)
        {
            return Log(inner, logType, string.Empty, Logger.GetExecutingAssemblyName(), true);
        }

        public SmtpLogger Log(Exception inner, LogType logType, string applicationName)
        {
            return Log(inner, logType, string.Empty, applicationName, true);
        }

        public SmtpLogger Log(Exception inner, LogType logType, string message, string applicationName, bool persist)
        {
            this.innerException = inner;
            this.logType = logType;
            this.message = message;
            this.applicationName = applicationName;

            if (persist)
                this.Save();

            return this;
        }

        public static string GetExecutingAssemblyName()
        {
            string executingAssemblyName = string.Empty;

            Assembly assembly = Assembly.GetEntryAssembly();

            if (assembly != null)
                executingAssemblyName = assembly.FullName;
            else
            {
                assembly = Assembly.GetExecutingAssembly();

                if (assembly != null)
                    executingAssemblyName = assembly.FullName;
            }

            return executingAssemblyName;
        }
    }
}
