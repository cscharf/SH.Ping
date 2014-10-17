using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.ContentManagement.MetaData;
using Orchard.DisplayManagement;
using Orchard.Forms.Services;
using Orchard.Localization;
using Orchard.Settings;

namespace SH.Ping.Forms
{
    public class PingForms : IFormProvider
    {
        private readonly ISiteService _siteService;

        protected dynamic Shape { get; set; }
        public Localizer T { get; set; }

        public PingForms(
            IShapeFactory shapeFactory,
            ISiteService siteService) {
            Shape = shapeFactory;
            _siteService = siteService;
            T = NullLocalizer.Instance;
        }

        public void Describe(DescribeContext context) {
            var settings = _siteService.GetSiteSettings();
            context.Form("ConfigurePingServices",
                shape => Shape.Form(
                    Id: "ConfigurePingServices",
                    _SiteName: Shape.Textbox(
                        Id: "sitename", Name: "SiteName",
                        Title: T("Site Name"),
                        Description: T("The site name that will be submitted to the ping services."),
                        Classes: new[] { "text medium" },
                        Value: settings.SiteName),
                    _SiteUrl: Shape.Textbox(
                        Id: "siteurl", Name: "SiteUrl",
                        Title: T("Site URL"),
                        Description: T("The site URL that will be submitted to the ping services."),
                        Classes: new[] { "text medium" },
                        Value: settings.BaseUrl),
                    _Services: Shape.TextArea(
                        Id: "services", Name: "Services",
                        Title: T("Ping Service URLs"),
                        Description: T("The XML-RPC endpoints/URLs that will be notified by this activity. Separate multiple service URLs with line breaks."),
                        Classes: new[] { "text medium", "tokenized" },
                        Rows: 8,
                        Value: @"http://rpc.pingomatic.com
http://rpc.twingly.com
http://api.moreover.com/RPC2
http://api.moreover.com/ping
http://www.blogdigger.com/RPC2
http://www.blogshares.com/rpc.php
http://www.blogsnow.com/ping
http://www.blogstreet.com/xrbin/xmlrpc.cgi
http://bulkfeeds.net/rpc
http://www.newsisfree.com/xmlrpctest.php
http://ping.blo.gs/
http://ping.feedburner.com
http://ping.syndic8.com/xmlrpc.php
http://ping.weblogalot.com/rpc.php
http://rpc.blogrolling.com/pinger/
http://rpc.technorati.com/rpc/ping
http://rpc.weblogs.com/RPC2
http://www.feedsubmitter.com
http://blo.gs/ping.php
http://www.pingerati.net
http://www.pingmyblog.com
http://geourl.org/ping
http://ipings.com
http://www.weblogalot.com/ping
")
                    )
                );
        }
    }
}