﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Vmgr;
using Vmgr.Data.Generic;
using Vmgr.Data.Generic.EntityClasses;
using Vmgr.Data.Generic.HelperClasses;
using Vmgr.Data.Generic.RelationClasses;
using Vmgr.Data.Generic.FactoryClasses;
using Vmgr.Data.Generic.Linq;
using Vmgr.Data.SqlServer;
/*
* THIS CODE WAS AUTO-GENERATED.  DO NOT MAKE MODIFICATIONS.
*/

namespace Vmgr.Data.Biz
{
    public class XActAbortDataAccessAdapter : DataAccessAdapter
    {
        /// <summary>CTor</summary>
        public XActAbortDataAccessAdapter()
            : base(ReadConnectionStringFromConfig(), false, null, null)
        {
        }

        /// <summary>CTor</summary>
        /// <param name="keepConnectionOpen">when true, the XActAbortDataAccessAdapter will not close an opened connection. Use this for multi action usage.</param>
        public XActAbortDataAccessAdapter(bool keepConnectionOpen)
            : base(ReadConnectionStringFromConfig(), keepConnectionOpen, null, null)
        {
        }

        /// <summary>CTor</summary>
        /// <param name="connectionString">The connection string to use when connecting to the database.</param>
        public XActAbortDataAccessAdapter(string connectionString)
            : base(connectionString, false, null, null)
        {
        }

        /// <summary>CTor</summary>
        /// <param name="connectionString">The connection string to use when connecting to the database.</param>
        /// <param name="keepConnectionOpen">when true, the XActAbortDataAccessAdapter will not close an opened connection. Use this for multi action usage.</param>
        public XActAbortDataAccessAdapter(string connectionString, bool keepConnectionOpen)
            : base(connectionString, keepConnectionOpen, null, null)
        {
        }

        protected override SD.LLBLGen.Pro.ORMSupportClasses.DynamicQueryEngineBase CreateDynamicQueryEngine()
        {
            return new XActAbortDynamicQueryEngine();
        }

        /// <summary>Reads the value of the setting with the key ConnectionStringKeyName from the *.config file and stores that value as the active connection string to use for this object.</summary>
        /// <returns>connection string read</returns>
        private static string ReadConnectionStringFromConfig()
        {
            return ConfigFileHelper.ReadConnectionStringFromConfig(ConnectionStringKeyName);
        }

        internal static void SetXActAbortFlag(bool value)
        {
            XActAbortDynamicQueryEngine.XActAbortOn = value;
        }
    }
}
