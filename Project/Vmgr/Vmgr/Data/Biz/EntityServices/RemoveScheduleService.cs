﻿using Vmgr.Data.SqlServer;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.Data.Biz
{
    public partial class AppService
    {
        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void RemoveSchedule(int scheduleId)
        {
            ActionProcedures.RemoveSchedule(scheduleId, this.Adapter);
            this.Adapter.Commit();
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
