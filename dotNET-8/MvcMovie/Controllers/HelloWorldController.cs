using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers;

public class HelloWorldController : Controller 
{
    // GET: /HelloWorld/
    public IActionResult Index() {
        return View();
    }

    // GET: /HelloWorld/Welcome?name=Subhasis&ID=123
    public IActionResult Welcome(string name, int numTimes = 1) {
        ViewData["Message"] = "Hi, " + name + "!";
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}
