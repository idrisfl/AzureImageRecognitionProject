using Microsoft.AspNetCore.Mvc;

namespace ImageRecognition.UI.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
