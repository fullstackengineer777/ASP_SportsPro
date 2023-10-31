using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;



namespace SportsPro.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

    }
}