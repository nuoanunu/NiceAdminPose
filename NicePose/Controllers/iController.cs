using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NicePose.Models;
namespace NicePose.Controllers
{
    public abstract class iController : Controller
    {

        public iController()
        {
            dbContext = new NicePoseEntities();
        }

        protected NicePoseEntities dbContext { get; set; }

        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}