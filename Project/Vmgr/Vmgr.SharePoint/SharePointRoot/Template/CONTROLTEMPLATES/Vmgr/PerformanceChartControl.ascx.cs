using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;

namespace Vmgr.SharePoint
{

    public class PerformanceChartTemplateContainer : WebControl, INamingContainer
    {
        internal PerformanceChartTemplateContainer()
            : base(HtmlTextWriterTag.Div)
        {
        }
    }


    [ParseChildren(true)]
    public partial class PerformanceChartControl : BaseControl
    {
        #region PRIVATE

        private ITemplate _performanceChartTemplate;

        #endregion

        #region PROTECTED

        #endregion

        #region PUBLIC

        [PersistenceMode(PersistenceMode.InnerProperty), TemplateContainer(typeof(PerformanceChartTemplateContainer))]
        public ITemplate PerformanceChartTemplate
        {
            get { return _performanceChartTemplate; }
            set { _performanceChartTemplate = value; }
        }

        [PersistenceMode(PersistenceMode.Attribute)]
        public string PackageUniqueId { get; set; }

        #endregion

        #region CTOR

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region Private Methods

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (_performanceChartTemplate != null)
            {
                _performanceChartTemplate.InstantiateIn(this.PlaceHolderPerformanceChart);

            }
        }

        #endregion

    }
}