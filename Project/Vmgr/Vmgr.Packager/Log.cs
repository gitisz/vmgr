﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Vmgr.Packager
{
    public class Log
    {
        public const string TRACESWITCH_CACHE_KEY = "TraceLevel";

        // Initialize and config the TraceLevelSwitch.
        private TraceSwitch _switch = null;
        
        protected TraceSwitch Switch
        {
            get { return _switch; }
            set { _switch = value; }
        }

        public static void Error(string message)
        {
            Trace.WriteLineIf(Log.Instance.Switch.TraceError, message);
        }

        public static void Warning(string message)
        {
            Trace.WriteLineIf(Log.Instance.Switch.TraceWarning, message);
        }

        public static void Information(string message)
        {
            Trace.WriteLineIf(Log.Instance.Switch.TraceInfo, message);
        }

        public static void Verbose(string message)
        {
            Trace.WriteLineIf(Log.Instance.Switch.TraceVerbose, message);
        }

        protected static readonly Log instance = new Log();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Log()
        {
        }

        Log()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;

            _switch = new TraceSwitch("TraceLevelSwitch", "Trace Level for Entire Application", "4");
        }

        protected static Log Instance
        {
            get
            {
                return instance;
            }
        }
    }
}