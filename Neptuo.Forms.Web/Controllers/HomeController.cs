using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neptuo.Web.Mvc.Html;

namespace Neptuo.Forms.Web.Controllers
{
    public class HomeController : Neptuo.Web.Mvc.Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ShowMessage(String.Format((L)"Welcome back, {0}", User.Identity.Name), HtmlMessageType.Success);
            return View();
        }

    }
}
