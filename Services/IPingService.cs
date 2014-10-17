using Orchard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SH.Ping.Services
{
    public interface IPingService : IDependency {
        bool SendPings(string siteName, string siteUrl, string services);
    }
}
