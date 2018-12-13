using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bellcast.Controllers
{
    public class DiseaseController : Controller
    {
        // GET: Disease
        public ActionResult Index()
        {
            return View();
        }
    }
}