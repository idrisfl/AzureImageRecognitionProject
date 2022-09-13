using Microsoft.AspNetCore.Mvc;

namespace ImageRecognition.UI.Controllers
{
    public class FaceAnalysisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
