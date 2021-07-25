using System.Collections.Generic;

namespace AsyncAwaitDeepDive
{
    public class ProgressReportModel
    {
        internal int PercentageComplete { get; set; }

        internal List<WebSiteDataModel> SitesDownloaded { get; set; } = new List<WebSiteDataModel>();

    }
}
