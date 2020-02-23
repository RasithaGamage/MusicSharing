﻿using MusicSharing.Models;
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
        // GET: Music
        public ActionResult SearchMp3()
        {
            return View();
        }
        public ActionResult AddMp3()
        {
            return View();
        }

        [HttpPost]
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

                        string fname1 = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/music/"), fname);

                        //use threads

                        using (DefaultConnection db=new DefaultConnection()) {

                            DateTime dt =  DateTime.Now;
                            String id =  dt.ToString("yyyyMMddHHmmssffff");

                           
                            var mid =  (from d in db.MusicFile orderby d.musicFileId descending select d.musicFileId).FirstOrDefault();

                            MusicFile music = new MusicFile
                            {
                                musicFileId = mid + 1,
                                musicFileName = fname,
                                songName = Request.Form["songTitle"],
                                singer = Request.Form["singer"],
                                musicFileUrl = fname1,
                                size =  file.ContentLength.ToString(),
                                addedDate = DateTime.Now
                            };

                            db.MusicFile.Add(music);
                            db.SaveChanges();

                        }

                        file.SaveAs(fname1);

                    }

                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
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