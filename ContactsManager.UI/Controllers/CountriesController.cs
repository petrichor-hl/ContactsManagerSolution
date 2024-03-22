using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI.Controllers
{
    public class CountriesController : Controller
    {
        [Route("/countries/uploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }
    }
}
