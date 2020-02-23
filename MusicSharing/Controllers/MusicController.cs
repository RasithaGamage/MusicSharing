using log4net;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicSharing.Controllers
{
    public class MusicController : Controller
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultController));
        // GET: Music
        [Authorize]
        public ActionResult SearchMp3()
        {
            Log.Info("SearchMp3 accessed by " + User.Identity.GetUserName());
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddMp3()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddMusic()
        {

           
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        fname = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/music/"), fname);
                        file.SaveAs(fname);
                    }
                    Log.Info("Add new mp3 file by" + User.Identity.GetUserName() + " - file.FileName");
                    return Json("File Uploaded Successfully!");
                    
                }
                catch (Exception ex)
                {
                    Log.Error("Error occored when Upload Music by "+ User.Identity.GetUserName(), ex);
                    return Json("Error occurred. Error details: " + ex.Message);
                    
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}