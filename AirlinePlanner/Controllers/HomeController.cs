using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AirlinePlanner.Models;

namespace AirlinePlanner.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }
    }
}
