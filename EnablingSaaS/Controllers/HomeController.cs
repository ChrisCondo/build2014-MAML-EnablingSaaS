using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using System.Threading.Tasks;
using System.Web.Mvc;
using WamlAndWaws.Models;

namespace WamlAndWaws.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var cred = ManagementLibraryUtilities.GetCredentials();
            var client = CloudContext.Clients.CreateWebSiteManagementClient(cred);
            var viewModel = await GetHomePageViewModel(client);
            return View(viewModel);
        }

        private async Task<HomePageViewModel> GetHomePageViewModel(WebSiteManagementClient client)
        {
            var viewModel = new HomePageViewModel();

            var listWebSpacesResponse = await client.WebSpaces.ListAsync();

            foreach (var webSpace in listWebSpacesResponse.WebSpaces)
            {
                viewModel.WebSpaces.Add(new WebSpaceListItem
                {
                    GeoRegion = webSpace.GeoRegion,
                    WebSpaceName = webSpace.Name
                });
            }

            return viewModel;
        }
    }
}