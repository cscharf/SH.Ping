using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using SH.Ping.Services;
using Orchard.Logging;

namespace SH.Ping.Activities
{
    public class PingActivity : Task {
        private readonly IPingService _pingService;

        public PingActivity(IPingService pingService) {
            _pingService = pingService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext) {
            return true;
        }

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext) {
            return new[] { T("Success"), T("Failed") };
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            string siteName = activityContext.GetState<string>("SiteName");
            string siteUrl = activityContext.GetState<string>("SiteUrl");
            string services = activityContext.GetState<string>("Services");
            yield return _pingService.SendPings(siteName, siteUrl, services) ? T("Success") : T("Failed");
        }

        public override string Name {
            get { return "Ping"; }
        }

        public override string Form {
            get {
                return "ConfigurePingServices";
            }
        }

        public override LocalizedString Category {
            get { return T("Content Items"); }
        }

        public override LocalizedString Description {
            get { return T("Performs a ping against configured XML-RPC endpoints."); }
        }
    }
}