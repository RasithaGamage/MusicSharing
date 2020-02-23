using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MusicSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicSharing.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultController));

        [Authorize(Roles = "Admin")]//Show Role List
        public ActionResult Index()
        {
           
            Log.Info(User.Identity.GetUserName()+ " Was access MusicSharing Index");
            return View(context.Roles.ToList());
        }

        [Authorize(Roles = "Admin")]//Load Create Role View
        public ActionResult Create()
        {


            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]//Save to DB new role
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                Log.Info(User.Identity.GetUserName() + " Was added new role call " + collection["RoleName"]);
                return View("Create");
            }
            catch
            {
                return View();
            }
        }
        //Delete Role
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            Log.Info(User.Identity.GetUserName() + " Was Deleted role call " + RoleName);
            return RedirectToAction("Create");
        }


        //Load Edit role UI
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            Log.Info(User.Identity.GetUserName() + " Was  open Role Edit page");
            return View(thisRole);
        }


        //Edit the role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                Log.Info(User.Identity.GetUserName() + " Was Edited role call ");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Log.Error("Error occerd when Edit the Role", ex);
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageUserRoles()
        {
            // prepopulat roles for the view dropdown
            Log.Info(User.Identity.GetUserName() + " Was  opened Role ManageUserRoles page");
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        [Authorize(Roles = "Admin")]//load mange user View
        public ActionResult ManageUsers()
        {
            // prepopulat roles for the view dropdown
            Log.Info(User.Identity.GetUserName() + " Was  opened Role ManageUsers page");
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Assign role to User
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {

            Log.Info(User.Identity.GetUserName() + " Was  Assign role to User :" + UserName);
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            manager.AddToRole(user.Id, RoleName);



            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageUsers");
        }

        //Retrive all the roles 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

                ViewBag.RolesForThisUser = manager.GetRoles(user.Id);
                Log.Info(User.Identity.GetUserName() + " Was  Checked the role of " + UserName);
                // prepopulat roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageUsers");
        }

        //Delete Role from User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
           
            if (manager.IsInRole(user.Id, RoleName))
            {
                manager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
                Log.Info(User.Identity.GetUserName() + " Was  remove role of " + RoleName + " form " + UserName);
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;


            return View("ManageUsers");
        }

    }
}