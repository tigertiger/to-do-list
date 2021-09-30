using Microsoft.AspNetCore.Mvc;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {

      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }

      [Route("/photos")]
      public ActionResult Photos()
      {
        return View();
      }
    }
}