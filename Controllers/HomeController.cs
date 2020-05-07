using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Mvc;

namespace DashboardMainDemo {
    public class HomeController : Controller {
        public ActionResult Index() {
            string workingModeString = Request.Query["mode"];
            var workingMode = !string.IsNullOrEmpty(workingModeString) && workingModeString == "designer" ? WorkingMode.Designer : WorkingMode.Viewer;
            return View(workingMode);
        }
        [Route("MobileDashboard")]
        public ActionResult MobileDashboard() {
            return View();
        }
        [Route("Emulator")]
        public ActionResult Emulator() {
            return View("Emulator");

        }
    }
}
