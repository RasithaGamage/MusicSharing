using log4net;
using Microsoft.AspNet.Identity;
using MusicSharing.Models;
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

            dynamic materials = new System.Dynamic.ExpandoObject();
            DefaultConnection db = new DefaultConnection();
            foreach (var a in db.MusicFile.ToList())
            {
                copyMp3(a.musicFileUrl); 
            }
          
            return View(db.MusicFile.ToList());
        }


        public void copyMp3(string musicFileUrl)
        {


            //Get the full path of the file    
            //  var fullFilePath  = "\\\\mylab.local\\dfs\\data\\" + musicFileUrl;
            var fullFilePath = "D:\\Android\\"+ musicFileUrl;

            // Get the destination path

            var copyToPath = Server.MapPath("~/Content/music/"+ musicFileUrl);

            if (!System.IO.File.Exists(copyToPath)) {
                System.IO.File.Copy(fullFilePath, copyToPath);
            }
               

        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddMp3()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
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

                        // string fname1 =  "\\\\mylab.local\\dfs\\data\\"+fname;
                        string fname1 = "D:\\Android\\" + fname;
                        //use threads

                        using (DefaultConnection db=new DefaultConnection()) {

                            DateTime dt =  DateTime.Now;
                            string id =  dt.ToString("yyyyMMddHHmmssffff");
                            var mid =  (from d in db.MusicFile orderby d.musicFileId descending select d.musicFileId).FirstOrDefault();
                            MusicFile music = new MusicFile
                            {
                                musicFileId = mid + 1,
                                musicFileName = fname,
                                songName = Request.Form["songTitle"],
                                singer = Request.Form["singer"],
                                musicFileUrl = fname,
                                size =  file.ContentLength.ToString(),
                                addedDate = DateTime.Now
                            };
                            db.MusicFile.Add(music);
                            db.SaveChanges();
                        }
                        file.SaveAs(fname1);
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