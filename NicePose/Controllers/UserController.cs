using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NicePose.Models;
namespace NicePose.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            NicePoseEntities dbContext = new NicePoseEntities();
            ViewData["UserList"] = dbContext.NiceposeUsers.ToList();
            return View();
        }
        public ActionResult Detail(int id)
        {
            NicePoseEntities dbContext = new NicePoseEntities();
            ViewData["thisUser"] = dbContext.NiceposeUsers.Find(id);
            return View("ProfileDetail");
        }
    }
}