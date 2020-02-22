using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicSharing.Controllers
{
    public class MusicController : Controller
    {
        // GET: Music
        public ActionResult SearchMp3()
        {
            return View();
        }

        public ActionResult AddMp3()
        {
            return View();
        }
    }
}