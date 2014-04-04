using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WamlAndWaws.Hubs
{
    [HubName("siteCreationTrace")]
    public class SiteCreationHub : Hub
    {
    }
}