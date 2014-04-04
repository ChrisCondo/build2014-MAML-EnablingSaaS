using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WamlAndWaws.Hubs;
using WamlAndWindows;

namespace WamlAndWaws.Controllers
{
    public class RestController : ApiController
    {
        [Route("api/checkname")]
        [HttpGet]
        public async Task<bool> CheckName(string webSiteName)
        {
            if (webSiteName.Length < 3) return false;

            var cred = ManagementLibraryUtilities.GetCredentials();
            var client = CloudContext.Clients.CreateWebSiteManagementClient(cred);

            var result = await client.WebSites.IsHostnameAvailableAsync(webSiteName);
            return result.IsAvailable;
        }

        private void Trace(string message, params object[] parms)
        {
            Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager
                .GetHubContext<SiteCreationHub>().Clients.All.traceReceived(
                    string.Format(message, parms)
                );
        }

        [Route("api/newsite")]
        [HttpGet]
        public async Task<bool> CreateSite(string SiteName,
            string WebSpaceName,
            string BlogName,
            string Description,
            string Copywright)
        {
            try
            {
                Trace("Getting Credentials");

                var cred = ManagementLibraryUtilities.GetCredentials();
                var client = new WebSiteManagementClient(cred);

                var parameters = new WebSiteCreateParameters
                {
                    Name = SiteName,
                    SiteMode = WebSiteMode.Limited,
                    ComputeMode = WebSiteComputeMode.Shared,
                    WebSpaceName = WebSpaceName
                };

                parameters.HostNames.Add(
                    string.Format("{0}.azurewebsites.net", SiteName)
                    );

                Trace("Creating Site {0}", SiteName);

                var result = client.WebSites.Create(WebSpaceName, parameters);

                Trace("Created {0}. Getting publish profiles.", SiteName);

                var profiles = await client.WebSites.GetPublishProfileAsync(WebSpaceName, SiteName);
                var webDeployProfile = profiles.First(x => x.MSDeploySite != null);
                var webSitePath = HttpContext.Current.Server.MapPath(@"~/App_Data/MiniBlog");

                Trace("Deploying site {0}", SiteName);

                new WebDeployPublishingHelper(
                    webDeployProfile.PublishUrl,
                    webDeployProfile.MSDeploySite,
                    webDeployProfile.UserName,
                    webDeployProfile.UserPassword,
                    webSitePath
                    )
                    .PublishFolder();

                Trace("Deployed site {0}", SiteName);

                Trace("Getting configuration for {0}", SiteName);

                // get the site's configuration
                var settingsResult = await client.WebSites.GetConfigurationAsync(WebSpaceName, SiteName);

                settingsResult.AppSettings.Add("blog:name", BlogName);
                settingsResult.AppSettings.Add("blog:description", Description);
                settingsResult.AppSettings.Add("blog:copyrightOwner", Copywright);

                Trace("Saving site configuration for {0}", SiteName);

                // update the site's configuration
                await client.WebSites.UpdateConfigurationAsync(WebSpaceName, SiteName,
                    new WebSiteUpdateConfigurationParameters
                    {
                        AppSettings = settingsResult.AppSettings
                    });

                Trace("Site {0} is ready.", SiteName);

                return true;
            }
            catch(Exception ex)
            {
                Trace("Exception: {0}", ex.ToString());
                return false;
            }
        }
    }
}