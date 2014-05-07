using System;
using Vmgr.Plugins;

namespace $rootnamespace$
{
    [Serializable]
    public class $rootname$ : BasePlugin
    {
        public override string Name
        {
            get { return "$pluginname$"; }
        }

        public override string Description
        {
            get { return "$plugindesc$"; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("$guid1$"); }
        }

        public override bool Schedulable
        {
            get { return false; }
        }
    }
}
